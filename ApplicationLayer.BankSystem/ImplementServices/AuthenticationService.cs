using ApplicationLayer.BankSystem.AbstractServices;
using Domainlayer.BankSystem.Entites;
using Domainlayer.BankSystem.Enums;
using Domainlayer.BankSystem.Results;
using InfrastructureLayer.BankSystem.AbstractRepositories;
using InfrastructureLayer.BankSystem.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace ApplicationLayer.BankSystem.ImplementServices
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly JwtSetting _jwtSetting;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(
            JwtSetting jwtSetting,
            UserManager<ApplicationUser> userManager,
            IRefreshTokenRepository refreshTokenRepository,
            ILogger<AuthenticationService> logger)
        {
            _jwtSetting = jwtSetting;
            _userManager = userManager;
            _refreshTokenRepository = refreshTokenRepository;
            _logger = logger;
        }

        // ------------------------- JWT + Refresh Token -------------------------

        public async Task<JwtAuthResult> GetJWTToken(ApplicationUser user)
        {
            var (jwtToken, accessToken) = await GenerateJwtToken(user);
            var (refreshPlain, expiresAt) = GenerateRefreshTokenPlain();
            var refreshHash = ComputeSha256Hash(refreshPlain);

            var userRefreshToken = new UserRefreshToken
            {
                AddedTime = DateTime.UtcNow,
                ExpiryDate = expiresAt,
                IsUsed = false,
                IsRevoked = false,
                JwtId = jwtToken.Id, // ✅ JWT ID for binding
                RefreshTokenHash = refreshHash, // ✅ Only store hash
                UserId = user.Id
                // ✅ NO Token = accessToken (security best practice)
            };

            try
            {
                await _refreshTokenRepository.AddAsync(userRefreshToken);
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var inner = ex.InnerException?.Message;
                // الـ inner ده هو اللي فيه السر (Conflict, Null, or Type mismatch)
            }
            return new JwtAuthResult
            {
                AccessToken = accessToken,
                refreshToken = new RefreshToken
                {
                    TokenString = refreshPlain, // ✅ Return plain to client
                    ExpireAt = expiresAt,
                    UserName = user.UserName
                }
            };
        }


        // ----------- Generate New Access Token Using Refresh Token -----------

        public async Task<JwtAuthResult> GenerateAccessTokenFromRefreshToken(
            ApplicationUser user,
            JwtSecurityToken oldJwtToken,
            DateTime? expireDate,
            string refreshTokenPlain)
        {
            // ✅ Step 1: Hash incoming refresh token
            var refreshHash = ComputeSha256Hash(refreshTokenPlain);

            // ✅ Step 2: Find stored entry by hash + user (with tracking for update)
            var stored = await _refreshTokenRepository.GetTableAsTracking()
                .FirstOrDefaultAsync(x => x.RefreshTokenHash == refreshHash && x.UserId == user.Id);

            if (stored == null)
            {
                _logger.LogWarning("Refresh token not found for user {UserId}", user.Id);
                throw new SecurityTokenException("Refresh token not found");
            }

            // ✅ Step 3: Validate not used/revoked
            if (stored.IsUsed || stored.IsRevoked)
            {
                await RevokeAndLog(stored, "reused_or_revoked_attempt");
                throw new SecurityTokenException("Refresh token already used or revoked");
            }

            // ✅ Step 4: Validate not expired
            if (stored.ExpiryDate < DateTime.UtcNow)
            {
                stored.IsRevoked = true;
                stored.IsUsed = true;
                await _refreshTokenRepository.UpdateAsync(stored);
                await _refreshTokenRepository.SaveChangesAsync();
                throw new SecurityTokenExpiredException("Refresh token expired");
            }

            // ✅ Step 5: CRITICAL - Validate JTI binding (prevents token substitution)
            if (!string.Equals(stored.JwtId, oldJwtToken.Id, StringComparison.Ordinal))
            {
                await RevokeAndLog(stored, "jti_mismatch");
                throw new SecurityTokenException("Refresh token does not match access token JTI");
            }

            // ✅ Step 6: Rotate - mark old as used+revoked
            stored.IsUsed = true;
            stored.IsRevoked = true;
            await _refreshTokenRepository.UpdateAsync(stored);

            // ✅ Step 7: Issue NEW access + refresh tokens
            var (newJwt, newAccess) = await GenerateJwtToken(user);
            var (newRefreshPlain, newExpiresAt) = GenerateRefreshTokenPlain();
            var newHash = ComputeSha256Hash(newRefreshPlain);

            var newEntity = new UserRefreshToken
            {
                AddedTime = DateTime.UtcNow,
                ExpiryDate = newExpiresAt,
                IsUsed = false,
                IsRevoked = false,
                JwtId = newJwt.Id,
                RefreshTokenHash = newHash,
                UserId = user.Id
            };

            await _refreshTokenRepository.AddAsync(newEntity);
            await _refreshTokenRepository.SaveChangesAsync();

            return new JwtAuthResult
            {
                AccessToken = newAccess,
                refreshToken = new RefreshToken
                {
                    TokenString = newRefreshPlain,
                    ExpireAt = newExpiresAt,
                    UserName = user.UserName
                }
            };
        }


        // ------------------------- Read JWT Without Validation -------------------------

        public JwtSecurityToken ReadJwtToken(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                throw new ArgumentNullException(nameof(accessToken));

            return new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
        }


        // ------------------------- Validate Token -------------------------

        public async Task<string> ValidateToken(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = _jwtSetting.ValidateIssuer,
                ValidIssuers = new[] { _jwtSetting.Issuer },
                ValidateIssuerSigningKey = _jwtSetting.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Secret)),
                ValidAudience = _jwtSetting.Audience,
                ValidateAudience = _jwtSetting.ValidateAudience,
                ValidateLifetime = _jwtSetting.ValidateLifeTime,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var validator = handler.ValidateToken(accessToken, parameters, out SecurityToken validatedToken);
                return validator == null ? "InvalidToken" : "NotExpired";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        // -------------------- Validate Token Details (Used in Handler) --------------------

        //public async Task<(string, DateTime?)> ValidateTokenDetails(
        //    JwtSecurityToken jwtToken,
        //    string accessToken,
        //    string refreshTokenPlain)
        //{
        //    // Check 1: Validate Algorithm
        //    if (jwtToken == null ||
        //        !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase))
        //    {
        //        return ("InvalidAlgorithm", null);
        //    }

        //    // Check 2: Ensure Access Token is expired
        //    if (jwtToken.ValidTo > DateTime.UtcNow)
        //    {
        //        return ("AccessTokenStillValid", jwtToken.ValidTo);
        //    }

        //    // Check 3: Extract and validate UserId
        //    var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        //    if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var parsedUserId))
        //    {
        //        return ("InvalidUserId", null);
        //    }

        //    // ✅ Check 4: Hash and find refresh token (NO need to compare accessToken)
        //    var refreshTokenHash = ComputeSha256Hash(refreshTokenPlain);

        //    var userRefreshToken = await _refreshTokenRepository.GetTableNoTracking()
        //        .FirstOrDefaultAsync(x =>
        //            x.RefreshTokenHash == refreshTokenHash &&
        //            x.UserId == parsedUserId);

        //    if (userRefreshToken == null)
        //        return ("TokenNotFound", null);

        //    // Check 5: Ensure token hasn't been used or revoked
        //    if (userRefreshToken.IsUsed || userRefreshToken.IsRevoked)
        //        return ("RefreshTokenUsedOrRevoked", null);

        //    // Check 6: Ensure refresh token hasn't expired
        //    if (userRefreshToken.ExpiryDate < DateTime.UtcNow)
        //        return ("RefreshTokenExpired", null);

        //    // ✅ Check 7: Validate JTI binding
        //    if (!string.Equals(userRefreshToken.JwtId, jwtToken.Id, StringComparison.Ordinal))
        //        return ("JwtIdMismatch", null);

        //    return (userId, userRefreshToken.ExpiryDate);
        //}


        public async Task<(TokenValidationStatus status, string UserId, DateTime? ExpireDate)> ValidateTokenDetails(
            JwtSecurityToken jwtToken,
            string accessToken,
            string refreshTokenPlain)
        {
            // Check 1: Validate Algorithm
            if (jwtToken == null ||
                !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase))
            {
                return (TokenValidationStatus.InvalidAlgorithm, string.Empty, null);
            }

            // Check 2: Ensure Access Token is expired
            if (jwtToken.ValidTo > DateTime.UtcNow)
            {
                return (TokenValidationStatus.AccessTokenStillValid, string.Empty, jwtToken.ValidTo);
            }

            // Check 3: Extract and validate UserId
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var parsedUserId))
            {
                return (TokenValidationStatus.InvalidUserId, string.Empty, null);
            }

            // Check 4: Hash and find refresh token
            var refreshTokenHash = ComputeSha256Hash(refreshTokenPlain);

            var userRefreshToken = await _refreshTokenRepository.GetTableNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.RefreshTokenHash == refreshTokenHash &&
                    x.UserId == parsedUserId);

            if (userRefreshToken == null)
                return (TokenValidationStatus.TokenNotFound, string.Empty, null);

            // Check 5: Ensure token hasn't been used or revoked
            if (userRefreshToken.IsUsed || userRefreshToken.IsRevoked)
                return (TokenValidationStatus.RefreshTokenUsedOrRevoked, string.Empty, null);

            // Check 6: Ensure refresh token hasn't expired
            if (userRefreshToken.ExpiryDate < DateTime.UtcNow)
                return (TokenValidationStatus.RefreshTokenExpired, string.Empty, null);

            // Check 7: Validate JTI binding
            if (!string.Equals(userRefreshToken.JwtId, jwtToken.Id, StringComparison.Ordinal))
                return (TokenValidationStatus.JwtIdMismatch, string.Empty, null);

            return (TokenValidationStatus.Success, userId, userRefreshToken.ExpiryDate);
        }

        // ------------------------- Generate JWT -------------------------

        private async Task<(JwtSecurityToken, string)> GenerateJwtToken(ApplicationUser user)
        {
            var claims = await GetClaims(user);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // ✅ Generate unique JTI
            var jti = Guid.NewGuid().ToString();
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, jti));

            var jwtToken = new JwtSecurityToken(
                issuer: _jwtSetting.Issuer,
                audience: _jwtSetting.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSetting.AccessTokenExpireMinutes), // ✅ Minutes, not days!
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return (jwtToken, accessToken);
        }


        // ------------------------- Claims Builder -------------------------

        private async Task<List<Claim>> GetClaims(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var userClaims = await _userManager.GetClaimsAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
                // ✅ JTI added in GenerateJwtToken
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // وإذا كان فيه Claims تانية مخزنة للمستخدم ضيفها برضه
            claims.AddRange(userClaims);

            return claims;
        }


        // ------------------------- Refresh Token Generator -------------------------

        private (string TokenString, DateTime ExpiresAt) GenerateRefreshTokenPlain()
        {
            var randomNumber = new byte[64]; // ✅ Large entropy
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            var token = Convert.ToBase64String(randomNumber);
            var expires = DateTime.UtcNow.AddDays(_jwtSetting.RefreshTokenExpireDays);

            return (token, expires);
        }


        // ------------------------- Hashing Helper -------------------------

        private static string ComputeSha256Hash(string raw)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(raw);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }


        // ------------------------- Revoke & Log -------------------------

        private async Task RevokeAndLog(UserRefreshToken token, string reason)
        {
            token.IsRevoked = true;
            token.IsUsed = true;
            await _refreshTokenRepository.UpdateAsync(token);
            await _refreshTokenRepository.SaveChangesAsync();

            _logger.LogWarning(
                "Revoked refresh token Id={Id} for user {UserId}. Reason: {Reason}",
                token.Id, token.UserId, reason);
        }

        public async Task<bool> RevokeRefreshTokenAsync(int userId, string refreshToken)
        {
            var hash = ComputeSha256Hash(refreshToken);
            var token = await _refreshTokenRepository.GetTableAsTracking()
                .FirstOrDefaultAsync(x => x.RefreshTokenHash == hash && x.UserId == userId);

            if (token == null || token.IsRevoked) return false;

            token.IsRevoked = true;
            token.IsUsed = true;
            await _refreshTokenRepository.SaveChangesAsync();
            return true;
        }

        public async Task<int> RevokeAllUserTokensAsync(int userId)
        {
            var tokens = await _refreshTokenRepository.GetTableAsTracking()
                .Where(x => x.UserId == userId && !x.IsRevoked)
                .ToListAsync();

            foreach (var t in tokens) t.IsRevoked = true;

            await _refreshTokenRepository.SaveChangesAsync();
            return tokens.Count;
        }
    }
}