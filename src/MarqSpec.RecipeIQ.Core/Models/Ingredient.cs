namespace MarqSpec.RecipeIQ.Core.Models;

public class Ingredient
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public string? Category { get; set; }
    public List<string> DietaryTags { get; set; } = new();
}

public class RecipeIngredient
{
    public Guid IngredientId { get; set; }
    public Ingredient? Ingredient { get; set; }
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public string? Notes { get; set; }
}

public class RetailerInventoryItem
{
    public Guid IngredientId { get; set; }
    public Ingredient? Ingredient { get; set; }
    public decimal PricePerUnit { get; set; }
    public bool InStock { get; set; } = true;
    public string? ProductUrl { get; set; }
}
