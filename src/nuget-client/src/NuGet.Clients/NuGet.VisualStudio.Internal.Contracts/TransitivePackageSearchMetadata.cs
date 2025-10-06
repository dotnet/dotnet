// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace NuGet.VisualStudio.Internal.Contracts
{
    public class TransitivePackageSearchMetadata : IPackageSearchMetadata
    {
        public IReadOnlyCollection<PackageIdentity> TransitiveOrigins { get; private set; }

        public string Authors => _packageSearchMetadata.Authors;

        public IEnumerable<PackageDependencyGroup> DependencySets => _packageSearchMetadata.DependencySets;

        public string Description => _packageSearchMetadata.Description;

        public long? DownloadCount => _packageSearchMetadata.DownloadCount;

        public Uri IconUrl => _packageSearchMetadata.IconUrl;

        public PackageIdentity Identity => _packageSearchMetadata.Identity;

        public Uri LicenseUrl => _packageSearchMetadata.LicenseUrl;

        public Uri ProjectUrl => _packageSearchMetadata.ProjectUrl;

        public Uri ReadmeUrl => _packageSearchMetadata.ReadmeUrl;

        public string ReadmeFileUrl => _packageSearchMetadata.ReadmeFileUrl;

        public Uri ReportAbuseUrl => _packageSearchMetadata.ReportAbuseUrl;

        public Uri PackageDetailsUrl => _packageSearchMetadata.PackageDetailsUrl;

        public DateTimeOffset? Published => _packageSearchMetadata.Published;

        public IReadOnlyList<string> OwnersList => _packageSearchMetadata.OwnersList;

        public string Owners => _packageSearchMetadata.Owners;

        public bool RequireLicenseAcceptance => _packageSearchMetadata.RequireLicenseAcceptance;

        public string Summary => _packageSearchMetadata.Summary;

        public string Tags => _packageSearchMetadata.Tags;

        public string Title => _packageSearchMetadata.Title;

        public bool IsListed => _packageSearchMetadata.IsListed;

        public bool PrefixReserved => _packageSearchMetadata.PrefixReserved;

        public LicenseMetadata LicenseMetadata => _packageSearchMetadata.LicenseMetadata;

        public IEnumerable<PackageVulnerabilityMetadata> Vulnerabilities => _packageSearchMetadata.Vulnerabilities;

        private readonly IPackageSearchMetadata _packageSearchMetadata;

        public TransitivePackageSearchMetadata(IPackageSearchMetadata package, IReadOnlyCollection<PackageIdentity> transitiveOrigins)
        {
            _packageSearchMetadata = package ?? throw new ArgumentNullException(nameof(package));
            TransitiveOrigins = transitiveOrigins ?? throw new ArgumentNullException(nameof(transitiveOrigins));
        }

        public Task<PackageDeprecationMetadata> GetDeprecationMetadataAsync()
        {
            return _packageSearchMetadata.GetDeprecationMetadataAsync();
        }

        public Task<IEnumerable<VersionInfo>> GetVersionsAsync()
        {
            return _packageSearchMetadata.GetVersionsAsync();
        }
    }
}
