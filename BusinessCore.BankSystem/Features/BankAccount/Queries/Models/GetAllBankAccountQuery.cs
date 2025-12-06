using BusinessCore.BankSystem.Bases;
using BusinessCore.BankSystem.Features.BankAccount.Queries.Responses;
using MediatR;

namespace BusinessCore.BankSystem.Features.BankAccount.Queries.Models
{
    public class GetAllBankAccountQuery : IRequest<Response<IEnumerable<BankAccountResponse>>>
    {
    }
}
