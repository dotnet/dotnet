// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using Microsoft.Build.Framework;
using Xunit;

namespace Microsoft.DotNet.Helix.Sdk.Tests
{
    public class QueueStatsLoggingTests
    {
        // Exercises the callback pair SendHelixJob hands to JobDefinition.SendAsync. Routine
        // submission progress must always stay at Normal; the opt-in queue-health summary must be
        // elevated to High only when EnableShowHelixQueueStats is set, so it survives the default
        // 'Minimal' build verbosity without turning routine output into noise.
        private static List<(string Message, MessageImportance Importance)> CaptureLogs(
            bool enableShowHelixQueueStats)
        {
            var captured = new List<(string, MessageImportance)>();
            var (logNormal, logQueueStats) = SendHelixJob.CreateSubmissionLoggers(
                enableShowHelixQueueStats,
                (msg, importance) => captured.Add((msg, importance)));

            logNormal("submitting payload");
            logQueueStats("Helix queue 'test' health:");
            return captured;
        }

        [Fact]
        public void QueueStatsSummary_LogsAtHigh_WhenEnabled()
        {
            var captured = CaptureLogs(enableShowHelixQueueStats: true);

            Assert.Equal(MessageImportance.Normal, captured[0].Importance);
            Assert.Equal(MessageImportance.High, captured[1].Importance);
        }

        [Fact]
        public void QueueStatsSummary_StaysAtNormal_WhenDisabled()
        {
            var captured = CaptureLogs(enableShowHelixQueueStats: false);

            Assert.Equal(MessageImportance.Normal, captured[0].Importance);
            Assert.Equal(MessageImportance.Normal, captured[1].Importance);
        }
    }
}
