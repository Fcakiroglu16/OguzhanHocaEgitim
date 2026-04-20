#region

using Asp.Versioning;
using Asp.Versioning.Builder;

#endregion

namespace Presentation.API.Extensions;

public static class VersioningExt
{
    public static IServiceCollection AddVersioningExt(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(new HeaderApiVersionReader("x-version"),
                new QueryStringApiVersionReader("version"), new UrlSegmentApiVersionReader());
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });


        return services;
    }


    public static ApiVersionSet AddVersionSetExt(this WebApplication app)
    {
        ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .HasApiVersion(new ApiVersion(2, 0))
            .HasApiVersion(new ApiVersion(2, 1))
            .HasApiVersion(new ApiVersion(3, 0))
            .ReportApiVersions()
            .Build();
        return apiVersionSet;
    }
}