using BusinessCore.BankSystem.Bases;
using Domainlayer.BankSystem.Enums;
using MediatR;

namespace BusinessCore.BankSystem.Features.BankAccount.Commands.Models
{
    public class UpdateAccountTypeCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public AccountTypeEnum accountTypeEnum { get; set; }
    }
}
