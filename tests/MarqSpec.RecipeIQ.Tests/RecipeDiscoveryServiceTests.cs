using MarqSpec.RecipeIQ.Core.Models;
using MarqSpec.RecipeIQ.Core.Services;
using Xunit;

namespace MarqSpec.RecipeIQ.Tests;

public class RecipeDiscoveryServiceTests
{
    private static InMemoryStore CreateStoreWithSeedData()
    {
        var store = new InMemoryStore();

        var creator = new Creator { Id = Guid.NewGuid(), Name = "Chef Alice" };
        store.Creators.Add(creator);

        var recipe1 = new Recipe
        {
            Id = Guid.NewGuid(),
            CreatorId = creator.Id,
            Title = "Vegan Tacos",
            Description = "Delicious plant-based tacos",
            IsPublished = true,
            DietaryTags = new List<string> { "vegan", "gluten-free" },
            CuisineTags = new List<string> { "Mexican" },
            MealTypeTags = new List<string> { "dinner" },
            AverageRating = 4.8,
            ViewCount = 500,
            OrderCount = 50,
            Servings = 2
        };

        var recipe2 = new Recipe
        {
            Id = Guid.NewGuid(),
            CreatorId = creator.Id,
            Title = "Chicken Pasta",
            Description = "Creamy chicken pasta",
            IsPublished = true,
            DietaryTags = new List<string> { "gluten-free" },
            CuisineTags = new List<string> { "Italian" },
            MealTypeTags = new List<string> { "dinner" },
            AverageRating = 4.2,
            ViewCount = 300,
            OrderCount = 30,
            Servings = 4
        };

        var unpublished = new Recipe
        {
            Id = Guid.NewGuid(),
            CreatorId = creator.Id,
            Title = "Draft Recipe",
            IsPublished = false
        };

        store.Recipes.AddRange(new[] { recipe1, recipe2, unpublished });

        var cook = new HomeCook
        {
            Id = Guid.NewGuid(),
            Name = "Bob Cook",
            DietaryRestrictions = new List<string> { "vegan" },
            CuisinePreferences = new List<string> { "Mexican" }
        };
        store.HomeCooks.Add(cook);

        return store;
    }

    [Fact]
    public async Task GetPersonalizedFeedAsync_ReturnsOnlyPublishedRecipes()
    {
        var store = CreateStoreWithSeedData();
        var service = new RecipeDiscoveryService(store);
        var cook = store.HomeCooks.First();

        var feed = await service.GetPersonalizedFeedAsync(cook.Id);

        Assert.All(feed, r => Assert.True(r.IsPublished));
    }

    [Fact]
    public async Task GetPersonalizedFeedAsync_UnknownHomeCook_ReturnsEmpty()
    {
        var store = CreateStoreWithSeedData();
        var service = new RecipeDiscoveryService(store);

        var feed = await service.GetPersonalizedFeedAsync(Guid.NewGuid());

        Assert.Empty(feed);
    }

    [Fact]
    public async Task SearchRecipesAsync_ByKeyword_ReturnsMatchingRecipes()
    {
        var store = CreateStoreWithSeedData();
        var service = new RecipeDiscoveryService(store);

        var results = await service.SearchRecipesAsync("Taco", null, null, null);

        Assert.Single(results);
        Assert.Contains(results, r => r.Title.Contains("Taco"));
    }

    [Fact]
    public async Task SearchRecipesAsync_ByDietaryTag_ReturnsMatchingRecipes()
    {
        var store = CreateStoreWithSeedData();
        var service = new RecipeDiscoveryService(store);

        var results = await service.SearchRecipesAsync(null, new[] { "vegan" }, null, null);

        Assert.All(results, r => Assert.Contains("vegan", r.DietaryTags));
    }

    [Fact]
    public async Task SearchRecipesAsync_ByCuisineTag_ReturnsMatchingRecipes()
    {
        var store = CreateStoreWithSeedData();
        var service = new RecipeDiscoveryService(store);

        var results = await service.SearchRecipesAsync(null, null, new[] { "Italian" }, null);

        Assert.All(results, r => Assert.Contains("Italian", r.CuisineTags));
    }

    [Fact]
    public async Task GetRecipeByIdAsync_ExistingRecipe_IncreasesViewCount()
    {
        var store = CreateStoreWithSeedData();
        var service = new RecipeDiscoveryService(store);
        var recipe = store.Recipes.First(r => r.IsPublished);
        var initialViews = recipe.ViewCount;

        await service.GetRecipeByIdAsync(recipe.Id);

        Assert.Equal(initialViews + 1, recipe.ViewCount);
    }

    [Fact]
    public async Task GetRecipeByIdAsync_UnknownId_ReturnsNull()
    {
        var store = CreateStoreWithSeedData();
        var service = new RecipeDiscoveryService(store);

        var recipe = await service.GetRecipeByIdAsync(Guid.NewGuid());

        Assert.Null(recipe);
    }

    [Fact]
    public async Task GetTrendingRecipesAsync_ReturnsMostOrderedFirst()
    {
        var store = CreateStoreWithSeedData();
        var service = new RecipeDiscoveryService(store);

        var trending = (await service.GetTrendingRecipesAsync()).ToList();

        Assert.True(trending.Count >= 1);
        // First recipe should have more orders than second
        if (trending.Count >= 2)
            Assert.True(trending[0].OrderCount >= trending[1].OrderCount);
    }
}
