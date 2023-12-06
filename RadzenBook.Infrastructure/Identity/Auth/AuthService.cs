using RadzenBook.Application.Common.Cache;
using RadzenBook.Application.Common.Exceptions;
using RadzenBook.Application.Common.Models;
using RadzenBook.Application.Identity.Auth;
using RadzenBook.Application.Identity.Token;
using RadzenBook.Infrastructure.Common.Extensions;
using RadzenBook.Infrastructure.Identity.Auth.OAuth2.Facebook;
using RadzenBook.Infrastructure.Identity.Auth.OAuth2.Google;
using RadzenBook.Infrastructure.Identity.Role;
using RadzenBook.Infrastructure.Identity.Token;
using RadzenBook.Infrastructure.Identity.User;

namespace RadzenBook.Infrastructure.Identity.Auth;

public class AuthService : IAuthService
{
    #region Services And Fields

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

    #endregion

    #region Constructor

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

    #endregion

    #region Public Methods

    public async Task<Result<UserAuthDto>> LoginAsync(LoginRequest loginRequest)
    {
        try
        {
            var user = await _userManager.Users
                .SingleOrDefaultAsync(x => x.UserName == loginRequest.Username || x.Email == loginRequest.Username);
            if (user == null)
                return Result<UserAuthDto>.Failure(_t["Incorrect username or password"],
                    (int)HttpStatusCode.Unauthorized);

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, false);
            if (!result.Succeeded)
                return Result<UserAuthDto>.Failure(_t["Incorrect username or password"],
                    (int)HttpStatusCode.Unauthorized);

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

    public async Task<Result<string>> ExternalLoginAsync(string provider)
    {
        try
        {
            var clientUrl = _httpContextAccessor.HttpContext!.Request.GetUrlFromRequest().IsNullOrEmpty()
                ? _authenticationSettings.DefaultClientAppUrl
                : _httpContextAccessor.HttpContext.Request.GetUrlFromRequest();
            var clientIp = _httpContextAccessor.HttpContext.GetIpAddress();
            var state = Guid.NewGuid().ToString();
            _cacheService.Set(state, Tuple.Create(clientUrl, clientIp), TimeSpan.FromMinutes(5));

            return await Task.FromResult(provider.ToLower() switch
            {
                "facebook" => Result<string>.Success(data: _facebookAuthService.GetLoginLinkUrl(state),
                    statusCode: (int)HttpStatusCode.Redirect),
                "google" => Result<string>.Success(data: _googleAuthService.GetLoginLinkUrl(state),
                    statusCode: (int)HttpStatusCode.Redirect),
                _ => Result<string>.Failure(_t["Invalid provider"], (int)HttpStatusCode.BadRequest)
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw ServiceException.Create(nameof(ExternalLoginAsync), nameof(AuthService), e.Message, e);
        }
    }

    public async Task<Result<string>> ExternalLoginCallbackAsync(string provider, string code, string? error = "",
        string? state = "")
    {
        try
        {
            dynamic user = provider.ToLower() switch
            {
                "google" => await _googleAuthService.GetUserFromCode(code),
                "facebook" => await _facebookAuthService.GetUserFromCode(code),
                _ => Result<string>.Failure(_t["Invalid provider"], (int)HttpStatusCode.BadRequest)
            };

            if (user == null)
            {
                _logger.LogError("External login error: {RemoteError}", error);
                return Result<string>.Failure(_t["External login error"], (int)HttpStatusCode.BadRequest);
            }

            var (clientUrl, clientIp) = _cacheService.Get<Tuple<string, string>>(state!)!;
            await _cacheService.RemoveAsync(state!);

            var signInCode = _tokenService.GenerateToken();

            var userExist = await _userManager.FindByEmailAsync(user.Email as string);
            if (userExist != null)
            {
                await _cacheService.SetAsync(signInCode, Tuple.Create(userExist.Id, clientIp), TimeSpan.FromMinutes(5));
                return Result<string>.Success(
                    data: clientUrl!.AddQueryParam("signInCode", signInCode),
                    statusCode: (int)HttpStatusCode.Redirect);
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
            await _userManager.AddToRoleAsync(appUser, RoleName.Customer);

            _logger.LogInformation("User {UserName} registered successfully", appUser.UserName);
            await _cacheService.SetAsync(signInCode, Tuple.Create(appUser.Id, clientIp), TimeSpan.FromMinutes(5));

            return Result<string>.Success(
                data: clientUrl!.AddQueryParam("signInCode", signInCode),
                statusCode: (int)HttpStatusCode.Redirect);
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
            if (userExist != null)
                return Result<UserAuthDto>.Failure(_t["Username already exists"], (int)HttpStatusCode.BadRequest);

            var user = new AppUser
            {
                UserName = registerRequest.Username,
                Email = registerRequest.Email,
                PhoneNumber = registerRequest.PhoneNumber
            };

            await _userManager.CreateAsync(user, registerRequest.Password);
            await _userManager.AddToRoleAsync(user, RoleName.Customer);

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

    public async Task<Result<UserAuthDto>> RefreshTokenAsync(string? signInCode = null)
    {
        try
        {
            AppUser? user;

            if (!signInCode.IsNullOrEmpty())
            {
                var (userId, clientIp) = _cacheService.Get<Tuple<Guid, string>>(signInCode!)!;
                if (userId == Guid.Empty || clientIp.IsNullOrEmpty())
                {
                    _logger.LogError("Invalid sign signInCode");
                    return Result<UserAuthDto>.Failure(_t["Invalid sign signInCode"], (int)HttpStatusCode.Unauthorized);
                }

                await _cacheService.RemoveAsync(signInCode!);

                if (clientIp != _httpContextAccessor.HttpContext!.GetIpAddress())
                {
                    _logger.LogError("Invalid request ip: {ClientIp}", clientIp);
                    return Result<UserAuthDto>.Failure(_t["Invalid sign signInCode"], (int)HttpStatusCode.Unauthorized);
                }

                user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    return Result<UserAuthDto>.Failure(_t["Invalid sign signInCode"], (int)HttpStatusCode.Unauthorized);

                await SetRefreshTokenAsync(user);
                _logger.LogInformation("User {UserName} refreshed token successfully", user.UserName);

                return Result<UserAuthDto>.Success(CreateUserAuthDto(user));
            }

            var refreshToken = _httpContextAccessor.HttpContext!.Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                return Result<UserAuthDto>.Failure(_t["Invalid refresh token"], (int)HttpStatusCode.Unauthorized);

            user = await _userManager.Users.Include(x => x.RefreshTokens)
                .SingleOrDefaultAsync(x => x.RefreshTokens.Any(t => t.Token == refreshToken));
            if (user == null)
                return Result<UserAuthDto>.Failure(_t["Invalid refresh token"], (int)HttpStatusCode.Unauthorized);

            var oldRefreshToken = user.RefreshTokens.Single(x => x.Token == refreshToken);
            if (!oldRefreshToken.IsActive)
                return Result<UserAuthDto>.Failure(_t["Invalid refresh token"], (int)HttpStatusCode.Unauthorized);
            oldRefreshToken.Revoked = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            await SetRefreshTokenAsync(user);
            _logger.LogInformation("User {UserName} refreshed token successfully", user.UserName);

            return Result<UserAuthDto>.Success(CreateUserAuthDto(user));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw ServiceException.Create(nameof(RefreshTokenAsync), nameof(AuthService), e.Message, e);
        }
    }

    #endregion

    #region Private Methods

    private async Task SetRefreshTokenAsync(AppUser user)
    {
        var refreshToken = _tokenService.GenerateToken();
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

        _logger.LogInformation("User {UserName} set refresh token successfully", user.UserName);
    }

    private UserAuthDto CreateUserAuthDto(AppUser user)
    {
        var userAuthDto = new UserAuthDto
        {
            DisplayName = user.DisplayName!,
            Email = user.Email,
            Avatar = user.AvatarUrl!,
            Token = _tokenService.GenerateAccessToken(user.Id),
        };

        return userAuthDto;
    }

    #endregion
}