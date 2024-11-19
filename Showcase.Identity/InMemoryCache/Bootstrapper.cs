using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Showcase.Identity.InMemoryCache;

public static class Bootstrapper
{
    public static void AddInMemoryCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();
        services.Configure<InMemoryCacheOptions>(options => configuration.GetSection(nameof(InMemoryCacheOptions)).Bind(options));
        services.TryAddSingleton<ICacheManager, CacheManager>();
    }
}