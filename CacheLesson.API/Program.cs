using CacheLesson.API;
using CacheLesson.API.Services;
using Microsoft.Extensions.Caching.Hybrid;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddHybridCache(options =>
{
    options.DefaultEntryOptions = new HybridCacheEntryOptions()
    {
        Expiration = TimeSpan.FromMinutes(10),
        LocalCacheExpiration = TimeSpan.FromMinutes(5)
    };
});


builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "cache-lesson-";
});

builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<ProductServiceAsInMemoryCache>();
builder.Services.AddScoped<ProductServiceAsDistributedCache>();

builder.Services.AddSingleton<RedisService>();
builder.Services.AddSingleton<CacheService>();
var app = builder.Build();


app.MapGet("/products", async (ProductServiceAsDistributedCache productService) =>
{
    var products = await productService.GetProductsAsync();
    return Results.Ok(products);
});

app.MapPost("/products", async (ProductServiceAsDistributedCache productService, Product product) =>
{
    await productService.AddProductAsync(product);
    return Results.Ok(product);
});

app.MapGet("/add-list", async (CacheService cacheService) =>
{
    await cacheService.AddList("my-list", "item1");
    await cacheService.AddList("my-list", "item2");
    await cacheService.AddList("my-list", "item3");
    return Results.Ok("Items added to the list.");
});
app.MapGet("/add-sorted-set", async (CacheService cacheService) =>
{
    await cacheService.AddSortedSet("my-sorted-set", "item1", 1);
    await cacheService.AddSortedSet("my-sorted-set", "item2", 50);
    await cacheService.AddSortedSet("my-sorted-set", "item3", 100);
    return Results.Ok("Items added to the sorted set.");
});
app.MapGet("/get-sorted-set", async (CacheService cacheService) =>
{
    var sortedSet = await cacheService.GetSortedList("my-sorted-set");
    return Results.Ok(sortedSet);
});

app.MapGet("/add-products-to-list", async (CacheService cacheService) =>
{
    await cacheService.AddUserToHash();
    return Results.Ok("Products added to the list.");
});


app.MapGet("/set-hybrid-cache", async (HybridCache hybridCache) =>
{
    await hybridCache.SetAsync("users-count", 10);
    return Results.Ok();
});
app.MapGet("/get-hybrid-cache", async (HybridCache hybridCache) =>
{
    var count = await hybridCache.GetOrCreateAsync("users-count", async cancellationToken =>
    {
        var count = 50; // db.users.count
        return count;
    });
    return Results.Ok(count);
});


app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.Run();


