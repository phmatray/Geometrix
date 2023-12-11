using Geometrix.Domain;

namespace Geometrix.Application.UseCases.GenerateImage;

/// <summary>
/// Represents an output port for image processing.
/// </summary>
public interface IOutputPort
{
    /// <summary>
    /// Image created.
    /// </summary>
    void Ok(ImageDescription image, byte[] bytes, string fileLocation);
    
    /// <summary>
    /// Invalid input.
    /// </summary>
    void Invalid();
}