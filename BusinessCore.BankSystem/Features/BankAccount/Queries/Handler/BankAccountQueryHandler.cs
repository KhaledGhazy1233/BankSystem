using ApplicationLayer.BankSystem.AbstractServices;
using BusinessCore.BankSystem.Features.BankAccount.Queries.Models;
using BusinessCore.BankSystem.Features.BankAccount.Queries.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BusinessCore.BankSystem.Features.BankAccount.Queries.Handler
{
    public class BankAccountQueryHandler :
                                         IRequestHandler<GetAllBankAccountQuery, Response<IEnumerable<BankAccountResponse>>>
                                        , IRequestHandler<GetBankAccountByIdQuery, Response<BankAccountResponse>>
                                        , IRequestHandler<GetMyAccountQuery, Response<IEnumerable<BankAccountResponse>>>
                                        , IRequestHandler<GetByAccountNumberQuery, Response<BankAccountResponse>>
                                        , IRequestHandler<GetAccountsByUserIdQuery, Response<IEnumerable<BankAccountResponse>>>
    {

        #region Fields
        private readonly IBankAccountService _accountService;
        private readonly ILogger<BankAccountQueryHandler> _logger;
        #endregion

        #region Constructor
        public BankAccountQueryHandler(IBankAccountService accountService, ILogger<BankAccountQueryHandler> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }
        #endregion

        #region HandelerMethods

        public async Task<Response<IEnumerable<BankAccountResponse>>> Handle(GetAllBankAccountQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Handling GetAllAccountsQuery");

                var accounts = await _accountService.GetAllAccountsAsync();


                if (accounts == null || !accounts.Any())
                {
                    _logger.LogInformation("No accounts found in the system");

                    return Response<IEnumerable<BankAccountResponse>>.Success(
                        new List<BankAccountResponse>(),
                        "No accounts found");
                }


                _logger.LogInformation("Retrieved {Count} accounts", accounts.Count());


                var accountsResponse = accounts.Select(a => new BankAccountResponse
                {
                    Id = a.Id,
                    AccountNumber = a.AccountNumber,
                    Balance = (decimal)a.Balance,
                    Currency = a.Currency,
                    AccountType = a.AccountType,

                }).ToList();
                return Response<IEnumerable<BankAccountResponse>>.Success(
                    accountsResponse,
                    $"{accounts.Count()} accounts retrieved successfully");
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt to GetAllAccounts");
                // بنستخدم الـ Static Method: Unauthorized
                return Response<IEnumerable<BankAccountResponse>>.Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while handling GetAllAccountsQuery");
                // بنستخدم الـ Static Method: InternalServerError
                return Response<IEnumerable<BankAccountResponse>>.InternalServerError(
                    "An error occurred while retrieving accounts");
            }
        }

        public async Task<Response<BankAccountResponse>> Handle(GetBankAccountByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Handling GetAccountByIdQuery for ID: {AccountId}", request.Id);
                var account = await _accountService.GetAccountByIdAsync(request.Id);

                var accountresponse = new BankAccountResponse
                {
                    Id = account.Id,
                    AccountNumber = account.AccountNumber,
                    Balance = (decimal)account.Balance,
                    Currency = account.Currency,
                    AccountType = account.AccountType,
                };
                return Response<BankAccountResponse>.Success(accountresponse, "Account retrieved successfully");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Account {AccountId} not found", request.Id);
                return Response<BankAccountResponse>.NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Response<BankAccountResponse>.Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAccountByIdQuery");
                return Response<BankAccountResponse>.InternalServerError("Error retrieving account");
            }
        }

        public async Task<Response<BankAccountResponse>> Handle(GetByAccountNumberQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Handling GetAccountByAccountNumberQuery: {AccountNumber}", request.AccountNumber);
                var account = await _accountService.GetByAccountNumberAsync(request.AccountNumber);
                var accountrespone = new BankAccountResponse
                {
                    Id = account.Id,
                    AccountNumber = account.AccountNumber,
                    Balance = (decimal)account.Balance,
                    Currency = account.Currency,
                    AccountType = account.AccountType,
                };

                return Response<BankAccountResponse>.Success(accountrespone);
            }
            catch (KeyNotFoundException ex)
            {
                return Response<BankAccountResponse>.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAccountByAccountNumberQuery");
                return Response<BankAccountResponse>.InternalServerError("Error retrieving account");
            }
        }

        public async Task<Response<IEnumerable<BankAccountResponse>>> Handle(GetMyAccountQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Handling GetMyAccountsQuery");
                var accounts = await _accountService.GetMyAccountsAsync();

                if (accounts == null || !accounts.Any())
                {
                    return Response<IEnumerable<BankAccountResponse>>.Success(new List<BankAccountResponse>(), "You don't have any accounts yet");
                }
                var accountsResponse = accounts.Select(a => new BankAccountResponse
                {
                    Id = a.Id,
                    AccountNumber = a.AccountNumber,
                    Balance = (decimal)a.Balance,
                    Currency = a.Currency,
                    AccountType = a.AccountType,
                }).ToList();
                return Response<IEnumerable<BankAccountResponse>>.Success(accountsResponse, "Accounts retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetMyAccountsQuery");
                return Response<IEnumerable<BankAccountResponse>>.InternalServerError("Error retrieving your accounts");
            }
        }

        public async Task<Response<IEnumerable<BankAccountResponse>>> Handle(GetAccountsByUserIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Handling GetAccountsByUserIdQuery for: {UserId}", request.UserId);
                var accounts = await _accountService.GetAccountsByUserIdAsync(request.UserId);
                var accountsResponse = accounts.Select(a => new BankAccountResponse
                {
                    Id = a.Id,
                    AccountNumber = a.AccountNumber,
                    Balance = (decimal)a.Balance,
                    Currency = a.Currency,
                    AccountType = a.AccountType,
                }).ToList();

                return Response<IEnumerable<BankAccountResponse>>.Success(accountsResponse ?? new List<BankAccountResponse>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAccountsByUserIdQuery");
                return Response<IEnumerable<BankAccountResponse>>.InternalServerError("Error retrieving accounts");
            }
        }


        #endregion
    }
}
