using Microsoft.AspNetCore.Mvc;
using MarqSpec.RecipeIQ.Core.Models;
using MarqSpec.RecipeIQ.Core.Services;

namespace MarqSpec.RecipeIQ.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatformController : ControllerBase
{
    private readonly IPlatformService _platform;

    public PlatformController(IPlatformService platform)
    {
        _platform = platform;
    }

    /// <summary>Get platform-wide metrics (revenue, users, recipes, orders).</summary>
    [HttpGet("metrics")]
    public async Task<IActionResult> GetMetrics()
    {
        var metrics = await _platform.GetMetricsAsync();
        return Ok(metrics);
    }

    /// <summary>List available subscription plans.</summary>
    [HttpGet("subscriptions")]
    public async Task<IActionResult> GetSubscriptionPlans()
    {
        var plans = await _platform.GetSubscriptionPlansAsync();
        return Ok(plans);
    }

    /// <summary>Upgrade a home cook's subscription tier.</summary>
    [HttpPost("subscriptions/{homeCookId:guid}")]
    public async Task<IActionResult> UpgradeSubscription(
        Guid homeCookId,
        [FromBody] UpgradeSubscriptionRequest request)
    {
        try
        {
            var cook = await _platform.UpgradeSubscriptionAsync(homeCookId, request.Tier);
            return Ok(cook);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>Record an advertising click event (spend from retailer budget).</summary>
    [HttpPost("advertising/click")]
    public async Task<IActionResult> RecordAdvertisingClick([FromBody] AdvertisingClickRequest request)
    {
        var amount = await _platform.RecordAdvertisingClickAsync(request.RetailerId, request.Amount);
        return Ok(new { recorded = amount });
    }
}

public class UpgradeSubscriptionRequest
{
    public SubscriptionTier Tier { get; set; }
}

public class AdvertisingClickRequest
{
    public Guid RetailerId { get; set; }
    public decimal Amount { get; set; }
}
