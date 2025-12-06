using ApplicationLayer.BankSystem.AbstractServices;
using BusinessCore.BankSystem.Bases;
using BusinessCore.BankSystem.Features.Authorization.Commands.Requests;
using Domainlayer.BankSystem.Entites;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCore.BankSystem.Features.Authorization.Commands.Handler
{
    public class ClaimCommandHandler :ResponseHandler,
                                     IRequestHandler<UpdateUserCommand,Response<string>>
    {
        #region Fields
        public RoleManager<Role>? _roleManager { get; set; }
        public IAuthorizationService? _authorizationService { get; set; }
        public UserManager<ApplicationUser> _userManager { get; set; }

        #endregion



        #region Constructor
        public ClaimCommandHandler(RoleManager<Role> roleManager, IAuthorizationService authorizationService, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _authorizationService = authorizationService;
            _userManager = userManager;
        }
        #endregion
        #region HandlerFunction
        public async Task<Response<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var response = await _authorizationService.UpdateUserClaim(request);
            if (response == null)
                return BadRequest<string>("No Claim Found");
            return Success(response);
        }
        #endregion
    }
}
