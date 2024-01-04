// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Core;



namespace Microsoft.DotNet.Helix.Client
{
    public partial interface ILogSearch
    {
        Task DoBuildSearchAsync(
            DateTimeOffset endDate,
            Models.ResponseType responseType,
            DateTimeOffset startDate,
            string repository = default,
            string searchString = default,
            CancellationToken cancellationToken = default
        );

        Task DoTestLogSearchAsync(
            DateTimeOffset endDate,
            Models.ResponseType responseType,
            DateTimeOffset startDate,
            string repository = default,
            string searchString = default,
            CancellationToken cancellationToken = default
        );

    }

    internal partial class LogSearch : IServiceOperations<HelixApi>, ILogSearch
    {
        public LogSearch(HelixApi client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public HelixApi Client { get; }

        partial void HandleFailedRequest(RestApiException ex);

        partial void HandleFailedDoBuildSearchRequest(RestApiException ex);

        public async Task DoBuildSearchAsync(
            DateTimeOffset endDate,
            Models.ResponseType responseType,
            DateTimeOffset startDate,
            string repository = default,
            string searchString = default,
            CancellationToken cancellationToken = default
        )
        {

            if (responseType == default(Models.ResponseType))
            {
                throw new ArgumentNullException(nameof(responseType));
            }

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/logs/search/build",
                false);

            if (!string.IsNullOrEmpty(repository))
            {
                _url.AppendQuery("repository", Client.Serialize(repository));
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                _url.AppendQuery("searchString", Client.Serialize(searchString));
            }
            if (startDate != default(DateTimeOffset))
            {
                _url.AppendQuery("startDate", Client.Serialize(startDate));
            }
            if (endDate != default(DateTimeOffset))
            {
                _url.AppendQuery("endDate", Client.Serialize(endDate));
            }
            if (responseType != default(Models.ResponseType))
            {
                _url.AppendQuery("responseType", Client.Serialize(responseType));
            }
            _url.AppendQuery("api-version", Client.Serialize(apiVersion));


            using (var _req = Client.Pipeline.CreateRequest())
            {
                _req.Uri = _url;
                _req.Method = RequestMethod.Get;

                using (var _res = await Client.SendAsync(_req, cancellationToken).ConfigureAwait(false))
                {
                    if (_res.Status < 200 || _res.Status >= 300)
                    {
                        await OnDoBuildSearchFailed(_req, _res).ConfigureAwait(false);
                    }


                    return;
                }
            }
        }

        internal async Task OnDoBuildSearchFailed(Request req, Response res)
        {
            string content = null;
            if (res.ContentStream != null)
            {
                using (var reader = new StreamReader(res.ContentStream))
                {
                    content = await reader.ReadToEndAsync().ConfigureAwait(false);
                }
            }

            var ex = new RestApiException<Models.ApiError>(
                req,
                res,
                content,
                Client.Deserialize<Models.ApiError>(content)
                );
            HandleFailedDoBuildSearchRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }

        partial void HandleFailedDoTestLogSearchRequest(RestApiException ex);

        public async Task DoTestLogSearchAsync(
            DateTimeOffset endDate,
            Models.ResponseType responseType,
            DateTimeOffset startDate,
            string repository = default,
            string searchString = default,
            CancellationToken cancellationToken = default
        )
        {

            if (responseType == default(Models.ResponseType))
            {
                throw new ArgumentNullException(nameof(responseType));
            }

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/logs/search/test",
                false);

            if (!string.IsNullOrEmpty(repository))
            {
                _url.AppendQuery("repository", Client.Serialize(repository));
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                _url.AppendQuery("searchString", Client.Serialize(searchString));
            }
            if (startDate != default(DateTimeOffset))
            {
                _url.AppendQuery("startDate", Client.Serialize(startDate));
            }
            if (endDate != default(DateTimeOffset))
            {
                _url.AppendQuery("endDate", Client.Serialize(endDate));
            }
            if (responseType != default(Models.ResponseType))
            {
                _url.AppendQuery("responseType", Client.Serialize(responseType));
            }
            _url.AppendQuery("api-version", Client.Serialize(apiVersion));


            using (var _req = Client.Pipeline.CreateRequest())
            {
                _req.Uri = _url;
                _req.Method = RequestMethod.Get;

                using (var _res = await Client.SendAsync(_req, cancellationToken).ConfigureAwait(false))
                {
                    if (_res.Status < 200 || _res.Status >= 300)
                    {
                        await OnDoTestLogSearchFailed(_req, _res).ConfigureAwait(false);
                    }


                    return;
                }
            }
        }

        internal async Task OnDoTestLogSearchFailed(Request req, Response res)
        {
            string content = null;
            if (res.ContentStream != null)
            {
                using (var reader = new StreamReader(res.ContentStream))
                {
                    content = await reader.ReadToEndAsync().ConfigureAwait(false);
                }
            }

            var ex = new RestApiException<Models.ApiError>(
                req,
                res,
                content,
                Client.Deserialize<Models.ApiError>(content)
                );
            HandleFailedDoTestLogSearchRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }
    }
}
