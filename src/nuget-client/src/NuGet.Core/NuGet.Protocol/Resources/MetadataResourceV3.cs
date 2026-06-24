// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Model;
using NuGet.Shared;
using NuGet.Versioning;

namespace NuGet.Protocol
{
    /// <summary>
    /// Returns the full package metadata
    /// </summary>
    public class MetadataResourceV3 : MetadataResource
    {
        private RegistrationResourceV3 _regResource;
        private readonly Common.IEnvironmentVariableReader _environmentVariableReader;

        public MetadataResourceV3(RegistrationResourceV3 regResource)
            : this(regResource, environmentVariableReader: null)
        {
        }

        internal MetadataResourceV3(RegistrationResourceV3 regResource, Common.IEnvironmentVariableReader environmentVariableReader)
            : base()
        {
            _regResource = regResource ?? throw new ArgumentNullException(nameof(regResource));
            _environmentVariableReader = environmentVariableReader;
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
                    if (NuGetFeatureFlags.UseSystemTextJsonDeserializationFeatureSwitch)
                    {
                        allVersions = await GetVersionsFromItemsAsync(id, includePrerelease, includeUnlisted, sourceCacheContext, log, token);
                    }
                    else if (NuGetFeatureFlags.IsSystemTextJsonDeserializationEnabledByEnvironment(_environmentVariableReader))
                    {
                        allVersions = await GetVersionsFromItemsAsync(id, includePrerelease, includeUnlisted, sourceCacheContext, log, token);
                    }
                    else
                    {
                        allVersions = await GetVersionsFromJObjectsAsync(id, includePrerelease, includeUnlisted, sourceCacheContext, log, token);
                    }
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
            if (NuGetFeatureFlags.UseSystemTextJsonDeserializationFeatureSwitch)
            {
                RegistrationLeafItem item = await _regResource.GetPackageMetadataItemAsync(identity, sourceCacheContext, log, token);

                // TODO: listed check
                return item != null;
            }
            else if (NuGetFeatureFlags.IsSystemTextJsonDeserializationEnabledByEnvironment(_environmentVariableReader))
            {
                RegistrationLeafItem item = await _regResource.GetPackageMetadataItemAsync(identity, sourceCacheContext, log, token);

                // TODO: listed check
                return item != null;
            }
            else
            {
                // To make the AoT linker reliably trim the Newtonsoft.Json code in async state machines, the entire method needs to be
                // in a separate method that is only called when the feature switch is disabled.
                return await ExistsFromJObjectAsync(identity, sourceCacheContext, log, token);
            }
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
            if (NuGetFeatureFlags.UseSystemTextJsonDeserializationFeatureSwitch)
            {
                return await GetVersionsFromItemsAsync(packageId, includePrerelease, includeUnlisted, sourceCacheContext, log, token);
            }

            if (NuGetFeatureFlags.IsSystemTextJsonDeserializationEnabledByEnvironment(_environmentVariableReader))
            {
                return await GetVersionsFromItemsAsync(packageId, includePrerelease, includeUnlisted, sourceCacheContext, log, token);
            }

            return await GetVersionsFromJObjectsAsync(packageId, includePrerelease, includeUnlisted, sourceCacheContext, log, token);
        }

        private async Task<List<NuGetVersion>> GetVersionsFromItemsAsync(
            string packageId,
            bool includePrerelease,
            bool includeUnlisted,
            SourceCacheContext sourceCacheContext,
            Common.ILogger log,
            CancellationToken token)
        {
            IReadOnlyList<RegistrationLeafItem> items = await _regResource.GetPackageMetadataItemsAsync(packageId, VersionRange.All, includePrerelease, includeUnlisted, sourceCacheContext, log, token);

            var versions = new List<NuGetVersion>();

            foreach (RegistrationLeafItem item in items.NoAllocEnumerate())
            {
                NuGetVersion version = item.CatalogEntry.Version;

                if (version != null)
                {
                    versions.Add(version);
                }
            }

            return versions;
        }

        private async Task<List<NuGetVersion>> GetVersionsFromJObjectsAsync(
            string packageId,
            bool includePrerelease,
            bool includeUnlisted,
            SourceCacheContext sourceCacheContext,
            Common.ILogger log,
            CancellationToken token)
        {
            IEnumerable<JObject> entries = await _regResource.GetPackageMetadata(packageId, includePrerelease, includeUnlisted, sourceCacheContext, log, token);

            var versions = new List<NuGetVersion>();

            foreach (var catalogEntry in entries)
            {
                NuGetVersion version = null;

                if (catalogEntry["version"] != null
                    && NuGetVersion.TryParse(catalogEntry["version"].ToString(), out version))
                {
                    if (includePrerelease || !version.IsPrerelease)
                    {
                        versions.Add(version);
                    }
                }
            }

            return versions;
        }

        private async Task<bool> ExistsFromJObjectAsync(
            PackageIdentity identity,
            SourceCacheContext sourceCacheContext,
            Common.ILogger log,
            CancellationToken token)
        {
            JObject metadata = await _regResource.GetPackageMetadata(identity, sourceCacheContext, log, token);

            // TODO: listed check
            return metadata != null;
        }
    }
}
