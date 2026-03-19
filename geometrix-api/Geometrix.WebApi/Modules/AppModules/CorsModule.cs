using Geometrix.WebApi.Modules.Common;
using TheAppManager.Modules;

namespace Geometrix.WebApi.Modules.AppModules;

/// <summary>
///     Module for CORS configuration.
/// </summary>
public class CorsModule : IAppModule
{
    public void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddCustomCors();
    }

    public void ConfigureMiddleware(WebApplication app)
    {
        app.UseCustomCors();
    }
}
