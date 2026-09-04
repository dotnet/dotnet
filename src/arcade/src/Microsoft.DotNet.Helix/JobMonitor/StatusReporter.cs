// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.DotNet.Helix.Client.Models;
using Microsoft.DotNet.Helix.JobMonitor.Models;
using Microsoft.Extensions.Logging;

namespace Microsoft.DotNet.Helix.JobMonitor
{
    /// <summary>
    /// Owns every log line the monitor emits about progress, completions, failures, and
    /// the timeout report. Reads from a shared <see cref="MonitorState"/> so the runner's
    /// main loop only has to call into a small number of intent-named methods.
    /// </summary>
    internal sealed class StatusReporter
    {
        private const string AzdoWarningPrefix = "##vso[task.logissue type=warning]";
        private const string AzdoErrorPrefix = "##vso[task.logissue type=error]";
        private const int MaximumReportedPipelineJobs = 10;

        /// <summary>
        /// Public link to the Helix Job Monitor user documentation. Printed at the start
        /// and end of every monitor invocation so pipeline users can self-serve on what
        /// the monitor does, how reruns work, and how it interacts with the rest of the
        /// build.
        /// </summary>
        private const string DocumentationUri =
            "https://github.com/dotnet/arcade/blob/main/src/Microsoft.DotNet.Helix/Sdk/Readme.md#helix-job-monitor-for-azure-devops";

        private readonly ILogger _logger;
        private readonly JobMonitorOptions _options;
        private readonly MonitorState _state;

        public StatusReporter(ILogger logger, JobMonitorOptions options, MonitorState state)
        {
            _logger = logger;
            _options = options;
            _state = state;
        }

        public void LogMonitorStart()
        {
            _logger.LogInformation("Monitoring Helix jobs for stage {stage} of build {BuildId}:{nl}{collectionUri}{teamProject}/_build/results?buildId={BuildId}",
                _options.StageName,
                _options.BuildId,
                Environment.NewLine,
                _options.CollectionUri,
                _options.TeamProject,
                _options.BuildId);

            _logger.LogInformation(
                "📖 Read more about what this monitor job does: {DocsUri}",
                DocumentationUri);
        }

        public void LogRetryPassFoundNothing()
        {
            _logger.LogInformation("No failed jobs found to resubmit");
        }

        public void LogRetryPassStart()
        {
            _logger.LogInformation("🔁 Checking for failed Helix jobs to resubmit the failed work items...");
        }

        /// <summary>
        /// Logs the work items being resubmitted for one Helix job, grouped by why each
        /// item is being retried (non-zero Helix exit code vs. work item that exited 0 but
        /// whose AzDO test results contained failures recorded by a prior monitor invocation).
        /// </summary>
        public void LogRetryPassResubmission(
            HelixJobInfo helixJob,
            IReadOnlyCollection<WorkItemSummary> exitCodeFailures,
            IReadOnlyCollection<WorkItemSummary> testOnlyFailures)
        {
            int total = exitCodeFailures.Count + testOnlyFailures.Count;
            IEnumerable<string> lines = exitCodeFailures
                .OrderBy(wi => wi.Name, StringComparer.OrdinalIgnoreCase)
                .Select(wi => $"{wi.Name} (Helix exit code {wi.ExitCode?.ToString() ?? "?"})")
                .Concat(testOnlyFailures
                    .OrderBy(wi => wi.Name, StringComparer.OrdinalIgnoreCase)
                    .Select(wi => $"{wi.Name} (exit code 0, failed AzDO tests)"));

            _logger.LogInformation(
                "🔁 Resubmitting {Total} work item(s) for job '{JobName}' " +
                "({ExitCodeCount} by Helix exit code, {TestOnlyCount} by failed AzDO tests):{nl}- {WorkItems}",
                total,
                helixJob.DisplayName,
                exitCodeFailures.Count,
                testOnlyFailures.Count,
                Environment.NewLine,
                string.Join(Environment.NewLine + "- ", lines));
        }

        public void LogJobCompleted(HelixJobInfo helixJob, IReadOnlyCollection<WorkItemSummary> workItems)
        {
            int failedWorkItemCount = workItems.Count(wi => wi.IsFailed);
            int successfulWorkItemCount = workItems.Count - failedWorkItemCount;

            _logger.LogInformation("{Icon} Job '{JobName}' {Status} ({PassedCount} passed, {FailedCount} failed){nl}{JobUri}",
                failedWorkItemCount == 0 ? "✅" : "❌",
                helixJob.DisplayName,
                failedWorkItemCount == 0 ? "succeeded" : "failed",
                successfulWorkItemCount,
                failedWorkItemCount,
                Environment.NewLine, helixJob.DetailsUri);
        }

        public void LogJobProcessingStart(HelixJobInfo helixJob)
        {
            _logger.LogInformation("Job {JobName} completed. Processing test results...{nl}{JobUri}",
                helixJob.DisplayName,
                Environment.NewLine,
                helixJob.DetailsUri);
        }

        /// <summary>
        /// Emits one console-link warning per newly-observed failed work item (deduplicated
        /// for the lifetime of this invocation).
        /// </summary>
        public void LogFailedWorkItemConsoleLinks(HelixJobInfo helixJob, IEnumerable<WorkItemSummary> workItems)
        {
            foreach (WorkItemSummary workItem in workItems.OrderBy(wi => wi.Name, StringComparer.OrdinalIgnoreCase))
            {
                if (!_state.TryReportFailedWorkItemConsoleLink($"{helixJob.JobName}/{workItem.Name}"))
                {
                    continue;
                }

                LogWarning($"Work item '{workItem.Name}' in job '{helixJob.DisplayName}' failed ({workItem.FormattedState}).{Environment.NewLine}Console: {MonitorState.GetConsoleOutputText(workItem.ConsoleOutputUri)}");
            }
        }

        /// <summary>
        /// Emits the one-line summary of work counts (and, in verbose mode, a tree of jobs
        /// and work items). Also emits per-failure console-link warnings for any failed
        /// work items observed in this poll that haven't been reported yet.
        /// </summary>
        public void LogPollStatus(
            IReadOnlyList<HelixJobInfo> jobs,
            IReadOnlyDictionary<string, IReadOnlyCollection<WorkItemSummary>> workItemsByJob,
            IReadOnlySet<string> completedJobNames,
            IReadOnlyList<AzureDevOpsTimelineRecord> timelineRecords,
            UploadPipelineSnapshot uploads)
        {
            List<HelixJobInfo> orderedJobs =
            [
                ..jobs.OrderBy(j => j.JobName, StringComparer.OrdinalIgnoreCase)
            ];

            foreach (HelixJobInfo job in orderedJobs)
            {
                IReadOnlyCollection<WorkItemSummary> workItems = workItemsByJob[job.JobName];
                LogFailedWorkItemConsoleLinks(job, workItems.Where(wi => wi.IsFailedAndTerminal));
            }

            JobWorkItemStatusCounts counts = ComputeCounts(orderedJobs, workItemsByJob, completedJobNames);
            IReadOnlyList<AzureDevOpsTimelineRecord> pipelineJobs =
            [
                ..HelixJobMonitorUtilities
                    .GetRelevantNonMonitorJobRecords(timelineRecords, _options.JobMonitorName)
            ];
            PipelineJobStatusCounts pipelineCounts = ComputePipelineJobCounts(pipelineJobs);

            _logger.LogInformation(
                "ℹ️ Status: Helix jobs: {ProcessedJobs} processed / {CompletedJobs} completed / {RunningJobs} running / {WaitingJobs} waiting{nl}"
              + "           Helix work items: {ProcessedWorkItems} processed / {CompletedWorkItems} completed / {RunningWorkItems} running / {WaitingWorkItems} waiting{nl}"
              + "           Azure DevOps jobs: {CompletedPipelineJobs} completed / {RunningPipelineJobs} running / {WaitingPipelineJobs} waiting",
                counts.ProcessedJobs,
                counts.CompletedJobs,
                counts.RunningJobs,
                counts.WaitingJobs,
                Environment.NewLine,
                counts.ProcessedWorkItems,
                counts.CompletedWorkItems,
                counts.RunningWorkItems,
                counts.WaitingWorkItems,
                Environment.NewLine,
                pipelineCounts.Completed,
                pipelineCounts.Running,
                pipelineCounts.Waiting);

            if (counts.RunningJobs == 0
                && counts.WaitingJobs == 0
                && pipelineCounts.Running + pipelineCounts.Waiting > 0)
            {
                LogPipelineCompletionWait(pipelineJobs);
            }

            if (_options.Verbose)
            {
                _logger.LogDebug(
                    "Upload pipeline: jobs {JobQueued} queued/{JobActive} active, work items "
                    + "{WorkItemQueued} queued/{WorkItemActive} active, finalizers "
                    + "{FinalizerQueued} queued/{FinalizerActive} active, {UploadedResults} results uploaded.",
                    uploads.Jobs.Queued,
                    uploads.Jobs.Active,
                    uploads.WorkItems.Queued,
                    uploads.WorkItems.Active,
                    uploads.Finalizers.Queued,
                    uploads.Finalizers.Active,
                    uploads.UploadedResults);
            }
        }

        public void LogFinalSummary(int totalAssociatedJobCount)
        {
            _logger.LogInformation(
                "📊 Final summary:{nl}"
              + "   Jobs:       {TotalJobs} submitted / {ResubmittedJobs} resubmitted / {ProcessedJobs} processed{nl}"
              + "   Work items: {TotalWorkItems} submitted / {ResubmittedWorkItems} resubmitted / {FailedWorkItems} failed",
                Environment.NewLine,
                totalAssociatedJobCount,
                _state.ResubmittedJobCount,
                _state.ProcessedJobCount,
                Environment.NewLine,
                _state.WorkItemOutcomeCount,
                _state.ResubmittedWorkItemCount,
                _state.FailedWorkItemCount);

            _logger.LogInformation(
                "📖 Read more about what this monitor job does: {DocsUri}",
                DocumentationUri);
        }

        public void LogPerformanceMetrics(
            JobMonitorMetricsSnapshot metrics,
            UploadPipelineSnapshot uploads,
            bool isPartial = false)
        {
            double pipelineSeconds = Math.Max(metrics.PipelineElapsed.TotalSeconds, 0.001);
            double resultThroughput = uploads.UploadedResults / pipelineSeconds;
            double workItemThroughput = uploads.WorkItems.Completed / pipelineSeconds;
            double payloadMiB = metrics.AzureDevOpsPayloadBytes / (1024d * 1024d);
            string heading = isPartial
                ? "⏱️ Partial performance metrics captured during cancellation"
                : "⏱️ Performance metrics";
            string throughputLabel = isPartial
                ? "Completed before cancellation"
                : "Upload throughput";
            string throughputSummary = isPartial
                ? $"{uploads.UploadedResults} test result(s); "
                    + $"{uploads.WorkItems.Completed} work item(s); rates unavailable for a partial snapshot"
                : $"{uploads.UploadedResults} test result(s), {resultThroughput:0.##} result(s)/s; "
                    + $"{uploads.WorkItems.Completed} work item(s), {workItemThroughput:0.##} work item(s)/s";

            _logger.LogInformation(
                "{Heading} ({Elapsed} monitor elapsed, {PipelineElapsed} observed upload pipeline activity):{nl}"
              + "   {ThroughputLabel}: {ThroughputSummary}{nl}"
              + "   Azure DevOps HTTP: {AzdoRequests} request attempt(s) "
              + "({ControlRequests} control, {ResultRequests} result batch, {AttachmentRequests} attachment), "
              + "{AzdoRetries} retry attempt(s), {AzdoFailedAttempts} failed attempt(s), "
              + "{PayloadMiB:0.##} MiB request payload; aggregate request time {AzdoRequestTime}, "
              + "max {MaximumAzdoRequestTime}{nl}"
              + "   Helix/storage: {HelixRequests} retry-wrapped remote request attempt(s), "
              + "{HelixRetries} retry attempt(s), {HelixFailedAttempts} failed attempt(s); "
              + "{BlobDownloads} result blob download attempt(s), {BlobFailures} failed{nl}"
              + "   Rate limiting: {RateLimitDeferrals} server-directed deferral(s), "
              + "{RateLimitDeferredTime} cumulative guidance, max {MaximumRateLimitDeferral}; "
              + "{RateLimitWaits} shared-gate worker wait(s), aggregate {RateLimitWaitTime}, "
              + "max {MaximumRateLimitWaitTime}{nl}"
              + "   Pipeline aggregate worker time (max single operation): download {DownloadTime} ({MaxDownloadTime}), "
              + "parse/aggregate {ParseTime} ({MaxParseTime}), publish {PublishTime} ({MaxPublishTime}), "
              + "create run {CreateTime} ({MaxCreateTime}), complete run {CompleteTime} ({MaxCompleteTime}).",
                heading,
                metrics.Elapsed,
                metrics.PipelineElapsed,
                Environment.NewLine,
                throughputLabel,
                throughputSummary,
                Environment.NewLine,
                metrics.AzureDevOpsRequests,
                metrics.AzureDevOpsControlRequests,
                metrics.AzureDevOpsResultRequests,
                metrics.AzureDevOpsAttachmentRequests,
                metrics.AzureDevOpsRetries,
                metrics.AzureDevOpsFailedAttempts,
                payloadMiB,
                metrics.AzureDevOpsRequestTime,
                metrics.MaximumAzureDevOpsRequestTime,
                Environment.NewLine,
                metrics.HelixRequests,
                metrics.HelixRetries,
                metrics.HelixFailedAttempts,
                metrics.ResultBlobDownloads,
                metrics.ResultBlobDownloadFailures,
                Environment.NewLine,
                metrics.RateLimitDeferrals,
                metrics.RateLimitDeferredTime,
                metrics.MaximumRateLimitDeferral,
                metrics.RateLimitWaits,
                metrics.RateLimitWaitTime,
                metrics.MaximumRateLimitWaitTime,
                Environment.NewLine,
                metrics.WorkItemDownloadTime,
                metrics.MaximumWorkItemDownloadTime,
                metrics.ParseTime,
                metrics.MaximumParseTime,
                metrics.WorkItemPublishTime,
                metrics.MaximumWorkItemPublishTime,
                metrics.TestRunCreateTime,
                metrics.MaximumTestRunCreateTime,
                metrics.TestRunCompleteTime,
                metrics.MaximumTestRunCompleteTime);
        }

        public void LogNonMonitorPipelineFailure()
        {
            // DO NOT CHANGE THIS LINE - it's matched by Build Analysis to ignore a failure
            LogError("One or more non-monitor pipeline jobs failed.");
        }

        public void LogNoHelixJobsFailure()
        {
            LogError($"No Helix jobs were submitted by this stage in any attempt. Stage: '{_options.StageName}', BuildId: {_options.BuildId}.");
        }

        /// <summary>
        /// Emits the final aggregated failure block (one block listing every still-failing
        /// work item, prefixed with the test-results URL).
        /// </summary>
        public void LogFinalFailedWorkItems()
        {
            IReadOnlyList<FailedWorkItemConsoleInfo> snapshot = _state.SnapshotFailedWorkItemConsoleInfo();
            if (snapshot.Count == 0)
            {
                return;
            }

            List<FailedWorkItemConsoleInfo> failures =
            [
                ..snapshot
                    .OrderBy(failure => failure.WorkItemName, StringComparer.OrdinalIgnoreCase)
            ];

            var lines = new List<string>();
            for (int i = 0; i < failures.Count; i++)
            {
                FailedWorkItemConsoleInfo failure = failures[i];
                bool isLast = i == failures.Count - 1;
                string connector = isLast ? "└─" : "├─";
                string childPrefix = isLast ? "   " : "│  ";
                lines.Add($"{connector} {failure.WorkItemName} (Job: {failure.JobName}) ({failure.State})");
                lines.Add($"{childPrefix}└─ Console: {failure.ConsoleOutput}");
            }

            // DO NOT CHANGE THIS LINE - it's matched by Build Analysis to ignore a failure
            LogError($"Failed work item information:{Environment.NewLine}Test results: {GetTestResultsUri()}{Environment.NewLine}{string.Join(Environment.NewLine, lines)}");
        }

        /// <summary>
        /// Emits the timeout report: groups of unfinished Helix jobs and in-progress AzDO
        /// pipeline jobs, or a single critical-level note if nothing unfinished was tracked.
        /// </summary>
        public void ReportTimeout()
        {
            var timeout = TimeSpan.FromMinutes(_options.MaximumWaitMinutes);

            List<HelixJobInfo> unfinishedHelixJobs =
            [
                ..MonitorState.GetLatestHelixJobAttempts(_state.SnapshotAssociatedJobs())
                    .Where(j => !j.IsCompleted || !_state.IsHelixJobProcessed(j.JobName))
                    .OrderBy(j => j.JobName, StringComparer.OrdinalIgnoreCase)
            ];

            List<AzureDevOpsTimelineRecord> inProgressPipelineJobs =
            [
                ..HelixJobMonitorUtilities
                    .GetRelevantNonMonitorJobRecords(_state.SnapshotTimelineRecords(), _options.JobMonitorName)
                    .Where(r => !string.Equals(r.State, "completed", StringComparison.OrdinalIgnoreCase))
                    .OrderBy(r => r.Name ?? r.ReferenceName ?? r.Identifier ?? string.Empty, StringComparer.OrdinalIgnoreCase)
            ];

            if (unfinishedHelixJobs.Count == 0 && inProgressPipelineJobs.Count == 0)
            {
                _logger.LogCritical("Helix Job Monitor timed out after {TimeoutMinutes} minute(s) ({Timeout}). No unfinished Helix or Azure DevOps jobs were tracked at the time of timeout.",
                    timeout.TotalMinutes,
                    timeout);
                return;
            }

            if (unfinishedHelixJobs.Count > 0)
            {
                LogError(
                    $"Helix Job Monitor timed out after {timeout.TotalMinutes} minute(s) ({timeout}). {unfinishedHelixJobs.Count} Helix job(s) were unfinished or unprocessed:{Environment.NewLine}- {string.Join(Environment.NewLine + "- ", unfinishedHelixJobs.Select(FormatUnfinishedHelixJob))}{Environment.NewLine}");
            }

            if (inProgressPipelineJobs.Count > 0)
            {
                LogError(
                    $"At timeout, {inProgressPipelineJobs.Count} non-monitor Azure DevOps pipeline job(s) were still in progress or queued:{Environment.NewLine}- {string.Join(Environment.NewLine + "- ", inProgressPipelineJobs.Select(FormatInProgressPipelineJob))}{Environment.NewLine}");
            }
        }

        private void LogWarning(string message)
            => _logger.LogWarning("{Prefix}{Message}", AzdoWarningPrefix, message);

        private void LogError(string message)
            => _logger.LogError("{Prefix}{Message}", AzdoErrorPrefix, message);

        private JobWorkItemStatusCounts ComputeCounts(
            IReadOnlyList<HelixJobInfo> jobs,
            IReadOnlyDictionary<string, IReadOnlyCollection<WorkItemSummary>> workItemsByJob,
            IReadOnlySet<string> completedJobNames)
        {
            int processedJobs = 0, processedWorkItems = 0;
            int completedJobs = 0, completedWorkItems = 0;
            int runningJobs = 0, runningWorkItems = 0;
            int waitingJobs = 0, waitingWorkItems = 0;

            foreach (HelixJobInfo job in jobs)
            {
                IReadOnlyCollection<WorkItemSummary> workItems = workItemsByJob[job.JobName];

                if (_state.IsHelixJobProcessed(job.JobName))
                {
                    processedJobs++;
                    processedWorkItems += workItems.Count;
                }

                if (completedJobNames.Contains(job.JobName))
                {
                    completedJobs++;
                    completedWorkItems += workItems.Count;
                }
                else if (workItems.Count > 0)
                {
                    int jobWaitingCount = 0;
                    foreach (WorkItemSummary wi in workItems)
                    {
                        if (string.Equals(wi.State, "Waiting", StringComparison.OrdinalIgnoreCase))
                        {
                            jobWaitingCount++;
                        }
                    }

                    runningJobs++;
                    waitingWorkItems += jobWaitingCount;
                    runningWorkItems += workItems.Count - jobWaitingCount;
                }
                else
                {
                    waitingJobs++;
                    waitingWorkItems += job.InitialWorkItemCount ?? 0;
                }
            }

            return new JobWorkItemStatusCounts(
                processedJobs, processedWorkItems,
                completedJobs, completedWorkItems,
                runningJobs, runningWorkItems,
                waitingJobs, waitingWorkItems);
        }

        private static PipelineJobStatusCounts ComputePipelineJobCounts(
            IReadOnlyList<AzureDevOpsTimelineRecord> pipelineJobs)
        {
            int completed = 0, running = 0, waiting = 0;

            foreach (AzureDevOpsTimelineRecord job in pipelineJobs)
            {
                if (string.Equals(job.State, "completed", StringComparison.OrdinalIgnoreCase))
                {
                    completed++;
                }
                else if (string.Equals(job.State, "inProgress", StringComparison.OrdinalIgnoreCase))
                {
                    running++;
                }
                else
                {
                    waiting++;
                }
            }

            return new PipelineJobStatusCounts(completed, running, waiting);
        }

        private void LogPipelineCompletionWait(
            IReadOnlyList<AzureDevOpsTimelineRecord> pipelineJobs)
        {
            List<AzureDevOpsTimelineRecord> unfinishedJobs =
            [
                ..pipelineJobs
                    .Where(job => !string.Equals(job.State, "completed", StringComparison.OrdinalIgnoreCase))
                    .OrderBy(job => job.Name ?? job.ReferenceName ?? job.Identifier, StringComparer.OrdinalIgnoreCase)
            ];
            List<string> details =
            [
                ..unfinishedJobs
                    .Take(MaximumReportedPipelineJobs)
                    .Select(FormatInProgressPipelineJob)
            ];

            if (unfinishedJobs.Count > MaximumReportedPipelineJobs)
            {
                details.Add($"... and {unfinishedJobs.Count - MaximumReportedPipelineJobs} more");
            }

            _logger.LogInformation(
                "Helix work is complete. Waiting for {JobCount} Azure DevOps job(s) before the monitor can finish:{nl}- {Jobs}",
                unfinishedJobs.Count,
                Environment.NewLine,
                string.Join(Environment.NewLine + "- ", details));
        }

        private string GetTestResultsUri()
            => $"{_options.CollectionUri}{_options.TeamProject}/_build/results?buildId={_options.BuildId}&view=ms.vss-test-web.build-test-results-tab";

        private static string FormatUnfinishedHelixJob(HelixJobInfo helixJob)
        {
            string workItemCountText = helixJob.InitialWorkItemCount?.ToString() ?? "unknown";
            return $"{helixJob.DisplayName} [status={helixJob.Status}, initialWorkItems={workItemCountText}]{Environment.NewLine}  {helixJob.DetailsUri}";
        }

        private static string FormatInProgressPipelineJob(AzureDevOpsTimelineRecord timelineRecord)
        {
            string state = string.IsNullOrEmpty(timelineRecord.State) ? "unknown" : timelineRecord.State;
            string result = string.IsNullOrEmpty(timelineRecord.Result) ? "none" : timelineRecord.Result;
            string name = string.IsNullOrEmpty(timelineRecord.Name) ? timelineRecord.ReferenceName : timelineRecord.Name;
            if (string.IsNullOrEmpty(name))
            {
                name = timelineRecord.Identifier;
            }

            return $"{name} [state={state}, result={result}]";
        }

        private sealed record JobWorkItemStatusCounts(
            int ProcessedJobs, int ProcessedWorkItems,
            int CompletedJobs, int CompletedWorkItems,
            int RunningJobs, int RunningWorkItems,
            int WaitingJobs, int WaitingWorkItems);

        private sealed record PipelineJobStatusCounts(
            int Completed,
            int Running,
            int Waiting);
    }
}
