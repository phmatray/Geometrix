namespace Geometrix.Domain.Helpers;

public static class IntegerExtension
{
    public static int Pow(this int bas, int exp = 2)
        => Enumerable
            .Repeat(bas, exp)
            .Aggregate(1, (a, b) => a * b);
}