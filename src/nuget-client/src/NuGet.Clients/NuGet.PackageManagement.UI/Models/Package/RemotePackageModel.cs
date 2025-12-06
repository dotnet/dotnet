// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.UI.Models.Package
{
    internal class RemotePackageModel : PackageModel, IDeprecationCapable, IVulnerableCapable
    {
        private readonly IDeprecationCapable _deprecationCapability;
        private readonly IVulnerableCapable _vulnerableCapability;

        public RemotePackageModel(
            PackageIdentity identity,
            IVulnerableCapable vulnerableCapability,
            IDeprecationCapable deprecationCapability,
            IEmbeddedResourcesCapable embeddedResources,
            string? title,
            string? description,
            string? authors,
            Uri? projectUrl,
            string[]? tags,
            IReadOnlyList<string>? ownersList,
            IReadOnlyCollection<PackageDependencyGroup>? packageDependencyGroups,
            string? summary,
            DateTimeOffset? publishedDate,
            LicenseMetadata? licenseMetadata,
            Uri? licenseUrl,
            bool requireLicenseAcceptance,
            bool isListed,
            Uri? packageDetailsUrl,
            long? downloadCount,
            Uri? readmeUrl,
            Uri? iconUrl)
            : base(identity, embeddedResources, title, description, authors, projectUrl, tags, ownersList, packageDependencyGroups, summary, publishedDate, licenseMetadata, licenseUrl, requireLicenseAcceptance, iconUrl)
        {
            IsListed = isListed;
            PackageDetailsUrl = packageDetailsUrl;
            DownloadCount = downloadCount;
            _deprecationCapability = deprecationCapability ?? throw new ArgumentNullException(nameof(deprecationCapability));
            _vulnerableCapability = vulnerableCapability ?? throw new ArgumentNullException(nameof(vulnerableCapability));
            ReadmeUrl = readmeUrl;
        }

        public bool IsListed { get; }
        public Uri? PackageDetailsUrl { get; }
        public long? DownloadCount { get; }
        public Uri? ReadmeUrl { get; }

        public bool IsDeprecated => _deprecationCapability.IsDeprecated;

        public PackageDeprecationReason PackageDeprecationReasons => _deprecationCapability.PackageDeprecationReasons;

        public AlternatePackageMetadataContextInfo? AlternatePackage => _deprecationCapability.AlternatePackage;

        public IReadOnlyList<PackageVulnerabilityMetadataContextInfo>? Vulnerabilities => _vulnerableCapability.Vulnerabilities;

        public bool IsVulnerable => _vulnerableCapability.IsVulnerable;

        public PackageVulnerabilitySeverity VulnerabilityMaxSeverity => _vulnerableCapability.VulnerabilityMaxSeverity;

        public PackageDeprecationMetadataContextInfo? DeprecationMetadata => _deprecationCapability.DeprecationMetadata;

        public override async Task PopulateDataAsync(CancellationToken cancellationToken)
        {
            await _vulnerableCapability.PopulateDataAsync(cancellationToken);
            await _deprecationCapability.PopulateDataAsync(cancellationToken);
        }
    }
}
