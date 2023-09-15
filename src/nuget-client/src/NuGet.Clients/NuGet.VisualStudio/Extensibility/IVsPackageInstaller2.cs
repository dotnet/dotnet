using System;
using System.Runtime.InteropServices;
using EnvDTE;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Contains method to install latest version of a single package into a project within the current solution.
    /// </summary>
    [ComImport]
    [Guid("4F3B122B-A53B-432C-8D85-0FAFB8BE4FF4")]
    public interface IVsPackageInstaller2 : IVsPackageInstaller
    {
        /// <summary>
        /// Installs the latest version of a single package from the specified package source.
        /// </summary>
        /// <remarks>Can be called from a background thread, if the UI thread is not blocked waiting for this to finish.
        /// See <a href="https://github.com/nuget/home/issues/11476">https://github.com/nuget/home/issues/11476</a></remarks>
        /// <param name="source">
        /// The package source to install the package from. This value can be <see langword="null" />
        /// to indicate that the user's configured sources should be used. Otherwise,
        /// this should be the source path as a string. If the user has credentials
        /// configured for a source, this value must exactly match the configured source
        /// value.
        /// </param>
        /// <param name="project">The target project for package installation.</param>
        /// <param name="packageId">The package ID of the package to install.</param>
        /// <param name="includePrerelease">
        /// Whether or not to consider prerelease versions when finding the latest version
        /// to install.
        /// </param>
        /// <param name="ignoreDependencies">
        /// A boolean indicating whether or not to ignore the package's dependencies
        /// during installation.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when <see paramref="includePrerelease"/> is <see langword="false" /> and no stable version
        /// of the package exists.
        /// </exception>
        void InstallLatestPackage(
            string source,
            Project project,
            string packageId,
            bool includePrerelease,
            bool ignoreDependencies);
    }
}
