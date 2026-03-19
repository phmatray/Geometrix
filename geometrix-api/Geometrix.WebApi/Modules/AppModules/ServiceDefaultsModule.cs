using TheAppManager.Modules;

namespace Geometrix.WebApi.Modules.AppModules;

/// <summary>
///     Module for Aspire service defaults.
/// </summary>
public class ServiceDefaultsModule : IAppModule
{
    public void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();
    }
}
