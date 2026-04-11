namespace MarqSpec.RecipeIQ.Core.Models;

public enum SubscriptionTier
{
    Free,
    Plus,
    Premium
}

public class SubscriptionPlan
{
    public SubscriptionTier Tier { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal MonthlyPrice { get; set; }
    public List<string> Features { get; set; } = new();
}
