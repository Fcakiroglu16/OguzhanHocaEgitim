using Applications.Products;
using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Endpoints.Products.GetAllByPaged;

public static class GetAllByPagedProductsEndpoint
{
    public static RouteGroupBuilder AddGetAllByPagedProductsEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/{page}/{pageSize}",
                ([FromServices] IProductService productService, int page, int pageSize) =>
                    productService.GetAllByPaged(page, pageSize).ToActionResult())
            .MapToApiVersion(new ApiVersion(1, 0));

        return group;
    }
}
