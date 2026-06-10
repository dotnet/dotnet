// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

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
        private readonly Uri[] _searchEndpoints;
        private readonly HashSet<Uri> _packageTypeCapableEndpoints;
        private readonly IEnvironmentVariableReader _environmentVariableReader;

        internal PackageSearchResourceV3(
            HttpSource client,
            IEnumerable<Uri> searchEndpoints,
            IEnumerable<Uri> packageTypeCapableEndpoints = null,
            IEnvironmentVariableReader environmentVariableReader = null)
            : base()
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _searchEndpoints = searchEndpoints?.ToArray() ?? throw new ArgumentNullException(nameof(searchEndpoints));
            _packageTypeCapableEndpoints = packageTypeCapableEndpoints == null
                ? null
                : new HashSet<Uri>(packageTypeCapableEndpoints);
            _environmentVariableReader = environmentVariableReader;
        }

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

            var searchResults = searchResultMetadata
                .Select(m => m.WithVersions(() => GetVersions(m, filter)))
                .Select(m => { ((PackageSearchMetadataBuilder.ClonedPackageSearchMetadata)m).CacheStrings(metadataCache); return m; })
                .ToArray();

            return searchResults;
        }

        private static IEnumerable<VersionInfo> GetVersions(PackageSearchMetadata metadata, SearchFilter filter)
        {
            var uniqueVersions = new HashSet<Versioning.NuGetVersion>();
            var versions = new List<VersionInfo>();
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
            var packageTypeFilterRequested = filters.PackageTypes != null && filters.PackageTypes.Any();

            if (packageTypeFilterRequested)
            {
                if (filters.PackageTypes.Count() > 1)
                {
                    throw new ArgumentException(Strings.Protocol_PackageTypeFilterMultipleNotSupported, nameof(filters));
                }

                if (_packageTypeCapableEndpoints == null || _packageTypeCapableEndpoints.Count == 0)
                {
                    throw new NotSupportedException(Strings.Protocol_PackageTypeFilterNotSupported);
                }
            }

            // When package type filtering is requested, only iterate endpoints that advertise
            // SearchQueryService/3.5.0. Otherwise, use the full set of search endpoints.
            var endpoints = packageTypeFilterRequested
                ? _packageTypeCapableEndpoints.ToArray()
                : _searchEndpoints;

            log.LogVerbose($"Found {endpoints.Length} search endpoints.");

            for (var i = 0; i < endpoints.Length; i++)
            {
                var endpoint = endpoints[i];

                // The search term comes in already encoded from VS
                var queryUrl = new UriBuilder(endpoint.AbsoluteUri);
                var queryString =
                    "q=" + searchTerm +
                    "&skip=" + skip.ToString(CultureInfo.CurrentCulture) +
                    "&take=" + take.ToString(CultureInfo.CurrentCulture) +
                    "&prerelease=" + filters.IncludePrerelease.ToString(CultureInfo.CurrentCulture).ToLowerInvariant();

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
                    // SearchQueryService/3.5.0 specifies a single 'packageType' parameter.
                    // Multi-value inputs are rejected above; here the collection has exactly one entry.
                    queryString += "&packageType=" + filters.PackageTypes.First();
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
                catch when (i < endpoints.Length - 1)
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

        private async Task<T> Search<T>(
            Func<HttpSource, Uri, Task<T>> getResultAsync,
            string searchTerm,
            SearchFilter filters,
            int skip,
            int take,
            Common.ILogger log,
            CancellationToken cancellationToken)
        {
            return await SearchPage(
                uri => getResultAsync(_client, uri),
                searchTerm,
                filters,
                skip,
                take,
                log,
                cancellationToken);
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
        internal async Task<IEnumerable<PackageSearchMetadata>> Search(
            string searchTerm,
            SearchFilter filters,
            int skip,
            int take,
            Common.ILogger log,
            CancellationToken cancellationToken)
        {
            return await Search(
                (httpSource, uri) => httpSource.ProcessHttpStreamAsync(
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

        internal async Task<IEnumerable<PackageSearchMetadata>> ProcessHttpStreamTakeCountedItemAsync(HttpResponseMessage httpInitialResponse, int take, CancellationToken token)
        {
            if (take <= 0)
            {
                return Enumerable.Empty<PackageSearchMetadata>();
            }

            return (await ProcessHttpStreamWithoutBufferingAsync(httpInitialResponse, (uint)take, token)).Data;
        }

        private async Task<V3SearchResults> ProcessHttpStreamWithoutBufferingAsync(HttpResponseMessage httpInitialResponse, uint take, CancellationToken token)
        {
            if (httpInitialResponse == null)
            {
                return null;
            }

            if (NuGetFeatureFlags.UseSystemTextJsonDeserializationFeatureSwitch
                || NuGetFeatureFlags.IsSystemTextJsonDeserializationEnabledByEnvironment(_environmentVariableReader))
            {
                return await ProcessHttpStreamWithStjAsync(httpInitialResponse, take, token);
            }
            else
            {
                return await ProcessHttpStreamWithNsjAsync(httpInitialResponse, take, token);
            }
        }

        private static async Task<V3SearchResults> ProcessHttpStreamWithStjAsync(HttpResponseMessage httpInitialResponse, uint take, CancellationToken token)
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

        private static async Task<V3SearchResults> ProcessHttpStreamWithNsjAsync(HttpResponseMessage httpInitialResponse, uint take, CancellationToken token)
        {
            var _newtonsoftConvertersSerializer = JsonSerializer.Create(JsonExtensions.ObjectSerializationSettings);
            _newtonsoftConvertersSerializer.Converters.Add(new Converters.V3SearchResultsConverter(take));

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
