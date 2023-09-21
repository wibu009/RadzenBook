using Microsoft.AspNetCore.Builder;

namespace RadzenBook.Infrastructure.Cors;

public static class Startup
{
    public static IServiceCollection AddCrossOriginResourceSharing(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                policy
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithExposedHeaders("WWW-Authenticate", "Pagination", "Accept-Language")
                    .WithOrigins("http://localhost:5000", "https://localhost:5001");
            });
        });

        return services;
    }
    
    public static IApplicationBuilder UseCrossOriginResourceSharing(this IApplicationBuilder app)
    {
        app.UseCors("CorsPolicy");

        return app;
    }
}