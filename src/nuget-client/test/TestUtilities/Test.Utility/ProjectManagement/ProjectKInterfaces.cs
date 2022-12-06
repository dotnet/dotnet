// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;

namespace Test.Utility.ProjectManagement
{
    /***************************************************************************************************
     * This file contains the interfaces needed for project.json projects.  These interfaces have been
     * removed from newer versions of Microsoft.VisualStudio.ProjectSystem.Managed.  In order for our
     * tests to still interact with Visual Studio, they need to be defined here as ComImports with
     * the appropriate GUID which will work just fine.
     ***************************************************************************************************/

    public enum NuGetOperation
    {
        /// <summary>
        /// Represent installing a package.
        /// </summary>
        Install,

        /// <summary>
        /// Represents uninstalling a package.
        /// </summary>
        Uninstall
    }

    /// <summary>
    /// Represents the progress of a nuget installation operation.
    /// </summary>
    [ComImport]
    [Guid("93B269C4-85D6-4AEA-9398-81754CA2560B")]
    public interface INuGetPackageInstallProgress
    {
        /// <summary>
        /// Count of packages installed so far.
        /// </summary>
        int InstalledPackagesCount { get; }

        /// <summary>
        /// Count of packages to be installed.
        /// </summary>
        int TotalPackagesToInstall { get; }
    }

    [ComImport]
    [Guid("FD2DC07E-9054-4115-B86B-26A9F9C1F00B")]
    public interface INuGetPackageManager
    {
        /// <summary>
        /// Returns the list of packages installed in the project. This should
        /// return only the direct dependencies.
        /// </summary>
        /// <remarks>
        /// A class with generic parameters (Task in this case)
        /// cannot use embedded types as type parameters.
        /// (See: http://msdn.microsoft.com/en-us/library/dd264728.aspx).
        /// That's the reason why the return type is marked as collection of objects rather than
        /// <see cref="T:Microsoft.VisualStudio.ProjectSystem.Interop.INuGetPackageMoniker" /> because the latter is an embedded type.
        /// The actual implementations should return <see cref="T:Microsoft.VisualStudio.ProjectSystem.Interop.INuGetPackageMoniker" /> objects
        /// in the collection.
        /// </remarks>
        Task<IReadOnlyCollection<object>> GetInstalledPackagesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Installs a package in the project.
        /// </summary>
        /// <param name="package">Package to install.</param>
        /// <param name="options">Any options specified by the user.</param>
        /// <param name="logger">Logger to report the progress back to the NuGet. Can be null.</param>
        /// <param name="progress">To report the progress back to NuGet. Can be null.</param>
        /// <param name="cancellationToken">Cancellation token.Cancellation token. When responding
        /// to cancellation, the implementors should ensure the rollback of
        /// everything done and bring the project to original state.</param>
        /// <remarks>
        /// This should typically download the package and it's dependencies into the
        /// project's packages folder, modify the package manifest file for the project,
        /// add references to assemblies and any other additional steps
        /// required for the package installation.
        /// </remarks>
        Task InstallPackageAsync(INuGetPackageMoniker package, IReadOnlyDictionary<string, object> options, TextWriter logger, IProgress<INuGetPackageInstallProgress> progress, CancellationToken cancellationToken);

        /// <summary>
        /// Uninstalls the package.
        /// </summary>
        /// <param name="package">Package to uninstall.</param>
        /// <param name="options">Any options specified by the user.</param>
        /// <param name="logger">Logger to report the progress back to the NuGet.</param>
        /// <param name="progress">To report the progress back to NuGet.</param>
        /// <param name="cancellationToken">Cancellation token. When responding
        /// to cancellation, the implementors should ensure the rollback of
        /// everything done and bring the project to original state.</param>
        /// <remarks>
        /// This should handle all the steps required to remove the project's dependency
        /// on package like removing the package reference from package manifest and also
        /// removing assembly references and any other additional steps.
        /// </remarks>
        Task UninstallPackageAsync(INuGetPackageMoniker package, IReadOnlyDictionary<string, object> options, TextWriter logger, IProgress<INuGetPackageInstallProgress> progress, CancellationToken cancellationToken);

        /// <summary>
        /// Specifies whether the current implementation actually supports the given option.
        /// </summary>
        /// <param name="optionName">Option name.</param>
        /// <param name="operation">NuGet Operation.</param>
        /// <returns><see langword="true" /> if the given optionName is supported for the given operation by
        /// this implementation; otherwise, <see langword="false" />.</returns>
        bool CanSupport(string optionName, NuGetOperation operation);

        /// <summary>
        /// Gets a list of frameworks supported by the project system.
        /// </summary>
        /// <returns>A readonly collection of <see cref="T:System.Runtime.Versioning.FrameworkName" />s
        /// supported by the project system.</returns>
        Task<IReadOnlyCollection<FrameworkName>> GetSupportedFrameworksAsync(CancellationToken cancellationToken);
    }

    /// <summary>
    /// Represents metadata about a nuget package.
    /// </summary>
    [ComImport]
    [Guid("BCA2E197-E352-4474-B0A7-D8CE606CD4E9")]
    public interface INuGetPackageMoniker
    {
        /// <summary>
        /// Gets the Id of package.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the Version of package.
        /// </summary>
        string Version { get; }
    }
}
