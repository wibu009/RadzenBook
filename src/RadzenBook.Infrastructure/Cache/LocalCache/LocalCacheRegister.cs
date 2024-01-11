namespace RadzenBook.Infrastructure.Cache.LocalCache;

public static class LocalCacheRegister
{
    public static IServiceCollection AddLocalCache(this IServiceCollection services)
    {
        services.AddMemoryCache();
        
        return services;
    }
}