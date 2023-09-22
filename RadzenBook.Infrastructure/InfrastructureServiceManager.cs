using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using RadzenBook.Application.Common;
using RadzenBook.Application.Common.Auth;
using RadzenBook.Application.Common.Email;
using RadzenBook.Application.Common.Photo;
using RadzenBook.Application.Identity;
using RadzenBook.Application.Identity.Account;
using RadzenBook.Infrastructure.Auth;
using RadzenBook.Infrastructure.Identity;
using RadzenBook.Infrastructure.Identity.Account;
using RadzenBook.Infrastructure.Identity.Role;
using RadzenBook.Infrastructure.Identity.Token;
using RadzenBook.Infrastructure.Identity.User;
using RadzenBook.Infrastructure.Mail;
using RadzenBook.Infrastructure.Photo;

namespace RadzenBook.Infrastructure;

public class InfrastructureServiceManager : IInfrastructureServiceManager
{
    private readonly Lazy<IUserAccessor> _userAccessor;
    private readonly Lazy<IPhotoAccessor> _photoAccessor;
    private readonly Lazy<IEmailSender> _emailSender;
    private readonly Lazy<ITokenService> _tokenService;
    private readonly Lazy<IAccountService> _authService;

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
        _authService = new Lazy<IAccountService>(() => new AccountService(userManager, signInManager, TokenService, loggerFactory, t));
    }

    public ITokenService TokenService => _tokenService.Value;
    public IUserAccessor UserAccessor => _userAccessor.Value;
    public IPhotoAccessor PhotoAccessor => _photoAccessor.Value;
    public IEmailSender EmailSender => _emailSender.Value;
    public IAccountService AccountService => _authService.Value;
}