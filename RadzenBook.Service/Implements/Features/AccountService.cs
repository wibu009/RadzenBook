using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using RadzenBook.Common.Exceptions;
using RadzenBook.Contract.Core;
using RadzenBook.Contract.DTO.Auth;
using RadzenBook.Entity;
using RadzenBook.Service.Interfaces.Features;
using RadzenBook.Service.Interfaces.Infrastructure;
using RadzenBook.Service.Interfaces.Infrastructure.Encrypt;

namespace RadzenBook.Service.Implements.Features;

public class AccountService : IAccountService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AccountService> _logger;
    private readonly IStringLocalizer _t;

    public AccountService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IInfrastructureServiceManager infrastructureServiceManager,
        ILoggerFactory loggerFactory,
        IStringLocalizerFactory t
        )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = infrastructureServiceManager.TokenService;
        _logger = loggerFactory.CreateLogger<AccountService>();
        _t = t.Create(typeof(AccountService));
    }

    public async Task<Result<UserAuthDto>> LoginAsync(LoginRequestDto loginRequestDto)
    {
        try
        {
            var user = await _userManager.Users
                .SingleOrDefaultAsync(x => x.UserName == loginRequestDto.Username || x.Email == loginRequestDto.Username);

            if (user == null) return Result<UserAuthDto>.Failure(_t["Incorrect username or password"], (int)HttpStatusCode.Unauthorized);

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequestDto.Password, false);
            
            if (!result.Succeeded) return Result<UserAuthDto>.Failure(_t["Incorrect username or password"], (int)HttpStatusCode.Unauthorized);
            
            // if (!user.EmailConfirmed) return Result<UserAuthDto>.Failure("Email not confirmed", (int)HttpStatusCode.Unauthorized);

            var userAuthDto = await CreateUserAuthDto(user);

            _logger.LogInformation("User {UserUserName} logged in successfully", user.UserName);

            return Result<UserAuthDto>.Success(userAuthDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Title when login");
            throw ServiceException.Create(nameof(LoginAsync), nameof(AccountService), e.Message, e);
        }
    }
    
    public async Task<Result<UserAuthDto>> RegisterAsync(RegisterRequestDto registerRequestDto)
    {
        try
        {
            //check username exist
            var userExist = await _userManager.FindByNameAsync(registerRequestDto.Username);
            if (userExist != null) return Result<UserAuthDto>.Failure(_t["Username already exists"], (int)HttpStatusCode.BadRequest);
            
            var user = new AppUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Email,
                PhoneNumber = registerRequestDto.PhoneNumber
            };

            await _userManager.CreateAsync(user, registerRequestDto.Password);
            
            await _userManager.AddToRoleAsync(user, "customer");
            
            var userAuthDto = await CreateUserAuthDto(user);

            _logger.LogInformation("User {UserUserName} registered successfully", user.UserName);

            return Result<UserAuthDto>.Success(userAuthDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Title when register");
            throw ServiceException.Create(nameof(RegisterAsync), nameof(AccountService), e.Message, e);
        }
    }
    
    private async Task<UserAuthDto> CreateUserAuthDto(AppUser user)
    {
        var userAuthDto = new UserAuthDto
        {
            Username = user.UserName,
            Email = user.Email,
            Token = await _tokenService.CreateTokenAsync(user),
        };

        return userAuthDto;
    }
}