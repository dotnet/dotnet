// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NuGet.Common;
using NuGet.Protocol.Converters;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Model;
using NuGet.Shared;

namespace NuGet.Protocol
{
    public class PackageSearchResourceV3 : PackageSearchResource
    {
        private readonly HttpSource _client;
        private readonly IReadOnlyList<Uri> _searchEndpoints;
        private readonly IReadOnlyList<Uri>? _packageTypeCapableEndpoints;
        private readonly IEnvironmentVariableReader? _environmentVariableReader;

        internal PackageSearchResourceV3(
            HttpSource client,
            IReadOnlyList<Uri> searchEndpoints,
            IReadOnlyList<Uri>? packageTypeCapableEndpoints = null,
            IEnvironmentVariableReader? environmentVariableReader = null)
            : base()
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _searchEndpoints = searchEndpoints ?? throw new ArgumentNullException(nameof(searchEndpoints));
            _packageTypeCapableEndpoints = packageTypeCapableEndpoints;
            _environmentVariableReader = environmentVariableReader;
        }

        public override bool SupportsPackageTypeFiltering => _packageTypeCapableEndpoints?.Count > 0;

        /// <summary>
        /// Query nuget package list from nuget server. This implementation optimized for performance so doesn't iterate whole result 
        /// returned nuget server, so as soon as find "take" number of result packages then stop processing and return the result. 
        /// </summary>
        /// <param name="searchTerm">The term we're searching for.</param>
        /// <param name="filter">Filter for whether to include prerelease, delisted, supportedframework flags in query.</param>
        /// <param name="skip">Skip how many items from beginning of list.</param>
        /// <param name="take">Return how many items.</param>
        /// <param name="log">Logger instance.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>List of package meta data.</returns>
        public override async Task<IEnumerable<IPackageSearchMetadata>> SearchAsync(string searchTerm, SearchFilter filter, int skip, int take, Common.ILogger log, CancellationToken cancellationToken)
        {
            var metadataCache = new MetadataReferenceCache();

            var searchResultMetadata = await Search(
                searchTerm,
                filter,
                skip,
                take,
                log,
                cancellationToken);

            List<IPackageSearchMetadata> searchResults = new(searchResultMetadata.Count);
            foreach (var metadata in searchResultMetadata)
            {
                var withVersions = metadata.WithVersions(() => GetVersions(metadata, filter));
                ((PackageSearchMetadataBuilder.ClonedPackageSearchMetadata)withVersions).CacheStrings(metadataCache);
                searchResults.Add(withVersions);
            }

            return searchResults;
        }

        private static IEnumerable<VersionInfo> GetVersions(PackageSearchMetadata metadata, SearchFilter filter)
        {
            var uniqueVersions = new HashSet<Versioning.NuGetVersion>(metadata.ParsedVersions.Length + 1);
            var versions = new List<VersionInfo>(metadata.ParsedVersions.Length + 1);
            foreach (var ver in metadata.ParsedVersions)
            {
                if ((filter.IncludePrerelease || !ver.Version.IsPrerelease) && uniqueVersions.Add(ver.Version))
                {
                    versions.Add(new VersionInfo(ver.Version, ver.DownloadCount));
                }
            }
            if (uniqueVersions.Add(metadata.Version))
            {
                versions.Add(new VersionInfo(metadata.Version, metadata.DownloadCount));
            }
            return versions;
        }

        private async Task<T> SearchPage<T>(
                    Func<Uri, Task<T>> getResultAsync,
                    string searchTerm,
                    SearchFilter filters,
                    int skip,
                    int take,
                    Common.ILogger log,
                    CancellationToken cancellationToken)
        {
            bool packageTypeFilterRequested = !string.IsNullOrEmpty(filters.PackageType);

            if (packageTypeFilterRequested)
            {
                if (_packageTypeCapableEndpoints == null || _packageTypeCapableEndpoints.Count == 0)
                {
                    throw new NotSupportedException(Strings.Protocol_PackageTypeFilterNotSupported);
                }
            }

            // When package type filtering is requested, only iterate endpoints that advertise
            // SearchQueryService/3.5.0. Otherwise, use the full set of search endpoints.
            IReadOnlyList<Uri> endpoints = packageTypeFilterRequested
                ? _packageTypeCapableEndpoints ?? []
                : _searchEndpoints;

            for (var i = 0; i < endpoints.Count; i++)
            {
                var endpoint = endpoints[i];

                // The search term comes in already encoded from VS
                var queryUrl = new UriBuilder(endpoint.AbsoluteUri);
                var queryString =
                    "q=" + searchTerm +
                    "&skip=" + skip.ToString(CultureInfo.CurrentCulture) +
                    "&take=" + take.ToString(CultureInfo.CurrentCulture) +
                    "&prerelease=" + (filters.IncludePrerelease ? "true" : "false");

                if (filters.IncludeDelisted)
                {
                    queryString += "&includeDelisted=true";
                }

                if (filters.SupportedFrameworks != null
                    && filters.SupportedFrameworks.Any())
                {
                    var frameworks =
                        string.Join("&",
                            filters.SupportedFrameworks.Select(
                                fx => "supportedFramework=" + fx.ToString(CultureInfo.InvariantCulture)));
                    queryString += "&" + frameworks;
                }

                if (packageTypeFilterRequested)
                {
                    queryString += "&packageType=" + filters.PackageType;
                }

                queryString += "&semVerLevel=2.0.0";

                queryUrl.Query = queryString;

                var searchResult = default(T);
                try
                {
                    log.LogVerbose($"Querying {queryUrl.Uri}");

                    searchResult = await getResultAsync(queryUrl.Uri);
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch when (i < endpoints.Count - 1)
                {
                    // Ignore all failures until the last endpoint
                }
                catch (JsonReaderException ex)
                {
                    throw new FatalProtocolException(string.Format(CultureInfo.CurrentCulture, Strings.Protocol_MalformedMetadataError, queryUrl.Uri), ex);
                }
                catch (HttpRequestException ex)
                {
                    throw new FatalProtocolException(string.Format(CultureInfo.CurrentCulture, Strings.Protocol_BadSource, queryUrl.Uri), ex);
                }

                if (searchResult != null)
                {
                    return searchResult;
                }
            }

            // TODO: get a better message for this
            throw new FatalProtocolException(Strings.Protocol_MissingSearchService);
        }

        /// <summary>
        /// Query nuget package list from nuget server. This implementation optimized for performance so doesn't iterate whole result 
        /// returned nuget server, so as soon as find "take" number of result packages then stop processing and return the result. 
        /// </summary>
        /// <param name="searchTerm">The term we're searching for.</param>
        /// <param name="filters">Filter for whether to include prerelease, delisted, supportedframework flags in query.</param>
        /// <param name="skip">Skip how many items from beginning of list.</param>
        /// <param name="take">Return how many items.</param>
        /// <param name="log">Logger instance.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>List of package meta data.</returns>
        internal async Task<IReadOnlyList<PackageSearchMetadata>> Search(
            string searchTerm,
            SearchFilter filters,
            int skip,
            int take,
            Common.ILogger log,
            CancellationToken cancellationToken)
        {
            return await SearchPage(
                uri => _client.ProcessHttpStreamAsync(
                    new HttpSourceRequest(uri, Common.NullLogger.Instance),
                    s => ProcessHttpStreamTakeCountedItemAsync(s, take, cancellationToken),
                    Common.NullLogger.Instance,
                    cancellationToken),
                searchTerm,
                filters,
                skip,
                take,
                log,
                cancellationToken);
        }

        internal async Task<IReadOnlyList<PackageSearchMetadata>> ProcessHttpStreamTakeCountedItemAsync(HttpResponseMessage? httpInitialResponse, int take, CancellationToken token)
        {
            if (take <= 0)
            {
                return [];
            }

            var results = await ProcessHttpStreamWithoutBufferingAsync(httpInitialResponse, (uint)take, token);
            return results?.Data ?? [];
        }

        private async Task<V3SearchResults?> ProcessHttpStreamWithoutBufferingAsync(HttpResponseMessage? httpInitialResponse, uint take, CancellationToken token)
        {
            if (httpInitialResponse == null)
            {
                return null;
            }

            if (NuGetFeatureFlags.UseSystemTextJsonDeserializationFeatureSwitch)
            {
                return await ProcessHttpStreamWithStjAsync(httpInitialResponse, take, token);
            }
            else if (NuGetFeatureFlags.IsSystemTextJsonDeserializationEnabledByEnvironment(_environmentVariableReader))
            {
                return await ProcessHttpStreamWithStjAsync(httpInitialResponse, take, token);
            }
            else
            {
                return await ProcessHttpStreamWithNsjAsync(httpInitialResponse, take, token);
            }
        }

        private static async Task<V3SearchResults?> ProcessHttpStreamWithStjAsync(HttpResponseMessage httpInitialResponse, uint take, CancellationToken token)
        {
#if NETCOREAPP2_0_OR_GREATER
            using var stream = await httpInitialResponse.Content.ReadAsStreamAsync(token);
#else
            using var stream = await httpInitialResponse.Content.ReadAsStreamAsync();
#endif
            var results = await System.Text.Json.JsonSerializer.DeserializeAsync(stream, PackageSearchJsonContext.Default.V3SearchResults, token);

            if (results?.Data?.Count > take)
            {
                results.Data = results.Data.Take((int)take).ToList();
            }

            return results;
        }

        private static async Task<V3SearchResults?> ProcessHttpStreamWithNsjAsync(HttpResponseMessage httpInitialResponse, uint take, CancellationToken token)
        {
#pragma warning disable IL2026, IL3050 // Legacy Newtonsoft.Json code path is unreachable when feature switch is true; ILC trims this branch in AOT
            var _newtonsoftConvertersSerializer = JsonSerializer.Create(JsonExtensions.ObjectSerializationSettings);
            _newtonsoftConvertersSerializer.Converters.Add(new Converters.V3SearchResultsConverter(take));
#pragma warning restore IL2026, IL3050

#if NETCOREAPP2_0_OR_GREATER
            using (var stream = await httpInitialResponse.Content.ReadAsStreamAsync(token))
#else
            using (var stream = await httpInitialResponse.Content.ReadAsStreamAsync())
#endif
            using (var streamReader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                return _newtonsoftConvertersSerializer.Deserialize<V3SearchResults>(jsonReader);
            }
        }
    }
}
