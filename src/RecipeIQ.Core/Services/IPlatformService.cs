using RecipeIQ.Core.Models;

namespace RecipeIQ.Core.Services;

public interface IPlatformService
{
    Task<PlatformMetrics> GetMetricsAsync();
    Task<IEnumerable<SubscriptionPlan>> GetSubscriptionPlansAsync();
    Task<HomeCook> UpgradeSubscriptionAsync(Guid homeCookId, SubscriptionTier tier);
    Task<decimal> RecordAdvertisingClickAsync(Guid retailerId, decimal amount);
}
