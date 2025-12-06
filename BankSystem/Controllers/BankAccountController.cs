using BusinessCore.BankSystem.Features.BankAccount.Commands.Models;
using BusinessCore.BankSystem.Features.BankAccount.Queries.Models;
using Domainlayer.BankSystem.AppMetaData;
using MediatR;
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

        [HttpGet(Router.BankAccount.GetAll)]
        public async Task<IActionResult> GetAllAccountsAsync()
        {
            try
            {
                var response = await _mediator.Send(new GetAllBankAccountQuery());
                return NewResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all accounts");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet(Router.BankAccount.GetById)]
        public async Task<IActionResult> GetAccountByIdAsync([FromRoute] int id)
        {
            try
            {
                var response = await _mediator.Send(new GetBankAccountByIdQuery(id));
                return NewResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting account by ID: {id}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet(Router.BankAccount.GetByUserId)]
        public async Task<IActionResult> GetAllAccountsByUserIdAsync([FromRoute] int id)
        {
            try
            {
                var response = await _mediator.Send(new GetBankAccountAllByIdQuery(id));
                return NewResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting all accounts by User ID: {id}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost(Router.BankAccount.Create)]
        public async Task<IActionResult> CreateAccountAsync([FromBody] CreateAccountCommand command)
        {
            try
            {
                var response = await _mediator.Send(command);
                return NewResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating account");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPut(Router.BankAccount.UpdateAccountType)]
        public async Task<IActionResult> UpdateAccountTypeAsync([FromBody] UpdateAccountTypeCommand command)
        {
            try
            {
                var response = await _mediator.Send(command);
                return NewResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating account type");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpDelete(Router.BankAccount.Delete)]
        public async Task<IActionResult> DeleteAccountAsync([FromBody] DeleteAccountCommand command)
        {
            try
            {
                var response = await _mediator.Send(command);
                return NewResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting account");
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}