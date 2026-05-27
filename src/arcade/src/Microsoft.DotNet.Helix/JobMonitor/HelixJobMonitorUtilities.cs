// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.JobMonitor
{
    public sealed class AzureDevOpsTimelineRecord
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("parentId")]
        public string ParentId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("refName")]
        public string ReferenceName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("result")]
        public string Result { get; set; }

        [JsonProperty("attempt")]
        public int Attempt { get; set; } = 1;

        [JsonProperty("previousAttempts")]
        public PreviousAttemptReference[] PreviousAttempts { get; set; }
    }

    public sealed class PreviousAttemptReference
    {
        [JsonProperty("attempt")]
        public int Attempt { get; set; }

        [JsonProperty("timelineId")]
        public string TimelineId { get; set; }

        [JsonProperty("recordId")]
        public string RecordId { get; set; }
    }

    public static class HelixJobMonitorUtilities
    {
        public static bool AreNonMonitorJobsComplete(IEnumerable<AzureDevOpsTimelineRecord> records, string jobMonitorName)
            => GetRelevantJobRecords(records, jobMonitorName).All(IsTerminal);

        public static bool HasFailedNonMonitorJobs(IEnumerable<AzureDevOpsTimelineRecord> records, string jobMonitorName)
            => HasFailedNonMonitorJobs(records, jobMonitorName, new HashSet<string>(StringComparer.OrdinalIgnoreCase));

        public static bool HasFailedNonMonitorJobs(
            IEnumerable<AzureDevOpsTimelineRecord> records,
            string jobMonitorName,
            IReadOnlySet<string> ignoredJobNames)
            => GetRelevantJobRecords(records, jobMonitorName)
                .Where(r => ignoredJobNames == null || !ignoredJobNames.Contains(r.ReferenceName))
                .Any(r =>
                    string.Equals(r.Result, "failed", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(r.Result, "canceled", StringComparison.OrdinalIgnoreCase));

        /// <summary>
        /// Returns the subset of <paramref name="records"/> that belongs to the pipeline stage
        /// named <paramref name="stageName"/>, including the Stage record itself and any
        /// descendant records (Phases, Jobs, Tasks). When the timeline contains no Stage records
        /// at all (single-stage build) every record is returned. When the timeline does contain
        /// Stage records but none match <paramref name="stageName"/>, an empty list is returned.
        /// </summary>
        public static IReadOnlyList<AzureDevOpsTimelineRecord> FilterRecordsToStage(
            IEnumerable<AzureDevOpsTimelineRecord> records,
            string stageName)
        {
            if (string.IsNullOrEmpty(stageName))
            {
                return [.. records];
            }

            List<AzureDevOpsTimelineRecord> all = records.ToList();
            List<AzureDevOpsTimelineRecord> stageRecords =
            [
                ..all.Where(r => string.Equals(r.Type, "Stage", StringComparison.OrdinalIgnoreCase))
            ];

            if (stageRecords.Count == 0)
            {
                // No stage records present in the timeline; treat the whole timeline as the
                // monitor's stage (single-stage build).
                return all;
            }

            AzureDevOpsTimelineRecord stageRoot = stageRecords.FirstOrDefault(r =>
                string.Equals(r.ReferenceName, stageName, StringComparison.OrdinalIgnoreCase));

            if (stageRoot == null)
            {
                return [];
            }

            // Iteratively collect all descendants of the stage record by following ParentId.
            var byParent = all
                .Where(r => !string.IsNullOrEmpty(r.ParentId))
                .ToLookup(r => r.ParentId, StringComparer.OrdinalIgnoreCase);

            var result = new List<AzureDevOpsTimelineRecord> { stageRoot };
            var queue = new Queue<string>();
            queue.Enqueue(stageRoot.Id);
            while (queue.Count > 0)
            {
                string parentId = queue.Dequeue();
                foreach (AzureDevOpsTimelineRecord child in byParent[parentId])
                {
                    result.Add(child);
                    if (!string.IsNullOrEmpty(child.Id))
                    {
                        queue.Enqueue(child.Id);
                    }
                }
            }

            return result;
        }

        private static IEnumerable<AzureDevOpsTimelineRecord> GetRelevantJobRecords(IEnumerable<AzureDevOpsTimelineRecord> records, string jobMonitorName)
        {
            List<AzureDevOpsTimelineRecord> allRecords = (records ?? []).ToList();
            HashSet<string> monitorRecordIds =
            [
                ..allRecords
                    .Where(r => IsMonitorRecord(r, jobMonitorName))
                    .Select(r => r.Id)
                    .Where(id => !string.IsNullOrEmpty(id))
            ];

            Dictionary<string, AzureDevOpsTimelineRecord> recordsById = allRecords
                .Where(r => !string.IsNullOrEmpty(r.Id))
                .GroupBy(r => r.Id, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);

            return allRecords
                .Where(r => string.Equals(r.Type, "Job", StringComparison.OrdinalIgnoreCase))
                .Where(r => !IsMonitorRecord(r, jobMonitorName))
                .Where(r => !HasMonitorAncestor(r, recordsById, monitorRecordIds));
        }

        private static bool IsTerminal(AzureDevOpsTimelineRecord record)
            => string.Equals(record?.State, "completed", StringComparison.OrdinalIgnoreCase);

        private static bool IsMonitorRecord(AzureDevOpsTimelineRecord record, string jobMonitorName)
        {
            return !string.IsNullOrEmpty(jobMonitorName)
                && (string.Equals(record.ReferenceName, jobMonitorName, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(record.Name, jobMonitorName, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(record.Identifier, jobMonitorName, StringComparison.OrdinalIgnoreCase));
        }

        private static bool HasMonitorAncestor(
            AzureDevOpsTimelineRecord record,
            IReadOnlyDictionary<string, AzureDevOpsTimelineRecord> recordsById,
            IReadOnlySet<string> monitorRecordIds)
        {
            string parentId = record.ParentId;
            var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            while (!string.IsNullOrEmpty(parentId) && visited.Add(parentId))
            {
                if (monitorRecordIds.Contains(parentId))
                {
                    return true;
                }

                if (!recordsById.TryGetValue(parentId, out AzureDevOpsTimelineRecord parent))
                {
                    return false;
                }

                parentId = parent.ParentId;
            }

            return false;
        }
    }
}
