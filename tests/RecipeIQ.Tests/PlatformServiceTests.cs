using RecipeIQ.Core.Models;
using RecipeIQ.Core.Services;
using Xunit;

namespace RecipeIQ.Tests;

public class PlatformServiceTests
{
    [Fact]
    public async Task GetMetricsAsync_ReflectsStoreState()
    {
        var store = new InMemoryStore();
        store.HomeCooks.Add(new HomeCook { Id = Guid.NewGuid() });
        store.Creators.Add(new Creator { Id = Guid.NewGuid() });
        store.Retailers.Add(new Retailer { Id = Guid.NewGuid() });
        store.Recipes.Add(new Recipe { Id = Guid.NewGuid(), IsPublished = true });
        store.TotalFulfillmentRevenue = 100m;
        store.TotalAdvertisingRevenue = 50m;
        store.TotalSubscriptionRevenue = 25m;

        var service = new PlatformService(store);
        var metrics = await service.GetMetricsAsync();

        Assert.Equal(1, metrics.TotalHomeCooks);
        Assert.Equal(1, metrics.TotalCreators);
        Assert.Equal(1, metrics.TotalRetailers);
        Assert.Equal(1, metrics.TotalRecipes);
        Assert.Equal(175m, metrics.TotalRevenue);
    }

    [Fact]
    public async Task GetSubscriptionPlansAsync_ReturnsAllThreeTiers()
    {
        var service = new PlatformService(new InMemoryStore());

        var plans = (await service.GetSubscriptionPlansAsync()).ToList();

        Assert.Equal(3, plans.Count);
        Assert.Contains(plans, p => p.Tier == SubscriptionTier.Free);
        Assert.Contains(plans, p => p.Tier == SubscriptionTier.Plus);
        Assert.Contains(plans, p => p.Tier == SubscriptionTier.Premium);
    }

    [Fact]
    public async Task UpgradeSubscriptionAsync_UpdatesCookTier()
    {
        var store = new InMemoryStore();
        var cook = new HomeCook { Id = Guid.NewGuid(), Subscription = SubscriptionTier.Free };
        store.HomeCooks.Add(cook);

        var service = new PlatformService(store);
        var updated = await service.UpgradeSubscriptionAsync(cook.Id, SubscriptionTier.Premium);

        Assert.Equal(SubscriptionTier.Premium, updated.Subscription);
    }

    [Fact]
    public async Task UpgradeSubscriptionAsync_AccumulatesSubscriptionRevenue()
    {
        var store = new InMemoryStore();
        var cook = new HomeCook { Id = Guid.NewGuid(), Subscription = SubscriptionTier.Free };
        store.HomeCooks.Add(cook);

        var service = new PlatformService(store);
        await service.UpgradeSubscriptionAsync(cook.Id, SubscriptionTier.Plus);

        Assert.True(store.TotalSubscriptionRevenue > 0);
    }

    [Fact]
    public async Task UpgradeSubscriptionAsync_UnknownHomeCook_Throws()
    {
        var service = new PlatformService(new InMemoryStore());

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.UpgradeSubscriptionAsync(Guid.NewGuid(), SubscriptionTier.Plus));
    }

    [Fact]
    public async Task RecordAdvertisingClickAsync_AccumulatesAdvertisingRevenue()
    {
        var store = new InMemoryStore();
        var retailer = new Retailer { Id = Guid.NewGuid(), AdvertisingBudget = 100m };
        store.Retailers.Add(retailer);

        var service = new PlatformService(store);
        await service.RecordAdvertisingClickAsync(retailer.Id, 1.50m);

        Assert.Equal(1.50m, store.TotalAdvertisingRevenue);
        Assert.Equal(98.50m, retailer.AdvertisingBudget);
    }
}
