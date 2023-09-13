using RadzenBook.Service.Interfaces.Infrastructure.Encrypt;
using RadzenBook.Service.Interfaces.Infrastructure.Photo;
using RadzenBook.Service.Interfaces.Infrastructure.Security;

namespace RadzenBook.Service.Interfaces.Infrastructure;

public interface IInfrastructureServiceManager
{
    ITokenService TokenService { get; }
    IUserAccessor UserAccessor { get; }
    IPhotoAccessor PhotoAccessor { get; }
}