namespace ColorsApi.Models;

public class JwtAuthOptions
{
    public const string SectionName = "Jwt";
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public int ExpirationInMinutes { get; set; }
    public int RefreshTokenExpirationDays { get; set; }
}
