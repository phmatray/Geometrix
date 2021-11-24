namespace Geometrix.Domain.Patterns;

public class PatternNull : IPattern
{
    public static PatternNull Instance { get; } = new();

    public string Id => string.Empty;

    public Pattern Expand()
    {
        throw new NotImplementedException();
    }
}