using Microsoft.AspNetCore.Identity;
using RadzenBook.Domain.Catalog;

namespace RadzenBook.Infrastructure.Identity.User;

public class AppUser : IdentityUser<Guid>
{
    public string? DisplayName { get; set; }
    public bool IsGuest { get; set; } = false;
    public string CreatedBy { get; set; } = "System";
    public string ModifiedBy { get; set; } = "System";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
    public ICollection<Address> Addresses { get; set; } = new List<Address>();
    public ICollection<Domain.Catalog.Photo> Photos { get; set; } = new List<Domain.Catalog.Photo>();
}