using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Interface allowing integration of alternate package manager suggestion for a NuGet package. 
    /// For example jQuery may appear on Bower and npm,
    /// it might be more appropriate to install a package from them for certain projects. 
    /// </summary>
    [Obsolete]
    [ComImport]
    [Guid("BCED5BF2-40FC-4D9F-BF0A-43CD4E9FF65F")]
    public interface IVsPackageManagerProvider
    {
        /// <summary>
        /// Localized display package manager name.
        /// </summary>
        string PackageManagerName { get; }

        /// <summary>
        /// Package manager unique id.
        /// </summary>
        string PackageManagerId { get; }

        /// <summary>
        /// The tool tip description for the package
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Check if a recommendation should be surfaced for an alternate package manager. 
        /// This code should not rely on slow network calls, and should return rapidly.
        /// </summary>
        /// <param name="packageId">Current package id</param>
        /// <param name="projectName">Unique project name for finding the project through VS dte</param>
        /// <param name="token">Cancellation Token</param>
        /// <returns>return true if need to direct to integrated package manager for this package</returns>
        Task<bool> CheckForPackageAsync(string packageId, string projectName, CancellationToken token);

        /// <summary>
        /// This Action should take the user to the other package manager.
        /// </summary>
        /// <param name="packageId">Current package id</param>
        /// <param name="projectName">Unique project name for finding the project through VS dte</param>
        void GoToPackage(string packageId, string projectName);
    }
}
