using TheAppManager.Modules;

namespace Geometrix.WebApi.Modules.AppModules;

/// <summary>
///     Module for core middleware pipeline configuration including
///     error handling, static files, and routing.
/// </summary>
public class MiddlewarePipelineModule : IAppModule
{
    public void ConfigureMiddleware(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/api/V1/CustomError");
            app.UseHsts();
        }

        // Redirect root to Swagger
        app.MapGet("/", () => Results.Redirect("/swagger"));

        app.UseStaticFiles();
    }
}
