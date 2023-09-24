using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using RadzenBook.Application.Auth;
using RadzenBook.Application.Common.Auth;
using RadzenBook.Application.Common.Exceptions;
using RadzenBook.Application.Common.Models;
using RadzenBook.Infrastructure.Identity.User;

namespace RadzenBook.Infrastructure.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthService> _logger;
    private readonly IStringLocalizer _t;

    public AuthService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ITokenService tokenService,
        ILoggerFactory loggerFactory,
        IStringLocalizerFactory t
        )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _logger = loggerFactory.CreateLogger<AuthService>();
        _t = t.Create(typeof(AuthService));
    }

    public async Task<Result<UserAuthDto>> LoginAsync(LoginRequest loginRequest)
    {
        try
        {
            var user = await _userManager.Users
                .SingleOrDefaultAsync(x => x.UserName == loginRequest.Username || x.Email == loginRequest.Username);

            if (user == null) return Result<UserAuthDto>.Failure(_t["Incorrect username or password"], (int)HttpStatusCode.Unauthorized);

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, false);
            
            if (!result.Succeeded) return Result<UserAuthDto>.Failure(_t["Incorrect username or password"], (int)HttpStatusCode.Unauthorized);
            
            // if (!user.EmailConfirmed) return Result<UserAuthDto>.Failure("Email not confirmed", (int)HttpStatusCode.Unauthorized);

            var userAuthDto = await CreateUserAuthDto(user);

            _logger.LogInformation("User {UserUserName} logged in successfully", user.UserName);

            return Result<UserAuthDto>.Success(userAuthDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw ServiceException.Create(nameof(LoginAsync), nameof(AuthService), e.Message, e);
        }
    }
    
    public async Task<Result<UserAuthDto>> RegisterAsync(RegisterRequest registerRequest)
    {
        try
        {
            //check username exist
            var userExist = await _userManager.FindByNameAsync(registerRequest.Username);
            if (userExist != null) return Result<UserAuthDto>.Failure(_t["Username already exists"], (int)HttpStatusCode.BadRequest);
            
            var user = new AppUser
            {
                UserName = registerRequest.Username,
                Email = registerRequest.Email,
                PhoneNumber = registerRequest.PhoneNumber
            };

            await _userManager.CreateAsync(user, registerRequest.Password);
            
            await _userManager.AddToRoleAsync(user, "customer");
            
            var userAuthDto = await CreateUserAuthDto(user);

            _logger.LogInformation("User {UserName} registered successfully", user.UserName);

            return Result<UserAuthDto>.Success(userAuthDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw ServiceException.Create(nameof(RegisterAsync), nameof(AuthService), e.Message, e);
        }
    }
    
    private async Task<UserAuthDto> CreateUserAuthDto(AppUser user)
    {
        var userAuthDto = new UserAuthDto
        {
            Username = user.UserName,
            Email = user.Email,
            Token = await _tokenService.CreateTokenAsync(user.Id),
        };

        return userAuthDto;
    }
}