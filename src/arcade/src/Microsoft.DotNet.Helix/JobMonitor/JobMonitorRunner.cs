// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.DotNet.Helix.Client;
using Microsoft.DotNet.Helix.Client.Models;
using Microsoft.DotNet.Helix.JobMonitor.Models;
using Microsoft.Extensions.Logging;

namespace Microsoft.DotNet.Helix.JobMonitor
{
    /// <summary>
    /// Orchestrates the per-invocation lifecycle described in <c>JobMonitorRunner.Design.md</c>:
    /// one-shot retry pass, poll loop (with upload + outcome reconciliation per iteration),
    /// final summary on completion, and timeout/cancel handling. All heavy lifting
    /// (status logging, uploads, state) lives in dedicated helpers.
    /// </summary>
    internal sealed class JobMonitorRunner : IJobMonitorRunner, IDisposable
    {
        private const string AzdoWarningPrefix = "##vso[task.logissue type=warning]";

        private readonly JobMonitorOptions _options;
        private readonly ILogger _logger;
        private readonly IAzureDevOpsService _azdo;
        private readonly IHelixService _helix;
        private readonly Func<TimeSpan, CancellationToken, Task> _delayFunc;
        private readonly string _helixSource;

        private readonly MonitorState _state = new();
        private readonly StatusReporter _reporter;
        private readonly TestResultUploadQueue _uploads;

        /// <summary>
        /// Constructor for production use with real services.
        /// </summary>
        public JobMonitorRunner(JobMonitorOptions options, ILogger logger)
            : this(options,
                  logger,
                  new AzureDevOpsService(options, logger),
                  new HelixService(string.IsNullOrEmpty(options.HelixAccessToken)
                      ? ApiFactory.GetAnonymous(options.HelixBaseUri)
                      : ApiFactory.GetAuthenticated(options.HelixBaseUri, options.HelixAccessToken),
                  logger),
                  delayFunc: null)
        {
        }

        /// <summary>
        /// Constructor for testing with injected services.
        /// </summary>
        internal JobMonitorRunner(
            JobMonitorOptions options,
            ILogger logger,
            IAzureDevOpsService azdo,
            IHelixService helix,
            Func<TimeSpan, CancellationToken, Task> delayFunc)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _azdo = azdo ?? throw new ArgumentNullException(nameof(azdo));
            _helix = helix ?? throw new ArgumentNullException(nameof(helix));
            _delayFunc = delayFunc ?? Task.Delay;
            Directory.CreateDirectory(_options.WorkingDirectory);

            // The Helix submitter (JobSender) records each job's Source as
            //   {prefix}/{teamProject}/{repository}/{branch}
            // where prefix is derived from BUILD_REASON / SYSTEM_TEAMPROJECT. Mirror that
            // derivation here so the monitor's Job.ListAsync query returns the same set of
            // jobs regardless of whether the build is a PR, scheduled, manual, IndividualCI,
            // BatchedCI, or internal official run.
            _helixSource = HelixJobSource.Compute(
                _options.BuildReason,
                _options.TeamProject,
                $"{_options.Organization}/{_options.RepositoryName}",
                _options.SourceBranch);

            _reporter = new StatusReporter(_logger, _options, _helix, _state);
            _uploads = new TestResultUploadQueue(_logger, _options, _azdo, _helix, Delay);
        }

        public async Task<int> RunAsync(CancellationToken cancellationToken)
        {
            _reporter.LogMonitorStart();

            foreach (string job in await _azdo.GetProcessedHelixJobNamesAsync(cancellationToken))
            {
                _state.ProcessedHelixJobs.Add(job);
            }

            try
            {
                IReadOnlyList<HelixJobInfo> jobsForFirstPoll = await ExecuteRetryPassAsync(cancellationToken);
                return await RunPollLoopAsync(jobsForFirstPoll, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // On cancellation (AzDO job timeout or build cancellation) the agent grants only a
                // few seconds before force-killing the process, so cancelling the in-flight Helix
                // jobs is the priority: do it immediately rather than waiting for the test-result
                // upload queue to drain.
                //
                // Any in-flight uploads are intentionally abandoned. A Helix job's results are only
                // treated as "processed" once their Azure DevOps test run reaches the Completed
                // state (the final upload step), so a job whose upload did not finish here is
                // re-uploaded in full by a subsequent monitor invocation. See
                // JobMonitorRunner.Design.md ("Crash and timeout resilience").
                _reporter.ReportTimeout();

                // Proactively cancel any Helix jobs we know about that haven't finished yet so
                // they don't keep consuming queue capacity after the monitor exits. Bounded,
                // independent cancellation budget because the runner's own token is already
                // cancelled.
                using var cancelCts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                await CancelInFlightHelixJobsAsync(cancelCts.Token);

                return 1;
            }
        }

        /// <summary>
        /// One-shot retry pass executed on entry. Walks the current Helix snapshot, finds
        /// the latest completed incarnation in each lineage chain, and resubmits any failed
        /// work items on those jobs. Returns the (scoped snapshot ∪ resubmitted jobs) so
        /// the first poll iteration sees the resubmissions immediately.
        /// </summary>
        private async Task<IReadOnlyList<HelixJobInfo>> ExecuteRetryPassAsync(CancellationToken cancellationToken)
        {
            _reporter.LogRetryPassStart();

            IReadOnlyList<HelixJobInfo> scopedJobs =
            [
                ..(await _helix.GetJobsForBuildAsync(_helixSource, _options.BuildId, cancellationToken))
                    .Where(IsInScope)
            ];

            var resubmittedJobs = new List<HelixJobInfo>();

            foreach (HelixJobInfo completedJob in MonitorState.GetLatestHelixJobAttempts(scopedJobs)
                                                              .Where(j => j.IsCompleted))
            {
                IReadOnlyCollection<WorkItemSummary> failedWorkItems =
                [
                    ..(await _helix.ListWorkItemsAsync(completedJob.JobName, cancellationToken))
                        .Where(wi => wi.IsFailed)
                ];

                if (failedWorkItems.Count == 0)
                {
                    continue;
                }

                HelixJobInfo resubmitted = await _helix.ResubmitWorkItemsAsync(completedJob, failedWorkItems, cancellationToken);
                if (resubmitted is null)
                {
                    continue;
                }

                resubmittedJobs.Add(resubmitted);
                _state.ResubmittedJobCount++;
                _state.ResubmittedWorkItemCount += failedWorkItems.Count;
                if (!string.IsNullOrEmpty(completedJob.SubmitterJobName))
                {
                    _state.RetryingHelixSubmitterJobs.Add(completedJob.SubmitterJobName);
                }
            }

            if (resubmittedJobs.Count == 0)
            {
                _reporter.LogRetryPassFoundNothing();
            }

            return [.. scopedJobs, .. resubmittedJobs];
        }

        private async Task<int> RunPollLoopAsync(IReadOnlyList<HelixJobInfo> jobsForFirstPoll, CancellationToken cancellationToken)
        {
            var loopState = new PollLoopState();

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                int? exitCode = await PollOnceAsync(jobsForFirstPoll, loopState, cancellationToken);
                jobsForFirstPoll = null; // first-poll seed is consumed

                if (exitCode.HasValue)
                {
                    return exitCode.Value;
                }

                await Delay(cancellationToken);
            }
        }

        /// <summary>
        /// One iteration of the poll loop. Returns a non-null exit code when termination
        /// conditions are met (all pipeline + Helix work complete), otherwise null.
        /// </summary>
        private async Task<int?> PollOnceAsync(
            IReadOnlyList<HelixJobInfo> jobsForFirstPoll,
            PollLoopState loopState,
            CancellationToken cancellationToken)
        {
            // Fetch fresh snapshots, scoped to the monitor's stage.
            IReadOnlyList<AzureDevOpsTimelineRecord> timelineRecords =
                HelixJobMonitorUtilities.FilterRecordsToStage(
                    await _azdo.GetTimelineRecordsAsync(cancellationToken),
                    _options.StageName);

            IReadOnlyList<HelixJobInfo> scopedJobs =
            [
                ..(jobsForFirstPoll ?? await _helix.GetJobsForBuildAsync(_helixSource, _options.BuildId, cancellationToken))
                    .Where(IsInScope)
            ];

            _state.LatestTimelineRecords.Clear();
            _state.LatestTimelineRecords.AddRange(timelineRecords);
            _state.ObserveJobs(scopedJobs);

            // Helix job summaries can omit Finished for failed jobs even after all work
            // items have terminal exit codes, so fall back to per-work-item status.
            IReadOnlyCollection<HelixJobInfo> completedJobs = await GetCompletedJobsAsync(scopedJobs, cancellationToken);
            var completedJobNames = new HashSet<string>(
                completedJobs.Select(j => j.JobName),
                StringComparer.OrdinalIgnoreCase);

            // First pass: upload + reconcile for any newly-completed jobs.
            foreach (HelixJobInfo job in completedJobs.Where(j => !_state.ProcessedHelixJobs.Contains(j.JobName)))
            {
                await ReconcileCompletedJobAsync(job, queueUpload: true, cancellationToken);
                _state.ProcessedHelixJobs.Add(job.JobName);
                _state.ProcessedJobCount++;
            }

            // Second pass: ensure outcomes for every completed scoped job are reflected in
            // the running outcome map (oldest-incarnation first so newer ones supersede
            // older ones). Idempotent — already-reconciled jobs early-return.
            foreach (HelixJobInfo job in MonitorState.OrderHelixJobsOldToNew(
                MonitorState.GetLatestHelixJobAttempts(scopedJobs)
                    .Where(j => completedJobNames.Contains(j.JobName))))
            {
                await ReconcileCompletedJobAsync(job, queueUpload: false, cancellationToken);
            }

            _uploads.Prune();

            bool shouldLogStatus = _options.Verbose
                || loopState.LastObservedJobCount != scopedJobs.Count
                || loopState.LastObservedCompletedCount != completedJobs.Count
                || (DateTime.UtcNow - loopState.LastStatusLogAt) >= TimeSpan.FromMinutes(5);

            if (shouldLogStatus)
            {
                await _reporter.LogPollStatusAsync(scopedJobs, completedJobNames, cancellationToken);
                loopState.LastObservedJobCount = scopedJobs.Count;
                loopState.LastObservedCompletedCount = completedJobs.Count;
                loopState.LastStatusLogAt = DateTime.UtcNow;
            }

            bool anyNonMonitorFailure = HelixJobMonitorUtilities.HasFailedNonMonitorJobs(
                timelineRecords,
                _options.JobMonitorName,
                _state.RetryingHelixSubmitterJobs);
            bool allPipelineJobsComplete = HelixJobMonitorUtilities.AreNonMonitorJobsComplete(timelineRecords, _options.JobMonitorName);
            bool allHelixJobsComplete = scopedJobs.Count == 0 || scopedJobs.All(j => completedJobNames.Contains(j.JobName));

            if (!(allPipelineJobsComplete && allHelixJobsComplete))
            {
                return null;
            }

            await _uploads.DrainAsync(cancellationToken);
            _reporter.LogFinalFailedWorkItems();
            _reporter.LogFinalSummary(_state.AssociatedJobs.Count);

            if (anyNonMonitorFailure)
            {
                _reporter.LogNonMonitorPipelineFailure();
                return 1;
            }

            return _state.HasFailedWorkItem ? 1 : 0;
        }

        /// <summary>
        /// Updates the per-work-item outcome map for one completed Helix job and (optionally)
        /// queues a test-result upload. Idempotent: a second call without
        /// <paramref name="queueUpload"/> early-returns if the outcomes were already recorded.
        /// </summary>
        private async Task ReconcileCompletedJobAsync(
            HelixJobInfo helixJob,
            bool queueUpload,
            CancellationToken cancellationToken)
        {
            // Skip jobs whose results have already been uploaded — either earlier in this
            // invocation (tracked via WorkItemOutcomeJobs) or by a previous monitor attempt
            // for the same build (tracked via ProcessedHelixJobs, seeded on entry from the
            // AzDO test-run name markers). Without this, a retried monitor invocation
            // re-logs "Job X completed" and re-reports failed work-item console links for
            // every job the prior attempt already finished, which is noisy and misleading.
            if (_state.WorkItemOutcomeJobs.Contains(helixJob.JobName)
                || _state.ProcessedHelixJobs.Contains(helixJob.JobName))
            {
                return;
            }

            _reporter.LogJobProcessingStart(helixJob);

            IReadOnlyCollection<WorkItemSummary> workItems =
                await _helix.ListWorkItemsAsync(helixJob.JobName, cancellationToken);
            _reporter.LogFailedWorkItemConsoleLinks(helixJob, workItems.Where(wi => wi.IsFailed));

            if (_state.WorkItemOutcomeJobs.Add(helixJob.JobName))
            {
                string chainKey = _state.GetSubmitterChainKey(helixJob);
                var jobWorkItems = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (WorkItemSummary wi in workItems)
                {
                    // Within the same AzDO submitter chain (i.e. resubmissions of the same
                    // logical AzDO job), the latest result overwrites the prior one for the
                    // same work item name. Across different submitter chains the key differs,
                    // so identically-named work items in different AzDO jobs are tracked
                    // independently.
                    _state.WorkItemOutcomes[(chainKey, wi.Name)] = !wi.IsFailed;
                    _state.TrackFailedWorkItemConsoleInfo(helixJob, chainKey, wi);
                    jobWorkItems.Add(wi.Name);
                }
            }

            if (queueUpload)
            {
                _uploads.Enqueue(helixJob, workItems, cancellationToken);
            }

            _reporter.LogJobCompleted(helixJob, workItems);
        }

        private async Task<IReadOnlyCollection<HelixJobInfo>> GetCompletedJobsAsync(
            IReadOnlyList<HelixJobInfo> jobs,
            CancellationToken cancellationToken)
        {
            var completed = new List<HelixJobInfo>();
            foreach (HelixJobInfo job in jobs)
            {
                if (job.IsCompleted || await AreAllWorkItemsTerminalAsync(job, cancellationToken))
                {
                    completed.Add(job);
                }
            }

            return MonitorState.OrderHelixJobsOldToNew(completed);
        }

        private async Task<bool> AreAllWorkItemsTerminalAsync(HelixJobInfo job, CancellationToken cancellationToken)
        {
            if (job.InitialWorkItemCount is not > 0)
            {
                return false;
            }

            IReadOnlyCollection<WorkItemSummary> workItems = await _helix.ListWorkItemsAsync(job.JobName, cancellationToken);
            return workItems.Count >= job.InitialWorkItemCount.Value
                && workItems.All(wi => wi.ExitCode.HasValue);
        }

        private async Task CancelInFlightHelixJobsAsync(CancellationToken cancellationToken)
        {
            List<HelixJobInfo> inFlightJobs =
            [
                ..MonitorState.GetLatestHelixJobAttempts(_state.AssociatedJobs.Values)
                    .Where(j => !j.IsCompleted && !_state.ProcessedHelixJobs.Contains(j.JobName))
                    .OrderBy(j => j.JobName, StringComparer.OrdinalIgnoreCase)
            ];

            if (inFlightJobs.Count == 0)
            {
                return;
            }

            LogWarning($"Cancellation requested. Attempting to cancel {inFlightJobs.Count} in-flight Helix job(s).");

            await Task.WhenAll(inFlightJobs.Select(async job =>
            {
                try
                {
                    await _helix.CancelJobAsync(job.JobName, cancellationToken);
                    _logger.LogWarning("🛑 Requested cancellation of Helix job {JobName}.{nl}{JobUri}",
                        job.DisplayName, Environment.NewLine, job.DetailsUri);
                }
                catch (OperationCanceledException)
                {
                    // Bounded cancel window elapsed; nothing more to do.
                }
                catch (Exception ex)
                {
                    LogWarning(ex, $"Failed to cancel Helix job {job.DisplayName}.");
                }
            }));
        }

        private bool IsInScope(HelixJobInfo job)
            => string.IsNullOrEmpty(job.StageName)
                || string.Equals(job.StageName, _options.StageName, StringComparison.OrdinalIgnoreCase);

        public void Dispose()
        {
            (_azdo as IDisposable)?.Dispose();
            (_helix as IDisposable)?.Dispose();
        }

        private Task Delay(CancellationToken cancellationToken)
            => _delayFunc(TimeSpan.FromSeconds(Math.Max(5, _options.PollingIntervalSeconds)), cancellationToken);

        private void LogWarning(string message)
            => _logger.LogWarning("{Prefix}{Message}", AzdoWarningPrefix, message);

        private void LogWarning(Exception exception, string message)
            => _logger.LogWarning(exception, "{Prefix}{Message}", AzdoWarningPrefix, message);

        /// <summary>
        /// Mutable per-loop cursor used by <see cref="PollOnceAsync"/> to decide when to
        /// emit a status log line.
        /// </summary>
        private sealed class PollLoopState
        {
            public int LastObservedJobCount { get; set; } = -1;
            public int LastObservedCompletedCount { get; set; } = -1;
            public DateTime LastStatusLogAt { get; set; } = DateTime.UtcNow;
        }
    }
}
