using BusinessCore.BankSystem.Features.Transaction.Commands.Responses;
using MediatR;

namespace BusinessCore.BankSystem.Features.Transaction.Queries.Models
{
    public class GetHistoryQueryPagination : IRequest<Response<IEnumerable<TransferResponse>>>
    {
        public string AccountNumber { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
