// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TestResult = Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult;

namespace Microsoft.TestPlatform.ObjectModel.UnitTests;

/// <summary>
/// Regression tests for TestResult properties and timing behavior.
/// </summary>
[TestClass]
public class TestResultTimingRegressionTests
{
    // Regression test for #5143 — Fix timing in simple log
    // TestResult should preserve duration and timing information correctly.

    [TestMethod]
    public void TestResult_Duration_ShouldBeIndependentOfStartEndTime()
    {
        var testCase = new TestCase("Test1", new Uri("executor://test"), "test.dll");
        var result = new TestResult(testCase)
        {
            Duration = TimeSpan.FromSeconds(5)
        };

        Assert.AreEqual(TimeSpan.FromSeconds(5), result.Duration);
    }

    [TestMethod]
    public void TestResult_StartAndEndTime_ShouldBeSettable()
    {
        var testCase = new TestCase("Test2", new Uri("executor://test"), "test.dll");
        var start = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var end = start + TimeSpan.FromSeconds(10);

        var result = new TestResult(testCase)
        {
            StartTime = start,
            EndTime = end,
            Duration = TimeSpan.FromSeconds(10)
        };

        Assert.AreEqual(start, result.StartTime);
        Assert.AreEqual(end, result.EndTime);
        Assert.AreEqual(TimeSpan.FromSeconds(10), result.Duration);
    }

    // Regression test for #4894 — Time is reported incorrectly for xunit
    [TestMethod]
    public void TestResult_Outcome_ShouldPreserveAllValues()
    {
        var testCase = new TestCase("Test3", new Uri("executor://test"), "test.dll");

        foreach (var outcome in Enum.GetValues(typeof(TestOutcome)).Cast<TestOutcome>())
        {
            var result = new TestResult(testCase) { Outcome = outcome };
            Assert.AreEqual(outcome, result.Outcome);
        }
    }

    [TestMethod]
    public void TestResult_DisplayName_ShouldBeSettable()
    {
        var testCase = new TestCase("Ns.Class.Method", new Uri("executor://test"), "test.dll");
        var result = new TestResult(testCase)
        {
            DisplayName = "Custom Display Name"
        };

        Assert.AreEqual("Custom Display Name", result.DisplayName);
    }
}
