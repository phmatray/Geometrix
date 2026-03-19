using Geometrix.WebApi.Modules.Common;
using TheAppManager.Modules;

namespace Geometrix.WebApi.Modules.AppModules;

/// <summary>
///     Module for rate limiting configuration.
/// </summary>
public class RateLimitingModule : IAppModule
{
    public void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddCustomRateLimiting();
    }

    public void ConfigureMiddleware(WebApplication app)
    {
        app.UseRateLimiter();
    }
}
