namespace Geometrix.Domain.ValueObjects;

public record PatternSeed
{
    public PatternSeed(string seed)
    {
        Seed = seed;
    }

    public string Seed { get; }
}