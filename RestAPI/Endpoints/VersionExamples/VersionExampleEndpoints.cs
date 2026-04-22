using Asp.Versioning.Builder;
using Presentation.API.Endpoints.Products.Create;
using Presentation.API.Endpoints.Products.Delete;
using Presentation.API.Endpoints.Products.GetAll;
using Presentation.API.Endpoints.Products.GetAllByPaged;
using Presentation.API.Endpoints.Products.Update;

namespace Presentation.API.Endpoints.VersionExamples;

public static class VersionExampleEndpoints
{
    public static void AddVersionExamplesEndpoints(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        var productsGroup = app.MapGroup("api/version-examples").WithTags("version-examples")
            .WithApiVersionSet(apiVersionSet);

        productsGroup.AddVersionExamples();
    }
}
