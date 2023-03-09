// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Frameworks;
using NuGet.PackageManagement;
using NuGet.Packaging.Core;
using NuGet.Resolver;
using NuGet.Versioning;

namespace NuGet.VisualStudio.Internal.Contracts
{
    public interface INuGetProjectManagerService : IDisposable
    {
        ValueTask<IReadOnlyCollection<IPackageReferenceContextInfo>> GetInstalledPackagesAsync(
            IReadOnlyCollection<string> projectIds,
            CancellationToken cancellationToken);
        ValueTask<IInstalledAndTransitivePackages> GetInstalledAndTransitivePackagesAsync(
            IReadOnlyCollection<string> projectIds,
            CancellationToken cancellationToken);

        /// <summary>
        /// Obtains the installed and transitive packages from all given projects, optionally including transitive origins for transitive packages.
        /// </summary>
        /// <param name="projectIds">Projects to retrieve installed and transitive packages</param>
        /// <param name="includeTransitiveOrigins">Set it to <c>true</c> to get transitive origins of each transitive package</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>An object with two lists: installed and transitive packages from all projects</returns>
        ValueTask<IInstalledAndTransitivePackages> GetInstalledAndTransitivePackagesAsync(
            IReadOnlyCollection<string> projectIds,
            bool includeTransitiveOrigins,
            CancellationToken cancellationToken);
        ValueTask<IReadOnlyCollection<NuGetFramework>> GetTargetFrameworksAsync(
            IReadOnlyCollection<string> projectIds,
            CancellationToken cancellationToken);
        ValueTask<IReadOnlyCollection<PackageDependencyInfo>> GetInstalledPackagesDependencyInfoAsync(
            string projectId,
            bool includeUnresolved,
            CancellationToken cancellationToken);
        ValueTask<bool> IsCentralPackageManagementEnabledAsync(string projectId, CancellationToken cancellationToken);
        ValueTask<IProjectMetadataContextInfo> GetMetadataAsync(string projectId, CancellationToken cancellationToken);
        ValueTask<IProjectContextInfo> GetProjectAsync(string projectId, CancellationToken cancellationToken);
        ValueTask<IReadOnlyCollection<IProjectContextInfo>> GetProjectsAsync(CancellationToken cancellationToken);
        ValueTask<(bool, string?)> TryGetInstalledPackageFilePathAsync(
            string projectId,
            PackageIdentity packageIdentity,
            CancellationToken cancellationToken);
        ValueTask<IReadOnlyCollection<IProjectContextInfo>> GetProjectsWithDeprecatedDotnetFrameworkAsync(
            CancellationToken cancellationToken);

        ValueTask BeginOperationAsync(CancellationToken cancellationToken);
        ValueTask EndOperationAsync(CancellationToken cancellationToken);
        ValueTask ExecuteActionsAsync(IReadOnlyList<ProjectAction> actions, CancellationToken cancellationToken);

        ValueTask<IReadOnlyList<ProjectAction>> GetInstallActionsAsync(
            IReadOnlyCollection<string> projectIds,
            PackageIdentity packageIdentity,
            VersionConstraints versionConstraints,
            bool includePrelease,
            DependencyBehavior dependencyBehavior,
            IReadOnlyList<string> packageSourceNames,
            CancellationToken cancellationToken);

        ValueTask<IReadOnlyList<ProjectAction>> GetInstallActionsAsync(
            IReadOnlyCollection<string> projectIds,
            PackageIdentity packageIdentity,
            VersionConstraints versionConstraints,
            bool includePrelease,
            DependencyBehavior dependencyBehavior,
            IReadOnlyList<string> packageSourceNames,
            VersionRange versionRange,
            CancellationToken cancellationToken);

        ValueTask<IReadOnlyList<ProjectAction>> GetUninstallActionsAsync(
            IReadOnlyCollection<string> projectIds,
            PackageIdentity packageIdentity,
            bool removeDependencies,
            bool forceRemove,
            CancellationToken cancellationToken);

        ValueTask<IReadOnlyList<ProjectAction>> GetUpdateActionsAsync(
            IReadOnlyCollection<string> projectIds,
            IReadOnlyCollection<PackageIdentity> packageIdentities,
            VersionConstraints versionConstraints,
            bool includePrelease,
            DependencyBehavior dependencyBehavior,
            IReadOnlyList<string> packageSourceNames,
            CancellationToken cancellationToken);

        /// <summary>
        /// Get package folders section from assets file from a collection of projects
        /// </summary>
        /// <param name="projectIds">A collection of project ID's</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A collection with all package folders found in each project assets file, deduplicated</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="projectIds"/> is <c>null</c></exception>
        /// <remarks><see cref="PackageManagement.VisualStudio.IPackageReferenceProject.GetPackageFoldersAsync(CancellationToken)"/></remarks>
        ValueTask<IReadOnlyCollection<string>> GetPackageFoldersAsync(
            IReadOnlyCollection<string> projectIds,
            CancellationToken cancellationToken);
    }
}
