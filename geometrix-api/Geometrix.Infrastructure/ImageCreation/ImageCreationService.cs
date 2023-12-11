using Geometrix.Application.Services;
using Geometrix.Domain;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Geometrix.Infrastructure.ImageCreation;

public sealed class ImageCreationService(ImageEditionService imageEdition)
    : IImageCreation
{
    public async Task<byte[]> CreateImageAsync(ImageDescription imageDescription)
    {
        var (pattern, settings, imageWidthPixel, imageHeightPixel) = imageDescription;
        using var image = new Image<Rgba32>(imageWidthPixel, imageHeightPixel);
        imageEdition.EditImage(image, pattern, settings);
        return await GetImageBytes(image);
    }

    public static async Task<byte[]> GetImageBytes(Image image)
    {
        await using var memory = new MemoryStream();
        await image.SaveAsPngAsync(memory);
        return memory.ToArray();
    }
}