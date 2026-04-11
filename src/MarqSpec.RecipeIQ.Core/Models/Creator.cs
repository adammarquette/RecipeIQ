namespace MarqSpec.RecipeIQ.Core.Models;

public class Creator
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string? ProfileImageUrl { get; set; }
    public decimal TotalEarnings { get; set; }
    public int RecipeCount { get; set; }
    public int FollowerCount { get; set; }
    public List<Recipe> Recipes { get; set; } = new();
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
