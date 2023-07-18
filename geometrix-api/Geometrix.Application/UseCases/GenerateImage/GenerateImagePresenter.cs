using Geometrix.Domain;

namespace Geometrix.Application.UseCases.GenerateImage;

public sealed class GenerateImagePresenter : IOutputPort
{
    public ImageDescription? Image { get; private set; }
    public byte[]? Bytes { get; private set; }
    public string? FileLocation { get; private set; }
    public bool? InvalidOutput { get; private set; }

    public void Ok(ImageDescription image, byte[] bytes, string fileLocation)
    {
        Image = image;
        Bytes = bytes;
        FileLocation = fileLocation;
    }

    public void Invalid()
    {
        InvalidOutput = true;
    }
}