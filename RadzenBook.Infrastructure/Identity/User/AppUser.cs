using RadzenBook.Infrastructure.Identity.Token;

namespace RadzenBook.Infrastructure.Identity.User;

public class AppUser : IdentityUser<Guid>
{
    public string? DisplayName { get; set; } = string.Empty;
    public bool IsGuest { get; set; } = false;
    public string CreatedBy { get; set; } = "System";
    public string ModifiedBy { get; set; } = "System";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
    public string? AvatarUrl { get; set; } = string.Empty;
    public virtual ICollection<Domain.Catalog.Address> Addresses { get; set; } = new List<Domain.Catalog.Address>();
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}