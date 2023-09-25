using RadzenBook.Infrastructure.Identity.User;

namespace RadzenBook.Infrastructure.Identity.Token;

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = default!;
    public DateTime Expires { get; set; } = DateTime.UtcNow.AddDays(30);
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public DateTime? Revoked { get; set; }
    public bool IsActive => Revoked == null && !IsExpired;
    public Guid AppUserId { get; set; }
    public AppUser AppUser { get; set; } = default!;
}