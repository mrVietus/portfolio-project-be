using Crawler.Application.Common.Interfaces;
using Crawler.Application.Common.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Crawler.Infrastructure.Cache;

public class MemoryCacheService(IMemoryCache memoryCache, IOptions<CrawlerSettings> options) : ICacheService
{
    private readonly int _expirationTimeInDays = options.Value.CacheItemsTimeSpanInDays;

    public T? GetFromCache<T>(string key)
    {
        if (memoryCache.TryGetValue(key, out T? value))
        {
            return value;
        }

        return default;
    }

    public void SetCache<T>(string key, T value)
    {
        var options = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromDays(_expirationTimeInDays));

        memoryCache.Set(key, value, options);
    }
}
