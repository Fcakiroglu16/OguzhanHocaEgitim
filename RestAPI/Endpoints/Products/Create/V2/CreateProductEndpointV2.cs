using Applications;
using Applications.Products;
using Applications.Products.Create;
using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Endpoints.Products.Create.V2;

public static class CreateProductEndpointV2
{
    public static RouteGroupBuilder AddCreateProductEndpointV2(this RouteGroupBuilder group)
    {
        group.MapPost("/",
                ([FromServices] IProductService productService, [FromBody] CreateProductRequest request) =>
                    Results.Ok("2.0 version post işlemi için"))
            .AddEndpointFilter<ValidationFilter<CreateProductRequest>>()
            .MapToApiVersion(new ApiVersion(2, 0));

        return group;
    }
}
