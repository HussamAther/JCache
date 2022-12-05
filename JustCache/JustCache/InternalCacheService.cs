using JustCache.Models;

namespace JustCache;

public class InternalCacheService : IInternalCacheService
{
    private readonly Dictionary<string, LinkedListNode<CacheItem>> _cache = new();
    private LinkedList<CacheItem> _collection = new();
    private readonly object _cacheLock = new();
    
    //setting a default capacity so the cache can be used.
    public int Capacity => _capacity;
    private int _capacity = 5;

    public void SetCapacity(int capacity)
    {
        if(capacity <= 0)
            throw new ArgumentException("Cannot set a capacity of less than or equal to zero.");
        
        _capacity = capacity;
    }
    
    public bool TryGetCacheItem(string key, out object value)
    {
        lock (_cacheLock)
        {
            value = null;
            KeyValidation(key);

            if (!_cache.TryGetValue(key, out LinkedListNode<CacheItem> node))
                return false;


            _collection.Remove(node);
            _collection.AddFirst(node);
            value = node.Value.Value;
            return true;
        }
    }

    public void AddCacheItem(string key, object value, out object evictedCacheItem)
    { 
        lock (_cacheLock)
        {
            evictedCacheItem = null;
            KeyValidation(key);
            if (CheckIfKeyExists(key))
                throw new ArgumentException($"{nameof(key)} already exists in the cache.");

            //if the key already exists then we will just update the collection to have the 
            //node at the top of the linked list. If it does not exist then we will 
            //have to add it. If the size of the linked list is then greater than the max capacity allowed
            //then we will remove the last node.
            if (_cache.TryGetValue(key, out LinkedListNode<CacheItem> _cacheNode))
            {
                _cacheNode.Value = new CacheItem(key, value);
                _collection.Remove(_cacheNode);
                _collection.AddFirst(_cacheNode);
                _cache[key] = _cacheNode;
            }
            else
            {
                var cacheItem = new CacheItem(key, value);
                _cacheNode = new LinkedListNode<CacheItem>(cacheItem);
                _cache.Add(key, _cacheNode);

                if (_cache.Count > _capacity)
                {
                    var lastNode = _collection.Last();
                    evictedCacheItem = lastNode.Value;
                    _cache.Remove(lastNode.Key);
                    _collection.RemoveLast();
                
                }
                _collection.AddFirst(_cacheNode);
            }   
        }
    }

    

    private void KeyValidation(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException($"{nameof(key)} is null or empty.");
    }

    private bool CheckIfKeyExists(string key)
    {
        return _cache.ContainsKey(key);
    }
}