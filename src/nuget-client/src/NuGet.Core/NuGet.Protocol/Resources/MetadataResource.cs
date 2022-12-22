// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace NuGet.Protocol.Core.Types
{
    /// <summary>
    /// Basic metadata
    /// </summary>
    public abstract class MetadataResource : INuGetResource
    {
        /// <summary>
        /// Get all versions of a package
        /// </summary>
        public async Task<IEnumerable<NuGetVersion>> GetVersions(string packageId, SourceCacheContext sourceCacheContext, Common.ILogger log, CancellationToken token)
        {
            return await GetVersions(packageId, true, false, sourceCacheContext, log, token);
        }

        /// <summary>
        /// Get all versions of a package
        /// </summary>
        public abstract Task<IEnumerable<NuGetVersion>> GetVersions(string packageId, bool includePrerelease, bool includeUnlisted, SourceCacheContext sourceCacheContext, Common.ILogger log, CancellationToken token);

        /// <summary>
        /// True if the package exists in the source
        /// Includes unlisted.
        /// </summary>
        public async Task<bool> Exists(PackageIdentity identity, SourceCacheContext sourceCacheContext, Common.ILogger log, CancellationToken token)
        {
            return await Exists(identity, true, sourceCacheContext, log, token);
        }

        /// <summary>
        /// True if the package exists in the source
        /// </summary>
        public abstract Task<bool> Exists(PackageIdentity identity, bool includeUnlisted, SourceCacheContext sourceCacheContext, Common.ILogger log, CancellationToken token);

        public async Task<bool> Exists(string packageId, SourceCacheContext sourceCacheContext, Common.ILogger log, CancellationToken token)
        {
            return await Exists(packageId, true, false, sourceCacheContext, log, token);
        }

        public abstract Task<bool> Exists(string packageId, bool includePrerelease, bool includeUnlisted, SourceCacheContext sourceCacheContext, Common.ILogger log, CancellationToken token);

        public abstract Task<IEnumerable<KeyValuePair<string, NuGetVersion>>> GetLatestVersions(IEnumerable<string> packageIds, bool includePrerelease, bool includeUnlisted, SourceCacheContext sourceCacheContext, Common.ILogger log, CancellationToken token);

        public async Task<NuGetVersion> GetLatestVersion(string packageId, bool includePrerelease, bool includeUnlisted, SourceCacheContext sourceCacheContext, Common.ILogger log, CancellationToken token)
        {
            var results = await GetLatestVersions(new string[] { packageId }, includePrerelease, includeUnlisted, sourceCacheContext, log, token);
            var result = results.SingleOrDefault();

            if (!result.Equals(default(KeyValuePair<string, bool>)))
            {
                return result.Value;
            }

            return null;
        }
    }
}
