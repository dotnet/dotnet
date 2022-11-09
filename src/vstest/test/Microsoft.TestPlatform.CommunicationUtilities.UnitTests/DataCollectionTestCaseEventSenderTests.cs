// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;
using System.Net;

using Microsoft.TestPlatform.CommunicationUtilities.UnitTests.TestDoubles;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Newtonsoft.Json.Linq;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests;

[TestClass]
public class DataCollectionTestCaseEventSenderTests
{
    private readonly DataCollectionTestCaseEventSender _dataCollectionTestCaseEventSender;
    private readonly Mock<ICommunicationManager> _mockCommunicationManager;
    private readonly TestCase _testCase = new("hello", new Uri("world://how"), "1.dll");

    public DataCollectionTestCaseEventSenderTests()
    {
        _mockCommunicationManager = new Mock<ICommunicationManager>();
        _dataCollectionTestCaseEventSender = new TestableDataCollectionTestCaseEventSender(_mockCommunicationManager.Object, JsonDataSerializer.Instance);
    }

    [TestMethod]
    public void InitializeShouldInitializeCommunicationManager()
    {
        _dataCollectionTestCaseEventSender.InitializeCommunication(123);

        _mockCommunicationManager.Verify(x => x.SetupClientAsync(new IPEndPoint(IPAddress.Loopback, 123)), Times.Once);
    }

    [TestMethod]
    public void InitializeShouldThrowExceptionIfThrownByCommunicationManager()
    {
        _mockCommunicationManager.Setup(x => x.SetupClientAsync(It.IsAny<IPEndPoint>())).Throws<Exception>();

        Assert.ThrowsException<Exception>(() => _dataCollectionTestCaseEventSender.InitializeCommunication(123));
    }

    [TestMethod]
    public void WaitForRequestSenderConnectionShouldInvokeWaitForServerConnection()
    {
        _dataCollectionTestCaseEventSender.WaitForRequestSenderConnection(123);

        _mockCommunicationManager.Verify(x => x.WaitForServerConnection(It.IsAny<int>()), Times.Once);
    }

    [TestMethod]
    public void WaitForRequestSenderConnectionShouldThrowExceptionIfThrownByCommunicationManager()
    {
        _mockCommunicationManager.Setup(x => x.WaitForServerConnection(It.IsAny<int>())).Throws<Exception>();

        Assert.ThrowsException<Exception>(() => _dataCollectionTestCaseEventSender.WaitForRequestSenderConnection(123));
    }

    [TestMethod]
    public void CloseShouldDisposeCommunicationManager()
    {
        _dataCollectionTestCaseEventSender.Close();

        _mockCommunicationManager.Verify(x => x.StopClient(), Times.Once);
    }

    [TestMethod]
    public void CloseShouldThrowExceptionIfThrownByCommunicationManager()
    {
        _mockCommunicationManager.Setup(x => x.StopClient()).Throws<Exception>();

        Assert.ThrowsException<Exception>(() => _dataCollectionTestCaseEventSender.Close());
    }

    [TestMethod]
    public void SendTestCaseStartShouldSendMessageThroughCommunicationManager()
    {
        _mockCommunicationManager.Setup(x => x.ReceiveMessage()).Returns(new Message() { MessageType = MessageType.DataCollectionTestStartAck });
        var testcaseStartEventArgs = new TestCaseStartEventArgs(_testCase);
        _dataCollectionTestCaseEventSender.SendTestCaseStart(testcaseStartEventArgs);

        _mockCommunicationManager.Verify(x => x.SendMessage(MessageType.DataCollectionTestStart, testcaseStartEventArgs), Times.Once);
        _mockCommunicationManager.Verify(x => x.ReceiveMessage(), Times.Once);
    }

    [TestMethod]
    public void SendTestCaseStartShouldThrowExceptionIfThrownByCommunicationManager()
    {
        var testcaseStartEventArgs = new TestCaseStartEventArgs(_testCase);
        _mockCommunicationManager.Setup(x => x.SendMessage(MessageType.DataCollectionTestStart, testcaseStartEventArgs)).Throws<Exception>();

        Assert.ThrowsException<Exception>(() => _dataCollectionTestCaseEventSender.SendTestCaseStart(testcaseStartEventArgs));
    }

    [TestMethod]
    public void SendTestCaseEndShouldReturnAttachments()
    {
        var testCaseEndEventArgs = new TestCaseEndEventArgs();

        var attachmentSet = new AttachmentSet(new Uri("my://attachment"), "displayname");
        _mockCommunicationManager.Setup(x => x.ReceiveMessage()).Returns(new Message() { MessageType = MessageType.DataCollectionTestEndResult, Payload = JToken.FromObject(new Collection<AttachmentSet>() { attachmentSet }) });
        var attachments = _dataCollectionTestCaseEventSender.SendTestCaseEnd(testCaseEndEventArgs)!;

        Assert.AreEqual(attachments[0].Uri, attachmentSet.Uri);
        Assert.AreEqual(attachments[0].DisplayName, attachmentSet.DisplayName);
    }

    [TestMethod]
    public void SendTestCaseCompletedShouldThrowExceptionIfThrownByCommunicationManager()
    {
        var testCaseEndEventArgs = new TestCaseEndEventArgs();

        _mockCommunicationManager.Setup(x => x.SendMessage(MessageType.DataCollectionTestEnd, It.IsAny<TestCaseEndEventArgs>())).Throws<Exception>();

        Assert.ThrowsException<Exception>(() => _dataCollectionTestCaseEventSender.SendTestCaseEnd(testCaseEndEventArgs));
    }
}
