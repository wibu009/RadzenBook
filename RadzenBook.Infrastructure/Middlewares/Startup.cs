using Microsoft.AspNetCore.Builder;

namespace RadzenBook.Infrastructure.Middlewares;

public static class Startup
{
    public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        
        return app;
    }
}