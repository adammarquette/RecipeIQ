using MarqSpec.RecipeIQ.Core.Models;

namespace MarqSpec.RecipeIQ.Core.Services;

public class FulfillmentService : IFulfillmentService
{
    private const decimal PlatformFeeRate = 0.05m;
    private const decimal CreatorRoyaltyRate = 0.03m;
    private const decimal FulfillmentFeeRate = 0.08m;

    private readonly InMemoryStore _store;

    public FulfillmentService(InMemoryStore store)
    {
        _store = store;
    }

    public Task<Order> CreateOrderAsync(Guid homeCookId, Guid recipeId, Guid retailerId, int servings)
    {
        var recipe = _store.Recipes.FirstOrDefault(r => r.Id == recipeId && r.IsPublished)
            ?? throw new InvalidOperationException($"Recipe {recipeId} not found.");

        var retailer = _store.Retailers.FirstOrDefault(r => r.Id == retailerId && r.IsActive)
            ?? throw new InvalidOperationException($"Retailer {retailerId} not found.");

        var servingMultiplier = (decimal)servings / Math.Max(1, recipe.Servings);

        var items = recipe.Ingredients.Select(ri =>
        {
            var inventoryItem = retailer.Inventory.FirstOrDefault(i => i.IngredientId == ri.IngredientId);
            var unitPrice = inventoryItem?.PricePerUnit ?? 0m;
            var qty = ri.Quantity * servingMultiplier;
            return new OrderItem
            {
                IngredientId = ri.IngredientId,
                IngredientName = ri.Ingredient?.Name ?? string.Empty,
                Quantity = qty,
                Unit = ri.Unit,
                UnitPrice = unitPrice,
                LineTotal = qty * unitPrice
            };
        }).ToList();

        var subtotal = items.Sum(i => i.LineTotal);
        var fulfillmentFee = Math.Round(subtotal * FulfillmentFeeRate, 2);
        var platformFee = Math.Round(subtotal * PlatformFeeRate, 2);
        var creatorRoyalty = Math.Round(subtotal * CreatorRoyaltyRate, 2);

        var order = new Order
        {
            Id = Guid.NewGuid(),
            HomeCookId = homeCookId,
            RecipeId = recipeId,
            RetailerId = retailerId,
            Items = items,
            Subtotal = subtotal,
            FulfillmentFee = fulfillmentFee,
            PlatformFee = platformFee,
            CreatorRoyalty = creatorRoyalty,
            Total = subtotal + fulfillmentFee + platformFee,
            Status = OrderStatus.Pending
        };

        _store.Orders.Add(order);

        recipe.OrderCount++;

        // Accumulate platform revenue
        _store.TotalFulfillmentRevenue += fulfillmentFee + platformFee;

        // Pay out creator royalty
        var creator = _store.Creators.FirstOrDefault(c => c.Id == recipe.CreatorId);
        if (creator is not null)
            creator.TotalEarnings += creatorRoyalty;

        return Task.FromResult(order);
    }

    public Task<Order?> GetOrderByIdAsync(Guid orderId)
    {
        var order = _store.Orders.FirstOrDefault(o => o.Id == orderId);
        return Task.FromResult(order);
    }

    public Task<IEnumerable<Order>> GetOrdersForHomeCookAsync(Guid homeCookId)
    {
        var orders = _store.Orders.Where(o => o.HomeCookId == homeCookId).AsEnumerable();
        return Task.FromResult(orders);
    }

    public Task<Order> UpdateOrderStatusAsync(Guid orderId, OrderStatus status)
    {
        var order = _store.Orders.FirstOrDefault(o => o.Id == orderId)
            ?? throw new InvalidOperationException($"Order {orderId} not found.");

        order.Status = status;

        if (status == OrderStatus.Delivered)
            order.FulfilledAt = DateTimeOffset.UtcNow;

        return Task.FromResult(order);
    }

    public Task<IEnumerable<Retailer>> GetRetailersForRecipeAsync(Guid recipeId, string? postalCode)
    {
        var recipe = _store.Recipes.FirstOrDefault(r => r.Id == recipeId);
        if (recipe is null)
            return Task.FromResult(Enumerable.Empty<Retailer>());

        var neededIngredientIds = recipe.Ingredients.Select(i => i.IngredientId).ToHashSet();

        var retailers = _store.Retailers
            .Where(r => r.IsActive)
            .Where(r => postalCode is null || r.PostalCode == postalCode || r.Region.Contains(postalCode))
            .Where(r => neededIngredientIds.All(id =>
                r.Inventory.Any(inv => inv.IngredientId == id && inv.InStock)))
            .AsEnumerable();

        return Task.FromResult(retailers);
    }
}
