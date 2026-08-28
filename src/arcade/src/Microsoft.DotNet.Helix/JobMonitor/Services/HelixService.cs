// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Arcade.Common;
using Microsoft.DotNet.Helix.Client;
using Microsoft.DotNet.Helix.Client.Models;
using Microsoft.DotNet.Helix.JobMonitor.Models;
using Microsoft.DotNet.Helix.AzureDevOpsTestPublisher;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.DotNet.Helix.JobMonitor
{
    internal sealed class HelixService : IHelixService
    {
        private const int MaximumJobsPerBuildQuery = 1_000;

        private readonly ILogger _logger;
        private readonly IHelixApi _helixApi;
        private readonly IBlobClientFactory _blobClientFactory;
        private readonly IFileSystem _fileSystem;
        private readonly JobMonitorMetrics _metrics;

        public HelixService(
            IHelixApi helixApi,
            ILogger logger,
            JobMonitorMetrics metrics = null)
            : this(helixApi, logger, new AzureBlobClientFactory(), new FileSystem(), metrics)
        {
        }

        internal HelixService(
            IHelixApi helixApi,
            ILogger logger,
            IBlobClientFactory blobClientFactory,
            IFileSystem fileSystem,
            JobMonitorMetrics metrics = null)
        {
            _helixApi = helixApi ?? throw new ArgumentNullException(nameof(helixApi));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _blobClientFactory = blobClientFactory ?? throw new ArgumentNullException(nameof(blobClientFactory));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _metrics = metrics ?? new JobMonitorMetrics();
        }

        public async Task<IReadOnlyList<HelixJobInfo>> GetJobsForBuildAsync(
            string source,
            string buildId,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentException("A non-empty Helix source filter must be provided.", nameof(source));
            }

            if (string.IsNullOrWhiteSpace(buildId))
            {
                throw new ArgumentException("A non-empty build ID filter must be provided.", nameof(buildId));
            }
            IImmutableDictionary<string, string> filterProperties = new Dictionary<string, string>()
            {
                ["BuildId"] = buildId,
            }.ToImmutableDictionary();

            IImmutableList<JobSummary> jobs = await RetryAsync(
                async () => await _helixApi.Job.ListAsync(
                    source: source,
                    properties: filterProperties,
                    count: MaximumJobsPerBuildQuery),
                cancellationToken);

            if (jobs.Count >= MaximumJobsPerBuildQuery)
            {
                throw new InvalidOperationException(
                    $"Helix returned {jobs.Count} jobs for build '{buildId}', reaching the query limit of " +
                    $"{MaximumJobsPerBuildQuery}. Refusing to monitor a potentially truncated result set.");
            }

            // Keep the local check as a defensive contract boundary in case Helix returns a
            // malformed or unexpectedly broad response.
            return
            [
                ..jobs
                    .Where(j => j.Properties is JObject properties
                        && properties.TryGetValue("BuildId", out JToken id)
                        && buildId == id.Value<string>())
                    .Select(j => new HelixJobInfo(j))
            ];
        }

        public async Task<WorkItemTestResults> DownloadTestResultsAsync(
            string jobName,
            string workItemName,
            string workingDirectory,
            CancellationToken cancellationToken)
        {
            List<Exception> transientFailures = [];
            string outputDirectory = _fileSystem.PathCombine(workingDirectory, SanitizeDirName(jobName));
            _fileSystem.CreateDirectory(outputDirectory);

            JobResultsUri resultsUri = await RetryAsync(() => _helixApi.Job.ResultsAsync(jobName), cancellationToken);
            IImmutableList<UploadedFile> availableFiles = await RetryAsync(
                () => _helixApi.WorkItem.ListFilesAsync(workItemName, jobName, false),
                cancellationToken);

            availableFiles = [.. availableFiles.Where(f => LooksLikeTestResultFile(f.Name))];
            if (availableFiles.Count == 0)
            {
                return new WorkItemTestResults(jobName, workItemName, []);
            }

            string workItemDirectory = _fileSystem.PathCombine(outputDirectory, SanitizeDirName(workItemName));
            _fileSystem.CreateDirectory(workItemDirectory);

            List<string> workItemFiles = [];
            foreach (UploadedFile file in availableFiles)
            {
                string relativePath = NormalizeUploadedFilePath(file.Name);
                string destinationFile = _fileSystem.PathCombine(workItemDirectory, relativePath);
                string directory = _fileSystem.GetDirectoryName(destinationFile);
                if (!string.IsNullOrEmpty(directory))
                {
                    _fileSystem.CreateDirectory(directory);
                }

                try
                {
                    IBlobClient blobClient = _blobClientFactory.CreateBlobClient(file.Link, resultsUri.ResultsUriRSAS);
                    await blobClient.DownloadToAsync(destinationFile, cancellationToken);
                    workItemFiles.Add(destinationFile);
                    _metrics.RecordResultBlobDownload(failed: false);
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    throw;
                }
                catch (Exception ex) when (TransientFailureDetector.IsTransient(ex))
                {
                    _metrics.RecordResultBlobDownload(failed: true);
                    transientFailures.Add(ex);
                    _logger.LogWarning(ex,
                        "Transient failure downloading '{FileName}' for '{JobName}/{WorkItemName}'. "
                        + "The remaining files will still be attempted before the work item is retried.",
                        file.Name,
                        jobName,
                        workItemName);
                }
                catch (Exception ex)
                {
                    _metrics.RecordResultBlobDownload(failed: true);
                    _logger.LogWarning(ex, "Failed to download '{FileName}' for '{JobName}/{WorkItemName}'.", file.Name, jobName, workItemName);
                }
            }

            if (transientFailures.Count > 0)
            {
                throw new IOException(
                    $"One or more transient test-result downloads failed for Helix job '{jobName}'.",
                    new AggregateException(transientFailures));
            }

            return new WorkItemTestResults(jobName, workItemName, workItemFiles);
        }

        private static bool LooksLikeTestResultFile(string path)
            => LocalTestResultsReader.LooksLikeTestResultFile(path);

        private static string NormalizeUploadedFilePath(string path)
            => path.Replace('\\', System.IO.Path.DirectorySeparatorChar).Replace('/', System.IO.Path.DirectorySeparatorChar);

        private static string SanitizeDirName(string value)
        {
            foreach (char invalidChar in System.IO.Path.GetInvalidFileNameChars())
            {
                value = value.Replace(invalidChar, '-');
            }

            return value;
        }

        public async Task<IReadOnlyCollection<WorkItemSummary>> ListWorkItemsAsync(
            string jobName,
            CancellationToken cancellationToken)
        {
            return await RetryAsync(async () =>
            {
                IImmutableList<WorkItemSummary> workItems = await _helixApi.WorkItem.ListAsync(jobName);
                return workItems
                    .Where(w => w.Name != "HelixController Work Queueing")
                    .ToList();
            }, cancellationToken);
        }

        public async Task CancelJobAsync(
            string jobName,
            CancellationToken cancellationToken)
        {
            await RetryAsync(
                async () =>
                {
                    await _helixApi.Job.CancelAsync(jobName, cancellationToken: cancellationToken);
                    return jobName;
                },
                cancellationToken);
        }

        public async Task<HelixJobInfo> ResubmitWorkItemsAsync(
            HelixJobInfo originalJob,
            IReadOnlyCollection<WorkItemSummary> failedWorkItems,
            string targetStageAttempt,
            string monitorJobAttempt,
            CancellationToken cancellationToken)
        {
            string originalJobName = originalJob.JobName;
            string originalDisplay = originalJob.DisplayName;
            IEnumerable<string> workItemsToLog = failedWorkItems.Select(wi => wi.Name);

            if (failedWorkItems.Count > 20)
            {
                workItemsToLog = workItemsToLog.Take(19).Append(failedWorkItems.Count - 19 + " more ...");
            }

            _logger.LogInformation("Resubmitting {Count} failed work item(s) for job {JobName}:{nl}- {WorkItems}",
                failedWorkItems.Count,
                originalDisplay,
                Environment.NewLine,
                string.Join(Environment.NewLine + "- ", workItemsToLog));

            // 1. Read the original job's metadata so we can clone its queue, type, source, and properties.
            JobDetails details = await RetryAsync(
                () => _helixApi.Job.DetailsAsync(originalJobName),
                cancellationToken);

            if (string.IsNullOrEmpty(details.QueueId)
                || string.IsNullOrEmpty(details.Type)
                || string.IsNullOrEmpty(details.JobList))
            {
                _logger.LogWarning(
                    "Cannot resubmit job '{JobName}' because its details are missing required fields (QueueId/Type/JobList).",
                    originalJobName);
                return null;
            }

            // 2. Download the original job-list JSON; it contains the work item entries that
            //    reference the existing payload/correlation payload blobs which we want to reuse.
            string originalJobListJson;
            try
            {
                originalJobListJson = await RetryAsync(
                    async () =>
                    {
                        IBlobClient jobListBlob = _blobClientFactory.CreateBlobClient(details.JobList);
                        BinaryData content = await jobListBlob.DownloadContentAsync(cancellationToken);
                        return content.ToString();
                    },
                    cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to download original job list for '{JobName}' from '{JobListUri}'.", originalJobName, details.JobList);
                return null;
            }

            // 3. Filter the entries to just the failed work items. We keep the JObject entries
            //    intact so PayloadUri / CorrelationPayloadUrisWithDestinations / Command etc. are
            //    preserved verbatim and continue to point at the original payload blobs.
            JArray originalEntries;
            try
            {
                originalEntries = JArray.Parse(originalJobListJson);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Original job list for '{JobName}' is not a valid JSON array.", originalJobName);
                return null;
            }

            var requestedSet = new HashSet<string>(failedWorkItems.Select(wi => wi.Name), StringComparer.OrdinalIgnoreCase);
            var filteredEntries = new JArray(
                originalEntries
                    .OfType<JObject>()
                    .Where(entry => entry["WorkItemId"] is JValue id
                        && id.Value is string idValue
                        && requestedSet.Contains(idValue)));

            if (filteredEntries.Count == 0)
            {
                _logger.LogWarning(
                    "Cannot resubmit job '{JobName}': none of the requested work items were found in its job list.",
                    originalJobName);
                return null;
            }

            string filteredJobListJson = filteredEntries.ToString(Formatting.Indented);

            // 4. Create a fresh storage container for the new job list and upload it.
            ContainerInformation container = await RetryAsync(
                () => _helixApi.Storage.NewAsync(
                    new ContainerCreationRequest(expirationInDays: 30, desiredName: "joblists", targetQueue: details.QueueId),
                    cancellationToken),
                cancellationToken);

            string blobName = $"job-list-{Guid.NewGuid():N}.json";
            var containerUri = new Uri($"https://{container.StorageAccountName}.blob.core.windows.net/{container.ContainerName}");
            IBlobClient jobListBlobClient = _blobClientFactory.CreateBlobClient(containerUri, blobName, container.WriteToken);

            await RetryAsync(
                async () =>
                {
                    await jobListBlobClient.UploadAsync(BinaryData.FromString(filteredJobListJson), overwrite: true, cancellationToken);
                    return true;
                },
                cancellationToken);

            string newJobListUri = AppendSasIfPresent(jobListBlobClient.Uri, container.ReadToken);

            // Stamp the resubmission with the resubmitting monitor's own stage attempt (when
            // known) so the monitor gates on its own resubmission; otherwise preserve the
            // original job's attempt (build + stage back-compat). Uses the same property name the
            // Helix submitter stamps (HelixJobInfo.StageAttemptPropertyName / System.StageAttempt).
            string resubmittedStageAttempt = !string.IsNullOrEmpty(targetStageAttempt)
                ? targetStageAttempt
                : GetStringPropertyFromProperties(details.Properties, HelixJobInfo.StageAttemptPropertyName);

            ImmutableDictionary<string, string> resubmittedProperties =
                ConvertPropertiesToImmutableDictionary(details.Properties)
                    .SetItem(HelixJobInfo.PreviousHelixJobNamePropertyName, originalJobName);
            if (!string.IsNullOrEmpty(resubmittedStageAttempt))
            {
                resubmittedProperties = resubmittedProperties.SetItem(HelixJobInfo.StageAttemptPropertyName, resubmittedStageAttempt);
            }
            if (!string.IsNullOrEmpty(monitorJobAttempt))
            {
                resubmittedProperties = resubmittedProperties.SetItem(
                    HelixJobInfo.ResubmittedByJobAttemptPropertyName,
                    monitorJobAttempt);
            }

            // 5. Build the new job creation request, copying over Source / Properties / Creator
            //    so the resubmitted job remains discoverable (BuildId, System.StageName, TestRunName, etc.).
            //    The QueueId / QueueAlias / DockerTag returned by Job.DetailsAsync ensure the resubmitted
            //    work item runs in the same execution environment (e.g. the same Docker container) as the
            //    original job.
            var creationRequest = new JobCreationRequest(details.Type, newJobListUri, details.QueueId)
            {
                Source = details.Source,
                Creator = details.Creator,
                DockerTag = details.DockerTag,
                QueueAlias = details.QueueAlias,
                Properties = resubmittedProperties,
            };

            string idempotencyKey = Guid.NewGuid().ToString("N");
            JobCreationResult newJob = await RetryAsync(
                () => _helixApi.Job.NewAsync(creationRequest, idempotencyKey, cancellationToken: cancellationToken),
                cancellationToken);

            string testRunName = GetStringPropertyFromProperties(details.Properties, "TestRunName") ?? newJob.Name;
            string stageName = GetStringPropertyFromProperties(details.Properties, "System.StageName");
            string submitterJobName = GetStringPropertyFromProperties(details.Properties, "System.JobName");
            string submitterJobDisplayName = GetStringPropertyFromProperties(details.Properties, "System.JobDisplayName");
            string logicalJobName = GetStringPropertyFromProperties(details.Properties, HelixJobInfo.LogicalJobNamePropertyName);
            string submitterPhaseName = GetStringPropertyFromProperties(details.Properties, "System.PhaseName");
            string submitterJobAttempt = GetStringPropertyFromProperties(details.Properties, HelixJobInfo.JobAttemptPropertyName);

            var newJobInfo = new HelixJobInfo(
                newJob.Name,
                "running",
                testRunName,
                stageName,
                submitterJobName,
                submitterJobDisplayName,
                details.QueueId,
                originalJobName,
                stageAttempt: resubmittedStageAttempt,
                jobAttempt: submitterJobAttempt,
                logicalJobName: logicalJobName,
                submitterPhaseName: submitterPhaseName);

            _logger.LogInformation("Resubmitted {Count} failed work item(s) from '{OriginalJobName}' as new job '{NewJobName}'{nl}{JobUri}",
                filteredEntries.Count,
                originalDisplay,
                newJobInfo.DisplayName,
                Environment.NewLine,
                HelixJobInfo.GetDetailsUri(newJob.Name));

            return newJobInfo;
        }

        private static ImmutableDictionary<string, string> ConvertPropertiesToImmutableDictionary(JToken properties)
        {
            if (properties is not JObject obj)
            {
                return [];
            }

            ImmutableDictionary<string, string>.Builder builder = ImmutableDictionary.CreateBuilder<string, string>();
            foreach (JProperty prop in obj.Properties())
            {
                if (prop.Value is null || prop.Value.Type == JTokenType.Null)
                {
                    continue;
                }

                builder[prop.Name] = prop.Value.Type == JTokenType.String
                    ? (string)prop.Value
                    : prop.Value.ToString(Formatting.None);
            }

            return builder.ToImmutable();
        }

        private async Task<T> RetryAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken)
        {
            Exception last = null;
            T result = default;
            int attempt = 0;
            var retryHandler = new ExponentialRetry
            {
                MaxAttempts = 5,
                DelayBase = 2,
                DelayConstant = 0,
                MinRandomFactor = 1,
                MaxRandomFactor = 1,
                RetryDelayCallback = (failedAttempt, delay) =>
                    _logger.LogDebug(
                        "Transient Helix request failure on attempt {Attempt} of {AttemptCount}. "
                        + "Waiting {RetryDelay} before the next attempt.",
                        failedAttempt,
                        5,
                        delay),
            };

            bool succeeded = await retryHandler.RunAsync(
                async _ =>
                {
                    int currentAttempt = attempt++;
                    try
                    {
                        result = await action();
                        _metrics.RecordHelixRequest(isRetry: currentAttempt > 0, failed: false);
                        return RetryResult.Success;
                    }
                    catch (Exception ex) when (!cancellationToken.IsCancellationRequested)
                    {
                        _metrics.RecordHelixRequest(isRetry: currentAttempt > 0, failed: true);
                        last = ex;
                        return RetryResult.Retry();
                    }
                },
                cancellationToken);

            return succeeded
                ? result
                : throw last ?? new InvalidOperationException("Retry failed without capturing an exception.");
        }

        private static string GetStringPropertyFromProperties(JToken properties, string name)
        {
            if (properties is JObject obj && obj.TryGetValue(name, out JToken token))
            {
                string value = token?.ToString();
                if (!string.IsNullOrEmpty(value))
                {
                    return value;
                }
            }

            return null;
        }

        private static string AppendSasIfPresent(Uri blobUri, string sas)
        {
            if (string.IsNullOrEmpty(sas))
            {
                return blobUri.ToString();
            }

            string trimmed = sas.StartsWith("?", StringComparison.Ordinal) ? sas.Substring(1) : sas;
            var builder = new UriBuilder(blobUri)
            {
                Query = trimmed,
            };
            return builder.Uri.ToString();
        }
    }
}
