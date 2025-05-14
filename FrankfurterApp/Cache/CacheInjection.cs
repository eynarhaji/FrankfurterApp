using FrankfurterApp.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FrankfurterApp.Cache;

public static class CacheInjection
{
    public static IServiceCollection AddCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSettingsToOptions(configuration, "Cache", out CacheSettings settings);

        if (settings.ForceRedis)
        {
            services.AddRedisCache(settings);
            return services;
        }

        services.AddInMemoryCache();
        return services;
    }

    public static IServiceCollection AddInMemoryCache(this IServiceCollection services)
    {
        services.AddDistributedMemoryCache();
        services.AddSingleton<ICacheStorage, CacheStorage>();

        return services;
    }

    public static IServiceCollection AddRedisCache(this IServiceCollection services, CacheSettings settings)
    {
        services.AddStackExchangeRedisCache(options => options.Configuration = settings.ConnectionString);
        services.AddSingleton<ICacheStorage, CacheStorage>();

        return services;
    }
}