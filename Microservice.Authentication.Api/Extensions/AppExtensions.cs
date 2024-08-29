using Asp.Versioning;
using Asp.Versioning.Builder;
using Microservice.Authentication.Api.Middleware;

namespace Microservice.Authentication.Api.Extensions;

public static class AppExtensions
{
    public static void ConfigureSwagger(this WebApplication webApplication)
    {
        if (webApplication.Environment.IsDevelopment())
        {
            webApplication.UseSwagger();
            webApplication.UseSwaggerUI(options =>
            {
                var descriptions = webApplication.DescribeApiVersions();

                // Build a swagger endpoint for each discovered API version
                foreach (var description in descriptions)
                {
                    var url = $"/swagger/{description.GroupName}/swagger.json";
                    var name = description.GroupName.ToUpperInvariant();
                    options.SwaggerEndpoint(url, name);
                }
            });
        }
    }

    public static ApiVersionSet GetApiVersionSet(this WebApplication webApplication)
    {
        return webApplication.NewApiVersionSet()
                  .HasApiVersion(new ApiVersion(1))
                  .ReportApiVersions()
                  .Build();
    }

    public static void ConfigureMiddleware(this WebApplication webApplication)
    {
        if (!webApplication.Environment.IsDevelopment())
        {
            webApplication.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
