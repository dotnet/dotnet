// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.DotNet.Helix.AzureDevOpsTestPublisher;
using Microsoft.DotNet.Helix.AzureDevOpsTestPublisher.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.DotNet.Helix.JobMonitor
{
    internal sealed class AzureDevOpsService : IAzureDevOpsService, IDisposable
    {
        // Marker appended to every test run name so we can recover the Helix job name on a
        // subsequent monitor attempt. Format: "{originalName} [HelixJob:{helixJobName}]".
        // The marker is intentionally human-readable to aid debugging in the AzDO UI.
        //
        // We use the run name (instead of "tags" or "customFields") because empirically that is
        // the only run property that both (a) is persisted by the Azure DevOps test runs API on
        // POST /test/runs and (b) is returned in the list response from
        // GET /test/runs?buildUri=... — so cross-attempt dedup needs no per-run round-trips.
        // Tags on test runs are silently dropped by the service; the dedicated tags endpoint
        // does not exist; and customFields is reserved for system values.
        private const string HelixJobNameMarkerStart = "[HelixJob:";
        private const string HelixJobNameMarkerEnd = "]";

        private readonly JobMonitorOptions _options;
        private readonly ILogger _logger;
        private readonly HttpClient _azdoClient;
        private readonly SemaphoreSlim _uploadSemaphore;

        public AzureDevOpsService(JobMonitorOptions options, ILogger logger)
        {
            _options = options;
            _logger = logger;
            _azdoClient = new HttpClient();
            _uploadSemaphore = new SemaphoreSlim(options.TestResultUploadParallelism);
            InitializeClient();
        }

        internal AzureDevOpsService(JobMonitorOptions options, ILogger logger, HttpClient azdoClient)
        {
            _options = options;
            _logger = logger;
            _azdoClient = azdoClient ?? throw new ArgumentNullException(nameof(azdoClient));
            _uploadSemaphore = new SemaphoreSlim(options.TestResultUploadParallelism);
            InitializeClient();
        }

        private void InitializeClient()
        {
            string encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes("unused:" + _options.SystemAccessToken));
            _azdoClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedToken);
            _azdoClient.DefaultRequestHeaders.UserAgent.ParseAdd("dotnet-helix-job-monitor");
        }

        public async Task<IReadOnlyList<AzureDevOpsTimelineRecord>> GetTimelineRecordsAsync(CancellationToken cancellationToken)
        {
            JObject data = await SendAsync(HttpMethod.Get, $"{_options.CollectionUri}{_options.TeamProject}/_apis/build/builds/{_options.BuildId}/timeline?api-version=7.1-preview.2", cancellationToken: cancellationToken);
            return data?["records"]?.ToObject<AzureDevOpsTimelineRecord[]>() ?? [];
        }

        public async Task<IReadOnlySet<string>> GetProcessedHelixJobNamesAsync(CancellationToken cancellationToken)
        {
            string buildUri = Uri.EscapeDataString($"vstfs:///Build/Build/{_options.BuildId}");
            JObject data = await SendAsync(HttpMethod.Get, $"{_options.CollectionUri}{_options.TeamProject}/_apis/test/runs?buildUri={buildUri}&$top=1000&api-version=7.1", cancellationToken: cancellationToken);
            var processed = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (JObject run in (data?["value"] as JArray ?? []).Cast<JObject>())
            {
                if (!string.Equals(run.Value<string>("state"), "Completed", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                string helixJobName = ExtractHelixJobNameFromRunName(run.Value<string>("name"));
                if (!string.IsNullOrEmpty(helixJobName))
                {
                    processed.Add(helixJobName);
                }
            }

            return processed;
        }

        internal static string EncodeRunName(string name, string helixJobName)
        {
            if (string.IsNullOrEmpty(helixJobName))
            {
                return name;
            }

            return $"{name} {HelixJobNameMarkerStart}{helixJobName}{HelixJobNameMarkerEnd}";
        }

        internal static string ExtractHelixJobNameFromRunName(string runName)
        {
            if (string.IsNullOrEmpty(runName) || !runName.EndsWith(HelixJobNameMarkerEnd, StringComparison.Ordinal))
            {
                return null;
            }

            int markerStart = runName.LastIndexOf(HelixJobNameMarkerStart, StringComparison.Ordinal);
            if (markerStart < 0)
            {
                return null;
            }

            int valueStart = markerStart + HelixJobNameMarkerStart.Length;
            int valueLength = runName.Length - valueStart - HelixJobNameMarkerEnd.Length;
            return valueLength > 0 ? runName.Substring(valueStart, valueLength) : null;
        }

        public async Task<int> CreateTestRunAsync(string name, string helixJobName, CancellationToken cancellationToken)
        {
            JObject result = await SendAsync(HttpMethod.Post,
                $"{_options.CollectionUri}{_options.TeamProject}/_apis/test/runs?api-version=7.1",
                new JObject
                {
                    ["automated"] = true,
                    ["build"] = new JObject { ["id"] = _options.BuildId },
                    ["name"] = EncodeRunName(name, helixJobName),
                    ["state"] = "InProgress",
                },
                cancellationToken: cancellationToken);
            return result?["id"]?.ToObject<int>() ?? 0;
        }

        public async Task CompleteTestRunAsync(int testRunId, CancellationToken cancellationToken)
        {
            await SendAsync(new HttpMethod("PATCH"),
                $"{_options.CollectionUri}{_options.TeamProject}/_apis/test/runs/{testRunId}?api-version=7.1",
                new JObject { ["state"] = "Completed" },
                cancellationToken: cancellationToken);
        }

        public async Task<int> UploadTestResultsAsync(int testRunId, IReadOnlyList<WorkItemTestResults> results, CancellationToken cancellationToken)
        {
            int uploadedCount = 0;
            using var publisher = new AzureDevOpsResultPublisher(
                new AzureDevOpsReportingParameters(
                    new Uri(_options.CollectionUri, UriKind.Absolute),
                    _options.TeamProject,
                    testRunId.ToString(CultureInfo.InvariantCulture),
                    _options.SystemAccessToken),
                _logger);

            async Task UploadWorkItemAsync(WorkItemTestResults workItem)
            {
                if (workItem.TestResultFiles.Count == 0)
                {
                    _logger.LogInformation("No test results to upload for work item {WorkItemId} in job {JobName}", workItem.WorkItemName, workItem.JobName);
                    return;
                }

                await _uploadSemaphore.WaitAsync(cancellationToken);

                try
                {
                    _logger.LogDebug("Publishing test results for work item '{WorkItemName}' in job '{JobName}'...", workItem.WorkItemName, workItem.JobName);
                    TestResultUploadSummary summary = await publisher.UploadTestResultsWithSummaryAsync(
                        workItem.TestResultFiles,
                        new
                        {
                            HelixJobId = workItem.JobName,
                            HelixWorkItemName = workItem.WorkItemName
                        },
                        cancellationToken);
                    Interlocked.Add(ref uploadedCount, checked((int)summary.UploadedCount));
                }
                finally
                {
                    _uploadSemaphore.Release();
                }
            }

            await Task.WhenAll(results.Select(UploadWorkItemAsync));
            return uploadedCount;
        }

        private async Task<JObject> SendAsync(HttpMethod method, string requestUri, JToken body = null, CancellationToken cancellationToken = default)
        {
            return await RetryHelper.RetryAsync(async () =>
            {
                using var request = new HttpRequestMessage(method, requestUri);
                if (body != null)
                {
                    request.Content = new StringContent(body.ToString(Formatting.None), Encoding.UTF8, "application/json");
                }

                using HttpResponseMessage response = await _azdoClient.SendAsync(request, cancellationToken);
                string content = response.Content != null ? await response.Content.ReadAsStringAsync(cancellationToken) : null;
                if (!response.IsSuccessStatusCode)
                {
                    await HonorRateLimitAsync(response, requestUri, cancellationToken);
                    throw new HttpRequestException($"Request to {requestUri} failed with {(int)response.StatusCode} {response.ReasonPhrase}. {content}");
                }

                await HonorRateLimitAsync(response, requestUri, cancellationToken);
                return string.IsNullOrWhiteSpace(content) ? [] : JObject.Parse(content);
            }, cancellationToken);
        }

        // Honors Azure DevOps rate limiting guidance:
        // https://learn.microsoft.com/azure/devops/integrate/concepts/rate-limits#api-client-experience
        // If the response carries a Retry-After header (RFC 6585) we wait the specified amount of
        // time before allowing the next request to be issued. We also log when the service reports
        // a non-zero X-RateLimit-Delay so callers have visibility into throttling behavior.
        private async Task HonorRateLimitAsync(HttpResponseMessage response, string requestUri, CancellationToken cancellationToken)
        {
            TimeSpan? retryAfter = null;
            RetryConditionHeaderValue retryAfterHeader = response.Headers.RetryAfter;
            if (retryAfterHeader != null)
            {
                if (retryAfterHeader.Delta.HasValue)
                {
                    retryAfter = retryAfterHeader.Delta.Value;
                }
                else if (retryAfterHeader.Date.HasValue)
                {
                    TimeSpan delta = retryAfterHeader.Date.Value - DateTimeOffset.UtcNow;
                    if (delta > TimeSpan.Zero)
                    {
                        retryAfter = delta;
                    }
                }
            }

            TimeSpan delayToApply = TimeSpan.Zero;

            if (response.Headers.TryGetValues("X-RateLimit-Delay", out IEnumerable<string> delayValues) &&
                double.TryParse(delayValues.FirstOrDefault(), NumberStyles.Float, CultureInfo.InvariantCulture, out double delaySeconds) &&
                delaySeconds > 0)
            {
                TimeSpan rateLimitDelay = TimeSpan.FromSeconds(delaySeconds);
                delayToApply = rateLimitDelay;
                _logger.LogDebug(
                    "Azure DevOps reported X-RateLimit-Delay of {DelaySeconds:0.###}s on request to {RequestUri}.",
                    delaySeconds,
                    requestUri);
            }

            if (retryAfter.HasValue && retryAfter.Value > TimeSpan.Zero)
            {
                delayToApply = delayToApply > retryAfter.Value ? delayToApply : retryAfter.Value;
            }

            if (delayToApply > TimeSpan.Zero)
            {
                _logger.LogDebug(
                    "Azure DevOps rate limit back-off. Delaying next request by {DelaySeconds:0.###}s (request: {RequestUri}).",
                    delayToApply.TotalSeconds,
                    requestUri);
                await Task.Delay(delayToApply, cancellationToken);
            }
        }

        public void Dispose()
        {
            _azdoClient.Dispose();
            _uploadSemaphore.Dispose();
        }
    }
}
