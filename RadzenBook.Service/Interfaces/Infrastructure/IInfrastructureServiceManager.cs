using RadzenBook.Service.Interfaces.Infrastructure.Encrypt;

namespace RadzenBook.Service.Interfaces.Infrastructure;

public interface IInfrastructureServiceManager
{
    ITokenService TokenService { get; }
}