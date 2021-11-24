using Geometrix.Domain;
using Geometrix.Domain.Patterns;
using Geometrix.Domain.ValueObjects;

namespace Geometrix.Infrastructure.DataAccess;

public sealed class EntityDescriptionFactory : IImageDescriptionFactory
{
    public ImageDescription NewImage(Pattern pattern, Settings settings) =>
        new(pattern, settings);

    public Pattern NewPattern(
        int mirrorPowerHorizontal,
        int mirrorPowerVertical,
        int cellGroupLength,
        bool includeEmptyAndFill,
        int seed) =>
        new(mirrorPowerHorizontal, mirrorPowerVertical, cellGroupLength, includeEmptyAndFill, seed);
}