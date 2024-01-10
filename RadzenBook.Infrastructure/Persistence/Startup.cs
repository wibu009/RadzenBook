using Microsoft.Extensions.Hosting;
using RadzenBook.Application.Common.Persistence;
using RadzenBook.Infrastructure.Identity.Role;
using RadzenBook.Infrastructure.Identity.User;
using RadzenBook.Infrastructure.Persistence.DataInitialization;

namespace RadzenBook.Infrastructure.Persistence;

public static class Startup
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<RadzenBookDbContext>(options =>
        {
            var databaseSettings = configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>()!;
            var connectionString = databaseSettings.ConnectionString;

            if (string.IsNullOrEmpty(connectionString))
                throw new NullReferenceException("Connection string is missing");

            switch (databaseSettings.DatabaseProvider)
            {
                case "SqlServer":
                    options.UseSqlServer(connectionString);
                    break;
                case "PostgreSql":
                    options.UseNpgsql(connectionString);
                    break;
                case "Sqlite":
                    options.UseSqlite(connectionString);
                    break;
                case "MySql":
                    options.UseMySQL(connectionString);
                    break;
                default:
                    throw new NullReferenceException("Database provider is missing");
            }
        });

        services.AddScoped<IUnitOfWork>(provider =>
            new UnitOfWork(provider.GetRequiredService<RadzenBookDbContext>()));

        return services;
    }

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