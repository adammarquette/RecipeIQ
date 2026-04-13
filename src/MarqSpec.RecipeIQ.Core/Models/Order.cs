namespace MarqSpec.RecipeIQ.Core.Models;

public class Order
{
    public Guid Id { get; set; }
    public Guid HomeCookId { get; set; }
    public Guid RecipeId { get; set; }
    public Guid RetailerId { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public decimal Subtotal { get; set; }
    public decimal FulfillmentFee { get; set; }
    public decimal PlatformFee { get; set; }
    public decimal CreatorRoyalty { get; set; }
    public decimal Total { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? FulfilledAt { get; set; }
}

public class OrderItem
{
    public Guid IngredientId { get; set; }
    public string IngredientName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }
}

public enum OrderStatus
{
    Pending,
    Confirmed,
    Preparing,
    ReadyForPickup,
    OutForDelivery,
    Fulfilled,
    Delivered,
    Cancelled
}
