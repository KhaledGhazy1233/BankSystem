using BusinessCore.BankSystem.Features.Transaction.Commands.Responses;
using Domainlayer.BankSystem.Requests;
using MediatR;

namespace BusinessCore.BankSystem.Features.Transaction.Commands.Models
{
    public class DepositCommand : DepositRequest
                                 , IRequest<Response<TransferResponse>>
    {
    }
}
