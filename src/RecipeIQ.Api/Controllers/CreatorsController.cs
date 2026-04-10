using Microsoft.AspNetCore.Mvc;
using RecipeIQ.Core.Models;
using RecipeIQ.Core.Services;

namespace RecipeIQ.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CreatorsController : ControllerBase
{
    private readonly ICreatorService _creatorService;
    private readonly InMemoryStore _store;

    public CreatorsController(ICreatorService creatorService, InMemoryStore store)
    {
        _creatorService = creatorService;
        _store = store;
    }

    /// <summary>List all creators.</summary>
    [HttpGet]
    public IActionResult GetAll() => Ok(_store.Creators);

    /// <summary>Register a new creator account.</summary>
    [HttpPost]
    public IActionResult Register([FromBody] Creator creator)
    {
        creator.Id = creator.Id == Guid.Empty ? Guid.NewGuid() : creator.Id;
        creator.CreatedAt = DateTimeOffset.UtcNow;
        _store.Creators.Add(creator);
        return CreatedAtAction(nameof(GetById), new { id = creator.Id }, creator);
    }

    /// <summary>Get creator profile.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var creator = await _creatorService.GetCreatorByIdAsync(id);
        return creator is null ? NotFound() : Ok(creator);
    }

    /// <summary>Publish a new recipe as a creator.</summary>
    [HttpPost("{creatorId:guid}/recipes")]
    public async Task<IActionResult> PublishRecipe(Guid creatorId, [FromBody] Recipe recipe)
    {
        try
        {
            var published = await _creatorService.PublishRecipeAsync(creatorId, recipe);
            return CreatedAtAction(nameof(GetRecipes), new { creatorId }, published);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>Update an existing recipe.</summary>
    [HttpPut("{creatorId:guid}/recipes/{recipeId:guid}")]
    public async Task<IActionResult> UpdateRecipe(Guid creatorId, Guid recipeId, [FromBody] Recipe recipe)
    {
        recipe.Id = recipeId;
        try
        {
            var updated = await _creatorService.UpdateRecipeAsync(creatorId, recipe);
            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>Delete a recipe.</summary>
    [HttpDelete("{creatorId:guid}/recipes/{recipeId:guid}")]
    public async Task<IActionResult> DeleteRecipe(Guid creatorId, Guid recipeId)
    {
        try
        {
            await _creatorService.DeleteRecipeAsync(creatorId, recipeId);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>List all recipes by a creator.</summary>
    [HttpGet("{creatorId:guid}/recipes")]
    public async Task<IActionResult> GetRecipes(Guid creatorId)
    {
        var recipes = await _creatorService.GetCreatorRecipesAsync(creatorId);
        return Ok(recipes);
    }

    /// <summary>Get total earnings for a creator.</summary>
    [HttpGet("{creatorId:guid}/earnings")]
    public async Task<IActionResult> GetEarnings(Guid creatorId)
    {
        var earnings = await _creatorService.GetCreatorEarningsAsync(creatorId);
        return Ok(new { creatorId, earnings });
    }

    /// <summary>Get demand signals to help creators decide what to publish next.</summary>
    [HttpGet("{creatorId:guid}/demand-signals")]
    public async Task<IActionResult> GetDemandSignals(Guid creatorId)
    {
        var signals = await _creatorService.GetDemandSignalsAsync(creatorId);
        return Ok(signals);
    }
}
