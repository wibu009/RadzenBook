namespace RadzenBook.Application.Identity.Auth;

public interface IAuthService
{
    Task<Result<UserAuthDto>> LoginAsync(LoginRequest loginRequest);
    Task<Result<UserAuthDto>> RegisterAsync(RegisterRequest registerRequest);
    Task<Result<UserAuthDto>> RefreshTokenAsync();
}