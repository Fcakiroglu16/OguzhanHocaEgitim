using Applications;
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


            productsGroup.MapGet("/",
                ([FromServices] IProductService productService) => productService.GetAll().ToActionResult());

            productsGroup.MapGet("/{page}/{pageSize}",
                ([FromServices] IProductService productService, int page, int pageSize) =>
                    productService.GetAllByPaged(page, pageSize).ToActionResult());


            productsGroup.MapPost("/",
                ([FromServices] IProductService productService, [FromBody] CreateProductRequest request) =>
                    productService.Create(request).ToActionResult()).AddEndpointFilter(async (context, next) =>
            {
                Console.WriteLine("1. filter before");

                var response = await next(context);
                Console.WriteLine("1. filter after");

                return response;
            }).AddEndpointFilter<ValidationFilter<CreateProductRequest>>();

            productsGroup.MapPut("/",
                    ([FromServices] IProductService productService, [FromBody] UpdateProductRequest request) =>
                        productService.Update(request).ToActionResult())
                .AddEndpointFilter<ValidationFilter<UpdateProductRequest>>();


            productsGroup.MapDelete("/{id:int}",
                ([FromServices] IProductService productService, [FromRoute] int id) =>
                    productService.Delete(id).ToActionResult());
        }
    }
}
