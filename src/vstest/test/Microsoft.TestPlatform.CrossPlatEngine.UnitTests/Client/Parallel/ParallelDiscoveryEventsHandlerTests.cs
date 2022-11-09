// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.Parallel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace TestPlatform.CrossPlatEngine.UnitTests.Client;

[TestClass]
public class ParallelDiscoveryEventsHandlerTests
{
    private readonly ParallelDiscoveryEventsHandler _parallelDiscoveryEventsHandler;
    private readonly Mock<IProxyDiscoveryManager> _mockProxyDiscoveryManager;
    private readonly Mock<ITestDiscoveryEventsHandler2> _mockTestDiscoveryEventsHandler;
    private readonly Mock<IParallelProxyDiscoveryManager> _mockParallelProxyDiscoveryManager;
    private readonly Mock<IDataSerializer> _mockDataSerializer;
    private readonly Mock<IRequestData> _mockRequestData;
    private readonly int _protocolVersion = 1;

    public ParallelDiscoveryEventsHandlerTests()
    {
        _mockProxyDiscoveryManager = new Mock<IProxyDiscoveryManager>();
        _mockTestDiscoveryEventsHandler = new Mock<ITestDiscoveryEventsHandler2>();
        _mockParallelProxyDiscoveryManager = new Mock<IParallelProxyDiscoveryManager>();
        _mockDataSerializer = new Mock<IDataSerializer>();
        _mockRequestData = new Mock<IRequestData>();
        _mockRequestData.Setup(rd => rd.MetricsCollection).Returns(new NoOpMetricsCollection());
        _mockRequestData.Setup(rd => rd.ProtocolConfig).Returns(new ProtocolConfig { Version = _protocolVersion });

        _parallelDiscoveryEventsHandler = new ParallelDiscoveryEventsHandler(_mockRequestData.Object, _mockProxyDiscoveryManager.Object,
            _mockTestDiscoveryEventsHandler.Object, _mockParallelProxyDiscoveryManager.Object,
            new DiscoveryDataAggregator(), _mockDataSerializer.Object);
    }

    [TestMethod]
    public void HandleDiscoveryCompleteShouldNotCallLastChunkResultsIfNotPresent()
    {
        int totalTests = 10;
        bool aborted = false;
        _mockParallelProxyDiscoveryManager.Setup(mp => mp.HandlePartialDiscoveryComplete(
            _mockProxyDiscoveryManager.Object, totalTests, null, aborted)).Returns(false);

        var discoveryCompleteEventsArgs = new DiscoveryCompleteEventArgs(totalTests, aborted);

        _parallelDiscoveryEventsHandler.HandleDiscoveryComplete(discoveryCompleteEventsArgs, null);

        // Raw message must be sent
        _mockTestDiscoveryEventsHandler.Verify(mt => mt.HandleRawMessage(It.IsAny<string>()), Times.Never);

        _mockTestDiscoveryEventsHandler.Verify(mt => mt.HandleDiscoveredTests(null), Times.Never);

        _mockParallelProxyDiscoveryManager.Verify(mp => mp.HandlePartialDiscoveryComplete(
            _mockProxyDiscoveryManager.Object, totalTests, null, aborted), Times.Once);
    }

    [TestMethod]
    public void HandleDiscoveryCompleteShouldCallLastChunkResultsIfPresent()
    {
        string payload = "Tests";
        int totalTests = 10;
        bool aborted = false;
        var lastChunk = new List<TestCase>();

        _mockDataSerializer.Setup(mds => mds.SerializePayload(MessageType.TestCasesFound, lastChunk, _protocolVersion))
            .Returns(payload);

        _mockParallelProxyDiscoveryManager.Setup(mp => mp.HandlePartialDiscoveryComplete(
            _mockProxyDiscoveryManager.Object, totalTests, lastChunk, aborted)).Returns(false);

        var discoveryCompleteEventsArgs = new DiscoveryCompleteEventArgs(totalTests, aborted);

        _parallelDiscoveryEventsHandler.HandleDiscoveryComplete(discoveryCompleteEventsArgs, lastChunk);

        // Raw message must be sent
        _mockTestDiscoveryEventsHandler.Verify(mt => mt.HandleRawMessage(payload), Times.Once);

        _mockTestDiscoveryEventsHandler.Verify(mt => mt.HandleDiscoveredTests(lastChunk), Times.Once);

        _mockParallelProxyDiscoveryManager.Verify(mp => mp.HandlePartialDiscoveryComplete(
            _mockProxyDiscoveryManager.Object, totalTests, null, aborted), Times.Once);
    }

    [TestMethod]
    public void HandleDiscoveryCompleteShouldCollectMetrics()
    {
        string payload = "DiscoveryComplete";
        int totalTests = 10;
        bool aborted = false;

        _mockParallelProxyDiscoveryManager.Setup(mp => mp.HandlePartialDiscoveryComplete(
            _mockProxyDiscoveryManager.Object, totalTests, null, aborted)).Returns(true);

        _mockDataSerializer.Setup(mds => mds.SerializeMessage(MessageType.DiscoveryComplete)).Returns(payload);

        var mockMetricsCollector = new Mock<IMetricsCollection>();
        _mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollector.Object);

        var discoveryCompleteEventsArgs = new DiscoveryCompleteEventArgs(totalTests, aborted);

        // Act.
        _parallelDiscoveryEventsHandler.HandleDiscoveryComplete(discoveryCompleteEventsArgs, null);

        // Verify.
        mockMetricsCollector.Verify(rd => rd.Add(TelemetryDataConstants.DiscoveryState, It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public void HandleDiscoveryCompleteShouldCallTestDiscoveryCompleteOnActualHandlerIfParallelManagerReturnsCompleteAsTrue()
    {
        string payload = "DiscoveryComplete";
        int totalTests = 10;
        bool aborted = false;

        _mockParallelProxyDiscoveryManager.Setup(mp => mp.HandlePartialDiscoveryComplete(
            _mockProxyDiscoveryManager.Object, totalTests, null, aborted)).Returns(true);

        _mockDataSerializer.Setup(mds => mds.SerializeMessage(MessageType.DiscoveryComplete)).Returns(payload);

        // Act
        var discoveryCompleteEventsArgs = new DiscoveryCompleteEventArgs(totalTests, aborted);

        _parallelDiscoveryEventsHandler.HandleDiscoveryComplete(discoveryCompleteEventsArgs, null);

        // Verify
        _mockTestDiscoveryEventsHandler.Verify(mt => mt.HandleDiscoveredTests(null), Times.Never);

        _mockParallelProxyDiscoveryManager.Verify(mp => mp.HandlePartialDiscoveryComplete(
            _mockProxyDiscoveryManager.Object, totalTests, null, aborted), Times.Once);

        _mockTestDiscoveryEventsHandler.Verify(mt => mt.HandleRawMessage(It.IsAny<string>()), Times.Once);

        _mockTestDiscoveryEventsHandler.Verify(mt => mt.HandleDiscoveryComplete(It.IsAny<DiscoveryCompleteEventArgs>(), null), Times.Once);
    }

    [TestMethod]
    public void HandleDiscoveryCompleteShouldCallConvertToRawMessageAndSendOnceIfDiscoveryIsComplete()
    {
        string payload = "DiscoveryComplete";
        int totalTests = 10;
        bool aborted = false;

        _mockParallelProxyDiscoveryManager.Setup(mp => mp.HandlePartialDiscoveryComplete(
            _mockProxyDiscoveryManager.Object, totalTests, null, aborted)).Returns(true);

        _mockDataSerializer.Setup(mds => mds.SerializeMessage(MessageType.DiscoveryComplete)).Returns(payload);

        // Act
        var discoveryCompleteEventsArgs = new DiscoveryCompleteEventArgs(totalTests, aborted);
        _parallelDiscoveryEventsHandler.HandleDiscoveryComplete(discoveryCompleteEventsArgs, null);

        // Verify
        _mockTestDiscoveryEventsHandler.Verify(mt => mt.HandleRawMessage(It.IsAny<string>()), Times.Once);
        _mockTestDiscoveryEventsHandler.Verify(mt => mt.HandleDiscoveryComplete(It.IsAny<DiscoveryCompleteEventArgs>(), null), Times.Once);
    }

    [TestMethod]
    public void HandleDiscoveryTestsShouldJustPassOnTheEventToDiscoveryEventsHandler()
    {
        var tests = new List<TestCase>();
        _parallelDiscoveryEventsHandler.HandleDiscoveredTests(tests);

        _mockTestDiscoveryEventsHandler.Verify(mt => mt.HandleDiscoveredTests(tests), Times.Once);
    }

    [TestMethod]
    public void HandleRawMessageShouldSendTestCasesFoundRawMessageToDiscoveryEventsHandler()
    {
        string payload = "Tests";
        _mockDataSerializer.Setup(mds => mds.DeserializeMessage(It.IsAny<string>()))
            .Returns(new Message() { MessageType = MessageType.TestCasesFound, Payload = payload });

        _parallelDiscoveryEventsHandler.HandleRawMessage(payload);

        _mockTestDiscoveryEventsHandler.Verify(mt => mt.HandleRawMessage(payload), Times.Once);
    }

    [TestMethod]
    public void HandleRawMessageShouldNotSendDiscoveryCompleteEventRawMessageToDiscoveryEventsHandler()
    {
        string payload = "DiscoveryComplete";
        _mockDataSerializer.Setup(mds => mds.DeserializeMessage(It.IsAny<string>()))
            .Returns(new Message() { MessageType = MessageType.DiscoveryComplete, Payload = payload });

        _parallelDiscoveryEventsHandler.HandleRawMessage(payload);

        _mockTestDiscoveryEventsHandler.Verify(mt => mt.HandleRawMessage(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public void HandleRawMessageShouldSendLoggerRawMessageToDiscoveryEventsHandler()
    {
        string payload = "LogMessage";
        _mockDataSerializer.Setup(mds => mds.DeserializeMessage(It.IsAny<string>()))
            .Returns(new Message() { MessageType = MessageType.TestMessage, Payload = payload });

        _parallelDiscoveryEventsHandler.HandleRawMessage(payload);

        _mockTestDiscoveryEventsHandler.Verify(mt => mt.HandleRawMessage(payload), Times.Once);
    }

    [TestMethod]
    public void HandleLogMessageShouldJustPassOnTheEventToDiscoveryEventsHandler()
    {
        string log = "Hello";
        _parallelDiscoveryEventsHandler.HandleLogMessage(TestMessageLevel.Error, log);

        _mockTestDiscoveryEventsHandler.Verify(mt =>
            mt.HandleLogMessage(TestMessageLevel.Error, log), Times.Once);
    }
}
