using RadzenBook.Infrastructure.Identity.Role;
using RadzenBook.Infrastructure.Identity.User;

namespace RadzenBook.Infrastructure.Persistence.Initialization;

public static class Seed
{
    public static async Task SeedData(this RadzenBookDbContext context, UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager)
    {
        #region Seed data for Identity

        await RoleGenerator.GenerateAsync(roleManager);
        await UserGenerator.GenerateAsync(userManager);

        #endregion

        #region Seed data for Catalog

        await GenresGenerator.GenerateAsync(context);
        await CategoryGenerator.GenerateAsync(context);
        await AuthorGenerator.GenerateAsync(context);

        #endregion

        await DemoGenerator.GenerateAsync(context);

        await context.SaveChangesAsync();
    }
}