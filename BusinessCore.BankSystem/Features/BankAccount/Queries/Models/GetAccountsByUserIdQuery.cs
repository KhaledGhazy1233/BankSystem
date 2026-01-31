using BusinessCore.BankSystem.Features.BankAccount.Queries.Responses;
using MediatR;

namespace BusinessCore.BankSystem.Features.BankAccount.Queries.Models
{
    public class GetAccountsByUserIdQuery : IRequest<Response<IEnumerable<BankAccountResponse>>>
    {
        public int UserId;
        public GetAccountsByUserIdQuery(int userId)
        {
            this.UserId = userId;
        }
    }
}
