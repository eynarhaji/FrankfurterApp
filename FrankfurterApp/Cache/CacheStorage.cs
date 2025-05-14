using System;
using System.Text;
using System.Threading.Tasks;
using FrankfurterApp.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FrankfurterApp.Cache;

public class CacheStorage : ICacheStorage
{
    private readonly IDistributedCache _distributedCache;
    private DistributedCacheEntryOptions _options;
    private readonly ILogger<CacheStorage> _logger;

    public CacheStorage(IDistributedCache distributedCache, IOptionsMonitor<CacheSettings> options, ILogger<CacheStorage> logger)
    {
        _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        
        var settings = (options ?? throw new ArgumentNullException(nameof(options))).CurrentValue;
        
        _options = new DistributedCacheEntryOptions()
        {
            AbsoluteExpiration = null,
            AbsoluteExpirationRelativeToNow = settings.AbsoluteExpirationRelativeToNow,
            SlidingExpiration = settings.SlidingExpiration
        };
        
        options.OnChange(cacheSettings => 
        {
            _options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = null,
                AbsoluteExpirationRelativeToNow = cacheSettings.AbsoluteExpirationRelativeToNow,
                SlidingExpiration = cacheSettings.SlidingExpiration
            };
        });
    }

    public virtual async Task<T> GetAsync<T>(string key)
    {
        _logger.LogInformation("Getting cache key: {Key}", key);
        
        var val = await _distributedCache.GetAsync(key);
        
        return val != null ? Encoding.UTF8.GetString(val).JsonDeserializeFromCamelCase<T>() : default;
    }

    public virtual async Task SetAsync<T>(string key, T value)
    {
        _logger.LogInformation("Setting cache key: {Key}", key);
        
        var val = Encoding.UTF8.GetBytes(value.JsonSerializeToCamelCase());
        
        await _distributedCache.SetAsync(key, val, _options);
    }

    public async Task RemoveAsync(string key)
    {
        _logger.LogInformation("Removing cache key: {Key}", key);
        
        await _distributedCache.RemoveAsync(key);
    }
}