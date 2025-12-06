using BusinessCore.BankSystem.Bases;
using BusinessCore.BankSystem.Features.BankAccount.Queries.Responses;
using MediatR;

namespace BusinessCore.BankSystem.Features.BankAccount.Queries.Models
{
    public class GetByAccountNumberQuery : IRequest<Response<BankAccountResponse>>
    {
        public string AccountNumber { get; set; }

        public GetByAccountNumberQuery(string accountNumber)
        {
            AccountNumber = accountNumber;
        }
    }
}
