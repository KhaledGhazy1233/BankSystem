using ApplicationLayer.BankSystem.AbstractServices;
using BusinessCore.BankSystem.Features.BankAccount.Commands.Models;
using BusinessCore.BankSystem.Features.BankAccount.Queries.Responses;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Security;

namespace BusinessCore.BankSystem.Features.BankAccount.Commands.Handler
{
    public class BankAccountCommandHandler :
                                           IRequestHandler<CreateAccountCommand, Response<BankAccountResponse>>
                                         , IRequestHandler<UpdateAccountTypeCommand, Response<string>>
                                         , IRequestHandler<DeleteAccountCommand, Response<string>>
    {


        #region Fields
        public IBankAccountService _accountService { get; set; }
        private readonly ILogger<BankAccountCommandHandler> _logger;
        #endregion

        #region Constructor
        public BankAccountCommandHandler(IBankAccountService accountService, ILogger<BankAccountCommandHandler> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }
        #endregion
        #region MethodsHandler

        public async Task<Response<BankAccountResponse>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Creating account for user {UserId}", request.UserId);

                var account = await _accountService.CreateAccountAsync(request);

                _logger.LogInformation("Account {AccountNumber} created successfully", account.AccountNumber);
                var accountresponse = new BankAccountResponse
                {
                    AccountNumber = account.AccountNumber,
                    AccountType = account.AccountType,
                    Balance = (Decimal)account.Balance,
                    CreatedData = account.CreatedData
                };
                // استخدام Static Method: Created
                return Response<BankAccountResponse>.Created(accountresponse, $"Account {account.AccountNumber} created successfully");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid data for account creation");
                return Response<BankAccountResponse>.BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized attempt to create account");
                return Response<BankAccountResponse>.Unauthorized(ex.Message);
            }
            catch (SecurityException ex)
            {
                _logger.LogWarning(ex, "Security violation");
                return Response<BankAccountResponse>.Forbidden(ex.Message); // استخدام Forbidden
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Critical error during account creation");
                return Response<BankAccountResponse>.InternalServerError("An error occurred. Please try again later.");
            }
        }

        public async Task<Response<string>> Handle(UpdateAccountTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Updating account {Id} type", request.Id);

                var result = await _accountService.UpdateAccountTypeAsync(request.Id, request.accountTypeEnum);

                if (result)
                {
                    // استخدام Static Method: Success
                    return Response<string>.Success("Account type updated successfully");
                }

                return Response<string>.BadRequest("Failed to update account type");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Account {Id} not found", request.Id);
                return Response<string>.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating account {Id}", request.Id);
                return Response<string>.InternalServerError();
            }
        }

        public async Task<Response<string>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Deleting account {Id}", request.id);

                var result = await _accountService.DeleteAccountAsync(request.id);

                if (result)
                {
                    // استخدام Static Method: Deleted
                    return Response<string>.Deleted("Account deleted successfully");
                }

                return Response<string>.BadRequest("Failed to delete account");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Account {Id} not found", request.id);
                return Response<string>.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting account {Id}", request.id);
                return Response<string>.InternalServerError();
            }
        }



        #endregion
    }
}
