using BusinessCore.BankSystem.Bases;
using BusinessCore.BankSystem.Features.User.Commands.Requests;
using Domainlayer.BankSystem.Entites;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ApplicationLayer.BankSystem.AbstractServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCore.BankSystem.Features.User.Commands.Handler
{
    public class ApplicationUserCommandHandler : ResponseHandler,
                                         IRequestHandler<AddUserCommand, Response<string>>,
                                         IRequestHandler<EditUserCommand, Response<string>>,
                                         IRequestHandler<DeleteUserCommand, Response<string>>,
                                         IRequestHandler<ChangeUserPasswordCommand, Response<string>>,
                                         IRequestHandler<SignInCommand, Response<string>>
    {
                                         
        #region Fields

        private  UserManager<ApplicationUser> _UserManager { get; set; }
        private  SignInManager<ApplicationUser> _SignInManager { get; set; }

        private IJwtTokenService _jwttokenService { get; set; }


        #endregion

        #region Constructors
        public ApplicationUserCommandHandler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
                                   IJwtTokenService jwttokenService)
        {
            _UserManager = userManager;
            _SignInManager = signInManager;
            _jwttokenService = jwttokenService;
        }
        #endregion


        #region HandlerFunctions        
        #endregion
        public async Task<Response<string>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
           
            var useremail = _UserManager.FindByEmailAsync(request.Email).Result;
            if (useremail != null)
            {
                return BadRequest<string>("EmailIsExists");
            }
            var userName = _UserManager.FindByNameAsync(request.UserName).Result;
            if (userName != null)
            {
                return BadRequest<string>("UserNameIsExists");
            }
            var user = new ApplicationUser
            {
                FullName = request.FullName,
                UserName = request.UserName,
                Email = request.Email,
                Address = request.Address,
                Country = request.Country,
                PhoneNumber = request.PhoneNumber
            };
            var result = await _UserManager.CreateAsync(user, request.Password);
            if (result == null)
            { 
             return BadRequest<string>(result.Errors.FirstOrDefault()?.Description ?? "ErrorCreatingUser");
            }
            return Success<string>(user.Id.ToString(), "UserCreatedSuccessfully");
        }

        public async Task<Response<string>> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
   
            var olduser = _UserManager.FindByIdAsync(request.Id.ToString()).Result;
            if (olduser == null)
            {
                return BadRequest<string>("UserNotFound");
            }
            
            olduser.FullName = request.FullName;
            olduser.UserName = request.UserName;
            olduser.Email = request.Email;
            olduser.Address = request.Address;
            olduser.Country = request.Country;
            olduser.PhoneNumber = request.PhoneNumber;
            var result = await _UserManager.UpdateAsync(olduser);
            if (result != null)
            {
                return BadRequest<string>(result.Errors.FirstOrDefault()?.Description ?? "ErrorUpdatingUser");
            }
            return Success<string>(olduser.Id.ToString(), "UserUpdatedSuccessfully");
        }

        public async Task<Response<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
      
            var user = _UserManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
            {
                return BadRequest<string>("UserNotFound");
            }
            var result = await _UserManager.DeleteAsync(user.Result);
            if (result == null)
            {
                return BadRequest<string>(result.Errors.FirstOrDefault()?.Description ?? "ErrorDeletingUser");
            }
            return Success<string>("SuccessDeleteing");
        }

        public async Task<Response<string>> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
          
                ///Do User exists Or Not
                ///is exist change password
             var user = _UserManager.FindByIdAsync(request.Id.ToString()).Result;
             if(user == null)
                {
                    return BadRequest<string>("UserNotFound");
                }
             var result = await _UserManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
             if (result != null)
             {
                 return BadRequest<string>(result.Errors.FirstOrDefault()?.Description ?? "ErrorChangingPassword");
             }
             return Success<string>("PasswordChangedSuccessfully");
        }

        public async Task<Response<string>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            ///Logic
            var username = await _UserManager.FindByNameAsync(request.UserName);
            if (username == null)
            {
                return BadRequest<string>("UserName Not Found"); 
            }
            var userpassword = _SignInManager.CheckPasswordSignInAsync(username, request.Password,false);
            if (!userpassword.IsCompletedSuccessfully)
            { 
               return BadRequest<string>("Password Not Correct");
            }
            var token = await _jwttokenService.GenerateJWTToken(username);
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest<string>("ErrorGeneratingToken");
            }
            return Success<string>(token, "SignInSuccessful");
              
        }
    }
}
