using Geometrix.WebApi.Modules.Common;
using Prometheus;
using TheAppManager.Modules;

namespace Geometrix.WebApi.Modules.AppModules;

/// <summary>
///     Module for Prometheus metrics configuration.
/// </summary>
public class MetricsModule : IAppModule
{
    public void ConfigureMiddleware(WebApplication app)
    {
        app.UseCustomHttpMetrics();
    }

    public void ConfigureEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapMetrics();
    }
}
