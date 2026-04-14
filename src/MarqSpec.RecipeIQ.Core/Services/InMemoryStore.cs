using MarqSpec.RecipeIQ.Core.Models;

namespace MarqSpec.RecipeIQ.Core.Services;

public class InMemoryStore
{
    public List<HomeCook> HomeCooks { get; } = new();
    public List<Creator> Creators { get; } = new();
    public List<Retailer> Retailers { get; } = new();
    public List<Recipe> Recipes { get; } = new();
    public List<Ingredient> Ingredients { get; } = new();
    public List<Order> Orders { get; } = new();
    public decimal TotalFulfillmentRevenue { get; set; }
    public decimal TotalAdvertisingRevenue { get; set; }
    public decimal TotalSubscriptionRevenue { get; set; }
}
