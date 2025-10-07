using BusinessCore.BankSystem.Bases;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCore.BankSystem.Features.Authorization.Commands.Requests
{
    public class DeleteRoleCommand :IRequest<Response<string>>
    {
        public int _Id { get; set; }

        public DeleteRoleCommand(int Id)
        {
            _Id = Id;
        }

    }
}
