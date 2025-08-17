using ApplicationLayer.BankSystem.AbstractServices;
using Domainlayer.BankSystem.Entites;
using InfrastructureLayer.BankSystem.Configurations;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.BankSystem.ImplementServices
{
    public class JwtTokenService : IJwtTokenService

    {

        #region Fields
        private  JwtSetting _jwtSetting;


        #endregion

        #region Constructors
        public JwtTokenService(JwtSetting jwtSetting)
        {
            _jwtSetting = jwtSetting;
        }

        #endregion

        #region HandlerMethods

        public async Task<string> GenerateJWTToken(ApplicationUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Secret));
            var claims = new List<Claim>
               {
                new Claim("UserName", user.UserName),
                new Claim("Email", user.Email),
                new Claim("PhoneNumber", user.PhoneNumber),
               };
            var jwtToken = new JwtSecurityToken(
         issuer: _jwtSetting.Issuer,
           audience: _jwtSetting.Audience,
            claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return accessToken;
        }

        #endregion
    }
}
