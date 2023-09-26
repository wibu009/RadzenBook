using System.Net;
using RadzenBook.Application.Common.Exceptions;
using RadzenBook.Application.Common.Models;
using RadzenBook.Application.Identity.Auth;
using RadzenBook.Application.Identity.Token;
using RadzenBook.Infrastructure.Identity.Token;
using RadzenBook.Infrastructure.Identity.User;
using RadzenBook.Infrastructure.Security;

namespace RadzenBook.Infrastructure.Identity.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthService> _logger;
    private readonly IStringLocalizer _t;
    private readonly TokenSettings _tokenSettings;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ITokenService tokenService,
        ILoggerFactory loggerFactory,
        IStringLocalizerFactory t,
        IConfiguration config,
        IHttpContextAccessor httpContextAccessor
        )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _logger = loggerFactory.CreateLogger<AuthService>();
        _t = t.Create(typeof(AuthService));
        _tokenSettings = config.GetSection("TokenSettings").Get<TokenSettings>()!;
        _httpContextAccessor = httpContextAccessor;
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

            var userAuthDto = CreateUserAuthDto(user);
            
            await SetRefreshTokenAsync(user);

            _logger.LogInformation("User {UserUserName} logged in successfully", user.UserName);

            return Result<UserAuthDto>.Success(userAuthDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(LoginAsync), nameof(AuthService), e.Message, e);
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
            
            var userAuthDto = CreateUserAuthDto(user);

            _logger.LogInformation("User {UserName} registered successfully", user.UserName);

            return Result<UserAuthDto>.Success(userAuthDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(RegisterAsync), nameof(AuthService), e.Message, e);
        }
    }
    
    public async Task<Result<UserAuthDto>> RefreshTokenAsync()
    {
        try
        {
            var refreshToken = _httpContextAccessor.HttpContext!.Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken)) return Result<UserAuthDto>.Failure(_t["Invalid refresh token"], (int)HttpStatusCode.Unauthorized);
            
            var user = await _userManager.Users.Include(x => x.RefreshTokens).SingleOrDefaultAsync(x => x.RefreshTokens.Any(t => t.Token == refreshToken));
            if (user == null) return Result<UserAuthDto>.Failure(_t["Invalid refresh token"], (int)HttpStatusCode.Unauthorized);
            
            var oldRefreshToken = user.RefreshTokens.Single(x => x.Token == refreshToken);
            if (!oldRefreshToken.IsActive) return Result<UserAuthDto>.Failure(_t["Invalid refresh token"], (int)HttpStatusCode.Unauthorized);
            oldRefreshToken.Revoked = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            
            var userAuthDto = CreateUserAuthDto(user);
            await SetRefreshTokenAsync(user);
            _logger.LogInformation("User {UserName} refreshed token successfully", user.UserName);
            
            return Result<UserAuthDto>.Success(userAuthDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(RefreshTokenAsync), nameof(AuthService), e.Message, e);
        }
    }

    private async Task SetRefreshTokenAsync(AppUser user)
    {
        var refreshToken = _tokenService.GenerateRefreshTokenAsync();
        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            Expires = DateTime.UtcNow.AddDays(_tokenSettings.RefreshTokenExpirationInDays),
        });
        await _userManager.UpdateAsync(user);
            
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(_tokenSettings.RefreshTokenExpirationInDays),
        };
        _httpContextAccessor.HttpContext!.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    } 
    
    private UserAuthDto CreateUserAuthDto(AppUser user)
    {
        var userAuthDto = new UserAuthDto
        {
            Username = user.UserName,
            DisplayName = user.DisplayName!,
            Email = user.Email,
            Token = _tokenService.GenerateAccessTokenAsync(user.Id),
        };

        return userAuthDto;
    }
}