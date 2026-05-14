using NuGet.Packaging.Core;

namespace NuGet.CommandLine.Test.Caching
{
    /// <summary>
    /// The nuget.exe command to test. This interface focuses on nuget.exe commands that cache
    /// packages or other data to the file system.
    /// </summary>
    public interface ICachingCommand
    {
        /// <summary>
        /// Gets the display name for this command.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Prepare the string arguments for nuget.exe so the command can be executed.
        /// </summary>
        /// <param name="context">The test context.</param>
        /// <param name="identity">The identity of the package to be installed.</param>
        /// <returns>The string arguments for nuget.exe.</returns>
        string PrepareArguments(CachingTestContext context, PackageIdentity identity);

        /// <summary>
        /// Determines whether the package was installed to the output directory.
        /// </summary>
        /// <param name="context">The test context.</param>
        /// <param name="identity">The identity of the package to check.</param>
        /// <returns>True if the package was installed to the output directory.</returns>
        bool IsPackageInstalled(CachingTestContext context, PackageIdentity identity);

        /// <summary>
        /// Gets the path where the package was installed to. This should be an absolute path
        /// to the directory where the package is extracted. Returns null of the package was
        /// not installed.
        /// </summary>
        /// <param name="context">The test context.</param>
        /// <param name="identity">The identity of the package.</param>
        /// <returns>The path.</returns>
        string GetInstalledPackagePath(CachingTestContext context, PackageIdentity identity);
    }
}
