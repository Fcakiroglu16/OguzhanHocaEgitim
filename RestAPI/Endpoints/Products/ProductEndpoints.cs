using Asp.Versioning.Builder;
using Presentation.API.Endpoints.Products.Create;
using Presentation.API.Endpoints.Products.CreateWithCategory;
using Presentation.API.Endpoints.Products.Delete;
using Presentation.API.Endpoints.Products.GetAll;
using Presentation.API.Endpoints.Products.GetAllByPaged;
using Presentation.API.Endpoints.Products.Update;

namespace Presentation.API.Endpoints.Products;

public static class VersionExampleEndpoints
{
    public static void AddProductEndpoints(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        var productsGroup = app.MapGroup("api/v{version:apiVersion}/products").WithTags("Products")
            .WithApiVersionSet(apiVersionSet);

        productsGroup
            .AddGetAllProductsEndpoint()
            .AddGetAllByPagedProductsEndpoint()
            .AddCreateProductEndpoint()
            .AddCreateWithCategoryEndpoint()
            .AddUpdateProductEndpoint()
            .AddDeleteProductEndpoint();
    }
}
