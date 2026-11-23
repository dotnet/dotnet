// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.Helix.JobMonitor;
using Xunit;

namespace Microsoft.DotNet.Helix.Sdk.Tests;

public class HelixJobMonitorUtilitiesTests
{
    [Fact]
    public void AreNonMonitorJobsComplete_IgnoresMonitorRecord()
    {
        var records = new[]
        {
            new AzureDevOpsTimelineRecord { Type = "Job", ReferenceName = "Build Linux", State = "completed", Result = "succeeded" },
            new AzureDevOpsTimelineRecord { Type = "Job", ReferenceName = "Helix Job Monitor", State = "inProgress", Result = null },
        };

        Assert.True(HelixJobMonitorUtilities.AreNonMonitorJobsComplete(records, "Helix Job Monitor"));
    }

    [Fact]
    public void HasFailedNonMonitorJobs_DetectsFailures()
    {
        var records = new[]
        {
            new AzureDevOpsTimelineRecord { Type = "Job", ReferenceName = "Build Linux", State = "completed", Result = "failed" },
            new AzureDevOpsTimelineRecord { Type = "Job", ReferenceName = "Helix Job Monitor", State = "inProgress", Result = null },
        };

        Assert.True(HelixJobMonitorUtilities.HasFailedNonMonitorJobs(records, "Helix Job Monitor"));
    }

    [Fact]
    public void AbandonedJob_WithAgentLostError_IsCompleteAndFailed()
    {
        var records = new[]
        {
            new AzureDevOpsTimelineRecord
            {
                Id = "job1",
                Type = "Job",
                ReferenceName = "Build OSX",
                State = "inProgress",
                Result = null,
                Issues =
                [
                    new AzureDevOpsTimelineIssue
                    {
                        Type = "error",
                        Message = "We stopped hearing from agent Azure Pipelines 19. Verify the agent machine is running and has a healthy network connection.",
                    },
                ],
            },
            new AzureDevOpsTimelineRecord { Id = "monitor", Type = "Job", ReferenceName = "Helix Job Monitor", State = "inProgress" },
        };

        Assert.True(HelixJobMonitorUtilities.AreNonMonitorJobsComplete(records, "Helix Job Monitor"));
        Assert.True(HelixJobMonitorUtilities.HasFailedNonMonitorJobs(records, "Helix Job Monitor"));
        Assert.Equal("job1", Assert.Single(HelixJobMonitorUtilities.GetAbandonedNonMonitorJobRecords(records, "Helix Job Monitor")).Id);
    }

    [Fact]
    public void AbandonedJob_WithCompletedParentPhase_IsCompleteAndFailed()
    {
        var records = new[]
        {
            new AzureDevOpsTimelineRecord { Id = "phase1", Type = "Phase", State = "completed", Result = "canceled" },
            new AzureDevOpsTimelineRecord
            {
                Id = "job1",
                ParentId = "phase1",
                Type = "Job",
                ReferenceName = "Build Windows",
                State = "inProgress",
                Issues =
                [
                    new AzureDevOpsTimelineIssue { Type = "warning", Message = "Remote machine provider issue: Received request to deprovision." },
                ],
            },
            new AzureDevOpsTimelineRecord { Id = "monitor", Type = "Job", ReferenceName = "Helix Job Monitor", State = "inProgress" },
        };

        Assert.True(HelixJobMonitorUtilities.AreNonMonitorJobsComplete(records, "Helix Job Monitor"));
        Assert.True(HelixJobMonitorUtilities.HasFailedNonMonitorJobs(records, "Helix Job Monitor"));
    }

    [Fact]
    public void RunningJob_WithRunningPhaseAndNoAgentLostError_IsNotComplete()
    {
        var records = new[]
        {
            new AzureDevOpsTimelineRecord { Id = "phase1", Type = "Phase", State = "inProgress" },
            new AzureDevOpsTimelineRecord
            {
                Id = "job1",
                ParentId = "phase1",
                Type = "Job",
                ReferenceName = "Build Linux",
                State = "inProgress",
                Issues = [new AzureDevOpsTimelineIssue { Type = "error", Message = "Some unrelated error" }],
            },
            new AzureDevOpsTimelineRecord { Id = "monitor", Type = "Job", ReferenceName = "Helix Job Monitor", State = "inProgress" },
        };

        Assert.False(HelixJobMonitorUtilities.AreNonMonitorJobsComplete(records, "Helix Job Monitor"));
        Assert.False(HelixJobMonitorUtilities.HasFailedNonMonitorJobs(records, "Helix Job Monitor"));
        Assert.Empty(HelixJobMonitorUtilities.GetAbandonedNonMonitorJobRecords(records, "Helix Job Monitor"));
    }

    [Fact]
    public void AbandonedJob_IsIgnoredForFailureWhenRetrying()
    {
        var records = new[]
        {
            new AzureDevOpsTimelineRecord
            {
                Id = "job1",
                Type = "Job",
                ReferenceName = "Build OSX",
                State = "inProgress",
                Issues = [new AzureDevOpsTimelineIssue { Type = "error", Message = "We stopped hearing from agent Azure Pipelines 37." }],
            },
        };

        Assert.False(HelixJobMonitorUtilities.HasFailedNonMonitorJobs(
            records,
            "Helix Job Monitor",
            new System.Collections.Generic.HashSet<string>(System.StringComparer.OrdinalIgnoreCase) { "Build OSX" }));
    }
}
