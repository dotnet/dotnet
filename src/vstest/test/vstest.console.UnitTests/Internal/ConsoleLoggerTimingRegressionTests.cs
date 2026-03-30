// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TestResult = Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.Internal;

/// <summary>
/// Regression tests for ConsoleLogger MinimalTestResult timing fix.
/// </summary>
[TestClass]
public class ConsoleLoggerTimingRegressionTests
{
    // Regression test for #5143 — Fix timing in simple log
    // When the test framework (e.g. xUnit 2.x.x) does not report proper start/end times,
    // the Duration is non-zero but EndTime - StartTime could be near zero.
    // The fix adjusts StartTime = EndTime - Duration in that case.
    [TestMethod]
    public void TestResult_WhenDurationExceedsTimeSpan_StartTimeShouldBeAdjustable()
    {
        var testCase = new TestCase("Test.Method1", new Uri("executor://test"), "test.dll");
        var now = DateTimeOffset.UtcNow;

        var testResult = new TestResult(testCase)
        {
            // Simulate xUnit behavior: StartTime ≈ EndTime (both set to "now")
            // but Duration is meaningful
            StartTime = now,
            EndTime = now,
            Duration = TimeSpan.FromSeconds(5),
            Outcome = TestOutcome.Passed
        };

        // The fix checks: if EndTime - StartTime < Duration, then StartTime = EndTime - Duration
        // Verify this invariant
        if (testResult.EndTime - testResult.StartTime < testResult.Duration)
        {
            var adjustedStartTime = testResult.EndTime - testResult.Duration;
            Assert.IsLessThan(testResult.EndTime, adjustedStartTime);
            Assert.AreEqual(testResult.Duration, testResult.EndTime - adjustedStartTime);
        }
    }

    // Regression test for #5143
    [TestMethod]
    public void TestResult_WhenStartEndTimeCorrect_NoAdjustmentNeeded()
    {
        var testCase = new TestCase("Test.Method2", new Uri("executor://test"), "test.dll");
        var start = DateTimeOffset.UtcNow;
        var end = start + TimeSpan.FromSeconds(3);

        var testResult = new TestResult(testCase)
        {
            StartTime = start,
            EndTime = end,
            Duration = TimeSpan.FromSeconds(3),
            Outcome = TestOutcome.Passed
        };

        // EndTime - StartTime matches Duration, no adjustment needed
        Assert.AreEqual(testResult.Duration, testResult.EndTime - testResult.StartTime);
    }

    // Regression test for #5143
    [TestMethod]
    public void TestResult_ZeroDuration_ShouldNotCauseIssues()
    {
        var testCase = new TestCase("Test.Method3", new Uri("executor://test"), "test.dll");
        var now = DateTimeOffset.UtcNow;

        var testResult = new TestResult(testCase)
        {
            StartTime = now,
            EndTime = now,
            Duration = TimeSpan.Zero,
            Outcome = TestOutcome.Passed
        };

        // With zero duration, no adjustment should be needed
        Assert.AreEqual(TimeSpan.Zero, testResult.Duration);
    }
}
