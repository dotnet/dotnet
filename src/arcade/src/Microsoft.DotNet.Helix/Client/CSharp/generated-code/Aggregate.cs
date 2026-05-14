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
    public partial interface IAggregate
    {
        Task<Models.AggregateWorkItemSummary> AnalysisSummaryAsync(
            IImmutableList<string> groupBy,
            IImmutableList<string> otherProperties,
            string workitem,
            string build = default,
            string creator = default,
            string name = default,
            string source = default,
            string type = default,
            CancellationToken cancellationToken = default
        );

        Task<IImmutableList<Models.BuildHistoryItem>> BuildHistoryAsync(
            IImmutableList<string> source,
            IImmutableList<string> type,
            CancellationToken cancellationToken = default
        );

        Task<Models.BuildAggregation> BuildAsync(
            string buildNumber,
            IImmutableList<string> sources,
            IImmutableList<string> types,
            CancellationToken cancellationToken = default
        );

        Task<IImmutableList<Models.AggregatedWorkItemCounts>> JobSummaryAsync(
            IImmutableList<string> groupBy,
            int maxResultSets,
            string build = default,
            string creator = default,
            string name = default,
            string source = default,
            string type = default,
            CancellationToken cancellationToken = default
        );

        Task<IImmutableList<Models.AggregatedWorkItemCounts>> WorkItemSummaryAsync(
            IImmutableList<string> groupBy,
            string build = default,
            string creator = default,
            string name = default,
            string source = default,
            string type = default,
            CancellationToken cancellationToken = default
        );

        Task<IImmutableList<Models.AggregateAnalysisDetail>> AnalysisDetailAsync(
            string analysisName,
            string analysisType,
            string build,
            IImmutableList<string> groupBy,
            string source,
            string type,
            string workitem,
            CancellationToken cancellationToken = default
        );

        Task<IImmutableDictionary<string, Newtonsoft.Json.Linq.JToken>> PropertiesAsync(
            string build = default,
            string creator = default,
            string name = default,
            string source = default,
            string type = default,
            CancellationToken cancellationToken = default
        );

        Task<Models.InvestigationResult> Investigation_ContinueAsync(
            string id,
            CancellationToken cancellationToken = default
        );

        Task<Models.InvestigationResult> InvestigationAsync(
            IImmutableList<string> groupBy,
            int maxGroups,
            int maxResults,
            string build = default,
            string creator = default,
            string name = default,
            string source = default,
            string type = default,
            CancellationToken cancellationToken = default
        );

        Task<IImmutableList<Models.HistoricalAnalysisItem>> HistoryAsync(
            string analysisName,
            string analysisType,
            int days,
            string source,
            string type,
            string workitem,
            CancellationToken cancellationToken = default
        );

        Task<Models.MultiSourceResponse> MultiSourceAsync(
            Models.MultiSourceRequest body,
            CancellationToken cancellationToken = default
        );

    }

    internal partial class Aggregate : IServiceOperations<HelixApi>, IAggregate
    {
        public Aggregate(HelixApi client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public HelixApi Client { get; }

        partial void HandleFailedRequest(RestApiException ex);

        partial void HandleFailedAnalysisSummaryRequest(RestApiException ex);

        public async Task<Models.AggregateWorkItemSummary> AnalysisSummaryAsync(
            IImmutableList<string> groupBy,
            IImmutableList<string> otherProperties,
            string workitem,
            string build = default,
            string creator = default,
            string name = default,
            string source = default,
            string type = default,
            CancellationToken cancellationToken = default
        )
        {

            if (groupBy == default(IImmutableList<string>))
            {
                throw new ArgumentNullException(nameof(groupBy));
            }

            if (otherProperties == default(IImmutableList<string>))
            {
                throw new ArgumentNullException(nameof(otherProperties));
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
                "/api/aggregate/analysis",
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
            if (!string.IsNullOrEmpty(workitem))
            {
                _url.AppendQuery("workitem", Client.Serialize(workitem));
            }
            if (groupBy != default(IImmutableList<string>))
            {
                foreach (var _item in groupBy)
                {
                    _url.AppendQuery("groupBy", Client.Serialize(_item));
                }
            }
            if (otherProperties != default(IImmutableList<string>))
            {
                foreach (var _item in otherProperties)
                {
                    _url.AppendQuery("otherProperties", Client.Serialize(_item));
                }
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
                        await OnAnalysisSummaryFailed(_req, _res).ConfigureAwait(false);
                    }

                    if (_res.ContentStream == null)
                    {
                        await OnAnalysisSummaryFailed(_req, _res).ConfigureAwait(false);
                    }

                    using (var _reader = new StreamReader(_res.ContentStream))
                    {
                        var _content = await _reader.ReadToEndAsync().ConfigureAwait(false);
                        var _body = Client.Deserialize<Models.AggregateWorkItemSummary>(_content);
                        return _body;
                    }
                }
            }
        }

        internal async Task OnAnalysisSummaryFailed(Request req, Response res)
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
            HandleFailedAnalysisSummaryRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }

        partial void HandleFailedBuildHistoryRequest(RestApiException ex);

        public async Task<IImmutableList<Models.BuildHistoryItem>> BuildHistoryAsync(
            IImmutableList<string> source,
            IImmutableList<string> type,
            CancellationToken cancellationToken = default
        )
        {

            if (source == default(IImmutableList<string>))
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (type == default(IImmutableList<string>))
            {
                throw new ArgumentNullException(nameof(type));
            }

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/aggregate/build/history",
                false);

            if (source != default(IImmutableList<string>))
            {
                foreach (var _item in source)
                {
                    _url.AppendQuery("source", Client.Serialize(_item));
                }
            }
            if (type != default(IImmutableList<string>))
            {
                foreach (var _item in type)
                {
                    _url.AppendQuery("type", Client.Serialize(_item));
                }
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
                        await OnBuildHistoryFailed(_req, _res).ConfigureAwait(false);
                    }

                    if (_res.ContentStream == null)
                    {
                        await OnBuildHistoryFailed(_req, _res).ConfigureAwait(false);
                    }

                    using (var _reader = new StreamReader(_res.ContentStream))
                    {
                        var _content = await _reader.ReadToEndAsync().ConfigureAwait(false);
                        var _body = Client.Deserialize<IImmutableList<Models.BuildHistoryItem>>(_content);
                        return _body;
                    }
                }
            }
        }

        internal async Task OnBuildHistoryFailed(Request req, Response res)
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
            HandleFailedBuildHistoryRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }

        partial void HandleFailedBuildRequest(RestApiException ex);

        public async Task<Models.BuildAggregation> BuildAsync(
            string buildNumber,
            IImmutableList<string> sources,
            IImmutableList<string> types,
            CancellationToken cancellationToken = default
        )
        {

            if (string.IsNullOrEmpty(buildNumber))
            {
                throw new ArgumentNullException(nameof(buildNumber));
            }

            if (sources == default(IImmutableList<string>))
            {
                throw new ArgumentNullException(nameof(sources));
            }

            if (types == default(IImmutableList<string>))
            {
                throw new ArgumentNullException(nameof(types));
            }

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/aggregate/build",
                false);

            if (sources != default(IImmutableList<string>))
            {
                foreach (var _item in sources)
                {
                    _url.AppendQuery("sources", Client.Serialize(_item));
                }
            }
            if (types != default(IImmutableList<string>))
            {
                foreach (var _item in types)
                {
                    _url.AppendQuery("types", Client.Serialize(_item));
                }
            }
            if (!string.IsNullOrEmpty(buildNumber))
            {
                _url.AppendQuery("buildNumber", Client.Serialize(buildNumber));
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
                        await OnBuildFailed(_req, _res).ConfigureAwait(false);
                    }

                    if (_res.ContentStream == null)
                    {
                        await OnBuildFailed(_req, _res).ConfigureAwait(false);
                    }

                    using (var _reader = new StreamReader(_res.ContentStream))
                    {
                        var _content = await _reader.ReadToEndAsync().ConfigureAwait(false);
                        var _body = Client.Deserialize<Models.BuildAggregation>(_content);
                        return _body;
                    }
                }
            }
        }

        internal async Task OnBuildFailed(Request req, Response res)
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
            HandleFailedBuildRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }

        partial void HandleFailedJobSummaryRequest(RestApiException ex);

        public async Task<IImmutableList<Models.AggregatedWorkItemCounts>> JobSummaryAsync(
            IImmutableList<string> groupBy,
            int maxResultSets,
            string build = default,
            string creator = default,
            string name = default,
            string source = default,
            string type = default,
            CancellationToken cancellationToken = default
        )
        {

            if (groupBy == default(IImmutableList<string>))
            {
                throw new ArgumentNullException(nameof(groupBy));
            }

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/aggregate/jobs",
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
            if (groupBy != default(IImmutableList<string>))
            {
                foreach (var _item in groupBy)
                {
                    _url.AppendQuery("groupBy", Client.Serialize(_item));
                }
            }
            if (maxResultSets != default(int))
            {
                _url.AppendQuery("maxResultSets", Client.Serialize(maxResultSets));
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
                        await OnJobSummaryFailed(_req, _res).ConfigureAwait(false);
                    }

                    if (_res.ContentStream == null)
                    {
                        await OnJobSummaryFailed(_req, _res).ConfigureAwait(false);
                    }

                    using (var _reader = new StreamReader(_res.ContentStream))
                    {
                        var _content = await _reader.ReadToEndAsync().ConfigureAwait(false);
                        var _body = Client.Deserialize<IImmutableList<Models.AggregatedWorkItemCounts>>(_content);
                        return _body;
                    }
                }
            }
        }

        internal async Task OnJobSummaryFailed(Request req, Response res)
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
            HandleFailedJobSummaryRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }

        partial void HandleFailedWorkItemSummaryRequest(RestApiException ex);

        public async Task<IImmutableList<Models.AggregatedWorkItemCounts>> WorkItemSummaryAsync(
            IImmutableList<string> groupBy,
            string build = default,
            string creator = default,
            string name = default,
            string source = default,
            string type = default,
            CancellationToken cancellationToken = default
        )
        {

            if (groupBy == default(IImmutableList<string>))
            {
                throw new ArgumentNullException(nameof(groupBy));
            }

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/aggregate/workitems",
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
            if (groupBy != default(IImmutableList<string>))
            {
                foreach (var _item in groupBy)
                {
                    _url.AppendQuery("groupBy", Client.Serialize(_item));
                }
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
                        await OnWorkItemSummaryFailed(_req, _res).ConfigureAwait(false);
                    }

                    if (_res.ContentStream == null)
                    {
                        await OnWorkItemSummaryFailed(_req, _res).ConfigureAwait(false);
                    }

                    using (var _reader = new StreamReader(_res.ContentStream))
                    {
                        var _content = await _reader.ReadToEndAsync().ConfigureAwait(false);
                        var _body = Client.Deserialize<IImmutableList<Models.AggregatedWorkItemCounts>>(_content);
                        return _body;
                    }
                }
            }
        }

        internal async Task OnWorkItemSummaryFailed(Request req, Response res)
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
            HandleFailedWorkItemSummaryRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }

        partial void HandleFailedAnalysisDetailRequest(RestApiException ex);

        public async Task<IImmutableList<Models.AggregateAnalysisDetail>> AnalysisDetailAsync(
            string analysisName,
            string analysisType,
            string build,
            IImmutableList<string> groupBy,
            string source,
            string type,
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

            if (string.IsNullOrEmpty(build))
            {
                throw new ArgumentNullException(nameof(build));
            }

            if (groupBy == default(IImmutableList<string>))
            {
                throw new ArgumentNullException(nameof(groupBy));
            }

            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
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
                "/api/aggregate/analysisdetail",
                false);

            if (!string.IsNullOrEmpty(source))
            {
                _url.AppendQuery("source", Client.Serialize(source));
            }
            if (!string.IsNullOrEmpty(type))
            {
                _url.AppendQuery("type", Client.Serialize(type));
            }
            if (!string.IsNullOrEmpty(build))
            {
                _url.AppendQuery("build", Client.Serialize(build));
            }
            if (!string.IsNullOrEmpty(workitem))
            {
                _url.AppendQuery("workitem", Client.Serialize(workitem));
            }
            if (!string.IsNullOrEmpty(analysisType))
            {
                _url.AppendQuery("analysisType", Client.Serialize(analysisType));
            }
            if (!string.IsNullOrEmpty(analysisName))
            {
                _url.AppendQuery("analysisName", Client.Serialize(analysisName));
            }
            if (groupBy != default(IImmutableList<string>))
            {
                foreach (var _item in groupBy)
                {
                    _url.AppendQuery("groupBy", Client.Serialize(_item));
                }
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
                        await OnAnalysisDetailFailed(_req, _res).ConfigureAwait(false);
                    }

                    if (_res.ContentStream == null)
                    {
                        await OnAnalysisDetailFailed(_req, _res).ConfigureAwait(false);
                    }

                    using (var _reader = new StreamReader(_res.ContentStream))
                    {
                        var _content = await _reader.ReadToEndAsync().ConfigureAwait(false);
                        var _body = Client.Deserialize<IImmutableList<Models.AggregateAnalysisDetail>>(_content);
                        return _body;
                    }
                }
            }
        }

        internal async Task OnAnalysisDetailFailed(Request req, Response res)
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
            HandleFailedAnalysisDetailRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }

        partial void HandleFailedPropertiesRequest(RestApiException ex);

        public async Task<IImmutableDictionary<string, Newtonsoft.Json.Linq.JToken>> PropertiesAsync(
            string build = default,
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
                "/api/aggregate/properties",
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
            _url.AppendQuery("api-version", Client.Serialize(apiVersion));


            using (var _req = Client.Pipeline.CreateRequest())
            {
                _req.Uri = _url;
                _req.Method = RequestMethod.Get;

                using (var _res = await Client.SendAsync(_req, cancellationToken).ConfigureAwait(false))
                {
                    if (_res.Status < 200 || _res.Status >= 300)
                    {
                        await OnPropertiesFailed(_req, _res).ConfigureAwait(false);
                    }

                    if (_res.ContentStream == null)
                    {
                        await OnPropertiesFailed(_req, _res).ConfigureAwait(false);
                    }

                    using (var _reader = new StreamReader(_res.ContentStream))
                    {
                        var _content = await _reader.ReadToEndAsync().ConfigureAwait(false);
                        var _body = Client.Deserialize<IImmutableDictionary<string, Newtonsoft.Json.Linq.JToken>>(_content);
                        return _body;
                    }
                }
            }
        }

        internal async Task OnPropertiesFailed(Request req, Response res)
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
            HandleFailedPropertiesRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }

        partial void HandleFailedInvestigation_ContinueRequest(RestApiException ex);

        public async Task<Models.InvestigationResult> Investigation_ContinueAsync(
            string id,
            CancellationToken cancellationToken = default
        )
        {

            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/aggregate/investigation/continue/{id}".Replace("{id}", Uri.EscapeDataString(Client.Serialize(id))),
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
                        await OnInvestigation_ContinueFailed(_req, _res).ConfigureAwait(false);
                    }

                    if (_res.ContentStream == null)
                    {
                        await OnInvestigation_ContinueFailed(_req, _res).ConfigureAwait(false);
                    }

                    using (var _reader = new StreamReader(_res.ContentStream))
                    {
                        var _content = await _reader.ReadToEndAsync().ConfigureAwait(false);
                        var _body = Client.Deserialize<Models.InvestigationResult>(_content);
                        return _body;
                    }
                }
            }
        }

        internal async Task OnInvestigation_ContinueFailed(Request req, Response res)
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
            HandleFailedInvestigation_ContinueRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }

        partial void HandleFailedInvestigationRequest(RestApiException ex);

        public async Task<Models.InvestigationResult> InvestigationAsync(
            IImmutableList<string> groupBy,
            int maxGroups,
            int maxResults,
            string build = default,
            string creator = default,
            string name = default,
            string source = default,
            string type = default,
            CancellationToken cancellationToken = default
        )
        {

            if (groupBy == default(IImmutableList<string>))
            {
                throw new ArgumentNullException(nameof(groupBy));
            }

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/aggregate/investigation",
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
            if (groupBy != default(IImmutableList<string>))
            {
                foreach (var _item in groupBy)
                {
                    _url.AppendQuery("groupBy", Client.Serialize(_item));
                }
            }
            if (maxGroups != default(int))
            {
                _url.AppendQuery("maxGroups", Client.Serialize(maxGroups));
            }
            if (maxResults != default(int))
            {
                _url.AppendQuery("maxResults", Client.Serialize(maxResults));
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
                        await OnInvestigationFailed(_req, _res).ConfigureAwait(false);
                    }

                    if (_res.ContentStream == null)
                    {
                        await OnInvestigationFailed(_req, _res).ConfigureAwait(false);
                    }

                    using (var _reader = new StreamReader(_res.ContentStream))
                    {
                        var _content = await _reader.ReadToEndAsync().ConfigureAwait(false);
                        var _body = Client.Deserialize<Models.InvestigationResult>(_content);
                        return _body;
                    }
                }
            }
        }

        internal async Task OnInvestigationFailed(Request req, Response res)
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
            HandleFailedInvestigationRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }

        partial void HandleFailedHistoryRequest(RestApiException ex);

        public async Task<IImmutableList<Models.HistoricalAnalysisItem>> HistoryAsync(
            string analysisName,
            string analysisType,
            int days,
            string source,
            string type,
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

            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
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
                "/api/aggregate/history/{analysisType}".Replace("{analysisType}", Uri.EscapeDataString(Client.Serialize(analysisType))),
                false);

            if (!string.IsNullOrEmpty(source))
            {
                _url.AppendQuery("source", Client.Serialize(source));
            }
            if (!string.IsNullOrEmpty(type))
            {
                _url.AppendQuery("type", Client.Serialize(type));
            }
            if (!string.IsNullOrEmpty(workitem))
            {
                _url.AppendQuery("workitem", Client.Serialize(workitem));
            }
            if (!string.IsNullOrEmpty(analysisName))
            {
                _url.AppendQuery("analysisName", Client.Serialize(analysisName));
            }
            if (days != default(int))
            {
                _url.AppendQuery("days", Client.Serialize(days));
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
                        await OnHistoryFailed(_req, _res).ConfigureAwait(false);
                    }

                    if (_res.ContentStream == null)
                    {
                        await OnHistoryFailed(_req, _res).ConfigureAwait(false);
                    }

                    using (var _reader = new StreamReader(_res.ContentStream))
                    {
                        var _content = await _reader.ReadToEndAsync().ConfigureAwait(false);
                        var _body = Client.Deserialize<IImmutableList<Models.HistoricalAnalysisItem>>(_content);
                        return _body;
                    }
                }
            }
        }

        internal async Task OnHistoryFailed(Request req, Response res)
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
            HandleFailedHistoryRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }

        partial void HandleFailedMultiSourceRequest(RestApiException ex);

        public async Task<Models.MultiSourceResponse> MultiSourceAsync(
            Models.MultiSourceRequest body,
            CancellationToken cancellationToken = default
        )
        {

            if (body == default(Models.MultiSourceRequest))
            {
                throw new ArgumentNullException(nameof(body));
            }

            const string apiVersion = "2019-06-17";

            var _baseUri = Client.Options.BaseUri;
            var _url = new RequestUriBuilder();
            _url.Reset(_baseUri);
            _url.AppendPath(
                "/api/aggregate/multi-source",
                false);

            _url.AppendQuery("api-version", Client.Serialize(apiVersion));


            using (var _req = Client.Pipeline.CreateRequest())
            {
                _req.Uri = _url;
                _req.Method = RequestMethod.Post;

                if (body != default(Models.MultiSourceRequest))
                {
                    _req.Content = RequestContent.Create(Encoding.UTF8.GetBytes(Client.Serialize(body)));
                    _req.Headers.Add("Content-Type", "application/json; charset=utf-8");
                }

                using (var _res = await Client.SendAsync(_req, cancellationToken).ConfigureAwait(false))
                {
                    if (_res.Status < 200 || _res.Status >= 300)
                    {
                        await OnMultiSourceFailed(_req, _res).ConfigureAwait(false);
                    }

                    if (_res.ContentStream == null)
                    {
                        await OnMultiSourceFailed(_req, _res).ConfigureAwait(false);
                    }

                    using (var _reader = new StreamReader(_res.ContentStream))
                    {
                        var _content = await _reader.ReadToEndAsync().ConfigureAwait(false);
                        var _body = Client.Deserialize<Models.MultiSourceResponse>(_content);
                        return _body;
                    }
                }
            }
        }

        internal async Task OnMultiSourceFailed(Request req, Response res)
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
            HandleFailedMultiSourceRequest(ex);
            HandleFailedRequest(ex);
            Client.OnFailedRequest(ex);
            throw ex;
        }
    }
}
