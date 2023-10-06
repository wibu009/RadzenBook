using AspNetCoreRateLimit;

namespace RadzenBook.Infrastructure.Security.RateLimit;

public static class RateLimitRegister
{
    public static IServiceCollection AddRateLimit(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<IpRateLimitOptions>(options =>
        {
            options.EnableEndpointRateLimiting = false;
            options.StackBlockedRequests = false;
            options.HttpStatusCode = (int) HttpStatusCode.TooManyRequests;
            options.RealIpHeader = "X-Real-IP";
            options.ClientIdHeader = "X-ClientId";
            options.GeneralRules = new List<RateLimitRule>
            {
                new RateLimitRule()
                {
                    Endpoint = "*",
                    Limit = 3000 ,
                    Period = "1m",
                    QuotaExceededResponse = new QuotaExceededResponse()
                    {
                        Content = "Too many requests, please try again later after 1 minute.",
                        ContentType = "application/json",
                        StatusCode = (int) HttpStatusCode.TooManyRequests
                    },
                    MonitorMode = false,
                },
            };
        });
        services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        services.AddInMemoryRateLimiting();

        return services;
    }

    public static IApplicationBuilder UseRateLimit(this IApplicationBuilder app)
    {
        app.UseIpRateLimiting();
        
        return app;
    }
}