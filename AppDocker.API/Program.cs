using AppDocker.API.Data;
using AppDocker.API.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.FileProviders;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))
//);

builder.AddSqlServerDbContext<AppDbContext>("sqlSever");


builder.Services.AddSingleton<IFileProvider>(serviceProvider =>
{
    var environment = serviceProvider.GetRequiredService<IWebHostEnvironment>();
    var filesPath = Path.Combine(
        environment.WebRootPath, "files");

    return new PhysicalFileProvider(filesPath);
});


builder.Services.AddDistributedMemoryCache();


builder.AddRedisDistributedCache("redis");

builder.Services.AddHttpClient<AppDocker2Service>(options =>
{
    // "appdocker2-api" matches the resource name registered in AppHost.cs and is resolved via Aspire service discovery.
    options.BaseAddress = new Uri("http://appdocker2-api");
});


var app = builder.Build();

app.MapDefaultEndpoints();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.MapOpenApi();
app.MapScalarApiReference();

app.UseStaticFiles();


app.MapGet("/check-test", async (AppDocker2Service appDocker2Service) =>
{
    var result = await appDocker2Service.CheckTest();
    return Results.Ok(result);
});


app.MapGet("/api/cache-test", (IDistributedCache distributedCache) =>
{
    distributedCache.SetString("x", "y");

    return Results.Ok();
});


app.MapGet("/api/products", (AppDbContext context) => { return Results.Ok(context.Products.ToList()); });


app.MapPost("/api/upload", async (IFormFile file, IFileProvider fileProvider, CancellationToken cancellationToken) =>
{
    if (file.Length == 0)
    {
        return Results.BadRequest("Dosya boş olamaz.");
    }

    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
    var physicalPath = fileProvider.GetFileInfo(fileName).PhysicalPath;

    if (string.IsNullOrEmpty(physicalPath))
    {
        return Results.Problem("Dosyanın kaydedileceği yol çözümlenemedi.");
    }

    await using var fileStream = new FileStream(physicalPath, FileMode.Create);


    try
    {
        await file.CopyToAsync(fileStream, cancellationToken);
    }
    catch (OperationCanceledException e)
    {
        Console.WriteLine(e);
        throw;
    }


    return Results.Created($"/files/{fileName}", new { fileName });
}).DisableAntiforgery();


app.Run();


