using System.Diagnostics.CodeAnalysis;

namespace Showcase.Identity.InMemoryCache;

public interface ICacheManager
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class;

    void Update<T>(string key, T value) where T : class;

    void Remove(string key);

    void Set<T>(string key, T value) where T : class?;

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public Task<T?> GetOrAddAsync<T>(string key, Func<string, Task<T?>> func, CancellationToken cancellationToken = default) where T : class?;

    public bool FlushCache();
}