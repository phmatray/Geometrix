namespace Geometrix.Application.UseCases.GenerateImage;

/// <summary>
/// Represents the use case for generating an image.
/// </summary>
public interface IGenerateImageUseCase
{
    /// <summary>
    /// Executes the Use Case.
    /// </summary>
    /// <returns>Task.</returns>
    Task Execute(
        int mirrorPowerHorizontal,
        int mirrorPowerVertical,
        int cellGroupLength,
        int cellWidthPixel,
        bool includeEmptyAndFill,
        int seed,
        string backgroundColor,
        string foregroundColor);

    /// <summary>
    /// Sets the Output Port.
    /// </summary>
    /// <param name="outputPort">Output Port</param>
    void SetOutputPort(IOutputPort outputPort);
}