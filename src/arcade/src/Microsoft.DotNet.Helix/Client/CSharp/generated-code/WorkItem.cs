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
    public partial interface IWorkItem
    {
        Task<System.IO.Stream> GetFileAsync(
            string file,
            string id,
            string job,
            bool? latestOnly = default,
            CancellationToken cancellationToken = default
        );

        Task<IImmutableList<Models.UploadedFile>> ListFilesAsync(
            string id,
            string job,
            bool? latestOnly = default,
            CancellationToken cancellationToken = default
        );

        Task<System.IO.Stream> ConsoleLogAsync(
            string id,
            string job,
            CancellationToken cancellationToken = default
        );

        Task<IImmutableList<Models.WorkItemSummary>> ListAsync(
            string job,
            CancellationToken cancellationToken = default
        );

        Task<Models.WorkItemDetails> DetailsAsync(
            string id,
            string job,
            CancellationToken cancellationToken = default
        );

    }

    internal partial class WorkItem : IServiceOperations<HelixApi>, IWorkItem
    {
        public WorkItem(HelixApi client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public HelixApi Client { get; }

        partial void HandleFailedRequest(RestApiException ex);

        partial void HandleFailedGetFileRequest(RestApiException ex);

        public async Task<System.IO.Stream> GetFileAsync(
            string file,
            string id,
            string job,
            bool? latestOnly = default,
            CancellationToken cancellationToken = default
        )
        {

            if (string.IsNullOrEmpty(file))
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (string.IsNullOrEmpty(job))
            {
                throw new ArgumentNullException(nameof(job));
            }

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/jobs/{job}/workitems/{id}/files/{file}".Replace("{job}", Uri.EscapeDataString(Client.Serialize(job))).Replace("{id}", Uri.EscapeDataString(Client.Serialize(id))).Replace("{file}", Uri.EscapeDataString(Client.Serialize(file))),
                false);

            if (latestOnly != default(bool?))
            {
                _url.AppendQuery("latestOnly", Client.Serialize(latestOnly));
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
                        await OnGetFileFailed(_req, _res).ConfigureAwait(false);
                    }

                    if (_res.ContentStream == null)
                    {
                        await OnGetFileFailed(_req, _res).ConfigureAwait(false);
                    }

                    return new ResponseStream(_res.ContentStream, _res);
                }
            }
        }

        internal async Task OnGetFileFailed(Request req, Response res)
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
            HandleFailedGetFileRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }

        partial void HandleFailedListFilesRequest(RestApiException ex);

        public async Task<IImmutableList<Models.UploadedFile>> ListFilesAsync(
            string id,
            string job,
            bool? latestOnly = default,
            CancellationToken cancellationToken = default
        )
        {

            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (string.IsNullOrEmpty(job))
            {
                throw new ArgumentNullException(nameof(job));
            }

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/jobs/{job}/workitems/{id}/files".Replace("{job}", Uri.EscapeDataString(Client.Serialize(job))).Replace("{id}", Uri.EscapeDataString(Client.Serialize(id))),
                false);

            if (latestOnly != default(bool?))
            {
                _url.AppendQuery("latestOnly", Client.Serialize(latestOnly));
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
                        await OnListFilesFailed(_req, _res).ConfigureAwait(false);
                    }

                    if (_res.ContentStream == null)
                    {
                        await OnListFilesFailed(_req, _res).ConfigureAwait(false);
                    }

                    using (var _reader = new StreamReader(_res.ContentStream))
                    {
                        var _content = await _reader.ReadToEndAsync().ConfigureAwait(false);
                        var _body = Client.Deserialize<IImmutableList<Models.UploadedFile>>(_content);
                        return _body;
                    }
                }
            }
        }

        internal async Task OnListFilesFailed(Request req, Response res)
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
            HandleFailedListFilesRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }

        partial void HandleFailedConsoleLogRequest(RestApiException ex);

        public async Task<System.IO.Stream> ConsoleLogAsync(
            string id,
            string job,
            CancellationToken cancellationToken = default
        )
        {

            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (string.IsNullOrEmpty(job))
            {
                throw new ArgumentNullException(nameof(job));
            }

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/jobs/{job}/workitems/{id}/console".Replace("{job}", Uri.EscapeDataString(Client.Serialize(job))).Replace("{id}", Uri.EscapeDataString(Client.Serialize(id))),
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
                        await OnConsoleLogFailed(_req, _res).ConfigureAwait(false);
                    }

                    if (_res.ContentStream == null)
                    {
                        await OnConsoleLogFailed(_req, _res).ConfigureAwait(false);
                    }

                    return new ResponseStream(_res.ContentStream, _res);
                }
            }
        }

        internal async Task OnConsoleLogFailed(Request req, Response res)
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
            HandleFailedConsoleLogRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }

        partial void HandleFailedListRequest(RestApiException ex);

        public async Task<IImmutableList<Models.WorkItemSummary>> ListAsync(
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
                "/api/jobs/{job}/workitems".Replace("{job}", Uri.EscapeDataString(Client.Serialize(job))),
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
                        await OnListFailed(_req, _res).ConfigureAwait(false);
                    }

                    if (_res.ContentStream == null)
                    {
                        await OnListFailed(_req, _res).ConfigureAwait(false);
                    }

                    using (var _reader = new StreamReader(_res.ContentStream))
                    {
                        var _content = await _reader.ReadToEndAsync().ConfigureAwait(false);
                        var _body = Client.Deserialize<IImmutableList<Models.WorkItemSummary>>(_content);
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

        partial void HandleFailedDetailsRequest(RestApiException ex);

        public async Task<Models.WorkItemDetails> DetailsAsync(
            string id,
            string job,
            CancellationToken cancellationToken = default
        )
        {

            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (string.IsNullOrEmpty(job))
            {
                throw new ArgumentNullException(nameof(job));
            }

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/jobs/{job}/workitems/{id}".Replace("{job}", Uri.EscapeDataString(Client.Serialize(job))).Replace("{id}", Uri.EscapeDataString(Client.Serialize(id))),
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
                        var _body = Client.Deserialize<Models.WorkItemDetails>(_content);
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
    }
}
