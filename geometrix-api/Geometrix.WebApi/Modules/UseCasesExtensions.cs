using Geometrix.Application.Services;
using Geometrix.Application.UseCases.GenerateImage;
using Geometrix.Domain;
using Geometrix.Infrastructure.DataAccess;
using Geometrix.Infrastructure.FileStorage;
using Geometrix.Infrastructure.ImageCreation;

namespace Geometrix.WebApi.Modules;

/// <summary>
///     Adds Use Cases classes.
/// </summary>
public static class UseCasesExtensions
{
    /// <summary>
    ///     Adds Use Cases to the ServiceCollection.
    /// </summary>
    /// <param name="services">Service Collection.</param>
    /// <returns>The modified instance.</returns>
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<Notification, Notification>();
        services.AddScoped<TriangleService, TriangleService>();
        services.AddScoped<ImageEditionService, ImageEditionService>();
        
        services.AddScoped<IFileStorageService, FileStorageService>();
        services.AddScoped<IImageDescriptionFactory, EntityDescriptionFactory>();
        services.AddScoped<IImageCreation, ImageCreationService>();

        // use cases
        services.AddScoped<IGenerateImageUseCase, GenerateImageUseCase>();
        services.Decorate<IGenerateImageUseCase, GenerateImageValidationUseCase>();

        return services;
    }
}