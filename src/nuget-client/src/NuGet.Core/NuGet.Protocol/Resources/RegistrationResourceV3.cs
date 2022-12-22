// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.Protocol
{
    /// <summary>
    /// Registration blob reader
    /// </summary>
    public class RegistrationResourceV3 : INuGetResource
    {
        private readonly HttpSource _client;

        public RegistrationResourceV3(HttpSource client, Uri baseUrl)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if (baseUrl == null)
            {
                throw new ArgumentNullException(nameof(baseUrl));
            }

            _client = client;
            BaseUri = baseUrl;
        }

        /// <summary>
        /// Gets the <see cref="Uri"/> for the source backing this resource.
        /// </summary>
        public Uri BaseUri { get; }

        /// <summary>
        /// Constructs the URI of a registration index blob
        /// </summary>
        public virtual Uri GetUri(string packageId)
        {
            if (String.IsNullOrEmpty(packageId))
            {
                throw new InvalidOperationException();
            }

            return new Uri(String.Format(CultureInfo.InvariantCulture, "{0}/{1}/index.json",
                BaseUri.AbsoluteUri.TrimEnd('/'), packageId.ToLowerInvariant()));
        }

        /// <summary>
        /// Constructs the URI of a registration blob with a specific version
        /// </summary>
        public virtual Uri GetUri(string id, NuGetVersion version)
        {
            if (String.IsNullOrEmpty(id))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(id));
            }

            if (version == null)
            {
                throw new ArgumentNullException(nameof(version));
            }

            return GetUri(new PackageIdentity(id, version));
        }

        /// <summary>
        /// Constructs the URI of a registration blob with a specific version
        /// </summary>
        public virtual Uri GetUri(PackageIdentity package)
        {
            if (package == null
                || package.Id == null
                || package.Version == null)
            {
                throw new InvalidOperationException();
            }

            return new Uri(String.Format(CultureInfo.InvariantCulture, "{0}/{1}/{2}.json", BaseUri.AbsoluteUri.TrimEnd('/'),
                package.Id.ToLowerInvariant(), package.Version.ToNormalizedString().ToLowerInvariant()));
        }

        /// <summary>
        /// Returns the registration blob for the id and version
        /// </summary>
        /// <remarks>The inlined entries are potentially going away soon</remarks>
        public virtual async Task<JObject> GetPackageMetadata(PackageIdentity identity, SourceCacheContext cacheContext, Common.ILogger log, CancellationToken token)
        {
            return (await GetPackageMetadata(identity.Id, new VersionRange(identity.Version, true, identity.Version, true), true, true, cacheContext, log, token)).SingleOrDefault();
        }

        /// <summary>
        /// Returns inlined catalog entry items for each registration blob
        /// </summary>
        /// <remarks>The inlined entries are potentially going away soon</remarks>
        public virtual async Task<IEnumerable<JObject>> GetPackageMetadata(string packageId, bool includePrerelease, bool includeUnlisted, SourceCacheContext cacheContext, Common.ILogger log, CancellationToken token)
        {
            return await GetPackageMetadata(packageId, VersionRange.All, includePrerelease, includeUnlisted, cacheContext, log, token);
        }

        /// <summary>
        /// Returns inlined catalog entry items for each registration blob
        /// </summary>
        /// <remarks>The inlined entries are potentially going away soon</remarks>
        public virtual async Task<IEnumerable<JObject>> GetPackageMetadata(
            string packageId,
            VersionRange range,
            bool includePrerelease,
            bool includeUnlisted,
            SourceCacheContext cacheContext,
            Common.ILogger log,
            CancellationToken token)
        {
            var results = new List<JObject>();

            var registrationUri = GetUri(packageId);

            var ranges = await RegistrationUtility.LoadRanges(_client, registrationUri, packageId, range, cacheContext, log, token);

            foreach (var rangeObj in ranges)
            {
                if (rangeObj == null)
                {
                    throw new InvalidDataException(registrationUri.AbsoluteUri);
                }

                foreach (JObject packageObj in rangeObj["items"])
                {
                    var catalogEntry = (JObject)packageObj["catalogEntry"];
                    var version = NuGetVersion.Parse(catalogEntry["version"].ToString());
                    var listed = catalogEntry.GetBoolean("listed") ?? true;

                    if (range.Satisfies(version)
                        && (includePrerelease || !version.IsPrerelease)
                        && (includeUnlisted || listed))
                    {
                        // add in the download url
                        if (packageObj["packageContent"] != null)
                        {
                            catalogEntry["packageContent"] = packageObj["packageContent"];
                        }

                        results.Add(catalogEntry);
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Returns all index entries of type Package within the given range and filters
        /// </summary>
        public virtual Task<IEnumerable<JObject>> GetPackageEntries(string packageId, bool includeUnlisted, SourceCacheContext cacheContext, Common.ILogger log, CancellationToken token)
        {
            return GetPackageMetadata(packageId, VersionRange.All, true, includeUnlisted, cacheContext, log, token);
        }
    }
}
