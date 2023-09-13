using Geometrix.WebApi.Modules;
using Geometrix.WebApi.Modules.Common;
using Geometrix.WebApi.Modules.Common.FeatureFlags;
using Geometrix.WebApi.Modules.Common.Swagger;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Prometheus;

namespace Geometrix.WebApi;

/// <summary>
///     Startup.
/// </summary>
public sealed class Startup
{
    /// <summary>
    ///     Startup constructor.
    /// </summary>
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    /// <summary>
    ///     Configure dependencies from application.
    /// </summary>
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddFeatureFlags(Configuration) // should be the first one.
            .AddInvalidRequestLogging()
            .AddHealthChecks(Configuration)
            .AddAuthentication(Configuration)
            .AddVersioning()
            .AddSwagger()
            .AddUseCases()
            .AddCustomControllers()
            .AddCustomCors()
            .AddProxy()
            .AddCustomDataProtection();

        var servicesCount = services.Count;
        Console.WriteLine($"Total services registered: {servicesCount}");
    }

    /// <summary>
    ///     Configure http request pipeline.
    /// </summary>
    public void Configure(
        IApplicationBuilder app,
        IWebHostEnvironment env,
        IApiVersionDescriptionProvider provider)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/api/V1/CustomError")
                .UseHsts();
        }

        app
            .UseStaticFiles()
            .UseProxy(Configuration)
            .UseHealthChecks()
            .UseCustomCors()
            .UseCustomHttpMetrics()
            .UseRouting()
            .UseVersionedSwagger(provider, Configuration)
            .UseAuthentication()
            .UseAuthorization()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapMetrics();
            });
    }
}