namespace RadzenBook.Application.Identity.Auth;

public interface IAuthService
{
    Task<Result<UserAuthDto>> LoginAsync(LoginRequest loginRequest);
    Task<Result<string>> ExternalLoginAsync(string provider);
    Task<Result<UserAuthDto>> ExternalLoginCallbackAsync(string provider, string code , string? remoteError = null);
    Task<Result<UserAuthDto>> RegisterAsync(RegisterRequest registerRequest);
    Task<Result<UserAuthDto>> RefreshTokenAsync();
}