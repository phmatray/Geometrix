using Geometrix.Domain.Cells;
using Geometrix.Domain.Helpers;

namespace Geometrix.Domain.Patterns;

public class Pattern : IPattern
{
    public Pattern(int mirrorPowerHorizontal, int mirrorPowerVertical, int cellGroupLength, bool includeEmptyAndFill, int seed)
    {
        MirrorPowerHorizontal = mirrorPowerHorizontal;
        MirrorPowerVertical = mirrorPowerVertical;
        CellGroupLength = cellGroupLength;
        IncludeEmptyAndFill = includeEmptyAndFill;
        Seed = seed;
        Cells = new CellsCollection(cellGroupLength, includeEmptyAndFill).FillWithRandomCells(seed);

        Expand();
    }

    public string Id =>
        $"{CellGroupLength}-{Seed}-{IncludeEmptyAndFill.ToString()[0]}-{MirrorPowerHorizontal}-{MirrorPowerVertical}";

    public int MirrorPowerHorizontal { get; }

    public int MirrorPowerVertical { get; }

    public int CellGroupLength { get; }

    public bool IncludeEmptyAndFill { get; }

    public int Seed { get; }

    public CellsCollection Cells { get; }

    public int HorizontalCell => CellGroupLength * 2.Pow(MirrorPowerHorizontal);

    public int VerticalCell => CellGroupLength * 2.Pow(MirrorPowerVertical);

    public Pattern Expand()
    {
        for (int currentPower = 1; currentPower <= MirrorPowerHorizontal; currentPower++)
        {
            Cells.ExpandRight(currentPower);
        }

        for (int currentPower = 1; currentPower <= MirrorPowerVertical; currentPower++)
        {
            Cells.ExpandDown(currentPower);
        }

        return this;
    }
}