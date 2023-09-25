using System.Security.Claims;
using RadzenBook.Application.Common.Security;

namespace RadzenBook.Infrastructure.Security;

public class UserAccessor : IUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUsername() => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name)!;

    public string GetUserId() => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)!;

    public string GetUserRole() => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role)!;
}