// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Shared;
using NuGet.Versioning;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.VisualStudio
{
    internal sealed class PackageSearchMetadataCacheItem
    {
        private IPackageSearchMetadata _packageSearchMetadata;
        private readonly IPackageMetadataProvider _packageMetadataProvider;
        private readonly ConcurrentDictionary<NuGetVersion, PackageSearchMetadataCacheItemEntry> _cachedItemEntries;

        public PackageSearchMetadataCacheItem(IPackageSearchMetadata packageSearchMetadata, IPackageMetadataProvider packageMetadataProvider)
        {
            Assumes.NotNull(packageSearchMetadata);
            Assumes.NotNull(packageMetadataProvider);

            _cachedItemEntries = new ConcurrentDictionary<NuGetVersion, PackageSearchMetadataCacheItemEntry>
            {
                [packageSearchMetadata.Identity.Version] = new PackageSearchMetadataCacheItemEntry(packageSearchMetadata, packageMetadataProvider)
            };

            _packageMetadataProvider = packageMetadataProvider;
            _packageSearchMetadata = packageSearchMetadata;
            AllVersionsContextInfo = GetVersionInfoContextInfoAsync();
        }

        public ValueTask<IReadOnlyCollection<VersionInfoContextInfo>> AllVersionsContextInfo { get; private set; }

        public async ValueTask<PackageSearchMetadataCacheItemEntry> GetPackageSearchMetadataCacheVersionedItemAsync(PackageIdentity packageIdentity, CancellationToken cancellationToken)
        {
            Assumes.NotNull(packageIdentity);

            if (!_cachedItemEntries.TryGetValue(packageIdentity.Version, out PackageSearchMetadataCacheItemEntry cacheItemEntry))
            {
                IPackageSearchMetadata packageSearchMetadata = await _packageMetadataProvider.GetPackageMetadataForIdentityAsync(packageIdentity, cancellationToken);
                cacheItemEntry = new PackageSearchMetadataCacheItemEntry(packageSearchMetadata, _packageMetadataProvider);
                _cachedItemEntries[packageIdentity.Version] = cacheItemEntry;
            }

            return cacheItemEntry;
        }

        public void UpdateSearchMetadata(IPackageSearchMetadata packageSearchMetadata)
        {
            _packageSearchMetadata = packageSearchMetadata;
            AllVersionsContextInfo = GetVersionInfoContextInfoAsync();
        }

        public static string GetCacheId(string packageId, bool includePrerelease, IReadOnlyCollection<PackageSourceContextInfo> packageSources)
        {
            var hashCodeCombiner = new HashCodeCombiner();
            hashCodeCombiner.AddSequence(packageSources);
            hashCodeCombiner.AddStringIgnoreCase(packageId);
            hashCodeCombiner.AddObject(includePrerelease);
            return hashCodeCombiner.CombinedHash.ToString(CultureInfo.InvariantCulture);
        }

        private async ValueTask<IReadOnlyCollection<VersionInfoContextInfo>> GetVersionInfoContextInfoAsync()
        {
            IEnumerable<VersionInfo> versions = await _packageSearchMetadata.GetVersionsAsync();
            IEnumerable<Task<VersionInfoContextInfo>> versionContextInfoTasks = versions.Select(async v => await VersionInfoContextInfo.CreateAsync(v));
            return await Task.WhenAll(versionContextInfoTasks);
        }
    }
}
