// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.Client.Discovery;

using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.VisualStudio.TestPlatform.Client.UnitTests.Discovery;

[TestClass]
public class DiscoveryRequestTests
{
    readonly DiscoveryRequest _discoveryRequest;
    readonly Mock<IProxyDiscoveryManager> _discoveryManager;
    private readonly Mock<ITestLoggerManager> _loggerManager;
    readonly DiscoveryCriteria _discoveryCriteria;
    private readonly Mock<IRequestData> _mockRequestData;
    private readonly Mock<IDataSerializer> _mockDataSerializer;

    public DiscoveryRequestTests()
    {
        _discoveryCriteria = new DiscoveryCriteria(new List<string> { "foo" }, 1, null);
        _discoveryManager = new Mock<IProxyDiscoveryManager>();
        _loggerManager = new Mock<ITestLoggerManager>();
        _mockRequestData = new Mock<IRequestData>();
        _mockRequestData.Setup(rd => rd.MetricsCollection).Returns(new NoOpMetricsCollection());
        _mockDataSerializer = new Mock<IDataSerializer>();
        _discoveryRequest = new DiscoveryRequest(_mockRequestData.Object, _discoveryCriteria, _discoveryManager.Object, _loggerManager.Object, _mockDataSerializer.Object);
    }

    public static IEnumerable<object[]> ProtocolConfigVersionProvider
        => Enumerable.Range(0, Constants.DefaultProtocolConfig.Version + 1)
            .Select(x => new object[] { x });

    [TestMethod]
    public void ConstructorSetsDiscoveryCriteriaAndDiscoveryManager()
    {
        Assert.AreEqual(_discoveryCriteria, _discoveryRequest.DiscoveryCriteria);
        Assert.AreEqual(_discoveryManager.Object, (_discoveryRequest as DiscoveryRequest).DiscoveryManager);
    }

    [TestMethod]
    public void DiscoveryAsycIfDiscoveryRequestIsDisposedThrowsObjectDisposedException()
    {
        _discoveryRequest.Dispose();

        Assert.ThrowsException<ObjectDisposedException>(() => _discoveryRequest.DiscoverAsync());
    }

    [TestMethod]
    public void DiscoverAsyncSetsDiscoveryInProgressAndCallManagerToDiscoverTests()
    {
        _discoveryRequest.DiscoverAsync();

        Assert.IsTrue((_discoveryRequest as DiscoveryRequest).DiscoveryInProgress);
        _discoveryManager.Verify(dm => dm.DiscoverTests(_discoveryCriteria, _discoveryRequest as DiscoveryRequest), Times.Once);
    }

    [TestMethod]
    public void DiscoveryAsyncIfDiscoverTestsThrowsExceptionSetsDiscoveryInProgressToFalseAndThrowsThatException()
    {
        _discoveryManager.Setup(dm => dm.DiscoverTests(_discoveryCriteria, _discoveryRequest as DiscoveryRequest)).Throws(new Exception("DummyException"));
        try
        {
            _discoveryRequest.DiscoverAsync();
        }
        catch (Exception ex)
        {
            Assert.IsTrue(ex is Exception);
            Assert.AreEqual("DummyException", ex.Message);
            Assert.IsFalse((_discoveryRequest as DiscoveryRequest).DiscoveryInProgress);
        }
    }

    [TestMethod]
    public void AbortIfDiscoveryRequestDisposedShouldThrowObjectDisposedException()
    {
        _discoveryRequest.Dispose();
        Assert.ThrowsException<ObjectDisposedException>(() => _discoveryRequest.Abort());
    }

    [DataTestMethod]
    [DynamicData(nameof(ProtocolConfigVersionProvider))]
    public void AbortIfDiscoveryIsinProgressShouldCallDiscoveryManagerAbort(int version)
    {
        // Just to set the IsDiscoveryInProgress flag
        _discoveryRequest.DiscoverAsync();
        // Set the protocol version to a version not supporting new abort overload.
        Constants.DefaultProtocolConfig.Version = version;

        _discoveryRequest.Abort();

        if (version < Constants.MinimumProtocolVersionWithCancelDiscoveryEventHandlerSupport)
        {
            _discoveryManager.Verify(dm => dm.Abort(), Times.Once);
            _discoveryManager.Verify(dm => dm.Abort(_discoveryRequest), Times.Never);
        }
        else
        {
            _discoveryManager.Verify(dm => dm.Abort(), Times.Never);
            _discoveryManager.Verify(dm => dm.Abort(_discoveryRequest), Times.Once);
        }
    }

    [TestMethod]
    public void AbortIfDiscoveryIsNotInProgressShouldNotCallDiscoveryManagerAbort()
    {
        // DiscoveryAsync has not been called, discoveryInProgress should be false
        _discoveryRequest.Abort();
        _discoveryManager.Verify(dm => dm.Abort(), Times.Never);
    }

    [TestMethod]
    public void WaitForCompletionIfDiscoveryRequestDisposedShouldThrowObjectDisposedException()
    {
        _discoveryRequest.Dispose();
        Assert.ThrowsException<ObjectDisposedException>(() => _discoveryRequest.WaitForCompletion());
    }

    [TestMethod]
    public void HandleDiscoveryCompleteShouldCloseDiscoveryManager()
    {
        var eventsHandler = _discoveryRequest as ITestDiscoveryEventsHandler2;

        eventsHandler.HandleDiscoveryComplete(new DiscoveryCompleteEventArgs(1, false), Enumerable.Empty<TestCase>());
        _discoveryManager.Verify(dm => dm.Close(), Times.Once);
    }

    [TestMethod]
    public void HandleDiscoveryCompleteShouldCloseDiscoveryManagerBeforeRaiseDiscoveryComplete()
    {
        var events = new List<string>();
        _discoveryManager.Setup(dm => dm.Close()).Callback(() => events.Add("close"));
        _discoveryRequest.OnDiscoveryComplete += (s, e) => events.Add("complete");
        var eventsHandler = _discoveryRequest as ITestDiscoveryEventsHandler2;

        eventsHandler.HandleDiscoveryComplete(new DiscoveryCompleteEventArgs(1, false), Enumerable.Empty<TestCase>());

        Assert.AreEqual(2, events.Count);
        Assert.AreEqual("close", events[0]);
        Assert.AreEqual("complete", events[1]);
    }

    /// <summary>
    /// DiscoverAsync should invoke OnDiscoveryStart event.
    /// </summary>
    [TestMethod]
    public void DiscoverAsyncShouldInvokeOnDiscoveryStart()
    {
        bool onDiscoveryStartHandlerCalled = false;
        _discoveryRequest.OnDiscoveryStart += (s, e) => onDiscoveryStartHandlerCalled = true;

        // Action
        _discoveryRequest.DiscoverAsync();

        // Assert
        Assert.IsTrue(onDiscoveryStartHandlerCalled, "DiscoverAsync should invoke OnDiscoveryStart event");
    }

    [TestMethod]
    public void DiscoverAsyncShouldInvokeHandleDiscoveryStartofLoggerManager()
    {
        // Action
        _discoveryRequest.DiscoverAsync();

        // Assert
        _loggerManager.Verify(lm => lm.HandleDiscoveryStart(It.IsAny<DiscoveryStartEventArgs>()), Times.Once);
    }

    [TestMethod]
    public void HandleDiscoveryCompleteShouldCollectMetrics()
    {
        var mockMetricsCollector = new Mock<IMetricsCollection>();
        var dict = new Dictionary<string, object>
        {
            { "DummyMessage", "DummyValue" }
        };

        mockMetricsCollector.Setup(mc => mc.Metrics).Returns(dict);
        _mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollector.Object);

        var eventsHandler = _discoveryRequest as ITestDiscoveryEventsHandler2;
        var discoveryCompleteEventArgs = new DiscoveryCompleteEventArgs(1, false);
        discoveryCompleteEventArgs.Metrics = dict;

        // Act
        eventsHandler.HandleDiscoveryComplete(discoveryCompleteEventArgs, Enumerable.Empty<TestCase>());

        // Verify.
        mockMetricsCollector.Verify(rd => rd.Add(TelemetryDataConstants.TimeTakenInSecForDiscovery, It.IsAny<double>()), Times.Once);
        mockMetricsCollector.Verify(rd => rd.Add("DummyMessage", "DummyValue"), Times.Once);
    }

    [TestMethod]
    public void HandleRawMessageShouldHandleRawMessage()
    {
        bool onDiscoveryCompleteInvoked = false;
        _discoveryRequest.OnRawMessageReceived += (object? sender, string e) => onDiscoveryCompleteInvoked = true;

        _discoveryRequest.HandleRawMessage(string.Empty);

        Assert.IsTrue(onDiscoveryCompleteInvoked);
    }

    [TestMethod]
    public void HandleRawMessageShouldAddVsTestDataPointsIfTelemetryOptedIn()
    {
        bool onDiscoveryCompleteInvoked = true;
        _mockRequestData.Setup(x => x.IsTelemetryOptedIn).Returns(true);
        _discoveryRequest.OnRawMessageReceived += (object? sender, string e) => onDiscoveryCompleteInvoked = true;

        _mockDataSerializer.Setup(x => x.DeserializeMessage(It.IsAny<string>()))
            .Returns(new Message() { MessageType = MessageType.DiscoveryComplete });

        _mockDataSerializer.Setup(x => x.DeserializePayload<DiscoveryCompletePayload>(It.IsAny<Message>()))
            .Returns(new DiscoveryCompletePayload());

        _discoveryRequest.HandleRawMessage(string.Empty);

        _mockDataSerializer.Verify(x => x.SerializePayload(It.IsAny<string>(), It.IsAny<DiscoveryCompletePayload>()), Times.Once);
        _mockRequestData.Verify(x => x.MetricsCollection, Times.AtLeastOnce);
        Assert.IsTrue(onDiscoveryCompleteInvoked);
    }

    [TestMethod]
    public void HandleRawMessageShouldInvokeHandleDiscoveryCompleteOfLoggerManager()
    {
        _loggerManager.Setup(x => x.LoggersInitialized).Returns(true);
        _mockDataSerializer.Setup(x => x.DeserializeMessage(It.IsAny<string>()))
            .Returns(new Message() { MessageType = MessageType.DiscoveryComplete });
        _mockDataSerializer.Setup(x => x.DeserializePayload<DiscoveryCompletePayload>(It.IsAny<Message>()))
            .Returns(new DiscoveryCompletePayload()
            {
                TotalTests = 1,
                IsAborted = false,
                LastDiscoveredTests = Enumerable.Empty<TestCase>()
            });

        _discoveryRequest.HandleRawMessage(string.Empty);

        _loggerManager.Verify(lm => lm.HandleDiscoveryComplete(It.IsAny<DiscoveryCompleteEventArgs>()), Times.Once);
    }

    [TestMethod]
    public void HandleDiscoveryCompleteShouldInvokeHandleDiscoveryCompleteOfLoggerManager()
    {
        var discoveryCompleteEventArgs = new DiscoveryCompleteEventArgs(1, false);
        var eventsHandler = _discoveryRequest as ITestDiscoveryEventsHandler2;
        eventsHandler.HandleDiscoveryComplete(discoveryCompleteEventArgs, Enumerable.Empty<TestCase>());

        _loggerManager.Verify(lm => lm.HandleDiscoveryComplete(discoveryCompleteEventArgs), Times.Once);
    }

    [TestMethod]
    public void HandleDiscoveryCompleteShouldNotInvokeHandleDiscoveredTestsIfLastChunkNotPresent()
    {
        var discoveryCompleteEventArgs = new DiscoveryCompleteEventArgs(1, false);
        var eventsHandler = _discoveryRequest as ITestDiscoveryEventsHandler2;
        eventsHandler.HandleDiscoveryComplete(discoveryCompleteEventArgs, Enumerable.Empty<TestCase>());

        _loggerManager.Verify(lm => lm.HandleDiscoveredTests(It.IsAny<DiscoveredTestsEventArgs>()), Times.Never);
    }

    [TestMethod]
    public void HandleDiscoveryCompleteShouldInvokeHandleDiscoveredTestsIfLastChunkPresent()
    {
        var activeTestCases = new List<TestCase>
        {
            new TestCase(
                "A.C.M2",
                new Uri("executor://dummy"),
                "A")
        };

        var discoveryCompleteEventArgs = new DiscoveryCompleteEventArgs(1, false);
        var eventsHandler = _discoveryRequest as ITestDiscoveryEventsHandler2;
        eventsHandler.HandleDiscoveryComplete(discoveryCompleteEventArgs, activeTestCases);

        _loggerManager.Verify(lm => lm.HandleDiscoveredTests(It.IsAny<DiscoveredTestsEventArgs>()), Times.Once);
    }

    [TestMethod]
    public void HandleDiscoveredTestsShouldInvokeHandleDiscoveredTestsOfLoggerManager()
    {
        _discoveryRequest.HandleDiscoveredTests(null);

        _loggerManager.Verify(lm => lm.HandleDiscoveredTests(It.IsAny<DiscoveredTestsEventArgs>()), Times.Once);
    }
}
