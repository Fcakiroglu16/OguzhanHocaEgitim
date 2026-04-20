using Applications;
using Applications.Products;
using Applications.Products.Update;
using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Endpoints.Products.Update;

public static class VersionExample
{
    public static RouteGroupBuilder AddUpdateProductEndpoint(this RouteGroupBuilder group)
    {
        group.MapPut("/",
                ([FromServices] IProductService productService, [FromBody] UpdateProductRequest request) =>
                    productService.Update(request).ToActionResult())
            .AddEndpointFilter<ValidationFilter<UpdateProductRequest>>()
            .MapToApiVersion(new ApiVersion(1, 0));

        return group;
    }
}
