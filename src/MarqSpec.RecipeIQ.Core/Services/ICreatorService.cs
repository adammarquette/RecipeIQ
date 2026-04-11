using MarqSpec.RecipeIQ.Core.Models;

namespace MarqSpec.RecipeIQ.Core.Services;

public interface ICreatorService
{
    Task<Creator?> GetCreatorByIdAsync(Guid creatorId);
    Task<Recipe> PublishRecipeAsync(Guid creatorId, Recipe recipe);
    Task<Recipe> UpdateRecipeAsync(Guid creatorId, Recipe recipe);
    Task DeleteRecipeAsync(Guid creatorId, Guid recipeId);
    Task<IEnumerable<Recipe>> GetCreatorRecipesAsync(Guid creatorId);
    Task<decimal> GetCreatorEarningsAsync(Guid creatorId);
    Task<IEnumerable<DemandSignal>> GetDemandSignalsAsync(Guid creatorId);
}

public class DemandSignal
{
    public string Tag { get; set; } = string.Empty;
    public int SearchVolume { get; set; }
    public double GrowthRate { get; set; }
    public int UnfulfilledDemand { get; set; }
}
