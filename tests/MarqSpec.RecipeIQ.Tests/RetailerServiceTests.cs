using MarqSpec.RecipeIQ.Core.Models;
using MarqSpec.RecipeIQ.Core.Services;
using Xunit;

namespace MarqSpec.RecipeIQ.Tests;

public class RetailerServiceTests
{
    private static (InMemoryStore store, Retailer retailer) CreateStoreWithRetailer(
        string postalCode = "12345",
        string region = "Northwest")
    {
        var store = new InMemoryStore();
        var retailer = new Retailer
        {
            Id = Guid.NewGuid(),
            Name = "Fresh Mart",
            Region = region,
            PostalCode = postalCode,
            IsActive = true
        };
        store.Retailers.Add(retailer);
        return (store, retailer);
    }

    [Fact]
    public async Task GetRetailerByIdAsync_KnownId_ReturnsRetailer()
    {
        var (store, retailer) = CreateStoreWithRetailer();
        var service = new RetailerService(store);

        var result = await service.GetRetailerByIdAsync(retailer.Id);

        Assert.NotNull(result);
        Assert.Equal(retailer.Id, result.Id);
    }

    [Fact]
    public async Task GetRetailerByIdAsync_UnknownId_ReturnsNull()
    {
        var store = new InMemoryStore();
        var service = new RetailerService(store);

        var result = await service.GetRetailerByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetRetailersInRegionAsync_MatchingPostalCode_ReturnsRetailer()
    {
        var (store, retailer) = CreateStoreWithRetailer(postalCode: "98101");
        var service = new RetailerService(store);

        var results = (await service.GetRetailersInRegionAsync("98101")).ToList();

        Assert.Single(results);
        Assert.Equal(retailer.Id, results[0].Id);
    }

    [Fact]
    public async Task GetRetailersInRegionAsync_InactiveRetailer_IsExcluded()
    {
        var (store, retailer) = CreateStoreWithRetailer(postalCode: "98101");
        retailer.IsActive = false;
        var service = new RetailerService(store);

        var results = await service.GetRetailersInRegionAsync("98101");

        Assert.Empty(results);
    }

    [Fact]
    public async Task UpdateInventoryAsync_ReplacesExistingInventory()
    {
        var (store, retailer) = CreateStoreWithRetailer();
        retailer.Inventory.Add(new RetailerInventoryItem { Name = "Old Item" });
        var service = new RetailerService(store);

        var newItems = new List<RetailerInventoryItem>
        {
            new() { Name = "Tomato", PricePerUnit = 0.99m },
            new() { Name = "Onion", PricePerUnit = 0.49m }
        };

        await service.UpdateInventoryAsync(retailer.Id, newItems);

        var inventory = (await service.GetInventoryAsync(retailer.Id)).ToList();
        Assert.Equal(2, inventory.Count);
        Assert.Contains(inventory, i => i.Name == "Tomato");
    }

    [Fact]
    public async Task UpdateInventoryAsync_UnknownRetailer_ThrowsInvalidOperationException()
    {
        var store = new InMemoryStore();
        var service = new RetailerService(store);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.UpdateInventoryAsync(Guid.NewGuid(), []));
    }

    [Fact]
    public async Task GetInventoryAsync_UnknownRetailer_ThrowsInvalidOperationException()
    {
        var store = new InMemoryStore();
        var service = new RetailerService(store);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.GetInventoryAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task GetPendingOrdersAsync_ReturnsPendingAndConfirmedOrders()
    {
        var (store, retailer) = CreateStoreWithRetailer();
        store.Orders.Add(new Order { Id = Guid.NewGuid(), RetailerId = retailer.Id, Status = OrderStatus.Pending });
        store.Orders.Add(new Order { Id = Guid.NewGuid(), RetailerId = retailer.Id, Status = OrderStatus.Confirmed });
        store.Orders.Add(new Order { Id = Guid.NewGuid(), RetailerId = retailer.Id, Status = OrderStatus.Fulfilled });
        var service = new RetailerService(store);

        var results = (await service.GetPendingOrdersAsync(retailer.Id)).ToList();

        Assert.Equal(2, results.Count);
        Assert.All(results, o => Assert.True(o.Status == OrderStatus.Pending || o.Status == OrderStatus.Confirmed));
    }

    [Fact]
    public async Task ConfirmOrderAsync_SetsStatusToConfirmed()
    {
        var (store, retailer) = CreateStoreWithRetailer();
        var order = new Order { Id = Guid.NewGuid(), RetailerId = retailer.Id, Status = OrderStatus.Pending };
        store.Orders.Add(order);
        var service = new RetailerService(store);

        await service.ConfirmOrderAsync(retailer.Id, order.Id);

        Assert.Equal(OrderStatus.Confirmed, order.Status);
    }

    [Fact]
    public async Task ConfirmOrderAsync_UnknownOrder_ThrowsInvalidOperationException()
    {
        var (store, retailer) = CreateStoreWithRetailer();
        var service = new RetailerService(store);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.ConfirmOrderAsync(retailer.Id, Guid.NewGuid()));
    }

    [Fact]
    public async Task GetAdvertisingBudgetAsync_KnownRetailer_ReturnsBudget()
    {
        var (store, retailer) = CreateStoreWithRetailer();
        retailer.AdvertisingBudget = 500m;
        var service = new RetailerService(store);

        var budget = await service.GetAdvertisingBudgetAsync(retailer.Id);

        Assert.Equal(500m, budget);
    }

    [Fact]
    public async Task GetAdvertisingBudgetAsync_UnknownRetailer_ReturnsZero()
    {
        var store = new InMemoryStore();
        var service = new RetailerService(store);

        var budget = await service.GetAdvertisingBudgetAsync(Guid.NewGuid());

        Assert.Equal(0m, budget);
    }
}
