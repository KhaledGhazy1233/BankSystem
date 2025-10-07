using BusinessCore.BankSystem.Features.User.Commands.Requests;
using Domainlayer.BankSystem.AppMetaData;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Controllers
{
    
    [ApiController]
    public class AuthenticationController : AppControllerBase
    {
        private readonly IMediator _mediator;
        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost(Router.ApplicationUser.SignIn)]
        public async Task<IActionResult> SignInAsync([FromForm] SignInCommand command)
        {
            try
            {
                var response = await _mediator.Send(command);
                return NewResult(response);
            }
            catch (Exception ex)
            {
                return BadRequest("Fail SignIn ");
            }
        }
        [HttpPost(Router.AuthenticationService.GenerateAccessToken)]
        public async Task<IActionResult> GenerateAccessToken([FromForm] RefreshTokenCommand command)
        {
            try
            {
                var response = await _mediator.Send(command);
                return Ok(new { Response =response});
            }
            catch (Exception ex)
            {
                return BadRequest("Fail SignIn ");
            }
        }



    }
}
