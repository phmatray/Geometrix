using System.ComponentModel.DataAnnotations;
using Geometrix.Domain;

namespace Geometrix.WebApi.ViewModels;

public sealed class ImageModel
{
    public ImageModel(ImageDescription imageDescription)
    {
        Id = imageDescription.Id;
        WidthPixel = imageDescription.WidthPixel;
        HeightPixel = imageDescription.HeightPixel;
        Seed = imageDescription.Pattern.Seed;
        IncludeEmptyAndFill = imageDescription.Pattern.IncludeEmptyAndFill;
        MirrorPowerHorizontal = imageDescription.Pattern.MirrorPowerHorizontal;
        MirrorPowerVertical = imageDescription.Pattern.MirrorPowerVertical;
        CellGroupLength = imageDescription.Pattern.CellGroupLength;
        HorizontalCell = imageDescription.Pattern.HorizontalCell;
        VerticalCell = imageDescription.Pattern.VerticalCell;
        CellWidthPixel = imageDescription.Settings.CellWidthPixel;
        BackgroundColor = imageDescription.Settings.BackgroundColor.Value;
        ForegroundColor = imageDescription.Settings.ForegroundColor.Value;
    }

    /// <summary>
    /// Gets unique identifier.
    /// </summary>
    [Required]
    public string Id { get; }

    /// <summary>
    /// Gets width in pixels.
    /// </summary>
    [Required]
    public int WidthPixel { get; }
 
    /// <summary>
    /// Gets height in pixels.
    /// </summary>
    [Required]
    public int HeightPixel { get; }
    
    /// <summary>
    /// Gets seed.
    /// </summary>
    [Required]
    public int Seed { get; }
    
    /// <summary>
    /// Includes empty and fill.
    /// </summary>
    [Required]
    public bool IncludeEmptyAndFill { get; }

    /// <summary>
    /// Gets mirror power horizontal.
    /// </summary>
    [Required]
    public int MirrorPowerHorizontal { get; }

    /// <summary>
    /// Gets mirror power vertical.
    /// </summary>
    [Required]
    public int MirrorPowerVertical { get; }
    
    /// <summary>
    /// Gets cell group length.
    /// </summary>
    [Required]
    public int CellGroupLength { get; }
    
    /// <summary>
    /// Gets horizontal cells count.
    /// </summary>
    [Required]
    public int HorizontalCell { get; }
    
    /// <summary>
    /// Gets vertical cells count.
    /// </summary>
    [Required]
    public int VerticalCell { get; }

    /// <summary>
    /// Gets cell width in pixels.
    /// </summary>
    [Required]
    public int CellWidthPixel { get; }

    /// <summary>
    /// Gets background color.
    /// </summary>
    [Required]
    public string BackgroundColor { get; }

    /// <summary>
    /// Gets foreground color.
    /// </summary>
    [Required]
    public string ForegroundColor { get; }
}