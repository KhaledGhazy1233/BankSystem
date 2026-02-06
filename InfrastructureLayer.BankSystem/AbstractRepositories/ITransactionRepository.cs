//using System.Transactions;
using Domainlayer.BankSystem.Entites;
using InfrastructureLayer.BankSystem.InfrastructureBases;
namespace InfrastructureLayer.BankSystem.AbstractRepositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<IEnumerable<Transaction>> GetByAccountNumberAsync(string accountNumber);
        Task<decimal> GetCurrentBalanceAsync(string accountNumber);
        Task<bool> HasSufficientBalanceAsync(string accountNumber, decimal amount);
        Task<bool> LockAccountAsync(string accountNumber);
        Task UnlockAccountAsync(string accountNumber);
        Task UpdateTransactionStatusAsync(int transactionId, string status);

        Task<IEnumerable<Transaction>> GetTransactionsByAccountNumberAsync(string accountNumber, int pageNumber, int pageSize);


    }
}
