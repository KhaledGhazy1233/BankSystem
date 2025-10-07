using BusinessCore.BankSystem.Bases;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCore.BankSystem.Features.Authorization.Commands.Requests
{
    public class EditRoleToUserCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public string ? newName { get; set; }
        public string ? oldName { get; set; }
    }
}
