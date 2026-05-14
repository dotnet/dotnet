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
    public partial interface ITelemetry
    {
        Task<string> StartJobAsync(
            Models.JobInfo body,
            CancellationToken cancellationToken = default
        );

        Task<string> StartBuildWorkItemAsync(
            string buildUri,
            string xHelixJobToken,
            CancellationToken cancellationToken = default
        );

        Task FinishBuildWorkItemAsync(
            int errorCount,
            string id,
            int warningCount,
            string xHelixJobToken,
            string logUri = default,
            CancellationToken cancellationToken = default
        );

        Task<string> StartXUnitWorkItemAsync(
            string friendlyName,
            string xHelixJobToken,
            CancellationToken cancellationToken = default
        );

        Task FinishXUnitWorkItemAsync(
            int exitCode,
            string id,
            string resultsXmlUri,
            string xHelixJobToken,
            string logUri = default,
            CancellationToken cancellationToken = default
        );

        Task WarningAsync(
            string eid,
            string id,
            string xHelixJobToken,
            string logUri = default,
            string message = default,
            CancellationToken cancellationToken = default
        );

        Task ErrorAsync(
            string eid,
            string id,
            string xHelixJobToken,
            string logUri = default,
            string message = default,
            CancellationToken cancellationToken = default
        );

        Task LogAsync(
            string id,
            string logUri,
            string xHelixJobToken,
            string format = default,
            string module = default,
            CancellationToken cancellationToken = default
        );

    }

    internal partial class Telemetry : IServiceOperations<HelixApi>, ITelemetry
    {
        public Telemetry(HelixApi client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public HelixApi Client { get; }

        partial void HandleFailedRequest(RestApiException ex);

        partial void HandleFailedStartJobRequest(RestApiException ex);

        public async Task<string> StartJobAsync(
            Models.JobInfo body,
            CancellationToken cancellationToken = default
        )
        {

            if (body == default(Models.JobInfo))
            {
                throw new ArgumentNullException(nameof(body));
            }

            if (!body.IsValid)
            {
                throw new ArgumentException("The parameter is not valid", nameof(body));
            }

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/telemetry/job",
                false);

            _url.AppendQuery("api-version", Client.Serialize(apiVersion));


            using (var _req = Client.Pipeline.CreateRequest())
            {
                _req.Uri = _url;
                _req.Method = RequestMethod.Post;

                if (body != default(Models.JobInfo))
                {
                    _req.Content = RequestContent.Create(Encoding.UTF8.GetBytes(Client.Serialize(body)));
                    _req.Headers.Add("Content-Type", "application/json; charset=utf-8");
                }

                using (var _res = await Client.SendAsync(_req, cancellationToken).ConfigureAwait(false))
                {
                    if (_res.Status < 200 || _res.Status >= 300)
                    {
                        await OnStartJobFailed(_req, _res).ConfigureAwait(false);
                    }

                    if (_res.ContentStream == null)
                    {
                        await OnStartJobFailed(_req, _res).ConfigureAwait(false);
                    }

                    using (var _reader = new StreamReader(_res.ContentStream))
                    {
                        var _content = await _reader.ReadToEndAsync().ConfigureAwait(false);
                        var _body = Client.Deserialize<string>(_content);
                        return _body;
                    }
                }
            }
        }

        internal async Task OnStartJobFailed(Request req, Response res)
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
            HandleFailedStartJobRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }

        partial void HandleFailedStartBuildWorkItemRequest(RestApiException ex);

        public async Task<string> StartBuildWorkItemAsync(
            string buildUri,
            string xHelixJobToken,
            CancellationToken cancellationToken = default
        )
        {

            if (string.IsNullOrEmpty(buildUri))
            {
                throw new ArgumentNullException(nameof(buildUri));
            }

            if (string.IsNullOrEmpty(xHelixJobToken))
            {
                throw new ArgumentNullException(nameof(xHelixJobToken));
            }

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/telemetry/job/build",
                false);

            if (!string.IsNullOrEmpty(buildUri))
            {
                _url.AppendQuery("buildUri", Client.Serialize(buildUri));
            }
            _url.AppendQuery("api-version", Client.Serialize(apiVersion));


            using (var _req = Client.Pipeline.CreateRequest())
            {
                _req.Uri = _url;
                _req.Method = RequestMethod.Post;

                if (!string.IsNullOrEmpty(xHelixJobToken))
                {
                    _req.Headers.Add("X-Helix-Job-Token", xHelixJobToken);
                }

                using (var _res = await Client.SendAsync(_req, cancellationToken).ConfigureAwait(false))
                {
                    if (_res.Status < 200 || _res.Status >= 300)
                    {
                        await OnStartBuildWorkItemFailed(_req, _res).ConfigureAwait(false);
                    }

                    if (_res.ContentStream == null)
                    {
                        await OnStartBuildWorkItemFailed(_req, _res).ConfigureAwait(false);
                    }

                    using (var _reader = new StreamReader(_res.ContentStream))
                    {
                        var _content = await _reader.ReadToEndAsync().ConfigureAwait(false);
                        var _body = Client.Deserialize<string>(_content);
                        return _body;
                    }
                }
            }
        }

        internal async Task OnStartBuildWorkItemFailed(Request req, Response res)
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
            HandleFailedStartBuildWorkItemRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }

        partial void HandleFailedFinishBuildWorkItemRequest(RestApiException ex);

        public async Task FinishBuildWorkItemAsync(
            int errorCount,
            string id,
            int warningCount,
            string xHelixJobToken,
            string logUri = default,
            CancellationToken cancellationToken = default
        )
        {

            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (string.IsNullOrEmpty(xHelixJobToken))
            {
                throw new ArgumentNullException(nameof(xHelixJobToken));
            }

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/telemetry/job/build/{id}/finish".Replace("{id}", Uri.EscapeDataString(Client.Serialize(id))),
                false);

            if (errorCount != default(int))
            {
                _url.AppendQuery("errorCount", Client.Serialize(errorCount));
            }
            if (warningCount != default(int))
            {
                _url.AppendQuery("warningCount", Client.Serialize(warningCount));
            }
            if (!string.IsNullOrEmpty(logUri))
            {
                _url.AppendQuery("logUri", Client.Serialize(logUri));
            }
            _url.AppendQuery("api-version", Client.Serialize(apiVersion));


            using (var _req = Client.Pipeline.CreateRequest())
            {
                _req.Uri = _url;
                _req.Method = RequestMethod.Post;

                if (!string.IsNullOrEmpty(xHelixJobToken))
                {
                    _req.Headers.Add("X-Helix-Job-Token", xHelixJobToken);
                }

                using (var _res = await Client.SendAsync(_req, cancellationToken).ConfigureAwait(false))
                {
                    if (_res.Status < 200 || _res.Status >= 300)
                    {
                        await OnFinishBuildWorkItemFailed(_req, _res).ConfigureAwait(false);
                    }


                    return;
                }
            }
        }

        internal async Task OnFinishBuildWorkItemFailed(Request req, Response res)
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
            HandleFailedFinishBuildWorkItemRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }

        partial void HandleFailedStartXUnitWorkItemRequest(RestApiException ex);

        public async Task<string> StartXUnitWorkItemAsync(
            string friendlyName,
            string xHelixJobToken,
            CancellationToken cancellationToken = default
        )
        {

            if (string.IsNullOrEmpty(friendlyName))
            {
                throw new ArgumentNullException(nameof(friendlyName));
            }

            if (string.IsNullOrEmpty(xHelixJobToken))
            {
                throw new ArgumentNullException(nameof(xHelixJobToken));
            }

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/telemetry/job/xunit",
                false);

            if (!string.IsNullOrEmpty(friendlyName))
            {
                _url.AppendQuery("friendlyName", Client.Serialize(friendlyName));
            }
            _url.AppendQuery("api-version", Client.Serialize(apiVersion));


            using (var _req = Client.Pipeline.CreateRequest())
            {
                _req.Uri = _url;
                _req.Method = RequestMethod.Post;

                if (!string.IsNullOrEmpty(xHelixJobToken))
                {
                    _req.Headers.Add("X-Helix-Job-Token", xHelixJobToken);
                }

                using (var _res = await Client.SendAsync(_req, cancellationToken).ConfigureAwait(false))
                {
                    if (_res.Status < 200 || _res.Status >= 300)
                    {
                        await OnStartXUnitWorkItemFailed(_req, _res).ConfigureAwait(false);
                    }

                    if (_res.ContentStream == null)
                    {
                        await OnStartXUnitWorkItemFailed(_req, _res).ConfigureAwait(false);
                    }

                    using (var _reader = new StreamReader(_res.ContentStream))
                    {
                        var _content = await _reader.ReadToEndAsync().ConfigureAwait(false);
                        var _body = Client.Deserialize<string>(_content);
                        return _body;
                    }
                }
            }
        }

        internal async Task OnStartXUnitWorkItemFailed(Request req, Response res)
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
            HandleFailedStartXUnitWorkItemRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }

        partial void HandleFailedFinishXUnitWorkItemRequest(RestApiException ex);

        public async Task FinishXUnitWorkItemAsync(
            int exitCode,
            string id,
            string resultsXmlUri,
            string xHelixJobToken,
            string logUri = default,
            CancellationToken cancellationToken = default
        )
        {

            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (string.IsNullOrEmpty(resultsXmlUri))
            {
                throw new ArgumentNullException(nameof(resultsXmlUri));
            }

            if (string.IsNullOrEmpty(xHelixJobToken))
            {
                throw new ArgumentNullException(nameof(xHelixJobToken));
            }

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/telemetry/job/xunit/{id}/finish".Replace("{id}", Uri.EscapeDataString(Client.Serialize(id))),
                false);

            if (exitCode != default(int))
            {
                _url.AppendQuery("exitCode", Client.Serialize(exitCode));
            }
            if (!string.IsNullOrEmpty(resultsXmlUri))
            {
                _url.AppendQuery("resultsXmlUri", Client.Serialize(resultsXmlUri));
            }
            if (!string.IsNullOrEmpty(logUri))
            {
                _url.AppendQuery("logUri", Client.Serialize(logUri));
            }
            _url.AppendQuery("api-version", Client.Serialize(apiVersion));


            using (var _req = Client.Pipeline.CreateRequest())
            {
                _req.Uri = _url;
                _req.Method = RequestMethod.Post;

                if (!string.IsNullOrEmpty(xHelixJobToken))
                {
                    _req.Headers.Add("X-Helix-Job-Token", xHelixJobToken);
                }

                using (var _res = await Client.SendAsync(_req, cancellationToken).ConfigureAwait(false))
                {
                    if (_res.Status < 200 || _res.Status >= 300)
                    {
                        await OnFinishXUnitWorkItemFailed(_req, _res).ConfigureAwait(false);
                    }


                    return;
                }
            }
        }

        internal async Task OnFinishXUnitWorkItemFailed(Request req, Response res)
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
            HandleFailedFinishXUnitWorkItemRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }

        partial void HandleFailedWarningRequest(RestApiException ex);

        public async Task WarningAsync(
            string eid,
            string id,
            string xHelixJobToken,
            string logUri = default,
            string message = default,
            CancellationToken cancellationToken = default
        )
        {

            if (string.IsNullOrEmpty(eid))
            {
                throw new ArgumentNullException(nameof(eid));
            }

            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (string.IsNullOrEmpty(xHelixJobToken))
            {
                throw new ArgumentNullException(nameof(xHelixJobToken));
            }

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/telemetry/job/workitem/{id}/warning".Replace("{id}", Uri.EscapeDataString(Client.Serialize(id))),
                false);

            if (!string.IsNullOrEmpty(eid))
            {
                _url.AppendQuery("eid", Client.Serialize(eid));
            }
            if (!string.IsNullOrEmpty(message))
            {
                _url.AppendQuery("message", Client.Serialize(message));
            }
            if (!string.IsNullOrEmpty(logUri))
            {
                _url.AppendQuery("logUri", Client.Serialize(logUri));
            }
            _url.AppendQuery("api-version", Client.Serialize(apiVersion));


            using (var _req = Client.Pipeline.CreateRequest())
            {
                _req.Uri = _url;
                _req.Method = RequestMethod.Post;

                if (!string.IsNullOrEmpty(xHelixJobToken))
                {
                    _req.Headers.Add("X-Helix-Job-Token", xHelixJobToken);
                }

                using (var _res = await Client.SendAsync(_req, cancellationToken).ConfigureAwait(false))
                {
                    if (_res.Status < 200 || _res.Status >= 300)
                    {
                        await OnWarningFailed(_req, _res).ConfigureAwait(false);
                    }


                    return;
                }
            }
        }

        internal async Task OnWarningFailed(Request req, Response res)
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
            HandleFailedWarningRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }

        partial void HandleFailedErrorRequest(RestApiException ex);

        public async Task ErrorAsync(
            string eid,
            string id,
            string xHelixJobToken,
            string logUri = default,
            string message = default,
            CancellationToken cancellationToken = default
        )
        {

            if (string.IsNullOrEmpty(eid))
            {
                throw new ArgumentNullException(nameof(eid));
            }

            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (string.IsNullOrEmpty(xHelixJobToken))
            {
                throw new ArgumentNullException(nameof(xHelixJobToken));
            }

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/telemetry/job/workitem/{id}/error".Replace("{id}", Uri.EscapeDataString(Client.Serialize(id))),
                false);

            if (!string.IsNullOrEmpty(eid))
            {
                _url.AppendQuery("eid", Client.Serialize(eid));
            }
            if (!string.IsNullOrEmpty(message))
            {
                _url.AppendQuery("message", Client.Serialize(message));
            }
            if (!string.IsNullOrEmpty(logUri))
            {
                _url.AppendQuery("logUri", Client.Serialize(logUri));
            }
            _url.AppendQuery("api-version", Client.Serialize(apiVersion));


            using (var _req = Client.Pipeline.CreateRequest())
            {
                _req.Uri = _url;
                _req.Method = RequestMethod.Post;

                if (!string.IsNullOrEmpty(xHelixJobToken))
                {
                    _req.Headers.Add("X-Helix-Job-Token", xHelixJobToken);
                }

                using (var _res = await Client.SendAsync(_req, cancellationToken).ConfigureAwait(false))
                {
                    if (_res.Status < 200 || _res.Status >= 300)
                    {
                        await OnErrorFailed(_req, _res).ConfigureAwait(false);
                    }


                    return;
                }
            }
        }

        internal async Task OnErrorFailed(Request req, Response res)
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
            HandleFailedErrorRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }

        partial void HandleFailedLogRequest(RestApiException ex);

        public async Task LogAsync(
            string id,
            string logUri,
            string xHelixJobToken,
            string format = default,
            string module = default,
            CancellationToken cancellationToken = default
        )
        {

            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (string.IsNullOrEmpty(logUri))
            {
                throw new ArgumentNullException(nameof(logUri));
            }

            if (string.IsNullOrEmpty(xHelixJobToken))
            {
                throw new ArgumentNullException(nameof(xHelixJobToken));
            }

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/telemetry/job/workitem/{id}/log".Replace("{id}", Uri.EscapeDataString(Client.Serialize(id))),
                false);

            if (!string.IsNullOrEmpty(logUri))
            {
                _url.AppendQuery("logUri", Client.Serialize(logUri));
            }
            if (!string.IsNullOrEmpty(format))
            {
                _url.AppendQuery("format", Client.Serialize(format));
            }
            if (!string.IsNullOrEmpty(module))
            {
                _url.AppendQuery("module", Client.Serialize(module));
            }
            _url.AppendQuery("api-version", Client.Serialize(apiVersion));


            using (var _req = Client.Pipeline.CreateRequest())
            {
                _req.Uri = _url;
                _req.Method = RequestMethod.Post;

                if (!string.IsNullOrEmpty(xHelixJobToken))
                {
                    _req.Headers.Add("X-Helix-Job-Token", xHelixJobToken);
                }

                using (var _res = await Client.SendAsync(_req, cancellationToken).ConfigureAwait(false))
                {
                    if (_res.Status < 200 || _res.Status >= 300)
                    {
                        await OnLogFailed(_req, _res).ConfigureAwait(false);
                    }


                    return;
                }
            }
        }

        internal async Task OnLogFailed(Request req, Response res)
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
            HandleFailedLogRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }
    }
}
