// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Model;
using NuGet.Versioning;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Helper class encapsulating common scenarios of source repository operations.
    /// </summary>
    internal static class SourceRepositoryExtensions
    {
        public static Task<SearchResult<IPackageSearchMetadata>> SearchAsync(this SourceRepository sourceRepository, string searchText, SearchFilter searchFilter, int pageSize, CancellationToken cancellationToken)
        {
            var searchToken = new FeedSearchContinuationToken
            {
                SearchString = searchText,
                SearchFilter = searchFilter,
                StartIndex = 0
            };

            return sourceRepository.SearchAsync(searchToken, pageSize, cancellationToken);
        }

        public static async Task<SearchResult<IPackageSearchMetadata>> SearchAsync(
            this SourceRepository sourceRepository, ContinuationToken continuationToken, int pageSize, CancellationToken cancellationToken)
        {
            var stopWatch = Stopwatch.StartNew();

            var searchToken = continuationToken as FeedSearchContinuationToken ?? throw new InvalidOperationException(Strings.Exception_InvalidContinuationToken);

            var searchResource = await sourceRepository.GetResourceAsync<PackageSearchResource>(cancellationToken);

            var searchResults = await searchResource?.SearchAsync(
                searchToken.SearchString,
                searchToken.SearchFilter,
                searchToken.StartIndex,
                pageSize + 1,
                Common.NullLogger.Instance,
                cancellationToken);

            var items = searchResults?.ToArray() ?? Array.Empty<IPackageSearchMetadata>();

            var hasMoreItems = items.Length > pageSize;
            if (hasMoreItems)
            {
                items = items.Take(items.Length - 1).ToArray();
            }

            var result = SearchResult.FromItems(items);

            var loadingStatus = hasMoreItems
                ? LoadingStatus.Ready
                : items.Length == 0
                ? LoadingStatus.NoItemsFound
                : LoadingStatus.NoMoreItems;
            result.SourceSearchStatus = new Dictionary<string, LoadingStatus>
            {
                { sourceRepository.PackageSource.Name, loadingStatus }
            };

            if (hasMoreItems)
            {
                result.NextToken = new FeedSearchContinuationToken
                {
                    SearchString = searchToken.SearchString,
                    SearchFilter = searchToken.SearchFilter,
                    StartIndex = searchToken.StartIndex + items.Length
                };
            }
            stopWatch.Stop();
            result.Duration = stopWatch.Elapsed;

            return result;
        }

        /// <summary>
        /// Get the package metadata for the given identity
        /// </summary>
        public static async Task<IPackageSearchMetadata> GetPackageMetadataForIdentityAsync(this SourceRepository sourceRepository, PackageIdentity identity, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var metadataResource = await sourceRepository.GetResourceAsync<PackageMetadataResource>(cancellationToken);

            if (metadataResource == null)
            {
                return null;
            }

            using (var sourceCacheContext = new SourceCacheContext())
            {
                // Update http source cache context MaxAge so that it can always go online to fetch latest version of packages.
                sourceCacheContext.MaxAge = DateTimeOffset.UtcNow;

                return await metadataResource.GetMetadataAsync(identity, sourceCacheContext, Common.NullLogger.Instance, cancellationToken);
            }
        }

        /// <summary>
        /// Get the package metadata for the given identity.Id
        /// </summary>
        public static async Task<IPackageSearchMetadata> GetPackageMetadataAsync(
            this SourceRepository sourceRepository, PackageIdentity identity, bool includePrerelease, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var metadataResource = await sourceRepository.GetResourceAsync<PackageMetadataResource>(cancellationToken);

            if (metadataResource == null)
            {
                return null;
            }

            using (var sourceCacheContext = new SourceCacheContext())
            {
                // Update http source cache context MaxAge so that it can always go online to fetch
                // latest version of packages.
                sourceCacheContext.MaxAge = DateTimeOffset.UtcNow;

                var packages = await metadataResource.GetMetadataAsync(
                    identity.Id,
                    includePrerelease: true,
                    includeUnlisted: false,
                    sourceCacheContext: sourceCacheContext,
                    log: Common.NullLogger.Instance,
                    token: cancellationToken);

                if (packages?.FirstOrDefault() == null)
                {
                    return null;
                }

                var packageMetadata = packages
                    .FirstOrDefault(p => p.Identity.Version == identity.Version)
                    ?? PackageSearchMetadataBuilder.FromIdentity(identity).Build();

                return packageMetadata.WithVersions(ToVersionInfo(packages, includePrerelease));
            }
        }

        public static async Task<IPackageSearchMetadata> GetPackageMetadataFromLocalSourceAsync(
            this SourceRepository localRepository, PackageIdentity identity, CancellationToken cancellationToken)
        {
            var localResource = await localRepository.GetResourceAsync<PackageMetadataResource>(cancellationToken);

            using (var sourceCacheContext = new SourceCacheContext())
            {
                var localPackages = await localResource?.GetMetadataAsync(
                    identity.Id,
                    includePrerelease: true,
                    includeUnlisted: true,
                    sourceCacheContext: sourceCacheContext,
                    log: Common.NullLogger.Instance,
                    token: cancellationToken);

                var packageMetadata = localPackages?.FirstOrDefault(p => p.Identity.Version == identity.Version);

                var versions = new[]
                {
                new VersionInfo(identity.Version)
                };

                return packageMetadata?.WithVersions(versions);
            }
        }

        public static async Task<IPackageSearchMetadata> GetLatestPackageMetadataAsync(
            this SourceRepository sourceRepository, string packageId, bool includePrerelease, CancellationToken cancellationToken, VersionRange allowedVersions)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var metadataResource = await sourceRepository.GetResourceAsync<PackageMetadataResource>(cancellationToken);

            using (var sourceCacheContext = new SourceCacheContext())
            {
                // Update http source cache context MaxAge so that it can always go online to fetch
                // latest version of packages.
                sourceCacheContext.MaxAge = DateTimeOffset.UtcNow;

                var packages = await metadataResource?.GetMetadataAsync(
                    packageId,
                    includePrerelease,
                    false,
                    sourceCacheContext,
                    Common.NullLogger.Instance,
                    cancellationToken);

                // filter packages based on allowed versions
                var updatedPackages = packages.Where(p => allowedVersions.Satisfies(p.Identity.Version));

                var highest = updatedPackages
                    .OrderByDescending(e => e.Identity.Version, VersionComparer.VersionRelease)
                    .FirstOrDefault();

                return highest?.WithVersions(ToVersionInfo(packages, includePrerelease));
            }
        }

        public static async Task<IEnumerable<IPackageSearchMetadata>> GetPackageMetadataListAsync(
            this SourceRepository sourceRepository, string packageId, bool includePrerelease, bool includeUnlisted, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var metadataResource = await sourceRepository.GetResourceAsync<PackageMetadataResource>(cancellationToken);

            using (var sourceCacheContext = new SourceCacheContext())
            {
                // Update http source cache context MaxAge so that it can always go online to fetch
                // latest versions of the package.
                sourceCacheContext.MaxAge = DateTimeOffset.UtcNow;

                var packages = await metadataResource?.GetMetadataAsync(
                    packageId,
                    includePrerelease,
                    includeUnlisted,
                    sourceCacheContext,
                    Common.NullLogger.Instance,
                    cancellationToken);

                return packages;
            }
        }

        public static async Task<GetVulnerabilityInfoResult> GetVulnerabilityInfoAsync(this SourceRepository sourceRepository, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var vulnerabilityResource = await sourceRepository.GetResourceAsync<IVulnerabilityInfoResource>(cancellationToken);
            if (vulnerabilityResource is null)
            {
                return null;
            }

            using (var sourceCacheContext = new SourceCacheContext())
            {
                return await vulnerabilityResource.GetVulnerabilityInfoAsync(sourceCacheContext, Common.NullLogger.Instance, cancellationToken);
            }
        }

        private static IEnumerable<VersionInfo> ToVersionInfo(IEnumerable<IPackageSearchMetadata> packages, bool includePrerelease)
        {
            return packages?
                .Where(v => includePrerelease || !v.Identity.Version.IsPrerelease)
                .OrderByDescending(m => m.Identity.Version, VersionComparer.VersionRelease)
                .Select(m => new VersionInfo(m.Identity.Version, m.DownloadCount)
                {
                    PackageSearchMetadata = m
                });
        }

        public static async Task<IEnumerable<string>> IdStartsWithAsync(
            this SourceRepository sourceRepository, string packageIdPrefix, bool includePrerelease, CancellationToken cancellationToken)
        {
            var autoCompleteResource = await sourceRepository.GetResourceAsync<AutoCompleteResource>(cancellationToken);
            var packageIds = await autoCompleteResource?.IdStartsWith(
                packageIdPrefix,
                includePrerelease: includePrerelease,
                log: Common.NullLogger.Instance,
                token: cancellationToken);

            return packageIds ?? Enumerable.Empty<string>();
        }

        public static async Task<IEnumerable<NuGetVersion>> VersionStartsWithAsync(
            this SourceRepository sourceRepository, string packageId, string versionPrefix, bool includePrerelease, CancellationToken cancellationToken)
        {
            var autoCompleteResource = await sourceRepository.GetResourceAsync<AutoCompleteResource>(cancellationToken);

            using (var sourceCacheContext = new SourceCacheContext())
            {
                var versions = await autoCompleteResource?.VersionStartsWith(
                    packageId,
                    versionPrefix,
                    includePrerelease: includePrerelease,
                    sourceCacheContext: sourceCacheContext,
                    log: Common.NullLogger.Instance,
                    token: cancellationToken);

                return versions ?? Enumerable.Empty<NuGetVersion>();
            }
        }
    }
}
