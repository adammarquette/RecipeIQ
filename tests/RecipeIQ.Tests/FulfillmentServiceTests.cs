using RecipeIQ.Core.Models;
using RecipeIQ.Core.Services;
using Xunit;

namespace RecipeIQ.Tests;

public class FulfillmentServiceTests
{
    private static InMemoryStore CreateFullStore()
    {
        var store = new InMemoryStore();

        var ingredient = new Ingredient { Id = Guid.NewGuid(), Name = "Flour", Unit = "g" };
        store.Ingredients.Add(ingredient);

        var creator = new Creator { Id = Guid.NewGuid(), Name = "Chef Alice" };
        store.Creators.Add(creator);

        var recipe = new Recipe
        {
            Id = Guid.NewGuid(),
            CreatorId = creator.Id,
            Title = "Simple Bread",
            IsPublished = true,
            Servings = 4,
            Ingredients = new List<RecipeIngredient>
            {
                new() { IngredientId = ingredient.Id, Ingredient = ingredient, Quantity = 200, Unit = "g" }
            }
        };
        store.Recipes.Add(recipe);

        var retailer = new Retailer
        {
            Id = Guid.NewGuid(),
            Name = "Local Grocer",
            Region = "Northeast",
            PostalCode = "10001",
            IsActive = true,
            Inventory = new List<RetailerInventoryItem>
            {
                new() { IngredientId = ingredient.Id, Ingredient = ingredient, PricePerUnit = 0.50m, InStock = true }
            }
        };
        store.Retailers.Add(retailer);

        var cook = new HomeCook { Id = Guid.NewGuid(), Name = "Bob Cook" };
        store.HomeCooks.Add(cook);

        return store;
    }

    [Fact]
    public async Task CreateOrderAsync_CreatesOrderWithCorrectFees()
    {
        var store = CreateFullStore();
        var service = new FulfillmentService(store);

        var cook = store.HomeCooks.First();
        var recipe = store.Recipes.First();
        var retailer = store.Retailers.First();

        var order = await service.CreateOrderAsync(cook.Id, recipe.Id, retailer.Id, servings: 4);

        Assert.NotEqual(Guid.Empty, order.Id);
        Assert.Equal(OrderStatus.Pending, order.Status);
        Assert.True(order.Subtotal > 0);
        Assert.True(order.FulfillmentFee > 0);
        Assert.True(order.PlatformFee > 0);
        Assert.True(order.CreatorRoyalty > 0);
        Assert.Equal(order.Subtotal + order.FulfillmentFee + order.PlatformFee, order.Total);
    }

    [Fact]
    public async Task CreateOrderAsync_IncrementsRecipeOrderCount()
    {
        var store = CreateFullStore();
        var service = new FulfillmentService(store);
        var recipe = store.Recipes.First();
        var initialOrderCount = recipe.OrderCount;

        await service.CreateOrderAsync(
            store.HomeCooks.First().Id, recipe.Id, store.Retailers.First().Id, 4);

        Assert.Equal(initialOrderCount + 1, recipe.OrderCount);
    }

    [Fact]
    public async Task CreateOrderAsync_AccumulatesPlatformFulfillmentRevenue()
    {
        var store = CreateFullStore();
        var service = new FulfillmentService(store);

        await service.CreateOrderAsync(
            store.HomeCooks.First().Id,
            store.Recipes.First().Id,
            store.Retailers.First().Id,
            4);

        Assert.True(store.TotalFulfillmentRevenue > 0);
    }

    [Fact]
    public async Task CreateOrderAsync_PayCreatorRoyalty()
    {
        var store = CreateFullStore();
        var service = new FulfillmentService(store);
        var creator = store.Creators.First();
        var initialEarnings = creator.TotalEarnings;

        await service.CreateOrderAsync(
            store.HomeCooks.First().Id,
            store.Recipes.First().Id,
            store.Retailers.First().Id,
            4);

        Assert.True(creator.TotalEarnings > initialEarnings);
    }

    [Fact]
    public async Task CreateOrderAsync_UnknownRecipe_Throws()
    {
        var store = CreateFullStore();
        var service = new FulfillmentService(store);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.CreateOrderAsync(
                store.HomeCooks.First().Id,
                Guid.NewGuid(),
                store.Retailers.First().Id,
                4));
    }

    [Fact]
    public async Task UpdateOrderStatusAsync_DeliveredSetsTimestamp()
    {
        var store = CreateFullStore();
        var service = new FulfillmentService(store);

        var order = await service.CreateOrderAsync(
            store.HomeCooks.First().Id,
            store.Recipes.First().Id,
            store.Retailers.First().Id,
            4);

        var updated = await service.UpdateOrderStatusAsync(order.Id, OrderStatus.Delivered);

        Assert.Equal(OrderStatus.Delivered, updated.Status);
        Assert.NotNull(updated.FulfilledAt);
    }

    [Fact]
    public async Task GetRetailersForRecipeAsync_ReturnsRetailersWithAllIngredients()
    {
        var store = CreateFullStore();
        var service = new FulfillmentService(store);
        var recipe = store.Recipes.First();

        var retailers = await service.GetRetailersForRecipeAsync(recipe.Id, null);

        Assert.NotEmpty(retailers);
    }
}
