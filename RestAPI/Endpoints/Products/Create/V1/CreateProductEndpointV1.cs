using Applications;
using Applications.Products;
using Applications.Products.Create;
using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Endpoints.Products.Create.V1;

public static class CreateProductEndpointV1
{
    public static RouteGroupBuilder AddCreateProductEndpointV1(this RouteGroupBuilder group)
    {
        group.MapPost("/",
                ([FromServices] IProductService productService, [FromBody] CreateProductRequest request) =>
                    productService.Create(request).ToActionResult())
            .AddEndpointFilter<ValidationFilter<CreateProductRequest>>()
            .MapToApiVersion(new ApiVersion(1, 0));

        return group;
    }
}
