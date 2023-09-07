using FirstBlazorProject_BookStore.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace FirstBlazorProject_BookStore.API.Extensions;

public static class DatabaseExtensions
{
    public static async Task ApplyMigrations(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("Migrations");
        try
        {
            var context = services.GetRequiredService<RadzenBookDataContext>();
            await context.Database.MigrateAsync();
            await context.SeedData();
            logger.LogInformation("Migrated successfully");
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred during migration");
        }
    }
}