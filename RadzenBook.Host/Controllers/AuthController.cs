using RadzenBook.Application.Auth;

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
}