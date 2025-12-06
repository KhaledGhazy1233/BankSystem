using BusinessCore.BankSystem.Bases;
using MediatR;

namespace BusinessCore.BankSystem.Features.BankAccount.Commands.Models
{
    public class DeleteAccountCommand : IRequest<Response<string>>
    {
        public int id { get; set; }
    }
}
