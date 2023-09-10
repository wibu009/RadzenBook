using Microsoft.Extensions.Configuration;
using RadzenBook.Service.Implements.Infrastructure.Encrypt;
using RadzenBook.Service.Interfaces.Infrastructure;
using RadzenBook.Service.Interfaces.Infrastructure.Encrypt;

namespace RadzenBook.Service.Implements.Infrastructure;

public class InfrastructureServiceManager : IInfrastructureServiceManager
{
    private readonly Lazy<ITokenService> _tokenService;

    public InfrastructureServiceManager(IConfiguration configuration)
    {
        _tokenService = new Lazy<ITokenService>(() => new TokenService(configuration));
    }

    public ITokenService TokenService => _tokenService.Value;
}