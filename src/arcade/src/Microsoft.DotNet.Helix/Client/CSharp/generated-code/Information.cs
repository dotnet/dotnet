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
    public partial interface IInformation
    {
        Task<Models.QueueInfo> QueueInfoAsync(
            string queueId,
            bool? includeQueueDepth = default,
            CancellationToken cancellationToken = default
        );

        Task<IImmutableList<Models.QueueInfo>> QueueInfoListAsync(
            bool? includeQueueDepth = default,
            CancellationToken cancellationToken = default
        );

        Task<IImmutableList<Models.Deploy1esImagesResult>> Deployed1esImagesInfoListAsync(
            CancellationToken cancellationToken = default
        );

    }

    internal partial class Information : IServiceOperations<HelixApi>, IInformation
    {
        public Information(HelixApi client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public HelixApi Client { get; }

        partial void HandleFailedRequest(RestApiException ex);

        partial void HandleFailedQueueInfoRequest(RestApiException ex);

        public async Task<Models.QueueInfo> QueueInfoAsync(
            string queueId,
            bool? includeQueueDepth = default,
            CancellationToken cancellationToken = default
        )
        {

            if (string.IsNullOrEmpty(queueId))
            {
                throw new ArgumentNullException(nameof(queueId));
            }

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/info/queues/{queueId}".Replace("{queueId}", Uri.EscapeDataString(Client.Serialize(queueId))),
                false);

            if (includeQueueDepth != default(bool?))
            {
                _url.AppendQuery("includeQueueDepth", Client.Serialize(includeQueueDepth));
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
                        await OnQueueInfoFailed(_req, _res).ConfigureAwait(false);
                    }

                    if (_res.ContentStream == null)
                    {
                        await OnQueueInfoFailed(_req, _res).ConfigureAwait(false);
                    }

                    using (var _reader = new StreamReader(_res.ContentStream))
                    {
                        var _content = await _reader.ReadToEndAsync().ConfigureAwait(false);
                        var _body = Client.Deserialize<Models.QueueInfo>(_content);
                        return _body;
                    }
                }
            }
        }

        internal async Task OnQueueInfoFailed(Request req, Response res)
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
            HandleFailedQueueInfoRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }

        partial void HandleFailedQueueInfoListRequest(RestApiException ex);

        public async Task<IImmutableList<Models.QueueInfo>> QueueInfoListAsync(
            bool? includeQueueDepth = default,
            CancellationToken cancellationToken = default
        )
        {

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/info/queues",
                false);

            if (includeQueueDepth != default(bool?))
            {
                _url.AppendQuery("includeQueueDepth", Client.Serialize(includeQueueDepth));
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
                        await OnQueueInfoListFailed(_req, _res).ConfigureAwait(false);
                    }

                    if (_res.ContentStream == null)
                    {
                        await OnQueueInfoListFailed(_req, _res).ConfigureAwait(false);
                    }

                    using (var _reader = new StreamReader(_res.ContentStream))
                    {
                        var _content = await _reader.ReadToEndAsync().ConfigureAwait(false);
                        var _body = Client.Deserialize<IImmutableList<Models.QueueInfo>>(_content);
                        return _body;
                    }
                }
            }
        }

        internal async Task OnQueueInfoListFailed(Request req, Response res)
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
            HandleFailedQueueInfoListRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }

        partial void HandleFailedDeployed1esImagesInfoListRequest(RestApiException ex);

        public async Task<IImmutableList<Models.Deploy1esImagesResult>> Deployed1esImagesInfoListAsync(
            CancellationToken cancellationToken = default
        )
        {

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/info/1esimages",
                false);

            _url.AppendQuery("api-version", Client.Serialize(apiVersion));


            using (var _req = Client.Pipeline.CreateRequest())
            {
                _req.Uri = _url;
                _req.Method = RequestMethod.Get;

                using (var _res = await Client.SendAsync(_req, cancellationToken).ConfigureAwait(false))
                {
                    if (_res.Status < 200 || _res.Status >= 300)
                    {
                        await OnDeployed1esImagesInfoListFailed(_req, _res).ConfigureAwait(false);
                    }

                    if (_res.ContentStream == null)
                    {
                        await OnDeployed1esImagesInfoListFailed(_req, _res).ConfigureAwait(false);
                    }

                    using (var _reader = new StreamReader(_res.ContentStream))
                    {
                        var _content = await _reader.ReadToEndAsync().ConfigureAwait(false);
                        var _body = Client.Deserialize<IImmutableList<Models.Deploy1esImagesResult>>(_content);
                        return _body;
                    }
                }
            }
        }

        internal async Task OnDeployed1esImagesInfoListFailed(Request req, Response res)
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
            HandleFailedDeployed1esImagesInfoListRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }
    }
}
