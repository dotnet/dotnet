// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.Helix.JobMonitor;
using Xunit;

namespace Microsoft.DotNet.Helix.Sdk.Tests
{
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
    }
}
