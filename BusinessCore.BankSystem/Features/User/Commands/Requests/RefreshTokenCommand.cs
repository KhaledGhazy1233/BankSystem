using BusinessCore.BankSystem.Bases;
using Domainlayer.BankSystem.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCore.BankSystem.Features.User.Commands.Requests
{
    public class RefreshTokenCommand :IRequest<Response<JwtAuthResult>>
    {
        public string ? AccessToken { get; set; }
        public string ? RefreshToken { get; set; }
    }
}
