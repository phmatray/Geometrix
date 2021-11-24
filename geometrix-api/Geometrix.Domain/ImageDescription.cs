using Geometrix.Domain.Patterns;
using Geometrix.Domain.ValueObjects;

namespace Geometrix.Domain;

public class ImageDescription : IImageDescription
{
    public ImageDescription(Pattern pattern, Settings settings)
    {
        Pattern = pattern;
        Settings = settings;
    }

    public string Id =>
        $"{Pattern.Id}-{Settings.Id}";

    public Pattern Pattern { get; }
    public Settings Settings { get; }

    public int WidthPixel =>
        Pattern.HorizontalCell * Settings.CellWidthPixel;

    public int HeightPixel =>
        Pattern.VerticalCell * Settings.CellWidthPixel;

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