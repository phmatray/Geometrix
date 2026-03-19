using TheAppManager.Modules;

namespace Geometrix.WebApi.Modules.AppModules;

/// <summary>
///     Module for health checks configuration.
/// </summary>
public class HealthChecksModule : IAppModule
{
    public void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks(builder.Configuration);
    }

    public void ConfigureMiddleware(WebApplication app)
    {
        app.UseHealthChecks();
    }
}
