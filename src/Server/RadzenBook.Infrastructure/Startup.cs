using RadzenBook.Infrastructure.ApiVersion;
using RadzenBook.Infrastructure.Cache;
using RadzenBook.Infrastructure.Cors;
using RadzenBook.Infrastructure.Identity;
using RadzenBook.Infrastructure.Localization;
using RadzenBook.Infrastructure.Logger;
using RadzenBook.Infrastructure.Mapper;
using RadzenBook.Infrastructure.Middlewares;
using RadzenBook.Infrastructure.OpenApi;
using RadzenBook.Infrastructure.Persistence;
using RadzenBook.Infrastructure.Security;
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
        services.AddHttpClient();
        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer();
        services.AddVersion();
        services.AddCache();
        services.AddCorsPolicy();
        services.AddOpenApi(configuration);
        services.AddLogger();
        services.AddPoLocalization(configuration);
        services.AddPersistence(configuration);
        services.AddMapping();
        services.AddValidations();
        services.AddScoped<IInfrastructureServiceManager, InfrastructureServiceManager>();
        services.AddIdentity(configuration);
        services.AddSecurity(configuration);
        
        return services;
    }
    
    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        app.UseRouting();
        app.UseCustomMiddleware();
        app.UseOpenApi();
        app.UseSecurity();
        app.UseIdentity();
        app.UseCorsPolicy();
        app.UsePoLocalization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        app.ApplyMigrations().Wait();
        
        return app;
    }
}