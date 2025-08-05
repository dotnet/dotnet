// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;
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
    DependencyGraphSpec GetDependencyGraphSpec(string project);

    /// <summary>
    /// Loads settings from the specified project directory.
    /// </summary>
    /// <param name="projectDirectory">The project or solution directory.</param>
    /// <returns>The settings for the provided solution or project.</returns>
    ISettings LoadSettings(string projectDirectory);

    /// <summary>
    /// Performs a restore preview operation without committing the result.
    /// </summary>
    /// <param name="dgSpec">The dependency graph specification.</param>
    /// <param name="cacheContext">The source cache context.</param>
    /// <param name="logger">Logger for the operation.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The restore result pair from the preview operation.</returns>
    Task<RestoreResult> PreviewUpdatePackageReferenceAsync(
        DependencyGraphSpec dgSpec,
        SourceCacheContext cacheContext,
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
    /// <param name="packageTfms">Target frameworks where the package is used.</param>
    /// <param name="restorePreviewResult">The restore preview result containing resolved package information.</param>
    /// <param name="packageDependency">Package dependency information.</param>
    /// <param name="logger">Logger for the operation.</param>
    void UpdatePackageReference(PackageSpec updatedPackageSpec, RestoreResult restorePreviewResult, List<NuGetFramework> packageTfms, PackageToUpdate packageDependency, ILogger logger);

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
