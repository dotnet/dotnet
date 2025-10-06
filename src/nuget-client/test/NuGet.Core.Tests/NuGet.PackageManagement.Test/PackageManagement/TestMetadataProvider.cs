// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace Test.Utility
{
    public class TestMetadataProvider : ResourceProvider
    {
        public List<SourcePackageDependencyInfo> Packages { get; set; }

        public TestMetadataProvider(List<SourcePackageDependencyInfo> packages)
            : base(typeof(MetadataResource))
        {
            Packages = packages;
        }

        public override Task<Tuple<bool, INuGetResource>> TryCreate(SourceRepository source, CancellationToken token)
        {
            var nuGetResource = new TestMetadataResource(Packages);
            return Task.FromResult(new Tuple<bool, INuGetResource>(true, nuGetResource));
        }
    }

    /// <summary>
    /// Retrieve versions from a local set of packages
    /// </summary>
    internal class TestMetadataResource : MetadataResource
    {
        public List<SourcePackageDependencyInfo> Packages { get; set; }

        public TestMetadataResource(List<SourcePackageDependencyInfo> packages)
        {
            Packages = packages;
        }

        public override Task<IEnumerable<NuGetVersion>> GetVersions(string packageId, bool includePrerelease, bool includeUnlisted, SourceCacheContext sourceCacheContext, NuGet.Common.ILogger log, CancellationToken token)
        {
            return Task.FromResult(Packages
                .Where(p =>
                    p.Id.Equals(packageId, StringComparison.InvariantCultureIgnoreCase)
                    && (!p.Version.IsPrerelease || includePrerelease)
                    && (p.Listed || includeUnlisted))
                .Select(p => p.Version)
            );
        }

        public override Task<bool> Exists(PackageIdentity identity, bool includeUnlisted, SourceCacheContext sourceCacheContext, NuGet.Common.ILogger log, CancellationToken token)
        {
            return Task.FromResult(Packages
                .Exists(p =>
                    ((PackageIdentity)p).Equals(identity)
                    && (p.Listed || includeUnlisted))
            );
        }

        public override Task<bool> Exists(string packageId, bool includePrerelease, bool includeUnlisted, SourceCacheContext sourceCacheContext, NuGet.Common.ILogger log, CancellationToken token)
        {
            return Task.FromResult(Packages
                .Exists((p) =>
                    p.Id.Equals(packageId, StringComparison.InvariantCultureIgnoreCase)
                    && (!p.Version.IsPrerelease || includePrerelease)
                    && (p.Listed || includeUnlisted))
            );
        }

        public async override Task<IEnumerable<KeyValuePair<string, NuGetVersion>>> GetLatestVersions(IEnumerable<string> packageIds, bool includePrerelease, bool includeUnlisted, SourceCacheContext sourceCacheContext, NuGet.Common.ILogger log, CancellationToken token)
        {
            var results = new List<KeyValuePair<string, NuGetVersion>>();

            foreach (var id in packageIds)
            {
                var versions = await GetVersions(id, sourceCacheContext, log, token);
                var latest = versions.OrderByDescending(p => p, VersionComparer.VersionRelease).FirstOrDefault();

                if (latest != null)
                {
                    results.Add(new KeyValuePair<string, NuGetVersion>(id, latest));
                }
            }

            return results;
        }
    }
}
