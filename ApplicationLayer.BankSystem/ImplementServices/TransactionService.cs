using ApplicationLayer.BankSystem.AbstractServices;
using Domainlayer.BankSystem.Entites;
using Domainlayer.BankSystem.Requests;
using InfrastructureLayer.BankSystem.AbstractRepositories;
using InfrastructureLayer.BankSystem.Data;

namespace ApplicationLayer.BankSystem.ImplementServices
{
    public class TransactionService : ITransactionService

    {

        private readonly ApplicationDbContext _context;
        private readonly IBankAccountRepository _accountRepo;
        private readonly ITransactionRepository _transactionRepo;

        public TransactionService(ApplicationDbContext context, IBankAccountRepository accountRepo, ITransactionRepository transactionRepo)
        {
            _context = context;
            _accountRepo = accountRepo;
            _transactionRepo = transactionRepo;
        }

        public async Task<Transaction> ExecuteTransferAsync(TransferRequest request, CancellationToken ct)
        {
            // 1. الأمان
            if (!await ValidateSecurityHelper(request.FromAccountNumber, request.Pin))
                throw new UnauthorizedAccessException("الـ PIN غير صحيح أو الحساب محظور.");

            // 2. القفل
            var (success, error) = await AcquireLocksHelper(request.FromAccountNumber, request.ToAccountNumber);
            if (!success) throw new InvalidOperationException(error);

            using var dbTransaction = await _context.Database.BeginTransactionAsync(ct);
            try
            {
                var from = await _accountRepo.GetByAccountNumberAsync(request.FromAccountNumber);
                var to = await _accountRepo.GetByAccountNumberAsync(request.ToAccountNumber);

                ValidateTransactionHelper(from, to, request.Amount);

                var txRecord = ExecuteActionHelper(from!, to!, request.Amount, request.Description);

                await SaveTransactionDataHelper(txRecord, ct);
                await dbTransaction.CommitAsync(ct);

                return txRecord; // بنرجع الـ Entity كامل للـ Handler
            }
            catch
            {
                await dbTransaction.RollbackAsync(ct);
                throw; // بنرمي الـ Exception عشان الـ Handler يتعامل معاه
            }
            finally
            {
                await ReleaseLocksHelper(request.FromAccountNumber, request.ToAccountNumber);
            }
        }

        public async Task<Transaction> ExecuteDepositAsync(DepositRequest request, CancellationToken ct)
        {
            // 1. قفل الحساب (Acquire Lock) لضمان عدم حدوث تضارب
            var (success, error) = await AcquireLocksHelper(request.AccountNumber);
            if (!success) throw new InvalidOperationException(error);

            using var dbTransaction = await _context.Database.BeginTransactionAsync(ct);
            try
            {
                // جلب الحساب
                var account = await _accountRepo.GetByAccountNumberAsync(request.AccountNumber);
                if (account == null) throw new Exception("الحساب غير موجود.");
                if (request.Amount <= 0) throw new Exception("المبلغ يجب أن يكون أكبر من صفر.");

                // 2. التنفيذ (Action)
                decimal balanceBefore = account.Balance;
                account.Balance += request.Amount; // زيادة الرصيد

                // إنشاء سجل العملية
                var txRecord = new Transaction
                {
                    ToAccountId = account.Id, // الفلوس رايحة للحساب ده
                    FromAccountId = null,     // إيداع نقدي (مفيش حساب مرسل)
                    Amount = request.Amount,
                    BalanceBefore = balanceBefore,
                    BalanceAfter = account.Balance,
                    Status = "Completed",
                    Description = request.Description ?? "Cash Deposit",
                    TransactionDate = DateTime.UtcNow
                };

                // 3. الحفظ (نفس الهيلبر المشترك)
                await SaveTransactionDataHelper(txRecord, ct);

                await dbTransaction.CommitAsync(ct);
                return txRecord;
            }
            catch
            {
                await dbTransaction.RollbackAsync(ct);
                throw;
            }
            finally
            {
                // 4. فك القفل (نفس الهيلبر المشترك)
                await ReleaseLocksHelper(request.AccountNumber);
            }
        }

        public async Task<Transaction> ExecuteWithdrawAsync(WithdrawRequest request, CancellationToken ct)
        {
            // 1. الأمان (لازم PIN عشان نسحب)
            if (!await ValidateSecurityHelper(request.AccountNumber, request.Pin))
                throw new UnauthorizedAccessException("الـ PIN غير صحيح أو الحساب محظور.");

            // 2. القفل (حساب واحد بس)
            var (success, error) = await AcquireLocksHelper(request.AccountNumber);
            if (!success) throw new InvalidOperationException(error);

            using var dbTransaction = await _context.Database.BeginTransactionAsync(ct);
            try
            {
                var account = await _accountRepo.GetByAccountNumberAsync(request.AccountNumber);

                // 3. التحقق من البيانات والرصيد
                if (account == null) throw new Exception("الحساب غير موجود.");
                if (request.Amount <= 0) throw new Exception("المبلغ يجب أن يكون أكبر من صفر.");
                if (account.Balance < request.Amount) throw new Exception("الرصيد لا يكفي لإتمام عملية السحب.");

                // 4. التنفيذ (طرح المبلغ)
                decimal balanceBefore = account.Balance;
                account.Balance -= request.Amount;

                var txRecord = new Transaction
                {
                    FromAccountId = account.Id, // الفلوس طالعة من الحساب ده
                    ToAccountId = null,       // رايحة "كاش" في إيد العميل
                    Amount = request.Amount,
                    BalanceBefore = balanceBefore,
                    BalanceAfter = account.Balance,
                    Status = "Completed",
                    Description = request.Description ?? "Cash Withdrawal",
                    TransactionDate = DateTime.UtcNow,
                    ReferenceNumber = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper() // توليد رقم مرجعي
                };

                // 5. الحفظ
                await SaveTransactionDataHelper(txRecord, ct);
                await dbTransaction.CommitAsync(ct);

                return txRecord;
            }
            catch
            {
                await dbTransaction.RollbackAsync(ct);
                throw;
            }
            finally
            {
                // 6. فك القفل
                await ReleaseLocksHelper(request.AccountNumber);
            }
        }


        // --- Helper Methods ---

        private async Task<bool> ValidateSecurityHelper(string accNum, string pin)
        {
            var account = await _accountRepo.GetByAccountNumberAsync(accNum);
            if (account == null || account.IsPinLocked) return false;

            if (account.Pin == pin)
            {
                if (account.FailedPinAttempts > 0)
                {
                    account.FailedPinAttempts = 0;
                    await _accountRepo.UpdateAsync(account);
                }
                return true;
            }
            account.FailedPinAttempts++;
            if (account.FailedPinAttempts >= 3) account.IsPinLocked = true;
            await _accountRepo.UpdateAsync(account); // حفظ محاولة الفشل فوراً
            return false;
        }

        private async Task<(bool success, string error)> AcquireLocksHelper(params string[] nums)
        {
            foreach (var num in nums)
            {
                if (!await _transactionRepo.LockAccountAsync(num))
                    return (false, $"الحساب {num} مشغول حالياً.");
            }
            return (true, "");
        }

        private void ValidateTransactionHelper(BankAccount? from, BankAccount? to, decimal amount)
        {
            if (from == null || to == null) throw new Exception("أحد الحسابات غير موجود.");
            if (from.Balance < amount) throw new Exception("الرصيد لا يكفي.");
        }

        private Transaction ExecuteActionHelper(BankAccount from, BankAccount to, decimal amount, string? desc)
        {
            decimal balanceBefore = from.Balance;
            from.Balance -= amount;
            to.Balance += amount;
            return new Transaction
            {
                FromAccountId = from.Id,
                ToAccountId = to.Id,
                Amount = amount,
                BalanceBefore = balanceBefore,
                BalanceAfter = from.Balance,
                Status = "Completed",
                Description = desc ?? "Transfer",
                TransactionDate = DateTime.UtcNow
            };
        }

        private async Task SaveTransactionDataHelper(Transaction tx, CancellationToken ct)
        {
            _context.transactions.Add(tx);
            await _context.SaveChangesAsync(ct);
        }

        private async Task ReleaseLocksHelper(params string[] nums)
        {
            foreach (var num in nums) await _transactionRepo.UnlockAccountAsync(num);
            await _context.SaveChangesAsync();
        }


    }
}
