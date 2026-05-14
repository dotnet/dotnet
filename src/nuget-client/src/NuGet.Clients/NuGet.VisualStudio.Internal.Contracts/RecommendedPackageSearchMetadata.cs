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
    public class RecommendedPackageSearchMetadata : IPackageSearchMetadata
    {
        private readonly IPackageSearchMetadata _inner;
        public bool IsRecommended { get; }
        public (string, string)? RecommenderVersion { get; }

        public RecommendedPackageSearchMetadata(IPackageSearchMetadata inner, (string, string)? recommenderVersion)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
            IsRecommended = true;
            RecommenderVersion = recommenderVersion;
        }

        // Implement IPackageSearchMetadata by delegating to the inner instance
        public string Authors => _inner.Authors;
        public IEnumerable<PackageDependencyGroup> DependencySets => _inner.DependencySets;
        public string Description => _inner.Description;
        public long? DownloadCount => _inner.DownloadCount;
        public Uri IconUrl => _inner.IconUrl;
        public PackageIdentity Identity => _inner.Identity;
        public Uri LicenseUrl => _inner.LicenseUrl;
        public Uri ProjectUrl => _inner.ProjectUrl;
        public string ReadmeFileUrl => _inner.ReadmeFileUrl;
        public Uri ReadmeUrl => _inner.ReadmeUrl;
        public Uri ReportAbuseUrl => _inner.ReportAbuseUrl;
        public Uri PackageDetailsUrl => _inner.PackageDetailsUrl;
        public DateTimeOffset? Published => _inner.Published;
        public IReadOnlyList<string> OwnersList => _inner.OwnersList;
        public string Owners => _inner.Owners;
        public bool RequireLicenseAcceptance => _inner.RequireLicenseAcceptance;
        public string Summary => _inner.Summary;
        public string Tags => _inner.Tags;
        public string Title => _inner.Title;
        public bool IsListed => _inner.IsListed;
        public bool PrefixReserved => _inner.PrefixReserved;
        public LicenseMetadata LicenseMetadata => _inner.LicenseMetadata;
        public Task<PackageDeprecationMetadata> GetDeprecationMetadataAsync() => _inner.GetDeprecationMetadataAsync();

        public Task<IEnumerable<VersionInfo>> GetVersionsAsync() => _inner.GetVersionsAsync();

        public IEnumerable<PackageVulnerabilityMetadata> Vulnerabilities => _inner.Vulnerabilities;
    }
}
