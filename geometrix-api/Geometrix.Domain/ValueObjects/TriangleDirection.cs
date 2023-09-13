namespace Geometrix.Domain.ValueObjects;

public readonly struct TriangleDirection : IEquatable<TriangleDirection>
{
    public Direction Value { get; }

    public TriangleDirection(Direction value)
    {
        Value = value;
    }

    public override bool Equals(object? obj)
    {
        return obj is TriangleDirection other && Equals(other);
    }

    public bool Equals(TriangleDirection other)
    {
        return Value == other.Value;
    }

    public override int GetHashCode()
    {
        return (int) Value;
    }

    public static bool operator ==(TriangleDirection left, TriangleDirection right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(TriangleDirection left, TriangleDirection right)
    {
        return !(left == right);
    }

    public static readonly TriangleDirection None = new(Direction.None);
    public static readonly TriangleDirection TopLeft = new(Direction.TopLeft);
    public static readonly TriangleDirection TopRight = new(Direction.TopRight);
    public static readonly TriangleDirection BottomLeft = new(Direction.BottomLeft);
    public static readonly TriangleDirection BottomRight = new(Direction.BottomRight);
    public static readonly TriangleDirection Filled = new(Direction.Filled);
    
    public static TriangleDirection MirrorRight(TriangleDirection direction)
    {
        return direction.Value switch
        {
            Direction.None => None,
            Direction.TopLeft => TopRight,
            Direction.TopRight => TopLeft,
            Direction.BottomLeft => BottomRight,
            Direction.BottomRight => BottomLeft,
            Direction.Filled => Filled,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static TriangleDirection MirrorDown(TriangleDirection direction)
    {
        return direction.Value switch
        {
            Direction.None => None,
            Direction.TopLeft => BottomLeft,
            Direction.TopRight => BottomRight,
            Direction.BottomLeft => TopLeft,
            Direction.BottomRight => TopRight,
            Direction.Filled => Filled,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static TriangleDirection CreateRandom(Random random, bool includeEmptyAndFill)
    {
        var direction = includeEmptyAndFill
            ? (Direction) random.Next(6)
            : (Direction) (random.Next(4) + 1);

        var triangleDirection = new TriangleDirection(direction);
        return triangleDirection;
    }

    public override string ToString()
    {
        return $"{nameof(Value)}: {Value}";
    }

    public enum Direction
    {
        None = 0,
        TopLeft = 1,
        TopRight = 2,
        BottomLeft = 3,
        BottomRight = 4,
        Filled = 5
    }
}