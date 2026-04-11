using MarqSpec.RecipeIQ.Core.Models;

namespace MarqSpec.RecipeIQ.Core.Services;

public class RecipeDiscoveryService : IRecipeDiscoveryService
{
    private readonly InMemoryStore _store;

    public RecipeDiscoveryService(InMemoryStore store)
    {
        _store = store;
    }

    public Task<IEnumerable<Recipe>> GetPersonalizedFeedAsync(Guid homeCookId)
    {
        var cook = _store.HomeCooks.FirstOrDefault(c => c.Id == homeCookId);
        if (cook is null)
            return Task.FromResult(Enumerable.Empty<Recipe>());

        var recipes = _store.Recipes
            .Where(r => r.IsPublished)
            .Where(r => !cook.DietaryRestrictions.Any() ||
                        r.DietaryTags.Any(t => cook.DietaryRestrictions.Contains(t)) ||
                        r.DietaryTags.Count == 0)
            .Where(r => !cook.CuisinePreferences.Any() ||
                        r.CuisineTags.Any(t => cook.CuisinePreferences.Contains(t)))
            .OrderByDescending(r => r.AverageRating)
            .ThenByDescending(r => r.ViewCount)
            .AsEnumerable();

        return Task.FromResult(recipes);
    }

    public Task<IEnumerable<Recipe>> SearchRecipesAsync(
        string? query,
        IEnumerable<string>? dietaryTags,
        IEnumerable<string>? cuisineTags,
        decimal? maxBudget)
    {
        var results = _store.Recipes.Where(r => r.IsPublished).AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
        {
            var q = query.ToLowerInvariant();
            results = results.Where(r =>
                r.Title.ToLowerInvariant().Contains(q) ||
                r.Description.ToLowerInvariant().Contains(q));
        }

        if (dietaryTags?.Any() == true)
        {
            results = results.Where(r => r.DietaryTags.Any(t => dietaryTags.Contains(t)));
        }

        if (cuisineTags?.Any() == true)
        {
            results = results.Where(r => r.CuisineTags.Any(t => cuisineTags.Contains(t)));
        }

        return Task.FromResult(results.AsEnumerable());
    }

    public Task<Recipe?> GetRecipeByIdAsync(Guid recipeId)
    {
        var recipe = _store.Recipes.FirstOrDefault(r => r.Id == recipeId);
        if (recipe is not null)
            recipe.ViewCount++;
        return Task.FromResult(recipe);
    }

    public Task<IEnumerable<Recipe>> GetTrendingRecipesAsync(int count = 10)
    {
        var trending = _store.Recipes
            .Where(r => r.IsPublished)
            .OrderByDescending(r => r.OrderCount)
            .ThenByDescending(r => r.ViewCount)
            .Take(count)
            .AsEnumerable();

        return Task.FromResult(trending);
    }
}
