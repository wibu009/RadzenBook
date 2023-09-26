using RadzenBook.Application.Common.Cache;

namespace RadzenBook.Infrastructure.Cache.LocalCache;

public class LocalCacheService : ICacheService
{
    private readonly ILogger<LocalCacheService> _logger;
    private readonly IMemoryCache _cache;
    
    public LocalCacheService(IMemoryCache cache, ILoggerFactory loggerFactory)
    {
        _cache = cache;
        _logger = loggerFactory.CreateLogger<LocalCacheService>();
    }
    
    public T? Get<T>(string key) =>
        _cache.Get<T>(key);
    
    public Task<T?> GetAsync<T>(string key, CancellationToken token = default) =>
        Task.FromResult(Get<T>(key));
    
    public void Refresh(string key) =>
        _cache.TryGetValue(key, out _);

    public Task RefreshAsync(string key, CancellationToken token = default)
    {
        Refresh(key);
        return Task.CompletedTask;
    }
    
    public void Remove(string key) =>
        _cache.Remove(key);

    public Task RemoveAsync(string key, CancellationToken token = default)
    {
        Remove(key);
        return Task.CompletedTask;
    }

    public void Set<T>(string key, T value, TimeSpan? slidingExpiration = null)
    {
        slidingExpiration ??= TimeSpan.FromMinutes(10); // Default expiration time of 10 minutes.
        
        _cache.Set(key, value, new MemoryCacheEntryOptions { SlidingExpiration = slidingExpiration });
        _logger.LogDebug("Added to Cache : {Key}", key);
    }
    
    public Task SetAsync<T>(string key, T value, TimeSpan? slidingExpiration = null, CancellationToken token = default)
    {
        Set(key, value, slidingExpiration);
        return Task.CompletedTask;
    }
}