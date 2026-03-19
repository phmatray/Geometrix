using TheAppManager.Modules;

namespace Geometrix.WebApi.Modules.AppModules;

/// <summary>
///     Module for business use cases registration.
/// </summary>
public class UseCasesModule : IAppModule
{
    public void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddUseCases();
    }
}
