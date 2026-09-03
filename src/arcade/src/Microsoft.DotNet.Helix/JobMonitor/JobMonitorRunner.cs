// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.DotNet.Helix.AzureDevOpsTestPublisher;
using Microsoft.DotNet.Helix.Client;
using Microsoft.DotNet.Helix.Client.Models;
using Microsoft.DotNet.Helix.JobMonitor.Models;
using Microsoft.DotNet.Helix.JobMonitor.Parallelism;
using Microsoft.Extensions.Logging;

namespace Microsoft.DotNet.Helix.JobMonitor
{
    /// <summary>
    /// Orchestrates the per-invocation lifecycle described in <c>Design/SemanticBehavior.md</c>:
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
        private readonly Func<TimeSpan, CancellationToken, Task> _statusDelayFunc;
        private readonly string _helixSource;

        private readonly MonitorState _state = new();
        private readonly JobMonitorMetrics _metrics;
        private readonly StatusReporter _reporter;
        private readonly TestResultUploadPipeline _uploads;
        private PollStatusSnapshot _latestStatus;

        /// <summary>
        /// Constructor for production use with real services.
        /// </summary>
        public JobMonitorRunner(JobMonitorOptions options, ILogger logger)
            : this(options, logger, CreateProductionDependencies(options, logger))
        {
        }

        private JobMonitorRunner(
            JobMonitorOptions options,
            ILogger logger,
            ProductionDependencies dependencies)
            : this(
                options,
                logger,
                dependencies.AzureDevOps,
                dependencies.Helix,
                delayFunc: null,
                statusDelayFunc: null,
                metrics: dependencies.Metrics,
                resultProcessor: dependencies.ResultProcessor,
                resultPublisher: dependencies.ResultPublisher)
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
            Func<TimeSpan, CancellationToken, Task> delayFunc,
            Func<TimeSpan, CancellationToken, Task> statusDelayFunc = null,
            JobMonitorMetrics metrics = null,
            ITestResultProcessor resultProcessor = null,
            IAzureDevOpsResultPublisher resultPublisher = null)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _azdo = azdo ?? throw new ArgumentNullException(nameof(azdo));
            _helix = helix ?? throw new ArgumentNullException(nameof(helix));
            _delayFunc = delayFunc ?? Task.Delay;
            _statusDelayFunc = statusDelayFunc ?? Task.Delay;
            _metrics = metrics ?? new JobMonitorMetrics();
            Directory.CreateDirectory(_options.WorkingDirectory);

            _helixSource = HelixJobSource.Compute(
                _options.BuildReason,
                _options.TeamProject,
                _options.Organization,
                _options.RepositoryName,
                _options.SourceBranch);

            _reporter = new StatusReporter(_logger, _options, _state);
            _uploads = new TestResultUploadPipeline(
                _logger,
                _options,
                _azdo,
                resultProcessor
                    ?? azdo as ITestResultProcessor
                    ?? throw new ArgumentException(
                        "A test result processor must be provided.",
                        nameof(resultProcessor)),
                resultPublisher
                    ?? azdo as IAzureDevOpsResultPublisher
                    ?? throw new ArgumentException(
                        "An Azure DevOps result publisher must be provided.",
                        nameof(resultPublisher)),
                _helix,
                _state,
                _metrics);
        }

        public async Task<int> RunAsync(CancellationToken cancellationToken)
        {
            _reporter.LogMonitorStart();

            _state.AddProcessedHelixJobs(await _azdo.GetProcessedHelixJobNamesAsync(cancellationToken));

            using var statusCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task statusTask = ReportStatusPeriodicallyAsync(statusCts.Token);
            try
            {
                IReadOnlyList<AzureDevOpsTimelineRecord> timelineForFirstPoll =
                    HelixJobMonitorUtilities.FilterRecordsToStage(
                        await _azdo.GetTimelineRecordsAsync(cancellationToken),
                        _options.StageName);
                IReadOnlyList<HelixJobInfo> jobsForFirstPoll =
                    await ExecuteRetryPassAsync(timelineForFirstPoll, cancellationToken);
                return await RunPollLoopAsync(jobsForFirstPoll, timelineForFirstPoll, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                _uploads.Cancel();
                // On cancellation (AzDO job timeout or build cancellation) the agent grants only a
                // few seconds before force-killing the process, so cancelling the in-flight Helix
                // jobs is the priority: do it immediately rather than waiting for the test-result
                // upload queue to drain.
                //
                // Any in-flight uploads are intentionally abandoned. A Helix job's results are only
                // treated as "processed" once their Azure DevOps test run reaches the Completed
                // state (the final upload step), so a job whose upload did not finish here is
                // re-uploaded in full by a subsequent monitor invocation. See
                // Design/SemanticBehavior.md ("Crash and timeout resilience").
                _reporter.ReportTimeout();

                // Proactively cancel any Helix jobs we know about that haven't finished yet so
                // they don't keep consuming queue capacity after the monitor exits. Bounded,
                // independent cancellation budget because the runner's own token is already
                // cancelled.
                using var cancelCts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                await CancelInFlightHelixJobsAsync(cancelCts.Token);
                _reporter.LogPerformanceMetrics(
                    _metrics.Snapshot(),
                    _uploads.Snapshot,
                    isPartial: true);

                return 1;
            }
            finally
            {
                statusCts.Cancel();
                try
                {
                    await statusTask;
                }
                catch (OperationCanceledException) when (statusCts.IsCancellationRequested)
                {
                }
            }
        }

        /// <summary>
        /// One-shot retry pass executed on entry. Reconciles the whole stage's Helix work
        /// (all attempts) into the current stage attempt: for each logical work stream it takes
        /// the latest incarnation and, when that incarnation is a previous attempt's failed or
        /// unfinished work whose submitter did not rerun, resubmits the not-yet-passed items into
        /// the current attempt. Returns the (stage snapshot ∪
        /// resubmitted jobs) so the first poll iteration sees the resubmissions immediately.
        /// See Design/SemanticBehavior.md §2.1 and §2.3.
        /// </summary>
        private async Task<IReadOnlyList<HelixJobInfo>> ExecuteRetryPassAsync(
            IReadOnlyList<AzureDevOpsTimelineRecord> timelineRecords,
            CancellationToken cancellationToken)
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

            // Seed the cross-poll cache so PreviousHelixJobName walks resolve to the root Helix
            // job while grouping streams below.
            _state.ObserveJobs(stageJobs);
            _state.SetTimelineRecords(timelineRecords);

            // The initial monitor invocation observes and reports failures; it does not create
            // additional Helix work. Retry reconciliation is only meaningful after AzDO has
            // retried the monitor job.
            if (MonitorState.ParseJobAttempt(_options.JobAttempt) <= 1)
            {
                _reporter.LogRetryPassFoundNothing();
                return stageJobs;
            }

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

                // Work created in this stage attempt belongs to the current execution, whether
                // it is running or has already failed. It is observed, not retried again on entry.
                if (!previousAttempt)
                {
                    continue;
                }

                bool hasSubmitterAttempt = TryGetCurrentSubmitterAttempt(
                    latest,
                    timelineRecords,
                    out int currentSubmitterAttempt,
                    out string submitterIdentity);
                int helixSubmitterAttempt = MonitorState.ParseJobAttempt(latest.JobAttempt);

                // The submitter itself reran. Its old Helix work is superseded even when the new
                // Helix job has not become visible yet; resubmitting here would duplicate a full
                // stage rerun or a selected failed-job retry.
                if (IsSupersededBySubmitterRerun(
                    latest,
                    hasSubmitterAttempt,
                    currentSubmitterAttempt,
                    helixSubmitterAttempt))
                {
                    _state.MarkSupersededBySubmitterRerun(latest.JobName);
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

                if (!hasSubmitterAttempt
                    || string.IsNullOrEmpty(latest.JobAttempt)
                    || currentSubmitterAttempt < helixSubmitterAttempt)
                {
                    IReadOnlyCollection<WorkItemSummary> ambiguousWork =
                    [
                        ..exitCodeFailures
                            .Concat(testOnlyFailures)
                            .DistinctBy(wi => wi.Name, StringComparer.OrdinalIgnoreCase)
                    ];
                    _state.RecordAbandonedWork(latest, ambiguousWork);
                    LogWarning(
                        $"Cannot safely reconcile {ambiguousWork.Count} failed/unfinished work item(s) from "
                        + $"{latest.DisplayName}: submitter '{submitterIdentity ?? "<unknown>"}' could not be "
                        + $"matched to compatible System.JobAttempt metadata in the current timeline. "
                        + "The work was not resubmitted to avoid duplicating a rerun.");
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
                    latest, failedWorkItems, _options.StageAttempt, _options.JobAttempt, cancellationToken);
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
                _state.RecordResubmission(
                    latest.SubmitterPhaseName ?? latest.SubmitterJobName,
                    failedWorkItems.Count);
            }

            if (resubmittedJobs.Count == 0)
            {
                _reporter.LogRetryPassFoundNothing();
            }

            return [.. stageJobs, .. resubmittedJobs];
        }

        private async Task<int> RunPollLoopAsync(
            IReadOnlyList<HelixJobInfo> jobsForFirstPoll,
            IReadOnlyList<AzureDevOpsTimelineRecord> timelineForFirstPoll,
            CancellationToken cancellationToken)
        {
            var loopState = new PollLoopState();

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                int? exitCode = await PollOnceAsync(
                    jobsForFirstPoll,
                    timelineForFirstPoll,
                    loopState,
                    cancellationToken);
                jobsForFirstPoll = null; // first-poll seed is consumed
                timelineForFirstPoll = null;

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
            IReadOnlyList<AzureDevOpsTimelineRecord> timelineForFirstPoll,
            PollLoopState loopState,
            CancellationToken cancellationToken)
        {
            int pollNumber = ++loopState.PollNumber;

            // Fetch fresh snapshots, scoped to the monitor's stage.
            IReadOnlyList<AzureDevOpsTimelineRecord> timelineRecords =
                timelineForFirstPoll
                ?? HelixJobMonitorUtilities.FilterRecordsToStage(
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
            // work has either been resubmitted into the current attempt or is already terminal —
            // it must never block termination (§2.1).
            IReadOnlyList<HelixJobInfo> currentAttemptJobs =
            [
                ..stageJobs.Where(IsStageAttemptInScope)
            ];

            _state.SetTimelineRecords(timelineRecords);
            _state.ObserveJobs(stageJobs);

            // A rerun submitter supersedes its previous Helix stream before the replacement
            // Helix job is necessarily visible. Keep those stale incarnations available for
            // durable upload, but exclude them from current status and pass/fail reconciliation.
            IReadOnlyList<HelixJobInfo> authoritativeJobs =
            [
                .._state.GetLatestIncarnationPerStream(stageJobs)
                    .Where(job => !_state.IsSupersededBySubmitterRerun(job.JobName))
            ];
            var authoritativeJobNames = new HashSet<string>(
                authoritativeJobs.Select(static job => job.JobName),
                StringComparer.OrdinalIgnoreCase);

            // Helix job summaries can omit Finished for failed jobs even after all work
            // items have terminal exit codes, so fall back to per-work-item status.
            IReadOnlyList<HelixJobInfo> jobsToRefresh =
            [
                ..stageJobs.Where(job =>
                    !_state.IsWorkItemOutcomesRecorded(job.JobName)
                    || !loopState.WorkItemsByJob.ContainsKey(job.JobName))
            ];
            IReadOnlyDictionary<string, IReadOnlyCollection<WorkItemSummary>> refreshedWorkItems =
                await GetWorkItemsAsync(jobsToRefresh, cancellationToken);
            int newlyTerminalWorkItems = 0;
            foreach ((string jobName, IReadOnlyCollection<WorkItemSummary> workItems) in refreshedWorkItems)
            {
                int previousTerminalCount =
                    loopState.WorkItemsByJob.TryGetValue(jobName, out IReadOnlyCollection<WorkItemSummary> previousWorkItems)
                        ? previousWorkItems.Count(static item => item.ExitCode.HasValue)
                        : 0;
                int currentTerminalCount = workItems.Count(static item => item.ExitCode.HasValue);
                // A Helix work-item exit code is immutable once assigned, so terminal counts
                // increase monotonically without retaining or rebuilding per-item identity sets.
                newlyTerminalWorkItems += Math.Max(0, currentTerminalCount - previousTerminalCount);
                loopState.WorkItemsByJob[jobName] = workItems;
            }

            IReadOnlyDictionary<string, IReadOnlyCollection<WorkItemSummary>> workItemsByJob =
                stageJobs.ToDictionary(
                    static job => job.JobName,
                    job => loopState.WorkItemsByJob[job.JobName],
                    StringComparer.OrdinalIgnoreCase);
            IReadOnlyCollection<HelixJobInfo> completedJobs = GetCompletedJobs(stageJobs, workItemsByJob);
            var completedJobNames = new HashSet<string>(
                completedJobs.Select(j => j.JobName),
                StringComparer.OrdinalIgnoreCase);

            // Upload terminal work items as soon as they are visible, even while their Helix job
            // is still running. Large jobs can finish hundreds of work items long before their
            // final job transition; deferring those uploads creates an avoidable final drain.
            foreach (HelixJobInfo job in stageJobs.Where(job => !_state.IsHelixJobProcessed(job.JobName)))
            {
                _uploads.TryEnqueue(
                    job,
                    workItemsByJob[job.JobName],
                    completedJobNames.Contains(job.JobName),
                    pollNumber);
            }

            // First pass: reconcile outcomes for any newly-completed jobs.
            foreach (HelixJobInfo job in completedJobs.Where(j => !_state.IsHelixJobProcessed(j.JobName)))
            {
                ReconcileCompletedJob(
                    job,
                    workItemsByJob[job.JobName],
                    queueUpload: false,
                    recordOutcomes: authoritativeJobNames.Contains(job.JobName),
                    discoveryPoll: pollNumber);
            }

            // Second pass: ensure outcomes for every completed job (any attempt) are reflected in
            // the running outcome map (oldest incarnation first, so linked resubmissions
            // supersede their predecessors). Idempotent — already-reconciled jobs early-return.
            foreach (HelixJobInfo job in MonitorState.OrderHelixJobsOldToNew(
                MonitorState.GetLatestHelixJobAttempts(authoritativeJobs)
                    .Where(j => completedJobNames.Contains(j.JobName))))
            {
                ReconcileCompletedJob(
                    job,
                    workItemsByJob[job.JobName],
                    queueUpload: false,
                    recordOutcomes: true,
                    discoveryPoll: pollNumber);
            }

            var authoritativeCompletedJobNames = new HashSet<string>(
                authoritativeJobNames.Where(completedJobNames.Contains),
                StringComparer.OrdinalIgnoreCase);
            Volatile.Write(
                ref _latestStatus,
                new PollStatusSnapshot(
                    authoritativeJobs,
                    workItemsByJob,
                    authoritativeCompletedJobNames,
                    timelineRecords));
            if (!loopState.HasLoggedInitialStatus)
            {
                LogLatestStatus();
                loopState.HasLoggedInitialStatus = true;
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

            await _uploads.DrainAsync(pollNumber, newlyTerminalWorkItems, cancellationToken);
            _reporter.LogPerformanceMetrics(_metrics.Snapshot(), _uploads.Snapshot);
            _reporter.LogFinalFailedWorkItems();
            _reporter.LogFinalSummary(_state.AssociatedJobsCount);

            if (anyNonMonitorFailure)
            {
                _reporter.LogNonMonitorPipelineFailure();
                return 1;
            }

            if (_state.AssociatedJobsCount == 0 && !_options.AllowNoHelixJobs)
            {
                _reporter.LogNoHelixJobsFailure();
                return 1;
            }

            return _state.HasFailedWorkItem ? 1 : 0;
        }

        /// <summary>
        /// Updates the per-work-item outcome map for one completed Helix job and (optionally)
        /// queues a test-result upload. Idempotent: a second call without
        /// <paramref name="queueUpload"/> early-returns if the outcomes were already recorded.
        /// </summary>
        private void ReconcileCompletedJob(
            HelixJobInfo helixJob,
            IReadOnlyCollection<WorkItemSummary> workItems,
            bool queueUpload,
            bool recordOutcomes,
            int discoveryPoll)
        {
            // Already reconciled earlier in this invocation — nothing more to do (idempotent).
            if (_state.IsWorkItemOutcomesRecorded(helixJob.JobName))
            {
                return;
            }

            // A previous monitor attempt for the same build already uploaded this job's results
            // (tracked via IsHelixJobProcessed, seeded on entry from the AzDO test-run tags). Its
            // work-item outcomes must still be reconciled so the final exit code accounts for
            // previously-uploaded failures that were never resubmitted — otherwise the monitor
            // could exit 0 despite a prior failed job. Only the re-upload and the noisy
            // completion / console-link logs are suppressed for such jobs.
            bool alreadyUploadedByPriorAttempt = _state.IsHelixJobProcessed(helixJob.JobName);

            if (!alreadyUploadedByPriorAttempt && recordOutcomes)
            {
                _reporter.LogJobProcessingStart(helixJob);
                _reporter.LogFailedWorkItemConsoleLinks(helixJob, workItems.Where(wi => wi.IsFailed));
            }

            if (recordOutcomes)
            {
                _state.TryRecordWorkItemOutcomes(helixJob, workItems);
            }
            else
            {
                _state.MarkWorkItemOutcomesIgnored(helixJob.JobName);
            }

            if (queueUpload && !alreadyUploadedByPriorAttempt)
            {
                _uploads.TryEnqueue(
                    helixJob,
                    workItems,
                    isJobComplete: true,
                    discoveryPoll: discoveryPoll);
            }

            if (!alreadyUploadedByPriorAttempt && recordOutcomes)
            {
                _reporter.LogJobCompleted(helixJob, workItems);
            }
        }

        private IReadOnlyCollection<HelixJobInfo> GetCompletedJobs(
            IReadOnlyList<HelixJobInfo> jobs,
            IReadOnlyDictionary<string, IReadOnlyCollection<WorkItemSummary>> workItemsByJob)
        {
            var completed = new List<HelixJobInfo>();
            foreach (HelixJobInfo job in jobs)
            {
                if (job.IsCompleted || AreAllWorkItemsTerminal(job, workItemsByJob[job.JobName]))
                {
                    completed.Add(job);
                }
            }

            return MonitorState.OrderHelixJobsOldToNew(completed);
        }

        private static bool AreAllWorkItemsTerminal(
            HelixJobInfo job,
            IReadOnlyCollection<WorkItemSummary> workItems)
        {
            if (job.InitialWorkItemCount is not > 0)
            {
                return false;
            }

            return workItems.Count >= job.InitialWorkItemCount.Value
                && workItems.All(wi => wi.ExitCode.HasValue);
        }

        private Task<IReadOnlyDictionary<string, IReadOnlyCollection<WorkItemSummary>>> GetWorkItemsAsync(
            IReadOnlyList<HelixJobInfo> jobs,
            CancellationToken cancellationToken)
            => ParallelAsync.ToDictionaryAsync(
                jobs,
                parallelism: Math.Max(8, _options.TestResultUploadParallelism),
                static job => job.JobName,
                async (job, token) => await _helix.ListWorkItemsAsync(job.JobName, token),
                StringComparer.OrdinalIgnoreCase,
                cancellationToken);

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

        private static bool TryGetCurrentSubmitterAttempt(
            HelixJobInfo job,
            IReadOnlyList<AzureDevOpsTimelineRecord> timelineRecords,
            out int attempt,
            out string submitterIdentity)
        {
            // SendHelixJob stamps System.PhaseName from the AzDO phase refName. The nested
            // job record frequently has refName "__default", so only use it when phase
            // identity was not available on the Helix job.
            string timelineRecordType;
            if (!string.IsNullOrEmpty(job.SubmitterPhaseName))
            {
                submitterIdentity = job.SubmitterPhaseName;
                timelineRecordType = "Phase";
            }
            else
            {
                submitterIdentity = job.SubmitterJobName;
                timelineRecordType = "Job";
            }

            attempt = 0;
            if (string.IsNullOrEmpty(submitterIdentity))
            {
                return false;
            }

            string identity = submitterIdentity;
            AzureDevOpsTimelineRecord[] matchingRecords =
            [
                ..timelineRecords
                    .Where(record =>
                        string.Equals(record.Type, timelineRecordType, StringComparison.OrdinalIgnoreCase)
                        && string.Equals(
                            record.ReferenceName,
                            identity,
                            StringComparison.OrdinalIgnoreCase))
            ];

            // A phase refName is normally unique. Job fallback can be "__default" for many
            // matrix legs; treating any one of those as the submitter could duplicate work.
            if (matchingRecords.Length != 1)
            {
                return false;
            }

            attempt = matchingRecords[0].Attempt;
            return true;
        }

        private static bool IsSupersededBySubmitterRerun(
            HelixJobInfo job,
            bool hasSubmitterAttempt,
            int currentSubmitterAttempt,
            int helixSubmitterAttempt)
            => hasSubmitterAttempt
                && !string.IsNullOrEmpty(job.JobAttempt)
                && currentSubmitterAttempt > helixSubmitterAttempt;

        private static ProductionDependencies CreateProductionDependencies(
            JobMonitorOptions options,
            ILogger logger)
        {
            var metrics = new JobMonitorMetrics();
            var azureDevOps = new AzureDevOpsService(options, logger, metrics);
            var resultProcessor = new TestResultProcessor(
                options.TestResultAttachmentMode,
                options.UseFullyQualifiedTestName,
                logger,
                metrics);
            var resultPublisher = new AzureDevOpsResultPublisher(
                logger,
                options.UseFullyQualifiedTestName,
                azureDevOps,
                metrics);
            var helix = new HelixService(
                string.IsNullOrEmpty(options.HelixAccessToken)
                    ? ApiFactory.GetAnonymous(options.HelixBaseUri)
                    : ApiFactory.GetAuthenticated(options.HelixBaseUri, options.HelixAccessToken),
                logger,
                metrics);
            return new ProductionDependencies(
                azureDevOps,
                resultProcessor,
                resultPublisher,
                helix,
                metrics);
        }

        public void Dispose()
        {
            _uploads.Cancel();
            (_azdo as IDisposable)?.Dispose();
            (_helix as IDisposable)?.Dispose();
        }

        private Task Delay(CancellationToken cancellationToken)
            => _delayFunc(TimeSpan.FromSeconds(Math.Max(5, _options.PollingIntervalSeconds)), cancellationToken);

        private async Task ReportStatusPeriodicallyAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                await _statusDelayFunc(TimeSpan.FromMinutes(5), cancellationToken);
                LogLatestStatus();
            }
        }

        private void LogLatestStatus()
        {
            PollStatusSnapshot snapshot = Volatile.Read(ref _latestStatus);
            if (snapshot is null)
            {
                return;
            }

            _reporter.LogPollStatus(
                snapshot.Jobs,
                snapshot.WorkItemsByJob,
                snapshot.CompletedJobNames,
                snapshot.TimelineRecords,
                _uploads.Snapshot);
        }

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
            public int PollNumber { get; set; }
            public bool HasLoggedInitialStatus { get; set; }
            public Dictionary<string, IReadOnlyCollection<WorkItemSummary>> WorkItemsByJob { get; } =
                new(StringComparer.OrdinalIgnoreCase);
        }

        private sealed record PollStatusSnapshot(
            IReadOnlyList<HelixJobInfo> Jobs,
            IReadOnlyDictionary<string, IReadOnlyCollection<WorkItemSummary>> WorkItemsByJob,
            IReadOnlySet<string> CompletedJobNames,
            IReadOnlyList<AzureDevOpsTimelineRecord> TimelineRecords);

        private sealed record ProductionDependencies(
            IAzureDevOpsService AzureDevOps,
            ITestResultProcessor ResultProcessor,
            IAzureDevOpsResultPublisher ResultPublisher,
            IHelixService Helix,
            JobMonitorMetrics Metrics);
    }
}
