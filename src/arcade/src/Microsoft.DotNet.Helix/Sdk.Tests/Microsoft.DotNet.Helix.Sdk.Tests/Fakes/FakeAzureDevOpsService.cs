// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.DotNet.Helix.AzureDevOpsTestPublisher;
using Microsoft.DotNet.Helix.JobMonitor;

namespace Microsoft.DotNet.Helix.Sdk.Tests.Fakes
{
    internal sealed class FakeAzureDevOpsService : IAzureDevOpsService
    {
        // FakeAzureDevOpsService is exercised concurrently when JobMonitorRunner kicks off
        // multiple test-result uploads in parallel via Task.Run. All mutable state is
        // guarded by _sync so observable assertions (e.g. UploadedJobNames count) are
        // deterministic across machines with varying parallelism levels.
        private readonly object _sync = new();
        private readonly List<AzureDevOpsTimelineRecord[]> _timelineResponses = [];
        private readonly HashSet<string> _previouslyProcessedJobs = new(StringComparer.OrdinalIgnoreCase);
        private readonly Queue<Exception> _createFailures = [];
        private readonly Queue<Exception> _uploadFailures = [];
        private readonly Queue<Exception> _completeFailures = [];
        private readonly Queue<Exception> _timelineFailures = [];
        private readonly HashSet<(string JobName, string WorkItemName)> _recordedFailedTests
            = new(FailedTestWorkItemComparer.Instance);
        private readonly HashSet<(string JobName, string WorkItemName)> _uploadFailedTests
            = new(FailedTestWorkItemComparer.Instance);
        private int _timelineCallCount;
        private int _nextTestRunId;
        private int _activeUploads;
        private int _maximumConcurrentUploads;

        // Observable state for test assertions
        public List<string> CreatedTestRuns { get; } = [];
        public List<int> CompletedTestRunIds { get; } = [];
        public Dictionary<int, List<WorkItemTestResults>> UploadedResultsByRunId { get; } = [];
        public List<string> UploadedJobNames { get; } = [];
        public int CreateTestRunCallCount { get; private set; }
        public int UploadTestResultsCallCount { get; private set; }
        public int CompleteTestRunCallCount { get; private set; }
        public int MaximumConcurrentUploads => Volatile.Read(ref _maximumConcurrentUploads);
        public TaskCompletionSource UploadStarted { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public TaskCompletionSource UploadCompleted { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public TaskCompletionSource TestRunCompleted { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public Task UploadBlocker { get; set; } = Task.CompletedTask;

        /// <summary>
        /// When true, <see cref="UploadTestResultsAsync"/> waits on <see cref="UploadBlocker"/>
        /// without observing the cancellation token, simulating an upload stuck in a
        /// non-cancellable operation when the monitor is cancelled.
        /// </summary>
        public bool UploadBlockerIgnoresCancellation { get; set; }

        /// <summary>
        /// Number of times <see cref="GetTimelineRecordsAsync"/> has been called.
        /// This equals the number of poll iterations the runner has completed.
        /// </summary>
        public int TimelineCallCount => _timelineCallCount;

        // Configuration

        /// <summary>
        /// Adds a timeline response. Each call to <see cref="GetTimelineRecordsAsync"/>
        /// returns the next response in order. Once all responses are consumed, the last
        /// one is repeated indefinitely.
        /// </summary>
        public FakeAzureDevOpsService AddTimelineResponse(params AzureDevOpsTimelineRecord[] records)
        {
            _timelineResponses.Add(records);
            return this;
        }

        public FakeAzureDevOpsService WithPreviouslyProcessedJob(string jobName)
        {
            lock (_sync)
            {
                _previouslyProcessedJobs.Add(jobName);
            }
            return this;
        }

        public FakeAzureDevOpsService FailNextTimeline(Exception exception)
        {
            lock (_sync)
            {
                _timelineFailures.Enqueue(exception);
            }

            return this;
        }

        public FakeAzureDevOpsService FailNextCreate(Exception exception = null)
        {
            lock (_sync)
            {
                _createFailures.Enqueue(exception ?? CreateTransientFailure("Injected test-run creation failure."));
            }

            return this;
        }

        public FakeAzureDevOpsService FailNextUpload(Exception exception = null)
        {
            lock (_sync)
            {
                _uploadFailures.Enqueue(exception ?? CreateTransientFailure("Injected test-result upload failure."));
            }

            return this;
        }

        public FakeAzureDevOpsService FailNextComplete(Exception exception = null)
        {
            lock (_sync)
            {
                _completeFailures.Enqueue(exception ?? CreateTransientFailure("Injected test-run completion failure."));
            }

            return this;
        }

        /// <summary>
        /// Marks a (Helix job, work item) pair as having had failed test results recorded by
        /// a prior monitor invocation (i.e. surfaced by <see cref="GetFailedTestWorkItemsAsync"/>).
        /// Used to test the retry pass’s behavior of resubmitting work items that passed by
        /// exit code but whose tests failed.
        /// </summary>
        public FakeAzureDevOpsService WithRecordedFailedTest(string helixJobName, string workItemName)
        {
            lock (_sync)
            {
                _recordedFailedTests.Add((helixJobName, workItemName));
            }
            return this;
        }

        /// <summary>
        /// Configures <see cref="UploadTestResultsAsync"/> to report
        /// <c>AllPassed = false</c> for the given (Helix job, work item) pair when the next
        /// upload includes it. Used to test that the monitor marks work items as failed
        /// based on their uploaded test results even when the work item passed by exit code.
        /// </summary>
        public FakeAzureDevOpsService WithFailedUpload(string helixJobName, string workItemName)
        {
            lock (_sync)
            {
                _uploadFailedTests.Add((helixJobName, workItemName));
            }
            return this;
        }

        // IAzureDevOpsService implementation
        public Task<IReadOnlyList<AzureDevOpsTimelineRecord>> GetTimelineRecordsAsync(CancellationToken cancellationToken)
        {
            lock (_sync)
            {
                if (_timelineFailures.TryDequeue(out Exception failure))
                {
                    return Task.FromException<IReadOnlyList<AzureDevOpsTimelineRecord>>(failure);
                }
            }

            if (_timelineResponses.Count == 0)
            {
                _timelineCallCount++;
                return Task.FromResult<IReadOnlyList<AzureDevOpsTimelineRecord>>(Array.Empty<AzureDevOpsTimelineRecord>());
            }

            int index = Math.Min(_timelineCallCount, _timelineResponses.Count - 1);
            _timelineCallCount++;
            return Task.FromResult<IReadOnlyList<AzureDevOpsTimelineRecord>>(_timelineResponses[index]);
        }

        public Task<IReadOnlySet<string>> GetProcessedHelixJobNamesAsync(CancellationToken cancellationToken)
        {
            lock (_sync)
            {
                var result = new HashSet<string>(_previouslyProcessedJobs, StringComparer.OrdinalIgnoreCase);
                return Task.FromResult<IReadOnlySet<string>>(result);
            }
        }

        public Task<IReadOnlyDictionary<string, IReadOnlySet<string>>> GetFailedTestWorkItemsAsync(CancellationToken cancellationToken)
        {
            lock (_sync)
            {
                var result = new Dictionary<string, IReadOnlySet<string>>(StringComparer.OrdinalIgnoreCase);
                foreach (IGrouping<string, (string JobName, string WorkItemName)> group in
                    _recordedFailedTests.GroupBy(p => p.JobName, StringComparer.OrdinalIgnoreCase))
                {
                    result[group.Key] = new HashSet<string>(
                        group.Select(p => p.WorkItemName),
                        StringComparer.OrdinalIgnoreCase);
                }
                return Task.FromResult<IReadOnlyDictionary<string, IReadOnlySet<string>>>(result);
            }
        }

        public Task<int> CreateTestRunAsync(string name, CancellationToken cancellationToken)
        {
            lock (_sync)
            {
                CreateTestRunCallCount++;
                if (_createFailures.Count > 0)
                {
                    throw _createFailures.Dequeue();
                }

                int id = Interlocked.Increment(ref _nextTestRunId);
                CreatedTestRuns.Add(name);
                return Task.FromResult(id);
            }
        }

        public Task CompleteTestRunAsync(int testRunId, string helixJobName, IReadOnlyCollection<string> failedWorkItems, CancellationToken cancellationToken)
        {
            lock (_sync)
            {
                CompleteTestRunCallCount++;
                if (_completeFailures.Count > 0)
                {
                    throw _completeFailures.Dequeue();
                }

                CompletedTestRunIds.Add(testRunId);
                _previouslyProcessedJobs.Add(helixJobName);
                foreach (string workItemName in failedWorkItems ?? [])
                {
                    if (!string.IsNullOrEmpty(workItemName))
                    {
                        _recordedFailedTests.Add((helixJobName, workItemName));
                    }
                }
                TestRunCompleted.TrySetResult();
                return Task.CompletedTask;
            }
        }

        public async Task<TestResultUploadSummary> UploadTestResultsAsync(
            int testRunId,
            WorkItemTestResults results,
            CancellationToken cancellationToken)
        {
            UploadStarted.TrySetResult();
            int active = Interlocked.Increment(ref _activeUploads);
            int observedMaximum;
            while (active > (observedMaximum = Volatile.Read(ref _maximumConcurrentUploads)))
            {
                if (Interlocked.CompareExchange(ref _maximumConcurrentUploads, active, observedMaximum) == observedMaximum)
                {
                    break;
                }
            }

            try
            {
                if (UploadBlockerIgnoresCancellation)
                {
                    await UploadBlocker;
                }
                else
                {
                    await UploadBlocker.WaitAsync(cancellationToken);
                }

                lock (_sync)
                {
                    UploadTestResultsCallCount++;
                    if (_uploadFailures.Count > 0)
                    {
                        throw _uploadFailures.Dequeue();
                    }

                    if (!UploadedResultsByRunId.TryGetValue(testRunId, out List<WorkItemTestResults> existing))
                    {
                        existing = [];
                        UploadedResultsByRunId[testRunId] = existing;
                    }

                    existing.Add(results);
                    if (!UploadedJobNames.Contains(results.JobName, StringComparer.OrdinalIgnoreCase))
                    {
                        UploadedJobNames.Add(results.JobName);
                    }

                    bool allPassed = !_uploadFailedTests.Contains((results.JobName, results.WorkItemName));
                    return new TestResultUploadSummary(allPassed, results.TestResultFiles.Count);
                }
            }
            finally
            {
                Interlocked.Decrement(ref _activeUploads);
                UploadCompleted.TrySetResult();
            }
        }

        private static HttpRequestException CreateTransientFailure(string message)
            => new(message, null, HttpStatusCode.ServiceUnavailable);

        private sealed class FailedTestWorkItemComparer : IEqualityComparer<(string JobName, string WorkItemName)>
        {
            public static readonly FailedTestWorkItemComparer Instance = new();

            public bool Equals((string JobName, string WorkItemName) x, (string JobName, string WorkItemName) y)
                => StringComparer.OrdinalIgnoreCase.Equals(x.JobName, y.JobName)
                    && StringComparer.OrdinalIgnoreCase.Equals(x.WorkItemName, y.WorkItemName);

            public int GetHashCode((string JobName, string WorkItemName) obj)
                => HashCode.Combine(
                    obj.JobName is null ? 0 : StringComparer.OrdinalIgnoreCase.GetHashCode(obj.JobName),
                    obj.WorkItemName is null ? 0 : StringComparer.OrdinalIgnoreCase.GetHashCode(obj.WorkItemName));
        }
    }
}
