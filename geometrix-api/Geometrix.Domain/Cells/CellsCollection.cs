using Geometrix.Domain.Helpers;
using Geometrix.Domain.ValueObjects;

namespace Geometrix.Domain.Cells;

public sealed class CellsCollection : List<Cell>
{
    private readonly int _cellGroupLength;

    public CellsCollection(int cellGroupLength)
    {
        _cellGroupLength = cellGroupLength;
    }

    public CellsCollection FillWithRandomCells(int seed, bool includeEmptyAndFill)
    {
        Random random = new(seed);

        Clear();

        for (var x = 0; x < _cellGroupLength; x++)
        {
            for (var y = 0; y < _cellGroupLength; y++)
            {
                var triangleDirection = TriangleDirection.CreateRandom(random, includeEmptyAndFill);
                Cell cell = new(x, y, triangleDirection);
                
                Add(cell);
            }
        }

        return this;
    }

    public CellsCollection ExpandRight(int currentPower)
    {
        int cellsCount = Count;

        for (var index = 0; index < cellsCount; index++)
        {
            (
                int x,
                int y,
                TriangleDirection triangleDirection
            ) = this[index];

            x = -x + _cellGroupLength * 2.Pow(currentPower) - 1;

            TriangleDirection mirrorDirection = TriangleDirection.MirrorRight(triangleDirection);

            Cell mirrorCell = new(x, y, mirrorDirection);

            Add(mirrorCell);
        }

        return this;
    }

    public CellsCollection ExpandDown(int currentPower)
    {
        int cellsCount = Count;

        for (var index = 0; index < cellsCount; index++)
        {
            (
                int x,
                int y,
                TriangleDirection triangleDirection
            ) = this[index];

            y = -y + _cellGroupLength * 2.Pow(currentPower) - 1;

            TriangleDirection mirrorDirection = TriangleDirection.MirrorDown(triangleDirection);

            Cell mirrorCell = new(x, y, mirrorDirection);

            Add(mirrorCell);
        }

        return this;
    }
}