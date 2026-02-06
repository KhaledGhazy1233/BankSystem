using ApplicationLayer.BankSystem.AbstractServices;
using BusinessCore.BankSystem.Features.Transaction.Commands.Responses;
using BusinessCore.BankSystem.Features.Transaction.Queries.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BusinessCore.BankSystem.Features.Transaction.Queries.Handler
{
    public class TransactionQueryHandler : IRequestHandler<GetHistoryQueryPagination, Response<IEnumerable<TransferResponse>>>
    {
        private readonly ILogger<TransactionQueryHandler> _logger;

        private readonly ITransactionService _transactionService;
        public TransactionQueryHandler(ILogger<TransactionQueryHandler> logger, ITransactionService transactionService)
        {
            _logger = logger;
            _transactionService = transactionService;
        }
        public async Task<Response<IEnumerable<TransferResponse>>> Handle(GetHistoryQueryPagination request, CancellationToken cancellationToken)
        {
            try
            {
                // 1. نكلم الـ Service وناخد الـ Entities
                var transactions = await _transactionService.GetHistoryEntitiesAsync(request.AccountNumber, request.PageNumber, request.PageSize);

                // 2. الـ Mapping (تحويل الـ Entity لـ DTO)
                var response = transactions.Select(t => new TransferResponse
                {
                    ReferenceNumber = t.ReferenceNumber,
                    Amount = t.Amount,
                    TransactionDate = t.TransactionDate,
                    Status = t.Status,
                    Description = t.Description,

                    // تحديد نوع العملية بذكاء
                    TransactionType = _transactionService.GetTransactionType(t, request.AccountNumber),

                    // تحديد الأطراف
                    FromAccountNumber = t.FromAccount?.AccountNumber ?? "Cash/ATM",
                    ToAccountNumber = t.ToAccount?.AccountNumber ?? "Cash Out",

                    // الرصيد بعد العملية (لصاحب الطلب فقط)
                    BalanceAfter = t.FromAccount?.AccountNumber == request.AccountNumber
                                   ? t.BalanceAfter  // لو هو اللي باعت
                                   : 0 // لو هو اللي مستلم (لو ضفت الحقل ده)
                }).ToList();

                return Response<IEnumerable<TransferResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                return Response<IEnumerable<TransferResponse>>.BadRequest(ex.Message);
            }
        }
    }
}
