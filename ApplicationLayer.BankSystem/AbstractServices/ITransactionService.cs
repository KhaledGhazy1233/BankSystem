using Domainlayer.BankSystem.Entites;
using Domainlayer.BankSystem.Requests;

namespace ApplicationLayer.BankSystem.AbstractServices
{
    public interface ITransactionService
    {
        Task<Transaction> ExecuteTransferAsync(TransferRequest request, CancellationToken ct);
        Task<Transaction> ExecuteDepositAsync(DepositRequest request, CancellationToken ct);
        Task<Transaction> ExecuteWithdrawAsync(WithdrawRequest request, CancellationToken ct);
    }
}
