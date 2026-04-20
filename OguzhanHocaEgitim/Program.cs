using Applications.Products;
using Applications.Products.Create;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Persistences.Repositories;
using Presentation.API.Endpoints.Products;
using Presentation.API.Endpoints.VersionExamples;
using Presentation.API.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


//transient> scoped > singleton

//Singleton
builder.Services.AddSingleton<TaxCalculate>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepositoryWithInMemory>();


// IValidator<CreateProductRequest> => CreateProductRequestValidator
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductRequestValidator>();


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddVersioningExt();
var app = builder.Build();

app.UseExceptionHandler(exceptionApp =>
{
    //  exceptionApp.Run();
});


app.AddProductEndpoints(app.AddVersionSetExt());
app.AddVersionExamplesEndpoints(app.AddVersionSetExt());
app.AddFilterEndpoints();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
