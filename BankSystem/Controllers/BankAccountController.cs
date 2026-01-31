//using BusinessCore.BankSystem.Features.BankAccount.Commands.Models;
using BusinessCore.BankSystem.Features.BankAccount.Commands.Models;
using BusinessCore.BankSystem.Features.BankAccount.Queries.Models;
using BusinessCore.BankSystem.Features.BankAccount.Queries.Responses;
using Domainlayer.BankSystem.AppMetaData;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace BankSystem.Controllers
{

    [ApiController]
    public class BankAccountController : AppControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BankAccountController> _logger;

        public BankAccountController(IMediator mediator, ILogger<BankAccountController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        #region Query Endpoints (GET)
        [HttpGet(Router.BankAccount.GetAll)]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(Response<List<BankAccountResponse>>), 200)]
        [ProducesResponseType(typeof(Response<string>), 401)]
        [ProducesResponseType(typeof(Response<string>), 403)]
        public async Task<IActionResult> GetAll()
        {
            var response = await _mediator.Send(new GetAllBankAccountQuery());
            return StatusCode((int)response.StatusCode, response);
        }



        [HttpGet(Router.BankAccount.GetMyAccounts)]
        [ProducesResponseType(typeof(Response<List<BankAccountResponse>>), 200)]
        public async Task<IActionResult> GetMyAccounts()
        {
            var response = await _mediator.Send(new GetMyAccountQuery());
            return StatusCode((int)response.StatusCode, response);
        }


        [HttpGet(Router.BankAccount.GetById)]
        [ProducesResponseType(typeof(Response<BankAccountResponse>), 200)]
        [ProducesResponseType(typeof(Response<string>), 404)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var response = await _mediator.Send(new GetAccountsByUserIdQuery(id));
            return StatusCode((int)response.StatusCode, response);
        }


        [HttpGet(Router.BankAccount.GetByAccountNumber)]
        [ProducesResponseType(typeof(Response<BankAccountResponse>), 200)]
        public async Task<IActionResult> GetByAccountNumber([FromRoute] string accountNumber)
        {
            var response = await _mediator.Send(new GetByAccountNumberQuery(accountNumber));
            return StatusCode((int)response.StatusCode, response);
        }
        #endregion


        #region Command Endpoints (POST, PUT, DELETE)
        /// <summary>
        /// Create new bank account
        /// </summary>
        [HttpPost(Router.BankAccount.Create)]
        [ProducesResponseType(typeof(Response<BankAccountResponse>), 201)]
        [ProducesResponseType(typeof(Response<string>), 400)]
        public async Task<IActionResult> Create([FromBody] CreateAccountCommand command)
        {
            var response = await _mediator.Send(command);
            // لاحظ هنا: الـ StatusCode يأتي ديناميكياً من الـ Response object
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Update account type
        /// </summary>
        [HttpPut(Router.BankAccount.UpdateAccountType)]
        [ProducesResponseType(typeof(Response<string>), 200)]
        [ProducesResponseType(typeof(Response<string>), 400)]
        [ProducesResponseType(typeof(Response<string>), 404)]
        public async Task<IActionResult> UpdateAccountType([FromBody] UpdateAccountTypeCommand command)
        {
            var response = await _mediator.Send(command);
            return StatusCode((int)response.StatusCode, response);
        }


        /// <summary>
        /// Delete account (soft delete)
        /// </summary>
        [HttpDelete(Router.BankAccount.Delete)]
        [ProducesResponseType(typeof(Response<string>), 200)]
        [ProducesResponseType(typeof(Response<string>), 404)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            // تأكد أن DeleteAccountCommand يحتوي على Id
            var response = await _mediator.Send(new DeleteAccountCommand { id = id });
            return StatusCode((int)response.StatusCode, response);
        }


        #endregion
    }
}