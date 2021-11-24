using Geometrix.Domain.ValueObjects;

namespace Geometrix.Domain.Cells;

public record Cell(
    int X,
    int Y,
    TriangleDirection Direction);