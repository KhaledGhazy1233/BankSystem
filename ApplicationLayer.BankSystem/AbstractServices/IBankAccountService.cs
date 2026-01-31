using Domainlayer.BankSystem.Entites;
using Domainlayer.BankSystem.Enums;
using Domainlayer.BankSystem.Requests;

namespace ApplicationLayer.BankSystem.AbstractServices
{
    public interface IBankAccountService
    {
        Task<bool> DeleteAccountAsync(int id);
        Task<bool> UpdateAccountTypeAsync(int id, AccountTypeEnum newType);

        Task<BankAccount> CreateAccountAsync(CreateAccountRequest model);
        Task<IEnumerable<BankAccount>> GetAllAccountsAsync();

        Task<BankAccount> GetByAccountNumberAsync(string accountNumber);
        Task<IEnumerable<BankAccount>> GetMyAccountsAsync();
        Task<BankAccount> GetAccountByIdAsync(int id);
        Task<IEnumerable<BankAccount>> GetAccountsByUserIdAsync(int userId);


    }
}
