using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using RadzenBook.Entity;
using RadzenBook.Service.Implements.Infrastructure.Encrypt;
using RadzenBook.Service.Implements.Infrastructure.Photo;
using RadzenBook.Service.Implements.Infrastructure.Security;
using RadzenBook.Service.Interfaces.Infrastructure;
using RadzenBook.Service.Interfaces.Infrastructure.Encrypt;
using RadzenBook.Service.Interfaces.Infrastructure.Photo;
using RadzenBook.Service.Interfaces.Infrastructure.Security;

namespace RadzenBook.Service.Implements.Infrastructure;

public class InfrastructureServiceManager : IInfrastructureServiceManager
{
    private readonly Lazy<ITokenService> _tokenService;
    private readonly Lazy<IUserAccessor> _userAccessor;
    private readonly Lazy<IPhotoAccessor> _photoAccessor;

    public InfrastructureServiceManager(
        IConfiguration configuration, 
        IHttpContextAccessor httpContextAccessor, 
        UserManager<AppUser> userManager)
    {
        _tokenService = new Lazy<ITokenService>(() => new TokenService(configuration, userManager));
        _userAccessor = new Lazy<IUserAccessor>(() => new UserAccessor(httpContextAccessor));
        _photoAccessor = new Lazy<IPhotoAccessor>(() => new PhotoAccessor(configuration));
    }

    public ITokenService TokenService => _tokenService.Value;
    public IUserAccessor UserAccessor => _userAccessor.Value;
    public IPhotoAccessor PhotoAccessor => _photoAccessor.Value;
}