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
    public class RoleQuaryHandler : ResponseHandler,
                                   IRequestHandler<GetRolesQuery, Response<List<string>>>,
                                   IRequestHandler<ManageUserRoleQuery, Response<ManageUserRoleResult>>

    {
        #region Fields
        public RoleManager<Role> _roleManager { get; set; }
        public IAuthorizationService _authorizationService { get; set; }
        #endregion

        #region Constructor
        public RoleQuaryHandler(RoleManager<Role> roleManager, IAuthorizationService authorizationService)
        {
            _roleManager = roleManager;
            _authorizationService = authorizationService;
        }

        #endregion

        #region HandlerFunction

        public async Task<Response<List<string>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var resultlist = await _authorizationService.GetRolesListAsync();
            if (resultlist == null)
                return BadRequest<List<string>>("No Roles Found");
            else
                return Success(resultlist);
        }



        public async Task<Response<ManageUserRoleResult>> Handle(ManageUserRoleQuery request, CancellationToken cancellationToken)
        {
           var response = await _authorizationService.ManageUserRoles(request.Id);
           if(response == null)
                return BadRequest<ManageUserRoleResult>("No User Found");
           return Success(response);
        }
        #endregion
    }
}
