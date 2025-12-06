using BusinessCore.BankSystem.Features.Authorization.Commands.Requests;
using BusinessCore.BankSystem.Features.Authorization.Queries.Requests;
using Domainlayer.BankSystem.AppMetaData;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Controllers
{

    [ApiController]
    public class AuthorizationController : AppControllerBase
    {
        private readonly IMediator _mediator;
        public AuthorizationController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost(Router.AuthorizationService.Create)]

        public async Task<IActionResult> AddRole([FromForm]AddRoleCommand Request)
        { 
          var response = await _mediator.Send(Request);
            try
            {
                return NewResult(response);

            }
            catch 
            {
                return BadRequest("Error");
            }
        }
        [HttpPut(Router.AuthorizationService.EditRole)]
        public async Task<IActionResult> EditRole([FromForm] EditRoleCommand Request)
        {
            var response = await _mediator.Send(Request);
            try
            {
                return NewResult(response);

            }
            catch
            {
                return BadRequest("Error");
            }
        }
        [HttpPost(Router.AuthorizationService.DeleteRole)]
        public async Task<IActionResult> DeleteRole([FromForm] DeleteRoleCommand request)
        {
            var response = await _mediator.Send(request);
            try
            {
                return NewResult(response);

            }
            catch
            {
                return BadRequest("Error");
            }
        }
        [HttpGet(Router.AuthorizationService.GetRoles)]
        public async Task<IActionResult> GetRoles([FromQuery] GetRolesQuery request)
        {
            var response = await _mediator.Send(request);
            try
            {
                return NewResult(response);

            }
            catch
            {
                return BadRequest("Error");
            }
        }

        [HttpGet(Router.AuthorizationService.IsRoleExist)]
        public async Task<IActionResult> IsRoleExist([FromQuery] IsRoleExistCommand request)
        {
            var response = await _mediator.Send(request);
            try
            {
                return NewResult(response);

            }
            catch
            {
                return BadRequest("Error");
            }
        }
        
        [HttpPost(Router.AuthorizationService.AddRoleToUser)]
        public async Task<IActionResult> AddRoleToUser(AddRoleToUserCommand request)
        {
            var response = await _mediator.Send(request);
            try
            {
                return NewResult(response);

            }
            catch
            {
                return BadRequest("Error");
            }
        }
        [HttpPut(Router.AuthorizationService.EditRoleToUser)]
        public async Task<IActionResult> EditRoleToUser(EditRoleToUserCommand request)
        {
            var response = await _mediator.Send(request);
            try
            {
                return NewResult(response);

            }
            catch
            {
                return BadRequest("Error");
            }
        }
        [HttpDelete(Router.AuthorizationService.DeleteRoleFromUser)]
        public async Task<IActionResult> DeleteRoleFromUser(DeleteRoleFromUserCommand request)
        {
            var response = await _mediator.Send(request);
            try
            {
                return NewResult(response);

            }
            catch
            {
                return BadRequest("Error");
            }
        }

        [HttpGet(Router.AuthorizationService.ManageUserRoles)]

        public async Task<IActionResult> ManageUserRoles([FromQuery] ManageUserRoleQuery request)
        {
            var response = await _mediator.Send(request);
            try
            {
                return NewResult(response);

            }
            catch
            {
                return BadRequest("Error");
            }
        }

        [HttpPut(Router.AuthorizationService.UpdateUserRoles)]
        public async Task<IActionResult> UpdateUserRoles([FromBody] UpdateUserRoleCommand request)
        {
            var response = await _mediator.Send(request);
            try
            {
                return NewResult(response);

            }
            catch
            {
                return BadRequest("Error");
            }
        }

        [HttpGet(Router.AuthorizationService.ManageUserClaims)]
        public async Task<IActionResult> ManageUserClaims([FromRoute] int id)
        {
            var response = await _mediator.Send(new ManageUserClaimQuery() { UserId = id});
            try
            {
                return NewResult(response);

            }
            catch
            {
                return BadRequest("Error");
            }
        }
        [HttpPut(Router.AuthorizationService.UpdateUserClaims)]
        public async Task<IActionResult> UpdateUserClaims([FromBody] UpdateUserCommand request)
        {
            var response = await _mediator.Send(request);
            try
            {
                return NewResult(response);

            }
            catch
            {
                return BadRequest("Error");
            }
        }
    }
}
