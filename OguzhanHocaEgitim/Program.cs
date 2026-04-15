using Applications.Products;
using Applications.Products.Create;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Persistences.Repositories;
using Presentation.API.Endpoints.Products;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


//transient> scoped > singleton

//Singleton
builder.Services.AddSingleton<TaxCalculate>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepositoryWithInMemory>();


// IValidator<CreateProductRequest> => CreateProductRequestValidator
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductRequestValidator>();


//N-Layer
//Clean Architecture-Onion Architecture


// Add services to the container.


// DI Container  / IoC Container => Library

//Dependency Inversion + Inversion of Control => Dependency Injection ( pattern )


//ProductController(high level) => ProductService(low level)
//ProductService(high level) => ProductRepository(low level)


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();


app.AddProductEndpoints();
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
