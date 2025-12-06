using ApplicationLayer.BankSystem.AbstractServices;
using BusinessCore.BankSystem.Bases;
using BusinessCore.BankSystem.Features.BankAccount.Commands.Models;
using MediatR;

namespace BusinessCore.BankSystem.Features.BankAccount.Commands.Handler
{
    public class BankAccountCommandHandler : ResponseHandler
                                         , IRequestHandler<CreateAccountCommand, Response<string>>
                                         , IRequestHandler<UpdateAccountTypeCommand, Response<string>>
                                         , IRequestHandler<DeleteAccountCommand, Response<string>>
    {


        #region Fields
        public IBankAccountService _accountService { get; set; }
        #endregion

        #region Constructor
        public BankAccountCommandHandler(IBankAccountService accountService)
        {
            _accountService = accountService;
        }
        #endregion
        #region MethodsHandler
        public async Task<Response<string>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var result = await _accountService.CreateAccountAsync(request);
            if (result == true)
                return Success("Account created successfully.");
            else
                return BadRequest<string>("Failed to create account.");
        }

        public async Task<Response<string>> Handle(UpdateAccountTypeCommand request, CancellationToken cancellationToken)
        {
            var result = await _accountService.UpdateAccountTypeAsync(request.Id, (int)request.accountTypeEnum);
            if (result == true)
                return Success("Account type updated successfully.");
            else
                return BadRequest<string>("Failed to update account type.");
        }

        public async Task<Response<string>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            var result = await _accountService.DeleteAccountAsync(request.id);
            if (result == true)
                return Success("Account deleted successfully.");
            else
                return BadRequest<string>("Failed to delete account.");
        }
        #endregion
    }
}
