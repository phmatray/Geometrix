﻿namespace Geometrix.Domain.ValueObjects;

public record Settings(
    int CellWidthPixel,
    ThemeColor BackgroundColor,
    ThemeColor ForegroundColor)
{
    public string Id =>
        $"{BackgroundColor.Value}-{ForegroundColor.Value}-{CellWidthPixel}";
}