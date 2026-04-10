using RecipeIQ.Core.Models;

namespace RecipeIQ.Core.Services;

public interface IFulfillmentService
{
    Task<Order> CreateOrderAsync(Guid homeCookId, Guid recipeId, Guid retailerId, int servings);
    Task<Order?> GetOrderByIdAsync(Guid orderId);
    Task<IEnumerable<Order>> GetOrdersForHomeCookAsync(Guid homeCookId);
    Task<Order> UpdateOrderStatusAsync(Guid orderId, OrderStatus status);
    Task<IEnumerable<Retailer>> GetRetailersForRecipeAsync(Guid recipeId, string? postalCode);
}
