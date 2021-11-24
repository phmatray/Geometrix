namespace Geometrix.Domain.Patterns;

public interface IPattern
{
    string Id { get; }
    Pattern Expand();
}