using Geometrix.Domain.Patterns;
using Geometrix.Domain.ValueObjects;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;

namespace Geometrix.Infrastructure.ImageCreation;

public sealed class ImageEditionService(TriangleService triangleService)
{
    public void EditImage(
        Image<Rgba32> image,
        Pattern pattern,
        Settings settings)
    {
        var (cellWidthPixel, backgroundThemeColor, foregroundThemeColor) = settings;

        Color background = ConvertToSixLaborsColor(backgroundThemeColor);
        SetBackground(image, background);
        
        Color foreground = ConvertToSixLaborsColor(foregroundThemeColor);
        var polygons = ConvertToPolygons(pattern, cellWidthPixel);
        SetPolygons(image, foreground, polygons);
    }

    private static void SetPolygons(Image<Rgba32> image, Color foreground, List<Polygon> polygons)
    {
        foreach (Polygon polygon in polygons)
        {
            SetPolygon(image, foreground, polygon);
        }
    }

    private static void SetPolygon(Image<Rgba32> image, Color foreground, Polygon polygon) 
        => image.Mutate(context => context.Fill(foreground, polygon));

    private static void SetBackground(Image<Rgba32> image, Color background)
        => image.Mutate(context => context.BackgroundColor(background));

    private static Color ConvertToSixLaborsColor(ThemeColor color)
        => color switch
        {
            _ when color == ThemeColor.Light => Color.FromRgb(0xf9, 0xfa, 0xfb),
            _ when color == ThemeColor.Dark => Color.FromRgb(0x11, 0x18, 0x27),
            _ when color == ThemeColor.Red => Color.FromRgb(0xef, 0x44, 0x44),
            _ when color == ThemeColor.Yellow => Color.FromRgb(0xf5, 0x9e, 0x0b),
            _ when color == ThemeColor.Green => Color.FromRgb(0x10, 0xb9, 0x81),
            _ when color == ThemeColor.Blue => Color.FromRgb(0x3b, 0x82, 0xf6),
            _ when color == ThemeColor.Indigo => Color.FromRgb(0x63, 0x66, 0xf1),
            _ when color == ThemeColor.Purple => Color.FromRgb(0x8b, 0x5c, 0xf6),
            _ when color == ThemeColor.Pink => Color.FromRgb(0xec, 0x48, 0x99),
            _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
        };

    private List<Polygon> ConvertToPolygons(Pattern pattern, int cellWidthPixel)
        => pattern.Cells
            .Select(cell => new
            {
                X = cell.X * cellWidthPixel,
                Y = cell.Y * cellWidthPixel,
                Direction = cell.TriangleDirection
            })
            .Select(cell => triangleService.GetTriangle(cell.Direction, cell.X, cell.Y, cellWidthPixel))
            .Where(polygon => polygon is not null)
            .Cast<Polygon>()
            .ToList();
}