using RadzenBook.Entity;

namespace RadzenBook.Service.Interfaces.Infrastructure.Encrypt;

public interface ITokenService
{
    string CreateToken(AppUser user);
    Task<string> CreateTokenAsync(AppUser user);
}