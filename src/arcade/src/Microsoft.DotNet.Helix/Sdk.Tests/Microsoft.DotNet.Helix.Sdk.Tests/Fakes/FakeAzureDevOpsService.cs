// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly Queue<Exception> _uploadFailures = [];
        private int _timelineCallCount;
        private int _nextTestRunId;

        // Observable state for test assertions
        public List<string> CreatedTestRuns { get; } = [];
        public List<int> CompletedTestRunIds { get; } = [];
        public Dictionary<int, List<WorkItemTestResults>> UploadedResultsByRunId { get; } = [];
        public List<string> UploadedJobNames { get; } = [];
        public int CreateTestRunCallCount { get; private set; }
        public int UploadTestResultsCallCount { get; private set; }
        public TaskCompletionSource UploadStarted { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public TaskCompletionSource UploadCompleted { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
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

        public FakeAzureDevOpsService FailNextUpload(Exception exception = null)
        {
            lock (_sync)
            {
                _uploadFailures.Enqueue(exception ?? new InvalidOperationException("Injected upload failure."));
            }

            return this;
        }

        // IAzureDevOpsService implementation
        public Task<IReadOnlyList<AzureDevOpsTimelineRecord>> GetTimelineRecordsAsync(CancellationToken cancellationToken)
        {
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

        public Task<int> CreateTestRunAsync(string name, CancellationToken cancellationToken)
        {
            lock (_sync)
            {
                CreateTestRunCallCount++;

                // Matches the real AzureDevOpsService: every call creates a brand new in-progress
                // run. The service never reuses an existing run, so a transient upload failure that
                // re-enters the retry loop leaves an orphaned (untagged) run behind — exactly the
                // crash-resilience behavior described in the design.
                int id = Interlocked.Increment(ref _nextTestRunId);
                CreatedTestRuns.Add(name);
                return Task.FromResult(id);
            }
        }

        public Task CompleteTestRunAsync(int testRunId, string helixJobName, CancellationToken cancellationToken)
        {
            lock (_sync)
            {
                CompletedTestRunIds.Add(testRunId);
                return Task.CompletedTask;
            }
        }

        public async Task<int> UploadTestResultsAsync(int testRunId, IReadOnlyList<WorkItemTestResults> results, CancellationToken cancellationToken)
        {
            UploadStarted.TrySetResult();
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

                existing.AddRange(results);

                foreach (string jobName in results.Select(r => r.JobName).Distinct(StringComparer.OrdinalIgnoreCase))
                {
                    UploadedJobNames.Add(jobName);
                    _previouslyProcessedJobs.Add(jobName);
                }
            }

            UploadCompleted.TrySetResult();
            return results.Sum(static result => result.TestResultFiles.Count);
        }
    }
}
