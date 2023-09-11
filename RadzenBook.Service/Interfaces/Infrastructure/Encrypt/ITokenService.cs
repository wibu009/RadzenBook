using RadzenBook.Entity;

namespace RadzenBook.Service.Interfaces.Infrastructure.Encrypt;

public interface ITokenService
{
    Task<string> CreateTokenAsync(AppUser user);
}