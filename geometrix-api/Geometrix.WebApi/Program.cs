using Geometrix.WebApi.Modules;
using Geometrix.WebApi.Modules.Common;
using Geometrix.WebApi.Modules.Common.FeatureFlags;
using Geometrix.WebApi.Modules.Common.Swagger;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;
var env = builder.Environment;

// Add Aspire services.
builder.AddServiceDefaults();

// Add other services.
services
    .AddFeatureFlags(configuration) // should be the first one.
    .AddInvalidRequestLogging()
    .AddHealthChecks(configuration)
    .AddAuthentication(configuration)
    .AddVersioning()
    .AddSwagger()
    .AddUseCases()
    .AddCustomControllers()
    .AddCustomCors()
    .AddProxy()
    .AddCustomDataProtection();

var servicesCount = services.Count;
Console.WriteLine($"Total services registered: {servicesCount}");

var app = builder.Build();

if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/api/V1/CustomError")
        .UseHsts();
}

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app
    .UseStaticFiles()
    .UseProxy(configuration)
    .UseHealthChecks()
    .UseCustomCors()
    .UseCustomHttpMetrics()
    .UseRouting()
    .UseVersionedSwagger(provider, configuration)
    .UseAuthentication()
    .UseAuthorization()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapMetrics();
    });


app.Run();