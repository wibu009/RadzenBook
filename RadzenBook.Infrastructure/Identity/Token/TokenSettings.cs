namespace RadzenBook.Infrastructure.Identity.Token;

public class TokenSettings
{
    public string Key { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public int AccessTokenExpirationInMinutes { get; set; }
    public int RefreshTokenExpirationInDays { get; set; }
}