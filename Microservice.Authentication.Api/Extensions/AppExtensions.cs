﻿using Asp.Versioning;
using Asp.Versioning.Builder;
using Microservice.Authentication.Api.Middleware;

namespace Microservice.Authentication.Api.Extensions;

public static class AppExtensions
{
    public static void ConfigureSwagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var descriptions = app.DescribeApiVersions();

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

    public static ApiVersionSet GetApiVersionSet(this WebApplication app)
    {
        return app.NewApiVersionSet()
                  .HasApiVersion(new ApiVersion(1))
                  .ReportApiVersions()
                  .Build();
    }

    public static void ConfigureMiddleware(this WebApplication app)
    {
        //if (!app.Environment.IsDevelopment())
        // {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        // }
    }
}
