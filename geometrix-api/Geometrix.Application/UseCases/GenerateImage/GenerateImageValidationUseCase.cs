using Geometrix.Application.Services;
using Geometrix.Domain.ValueObjects;

namespace Geometrix.Application.UseCases.GenerateImage;

public sealed class GenerateImageValidationUseCase(
    IGenerateImageUseCase useCase,
    Notification notification)
    : IGenerateImageUseCase
{
    private IOutputPort _outputPort = new GenerateImagePresenter();

    public void SetOutputPort(IOutputPort outputPort)
    {
        _outputPort = outputPort;
        useCase.SetOutputPort(outputPort);
    }

    public async Task Execute(
        int mirrorPowerHorizontal,
        int mirrorPowerVertical,
        int cellGroupLength,
        int cellWidthPixel,
        bool includeEmptyAndFill,
        int seed,
        string backgroundColor,
        string foregroundColor)
    {
        if (mirrorPowerHorizontal is < 1 or > 4)
        {
            notification
                .Add(nameof(mirrorPowerHorizontal), "MirrorPowerHorizontal is required.");
        }
     
        if (mirrorPowerVertical is < 1 or > 4)
        {
            notification
                .Add(nameof(mirrorPowerVertical), "MirrorPowerVertical is required.");
        }

        if (cellGroupLength is < 2 or > 8)
        {
            notification
                .Add(nameof(cellGroupLength), "CellGroupLength is required.");
        }

        if (cellWidthPixel is < 32 or > 256)
        {
            notification
                .Add(nameof(cellWidthPixel), "CellWidthPixel is required.");
        }

        if (seed is < 0 or > 100000)
        {
            notification
                .Add(nameof(seed), "Seed is required.");
        }

        if (backgroundColor != ThemeColor.Light.Value &&
            backgroundColor != ThemeColor.Dark.Value &&
            backgroundColor != ThemeColor.Red.Value &&
            backgroundColor != ThemeColor.Yellow.Value &&
            backgroundColor != ThemeColor.Green.Value &&
            backgroundColor != ThemeColor.Blue.Value &&
            backgroundColor != ThemeColor.Indigo.Value &&
            backgroundColor != ThemeColor.Purple.Value &&
            backgroundColor != ThemeColor.Pink.Value)
        {
            notification
                .Add(nameof(backgroundColor), "BackgroundColor is required.");
        }

        if (foregroundColor != ThemeColor.Light.Value &&
            foregroundColor != ThemeColor.Dark.Value &&
            foregroundColor != ThemeColor.Red.Value &&
            foregroundColor != ThemeColor.Yellow.Value &&
            foregroundColor != ThemeColor.Green.Value &&
            foregroundColor != ThemeColor.Blue.Value &&
            foregroundColor != ThemeColor.Indigo.Value &&
            foregroundColor != ThemeColor.Purple.Value &&
            foregroundColor != ThemeColor.Pink.Value)
        {
            notification
                .Add(nameof(backgroundColor), "ForegroundColor is required.");
        }

        if (notification.IsInvalid)
        {
            _outputPort.Invalid();
            return;
        }

        await useCase
            .Execute(
                mirrorPowerHorizontal, mirrorPowerVertical, cellGroupLength,
                cellWidthPixel, includeEmptyAndFill, seed,
                backgroundColor, foregroundColor)
            .ConfigureAwait(false);
    }
}