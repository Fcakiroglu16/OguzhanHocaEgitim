using Applications.Products;
using Applications.Products.Create;
using Applications.Products.Update;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Endpoints.Products
{
    public static class FilterEndpoints
    {
        //public static double Calculate(this double price, double tax)
        //{
        //    return price + (price * tax) / 100;
        //}


        public static void AddFilterEndpoints(this WebApplication app)
        {
            var productsGroup = app.MapGroup("minimal-api/filter-endpoints").WithTags("filter-Minimals");


            productsGroup.MapPost("/",
                ([FromBody] CreateProductRequest request) => Results.Ok()).AddEndpointFilter(async (context, next) =>
            {
                Console.WriteLine("1. filter before");

                var response = await next(context);
                Console.WriteLine("1. filter after");

                return response;
            }).AddEndpointFilter(async (context, next) =>
            {
                Console.WriteLine("2. filter before");
                var response = await next(context);

                Console.WriteLine("2. filter after");
                return response;
            });
        }
    }
}
