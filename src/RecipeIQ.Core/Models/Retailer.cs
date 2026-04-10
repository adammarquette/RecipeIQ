namespace RecipeIQ.Core.Models;

public class Retailer
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string? PostalCode { get; set; }
    public string ContactEmail { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public decimal AdvertisingBudget { get; set; }
    public List<RetailerInventoryItem> Inventory { get; set; } = new();
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
