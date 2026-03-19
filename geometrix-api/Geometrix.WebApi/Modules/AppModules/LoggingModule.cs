using Geometrix.WebApi.Modules.Common;
using TheAppManager.Modules;

namespace Geometrix.WebApi.Modules.AppModules;

/// <summary>
///     Module for invalid request logging.
/// </summary>
public class LoggingModule : IAppModule
{
    public void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddInvalidRequestLogging();
    }
}
