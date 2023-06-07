namespace Master.Utilities.Services.Implementation.Caching;

using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Utilities.Extentions;
using Utilities.Services.Abstraction.Caching;
using Utilities.Services.Abstraction.Serializing;

public class DistributedRedisCache : ICache
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<DistributedRedisCache> _logger;
    private readonly ISerialize _serializer;

    public DistributedRedisCache(IDistributedCache cache, ILogger<DistributedRedisCache> logger, ISerialize serialize)
    {
        _cache = cache;
        _logger = logger;
        _serializer = serialize;
        _logger.LogInformation("Distributed Redis Cache Start working");
    }

    public void Set<TSource>(string key, TSource source, DateTime? absoluteExpiration, TimeSpan? slidingExpiration)
    {
        _logger.LogTrace("Distributed Redis Cache {obj} with key : {key} " +
                      ", with data : {data} " +
                      ", with absoluteExpiration : {absoluteExpiration} " +
                      ", with slidingExpiration : {slidingExpiration}",
                      typeof(TSource),
                      key,
                      _serializer.Serialize(source),
                      absoluteExpiration.ToString(),
                      slidingExpiration.ToString());

        var bytes = Encoding.UTF8.GetBytes(_serializer.Serialize(source));
        _cache.Set(key, bytes, new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = absoluteExpiration,
            SlidingExpiration = slidingExpiration
        });
    }

    public TOutput Get<TOutput>(string key)
    {
        var result = default(TOutput);
        var output = _cache.GetString(key);
        if (output.IsNull())
        {
            _logger.LogTrace("Distributed Redis Cache Failed Get Cache with key : {key}", key);
        }
        else
        {
            _logger.LogTrace("Distributed Redis Cache Successful Get Cache with key : {key} and data : {data}",
                         key, output);

            result = _serializer.Deserialize<TOutput>(output);
        }
        return result;
    }

    public void Remove(string key)
    {
        _logger.LogTrace("Distributed Redis Cache Remove Cache with key : {key}", key);
        _cache.Remove(key);
    }
}

public class DistributedRedisCacheConfig
{
    public string Configuration { get; set; } = String.Empty;
    public string InstanceName { get; set; } = String.Empty;
}
