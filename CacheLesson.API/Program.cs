using CacheLesson.API;
using CacheLesson.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "cache-lesson-";
});

builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<ProductServiceAsInMemoryCache>();
builder.Services.AddScoped<ProductServiceAsDistributedCache>();

builder.Services.AddSingleton<RedisService>();
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
app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.Run();


