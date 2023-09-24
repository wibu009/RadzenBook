namespace RadzenBook.Application.Auth;

public interface IAuthService
{
    Task<Result<UserAuthDto>> LoginAsync(LoginRequest loginRequest);
    Task<Result<UserAuthDto>> RegisterAsync(RegisterRequest registerRequest);
}