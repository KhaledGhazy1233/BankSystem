using BusinessCore.BankSystem.Features.User.Commands.Requests;
using Domainlayer.BankSystem.AppMetaData;
using Domainlayer.BankSystem.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
                return Ok(new { Response = response });
            }
            catch (Exception ex)
            {
                return BadRequest("Fail SignIn ");
            }
        }
        [HttpPost(Router.AuthenticationService.Logout)]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var response = await _mediator.Send(new LogoutCommand
            {
                UserId = userId,
                RefreshToken = request.Refreshtoken
            });
            return NewResult(response);
        }

        [HttpPost(Router.AuthenticationService.LogoutAll)]
        [Authorize]
        public async Task<IActionResult> LogoutAll()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var response = await _mediator.Send(new LogoutCommand
            {
                UserId = userId,
                RevokeAllTokens = true
            });
            return NewResult(response);
        }


    }
}
