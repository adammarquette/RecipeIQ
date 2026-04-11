namespace MarqSpec.RecipeIQ.Core.Models;

public class HomeCook
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int HouseholdSize { get; set; }
    public decimal WeeklyBudget { get; set; }
    public List<string> DietaryRestrictions { get; set; } = new();
    public List<string> CuisinePreferences { get; set; } = new();
    public SubscriptionTier Subscription { get; set; } = SubscriptionTier.Free;
    public string? PostalCode { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
