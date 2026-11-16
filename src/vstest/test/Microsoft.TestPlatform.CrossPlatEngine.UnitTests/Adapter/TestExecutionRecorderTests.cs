// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using TestPlatform.CrossPlatEngine.UnitTests.TestableImplementations;

namespace TestPlatform.CrossPlatEngine.UnitTests.Adapter;

[TestClass]
public class TestExecutionRecorderTests
{
    private readonly TestableTestRunCache _testableTestRunCache;
    private readonly Mock<ITestCaseEventsHandler> _mockTestCaseEventsHandler;
    private readonly TestExecutionRecorder _testRecorder, _testRecorderWithTestEventsHandler;
    private readonly TestCase _testCase;
    private readonly Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult _testResult;

    public TestExecutionRecorderTests()
    {
        _testableTestRunCache = new TestableTestRunCache();
        _testRecorder = new TestExecutionRecorder(null, _testableTestRunCache);

        _testCase = new TestCase("A.C.M", new Uri("executor://dummy"), "A");
        _testResult = new Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult(_testCase);

        _mockTestCaseEventsHandler = new Mock<ITestCaseEventsHandler>();
        _testRecorderWithTestEventsHandler = new TestExecutionRecorder(_mockTestCaseEventsHandler.Object, _testableTestRunCache);

    }

    [TestMethod]
    public void AttachmentsShouldReturnEmptyListByDefault()
    {
        var attachments = _testRecorder.Attachments;

        Assert.IsNotNull(attachments);
        Assert.IsEmpty(attachments);
    }

    [TestMethod]
    public void RecordStartShouldUpdateTestRunCache()
    {
        _testRecorder.RecordStart(_testCase);
        Assert.Contains(_testCase, _testableTestRunCache.TestStartedList);
    }

    [TestMethod]
    public void RecordResultShouldUpdateTestRunCache()
    {
        _testRecorder.RecordResult(_testResult);
        Assert.Contains(_testResult, _testableTestRunCache.TestResultList);
    }

    [TestMethod]
    public void RecordEndShouldUpdateTestRunCache()
    {
        _testRecorder.RecordEnd(_testCase, TestOutcome.Passed);
        Assert.Contains(_testCase, _testableTestRunCache.TestCompletedList);
    }

    [TestMethod]
    public void RecordAttachmentsShouldAddToAttachmentSet()
    {
        var attachmentSet = new List<AttachmentSet> { new(new Uri("attachment://dummy"), "attachment") };

        _testRecorder.RecordAttachments(attachmentSet);

        var attachments = _testRecorder.Attachments;

        Assert.IsNotNull(attachments);
        CollectionAssert.AreEqual(attachmentSet, attachments);
    }

    [TestMethod]
    public void RecordAttachmentsShouldAddToAttachmentSetForMultipleAttachments()
    {
        var attachmentSet = new List<AttachmentSet>
        {
            new(new Uri("attachment://dummy"), "attachment"),
            new(new Uri("attachment://infinite"), "infinity")
        };

        _testRecorder.RecordAttachments(attachmentSet);

        var attachments = _testRecorder.Attachments;

        Assert.IsNotNull(attachments);
        CollectionAssert.AreEqual(attachmentSet, attachments);

        var newAttachmentSet = new AttachmentSet(new Uri("attachment://median"), "mid");
        attachmentSet.Add(newAttachmentSet);

        _testRecorder.RecordAttachments(new List<AttachmentSet> { newAttachmentSet });

        attachments = _testRecorder.Attachments;

        Assert.IsNotNull(attachments);
        CollectionAssert.AreEqual(attachmentSet, attachments);
    }

    #region TestCaseResult caching tests.
    [TestMethod]
    public void RecordStartShouldInvokeSendTestCaseStart()
    {
        _testRecorderWithTestEventsHandler.RecordStart(_testCase);

        _mockTestCaseEventsHandler.Verify(x => x.SendTestCaseStart(_testCase), Times.Once);
    }

    [TestMethod]
    public void RecordEndShouldInovkeTestCaseEndEventOnlyIfTestCaseStartWasCalledBefore()
    {
        _testRecorderWithTestEventsHandler.RecordStart(_testCase);
        _testRecorderWithTestEventsHandler.RecordEnd(_testCase, TestOutcome.Passed);

        _mockTestCaseEventsHandler.Verify(x => x.SendTestCaseEnd(_testCase, TestOutcome.Passed), Times.Once);
    }

    [TestMethod]
    public void RecordEndShouldNotInovkeTestCaseEndEventIfTestCaseStartWasNotCalledBefore()
    {
        _testRecorderWithTestEventsHandler.RecordEnd(_testCase, TestOutcome.Passed);

        _mockTestCaseEventsHandler.Verify(x => x.SendTestCaseEnd(_testCase, TestOutcome.Passed), Times.Never);
    }

    [TestMethod]
    public void RecordEndShouldNotInvokeTestCaseEndEventInCaseOfAMissingTestCaseStartInDataDrivenScenario()
    {
        _testRecorderWithTestEventsHandler.RecordStart(_testCase);
        _testRecorderWithTestEventsHandler.RecordEnd(_testCase, TestOutcome.Passed);
        _testRecorderWithTestEventsHandler.RecordEnd(_testCase, TestOutcome.Failed);

        _mockTestCaseEventsHandler.Verify(x => x.SendTestCaseEnd(_testCase, TestOutcome.Passed), Times.Once);
        _mockTestCaseEventsHandler.Verify(x => x.SendTestCaseEnd(_testCase, TestOutcome.Failed), Times.Never);
    }

    [TestMethod]
    public void RecordEndShouldInvokeSendTestCaseEndMultipleTimesInDataDrivenScenario()
    {
        _testRecorderWithTestEventsHandler.RecordStart(_testCase);
        _testRecorderWithTestEventsHandler.RecordEnd(_testCase, TestOutcome.Passed);
        _testRecorderWithTestEventsHandler.RecordStart(_testCase);
        _testRecorderWithTestEventsHandler.RecordEnd(_testCase, TestOutcome.Passed);

        _mockTestCaseEventsHandler.Verify(x => x.SendTestCaseEnd(_testCase, TestOutcome.Passed), Times.Exactly(2));
    }

    [TestMethod]
    public void RecordStartAndRecordEndShouldForwardAllTestCaseStartAndEndEvents()
    {
        _testRecorderWithTestEventsHandler.RecordStart(_testCase);
        _testRecorderWithTestEventsHandler.RecordStart(_testCase);
        _testRecorderWithTestEventsHandler.RecordEnd(_testCase, TestOutcome.Passed);
        _testRecorderWithTestEventsHandler.RecordEnd(_testCase, TestOutcome.Passed);

        _mockTestCaseEventsHandler.Verify(x => x.SendTestCaseStart(_testCase), Times.Exactly(2));
        _mockTestCaseEventsHandler.Verify(x => x.SendTestCaseEnd(_testCase, TestOutcome.Passed), Times.Exactly(2));
    }

    [TestMethod]
    public void RecordStartAndRecordEndShouldDeduplicateEventsWhenMultipleTestCaseEventsAreDisabled()
    {
        var featureFlag = new Mock<IFeatureFlag>();
        featureFlag
            .Setup(x => x.IsSet(FeatureFlag.VSTEST_DISABLE_MULTIPLE_TESTCASE_EVENTS))
            .Returns(true);
        var testRecorder = new TestExecutionRecorder(_mockTestCaseEventsHandler.Object, _testableTestRunCache, featureFlag.Object);

        testRecorder.RecordStart(_testCase);
        testRecorder.RecordStart(_testCase);
        testRecorder.RecordEnd(_testCase, TestOutcome.Passed);
        testRecorder.RecordEnd(_testCase, TestOutcome.Passed);

        _mockTestCaseEventsHandler.Verify(x => x.SendTestCaseStart(_testCase), Times.Once);
        _mockTestCaseEventsHandler.Verify(x => x.SendTestCaseEnd(_testCase, TestOutcome.Passed), Times.Once);
    }

    [TestMethod]
    public void RecordResultShouldNotSendSecondTestCaseEndWhenMultipleTestCaseEventsAreDisabled()
    {
        // With the flag on, the deduplicated start must not leave the result safety net armed:
        // Start(tc) → Start(tc, deduplicated) → End(tc) → Result(tc) sends one of each.
        var featureFlag = new Mock<IFeatureFlag>();
        featureFlag
            .Setup(x => x.IsSet(FeatureFlag.VSTEST_DISABLE_MULTIPLE_TESTCASE_EVENTS))
            .Returns(true);
        var testRecorder = new TestExecutionRecorder(_mockTestCaseEventsHandler.Object, _testableTestRunCache, featureFlag.Object);
        _testResult.Outcome = TestOutcome.Passed;

        testRecorder.RecordStart(_testCase);
        testRecorder.RecordStart(_testCase);
        testRecorder.RecordEnd(_testCase, TestOutcome.Passed);
        testRecorder.RecordResult(_testResult);

        _mockTestCaseEventsHandler.Verify(x => x.SendTestCaseStart(_testCase), Times.Once);
        _mockTestCaseEventsHandler.Verify(x => x.SendTestCaseEnd(_testCase, TestOutcome.Passed), Times.Once);
        _mockTestCaseEventsHandler.Verify(x => x.SendTestResult(_testResult), Times.Once);
    }

    [TestMethod]
    public void RecordStartAndRecordEndShouldSendEventsForNestedDataDrivenTestsWithSameId()
    {
        // Simulate a data-driven scenario where the parent test and its row executions
        // share the same TestCase.Id (same fully-qualified name), with nested Start calls:
        // Start(parent) → Start(row1) → End(row1) → Start(row2) → End(row2) → End(parent)
        _testRecorderWithTestEventsHandler.RecordStart(_testCase);   // parent start
        _testRecorderWithTestEventsHandler.RecordStart(_testCase);   // row1 start (same ID)
        _testRecorderWithTestEventsHandler.RecordEnd(_testCase, TestOutcome.Passed);   // row1 end
        _testRecorderWithTestEventsHandler.RecordStart(_testCase);   // row2 start (same ID)
        _testRecorderWithTestEventsHandler.RecordEnd(_testCase, TestOutcome.Passed);   // row2 end
        _testRecorderWithTestEventsHandler.RecordEnd(_testCase, TestOutcome.Passed);   // parent end

        _mockTestCaseEventsHandler.Verify(x => x.SendTestCaseStart(_testCase), Times.Exactly(3));
        _mockTestCaseEventsHandler.Verify(x => x.SendTestCaseEnd(_testCase, TestOutcome.Passed), Times.Exactly(3));
    }

    [TestMethod]
    public void RecordResultShouldPublishTestResultIfRecordStartAndRecordEndEventsAreNotPublished()
    {
        _testRecorderWithTestEventsHandler.RecordResult(_testResult);

        _mockTestCaseEventsHandler.Verify(x => x.SendTestResult(_testResult), Times.Once);
    }

    [TestMethod]
    public void RecordResultShouldPublishTestCaseResultEventIfTestCaseStartAndTestCaseEndEventsArePublished()
    {
        _testRecorderWithTestEventsHandler.RecordStart(_testCase);
        _testRecorderWithTestEventsHandler.RecordEnd(_testCase, TestOutcome.Passed);
        _testRecorderWithTestEventsHandler.RecordResult(_testResult);

        _mockTestCaseEventsHandler.Verify(x => x.SendTestResult(_testResult), Times.Once);
    }

    [TestMethod]
    public void RecordResultShouldFlushIfRecordEndWasCalledBefore()
    {
        _testRecorderWithTestEventsHandler.RecordStart(_testCase);
        _testRecorderWithTestEventsHandler.RecordEnd(_testCase, TestOutcome.Passed);
        _testRecorderWithTestEventsHandler.RecordResult(_testResult);

        _mockTestCaseEventsHandler.Verify(x => x.SendTestCaseEnd(_testCase, TestOutcome.Passed), Times.Once);
        Assert.Contains(_testResult, _testableTestRunCache.TestResultList);
    }

    [TestMethod]
    public void RecordResultShouldSendTestCaseEndEventAndFlushIfRecordEndWasCalledAfterRecordResult()
    {
        _testResult.Outcome = TestOutcome.Passed;
        _testRecorderWithTestEventsHandler.RecordStart(_testCase);
        _testRecorderWithTestEventsHandler.RecordResult(_testResult);
        _testRecorderWithTestEventsHandler.RecordEnd(_testCase, _testResult.Outcome);

        _mockTestCaseEventsHandler.Verify(x => x.SendTestCaseEnd(_testCase, TestOutcome.Passed), Times.Once);
        Assert.Contains(_testResult, _testableTestRunCache.TestResultList);
    }

    [TestMethod]
    public void RecordResultShouldSendTestCaseEndEventIfRecordEndWasNotCalled()
    {
        _testResult.Outcome = TestOutcome.Passed;
        _testRecorderWithTestEventsHandler.RecordStart(_testCase);
        _testRecorderWithTestEventsHandler.RecordResult(_testResult);

        _mockTestCaseEventsHandler.Verify(x => x.SendTestCaseEnd(_testCase, TestOutcome.Passed), Times.Once);
        Assert.Contains(_testResult, _testableTestRunCache.TestResultList);
    }

    [TestMethod]
    public void RecordResultShouldNotSendSpuriousTestCaseEndForParentInNestedDataDrivenScenario()
    {
        // Regression test for the nested data-driven scenario where the parent test and its row
        // executions share the same TestCase.Id (same fully-qualified name).
        //
        // Pattern: Start(parent) → Start(row1) → End(row1) → Result(row1) → End(parent)
        //
        // Before the fix, RecordResult's safety-net would fire after End(row1) decremented
        // the count to 1, consuming the count slot that belonged to the parent and causing
        // the subsequent End(parent) to be silently dropped.
        _testResult.Outcome = TestOutcome.Passed;

        _testRecorderWithTestEventsHandler.RecordStart(_testCase);                       // parent start → count=1
        _testRecorderWithTestEventsHandler.RecordStart(_testCase);                       // row1 start  → count=2
        _testRecorderWithTestEventsHandler.RecordEnd(_testCase, TestOutcome.Passed);     // row1 end    → count=1, sends End
        _testRecorderWithTestEventsHandler.RecordResult(_testResult);                    // row1 result → safety-net must NOT fire
        _testRecorderWithTestEventsHandler.RecordEnd(_testCase, TestOutcome.Passed);     // parent end  → count=0, sends End

        // Exactly two Ends: one for row1 (from RecordEnd) and one for the parent (from RecordEnd).
        // A third End from the RecordResult safety-net would indicate the regression is present.
        _mockTestCaseEventsHandler.Verify(x => x.SendTestCaseEnd(_testCase, TestOutcome.Passed), Times.Exactly(2));
        _mockTestCaseEventsHandler.Verify(x => x.SendTestResult(_testResult), Times.Once);
    }

    [TestMethod]
    public void RecordResultShouldSendTestCaseEndWhenExecutionsWithSameIdMixExplicitAndImplicitEnds()
    {
        _testResult.Outcome = TestOutcome.Passed;

        _testRecorderWithTestEventsHandler.RecordStart(_testCase);
        _testRecorderWithTestEventsHandler.RecordStart(_testCase);
        _testRecorderWithTestEventsHandler.RecordEnd(_testCase, TestOutcome.Passed);
        _testRecorderWithTestEventsHandler.RecordResult(_testResult);
        _testRecorderWithTestEventsHandler.RecordStart(_testCase);
        _testRecorderWithTestEventsHandler.RecordResult(_testResult);
        _testRecorderWithTestEventsHandler.RecordEnd(_testCase, TestOutcome.Passed);
        _testRecorderWithTestEventsHandler.RecordResult(_testResult);

        _mockTestCaseEventsHandler.Verify(x => x.SendTestCaseStart(_testCase), Times.Exactly(3));
        _mockTestCaseEventsHandler.Verify(x => x.SendTestCaseEnd(_testCase, TestOutcome.Passed), Times.Exactly(3));
        _mockTestCaseEventsHandler.Verify(x => x.SendTestResult(_testResult), Times.Exactly(3));
    }

    [TestMethod]
    public void RecordResultShouldPairExplicitEndWithCorrectExecutionWhenSameIdEventsInterleave()
    {
        var firstTestCase = new TestCase("A.C.M", new Uri("executor://dummy"), "A");
        var secondTestCase = new TestCase("A.C.M", new Uri("executor://dummy"), "A");
        var firstResult = new Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult(firstTestCase) { Outcome = TestOutcome.Passed };
        var secondResult = new Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult(secondTestCase) { Outcome = TestOutcome.Failed };

        _testRecorderWithTestEventsHandler.RecordStart(firstTestCase);
        _testRecorderWithTestEventsHandler.RecordStart(secondTestCase);
        _testRecorderWithTestEventsHandler.RecordEnd(firstTestCase, TestOutcome.Passed);
        _testRecorderWithTestEventsHandler.RecordResult(secondResult);
        _testRecorderWithTestEventsHandler.RecordResult(firstResult);

        _mockTestCaseEventsHandler.Verify(
            x => x.SendTestCaseEnd(It.Is<TestCase>(testCase => ReferenceEquals(testCase, firstTestCase)), TestOutcome.Passed),
            Times.Once);
        _mockTestCaseEventsHandler.Verify(
            x => x.SendTestCaseEnd(It.Is<TestCase>(testCase => ReferenceEquals(testCase, secondTestCase)), TestOutcome.Failed),
            Times.Once);
    }

    [TestMethod]
    public void RecordResultShouldNotConsumeNewExecutionAfterSharedIdCountReachedZero()
    {
        var firstTestCase = new TestCase("A.C.M", new Uri("executor://dummy"), "A");
        var secondTestCase = new TestCase("A.C.M", new Uri("executor://dummy"), "A");
        var thirdTestCase = new TestCase("A.C.M", new Uri("executor://dummy"), "A");
        var firstResult = new Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult(firstTestCase) { Outcome = TestOutcome.Passed };
        var secondResult = new Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult(secondTestCase) { Outcome = TestOutcome.Failed };
        var thirdResult = new Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult(thirdTestCase) { Outcome = TestOutcome.Skipped };

        _testRecorderWithTestEventsHandler.RecordStart(firstTestCase);
        _testRecorderWithTestEventsHandler.RecordStart(secondTestCase);
        _testRecorderWithTestEventsHandler.RecordEnd(firstTestCase, TestOutcome.Passed);
        _testRecorderWithTestEventsHandler.RecordResult(secondResult);
        _testRecorderWithTestEventsHandler.RecordStart(thirdTestCase);
        _testRecorderWithTestEventsHandler.RecordResult(firstResult);
        _testRecorderWithTestEventsHandler.RecordResult(thirdResult);

        _mockTestCaseEventsHandler.Verify(
            x => x.SendTestCaseEnd(It.Is<TestCase>(testCase => ReferenceEquals(testCase, firstTestCase)), TestOutcome.Passed),
            Times.Once);
        _mockTestCaseEventsHandler.Verify(
            x => x.SendTestCaseEnd(It.Is<TestCase>(testCase => ReferenceEquals(testCase, secondTestCase)), TestOutcome.Failed),
            Times.Once);
        _mockTestCaseEventsHandler.Verify(
            x => x.SendTestCaseEnd(It.Is<TestCase>(testCase => ReferenceEquals(testCase, thirdTestCase)), TestOutcome.Skipped),
            Times.Once);
    }

    [TestMethod]
    public void RecordResultShouldKeepStartsAndEndsBalancedWhenOneInstanceIsReusedForSequentialExecutions()
    {
        // An adapter may reuse a single TestCase instance for sequential executions that share an id,
        // and the second execution can start before the first one's result arrives:
        //
        //   Start(tc) → End(tc) → Start(tc) → Result(tc) → Result(tc)
        //
        // The first result consumes the pending explicit end, so no end is sent for it. The second
        // result finds no pending end and falls back to the safety net, which sends the end for the
        // second execution. Both executions therefore get a start and an end.
        _testResult.Outcome = TestOutcome.Passed;

        _testRecorderWithTestEventsHandler.RecordStart(_testCase);                       // execution 1 start
        _testRecorderWithTestEventsHandler.RecordEnd(_testCase, TestOutcome.Passed);     // execution 1 explicit end
        _testRecorderWithTestEventsHandler.RecordStart(_testCase);                       // execution 2 start, same instance
        _testRecorderWithTestEventsHandler.RecordResult(_testResult);                    // consumes the pending end
        _testRecorderWithTestEventsHandler.RecordResult(_testResult);                    // safety net sends execution 2's end

        _mockTestCaseEventsHandler.Verify(x => x.SendTestCaseStart(_testCase), Times.Exactly(2));
        _mockTestCaseEventsHandler.Verify(x => x.SendTestCaseEnd(_testCase, TestOutcome.Passed), Times.Exactly(2));
        _mockTestCaseEventsHandler.Verify(x => x.SendTestResult(_testResult), Times.Exactly(2));
    }

    #endregion
}
