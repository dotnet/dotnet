// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Arcade.Common;
using Microsoft.DotNet.Helix.AzureDevOpsTestPublisher;
using Microsoft.DotNet.Helix.AzureDevOpsTestPublisher.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.DotNet.Helix.JobMonitor
{
    internal sealed class AzureDevOpsService : IAzureDevOpsService, IDisposable
    {
        private const int ControlRequestAttemptCount = 5;
        private const int ResultRequestAttemptCount = 10;
        private static readonly TimeSpan s_maximumRetryDelay = TimeSpan.FromSeconds(30);
        private static readonly System.Text.Json.JsonSerializerOptions s_serializerOptions =
            new(System.Text.Json.JsonSerializerDefaults.Web);

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

        // Name of the JSON attachment uploaded to each completed test run that lists the
        // Helix work items whose tests failed during that run. The payload schema is:
        //   { "failedWorkItems": ["wi-1", "wi-2", ...] }
        // The Helix job name itself is recovered from the run's helix-job tag (see
        // EncodeHelixJobTag / GetHelixJobNameFromRunTagsAsync); the attachment exists solely
        // to replace the previous paginated scan of /test/runs/{id}/results?outcomes=Failed
        // with a single fixed-cost call per run.
        private const string FailedWorkItemsAttachmentFileName = "helix-failed-workitems.json";

        private readonly JobMonitorOptions _options;
        private readonly ILogger _logger;
        private readonly HttpClient _azdoClient;
        private readonly AzureDevOpsRateLimitGate _rateLimitGate;
        private readonly JobMonitorMetrics _metrics;

        public AzureDevOpsService(
            JobMonitorOptions options,
            ILogger logger,
            JobMonitorMetrics metrics = null)
        {
            _options = options;
            _logger = logger;
            _metrics = metrics ?? new JobMonitorMetrics();
            _rateLimitGate = new AzureDevOpsRateLimitGate(_metrics);
            _azdoClient = new HttpClient();
            InitializeClient();
        }

        internal AzureDevOpsService(
            JobMonitorOptions options,
            ILogger logger,
            HttpClient azdoClient,
            JobMonitorMetrics metrics = null)
        {
            _options = options;
            _logger = logger;
            _metrics = metrics ?? new JobMonitorMetrics();
            _rateLimitGate = new AzureDevOpsRateLimitGate(_metrics);
            _azdoClient = azdoClient ?? throw new ArgumentNullException(nameof(azdoClient));
            InitializeClient();
        }

        private void InitializeClient()
        {
            string encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes("unused:" + _options.SystemAccessToken));
            _azdoClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedToken);
            _azdoClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _azdoClient.DefaultRequestHeaders.UserAgent.ParseAdd("dotnet-helix-job-monitor");
            _azdoClient.Timeout = TimeSpan.FromMinutes(5);
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

        public async Task<IReadOnlyDictionary<string, IReadOnlySet<string>>> GetFailedTestWorkItemsAsync(CancellationToken cancellationToken)
        {
            string buildUri = Uri.EscapeDataString($"vstfs:///Build/Build/{_options.BuildId}");
            JObject data = await SendAsync(HttpMethod.Get, $"{_options.CollectionUri}{_options.TeamProject}/_apis/test/runs?buildUri={buildUri}&$top=1000&api-version=7.1", cancellationToken: cancellationToken);

            var failedByJob = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);

            foreach (JObject run in (data?["value"] as JArray ?? []).Cast<JObject>())
            {
                if (!string.Equals(run.Value<string>("state"), "Completed", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                int? runId = run.Value<int?>("id");
                if (runId is null)
                {
                    continue;
                }

                // The Helix job name for the run comes from the per-run helix-job tag (the
                // same encoding used by GetProcessedHelixJobNamesAsync). Tags are not returned
                // inline by the /test/runs endpoint, so a single vstmr tags call per completed
                // run is required to map the run to its Helix job.
                string helixJobName = await GetHelixJobNameFromRunTagsAsync(runId.Value, cancellationToken);
                if (string.IsNullOrEmpty(helixJobName))
                {
                    continue;
                }

                // The list of work items whose tests failed is recovered from a well-known
                // JSON attachment uploaded alongside the run's completion. A single small
                // attachment-list + attachment-download replaces the previous paginated scan
                // of /test/runs/{id}/results?outcomes=Failed.
                FailedWorkItemsAttachment payload = await TryReadFailedWorkItemsAttachmentAsync(runId.Value, cancellationToken);
                if (payload is null)
                {
                    continue;
                }

                if (!failedByJob.TryGetValue(helixJobName, out HashSet<string> workItems))
                {
                    workItems = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    failedByJob[helixJobName] = workItems;
                }

                foreach (string workItemName in payload.FailedWorkItems ?? [])
                {
                    if (!string.IsNullOrEmpty(workItemName))
                    {
                        workItems.Add(workItemName);
                    }
                }
            }

            return failedByJob.ToDictionary(
                kvp => kvp.Key,
                kvp => (IReadOnlySet<string>)kvp.Value,
                StringComparer.OrdinalIgnoreCase);
        }

        // Schema of the JSON attachment uploaded to each completed test run that records
        // the names of work items whose tests failed. Designed to be forward-compatible:
        // unknown fields are ignored, and an absent failedWorkItems array is treated as
        // "no failures recorded". The Helix job name is intentionally NOT included here —
        // it is recovered from the run's helix-job tag.
        private sealed class FailedWorkItemsAttachment
        {
            [JsonProperty("failedWorkItems")]
            public string[] FailedWorkItems { get; set; }
        }

        // Looks up the Helix job tag attached to a single completed test run via the vstmr
        // Get-Run endpoint with includeTags=true. The dedicated /testresults/runs/{id}/tags
        // sub-resource only supports add/remove (PATCH/DELETE) and returns 405 for GET, so
        // we read tags inline off the TestRun payload instead. Returns null when the run
        // carries no Helix job tag (e.g. runs created by other tools or runs that never
        // reached completion).
        private async Task<string> GetHelixJobNameFromRunTagsAsync(int runId, CancellationToken cancellationToken)
        {
            string uri = $"{GetVstmrCollectionUri()}{_options.TeamProject}/_apis/testresults/runs/{runId}?includeTags=true&api-version=7.1-preview.1";
            JObject data = await SendAsync(HttpMethod.Get, uri, cancellationToken: cancellationToken);
            foreach (JObject tag in (data?["tags"] as JArray ?? []).OfType<JObject>())
            {
                string helixJobName = DecodeHelixJobTag(tag.Value<string>("name"));
                if (!string.IsNullOrEmpty(helixJobName))
                {
                    return helixJobName;
                }
            }

            return null;
        }

        // Reads the failed-work-items JSON attachment from a completed test run. Returns null
        // when the run carries no such attachment (for example a run created by another tool
        // in the same build, or a monitor run whose upload never finished). Issues at most two
        // HTTP calls per run regardless of how many failures it contains.
        private async Task<FailedWorkItemsAttachment> TryReadFailedWorkItemsAttachmentAsync(int testRunId, CancellationToken cancellationToken)        {
            JObject listing = await SendAsync(
                HttpMethod.Get,
                $"{_options.CollectionUri}{_options.TeamProject}/_apis/test/Runs/{testRunId}/attachments?api-version=7.1",
                cancellationToken: cancellationToken);

            int? attachmentId = null;
            foreach (JObject attachment in (listing?["value"] as JArray ?? []).OfType<JObject>())
            {
                if (string.Equals(
                    attachment.Value<string>("fileName"),
                    FailedWorkItemsAttachmentFileName,
                    StringComparison.OrdinalIgnoreCase))
                {
                    attachmentId = attachment.Value<int?>("id");
                    if (attachmentId.HasValue)
                    {
                        break;
                    }
                }
            }

            if (attachmentId is null)
            {
                return null;
            }

            string content = await SendForStringAsync(
                HttpMethod.Get,
                $"{_options.CollectionUri}{_options.TeamProject}/_apis/test/Runs/{testRunId}/attachments/{attachmentId.Value}?api-version=7.1",
                cancellationToken: cancellationToken);

            if (string.IsNullOrWhiteSpace(content))
            {
                return null;
            }

            try
            {
                return JsonConvert.DeserializeObject<FailedWorkItemsAttachment>(content);
            }
            catch (JsonException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Failed to parse '{FileName}' attachment on Azure DevOps test run {TestRunId}; ignoring.",
                    FailedWorkItemsAttachmentFileName,
                    testRunId);
                return null;
            }
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
                retryTransientFailures: false,
                cancellationToken: cancellationToken);
            return result?["id"]?.ToObject<int>() ?? 0;
        }

        public async Task CompleteTestRunAsync(
            int testRunId,
            string helixJobName,
            IReadOnlyCollection<string> failedWorkItems,
            CancellationToken cancellationToken)
        {
            // Upload the failed-work-items attachment BEFORE marking the run Completed (and
            // before applying the helix-job tag). The tag is the canonical "this run is fully
            // processed" marker — if we crashed between the PATCH and the attachment upload,
            // a later monitor invocation would treat the job as done but have no list of
            // failed work items, silently dropping resubmissions. Uploading the attachment
            // first preserves the existing crash-resilience invariant: a crash leaves the run
            // un-tagged, so the next invocation re-uploads everything in full.
            if (failedWorkItems is { Count: > 0 })
            {
                await UploadFailedWorkItemsAttachmentAsync(testRunId, failedWorkItems, cancellationToken);
            }

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
                retryTransientFailures: true,
                cancellationToken: cancellationToken);
        }

        // Uploads the failed-work-items JSON attachment to a test run. The payload is the
        // canonical mechanism by which a later monitor invocation rediscovers the set of work
        // items whose tests failed and therefore need resubmission, replacing the previous
        // approach of paginating /test/runs/{id}/results?outcomes=Failed and parsing the
        // per-result comment JSON. See GetFailedTestWorkItemsAsync for the read side.
        private async Task UploadFailedWorkItemsAttachmentAsync(
            int testRunId,
            IReadOnlyCollection<string> failedWorkItems,
            CancellationToken cancellationToken)
        {
            var payload = new JObject
            {
                ["failedWorkItems"] = new JArray(failedWorkItems.Where(w => !string.IsNullOrEmpty(w)).Cast<object>().ToArray()),
            };

            byte[] bytes = Encoding.UTF8.GetBytes(payload.ToString(Formatting.None));
            var body = new JObject
            {
                ["stream"] = Convert.ToBase64String(bytes),
                ["fileName"] = FailedWorkItemsAttachmentFileName,
                ["comment"] = "Helix work items whose tests failed during this run; consumed by the Helix job monitor retry pass.",
                ["attachmentType"] = "GeneralAttachment",
            };

            await SendAsync(
                HttpMethod.Post,
                $"{_options.CollectionUri}{_options.TeamProject}/_apis/test/Runs/{testRunId}/attachments?api-version=7.1",
                body,
                retryTransientFailures: false,
                cancellationToken: cancellationToken);
        }

        public async Task<TestResultUploadSummary> UploadTestResultsAsync(
            int testRunId,
            WorkItemTestResults results,
            CancellationToken cancellationToken)
        {
            if (results.TestResultFiles.Count == 0)
            {
                return new TestResultUploadSummary(true, 0);
            }

            var publisher = new AzureDevOpsResultPublisher(
                _options.TestResultAttachmentMode,
                _options.UseFullyQualifiedTestName,
                _logger,
                CreateResultTransport(testRunId),
                _metrics);

            return await publisher.UploadTestResultsWithSummaryAsync(
                results.TestResultFiles,
                new
                {
                    HelixJobId = results.JobName,
                    HelixWorkItemName = results.WorkItemName
                },
                cancellationToken);
        }

        internal IAzureDevOpsResultTransport CreateResultTransport(int testRunId)
            => new AzureDevOpsResultTransport(this, testRunId);

        private async Task<JObject> SendAsync(
            HttpMethod method,
            string requestUri,
            JToken body = null,
            bool retryTransientFailures = true,
            CancellationToken cancellationToken = default)
        {
            string content = await SendForStringAsync(method, requestUri, body, retryTransientFailures, cancellationToken);
            return string.IsNullOrWhiteSpace(content) ? [] : JObject.Parse(content);
        }

        private Task<string> SendForStringAsync(
            HttpMethod method,
            string requestUri,
            JToken body = null,
            bool retryTransientFailures = true,
            CancellationToken cancellationToken = default)
            => SendForStringAsync(
                method,
                requestUri,
                body?.ToString(Formatting.None),
                AzureDevOpsRequestKind.Control,
                retryTransientFailures,
                ControlRequestAttemptCount,
                cancellationToken);

        // All Azure DevOps calls flow through this method so request validation, throttling,
        // retries, and metrics remain consistent across control, result, and attachment traffic.
        private async Task<string> SendForStringAsync(
            HttpMethod method,
            string requestUri,
            string serializedBody,
            AzureDevOpsRequestKind requestKind,
            bool retryTransientFailures,
            int attemptCount,
            CancellationToken cancellationToken)
        {
            int payloadBytes = serializedBody is null ? 0 : Encoding.UTF8.GetByteCount(serializedBody);

            async Task<string> SendOnceAsync(int attempt)
            {
                await _rateLimitGate.WaitAsync(cancellationToken);
                long requestStartedAt = JobMonitorMetrics.StartOperation();
                bool failed = true;
                using var request = new HttpRequestMessage(method, requestUri);
                if (serializedBody != null)
                {
                    request.Content = new StringContent(serializedBody, Encoding.UTF8, "application/json");
                }

                try
                {
                    using HttpResponseMessage response = await _azdoClient.SendAsync(request, cancellationToken);
                    string content = response.Content != null ? await response.Content.ReadAsStringAsync(cancellationToken) : null;
                    TimeSpan? rateLimitDelay = GetRateLimitDelay(response);
                    if (response.StatusCode == HttpStatusCode.TooManyRequests && rateLimitDelay is null)
                    {
                        rateLimitDelay = TimeSpan.FromSeconds(30);
                    }

                    if (rateLimitDelay is { } delay)
                    {
                        // The current request has completed. Extend only the shared deadline for
                        // requests that have not yet started.
                        _rateLimitGate.ExtendDeadline(delay);
                    }

                    if (!response.IsSuccessStatusCode)
                    {
                        ThrowForFailure(response, content, requestUri, requestKind, rateLimitDelay);
                    }

                    failed = false;
                    return content;
                }
                finally
                {
                    _metrics.RecordAzureDevOpsRequest(
                        requestKind,
                        payloadBytes,
                        isRetry: attempt > 0,
                        failed: failed,
                        startedAt: requestStartedAt);
                }
            }

            if (!retryTransientFailures)
            {
                return await SendOnceAsync(0);
            }

            string result = null;
            Exception lastException = null;
            var retryHandler = new ExponentialRetry
            {
                MaxAttempts = attemptCount,
                DelayBase = requestKind == AzureDevOpsRequestKind.Control ? 2 : 3,
                DelayConstant = 0,
                MinRandomFactor = 1,
                MaxRandomFactor = 1,
                MaximumDelay = s_maximumRetryDelay,
                RetryDelayCallback = (failedAttempt, delay) =>
                    _logger.LogDebug(
                        "Azure DevOps {Method} request to '{RequestUri}' failed on attempt {Attempt} of {AttemptCount}. "
                        + "Waiting {RetryDelay} before the next attempt.",
                        method,
                        requestUri,
                        failedAttempt,
                        attemptCount,
                        delay),
            };

            bool succeeded = await retryHandler.RunAsync(
                async attempt =>
                {
                    try
                    {
                        result = await SendOnceAsync(attempt);
                        return RetryResult.Success;
                    }
                    catch (Exception ex) when (IsTransientException(ex, cancellationToken))
                    {
                        lastException = ex;
                        return RetryResult.Retry((ex as TransientAzureDevOpsRequestException)?.RetryAfter);
                    }
                },
                cancellationToken);

            return succeeded
                ? result
                : throw lastException ?? new InvalidOperationException("Retry failed without completing the Azure DevOps request.");
        }

        internal static bool IsTransientException(Exception exception, CancellationToken cancellationToken)
            => !cancellationToken.IsCancellationRequested
                && exception is OperationCanceledException { InnerException: TimeoutException }
                    or HttpRequestException
                or TimeoutException
                or SocketException
                or IOException;

        internal static TimeSpan? GetRateLimitDelay(HttpResponseMessage response)
        {
            TimeSpan delay = TimeSpan.Zero;
            RetryConditionHeaderValue retryAfterHeader = response.Headers.RetryAfter;
            if (retryAfterHeader?.Delta is { } delta && delta > delay)
            {
                delay = delta;
            }

            if (retryAfterHeader?.Date is { } date)
            {
                TimeSpan datedDelay = TimeSpan.FromTicks(date.UtcTicks - DateTimeOffset.UtcNow.UtcTicks);
                if (datedDelay > delay)
                {
                    delay = datedDelay;
                }
            }

            if (response.Headers.TryGetValues("X-RateLimit-Delay", out IEnumerable<string> delayValues) &&
                double.TryParse(delayValues.FirstOrDefault(), NumberStyles.Float, CultureInfo.InvariantCulture, out double delaySeconds) &&
                delaySeconds > 0)
            {
                TimeSpan rateLimitDelay = TimeSpan.FromSeconds(delaySeconds);
                if (rateLimitDelay > delay)
                {
                    delay = rateLimitDelay;
                }
            }

            return delay > TimeSpan.Zero ? delay : null;
        }

        private static void ThrowForFailure(
            HttpResponseMessage response,
            string responseBody,
            string requestUri,
            AzureDevOpsRequestKind requestKind,
            TimeSpan? rateLimitDelay)
        {
            responseBody ??= string.Empty;
            if (responseBody.Contains("It may have been deleted", StringComparison.OrdinalIgnoreCase)
                || responseBody.Contains("not authorized to access this resource", StringComparison.OrdinalIgnoreCase)
                || responseBody.Contains("cannot be added or updated for a test run which is in Completed state", StringComparison.OrdinalIgnoreCase)
                || response.StatusCode == HttpStatusCode.Forbidden
                || response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new TerminalError(responseBody);
            }

            string message = $"Request to {requestUri} failed with {(int)response.StatusCode} {response.ReasonPhrase}. {responseBody}";
            if ((int)response.StatusCode >= 500 || response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                throw new TransientAzureDevOpsRequestException(
                    message,
                    response.StatusCode,
                    rateLimitDelay ?? (response.StatusCode == HttpStatusCode.TooManyRequests ? TimeSpan.FromSeconds(30) : null));
            }

            if (requestKind == AzureDevOpsRequestKind.Control)
            {
                throw new HttpRequestException(message, null, response.StatusCode);
            }

            throw new AzureDevOpsReportingError(message);
        }

        private sealed class AzureDevOpsResultTransport(
            AzureDevOpsService service,
            int testRunId) : IAzureDevOpsResultTransport
        {
            public Task<string> PublishResultsAsync(object results, CancellationToken cancellationToken)
                => service.SendForStringAsync(
                    HttpMethod.Post,
                    $"{service._options.CollectionUri}{service._options.TeamProject}/_apis/test/runs/{testRunId}/results?api-version=7.1-preview.6",
                    System.Text.Json.JsonSerializer.Serialize(results, s_serializerOptions),
                    AzureDevOpsRequestKind.ResultBatch,
                    retryTransientFailures: true,
                    ResultRequestAttemptCount,
                    cancellationToken);

            public Task UploadAttachmentAsync(
                long testResultId,
                long? testSubResultId,
                string fileName,
                string stream,
                CancellationToken cancellationToken)
            {
                string query = testSubResultId is long subResultId
                    ? $"?testSubResultId={subResultId}&api-version=7.1-preview.1"
                    : "?api-version=7.1-preview.1";
                var body = new JObject
                {
                    ["fileName"] = fileName,
                    ["stream"] = stream,
                };

                return service.SendForStringAsync(
                    HttpMethod.Post,
                    $"{service._options.CollectionUri}{service._options.TeamProject}/_apis/test/runs/{testRunId}/results/{testResultId}/attachments{query}",
                    body.ToString(Formatting.None),
                    AzureDevOpsRequestKind.Attachment,
                    retryTransientFailures: true,
                    ResultRequestAttemptCount,
                    cancellationToken);
            }
        }

        private sealed class TransientAzureDevOpsRequestException(
            string message,
            HttpStatusCode statusCode,
            TimeSpan? retryAfter)
            : HttpRequestException(message, null, statusCode)
        {
            public TimeSpan? RetryAfter { get; } = retryAfter;
        }

        public void Dispose()
        {
            _azdoClient.Dispose();
        }
    }
}
