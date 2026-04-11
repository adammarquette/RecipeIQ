using MarqSpec.RecipeIQ.Core.Models;

namespace MarqSpec.RecipeIQ.Core.Services;

public class RetailerService : IRetailerService
{
    private readonly InMemoryStore _store;

    public RetailerService(InMemoryStore store)
    {
        _store = store;
    }

    public Task<Retailer?> GetRetailerByIdAsync(Guid retailerId)
    {
        var retailer = _store.Retailers.FirstOrDefault(r => r.Id == retailerId);
        return Task.FromResult(retailer);
    }

    public Task<IEnumerable<Retailer>> GetRetailersInRegionAsync(string postalCode)
    {
        var retailers = _store.Retailers
            .Where(r => r.IsActive && (r.PostalCode == postalCode || r.Region.Contains(postalCode)))
            .AsEnumerable();
        return Task.FromResult(retailers);
    }

    public Task UpdateInventoryAsync(Guid retailerId, IEnumerable<RetailerInventoryItem> items)
    {
        var retailer = _store.Retailers.FirstOrDefault(r => r.Id == retailerId)
            ?? throw new InvalidOperationException($"Retailer {retailerId} not found.");

        retailer.Inventory.Clear();
        retailer.Inventory.AddRange(items);

        return Task.CompletedTask;
    }

    public Task<IEnumerable<RetailerInventoryItem>> GetInventoryAsync(Guid retailerId)
    {
        var retailer = _store.Retailers.FirstOrDefault(r => r.Id == retailerId)
            ?? throw new InvalidOperationException($"Retailer {retailerId} not found.");

        return Task.FromResult(retailer.Inventory.AsEnumerable());
    }

    public Task<IEnumerable<Order>> GetPendingOrdersAsync(Guid retailerId)
    {
        var orders = _store.Orders
            .Where(o => o.RetailerId == retailerId &&
                        (o.Status == OrderStatus.Pending || o.Status == OrderStatus.Confirmed))
            .AsEnumerable();
        return Task.FromResult(orders);
    }

    public Task ConfirmOrderAsync(Guid retailerId, Guid orderId)
    {
        var order = _store.Orders.FirstOrDefault(o => o.Id == orderId && o.RetailerId == retailerId)
            ?? throw new InvalidOperationException($"Order {orderId} not found for retailer {retailerId}.");

        order.Status = OrderStatus.Confirmed;
        return Task.CompletedTask;
    }

    public Task<decimal> GetAdvertisingBudgetAsync(Guid retailerId)
    {
        var retailer = _store.Retailers.FirstOrDefault(r => r.Id == retailerId);
        return Task.FromResult(retailer?.AdvertisingBudget ?? 0m);
    }
}
