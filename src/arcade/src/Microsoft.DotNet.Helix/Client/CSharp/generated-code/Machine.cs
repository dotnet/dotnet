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
    public partial interface IMachine
    {
        Task ChangeStateAsync(
            Models.MachineStateChangeRequest body,
            string machineName,
            string queueId,
            CancellationToken cancellationToken = default
        );

        Task<Models.MachineInformation> GetMachineStatusAsync(
            string machineName,
            string queueId,
            CancellationToken cancellationToken = default
        );

    }

    internal partial class Machine : IServiceOperations<HelixApi>, IMachine
    {
        public Machine(HelixApi client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public HelixApi Client { get; }

        partial void HandleFailedRequest(RestApiException ex);

        partial void HandleFailedChangeStateRequest(RestApiException ex);

        public async Task ChangeStateAsync(
            Models.MachineStateChangeRequest body,
            string machineName,
            string queueId,
            CancellationToken cancellationToken = default
        )
        {

            if (body == default(Models.MachineStateChangeRequest))
            {
                throw new ArgumentNullException(nameof(body));
            }

            if (string.IsNullOrEmpty(machineName))
            {
                throw new ArgumentNullException(nameof(machineName));
            }

            if (string.IsNullOrEmpty(queueId))
            {
                throw new ArgumentNullException(nameof(queueId));
            }

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/machines/{queueId}/{machineName}/state".Replace("{queueId}", Uri.EscapeDataString(Client.Serialize(queueId))).Replace("{machineName}", Uri.EscapeDataString(Client.Serialize(machineName))),
                false);

            _url.AppendQuery("api-version", Client.Serialize(apiVersion));


            using (var _req = Client.Pipeline.CreateRequest())
            {
                _req.Uri = _url;
                _req.Method = RequestMethod.Put;

                if (body != default(Models.MachineStateChangeRequest))
                {
                    _req.Content = RequestContent.Create(Encoding.UTF8.GetBytes(Client.Serialize(body)));
                    _req.Headers.Add("Content-Type", "application/json; charset=utf-8");
                }

                using (var _res = await Client.SendAsync(_req, cancellationToken).ConfigureAwait(false))
                {
                    if (_res.Status < 200 || _res.Status >= 300)
                    {
                        await OnChangeStateFailed(_req, _res).ConfigureAwait(false);
                    }


                    return;
                }
            }
        }

        internal async Task OnChangeStateFailed(Request req, Response res)
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
            HandleFailedChangeStateRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }

        partial void HandleFailedGetMachineStatusRequest(RestApiException ex);

        public async Task<Models.MachineInformation> GetMachineStatusAsync(
            string machineName,
            string queueId,
            CancellationToken cancellationToken = default
        )
        {

            if (string.IsNullOrEmpty(machineName))
            {
                throw new ArgumentNullException(nameof(machineName));
            }

            if (string.IsNullOrEmpty(queueId))
            {
                throw new ArgumentNullException(nameof(queueId));
            }

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/machines/{queueId}/{machineName}/state".Replace("{queueId}", Uri.EscapeDataString(Client.Serialize(queueId))).Replace("{machineName}", Uri.EscapeDataString(Client.Serialize(machineName))),
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
                        await OnGetMachineStatusFailed(_req, _res).ConfigureAwait(false);
                    }

                    if (_res.ContentStream == null)
                    {
                        await OnGetMachineStatusFailed(_req, _res).ConfigureAwait(false);
                    }

                    using (var _reader = new StreamReader(_res.ContentStream))
                    {
                        var _content = await _reader.ReadToEndAsync().ConfigureAwait(false);
                        var _body = Client.Deserialize<Models.MachineInformation>(_content);
                        return _body;
                    }
                }
            }
        }

        internal async Task OnGetMachineStatusFailed(Request req, Response res)
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
            HandleFailedGetMachineStatusRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }
    }
}
