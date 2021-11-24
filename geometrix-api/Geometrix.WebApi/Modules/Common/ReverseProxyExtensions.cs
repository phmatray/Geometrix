using Microsoft.AspNetCore.HttpOverrides;

namespace Geometrix.WebApi.Modules.Common;

/// <summary>
///     Reverse Proxy Extensions.
/// </summary>
public static class ReverseProxyExtensions
{
    /// <summary>
    ///     Add Proxy.
    /// </summary>
    public static IServiceCollection AddProxy(this IServiceCollection services)
    {
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });

        return services;
    }

    /// <summary>
    ///     Use Proxy.
    /// </summary>
    public static IApplicationBuilder UseProxy(this IApplicationBuilder app, IConfiguration configuration)
    {
        string basePath = configuration["ASPNETCORE_BASEPATH"];
        if (!string.IsNullOrEmpty(basePath))
        {
            app.Use(async (context, next) =>
            {
                context.Request.PathBase = basePath;
                await next.Invoke()
                    .ConfigureAwait(false);
            });
        }

        app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.All });

        return app;
    }
}