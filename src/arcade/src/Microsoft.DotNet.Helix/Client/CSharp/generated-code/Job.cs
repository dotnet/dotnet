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
    public partial interface IJob
    {
        Task<Models.JobCreationResult> NewAsync(
            Models.JobCreationRequest body,
            string idempotencyKey,
            CancellationToken cancellationToken = default
        );

        Task<IImmutableList<Models.JobSummary>> ListAsync(
            string build = default,
            int? count = default,
            string creator = default,
            string name = default,
            string source = default,
            string type = default,
            CancellationToken cancellationToken = default
        );

        Task<Models.JobPassFail> PassFailAsync(
            string job,
            CancellationToken cancellationToken = default
        );

        Task<Models.JobSummary> SummaryAsync(
            string job,
            CancellationToken cancellationToken = default
        );

        Task<Models.JobDetails> DetailsAsync(
            string job,
            CancellationToken cancellationToken = default
        );

        Task CancelAsync(
            string job,
            string jobCancellationToken = default,
            CancellationToken cancellationToken = default
        );

        Task WaitAsync(
            string job,
            CancellationToken cancellationToken = default
        );

    }

    internal partial class Job : IServiceOperations<HelixApi>, IJob
    {
        public Job(HelixApi client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public HelixApi Client { get; }

        partial void HandleFailedRequest(RestApiException ex);

        partial void HandleFailedNewRequest(RestApiException ex);

        public async Task<Models.JobCreationResult> NewAsync(
            Models.JobCreationRequest body,
            string idempotencyKey,
            CancellationToken cancellationToken = default
        )
        {

            if (body == default(Models.JobCreationRequest))
            {
                throw new ArgumentNullException(nameof(body));
            }

            if (!body.IsValid)
            {
                throw new ArgumentException("The parameter is not valid", nameof(body));
            }

            if (string.IsNullOrEmpty(idempotencyKey))
            {
                throw new ArgumentNullException(nameof(idempotencyKey));
            }

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/jobs",
                false);

            _url.AppendQuery("api-version", Client.Serialize(apiVersion));


            using (var _req = Client.Pipeline.CreateRequest())
            {
                _req.Uri = _url;
                _req.Method = RequestMethod.Post;

                if (!string.IsNullOrEmpty(idempotencyKey))
                {
                    _req.Headers.Add("Idempotency-Key", idempotencyKey);
                }

                if (body != default(Models.JobCreationRequest))
                {
                    _req.Content = RequestContent.Create(Encoding.UTF8.GetBytes(Client.Serialize(body)));
                    _req.Headers.Add("Content-Type", "application/json; charset=utf-8");
                }

                using (var _res = await Client.SendAsync(_req, cancellationToken).ConfigureAwait(false))
                {
                    if (_res.Status < 200 || _res.Status >= 300)
                    {
                        await OnNewFailed(_req, _res).ConfigureAwait(false);
                    }

                    if (_res.ContentStream == null)
                    {
                        await OnNewFailed(_req, _res).ConfigureAwait(false);
                    }

                    using (var _reader = new StreamReader(_res.ContentStream))
                    {
                        var _content = await _reader.ReadToEndAsync().ConfigureAwait(false);
                        var _body = Client.Deserialize<Models.JobCreationResult>(_content);
                        return _body;
                    }
                }
            }
        }

        internal async Task OnNewFailed(Request req, Response res)
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
            HandleFailedNewRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }

        partial void HandleFailedListRequest(RestApiException ex);

        public async Task<IImmutableList<Models.JobSummary>> ListAsync(
            string build = default,
            int? count = default,
            string creator = default,
            string name = default,
            string source = default,
            string type = default,
            CancellationToken cancellationToken = default
        )
        {

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/jobs",
                false);

            if (!string.IsNullOrEmpty(creator))
            {
                _url.AppendQuery("Creator", Client.Serialize(creator));
            }
            if (!string.IsNullOrEmpty(source))
            {
                _url.AppendQuery("Source", Client.Serialize(source));
            }
            if (!string.IsNullOrEmpty(type))
            {
                _url.AppendQuery("Type", Client.Serialize(type));
            }
            if (!string.IsNullOrEmpty(build))
            {
                _url.AppendQuery("Build", Client.Serialize(build));
            }
            if (!string.IsNullOrEmpty(name))
            {
                _url.AppendQuery("Name", Client.Serialize(name));
            }
            if (count != default(int?))
            {
                _url.AppendQuery("count", Client.Serialize(count));
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
                        await OnListFailed(_req, _res).ConfigureAwait(false);
                    }

                    if (_res.ContentStream == null)
                    {
                        await OnListFailed(_req, _res).ConfigureAwait(false);
                    }

                    using (var _reader = new StreamReader(_res.ContentStream))
                    {
                        var _content = await _reader.ReadToEndAsync().ConfigureAwait(false);
                        var _body = Client.Deserialize<IImmutableList<Models.JobSummary>>(_content);
                        return _body;
                    }
                }
            }
        }

        internal async Task OnListFailed(Request req, Response res)
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
            HandleFailedListRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }

        partial void HandleFailedPassFailRequest(RestApiException ex);

        public async Task<Models.JobPassFail> PassFailAsync(
            string job,
            CancellationToken cancellationToken = default
        )
        {

            if (string.IsNullOrEmpty(job))
            {
                throw new ArgumentNullException(nameof(job));
            }

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/jobs/{job}/pf".Replace("{job}", Uri.EscapeDataString(Client.Serialize(job))),
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
                        await OnPassFailFailed(_req, _res).ConfigureAwait(false);
                    }

                    if (_res.ContentStream == null)
                    {
                        await OnPassFailFailed(_req, _res).ConfigureAwait(false);
                    }

                    using (var _reader = new StreamReader(_res.ContentStream))
                    {
                        var _content = await _reader.ReadToEndAsync().ConfigureAwait(false);
                        var _body = Client.Deserialize<Models.JobPassFail>(_content);
                        return _body;
                    }
                }
            }
        }

        internal async Task OnPassFailFailed(Request req, Response res)
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
            HandleFailedPassFailRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }

        partial void HandleFailedSummaryRequest(RestApiException ex);

        public async Task<Models.JobSummary> SummaryAsync(
            string job,
            CancellationToken cancellationToken = default
        )
        {

            if (string.IsNullOrEmpty(job))
            {
                throw new ArgumentNullException(nameof(job));
            }

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/jobs/{job}".Replace("{job}", Uri.EscapeDataString(Client.Serialize(job))),
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
                        await OnSummaryFailed(_req, _res).ConfigureAwait(false);
                    }

                    if (_res.ContentStream == null)
                    {
                        await OnSummaryFailed(_req, _res).ConfigureAwait(false);
                    }

                    using (var _reader = new StreamReader(_res.ContentStream))
                    {
                        var _content = await _reader.ReadToEndAsync().ConfigureAwait(false);
                        var _body = Client.Deserialize<Models.JobSummary>(_content);
                        return _body;
                    }
                }
            }
        }

        internal async Task OnSummaryFailed(Request req, Response res)
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
            HandleFailedSummaryRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }

        partial void HandleFailedDetailsRequest(RestApiException ex);

        public async Task<Models.JobDetails> DetailsAsync(
            string job,
            CancellationToken cancellationToken = default
        )
        {

            if (string.IsNullOrEmpty(job))
            {
                throw new ArgumentNullException(nameof(job));
            }

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/jobs/{job}/details".Replace("{job}", Uri.EscapeDataString(Client.Serialize(job))),
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
                        await OnDetailsFailed(_req, _res).ConfigureAwait(false);
                    }

                    if (_res.ContentStream == null)
                    {
                        await OnDetailsFailed(_req, _res).ConfigureAwait(false);
                    }

                    using (var _reader = new StreamReader(_res.ContentStream))
                    {
                        var _content = await _reader.ReadToEndAsync().ConfigureAwait(false);
                        var _body = Client.Deserialize<Models.JobDetails>(_content);
                        return _body;
                    }
                }
            }
        }

        internal async Task OnDetailsFailed(Request req, Response res)
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
            HandleFailedDetailsRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }

        partial void HandleFailedCancelRequest(RestApiException ex);

        public async Task CancelAsync(
            string job,
            string jobCancellationToken = default,
            CancellationToken cancellationToken = default
        )
        {

            if (string.IsNullOrEmpty(job))
            {
                throw new ArgumentNullException(nameof(job));
            }

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/jobs/{job}/cancel".Replace("{job}", Uri.EscapeDataString(Client.Serialize(job))),
                false);

            if (!string.IsNullOrEmpty(jobCancellationToken))
            {
                _url.AppendQuery("jobCancellationToken", Client.Serialize(jobCancellationToken));
            }
            _url.AppendQuery("api-version", Client.Serialize(apiVersion));


            using (var _req = Client.Pipeline.CreateRequest())
            {
                _req.Uri = _url;
                _req.Method = RequestMethod.Post;

                using (var _res = await Client.SendAsync(_req, cancellationToken).ConfigureAwait(false))
                {
                    if (_res.Status < 200 || _res.Status >= 300)
                    {
                        await OnCancelFailed(_req, _res).ConfigureAwait(false);
                    }


                    return;
                }
            }
        }

        internal async Task OnCancelFailed(Request req, Response res)
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
            HandleFailedCancelRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }

        partial void HandleFailedWaitRequest(RestApiException ex);

        public async Task WaitAsync(
            string job,
            CancellationToken cancellationToken = default
        )
        {

            if (string.IsNullOrEmpty(job))
            {
                throw new ArgumentNullException(nameof(job));
            }

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/jobs/{job}/wait".Replace("{job}", Uri.EscapeDataString(Client.Serialize(job))),
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
                        await OnWaitFailed(_req, _res).ConfigureAwait(false);
                    }


                    return;
                }
            }
        }

        internal async Task OnWaitFailed(Request req, Response res)
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
            HandleFailedWaitRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }
    }
}
