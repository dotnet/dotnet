// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.Protocol
{
    public class PackageSearchMetadataV2Feed : IPackageSearchMetadata
    {
        public PackageSearchMetadataV2Feed(V2FeedPackageInfo package)
        {
            Authors = string.Join(", ", package.Authors);
            DependencySets = package.DependencySets;
            Description = package.Description;
            IconUrl = GetUriSafe(package.IconUrl);
            LicenseUrl = GetUriSafe(package.LicenseUrl);
            Owners = string.Join(", ", package.Owners);
            PackageId = package.Id;
            ProjectUrl = GetUriSafe(package.ProjectUrl);
            Created = package.Created;
            LastEdited = package.LastEdited;
            Published = package.Published;
            ReportAbuseUrl = GetUriSafe(package.ReportAbuseUrl);
            PackageDetailsUrl = GetUriSafe(package.GalleryDetailsUrl);
            RequireLicenseAcceptance = package.RequireLicenseAcceptance;
            Summary = package.Summary;
            Tags = package.Tags;
            Title = package.Title;
            Version = package.Version;
            IsListed = package.IsListed;

            long count;
            if (long.TryParse(package.DownloadCount, out count))
            {
                DownloadCount = count;
            }
        }
        public PackageSearchMetadataV2Feed(V2FeedPackageInfo package, MetadataReferenceCache metadataCache)
        {
            Authors = metadataCache.GetString(string.Join(", ", package.Authors));
            DependencySets = package.DependencySets;
            Description = package.Description;
            IconUrl = GetUriSafe(package.IconUrl);
            LicenseUrl = GetUriSafe(package.LicenseUrl);
            _ownersList = (IReadOnlyList<string>)package.Owners;
            Owners = metadataCache.GetString(string.Join(", ", package.Owners));
            PackageId = package.Id;
            ProjectUrl = GetUriSafe(package.ProjectUrl);
            Created = package.Created;
            LastEdited = package.LastEdited;
            Published = package.Published;
            ReportAbuseUrl = GetUriSafe(package.ReportAbuseUrl);
            PackageDetailsUrl = GetUriSafe(package.GalleryDetailsUrl);
            RequireLicenseAcceptance = package.RequireLicenseAcceptance;
            Summary = package.Summary;
            Tags = package.Tags;
            Title = package.Title;
            Version = package.Version;
            IsListed = package.IsListed;

            long count;
            if (long.TryParse(package.DownloadCount, out count))
            {
                DownloadCount = count;
            }
        }

        public string Authors { get; private set; }

        public IEnumerable<PackageDependencyGroup> DependencySets { get; private set; }

        public string Description { get; private set; }

        public long? DownloadCount { get; private set; }

        public Uri IconUrl { get; private set; }

        public PackageIdentity Identity => new PackageIdentity(PackageId, Version);

        public Uri LicenseUrl { get; private set; }

        private IReadOnlyList<string> _ownersList;
        public IReadOnlyList<string> OwnersList => _ownersList;

        public string Owners { get; private set; }

        public string PackageId { get; private set; }

        public Uri ProjectUrl { get; private set; }

        // Prefix Reservation should never be shown on a V2 Feed
        public bool PrefixReserved => false;

        public DateTimeOffset? Created { get; private set; }

        public DateTimeOffset? LastEdited { get; private set; }

        public DateTimeOffset? Published { get; private set; }

        public Uri ReadmeUrl { get; } = null; // The ReadmeUrl has not been added to the V2 feed.

        public string ReadmeFileUrl { get; } = null; // The ReadmeFileUrl has not been added to the V2 feed.

        public Uri ReportAbuseUrl { get; private set; }

        public Uri PackageDetailsUrl { get; private set; }

        public bool RequireLicenseAcceptance { get; private set; }

        private string _summaryValue;
        public string Summary
        {
            get { return !string.IsNullOrEmpty(_summaryValue) ? _summaryValue : Description; }
            private set { _summaryValue = value; }
        }

        public string Tags { get; private set; }

        private string _titleValue;

        public string Title
        {
            get { return !string.IsNullOrEmpty(_titleValue) ? _titleValue : PackageId; }
            private set { _titleValue = value; }
        }

        public LicenseMetadata LicenseMetadata { get; } = null; // The LicenseExpression is not added to the V2 feed.

        public PackageDeprecationMetadata DeprecationMetadata { get; } = null; // Deprecation metadata is not added to the v2 feed.

        public NuGetVersion Version { get; private set; }

        /// <inheritdoc cref="IPackageSearchMetadata.GetVersionsAsync" />
        public Task<IEnumerable<VersionInfo>> GetVersionsAsync() => TaskResult.EmptyEnumerable<VersionInfo>();

        private static Uri GetUriSafe(string url)
        {
            Uri uri = null;
            Uri.TryCreate(url, UriKind.Absolute, out uri);
            return uri;
        }

        /// <inheritdoc cref="IPackageSearchMetadata.GetDeprecationMetadataAsync" />
        public Task<PackageDeprecationMetadata> GetDeprecationMetadataAsync() => TaskResult.Null<PackageDeprecationMetadata>();

        /// <inheritdoc cref="IPackageSearchMetadata.Vulnerabilities" />
        public IEnumerable<PackageVulnerabilityMetadata> Vulnerabilities { get; } = null; // Vulnerability metadata is not added to nuget.org's v2 feed.

        public bool IsListed { get; }
    }
}
