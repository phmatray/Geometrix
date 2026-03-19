using TheAppManager.Modules;

namespace Geometrix.WebApi.Modules.AppModules;

/// <summary>
///     Module for routing middleware.
/// </summary>
public class RoutingModule : IAppModule
{
    public void ConfigureMiddleware(WebApplication app)
    {
        app.UseRouting();
    }

    public void ConfigureEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapControllers();
    }
}
