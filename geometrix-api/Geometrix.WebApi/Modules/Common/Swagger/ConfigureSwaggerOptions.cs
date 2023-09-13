using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Geometrix.WebApi.Modules.Common.Swagger;

/// <summary>
///     Configures the Swagger generation options.
/// </summary>
/// <remarks>
///     This allows API versioning to define a Swagger document per API version after the
///     <see cref="IApiVersionDescriptionProvider" /> service has been resolved from the service container.
/// </remarks>
public sealed class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private const string UriString = "https://paulovich.net/";

    private const string UriString1 =
        "https://raw.githubusercontent.com/ivanpaulovich/clean-architecture-manga/master/README.md";

    private readonly IApiVersionDescriptionProvider _provider;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ConfigureSwaggerOptions" /> class.
    /// </summary>
    /// <param name="provider">
    ///     The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger
    ///     documents.
    /// </param>
    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

    /// <inheritdoc />
    public void Configure(SwaggerGenOptions options)
    {
        // add a swagger document for each discovered API version
        // note: you might choose to skip or document deprecated API versions differently
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = "Geometrix API",
            Version = description.ApiVersion.ToString(),
            Description = "Clean Architecture, DDD and TDD implementation.",
            Contact = new OpenApiContact { Name = "Philippe Matray", Email = "philippe@atypical.consulting" },
            TermsOfService = new Uri(UriString),
            License = new OpenApiLicense
            {
                Name = "Apache License",
                Url = new Uri(
                    UriString1)
            }
        };

        if (description.IsDeprecated)
        {
            info.Description += " This API version has been deprecated.";
        }

        return info;
    }
}