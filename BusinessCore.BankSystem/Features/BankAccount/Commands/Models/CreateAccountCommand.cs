using BusinessCore.BankSystem.Bases;
using Domainlayer.BankSystem.Requests;
using MediatR;

namespace BusinessCore.BankSystem.Features.BankAccount.Commands.Models
{
    public class CreateAccountCommand : CreateAccountRequest,
                                        IRequest<Response<string>>
    {
    }
}
