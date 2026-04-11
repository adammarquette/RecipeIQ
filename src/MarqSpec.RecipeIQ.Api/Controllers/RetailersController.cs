using Microsoft.AspNetCore.Mvc;
using MarqSpec.RecipeIQ.Core.Models;
using MarqSpec.RecipeIQ.Core.Services;

namespace MarqSpec.RecipeIQ.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RetailersController : ControllerBase
{
    private readonly IRetailerService _retailerService;
    private readonly InMemoryStore _store;

    public RetailersController(IRetailerService retailerService, InMemoryStore store)
    {
        _retailerService = retailerService;
        _store = store;
    }

    /// <summary>List all retailers.</summary>
    [HttpGet]
    public IActionResult GetAll() => Ok(_store.Retailers);

    /// <summary>Register a new regional retailer.</summary>
    [HttpPost]
    public IActionResult Register([FromBody] Retailer retailer)
    {
        retailer.Id = retailer.Id == Guid.Empty ? Guid.NewGuid() : retailer.Id;
        retailer.CreatedAt = DateTimeOffset.UtcNow;
        _store.Retailers.Add(retailer);
        return CreatedAtAction(nameof(GetById), new { id = retailer.Id }, retailer);
    }

    /// <summary>Get a retailer by ID.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var retailer = await _retailerService.GetRetailerByIdAsync(id);
        return retailer is null ? NotFound() : Ok(retailer);
    }

    /// <summary>Find retailers in a region by postal code.</summary>
    [HttpGet("region/{postalCode}")]
    public async Task<IActionResult> GetByRegion(string postalCode)
    {
        var retailers = await _retailerService.GetRetailersInRegionAsync(postalCode);
        return Ok(retailers);
    }

    /// <summary>Update retailer inventory.</summary>
    [HttpPut("{retailerId:guid}/inventory")]
    public async Task<IActionResult> UpdateInventory(
        Guid retailerId,
        [FromBody] List<RetailerInventoryItem> items)
    {
        try
        {
            await _retailerService.UpdateInventoryAsync(retailerId, items);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>Get retailer inventory.</summary>
    [HttpGet("{retailerId:guid}/inventory")]
    public async Task<IActionResult> GetInventory(Guid retailerId)
    {
        try
        {
            var inventory = await _retailerService.GetInventoryAsync(retailerId);
            return Ok(inventory);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>Get pending orders for a retailer to fulfill.</summary>
    [HttpGet("{retailerId:guid}/orders/pending")]
    public async Task<IActionResult> GetPendingOrders(Guid retailerId)
    {
        var orders = await _retailerService.GetPendingOrdersAsync(retailerId);
        return Ok(orders);
    }

    /// <summary>Confirm (accept) an order for fulfillment.</summary>
    [HttpPost("{retailerId:guid}/orders/{orderId:guid}/confirm")]
    public async Task<IActionResult> ConfirmOrder(Guid retailerId, Guid orderId)
    {
        try
        {
            await _retailerService.ConfirmOrderAsync(retailerId, orderId);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
