// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Packaging;
using NuGet.Packaging.Core;

namespace NuGet.PackageManagement.UI.Models.Package
{
    internal class RecommendedPackageModel : RemotePackageModel
    {
        public RecommendedPackageModel(
            PackageIdentity identity,
            IVulnerableCapable vulnerableCapability,
            IDeprecationCapable deprecationCapability,
            IEmbeddedResourcesCapable embeddedResources,
            (string modelVersion, string vsixVersion) recommenderVersion,
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
            : base(identity, vulnerableCapability, deprecationCapability, embeddedResources, title, description, authors, projectUrl, tags, ownersList, packageDependencyGroups, summary, publishedDate, licenseMetadata, licenseUrl, requireLicenseAcceptance, isListed, packageDetailsUrl, downloadCount, readmeUrl, iconUrl)
        {
            RecommenderVersion = recommenderVersion;
        }

        public (string modelVersion, string vsixVersion) RecommenderVersion { get; }
    }
}

