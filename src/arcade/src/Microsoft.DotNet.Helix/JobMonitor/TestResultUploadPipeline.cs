// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Concurrent;
using Microsoft.Arcade.Common;
using Microsoft.DotNet.Helix.AzureDevOpsTestPublisher;
using Microsoft.DotNet.Helix.Client.Models;
using Microsoft.DotNet.Helix.JobMonitor.Models;
using Microsoft.DotNet.Helix.JobMonitor.Parallelism;
using Microsoft.Extensions.Logging;

namespace Microsoft.DotNet.Helix.JobMonitor;

internal sealed class TestResultUploadPipeline : IAsyncDisposable
{
    private const int MaximumTransientDownloadRetries = 2;
    private const string AzdoWarningPrefix = "##vso[task.logissue type=warning]";

    private readonly ILogger _logger;
    private readonly JobMonitorOptions _options;
    private readonly IAzureDevOpsService _azdo;
    private readonly IHelixService _helix;
    private readonly MonitorState _state;
    private readonly JobMonitorMetrics _metrics;
    private readonly ConcurrentDictionary<string, JobUploadSession> _sessions =
        new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<int, long> _remainingWorkItemsByPoll = [];
    private readonly ConcurrentDictionary<int, long> _acceptedWorkItemsByPoll = [];
    private readonly ActionQueue<JobUploadRequest> _jobs;
    private readonly ActionQueue<WorkItemUploadRequest> _workItems;
    private readonly ActionQueue<JobUploadSession> _finalizers;
    private int _draining;

    public TestResultUploadPipeline(
        ILogger logger,
        JobMonitorOptions options,
        IAzureDevOpsService azdo,
        IHelixService helix,
        MonitorState state,
        JobMonitorMetrics metrics)
    {
        _logger = logger;
        _options = options;
        _azdo = azdo;
        _helix = helix;
        _state = state;
        _metrics = metrics;

        int uploadParallelism = options.TestResultUploadParallelism;
        _jobs = new ActionQueue<JobUploadRequest>(
            parallelism: Math.Min(4, uploadParallelism),
            ExpandJobAsync);
        _workItems = new ActionQueue<WorkItemUploadRequest>(
            capacity: Math.Max(64, uploadParallelism * 8),
            parallelism: uploadParallelism,
            ProcessWorkItemAsync);
        _finalizers = new ActionQueue<JobUploadSession>(
            capacity: Math.Max(64, uploadParallelism * 2),
            parallelism: Math.Min(4, uploadParallelism),
            FinalizeJobAsync);
    }

    public UploadPipelineSnapshot Snapshot => new(
        _jobs.Snapshot,
        _workItems.Snapshot,
        _finalizers.Snapshot,
        _sessions.Values.Count(static session => session.HasFailed),
        _sessions.Values.Sum(static session => session.UploadedResultCount));

    public bool TryEnqueue(
        HelixJobInfo job,
        IReadOnlyCollection<WorkItemSummary> workItems,
        bool isJobComplete,
        int discoveryPoll)
    {
        if (Volatile.Read(ref _draining) != 0 || _state.IsHelixJobProcessed(job.JobName))
        {
            return false;
        }

        bool addedSession = false;
        JobUploadSession session = _sessions.GetOrAdd(
            job.JobName,
            _ =>
            {
                addedSession = true;
                return new JobUploadSession(job);
            });
        if (session.IsFinalized)
        {
            return false;
        }

        IReadOnlyList<string> newWorkItems = session.AddWorkItems(
            workItems.Where(static workItem => workItem.ExitCode.HasValue),
            isJobComplete);
        if (newWorkItems.Count == 0 && !session.IsReadyToFinalize)
        {
            return false;
        }

        if (!_jobs.TryEnqueue(new JobUploadRequest(session, newWorkItems, discoveryPoll)))
        {
            // Polling is the only producer, the queue is unbounded, and draining is rejected
            // above, so this indicates a broken pipeline invariant rather than backpressure.
            throw new InvalidOperationException("The test result upload pipeline stopped accepting jobs before drain began.");
        }

        _remainingWorkItemsByPoll.AddOrUpdate(
            discoveryPoll,
            newWorkItems.Count,
            (_, remaining) => remaining + newWorkItems.Count);
        _acceptedWorkItemsByPoll.AddOrUpdate(
            discoveryPoll,
            newWorkItems.Count,
            (_, accepted) => accepted + newWorkItems.Count);

        if (addedSession)
        {
            _state.TryQueueHelixJobUpload(job.JobName);
        }

        return true;
    }

    public async Task DrainAsync(
        int finalPoll,
        int newlyTerminalWorkItems,
        CancellationToken cancellationToken)
    {
        if (Interlocked.Exchange(ref _draining, 1) != 0)
        {
            return;
        }

        DateTimeOffset startedAt = DateTimeOffset.UtcNow;
        long finalPollRemainingWorkItems = GetRemainingWorkItems(finalPoll);
        long priorPollBacklog = _remainingWorkItemsByPoll
            .Where(pair => pair.Key != finalPoll)
            .Sum(static pair => Math.Max(0, pair.Value));
        long remainingWorkItems = finalPollRemainingWorkItems + priorPollBacklog;
        long finalPollEligibleWorkItems = _acceptedWorkItemsByPoll.TryGetValue(finalPoll, out long accepted)
            ? accepted
            : 0;
        int remainingFinalizations = _sessions.Values.Count(static session => !session.IsFinalized);

        _logger.LogInformation(
            "Starting final test result drain: {NewlyTerminalWorkItems} work item(s) were first observed "
            + "terminal and {FinalPollEligibleWorkItems} work item upload(s) became eligible in the final poll; "
            + "{RemainingWorkItems} work item upload(s) remain "
            + "({FinalPollRemainingWorkItems} from the final poll, {PriorPollBacklog} from earlier polls), "
            + "plus {RemainingFinalizations} job finalization(s).",
            newlyTerminalWorkItems,
            finalPollEligibleWorkItems,
            remainingWorkItems,
            finalPollRemainingWorkItems,
            priorPollBacklog,
            remainingFinalizations);

        _jobs.Complete();
        await _jobs.DrainAsync().WaitAsync(cancellationToken);

        _workItems.Complete();
        await _workItems.DrainAsync().WaitAsync(cancellationToken);

        _finalizers.Complete();
        await _finalizers.DrainAsync().WaitAsync(cancellationToken);

        UploadPipelineSnapshot snapshot = Snapshot;
        _logger.LogInformation(
            "Test result pipeline drained in {Elapsed}. {JobCount} job(s), {WorkItemCount} work item(s), "
            + "and {ResultCount} result(s) were processed; {FailedJobCount} job upload(s) remain untagged.",
            DateTimeOffset.UtcNow - startedAt,
            _sessions.Count,
            snapshot.WorkItems.Completed,
            snapshot.UploadedResults,
            snapshot.FailedJobs);
    }

    public void Cancel()
    {
        _jobs.Cancel();
        _workItems.Cancel();
        _finalizers.Cancel();
    }

    public async ValueTask DisposeAsync()
    {
        Cancel();
        await _jobs.DisposeAsync();
        await _workItems.DisposeAsync();
        await _finalizers.DisposeAsync();
    }

    private async ValueTask ExpandJobAsync(JobUploadRequest request, CancellationToken cancellationToken)
    {
        JobUploadSession session = request.Session;
        _state.MarkHelixJobUploadInProgress(session.Job.JobName);

        foreach (string workItemName in request.WorkItemNames)
        {
            await _workItems.EnqueueAsync(
                new WorkItemUploadRequest(session, workItemName, request.DiscoveryPoll),
                cancellationToken);
        }

        if (session.TryQueueFinalizer())
        {
            await _finalizers.EnqueueAsync(session, cancellationToken);
        }
    }

    private long GetRemainingWorkItems(int discoveryPoll)
        => _remainingWorkItemsByPoll.TryGetValue(discoveryPoll, out long remaining)
            ? Math.Max(0, remaining)
            : 0;

    private async ValueTask ProcessWorkItemAsync(
        WorkItemUploadRequest request,
        CancellationToken cancellationToken)
    {
        JobUploadSession session = request.Session;
        try
        {
            WorkItemTestResults downloaded;
            long downloadStartedAt = JobMonitorMetrics.StartOperation();
            try
            {
                downloaded = await ExecuteDownloadWithRetryAsync(
                    session.Job,
                    request.WorkItemName,
                    cancellationToken);
            }
            finally
            {
                _metrics.RecordPipelineOperation(
                    PipelineOperation.WorkItemDownload,
                    downloadStartedAt);
            }

            int testRunId = await session.GetOrCreateTestRunAsync(
                () => CreateTestRunAsync(session.Job.TestRunName, cancellationToken));

            TestResultUploadSummary summary =
                await _azdo.UploadTestResultsAsync(testRunId, downloaded, cancellationToken);

            session.RecordSuccess(
                request.WorkItemName,
                downloaded.TestResultFiles.Count,
                summary);
            if (_options.FailWorkItemsWithFailedTests)
            {
                _state.ObserveTestResult(session.Job.JobName, request.WorkItemName, summary);
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            session.RecordFailure();
            LogUploadFailure(
                ex,
                $"process test results for work item '{request.WorkItemName}' in job '{session.Job.DisplayName}'");
        }
        finally
        {
            _remainingWorkItemsByPoll.AddOrUpdate(
                request.DiscoveryPoll,
                0,
                static (_, remaining) => remaining - 1);
            if (session.MarkWorkItemFinished())
            {
                await _finalizers.EnqueueAsync(session, cancellationToken);
            }
        }
    }

    private async ValueTask FinalizeJobAsync(
        JobUploadSession session,
        CancellationToken cancellationToken)
    {
        try
        {
            if (session.HasFailed)
            {
                _state.MarkHelixJobUploadFailed(session.Job.JobName);
                return;
            }

            try
            {
                int testRunId = await session.GetOrCreateTestRunAsync(
                    () => CreateTestRunAsync(session.Job.TestRunName, cancellationToken));
                long completeStartedAt = JobMonitorMetrics.StartOperation();
                try
                {
                    await _azdo.CompleteTestRunAsync(
                        testRunId,
                        session.Job.JobName,
                        session.FailedWorkItems,
                        cancellationToken);
                }
                finally
                {
                    _metrics.RecordPipelineOperation(
                        PipelineOperation.TestRunComplete,
                        completeStartedAt);
                }

                _state.TryMarkHelixJobProcessed(session.Job.JobName);
                _logger.LogInformation(
                    "Test result processing completed for job '{JobName}': {WorkItemCount} work item(s), "
                    + "{ResultFileCount} recognized result file(s), and {UploadedCount} test result(s) uploaded.",
                    session.Job.DisplayName,
                    session.WorkItemCount,
                    session.ResultFileCount,
                    session.UploadedResultCount);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                session.RecordFailure();
                _state.MarkHelixJobUploadFailed(session.Job.JobName);
                LogUploadFailure(ex, $"complete Azure DevOps test run for job '{session.Job.DisplayName}'");
            }
        }
        finally
        {
            session.MarkFinalized();
        }
    }

    private async Task<int> CreateTestRunAsync(
        string testRunName,
        CancellationToken cancellationToken)
    {
        long startedAt = JobMonitorMetrics.StartOperation();
        try
        {
            return await _azdo.CreateTestRunAsync(testRunName, cancellationToken);
        }
        finally
        {
            _metrics.RecordPipelineOperation(PipelineOperation.TestRunCreate, startedAt);
        }
    }

    private async Task<WorkItemTestResults> ExecuteDownloadWithRetryAsync(
        HelixJobInfo job,
        string workItemName,
        CancellationToken cancellationToken)
    {
        WorkItemTestResults result = null;
        Exception lastException = null;
        var retry = new ExponentialRetry
        {
            MaxAttempts = MaximumTransientDownloadRetries + 1,
            RetryDelayCallback = (attempt, delay) =>
                _logger.LogDebug(
                    "Transient result download failure for '{JobName}/{WorkItemName}' on attempt {Attempt}. "
                    + "Retrying after {Delay}.",
                    job.DisplayName,
                    workItemName,
                    attempt,
                    delay),
        };

        bool succeeded = await retry.RunAsync(
            async _ =>
            {
                try
                {
                    result = await _helix.DownloadTestResultsAsync(
                        job.JobName,
                        workItemName,
                        _options.WorkingDirectory,
                        cancellationToken);
                    return RetryResult.Success;
                }
                catch (Exception ex) when (
                    !cancellationToken.IsCancellationRequested
                    && TransientFailureDetector.IsTransient(ex))
                {
                    lastException = ex;
                    return RetryResult.Retry();
                }
            },
            cancellationToken);

        return succeeded
            ? result
            : throw lastException ?? new InvalidOperationException("Result download retry exited unexpectedly.");
    }

    private void LogUploadFailure(Exception exception, string operation)
        => _logger.LogWarning(
            exception,
            "{Prefix}Failed to {Operation}. The Helix job remains untagged so a later monitor invocation can replay it.",
            AzdoWarningPrefix,
            operation);

    private sealed record JobUploadRequest(
        JobUploadSession Session,
        IReadOnlyList<string> WorkItemNames,
        int DiscoveryPoll);

    private sealed record WorkItemUploadRequest(
        JobUploadSession Session,
        string WorkItemName,
        int DiscoveryPoll);

    private sealed class JobUploadSession
    {
        private readonly object _sync = new();
        private readonly HashSet<string> _failedWorkItems = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _workItems = new(StringComparer.OrdinalIgnoreCase);
        private Task<int> _testRunTask;
        private int _pendingWorkItems;
        private int _jobComplete;
        private int _finalizerQueued;
        private int _finalized;
        private int _failed;
        private long _resultFileCount;
        private long _uploadedResultCount;

        public JobUploadSession(HelixJobInfo job)
        {
            Job = job;
        }

        public HelixJobInfo Job { get; }

        public int WorkItemCount
        {
            get { lock (_sync) { return _workItems.Count; } }
        }

        public bool IsFinalized => Volatile.Read(ref _finalized) != 0;

        public bool IsReadyToFinalize
        {
            get
            {
                lock (_sync)
                {
                    return IsReadyToFinalizeLocked();
                }
            }
        }

        public bool HasFailed => Volatile.Read(ref _failed) != 0;

        public long UploadedResultCount => Interlocked.Read(ref _uploadedResultCount);

        public long ResultFileCount => Interlocked.Read(ref _resultFileCount);

        public IReadOnlyCollection<string> FailedWorkItems
        {
            get
            {
                lock (_sync)
                {
                    return [.. _failedWorkItems];
                }
            }
        }

        public Task<int> GetOrCreateTestRunAsync(Func<Task<int>> create)
        {
            lock (_sync)
            {
                return _testRunTask ??= InvokeCreate();
            }

            Task<int> InvokeCreate()
            {
                try
                {
                    return create();
                }
                catch (Exception ex)
                {
                    return Task.FromException<int>(ex);
                }
            }
        }

        public void RecordSuccess(
            string workItemName,
            int resultFileCount,
            TestResultUploadSummary summary)
        {
            Interlocked.Add(ref _resultFileCount, resultFileCount);
            Interlocked.Add(ref _uploadedResultCount, summary.UploadedCount);
            if (!summary.AllPassed)
            {
                lock (_sync)
                {
                    _failedWorkItems.Add(workItemName);
                }
            }
        }

        public void RecordFailure() => Interlocked.Exchange(ref _failed, 1);

        public IReadOnlyList<string> AddWorkItems(
            IEnumerable<WorkItemSummary> workItems,
            bool isJobComplete)
        {
            lock (_sync)
            {
                var added = new List<string>();
                foreach (WorkItemSummary workItem in workItems)
                {
                    if (_workItems.Add(workItem.Name))
                    {
                        added.Add(workItem.Name);
                        _pendingWorkItems++;
                    }
                }

                if (isJobComplete)
                {
                    _jobComplete = 1;
                }

                return added;
            }
        }

        public bool MarkWorkItemFinished()
        {
            lock (_sync)
            {
                _pendingWorkItems--;
                return TryQueueFinalizerLocked();
            }
        }

        public bool TryQueueFinalizer()
        {
            lock (_sync)
            {
                return TryQueueFinalizerLocked();
            }
        }

        public void MarkFinalized() => Interlocked.Exchange(ref _finalized, 1);

        private bool TryQueueFinalizerLocked()
        {
            if (!IsReadyToFinalizeLocked() || _finalizerQueued != 0)
            {
                return false;
            }

            _finalizerQueued = 1;
            return true;
        }

        private bool IsReadyToFinalizeLocked()
            => _jobComplete != 0 && _pendingWorkItems == 0;
    }
}

internal readonly record struct UploadPipelineSnapshot(
    QueueSnapshot Jobs,
    QueueSnapshot WorkItems,
    QueueSnapshot Finalizers,
    int FailedJobs,
    long UploadedResults);
