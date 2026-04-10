using Microsoft.AspNetCore.Mvc;
using RecipeIQ.Core.Models;
using RecipeIQ.Core.Services;

namespace RecipeIQ.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipesController : ControllerBase
{
    private readonly IRecipeDiscoveryService _discovery;
    private readonly InMemoryStore _store;

    public RecipesController(IRecipeDiscoveryService discovery, InMemoryStore store)
    {
        _discovery = discovery;
        _store = store;
    }

    /// <summary>Returns a personalized recipe feed for a home cook.</summary>
    [HttpGet("feed/{homeCookId:guid}")]
    public async Task<IActionResult> GetPersonalizedFeed(Guid homeCookId)
    {
        var recipes = await _discovery.GetPersonalizedFeedAsync(homeCookId);
        return Ok(recipes);
    }

    /// <summary>Search recipes by keyword, dietary tags, cuisine, or budget.</summary>
    [HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] string? q,
        [FromQuery] string? dietary,
        [FromQuery] string? cuisine,
        [FromQuery] decimal? maxBudget)
    {
        var dietaryTags = dietary?.Split(',', StringSplitOptions.RemoveEmptyEntries);
        var cuisineTags = cuisine?.Split(',', StringSplitOptions.RemoveEmptyEntries);
        var recipes = await _discovery.SearchRecipesAsync(q, dietaryTags, cuisineTags, maxBudget);
        return Ok(recipes);
    }

    /// <summary>Get a single recipe by ID.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var recipe = await _discovery.GetRecipeByIdAsync(id);
        return recipe is null ? NotFound() : Ok(recipe);
    }

    /// <summary>Get trending recipes by order count.</summary>
    [HttpGet("trending")]
    public async Task<IActionResult> GetTrending([FromQuery] int count = 10)
    {
        var recipes = await _discovery.GetTrendingRecipesAsync(count);
        return Ok(recipes);
    }

    /// <summary>List all home cooks (for demo/admin purposes).</summary>
    [HttpGet("/api/homecooks")]
    public IActionResult GetHomeCooks() => Ok(_store.HomeCooks);

    /// <summary>Get a home cook by ID.</summary>
    [HttpGet("/api/homecooks/{id:guid}")]
    public IActionResult GetHomeCookById(Guid id)
    {
        var cook = _store.HomeCooks.FirstOrDefault(c => c.Id == id);
        return cook is null ? NotFound() : Ok(cook);
    }

    /// <summary>Register a new home cook.</summary>
    [HttpPost("/api/homecooks")]
    public IActionResult RegisterHomeCook([FromBody] HomeCook cook)
    {
        cook.Id = cook.Id == Guid.Empty ? Guid.NewGuid() : cook.Id;
        cook.CreatedAt = DateTimeOffset.UtcNow;
        _store.HomeCooks.Add(cook);
        return CreatedAtAction(nameof(GetHomeCookById), new { id = cook.Id }, cook);
    }
}
