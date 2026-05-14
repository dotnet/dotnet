// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Packaging;
using NuGet.Packaging.Core;

namespace NuGet.PackageManagement.UI.Models.Package
{
    internal class LocalPackageModel : PackageModel
    {
        public LocalPackageModel(PackageIdentity identity,
            string packagePath,
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
            Uri? iconUrl)
            : base(identity, embeddedResources, title, description, authors, projectUrl, tags, ownersList, packageDependencyGroups, summary, publishedDate, licenseMetadata, licenseUrl, requireLicenseAcceptance, iconUrl)
        {
            PackagePath = packagePath;
        }

        public string PackagePath { get; }

        public override Task PopulateDataAsync(CancellationToken cancellationToken)
        {
            // LocalPackageModel does not need to populate any additional data.
            return Task.CompletedTask;
        }
    }
}
