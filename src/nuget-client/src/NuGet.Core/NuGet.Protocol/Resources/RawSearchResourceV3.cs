// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    [Obsolete("Use PackageSearchResource instead (via SourceRepository.GetResourceAsync<PackageSearchResource>")]
    public class RawSearchResourceV3 : INuGetResource
    {
        private readonly HttpSource _client;
        private readonly Uri[] _searchEndpoints;

        public RawSearchResourceV3(HttpSource client, IEnumerable<Uri> searchEndpoints)
            : base()
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if (searchEndpoints == null)
            {
                throw new ArgumentNullException(nameof(searchEndpoints));
            }

            _client = client;
            _searchEndpoints = searchEndpoints.ToArray();
        }

        [Obsolete("Use PackageSearchResource instead (via SourceRepository.GetResourceAsync<PackageSearchResource>")]
        public virtual async Task<JObject> SearchPage(string searchTerm, SearchFilter filters, int skip, int take, Common.ILogger log, CancellationToken cancellationToken)
        {
            for (var i = 0; i < _searchEndpoints.Length; i++)
            {
                var endpoint = _searchEndpoints[i];

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

                if (filters.PackageTypes != null
                    && filters.PackageTypes.Any())
                {
                    var types = string.Join("&",
                        filters.PackageTypes.Select(
                            s => "packageTypeFilter=" + s));
                    queryString += "&" + types;
                }

                queryString += "&semVerLevel=2.0.0";

                queryUrl.Query = queryString;

                JObject searchJson = null;
                try
                {
                    searchJson = await _client.GetJObjectAsync(
                        new HttpSourceRequest(queryUrl.Uri, log),
                        log,
                        cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch when (i < _searchEndpoints.Length - 1)
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

                if (searchJson != null)
                {
                    return searchJson;
                }
            }

            // TODO: get a better message for this
            throw new FatalProtocolException(Strings.Protocol_MissingSearchService);
        }

        [Obsolete("Use PackageSearchResource instead (via SourceRepository.GetResourceAsync<PackageSearchResource>")]
        public virtual async Task<IEnumerable<JObject>> Search(string searchTerm, SearchFilter filters, int skip, int take, Common.ILogger log, CancellationToken cancellationToken)
        {
            var results = await SearchPage(searchTerm, filters, skip, take, log, cancellationToken);

            var data = results[JsonProperties.Data] as JArray ?? Enumerable.Empty<JToken>();
            return data.OfType<JObject>();
        }

    }
}
