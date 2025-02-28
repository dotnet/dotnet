namespace Microsoft.NET.Sdk.Razor.SourceGenerators;

internal static class RazorCohostingOptions
{
    /// <summary>
    /// True if razor is running in the cohosting mode
    /// </summary>
    internal static bool UseRazorCohostServer { get; set; } = false;
}
