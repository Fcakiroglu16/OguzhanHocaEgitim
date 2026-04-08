using Applications.Products;
using Persistences.Repositories;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


//transient> scoped > singleton

//Singleton
builder.Services.AddSingleton<TaxCalculate>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepositoryWithInMemory>();

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
