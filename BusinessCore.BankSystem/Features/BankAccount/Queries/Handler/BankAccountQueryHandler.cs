using ApplicationLayer.BankSystem.AbstractServices;
using BusinessCore.BankSystem.Bases;
using BusinessCore.BankSystem.Features.BankAccount.Queries.Models;
using BusinessCore.BankSystem.Features.BankAccount.Queries.Responses;
using MediatR;

namespace BusinessCore.BankSystem.Features.BankAccount.Queries.Handler
{
    public class BankAccountQueryHandler : ResponseHandler
                                        , IRequestHandler<GetAllBankAccountQuery, Response<IEnumerable<BankAccountResponse>>>
                                        , IRequestHandler<GetBankAccountByIdQuery, Response<BankAccountResponse>>
                                        , IRequestHandler<GetBankAccountAllByIdQuery, Response<IEnumerable<BankAccountResponse>>>
    {

        #region Fields
        public IBankAccountService _accountService { get; set; }
        #endregion

        #region Constructor
        public BankAccountQueryHandler(IBankAccountService accountService)
        {
            _accountService = accountService;
        }
        #endregion

        #region HandelerMethods

        public async Task<Response<IEnumerable<BankAccountResponse>>> Handle(GetAllBankAccountQuery request, CancellationToken cancellationToken)
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            var response = new List<BankAccountResponse>();
            foreach (var account in accounts)
            {
                var accountResponse = new BankAccountResponse
                {
                    Id = account.Id,
                    AccountType = account.AccountType,
                    AccountNumber = account.AccountNumber,
                    Balance = (decimal)account.Balance,
                    Currency = account.Currency,
                    IsActive = account.IsActive,
                    CreatedData = account.CreatedData,
                    UpdatedData = account.UpdatedData,
                    UserId = account.UserId
                };
                response.Add(accountResponse);
            }
            if (response.Count > 0)
                return Success<IEnumerable<BankAccountResponse>>(response);
            else
                return NotFound<IEnumerable<BankAccountResponse>>("No accounts found.");
        }

        public async Task<Response<BankAccountResponse>> Handle(GetBankAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var account = await _accountService.GetAccountByIdAsync(request.Id);
            if (account == null)
                return NotFound<BankAccountResponse>("Not found object");
            var accountResponse = new BankAccountResponse
            {
                Id = account.Id,
                AccountType = account.AccountType,
                AccountNumber = account.AccountNumber,
                Balance = (decimal)account.Balance,
                Currency = account.Currency,
                IsActive = account.IsActive,
                CreatedData = account.CreatedData,
                UpdatedData = account.UpdatedData,
                UserId = account.UserId
            };
            return Success<BankAccountResponse>(accountResponse);

        }

        public async Task<Response<IEnumerable<BankAccountResponse>>> Handle(GetBankAccountAllByIdQuery request, CancellationToken cancellationToken)
        {
            var accounts = await _accountService.GetAccountsByUserIdAsync(request.Id);
            var accountresponse = accounts.Select(account => new BankAccountResponse
            {
                Id = account.Id,
                AccountType = account.AccountType,
                AccountNumber = account.AccountNumber,
                Balance = (decimal)account.Balance,
                Currency = account.Currency,
                IsActive = account.IsActive,
                CreatedData = account.CreatedData,
                UpdatedData = account.UpdatedData,
                UserId = account.UserId
            }).ToList();
            if (accountresponse.Count > 0)
                return Success<IEnumerable<BankAccountResponse>>(accountresponse);
            else
                return NotFound<IEnumerable<BankAccountResponse>>("No accounts found for the user.");
        }

        #endregion
    }
}
