using RadzenBook.Application.Identity.Auth;

namespace RadzenBook.Host.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
public class AuthController : BaseApiController
{
    [HttpPost("login")]
    [SwaggerOperation(Summary = "Login")]
    [SwaggerResponse(StatusCodes.Status200OK, "Login", typeof(UserAuthDto))]
    public async Task<IActionResult> Login(LoginRequest loginRequest) 
        => HandleResult(await InfrastructureServiceManager.AuthService.LoginAsync(loginRequest));
    
    
    [HttpPost("register")]
    [SwaggerOperation(Summary = "Register")]
    [SwaggerResponse(StatusCodes.Status200OK, "Register", typeof(UserAuthDto))]
    public async Task<IActionResult> Register(RegisterRequest registerRequest) 
        => HandleResult(await InfrastructureServiceManager.AuthService.RegisterAsync(registerRequest));
    
    [HttpPost("refresh-token")]
    [SwaggerOperation(Summary = "Refresh token")]
    [SwaggerResponse(StatusCodes.Status200OK, "Refresh token", typeof(UserAuthDto))]
    public async Task<IActionResult> RefreshToken() 
        => HandleResult(await InfrastructureServiceManager.AuthService.RefreshTokenAsync());
}