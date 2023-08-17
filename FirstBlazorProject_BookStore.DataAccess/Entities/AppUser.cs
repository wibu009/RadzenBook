using Microsoft.AspNetCore.Identity;

namespace FirstBlazorProject_BookStore.DataAccess.Entities;

public class AppUser : IdentityUser<Guid>
{
    [PersonalData]
    public string FullName { get; set; } = default!;
    [PersonalData]
    public DateTime Dob { get; set; } = DateTime.Now;
    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
}