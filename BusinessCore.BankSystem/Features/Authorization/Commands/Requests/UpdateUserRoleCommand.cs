using BusinessCore.BankSystem.Bases;
using Domainlayer.BankSystem.Requests;
using Domainlayer.BankSystem.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCore.BankSystem.Features.Authorization.Commands.Requests
{
    public class UpdateUserRoleCommand : UpdateUserRolesRequest ,
                                      IRequest<Response<string>>
    {
    }
}
