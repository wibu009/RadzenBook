using RadzenBook.Application;
using RadzenBook.Application.Common;
using RadzenBook.Application.Common.Email;
using RadzenBook.Application.Common.Photo;
using RadzenBook.Application.Common.Security;
using RadzenBook.Application.Identity.Auth;
using RadzenBook.Application.Identity.Token;
using RadzenBook.Infrastructure.Identity.Auth;
using RadzenBook.Infrastructure.Identity.Token;
using RadzenBook.Infrastructure.Identity.User;
using RadzenBook.Infrastructure.Mail;
using RadzenBook.Infrastructure.Photo;
using RadzenBook.Infrastructure.Security;

namespace RadzenBook.Infrastructure;

public class InfrastructureServiceManager : IInfrastructureServiceManager
{
    private readonly Lazy<IUserAccessor> _userAccessor;
    private readonly Lazy<IPhotoAccessor> _photoAccessor;
    private readonly Lazy<IEmailSender> _emailSender;
    private readonly Lazy<ITokenService> _tokenService;
    private readonly Lazy<IAuthService> _authService;

    public InfrastructureServiceManager(
        IConfiguration configuration, 
        ILoggerFactory loggerFactory,
        IStringLocalizerFactory t,
        IHttpContextAccessor httpContextAccessor,
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager
        )
    {
        _tokenService = new Lazy<ITokenService>(() => new TokenService(configuration, userManager));
        _userAccessor = new Lazy<IUserAccessor>(() => new UserAccessor(httpContextAccessor));
        _photoAccessor = new Lazy<IPhotoAccessor>(() => new PhotoAccessor(configuration));
        _emailSender = new Lazy<IEmailSender>(() => new EmailSender(configuration));
        _authService = new Lazy<IAuthService>(() => new AuthService(userManager, signInManager, TokenService, loggerFactory, t, configuration, httpContextAccessor));
    }

    public ITokenService TokenService => _tokenService.Value;
    public IUserAccessor UserAccessor => _userAccessor.Value;
    public IPhotoAccessor PhotoAccessor => _photoAccessor.Value;
    public IEmailSender EmailSender => _emailSender.Value;
    public IAuthService AuthService => _authService.Value;
}