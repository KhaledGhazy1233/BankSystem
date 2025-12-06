using ApplicationLayer.BankSystem.AbstractServices;
using Domainlayer.BankSystem.Entites;
using Domainlayer.BankSystem.Enums;
using Domainlayer.BankSystem.Requests;
using InfrastructureLayer.BankSystem.AbstractRepositories;

namespace ApplicationLayer.BankSystem.ImplementServices
{
    public class BankAccountService : IBankAccountService
    {
        #region Fields
        private IBankAccountRepository _accountRepository { get; set; }
        #endregion

        #region Constructor
        public BankAccountService(IBankAccountRepository BankAccountRepo)
        {
            _accountRepository = BankAccountRepo;
        }

        public async Task<IEnumerable<BankAccount>> GetAllAccountsAsync()
        {

            var accounts = await _accountRepository.GetAllAsync();

            //var results = new List<BankAccountResult>();
            //foreach (var account in accounts)
            //{
            //    var resultList = new BankAccountResult
            //    {
            //        Id = account.Id,
            //        AccountType = account.AccountType,
            //        AccountNumber = account.AccountNumber,
            //        Balance = (decimal)account.Balance,
            //        Currency = account.Currency,
            //        IsActive = account.IsActive,
            //        CreatedData = account.CreatedData,
            //        UpdatedData = account.UpdatedData,
            //        UserId = account.UserId
            //    };
            //    results.Add(resultList);
            //}

            return accounts;
        }

        public async Task<BankAccount> GetAccountByIdAsync(int id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            //return new BankAccountResult
            //{
            //    Id = account.Id,
            //    AccountType = account.AccountType,
            //    AccountNumber = account.AccountNumber,
            //    Balance = (decimal)account.Balance,
            //    Currency = account.Currency,
            //    IsActive = account.IsActive,
            //    CreatedData = account.CreatedData,
            //    UpdatedData = account.UpdatedData,
            //    UserId = account.UserId
            //};
            return account;
        }

        public async Task<bool> CreateAccountAsync(CreateAccountRequest model)
        {
            var newAcc = new BankAccount
            {
                AccountType = model.AccountType,
                Currency = model.Currency,
                UserId = model.UserId,
                AccountNumber = GenerateAccountNumber(),
                Balance = 0,
                IsActive = true,
                CreatedData = DateTime.Now,
                UpdatedData = DateTime.Now
            };

            var result = await _accountRepository.AddAsync(newAcc);

            return result != null;
        }

        public async Task<bool> UpdateAccountTypeAsync(int id, int accountTypeEnum)
        {

            var account = await _accountRepository.GetByIdAsync(id);
            if (account != null && Enum.IsDefined(typeof(AccountTypeEnum), accountTypeEnum))
            {
                account.AccountType = (AccountTypeEnum)accountTypeEnum;
                await _accountRepository.UpdateAsync(account);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAccountAsync(int id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null)
                return false;
            account.ISDeleted = true;
            await _accountRepository.UpdateAsync(account);

            return true;
        }

        public async Task<IEnumerable<BankAccount>> GetAccountsByUserIdAsync(int userId)
        {
            var accounts = await _accountRepository.GetAccountsByUserIdAsync(userId);

            //var results = accounts.Select(account => new BankAccountResult
            //{
            //    Id = account.Id,
            //    AccountType = account.AccountType,
            //    AccountNumber = account.AccountNumber,
            //    Currency = account.Currency,
            //    IsActive = account.IsActive,
            //    CreatedData = account.CreatedData,
            //    UpdatedData = account.UpdatedData,
            //    UserId = account.UserId
            //}).ToList();

            return accounts;

        }



        public async Task<BankAccount> GetByAccountNumberAsync(string accountNumber)
        {
            var account = await _accountRepository.GetByAccountNumberAsync(accountNumber);
            if (account == null)
                return null;
            ////var accountresult = new BankAccountResult
            ////{
            ////    AccountNumber = account.AccountNumber,
            ////    AccountType = account.AccountType,
            ////    Currency = account.Currency,
            ////    CreatedData = account.CreatedData,
            ////    UpdatedData = account.UpdatedData,
            ////    Id = account.Id,
            ////    IsActive = account.IsActive,
            ////    UserId = account.UserId
            //};
            return account;
        }
        private string GenerateAccountNumber()
        {
            return $"ACCT-{DateTime.UtcNow.Ticks}";
        }



        #endregion

        #region MethodsHandler

        #endregion
    }
}
