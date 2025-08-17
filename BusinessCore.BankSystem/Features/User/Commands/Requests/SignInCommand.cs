using BusinessCore.BankSystem.Bases;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCore.BankSystem.Features.User.Commands.Requests
{
    public class SignInCommand :IRequest<Response<string>>
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
