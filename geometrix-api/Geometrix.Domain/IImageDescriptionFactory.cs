using Geometrix.Domain.Patterns;
using Geometrix.Domain.ValueObjects;

namespace Geometrix.Domain;

public interface IImageDescriptionFactory
{
    ImageDescription NewImage(
        Pattern pattern,
        Settings settings);

    Pattern NewPattern(
        int mirrorPowerHorizontal,
        int mirrorPowerVertical,
        int cellGroupLength,
        bool includeEmptyAndFill,
        int seed);
}