using MarqSpec.RecipeIQ.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace MarqSpec.RecipeIQ.Data;

public class RecipeIQDbContext(DbContextOptions<RecipeIQDbContext> options) : DbContext(options)
{
    public DbSet<HomeCook> HomeCooks => Set<HomeCook>();
    public DbSet<Creator> Creators => Set<Creator>();
    public DbSet<Retailer> Retailers => Set<Retailer>();
    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<SubscriptionPlan> Subscriptions => Set<SubscriptionPlan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RecipeIQDbContext).Assembly);
    }
}
