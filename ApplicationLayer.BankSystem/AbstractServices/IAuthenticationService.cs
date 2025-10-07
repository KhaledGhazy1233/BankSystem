using Domainlayer.BankSystem.Entites;
using Domainlayer.BankSystem.Results;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.BankSystem.AbstractServices
{
    public interface IAuthenticationService
    {
        public  Task<JwtAuthResult> GetJWTToken(ApplicationUser user);
        public Task<(string, DateTime?)> ValidateTokenDetails(JwtSecurityToken jwtToken, string accessToken, string refreshToken);
        public Task<string> ValidateToken(string accesstoken);
        public JwtSecurityToken ReadJwtToken(string accessToken);
        public Task<JwtAuthResult> GenerateAccessTokenFromRefreshToken(ApplicationUser user, JwtSecurityToken jwttoken, DateTime? expiredate, string RefreshToken);
    }
}
