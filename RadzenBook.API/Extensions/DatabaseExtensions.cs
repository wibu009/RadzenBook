using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RadzenBook.Database;
using RadzenBook.Entity;

namespace RadzenBook.API.Extensions;

public static class DatabaseExtensions
{
    public static async Task ApplyMigrations(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
        var logger = loggerFactory.CreateLogger("Migrations");
        try
        {
            var context = services.GetRequiredService<RadzenBookDbContext>();
            await context.Database.MigrateAsync();
            await context.SeedData(userManager, roleManager);
            logger.LogInformation("Migrated successfully");
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred during migration");
        }
    }
}