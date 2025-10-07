using BusinessCore.BankSystem.Features.User.Commands.Requests;
using BusinessCore.BankSystem.Features.User.Queries.Requests;
using Domainlayer.BankSystem.AppMetaData;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace BankSystem.Controllers
{
    //[Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class ApplicationUserController : AppControllerBase
    {
        private readonly IMediator _mediator;
        public ApplicationUserController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost(Router.ApplicationUser.Create)]
        public async Task<IActionResult> AddUserAsync([FromForm] AddUserCommand command)
        {
            try
            {
                var response = await _mediator.Send(command);
                return NewResult(response);
            }
            catch (Exception ex)
            {
                return BadRequest("Not Create");
            }
        }
        [HttpGet(Router.ApplicationUser.GetUserById)]
        public async Task<IActionResult> GetUserByIdAsync([FromRoute] int id)
        {
            try
            {
                var response = await _mediator.Send(new GetUserByIdQuery(id));
                return NewResult(response);
            }
            catch (Exception ex)
            {
                return BadRequest("Not Found User");
            }
        }

        [HttpPut(Router.ApplicationUser.Edit)]
        public async Task<IActionResult> EditUserAsync([FromBody] EditUserCommand command)
        {
            try
            {
                var response = await _mediator.Send(command);
                return NewResult(response);
            }
            catch (Exception ex)
            {
                return BadRequest("Fail Edit User");
            }
        }

        [HttpDelete(Router.ApplicationUser.Delete)]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] int id)
        {
            try
            {
                var response = await _mediator.Send(new DeleteUserCommand(id));
                return NewResult(response);
            }
            catch (Exception ex)
            {
                return BadRequest("Fail Delete User");
            }
        }

        [HttpPut(Router.ApplicationUser.ChangePassword)]
        public async Task<IActionResult> ChangeUserPasswordAsync([FromBody] ChangeUserPasswordCommand command)
        {
            try
            {
                var response = await _mediator.Send(command);
                return NewResult(response);
            }
            catch (Exception ex)
            {
                return BadRequest("Fail changePassword User");
            }
        }
       
        [HttpPost(Router.ApplicationUser.GetUserPaginatedList)]
        public async Task<IActionResult> PaginatedAsync([FromForm] GetUserPaginatedQuery request)
        {

            try 
            { 
              var response =await _mediator.Send(request);
                return Ok(new {Response = response});
            }
            catch 
            { 
              return BadRequest("Fail Get User PaginatedList");
            }
        }
    }
}
