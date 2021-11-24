using Geometrix.Domain.Patterns;
using Geometrix.Domain.ValueObjects;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Geometrix.Infrastructure.ImageCreation;

public sealed class ImageEditionService
{
    private readonly TriangleService _triangleService;

    public ImageEditionService(TriangleService triangleService)
    {
        _triangleService = triangleService;
    }

    public void EditImage(
        Image<Rgba32> image,
        Pattern pattern,
        Settings settings)
    {
        (
            int cellWidthPixel,
            ThemeColor backgroundThemeColor,
            ThemeColor foregroundThemeColor
        ) = settings;

        Color background = ConvertToSixLaborsColor(backgroundThemeColor);
        SetBackground(image, background);
        
        Color foreground = ConvertToSixLaborsColor(foregroundThemeColor);
        List<Polygon> polygons = ConvertToPolygons(pattern, cellWidthPixel);
        SetPolygons(image, foreground, polygons);
    }

    public void SetPolygons(Image<Rgba32> image, Color foreground, List<Polygon> polygons)
    {
        foreach (Polygon polygon in polygons)
        {
            SetPolygon(image, foreground, polygon);
        }
    }

    public void SetPolygon(Image<Rgba32> image, Color foreground, Polygon polygon)
    {
        image.Mutate(context => FillPathExtensions.Fill(context, foreground, polygon));
    }

    public void SetBackground(Image<Rgba32> image, Color background)
    {
        image.Mutate(context => context.BackgroundColor(background));
    }

    public Color ConvertToSixLaborsColor(ThemeColor color)
    {
        return color.Value switch
        {
            "light" => Color.FromRgb(0xf9, 0xfa, 0xfb),
            "dark" => Color.FromRgb(0x11, 0x18, 0x27),
            "red" => Color.FromRgb(0xef, 0x44, 0x44),
            "yellow" => Color.FromRgb(0xf5, 0x9e, 0x0b),
            "green" => Color.FromRgb(0x10, 0xb9, 0x81),
            "blue" => Color.FromRgb(0x3b, 0x82, 0xf6),
            "indigo" => Color.FromRgb(0x63, 0x66, 0xf1),
            "purple" => Color.FromRgb(0x8b, 0x5c, 0xf6),
            "pink" => Color.FromRgb(0xec, 0x48, 0x99),
            _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
        };
    }

    public List<Polygon> ConvertToPolygons(Pattern pattern, int cellWidthPixel)
    {
        var polygons = from cell in pattern.Cells
            let x = cell.X * cellWidthPixel
            let y = cell.Y * cellWidthPixel
            let direction = cell.Direction
            select _triangleService.GetTriangle(direction, x, y, cellWidthPixel);

        return polygons
            .Where(polygon => polygon is not null)
            .ToList();
    }
}