using RadzenBook.Contract.Core;
using RadzenBook.Contract.DTO.Auth;

namespace RadzenBook.Service.Interfaces.Features;

public interface IAccountService
{
    Task<Result<UserAuthDto>> LoginAsync(LoginRequestDto loginRequestDto);
    Task<Result<UserAuthDto>> RegisterAsync(RegisterRequestDto registerRequestDto);
}