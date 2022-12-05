using System.Diagnostics;
using Xunit.Abstractions;

namespace JustCache.Tests;

public class JCacheTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public JCacheTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    [Fact]
    public void Get_EmptyKey_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => JCache.Instance.GetItem("", out object itemReturnedFromCache));
    }

    [Fact]
    public void Get_NullKey_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => JCache.Instance.GetItem(null, out object itemReturnedFromCache));
    }

    [Fact]
    public void GetItem_ItemDoesNotExist_NullReturned()
    {
        string key = "testkey";
        bool returned = JCache.Instance.GetItem(key,  out object itemReturnedFromCache);
        
        Assert.Null(itemReturnedFromCache);
        Assert.False(returned);
    }

    [Fact]
    public void AddItem_KeyExists_ThrowsArgumentException()
    {
        string key = "testkey";
        JCache.Instance.AddItem(key,"testkey", out object evictedFromCache);
        Assert.Throws<ArgumentException>(() => JCache.Instance.AddItem(key, "testkey", out evictedFromCache));
    }

    [Fact]
    public void AddItem_ItemAdded_ItemCanBeReturned()
    {
        string key = "testkey1";
        JCache.Instance.AddItem(key,"testkey1", out object evictedFromCache);
        bool returned = JCache.Instance.GetItem(key, out object cacheItem);
        Assert.True(returned);
        Assert.NotNull(cacheItem);
        Assert.Equal("testkey1", cacheItem);
    }

    // [Fact]
    // public void PerformanceCheck()
    // {
    //     var dict = new Dictionary<string, int>();
    //     var sw = new Stopwatch();
    //     sw.Start();
    //     JCache.Instance.Capacity = 1000000;
    //     for (int i = 0; i < 1000000; i++)
    //     {
    //         string key = i.ToString();
    //         dict.Add(i.ToString(),i);
    //         JCache.Instance.AddItem(key, i, out object evictedCacheItem);
    //         Assert.Null(evictedCacheItem);
    //     }
    //
    //
    //     foreach (var kvp in dict)
    //     {
    //         bool returned = JCache.Instance.GetItem(kvp.Key, out object value);
    //         Assert.True(returned);
    //     }
    //     
    //     sw.Stop();
    //
    //     _testOutputHelper.WriteLine(sw.ElapsedMilliseconds.ToString());
    // }
}