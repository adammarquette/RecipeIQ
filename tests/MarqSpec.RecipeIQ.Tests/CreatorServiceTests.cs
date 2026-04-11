using MarqSpec.RecipeIQ.Core.Models;
using MarqSpec.RecipeIQ.Core.Services;
using Xunit;

namespace MarqSpec.RecipeIQ.Tests;

public class CreatorServiceTests
{
    private static (InMemoryStore store, Creator creator) CreateStoreWithCreator()
    {
        var store = new InMemoryStore();
        var creator = new Creator { Id = Guid.NewGuid(), Name = "Chef Alice" };
        store.Creators.Add(creator);
        return (store, creator);
    }

    [Fact]
    public async Task PublishRecipeAsync_AssignsIdAndCreatorId()
    {
        var (store, creator) = CreateStoreWithCreator();
        var service = new CreatorService(store);

        var recipe = new Recipe { Title = "New Recipe", Servings = 2 };
        var published = await service.PublishRecipeAsync(creator.Id, recipe);

        Assert.NotEqual(Guid.Empty, published.Id);
        Assert.Equal(creator.Id, published.CreatorId);
        Assert.True(published.IsPublished);
    }

    [Fact]
    public async Task PublishRecipeAsync_AddsToStore()
    {
        var (store, creator) = CreateStoreWithCreator();
        var service = new CreatorService(store);

        await service.PublishRecipeAsync(creator.Id, new Recipe { Title = "Recipe A" });

        Assert.Single(store.Recipes);
    }

    [Fact]
    public async Task PublishRecipeAsync_UnknownCreator_ThrowsInvalidOperationException()
    {
        var store = new InMemoryStore();
        var service = new CreatorService(store);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.PublishRecipeAsync(Guid.NewGuid(), new Recipe { Title = "Recipe" }));
    }

    [Fact]
    public async Task DeleteRecipeAsync_RemovesRecipeFromStore()
    {
        var (store, creator) = CreateStoreWithCreator();
        var service = new CreatorService(store);
        var recipe = await service.PublishRecipeAsync(creator.Id, new Recipe { Title = "To Delete" });

        await service.DeleteRecipeAsync(creator.Id, recipe.Id);

        Assert.Empty(store.Recipes);
    }

    [Fact]
    public async Task DeleteRecipeAsync_UnknownRecipe_ThrowsInvalidOperationException()
    {
        var (store, creator) = CreateStoreWithCreator();
        var service = new CreatorService(store);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.DeleteRecipeAsync(creator.Id, Guid.NewGuid()));
    }

    [Fact]
    public async Task GetCreatorEarningsAsync_ReturnsCorrectAmount()
    {
        var (store, creator) = CreateStoreWithCreator();
        creator.TotalEarnings = 42.50m;
        var service = new CreatorService(store);

        var earnings = await service.GetCreatorEarningsAsync(creator.Id);

        Assert.Equal(42.50m, earnings);
    }

    [Fact]
    public async Task GetCreatorRecipesAsync_ReturnsOnlyCreatorsRecipes()
    {
        var (store, creator1) = CreateStoreWithCreator();
        var creator2 = new Creator { Id = Guid.NewGuid(), Name = "Chef Bob" };
        store.Creators.Add(creator2);

        var service = new CreatorService(store);
        await service.PublishRecipeAsync(creator1.Id, new Recipe { Title = "Alice Recipe" });
        await service.PublishRecipeAsync(creator2.Id, new Recipe { Title = "Bob Recipe" });

        var recipes = await service.GetCreatorRecipesAsync(creator1.Id);

        Assert.Single(recipes);
        Assert.All(recipes, r => Assert.Equal(creator1.Id, r.CreatorId));
    }
}
