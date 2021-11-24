using Geometrix.Application.Services;
using Geometrix.Domain;
using Geometrix.Domain.Patterns;
using Geometrix.Domain.ValueObjects;

namespace Geometrix.Application.UseCases.GenerateImage;

public sealed class GenerateImageUseCase : IGenerateImageUseCase
{
    private readonly IImageCreation _imageCreationService;
    private readonly IFileStorageService _fileStorageService;
    private readonly IImageDescriptionFactory _imageDescriptionFactory;
    private IOutputPort _outputPort;

    public GenerateImageUseCase(
        IImageCreation imageCreationService,
        IFileStorageService fileStorageService,
        IImageDescriptionFactory imageDescriptionFactory)
    {
        _imageCreationService = imageCreationService;
        _fileStorageService = fileStorageService;
        _imageDescriptionFactory = imageDescriptionFactory;
        _outputPort = new GenerateImagePresenter();
    }

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
        Pattern pattern = _imageDescriptionFactory
            .NewPattern(
                mirrorPowerHorizontal, mirrorPowerVertical, cellGroupLength,
                includeEmptyAndFill, seed);

        var imageConfiguration = new Settings(
            cellWidthPixel,
            new ThemeColor(backgroundColor),
            new ThemeColor(foregroundColor));

        ImageDescription imageDescription = _imageDescriptionFactory
            .NewImage(pattern, imageConfiguration);

        return GeneratePng(imageDescription);
    }

    private async Task GeneratePng(ImageDescription imageDescription)
    {
        byte[] bytes = await _imageCreationService
            .CreateImageAsync(imageDescription);

        string? fileName = await _fileStorageService
            .SaveFileAsync(bytes, imageDescription.Id);

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