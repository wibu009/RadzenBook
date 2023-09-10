using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RadzenBook.Service.Implements.Infrastructure.Encrypt;
using RadzenBook.Service.Implements.Infrastructure.Security;
using RadzenBook.Service.Interfaces.Infrastructure;
using RadzenBook.Service.Interfaces.Infrastructure.Encrypt;
using RadzenBook.Service.Interfaces.Infrastructure.Security;

namespace RadzenBook.Service.Implements.Infrastructure;

public class InfrastructureServiceManager : IInfrastructureServiceManager
{
    private readonly Lazy<ITokenService> _tokenService;
    private readonly Lazy<IUserAccessor> _userAccessor;

    public InfrastructureServiceManager(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _tokenService = new Lazy<ITokenService>(() => new TokenService(configuration));
        _userAccessor = new Lazy<IUserAccessor>(() => new UserAccessor(httpContextAccessor));
    }

    public ITokenService TokenService => _tokenService.Value;
    public IUserAccessor UserAccessor => _userAccessor.Value;
}