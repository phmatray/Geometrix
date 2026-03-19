using Geometrix.WebApi.Modules.Common;
using TheAppManager.Modules;

namespace Geometrix.WebApi.Modules.AppModules;

/// <summary>
///     Module for reverse proxy configuration.
/// </summary>
public class ProxyModule : IAppModule
{
    public void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddProxy();
    }

    public void ConfigureMiddleware(WebApplication app)
    {
        app.UseProxy(app.Configuration);
    }
}
