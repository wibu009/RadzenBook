using RadzenBook.Infrastructure.Identity.Role;
using RadzenBook.Infrastructure.Identity.User;

namespace RadzenBook.Infrastructure.Persistence.Seed;

public static partial class Seed
{
    public static async Task SeedData(this RadzenBookDbContext context, UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager)
    {
        await SeedRoles(roleManager);
        await SeedUsers(userManager);
        await SeedDemos(context);
        await SeedAuthors(context);
        await SeedCategories(context);

        await context.SaveChangesAsync();
    }
}