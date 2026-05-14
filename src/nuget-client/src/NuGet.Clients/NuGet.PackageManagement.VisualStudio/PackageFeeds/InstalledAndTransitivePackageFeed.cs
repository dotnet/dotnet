// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.VisualStudio
{
    public sealed class InstalledAndTransitivePackageFeed : InstalledPackageFeed
    {
        private readonly IEnumerable<PackageCollectionItem> _transitivePackages;

        public InstalledAndTransitivePackageFeed(IEnumerable<PackageCollectionItem> installedPackages, IEnumerable<PackageCollectionItem> transitivePackages, IPackageMetadataProvider metadataProvider)
            : base(installedPackages, metadataProvider)
        {
            _transitivePackages = transitivePackages ?? throw new ArgumentNullException(nameof(transitivePackages));
        }

        /// <inheritdoc cref="IPackageFeed.ContinueSearchAsync(ContinuationToken, CancellationToken)" />
        public override async Task<SearchResult<IPackageSearchMetadata>> ContinueSearchAsync(ContinuationToken continuationToken, CancellationToken cancellationToken)
        {
            var searchToken = continuationToken as FeedSearchContinuationToken ?? throw new InvalidOperationException(Strings.Exception_InvalidContinuationToken);
            cancellationToken.ThrowIfCancellationRequested();

            // Remove transitive packages from project references. Those transitive packages don't have Transitive Origins
            IEnumerable<PackageCollectionItem> transitivePkgsWithOrigins = GetTransitivePackagesWithOrigins(_transitivePackages);

            // Get the metadata for the latest version of the installed packages and transitive packages
            IEnumerable<PackageCollectionItem> latestInstalledPackages = GetLatestPackages(_installedPackages);
            IEnumerable<PackageCollectionItem> latestTransitivePackages = GetLatestPackages(transitivePkgsWithOrigins);

            var installedItems = await GetMetadataForPackagesAsync(PerformLookup(latestInstalledPackages, searchToken), searchToken.SearchFilter.IncludePrerelease, cancellationToken);
            var transitiveItems = await GetMetadataForPackagesAsync(PerformLookup(latestTransitivePackages, searchToken), searchToken.SearchFilter.IncludePrerelease, cancellationToken);

            // Get metadata from identity for the rest of the installed and transitive packages
            var remainingInstalledItems = await GetRemainingPackagesMetadataAsync(_installedPackages, searchToken, cancellationToken);
            var remainingTransitiveItems = await GetRemainingPackagesMetadataAsync(transitivePkgsWithOrigins, searchToken, cancellationToken);

            // Combine the installed and transitive packages
            IPackageSearchMetadata[] items = installedItems
                .Concat(remainingInstalledItems)
                .Concat(transitiveItems)
                .Concat(remainingTransitiveItems)
                .ToArray();

            return CreateResult(items);
        }

        private static IEnumerable<PackageCollectionItem> GetTransitivePackagesWithOrigins(IEnumerable<PackageCollectionItem> transitivePackages)
        {
            return transitivePackages
                .Where(t => t.PackageReferences.All(x => x is ITransitivePackageReferenceContextInfo y && y.TransitiveOrigins.Any()));
        }

        private static IEnumerable<PackageCollectionItem> GetLatestPackages(IEnumerable<PackageCollectionItem> packages)
        {
            return packages
                .GroupById()
                .Select(g => g.OrderByDescending(x => x.Version).First());
        }

        private static async Task<IPackageSearchMetadata[]> GetRemainingPackagesMetadataAsync(IEnumerable<PackageCollectionItem> packages, FeedSearchContinuationToken searchToken, CancellationToken cancellationToken)
        {
            var remainingPackages = packages
                .GroupById()
                .Select(g => g.OrderByDescending(x => x.Version).Skip(1)) // Skip 1 to get the remaining packages, latest package is already processed
                .SelectMany(s => s);

            return await GetMetadataFromIdentityForPackagesAsync(PerformLookup(remainingPackages, searchToken), searchToken.SearchFilter.IncludePrerelease, cancellationToken);
        }

        private static async Task<IPackageSearchMetadata[]> GetMetadataFromIdentityForPackagesAsync<T>(T[] packages, bool includePrerelease, CancellationToken cancellationToken) where T : PackageIdentity
        {
            cancellationToken.ThrowIfCancellationRequested();

            IEnumerable<IPackageSearchMetadata> items = await TaskCombinators.ThrottledAsync(
                packages,
                (p, t) => Task.FromResult(GetMetadataFromIdentityForPackage(p, t)),
                cancellationToken);

            return SortPackagesMetadata(items);
        }

        private static IPackageSearchMetadata GetMetadataFromIdentityForPackage<T>(T identity, CancellationToken cancellationToken) where T : PackageIdentity
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (identity is PackageCollectionItem packageCollectionItem)
            {
                IEnumerable<ITransitivePackageReferenceContextInfo> transitivePRs = packageCollectionItem.PackageReferences.OfType<ITransitivePackageReferenceContextInfo>();
                ITransitivePackageReferenceContextInfo transitivePR = transitivePRs.OrderByDescending(x => x.Identity.Version).FirstOrDefault();
                IReadOnlyCollection<PackageIdentity> transitiveOrigins = transitivePR?.TransitiveOrigins?.Select(to => to.Identity).ToArray() ?? Array.Empty<PackageIdentity>();
                if (transitiveOrigins.Any())
                {
                    var packageMetadata = PackageSearchMetadataBuilder.FromIdentity(packageCollectionItem).Build(); // create metadata object only with ID

                    return new TransitivePackageSearchMetadata(packageMetadata, transitiveOrigins);
                }
            }

            return PackageSearchMetadataBuilder.FromIdentity(identity).Build();
        }

        internal override async Task<IPackageSearchMetadata> GetPackageMetadataAsync(PackageIdentity identity, bool includePrerelease, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (identity is PackageCollectionItem pkgColItem)
            {
                IEnumerable<ITransitivePackageReferenceContextInfo> transitivePRs = pkgColItem.PackageReferences.OfType<ITransitivePackageReferenceContextInfo>();
                ITransitivePackageReferenceContextInfo transitivePR = transitivePRs.OrderByDescending(x => x.Identity.Version).FirstOrDefault();
                IReadOnlyCollection<PackageIdentity> transitiveOrigins = transitivePR?.TransitiveOrigins?.Select(to => to.Identity).ToArray() ?? Array.Empty<PackageIdentity>();
                if (transitiveOrigins.Any())
                {
                    // Get only local metadata. We don't want Deprecation and Vulnerabilities Metadata on Transitive packages
                    IPackageSearchMetadata packageMetadata = await _metadataProvider.GetOnlyLocalPackageMetadataAsync(pkgColItem, cancellationToken);
                    if (packageMetadata == null) // Edge case: local metadata not found
                    {
                        packageMetadata = PackageSearchMetadataBuilder.FromIdentity(pkgColItem).Build(); // create metadata object only with ID
                    }

                    return new TransitivePackageSearchMetadata(packageMetadata, transitiveOrigins);
                }
            }

            return await base.GetPackageMetadataAsync(identity, includePrerelease, cancellationToken);
        }
    }
}
