using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
    private readonly RoleManager<AppRole> _roleManager;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AccountService> _logger;

    public AccountService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        RoleManager<AppRole> roleManager,
        IInfrastructureServiceManager infrastructureServiceManager,
        ILogger<AccountService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _tokenService = infrastructureServiceManager.TokenService;
        _logger = logger;
    }

    public async Task<Result<UserAuthDto>> LoginAsync(LoginRequestDto loginRequestDto)
    {
        try
        {
            var user = await _userManager.Users
                .SingleOrDefaultAsync(x => x.UserName == loginRequestDto.Username || x.Email == loginRequestDto.Username);

            if (user == null) return Result<UserAuthDto>.Failure("Invalid username or password", (int)HttpStatusCode.Unauthorized);

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequestDto.Password, false);

            if (!result.Succeeded) return Result<UserAuthDto>.Failure("Invalid username or password", (int)HttpStatusCode.Unauthorized);
            
            //check email confirmed
            if (!user.EmailConfirmed) return Result<UserAuthDto>.Failure("Email not confirmed", (int)HttpStatusCode.Unauthorized);

            var userAuthDto = CreateUserAuthDto(user);
            
            //add role is customer
            var role = await _roleManager.FindByNameAsync("customer");
            if (role != null)
            {
                await _userManager.AddToRoleAsync(user, role.Name);
            }

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
            var user = new AppUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Email,
                PhoneNumber = registerRequestDto.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, registerRequestDto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);
                return Result<UserAuthDto>.Failure(errors.ToString()!);
            }
            
            var userAuthDto = CreateUserAuthDto(user);

            _logger.LogInformation("User {UserUserName} registered successfully", user.UserName);

            return Result<UserAuthDto>.Success(userAuthDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Title when register");
            throw ServiceException.Create(nameof(RegisterAsync), nameof(AccountService), e.Message, e);
        }
    }
    
    private UserAuthDto CreateUserAuthDto(AppUser user)
    {
        return new UserAuthDto
        {
            Username = user.UserName,
            Email = user.Email,
            Avatar = string.Empty,
            Token = _tokenService.CreateTokenAsync(user).Result
        };
    }
}