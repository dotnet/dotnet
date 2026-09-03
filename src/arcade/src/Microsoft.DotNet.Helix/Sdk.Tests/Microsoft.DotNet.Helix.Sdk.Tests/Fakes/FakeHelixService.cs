// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.DotNet.Helix.Client.Models;
using Microsoft.DotNet.Helix.JobMonitor;
using Microsoft.DotNet.Helix.JobMonitor.Models;

namespace Microsoft.DotNet.Helix.Sdk.Tests.Fakes
{
    internal sealed class FakeHelixService : IHelixService
    {
        private readonly List<HelixSnapshot> _responses = [];
        private readonly HashSet<string> _downloadFailureJobs = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, IReadOnlyCollection<WorkItemSummary>> _customWorkItems = new(StringComparer.OrdinalIgnoreCase);
        private int _getJobsCallCount;

        /// <summary>
        /// Adds a Helix response. Each call to <see cref="GetJobsForBuildAsync"/> returns the next
        /// response in order. Once all responses are consumed, the last one is repeated.
        /// <see cref="ListWorkItemsAsync"/> and <see cref="DownloadTestResultsAsync"/> use
        /// the same current response for pass/fail and result data.
        /// </summary>
        public FakeHelixService AddResponse(
            HelixJobInfo[] jobs,
            Dictionary<string, HelixJobPassFail> passFailByJob = null,
            Dictionary<string, List<WorkItemTestResults>> testResultsByJob = null)
        {
            _responses.Add(new HelixSnapshot(
                jobs,
                passFailByJob ?? new Dictionary<string, HelixJobPassFail>(StringComparer.OrdinalIgnoreCase),
                testResultsByJob ?? new Dictionary<string, List<WorkItemTestResults>>(StringComparer.OrdinalIgnoreCase)));
            return this;
        }

        public FakeHelixService FailDownloadForJob(string jobName) { _downloadFailureJobs.Add(jobName); return this; }
        public void ClearDownloadFailures() { _downloadFailureJobs.Clear(); }

        public FakeHelixService WithWorkItems(string jobName, IReadOnlyCollection<WorkItemSummary> workItems)
        {
            _customWorkItems[jobName] = workItems;
            return this;
        }

        /// <summary>Number of times <see cref="GetJobsForBuildAsync"/> has been called.</summary>
        public int GetJobsCallCount => _getJobsCallCount;

        public ConcurrentBag<string> CanceledJobs { get; } = [];

        private HelixSnapshot CurrentSnapshot
        {
            get
            {
                int index = Math.Min(Math.Max(_getJobsCallCount - 1, 0), _responses.Count - 1);
                return _responses[index];
            }
        }

        public Task<IReadOnlyList<HelixJobInfo>> GetJobsForBuildAsync(string source, string buildId, CancellationToken cancellationToken)
        {
            if (_responses.Count == 0)
            {
                _getJobsCallCount++;
                return Task.FromResult<IReadOnlyList<HelixJobInfo>>([]);
            }

            int index = Math.Min(_getJobsCallCount, _responses.Count - 1);
            _getJobsCallCount++;
            return Task.FromResult<IReadOnlyList<HelixJobInfo>>(_responses[index].Jobs);
        }

        public Task<IReadOnlyList<WorkItemTestResults>> DownloadTestResultsAsync(
            string jobName, IReadOnlyCollection<string> workItemNames, string workingDirectory, CancellationToken cancellationToken)
        {
            if (_downloadFailureJobs.Contains(jobName))
            {
                throw new InvalidOperationException($"Injected download failure for Helix job '{jobName}'.");
            }

            if (CurrentSnapshot.TestResultsByJob.TryGetValue(jobName, out List<WorkItemTestResults> explicitResults))
            {
                return Task.FromResult<IReadOnlyList<WorkItemTestResults>>(explicitResults);
            }

            workItemNames = workItemNames
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .DefaultIfEmpty($"{jobName}-synthetic")
                .ToList();

            IReadOnlyList<WorkItemTestResults> generated = workItemNames
                .Select(wi => new WorkItemTestResults(jobName, wi, []))
                .ToList();

            return Task.FromResult(generated);
        }

        public Task<IReadOnlyCollection<WorkItemSummary>> ListWorkItemsAsync(
            string jobName,
            CancellationToken _)
        {
            if (_customWorkItems.TryGetValue(jobName, out IReadOnlyCollection<WorkItemSummary> customWorkItems))
            {
                return Task.FromResult(customWorkItems);
            }

            var items = new List<WorkItemSummary>();
            if (!CurrentSnapshot.PassFailByJob.TryGetValue(jobName, out HelixJobPassFail passFail))
            {
                return Task.FromResult<IReadOnlyCollection<WorkItemSummary>>(items);
            }

            foreach (string w in passFail.PassedWorkItems)
            {
                var wi = new WorkItemSummary($"{jobName}/{w}", jobName, w, "Finished") { ExitCode = 0 };
                items.Add(wi);
            }

            foreach (string w in passFail.FailedWorkItems)
            {
                var wi = new WorkItemSummary($"{jobName}/{w}", jobName, w, "Finished") { ExitCode = 1 };
                items.Add(wi);
            }

            return Task.FromResult<IReadOnlyCollection<WorkItemSummary>>(items);
        }

        public Task CancelJobAsync(string jobName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            CanceledJobs.Add(jobName);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Tracks resubmission calls for test assertions.
        /// Each entry is (originalJobName, failedWorkItemNames, newJobName).
        /// </summary>
        public List<(string OriginalJob, IReadOnlyCollection<string> FailedItems, string NewJob)> Resubmissions { get; } = [];

        /// <summary>
        /// Configures the result of a resubmission. When <see cref="ResubmitWorkItemsAsync"/>
        /// is called for <paramref name="originalJobName"/>, a new <see cref="HelixJobInfo"/> with
        /// <paramref name="newJobName"/> is returned. The new job will appear in subsequent
        /// <see cref="GetJobsForBuildAsync"/> calls via the responses already configured.
        /// </summary>
        private readonly Dictionary<string, string> _resubmissionNewJobNames = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _nullResubmissions = new(StringComparer.OrdinalIgnoreCase);

        public FakeHelixService ConfigureResubmission(string originalJobName, string newJobName)
        {
            _resubmissionNewJobNames[originalJobName] = newJobName;
            return this;
        }

        public FakeHelixService ConfigureNullResubmission(string originalJobName)
        {
            _nullResubmissions.Add(originalJobName);
            return this;
        }

        public Task<HelixJobInfo> ResubmitWorkItemsAsync(
            HelixJobInfo originalJob,
            IReadOnlyCollection<WorkItemSummary> failedWorkItems,
            CancellationToken cancellationToken)
        {
            string originalJobName = originalJob.JobName;
            HelixJobInfo originalSnapshotJob = CurrentSnapshot.Jobs.FirstOrDefault(j =>
                string.Equals(j.JobName, originalJobName, StringComparison.OrdinalIgnoreCase))
                ?? originalJob;

            if (!_resubmissionNewJobNames.TryGetValue(originalJobName, out string newJobName))
            {
                newJobName = $"{originalJobName}-resubmit";
            }

            IReadOnlyCollection<string> failedItemNames = [..failedWorkItems.Select(wi => wi.Name)];
            if (_nullResubmissions.Contains(originalJobName))
            {
                Resubmissions.Add((originalJobName, failedItemNames, null));
                return Task.FromResult<HelixJobInfo>(null);
            }

            Resubmissions.Add((originalJobName, failedItemNames, newJobName));
            return Task.FromResult(new HelixJobInfo(
                newJobName,
                "running",
                originalSnapshotJob?.TestRunName,
                originalSnapshotJob?.StageName,
                originalSnapshotJob?.SubmitterJobName ?? originalJob.SubmitterJobName,
                originalSnapshotJob?.SubmitterJobDisplayName ?? originalJob.SubmitterJobDisplayName,
                originalSnapshotJob?.QueueId ?? originalJob.QueueId,
                originalJobName));
        }

        private sealed record HelixSnapshot(
            HelixJobInfo[] Jobs,
            Dictionary<string, HelixJobPassFail> PassFailByJob,
            Dictionary<string, List<WorkItemTestResults>> TestResultsByJob);
    }
}
