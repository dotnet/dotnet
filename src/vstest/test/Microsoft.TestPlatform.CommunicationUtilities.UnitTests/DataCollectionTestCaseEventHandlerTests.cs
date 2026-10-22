// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;
using System.Net;

using Microsoft.VisualStudio.TestPlatform.Common.DataCollector.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.DataCollection;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Newtonsoft.Json.Linq;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests;

[TestClass]
public class DataCollectionTestCaseEventHandlerTests
{
    private readonly Mock<ICommunicationManager> _mockCommunicationManager;
    private readonly Mock<IDataCollectionManager> _mockDataCollectionManager;
    private readonly DataCollectionTestCaseEventHandler _requestHandler;
    private readonly Mock<IDataSerializer> _dataSerializer;
    private readonly Mock<IMessageSink> _messageSink;

    public DataCollectionTestCaseEventHandlerTests()
    {
        _mockCommunicationManager = new Mock<ICommunicationManager>();
        _mockDataCollectionManager = new Mock<IDataCollectionManager>();
        _dataSerializer = new Mock<IDataSerializer>();
        _messageSink = new Mock<IMessageSink>();
        _requestHandler = new DataCollectionTestCaseEventHandler(_messageSink.Object, _mockCommunicationManager.Object, new Mock<IDataCollectionManager>().Object, _dataSerializer.Object);
    }

    [TestMethod]
    public void InitializeShouldInitializeConnection()
    {
        _mockCommunicationManager.Setup(x => x.HostServer(new IPEndPoint(IPAddress.Loopback, 0))).Returns(new IPEndPoint(IPAddress.Loopback, 1));
        _requestHandler.InitializeCommunication();

        _mockCommunicationManager.Verify(x => x.HostServer(new IPEndPoint(IPAddress.Loopback, 0)), Times.Once);
        _mockCommunicationManager.Verify(x => x.AcceptClientAsync(), Times.Once);
    }

    [TestMethod]
    public void InitializeShouldThrowExceptionIfExceptionIsThrownByCommunicationManager()
    {
        _mockCommunicationManager.Setup(x => x.HostServer(new IPEndPoint(IPAddress.Loopback, 0))).Throws<Exception>();
        Assert.ThrowsException<Exception>(() => _requestHandler.InitializeCommunication());
    }

    [TestMethod]
    public void WaitForRequestHandlerConnectionShouldWaitForConnectionToBeCompleted()
    {
        _mockCommunicationManager.Setup(x => x.WaitForClientConnection(It.IsAny<int>())).Returns(true);

        var result = _requestHandler.WaitForRequestHandlerConnection(10);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void WaitForRequestHandlerConnectionShouldThrowExceptionIfThrownByConnectionManager()
    {
        _mockCommunicationManager.Setup(x => x.WaitForClientConnection(It.IsAny<int>())).Throws<Exception>();

        Assert.ThrowsException<Exception>(() => _requestHandler.WaitForRequestHandlerConnection(10));
    }

    [TestMethod]
    public void CloseShouldStopServer()
    {
        _requestHandler.Close();

        _mockCommunicationManager.Verify(x => x.StopServer(), Times.Once);
    }

    [TestMethod]
    public void CloseShouldThrowExceptionIfThrownByCommunicationManager()
    {
        _mockCommunicationManager.Setup(x => x.StopServer()).Throws<Exception>();

        Assert.ThrowsException<Exception>(
            () => _requestHandler.Close());
    }

    [TestMethod]
    public void CloseShouldNotThrowExceptionIfCommunicationManagerIsNull()
    {
        var requestHandler = new DataCollectionTestCaseEventHandler(_messageSink.Object, null!, new Mock<IDataCollectionManager>().Object, _dataSerializer.Object);

        requestHandler.Close();

        _mockCommunicationManager.Verify(x => x.StopServer(), Times.Never);
    }

    [TestMethod]
    public void ProcessRequestsShouldProcessBeforeTestCaseStartEvent()
    {
        var message = new Message();
        message.MessageType = MessageType.DataCollectionTestStart;
        message.Payload = JToken.FromObject(new TestCaseEndEventArgs());

        _mockCommunicationManager.SetupSequence(x => x.ReceiveMessage()).Returns(message).Returns(new Message() { MessageType = MessageType.SessionEnd, Payload = "false" });

        var requestHandler = new DataCollectionTestCaseEventHandler(_messageSink.Object, _mockCommunicationManager.Object, _mockDataCollectionManager.Object, _dataSerializer.Object);
        _dataSerializer.Setup(x => x.DeserializePayload<TestCaseStartEventArgs>(message)).Returns(new TestCaseStartEventArgs());

        requestHandler.ProcessRequests();

        _mockDataCollectionManager.Verify(x => x.TestCaseStarted(It.IsAny<TestCaseStartEventArgs>()), Times.Once);
    }

    [TestMethod]
    public void ProcessRequestsShouldProcessAfterTestCaseCompleteEvent()
    {
        var message = new Message();
        message.MessageType = MessageType.DataCollectionTestEnd;
        var testCase = new TestCase("hello", new Uri("world://how"), "1.dll");
        message.Payload = JToken.FromObject(new TestResultEventArgs(new VisualStudio.TestPlatform.ObjectModel.TestResult(testCase)));

        _mockCommunicationManager.SetupSequence(x => x.ReceiveMessage()).Returns(message).Returns(new Message() { MessageType = MessageType.SessionEnd, Payload = "false" });

        var requestHandler = new DataCollectionTestCaseEventHandler(_messageSink.Object, _mockCommunicationManager.Object, _mockDataCollectionManager.Object, _dataSerializer.Object);
        _dataSerializer.Setup(x => x.DeserializePayload<TestCaseEndEventArgs>(message)).Returns(new TestCaseEndEventArgs());

        requestHandler.ProcessRequests();

        _mockDataCollectionManager.Verify(x => x.TestCaseEnded(It.IsAny<TestCaseEndEventArgs>()), Times.Once);
        _mockCommunicationManager.Verify(x => x.SendMessage(MessageType.DataCollectionTestEndResult, It.IsAny<Collection<AttachmentSet>>()));
    }

    [TestMethod]
    public void ProcessRequestsShouldThrowExceptionIfThrownByCommunicationManager()
    {
        _mockCommunicationManager.Setup(x => x.ReceiveMessage()).Throws<Exception>();

        Assert.ThrowsException<Exception>(() => _requestHandler.ProcessRequests());
    }
}
