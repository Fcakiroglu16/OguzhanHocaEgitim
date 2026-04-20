using Asp.Versioning.Builder;
using Presentation.API.Endpoints.Products.Create.V1;
using Presentation.API.Endpoints.Products.Create.V2;
using Presentation.API.Endpoints.Products.Create.V2_1;

namespace Presentation.API.Endpoints.Products.Create;

public static class CreateProductEndpoint
{
    public static RouteGroupBuilder AddCreateProductEndpoint(this RouteGroupBuilder group)
    {
        group
            .AddCreateProductEndpointV1()
            .AddCreateProductEndpointV2()
            .AddCreateProductEndpointV2_1();

        return group;
    }
}
