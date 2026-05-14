// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.CommandLine;

[TestClass]
public class TestRunResultAggregatorTests
{
    private readonly TestRunResultAggregator _resultAggregator = TestRunResultAggregator.Instance;
    private readonly Mock<ITestRunRequest> _mockTestRunRequest;

    public TestRunResultAggregatorTests()
    {
        _resultAggregator.Reset();
        _mockTestRunRequest = new Mock<ITestRunRequest>();
        _resultAggregator.RegisterTestRunEvents(_mockTestRunRequest.Object);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        _resultAggregator.UnregisterTestRunEvents(_mockTestRunRequest.Object);
    }

    [TestMethod]
    public void DefaultOutcomeIsPassed()
    {
        Assert.AreEqual(TestOutcome.Passed, _resultAggregator.Outcome);
    }

    [TestMethod]
    public void MarkTestRunFailedSetsOutcomeToFailed()
    {
        _resultAggregator.MarkTestRunFailed();
        Assert.AreEqual(TestOutcome.Failed, _resultAggregator.Outcome);
    }

    [TestMethod]
    public void TestRunMessageHandlerForMessageLevelErrorSetsOutcomeToFailed()
    {
        var messageArgs = new TestRunMessageEventArgs(TestMessageLevel.Error, "bad stuff");
        _mockTestRunRequest.Raise(tr => tr.TestRunMessage += null, messageArgs);
        Assert.AreEqual(TestOutcome.Failed, _resultAggregator.Outcome);
    }

    [TestMethod]
    public void TestRunCompletionHandlerForTestRunStatisticsNullSetsOutcomeToFailed()
    {
        var messageArgs = new TestRunCompleteEventArgs(null, false, false, null, null, null, new TimeSpan());
        _mockTestRunRequest.Raise(tr => tr.OnRunCompletion += null, messageArgs);
        Assert.AreEqual(TestOutcome.Failed, _resultAggregator.Outcome);
    }

    [TestMethod]
    public void TestRunCompletionHandlerForTestRunStatsWithOneOrMoreFailingTestsSetsOutcomeToFailed()
    {
        var testOutcomeDict = new Dictionary<TestOutcome, long>
        {
            { TestOutcome.Failed, 1 }
        };
        var stats = new TestableTestRunStats(testOutcomeDict);

        var messageArgs = new TestRunCompleteEventArgs(stats, false, false, null, null, null, new TimeSpan());
        _mockTestRunRequest.Raise(tr => tr.OnRunCompletion += null, messageArgs);
        Assert.AreEqual(TestOutcome.Failed, _resultAggregator.Outcome);
    }

    [TestMethod]
    public void TestRunCompletionHandlerForCanceledRunShouldSetsOutcomeToFailed()
    {
        var testOutcomeDict = new Dictionary<TestOutcome, long>
        {
            { TestOutcome.Passed, 1 }
        };
        var stats = new TestableTestRunStats(testOutcomeDict);

        var messageArgs = new TestRunCompleteEventArgs(stats, true, false, null, null, null, new TimeSpan());
        _mockTestRunRequest.Raise(tr => tr.OnRunCompletion += null, messageArgs);
        Assert.AreEqual(TestOutcome.Failed, _resultAggregator.Outcome);
    }

    [TestMethod]
    public void TestRunCompletionHandlerForAbortedRunShouldSetsOutcomeToFailed()
    {
        var testOutcomeDict = new Dictionary<TestOutcome, long>
        {
            { TestOutcome.Passed, 1 }
        };
        var stats = new TestableTestRunStats(testOutcomeDict);

        var messageArgs = new TestRunCompleteEventArgs(stats, false, true, null, null, null, new TimeSpan());
        _mockTestRunRequest.Raise(tr => tr.OnRunCompletion += null, messageArgs);
        Assert.AreEqual(TestOutcome.Failed, _resultAggregator.Outcome);
    }

    #region Implementation

    private class TestableTestRunStats : ITestRunStatistics
    {
        public TestableTestRunStats(Dictionary<TestOutcome, long> stats)
        {
            Stats = stats;
        }

        public long ExecutedTests { get; set; }

        /// <summary>
        /// Gets the test stats which is the test outcome versus its state.
        /// </summary>
        [DataMember]
        public IDictionary<TestOutcome, long> Stats { get; private set; }

        /// <summary>
        /// Gets the number of tests with a specified outcome.
        /// </summary>
        /// <param name="testOutcome"> The test outcome. </param>
        /// <returns> The number of tests with this outcome. </returns>
        public long this[TestOutcome testOutcome]
        {
            get
            {
                return Stats.TryGetValue(testOutcome, out var count) ? count : 0;
            }
        }
    }

    #endregion
}
