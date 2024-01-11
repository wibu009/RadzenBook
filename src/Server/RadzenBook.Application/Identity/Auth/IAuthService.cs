namespace RadzenBook.Application.Identity.Auth;

public interface IAuthService
{
    Task<Result<UserAuthDto>> LoginAsync(LoginRequest loginRequest);
    Task<Result<string>> ExternalLoginAsync(string provider);
    Task<Result<string>> ExternalLoginCallbackAsync(string provider, string code , string? error = "", string? state = "");
    Task<Result<UserAuthDto>> RegisterAsync(RegisterRequest registerRequest);
    Task<Result<UserAuthDto>> RefreshTokenAsync(string? signInCode = null);
}