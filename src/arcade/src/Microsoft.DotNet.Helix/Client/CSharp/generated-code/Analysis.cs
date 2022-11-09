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
    public partial interface IAnalysis
    {
        Task SetReasonAsync(
            string analysisName,
            string analysisType,
            Models.FailureReason body,
            string job,
            string workitem,
            CancellationToken cancellationToken = default
        );

        Task<Newtonsoft.Json.Linq.JToken> GetDetailsAsync(
            string analysisName,
            string analysisType,
            string job,
            string workitem,
            CancellationToken cancellationToken = default
        );

    }

    internal partial class Analysis : IServiceOperations<HelixApi>, IAnalysis
    {
        public Analysis(HelixApi client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public HelixApi Client { get; }

        partial void HandleFailedRequest(RestApiException ex);

        partial void HandleFailedSetReasonRequest(RestApiException ex);

        public async Task SetReasonAsync(
            string analysisName,
            string analysisType,
            Models.FailureReason body,
            string job,
            string workitem,
            CancellationToken cancellationToken = default
        )
        {

            if (string.IsNullOrEmpty(analysisName))
            {
                throw new ArgumentNullException(nameof(analysisName));
            }

            if (string.IsNullOrEmpty(analysisType))
            {
                throw new ArgumentNullException(nameof(analysisType));
            }

            if (body == default(Models.FailureReason))
            {
                throw new ArgumentNullException(nameof(body));
            }

            if (string.IsNullOrEmpty(job))
            {
                throw new ArgumentNullException(nameof(job));
            }

            if (string.IsNullOrEmpty(workitem))
            {
                throw new ArgumentNullException(nameof(workitem));
            }

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/analysis/{job}/{analysisType}/reason".Replace("{job}", Uri.EscapeDataString(Client.Serialize(job))).Replace("{analysisType}", Uri.EscapeDataString(Client.Serialize(analysisType))),
                false);

            if (!string.IsNullOrEmpty(workitem))
            {
                _url.AppendQuery("workitem", Client.Serialize(workitem));
            }
            if (!string.IsNullOrEmpty(analysisName))
            {
                _url.AppendQuery("analysisName", Client.Serialize(analysisName));
            }
            _url.AppendQuery("api-version", Client.Serialize(apiVersion));


            using (var _req = Client.Pipeline.CreateRequest())
            {
                _req.Uri = _url;
                _req.Method = RequestMethod.Put;

                if (body != default(Models.FailureReason))
                {
                    _req.Content = RequestContent.Create(Encoding.UTF8.GetBytes(Client.Serialize(body)));
                    _req.Headers.Add("Content-Type", "application/json; charset=utf-8");
                }

                using (var _res = await Client.SendAsync(_req, cancellationToken).ConfigureAwait(false))
                {
                    if (_res.Status < 200 || _res.Status >= 300)
                    {
                        await OnSetReasonFailed(_req, _res).ConfigureAwait(false);
                    }


                    return;
                }
            }
        }

        internal async Task OnSetReasonFailed(Request req, Response res)
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
            HandleFailedSetReasonRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }

        partial void HandleFailedGetDetailsRequest(RestApiException ex);

        public async Task<Newtonsoft.Json.Linq.JToken> GetDetailsAsync(
            string analysisName,
            string analysisType,
            string job,
            string workitem,
            CancellationToken cancellationToken = default
        )
        {

            if (string.IsNullOrEmpty(analysisName))
            {
                throw new ArgumentNullException(nameof(analysisName));
            }

            if (string.IsNullOrEmpty(analysisType))
            {
                throw new ArgumentNullException(nameof(analysisType));
            }

            if (string.IsNullOrEmpty(job))
            {
                throw new ArgumentNullException(nameof(job));
            }

            if (string.IsNullOrEmpty(workitem))
            {
                throw new ArgumentNullException(nameof(workitem));
            }

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/analysis/{job}/{analysisType}".Replace("{job}", Uri.EscapeDataString(Client.Serialize(job))).Replace("{analysisType}", Uri.EscapeDataString(Client.Serialize(analysisType))),
                false);

            if (!string.IsNullOrEmpty(workitem))
            {
                _url.AppendQuery("workitem", Client.Serialize(workitem));
            }
            if (!string.IsNullOrEmpty(analysisName))
            {
                _url.AppendQuery("analysisName", Client.Serialize(analysisName));
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
                        await OnGetDetailsFailed(_req, _res).ConfigureAwait(false);
                    }

                    if (_res.ContentStream == null)
                    {
                        await OnGetDetailsFailed(_req, _res).ConfigureAwait(false);
                    }

                    using (var _reader = new StreamReader(_res.ContentStream))
                    {
                        var _content = await _reader.ReadToEndAsync().ConfigureAwait(false);
                        var _body = Client.Deserialize<Newtonsoft.Json.Linq.JToken>(_content);
                        return _body;
                    }
                }
            }
        }

        internal async Task OnGetDetailsFailed(Request req, Response res)
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
            HandleFailedGetDetailsRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }
    }
}
