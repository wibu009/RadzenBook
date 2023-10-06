namespace RadzenBook.Infrastructure.Identity.Auth;

public class AuthenticationSettings
{
    public JwtSettings JwtSettings { get; set; } = default!;
    public FacebookSettings FacebookSettings { get; set; } = default!;
    public GoogleSettings GoogleSettings { get; set; } = default!;
    public string DefaultClientAppUrl { get; set; } = default!;
}

public class JwtSettings
{
    public string Key { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string Authority { get; set; } = default!;
    public int AccessTokenExpirationInMinutes { get; set; }
    public int RefreshTokenExpirationInDays { get; set; }
}

public class FacebookSettings
{
    public string AppId { get; set; } = default!;
    public string AppSecret { get; set; } = default!;
    public string CallBackPath { get; set; } = default!;
}

public class GoogleSettings
{
    public string ClientId { get; set; } = default!;
    public string ClientSecret { get; set; } = default!;
    public string CallBackPath { get; set; } = default!;
}