using Geometrix.Domain.Patterns;
using Geometrix.Domain.ValueObjects;

namespace Geometrix.Domain;

public class ImageDescription(Pattern pattern, Settings settings)
    : IImageDescription
{
    public Pattern Pattern { get; } = pattern;
    public Settings Settings { get; } = settings;
    
    public string Id =>
        $"{Pattern.Id}-{Settings.Id}";

    public int WidthPixel =>
        Pattern.HorizontalCell * Settings.CellWidthPixel.Value;

    public int HeightPixel =>
        Pattern.VerticalCell * Settings.CellWidthPixel.Value;

    public void Deconstruct(
        out Pattern pattern,
        out Settings settings,
        out int imageWidthPixel,
        out int imageHeightPixel)
    {
        pattern = Pattern;
        settings = Settings;
        imageWidthPixel = WidthPixel;
        imageHeightPixel = HeightPixel;
    }
}