﻿using Geometrix.Application.Services;
using Geometrix.Domain;
using Geometrix.Domain.Patterns;
using Geometrix.Domain.ValueObjects;

namespace Geometrix.Infrastructure.ImageCreation;

public sealed class ImageCreationService : IImageCreation
{
    private readonly ImageEditionService _imageEdition;

    public ImageCreationService(ImageEditionService imageEdition)
    {
        _imageEdition = imageEdition;
    }

    public async Task<byte[]> CreateImageAsync(ImageDescription imageDescription)
    {
        var (pattern, settings, imageWidthPixel, imageHeightPixel) = imageDescription;
        using var image = new Image<Rgba32>(imageWidthPixel, imageHeightPixel);
        _imageEdition.EditImage(image, pattern, settings);
        return await GetImageBytes(image);
    }

    public static async Task<byte[]> GetImageBytes(Image image)
    {
        await using var memory = new MemoryStream();
        await image.SaveAsPngAsync(memory);
        return memory.ToArray();
    }
}