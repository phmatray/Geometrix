﻿using System.Net.Mime;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Geometrix.WebApi.Modules;

/// <summary>
///     HealthChecks Extensions.
/// </summary>
public static class HealthChecksExtensions
{
    /// <summary>
    ///     Add Health Checks dependencies varying on configuration.
    /// </summary>
    public static IServiceCollection AddHealthChecks(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var healthChecks = services.AddHealthChecks();

        //IFeatureManager featureManager = services
        //    .BuildServiceProvider()
        //    .GetRequiredService<IFeatureManager>();

        //bool isEnabled = featureManager
        //    .IsEnabledAsync(nameof(CustomFeature.SQLServer))
        //    .ConfigureAwait(false)
        //    .GetAwaiter()
        //    .GetResult();

        //if (isEnabled)
        //{
        //    healthChecks.AddDbContextCheck<MangaContext>("MangaDbContext");
        //}

        return healthChecks.Services;
    }

    /// <summary>
    ///     Use Health Checks dependencies.
    /// </summary>
    public static IApplicationBuilder UseHealthChecks(
        this IApplicationBuilder app)
    {
        app.UseHealthChecks("/health",
            new HealthCheckOptions { ResponseWriter = WriteResponse });

        return app;
    }

    private static Task WriteResponse(HttpContext context, HealthReport result)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;

        var json = new JObject(
            new JProperty("status", result.Status.ToString()),
            new JProperty("results", new JObject(result.Entries.Select(pair =>
                new JProperty(pair.Key, new JObject(
                    new JProperty("status", pair.Value.Status.ToString()),
                    new JProperty("description", pair.Value.Description),
                    new JProperty("data", new JObject(pair.Value.Data.Select(
                        p => new JProperty(p.Key, p.Value))))))))));

        return context.Response.WriteAsync(
            json.ToString(Formatting.Indented));
    }
}