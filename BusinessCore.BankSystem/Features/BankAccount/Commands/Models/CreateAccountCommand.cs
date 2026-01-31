using BusinessCore.BankSystem.Features.BankAccount.Queries.Responses;
using Domainlayer.BankSystem.Requests;
using MediatR;

namespace BusinessCore.BankSystem.Features.BankAccount.Commands.Models
{
    public class CreateAccountCommand : CreateAccountRequest,
                                        IRequest<Response<BankAccountResponse>>
    {
    }
}
