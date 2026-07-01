// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.DotNet.Helix.AzureDevOpsTestPublisher;
using Microsoft.DotNet.Helix.Client.Models;
using Microsoft.DotNet.Helix.JobMonitor.Models;

namespace Microsoft.DotNet.Helix.JobMonitor
{
    /// <summary>
    /// Thread-safe container for every piece of runtime state the monitor accumulates while
    /// observing a single invocation. Mutations from the main poll loop and from background
    /// test-result upload tasks (via <see cref="ObserveTestResults"/>) are serialized through
    /// an internal lock; collections are never exposed directly so callers cannot enumerate
    /// or mutate them outside the lock.
    /// </summary>
    internal sealed class MonitorState
    {
        private readonly object _sync = new();

        // All Helix jobs the monitor has observed for this build, keyed by Helix job name.
        // Overwritten per poll so the cached entry reflects the latest Helix-side state
        // (in particular the Finished timestamp transitioning from null to a value). Also used
        // by GetSubmitterChainKey to walk back through PreviousHelixJobName links across polls.
        private readonly Dictionary<string, HelixJobInfo> _associatedJobs = new(StringComparer.OrdinalIgnoreCase);

        // Helix jobs whose results have been uploaded to Azure DevOps in this or a prior
        // monitor invocation. Seeded on entry from the AzDO test-run tags.
        private readonly HashSet<string> _processedHelixJobs = new(StringComparer.OrdinalIgnoreCase);

        // Tracks the latest outcome for each logical work item, keyed by
        // (SubmitterChainKey, WorkItemName). See GetSubmitterChainKey for the keying rationale.
        private readonly Dictionary<(string ChainKey, string WorkItemName), bool> _workItemOutcomes
            = new(WorkItemOutcomeKeyComparer.Instance);

        // Helix job names whose per-work-item outcomes have already been reconciled into
        // _workItemOutcomes. Prevents the second reconciliation pass from re-processing
        // jobs that were observed in an earlier poll.
        private readonly HashSet<string> _workItemOutcomeJobs = new(StringComparer.OrdinalIgnoreCase);

        // Latest known console-link information for every failed work item, keyed the same
        // way as _workItemOutcomes. Cleared per key when a later incarnation passes.
        private readonly Dictionary<(string ChainKey, string WorkItemName), FailedWorkItemConsoleInfo> _failedWorkItemConsoleInfo
            = new(WorkItemOutcomeKeyComparer.Instance);

        // Deduplication set for per-failure console-link warnings.
        private readonly HashSet<string> _reportedFailedWorkItemConsoleLinks = new(StringComparer.OrdinalIgnoreCase);

        // Last observed AzDO timeline records (scoped to the monitor's stage).
        private readonly List<AzureDevOpsTimelineRecord> _latestTimelineRecords = [];

        // AzDO submitter job names whose Helix work was resubmitted during the retry pass.
        private readonly HashSet<string> _retryingHelixSubmitterJobs = new(StringComparer.OrdinalIgnoreCase);

        private int _resubmittedJobCount;
        private int _resubmittedWorkItemCount;
        private int _processedJobCount;

        public int ResubmittedJobCount => Volatile.Read(ref _resubmittedJobCount);

        public int ResubmittedWorkItemCount => Volatile.Read(ref _resubmittedWorkItemCount);

        public int ProcessedJobCount => Volatile.Read(ref _processedJobCount);

        public int AssociatedJobsCount
        {
            get { lock (_sync) { return _associatedJobs.Count; } }
        }

        public int WorkItemOutcomeCount
        {
            get { lock (_sync) { return _workItemOutcomes.Count; } }
        }

        public int FailedWorkItemConsoleInfoCount
        {
            get { lock (_sync) { return _failedWorkItemConsoleInfo.Count; } }
        }

        public int FailedWorkItemCount
        {
            get
            {
                lock (_sync)
                {
                    int count = 0;
                    foreach (bool passed in _workItemOutcomes.Values)
                    {
                        if (!passed)
                        {
                            count++;
                        }
                    }

                    return count;
                }
            }
        }

        public bool HasFailedWorkItem
        {
            get
            {
                lock (_sync)
                {
                    foreach (bool passed in _workItemOutcomes.Values)
                    {
                        if (!passed)
                        {
                            return true;
                        }
                    }

                    return false;
                }
            }
        }

        /// <summary>
        /// Record a freshly-seen set of jobs into the per-poll and cross-poll caches.
        /// </summary>
        public void ObserveJobs(IEnumerable<HelixJobInfo> jobs)
        {
            lock (_sync)
            {
                foreach (HelixJobInfo job in jobs)
                {
                    _associatedJobs[job.JobName] = job;
                }
            }
        }

        /// <summary>
        /// Returns a stable snapshot of every job observed so far. Safe to enumerate from any
        /// thread (the underlying dictionary will not mutate during iteration).
        /// </summary>
        public IReadOnlyList<HelixJobInfo> SnapshotAssociatedJobs()
        {
            lock (_sync)
            {
                return [.._associatedJobs.Values];
            }
        }

        /// <summary>
        /// Seeds the set of Helix jobs whose results were already uploaded in a prior monitor
        /// invocation. Called once at startup before any background work begins.
        /// </summary>
        public void AddProcessedHelixJobs(IEnumerable<string> jobNames)
        {
            lock (_sync)
            {
                foreach (string jobName in jobNames)
                {
                    _processedHelixJobs.Add(jobName);
                }
            }
        }

        /// <summary>
        /// Marks the given Helix job as processed and increments <see cref="ProcessedJobCount"/>.
        /// Returns true if this is the first time the job was marked.
        /// </summary>
        public bool TryMarkHelixJobProcessed(string jobName)
        {
            lock (_sync)
            {
                if (_processedHelixJobs.Add(jobName))
                {
                    _processedJobCount++;
                    return true;
                }

                return false;
            }
        }

        public bool IsHelixJobProcessed(string jobName)
        {
            lock (_sync)
            {
                return _processedHelixJobs.Contains(jobName);
            }
        }

        public bool IsWorkItemOutcomesRecorded(string jobName)
        {
            lock (_sync)
            {
                return _workItemOutcomeJobs.Contains(jobName);
            }
        }

        /// <summary>
        /// Atomically records all per-work-item outcomes for one completed Helix job:
        /// updates <see cref="WorkItemOutcomeCount"/>, the failure map, and the failed-work-item
        /// console-info map. Returns true the first time it is called for a given job; subsequent
        /// calls with the same job no-op so the reconciliation pass is idempotent.
        /// </summary>
        public bool TryRecordWorkItemOutcomes(HelixJobInfo helixJob, IReadOnlyCollection<WorkItemSummary> workItems)
        {
            lock (_sync)
            {
                if (!_workItemOutcomeJobs.Add(helixJob.JobName))
                {
                    return false;
                }

                string chainKey = GetSubmitterChainKeyLocked(helixJob);
                foreach (WorkItemSummary wi in workItems)
                {
                    // Within the same AzDO submitter chain (i.e. resubmissions of the same
                    // logical AzDO job), the latest result overwrites the prior one for the
                    // same work item name. Across different submitter chains the key differs,
                    // so identically-named work items in different AzDO jobs are tracked
                    // independently.
                    _workItemOutcomes[(chainKey, wi.Name)] = !wi.IsFailed;
                    TrackFailedWorkItemConsoleInfoLocked(helixJob, chainKey, wi);
                }

                return true;
            }
        }

        /// <summary>
        /// Marks work items whose uploaded test results contained any failure as failed in
        /// the outcome map. Work items whose tests all passed are left alone so the Helix-side
        /// outcome (recorded by the reconciliation pass) is preserved — a work item that the
        /// Helix runner reported as failed must stay failed even if it produced no failed
        /// test results.
        /// </summary>
        public void ObserveTestResults(
            IReadOnlyDictionary<(string JobName, string WorkItemName), TestResultUploadSummary> testResults)
        {
            lock (_sync)
            {
                foreach (KeyValuePair<(string JobName, string WorkItemName), TestResultUploadSummary> entry in testResults)
                {
                    if (entry.Value.AllPassed)
                    {
                        continue;
                    }

                    if (!_associatedJobs.TryGetValue(entry.Key.JobName, out HelixJobInfo job))
                    {
                        continue;
                    }

                    // If this job has been superseded by a later attempt whose outcomes were
                    // already reconciled, ignore late-arriving summaries from the older attempt
                    // so they cannot overwrite the newer outcome.
                    bool supersededByReconciledAttempt = _associatedJobs.Values.Any(j =>
                        !string.IsNullOrEmpty(j.PreviousHelixJobName)
                        && StringComparer.OrdinalIgnoreCase.Equals(j.PreviousHelixJobName, job.JobName)
                        && _workItemOutcomeJobs.Contains(j.JobName));
                    if (supersededByReconciledAttempt)
                    {
                        continue;
                    }

                    string chainKey = GetSubmitterChainKeyLocked(job);
                    var key = (chainKey, entry.Key.WorkItemName);
                    _workItemOutcomes[key] = false;

                    // Ensure the final failure report includes test-only failures too.
                    if (!_failedWorkItemConsoleInfo.ContainsKey(key))
                    {
                        _failedWorkItemConsoleInfo[key] = new FailedWorkItemConsoleInfo(
                            job.DisplayName,
                            entry.Key.WorkItemName,
                            "Failed (AzDO tests)",
                            "see Azure DevOps test run results");
                    }
                }
            }
        }

        /// <summary>
        /// Returns true if this is the first time a console-link warning is being emitted for
        /// the given (jobName, workItemName) key. Used to deduplicate console-link logging.
        /// </summary>
        public bool TryReportFailedWorkItemConsoleLink(string deduplicationKey)
        {
            lock (_sync)
            {
                return _reportedFailedWorkItemConsoleLinks.Add(deduplicationKey);
            }
        }

        public void SetTimelineRecords(IEnumerable<AzureDevOpsTimelineRecord> records)
        {
            lock (_sync)
            {
                _latestTimelineRecords.Clear();
                _latestTimelineRecords.AddRange(records);
            }
        }

        public IReadOnlyList<AzureDevOpsTimelineRecord> SnapshotTimelineRecords()
        {
            lock (_sync)
            {
                return [.._latestTimelineRecords];
            }
        }

        /// <summary>
        /// Records a single successful resubmission: bumps the resubmitted job/work-item
        /// counters and (when non-empty) adds the AzDO submitter job name to the set excluded
        /// from the non-monitor failure check.
        /// </summary>
        public void RecordResubmission(string submitterJobName, int resubmittedWorkItemCount)
        {
            lock (_sync)
            {
                _resubmittedJobCount++;
                _resubmittedWorkItemCount += resubmittedWorkItemCount;
                if (!string.IsNullOrEmpty(submitterJobName))
                {
                    _retryingHelixSubmitterJobs.Add(submitterJobName);
                }
            }
        }

        public IReadOnlySet<string> SnapshotRetryingHelixSubmitterJobs()
        {
            lock (_sync)
            {
                return new HashSet<string>(_retryingHelixSubmitterJobs, StringComparer.OrdinalIgnoreCase);
            }
        }

        public IReadOnlyList<FailedWorkItemConsoleInfo> SnapshotFailedWorkItemConsoleInfo()
        {
            lock (_sync)
            {
                return [.._failedWorkItemConsoleInfo.Values];
            }
        }

        /// <summary>
        /// Produces a key that rolls up work-item outcomes within a logical AzDO submitter
        /// chain. When the job carries an AzDO <c>System.JobName</c>, the chain key is based
        /// on that name combined with the Helix <c>QueueId</c> (so resubmissions of the same
        /// AzDO job to the same queue share the same key while a single AzDO matrix leg that
        /// fans out to multiple Helix queues — each producing its own Helix job under the
        /// same <c>System.JobName</c> — stays distinct and cannot overwrite a sibling queue's
        /// failure with a pass). When there is no submitter name (test scenarios, manual
        /// Helix submissions), the chain is followed back through <c>PreviousHelixJobName</c>
        /// links to the root and the root Helix job name is used instead, so that retries
        /// still overwrite prior failures correctly.
        /// </summary>
        public string GetSubmitterChainKey(HelixJobInfo job)
        {
            lock (_sync)
            {
                return GetSubmitterChainKeyLocked(job);
            }
        }

        private string GetSubmitterChainKeyLocked(HelixJobInfo job)
        {
            if (!string.IsNullOrEmpty(job.SubmitterJobName))
            {
                return FormatSubmitterChainKey(job.SubmitterJobName, job.QueueId);
            }

            HelixJobInfo current = job;
            var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            while (current is not null
                && !string.IsNullOrEmpty(current.PreviousHelixJobName)
                && visited.Add(current.JobName))
            {
                if (!_associatedJobs.TryGetValue(current.PreviousHelixJobName, out HelixJobInfo previous))
                {
                    return $"helix:{current.PreviousHelixJobName}";
                }

                if (!string.IsNullOrEmpty(previous.SubmitterJobName))
                {
                    return FormatSubmitterChainKey(previous.SubmitterJobName, previous.QueueId);
                }

                current = previous;
            }

            return $"helix:{(current?.JobName ?? job.JobName)}";
        }

        private static string FormatSubmitterChainKey(string submitterJobName, string queueId)
            => string.IsNullOrEmpty(queueId)
                ? $"submitter:{submitterJobName}"
                : $"submitter:{submitterJobName}|queue:{queueId}";

        /// <summary>
        /// From an arbitrary set of Helix jobs return only the leaves of each lineage chain —
        /// jobs that are not pointed at by any other job's <c>PreviousHelixJobName</c>.
        /// </summary>
        public static IReadOnlyList<HelixJobInfo> GetLatestHelixJobAttempts(IEnumerable<HelixJobInfo> jobs)
        {
            var supersededJobNames = new HashSet<string>(
                jobs.Select(j => j.PreviousHelixJobName)
                    .Where(previousJobName => !string.IsNullOrEmpty(previousJobName)),
                StringComparer.OrdinalIgnoreCase);

            return [.. jobs.Where(j => !supersededJobNames.Contains(j.JobName))];
        }

        /// <summary>
        /// Orders Helix jobs from oldest incarnation to newest by following the
        /// <c>PreviousHelixJobName</c> link backwards. Used to ensure upload and outcome
        /// reconciliation process lineage in the right order (older first, so newer
        /// incarnations supersede older ones).
        /// </summary>
        public static IReadOnlyList<HelixJobInfo> OrderHelixJobsOldToNew(IEnumerable<HelixJobInfo> jobs)
        {
            var jobByName = jobs.ToDictionary(j => j.JobName, StringComparer.OrdinalIgnoreCase);
            return
            [
                ..jobs
                    .OrderBy(j => GetLineageDepth(j, jobByName))
                    .ThenBy(j => j.JobName, StringComparer.OrdinalIgnoreCase)
            ];
        }

        private static int GetLineageDepth(HelixJobInfo job, Dictionary<string, HelixJobInfo> jobByName)
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

        private void TrackFailedWorkItemConsoleInfoLocked(HelixJobInfo helixJob, string chainKey, WorkItemSummary workItem)
        {
            var key = (chainKey, workItem.Name);
            if (workItem.IsFailed)
            {
                _failedWorkItemConsoleInfo[key] = new FailedWorkItemConsoleInfo(
                    helixJob.DisplayName,
                    workItem.Name,
                    workItem.FormattedState,
                    GetConsoleOutputText(workItem.ConsoleOutputUri));
            }
            else
            {
                _failedWorkItemConsoleInfo.Remove(key);
            }
        }

        public static string GetConsoleOutputText(string consoleOutputUri)
            => string.IsNullOrEmpty(consoleOutputUri) ? "no console link available" : consoleOutputUri;
    }

    internal sealed record FailedWorkItemConsoleInfo(
        string JobName,
        string WorkItemName,
        string State,
        string ConsoleOutput);

    internal sealed class WorkItemOutcomeKeyComparer : IEqualityComparer<(string ChainKey, string WorkItemName)>
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
}
