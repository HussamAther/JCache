namespace JustCache;

public interface IInternalCacheService
{
    bool TryGetCacheItem(string key, out object value);
    void AddCacheItem(string key, object value, out object evictedCacheItem);
    void SetCapacity(int capacity);
    int Capacity { get;  }
}