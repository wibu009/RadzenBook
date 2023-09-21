using RadzenBook.Application.Common.Models;

namespace RadzenBook.Application.Identity.Account;

public interface IAccountService
{
    Task<Result<UserAuthDto>> LoginAsync(LoginRequest loginRequest);
    Task<Result<UserAuthDto>> RegisterAsync(RegisterRequest registerRequest);
}