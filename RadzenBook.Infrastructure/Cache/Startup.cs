using RadzenBook.Infrastructure.Cache.LocalCache;

namespace RadzenBook.Infrastructure.Cache;

public static class Startup
{
    public static IServiceCollection AddCache(this IServiceCollection services)
    {
        services.AddLocalCache();
        
        return services;
    }
}