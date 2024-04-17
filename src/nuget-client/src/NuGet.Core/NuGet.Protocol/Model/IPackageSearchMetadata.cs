// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NuGet.Packaging;
using NuGet.Packaging.Core;

namespace NuGet.Protocol.Core.Types
{
    /// <summary>
    /// Package metadata only containing select fields relevant to search results processing and presenting.
    /// Immutable.
    /// </summary>
    public interface IPackageSearchMetadata
    {
        string Authors { get; }
        IEnumerable<PackageDependencyGroup> DependencySets { get; }
        string Description { get; }
        long? DownloadCount { get; }
        Uri IconUrl { get; }
        PackageIdentity Identity { get; }
        Uri LicenseUrl { get; }
        Uri ProjectUrl { get; }
        Uri ReadmeUrl { get; }
        Uri ReportAbuseUrl { get; }
        Uri PackageDetailsUrl { get; }
        DateTimeOffset? Published { get; }
        IReadOnlyList<string> OwnersList { get; }
        string Owners { get; }
        bool RequireLicenseAcceptance { get; }
        string Summary { get; }
        string Tags { get; }
        string Title { get; }

        bool IsListed { get; }
        bool PrefixReserved { get; }

        LicenseMetadata LicenseMetadata { get; }

        /// <summary>
        /// Gets the deprecation metadata for the package.
        /// </summary>
        /// <remarks>
        /// Deprecation metadata is only available through remote feeds, not local feeds. Some servers do not return deprecation information via
        /// <see cref="PackageSearchResource" /> results, only through <see cref="PackageMetadataResource" /> or <see cref="FindPackageByIdResource" />.
        /// </remarks>
        Task<PackageDeprecationMetadata> GetDeprecationMetadataAsync();

        /// <summary>
        /// Lists the available versions of the package on the source.
        /// </summary>
        Task<IEnumerable<VersionInfo>> GetVersionsAsync();

        /// <summary>
        /// Gets the vulnerability metadata for the package.
        /// </summary>
        /// <remarks>
        /// Vulnerability metadata is only available through remote feeds, not local feeds. Some servers do not return vulnerability information via
        /// <see cref="PackageSearchResource" /> results, only through <see cref="PackageMetadataResource" /> or <see cref= "FindPackageByIdResource" />.
        /// </remarks>
        IEnumerable<PackageVulnerabilityMetadata> Vulnerabilities { get; }
    }
}
