// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.Protocol
{
    public class MetadataResourceV2Feed : MetadataResource
    {
        private readonly V2FeedParser _feedParser;
        private readonly SourceRepository _source;

        public MetadataResourceV2Feed(V2FeedParser feedParser, SourceRepository source)
        {
            if (feedParser == null)
            {
                throw new ArgumentNullException(nameof(feedParser));
            }

            _feedParser = feedParser;
            _source = source;
        }

        public override async Task<IEnumerable<KeyValuePair<string, NuGetVersion>>> GetLatestVersions(IEnumerable<string> packageIds, bool includePrerelease, bool includeUnlisted,
            SourceCacheContext sourceCacheContext, ILogger log, CancellationToken token)
        {
            var results = new List<KeyValuePair<string, NuGetVersion>>();

            var tasks = new Stack<KeyValuePair<string, Task<IEnumerable<NuGetVersion>>>>();

            // fetch all ids in parallel
            foreach (var id in packageIds)
            {
                var task = new KeyValuePair<string, Task<IEnumerable<NuGetVersion>>>(id, GetVersions(id, includePrerelease, includeUnlisted, sourceCacheContext, log, token));
                tasks.Push(task);
            }

            foreach (var pair in tasks)
            {
                // wait for the query to finish
                var versions = await pair.Value;

                if (versions == null || !versions.Any())
                {
                    results.Add(new KeyValuePair<string, NuGetVersion>(pair.Key, null));
                }
                else
                {
                    // sort and take only the highest version
                    NuGetVersion latestVersion = versions.OrderByDescending(p => p, VersionComparer.VersionRelease).FirstOrDefault();

                    results.Add(new KeyValuePair<string, NuGetVersion>(pair.Key, latestVersion));
                }
            }

            return results;
        }

        public override async Task<IEnumerable<NuGetVersion>> GetVersions(string packageId, bool includePrerelease, bool includeUnlisted, SourceCacheContext sourceCacheContext, ILogger log, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            try
            {
                var packages = await _feedParser.FindPackagesByIdAsync(packageId, includeUnlisted, includePrerelease, sourceCacheContext, log, token);

                return packages.Select(p => p.Version).ToArray();
            }
            catch (Exception ex)
            {
                throw new FatalProtocolException(string.Format(CultureInfo.CurrentCulture, Strings.Protocol_PackageMetadataError, packageId, _source), ex);
            }
        }

        public override async Task<bool> Exists(PackageIdentity identity, bool includeUnlisted, SourceCacheContext sourceCacheContext, ILogger log, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            try
            {
                var package = await _feedParser.GetPackage(identity, sourceCacheContext, log, token);

                return package != null;
            }
            catch (Exception ex)
            {
                throw new FatalProtocolException(string.Format(CultureInfo.CurrentCulture, Strings.Protocol_PackageMetadataError, identity, _source), ex);
            }
        }

        public override async Task<bool> Exists(string packageId, bool includePrerelease, bool includeUnlisted, SourceCacheContext sourceCacheContext, ILogger log, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var versions = await GetVersions(packageId, includePrerelease, includeUnlisted, sourceCacheContext, log, token);

            return versions.Any();
        }
    }
}
