using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Showcase.Identity.InMemoryCache;

public class CacheManager : ICacheManager
{
    private readonly ILogger<CacheManager> _logger;
    private readonly IMemoryCache _memoryCache;
    private readonly int _cacheExpireTimeInSeconds;

    public CacheManager(
        IOptions<InMemoryCacheOptions> cacheOptions,
        ILogger<CacheManager> logger,
        IMemoryCache memoryCache)
    {
        _cacheExpireTimeInSeconds = cacheOptions.Value.ExpireTimeInSeconds;
        _logger = logger;
        _memoryCache = memoryCache;
    }

    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
        => _memoryCache.TryGetValue(key, out var value) ? Task.FromResult((T?)value) : Task.FromResult<T?>(null);

    public async Task<T?> GetOrAddAsync<T>(string key, Func<string, Task<T?>> func, CancellationToken cancellationToken = default) where T : class?
    {
        try
        {
            if (_memoryCache.TryGetValue(key, out var value))
                return (T?)value;

            var obj = await func(key);
            if (obj is null)
                return null;

            Set(key, obj);
            return obj;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Message}", ex.Message);
            throw;
        }
    }

    public bool FlushCache()
    {
        try
        {
            ((MemoryCache)_memoryCache).Compact(1.0);
            _logger.LogInformation("Cache flushed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while compacting memory cache");
        }

        return true;
    }

    public void Update<T>(string key, T value) where T : class
    {
        try
        {
            _memoryCache.Remove(key);
            Set(key, value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Message}", ex.Message);
            throw;
        }
    }

    public void Remove(string key)
    {
        try
        {
            _memoryCache.Remove(key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Message}", ex.Message);
            throw;
        }
    }

    public void Set<T>(string key, T value) where T : class?
    {
        if (value is null)
        {
            _logger.LogError("In memory cache null value exception");
            return;
        }

        try
        {
            _memoryCache.Set(key, value, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(_cacheExpireTimeInSeconds)));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Message}", ex.Message);
            throw;
        }
    }
}