using Geometrix.Domain.ValueObjects;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;

namespace Geometrix.Infrastructure.ImageCreation;

public sealed class TriangleService
{
    public Polygon? GetTriangle(TriangleDirection direction, int x, int y, int cellWidthPixel)
    {
        return direction.Value switch
        {
            TriangleDirection.Direction.None => null,
            TriangleDirection.Direction.TopLeft => CreateTopLeftTriangle(x, y, cellWidthPixel),
            TriangleDirection.Direction.TopRight => CreateTopRightTriangle(x, y, cellWidthPixel),
            TriangleDirection.Direction.BottomLeft => CreateBottomLeftTriangle(x, y, cellWidthPixel),
            TriangleDirection.Direction.BottomRight => CreateBottomRightTriangle(x, y, cellWidthPixel),
            TriangleDirection.Direction.Filled => CreateFilled(x, y, cellWidthPixel),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    private Polygon CreateTopLeftTriangle(int x, int y, int cellWidthPixel)
    {
        return new Polygon(new List<ILineSegment>
        {
            new LinearLineSegment(
                new PointF(x, y),
                new PointF(x + cellWidthPixel, y),
                new PointF(x, y + cellWidthPixel)
            )
        });
    }

    private Polygon CreateTopRightTriangle(int x, int y, int cellWidthPixel)
    {
        return new Polygon(new List<ILineSegment>
        {
            new LinearLineSegment(
                new PointF(x + cellWidthPixel, y),
                new PointF(x + cellWidthPixel, y + cellWidthPixel),
                new PointF(x, y)
            )
        });
    }

    private Polygon CreateBottomLeftTriangle(int x, int y, int cellWidthPixel)
    {
        return new Polygon(new List<ILineSegment>
        {
            new LinearLineSegment(
                new PointF(x, y + cellWidthPixel),
                new PointF(x, y),
                new PointF(x + cellWidthPixel, y + cellWidthPixel)
            )
        });
    }

    private Polygon CreateBottomRightTriangle(int x, int y, int cellWidthPixel)
    {
        return new Polygon(new List<ILineSegment>
        {
            new LinearLineSegment(
                new PointF(x + cellWidthPixel, y + cellWidthPixel),
                new PointF(x, y + cellWidthPixel),
                new PointF(x + cellWidthPixel, y)
            )
        });
    }

    private Polygon CreateFilled(int x, int y, int cellWidthPixel)
    {
        return new Polygon(new List<ILineSegment>
        {
            new LinearLineSegment(
                new PointF(x, y),
                new PointF(x + cellWidthPixel, y),
                new PointF(x + cellWidthPixel, y + cellWidthPixel),
                new PointF(x, y + cellWidthPixel)
            )
        });
    }
}