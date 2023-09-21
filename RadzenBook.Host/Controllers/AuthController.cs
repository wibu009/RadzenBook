using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RadzenBook.Application.Identity;
using RadzenBook.Application.Identity.Account;
using Swashbuckle.AspNetCore.Annotations;

namespace RadzenBook.Host.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
public class AuthController : BaseApiController
{
    [HttpPost("login")]
    [SwaggerOperation(Summary = "Login")]
    [SwaggerResponse(StatusCodes.Status200OK, "Login", typeof(UserAuthDto))]
    public async Task<IActionResult> Login(LoginRequest loginRequest) 
        => HandleResult(await InfrastructureServiceManager.AccountService.LoginAsync(loginRequest));
    
    
    [HttpPost("register")]
    [SwaggerOperation(Summary = "Register")]
    [SwaggerResponse(StatusCodes.Status200OK, "Register", typeof(UserAuthDto))]
    public async Task<IActionResult> Register(RegisterRequest registerRequest) 
        => HandleResult(await InfrastructureServiceManager.AccountService.RegisterAsync(registerRequest));
}