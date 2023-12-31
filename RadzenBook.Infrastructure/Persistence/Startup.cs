﻿using Microsoft.Extensions.Hosting;
using RadzenBook.Application.Common.Persistence;
using RadzenBook.Infrastructure.Identity.Role;
using RadzenBook.Infrastructure.Identity.User;
using RadzenBook.Infrastructure.Persistence.Seed;

namespace RadzenBook.Infrastructure.Persistence;

public static class Startup
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<RadzenBookDbContext>(options =>
        {
            var proEnv = Environment.GetEnvironmentVariable("PROVIDER_ENVIRONMENT");

            var connectionString = proEnv switch
            {
                "Development" => configuration.GetConnectionString("LocalConnection"),
                "Production" => configuration.GetConnectionString("RemoteConnection"),
                "Docker" => configuration.GetConnectionString("DockerConnection"),
                _ => throw new NullReferenceException("Connection string is missing")
            };

            if (string.IsNullOrEmpty(connectionString))
                throw new NullReferenceException("Connection string is missing");

            options.UseSqlServer(connectionString);
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