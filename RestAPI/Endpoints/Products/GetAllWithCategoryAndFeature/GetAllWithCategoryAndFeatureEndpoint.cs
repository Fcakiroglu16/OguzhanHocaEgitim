using Applications.Products;
using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Endpoints.Products.GetAllWithCategoryAndFeature;

public static class GetAllWithCategoryAndFeatureEndpoint
{
    public static RouteGroupBuilder AddGetAllWithCategoryAndFeatureEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/with-category-and-feature",
                ([FromServices] IProductService productService) =>
                    productService.GetAllWithCategoryAndFeature().ToActionResult())
            .MapToApiVersion(new ApiVersion(1, 0));

        return group;
    }
}
