namespace JustCache.Models;

public class CacheItem
{
    public string Key { get; }
    public object Value { get; }

    public CacheItem(string key, object value)
    {
        Key = key;
        Value = value;
    }
}
