// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Packaging;
using NuGet.Versioning;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.UI
{
    public class DetailedPackageMetadata
    {
        public DetailedPackageMetadata()
        {
        }

        public DetailedPackageMetadata(PackageSearchMetadataContextInfo serverData, PackageDeprecationMetadataContextInfo deprecationMetadata, long? downloadCount)
        {
            Id = serverData.Identity.Id;
            Version = serverData.Identity.Version;
            Summary = serverData.Summary;
            Description = serverData.Description;
            Authors = serverData.Authors;
            Owners = serverData.Owners;
            IconUrl = serverData.IconUrl;
            LicenseUrl = serverData.LicenseUrl;
            ProjectUrl = serverData.ProjectUrl;
            ReadmeUrl = serverData.ReadmeUrl;
            ReportAbuseUrl = serverData.ReportAbuseUrl;
            // Some server implementations send down an array with an empty string, which ends up as an empty string.
            // In PM UI, we want Tags to work like most other properties from the server (Authors/Owners), and be null, if there is no value.
            Tags = string.IsNullOrEmpty(serverData.Tags) ? null : serverData.Tags;
            DownloadCount = downloadCount;
            Published = serverData.Published;

            IEnumerable<PackageDependencyGroup> dependencySets = serverData.DependencySets;
            if (dependencySets != null && dependencySets.Any())
            {
                DependencySets = dependencySets.Select(e => new PackageDependencySetMetadata(e)).ToArray();
            }
            else
            {
                DependencySets = NoDependenciesPlaceholder;
            }

            PrefixReserved = serverData.PrefixReserved;
            LicenseMetadata = serverData.LicenseMetadata;
            DeprecationMetadata = deprecationMetadata;
            Vulnerabilities = serverData.Vulnerabilities;
            PackagePath = serverData.PackagePath;

            // Determine the package details URL and text.
            PackageDetailsUrl = null;
            PackageDetailsText = null;
            if (serverData.PackageDetailsUrl != null
                && serverData.PackageDetailsUrl.IsAbsoluteUri
                && serverData.PackageDetailsUrl.Host != null)
            {
                PackageDetailsUrl = serverData.PackageDetailsUrl;
                PackageDetailsText = serverData.PackageDetailsUrl.Host;

                // Special case the subdomain "www." - we hide it. Other subdomains are not hidden.
                const string wwwDot = "www.";
                if (PackageDetailsText.StartsWith(wwwDot, StringComparison.OrdinalIgnoreCase)
                    && PackageDetailsText.Length > wwwDot.Length)
                {
                    PackageDetailsText = PackageDetailsText.Substring(wwwDot.Length);
                }
            }
        }

        public string Id { get; set; }

        public NuGetVersion Version { get; set; }

        public string Summary { get; set; }

        public string Description { get; set; }

        public string Authors { get; set; }

        public string Owners { get; set; }

        public Uri IconUrl { get; set; }

        public Uri LicenseUrl { get; set; }

        public Uri ProjectUrl { get; set; }

        public Uri ReadmeUrl { get; set; }

        public Uri ReportAbuseUrl { get; set; }

        public Uri PackageDetailsUrl { get; set; }

        public string PackageDetailsText { get; set; }

        public string Tags { get; set; }

        public long? DownloadCount { get; set; }

        public DateTimeOffset? Published { get; set; }

        public IEnumerable<PackageDependencySetMetadata> DependencySets { get; set; }

        public bool PrefixReserved { get; set; }

        public LicenseMetadata LicenseMetadata { get; set; }

        public PackageDeprecationMetadataContextInfo DeprecationMetadata { get; set; }

        public IEnumerable<PackageVulnerabilityMetadataContextInfo> Vulnerabilities { get; set; }

        public IReadOnlyList<IText> LicenseLinks => PackageLicenseUtilities.GenerateLicenseLinks(this);

        private static readonly IReadOnlyList<PackageDependencySetMetadata> NoDependenciesPlaceholder = new PackageDependencySetMetadata[] { new PackageDependencySetMetadata(dependencyGroup: null) };

        public string PackagePath { get; set; }
    }
}
