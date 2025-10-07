using BusinessCore.BankSystem.Bases;
using BusinessCore.BankSystem.Features.User.Queries.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCore.BankSystem.Features.User.Queries.Requests
{
    public class GetUserByIdQuery : IRequest<Response<GetUserByIdResponse>>
    {
        public int _UserId { get; set; }

        public GetUserByIdQuery(int userId)
        {
            _UserId = userId;
        }
    }
}
