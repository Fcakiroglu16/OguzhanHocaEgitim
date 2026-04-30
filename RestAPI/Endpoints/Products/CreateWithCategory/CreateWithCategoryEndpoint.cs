using Applications.Products;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Endpoints.Products.CreateWithCategory;

public static class CreateWithCategoryEndpoint
{
    public static RouteGroupBuilder AddCreateWithCategoryEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/create-with-category",
                ([FromServices] IProductService productService,
                    [FromBody] ProductService.CreateProductAndCategoryRequest request) =>
                    productService.CreateWithCategory(request).ToActionResult())
            .MapToApiVersion(new ApiVersion(1, 0));

        return group;
    }
}
