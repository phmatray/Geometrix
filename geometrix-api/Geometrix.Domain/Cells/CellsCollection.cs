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

    public void FillWithRandomCells(int seed, bool includeEmptyAndFill)
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
    }

    public void ExpandRight(int currentPower)
    {
        var mirroredCells = new List<Cell>(Count);
        mirroredCells.AddRange(this.Select(cell => CreateMirrorCell(cell, true, currentPower)));

        AddRange(mirroredCells);
    }

    public void ExpandDown(int currentPower)
    {
        var mirroredCells = new List<Cell>(Count);
        mirroredCells.AddRange(this.Select(cell => CreateMirrorCell(cell, false, currentPower)));

        AddRange(mirroredCells);
    }

    private Cell CreateMirrorCell(Cell original, bool isRight, int currentPower)
    {
        int mirrorFactor = _cellGroupLength * 2.Pow(currentPower) - 1;
        int x = isRight ? -original.X + mirrorFactor : original.X;
        int y = isRight ? original.Y : -original.Y + mirrorFactor;

        TriangleDirection triangleDirection = isRight
            ? TriangleDirection.MirrorRight(original.TriangleDirection)
            : TriangleDirection.MirrorDown(original.TriangleDirection);

        return new Cell(x, y, triangleDirection);
    }
}