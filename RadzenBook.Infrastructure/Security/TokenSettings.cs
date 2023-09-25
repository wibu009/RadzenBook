namespace RadzenBook.Infrastructure.Security;

public class TokenSettings
{
    public string Key { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    // In minutes
    public int AccessTokenExpirationInMinutes { get; set; }
    // In days
    public int RefreshTokenExpirationInDays { get; set; }
}