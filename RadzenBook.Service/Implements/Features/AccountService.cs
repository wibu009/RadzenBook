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
    private readonly ITokenService _tokenService;
    private readonly ILogger<AccountService> _logger;

    public AccountService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IInfrastructureServiceManager infrastructureServiceManager,
        ILogger<AccountService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
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

            var userAuthDto = new UserAuthDto
            {
                Username = user.UserName,
                Email = user.Email,
                Avatar = string.Empty,
                Token = await _tokenService.CreateTokenAsync(user)
            };

            _logger.LogInformation("User {UserUserName} logged in successfully", user.UserName);

            return Result<UserAuthDto>.Success(userAuthDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Message when login");
            throw ServiceException.Create(nameof(LoginAsync), nameof(AccountService), e.Message, e);
        }
    }
}