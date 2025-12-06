using BusinessCore.BankSystem.Bases;
using BusinessCore.BankSystem.Features.BankAccount.Queries.Responses;
using MediatR;

namespace BusinessCore.BankSystem.Features.BankAccount.Queries.Models
{
    public class GetBankAccountByIdQuery : IRequest<Response<BankAccountResponse>>
    {
        public int Id { get; set; }

        public GetBankAccountByIdQuery(int id)
        {
            Id = id;
        }


    }
}
