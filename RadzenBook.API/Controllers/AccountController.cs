using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RadzenBook.Contract.DTO.Auth;
using Swashbuckle.AspNetCore.Annotations;

namespace RadzenBook.API.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
public class AccountController : BaseApiController
{
    [HttpPost("login")]
    [SwaggerOperation(Summary = "Login")]
    [SwaggerResponse(StatusCodes.Status200OK, "Login", typeof(UserAuthDto))]
    public async Task<IActionResult> Login(LoginRequestDto loginRequestDto) 
        => HandleResult(await FeatureServiceManager.AccountService.LoginAsync(loginRequestDto));
    
    
    [HttpPost("register")]
    [SwaggerOperation(Summary = "Register")]
    [SwaggerResponse(StatusCodes.Status200OK, "Register", typeof(UserAuthDto))]
    public async Task<IActionResult> Register(RegisterRequestDto registerRequestDto) 
        => HandleResult(await FeatureServiceManager.AccountService.RegisterAsync(registerRequestDto));
}