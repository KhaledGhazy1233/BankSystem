using BusinessCore.BankSystem.Bases;
using BusinessCore.BankSystem.Features.Authorization.Commands.Requests;
using Domainlayer.BankSystem.Entites;
using MediatR;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationLayer.BankSystem.AbstractServices;
using System.Data;
using Domainlayer.BankSystem.Requests;
using BusinessCore.BankSystem.Features.Authorization.Queries.Requests;
using Domainlayer.BankSystem.Results;

namespace BusinessCore.BankSystem.Features.Authorization.Commands.Handler
{
    public class RoleCommandHandler : ResponseHandler,
                                  IRequestHandler<AddRoleCommand, Response<string>>,
                                  IRequestHandler<EditRoleCommand, Response<string>>,
                                  IRequestHandler<DeleteRoleCommand, Response<string>>,
                                  IRequestHandler<IsRoleExistCommand, Response<bool>>,
                                  IRequestHandler<EditRoleToUserCommand, Response<string>>,
                                  IRequestHandler<AddRoleToUserCommand, Response<string>>,
                                  IRequestHandler<DeleteRoleFromUserCommand, Response<string>>,
                                  IRequestHandler<UpdateUserRoleCommand, Response<string>>
        
        
        {        
        #region Fields
        public RoleManager<Role> _roleManager { get; set; }
        public IAuthorizationService _authorizationService { get; set; }
        #endregion

        #region Constructor
        public RoleCommandHandler(RoleManager<Role> roleManager, IAuthorizationService authorizationService)
        {
            _roleManager = roleManager;
            _authorizationService = authorizationService;
        }

        #endregion


        #region handlerFunction

        #endregion
        public async Task<Response<string>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            var response =await _authorizationService.AddRoleAsync(request.RoleName);
            if (response == "Successful")
                return Success<string>("");
            else
            {
                return BadRequest<string>("Failed");
            }
        }


        public async Task<Response<string>> Handle(EditRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationService.EditRoleAsync(request);
         
            if (result == "Successful")
                return Success("EditRoleSuccessfully");
            else

                return BadRequest<string>("EditRoleFailed");
        }
      
        
        public async Task<Response<string>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationService.DeleteRoleAsync(request._Id);

            if (result == "Successful")
                return Success("Delete Role Successfully");
            else

                return BadRequest<string>("DeleteRoleFailed");
        }

        public async Task<Response<bool>> Handle(IsRoleExistCommand request, CancellationToken cancellationToken)
        {
            var resultlist = await _authorizationService.IsRoleExistAsync(request.Name);
            if (resultlist == null)
                return BadRequest<bool>("No Roles Found");
            else
                return Success(resultlist);
        }

        public async Task<Response<string>> Handle(EditRoleToUserCommand request, CancellationToken cancellationToken)
        {
               var result = await _authorizationService.EditRoleFromUser(request.Id ,request.oldName,request.newName);
             switch (result)
              { 
               case "Successful": return Success("Edit Role To User Successfully");
               case "OldRoleNotFound": return BadRequest<string>("OldRoleNotFound");
               case "UserNotFound": return BadRequest<string>("User Not Found");
               default: return BadRequest<string>("Edit Role To User Failed");
               }
        }

        public async Task<Response<string>> Handle(AddRoleToUserCommand request, CancellationToken cancellationToken)
        {
            var result  =await _authorizationService.AddRoleToUser(request.Id ,request.RoleName);
            switch (result) 
            {
                case "UserNotFound": return BadRequest<string>("UserNotFound");
                case "RoleNotFound": return BadRequest<string>("RoleNotFound");
                case "Successful": return Success("Add Role To User Successfully");
                default: return BadRequest<string>("Add Role To User Failed");
            };
        }

        public async Task<Response<string>> Handle(DeleteRoleFromUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationService.RemoveRoleFromUser(request.Id);
            switch (result)
            {
                case "UserNotFound": return BadRequest<string>("UserNotFound");
                case "NoRolesFound": return BadRequest<string>("RoleNotFound");
                case "Successful": return Success("Add Role To User Successfully");
                default: return BadRequest<string>("Add Role To User Failed"); ;
            }
        }

        public async Task<Response<string>> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
        {
            var response = await _authorizationService.UpdateUserRoles(request);
            switch (response) {
                case "UserNotFound": return BadRequest<string>("UserNotFound");
                case "Successful": return Success("Update Role To User Successfully");
                default: return BadRequest<string>("Update Role To User Failed");
            };
        }
    }
    }

