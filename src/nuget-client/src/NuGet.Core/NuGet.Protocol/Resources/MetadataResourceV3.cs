// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.Protocol
{
    /// <summary>
    /// Returns the full package metadata
    /// </summary>
    public class MetadataResourceV3 : MetadataResource
    {
        private RegistrationResourceV3 _regResource;

        public MetadataResourceV3(RegistrationResourceV3 regResource)
            : base()
        {
            _regResource = regResource ?? throw new ArgumentNullException(nameof(regResource));
        }

        /// <summary>
        /// Find the latest version of the package
        /// </summary>
        /// <param name="includePrerelease">include versions with prerelease labels</param>
        /// <param name="includeUnlisted">not implemented yet</param>
        public override async Task<IEnumerable<KeyValuePair<string, NuGetVersion>>> GetLatestVersions(
            IEnumerable<string> packageIds,
            bool includePrerelease,
            bool includeUnlisted,
            SourceCacheContext sourceCacheContext,
            Common.ILogger log,
            CancellationToken token)
        {
            var results = new List<KeyValuePair<string, NuGetVersion>>();

            foreach (var id in packageIds)
            {
                IEnumerable<NuGetVersion> allVersions;
                try
                {
                    var catalogEntries = await _regResource.GetPackageMetadata(id, includePrerelease, includeUnlisted, sourceCacheContext, log, token);
                    allVersions = catalogEntries.Select(p => NuGetVersion.Parse(p["version"].ToString()));
                }
                catch (Exception ex)
                {
                    throw new FatalProtocolException(string.Format(CultureInfo.CurrentCulture, Strings.Protocol_PackageMetadataError, id, _regResource.BaseUri), ex);
                }

                // find the latest
                var latest = allVersions.OrderByDescending(p => p, VersionComparer.VersionRelease).FirstOrDefault();

                results.Add(new KeyValuePair<string, NuGetVersion>(id, latest));
            }

            return results;
        }

        public override async Task<bool> Exists(
            PackageIdentity identity,
            bool includeUnlisted,
            SourceCacheContext sourceCacheContext,
            Common.ILogger log,
            CancellationToken token)
        {
            // TODO: get the url and just check the headers?
            var metadata = await _regResource.GetPackageMetadata(identity, sourceCacheContext, log, token);

            // TODO: listed check
            return metadata != null;
        }

        public override async Task<bool> Exists(
            string packageId,
            bool includePrerelease,
            bool includeUnlisted,
            SourceCacheContext sourceCacheContext,
            Common.ILogger log,
            CancellationToken token)
        {
            var entries = await GetVersions(packageId, includePrerelease, includeUnlisted, sourceCacheContext, log, token);

            return entries != null && entries.Any();
        }

        public override async Task<IEnumerable<NuGetVersion>> GetVersions(
            string packageId,
            bool includePrerelease,
            bool includeUnlisted,
            SourceCacheContext sourceCacheContext,
            Common.ILogger log,
            CancellationToken token)
        {
            var results = new List<NuGetVersion>();

            var entries = await _regResource.GetPackageEntries(packageId, includeUnlisted, sourceCacheContext, log, token);

            foreach (var catalogEntry in entries)
            {
                NuGetVersion version = null;

                if (catalogEntry["version"] != null
                    && NuGetVersion.TryParse(catalogEntry["version"].ToString(), out version))
                {
                    if (includePrerelease || !version.IsPrerelease)
                    {
                        results.Add(version);
                    }
                }
            }

            return results;
        }
    }
}
