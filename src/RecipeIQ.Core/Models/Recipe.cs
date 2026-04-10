namespace RecipeIQ.Core.Models;

public class Recipe
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public Creator? Creator { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int PrepTimeMinutes { get; set; }
    public int CookTimeMinutes { get; set; }
    public int Servings { get; set; }
    public DifficultyLevel Difficulty { get; set; } = DifficultyLevel.Medium;
    public List<RecipeIngredient> Ingredients { get; set; } = new();
    public List<string> Instructions { get; set; } = new();
    public List<string> DietaryTags { get; set; } = new();
    public List<string> CuisineTags { get; set; } = new();
    public List<string> MealTypeTags { get; set; } = new();
    public int ViewCount { get; set; }
    public int OrderCount { get; set; }
    public double AverageRating { get; set; }
    public int RatingCount { get; set; }
    public bool IsPublished { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}

public enum DifficultyLevel
{
    Easy,
    Medium,
    Hard
}
