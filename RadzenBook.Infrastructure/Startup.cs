using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using RadzenBook.Application.Common;
using RadzenBook.Infrastructure.Auth;
using RadzenBook.Infrastructure.Cache;
using RadzenBook.Infrastructure.Identity;
using RadzenBook.Infrastructure.Localization;
using RadzenBook.Infrastructure.Logger;
using RadzenBook.Infrastructure.Mapper;
using RadzenBook.Infrastructure.Middlewares;
using RadzenBook.Infrastructure.OpenApi;
using RadzenBook.Infrastructure.Persistence;
using RadzenBook.Infrastructure.SecurityHeaders;
using RadzenBook.Infrastructure.Validations;

namespace RadzenBook.Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers(options =>
        {
            var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            options.Filters.Add(new AuthorizeFilter(policy));
        });
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer();
        services.AddCache();
        services.AddOpenApi(configuration);
        services.AddLogger();
        services.AddPoLocalization(configuration);
        services.AddPersistence(configuration);
        services.AddMapping();
        services.AddValidations();
        services.AddScoped<IInfrastructureServiceManager, InfrastructureServiceManager>();
        services.AddIdentity(configuration);
        services.AddAuth(configuration);
        
        return services;
    }
    
    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        app.UseOpenApi();
        app.UseCustomMiddleware();
        app.UseSecurityHeaders();
        app.UseAuth();
        app.UsePoLocalization();
        app.MapControllers();
        app.ApplyMigrations().Wait();
        
        return app;
    }
}