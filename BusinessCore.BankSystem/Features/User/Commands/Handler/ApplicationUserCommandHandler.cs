using ApplicationLayer.BankSystem.AbstractServices;
using BusinessCore.BankSystem.Bases;
using BusinessCore.BankSystem.Features.User.Commands.Requests;
using Domainlayer.BankSystem.Entites;
using Domainlayer.BankSystem.Enums;
using Domainlayer.BankSystem.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BusinessCore.BankSystem.Features.User.Commands.Handler
{
    public class ApplicationUserCommandHandler : ResponseHandler,
                                         IRequestHandler<AddUserCommand, Response<string>>,
                                         IRequestHandler<EditUserCommand, Response<string>>,
                                         IRequestHandler<DeleteUserCommand, Response<string>>,
                                         IRequestHandler<ChangeUserPasswordCommand, Response<string>>,
                                         IRequestHandler<SignInCommand, Response<JwtAuthResult>>
                                       , IRequestHandler<RefreshTokenCommand, Response<JwtAuthResult>>,
                                        IRequestHandler<LogoutCommand, Response<string>>
    {

        #region Fields

        private UserManager<ApplicationUser> _UserManager { get; set; }
        private SignInManager<ApplicationUser> _SignInManager { get; set; }

        private IAuthenticationService _jwttokenService { get; set; }
        private IAddUserImageService _addUserImageService { get; set; }
        private IFormFileService _formFileService { get; set; }


        #endregion

        #region Constructors
        public ApplicationUserCommandHandler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
                                   IAuthenticationService jwttokenService, IAddUserImageService addUserImageService, IFormFileService formFileService)
        {
            _UserManager = userManager;
            _SignInManager = signInManager;
            _jwttokenService = jwttokenService;
            _addUserImageService = addUserImageService;
            _formFileService = formFileService;
        }
        #endregion


        #region HandlerFunctions        
        #endregion
        public async Task<Response<string>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var useremail = await _UserManager.FindByEmailAsync(request.Email);
            if (useremail != null)
            {
                return BadRequest<string>("EmailIsExists");
            }

            var userName = await _UserManager.FindByNameAsync(request.UserName);
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

            if (request.Image != null)
            {
                var ImageUrl = await _formFileService.UploadImage("ApplicationUser", request.Image);
                //var imageResult = await _addUserImageService.AddUserImage(user, request.Image);
                if (ImageUrl != null)
                    user.Image = ImageUrl;
            }



            var result = await _UserManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest<string>(result.Errors.FirstOrDefault()?.Description ?? "ErrorCreatingUser");
            }


            // ✅ return واحد في الآخر
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
                return Success<string>(olduser.Id.ToString(), "UserUpdatedSuccessfully");

            }
            return BadRequest<string>(result.Errors.FirstOrDefault()?.Description ?? "ErrorUpdatingUser");
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
            if (user == null)
            {
                return BadRequest<string>("UserNotFound");
            }
            var result = await _UserManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (result != null)
            {
                return Success<string>("PasswordChangedSuccessfully");

            }
            return BadRequest<string>(result.Errors.FirstOrDefault()?.Description ?? "ErrorChangingPassword");
        }

        public async Task<Response<JwtAuthResult>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var username = await _UserManager.FindByNameAsync(request.UserName);
            if (username == null)
            {
                return BadRequest<JwtAuthResult>("UserName Not Found");
            }

            var signInResult = await _SignInManager.CheckPasswordSignInAsync(username, request.Password, false);
            if (!signInResult.Succeeded)
            {
                return BadRequest<JwtAuthResult>("Password Not Correct");
            }

            var tokenResult = await _jwttokenService.GetJWTToken(username);
            if (tokenResult == null)
            {
                return BadRequest<JwtAuthResult>("ErrorGeneratingToken");
            }

            return Success(tokenResult, "SignInSuccessful");
        }


        public async Task<Response<JwtAuthResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            // Read JWT token
            var jwtToken = _jwttokenService.ReadJwtToken(request.AccessToken);
            if (jwtToken == null)
            {
                return BadRequest<JwtAuthResult>("InvalidAccessToken");
            }

            // Validate token details
            var (result, userId, expireDate) = await _jwttokenService.ValidateTokenDetails(
                jwtToken,
                request.AccessToken,
                request.RefreshToken);

            // Check validation result
            if (result != TokenValidationStatus.Success)
            {
                var errorMessage = result switch
                {
                    TokenValidationStatus.InvalidAlgorithm => "AlgorithmIsWrong",
                    TokenValidationStatus.AccessTokenStillValid => "TokenIsNotExpired",
                    TokenValidationStatus.InvalidUserId => "InvalidUserId",
                    TokenValidationStatus.TokenNotFound => "RefreshTokenIsNotFound",
                    TokenValidationStatus.RefreshTokenExpired => "RefreshTokenIsExpired",
                    TokenValidationStatus.RefreshTokenUsedOrRevoked => "RefreshTokenUsedOrRevoked",
                    TokenValidationStatus.JwtIdMismatch => "JwtIdMismatch",
                    _ => "UnknownValidationError"
                };

                return Unauthorized<JwtAuthResult>(errorMessage);
            }

            // Find user
            var user = await _UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound<JwtAuthResult>("UserNotFound");
            }

            // Generate new tokens
            var response = await _jwttokenService.GenerateAccessTokenFromRefreshToken(
                user,
                jwtToken,
                expireDate,
                request.RefreshToken);

            return Success(response);
        }

        public async Task<Response<string>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var user = await _UserManager.FindByIdAsync(request.UserId.ToString());
            if (user == null) return NotFound<string>("User not found");

            if (request.RevokeAllTokens)
            {
                var countdevices = await _jwttokenService.RevokeAllUserTokensAsync(request.UserId);
                return Success($"Logged out from all devices{countdevices}");
            }

            var revoked = await _jwttokenService.RevokeRefreshTokenAsync(request.UserId, request.RefreshToken);
            return revoked ? Success("Logged out successfully") : BadRequest<string>("Invalid token");
        }
    }
}
