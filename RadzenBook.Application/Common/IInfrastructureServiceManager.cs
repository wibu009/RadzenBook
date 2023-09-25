using RadzenBook.Application.Common.Email;
using RadzenBook.Application.Common.Photo;
using RadzenBook.Application.Common.Security;
using RadzenBook.Application.Identity.Auth;
using RadzenBook.Application.Identity.Token;

namespace RadzenBook.Application.Common;

public interface IInfrastructureServiceManager
{
    IAuthService AuthService { get; }
    ITokenService TokenService { get; }
    IUserAccessor UserAccessor { get; }
    IPhotoAccessor PhotoAccessor { get; }
    IEmailSender EmailSender { get; }
}