using System;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessCore.BankSystem.Bases;
using BusinessCore.BankSystem.Features.User.Queries.Requests;
using BusinessCore.BankSystem.Features.User.Queries.Response;
using Microsoft.AspNetCore.Identity;
using Domainlayer.BankSystem.Entites;

namespace BusinessCore.BankSystem.Features.User.Queries.Handler
{
    public class GetByUserIdHandler : ResponseHandler,
                                   IRequestHandler<GetUserByIdQuery, Response<GetUserByIdResponse>>
    {
        #region Fields
        private UserManager<ApplicationUser> _usermanager;


        #endregion


        #region Constructors
        public GetByUserIdHandler(UserManager<ApplicationUser> usermanager)
        {
            _usermanager = usermanager;
        }
        #endregion


        #region HandlerMethods
        #endregion
        public async Task<Response<GetUserByIdResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            ///if user exist
            var user = await _usermanager.FindByIdAsync(request._UserId.ToString());
            if (user == null)
            {
                return BadRequest<GetUserByIdResponse>("UserNotFound");
            }

            var userResponse = new GetUserByIdResponse
            {
                FullName = user.FullName,
                Email = user.Email,
                Address = user.Address,
                Country = user.Country
            };
            return Success<GetUserByIdResponse>(userResponse, "UserFound");
        }
    }
}
