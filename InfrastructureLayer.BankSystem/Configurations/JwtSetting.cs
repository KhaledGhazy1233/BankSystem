namespace InfrastructureLayer.BankSystem.Configurations
{
    public class JwtSetting
    {
        public string Secret { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public bool ValidateIssuer { get; set; } = true;
        public bool ValidateAudience { get; set; } = true;
        public bool ValidateLifeTime { get; set; } = true;
        public bool ValidateIssuerSigningKey { get; set; } = true;

        public int AccessTokenExpireMinutes { get; set; }

        public int RefreshTokenExpireDays { get; set; }
    }
}
