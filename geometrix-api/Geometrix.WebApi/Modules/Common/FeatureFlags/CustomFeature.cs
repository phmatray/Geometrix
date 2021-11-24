namespace Geometrix.WebApi.Modules.Common.FeatureFlags;

/// <summary>
///     Features Flags Enum.
/// </summary>
public enum CustomFeature
{
    /// <summary>
    ///     Generate ImageDescription.
    /// </summary>
    GenerateImage,

    /// <summary>
    ///     Filter errors out.
    /// </summary>
    ErrorFilter,

    /// <summary>
    ///     Use Swagger.
    /// </summary>
    Swagger,

    /// <summary>
    ///     Use authentication.
    /// </summary>
    Authentication
}