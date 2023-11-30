using RadzenBook.Client.Models;

namespace RadzenBook.Client.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(UserRegisterDTO req);
        Task<ServiceResponse<string>> Login(UserLoginDTO req);
        Task<ServiceResponse<bool>> ChangePassword(UserChangePasswordDTO req);
    }
}
