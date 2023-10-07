using RadzenBook.Application.Identity.Auth;

namespace RadzenBook.Host.Controllers;

[AllowAnonymous]
[ApiVersion(ApiVersionName.V1)]
[ApiVersion(ApiVersionName.V2)]
public class AuthController : BaseApiController
{
    [HttpPost("login")]
    [SwaggerOperation(Summary = "Login")]
    [SwaggerResponse(StatusCodes.Status200OK, "Login successful", typeof(UserAuthDto))]
    public async Task<IActionResult> Login(LoginRequest loginRequest) 
        => HandleResult(await InfrastructureServiceManager.AuthService.LoginAsync(loginRequest));
    
    [HttpGet("external-login/{provider}")]
    [MapToApiVersion(ApiVersionName.V2)]
    [SwaggerOperation(Summary = "External login")]
    [SwaggerResponse(StatusCodes.Status200OK, "Redirect to external login provider")]
    public async Task<IActionResult> ExternalLogin(
        [FromRoute] string provider) 
        => HandleResult(await InfrastructureServiceManager.AuthService.ExternalLoginAsync(provider));
    
    [HttpGet("external-login-callback/{provider}")]
    [MapToApiVersion(ApiVersionName.V2)]
    [SwaggerOperation(Summary = "External login callback")]
    [SwaggerResponse(StatusCodes.Status200OK, "External login callback and redirect to client app")]
    public async Task<IActionResult> ExternalLoginCallback(
        [FromRoute] string provider, 
        [FromQuery] string code, 
        [FromQuery] string? error = "",
        [FromQuery] string? state = "")
        => HandleResult(await InfrastructureServiceManager.AuthService.ExternalLoginCallbackAsync(provider, code, error, state));
    
    [HttpPost("register")]
    [SwaggerOperation(Summary = "Register")]
    [SwaggerResponse(StatusCodes.Status200OK, "Register", typeof(UserAuthDto))]
    public async Task<IActionResult> Register(RegisterRequest registerRequest) 
        => HandleResult(await InfrastructureServiceManager.AuthService.RegisterAsync(registerRequest));
    
    [HttpPost("refresh-token")]
    [MapToApiVersion(ApiVersionName.V2)]
    [SwaggerOperation(Summary = "Refresh token")]
    [SwaggerResponse(StatusCodes.Status200OK, "Refresh token", typeof(UserAuthDto))]
    public async Task<IActionResult> RefreshToken(
        [FromQuery]
         string? signInCode = null)
        => HandleResult(await InfrastructureServiceManager.AuthService.RefreshTokenAsync(signInCode));
}