namespace Musync.Application.Models.Identity
{
    public sealed class JwtSettings {
        public string Key { get; set; } = String.Empty;
        public string Issuer { get; set; } = String.Empty;
        public string Audience { get; set; } = String.Empty;
        public double DurationInMinutes { get; set; } = 60;
        public double RefreshTokenDurationInDays { get; set; } = 30;
    };
}
