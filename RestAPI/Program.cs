using System.Diagnostics;
using Applications.ActivitySource;
using Applications.Metrics;
using Applications.Products;
using Applications.Products.Create;
using Domains;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Persistences;
using Persistences.Repositories;
using Presentation.API.Endpoints.ErrorHandlerExample;
using Presentation.API.Endpoints.Products;
using Presentation.API.Endpoints.VersionExamples;
using Presentation.API.Endpoints.WeatherForecast;
using Presentation.API.ExceptionHandler;
using Presentation.API.Extensions;
using Presentation.API.Middleware;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddPersistenceExt(builder.Configuration);


builder.AddServiceDefaults();
ActivitySourceProvider.ActivitySource = new ActivitySource(builder.Environment.ApplicationName);

//transient> scoped > singleton

//Singleton
builder.Services.AddSingleton<TaxCalculate>();
builder.Services.AddSingleton<GlobalMetrics>();
builder.Services.AddScoped<IProductService, ProductService>();


// IValidator<CreateProductRequest> => CreateProductRequestValidator
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductRequestValidator>();


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddVersioningExt();

builder.Services.AddExceptionHandler<BusinessExceptionHandler>().AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddHttpClient("webapplication2-api", client =>
{
    client.BaseAddress = new Uri("https+http://webapplication2-api");
});


var app = builder.Build();

app.MapPost("/upload", async (IFormFile file, CancellationToken WritecancellationToken) =>
{

    
    try
    {
        await File.WriteAllTextAsync("path/to/file.txt", "content", WritecancellationToken);
    }
    catch (OperationCanceledException e)
    {
        Console.WriteLine(e);
        throw;
    }
    catch (Exception ex)
    {

    }

    return Results.Ok();
}


app.UseMiddleware<LogScopeMiddleware>();
app.MapDefaultEndpoints();

app.AddExceptionHandlerEndpoints();


app.UseExceptionHandler(exceptionApp => { });


app.AddProductEndpoints(app.AddVersionSetExt());
app.AddVersionExamplesEndpoints(app.AddVersionSetExt());
app.AddWeatherForecastEndpoints();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
