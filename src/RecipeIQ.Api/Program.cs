using RecipeIQ.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "RecipeIQ API",
        Version = "v1",
        Description = "Personalized home cooking platform — four-sided marketplace connecting home cooks, creators, retailers, and the platform."
    });
});

builder.Services.AddControllers();

// Register the shared in-memory store as a singleton (acts as the database for this implementation)
builder.Services.AddSingleton<InMemoryStore>();

// Register platform services
builder.Services.AddScoped<IRecipeDiscoveryService, RecipeDiscoveryService>();
builder.Services.AddScoped<ICreatorService, CreatorService>();
builder.Services.AddScoped<IRetailerService, RetailerService>();
builder.Services.AddScoped<IFulfillmentService, FulfillmentService>();
builder.Services.AddScoped<IPlatformService, PlatformService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "RecipeIQ API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
