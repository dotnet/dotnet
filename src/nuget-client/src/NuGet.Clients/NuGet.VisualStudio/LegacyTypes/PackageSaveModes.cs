namespace NuGet
{
    /// <summary>
    /// Legacy
    /// </summary>
    public enum PackageSaveModes
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Nuspec
        /// </summary>
        Nuspec = 1,

        /// <summary>
        /// Nupkg
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Naming",
            "CA1704:IdentifiersShouldBeSpelledCorrectly",
            MessageId = "Nupkg",
            Justification = "nupkg is the file extension of the package file")]
        Nupkg = 2
    }
}
