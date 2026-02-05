using ApplicationLayer.BankSystem.AbstractServices;
using BusinessCore.BankSystem.Features.Transaction.Commands.Models;
using BusinessCore.BankSystem.Features.Transaction.Commands.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BusinessCore.BankSystem.Features.Transaction.Commands.Handler
{
    public class TransactionCommandHandler : IRequestHandler<TransferCommand, Response<TransferResponse>>
                                             , IRequestHandler<DepositCommand, Response<TransferResponse>>
                                             , IRequestHandler<WithdrawCommand, Response<TransferResponse>>
    {
        private readonly ILogger<TransactionCommandHandler> _logger;

        private readonly ITransactionService _transactionService;
        public TransactionCommandHandler(ILogger<TransactionCommandHandler> logger, ITransactionService transactionService)
        {
            _logger = logger;
            _transactionService = transactionService;
        }
        public async Task<Response<TransferResponse>> Handle(TransferCommand request, CancellationToken ct)
        {
            try
            {
                // 1. تنفيذ العملية (الخدمة بترجع Entity دلوقتي)
                var transaction = await _transactionService.ExecuteTransferAsync(request, ct);


                var transferDto = new TransferResponse
                {
                    ReferenceNumber = transaction.ReferenceNumber,
                    Amount = transaction.Amount,
                    FromAccountNumber = request.FromAccountNumber,
                    ToAccountNumber = request.ToAccountNumber,
                    BalanceAfter = transaction.BalanceAfter,
                    TransactionDate = transaction.TransactionDate,
                    Status = "Success" // إضافة الـ Status يدوياً
                };

                // 3. إرجاع الـ Success Status
                return Response<TransferResponse>.Success(transferDto, "تم التحويل بنجاح.");
            }
            catch (Exception ex)
            {
                // 4. الـ Status في حالة الفشل
                // هنا الـ Handler بيمسك الـ Exception ويحوله لـ Error Response شيك
                return Response<TransferResponse>.BadRequest($"فشلت العملية: {ex.Message}");

            }
        }

        public async Task<Response<TransferResponse>> Handle(DepositCommand request, CancellationToken ct)
        {
            try
            {
                var tx = await _transactionService.ExecuteDepositAsync(request, ct);

                // Manual Mapping
                var responseDto = new TransferResponse
                {
                    ReferenceNumber = tx.ReferenceNumber,
                    Amount = tx.Amount,
                    ToAccountNumber = request.AccountNumber,
                    FromAccountNumber = "Cash/ATM", // دلالة إنها إيداع
                    BalanceAfter = tx.BalanceAfter,
                    TransactionDate = tx.TransactionDate,
                    Status = "Success"
                };

                return Response<TransferResponse>.Success(responseDto, "تمت عملية الإيداع بنجاح.");
            }
            catch (Exception ex)
            {
                return Response<TransferResponse>.BadRequest($"فشل الإيداع: {ex.Message}");
            }
        }

        public async Task<Response<TransferResponse>> Handle(WithdrawCommand request, CancellationToken ct)
        {
            try
            {
                var tx = await _transactionService.ExecuteWithdrawAsync(request, ct);

                // Manual Mapping للـ Response الموحدة
                var result = new TransferResponse
                {
                    ReferenceNumber = tx.ReferenceNumber,
                    TransactionType = "Withdraw",
                    Amount = tx.Amount,
                    FromAccountNumber = request.AccountNumber,
                    ToAccountNumber = "Cash Out",
                    BalanceAfter = tx.BalanceAfter,
                    TransactionDate = tx.TransactionDate,
                    Description = tx.Description,
                    Status = "Success"
                };

                return Response<TransferResponse>.Success(result, "تم السحب بنجاح.");
            }
            catch (Exception ex)
            {
                return Response<TransferResponse>.BadRequest($"فشل السحب: {ex.Message}");
            }
        }
    }
}
