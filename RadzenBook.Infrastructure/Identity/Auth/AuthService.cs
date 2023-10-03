using System.Net;
using Facebook;
using RadzenBook.Application.Common.Cache;
using RadzenBook.Application.Common.Exceptions;
using RadzenBook.Application.Common.Models;
using RadzenBook.Application.Identity.Auth;
using RadzenBook.Application.Identity.Token;
using RadzenBook.Infrastructure.Identity.Auth.OAuth2.Facebook;
using RadzenBook.Infrastructure.Identity.Auth.OAuth2.Google;
using RadzenBook.Infrastructure.Identity.Token;
using RadzenBook.Infrastructure.Identity.User;

namespace RadzenBook.Infrastructure.Identity.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthService> _logger;
    private readonly IStringLocalizer _t;
    private readonly AuthenticationSettings _authenticationSettings;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly GoogleAuthService _googleAuthService;
    private readonly FacebookAuthService _facebookAuthService;
    private readonly ICacheService _cacheService;

    public AuthService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ITokenService tokenService,
        ILoggerFactory loggerFactory,
        IStringLocalizerFactory t,
        IConfiguration config,
        IHttpContextAccessor httpContextAccessor, 
        ICacheService cacheService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _logger = loggerFactory.CreateLogger<AuthService>();
        _t = t.Create(typeof(AuthService));
        _authenticationSettings = config.GetSection("AuthenticationSettings").Get<AuthenticationSettings>()!;
        _httpContextAccessor = httpContextAccessor;
        _cacheService = cacheService;
        _googleAuthService = new GoogleAuthService(config, httpContextAccessor);
        _facebookAuthService = new FacebookAuthService(config, httpContextAccessor);
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
            throw ServiceException.Create(nameof(LoginAsync), nameof(AuthService), e.Message, e);
        }
    }

    public Task<Result<string>> ExternalLoginAsync(string provider)
    {
        try
        {
            var state = _httpContextAccessor.HttpContext!;
            
            return Task.FromResult(provider.ToLower() switch
            {
                "facebook" => Result<string>.Success(data: _facebookAuthService.GetLoginLinkUrl(), statusCode: (int)HttpStatusCode.Redirect),
                "google" => Result<string>.Success(data: _googleAuthService.GetLoginLinkUrl(), statusCode: (int)HttpStatusCode.Redirect),
                _ => throw new ArgumentException($"Invalid provider: {provider}")
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw ServiceException.Create(nameof(ExternalLoginAsync), nameof(AuthService), e.Message, e);
        }
    }

    public async Task<Result<UserAuthDto>> ExternalLoginCallbackAsync(string provider, string code, string? error = "")
    {
        try
        {
            dynamic user = provider.ToLower() switch
            {
                "google" => await _googleAuthService.GetUserFromCode(code),
                "facebook" => await _facebookAuthService.GetUserFromCode(code),
                _ => throw new ArgumentException($"Invalid provider: {provider}")
            };

            if (user == null)
            {
                _logger.LogError("External login error: {RemoteError}", error);
                return Result<UserAuthDto>.Failure(_t["External login error"], (int)HttpStatusCode.BadRequest);
            }
            
            var userExist = await _userManager.FindByEmailAsync(user.Email);
            if (userExist != null)
            {
                var userAuthDto = CreateUserAuthDto(userExist);
                await SetRefreshTokenAsync(userExist);
                return Result<UserAuthDto>.Success(userAuthDto);
            }
            
            var appUser = new AppUser
            {
                UserName = user.Email,
                Email = user.Email,
                DisplayName = user.Name,
                AvatarUrl = user.Picture,
                EmailConfirmed = true
            };
            
            await _userManager.CreateAsync(appUser);
            await _userManager.AddToRoleAsync(appUser, "customer");
            
            var userAuthDto2 = CreateUserAuthDto(appUser);
            await SetRefreshTokenAsync(appUser);
            
            _logger.LogInformation("User {UserName} registered successfully", appUser.UserName);
            return Result<UserAuthDto>.Success(userAuthDto2);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw ServiceException.Create(nameof(ExternalLoginCallbackAsync), nameof(AuthService), e.Message, e);
        }
    }

    public async Task<Result<UserAuthDto>> RegisterAsync(RegisterRequest registerRequest)
    {
        try
        {
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
            throw ServiceException.Create(nameof(RegisterAsync), nameof(AuthService), e.Message, e);
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
            throw ServiceException.Create(nameof(RefreshTokenAsync), nameof(AuthService), e.Message, e);
        }
    }

    private async Task SetRefreshTokenAsync(AppUser user)
    {
        var refreshToken = _tokenService.GenerateRefreshTokenAsync();
        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            Expires = DateTime.UtcNow.AddDays(_authenticationSettings.JwtSettings.RefreshTokenExpirationInDays),
        });
        await _userManager.UpdateAsync(user);
            
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(_authenticationSettings.JwtSettings.RefreshTokenExpirationInDays),
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
            Avatar = user.AvatarUrl!,
            Token = _tokenService.GenerateAccessTokenAsync(user.Id),
        };

        return userAuthDto;
    }
}