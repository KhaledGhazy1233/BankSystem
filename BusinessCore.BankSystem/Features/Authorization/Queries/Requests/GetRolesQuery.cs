using BusinessCore.BankSystem.Bases;
using Domainlayer.BankSystem.Entites;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCore.BankSystem.Features.Authorization.Queries.Requests
{
    public class GetRolesQuery : IRequest<Response<List<string>>>
    {
    }
}
