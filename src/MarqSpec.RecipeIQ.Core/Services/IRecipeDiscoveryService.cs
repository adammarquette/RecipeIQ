using MarqSpec.RecipeIQ.Core.Models;

namespace MarqSpec.RecipeIQ.Core.Services;

public interface IRecipeDiscoveryService
{
    Task<IEnumerable<Recipe>> GetPersonalizedFeedAsync(Guid homeCookId);
    Task<IEnumerable<Recipe>> SearchRecipesAsync(string? query, IEnumerable<string>? dietaryTags, IEnumerable<string>? cuisineTags, decimal? maxBudget);
    Task<Recipe?> GetRecipeByIdAsync(Guid recipeId);
    Task<IEnumerable<Recipe>> GetTrendingRecipesAsync(int count = 10);
}
