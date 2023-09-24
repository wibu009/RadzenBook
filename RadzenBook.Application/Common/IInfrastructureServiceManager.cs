using RadzenBook.Application.Auth;
using RadzenBook.Application.Common.Auth;
using RadzenBook.Application.Common.Email;
using RadzenBook.Application.Common.Photo;

namespace RadzenBook.Application.Common;

public interface IInfrastructureServiceManager
{
    IAuthService AuthService { get; }
    ITokenService TokenService { get; }
    IUserAccessor UserAccessor { get; }
    IPhotoAccessor PhotoAccessor { get; }
    IEmailSender EmailSender { get; }
}