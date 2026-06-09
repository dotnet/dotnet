// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.DotNet.Helix.Client.Models;
using Microsoft.DotNet.Helix.JobMonitor.Models;

namespace Microsoft.DotNet.Helix.JobMonitor
{
    /// <summary>
    /// Mutable container for every piece of runtime state the monitor accumulates while
    /// observing a single invocation. Helpers (status reporter, upload queue, timeout
    /// reporter) receive this by reference and read/update its fields directly so that the
    /// runner's main loop is the only place that drives the lifecycle.
    /// </summary>
    internal sealed class MonitorState
    {
        /// <summary>
        /// All Helix jobs the monitor has observed for this build, keyed by Helix job name.
        /// Overwritten per poll so the cached entry reflects the latest Helix-side state
        /// (in particular the <c>Finished</c> timestamp transitioning from null to a value).
        /// </summary>
        public Dictionary<string, HelixJobInfo> AssociatedJobs { get; } = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Cache of every Helix job we have seen this run, indexed by job name, so that
        /// <see cref="GetSubmitterChainKey"/> can walk back through <c>PreviousHelixJobName</c>
        /// links across polls.
        /// </summary>
        public Dictionary<string, HelixJobInfo> KnownJobsByName { get; } = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Helix jobs whose results have been uploaded to Azure DevOps in this or a prior
        /// monitor invocation. Seeded on entry from the AzDO test-run name markers.
        /// </summary>
        public HashSet<string> ProcessedHelixJobs { get; } = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Tracks the latest outcome for each logical work item, keyed by
        /// (SubmitterChainKey, WorkItemName). The chain key (not just the work-item name)
        /// is used so that two AzDO jobs which happen to run identically-named Helix work
        /// items do not overwrite each other's outcomes. Within a single AzDO submitter
        /// chain, a resubmission still overwrites a prior failure for the same work-item
        /// name because resubmitted Helix jobs inherit <c>System.JobName</c>.
        /// </summary>
        public Dictionary<(string ChainKey, string WorkItemName), bool> WorkItemOutcomes { get; }
            = new(WorkItemOutcomeKeyComparer.Instance);

        /// <summary>
        /// Helix job names whose per-work-item outcomes have already been reconciled into
        /// <see cref="WorkItemOutcomes"/>. Prevents the second reconciliation pass from
        /// re-processing jobs that were observed in an earlier poll.
        /// </summary>
        public HashSet<string> WorkItemOutcomeJobs { get; } = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Latest known console-link information for every failed work item, keyed the same
        /// way as <see cref="WorkItemOutcomes"/>. Cleared per key when a later incarnation
        /// passes. Used to build the final aggregated failure report.
        /// </summary>
        public Dictionary<(string ChainKey, string WorkItemName), FailedWorkItemConsoleInfo> FailedWorkItemConsoleInfo { get; }
            = new(WorkItemOutcomeKeyComparer.Instance);

        /// <summary>
        /// Deduplication set for the per-failure console-link warnings emitted during
        /// status logs and upload, so we don't spam the same link.
        /// </summary>
        public HashSet<string> ReportedFailedWorkItemConsoleLinks { get; } = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Last observed AzDO timeline records (scoped to the monitor's stage). Refreshed
        /// every poll and consumed by the timeout report.
        /// </summary>
        public List<AzureDevOpsTimelineRecord> LatestTimelineRecords { get; } = [];

        /// <summary>
        /// AzDO submitter job names whose Helix work was resubmitted during the one-shot
        /// retry pass. These jobs are excluded from the AzDO non-monitor failure check while
        /// the current invocation runs (the failure is represented by the resubmitted work).
        /// </summary>
        public HashSet<string> RetryingHelixSubmitterJobs { get; } = new(StringComparer.OrdinalIgnoreCase);

        public int ResubmittedJobCount { get; set; }
        public int ResubmittedWorkItemCount { get; set; }
        public int ProcessedJobCount { get; set; }

        public int FailedWorkItemCount => WorkItemOutcomes.Values.Count(passed => !passed);

        public bool HasFailedWorkItem => WorkItemOutcomes.Values.Any(passed => !passed);

        /// <summary>
        /// Record a freshly-seen set of jobs into the per-poll and cross-poll caches.
        /// </summary>
        public void ObserveJobs(IEnumerable<HelixJobInfo> jobs)
        {
            foreach (HelixJobInfo job in jobs)
            {
                AssociatedJobs[job.JobName] = job;
                KnownJobsByName[job.JobName] = job;
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
                if (!KnownJobsByName.TryGetValue(current.PreviousHelixJobName, out HelixJobInfo previous))
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

        /// <summary>
        /// Tracks (or removes) the per-failure console-info record for a single observed
        /// work item. Removal happens when a later incarnation passes.
        /// </summary>
        public void TrackFailedWorkItemConsoleInfo(HelixJobInfo helixJob, string chainKey, WorkItemSummary workItem)
        {
            var key = (chainKey, workItem.Name);
            if (workItem.IsFailed)
            {
                FailedWorkItemConsoleInfo[key] = new FailedWorkItemConsoleInfo(
                    helixJob.DisplayName,
                    workItem.Name,
                    workItem.FormattedState,
                    GetConsoleOutputText(workItem.ConsoleOutputUri));
            }
            else
            {
                FailedWorkItemConsoleInfo.Remove(key);
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
