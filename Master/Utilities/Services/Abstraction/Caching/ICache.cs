namespace Master.Utilities.Services.Abstraction.Caching;

public interface ICache
{
    void Set<TSource>(string key, TSource source, DateTime? absoluteExpiration, TimeSpan? slidingExpiration);
    TOutput Get<TOutput>(string key);
    void Remove(string key);
}
