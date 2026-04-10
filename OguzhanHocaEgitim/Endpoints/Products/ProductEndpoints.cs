using Applications.Products;
using Applications.Products.Create;
using Applications.Products.Update;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Endpoints.Products
{
    //   CalculateService.Calculate(100,20) => 120


    public static class ProductEndpoints
    {
        //public static double Calculate(this double price, double tax)
        //{
        //    return price + (price * tax) / 100;
        //}


        public static void AddProductEndpoints(this WebApplication app)
        {
            var productsGroup = app.MapGroup("minimal-api/products").WithTags("Products-Minimals");


            productsGroup.MapGet("/", ([FromServices] IProductService productService) =>
            {
                var products = productService.GetAll();

                return Results.Ok(products);
            });

            productsGroup.MapGet("/{page}/{pageSize}",
                ([FromServices] IProductService productService, int page, int pageSize) =>
                {
                    var products = productService.GetAllByPaged(page, pageSize);

                    return Results.Ok(products);
                });


            productsGroup.MapPost("/",
                ([FromServices] IProductService productService, [FromBody] CreateProductRequest request) =>
                {
                    var result = productService.Create(request);

                    return Results.Created($"minimal-api/products/{result.Data!.Id}", result);
                });

            productsGroup.MapPut("/",
                ([FromServices] IProductService productService, [FromBody] UpdateProductRequest request) =>
                {
                    var result = productService.Update(request);

                    return Results.NoContent();
                });

            productsGroup.MapDelete("/{id}",
                ([FromServices] IProductService productService, [FromRoute] int id) =>
                {
                    var result = productService.Delete(id);

                    return Results.NoContent();
                });
        }
    }
}
