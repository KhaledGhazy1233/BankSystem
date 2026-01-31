using Domainlayer.BankSystem.Entites;
using InfrastructureLayer.BankSystem.InfrastructureBases;

namespace InfrastructureLayer.BankSystem.AbstractRepositories
{
    public interface IBankAccountRepository : IRepository<BankAccount>
    {
        Task<BankAccount> GetByAccountNumberAsync(string accountNumber);
        Task<IEnumerable<BankAccount>> GetAccountsByUserIdAsync(int userId);
        Task<IEnumerable<BankAccount>> GetActiveAccountsAsync();
        Task<BankAccount?> GetUserIdAsync(int id);
    }
}
