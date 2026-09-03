// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
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
        // Matches the "(alias)queueId" portion of a Helix target queue string. Mirrors the regex used by
        // JobDefinition.ParseQueueId in the Helix JobSender so resubmission parses queues identically.
        // Part of the operatingSystem-property workaround; see ResolveTargetQueueSpec and dotnet/arcade#16964.
        private static readonly Regex s_queueAliasRegex = new(@"\((.+?)\)(.*)", RegexOptions.Compiled);

        private readonly ILogger _logger;
        private readonly IHelixApi _helixApi;
        private readonly IBlobClientFactory _blobClientFactory;
        private readonly IFileSystem _fileSystem;

        public HelixService(IHelixApi helixApi, ILogger logger)
            : this(helixApi, logger, new AzureBlobClientFactory(), new FileSystem())
        {
        }

        internal HelixService(
            IHelixApi helixApi,
            ILogger logger,
            IBlobClientFactory blobClientFactory,
            IFileSystem fileSystem)
        {
            _helixApi = helixApi ?? throw new ArgumentNullException(nameof(helixApi));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _blobClientFactory = blobClientFactory ?? throw new ArgumentNullException(nameof(blobClientFactory));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
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

            IImmutableList<JobSummary> jobs = await RetryHelper.RetryAsync(
                async () => await _helixApi.Job.ListAsync(source: source, count: 100_000),
                cancellationToken);

            return
            [
                ..jobs
                    .Where(j => j.Properties is JObject properties
                        && properties.TryGetValue("BuildId", out JToken id)
                        && buildId == id.Value<string>())
                    .Select(j => new HelixJobInfo(j))
             ];
        }

        public async Task<IReadOnlyList<WorkItemTestResults>> DownloadTestResultsAsync(
            string jobName,
            IReadOnlyCollection<string> workItemNames,
            string workingDirectory,
            CancellationToken cancellationToken)
        {
            List<WorkItemTestResults> downloadedFiles = [];
            string outputDirectory = _fileSystem.PathCombine(workingDirectory, SanitizeDirName(jobName));
            _fileSystem.CreateDirectory(outputDirectory);

            JobResultsUri resultsUri = await RetryHelper.RetryAsync(() => _helixApi.Job.ResultsAsync(jobName), cancellationToken);

            foreach (string workItemName in workItemNames)
            {
                IImmutableList<UploadedFile> availableFiles = await RetryHelper.RetryAsync(
                    () => _helixApi.WorkItem.ListFilesAsync(workItemName, jobName, false),
                    cancellationToken);

                availableFiles = [.. availableFiles.Where(f => LooksLikeTestResultFile(f.Name))];
                if (availableFiles.Count == 0)
                {
                    continue;
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
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to download '{FileName}' for '{JobName}/{WorkItemName}'.", file.Name, jobName, workItemName);
                    }
                }

                downloadedFiles.Add(new WorkItemTestResults(jobName, workItemName, workItemFiles));
            }

            return downloadedFiles;
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
            return await RetryHelper.RetryAsync(async () =>
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
            await RetryHelper.RetryAsync(
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
            JobDetails details = await RetryHelper.RetryAsync(
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
                originalJobListJson = await RetryHelper.RetryAsync(
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
            ContainerInformation container = await RetryHelper.RetryAsync(
                () => _helixApi.Storage.NewAsync(
                    new ContainerCreationRequest(expirationInDays: 30, desiredName: "joblists", targetQueue: details.QueueId),
                    cancellationToken),
                cancellationToken);

            string blobName = $"job-list-{Guid.NewGuid():N}.json";
            var containerUri = new Uri($"https://{container.StorageAccountName}.blob.core.windows.net/{container.ContainerName}");
            IBlobClient jobListBlobClient = _blobClientFactory.CreateBlobClient(containerUri, blobName, container.WriteToken);

            await RetryHelper.RetryAsync(
                async () =>
                {
                    await jobListBlobClient.UploadAsync(BinaryData.FromString(filteredJobListJson), overwrite: true, cancellationToken);
                    return true;
                },
                cancellationToken);

            string newJobListUri = AppendSasIfPresent(jobListBlobClient.Uri, container.ReadToken);

            // 5. Reconstruct the original target queue so the resubmitted work item runs in the same
            //    execution environment (e.g. the same Docker container) as the original job. The logic
            //    used to recover the Docker tag / queue alias is intentionally isolated in a single
            //    replaceable method (see ResolveTargetQueueSpec) because it is a temporary workaround.
            TargetQueueSpec targetQueue = ResolveTargetQueueSpec(details, originalJobName);

            // 6. Build the new job creation request, copying over Source / Properties / Creator
            //    so the resubmitted job remains discoverable (BuildId, System.StageName, TestRunName, etc.).
            var creationRequest = new JobCreationRequest(details.Type, newJobListUri, targetQueue.QueueId)
            {
                Source = details.Source,
                Creator = details.Creator,
                DockerTag = targetQueue.DockerTag,
                QueueAlias = targetQueue.QueueAlias,
                Properties = ConvertPropertiesToImmutableDictionary(details.Properties)
                    .SetItem(HelixJobInfo.PreviousHelixJobNamePropertyName, originalJobName),
            };

            string idempotencyKey = Guid.NewGuid().ToString("N");
            JobCreationResult newJob = await RetryHelper.RetryAsync(
                () => _helixApi.Job.NewAsync(creationRequest, idempotencyKey, cancellationToken: cancellationToken),
                cancellationToken);

            string testRunName = GetStringPropertyFromProperties(details.Properties, "TestRunName") ?? newJob.Name;
            string stageName = GetStringPropertyFromProperties(details.Properties, "System.StageName");
            string submitterJobName = GetStringPropertyFromProperties(details.Properties, "System.JobName");
            string submitterJobDisplayName = GetStringPropertyFromProperties(details.Properties, "System.JobDisplayName");

            var newJobInfo = new HelixJobInfo(
                newJob.Name,
                "running",
                testRunName,
                stageName,
                submitterJobName,
                submitterJobDisplayName,
                details.QueueId,
                originalJobName);

            _logger.LogInformation("Resubmitted {Count} failed work item(s) from '{OriginalJobName}' as new job '{NewJobName}'{nl}{JobUri}",
                filteredEntries.Count,
                originalDisplay,
                newJobInfo.DisplayName,
                Environment.NewLine,
                HelixJobInfo.GetDetailsUri(newJob.Name));

            return newJobInfo;
        }

        // ----------------------------------------------------------------------------------------------
        // WORKAROUND (tracked by https://github.com/dotnet/arcade/issues/16964)
        //
        // Determines the (queueId, dockerTag, queueAlias) to resubmit a job into the SAME execution
        // environment as the original. Today Job.DetailsAsync only returns the *resolved* QueueId; the
        // Docker tag and queue alias are stripped from the response. We recover them from the
        // 'operatingSystem' property, which the Helix SDK stamps with the verbatim
        // "(alias)queueId@dockerTag" target-queue string (see Microsoft.DotNet.Helix.Sdk/MonoQueue.targets),
        // re-parsing it with the same logic the fresh submission path uses (JobDefinition.ParseQueueId).
        // We fall back to the resolved QueueId (no Docker tag/alias) when the property is missing or
        // unparseable.
        //
        // This is the single seam that should change once dotnet-helix-service PR 61770 ships and
        // Microsoft.DotNet.Helix.Client is regenerated to expose JobDetails.DockerTag /
        // JobDetails.QueueAlias: replace the body below with a direct read of those fields, then delete
        // ParseQueueId and s_queueAliasRegex.
        // ----------------------------------------------------------------------------------------------
        private TargetQueueSpec ResolveTargetQueueSpec(JobDetails details, string originalJobName)
        {
            string originalTargetQueue = GetStringPropertyFromProperties(details.Properties, "operatingSystem");
            if (!string.IsNullOrEmpty(originalTargetQueue))
            {
                (string parsedQueueId, string parsedDockerTag, string parsedQueueAlias) = ParseQueueId(originalTargetQueue);
                if (!string.IsNullOrEmpty(parsedQueueId))
                {
                    return new TargetQueueSpec(parsedQueueId, parsedDockerTag, parsedQueueAlias);
                }

                _logger.LogWarning(
                    "Could not parse a queue id from the 'operatingSystem' property '{OperatingSystem}' of job '{JobName}'; resubmitting on resolved queue '{QueueId}' without a Docker tag.",
                    originalTargetQueue, originalJobName, details.QueueId);
            }

            return new TargetQueueSpec(details.QueueId, DockerTag: null, QueueAlias: null);
        }

        // The execution-environment coordinates used to (re)submit a Helix job.
        private readonly record struct TargetQueueSpec(string QueueId, string DockerTag, string QueueAlias);

        // Splits a Helix target queue string of the form "(alias)queueId@dockerTag" into its parts.
        // Mirrors JobDefinition.ParseQueueId in the Helix JobSender so a resubmitted job reconstructs the
        // same (queueId, dockerTag, queueAlias) tuple the fresh submission path produced.
        // Part of the operatingSystem-property workaround; see ResolveTargetQueueSpec and dotnet/arcade#16964.
        private static (string queueId, string dockerTag, string queueAlias) ParseQueueId(string value)
        {
            int index = value.IndexOf('@');
            if (index < 0)
            {
                return (value, string.Empty, value);
            }

            string queueInfo = value.Substring(0, index);
            string dockerTag = value.Substring(index + 1);

            Match queueInfoSplit = s_queueAliasRegex.Match(queueInfo);
            if (queueInfoSplit.Success && queueInfoSplit.Groups.Count == 3)
            {
                return (queueInfoSplit.Groups[2].Value, dockerTag, queueInfoSplit.Groups[1].Value);
            }

            return (queueInfo, dockerTag, queueInfo);
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
