using Applications.Products;
using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Endpoints.Products.GetAll;

public static class GetAllProductsEndpoint
{
    public static RouteGroupBuilder AddGetAllProductsEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/",
                ([FromServices] IProductService productService) => productService.GetAll().ToActionResult())
            .MapToApiVersion(new ApiVersion(1, 0));

        return group;
    }
}
