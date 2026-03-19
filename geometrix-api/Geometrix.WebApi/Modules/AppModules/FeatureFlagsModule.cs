using Geometrix.WebApi.Modules.Common.FeatureFlags;
using TheAppManager.Modules;

namespace Geometrix.WebApi.Modules.AppModules;

/// <summary>
///     Module for feature flags configuration. Should be registered first.
/// </summary>
public class FeatureFlagsModule : IAppModule
{
    public void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddFeatureFlags(builder.Configuration);
    }
}
