using System.Diagnostics;
using Applications.ActivitySource;
using Applications.Metrics;
using Applications.Products;
using Applications.Products.Create;
using Domains;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Persistences.Repositories;
using Presentation.API.Endpoints.ErrorHandlerExample;
using Presentation.API.Endpoints.Products;
using Presentation.API.Endpoints.VersionExamples;
using Presentation.API.Endpoints.WeatherForecast;
using Presentation.API.ExceptionHandler;
using Presentation.API.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
ActivitySourceProvider.ActivitySource = new ActivitySource(builder.Environment.ApplicationName);

//transient> scoped > singleton

//Singleton
builder.Services.AddSingleton<TaxCalculate>();
builder.Services.AddSingleton<GlobalMetrics>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepositoryWithInMemory>();


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


//


//builder.Services.AddOpenTelemetry()
//    .ConfigureResource(resource => resource
//        .AddService(serviceName: "RestAPI", serviceVersion: "1.0")
//        .AddAttributes(new Dictionary<string, object>
//        {
//            ["deployment.environment"] = builder.Environment.EnvironmentName
//        }))
//    .WithTracing(traceBuilder =>
//    {
//        traceBuilder.AddAspNetCoreInstrumentation();
//        traceBuilder.AddSource("Applications.ActivitySource");
//        traceBuilder.AddConsoleExporter();
//    });


var app = builder.Build();

app.MapDefaultEndpoints();

app.AddExceptionHandlerEndpoints();


app.UseExceptionHandler(exceptionApp =>
{
    //exceptionApp.Run((context) =>
    //{
    //    var exceptionHandlerPathFeature =
    //        context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
    //    var exception = exceptionHandlerPathFeature?.Error;


    //    context.Response.ContentType = "application/json";
    //    context.Response.StatusCode = 500; // Internal Server Error
    //    return context.Response.WriteAsJsonAsync(new
    //    {
    //        Message = "An unexpected error occurred."
    //    });
    //});
});


app.AddProductEndpoints(app.AddVersionSetExt());
app.AddVersionExamplesEndpoints(app.AddVersionSetExt());
app.AddFilterEndpoints();
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
