using System.Security.Claims;

namespace RadzenBook.Application.Identity.Token;

public interface ITokenService
{
    string GenerateAccessToken<TKey>(TKey userId);
    string GenerateToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}