using Applications.Products;
using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Endpoints.Products.Delete;

public static class DeleteProductEndpoint
{
    public static RouteGroupBuilder AddDeleteProductEndpoint(this RouteGroupBuilder group)
    {
        group.MapDelete("/{id:int}",
                ([FromServices] IProductService productService, [FromRoute] int id) =>
                    productService.Delete(id).ToActionResult())
            .MapToApiVersion(new ApiVersion(1, 0));

        return group;
    }
}
