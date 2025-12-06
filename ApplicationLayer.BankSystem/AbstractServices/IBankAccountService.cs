using Domainlayer.BankSystem.Entites;
using Domainlayer.BankSystem.Requests;

namespace ApplicationLayer.BankSystem.AbstractServices
{
    public interface IBankAccountService
    {
        Task<IEnumerable<BankAccount>> GetAllAccountsAsync();
        Task<BankAccount> GetAccountByIdAsync(int id);
        Task<bool> CreateAccountAsync(CreateAccountRequest createAccountRequest);
        Task<bool> UpdateAccountTypeAsync(int id, int accountTypeEnum);
        Task<bool> DeleteAccountAsync(int id);
        Task<IEnumerable<BankAccount>> GetAccountsByUserIdAsync(int userId);


        // العمليات الإضافية من الـ Repository
        Task<BankAccount> GetByAccountNumberAsync(string accountNumber);

    }
}
