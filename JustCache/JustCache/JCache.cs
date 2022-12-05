namespace JustCache;


public class JCache
{
    private IInternalCacheService _internalCacheService = new InternalCacheService();
    
    public static JCache Instance { get; } = new();

    /// <summary>
    /// Capacity of the cache. Can be set.
    /// </summary>
    public int Capacity 
    {
        get
        {
            return _internalCacheService.Capacity;
        }
        set
        {
            _internalCacheService.SetCapacity(value);
        }
    }
    
    /// <summary>
    /// Get an item from the cache.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="itemReturned"></param>
    /// <typeparam name="TItem"></typeparam>
    /// <returns></returns>
    public bool GetItem(string key, out object itemReturned)
    {
        itemReturned = null;

        if(!_internalCacheService.TryGetCacheItem(key, out itemReturned))
            return false;
        
        return true;
    }

    /// <summary>
    /// Add an item to the cache. If the cache capacity is reached then the least recently used item will be removed
    /// from the cache and returned.
    /// </summary>
    /// <param name="key">Key for the item.</param>
    /// <param name="item">Item to add.</param>
    /// <param name="evictedCacheItem">Least recently used item evicted from the cache.</param>
    /// <returns></returns>
    public bool AddItem(string key, object item, out object evictedCacheItem)
    {
        _internalCacheService.AddCacheItem(key, item, out evictedCacheItem);
        return true;
    }
}