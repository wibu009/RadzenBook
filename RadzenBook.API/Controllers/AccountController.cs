using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RadzenBook.Contract.DTO.Auth;
using RadzenBook.Service.Interfaces.Features;
using Swashbuckle.AspNetCore.Annotations;

namespace RadzenBook.API.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
public class AccountController : BaseApiController
{
    private readonly IAccountService _accountService;

    public AccountController(IFeaturesServiceManager featuresServiceManager)
    {
        _accountService = featuresServiceManager.AccountService;
    }

    [HttpPost("login")]
    [SwaggerOperation(Summary = "Login")]
    [SwaggerResponse(StatusCodes.Status200OK, "Login", typeof(UserAuthDto))]
    public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
    {
        return HandleResult(await _accountService.LoginAsync(loginRequestDto));
    }
    
    [HttpPost("register")]
    [SwaggerOperation(Summary = "Register")]
    [SwaggerResponse(StatusCodes.Status200OK, "Register", typeof(UserAuthDto))]
    public async Task<IActionResult> Register(RegisterRequestDto registerRequestDto)
    {
        return HandleResult(await _accountService.RegisterAsync(registerRequestDto));
    }
}