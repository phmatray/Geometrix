﻿using System.Reflection;
using Geometrix.WebApi.Modules.Common.FeatureFlags;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.FeatureManagement;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Geometrix.WebApi.Modules.Common.Swagger;

/// <summary>
///     Swagger Extensions.
/// </summary>
public static class SwaggerExtensions
{
    private static string XmlCommentsFilePath
    {
        get
        {
            string basePath = PlatformServices.Default.Application.ApplicationBasePath;
            string fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
            return Path.Combine(basePath, fileName);
        }
    }

    /// <summary>
    ///     Add Swagger Configuration dependencies.
    /// </summary>
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        var featureManager = services
            .BuildServiceProvider()
            .GetRequiredService<IFeatureManager>();

        bool isEnabled = featureManager
            .IsEnabledAsync(nameof(CustomFeature.Swagger))
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();

        if (isEnabled)
        {
            services
                .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
                .AddSwaggerGen(
                    c =>
                    {
                        c.IncludeXmlComments(XmlCommentsFilePath);
                        c.AddSecurityDefinition("Bearer",
                            new OpenApiSecurityScheme
                            {
                                In = ParameterLocation.Header,
                                Description = "Please insert JWT with Bearer into field",
                                Name = "Authorization",
                                Type = SecuritySchemeType.ApiKey
                            });
                        c.AddSecurityRequirement(new OpenApiSecurityRequirement
                        {
                                {
                                    new OpenApiSecurityScheme
                                    {
                                        Reference = new OpenApiReference
                                        {
                                            Type = ReferenceType.SecurityScheme, Id = "Bearer"
                                        }
                                    },
                                    new string[] { }
                                }
                        });
                    });
        }

        return services;
    }

    /// <summary>
    ///     Add Swagger dependencies.
    /// </summary>
    public static IApplicationBuilder UseVersionedSwagger(
        this IApplicationBuilder app,
        IApiVersionDescriptionProvider provider,
        IConfiguration configuration)
    {
        app.UseSwagger();
        app.UseSwaggerUI(
            options =>
            {
                foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
                {
                    string? basePath = configuration["ASPNETCORE_BASEPATH"];

                    string swaggerEndpoint = !string.IsNullOrEmpty(basePath)
                        ? $"{basePath}/swagger/{description.GroupName}/swagger.json"
                        : $"/swagger/{description.GroupName}/swagger.json";

                    options.SwaggerEndpoint(swaggerEndpoint, description.GroupName.ToUpperInvariant());
                }
            });

        return app;
    }
}