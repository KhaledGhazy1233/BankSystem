using BusinessCore.BankSystem.Features.User.Queries.Response;
using BusinessCore.BankSystem.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCore.BankSystem.Features.User.Queries.Requests
{
    public class GetUserPaginatedQuery :IRequest<PaginatedResult<GetUserPaginatedResponse>>
    {
        public int PageNumber { get; set; } 
        public int PageSize { get; set; } 
    }
}
