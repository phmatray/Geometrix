using System.ComponentModel.DataAnnotations;
using Geometrix.WebApi.ViewModels;

namespace Geometrix.WebApi.UseCases.V1.Images.GenerateImage;

public sealed class GenerateImageResponse
{
    public GenerateImageResponse(string fileLocation, ImageModel imageModel)
    {
        FileLocation = fileLocation;
        ImageModel = imageModel;
    }
    
    [Required]
    public string FileLocation { get; }

    [Required]
    public ImageModel ImageModel { get; }
}