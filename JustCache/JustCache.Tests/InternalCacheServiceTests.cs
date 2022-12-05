namespace JustCache.Tests;

public class InternalCacheServiceTests
{
    [Fact]
    public void SetCapacity_CapacitySetToValue_CapacityMatchesValue()
    {
        IInternalCacheService internalCacheService = new InternalCacheService();
        internalCacheService.SetCapacity(2);
        
        Assert.Equal(2, internalCacheService.Capacity);
    }

    [Fact]
    public void SetCapacity_CapacitySetToNegative_ThrowsArgumentException()
    {
        IInternalCacheService internalCacheService = new InternalCacheService();
        Assert.Throws<ArgumentException>(() => internalCacheService.SetCapacity(-1));
    }

    [Fact]
    public void SetCapacity_CapacitySetToZero_ThrowsArgumentException()
    {
        IInternalCacheService internalCacheService = new InternalCacheService();
        Assert.Throws<ArgumentException>(() => internalCacheService.SetCapacity(0));
    }
    
    [Fact]
    public void TryGetCacheItem_NullKey_ThrowsArgumentNullException()
    {
        IInternalCacheService internalCacheService = new InternalCacheService();
        string key = null;

        Assert.Throws<ArgumentNullException>(() => internalCacheService.TryGetCacheItem(key, out object value));
    }

    [Fact]
    public void TryGetCacheItem_EmptyKey_ThrowsArgumentNullException()
    {
        IInternalCacheService internalCacheService = new InternalCacheService();
        string key = null;

        Assert.Throws<ArgumentNullException>(() => internalCacheService.TryGetCacheItem(key, out object value));
    }

    [Fact]
    public void TryGetCacheItem_KeyDoesntExist_ReturnsFalseNullValue()
    {
        IInternalCacheService internalCacheService = new InternalCacheService();
        string key = "key";
        bool returned = internalCacheService.TryGetCacheItem(key, out object value);
        Assert.False(returned);
        Assert.Null(value);
    }

    [Fact]
    public void AddCacheItem_KeyAlreadyExists_ThrowsArgumentException()
    {
        IInternalCacheService internalCacheService = new InternalCacheService();

        internalCacheService.AddCacheItem("key2", "newValue", out object evictedValue);
        string key = "key2";
        
        Assert.Throws<ArgumentException>(() => internalCacheService.AddCacheItem(key,"add",out object evictedValue));
    }

    [Fact]
    public void AddCacheItem_EmptyKey_ThrowsArgumentNullException()
    {
        IInternalCacheService internalCacheService = new InternalCacheService();
        internalCacheService.SetCapacity(1);
        string key = " ";
        
        Assert.Throws<ArgumentNullException>(() => internalCacheService.AddCacheItem(key,"add",out object 
        evictedValue));
    }

    [Fact]
    public void AddCacheItem_NullKey_ThrowsArgumentNullException()
    {
        IInternalCacheService internalCacheService = new InternalCacheService();
        internalCacheService.SetCapacity(1);
        string key = null;
        
        Assert.Throws<ArgumentNullException>(() => internalCacheService.AddCacheItem(key,"add",out object 
            evictedValue));
    }
    
    [Fact]
    public void AddCacheItem_LengthOfCacheWillExceedSoReturnsEvictedValue_EvictedValueReturned()
    {
        IInternalCacheService internalCacheService = new InternalCacheService();
        internalCacheService.SetCapacity(1);
        string key = "key";

        internalCacheService.AddCacheItem(key, "add", out object evictedValue);
        
        string key2 = "key2";
        internalCacheService.AddCacheItem(key2, "add2", out object evictedValue2);
        
        Assert.NotNull(evictedValue2);
        Assert.Equal("add", evictedValue2.ToString());
    }

    [Fact]
    public void AddCacheItem_MultipleAdds_EachEvictedItemReturned()
    {
        IInternalCacheService internalCacheService = new InternalCacheService();
        internalCacheService.SetCapacity(3);
        
        string keyOne = "keyOne";
        string keyTwo = "keyTwo";
        string keyThree = "keyThree";

        internalCacheService.AddCacheItem(keyOne, "keyOne", out object evictedValue);
        internalCacheService.AddCacheItem(keyTwo,"keyTwo", out evictedValue);
        internalCacheService.AddCacheItem(keyThree,"keyThree", out evictedValue);
        
        internalCacheService.AddCacheItem("keyFour", "newValue", out evictedValue);
        Assert.NotNull(evictedValue);
        Assert.Equal("keyOne", evictedValue.ToString());
        
        internalCacheService.AddCacheItem("keyFive","newValue", out evictedValue);
        Assert.NotNull(evictedValue);
        Assert.Equal("keyTwo", evictedValue.ToString());
    }

    [Fact]
    public void AddCacheItem_CapacityIsOneThenSetToTwoButThirdInsertWillRemoveLeastUsed_CacheCanBeAddedToAndLeastUsedEvicted()
    {
        IInternalCacheService internalCacheService = new InternalCacheService();
        internalCacheService.SetCapacity(1);
        
        string keyOne = "keyOne";
        string keyTwo = "keyTwo";
        string keyThree = "keyThree";
        internalCacheService.AddCacheItem(keyOne, "keyOne", out object evictedValue);
        Assert.Null(evictedValue);
        internalCacheService.SetCapacity(2);
        internalCacheService.AddCacheItem(keyTwo, "keyTwo", out evictedValue);
        bool returned = internalCacheService.TryGetCacheItem(keyOne, out object value);
        Assert.True(returned);
        Assert.NotNull(value);
        
        internalCacheService.AddCacheItem(keyThree, "keyThree", out evictedValue);
        
        Assert.NotNull(evictedValue);
        Assert.Equal(keyTwo, evictedValue.ToString());
    }
}