using Geometrix.Domain.ValueObjects;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;

namespace Geometrix.Infrastructure.ImageCreation;

public sealed class TriangleService
{
    private readonly TriangleFactory _triangleFactory = new();

    /// <summary>
    /// Get a triangle from a direction and a position.
    /// </summary>
    /// <param name="direction">The direction of the triangle.</param>
    /// <param name="x">The x position of the triangle.</param>
    /// <param name="y">The y position of the triangle.</param>
    /// <param name="cellWidthPixel">The width of the cell in pixel.</param>
    /// <returns>The triangle.</returns>
    /// <exception cref="ArgumentOutOfRangeException">direction</exception>
    public Polygon? GetTriangle(TriangleDirection direction, int x, int y, int cellWidthPixel)
    {
        return direction.Value == TriangleDirection.None.Value 
            ? null 
            : _triangleFactory.CreateTriangle(direction, x, y, cellWidthPixel);
    }
}

public class TriangleFactory
{
    public Polygon CreateTriangle(TriangleDirection direction, int x, int y, int cellWidthPixel)
    {
        return direction.Value switch
        {
            1 => CreateTriangle([(x, y), (x + cellWidthPixel, y), (x, y + cellWidthPixel)]),
            2 => CreateTriangle([(x + cellWidthPixel, y), (x + cellWidthPixel, y + cellWidthPixel), (x, y)]),
            3 => CreateTriangle([(x, y + cellWidthPixel), (x, y), (x + cellWidthPixel, y + cellWidthPixel)]),
            4 => CreateTriangle([(x + cellWidthPixel, y + cellWidthPixel), (x, y + cellWidthPixel), (x + cellWidthPixel, y)]),
            5 => CreateRectangle(x, y, cellWidthPixel),
            _ => throw new ArgumentException("Invalid triangle direction", nameof(direction))
        };
    }

    private Polygon CreateTriangle(IList<(int x, int y)> points)
    {
        var lineSegments = new List<ILineSegment>
        {
            new LinearLineSegment(
                new PointF(points[0].x, points[0].y),
                new PointF(points[1].x, points[1].y),
                new PointF(points[2].x, points[2].y)
            )
        };

        return new Polygon(lineSegments);
    }

    private Polygon CreateRectangle(int x, int y, int cellWidthPixel)
    {
        var lineSegments = new List<ILineSegment>
        {
            new LinearLineSegment(
                new PointF(x, y),
                new PointF(x + cellWidthPixel, y),
                new PointF(x + cellWidthPixel, y + cellWidthPixel),
                new PointF(x, y + cellWidthPixel)
            )
        };

        return new Polygon(lineSegments);
    }
}
