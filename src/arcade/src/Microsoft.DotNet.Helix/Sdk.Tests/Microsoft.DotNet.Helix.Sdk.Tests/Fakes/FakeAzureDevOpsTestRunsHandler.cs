// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Microsoft.DotNet.Helix.Sdk.Tests.Fakes
{
    /// <summary>
    /// Test double for the Azure DevOps test-runs REST API endpoints consumed by
    /// <see cref="JobMonitor.AzureDevOpsService"/>.
    ///
    /// <para>
    /// This fake mimics several empirically-verified production quirks of the test
    /// runs tags API:
    /// </para>
    /// <list type="bullet">
    /// <item>Tags persist only when posted as objects (<c>"tags": [{ "name": "..." }]</c>).
    /// The legacy string form (<c>"tags": ["..."]</c>) is silently dropped — this was
    /// the original bug the monitor used to work around.</item>
    /// <item>Tags are never returned inline on a run: <c>GET /_apis/test/runs</c> (by id
    /// or list) omits them entirely.</item>
    /// <item>Tags are only retrievable via the build-scoped test results tags endpoint
    /// <c>GET /_apis/testresults/tags?buildId=...</c>, which returns the union of tag
    /// names across the whole build regardless of run state.</item>
    /// </list>
    /// </summary>
    internal sealed class FakeAzureDevOpsTestRunsHandler : HttpMessageHandler
    {
        private readonly object _sync = new();
        private readonly Dictionary<int, StoredRun> _runs = [];
        private int _nextId;

        public List<HttpRequestMessage> Requests { get; } = [];
        public List<string> Bodies { get; } = [];

        public IReadOnlyDictionary<int, StoredRun> Runs
        {
            get
            {
                lock (_sync)
                {
                    return new Dictionary<int, StoredRun>(_runs);
                }
            }
        }

        /// <summary>
        /// Server-side representation of a test run. Tags are stored here but, matching real
        /// Azure DevOps, are never echoed back on the run object — only via the build tags
        /// endpoint.
        /// </summary>
        internal sealed record StoredRun(string Name, string State, IReadOnlyList<string> Tags);

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Requests.Add(request);

            string body = null;
            if (request.Content != null)
            {
                body = await request.Content.ReadAsStringAsync(cancellationToken);
                Bodies.Add(body);
            }

            string path = request.RequestUri.AbsolutePath;

            if (request.Method == HttpMethod.Get && path.EndsWith("/_apis/testresults/tags", StringComparison.OrdinalIgnoreCase))
            {
                return GetBuildTags();
            }

            if (request.Method == HttpMethod.Post && path.EndsWith("/_apis/test/runs", StringComparison.OrdinalIgnoreCase))
            {
                return CreateRun(body);
            }

            if (request.Method.Method.Equals("PATCH", StringComparison.OrdinalIgnoreCase)
                && path.Contains("/_apis/test/runs/", StringComparison.OrdinalIgnoreCase))
            {
                return UpdateRun(path, body);
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        // Only object-form tags ([{ "name": "..." }]) are persisted; string-form tags are dropped,
        // matching the historical Azure DevOps behavior.
        private static IReadOnlyList<string> ParseTags(JObject json)
        {
            if (json["tags"] is not JArray tags)
            {
                return [];
            }

            return [.. tags.OfType<JObject>()
                .Select(t => t.Value<string>("name"))
                .Where(name => !string.IsNullOrEmpty(name))];
        }

        private HttpResponseMessage CreateRun(string body)
        {
            JObject json = JObject.Parse(body);
            int id;
            lock (_sync)
            {
                id = ++_nextId;
                _runs[id] = new StoredRun(
                    Name: json.Value<string>("name"),
                    State: json.Value<string>("state"),
                    Tags: ParseTags(json));
            }

            return JsonResponse(new JObject { ["id"] = id });
        }

        private HttpResponseMessage UpdateRun(string path, string body)
        {
            int slash = path.LastIndexOf('/');
            if (slash < 0 || !int.TryParse(path[(slash + 1)..], out int id))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            JObject json = JObject.Parse(body);
            lock (_sync)
            {
                if (!_runs.TryGetValue(id, out StoredRun existing))
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }

                IReadOnlyList<string> tags = json["tags"] is JArray ? ParseTags(json) : existing.Tags;
                _runs[id] = existing with
                {
                    State = json.Value<string>("state") ?? existing.State,
                    Tags = tags,
                };
            }

            return JsonResponse(new JObject { ["id"] = id });
        }

        private HttpResponseMessage GetBuildTags()
        {
            var array = new JArray();
            lock (_sync)
            {
                foreach (string tag in _runs.Values.SelectMany(r => r.Tags).Distinct(StringComparer.Ordinal))
                {
                    array.Add(new JObject { ["name"] = tag });
                }
            }

            return JsonResponse(new JObject { ["count"] = array.Count, ["value"] = array });
        }

        private static HttpResponseMessage JsonResponse(JObject obj)
            => new(HttpStatusCode.OK) { Content = new StringContent(obj.ToString()) };
    }
}
