using Geometrix.WebApi.Modules.Common.Swagger;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using TheAppManager.Modules;

namespace Geometrix.WebApi.Modules.AppModules;

/// <summary>
///     Module for Swagger/OpenAPI configuration.
/// </summary>
public class SwaggerModule : IAppModule
{
    public void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddSwagger();
    }

    public void ConfigureMiddleware(WebApplication app)
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        app.UseVersionedSwagger(provider, app.Configuration);
    }
}
