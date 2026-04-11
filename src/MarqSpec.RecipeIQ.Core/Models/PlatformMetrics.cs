namespace MarqSpec.RecipeIQ.Core.Models;

public class PlatformMetrics
{
    public int TotalHomeCooks { get; set; }
    public int TotalCreators { get; set; }
    public int TotalRetailers { get; set; }
    public int TotalRecipes { get; set; }
    public int TotalOrders { get; set; }
    public decimal TotalFulfillmentRevenue { get; set; }
    public decimal TotalAdvertisingRevenue { get; set; }
    public decimal TotalSubscriptionRevenue { get; set; }
    public decimal TotalRevenue => TotalFulfillmentRevenue + TotalAdvertisingRevenue + TotalSubscriptionRevenue;
    public DateTimeOffset AsOf { get; set; } = DateTimeOffset.UtcNow;
}
