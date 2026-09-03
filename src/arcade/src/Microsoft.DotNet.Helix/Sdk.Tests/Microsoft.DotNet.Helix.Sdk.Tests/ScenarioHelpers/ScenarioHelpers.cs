// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.Helix.JobMonitor;
using Microsoft.DotNet.Helix.JobMonitor.Models;

namespace Microsoft.DotNet.Helix.Sdk.Tests.ScenarioHelpers
{
    internal static class ScenarioHelpers
    {
        public const string DefaultMonitorName = "Helix Job Monitor";

        public static AzureDevOpsTimelineRecord StageRecord(string name, string id, string state, string result = null)
            => new() { Type = "Stage", ReferenceName = name, Id = id, State = state, Result = result };

        public static AzureDevOpsTimelineRecord PipelineJob(
            string name, string state, string result = null, int attempt = 1,
            PreviousAttemptReference[] previousAttempts = null, string parentId = null, string id = null)
            => new()
            {
                Type = "Job",
                ReferenceName = name,
                State = state,
                Result = result,
                Attempt = attempt,
                PreviousAttempts = previousAttempts,
                ParentId = parentId,
                Id = id ?? name,
            };

        public static AzureDevOpsTimelineRecord MonitorJob(
            string name = DefaultMonitorName, int attempt = 1,
            PreviousAttemptReference[] previousAttempts = null, string parentId = null)
            => new()
            {
                Type = "Job",
                ReferenceName = name,
                State = "inProgress",
                Attempt = attempt,
                PreviousAttempts = previousAttempts,
                ParentId = parentId,
                Id = name,
            };

        public static PreviousAttemptReference PreviousAttempt(int attempt, string timelineId = null, string recordId = null)
            => new() { Attempt = attempt, TimelineId = timelineId ?? $"timeline-attempt-{attempt}", RecordId = recordId ?? $"record-attempt-{attempt}" };

        public static HelixJobInfo HelixJob(
            string jobName,
            string status,
            string stageName = null,
            string submitterJobName = null,
            string submitterJobDisplayName = null,
            string queueId = null,
            string previousHelixJobName = null,
            int? initialWorkItemCount = null)
            => new(
                jobName,
                status,
                stageName: stageName,
                submitterJobName: submitterJobName,
                submitterJobDisplayName: submitterJobDisplayName,
                queueId: queueId,
                previousHelixJobName: previousHelixJobName,
                initialWorkItemCount: initialWorkItemCount);

        public static HelixJobPassFail PassFail(string[] passed = null, string[] failed = null)
            => new(passed ?? [], failed ?? []);
    }
}
