using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using RadzenBook.Infrastructure.Logger.Serilog;

namespace RadzenBook.Infrastructure.Logger;

public static class Startup
{
    public static IServiceCollection AddLogger(this IServiceCollection services)
    {
        services.AddLogging();
        services.AddSingleton<ILoggerFactory, LoggerFactory>();
        
        return services;
    }

    public static WebApplicationBuilder UseLogging(this WebApplicationBuilder builder)
    {
        builder.UseSerilogging();

        return builder;
    }
}