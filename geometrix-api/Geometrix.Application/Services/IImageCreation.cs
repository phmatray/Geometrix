using Geometrix.Domain;

namespace Geometrix.Application.Services;

public interface IImageCreation
{
    Task<byte[]> CreateImageAsync(ImageDescription imageDescription);
}