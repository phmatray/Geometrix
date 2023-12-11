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
        if (!Enum.IsDefined(typeof(TriangleDirection.Direction), direction.Value))
        {
            throw new ArgumentOutOfRangeException(nameof(direction), $"Invalid triangle direction: {direction}");
        }

        return direction.Value == TriangleDirection.Direction.None 
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
            TriangleDirection.Direction.TopLeft => CreateTriangle(new[] { (x, y), (x + cellWidthPixel, y), (x, y + cellWidthPixel) }),
            TriangleDirection.Direction.TopRight => CreateTriangle(new[] { (x + cellWidthPixel, y), (x + cellWidthPixel, y + cellWidthPixel), (x, y) }),
            TriangleDirection.Direction.BottomLeft => CreateTriangle(new[] { (x, y + cellWidthPixel), (x, y), (x + cellWidthPixel, y + cellWidthPixel) }),
            TriangleDirection.Direction.BottomRight => CreateTriangle(new[] { (x + cellWidthPixel, y + cellWidthPixel), (x, y + cellWidthPixel), (x + cellWidthPixel, y) }),
            TriangleDirection.Direction.Filled => CreateRectangle(x, y, cellWidthPixel),
            _ => throw new ArgumentException("Invalid triangle direction", nameof(direction))
        };
    }

    private Polygon CreateTriangle((int x, int y)[] points)
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
