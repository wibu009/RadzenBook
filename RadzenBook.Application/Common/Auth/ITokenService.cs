namespace RadzenBook.Application.Common.Auth;

public interface ITokenService
{
    Task<string> CreateTokenAsync<TKey>(TKey userId);
}