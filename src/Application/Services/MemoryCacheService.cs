﻿using Application.Common.Settings;
using Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Application.Services;

public class MemoryCacheService : ICacheService
{
    private readonly int _expirationTimeInDays;
    private readonly IMemoryCache _memoryCache;

    public MemoryCacheService(IMemoryCache memoryCache, IOptions<CrawlerSettings> options)
    {
        _expirationTimeInDays = options.Value.CacheItemsTimeSpanInDays;
        _memoryCache = memoryCache;
    }

    public T? GetFromCache<T>(string key)
    {
        if (_memoryCache.TryGetValue(key, out T value))
        {
            return value;
        }

        return default(T);
    }

    public void SetCache<T>(string key, T value)
    {
        var options = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromDays(_expirationTimeInDays));

        _memoryCache.Set(key, value, options);
    }
}
