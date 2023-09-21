using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using RadzenBook.Application.Common.Auth;

namespace RadzenBook.Infrastructure.Auth;

public class UserAccessor : IUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUsername() => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name)!;
    
    public string GetUserEmail() => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email)!;

    public string GetUserId() => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)!;

    public string GetUserRole() => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role)!;
}