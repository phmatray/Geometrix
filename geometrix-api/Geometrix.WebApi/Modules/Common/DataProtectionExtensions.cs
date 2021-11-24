using Microsoft.AspNetCore.DataProtection;

namespace Geometrix.WebApi.Modules.Common;

/// <summary>
///     Data Protection Extensions.
/// </summary>
public static class DataProtectionExtensions
{
    /// <summary>
    ///     Add Data Protection.
    /// </summary>
    public static IServiceCollection AddCustomDataProtection(this IServiceCollection services)
    {
        services.AddDataProtection()
            .SetApplicationName("geometrix-api")
            .PersistKeysToFileSystem(new DirectoryInfo(@"./"));

        return services;
    }
}