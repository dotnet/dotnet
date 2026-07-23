// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Model;
using NuGet.Protocol.Utility;
using NuGet.Shared;
using NuGet.Versioning;

namespace NuGet.Protocol
{
    public class AutoCompleteResourceV3 : AutoCompleteResource
    {
        internal readonly RegistrationResourceV3 _regResource;
        internal readonly ServiceIndexResourceV3 _serviceIndex;
        internal readonly HttpSource _client;
        private readonly IEnvironmentVariableReader? _environmentVariableReader;

        public AutoCompleteResourceV3(HttpSource client, ServiceIndexResourceV3 serviceIndex, RegistrationResourceV3 regResource)
            : this(client, serviceIndex, regResource, null)
        {
        }

        internal AutoCompleteResourceV3(HttpSource client, ServiceIndexResourceV3 serviceIndex, RegistrationResourceV3 regResource, IEnvironmentVariableReader? environmentVariableReader)
            : base()
        {
            _regResource = regResource;
            _serviceIndex = serviceIndex;
            _client = client;
            _environmentVariableReader = environmentVariableReader;
        }

        public override async Task<IEnumerable<string>> IdStartsWith(
            string packageIdPrefix,
            bool includePrerelease,
            Common.ILogger log,
            CancellationToken token)
        {
            if (NuGetFeatureFlags.UseSystemTextJsonDeserializationFeatureSwitch)
            {
                return await IdStartsWithStjAsync(packageIdPrefix, includePrerelease, log, token);
            }
            else if (NuGetFeatureFlags.IsSystemTextJsonDeserializationEnabledByEnvironment(_environmentVariableReader))
            {
                return await IdStartsWithStjAsync(packageIdPrefix, includePrerelease, log, token);
            }
            else
            {
                return await IdStartsWithNsjAsync(packageIdPrefix, includePrerelease, log, token);
            }
        }

        private async Task<IEnumerable<string>> IdStartsWithStjAsync(
            string packageIdPrefix,
            bool includePrerelease,
            Common.ILogger log,
            CancellationToken token)
        {
            var queryUri = BuildQueryUri(packageIdPrefix, includePrerelease);
            Common.ILogger logger = log ?? Common.NullLogger.Instance;

            AutoCompleteModel? results = await _client.ProcessStreamAsync(
                new HttpSourceRequest(queryUri, logger),
                async stream =>
                {
                    if (stream == null)
                    {
                        return null;
                    }

                    return await JsonSerializer.DeserializeAsync(stream, JsonContext.Default.AutoCompleteModel, token);
                },
                logger,
                token);

            token.ThrowIfCancellationRequested();

            return results?.Data?.Where(item => item != null && item.StartsWith(packageIdPrefix, StringComparison.OrdinalIgnoreCase))
                ?? [];
        }

        private async Task<IEnumerable<string>> IdStartsWithNsjAsync(
            string packageIdPrefix,
            bool includePrerelease,
            Common.ILogger log,
            CancellationToken token)
        {
            var queryUri = BuildQueryUri(packageIdPrefix, includePrerelease);
            Common.ILogger logger = log ?? Common.NullLogger.Instance;

            var results = await _client.GetJObjectAsync(
                new HttpSourceRequest(queryUri, logger),
                logger,
                token);
            token.ThrowIfCancellationRequested();
            if (results == null)
            {
                return Enumerable.Empty<string>();
            }
            var data = results.Value<JArray>("data");
            if (data == null)
            {
                return Enumerable.Empty<string>();
            }

            // Resolve all the objects
            var outputs = new List<string>();
            foreach (var result in data)
            {
                if (result != null)
                {
                    outputs.Add(result.ToString());
                }
            }

            return outputs.Where(item => item.StartsWith(packageIdPrefix, StringComparison.OrdinalIgnoreCase));
        }

        private Uri BuildQueryUri(string packageIdPrefix, bool includePrerelease)
        {
            var searchUrl = _serviceIndex.GetServiceEntryUri(ServiceTypes.SearchAutocompleteService);

            if (searchUrl == null)
            {
                throw new FatalProtocolException(Strings.Protocol_MissingSearchService);
            }

            // Construct the query
            var queryUrl = new UriBuilder(searchUrl.AbsoluteUri);
            queryUrl.Query =
                "q=" + WebUtility.UrlEncode(packageIdPrefix) +
                "&prerelease=" + includePrerelease.ToString(CultureInfo.CurrentCulture).ToLowerInvariant() +
                "&semVerLevel=2.0.0";

            return queryUrl.Uri;
        }

        public override async Task<IEnumerable<NuGetVersion>> VersionStartsWith(
            string packageId,
            string versionPrefix,
            bool includePrerelease,
            SourceCacheContext sourceCacheContext,
            Common.ILogger log,
            CancellationToken token)
        {
            Common.ILogger logger = log ?? Common.NullLogger.Instance;

            //*TODOs : Take prerelease as parameter. Also it should return both listed and unlisted for powershell ?
            if (NuGetFeatureFlags.UseSystemTextJsonDeserializationFeatureSwitch)
            {
                return await VersionStartsWithFromItemsAsync(packageId, versionPrefix, includePrerelease, sourceCacheContext, logger, token);
            }

            if (NuGetFeatureFlags.IsSystemTextJsonDeserializationEnabledByEnvironment(_environmentVariableReader))
            {
                return await VersionStartsWithFromItemsAsync(packageId, versionPrefix, includePrerelease, sourceCacheContext, logger, token);
            }

            return await VersionStartsWithFromJObjectsAsync(packageId, versionPrefix, includePrerelease, sourceCacheContext, logger, token);
        }

        private async Task<IEnumerable<NuGetVersion>> VersionStartsWithFromItemsAsync(
            string packageId,
            string versionPrefix,
            bool includePrerelease,
            SourceCacheContext sourceCacheContext,
            Common.ILogger logger,
            CancellationToken token)
        {
            var versions = new List<NuGetVersion>();
            IReadOnlyList<RegistrationLeafItem> items = await _regResource.GetPackageMetadataItemsAsync(packageId, VersionRange.All, includePrerelease, includeUnlisted: false, sourceCacheContext, logger, token);
            foreach (RegistrationLeafItem item in items.NoAllocEnumerate())
            {
                NuGetVersion? version = item.CatalogEntry?.Version;
                if (version != null
                    && version.ToString().StartsWith(versionPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    versions.Add(version);
                }
            }
            return versions;
        }

        private async Task<IEnumerable<NuGetVersion>> VersionStartsWithFromJObjectsAsync(
            string packageId,
            string versionPrefix,
            bool includePrerelease,
            SourceCacheContext sourceCacheContext,
            Common.ILogger logger,
            CancellationToken token)
        {
            var versions = new List<NuGetVersion>();
            IEnumerable<JObject> packages = await _regResource.GetPackageMetadata(packageId, includePrerelease, false, sourceCacheContext, logger, token);
            foreach (JObject package in packages.NoAllocEnumerate())
            {
                var version = (string?)package["version"];
                if (version != null && version.StartsWith(versionPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    versions.Add(new NuGetVersion(version));
                }
            }
            return versions;
        }
    }
}
