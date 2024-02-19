using Intellenum;

namespace Geometrix.Domain.ValueObjects;

[Intellenum]
[Member("None", 0)]
[Member("TopLeft", 1)]
[Member("TopRight", 2)]
[Member("BottomLeft", 3)]
[Member("BottomRight", 4)]
[Member("Filled", 5)]
public partial class TriangleDirection
{
    public static TriangleDirection MirrorRight(TriangleDirection direction)
    {
        return direction.Value switch
        {
            0 => None,
            1 => TopRight,
            2 => TopLeft,
            3 => BottomRight,
            4 => BottomLeft,
            5 => Filled,
            _ => throw new ArgumentOutOfRangeException(nameof(direction))
        };
    }

    public static TriangleDirection MirrorDown(TriangleDirection direction)
    {
        return direction.Value switch
        {
            0 => None,
            1 => BottomLeft,
            2 => BottomRight,
            3 => TopLeft,
            4 => TopRight,
            5 => Filled,
            _ => throw new ArgumentOutOfRangeException(nameof(direction))
        };
    }

    public static TriangleDirection CreateRandom(Random random, bool includeEmptyAndFill)
    {
        var direction = includeEmptyAndFill
            ? random.Next(6)
            : random.Next(4) + 1;

        return FromValue(direction);
    }
}
