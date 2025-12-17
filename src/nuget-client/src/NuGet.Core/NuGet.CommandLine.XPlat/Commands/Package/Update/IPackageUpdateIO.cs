// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Configuration;
using NuGet.Common;
using NuGet.ProjectModel;
using NuGet.Protocol.Model;
using NuGet.Versioning;
using static NuGet.CommandLine.XPlat.Commands.Package.Update.PackageUpdateCommandRunner;

namespace NuGet.CommandLine.XPlat.Commands.Package.Update;

/// <summary>
/// Interface for performing restore operations for package updates.
/// </summary>
internal interface IPackageUpdateIO
{
    /// <summary>
    /// Loads a project or solution and gets the restore inputs as a DependencyGraphSpec.
    /// </summary>
    /// <param name="project">The project or solution requested.</param>
    /// <returns>A DependencyGraphSpec representing the restore inputs.</returns>
    DependencyGraphSpec? GetDependencyGraphSpec(string project);

    /// <summary>
    /// Performs a restore preview operation without committing the result.
    /// </summary>
    /// <param name="dgSpec">The dependency graph specification.</param>
    /// <param name="logger">Logger for the operation.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The restore result pair from the preview operation.</returns>
    Task<RestoreResult> PreviewUpdatePackageReferenceAsync(
        DependencyGraphSpec dgSpec,
        ILogger logger,
        CancellationToken cancellationToken);

    /// <summary>
    /// Commit the restore (write the restore output files, like the assets file) after a preview operation.
    /// </summary>
    /// <param name="restorePreviewResult">The preview restore results to be committed.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns></returns>
    Task CommitAsync(RestoreResult restorePreviewResult, CancellationToken cancellationToken);

    /// <summary>
    /// Updates the package reference in the project, automatically generating the LibraryDependency
    /// and choosing between unconditional or conditional references based on whether the package
    /// is used by all target frameworks.
    /// </summary>
    /// <param name="updatedPackageSpec">The updated project specification containing target framework information.</param>
    /// <param name="packageTfmAliases">Target frameworks where the package is used.</param>
    /// <param name="restorePreviewResult">The restore preview result containing resolved package information.</param>
    /// <param name="packageDependency">Package dependency information.</param>
    /// <param name="logger">Logger for the operation.</param>
    void UpdatePackageReference(
        PackageSpec updatedPackageSpec,
        RestoreResult restorePreviewResult,
        List<string> packageTfmAliases,
        PackageToUpdate packageDependency,
        ILogger logger);

    /// <summary>
    /// Gets the latest version of a package from package sources.
    /// </summary>
    /// <param name="packageId">The package name to check.</param>
    /// <param name="includePrerelease">Whether prerelease packages should be considered.</param>
    /// <param name="allowedSources">Package source mapping sources configured for this package name.
    /// <see langword="null"/> if package source mapping is not configured.</param>
    /// <param name="logger">Output logger</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The <see cref="NuGetVersion"/> of the highest version of the package available.
    /// <see langword="null"/> if no versions of the package are found.</returns>
    Task<NuGetVersion?> GetLatestVersionAsync(
        string packageId,
        bool includePrerelease,
        IReadOnlyList<string>? allowedSources,
        ILogger logger,
        CancellationToken cancellationToken);

    /// <summary>Gets the vulnerability database from the source(s) vulnerability info resource. Uses
    /// audit sources if the settings have any configured, otherwise uses package sources, just like restore.</summary>
    /// <param name="logger">The output logger.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The vulnerability database.</returns>
    Task<IReadOnlyList<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>>
        GetKnownVulnerabilitiesAsync(ILogger logger, CancellationToken cancellationToken);

    /// <summary>Finds the lowest package version above a minimum version, that does not have any
    /// known vulnerabilities.</summary>
    /// <param name="packageId">The package name to check</param>
    /// <param name="allowedSources">Package source mapping sources configured for this package name.
    /// <see langword="null"/> if package source mapping is not configured.</param>
    /// <param name="minVersion">The minimum version to accept.</param>
    /// <param name="logger">Output logger</param>
    /// <param name="knownVulnerabilities">The known vulnerabilities list.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The <see cref="NuGetVersion"/> of the lowest version without a known vulnerability.
    /// <see langword="null"/> if the package name can't be found on the source(s), or if all the versions
    /// available on the source(s) have known vulnerabilities.</returns>
    Task<NuGetVersion?> GetNonVulnerableAsync(
        string packageId,
        IReadOnlyList<string>? allowedSources,
        NuGetVersion minVersion,
        ILogger logger,
        IReadOnlyList<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> knownVulnerabilities,
        CancellationToken cancellationToken);

    /// <summary>Gets the assets file for a project.</summary>
    /// <param name="dgSpec">The restore inputs for the project.</param>
    /// <param name="logger">The output logger</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The assets file for the project.</returns>
    Task<LockFile> GetProjectAssetsFileAsync(DependencyGraphSpec dgSpec, ILogger logger, CancellationToken cancellationToken);

    /// <summary>Gets the package source mapping configuration for the current settins context.</summary>
    /// <returns>The package source mapping settings.</returns>
    PackageSourceMapping GetPackageSourceMapping();

    /// <summary>
    /// An opaque type, to aid in testing, representing the result of a restore operation.
    /// </summary>
    internal abstract class RestoreResult
    {
        /// <summary>
        /// Was the preview restore operation successful
        /// </summary>
        public abstract bool Success { get; }
    }
}
