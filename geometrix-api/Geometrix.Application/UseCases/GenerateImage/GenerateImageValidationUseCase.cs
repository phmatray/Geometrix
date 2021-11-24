using Geometrix.Application.Services;
using Geometrix.Domain.ValueObjects;

namespace Geometrix.Application.UseCases.GenerateImage;

public sealed class GenerateImageValidationUseCase : IGenerateImageUseCase
{
    private readonly IGenerateImageUseCase _useCase;
    private readonly Notification _notification;
    private IOutputPort _outputPort;

    public GenerateImageValidationUseCase(
        IGenerateImageUseCase useCase,
        Notification notification)
    {
        _useCase = useCase;
        _notification = notification;
        _outputPort = new GenerateImagePresenter();
    }

    public void SetOutputPort(IOutputPort outputPort)
    {
        _outputPort = outputPort;
        _useCase.SetOutputPort(outputPort);
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
            _notification
                .Add(nameof(mirrorPowerHorizontal), "MirrorPowerHorizontal is required.");
        }
     
        if (mirrorPowerVertical is < 1 or > 4)
        {
            _notification
                .Add(nameof(mirrorPowerVertical), "MirrorPowerVertical is required.");
        }

        if (cellGroupLength is < 2 or > 8)
        {
            _notification
                .Add(nameof(cellGroupLength), "CellGroupLength is required.");
        }

        if (cellWidthPixel is < 32 or > 256)
        {
            _notification
                .Add(nameof(cellWidthPixel), "CellWidthPixel is required.");
        }

        if (seed is < 0 or > 100000)
        {
            _notification
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
            _notification
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
            _notification
                .Add(nameof(backgroundColor), "ForegroundColor is required.");
        }

        if (_notification.IsInvalid)
        {
            _outputPort.Invalid();
            return;
        }

        await _useCase
            .Execute(
                mirrorPowerHorizontal, mirrorPowerVertical, cellGroupLength,
                cellWidthPixel, includeEmptyAndFill, seed,
                backgroundColor, foregroundColor)
            .ConfigureAwait(false);
    }
}