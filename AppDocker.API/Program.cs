using AppDocker.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))
);

builder.Services.AddSingleton<IFileProvider>(serviceProvider =>
{
    var environment = serviceProvider.GetRequiredService<IWebHostEnvironment>();
    var filesPath = Path.Combine(
        environment.WebRootPath, "files");

    return new PhysicalFileProvider(filesPath);
});


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.MapOpenApi();
app.MapScalarApiReference();

app.UseStaticFiles();


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


