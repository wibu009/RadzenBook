using RadzenBook.Infrastructure.Identity.Role;

namespace RadzenBook.Infrastructure.Persistence.DataInitialization;

public static class RoleGenerator
{
    public static async Task<IEnumerable<AppRole>> GenerateAsync(RoleManager<AppRole> roleManager)
    {
        if (roleManager.Roles.Any()) return roleManager.Roles.ToList();

        var roles = new List<AppRole>
        {
            new() { Name = RoleName.Manager },
            new() { Name = RoleName.Employee },
            new() { Name = RoleName.Customer }
        };

        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }

        return roleManager.Roles.ToList();
    }
}