using Facebook;

namespace RadzenBook.Infrastructure.Identity.Auth.OAuth2.Facebook;

public class FacebookAuthService
{
    private readonly FacebookSettings _facebookSettings;
    private readonly string _redirectUri;
    
    public FacebookAuthService(IConfiguration config, IHttpContextAccessor httpContextAccessor)
    {
        _facebookSettings = config.GetSection("AuthenticationSettings").Get<AuthenticationSettings>()!.FacebookSettings;
        _redirectUri = $"{httpContextAccessor.HttpContext!.Request.Scheme}://{httpContextAccessor.HttpContext!.Request.Host}{_facebookSettings.CallBackPath}";
    }
    
    public string GetLoginLinkUrl(string? state = null)
    {
        var fb = new FacebookClient();
        
        var loginUrl = fb.GetLoginUrl(new
        {
            client_id = _facebookSettings.AppId,
            client_secret = _facebookSettings.AppSecret,
            redirect_uri = _redirectUri,
            response_type = "code",
            scope = "email",
            state = state
        });
        
        return loginUrl.AbsoluteUri;
    }
    
    public async Task<Userinfo> GetUserFromCode(string code)
    {
        var fb = new FacebookClient();
        
        dynamic result = fb.Post("oauth/access_token", new
        {
            client_id = _facebookSettings.AppId,
            client_secret = _facebookSettings.AppSecret,
            redirect_uri = _redirectUri,
            code
        });
        
        var accessToken = result.access_token;
        
        fb.AccessToken = accessToken;
        
        dynamic me = await fb.GetTaskAsync("me", new { fields = "id,name,email,picture.width(100).height(100)" });
        
        var userinfo = new Userinfo
        {
            Id = me.id,
            Name = me.name,
            Email = string.IsNullOrEmpty(me.email) ? $"{me.id + "@facebook.com"}" : me.email,
            Picture = me.picture.data.url
        };
        
        return userinfo;
    }
}