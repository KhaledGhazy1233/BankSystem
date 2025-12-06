using BusinessCore.BankSystem.Bases;
using Domainlayer.BankSystem.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCore.BankSystem.Features.Authorization.Queries.Requests
{
    public class ManageUserClaimQuery : IRequest<Response<ManageUserClaimResult>>
    {
        public int UserId { get; set; }//Ahmed-->Create,,Edit,,Delete
    }
}
