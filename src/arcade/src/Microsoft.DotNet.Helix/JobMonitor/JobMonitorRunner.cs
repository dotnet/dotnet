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
            _uploads = new TestResultUploadQueue(_logger, _options, _azdo, _helix, _state, Delay);
        }

        public async Task<int> RunAsync(CancellationToken cancellationToken)
        {
            _reporter.LogMonitorStart();

            _state.AddProcessedHelixJobs(await _azdo.GetProcessedHelixJobNamesAsync(cancellationToken));

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
        /// One-shot retry pass executed on entry. Reconciles the whole stage's Helix work
        /// (all attempts) into the current stage attempt: for each logical work stream it takes
        /// the latest incarnation and, when that incarnation is a previous attempt's failed or
        /// unfinished work (or a current attempt's completed-with-failures work), resubmits the
        /// not-yet-passed items into the current attempt. Returns the (stage snapshot ∪
        /// resubmitted jobs) so the first poll iteration sees the resubmissions immediately.
        /// See JobMonitorRunner.Design.md §2.1 and §2.3.
        /// </summary>
        private async Task<IReadOnlyList<HelixJobInfo>> ExecuteRetryPassAsync(CancellationToken cancellationToken)
        {
            _reporter.LogRetryPassStart();

            // Discover the whole stage's work across ALL attempts (not just the current one),
            // so previous-attempt work can be reconciled into the current attempt rather than
            // silently dropped (§2.1). Attempt classification happens per work stream below.
            IReadOnlyList<HelixJobInfo> stageJobs =
            [
                ..(await _helix.GetJobsForBuildAsync(_helixSource, _options.BuildId, cancellationToken))
                    .Where(IsStageInScope)
            ];

            // Seed the cross-poll cache so submitter-chain-key lineage (PreviousHelixJobName
            // walks) resolves while grouping streams below.
            _state.ObserveJobs(stageJobs);

            // Surfacing work items that passed by exit code but whose AzDO test results contain
            // failures: a prior monitor invocation may have uploaded failed tests for a job
            // before being cancelled / crashing. Without this, the retry pass would only
            // resubmit work items whose Helix exit code was non-zero and would silently leave
            // test-only failures in place. The lookup is skipped (and a noop dictionary used)
            // when --fail-on-failed-tests is disabled so AzDO test results no longer influence
            // retry decisions.
            IReadOnlyDictionary<string, IReadOnlySet<string>> failedTestWorkItemsByJob =
                _options.FailWorkItemsWithFailedTests
                    ? await _azdo.GetFailedTestWorkItemsAsync(cancellationToken)
                    : new Dictionary<string, IReadOnlySet<string>>(StringComparer.OrdinalIgnoreCase);

            var resubmittedJobs = new List<HelixJobInfo>();

            foreach (HelixJobInfo latest in _state.GetLatestIncarnationPerStream(stageJobs))
            {
                bool previousAttempt = IsPreviousAttempt(latest);

                // A current-attempt incarnation that is still in flight is gated on, not
                // resubmitted (this also covers the fast-rerun case where a fresh current-attempt
                // incarnation exists while a previous one is still running — §2.3.1 case 4).
                if (!previousAttempt && !latest.IsCompleted)
                {
                    continue;
                }

                IReadOnlyCollection<WorkItemSummary> jobWorkItems =
                    await _helix.ListWorkItemsAsync(latest.JobName, cancellationToken);

                failedTestWorkItemsByJob.TryGetValue(latest.JobName, out IReadOnlySet<string> testFailedNames);

                // For a previous-attempt incarnation, IsFailed also captures still-unfinished
                // (Waiting/running) items — those are work the prior attempt abandoned that must
                // be driven to completion under the current attempt.
                IReadOnlyList<WorkItemSummary> exitCodeFailures = [..jobWorkItems.Where(wi => wi.IsFailed)];
                IReadOnlyList<WorkItemSummary> testOnlyFailures =
                [
                    ..jobWorkItems.Where(wi => !wi.IsFailed
                        && testFailedNames is not null
                        && testFailedNames.Contains(wi.Name))
                ];

                if (exitCodeFailures.Count + testOnlyFailures.Count == 0)
                {
                    // Nothing to resubmit: a fully-passed incarnation. Its results are uploaded
                    // and its outcome counted by the poll loop.
                    continue;
                }

                _reporter.LogRetryPassResubmission(latest, exitCodeFailures, testOnlyFailures);

                // exitCodeFailures and testOnlyFailures are disjoint by construction (one
                // requires IsFailed, the other !IsFailed), but guard against duplicate work
                // item names within a job so each item is only resubmitted once.
                IReadOnlyCollection<WorkItemSummary> failedWorkItems =
                [
                    ..exitCodeFailures
                        .Concat(testOnlyFailures)
                        .DistinctBy(wi => wi.Name, StringComparer.OrdinalIgnoreCase)
                ];

                HelixJobInfo resubmitted = await _helix.ResubmitWorkItemsAsync(
                    latest, failedWorkItems, _options.StageAttempt, cancellationToken);
                if (resubmitted is null)
                {
                    // Previous-attempt work that can never run again (e.g. its queue was removed)
                    // must not be silently dropped or waited on forever — record it as a hard
                    // failure so the monitor fails fast with actionable output (§2.3.1 case 6).
                    if (previousAttempt)
                    {
                        _state.RecordAbandonedWork(latest, failedWorkItems);
                        LogWarning(
                            $"Could not resubmit {failedWorkItems.Count} unfinished/failed work item(s) from a "
                            + $"previous attempt of {latest.DisplayName} (e.g. its Helix queue may have been removed). "
                            + "Marking them as failed so this build does not wait indefinitely.");
                    }

                    continue;
                }

                resubmittedJobs.Add(resubmitted);
                _state.RecordResubmission(latest.SubmitterJobName, failedWorkItems.Count);
            }

            if (resubmittedJobs.Count == 0)
            {
                _reporter.LogRetryPassFoundNothing();
            }

            return [.. stageJobs, .. resubmittedJobs];
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

            // The full set of the stage's Helix work across ALL attempts. Uploads and outcome
            // reconciliation consider all of it (previous-attempt results still belong to this
            // build), but completion gating considers only the current attempt (below).
            IReadOnlyList<HelixJobInfo> stageJobs =
            [
                ..(jobsForFirstPoll ?? await _helix.GetJobsForBuildAsync(_helixSource, _options.BuildId, cancellationToken))
                    .Where(IsStageInScope)
            ];

            // Current-attempt work (including this invocation's resubmissions, which are stamped
            // with the current attempt). Completion is gated on exactly this set: previous-attempt
            // work has either been superseded by a current incarnation, resubmitted into the
            // current attempt, or is already terminal — it must never block termination (§2.1).
            IReadOnlyList<HelixJobInfo> currentAttemptJobs =
            [
                ..stageJobs.Where(IsStageAttemptInScope)
            ];

            _state.SetTimelineRecords(timelineRecords);
            _state.ObserveJobs(stageJobs);

            // Helix job summaries can omit Finished for failed jobs even after all work
            // items have terminal exit codes, so fall back to per-work-item status.
            IReadOnlyCollection<HelixJobInfo> completedJobs = await GetCompletedJobsAsync(stageJobs, cancellationToken);
            var completedJobNames = new HashSet<string>(
                completedJobs.Select(j => j.JobName),
                StringComparer.OrdinalIgnoreCase);

            // First pass: upload + reconcile for any newly-completed jobs.
            foreach (HelixJobInfo job in completedJobs.Where(j => !_state.IsHelixJobProcessed(j.JobName)))
            {
                await ReconcileCompletedJobAsync(job, queueUpload: true, cancellationToken);
                _state.TryMarkHelixJobProcessed(job.JobName);
            }

            // Second pass: ensure outcomes for every completed job (any attempt) are reflected in
            // the running outcome map (oldest-incarnation first, then lower stage attempt first,
            // so newer incarnations — including higher-attempt rerun duplicates — supersede older
            // ones). Idempotent — already-reconciled jobs early-return.
            foreach (HelixJobInfo job in MonitorState.OrderHelixJobsOldToNew(
                MonitorState.GetLatestHelixJobAttempts(stageJobs)
                    .Where(j => completedJobNames.Contains(j.JobName))))
            {
                await ReconcileCompletedJobAsync(job, queueUpload: false, cancellationToken);
            }

            _uploads.Prune();

            bool shouldLogStatus = _options.Verbose
                || loopState.LastObservedJobCount != stageJobs.Count
                || loopState.LastObservedCompletedCount != completedJobs.Count
                || (DateTime.UtcNow - loopState.LastStatusLogAt) >= TimeSpan.FromMinutes(5);

            if (shouldLogStatus)
            {
                await _reporter.LogPollStatusAsync(stageJobs, completedJobNames, cancellationToken);
                loopState.LastObservedJobCount = stageJobs.Count;
                loopState.LastObservedCompletedCount = completedJobs.Count;
                loopState.LastStatusLogAt = DateTime.UtcNow;
            }

            bool anyNonMonitorFailure = HelixJobMonitorUtilities.HasFailedNonMonitorJobs(
                timelineRecords,
                _options.JobMonitorName,
                _state.SnapshotRetryingHelixSubmitterJobs());
            bool allPipelineJobsComplete = HelixJobMonitorUtilities.AreNonMonitorJobsComplete(timelineRecords, _options.JobMonitorName);
            bool allHelixJobsComplete = currentAttemptJobs.Count == 0 || currentAttemptJobs.All(j => completedJobNames.Contains(j.JobName));

            if (!(allPipelineJobsComplete && allHelixJobsComplete))
            {
                return null;
            }

            await _uploads.DrainAsync(cancellationToken);
            _reporter.LogFinalFailedWorkItems();
            _reporter.LogFinalSummary(_state.AssociatedJobsCount);

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
            // Already reconciled earlier in this invocation — nothing more to do (idempotent).
            if (_state.IsWorkItemOutcomesRecorded(helixJob.JobName))
            {
                return;
            }

            IReadOnlyCollection<WorkItemSummary> workItems =
                await _helix.ListWorkItemsAsync(helixJob.JobName, cancellationToken);

            // A previous monitor attempt for the same build already uploaded this job's results
            // (tracked via IsHelixJobProcessed, seeded on entry from the AzDO test-run tags). Its
            // work-item outcomes must still be reconciled so the final exit code accounts for
            // previously-uploaded failures that were never resubmitted — otherwise the monitor
            // could exit 0 despite a prior failed job. Only the re-upload and the noisy
            // completion / console-link logs are suppressed for such jobs.
            bool alreadyUploadedByPriorAttempt = _state.IsHelixJobProcessed(helixJob.JobName);

            if (!alreadyUploadedByPriorAttempt)
            {
                _reporter.LogJobProcessingStart(helixJob);
                _reporter.LogFailedWorkItemConsoleLinks(helixJob, workItems.Where(wi => wi.IsFailed));
            }

            _state.TryRecordWorkItemOutcomes(helixJob, workItems);

            if (queueUpload && !alreadyUploadedByPriorAttempt)
            {
                _uploads.Enqueue(helixJob, workItems, cancellationToken);
            }

            if (!alreadyUploadedByPriorAttempt)
            {
                _reporter.LogJobCompleted(helixJob, workItems);
            }
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
                ..MonitorState.GetLatestHelixJobAttempts(_state.SnapshotAssociatedJobs())
                    .Where(j => !j.IsCompleted && !_state.IsHelixJobProcessed(j.JobName))
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

        private bool IsStageInScope(HelixJobInfo job)
            => string.IsNullOrEmpty(job.StageName)
                || string.Equals(job.StageName, _options.StageName, StringComparison.OrdinalIgnoreCase);

        // Per-attempt scoping for completion gating: a job is "current attempt" when the
        // monitor's own attempt is unknown (build+stage back-compat), the job's attempt is
        // unknown (older jobs / non-stage submissions), or the two match. Only current-attempt
        // work gates termination (§2.1).
        private bool IsStageAttemptInScope(HelixJobInfo job)
            => string.IsNullOrEmpty(_options.StageAttempt)
                || string.IsNullOrEmpty(job.StageAttempt)
                || string.Equals(job.StageAttempt, _options.StageAttempt, StringComparison.OrdinalIgnoreCase);

        // A job belongs to a strictly-earlier attempt of this stage than the monitor's own.
        // Such work is reconciled into the current attempt by the retry pass (§2.3) but never
        // gated on directly. Requires both attempts to be known; unknown attempts are treated
        // as current (see IsStageAttemptInScope).
        private bool IsPreviousAttempt(HelixJobInfo job)
            => !string.IsNullOrEmpty(_options.StageAttempt)
                && !string.IsNullOrEmpty(job.StageAttempt)
                && MonitorState.ParseStageAttempt(job.StageAttempt) < MonitorState.ParseStageAttempt(_options.StageAttempt);

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
