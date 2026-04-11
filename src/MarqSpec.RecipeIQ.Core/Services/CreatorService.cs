using MarqSpec.RecipeIQ.Core.Models;

namespace MarqSpec.RecipeIQ.Core.Services;

public class CreatorService : ICreatorService
{
    private readonly InMemoryStore _store;

    public CreatorService(InMemoryStore store)
    {
        _store = store;
    }

    public Task<Creator?> GetCreatorByIdAsync(Guid creatorId)
    {
        var creator = _store.Creators.FirstOrDefault(c => c.Id == creatorId);
        return Task.FromResult(creator);
    }

    public Task<Recipe> PublishRecipeAsync(Guid creatorId, Recipe recipe)
    {
        if (!_store.Creators.Any(c => c.Id == creatorId))
            throw new InvalidOperationException($"Creator {creatorId} not found.");

        recipe.Id = recipe.Id == Guid.Empty ? Guid.NewGuid() : recipe.Id;
        recipe.CreatorId = creatorId;
        recipe.IsPublished = true;
        recipe.CreatedAt = DateTimeOffset.UtcNow;
        recipe.UpdatedAt = DateTimeOffset.UtcNow;

        _store.Recipes.Add(recipe);

        var creator = _store.Creators.First(c => c.Id == creatorId);
        creator.Recipes.Add(recipe);
        creator.RecipeCount++;

        return Task.FromResult(recipe);
    }

    public Task<Recipe> UpdateRecipeAsync(Guid creatorId, Recipe recipe)
    {
        var existing = _store.Recipes.FirstOrDefault(r => r.Id == recipe.Id && r.CreatorId == creatorId)
            ?? throw new InvalidOperationException($"Recipe {recipe.Id} not found for creator {creatorId}.");

        var index = _store.Recipes.IndexOf(existing);
        recipe.CreatorId = creatorId;
        recipe.UpdatedAt = DateTimeOffset.UtcNow;
        _store.Recipes[index] = recipe;

        return Task.FromResult(recipe);
    }

    public Task DeleteRecipeAsync(Guid creatorId, Guid recipeId)
    {
        var recipe = _store.Recipes.FirstOrDefault(r => r.Id == recipeId && r.CreatorId == creatorId)
            ?? throw new InvalidOperationException($"Recipe {recipeId} not found for creator {creatorId}.");

        _store.Recipes.Remove(recipe);

        var creator = _store.Creators.FirstOrDefault(c => c.Id == creatorId);
        if (creator is not null)
        {
            creator.Recipes.RemoveAll(r => r.Id == recipeId);
            creator.RecipeCount = Math.Max(0, creator.RecipeCount - 1);
        }

        return Task.CompletedTask;
    }

    public Task<IEnumerable<Recipe>> GetCreatorRecipesAsync(Guid creatorId)
    {
        var recipes = _store.Recipes
            .Where(r => r.CreatorId == creatorId)
            .AsEnumerable();
        return Task.FromResult(recipes);
    }

    public Task<decimal> GetCreatorEarningsAsync(Guid creatorId)
    {
        var creator = _store.Creators.FirstOrDefault(c => c.Id == creatorId);
        return Task.FromResult(creator?.TotalEarnings ?? 0m);
    }

    public Task<IEnumerable<DemandSignal>> GetDemandSignalsAsync(Guid creatorId)
    {
        // Aggregate search and order data to surface demand opportunities
        var recipeTags = _store.Recipes
            .Where(r => r.IsPublished)
            .SelectMany(r => r.DietaryTags.Concat(r.CuisineTags).Concat(r.MealTypeTags))
            .GroupBy(t => t)
            .Select(g => new DemandSignal
            {
                Tag = g.Key,
                SearchVolume = g.Count() * 10,
                GrowthRate = 0.05 * g.Count(),
                UnfulfilledDemand = Math.Max(0, g.Count() * 3 -
                    _store.Recipes.Count(r => r.IsPublished && r.CreatorId == creatorId &&
                        (r.DietaryTags.Contains(g.Key) || r.CuisineTags.Contains(g.Key) || r.MealTypeTags.Contains(g.Key))))
            })
            .OrderByDescending(d => d.UnfulfilledDemand)
            .AsEnumerable();

        return Task.FromResult(recipeTags);
    }
}
