namespace Domainlayer.BankSystem.Enums
{
    public enum TokenValidationStatus
    {
        Success,
        InvalidAlgorithm,
        AccessTokenStillValid,
        InvalidUserId,
        TokenNotFound,
        RefreshTokenExpired,
        RefreshTokenUsedOrRevoked,
        JwtIdMismatch
    }

}
