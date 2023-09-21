using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RadzenBook.Application.Common.Auth;
using RadzenBook.Infrastructure.Auth;
using RadzenBook.Infrastructure.Identity.User;

namespace RadzenBook.Infrastructure.Identity.Token;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly UserManager<AppUser> _userManager;

    public TokenService(IConfiguration config, UserManager<AppUser> userManager)
    {
        _config = config;
        _userManager = userManager;
    }

    public async Task<string> CreateTokenAsync<TKey>(TKey userId)
    {
        var jwtSettings = _config.GetSection("JwtSettings").Get<JwtSettings>();
        
        var user = await _userManager.FindByIdAsync(userId!.ToString());
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
        };

        var roles = await _userManager.GetRolesAsync(user);

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings!.Key));

        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(jwtSettings.ExpireDays),
            SigningCredentials = cred,
            Issuer = jwtSettings.Issuer,
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}