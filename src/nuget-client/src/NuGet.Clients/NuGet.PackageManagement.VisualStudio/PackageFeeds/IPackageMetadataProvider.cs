// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.Protocol.Core.Types;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Contract for a generalized package metadata provider associated with a package source(s).
    /// </summary>
    public interface IPackageMetadataProvider
    {
        /// <summary>
        /// Retrieves a package metadata of a specific version along with list of all available versions
        /// </summary>
        /// <param name="identity">Desired package id with version</param>
        /// <param name="includePrerelease">Filters pre-release versions</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Package metadata</returns>
        Task<IPackageSearchMetadata> GetPackageMetadataAsync(PackageIdentity identity,
            bool includePrerelease, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a package metadata of a highest available version along with list of all available versions
        /// </summary>
        /// <param name="identity">Desired package identity</param>
        /// <param name="project">Project reference to determine the latest version</param>
        /// <param name="includePrerelease">Filters pre-release versions</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>Package metadata</returns>
        Task<IPackageSearchMetadata> GetLatestPackageMetadataAsync(PackageIdentity identity,
            NuGetProject project, bool includePrerelease, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a list of metadata objects of all available versions for given package id.
        /// </summary>
        /// <param name="packageId">Desired package Id</param>
        /// <param name="includePrerelease">Filters pre-release versions</param>
        /// <param name="includeUnlisted">Filters unlisted versions</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>Collection of packages matching query parameters</returns>
        Task<IEnumerable<IPackageSearchMetadata>> GetPackageMetadataListAsync(string packageId,
            bool includePrerelease, bool includeUnlisted, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a package metadata of a specific version along with list of all available versions
        /// </summary>
        /// <param name="identity">Desired package id with version</param>
        /// <param name="includePrerelease">Filters pre-release versions</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Package metadata</returns>
        Task<IPackageSearchMetadata> GetLocalPackageMetadataAsync(PackageIdentity identity,
            bool includePrerelease, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a package metadata of a specific version without a list of all available versions
        /// </summary>
        /// <param name="identity">Desired package id with version</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Package metadata</returns>
        Task<IPackageSearchMetadata> GetPackageMetadataForIdentityAsync(PackageIdentity identity, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves package metadata only from local sources
        /// </summary>
        /// <param name="identity">Desired package id with version</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Package metadata, or <see langword="null" /> if not found in any local source</returns>
        Task<IPackageSearchMetadata> GetOnlyLocalPackageMetadataAsync(PackageIdentity identity, CancellationToken cancellationToken);
    }
}
