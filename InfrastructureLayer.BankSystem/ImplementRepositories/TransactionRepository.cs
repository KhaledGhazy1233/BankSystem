//using System.Transactions;
using Domainlayer.BankSystem.Entites;
using InfrastructureLayer.BankSystem.AbstractRepositories;
using InfrastructureLayer.BankSystem.Data;
using InfrastructureLayer.BankSystem.InfrastructureBases;
using Microsoft.EntityFrameworkCore;
//using static Domainlayer.BankSystem.AppMetaData.Router;
namespace InfrastructureLayer.BankSystem.ImplementRepositories
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        private readonly DbSet<Transaction> _transactions;
        private readonly DbSet<BankAccount> _bankaccount;

        public TransactionRepository(ApplicationDbContext context) : base(context)
        {
            _transactions = context.Set<Transaction>();
            _bankaccount = context.Set<BankAccount>();
        }



        public async Task<IEnumerable<Transaction>> GetByAccountNumberAsync(string accountNumber)
        {
            return await _transactions
                .Include(t => t.FromAccount).Include(t => t.ToAccount)
                .Where(t => (t.FromAccount != null && t.FromAccount.AccountNumber == accountNumber)
                         || (t.ToAccount != null && t.ToAccount.AccountNumber == accountNumber))
                .ToListAsync();
        }




        public async Task<decimal> GetCurrentBalanceAsync(string accountNumber)
        {
            var account = await _bankaccount
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

            return account?.Balance ?? 0;
        }

        public async Task<bool> HasSufficientBalanceAsync(string accountNumber, decimal amount)
        {
            var balance = await GetCurrentBalanceAsync(accountNumber);
            return balance >= amount;
        }


        public async Task<bool> LockAccountAsync(string accountNumber)
        {
            var account = await _bankaccount.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
            if (account == null || account.IsLocked) return false;
            account.IsLocked = true; // تعديل في الميموري فقط
            account.LockedAt = DateTime.UtcNow;

            await base.SaveChangesAsync();
            return true;
        }




        public async Task UnlockAccountAsync(string accountNumber)
        {
            var account = await _bankaccount
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);//==context.BankAccounts

            if (account != null)
            {
                account.IsLocked = false;
                account.LockedAt = null;
                await base.SaveChangesAsync();
            }
        }

        public async Task UpdateTransactionStatusAsync(int transactionId, string status)
        {
            var transaction = await _transactions.FindAsync(transactionId);
            if (transaction != null)
            {
                transaction.Status = status;
                transaction.UpdatedAt = DateTime.UtcNow;
                await base.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByAccountNumberAsync(string accountNumber, int pageNumber, int pageSize)
        {
            return await _transactions
                .AsNoTracking() // مهم جداً لأننا بنقرأ فقط (Read-Only)
                .Include(t => t.FromAccount) // عشان نعرف الحساب اللي بعت
                .Include(t => t.ToAccount)   // عشان نعرف الحساب اللي استلم
                .Where(t => t.FromAccount.AccountNumber == accountNumber ||
                            t.ToAccount.AccountNumber == accountNumber)
                .OrderByDescending(t => t.TransactionDate) // الأحدث دائماً في الأول
                .Skip((pageNumber - 1) * pageSize) // تخطي الصفحات السابقة
                .Take(pageSize) // أخذ عدد معين فقط (مثلاً 10)
                .ToListAsync();
        }



    }
}
