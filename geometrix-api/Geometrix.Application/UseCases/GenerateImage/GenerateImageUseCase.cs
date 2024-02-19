using Geometrix.Application.Services;
using Geometrix.Domain;
using Geometrix.Domain.ValueObjects;

namespace Geometrix.Application.UseCases.GenerateImage;

public sealed class GenerateImageUseCase(
    IImageCreation imageCreationService,
    IFileStorageService fileStorageService,
    IImageDescriptionFactory imageDescriptionFactory)
    : IGenerateImageUseCase
{
    private IOutputPort _outputPort = new GenerateImagePresenter();

    public Task Execute(
        int mirrorPowerHorizontal,
        int mirrorPowerVertical,
        int cellGroupLength,
        int cellWidthPixel,
        bool includeEmptyAndFill,
        int seed,
        string backgroundColor,
        string foregroundColor)
    {
        var pattern = imageDescriptionFactory
            .NewPattern(
                mirrorPowerHorizontal, mirrorPowerVertical, cellGroupLength,
                includeEmptyAndFill, seed);

        var imageConfiguration = new Settings(
            CellWidthPixel.From(cellWidthPixel),
            ThemeColor.From(backgroundColor),
            ThemeColor.From(foregroundColor));

        var imageDescription = imageDescriptionFactory
            .NewImage(pattern, imageConfiguration);

        return GeneratePng(imageDescription);
    }

    private async Task GeneratePng(ImageDescription imageDescription)
    {
        var bytes = await imageCreationService
            .CreateImageAsync(imageDescription);

        var fileName = await fileStorageService
            .SaveFileAsync(bytes, imageDescription.Id, ".png");

        if (fileName is null)
        {
            _outputPort.Invalid();
        }
        else
        {
            _outputPort.Ok(imageDescription, bytes, fileName);
        }
    }

    public void SetOutputPort(IOutputPort outputPort)
    {
        _outputPort = outputPort;
    }
}