using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using RadzenBook.Application.Identity.Token;
using RadzenBook.Infrastructure.Identity.Auth;
using RadzenBook.Infrastructure.Identity.User;

namespace RadzenBook.Infrastructure.Identity.Token;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<AppUser> _userManager;

    public TokenService(IConfiguration config, UserManager<AppUser> userManager)
    {
        _jwtSettings = config.GetSection("AuthenticationSettings").Get<AuthenticationSettings>()!.JwtSettings;
        _userManager = userManager;
    }

    public string GenerateAccessToken<TKey>(TKey userId)
    {
        var user = _userManager.FindByIdAsync(userId!.ToString()).Result;
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
        };
        var roles = _userManager.GetRolesAsync(user).Result;
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(_jwtSettings.AccessTokenExpirationInMinutes),
            SigningCredentials = cred,
            Issuer = _jwtSettings.Issuer,
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
    
    public string GenerateToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
            ValidateIssuer = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = _jwtSettings.Audience,
            ValidateLifetime = false,
            ClockSkew = TimeSpan.Zero
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }
}