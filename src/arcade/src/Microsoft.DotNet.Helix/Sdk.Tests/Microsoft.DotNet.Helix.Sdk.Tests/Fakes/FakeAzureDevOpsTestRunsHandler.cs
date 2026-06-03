// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
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
    /// IMPORTANT: this fake intentionally mimics one production quirk that took
    /// considerable empirical work to pin down — Azure DevOps SILENTLY DROPS the
    /// <c>tags</c> property on <c>POST /_apis/test/runs</c> and
    /// <c>PATCH /_apis/test/runs/{id}</c>. The synchronous POST/PATCH response
    /// echoes the posted tags back, but a subsequent GET of the run — whether by id
    /// or via the runs list — always returns <c>tags: []</c>. This was verified
    /// across api-versions 5.1, 6.0, 7.1, 7.1-preview.3 and 7.2-preview.3; the
    /// dedicated <c>/_apis/test/Runs/{id}/tags</c> endpoint 404s on every method
    /// and version. The Helix job monitor therefore round-trips the Helix job name
    /// through the run <c>name</c> field instead. If a regression ever reintroduces
    /// tag-based dedup, tests that use this handler should catch it because the
    /// stored runs never carry tags forward.
    /// </para>
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
        /// Server-side representation of a test run. Note the deliberate absence of
        /// a Tags property: real Azure DevOps does not persist tags, so the fake
        /// must not either.
        /// </summary>
        internal sealed record StoredRun(string Name, string State);

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

            if (request.Method == HttpMethod.Post && path.EndsWith("/_apis/test/runs", StringComparison.OrdinalIgnoreCase))
            {
                return CreateRun(body);
            }

            if (request.Method == HttpMethod.Get && path.EndsWith("/_apis/test/runs", StringComparison.OrdinalIgnoreCase))
            {
                return ListRuns();
            }

            if (request.Method.Method.Equals("PATCH", StringComparison.OrdinalIgnoreCase)
                && path.Contains("/_apis/test/runs/", StringComparison.OrdinalIgnoreCase))
            {
                return UpdateRun(path, body);
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        private HttpResponseMessage CreateRun(string body)
        {
            JObject json = JObject.Parse(body);
            int id;
            lock (_sync)
            {
                id = ++_nextId;
                // Tags from the request are intentionally discarded — see the
                // class-level note for the empirical justification.
                _runs[id] = new StoredRun(
                    Name: json.Value<string>("name"),
                    State: json.Value<string>("state"));
            }

            return JsonResponse(new JObject { ["id"] = id });
        }

        private HttpResponseMessage ListRuns()
        {
            var array = new JArray();
            lock (_sync)
            {
                foreach (var (id, run) in _runs)
                {
                    array.Add(new JObject
                    {
                        ["id"] = id,
                        ["name"] = run.Name,
                        ["state"] = run.State,
                        // No "tags" field is ever emitted — the real list response
                        // does not include it.
                    });
                }
            }

            return JsonResponse(new JObject { ["value"] = array });
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

                // As with POST, tags in PATCH bodies are silently dropped server-side.
                _runs[id] = existing with { State = json.Value<string>("state") ?? existing.State };
            }

            return JsonResponse(new JObject { ["id"] = id });
        }

        private static HttpResponseMessage JsonResponse(JObject obj)
            => new(HttpStatusCode.OK) { Content = new StringContent(obj.ToString()) };
    }
}
