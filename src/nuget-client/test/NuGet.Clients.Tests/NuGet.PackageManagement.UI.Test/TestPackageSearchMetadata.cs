// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace NuGet.PackageManagement.UI.Test
{
    /// <summary>
    /// Test implementation of <see cref="IPackageSearchMetadata"/>. Not for production use
    /// </summary>
    class TestPackageSearchMetadata : IPackageSearchMetadata
    {
        public string Authors { get; set; }

        public IEnumerable<PackageDependencyGroup> DependencySets { get; set; }

        public string Description { get; set; }

        public long? DownloadCount { get; set; }

        public Uri IconUrl { get; set; }

        public PackageIdentity Identity { get; set; }

        public Uri LicenseUrl { get; set; }

        public Uri ProjectUrl { get; set; }

        public Uri ReadmeUrl { get; set; }

        public Uri ReportAbuseUrl { get; set; }

        public Uri PackageDetailsUrl { get; set; }

        public string PackagePath { get; set; }

        public DateTimeOffset? Published { get; set; }

        public string Owners { get; set; }

        public bool RequireLicenseAcceptance { get; set; }

        public string Summary { get; set; }

        public string Tags { get; set; }

        public string Title { get; set; }

        public bool IsListed { get; set; }

        public bool PrefixReserved { get; set; }

        public LicenseMetadata LicenseMetadata { get; set; }

        public Task<PackageDeprecationMetadata> GetDeprecationMetadataAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<VersionInfo>> GetVersionsAsync()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PackageVulnerabilityMetadata> Vulnerabilities { get; set; }
    }
}
