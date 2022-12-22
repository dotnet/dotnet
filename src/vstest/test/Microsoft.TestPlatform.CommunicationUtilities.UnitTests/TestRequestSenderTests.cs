// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests;

[TestClass]
public class TestRequestSenderTests
{
    private const int Dummyprotocolversion = 42;
    private const int Defaultprotocolversion = 1;
    private const int Dummynegotiatedprotocolversion = 41;
    private static readonly string TimoutErrorMessage = "Failed to negotiate protocol, waiting for response timed out after 0 seconds. This may occur due to machine slowness, please set environment variable VSTEST_CONNECTION_TIMEOUT to increase timeout.";

    private readonly Mock<ICommunicationEndPoint> _mockServer;
    private readonly Mock<IDataSerializer> _mockDataSerializer;
    private readonly Mock<ICommunicationChannel> _mockChannel;

    private readonly List<string> _pathToAdditionalExtensions = new() { "Hello", "World" };
    private readonly Mock<ITestDiscoveryEventsHandler2> _mockDiscoveryEventsHandler;
    private readonly Mock<IInternalTestRunEventsHandler> _mockExecutionEventsHandler;
    private readonly TestRunCriteriaWithSources _testRunCriteriaWithSources;
    private TestHostConnectionInfo _connectionInfo;
    private readonly ITestRequestSender _testRequestSender;
    private ConnectedEventArgs _connectedEventArgs;

    public TestRequestSenderTests()
    {
        _connectionInfo = new TestHostConnectionInfo
        {
            Endpoint = IPAddress.Loopback + ":123",
            Role = ConnectionRole.Client,
            Transport = Transport.Sockets
        };
        _mockChannel = new Mock<ICommunicationChannel>();
        _mockServer = new Mock<ICommunicationEndPoint>();
        _mockDataSerializer = new Mock<IDataSerializer>();
        _testRequestSender = new TestableTestRequestSender(_mockServer.Object, _connectionInfo, _mockDataSerializer.Object, new ProtocolConfig { Version = Dummyprotocolversion });

        _connectedEventArgs = new ConnectedEventArgs(_mockChannel.Object);
        _mockDiscoveryEventsHandler = new Mock<ITestDiscoveryEventsHandler2>();
        _mockExecutionEventsHandler = new Mock<IInternalTestRunEventsHandler>();
        _testRunCriteriaWithSources = new TestRunCriteriaWithSources(new Dictionary<string, IEnumerable<string>>(), "runsettings", null, null!);
    }

    [TestCleanup]
    public void Cleanup()
    {
        Environment.SetEnvironmentVariable(EnvironmentHelper.VstestConnectionTimeout, string.Empty);
    }

    [TestMethod]
    public void InitializeCommunicationShouldHostServerAndAcceptClient()
    {
        var port = SetupFakeCommunicationChannel();

        Assert.AreEqual("123", port, "Correct port must be returned.");
    }

    [TestMethod]
    public void WaitForRequestHandlerConnectionShouldWaitForClientToConnect()
    {
        SetupFakeCommunicationChannel();

        var connected = _testRequestSender.WaitForRequestHandlerConnection(1, It.IsAny<CancellationToken>());

        Assert.IsTrue(connected);
    }

    [TestMethod]
    public void WaitForRequestHandlerConnectionShouldNotConnectIfExceptionWasThrownByTcpLayer()
    {
        _connectedEventArgs = new ConnectedEventArgs(new SocketException());
        SetupFakeCommunicationChannel();

        var connected = _testRequestSender.WaitForRequestHandlerConnection(1, It.IsAny<CancellationToken>());

        Assert.IsFalse(connected);
    }

    [TestMethod]
    public void WaitForRequestHandlerConnectionWithTimeoutShouldReturnImmediatelyWhenCancellationRequested()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        var connectionTimeout = 5000;
        var watch = System.Diagnostics.Stopwatch.StartNew();
        var connected = _testRequestSender.WaitForRequestHandlerConnection(connectionTimeout, cancellationTokenSource.Token);
        watch.Stop();

        Assert.IsFalse(connected);
        Assert.IsTrue(watch.ElapsedMilliseconds < connectionTimeout);
    }

    [TestMethod]
    public void WaitForRequestHandlerConnectionWithTimeoutShouldReturnImmediatelyIfHostExitedUnexpectedly()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        _testRequestSender.OnClientProcessExit("DummyError");

        var connectionTimeout = 5000;
        var watch = System.Diagnostics.Stopwatch.StartNew();
        var connected = _testRequestSender.WaitForRequestHandlerConnection(connectionTimeout, cancellationTokenSource.Token);
        watch.Stop();

        Assert.IsFalse(connected);
        Assert.IsTrue(watch.ElapsedMilliseconds < connectionTimeout);
    }

    [TestMethod]
    public void CloseShouldCallStopServerOnCommunicationManager()
    {
        _testRequestSender.Close();

        _mockServer.Verify(mc => mc.Stop(), Times.Once);
    }

    [TestMethod]
    public void DisposeShouldCallStopServerOnCommunicationManager()
    {
        _testRequestSender.Dispose();

        _mockServer.Verify(mc => mc.Stop(), Times.Once);
    }

    [TestMethod]
    public void EndSessionShouldSendSessionEndMessage()
    {
        SetupFakeCommunicationChannel();

        _testRequestSender.EndSession();

        _mockDataSerializer.Verify(ds => ds.SerializeMessage(MessageType.SessionEnd), Times.Once);
        _mockChannel.Verify(c => c.Send(It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public void EndSessionShouldNotSendSessionEndMessageIfClientDisconnected()
    {
        SetupFakeCommunicationChannel();
        _testRequestSender.DiscoverTests(new DiscoveryCriteria(), _mockDiscoveryEventsHandler.Object);
        RaiseClientDisconnectedEvent();

        _testRequestSender.EndSession();

        _mockDataSerializer.Verify(ds => ds.SerializeMessage(MessageType.SessionEnd), Times.Never);
    }

    [TestMethod]
    public void EndSessionShouldNotSendSessionEndMessageIfTestHostProcessExited()
    {
        SetupFakeCommunicationChannel();
        _testRequestSender.StartTestRun(_testRunCriteriaWithSources, _mockExecutionEventsHandler.Object);
        _testRequestSender.OnClientProcessExit("Dummy Message");

        _testRequestSender.EndSession();

        _mockDataSerializer.Verify(ds => ds.SerializeMessage(MessageType.SessionEnd), Times.Once);
    }

    [TestMethod]
    public void EndSessionShouldNotSendTestRunCancelMessageIfClientDisconnected()
    {
        SetupFakeCommunicationChannel();
        _testRequestSender.DiscoverTests(new DiscoveryCriteria(), _mockDiscoveryEventsHandler.Object);
        RaiseClientDisconnectedEvent();

        _testRequestSender.SendTestRunCancel();

        _mockChannel.Verify(mockChannel => mockChannel.Send(MessageType.CancelTestRun), Times.Never);
    }

    [TestMethod]
    public void EndSessionShouldNotSendTestRunAbortMessageIfClientDisconnected()
    {
        SetupFakeCommunicationChannel();
        _testRequestSender.DiscoverTests(new DiscoveryCriteria(), _mockDiscoveryEventsHandler.Object);
        RaiseClientDisconnectedEvent();

        _testRequestSender.SendTestRunAbort();

        _mockChannel.Verify(mockChannel => mockChannel.Send(MessageType.CancelTestRun), Times.Never);
    }

    [DataTestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow(null)]
    public void OnClientProcessExitShouldSendErrorMessageIfStdErrIsEmpty(string stderr)
    {
        SetupFakeCommunicationChannel();
        _testRequestSender.DiscoverTests(new DiscoveryCriteria(), _mockDiscoveryEventsHandler.Object);

        _testRequestSender.OnClientProcessExit(stderr);

        var expectedErrorMessage = "Reason: Test host process crashed";
        RaiseClientDisconnectedEvent();
        _mockDiscoveryEventsHandler.Verify(eh => eh.HandleLogMessage(TestMessageLevel.Error, It.Is<string>(s => s.EndsWith(expectedErrorMessage))), Times.Once);
    }

    [TestMethod]
    public void OnClientProcessExitShouldNotSendErrorMessageIfOperationNotStarted()
    {
        SetupFakeCommunicationChannel();
        _testRequestSender.OnClientProcessExit("Dummy Stderr");

        RaiseClientDisconnectedEvent();
        _mockDiscoveryEventsHandler.Verify(eh => eh.HandleLogMessage(TestMessageLevel.Error, It.Is<string>(s => s.Contains("Dummy Stderr"))), Times.Never);
    }

    [TestMethod]
    public void OnClientProcessExitShouldNotSendRawMessageIfOperationNotStarted()
    {
        SetupFakeCommunicationChannel();

        _testRequestSender.OnClientProcessExit("Dummy Stderr");

        RaiseClientDisconnectedEvent();
        _mockDataSerializer.Verify(ds => ds.SerializePayload(MessageType.TestMessage, It.Is<TestMessagePayload>(p => p.Message!.Contains("Dummy Stderr"))), Times.Never);
        _mockExecutionEventsHandler.Verify(eh => eh.HandleRawMessage(It.IsAny<string>()), Times.Never);
    }

    #region Version Check Tests

    [TestMethod]
    public void CheckVersionWithTestHostShouldSendHighestSupportedVersion()
    {
        SetupDeserializeMessage(MessageType.VersionCheck, 99);
        SetupRaiseMessageReceivedOnCheckVersion();
        SetupFakeCommunicationChannel();

        _testRequestSender.CheckVersionWithTestHost();

        _mockDataSerializer.Verify(ds => ds.SerializePayload(MessageType.VersionCheck, Dummyprotocolversion), Times.Once);
        _mockChannel.Verify(mc => mc.Send(It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public void CheckVersionWithTestHostShouldThrowIfTestHostVersionDoesNotMatch()
    {
        SetupDeserializeMessage(MessageType.ProtocolError, string.Empty);
        SetupRaiseMessageReceivedOnCheckVersion();
        SetupFakeCommunicationChannel();

        Assert.ThrowsException<TestPlatformException>(() => _testRequestSender.CheckVersionWithTestHost());
    }

    [TestMethod]
    public void CheckVersionWithTestHostShouldThrowIfUnexpectedResponseIsReceived()
    {
        SetupDeserializeMessage(MessageType.TestCasesFound, string.Empty);
        SetupRaiseMessageReceivedOnCheckVersion();
        SetupFakeCommunicationChannel();

        Assert.ThrowsException<TestPlatformException>(() => _testRequestSender.CheckVersionWithTestHost());
    }

    [TestMethod]
    public void CheckVersionWithTestHostShouldThrowIfProtocolNegotiationTimeouts()
    {
        Environment.SetEnvironmentVariable(EnvironmentHelper.VstestConnectionTimeout, "0");

        SetupFakeCommunicationChannel();

        var message = Assert.ThrowsException<TestPlatformException>(() => _testRequestSender.CheckVersionWithTestHost()).Message;

        Assert.AreEqual(message, TimoutErrorMessage);
    }

    #endregion

    #region Discovery Protocol Tests
    [TestMethod]
    public void InitializeDiscoveryShouldSendCommunicationMessageWithCorrectParameters()
    {
        SetupFakeCommunicationChannel();

        _testRequestSender.InitializeDiscovery(_pathToAdditionalExtensions);

        _mockDataSerializer.Verify(d => d.SerializePayload(MessageType.DiscoveryInitialize, _pathToAdditionalExtensions, 1), Times.Once);
        _mockChannel.Verify(mc => mc.Send(It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public void InitializeDiscoveryShouldSendCommunicationMessageWithCorrectParametersWithVersion()
    {
        SetupFakeChannelWithVersionNegotiation();

        _testRequestSender.InitializeDiscovery(_pathToAdditionalExtensions);

        _mockDataSerializer.Verify(d => d.SerializePayload(MessageType.DiscoveryInitialize, _pathToAdditionalExtensions, Dummynegotiatedprotocolversion), Times.Once);
    }

    [TestMethod]
    public void DiscoverTestsShouldSendStartDiscoveryMessageOnChannel()
    {
        SetupFakeCommunicationChannel();

        _testRequestSender.DiscoverTests(new DiscoveryCriteria(), _mockDiscoveryEventsHandler.Object);

        _mockDataSerializer.Verify(
            s => s.SerializePayload(MessageType.StartDiscovery, It.IsAny<DiscoveryCriteria>(), Defaultprotocolversion),
            Times.Once);
        _mockChannel.Verify(mc => mc.Send(It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public void DiscoverTestsShouldSendStartDiscoveryMessageOnChannelWithVersion()
    {
        SetupFakeChannelWithVersionNegotiation();

        _testRequestSender.DiscoverTests(new DiscoveryCriteria(), _mockDiscoveryEventsHandler.Object);

        _mockDataSerializer.Verify(
            s => s.SerializePayload(MessageType.StartDiscovery, It.IsAny<DiscoveryCriteria>(), Dummynegotiatedprotocolversion),
            Times.Once);
    }

    [TestMethod]
    public void DiscoverTestsShouldNotifyRawMessageOnMessageReceived()
    {
        SetupDeserializeMessage(MessageType.TestMessage, new TestMessagePayload());
        SetupFakeCommunicationChannel();

        _testRequestSender.DiscoverTests(new DiscoveryCriteria(), _mockDiscoveryEventsHandler.Object);

        RaiseMessageReceivedEvent();
        _mockDiscoveryEventsHandler.Verify(eh => eh.HandleRawMessage("DummyData"), Times.Once);
    }

    [TestMethod]
    public void DiscoverTestsShouldNotifyDiscoveredTestsOnTestCasesFoundMessageReceived()
    {
        SetupDeserializeMessage<IEnumerable<TestCase>>(MessageType.TestCasesFound, new TestCase[2]);
        SetupFakeCommunicationChannel();

        _testRequestSender.DiscoverTests(new DiscoveryCriteria(), _mockDiscoveryEventsHandler.Object);

        RaiseMessageReceivedEvent();
        _mockDiscoveryEventsHandler.Verify(eh => eh.HandleDiscoveredTests(It.Is<IEnumerable<TestCase>>(t => t.Count() == 2)));
    }

    [TestMethod]
    public void DiscoverTestsShouldNotifyDiscoveryCompleteOnCompleteMessageReceived()
    {
        var completePayload = new DiscoveryCompletePayload { TotalTests = 10, IsAborted = false };
        SetupDeserializeMessage(MessageType.DiscoveryComplete, completePayload);
        SetupFakeCommunicationChannel();

        _testRequestSender.DiscoverTests(new DiscoveryCriteria(), _mockDiscoveryEventsHandler.Object);

        RaiseMessageReceivedEvent();
        _mockDiscoveryEventsHandler.Verify(eh => eh.HandleDiscoveryComplete(It.Is<DiscoveryCompleteEventArgs>(dc => dc.IsAborted == false && dc.TotalCount == 10), null));
    }

    [TestMethod]
    public void DiscoverTestsShouldStopServerOnCompleteMessageReceived()
    {
        var completePayload = new DiscoveryCompletePayload { TotalTests = 10, IsAborted = false };
        SetupDeserializeMessage(MessageType.DiscoveryComplete, completePayload);
        SetupFakeCommunicationChannel();

        _testRequestSender.DiscoverTests(new DiscoveryCriteria(), _mockDiscoveryEventsHandler.Object);

        RaiseMessageReceivedEvent();

        _mockServer.Verify(ms => ms.Stop());
    }

    [TestMethod]
    public void DiscoverTestShouldNotifyLogMessageOnTestMessageReceived()
    {
        var message = new TestMessagePayload { MessageLevel = TestMessageLevel.Error, Message = "Message1" };
        SetupDeserializeMessage(MessageType.TestMessage, message);
        SetupFakeCommunicationChannel();

        _testRequestSender.DiscoverTests(new DiscoveryCriteria(), _mockDiscoveryEventsHandler.Object);

        RaiseMessageReceivedEvent();
        _mockDiscoveryEventsHandler.Verify(eh => eh.HandleLogMessage(TestMessageLevel.Error, "Message1"));
    }

    [TestMethod]
    public void DiscoverTestShouldNotifyLogMessageIfExceptionThrownOnMessageReceived()
    {
        SetupExceptionOnMessageReceived();
        SetupFakeCommunicationChannel();

        _testRequestSender.DiscoverTests(new DiscoveryCriteria(), _mockDiscoveryEventsHandler.Object);

        RaiseMessageReceivedEvent();
        _mockDiscoveryEventsHandler.Verify(eh => eh.HandleLogMessage(TestMessageLevel.Error, It.Is<string>(s => s.Contains("Dummy Message"))));
        _mockDiscoveryEventsHandler.Verify(eh => eh.HandleRawMessage("SerializedMessage"), Times.Once);
    }

    [TestMethod]
    public void DiscoverTestShouldNotifyDiscoveryCompleteIfExceptionThrownOnMessageReceived()
    {
        SetupExceptionOnMessageReceived();
        SetupFakeCommunicationChannel();

        _testRequestSender.DiscoverTests(new DiscoveryCriteria(), _mockDiscoveryEventsHandler.Object);

        RaiseMessageReceivedEvent();
        _mockDiscoveryEventsHandler.Verify(eh => eh.HandleDiscoveryComplete(It.Is<DiscoveryCompleteEventArgs>(dc => dc.IsAborted == true && dc.TotalCount == -1), null));
    }

    [TestMethod]
    public void DiscoverTestsShouldNotAbortDiscoveryIfClientDisconnectedAndOperationIsComplete()
    {
        var completePayload = new DiscoveryCompletePayload { TotalTests = 10, IsAborted = false };
        SetupDeserializeMessage(MessageType.DiscoveryComplete, completePayload);
        SetupFakeCommunicationChannel();
        _testRequestSender.DiscoverTests(new DiscoveryCriteria(), _mockDiscoveryEventsHandler.Object);
        RaiseMessageReceivedEvent();   // Raise discovery complete

        RaiseClientDisconnectedEvent();

        _mockDiscoveryEventsHandler.Verify(eh => eh.HandleLogMessage(TestMessageLevel.Error, It.IsAny<string>()), Times.Never);
        _mockDiscoveryEventsHandler.Verify(eh => eh.HandleDiscoveryComplete(new DiscoveryCompleteEventArgs(-1, true), null), Times.Never);
    }

    [TestMethod]
    public void DiscoverTestShouldNotifyLogMessageIfClientDisconnected()
    {
        // Expect default error message since we've not set any client exit message
        var expectedErrorMessage = "Reason: Unable to communicate";
        SetupFakeCommunicationChannel();
        _mockDataSerializer.Setup(ds => ds.SerializePayload(MessageType.TestMessage, It.Is<TestMessagePayload>(p => p.Message!.Contains(expectedErrorMessage))))
            .Returns("Serialized error");
        _testRequestSender.DiscoverTests(new DiscoveryCriteria(), _mockDiscoveryEventsHandler.Object);

        RaiseClientDisconnectedEvent();

        _mockDiscoveryEventsHandler.Verify(eh => eh.HandleLogMessage(TestMessageLevel.Error, It.Is<string>(s => s.Contains(expectedErrorMessage))));
        _mockDiscoveryEventsHandler.Verify(eh => eh.HandleRawMessage(It.Is<string>(s => !string.IsNullOrEmpty(s) && s.Equals("Serialized error"))), Times.Once);
    }

    [TestMethod]
    public void DiscoverTestShouldNotifyLogMessageIfClientDisconnectedWithClientExit()
    {
        SetupFakeCommunicationChannel();
        _mockDataSerializer.Setup(ds => ds.SerializePayload(MessageType.TestMessage, It.Is<TestMessagePayload>(p => p.Message!.Contains("Dummy Stderr"))))
            .Returns("Serialized Stderr");
        _testRequestSender.DiscoverTests(new DiscoveryCriteria(), _mockDiscoveryEventsHandler.Object);
        _testRequestSender.OnClientProcessExit("Dummy Stderr");

        RaiseClientDisconnectedEvent();

        _mockDiscoveryEventsHandler.Verify(eh => eh.HandleLogMessage(TestMessageLevel.Error, It.Is<string>(s => s.Contains("Dummy Stderr"))), Times.Once);
        _mockDiscoveryEventsHandler.Verify(eh => eh.HandleRawMessage(It.Is<string>(s => !string.IsNullOrEmpty(s) && s.Equals("Serialized Stderr"))), Times.Once);
    }

    [TestMethod]
    public void DiscoverTestShouldNotifyDiscoveryCompleteIfClientDisconnectedBeforeDiscovery()
    {
        SetupFakeCommunicationChannel();

        RaiseClientDisconnectedEvent();

        _testRequestSender.DiscoverTests(new DiscoveryCriteria(), _mockDiscoveryEventsHandler.Object);

        _mockDiscoveryEventsHandler.Verify(eh => eh.HandleDiscoveryComplete(It.Is<DiscoveryCompleteEventArgs>(dc => dc.IsAborted == true && dc.TotalCount == -1), null));
    }

    [TestMethod]
    public void DiscoverTestShouldNotifyDiscoveryCompleteIfClientDisconnected()
    {
        SetupFakeCommunicationChannel();
        _testRequestSender.DiscoverTests(new DiscoveryCriteria(), _mockDiscoveryEventsHandler.Object);

        RaiseClientDisconnectedEvent();

        _mockDiscoveryEventsHandler.Verify(eh => eh.HandleDiscoveryComplete(It.Is<DiscoveryCompleteEventArgs>(dc => dc.IsAborted == true && dc.TotalCount == -1), null));
    }

    #endregion

    #region Execution Protocol Tests

    [TestMethod]
    public void InitializeExecutionShouldSendCommunicationMessageWithCorrectParameters()
    {
        SetupFakeCommunicationChannel();

        _testRequestSender.InitializeExecution(_pathToAdditionalExtensions);

        _mockDataSerializer.Verify(d => d.SerializePayload(MessageType.ExecutionInitialize, _pathToAdditionalExtensions, 1), Times.Once);
        _mockChannel.Verify(mc => mc.Send(It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public void InitializeExecutionShouldSendCommunicationMessageWithCorrectParametersWithVersion()
    {
        SetupFakeChannelWithVersionNegotiation();

        _testRequestSender.InitializeExecution(_pathToAdditionalExtensions);

        _mockDataSerializer.Verify(d => d.SerializePayload(MessageType.ExecutionInitialize, _pathToAdditionalExtensions, Dummynegotiatedprotocolversion), Times.Once);
    }

    [TestMethod]
    public void StartTestRunShouldSendStartTestExecutionWithSourcesOnChannel()
    {
        SetupFakeCommunicationChannel();

        _testRequestSender.StartTestRun(_testRunCriteriaWithSources, _mockExecutionEventsHandler.Object);

        _mockDataSerializer.Verify(d => d.SerializePayload(MessageType.StartTestExecutionWithSources, _testRunCriteriaWithSources, Defaultprotocolversion), Times.Once);
        _mockChannel.Verify(mc => mc.Send(It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public void StartTestRunShouldSendStartTestExecutionWithSourcesOnChannelWithVersion()
    {
        SetupFakeChannelWithVersionNegotiation();

        _testRequestSender.StartTestRun(_testRunCriteriaWithSources, _mockExecutionEventsHandler.Object);

        _mockDataSerializer.Verify(d => d.SerializePayload(MessageType.StartTestExecutionWithSources, _testRunCriteriaWithSources, Dummynegotiatedprotocolversion), Times.Once);
    }

    [TestMethod]
    public void StartTestRunWithTestsShouldSendStartTestExecutionWithTestsOnChannel()
    {
        var runCriteria = new TestRunCriteriaWithTests(new TestCase[2], "runsettings", null, null!);
        SetupFakeCommunicationChannel();

        _testRequestSender.StartTestRun(runCriteria, _mockExecutionEventsHandler.Object);

        _mockDataSerializer.Verify(d => d.SerializePayload(MessageType.StartTestExecutionWithTests, runCriteria, Defaultprotocolversion), Times.Once);
        _mockChannel.Verify(mc => mc.Send(It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public void StartTestRunWithTestsShouldSendStartTestExecutionWithTestsOnChannelWithVersion()
    {
        var runCriteria = new TestRunCriteriaWithTests(new TestCase[2], "runsettings", null, null!);
        SetupFakeChannelWithVersionNegotiation();

        _testRequestSender.StartTestRun(runCriteria, _mockExecutionEventsHandler.Object);

        _mockDataSerializer.Verify(d => d.SerializePayload(MessageType.StartTestExecutionWithTests, runCriteria, Dummynegotiatedprotocolversion), Times.Once);
    }

    [TestMethod]
    public void StartTestRunShouldNotifyRawMessageOnMessageReceived()
    {
        SetupDeserializeMessage(MessageType.TestMessage, new TestMessagePayload());
        SetupFakeCommunicationChannel();

        _testRequestSender.StartTestRun(_testRunCriteriaWithSources, _mockExecutionEventsHandler.Object);

        RaiseMessageReceivedEvent();
        _mockExecutionEventsHandler.Verify(eh => eh.HandleRawMessage("DummyData"), Times.Once);
    }

    [TestMethod]
    public void StartTestRunShouldNotifyTestRunStatsChangeOnRunStatsMessageReceived()
    {
        var testRunChangedArgs = new TestRunChangedEventArgs(
            null,
            new VisualStudio.TestPlatform.ObjectModel.TestResult[2],
            new TestCase[2]);
        SetupDeserializeMessage(MessageType.TestRunStatsChange, testRunChangedArgs);
        SetupFakeCommunicationChannel();

        _testRequestSender.StartTestRun(_testRunCriteriaWithSources, _mockExecutionEventsHandler.Object);

        RaiseMessageReceivedEvent();
        _mockExecutionEventsHandler.Verify(eh => eh.HandleTestRunStatsChange(testRunChangedArgs), Times.Once);
    }

    [TestMethod]
    public void StartTestRunShouldNotifyExecutionCompleteOnRunCompleteMessageReceived()
    {
        var testRunCompletePayload = new TestRunCompletePayload
        {
            TestRunCompleteArgs = new TestRunCompleteEventArgs(null, false, false, null, null, null, TimeSpan.MaxValue),
            LastRunTests = new TestRunChangedEventArgs(null, null, null),
            RunAttachments = new List<AttachmentSet>()
        };
        SetupDeserializeMessage(MessageType.ExecutionComplete, testRunCompletePayload);
        SetupFakeCommunicationChannel();

        _testRequestSender.StartTestRun(_testRunCriteriaWithSources, _mockExecutionEventsHandler.Object);

        RaiseMessageReceivedEvent();
        _mockExecutionEventsHandler.Verify(
            eh => eh.HandleTestRunComplete(
                testRunCompletePayload.TestRunCompleteArgs,
                testRunCompletePayload.LastRunTests,
                testRunCompletePayload.RunAttachments,
                It.IsAny<ICollection<string>>()),
            Times.Once);
    }

    [TestMethod]
    public void StartTestRunShouldStopServerOnRunCompleteMessageReceived()
    {
        var testRunCompletePayload = new TestRunCompletePayload
        {
            TestRunCompleteArgs = new TestRunCompleteEventArgs(null, false, false, null, null, null, TimeSpan.MaxValue),
            LastRunTests = new TestRunChangedEventArgs(null, null, null),
            RunAttachments = new List<AttachmentSet>()
        };
        SetupDeserializeMessage(MessageType.ExecutionComplete, testRunCompletePayload);
        SetupFakeCommunicationChannel();

        _testRequestSender.StartTestRun(_testRunCriteriaWithSources, _mockExecutionEventsHandler.Object);

        RaiseMessageReceivedEvent();

        _mockServer.Verify(ms => ms.Stop());
    }

    [TestMethod]
    public void StartTestRunShouldNotifyLogMessageOnTestMessageReceived()
    {
        var testMessagePayload = new TestMessagePayload { MessageLevel = TestMessageLevel.Error, Message = "Dummy" };
        SetupDeserializeMessage(MessageType.TestMessage, testMessagePayload);
        SetupFakeCommunicationChannel();

        _testRequestSender.StartTestRun(_testRunCriteriaWithSources, _mockExecutionEventsHandler.Object);

        RaiseMessageReceivedEvent();
        _mockExecutionEventsHandler.Verify(eh => eh.HandleLogMessage(TestMessageLevel.Error, "Dummy"), Times.Once);
    }

    [TestMethod]
    public void StartTestRunShouldNotifyLaunchWithDebuggerOnMessageReceived()
    {
        var launchMessagePayload = new TestProcessStartInfo();
        SetupDeserializeMessage(MessageType.LaunchAdapterProcessWithDebuggerAttached, launchMessagePayload);
        SetupFakeCommunicationChannel();

        _testRequestSender.StartTestRun(_testRunCriteriaWithSources, _mockExecutionEventsHandler.Object);

        RaiseMessageReceivedEvent();
        _mockExecutionEventsHandler.Verify(eh => eh.LaunchProcessWithDebuggerAttached(launchMessagePayload), Times.Once);
    }

    [TestMethod]
    public void StartTestRunShouldSendLaunchDebuggerAttachedCallbackOnMessageReceived()
    {
        var launchMessagePayload = new TestProcessStartInfo();
        SetupDeserializeMessage(MessageType.LaunchAdapterProcessWithDebuggerAttached, launchMessagePayload);
        SetupFakeCommunicationChannel();

        _testRequestSender.StartTestRun(_testRunCriteriaWithSources, _mockExecutionEventsHandler.Object);

        RaiseMessageReceivedEvent();
        _mockDataSerializer.Verify(ds => ds.SerializePayload(MessageType.LaunchAdapterProcessWithDebuggerAttachedCallback, It.IsAny<int>(), 1), Times.Once);
        _mockChannel.Verify(c => c.Send(It.IsAny<string>()), Times.AtLeastOnce);
    }

    [TestMethod]
    public void StartTestRunShouldSendLaunchDebuggerAttachedCallbackOnMessageReceivedWithVersion()
    {
        var launchMessagePayload = new TestProcessStartInfo();
        SetupFakeChannelWithVersionNegotiation();
        SetupDeserializeMessage(MessageType.LaunchAdapterProcessWithDebuggerAttached, launchMessagePayload);

        _testRequestSender.StartTestRun(_testRunCriteriaWithSources, _mockExecutionEventsHandler.Object);

        RaiseMessageReceivedEvent();
        _mockDataSerializer.Verify(ds => ds.SerializePayload(MessageType.LaunchAdapterProcessWithDebuggerAttachedCallback, It.IsAny<int>(), Dummynegotiatedprotocolversion), Times.Once);
    }

    [TestMethod]
    public void StartTestRunShouldNotifyLogMessageIfExceptionIsThrownOnMessageReceived()
    {
        SetupExceptionOnMessageReceived();
        SetupFakeCommunicationChannel();

        _testRequestSender.StartTestRun(_testRunCriteriaWithSources, _mockExecutionEventsHandler.Object);

        RaiseMessageReceivedEvent();
        _mockExecutionEventsHandler.Verify(eh => eh.HandleLogMessage(TestMessageLevel.Error, It.Is<string>(s => s.Contains("Dummy Message"))), Times.Once);
        _mockExecutionEventsHandler.Verify(eh => eh.HandleRawMessage("SerializedMessage"), Times.Once);
    }

    [TestMethod]
    public void StartTestRunShouldNotifyExecutionCompleteIfExceptionIsThrownOnMessageReceived()
    {
        SetupExceptionOnMessageReceived();
        SetupFakeCommunicationChannel();

        _testRequestSender.StartTestRun(_testRunCriteriaWithSources, _mockExecutionEventsHandler.Object);

        RaiseMessageReceivedEvent();
        _mockExecutionEventsHandler.Verify(eh => eh.HandleTestRunComplete(It.Is<TestRunCompleteEventArgs>(t => t.IsAborted), null, null, null), Times.Once);
        _mockExecutionEventsHandler.Verify(eh => eh.HandleRawMessage("SerializedAbortedPayload"), Times.Once);
    }

    [TestMethod]
    public void StartTestRunShouldNotNotifyExecutionCompleteIfClientDisconnectedAndOperationComplete()
    {
        var testRunCompletePayload = new TestRunCompletePayload
        {
            TestRunCompleteArgs = new TestRunCompleteEventArgs(null, false, false, null, null, null, TimeSpan.MaxValue),
            LastRunTests = new TestRunChangedEventArgs(null, null, null),
            RunAttachments = new List<AttachmentSet>()
        };
        SetupDeserializeMessage(MessageType.ExecutionComplete, testRunCompletePayload);
        SetupFakeCommunicationChannel();
        _testRequestSender.StartTestRun(_testRunCriteriaWithSources, _mockExecutionEventsHandler.Object);
        RaiseMessageReceivedEvent();   // Raise test run complete

        RaiseClientDisconnectedEvent();

        _mockExecutionEventsHandler.Verify(eh => eh.HandleTestRunComplete(It.Is<TestRunCompleteEventArgs>(t => t.IsAborted), null, null, null), Times.Never);
        _mockExecutionEventsHandler.Verify(eh => eh.HandleRawMessage("SerializedAbortedPayload"), Times.Never);
    }

    [TestMethod]
    public void StartTestRunShouldNotifyErrorLogMessageIfClientDisconnected()
    {
        SetupFakeCommunicationChannel();
        _testRequestSender.StartTestRun(_testRunCriteriaWithSources, _mockExecutionEventsHandler.Object);

        RaiseClientDisconnectedEvent();

        // Expect default error message since we've not set any client exit message
        var expectedErrorMessage = "Reason: Unable to communicate";
        _mockExecutionEventsHandler.Verify(eh => eh.HandleLogMessage(TestMessageLevel.Error, It.Is<string>(s => s.Contains(expectedErrorMessage))), Times.Once);
    }

    [TestMethod]
    public void StartTestRunShouldNotifyErrorLogMessageIfClientDisconnectedWithClientExit()
    {
        SetupFakeCommunicationChannel();
        _testRequestSender.StartTestRun(_testRunCriteriaWithSources, _mockExecutionEventsHandler.Object);
        _testRequestSender.OnClientProcessExit("Dummy Stderr");

        RaiseClientDisconnectedEvent();

        var expectedErrorMessage = "Reason: Test host process crashed : Dummy Stderr";
        _mockExecutionEventsHandler.Verify(eh => eh.HandleLogMessage(TestMessageLevel.Error, It.Is<string>(s => s.Contains(expectedErrorMessage))), Times.Once);
    }

    [TestMethod]
    public void StartTestRunShouldNotifyExecutionCompleteIfClientDisconnectedBeforeRun()
    {
        SetupOperationAbortedPayload();
        SetupFakeCommunicationChannel();

        RaiseClientDisconnectedEvent();

        _testRequestSender.StartTestRun(_testRunCriteriaWithSources, _mockExecutionEventsHandler.Object);

        _mockExecutionEventsHandler.Verify(eh => eh.HandleTestRunComplete(It.Is<TestRunCompleteEventArgs>(t => t.IsAborted), null, null, null), Times.Once);
        _mockExecutionEventsHandler.Verify(eh => eh.HandleRawMessage("SerializedAbortedPayload"), Times.Once);
    }

    [TestMethod]
    public void StartTestRunWithTestsShouldNotifyExecutionCompleteIfClientDisconnectedBeforeRun()
    {
        var runCriteria = new TestRunCriteriaWithTests(new TestCase[2], "runsettings", null, null!);
        SetupOperationAbortedPayload();
        SetupFakeCommunicationChannel();

        RaiseClientDisconnectedEvent();

        _testRequestSender.StartTestRun(runCriteria, _mockExecutionEventsHandler.Object);

        _mockExecutionEventsHandler.Verify(eh => eh.HandleTestRunComplete(It.Is<TestRunCompleteEventArgs>(t => t.IsAborted), null, null, null), Times.Once);
        _mockExecutionEventsHandler.Verify(eh => eh.HandleRawMessage("SerializedAbortedPayload"), Times.Once);
    }

    [TestMethod]
    public async Task StartTestRunWithTestsShouldNotifyExecutionCompleteIfClientDisconnectedBeforeRunInAThreadSafeWay()
    {
        var runCriteria = new TestRunCriteriaWithTests(new TestCase[2], "runsettings", null, null!);
        SetupOperationAbortedPayload();
        SetupFakeCommunicationChannel();

        // Note: Even if the calls get invoked on separate threads, the request sender should send back the complete message just once.
        var t1 = Task.Run(RaiseClientDisconnectedEvent);
        var t2 = Task.Run(() => _testRequestSender.StartTestRun(runCriteria, _mockExecutionEventsHandler.Object));

        await Task.WhenAll(t1, t2);

        _mockExecutionEventsHandler.Verify(eh => eh.HandleTestRunComplete(It.Is<TestRunCompleteEventArgs>(t => t.IsAborted), null, null, null), Times.Once);
        _mockExecutionEventsHandler.Verify(eh => eh.HandleRawMessage("SerializedAbortedPayload"), Times.Once);
    }

    [TestMethod]
    public void StartTestRunShouldNotifyExecutionCompleteIfClientDisconnected()
    {
        SetupOperationAbortedPayload();
        SetupFakeCommunicationChannel();
        _testRequestSender.StartTestRun(_testRunCriteriaWithSources, _mockExecutionEventsHandler.Object);

        RaiseClientDisconnectedEvent();

        _mockExecutionEventsHandler.Verify(eh => eh.HandleTestRunComplete(It.Is<TestRunCompleteEventArgs>(t => t.IsAborted), null, null, null), Times.Once);
        _mockExecutionEventsHandler.Verify(eh => eh.HandleRawMessage("SerializedAbortedPayload"), Times.Once);
    }

    [TestMethod]
    public void SendTestRunCancelShouldSendCancelTestRunMessage()
    {
        SetupFakeCommunicationChannel();

        _testRequestSender.SendTestRunCancel();

        _mockDataSerializer.Verify(ds => ds.SerializeMessage(MessageType.CancelTestRun), Times.Once);
        _mockChannel.Verify(c => c.Send(It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public void SendTestRunAbortShouldSendAbortTestRunMessage()
    {
        SetupFakeCommunicationChannel();

        _testRequestSender.SendTestRunAbort();

        _mockDataSerializer.Verify(ds => ds.SerializeMessage(MessageType.AbortTestRun), Times.Once);
        _mockChannel.Verify(c => c.Send(It.IsAny<string>()), Times.Once);
    }

    #endregion

    private string SetupFakeCommunicationChannel(string connectionArgs = "123")
    {
        _connectionInfo = new TestHostConnectionInfo
        {
            Endpoint = IPAddress.Loopback + ":" + connectionArgs,
            Role = ConnectionRole.Client,
            Transport = Transport.Sockets
        };

        // Setup mock connected event and initialize communication channel
        _mockServer.Setup(mc => mc.Start(_connectionInfo.Endpoint))
            .Returns(_connectionInfo.Endpoint)
            .Callback(() => _mockServer.Raise(s => s.Connected += null, _mockServer.Object, _connectedEventArgs));

        return _testRequestSender.InitializeCommunication().ToString(CultureInfo.CurrentCulture);
    }

    private void SetupFakeChannelWithVersionNegotiation()
    {
        // Sends a check version message to setup the negotiated protocol version.
        // This method is only required in specific tests.
        SetupDeserializeMessage(MessageType.VersionCheck, Dummynegotiatedprotocolversion);
        SetupRaiseMessageReceivedOnCheckVersion();
        SetupFakeCommunicationChannel();
        _testRequestSender.CheckVersionWithTestHost();
        ResetRaiseMessageReceivedOnCheckVersion();
    }

    private void RaiseMessageReceivedEvent()
    {
        _mockChannel.Raise(
            c => c.MessageReceived += null,
            _mockChannel.Object,
            new MessageReceivedEventArgs { Data = "DummyData" });
    }

    private void RaiseClientDisconnectedEvent()
    {
        _mockServer.Raise(
            s => s.Disconnected += null,
            _mockServer.Object,
            new DisconnectedEventArgs { Error = new Exception("Dummy Message") });
    }

    private void SetupDeserializeMessage<TPayload>(string messageType, TPayload payload)
    {
        _mockDataSerializer.Setup(ds => ds.DeserializeMessage(It.IsAny<string>()))
            .Returns(new Message { MessageType = messageType });
        _mockDataSerializer.Setup(ds => ds.DeserializePayload<TPayload>(It.IsAny<Message>()))
            .Returns(payload);
    }

    private void SetupExceptionMessageSerialize()
    {
        // Serialize the exception message
        _mockDataSerializer
            .Setup(ds => ds.SerializePayload(MessageType.TestMessage, It.Is<TestMessagePayload>(p => p.Message!.Contains("Dummy Message"))))
            .Returns("SerializedMessage");
    }

    private void SetupOperationAbortedPayload()
    {
        // Serialize the execution aborted
        _mockDataSerializer
            .Setup(ds => ds.SerializePayload(MessageType.ExecutionComplete, It.Is<TestRunCompletePayload>(p => p.TestRunCompleteArgs!.IsAborted)))
            .Returns("SerializedAbortedPayload");
    }

    private void SetupExceptionOnMessageReceived()
    {
        SetupExceptionMessageSerialize();
        SetupOperationAbortedPayload();

        _mockDataSerializer.Setup(ds => ds.DeserializeMessage(It.IsAny<string>()))
            .Callback(() => throw new Exception("Dummy Message"));
    }

    private void SetupRaiseMessageReceivedOnCheckVersion()
    {
        _mockChannel.Setup(mc => mc.Send(It.IsAny<string>())).Callback(RaiseMessageReceivedEvent);
    }

    private void ResetRaiseMessageReceivedOnCheckVersion()
    {
        _mockChannel.Reset();
    }

    private class TestableTestRequestSender : TestRequestSender
    {
        public TestableTestRequestSender(ICommunicationEndPoint commEndpoint, TestHostConnectionInfo connectionInfo, IDataSerializer serializer, ProtocolConfig protocolConfig)
            : base(commEndpoint, connectionInfo, serializer, protocolConfig, 0)
        {
        }
    }
}
