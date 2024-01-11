using RadzenBook.Infrastructure.Security.Header;
using RadzenBook.Infrastructure.Security.RateLimit;

namespace RadzenBook.Infrastructure.Security;

public static class Startup
{
    public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration config)
    {
        services.AddRateLimit(config);

        return services;
    }
    
    public static IApplicationBuilder UseSecurity(this IApplicationBuilder app)
    {
        app.UseSecurityHeaders();
        app.UseRateLimit();
        
        return app;
    }
}