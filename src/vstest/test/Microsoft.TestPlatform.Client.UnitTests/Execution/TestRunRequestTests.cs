// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

using Microsoft.VisualStudio.TestPlatform.Client.Execution;
using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.VisualStudio.TestPlatform.Client.UnitTests.Execution;

[TestClass]
public class TestRunRequestTests
{
    private TestRunRequest _testRunRequest;
    private Mock<IProxyExecutionManager> _executionManager;
    private readonly Mock<ITestLoggerManager> _loggerManager;
    private TestRunCriteria _testRunCriteria;
    private readonly Mock<IRequestData> _mockRequestData;

    private readonly Mock<IDataSerializer> _mockDataSerializer;

    public TestRunRequestTests()
    {
        _testRunCriteria = new TestRunCriteria(new List<string> { "foo" }, 1);
        _executionManager = new Mock<IProxyExecutionManager>();
        _loggerManager = new Mock<ITestLoggerManager>();
        _mockRequestData = new Mock<IRequestData>();
        _mockRequestData.Setup(rd => rd.MetricsCollection).Returns(new NoOpMetricsCollection());
        _mockDataSerializer = new Mock<IDataSerializer>();
        _testRunRequest = new TestRunRequest(_mockRequestData.Object, _testRunCriteria, _executionManager.Object, _loggerManager.Object, _mockDataSerializer.Object);
    }

    [TestMethod]
    public void ConstructorSetsTestRunCriteriaExecutionManagerAndState()
    {
        Assert.AreEqual(TestRunState.Pending, _testRunRequest.State);
        Assert.AreEqual(_testRunCriteria, _testRunRequest.TestRunConfiguration);
        Assert.AreEqual(_executionManager.Object, _testRunRequest.ExecutionManager);
    }

    [TestMethod]
    public void ExecuteAsycIfTestRunRequestIsDisposedThrowsObjectDisposedException()
    {
        _testRunRequest.Dispose();

        Assert.ThrowsException<ObjectDisposedException>(() => _testRunRequest.ExecuteAsync());
    }

    [TestMethod]
    public void ExecuteAsycIfStateIsNotPendingThrowsInvalidOperationException()
    {
        _testRunRequest.ExecuteAsync();
        Assert.ThrowsException<InvalidOperationException>(() => _testRunRequest.ExecuteAsync());
    }

    [TestMethod]
    public void ExecuteAsyncSetsStateToInProgressAndCallManagerToStartTestRun()
    {
        _testRunRequest.ExecuteAsync();

        Assert.AreEqual(TestRunState.InProgress, _testRunRequest.State);
        _executionManager.Verify(em => em.StartTestRun(_testRunCriteria, _testRunRequest), Times.Once);
    }

    [TestMethod]
    public void ExecuteAsyncIfStartTestRunThrowsExceptionSetsStateToPendingAndThrowsThatException()
    {
        _executionManager.Setup(em => em.StartTestRun(_testRunCriteria, _testRunRequest)).Throws(new Exception("DummyException"));
        try
        {
            _testRunRequest.ExecuteAsync();
        }
        catch (Exception ex)
        {
            Assert.IsTrue(ex is not null);
            Assert.AreEqual("DummyException", ex.Message);
            Assert.AreEqual(TestRunState.Pending, _testRunRequest.State);
        }
    }

    [TestMethod]
    public void AbortIfTestRunRequestDisposedShouldNotThrowException()
    {
        _testRunRequest.Dispose();
        _testRunRequest.Abort();
    }

    [TestMethod]
    public void AbortIfTestRunStateIsNotInProgressShouldNotCallExecutionManagerAbort()
    {
        //ExecuteAsync has not been called, so State is not InProgress
        _testRunRequest.Abort();
        _executionManager.Verify(dm => dm.Abort(It.IsAny<IInternalTestRunEventsHandler>()), Times.Never);
    }

    [TestMethod]
    public void AbortIfDiscoveryIsinProgressShouldCallDiscoveryManagerAbort()
    {
        // Set the State to InProgress
        _testRunRequest.ExecuteAsync();

        _testRunRequest.Abort();
        _executionManager.Verify(dm => dm.Abort(It.IsAny<IInternalTestRunEventsHandler>()), Times.Once);
    }

    [TestMethod]
    public void WaitForCompletionIfTestRunRequestDisposedShouldThrowObjectDisposedException()
    {
        _testRunRequest.Dispose();
        Assert.ThrowsException<ObjectDisposedException>(() => _testRunRequest.WaitForCompletion());
    }

    [TestMethod]
    public void WaitForCompletionIfTestRunStatePendingShouldThrowInvalidOperationException()
    {
        Assert.ThrowsException<InvalidOperationException>(() => _testRunRequest.WaitForCompletion());
    }

    [TestMethod]
    public void CancelAsyncIfTestRunRequestDisposedShouldNotThrowException()
    {
        _testRunRequest.Dispose();
        _testRunRequest.CancelAsync();
    }

    [TestMethod]
    public void CancelAsyncIfTestRunStateNotInProgressWillNotCallExecutionManagerCancel()
    {
        _testRunRequest.CancelAsync();
        _executionManager.Verify(dm => dm.Cancel(It.IsAny<IInternalTestRunEventsHandler>()), Times.Never);
    }

    [TestMethod]
    public void CancelAsyncIfTestRunStateInProgressCallsExecutionManagerCancel()
    {
        _testRunRequest.ExecuteAsync();
        _testRunRequest.CancelAsync();
        _executionManager.Verify(dm => dm.Cancel(It.IsAny<IInternalTestRunEventsHandler>()), Times.Once);
    }

    [TestMethod]
    public void OnTestSessionTimeoutShouldCallAbort()
    {
        _testRunRequest.ExecuteAsync();
        _testRunRequest.OnTestSessionTimeout(null);
        _executionManager.Verify(o => o.Abort(It.IsAny<IInternalTestRunEventsHandler>()), Times.Once);
    }

    [TestMethod]
    public void OnTestSessionTimeoutShouldLogMessage()
    {
        bool handleLogMessageCalled = false;
        bool handleRawMessageCalled = false;

        _mockDataSerializer
            .Setup(s => s.SerializePayload(It.IsAny<string>(), It.IsAny<Object>()))
            .Returns("non-empty rawMessage");

        _testRunRequest.TestRunMessage += (object? sender, TestRunMessageEventArgs e) => handleLogMessageCalled = true;

        _testRunRequest.OnRawMessageReceived += (object? sender, string message) => handleRawMessageCalled = true;

        _testRunRequest.OnTestSessionTimeout(null);

        Assert.IsTrue(handleLogMessageCalled, "OnTestSessionTimeout should call HandleLogMessage");
        Assert.IsTrue(handleRawMessageCalled, "OnTestSessionTimeout should call HandleRawMessage");
    }

    [TestMethod]
    public void OnTestSessionTimeoutShouldGetCalledWhenExecutionCrossedTestSessionTimeout()
    {
        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <TestSessionTimeout>1000</TestSessionTimeout>
                     </RunConfiguration>
                </RunSettings>";

        var testRunCriteria = new TestRunCriteria(new List<string> { "foo" }, 1, true, settingsXml);
        var executionManager = new Mock<IProxyExecutionManager>();
        var testRunRequest = new TestRunRequest(_mockRequestData.Object, testRunCriteria, executionManager.Object, _loggerManager.Object);

        ManualResetEvent onTestSessionTimeoutCalled = new(true);
        onTestSessionTimeoutCalled.Reset();
        executionManager.Setup(o => o.Abort(It.IsAny<IInternalTestRunEventsHandler>())).Callback(() => onTestSessionTimeoutCalled.Set());

        testRunRequest.ExecuteAsync();
        onTestSessionTimeoutCalled.WaitOne(20 * 1000);

        executionManager.Verify(o => o.Abort(It.IsAny<IInternalTestRunEventsHandler>()), Times.Once);
    }

    /// <summary>
    /// Test session timeout should be infinity if TestSessionTimeout is 0.
    /// </summary>
    [TestMethod]
    public void OnTestSessionTimeoutShouldNotGetCalledWhenTestSessionTimeoutIsZero()
    {
        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <TestSessionTimeout>0</TestSessionTimeout>
                     </RunConfiguration>
                </RunSettings>";

        var testRunCriteria = new TestRunCriteria(new List<string> { "foo" }, 1, true, settingsXml);
        var executionManager = new Mock<IProxyExecutionManager>();
        var testRunRequest = new TestRunRequest(_mockRequestData.Object, testRunCriteria, executionManager.Object, _loggerManager.Object);

        executionManager.Setup(o => o.StartTestRun(It.IsAny<TestRunCriteria>(), It.IsAny<IInternalTestRunEventsHandler>())).Callback(() => Thread.Sleep(5 * 1000));

        testRunRequest.ExecuteAsync();

        executionManager.Verify(o => o.Abort(It.IsAny<IInternalTestRunEventsHandler>()), Times.Never);
    }

    [TestMethod]
    public void HandleTestRunStatsChangeShouldInvokeListenersWithTestRunChangedEventArgs()
    {
        var mockStats = new Mock<ITestRunStatistics>();

        var testResults = new List<ObjectModel.TestResult>
        {
            new ObjectModel.TestResult(
                new TestCase(
                    "A.C.M",
                    new Uri("executor://dummy"),
                    "A"))
        };
        var activeTestCases = new List<TestCase>
        {
            new TestCase(
                "A.C.M2",
                new Uri("executor://dummy"),
                "A")
        };
        var testRunChangedEventArgs = new TestRunChangedEventArgs(mockStats.Object, testResults, activeTestCases);
        TestRunChangedEventArgs? receivedArgs = null;

        _testRunRequest.OnRunStatsChange += (object? sender, TestRunChangedEventArgs e) => receivedArgs = e;

        // Act.
        _testRunRequest.HandleTestRunStatsChange(testRunChangedEventArgs);

        // Assert.
        Assert.IsNotNull(receivedArgs);
        Assert.AreEqual(testRunChangedEventArgs.TestRunStatistics, receivedArgs.TestRunStatistics);
        CollectionAssert.AreEqual(
            testRunChangedEventArgs.NewTestResults!.ToList(),
            receivedArgs.NewTestResults!.ToList());
        CollectionAssert.AreEqual(testRunChangedEventArgs.ActiveTests!.ToList(), receivedArgs.ActiveTests!.ToList());
    }

    [TestMethod]
    public void HandleRawMessageShouldCallOnRawMessageReceived()
    {
        string rawMessage = "HelloWorld";
        string? messageReceived = null;

        // Call should NOT fail even if on raw message received is not registered.
        _testRunRequest.HandleRawMessage(rawMessage);

        EventHandler<string> handler = (sender, e) => messageReceived = e;
        _testRunRequest.OnRawMessageReceived += handler;

        _testRunRequest.HandleRawMessage(rawMessage);

        Assert.AreEqual(rawMessage, messageReceived, "RunRequest should just pass the message as is.");
        _testRunRequest.OnRawMessageReceived -= handler;
    }

    [TestMethod]
    public void HandleRawMessageShouldAddVsTestDataPointsIfTelemetryOptedIn()
    {
        bool onDiscoveryCompleteInvoked = true;
        _mockRequestData.Setup(x => x.IsTelemetryOptedIn).Returns(true);
        _testRunRequest.OnRawMessageReceived += (object? sender, string e) => onDiscoveryCompleteInvoked = true;

        _mockDataSerializer.Setup(x => x.DeserializeMessage(It.IsAny<string>()))
            .Returns(new Message() { MessageType = MessageType.ExecutionComplete });

        _mockDataSerializer.Setup(x => x.DeserializePayload<TestRunCompletePayload>(It.IsAny<Message>()))
            .Returns(new TestRunCompletePayload()
            {
                TestRunCompleteArgs = new TestRunCompleteEventArgs(null, false, false, null, null, null, TimeSpan.MinValue)
            });

        _testRunRequest.HandleRawMessage(string.Empty);

        _mockDataSerializer.Verify(x => x.SerializePayload(It.IsAny<string>(), It.IsAny<TestRunCompletePayload>()), Times.Once);
        _mockRequestData.Verify(x => x.MetricsCollection, Times.AtLeastOnce);
        Assert.IsTrue(onDiscoveryCompleteInvoked);
    }

    [TestMethod]
    public void HandleRawMessageShouldInvokeHandleTestRunCompleteOfLoggerManager()
    {
        _loggerManager.Setup(x => x.LoggersInitialized).Returns(true);
        _mockDataSerializer.Setup(x => x.DeserializeMessage(It.IsAny<string>()))
            .Returns(new Message() { MessageType = MessageType.ExecutionComplete });

        var testRunCompleteEvent = new TestRunCompleteEventArgs(new TestRunStatistics(1, null), false, false, null, null,
            null, TimeSpan.FromSeconds(0));
        _mockDataSerializer.Setup(x => x.DeserializePayload<TestRunCompletePayload>(It.IsAny<Message>()))
            .Returns(new TestRunCompletePayload()
            {
                TestRunCompleteArgs = testRunCompleteEvent
            });

        _testRunRequest.ExecuteAsync();
        _testRunRequest.HandleRawMessage(string.Empty);

        _loggerManager.Verify(lm => lm.HandleTestRunComplete(It.IsAny<TestRunCompleteEventArgs>()), Times.Once);
    }

    [TestMethod]
    public void HandleRawMessageShouldNotInvokeHandleTestRunCompleteOfLoggerManagerWhenNoLoggersInitiailized()
    {
        _loggerManager.Setup(x => x.LoggersInitialized).Returns(false);
        _mockDataSerializer.Setup(x => x.DeserializeMessage(It.IsAny<string>()))
            .Returns(new Message() { MessageType = MessageType.ExecutionComplete });

        var testRunCompleteEvent = new TestRunCompleteEventArgs(new TestRunStatistics(1, null), false, false, null, null,
            null, TimeSpan.FromSeconds(0));
        _mockDataSerializer.Setup(x => x.DeserializePayload<TestRunCompletePayload>(It.IsAny<Message>()))
            .Returns(new TestRunCompletePayload()
            {
                TestRunCompleteArgs = testRunCompleteEvent
            });

        _testRunRequest.ExecuteAsync();
        _testRunRequest.HandleRawMessage(string.Empty);

        _loggerManager.Verify(lm => lm.HandleTestRunComplete(It.IsAny<TestRunCompleteEventArgs>()), Times.Never);
    }

    [TestMethod]
    public void HandleRawMessageShouldInvokeShouldInvokeHandleTestRunStatsChangeOfLoggerManagerWhenLastChunkAvailable()
    {
        var mockStats = new Mock<ITestRunStatistics>();

        var testResults = new List<ObjectModel.TestResult>
        {
            new ObjectModel.TestResult(
                new TestCase(
                    "A.C.M",
                    new Uri("executor://dummy"),
                    "A"))
        };
        var activeTestCases = new List<TestCase>
        {
            new TestCase(
                "A.C.M2",
                new Uri("executor://dummy"),
                "A")
        };

        _loggerManager.Setup(x => x.LoggersInitialized).Returns(true);
        _mockDataSerializer.Setup(x => x.DeserializeMessage(It.IsAny<string>()))
            .Returns(new Message() { MessageType = MessageType.ExecutionComplete });

        var testRunChangedEventArgs = new TestRunChangedEventArgs(mockStats.Object, testResults, activeTestCases);
        var testRunCompleteEvent = new TestRunCompleteEventArgs(new TestRunStatistics(1, null), false, false, null, null,
            null, TimeSpan.FromSeconds(0));

        _mockDataSerializer.Setup(x => x.DeserializePayload<TestRunCompletePayload>(It.IsAny<Message>()))
            .Returns(new TestRunCompletePayload()
            {
                TestRunCompleteArgs = testRunCompleteEvent,
                LastRunTests = testRunChangedEventArgs
            });

        _testRunRequest.ExecuteAsync();
        _testRunRequest.HandleRawMessage(string.Empty);

        _loggerManager.Verify(lm => lm.HandleTestRunStatsChange(testRunChangedEventArgs), Times.Once);
        _loggerManager.Verify(lm => lm.HandleTestRunComplete(It.IsAny<TestRunCompleteEventArgs>()), Times.Once);
    }

    [TestMethod]
    public void HandleTestRunStatsChangeShouldInvokeHandleTestRunStatsChangeOfLoggerManager()
    {
        var mockStats = new Mock<ITestRunStatistics>();

        var testResults = new List<ObjectModel.TestResult>
        {
            new ObjectModel.TestResult(
                new TestCase(
                    "A.C.M",
                    new Uri("executor://dummy"),
                    "A"))
        };
        var activeTestCases = new List<TestCase>
        {
            new TestCase(
                "A.C.M2",
                new Uri("executor://dummy"),
                "A")
        };

        var testRunChangedEventArgs = new TestRunChangedEventArgs(mockStats.Object, testResults, activeTestCases);
        _testRunRequest.HandleTestRunStatsChange(testRunChangedEventArgs);

        _loggerManager.Verify(lm => lm.HandleTestRunStatsChange(testRunChangedEventArgs), Times.Once);
    }

    [TestMethod]
    public void HandleTestRunCompleteShouldInvokeHandleTestRunStatsChangeOfLoggerManagerWhenLastChunkAvailable()
    {
        var mockStats = new Mock<ITestRunStatistics>();

        var testResults = new List<ObjectModel.TestResult>
        {
            new ObjectModel.TestResult(
                new TestCase(
                    "A.C.M",
                    new Uri("executor://dummy"),
                    "A"))
        };
        var activeTestCases = new List<TestCase>
        {
            new TestCase(
                "A.C.M2",
                new Uri("executor://dummy"),
                "A")
        };
        var testRunChangedEventArgs = new TestRunChangedEventArgs(mockStats.Object, testResults, activeTestCases);
        var testRunCompleteEvent = new TestRunCompleteEventArgs(new TestRunStatistics(1, null), false, false, null, null,
            null, TimeSpan.FromSeconds(0));

        _testRunRequest.ExecuteAsync();
        _testRunRequest.HandleTestRunComplete(testRunCompleteEvent, testRunChangedEventArgs, null, null);

        _loggerManager.Verify(lm => lm.HandleTestRunStatsChange(testRunChangedEventArgs), Times.Once);
        _loggerManager.Verify(lm => lm.HandleTestRunComplete(It.IsAny<TestRunCompleteEventArgs>()), Times.Once);
    }

    [TestMethod]
    public void HandleTestRunCompleteShouldInvokeHandleTestRunCompleteOfLoggerManager()
    {
        var testRunCompleteEvent = new TestRunCompleteEventArgs(new TestRunStatistics(1, null), false, false, null, null,
            null, TimeSpan.FromSeconds(0));

        _testRunRequest.ExecuteAsync();
        _testRunRequest.HandleTestRunComplete(testRunCompleteEvent, null, null, null);

        _loggerManager.Verify(lm => lm.HandleTestRunComplete(It.IsAny<TestRunCompleteEventArgs>()), Times.Once);
    }

    [TestMethod]
    public void HandleLogMessageShouldInvokeHandleLogMessageOfLoggerManager()
    {
        _testRunRequest.HandleLogMessage(TestMessageLevel.Error, "hello");
        _loggerManager.Verify(lm => lm.HandleTestRunMessage(It.IsAny<TestRunMessageEventArgs>()), Times.Once);
    }

    [TestMethod]
    public void HandleTestRunCompleteShouldCollectMetrics()
    {
        var mockMetricsCollector = new Mock<IMetricsCollection>();
        var dict = new Dictionary<string, object>
        {
            { "DummyMessage", "DummyValue" }
        };

        mockMetricsCollector.Setup(mc => mc.Metrics).Returns(dict);
        _mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollector.Object);

        _testRunRequest.ExecuteAsync();
        var testRunCompleteEventsArgs = new TestRunCompleteEventArgs(
            new TestRunStatistics(1, null),
            false,
            false,
            null,
            null,
            null,
            TimeSpan.FromSeconds(0));
        testRunCompleteEventsArgs.Metrics = dict;

        // Act
        _testRunRequest.HandleTestRunComplete(testRunCompleteEventsArgs, null, null, null);

        // Verify.
        mockMetricsCollector.Verify(rd => rd.Add(TelemetryDataConstants.TimeTakenInSecForRun, It.IsAny<double>()), Times.Once);
        mockMetricsCollector.Verify(rd => rd.Add("DummyMessage", "DummyValue"), Times.Once);
    }

    [TestMethod]
    public void HandleTestRunCompleteShouldHandleListAttachments()
    {
        bool attachmentsFound = false;
        _testRunRequest.OnRunCompletion += (s, e) => attachmentsFound = e.AttachmentSets != null && e.AttachmentSets.Count == 1;

        List<AttachmentSet> attachmentSets = new() { new AttachmentSet(new Uri("datacollector://attachment"), "datacollectorAttachment") };

        _testRunRequest.ExecuteAsync();
        var testRunCompleteEventsArgs = new TestRunCompleteEventArgs(
            new TestRunStatistics(1, null),
            false,
            false,
            null,
            null,
            null,
            TimeSpan.FromSeconds(0));

        // Act
        _testRunRequest.HandleTestRunComplete(testRunCompleteEventsArgs, null, attachmentSets, null);

        // Verify.
        Assert.IsTrue(attachmentsFound);
    }

    [TestMethod]
    public void HandleTestRunCompleteShouldHandleCollectionAttachments()
    {
        bool attachmentsFound = false;
        _testRunRequest.OnRunCompletion += (s, e) => attachmentsFound = e.AttachmentSets != null && e.AttachmentSets.Count == 1;

        Collection<AttachmentSet> attachmentSets = new(new List<AttachmentSet> { new AttachmentSet(new Uri("datacollector://attachment"), "datacollectorAttachment") });

        _testRunRequest.ExecuteAsync();
        var testRunCompleteEventsArgs = new TestRunCompleteEventArgs(
            new TestRunStatistics(1, null),
            false,
            false,
            null,
            null,
            null,
            TimeSpan.FromSeconds(0));

        // Act
        _testRunRequest.HandleTestRunComplete(testRunCompleteEventsArgs, null, attachmentSets, null);

        // Verify.
        Assert.IsTrue(attachmentsFound);
    }

    [TestMethod]
    public void HandleTestRunCompleteShouldHandleEmptyAttachments()
    {
        bool attachmentsFound = false;
        _testRunRequest.OnRunCompletion += (s, e) => attachmentsFound = (e.AttachmentSets.Count == 0);

        _testRunRequest.ExecuteAsync();
        var testRunCompleteEventsArgs = new TestRunCompleteEventArgs(
            new TestRunStatistics(1, null),
            false,
            false,
            null,
            null,
            null,
            TimeSpan.FromSeconds(0));

        // Act
        _testRunRequest.HandleTestRunComplete(testRunCompleteEventsArgs, null, null, null);

        // Verify.
        Assert.IsTrue(attachmentsFound);
    }

    [TestMethod]
    public void HandleTestRunCompleteShouldCloseExecutionManager()
    {
        var events = new List<string>();
        _executionManager.Setup(em => em.Close()).Callback(() => events.Add("close"));
        _testRunRequest.OnRunCompletion += (s, e) => events.Add("complete");
        _testRunRequest.ExecuteAsync();

        _testRunRequest.HandleTestRunComplete(new TestRunCompleteEventArgs(new TestRunStatistics(1, null), false, false, null, null, null, TimeSpan.FromSeconds(0)), null, null, null);

        Assert.AreEqual(2, events.Count);
        Assert.AreEqual("close", events[0]);
        Assert.AreEqual("complete", events[1]);
    }

    [TestMethod]
    public void LaunchProcessWithDebuggerAttachedShouldNotCallCustomLauncherIfTestRunIsNotInProgress()
    {
        var mockCustomLauncher = new Mock<ITestHostLauncher>();
        _testRunCriteria = new TestRunCriteria(new List<string> { "foo" }, 1, false, null, TimeSpan.Zero, mockCustomLauncher.Object);
        _executionManager = new Mock<IProxyExecutionManager>();
        _testRunRequest = new TestRunRequest(_mockRequestData.Object, _testRunCriteria, _executionManager.Object, _loggerManager.Object);

        var testProcessStartInfo = new TestProcessStartInfo();
        _testRunRequest.LaunchProcessWithDebuggerAttached(testProcessStartInfo);

        mockCustomLauncher.Verify(ml => ml.LaunchTestHost(It.IsAny<TestProcessStartInfo>()), Times.Never);
    }

    [TestMethod]
    public void LaunchProcessWithDebuggerAttachedShouldNotCallCustomLauncherIfLauncherIsNotDebug()
    {
        var mockCustomLauncher = new Mock<ITestHostLauncher>();
        _testRunCriteria = new TestRunCriteria(new List<string> { "foo" }, 1, false, null, TimeSpan.Zero, mockCustomLauncher.Object);
        _executionManager = new Mock<IProxyExecutionManager>();
        _testRunRequest = new TestRunRequest(_mockRequestData.Object, _testRunCriteria, _executionManager.Object, _loggerManager.Object);

        _testRunRequest.ExecuteAsync();

        var testProcessStartInfo = new TestProcessStartInfo();
        _testRunRequest.LaunchProcessWithDebuggerAttached(testProcessStartInfo);

        mockCustomLauncher.Verify(ml => ml.LaunchTestHost(It.IsAny<TestProcessStartInfo>()), Times.Never);
    }

    [TestMethod]
    public void LaunchProcessWithDebuggerAttachedShouldCallCustomLauncherIfLauncherIsDebugAndRunInProgress()
    {
        var mockCustomLauncher = new Mock<ITestHostLauncher>();
        _testRunCriteria = new TestRunCriteria(new List<string> { "foo" }, 1, false, null, TimeSpan.Zero, mockCustomLauncher.Object);
        _executionManager = new Mock<IProxyExecutionManager>();
        _testRunRequest = new TestRunRequest(_mockRequestData.Object, _testRunCriteria, _executionManager.Object, _loggerManager.Object);

        _testRunRequest.ExecuteAsync();

        var testProcessStartInfo = new TestProcessStartInfo();
        mockCustomLauncher.Setup(ml => ml.IsDebug).Returns(true);
        _testRunRequest.LaunchProcessWithDebuggerAttached(testProcessStartInfo);

        mockCustomLauncher.Verify(ml => ml.LaunchTestHost(testProcessStartInfo), Times.Once);
    }

    /// <summary>
    /// ExecuteAsync should invoke OnRunStart event.
    /// </summary>
    [TestMethod]
    public void ExecuteAsyncShouldInvokeOnRunStart()
    {
        bool onRunStartHandlerCalled = false;
        _testRunRequest.OnRunStart += (s, e) => onRunStartHandlerCalled = true;

        // Action
        _testRunRequest.ExecuteAsync();

        // Assert
        Assert.IsTrue(onRunStartHandlerCalled, "ExecuteAsync should invoke OnRunstart event");
    }

    [TestMethod]
    public void ExecuteAsyncShouldInvokeHandleTestRunStartOfLoggerManager()
    {
        _testRunRequest.ExecuteAsync();

        _loggerManager.Verify(lm => lm.HandleTestRunStart(It.IsAny<TestRunStartEventArgs>()), Times.Once);
    }
}
