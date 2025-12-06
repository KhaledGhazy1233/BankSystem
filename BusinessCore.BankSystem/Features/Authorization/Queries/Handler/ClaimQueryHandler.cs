using ApplicationLayer.BankSystem.AbstractServices;
using BusinessCore.BankSystem.Bases;
using BusinessCore.BankSystem.Features.Authorization.Queries.Requests;
using Domainlayer.BankSystem.Entites;
using Domainlayer.BankSystem.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCore.BankSystem.Features.Authorization.Queries.Handler
{
    public class ClaimQueryHandler : ResponseHandler
                                    , IRequestHandler<ManageUserClaimQuery, Response<ManageUserClaimResult>>
    {
        #region Fields
        public RoleManager<Role> ?_roleManager { get; set; }
        public IAuthorizationService ?_authorizationService { get; set; }
        public UserManager<ApplicationUser> _userManager { get; set; }

        #endregion



        #region Constructor
        public ClaimQueryHandler(RoleManager<Role> roleManager, IAuthorizationService authorizationService, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _authorizationService = authorizationService;
            _userManager = userManager;
        }
        #endregion



        #region HandlerFunction
        public async Task<Response<ManageUserClaimResult>> Handle(ManageUserClaimQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
                return BadRequest<ManageUserClaimResult>("No User Found");
            var response = await _authorizationService.ManageUserClaim(user);
              if(response==null)
                return BadRequest<ManageUserClaimResult>("No Claim Found");
             return Success(response);
        }

        #endregion

    }
}
