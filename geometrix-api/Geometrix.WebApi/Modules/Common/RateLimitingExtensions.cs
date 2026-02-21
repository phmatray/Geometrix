using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace Geometrix.WebApi.Modules.Common;

/// <summary>
///     Rate Limiting Extensions.
/// </summary>
public static class RateLimitingExtensions
{
    private const string ImagesPolicyName = "images";

    /// <summary>
    ///     Adds rate limiting with a fixed-window policy of 10 requests/minute on the "images" policy.
    /// </summary>
    public static IServiceCollection AddCustomRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter(ImagesPolicyName, limiterOptions =>
            {
                limiterOptions.PermitLimit = 10;
                limiterOptions.Window = TimeSpan.FromMinutes(1);
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                limiterOptions.QueueLimit = 0;
            });

            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });

        return services;
    }
}
