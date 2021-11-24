namespace Geometrix.Domain.ValueObjects;

public readonly struct ThemeColor : IEquatable<ThemeColor>
{
    public string Value { get; }

    public ThemeColor(string value)
    {
        Value = value;
    }
    
    public override bool Equals(object? obj)
    {
        return obj is ThemeColor o && Equals(o);
    }

    public bool Equals(ThemeColor other)
    {
        return Value == other.Value;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value);
    }

    public static bool operator ==(ThemeColor left, ThemeColor right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ThemeColor left, ThemeColor right)
    {
        return !(left == right);
    }

    /// <summary>
    ///     Light.
    /// </summary>
    /// <returns>ThemeColor.</returns>
    public static readonly ThemeColor Light = new("light");

    /// <summary>
    ///     Dark.
    /// </summary>
    /// <returns>ThemeColor.</returns>
    public static readonly ThemeColor Dark = new("dark");

    /// <summary>
    ///     Red.
    /// </summary>
    /// <returns>ThemeColor.</returns>
    public static readonly ThemeColor Red = new("red");

    /// <summary>
    ///     Yellow.
    /// </summary>
    /// <returns>ThemeColor.</returns>
    public static readonly ThemeColor Yellow = new("yellow");

    /// <summary>
    ///     Green.
    /// </summary>
    /// <returns>ThemeColor.</returns>
    public static readonly ThemeColor Green = new("green");

    /// <summary>
    ///     Blue.
    /// </summary>
    /// <returns>ThemeColor.</returns>
    public static readonly ThemeColor Blue = new("blue");

    /// <summary>
    ///     Indigo.
    /// </summary>
    /// <returns>ThemeColor.</returns>
    public static readonly ThemeColor Indigo = new("indigo");

    /// <summary>
    ///     Purple.
    /// </summary>
    /// <returns>ThemeColor.</returns>
    public static readonly ThemeColor Purple = new("purple");

    /// <summary>
    ///     Pink.
    /// </summary>
    /// <returns>ThemeColor.</returns>
    public static readonly ThemeColor Pink = new("pink");

    public override string ToString()
    {
        return Value;
    }
}