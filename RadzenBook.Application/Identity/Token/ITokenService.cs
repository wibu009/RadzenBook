using System.Security.Claims;

namespace RadzenBook.Application.Identity.Token;

public interface ITokenService
{
    string GenerateAccessTokenAsync<TKey>(TKey userId);
    string GenerateRefreshTokenAsync();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}