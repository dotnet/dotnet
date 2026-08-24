// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Arcade.Common;
using Microsoft.DotNet.Helix.AzureDevOpsTestPublisher;
using Microsoft.DotNet.Helix.Client.Models;
using Microsoft.DotNet.Helix.JobMonitor.Models;
using Microsoft.Extensions.Logging;

namespace Microsoft.DotNet.Helix.JobMonitor
{
    /// <summary>
    /// Fire-and-forget queue for AzDO test-result uploads. Each queued upload runs as an
    /// independent task. Transient reads and test-result/attachment publishing use bounded
    /// retries; test-run creation and completion are attempted once. On normal completion the
    /// queue is drained so results in flight when the runner exits are not lost. On cancellation
    /// the queue is intentionally NOT drained: cancelling the in-flight Helix jobs takes priority,
    /// and any unfinished upload is re-uploaded in full by a later monitor invocation (a Helix job
    /// is only "processed" once its test run reaches the Completed state).
    /// </summary>
    internal sealed class TestResultUploadQueue
    {
        private const int MaximumTransientRetries = 2;
        private const string AzdoWarningPrefix = "##vso[task.logissue type=warning]";

        private readonly ILogger _logger;
        private readonly JobMonitorOptions _options;
        private readonly IAzureDevOpsService _azdo;
        private readonly IHelixService _helix;
        private readonly MonitorState _monitorState;
        private readonly List<PendingUpload> _pending = [];

        public TestResultUploadQueue(
            ILogger logger,
            JobMonitorOptions options,
            IAzureDevOpsService azdo,
            IHelixService helix,
            MonitorState monitorState)
        {
            _logger = logger;
            _options = options;
            _azdo = azdo;
            _helix = helix;
            _monitorState = monitorState;
        }

        public void Enqueue(HelixJobInfo helixJob, IReadOnlyCollection<WorkItemSummary> workItems, CancellationToken cancellationToken)
        {
            IReadOnlyList<string> workItemNames = [.. workItems.Select(w => w.Name)];
            var pendingUpload = new PendingUpload(helixJob);
            // Scheduling uses CancellationToken.None so the upload task is always allowed to start.
            // The upload body still observes the runner's token, so when the runner is cancelled the
            // upload stops promptly and the job's results are re-uploaded by a later invocation.
            pendingUpload.Task = Task.Run(
                () => UploadAsync(pendingUpload, workItemNames, cancellationToken),
                CancellationToken.None);
            _pending.Add(pendingUpload);
        }

        public void Prune()
        {
            _pending.RemoveAll(static upload => upload.Task.IsCompleted);
        }

        public async Task DrainAsync(CancellationToken cancellationToken)
        {
            Prune();
            if (_pending.Count == 0)
            {
                return;
            }

            _logger.LogInformation("Waiting for {Count} pending test result upload(s) to complete.", _pending.Count);
            try
            {
                Task allUploads = Task.WhenAll(_pending.Select(upload => upload.Task));
                if (_options.Verbose)
                {
                    LogPendingUploads();
                    TimeSpan heartbeatInterval = TimeSpan.FromSeconds(Math.Max(1, _options.PollingIntervalSeconds));
                    while (!allUploads.IsCompleted)
                    {
                        Task heartbeat = Task.Delay(heartbeatInterval, cancellationToken);
                        if (await Task.WhenAny(allUploads, heartbeat) == allUploads)
                        {
                            break;
                        }

                        cancellationToken.ThrowIfCancellationRequested();
                        Prune();
                        LogPendingUploads();
                    }
                }

                await allUploads.WaitAsync(cancellationToken);
            }
            catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
            {
                // One or more upload tasks were cancelled; treat as best-effort drained.
            }
            Prune();
        }

        private async Task UploadAsync(
            PendingUpload pendingUpload,
            IReadOnlyCollection<string> workItemNames,
            CancellationToken cancellationToken)
        {
            HelixJobInfo helixJob = pendingUpload.HelixJob;
            _monitorState.MarkHelixJobUploadInProgress(helixJob.JobName);

            SetPhase(pendingUpload, $"downloading Helix test results for {workItemNames.Count} work item(s)");
            (bool downloadedSuccessfully, IReadOnlyList<WorkItemTestResults> downloaded) = await TryExecuteWithRetryAsync(
                () => _helix.DownloadTestResultsAsync(
                        helixJob.JobName,
                        workItemNames,
                        _options.WorkingDirectory,
                        cancellationToken),
                "download the Helix test results",
                helixJob,
                testRunId: 0,
                retryCount: MaximumTransientRetries,
                cancellationToken);
            if (!downloadedSuccessfully)
            {
                SetPhase(pendingUpload, "failed while downloading Helix test results");
                _monitorState.MarkHelixJobUploadFailed(helixJob.JobName);
                return;
            }

            SetPhase(pendingUpload, "creating the Azure DevOps test run");
            (bool created, int testRunId) = await TryExecuteAsync(
                () => _azdo.CreateTestRunAsync(helixJob.TestRunName, cancellationToken),
                "create the Azure DevOps test run",
                helixJob,
                testRunId: 0,
                cancellationToken);
            if (!created)
            {
                SetPhase(pendingUpload, "failed while creating the Azure DevOps test run");
                _monitorState.MarkHelixJobUploadFailed(helixJob.JobName);
                return;
            }

            SetPhase(pendingUpload, $"publishing {downloaded.Count} work item(s) to Azure DevOps test run {testRunId}");
            (bool uploadedSuccessfully, IReadOnlyDictionary<(string JobName, string WorkItemName), TestResultUploadSummary> testResults)
                = await TryExecuteAsync(
                    () => _azdo.UploadTestResultsAsync(testRunId, downloaded, cancellationToken),
                    "upload the test results to Azure DevOps",
                    helixJob,
                    testRunId,
                    cancellationToken);
            if (!uploadedSuccessfully)
            {
                SetPhase(pendingUpload, $"failed while publishing to Azure DevOps test run {testRunId}");
                _monitorState.MarkHelixJobUploadFailed(helixJob.JobName);
                return;
            }

            if (_options.FailWorkItemsWithFailedTests)
            {
                _monitorState.ObserveTestResults(testResults);
            }

            IReadOnlyCollection<string> failedWorkItems =
            [
                .. testResults
                    .Where(kv => !kv.Value.AllPassed)
                    .Select(kv => kv.Key.WorkItemName)
            ];

            SetPhase(pendingUpload, $"completing and tagging Azure DevOps test run {testRunId}");
            (bool completed, _) = await TryExecuteAsync(
                async () =>
                {
                    await _azdo.CompleteTestRunAsync(testRunId, helixJob.JobName, failedWorkItems, cancellationToken);
                    return true;
                },
                "complete and tag the Azure DevOps test run",
                helixJob,
                testRunId,
                cancellationToken);
            if (!completed)
            {
                SetPhase(pendingUpload, $"failed while completing Azure DevOps test run {testRunId}");
                _monitorState.MarkHelixJobUploadFailed(helixJob.JobName);
                return;
            }

            SetPhase(pendingUpload, $"completed Azure DevOps test run {testRunId}");
            _monitorState.TryMarkHelixJobProcessed(helixJob.JobName);

            long uploadedCount = testResults.Values.Sum(r => r.UploadedCount);
            _logger.LogInformation("{UploadedCount} test results for job '{JobName}' processed.",
                uploadedCount,
                helixJob.DisplayName);
        }

        private Task<(bool Success, T Result)> TryExecuteAsync<T>(
            Func<Task<T>> operation,
            string operationDescription,
            HelixJobInfo helixJob,
            int testRunId,
            CancellationToken cancellationToken)
            => TryExecuteWithRetryAsync(
                operation,
                operationDescription,
                helixJob,
                testRunId,
                retryCount: 0,
                cancellationToken);

        private async Task<(bool Success, T Result)> TryExecuteWithRetryAsync<T>(
            Func<Task<T>> operation,
            string operationDescription,
            HelixJobInfo helixJob,
            int testRunId,
            int retryCount,
            CancellationToken cancellationToken)
        {
            try
            {
                T result = default;
                Exception lastException = null;
                var retry = new ExponentialRetry
                {
                    MaxAttempts = retryCount + 1,
                    RetryDelayCallback = (failedAttempt, delay) =>
                        _logger.LogDebug(
                            "Failed to {OperationDescription} for job {JobName}. Test run ID was {TestRunId}. "
                            + "Waiting {RetryDelay} before attempt {NextAttempt} of {AttemptCount}.",
                            operationDescription,
                            helixJob.DisplayName,
                            testRunId,
                            delay,
                            failedAttempt + 1,
                            retryCount + 1),
                };

                bool succeeded = await retry.RunAsync(
                    async attempt =>
                    {
                        try
                        {
                            result = await operation();
                            return true;
                        }
                        catch (Exception ex) when (
                            !cancellationToken.IsCancellationRequested
                            && TransientFailureDetector.IsTransient(ex))
                        {
                            lastException = ex;
                            _logger.LogDebug(ex,
                                "Failed to {OperationDescription} for job {JobName}. Test run ID was {TestRunId}. "
                                + "Transient attempt {Attempt} of {AttemptCount} failed.",
                                operationDescription,
                                helixJob.DisplayName,
                                testRunId,
                                attempt + 1,
                                retryCount + 1);
                            return false;
                        }
                    },
                    cancellationToken);

                if (!succeeded)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    throw lastException ?? new InvalidOperationException("Upload retry loop exited unexpectedly.");
                }

                return (true, result);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                string failureKind = TransientFailureDetector.IsTransient(ex)
                    ? retryCount > 0
                        ? "Transient retry limit reached."
                        : "The operation may have partially completed and is not safe to replay in this invocation."
                    : "The failure is not retryable.";
                _logger.LogWarning(ex,
                    "{Prefix}Failed to {OperationDescription} for job {JobName}. Test run ID was {TestRunId}. "
                    + "{FailureKind} The run remains untagged and a later monitor invocation may retry the upload.",
                    AzdoWarningPrefix,
                    operationDescription,
                    helixJob.DisplayName,
                    testRunId,
                    failureKind);
                return (false, default);
            }
        }

        private void SetPhase(PendingUpload upload, string phase)
        {
            upload.SetPhase(phase);
            if (_options.Verbose)
            {
                _logger.LogDebug(
                    "Test result upload for job '{JobName}' entered phase '{Phase}' after {Elapsed}.",
                    upload.HelixJob.DisplayName,
                    phase,
                    DateTimeOffset.UtcNow - upload.StartedAt);
            }
        }

        private void LogPendingUploads()
        {
            if (_pending.Count == 0)
            {
                return;
            }

            DateTimeOffset now = DateTimeOffset.UtcNow;
            string details = string.Join(
                Environment.NewLine,
                _pending
                    .OrderBy(upload => upload.StartedAt)
                    .Select(upload =>
                    {
                        (string phase, DateTimeOffset phaseStartedAt) = upload.GetPhase();
                        return $"- {upload.HelixJob.DisplayName}: phase='{phase}', "
                            + $"phase elapsed={now - phaseStartedAt:c}, total elapsed={now - upload.StartedAt:c}";
                    }));

            _logger.LogDebug(
                "{Count} test result upload(s) remain pending:{nl}{Details}",
                _pending.Count,
                Environment.NewLine,
                details);
        }

        private sealed class PendingUpload
        {
            private readonly object _sync = new();
            private string _phase = "queued";
            private DateTimeOffset _phaseStartedAt;

            public PendingUpload(HelixJobInfo helixJob)
            {
                HelixJob = helixJob;
                StartedAt = DateTimeOffset.UtcNow;
                _phaseStartedAt = StartedAt;
            }

            public HelixJobInfo HelixJob { get; }

            public DateTimeOffset StartedAt { get; }

            public Task Task { get; set; }

            public void SetPhase(string phase)
            {
                lock (_sync)
                {
                    _phase = phase;
                    _phaseStartedAt = DateTimeOffset.UtcNow;
                }
            }

            public (string Phase, DateTimeOffset PhaseStartedAt) GetPhase()
            {
                lock (_sync)
                {
                    return (_phase, _phaseStartedAt);
                }
            }
        }

    }
}
