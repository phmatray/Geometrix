using Geometrix.Domain;

namespace Geometrix.Application.UseCases.GenerateImage;

public interface IOutputPort
{
    /// <summary>
    ///     Image created.
    /// </summary>
    void Ok(ImageDescription image, byte[] bytes, string fileLocation);
    
    /// <summary>
    ///     Invalid input.
    /// </summary>
    void Invalid();
}