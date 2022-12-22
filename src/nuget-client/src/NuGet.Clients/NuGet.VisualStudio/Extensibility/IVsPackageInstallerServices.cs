// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using EnvDTE;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Contains methods to query for installed packages within the current solution.
    /// </summary>
    [ComImport]
    [Guid("B858E847-4920-4313-9D3B-176BB0D2F5C2")]
    [Obsolete("Use INuGetProjectService in the NuGet.VisualStudio.Contracts package instead.")]
    public interface IVsPackageInstallerServices
    {
        // IMPORTANT: do NOT rearrange the methods here. The order is important to maintain 
        // backwards compatibility with clients that were compiled against old versions of NuGet.

        /// <summary>
        /// Get the list of NuGet packages installed in the current solution.
        /// </summary>
        [Obsolete("This method can cause UI delays if called on the UI thread. Use INuGetProjectService.GetInstalledPackagesAsync in the NuGet.VisualStudio.Contracts package instead, and iterate all projects in the solution")]
        IEnumerable<IVsPackageMetadata> GetInstalledPackages();

        /// <summary>
        /// Checks if a NuGet package with the specified Id is installed in the specified project.
        /// </summary>
        /// <param name="project">The project to check for NuGet package.</param>
        /// <param name="id">The id of the package to check.</param>
        /// <returns><c>true</c> if the package is install. <c>false</c> otherwise.</returns>
        /// <exception cref="InvalidOperationException">A "project not nominated" exception will be thrown if the project system has not yet told NuGet about the project.
        /// You can use <see cref="IVsNuGetProjectUpdateEvents"/> or Microsoft.VisualStudio.OperationProgress to be notified when the project is ready.</exception>
        [Obsolete("This method can cause UI delays if called on the UI thread. Use INuGetProjectService.GetInstalledPackagesAsync in the NuGet.VisualStudio.Contracts package instead, and check the specific package you're interested in")]
        bool IsPackageInstalled(Project project, string id);

        /// <summary>
        /// Checks if a NuGet package with the specified Id and version is installed in the specified project.
        /// </summary>
        /// <param name="project">The project to check for NuGet package.</param>
        /// <param name="id">The id of the package to check.</param>
        /// <param name="version">The version of the package to check.</param>
        /// <returns><c>true</c> if the package is install. <c>false</c> otherwise.</returns>
        /// <exception cref="InvalidOperationException">A "project not nominated" exception will be thrown if the project system has not yet told NuGet about the project.
        /// You can use <see cref="IVsNuGetProjectUpdateEvents"/> or Microsoft.VisualStudio.OperationProgress to be notified when the project is ready.</exception>
        [Obsolete("This method can cause UI delays if called on the UI thread. Use INuGetProjectService.GetInstalledPackagesAsync in the NuGet.VisualStudio.Contracts package instead, and check the specific package you're interested in")]
        bool IsPackageInstalled(Project project, string id, SemanticVersion version);

        /// <summary>
        /// Checks if a NuGet package with the specified Id and version is installed in the specified project.
        /// </summary>
        /// <param name="project">The project to check for NuGet package.</param>
        /// <param name="id">The id of the package to check.</param>
        /// <param name="versionString">The version of the package to check.</param>
        /// <returns><c>true</c> if the package is install. <c>false</c> otherwise.</returns>
        /// <remarks>
        /// The reason this method is named IsPackageInstalledEx, instead of IsPackageInstalled, is that
        /// when client project compiles against this assembly, the compiler would attempt to bind against
        /// the other overload which accepts SemanticVersion and would require client project to reference NuGet.Core.
        /// </remarks>
        /// <exception cref="InvalidOperationException">A "project not nominated" exception will be thrown if the project system has not yet told NuGet about the project.
        /// You can use <see cref="IVsNuGetProjectUpdateEvents"/> or Microsoft.VisualStudio.OperationProgress to be notified when the project is ready.</exception>
        [Obsolete("This method can cause UI delays if called on the UI thread. Use INuGetProjectService.GetInstalledPackagesAsync in the NuGet.VisualStudio.Contracts package instead, and check the specific package you're interested in")]
        bool IsPackageInstalledEx(Project project, string id, string versionString);

        /// <summary>
        /// Get the list of NuGet packages installed in the specified project.
        /// </summary>
        /// <param name="project">The project to get NuGet packages from.</param>
        /// <exception cref="InvalidOperationException">A "project not nominated" exception will be thrown if the project system has not yet told NuGet about the project.
        /// You can use <see cref="IVsNuGetProjectUpdateEvents"/> or Microsoft.VisualStudio.OperationProgress to be notified when the project is ready.</exception>
        [Obsolete("This method can cause UI delays if called on the UI thread. Use INuGetProjectService.GetInstalledPackagesAsync in the NuGet.VisualStudio.Contracts package instead")]
        IEnumerable<IVsPackageMetadata> GetInstalledPackages(Project project);
    }
}
