using Applications;
using Applications.Products;
using Applications.Products.Update;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Endpoints.VersionExamples;

public static class VersionExample
{
    public static RouteGroupBuilder AddVersionExamples(this RouteGroupBuilder group)
    {
        group.MapGet("/", () => Results.Ok("1. version")).MapToApiVersion(new ApiVersion(1, 0));
        group.MapGet("/", () => Results.Ok("2. version")).MapToApiVersion(new ApiVersion(2, 0));
        group.MapGet("/", () => Results.Ok("3. version")).MapToApiVersion(new ApiVersion(3, 0));

        return group;
    }
}
