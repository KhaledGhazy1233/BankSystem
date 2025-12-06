using ApplicationLayer.BankSystem.AbstractServices;
using Domainlayer.BankSystem.Entites;
using Domainlayer.BankSystem.Results;
using InfrastructureLayer.BankSystem.AbstractRepositories;
using InfrastructureLayer.BankSystem.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ApplicationLayer.BankSystem.ImplementServices
{
    public class AuthenticationService : IAuthenticationService

    {

        #region Fields
        private JwtSetting _jwtSetting;
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        #endregion

        #region Constructors
        public AuthenticationService(JwtSetting jwtSetting, UserManager<ApplicationUser> usermanager, IRefreshTokenRepository refreshTokenRepository)
        {
            _jwtSetting = jwtSetting;

            _usermanager = usermanager;
            _refreshTokenRepository = refreshTokenRepository;
        }

        #endregion

        #region HandlerMethods

        public async Task<JwtAuthResult> GetJWTToken(ApplicationUser user)
        {


            var refreshToken = GetRefreshToken(user.UserName);


            var (jwtToken, accessToken) = await GenerateJwtToken(user);

            var userRefreshToken = new UserRefreshToken
            {
                AddedTime = DateTime.Now,
                ExpiryDate = DateTime.Now.AddDays(-100),
                IsUsed = true,
                IsRevoked = false,
                JwtId = jwtToken.Id,
                RefreshToken = refreshToken.TokenString,
                Token = accessToken,
                UserId = user.Id
            };
            await _refreshTokenRepository.AddAsync(userRefreshToken);


            var ResultResponse = new JwtAuthResult
            {
                AccessToken = accessToken,
                refreshToken = refreshToken
            };

            return ResultResponse;
        }

        private async Task<(JwtSecurityToken, string)> GenerateJwtToken(ApplicationUser user)
        {
            var allclaims = await GetClaims(user);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Secret));
            var jwtToken = new JwtSecurityToken(
              issuer: _jwtSetting.Issuer,
              audience: _jwtSetting.Audience,
              claims: allclaims,
              expires: DateTime.Now.AddDays(_jwtSetting.AccessTokenExpireDate),
              signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            var refreshToken = GetRefreshToken(user.UserName);//flag-->

            return (jwtToken, accessToken);
        }




        private string GenerateRefershToken()
        {
            var randomNumber = new byte[32];
            var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);

        }

        private RefreshToken GetRefreshToken(string username)
        {

            return new RefreshToken
            {
                ExpireAt = DateTime.Now.AddDays(_jwtSetting.RefreshTokenExpireDate),
                TokenString = GenerateRefershToken(),//receive string
                UserName = username
            };


        }

        public async Task<List<Claim>> GetClaims(ApplicationUser user)
        {
            var roles = await _usermanager.GetRolesAsync(user);
            var allUserClaims = await _usermanager.GetClaimsAsync(user);
            var claims = new List<Claim>
             {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) ,
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            };
            claims.AddRange(allUserClaims);
            //new Claim(nameof(UserClaimModel.UserName), user.UserName),
            //new Claim(nameof(UserClaimModel.Email), user.Email),
            //new Claim(nameof(UserClaimModel.PhoneNumber), user.PhoneNumber),
            //new Claim(nameof(UserClaimModel.Role), user.PhoneNumber),


            return claims;
        }
        public async Task<JwtAuthResult> GenerateAccessTokenFromRefreshToken(ApplicationUser user, JwtSecurityToken jwttoken, DateTime? expiredate, string RefreshToken)
        {
            var (jwtSecurityToken, newaccesstoken) = await GenerateJwtToken(user);
            var response = new JwtAuthResult();
            response.AccessToken = newaccesstoken;

            var refreshtokenresult = new RefreshToken();
            refreshtokenresult.ExpireAt = (DateTime)expiredate;
            refreshtokenresult.TokenString = RefreshToken;
            refreshtokenresult.UserName = jwttoken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            response.refreshToken = refreshtokenresult;
            return response;
        }
        public JwtSecurityToken ReadJwtToken(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }
            var handler = new JwtSecurityTokenHandler();
            var response = handler.ReadJwtToken(accessToken);
            return response;
        }
        public async Task<string> ValidateToken(string accesstoken)
        {
            var handler = new JwtSecurityTokenHandler();
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = _jwtSetting.ValidateIssuer,
                ValidIssuers = new[] { _jwtSetting.Issuer },
                ValidateIssuerSigningKey = _jwtSetting.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSetting.Secret)),
                ValidAudience = _jwtSetting.Audience,
                ValidateAudience = _jwtSetting.ValidateAudience,
                ValidateLifetime = _jwtSetting.ValidateLifeTime,
            };

            try
            {
                var validator = handler.ValidateToken(accesstoken, parameters, out SecurityToken validatedToken);
                if (validator == null)
                {
                    return "InvalidToken";
                }
                return "NotExpired";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public async Task<(string, DateTime?)> ValidateTokenDetails(JwtSecurityToken jwtToken, string accessToken, string refreshToken)
        {
            if (jwtToken == null ||
                !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase))
            {
                return ("InvalidAlgorithm", null);
            }

            // AccessToken لسه شغال، مش محتاج تعمل Refresh
            if (jwtToken.ValidTo > DateTime.UtcNow)
            {
                return ("AccessTokenStillValid", jwtToken.ValidTo);
            }

            // استخراج UserId من الـ Claims
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ("UserIdNotFoundInToken", null);
            }

            // البحث عن RefreshToken في الداتا
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var parsedUserId))
            {
                return ("InvalidUserId", null);
            }

            var userRefreshToken = await _refreshTokenRepository.GetTableNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.Token == accessToken &&
                    x.RefreshToken == refreshToken &&
                    x.UserId == parsedUserId);

            if (userRefreshToken == null)
                return ("TokenNotFound", null);

            // التحقق من انتهاء صلاحية RefreshToken
            if (userRefreshToken.ExpiryDate < DateTime.UtcNow)
            {
                userRefreshToken.IsRevoked = true;
                userRefreshToken.IsUsed = false;
                await _refreshTokenRepository.UpdateAsync(userRefreshToken);
                return ("RefreshTokenExpired", null);
            }

            return (userId.ToString(), userRefreshToken.ExpiryDate);
        }

        #endregion

    }
}
