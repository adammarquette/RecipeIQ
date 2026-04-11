using Microsoft.AspNetCore.Mvc;
using RecipeIQ.Core.Models;
using RecipeIQ.Core.Services;

namespace RecipeIQ.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IFulfillmentService _fulfillment;

    public OrdersController(IFulfillmentService fulfillment)
    {
        _fulfillment = fulfillment;
    }

    /// <summary>Create an order for ingredients needed to make a recipe.</summary>
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        try
        {
            var order = await _fulfillment.CreateOrderAsync(
                request.HomeCookId, request.RecipeId, request.RetailerId, request.Servings);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>Get an order by ID.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var order = await _fulfillment.GetOrderByIdAsync(id);
        return order is null ? NotFound() : Ok(order);
    }

    /// <summary>Get all orders for a home cook.</summary>
    [HttpGet("homecook/{homeCookId:guid}")]
    public async Task<IActionResult> GetForHomeCook(Guid homeCookId)
    {
        var orders = await _fulfillment.GetOrdersForHomeCookAsync(homeCookId);
        return Ok(orders);
    }

    /// <summary>Update order status (e.g., mark as delivered).</summary>
    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateOrderStatusRequest request)
    {
        try
        {
            var order = await _fulfillment.UpdateOrderStatusAsync(id, request.Status);
            return Ok(order);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>Find retailers that can fulfill all ingredients for a recipe.</summary>
    [HttpGet("retailers-for-recipe/{recipeId:guid}")]
    public async Task<IActionResult> GetRetailersForRecipe(Guid recipeId, [FromQuery] string? postalCode)
    {
        var retailers = await _fulfillment.GetRetailersForRecipeAsync(recipeId, postalCode);
        return Ok(retailers);
    }
}

public class CreateOrderRequest
{
    public Guid HomeCookId { get; set; }
    public Guid RecipeId { get; set; }
    public Guid RetailerId { get; set; }
    public int Servings { get; set; } = 4;
}

public class UpdateOrderStatusRequest
{
    public OrderStatus Status { get; set; }
}
