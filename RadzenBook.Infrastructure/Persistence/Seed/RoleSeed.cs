using RadzenBook.Infrastructure.Identity.Role;

namespace RadzenBook.Infrastructure.Persistence.Seed;

public static partial class Seed
{
    private static async Task SeedRoles(RoleManager<AppRole> roleManager)
    {
        // Create roles if they don't exist
        var roles = new List<string> { RoleName.Manager, RoleName.Employee, RoleName.Customer };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new AppRole { Name = role });
            }
        }
    }
}