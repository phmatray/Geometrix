using Microsoft.FeatureManagement;

namespace Geometrix.WebApi.Modules.Common.FeatureFlags;

/// <summary>
///     Feature Flags Extension.
/// </summary>
public static class FeatureFlagsExtensions
{
    /// <summary>
    ///     Add Feature Flags dependencies.
    /// </summary>
    public static IServiceCollection AddFeatureFlags(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFeatureManagement(configuration);

        var featureManager = services.BuildServiceProvider()
            .GetRequiredService<IFeatureManager>();

        services.AddMvc()
            .ConfigureApplicationPartManager(apm =>
                apm.FeatureProviders.Add(
                    new CustomControllerFeatureProvider(featureManager)));

        return services;
    }
}