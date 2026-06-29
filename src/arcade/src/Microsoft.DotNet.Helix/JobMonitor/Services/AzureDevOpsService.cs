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
        // A test run tag is applied to every completed test run so we can recover the Helix job
        // name on a subsequent monitor attempt. The Helix job name (a GUID) is encoded as
        // "{HelixJobTagPrefix}{guidWithoutDashes}" because Azure DevOps only accepts alphanumeric
        // test run tags (no dashes/colons) and limits each tag to 50 characters.
        //
        // Tag mechanics (verified empirically against the Azure DevOps test runs API):
        //   * Tags persist only when posted as objects: "tags": [{ "name": "..." }]. The legacy
        //     string form ("tags": ["..."]) is silently dropped — that was the original bug.
        //   * Tags are NOT returned inline on a test run (GET /test/runs returns no tags). They
        //     are read back via the dedicated, build-scoped test results tags endpoint on the
        //     vstmr host: GET {vstmr}/{project}/_apis/testresults/tags?buildId=... which returns a
        //     flat set of tag names across the whole build.
        //   * Because that endpoint is build-scoped and has no per-run state, the tag is applied at
        //     run COMPLETION (not creation). A tag therefore exists if and only if the run reached
        //     the Completed state and its results finished uploading, preserving crash resilience:
        //     a monitor that crashes mid-upload leaves an untagged in-progress run that a
        //     subsequent attempt re-uploads.
        private const string HelixJobTagPrefix = "helixjob";

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
            var processed = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // Every completed run is tagged with the Helix job name, and the build-scoped test
            // results tags endpoint returns the union of tags across the whole build in a single
            // call.
            string tagsUri = $"{GetVstmrCollectionUri()}{_options.TeamProject}/_apis/testresults/tags?buildId={_options.BuildId}&api-version=7.1-preview.1";
            JObject tagsData = await SendAsync(HttpMethod.Get, tagsUri, cancellationToken: cancellationToken);
            foreach (JObject tag in (tagsData?["value"] as JArray ?? []).OfType<JObject>())
            {
                string helixJobName = DecodeHelixJobTag(tag.Value<string>("name"));
                if (!string.IsNullOrEmpty(helixJobName))
                {
                    processed.Add(helixJobName);
                }
            }

            return processed;
        }

        // The test results tags endpoint is only served from the "vstmr" host, so derive it from
        // the configured collection URI (e.g. https://dev.azure.com/{org}/ ->
        // https://vstmr.dev.azure.com/{org}/, https://{org}.visualstudio.com/ ->
        // https://{org}.vstmr.visualstudio.com/).
        internal static string ToVstmrCollectionUri(string collectionUri)
        {
            var uri = new Uri(collectionUri, UriKind.Absolute);
            string host = uri.Host;
            string vstmrHost;
            if (host.Equals("dev.azure.com", StringComparison.OrdinalIgnoreCase))
            {
                vstmrHost = "vstmr.dev.azure.com";
            }
            else if (host.EndsWith(".visualstudio.com", StringComparison.OrdinalIgnoreCase) && host.Contains('.'))
            {
                vstmrHost = host.Insert(host.IndexOf('.'), ".vstmr");
            }
            else
            {
                vstmrHost = host;
            }

            return new UriBuilder(uri) { Host = vstmrHost }.Uri.ToString();
        }

        private string GetVstmrCollectionUri() => ToVstmrCollectionUri(_options.CollectionUri);

        // Encodes a Helix job name (a GUID) as an Azure DevOps test run tag. Azure DevOps only
        // accepts alphanumeric tags up to 50 characters, so the GUID's dashes are removed. Returns
        // null when the job name is not a GUID (defensive; Helix job names are always GUIDs).
        internal static string EncodeHelixJobTag(string helixJobName)
        {
            return Guid.TryParse(helixJobName, out Guid id)
                ? HelixJobTagPrefix + id.ToString("N")
                : null;
        }

        // Inverse of <see cref="EncodeHelixJobTag"/>. Returns the original Helix job GUID (in the
        // canonical dashed form) or null when the tag is not a Helix job tag.
        internal static string DecodeHelixJobTag(string tag)
        {
            if (string.IsNullOrEmpty(tag) || !tag.StartsWith(HelixJobTagPrefix, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            string encoded = tag.Substring(HelixJobTagPrefix.Length);
            return Guid.TryParseExact(encoded, "N", out Guid id) ? id.ToString("D") : null;
        }

        public async Task<int> CreateTestRunAsync(string name, CancellationToken cancellationToken)
        {
            // The run name is the plain, human-readable name. The Helix job name is recorded as a
            // tag when the run is completed (see CompleteTestRunAsync), not encoded into the name.
            JObject result = await SendAsync(HttpMethod.Post,
                $"{_options.CollectionUri}{_options.TeamProject}/_apis/test/runs?api-version=7.1",
                new JObject
                {
                    ["automated"] = true,
                    ["build"] = new JObject { ["id"] = _options.BuildId },
                    ["name"] = name,
                    ["state"] = "InProgress",
                },
                cancellationToken: cancellationToken);
            return result?["id"]?.ToObject<int>() ?? 0;
        }

        public async Task CompleteTestRunAsync(int testRunId, string helixJobName, CancellationToken cancellationToken)
        {
            var body = new JObject { ["state"] = "Completed" };

            // Tag the completed run with the Helix job name so a subsequent monitor attempt can tell
            // this job's results have already been uploaded. Tags must be posted as objects to
            // persist (the string form is silently dropped by Azure DevOps).
            string tag = EncodeHelixJobTag(helixJobName);
            if (tag != null)
            {
                body["tags"] = new JArray(new JObject { ["name"] = tag });
            }
            else
            {
                _logger.LogWarning(
                    "Could not encode Helix job name '{HelixJobName}' as a test run tag; test results for this job may be re-uploaded if the monitor is retried.",
                    helixJobName);
            }

            await SendAsync(new HttpMethod("PATCH"),
                $"{_options.CollectionUri}{_options.TeamProject}/_apis/test/runs/{testRunId}?api-version=7.1",
                body,
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
