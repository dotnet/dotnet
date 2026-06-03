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
    internal sealed class JobMonitorRunner : IJobMonitorRunner, IDisposable
    {
        private readonly JobMonitorOptions _options;
        private readonly ILogger _logger;
        private readonly IAzureDevOpsService _azdo;
        private readonly IHelixService _helix;
        private readonly Func<TimeSpan, CancellationToken, Task> _delayFunc;
        private readonly string _helixSource;

        /// <summary>
        /// Tracks the latest outcome for each logical work item, keyed by
        /// (SubmitterChainKey, WorkItemName). Using the submitter chain key (rather than just
        /// the work-item name) ensures that two different AzDO jobs which happen to run
        /// identically-named Helix work items do not overwrite each other's outcomes. Within
        /// a single AzDO submitter chain, a resubmission still overwrites a prior failure
        /// for the same work-item name because resubmitted Helix jobs inherit
        /// <c>System.JobName</c>.
        /// </summary>
        private readonly Dictionary<(string ChainKey, string WorkItemName), bool> _workItemOutcomes = new(WorkItemOutcomeKeyComparer.Instance);
        private readonly HashSet<string> _workItemOutcomeJobs = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Cache of every Helix job we have seen this run, indexed by job name, so that
        /// <see cref="GetSubmitterChainKey"/> can walk back through <c>PreviousHelixJobName</c>
        /// links when a submitter job name is not present on the job (e.g. unit-test
        /// scenarios that submit Helix jobs without an AzDO submitter context).
        /// </summary>
        private readonly Dictionary<string, HelixJobInfo> _knownJobsByName = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Tracks which work item names belong to which Helix job, so resubmission only
        /// resubmits items from the specific source job.
        /// </summary>
        private readonly Dictionary<string, HashSet<string>> _workItemsByJob = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<(string ChainKey, string WorkItemName), FailedWorkItemConsoleInfo> _failedWorkItemConsoleInfo = new(WorkItemOutcomeKeyComparer.Instance);
        private readonly HashSet<string> _reportedFailedWorkItemConsoleLinks = new(StringComparer.OrdinalIgnoreCase);
        private readonly List<Task> _pendingTestResultUploads = [];

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
        }

        public Task<int> RunAsync(CancellationToken cancellationToken)
        {
            return RunCoreAsync(cancellationToken);
        }

        private async Task<int> RunCoreAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Monitoring Helix jobs for stage {stage} of build {BuildId}:{nl}{collectionUri}{teamProject}/_build/results?buildId={BuildId}",
                _options.StageName,
                _options.BuildId,
                Environment.NewLine,
                _options.CollectionUri,
                _options.TeamProject,
                _options.BuildId);

            IReadOnlySet<string> alreadyProcessed = await _azdo.GetProcessedHelixJobNamesAsync(cancellationToken);
            HashSet<string> processedHelixJobs = new(alreadyProcessed, StringComparer.OrdinalIgnoreCase);
            // Keyed by Helix job name so that each poll overwrites the cached snapshot with the
            // latest state from Helix. Using a HashSet here would retain the first-seen snapshot
            // (typically Status="running", Finished=null), which would cause ReportTimeout and
            // CancelInFlightHelixJobsAsync to treat already-completed jobs as still in flight.
            Dictionary<string, HelixJobInfo> associatedJobs = new(StringComparer.OrdinalIgnoreCase);

            try
            {
                JobResubmissionResult jobResubmission = await ResubmitFailedJobsAsync(cancellationToken);

                return await RunLoopAsync(
                    processedHelixJobs,
                    associatedJobs,
                    jobResubmission,
                    cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Drain in-flight test-result uploads before exiting. The uploads were started
                // via Task.Run and are not bound to the runner's cancellation token; if we
                // return without awaiting them, any results that hadn't reached AzDO yet are
                // lost (and tests that observe UploadedJobNames immediately after a cancelled
                // run see a partial set).
                await WaitForPendingTestResultUploadsAsync(CancellationToken.None);
                ReportTimeout(associatedJobs.Values, processedHelixJobs);

                // On cancellation (typically the overall timeout), proactively cancel any
                // Helix jobs we know about that haven't finished yet so they don't keep
                // consuming queue capacity after the monitor exits. We use a short, bounded
                // timeout that is independent of the runner's cancellation token (which is
                // already cancelled) so the best-effort cancel calls actually run.
                using (var cancelCts = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
                {
                    await CancelInFlightHelixJobsAsync(associatedJobs.Values, processedHelixJobs, cancelCts.Token);
                }

                return 1;
            }
        }

        private async Task CancelInFlightHelixJobsAsync(
            IEnumerable<HelixJobInfo> associatedJobs,
            HashSet<string> processedHelixJobs,
            CancellationToken cancellationToken)
        {
            List<HelixJobInfo> inFlightJobs =
            [
                ..GetLatestHelixJobAttempts(associatedJobs)
                    .Where(j => !j.IsCompleted && !processedHelixJobs.Contains(j.JobName))
                    .OrderBy(j => j.JobName, StringComparer.OrdinalIgnoreCase)
            ];

            if (inFlightJobs.Count == 0)
            {
                return;
            }

            _logger.LogWarning("Cancellation requested. Attempting to cancel {Count} in-flight Helix job(s).", inFlightJobs.Count);

            Task[] cancelTasks =
            [
                ..inFlightJobs.Select(async job =>
                {
                    try
                    {
                        await _helix.CancelJobAsync(job.JobName, cancellationToken);
                        _logger.LogInformation("🛑 Requested cancellation of Helix job {JobName}.{nl}{JobUri}",
                            job.DisplayName, Environment.NewLine, job.DetailsUri);
                    }
                    catch (OperationCanceledException)
                    {
                        // Bounded cancel window elapsed; nothing more to do.
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to cancel Helix job {JobName}.", job.DisplayName);
                    }
                })
            ];

            await Task.WhenAll(cancelTasks);
        }

        private async Task<int> RunLoopAsync(
            HashSet<string> processedHelixJobs,
            Dictionary<string, HelixJobInfo> associatedJobs,
            JobResubmissionResult jobResubmission,
            CancellationToken cancellationToken)
        {
            bool anyNonMonitorJobFailures = false;
            int processedHelixJobCount = 0;
            int allHelixJobCount = 0;
            int completedJobsCount = -1;
            DateTime lastPrintTime = DateTime.UtcNow;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                IReadOnlyList<AzureDevOpsTimelineRecord> timelineRecords = await _azdo.GetTimelineRecordsAsync(cancellationToken);
                IReadOnlyList<HelixJobInfo> associatedJobsWithBuild = jobResubmission.JobsForFirstPoll ?? await _helix.GetJobsForBuildAsync(
                    _helixSource,
                    _options.BuildId,
                    cancellationToken);
                jobResubmission = jobResubmission with { JobsForFirstPoll = null };

                // Drop timeline records and Helix jobs that belong to other stages so they don't
                // gate completion or contribute failures.
                timelineRecords = HelixJobMonitorUtilities.FilterRecordsToStage(timelineRecords, _options.StageName);
                associatedJobsWithBuild =
                [
                    ..associatedJobsWithBuild.Where(j =>
                        string.IsNullOrEmpty(j.StageName)
                        || string.Equals(j.StageName, _options.StageName, StringComparison.OrdinalIgnoreCase))
                ];

                // Overwrite per poll so the cached entry reflects the latest Helix-side state
                // (in particular, the Finished timestamp transitioning from null to a value).
                foreach (HelixJobInfo j in associatedJobsWithBuild)
                {
                    associatedJobs[j.JobName] = j;
                }

                // Cache every job we have seen so GetSubmitterChainKey can follow the
                // PreviousHelixJobName chain back to a root, even across polls.
                foreach (HelixJobInfo j in associatedJobsWithBuild)
                {
                    _knownJobsByName[j.JobName] = j;
                }

                // Filter jobs to completed ones belonging to this build. Helix job summaries can
                // omit Finished for failed jobs even after all work items have terminal exit codes.
                IReadOnlyCollection<HelixJobInfo> completedJobs =
                    await GetCompletedHelixJobsAsync(associatedJobsWithBuild, cancellationToken);
                var completedJobNames = new HashSet<string>(
                    completedJobs.Select(j => j.JobName),
                    StringComparer.OrdinalIgnoreCase);

                bool shouldLogStatus = _options.Verbose
                    || allHelixJobCount != associatedJobsWithBuild.Count
                    || completedJobsCount != completedJobs.Count
                    || (DateTime.UtcNow - lastPrintTime) >= TimeSpan.FromMinutes(5);

                foreach (HelixJobInfo job in completedJobs.Where(j => !processedHelixJobs.Contains(j.JobName)))
                {
                    await ProcessCompletedJobAsync(job, uploadTestResults: true, cancellationToken);
                    processedHelixJobs.Add(job.JobName);
                    processedHelixJobCount++;
                }

                foreach (HelixJobInfo job in OrderHelixJobsOldToNew(GetLatestHelixJobAttempts(associatedJobsWithBuild).Where(j => completedJobNames.Contains(j.JobName))))
                {
                    await ProcessCompletedJobAsync(job, uploadTestResults: false, cancellationToken);
                }

                PruneCompletedTestResultUploads();

                if (shouldLogStatus)
                {
                    await LogHelixJobStatusAsync(associatedJobsWithBuild, completedJobNames, processedHelixJobs, cancellationToken);
                    allHelixJobCount = associatedJobsWithBuild.Count;
                    completedJobsCount = completedJobs.Count;
                    lastPrintTime = DateTime.UtcNow;
                }

                anyNonMonitorJobFailures = HelixJobMonitorUtilities.HasFailedNonMonitorJobs(
                    timelineRecords,
                    _options.JobMonitorName,
                    jobResubmission.RetryingHelixSubmitterJobs);
                bool allPipelineJobsComplete = HelixJobMonitorUtilities.AreNonMonitorJobsComplete(timelineRecords, _options.JobMonitorName);
                bool allHelixJobsComplete = associatedJobsWithBuild.Count == 0 || associatedJobsWithBuild.All(j => completedJobNames.Contains(j.JobName));

                if (allPipelineJobsComplete && allHelixJobsComplete)
                {
                    await WaitForPendingTestResultUploadsAsync(cancellationToken);

                    LogFinalFailedWorkItemConsoleInfo();

                    bool anyWorkItemFailed = _workItemOutcomes.Values.Any(passed => !passed);
                    int totalWorkItems = _workItemOutcomes.Count;
                    int failedWorkItems = _workItemOutcomes.Values.Count(passed => !passed);
                    _logger.LogInformation(
                        "📊 Final summary:{nl}"
                      + "   Jobs:       {TotalJobs} submitted / {ResubmittedJobs} resubmitted / {ProcessedJobs} processed{nl}"
                      + "   Work items: {TotalWorkItems} submitted / {ResubmittedWorkItems} resubmitted / {FailedWorkItems} failed",
                        Environment.NewLine,
                        associatedJobs.Count,
                        jobResubmission.ResubmittedJobCount,
                        processedHelixJobCount,
                        Environment.NewLine,
                        totalWorkItems,
                        jobResubmission.ResubmittedWorkItemCount,
                        failedWorkItems);

                    if (anyNonMonitorJobFailures)
                    {
                        _logger.LogError("One or more non-monitor pipeline jobs failed.");
                        return 1;
                    }

                    if (anyWorkItemFailed)
                    {
                        return 1;
                    }

                    return 0;
                }

                await Delay(cancellationToken);
            }
        }

        private async Task<IReadOnlyCollection<HelixJobInfo>> GetCompletedHelixJobsAsync(
            IReadOnlyList<HelixJobInfo> jobs,
            CancellationToken cancellationToken)
        {
            var completedJobs = new List<HelixJobInfo>();

            foreach (HelixJobInfo job in jobs)
            {
                if (job.IsCompleted || await AreAllWorkItemsTerminalAsync(job, cancellationToken))
                {
                    completedJobs.Add(job);
                }
            }

            return OrderHelixJobsOldToNew(completedJobs);
        }

        private async Task LogHelixJobStatusAsync(
            IReadOnlyList<HelixJobInfo> jobs,
            HashSet<string> completedJobNames,
            HashSet<string> processedJobNames,
            CancellationToken cancellationToken)
        {
            List<HelixJobInfo> orderedJobs =
            [
                ..jobs
                    .OrderBy(job => job.JobName, StringComparer.OrdinalIgnoreCase)
            ];
            var workItemsByJob = new Dictionary<string, IReadOnlyCollection<WorkItemSummary>>(StringComparer.OrdinalIgnoreCase);

            foreach (HelixJobInfo job in orderedJobs)
            {
                IReadOnlyCollection<WorkItemSummary> workItems = await _helix.ListWorkItemsAsync(job.JobName, cancellationToken);
                LogFailedWorkItemConsoleLinks(job, [..workItems.Where(IsFailedWorkItem)]);
                workItemsByJob[job.JobName] = workItems;
            }

            JobWorkItemStatusCounts counts = GetStatusCounts(orderedJobs, workItemsByJob, completedJobNames, processedJobNames);
            _logger.LogInformation(
                "ℹ️ Status: {ProcessedJobs} processed / {CompletedJobs} completed / {RunningJobs} running / {WaitingJobs} waiting jobs{nl}"
              + "           {ProcessedWorkItems} processed / {CompletedWorkItems} completed / {RunningWorkItems} running / {WaitingWorkItems} waiting work items",
                counts.ProcessedJobs,
                counts.CompletedJobs,
                counts.RunningJobs,
                counts.WaitingJobs,
                Environment.NewLine,
                counts.ProcessedWorkItems,
                counts.CompletedWorkItems,
                counts.RunningWorkItems,
                counts.WaitingWorkItems);

            if (_options.Verbose)
            {
                LogVerboseHelixJobStatus(orderedJobs, workItemsByJob, completedJobNames, processedJobNames);
            }
        }

        private static JobWorkItemStatusCounts GetStatusCounts(
            IReadOnlyList<HelixJobInfo> jobs,
            IReadOnlyDictionary<string, IReadOnlyCollection<WorkItemSummary>> workItemsByJob,
            HashSet<string> completedJobNames,
            HashSet<string> processedJobNames)
        {
            int processedJobs = 0;
            int processedWorkItems = 0;
            int completedJobs = 0;
            int completedWorkItems = 0;
            int runningJobs = 0;
            int runningWorkItems = 0;
            int waitingJobs = 0;
            int waitingWorkItems = 0;

            foreach (HelixJobInfo job in jobs)
            {
                IReadOnlyCollection<WorkItemSummary> workItems = workItemsByJob[job.JobName];

                if (processedJobNames.Contains(job.JobName))
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
                    runningJobs++;
                    runningWorkItems += workItems.Count;
                }
                else
                {
                    waitingJobs++;
                    waitingWorkItems += job.InitialWorkItemCount ?? 0;
                }
            }

            return new JobWorkItemStatusCounts(
                processedJobs,
                processedWorkItems,
                completedJobs,
                completedWorkItems,
                runningJobs,
                runningWorkItems,
                waitingJobs,
                waitingWorkItems);
        }

        private void LogVerboseHelixJobStatus(
            IReadOnlyList<HelixJobInfo> jobs,
            IReadOnlyDictionary<string, IReadOnlyCollection<WorkItemSummary>> workItemsByJob,
            HashSet<string> completedJobNames,
            HashSet<string> processedJobNames)
        {
            if (jobs.Count == 0)
            {
                _logger.LogInformation("⏳ Helix job details:{nl}└─ no Helix jobs discovered yet", Environment.NewLine);
                return;
            }

            var lines = new List<string>();
            for (int jobIndex = 0; jobIndex < jobs.Count; jobIndex++)
            {
                HelixJobInfo job = jobs[jobIndex];
                IReadOnlyCollection<WorkItemSummary> workItems = workItemsByJob[job.JobName];
                AddVerboseJobLines(lines, job, workItems, GetJobStatus(job, workItems, completedJobNames, processedJobNames), isLastJob: jobIndex == jobs.Count - 1);
            }

            _logger.LogInformation("⏳ Helix job details:{nl}{JobDetails}",
                Environment.NewLine,
                string.Join(Environment.NewLine, lines));
        }

        private static void AddVerboseJobLines(
            List<string> lines,
            HelixJobInfo job,
            IReadOnlyCollection<WorkItemSummary> workItems,
            string jobStatus,
            bool isLastJob)
        {
            string jobConnector = isLastJob ? "└─" : "├─";
            string childPrefix = isLastJob ? "   " : "│  ";
            lines.Add($"{jobConnector} 🧪 Helix job {job.DisplayName} [{jobStatus}]");

            List<WorkItemSummary> orderedWorkItems =
            [
                ..workItems.OrderBy(wi => wi.Name, StringComparer.OrdinalIgnoreCase)
            ];

            if (orderedWorkItems.Count == 0)
            {
                lines.Add($"{childPrefix}└─ no work items reported yet");
                return;
            }

            for (int workItemIndex = 0; workItemIndex < orderedWorkItems.Count; workItemIndex++)
            {
                WorkItemSummary workItem = orderedWorkItems[workItemIndex];
                string workItemConnector = workItemIndex == orderedWorkItems.Count - 1 ? "└─" : "├─";
                string console = IsFailedWorkItem(workItem)
                    ? $" | Console: {GetConsoleOutputText(workItem.ConsoleOutputUri)}"
                    : string.Empty;
                lines.Add($"{childPrefix}{workItemConnector} {workItem.Name} ({FormatWorkItemState(workItem)}){console}");
            }
        }

        private static string GetJobStatus(
            HelixJobInfo job,
            IReadOnlyCollection<WorkItemSummary> workItems,
            HashSet<string> completedJobNames,
            HashSet<string> processedJobNames)
        {
            if (processedJobNames.Contains(job.JobName))
            {
                return "Processed";
            }

            if (completedJobNames.Contains(job.JobName))
            {
                return "Completed";
            }

            return workItems.Count > 0 ? "Running" : "Waiting";
        }

        private static string FormatWorkItemState(WorkItemSummary workItem)
        {
            string exitCode = workItem.ExitCode.HasValue ? $", exit code {workItem.ExitCode.Value}" : string.Empty;
            return $"{workItem.State}{exitCode}";
        }

        private async Task<bool> AreAllWorkItemsTerminalAsync(
            HelixJobInfo job,
            CancellationToken cancellationToken)
        {
            if (job.InitialWorkItemCount is not > 0)
            {
                return false;
            }

            IReadOnlyCollection<WorkItemSummary> workItems = await _helix.ListWorkItemsAsync(job.JobName, cancellationToken);
            return workItems.Count >= job.InitialWorkItemCount.Value
                && workItems.All(wi => wi.ExitCode.HasValue);
        }

        private async Task ProcessCompletedJobAsync(
            HelixJobInfo helixJob,
            bool uploadTestResults,
            CancellationToken cancellationToken)
        {
            if (!uploadTestResults && _workItemOutcomeJobs.Contains(helixJob.JobName))
            {
                return;
            }

            _logger.LogInformation("Job {JobName} completed. Processing test results...{nl}{JobUri}", helixJob.DisplayName, Environment.NewLine, helixJob.DetailsUri);

            IReadOnlyCollection<WorkItemSummary> workItems = await _helix.ListWorkItemsAsync(helixJob.JobName, cancellationToken);
            LogFailedWorkItemConsoleLinks(helixJob, [..workItems.Where(wi => wi.IsFailed)]);

            // Update per-work-item outcome tracking
            if (_workItemOutcomeJobs.Add(helixJob.JobName))
            {
                string chainKey = GetSubmitterChainKey(helixJob);
                var jobWorkItems = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (WorkItemSummary wi in workItems)
                {
                    // Within the same AzDO submitter chain (i.e. resubmissions of the same
                    // logical AzDO job), the latest result overwrites the prior one for the
                    // same work item name. Across different submitter chains the key differs,
                    // so identically-named work items in different AzDO jobs are tracked
                    // independently.
                    _workItemOutcomes[(chainKey, wi.Name)] = !wi.IsFailed;
                    TrackFailedWorkItemConsoleInfo(helixJob, chainKey, wi);
                    jobWorkItems.Add(wi.Name);
                }

                _workItemsByJob[helixJob.JobName] = jobWorkItems;
            }

            int failedWorkItemCount = workItems.Count(wi => wi.IsFailed);
            int successfulWorkItemCount = workItems.Count - failedWorkItemCount;

            if (uploadTestResults)
            {
                QueueTestResultUpload(helixJob, workItems, cancellationToken);
            }

            _logger.LogInformation("{Icon} Job '{JobName}' {Status} ({PassedCount} passed, {FailedCount} failed){nl}{JobUri}",
                failedWorkItemCount == 0 ? "✅" : "❌",
                helixJob.DisplayName,
                failedWorkItemCount == 0 ? "succeeded" : "failed",
                successfulWorkItemCount,
                failedWorkItemCount,
                Environment.NewLine, helixJob.DetailsUri);
        }

        private void QueueTestResultUpload(
            HelixJobInfo helixJob,
            IReadOnlyCollection<WorkItemSummary> workItems,
            CancellationToken cancellationToken)
        {
            IReadOnlyList<string> workItemNames = [.. workItems.Select(w => w.Name)];
            Task uploadTask = Task.Run(
                () => UploadTestResultsForJobAsync(helixJob, workItemNames, cancellationToken),
                CancellationToken.None);
            _pendingTestResultUploads.Add(uploadTask);
        }

        private async Task UploadTestResultsForJobAsync(
            HelixJobInfo helixJob,
            IReadOnlyCollection<string> workItemNames,
            CancellationToken cancellationToken)
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                int testRunId = 0;

                try
                {
                    testRunId = await _azdo.CreateTestRunAsync(helixJob.TestRunName, helixJob.JobName, cancellationToken);
                    IReadOnlyList<WorkItemTestResults> downloadedFiles = await _helix.DownloadTestResultsAsync(
                        helixJob.JobName,
                        workItemNames,
                        _options.WorkingDirectory,
                        cancellationToken);

                    int uploadedCount = await _azdo.UploadTestResultsAsync(testRunId, downloadedFiles, cancellationToken);
                    await CompleteUploadedTestRunAsync(testRunId, helixJob, cancellationToken);

                    _logger.LogInformation("{UploadedCount} test results for job '{JobName}' processed.",
                        uploadedCount,
                        helixJob.DisplayName);
                    return;
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogDebug(ex,
                        "Failed to upload test results for job {JobName} to Azure DevOps. Test run ID was {TestRunId}. Retrying after delay.",
                        helixJob.DisplayName,
                        testRunId);
                    await Delay(cancellationToken);
                }
            }
        }

        private async Task CompleteUploadedTestRunAsync(
            int testRunId,
            HelixJobInfo helixJob,
            CancellationToken cancellationToken)
        {
            while (true)
            {
                try
                {
                    await _azdo.CompleteTestRunAsync(testRunId, cancellationToken);
                    return;
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogDebug(ex,
                        "Failed to complete Azure DevOps test run {TestRunId} for job {JobName}. Retrying after delay.",
                        testRunId,
                        helixJob.JobName);
                    await Delay(cancellationToken);
                }
            }
        }

        private void PruneCompletedTestResultUploads()
        {
            _pendingTestResultUploads.RemoveAll(static task => task.IsCompleted);
        }

        private async Task WaitForPendingTestResultUploadsAsync(CancellationToken cancellationToken)
        {
            PruneCompletedTestResultUploads();
            if (_pendingTestResultUploads.Count == 0)
            {
                return;
            }

            // When test uploads are the last thing running, we want to speed it up
            // Rate limiting should be dealt with by retry logic in the upload tasks
            _options.TestResultUploadParallelism = 16;

            _logger.LogInformation("Waiting for {Count} pending test result upload(s) to complete.", _pendingTestResultUploads.Count);
            await Task.WhenAll(_pendingTestResultUploads);
            PruneCompletedTestResultUploads();
        }

        private void LogFailedWorkItemConsoleLinks(HelixJobInfo helixJob, IReadOnlyCollection<WorkItemSummary> workItems)
        {
            foreach (WorkItemSummary workItem in workItems.OrderBy(wi => wi.Name, StringComparer.OrdinalIgnoreCase))
            {
                if (!_reportedFailedWorkItemConsoleLinks.Add(GetWorkItemKey(helixJob.JobName, workItem.Name)))
                {
                    continue;
                }

                _logger.LogInformation("❌ Work item '{WorkItemName}' in job '{JobName}' failed ({State}).{nl}Console: {ConsoleOutputUri}",
                    workItem.Name,
                    helixJob.DisplayName,
                    FormatWorkItemState(workItem),
                    Environment.NewLine,
                    GetConsoleOutputText(workItem.ConsoleOutputUri));
            }
        }

        private static bool IsUnfinishedWorkItem(WorkItemSummary workItem)
            => !workItem.ExitCode.HasValue
                && !string.Equals(workItem.State, "Finished", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(workItem.State, "Failed", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(workItem.State, "TimedOut", StringComparison.OrdinalIgnoreCase);

        private static bool IsFailedWorkItem(WorkItemSummary workItem)
            => workItem.IsFailed && !IsUnfinishedWorkItem(workItem);

        private sealed record JobWorkItemStatusCounts(
            int ProcessedJobs,
            int ProcessedWorkItems,
            int CompletedJobs,
            int CompletedWorkItems,
            int RunningJobs,
            int RunningWorkItems,
            int WaitingJobs,
            int WaitingWorkItems);

        private static string GetWorkItemKey(string jobName, string workItemName)
            => $"{jobName}/{workItemName}";

        private void TrackFailedWorkItemConsoleInfo(HelixJobInfo helixJob, string chainKey, WorkItemSummary workItem)
        {
            var key = (chainKey, workItem.Name);
            if (workItem.IsFailed)
            {
                _failedWorkItemConsoleInfo[key] = new FailedWorkItemConsoleInfo(
                    helixJob.DisplayName,
                    workItem.Name,
                    FormatWorkItemState(workItem),
                    GetConsoleOutputText(workItem.ConsoleOutputUri));
            }
            else
            {
                _failedWorkItemConsoleInfo.Remove(key);
            }
        }

        /// <summary>
        /// Produces a key that rolls up work-item outcomes within a logical AzDO submitter
        /// chain. When the job carries an AzDO <c>System.JobName</c>, the chain key is based
        /// on that name (so resubmissions of the same AzDO job share the same key while two
        /// independent AzDO jobs running identically-named work items stay distinct). When
        /// there is no submitter name (test scenarios, manual Helix submissions), the chain
        /// is followed back through <c>PreviousHelixJobName</c> links to the root and the
        /// root Helix job name is used instead, so that retries still overwrite prior
        /// failures correctly.
        /// </summary>
        private string GetSubmitterChainKey(HelixJobInfo job)
        {
            if (!string.IsNullOrEmpty(job.SubmitterJobName))
            {
                return $"submitter:{job.SubmitterJobName}";
            }

            HelixJobInfo current = job;
            var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            while (current is not null
                && !string.IsNullOrEmpty(current.PreviousHelixJobName)
                && visited.Add(current.JobName))
            {
                if (!_knownJobsByName.TryGetValue(current.PreviousHelixJobName, out HelixJobInfo previous))
                {
                    return $"helix:{current.PreviousHelixJobName}";
                }

                if (!string.IsNullOrEmpty(previous.SubmitterJobName))
                {
                    return $"submitter:{previous.SubmitterJobName}";
                }

                current = previous;
            }

            return $"helix:{(current?.JobName ?? job.JobName)}";
        }

        private void LogFinalFailedWorkItemConsoleInfo()
        {
            if (_failedWorkItemConsoleInfo.Count == 0)
            {
                return;
            }

            List<FailedWorkItemConsoleInfo> failures =
            [
                .._failedWorkItemConsoleInfo.Values
                    .OrderBy(failure => failure.WorkItemName, StringComparer.OrdinalIgnoreCase)
            ];

            var lines = new List<string>();
            for (int i = 0; i < failures.Count; i++)
            {
                FailedWorkItemConsoleInfo failure = failures[i];
                string connector = i == failures.Count - 1 ? "└─" : "├─";
                lines.Add($"{connector} {failure.WorkItemName} (Job: {failure.JobName}) ({failure.State})");
                lines.Add($"{(i == failures.Count - 1 ? "   " : "│  ")}└─ Console: {failure.ConsoleOutput}");
            }

            _logger.LogError("❌ Failed work item console logs:{nl}Test results: {TestResultsUri}{nl}{FailedWorkItemConsoleLogs}",
                Environment.NewLine,
                GetTestResultsUri(),
                Environment.NewLine,
                string.Join(Environment.NewLine, lines));
        }

        private string GetTestResultsUri()
            => $"{_options.CollectionUri}{_options.TeamProject}/_build/results?buildId={_options.BuildId}&view=ms.vss-test-web.build-test-results-tab";

        private static string GetConsoleOutputText(string consoleOutputUri)
            => string.IsNullOrEmpty(consoleOutputUri) ? "no console link available" : consoleOutputUri;

        private async Task<JobResubmissionResult> ResubmitFailedJobsAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("🔁 Checking for failed Helix jobs to resubmit the failed work items...");

            var retryingHelixSubmitterJobs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var resubmittedJobs = new List<HelixJobInfo>();
            int resubmittedWorkItemCount = 0;

            // This snapshot is taken when the monitor starts. Failed latest work items here are
            // not retried again until the monitor starts again, even if they fail during this run.
            IReadOnlyList<HelixJobInfo> allJobs = await _helix.GetJobsForBuildAsync(_helixSource, _options.BuildId, cancellationToken);
            IReadOnlyList<HelixJobInfo> scopedJobs =
            [
                ..allJobs.Where(IsHelixJobInScope)
            ];
            IReadOnlyList<HelixJobInfo> latestJobs = GetLatestHelixJobAttempts(scopedJobs);
            List<HelixJobInfo> completedHelixJobs =
            [
                ..latestJobs.Where(j => j.IsCompleted && IsHelixJobInScope(j))
            ];

            foreach (HelixJobInfo completedJob in completedHelixJobs)
            {
                IReadOnlyCollection<WorkItemSummary> workItems = await _helix.ListWorkItemsAsync(completedJob.JobName, cancellationToken);
                IReadOnlyCollection<WorkItemSummary> failedWorkItems = [..workItems.Where(wi => wi.IsFailed)];

                if (failedWorkItems.Count > 0)
                {
                    HelixJobInfo resubmittedJob = await _helix.ResubmitWorkItemsAsync(completedJob, failedWorkItems, cancellationToken);
                    if (resubmittedJob != null)
                    {
                        resubmittedJobs.Add(resubmittedJob);
                        resubmittedWorkItemCount += failedWorkItems.Count;

                        if (!string.IsNullOrEmpty(completedJob.SubmitterJobName))
                        {
                            retryingHelixSubmitterJobs.Add(completedJob.SubmitterJobName);
                        }
                    }
                }
            }

            if (resubmittedJobs.Count == 0)
            {
                _logger.LogInformation("No failed jobs found to resubmit");
            }

            return new JobResubmissionResult(
                retryingHelixSubmitterJobs,
                [..scopedJobs, ..resubmittedJobs],
                resubmittedJobs.Count,
                resubmittedWorkItemCount);
        }

        private bool IsHelixJobInScope(HelixJobInfo job)
        {
            return string.IsNullOrEmpty(job.StageName)
                || string.Equals(job.StageName, _options.StageName, StringComparison.OrdinalIgnoreCase);
        }

        private static IReadOnlyList<HelixJobInfo> GetLatestHelixJobAttempts(IEnumerable<HelixJobInfo> jobs)
        {
            var supersededJobNames = new HashSet<string>(
                jobs
                    .Select(j => j.PreviousHelixJobName)
                    .Where(previousJobName => !string.IsNullOrEmpty(previousJobName)),
                StringComparer.OrdinalIgnoreCase);

            return
            [
                ..jobs.Where(j => !supersededJobNames.Contains(j.JobName))
            ];
        }

        private static IReadOnlyList<HelixJobInfo> OrderHelixJobsOldToNew(IEnumerable<HelixJobInfo> jobs)
        {
            var jobByName = jobs.ToDictionary(j => j.JobName, StringComparer.OrdinalIgnoreCase);
            return
            [
                ..jobs
                    .OrderBy(j => GetHelixJobLineageDepth(j, jobByName))
                    .ThenBy(j => j.JobName, StringComparer.OrdinalIgnoreCase)
            ];
        }

        private static int GetHelixJobLineageDepth(HelixJobInfo job, Dictionary<string, HelixJobInfo> jobByName)
        {
            int depth = 0;
            var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            while (!string.IsNullOrEmpty(job.PreviousHelixJobName)
                && visited.Add(job.PreviousHelixJobName)
                && jobByName.TryGetValue(job.PreviousHelixJobName, out job))
            {
                depth++;
            }

            return depth;
        }

        private sealed record JobResubmissionResult(
            HashSet<string> RetryingHelixSubmitterJobs,
            IReadOnlyList<HelixJobInfo> JobsForFirstPoll,
            int ResubmittedJobCount,
            int ResubmittedWorkItemCount);

        private sealed record FailedWorkItemConsoleInfo(
            string JobName,
            string WorkItemName,
            string State,
            string ConsoleOutput);

        private static string FormatChainKeyForDisplay(string chainKey)
        {
            const string submitterPrefix = "submitter:";
            const string helixPrefix = "helix:";
            if (chainKey is null) return string.Empty;
            if (chainKey.StartsWith(submitterPrefix, StringComparison.Ordinal))
            {
                return chainKey.Substring(submitterPrefix.Length);
            }

            if (chainKey.StartsWith(helixPrefix, StringComparison.Ordinal))
            {
                return chainKey.Substring(helixPrefix.Length);
            }

            return chainKey;
        }

        private sealed class WorkItemOutcomeKeyComparer : IEqualityComparer<(string ChainKey, string WorkItemName)>
        {
            public static readonly WorkItemOutcomeKeyComparer Instance = new();

            public bool Equals((string ChainKey, string WorkItemName) x, (string ChainKey, string WorkItemName) y)
                => StringComparer.OrdinalIgnoreCase.Equals(x.ChainKey, y.ChainKey)
                    && StringComparer.OrdinalIgnoreCase.Equals(x.WorkItemName, y.WorkItemName);

            public int GetHashCode((string ChainKey, string WorkItemName) obj)
                => HashCode.Combine(
                    obj.ChainKey is null ? 0 : StringComparer.OrdinalIgnoreCase.GetHashCode(obj.ChainKey),
                    obj.WorkItemName is null ? 0 : StringComparer.OrdinalIgnoreCase.GetHashCode(obj.WorkItemName));
        }

        private void ReportTimeout(
            IEnumerable<HelixJobInfo> latestAssociatedJobs,
            HashSet<string> processedHelixJobs)
        {
            var timeout = TimeSpan.FromMinutes(_options.MaximumWaitMinutes);
            var unfinishedJobs = latestAssociatedJobs
                .Where(j => !j.IsCompleted || !processedHelixJobs.Contains(j.JobName))
                .OrderBy(j => j.JobName, StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (unfinishedJobs.Count == 0)
            {
                _logger.LogCritical("Helix Job Monitor timed out after {TimeoutMinutes} minute(s) ({Timeout}). No unfinished Helix jobs were tracked at the time of timeout.",
                    timeout.TotalMinutes,
                    timeout);
                return;
            }

            _logger.LogError(
                $"Helix Job Monitor timed out after {{TimeoutMinutes}} minute(s) ({{Timeout}}). {{UnfinishedCount}} Helix job(s) had not finished:{Environment.NewLine}- {{UnfinishedJobs}}{Environment.NewLine}",
                timeout.TotalMinutes,
                timeout,
                unfinishedJobs.Count,
                string.Join(Environment.NewLine + "- ", unfinishedJobs.Select(j => $"{j.DetailsUri} ({j.QueueId})")));
        }

        public void Dispose()
        {
            (_azdo as IDisposable)?.Dispose();
            (_helix as IDisposable)?.Dispose();
        }

        private Task Delay(CancellationToken cancellationToken)
            => _delayFunc(TimeSpan.FromSeconds(Math.Max(5, _options.PollingIntervalSeconds)), cancellationToken);
    }
}

static file class WorkItemExtensions
{
    extension(WorkItemSummary workItem)
    {
        public bool IsFailed => workItem.ExitCode != 0 || !workItem.State.Equals("Finished", StringComparison.OrdinalIgnoreCase);
    }
}
