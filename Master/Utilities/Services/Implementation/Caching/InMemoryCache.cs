namespace Master.Utilities.Services.Implementation.Caching;

using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Abstraction.Caching;
using Abstraction.Serializing;

public class InMemoryCache : ICache
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<InMemoryCache> _logger;
    private readonly ISerialize _serializer;

    public InMemoryCache(IMemoryCache cache, ILogger<InMemoryCache> logger, ISerialize serialize)
    {
        _cache = cache;
        _logger = logger;
        _serializer = serialize;
        _logger.LogInformation("InMemory Cache Start working");
    }

    public void Set<TSource>(string key, TSource source, DateTime? absoluteExpiration, TimeSpan? slidingExpiration)
    {
        _logger.LogTrace("InMemoryCache Cache {obj} with key : {key} " +
                      ", with data : {data} " +
                      ", with absoluteExpiration : {absoluteExpiration} " +
                      ", with slidingExpiration : {slidingExpiration}",
                      typeof(TSource),
                      key,
                      _serializer.Serialize(source),
                      absoluteExpiration.ToString(),
                      slidingExpiration.ToString());

        _cache.Set<TSource>(key, source, new MemoryCacheEntryOptions
        {
            AbsoluteExpiration = absoluteExpiration,
            SlidingExpiration = slidingExpiration
        });
    }

    public TOutput Get<TOutput>(string key)
    {
        TOutput result = default;
        _logger.LogTrace("InMemoryCache Try Get Cache with key : {key}", key);

        if (_cache.TryGetValue<TOutput>(key, out result))
        {
            _logger
                .LogTrace("InMemoryCache Successful Get Cache with key : {key} and data : {data}",
                                    key,
                                    _serializer.Serialize(result));
        }
        else
            _logger.LogTrace("InMemoryCache Failed Get Cache with key : {key}", key);

        return result;
    }

    public void Remove(string key)
    {
        _logger.LogTrace("InMemoryCache Remove Cache with key : {key}", key);
        _cache.Remove(key);
    }
}
