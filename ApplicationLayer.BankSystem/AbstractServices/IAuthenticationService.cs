using Domainlayer.BankSystem.Entites;
using Domainlayer.BankSystem.Enums;
using Domainlayer.BankSystem.Results;
using System.IdentityModel.Tokens.Jwt;

namespace ApplicationLayer.BankSystem.AbstractServices
{
    public interface IAuthenticationService
    {
        public Task<JwtAuthResult> GetJWTToken(ApplicationUser user);
        public Task<(TokenValidationStatus status, string UserId, DateTime? ExpireDate)> ValidateTokenDetails(JwtSecurityToken jwtToken, string accessToken, string refreshToken);
        public Task<string> ValidateToken(string accesstoken);
        public JwtSecurityToken ReadJwtToken(string accessToken);
        public Task<JwtAuthResult> GenerateAccessTokenFromRefreshToken(ApplicationUser user, JwtSecurityToken jwttoken, DateTime? expiredate, string RefreshToken);

        Task<bool> RevokeRefreshTokenAsync(int userId, string refreshToken);
        Task<int> RevokeAllUserTokensAsync(int userId);
    }
}
