using RadzenBook.Application.Common.Auth;
using RadzenBook.Application.Common.Email;
using RadzenBook.Application.Common.Photo;
using RadzenBook.Application.Identity.Account;

namespace RadzenBook.Application.Common;

public interface IInfrastructureServiceManager
{
    IAccountService AccountService { get; }
    ITokenService TokenService { get; }
    IUserAccessor UserAccessor { get; }
    IPhotoAccessor PhotoAccessor { get; }
    IEmailSender EmailSender { get; }
}