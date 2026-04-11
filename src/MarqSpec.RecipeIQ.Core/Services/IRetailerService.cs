using MarqSpec.RecipeIQ.Core.Models;

namespace MarqSpec.RecipeIQ.Core.Services;

public interface IRetailerService
{
    Task<Retailer?> GetRetailerByIdAsync(Guid retailerId);
    Task<IEnumerable<Retailer>> GetRetailersInRegionAsync(string postalCode);
    Task UpdateInventoryAsync(Guid retailerId, IEnumerable<RetailerInventoryItem> items);
    Task<IEnumerable<RetailerInventoryItem>> GetInventoryAsync(Guid retailerId);
    Task<IEnumerable<Order>> GetPendingOrdersAsync(Guid retailerId);
    Task ConfirmOrderAsync(Guid retailerId, Guid orderId);
    Task<decimal> GetAdvertisingBudgetAsync(Guid retailerId);
}
