using RecipeIQ.Core.Models;

namespace RecipeIQ.Core.Services;

public class PlatformService : IPlatformService
{
    private readonly InMemoryStore _store;

    public PlatformService(InMemoryStore store)
    {
        _store = store;
    }

    public Task<PlatformMetrics> GetMetricsAsync()
    {
        var metrics = new PlatformMetrics
        {
            TotalHomeCooks = _store.HomeCooks.Count,
            TotalCreators = _store.Creators.Count,
            TotalRetailers = _store.Retailers.Count,
            TotalRecipes = _store.Recipes.Count(r => r.IsPublished),
            TotalOrders = _store.Orders.Count,
            TotalFulfillmentRevenue = _store.TotalFulfillmentRevenue,
            TotalAdvertisingRevenue = _store.TotalAdvertisingRevenue,
            TotalSubscriptionRevenue = _store.TotalSubscriptionRevenue,
            AsOf = DateTimeOffset.UtcNow
        };
        return Task.FromResult(metrics);
    }

    public Task<IEnumerable<SubscriptionPlan>> GetSubscriptionPlansAsync()
    {
        var plans = new List<SubscriptionPlan>
        {
            new()
            {
                Tier = SubscriptionTier.Free,
                Name = "Free",
                Description = "Get started with RecipeIQ at no cost.",
                MonthlyPrice = 0m,
                Features = new List<string>
                {
                    "Browse public recipes",
                    "Basic dietary filters",
                    "5 saved recipes"
                }
            },
            new()
            {
                Tier = SubscriptionTier.Plus,
                Name = "Plus",
                Description = "Enhanced personalization and unlimited saves.",
                MonthlyPrice = 4.99m,
                Features = new List<string>
                {
                    "Personalized recipe feed",
                    "All dietary filters",
                    "Unlimited saved recipes",
                    "Household & budget matching",
                    "One-tap ingredient ordering"
                }
            },
            new()
            {
                Tier = SubscriptionTier.Premium,
                Name = "Premium",
                Description = "The full RecipeIQ experience.",
                MonthlyPrice = 9.99m,
                Features = new List<string>
                {
                    "Everything in Plus",
                    "Exclusive creator content",
                    "Meal planning calendar",
                    "Nutritional insights",
                    "Priority fulfillment",
                    "Ad-free experience"
                }
            }
        };

        return Task.FromResult(plans.AsEnumerable());
    }

    public async Task<HomeCook> UpgradeSubscriptionAsync(Guid homeCookId, SubscriptionTier tier)
    {
        var cook = _store.HomeCooks.FirstOrDefault(c => c.Id == homeCookId)
            ?? throw new InvalidOperationException($"HomeCook {homeCookId} not found.");

        cook.Subscription = tier;

        // Record subscription revenue (simplified: charge monthly fee once)
        var plans = await GetSubscriptionPlansAsync();
        var plan = plans.FirstOrDefault(p => p.Tier == tier);
        if (plan is not null)
            _store.TotalSubscriptionRevenue += plan.MonthlyPrice;

        return cook;
    }

    public Task<decimal> RecordAdvertisingClickAsync(Guid retailerId, decimal amount)
    {
        _store.TotalAdvertisingRevenue += amount;

        var retailer = _store.Retailers.FirstOrDefault(r => r.Id == retailerId);
        if (retailer is not null)
            retailer.AdvertisingBudget = Math.Max(0, retailer.AdvertisingBudget - amount);

        return Task.FromResult(amount);
    }
}
