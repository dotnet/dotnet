// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using AwesomeAssertions;
using Microsoft.DotNet.Helix.Client.Models;
using Microsoft.DotNet.Helix.Sdk;
using Xunit;

namespace Microsoft.DotNet.Helix.Sdk.Tests
{
    public class WaitForHelixJobCompletionTests
    {
        [Fact]
        public void SystemWorkItemOnlyDoesNotCompleteJob()
        {
            JobDetails jobDetails = CreateJobDetails(initialWorkItemCount: 1);
            IReadOnlyCollection<WorkItemSummary> realWorkItems = WaitForHelixJobCompletion.GetRealWorkItems(
            [
                CreateWorkItem(WaitForHelixJobCompletion.HelixControllerWorkQueueingWorkItemName, exitCode: 0),
            ]);

            realWorkItems.Should().BeEmpty();
            WaitForHelixJobCompletion.IsJobComplete(jobDetails, realWorkItems).Should().BeFalse();
        }

        [Fact]
        public void CompletesWhenAllExpectedRealWorkItemsHaveExitCodes()
        {
            JobDetails jobDetails = CreateJobDetails(initialWorkItemCount: 2);
            IReadOnlyCollection<WorkItemSummary> realWorkItems = WaitForHelixJobCompletion.GetRealWorkItems(
            [
                CreateWorkItem(WaitForHelixJobCompletion.HelixControllerWorkQueueingWorkItemName, exitCode: 0),
                CreateWorkItem("work-item-1", exitCode: 0),
                CreateWorkItem("work-item-2", exitCode: 1),
            ]);

            realWorkItems.Should().HaveCount(2);
            WaitForHelixJobCompletion.IsJobComplete(jobDetails, realWorkItems).Should().BeTrue();
        }

        [Fact]
        public void FinishedJobDetailsCompletesJob()
        {
            JobDetails jobDetails = CreateJobDetails(initialWorkItemCount: null, finished: "2026-07-16T00:00:00Z");

            WaitForHelixJobCompletion.IsJobComplete(jobDetails, []).Should().BeTrue();
        }

        private static JobDetails CreateJobDetails(int? initialWorkItemCount, string finished = null)
            => new("job-list", null, "job-name", "wait", "source", "type", "build")
            {
                InitialWorkItemCount = initialWorkItemCount,
                Finished = finished,
            };

        private static WorkItemSummary CreateWorkItem(string name, int? exitCode)
            => new($"details/{name}", "job-name", name, "Finished")
            {
                ExitCode = exitCode,
            };
    }
}
