using System;

namespace FrankfurterApp.Cache;

public class CacheSettings
{
    public bool ForceRedis { get; set; } = false;
    public string ConnectionString { get; set; }
    public DateTimeOffset? AbsoluteExpiration { get; set; }
    public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }
    public TimeSpan? SlidingExpiration { get; set; }
}