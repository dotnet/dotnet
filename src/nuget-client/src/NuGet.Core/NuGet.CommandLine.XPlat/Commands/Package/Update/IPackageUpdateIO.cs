// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
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
    /// <returns>The highest eligible version and the highest version still in cooldown.</returns>
    Task<PackageVersionLookupResult> GetLatestVersionAsync(
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
    /// <returns>The lowest eligible non-vulnerable version and the lowest non-vulnerable version still in cooldown.</returns>
    Task<PackageVersionLookupResult> GetNonVulnerableAsync(
        string packageId,
        IReadOnlyList<string>? allowedSources,
        NuGetVersion minVersion,
        ILogger logger,
        IReadOnlyList<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> knownVulnerabilities,
        CancellationToken cancellationToken);

    /// <summary>Gets the assets file for a project.</summary>
    /// <param name="dgSpec">The restore inputs for the project.</param>
    /// <param name="projectPath">The path to the project to get the assets file for.</param>
    /// <param name="logger">The output logger</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The assets file for the project.</returns>
    Task<LockFile> GetProjectAssetsFileAsync(DependencyGraphSpec dgSpec, string projectPath, ILogger logger, CancellationToken cancellationToken);

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

/// <summary>
/// Contains the results of "get package versions" lookup operation. For "get latest", Version represents the
/// highest version that is not in cooldown, and VersionInCooldown represents the highest version that is still
/// in cooldown. For "get non-vulnerable", Version represents the lowest version that is not in cooldown, and
/// VersionInCooldown represents the lowest version that is still in cooldown, but only when it is lower than
/// Version and would therefore have been preferred if it were eligible.
/// </summary>
/// <param name="Version">
/// A version that satisfies the lookup criteria and the minimum publish age configured for at least one source.
/// <see langword="null"/> when no source provides a matching version that has reached its minimum publish age.
/// </param>
/// <param name="VersionInCooldown">
/// A version that satisfies the lookup criteria but has not reached a source's configured minimum publish age.
/// <see langword="null"/> when no such version would be preferred over <paramref name="Version"/>.
/// </param>
internal readonly record struct PackageVersionLookupResult(
    NuGetVersion? Version,
    NuGetVersion? VersionInCooldown);
