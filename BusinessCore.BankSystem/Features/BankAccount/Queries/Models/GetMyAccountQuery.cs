using BusinessCore.BankSystem.Features.BankAccount.Queries.Responses;
using MediatR;

namespace BusinessCore.BankSystem.Features.BankAccount.Queries.Models
{
    public class GetMyAccountQuery : IRequest<Response<IEnumerable<BankAccountResponse>>>
    {
    }
}
