using RadzenBook.Application.Common.Cache;
using RadzenBook.Application.Common.Email;
using RadzenBook.Application.Common.Photo;
using RadzenBook.Application.Common.Security;
using RadzenBook.Application.Identity.Auth;
using RadzenBook.Application.Identity.Token;
using RadzenBook.Infrastructure.Cache.LocalCache;
using RadzenBook.Infrastructure.Identity.Auth;
using RadzenBook.Infrastructure.Identity.Token;
using RadzenBook.Infrastructure.Identity.User;
using RadzenBook.Infrastructure.Mail;
using RadzenBook.Infrastructure.Mail.SendGrid;
using RadzenBook.Infrastructure.Photo;
using RadzenBook.Infrastructure.Security;

namespace RadzenBook.Infrastructure;

public class InfrastructureServiceManager : IInfrastructureServiceManager
{
    private readonly Lazy<IUserAccessor> _userAccessor;
    private readonly Lazy<IPhotoAccessor> _photoAccessor;
    private readonly Lazy<IEmailSender> _sendGridEmailSender;
    private readonly Lazy<ITokenService> _tokenService;
    private readonly Lazy<IAuthService> _authService;
    private readonly Lazy<ICacheService> _localCacheService;

    public InfrastructureServiceManager(
        IConfiguration configuration,
        ILoggerFactory loggerFactory,
        IStringLocalizerFactory t,
        IMemoryCache memoryCache,
        IHttpContextAccessor httpContextAccessor,
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager
    )
    {
        _tokenService = new Lazy<ITokenService>(() => new TokenService(configuration, userManager));
        _userAccessor = new Lazy<IUserAccessor>(() => new UserAccessor(httpContextAccessor));
        _photoAccessor = new Lazy<IPhotoAccessor>(() => new PhotoAccessor(configuration));
        _sendGridEmailSender = new Lazy<IEmailSender>(() => new SendGridEmailSender(configuration));
        _authService = new Lazy<IAuthService>(() => new AuthService(userManager, signInManager, TokenService,
            loggerFactory, t, configuration, httpContextAccessor, LocalCacheService));
        _localCacheService = new Lazy<ICacheService>(() => new LocalCacheService(memoryCache, loggerFactory));
    }

    public ITokenService TokenService => _tokenService.Value;
    public IUserAccessor UserAccessor => _userAccessor.Value;
    public IPhotoAccessor PhotoAccessor => _photoAccessor.Value;
    public IEmailSender SendGridEmailSender => _sendGridEmailSender.Value;
    public IAuthService AuthService => _authService.Value;
    public ICacheService LocalCacheService => _localCacheService.Value;
}