// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
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
        Assert.AreEqual(0, attachments.Count);
    }

    [TestMethod]
    public void RecordStartShouldUpdateTestRunCache()
    {
        _testRecorder.RecordStart(_testCase);
        Assert.IsTrue(_testableTestRunCache.TestStartedList.Contains(_testCase));
    }

    [TestMethod]
    public void RecordResultShouldUpdateTestRunCache()
    {
        _testRecorder.RecordResult(_testResult);
        Assert.IsTrue(_testableTestRunCache.TestResultList.Contains(_testResult));
    }

    [TestMethod]
    public void RecordEndShouldUpdateTestRunCache()
    {
        _testRecorder.RecordEnd(_testCase, TestOutcome.Passed);
        Assert.IsTrue(_testableTestRunCache.TestCompletedList.Contains(_testCase));
    }

    [TestMethod]
    public void RecordAttachmentsShouldAddToAttachmentSet()
    {
        var attachmentSet = new List<AttachmentSet> { new AttachmentSet(new Uri("attachment://dummy"), "attachment") };

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
            new AttachmentSet(new Uri("attachment://dummy"), "attachment"),
            new AttachmentSet(new Uri("attachment://infinite"), "infinity")
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
    public void RecordStartAndRecordEndShouldIgnoreRedundantTestCaseStartAndTestCaseEnd()
    {
        _testRecorderWithTestEventsHandler.RecordStart(_testCase);
        _testRecorderWithTestEventsHandler.RecordStart(_testCase);
        _testRecorderWithTestEventsHandler.RecordEnd(_testCase, TestOutcome.Passed);
        _testRecorderWithTestEventsHandler.RecordEnd(_testCase, TestOutcome.Passed);

        _mockTestCaseEventsHandler.Verify(x => x.SendTestCaseStart(_testCase), Times.Exactly(1));
        _mockTestCaseEventsHandler.Verify(x => x.SendTestCaseEnd(_testCase, TestOutcome.Passed), Times.Exactly(1));
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
        Assert.IsTrue(_testableTestRunCache.TestResultList.Contains(_testResult));
    }

    [TestMethod]
    public void RecordResultShouldSendTestCaseEndEventAndFlushIfRecordEndWasCalledAfterRecordResult()
    {
        _testResult.Outcome = TestOutcome.Passed;
        _testRecorderWithTestEventsHandler.RecordStart(_testCase);
        _testRecorderWithTestEventsHandler.RecordResult(_testResult);
        _testRecorderWithTestEventsHandler.RecordEnd(_testCase, _testResult.Outcome);

        _mockTestCaseEventsHandler.Verify(x => x.SendTestCaseEnd(_testCase, TestOutcome.Passed), Times.Once);
        Assert.IsTrue(_testableTestRunCache.TestResultList.Contains(_testResult));
    }

    [TestMethod]
    public void RecordResultShouldSendTestCaseEndEventIfRecordEndWasNotCalled()
    {
        _testResult.Outcome = TestOutcome.Passed;
        _testRecorderWithTestEventsHandler.RecordStart(_testCase);
        _testRecorderWithTestEventsHandler.RecordResult(_testResult);

        _mockTestCaseEventsHandler.Verify(x => x.SendTestCaseEnd(_testCase, TestOutcome.Passed), Times.Once);
        Assert.IsTrue(_testableTestRunCache.TestResultList.Contains(_testResult));
    }

    #endregion
}
