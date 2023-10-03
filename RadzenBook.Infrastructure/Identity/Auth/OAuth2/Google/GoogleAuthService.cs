using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Oauth2.v2;
using Google.Apis.Oauth2.v2.Data;
using Google.Apis.Services;

namespace RadzenBook.Infrastructure.Identity.Auth.OAuth2.Google;

public class GoogleAuthService
{
    private readonly GoogleSettings _googleSettings;
    private readonly string _redirectUri;

    public GoogleAuthService(IConfiguration config, IHttpContextAccessor httpContextAccessor)
    {
        _googleSettings = config.GetSection("AuthenticationSettings").Get<AuthenticationSettings>()!.GoogleSettings;
        _redirectUri = $"{httpContextAccessor.HttpContext!.Request.Scheme}://{httpContextAccessor.HttpContext!.Request.Host}{_googleSettings.CallBackPath}";
    }
    
    public string GetLoginLinkUrl(string? state = null)
    {
        var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets
            {
                ClientId = _googleSettings.ClientId,
                ClientSecret = _googleSettings.ClientSecret
            },
            Scopes = new[] { "email", "profile" },
        });

        var uri = flow.CreateAuthorizationCodeRequest(_redirectUri).Build().AbsoluteUri;

        return uri;
    }
    
    public async Task<Userinfo> GetUserFromCode(string code)
    {
        var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets
            {
                ClientId = _googleSettings.ClientId,
                ClientSecret = _googleSettings.ClientSecret
            },
            Scopes = new[] { "email", "profile" },
        });

        var token = await flow.ExchangeCodeForTokenAsync("", code, _redirectUri, CancellationToken.None);

        var credentials = new UserCredential(flow, "", token);

        var service = new Oauth2Service(new BaseClientService.Initializer
        {
            HttpClientInitializer = credentials,
            ApplicationName = "RadzenBook"
        });

        return await service.Userinfo.Get().ExecuteAsync();
    }
}