using BusinessCore.BankSystem.Bases;
using BusinessCore.BankSystem.Features.BankAccount.Queries.Responses;
using MediatR;

namespace BusinessCore.BankSystem.Features.BankAccount.Queries.Models
{
    public class GetBankAccountAllByIdQuery : IRequest<Response<IEnumerable<BankAccountResponse>>>
    {
        public int Id { get; set; }

        public GetBankAccountAllByIdQuery(int id)
        {
            Id = id;
        }
    }
}
