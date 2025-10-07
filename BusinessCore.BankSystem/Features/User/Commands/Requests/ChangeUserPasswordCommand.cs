using BusinessCore.BankSystem.Bases;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCore.BankSystem.Features.User.Commands.Requests
{
    public class ChangeUserPasswordCommand : IRequest<Response<string>>
    {
        public int Id{ get; set; }
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmPassword { get; set; }

    }
}
