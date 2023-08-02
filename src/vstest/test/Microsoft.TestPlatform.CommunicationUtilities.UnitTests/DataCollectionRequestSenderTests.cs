// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.DataCollection;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests;

[TestClass]
public class DataCollectionRequestSenderTests
{
    private readonly Mock<ICommunicationManager> _mockCommunicationManager;
    private readonly DataCollectionRequestSender _requestSender;
    private readonly Mock<IDataSerializer> _mockDataSerializer;

    public DataCollectionRequestSenderTests()
    {
        _mockCommunicationManager = new Mock<ICommunicationManager>();
        _mockDataSerializer = new Mock<IDataSerializer>();
        _requestSender = new DataCollectionRequestSender(_mockCommunicationManager.Object, _mockDataSerializer.Object);
    }

    [TestMethod]
    public void SendAfterTestRunEndAndGetResultShouldReturnAttachments()
    {
        var datacollectorUri = new Uri("my://custom/datacollector");
        var attachmentUri = new Uri("my://filename.txt");
        var displayName = "CustomDataCollector";
        var rawMessage1 = "rawMessage1";
        var rawMessage2 = "rawMessage2";
        var attachment = new AttachmentSet(datacollectorUri, displayName);
        attachment.Attachments.Add(new UriDataAttachment(attachmentUri, "filename.txt"));
        var invokedDataCollector = new InvokedDataCollector(datacollectorUri, displayName, typeof(string).AssemblyQualifiedName!, typeof(string).Assembly.Location, false);
        _mockDataSerializer.Setup(x => x.DeserializePayload<AfterTestRunEndResult>(It.IsAny<Message>())).Returns(
            new AfterTestRunEndResult(new Collection<AttachmentSet>() { attachment }, new Collection<InvokedDataCollector>() { invokedDataCollector }, new Dictionary<string, object>()));
        _mockCommunicationManager.SetupSequence(x => x.ReceiveRawMessage()).Returns(rawMessage1).Returns(rawMessage2);
        _mockDataSerializer.Setup(x => x.DeserializeMessage(rawMessage1)).Returns(new Message() { MessageType = MessageType.TelemetryEventMessage, Payload = null });
        _mockDataSerializer.Setup(x => x.DeserializeMessage(rawMessage2)).Returns(new Message() { MessageType = MessageType.AfterTestRunEndResult, Payload = null });

        var result = _requestSender.SendAfterTestRunEndAndGetResult(null, false);

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.AttachmentSets);
        Assert.IsNotNull(result.AttachmentSets);
        Assert.IsNotNull(result.Metrics);
        Assert.AreEqual(1, result.AttachmentSets.Count);
        Assert.AreEqual(1, result.InvokedDataCollectors!.Count);
        Assert.AreEqual(0, result.Metrics.Count);
        Assert.IsNotNull(result.AttachmentSets[0]);
        Assert.AreEqual(displayName, result.AttachmentSets[0].DisplayName);
        Assert.AreEqual(datacollectorUri, result.AttachmentSets[0].Uri);
        Assert.AreEqual(attachmentUri, result.AttachmentSets[0].Attachments[0].Uri);
        Assert.IsNotNull(result.InvokedDataCollectors[0]);
        Assert.AreEqual(datacollectorUri, result.InvokedDataCollectors[0].Uri);
        Assert.AreEqual(invokedDataCollector.FilePath, result.InvokedDataCollectors[0].FilePath);
        Assert.AreEqual(invokedDataCollector.AssemblyQualifiedName, result.InvokedDataCollectors[0].AssemblyQualifiedName);
    }

    [TestMethod]
    public void SendAfterTestRunEndAndGetResultShouldReturnAttachmentsAndPropagateTelemetry()
    {
        var datacollectorUri = new Uri("my://custom/datacollector");
        var attachmentUri = new Uri("my://filename.txt");
        var displayName = "CustomDataCollector";
        var rawMessage1 = "rawMessage1";
        var rawMessage2 = "rawMessage2";
        var attachment = new AttachmentSet(datacollectorUri, displayName);
        attachment.Attachments.Add(new UriDataAttachment(attachmentUri, "filename.txt"));
        var invokedDataCollector = new InvokedDataCollector(datacollectorUri, displayName, typeof(string).AssemblyQualifiedName!, typeof(string).Assembly.Location, false);
        _mockDataSerializer.Setup(x => x.DeserializePayload<AfterTestRunEndResult>(It.IsAny<Message>())).Returns(
            new AfterTestRunEndResult(new Collection<AttachmentSet>() { attachment }, new Collection<InvokedDataCollector>() { invokedDataCollector }, new Dictionary<string, object>()));
        _mockCommunicationManager.SetupSequence(x => x.ReceiveRawMessage()).Returns(rawMessage1).Returns(rawMessage2);
        _mockDataSerializer.Setup(x => x.DeserializeMessage(rawMessage1)).Returns(new Message() { MessageType = MessageType.TelemetryEventMessage, Payload = null });
        _mockDataSerializer.Setup(x => x.DeserializeMessage(rawMessage2)).Returns(new Message() { MessageType = MessageType.AfterTestRunEndResult, Payload = null });
        var handlerMock = new Mock<ITestMessageEventHandler>();

        var result = _requestSender.SendAfterTestRunEndAndGetResult(handlerMock.Object, false);

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.AttachmentSets);
        Assert.IsNotNull(result.AttachmentSets);
        Assert.IsNotNull(result.Metrics);
        Assert.AreEqual(1, result.AttachmentSets.Count);
        Assert.AreEqual(1, result.InvokedDataCollectors!.Count);
        Assert.AreEqual(0, result.Metrics.Count);
        Assert.IsNotNull(result.AttachmentSets[0]);
        Assert.AreEqual(displayName, result.AttachmentSets[0].DisplayName);
        Assert.AreEqual(datacollectorUri, result.AttachmentSets[0].Uri);
        Assert.AreEqual(attachmentUri, result.AttachmentSets[0].Attachments[0].Uri);
        Assert.IsNotNull(result.InvokedDataCollectors[0]);
        Assert.AreEqual(datacollectorUri, result.InvokedDataCollectors[0].Uri);
        Assert.AreEqual(invokedDataCollector.FilePath, result.InvokedDataCollectors[0].FilePath);
        Assert.AreEqual(invokedDataCollector.AssemblyQualifiedName, result.InvokedDataCollectors[0].AssemblyQualifiedName);

        handlerMock.Verify(h => h.HandleRawMessage(rawMessage1));
    }

    [TestMethod]
    public void SendAfterTestRunEndAndGetResultShouldNotReturnAttachmentsWhenRequestCancelled()
    {
        var attachmentSets = _requestSender.SendAfterTestRunEndAndGetResult(null, true);

        Assert.IsNull(attachmentSets);
    }

    [TestMethod]
    public void SendBeforeTestRunStartAndGetResultShouldSendBeforeTestRunStartMessageAndPayload()
    {
        var rawMessage = "rawMessage";
        var testSources = new List<string>() { "test1.dll" };
        _mockCommunicationManager.Setup(x => x.ReceiveRawMessage()).Returns(rawMessage);
        _mockDataSerializer.Setup(x => x.DeserializeMessage(rawMessage)).Returns(new Message() { MessageType = MessageType.BeforeTestRunStartResult, Payload = null });
        _requestSender.SendBeforeTestRunStartAndGetResult(string.Empty, testSources, true, null);

        _mockCommunicationManager.Verify(x => x.SendMessage(MessageType.BeforeTestRunStart, It.Is<BeforeTestRunStartPayload>(p => p.SettingsXml == string.Empty && p.IsTelemetryOptedIn)));
    }

    [TestMethod]
    public void SendBeforeTestRunStartAndGetResultShouldSendRawMessageIfTelemetry()
    {
        var rawMessage1 = "rawMessage1";
        var rawMessage2 = "rawMessage2";
        var testSources = new List<string>() { "test1.dll" };
        var handlerMock = new Mock<ITestMessageEventHandler>();
        _mockCommunicationManager.SetupSequence(x => x.ReceiveRawMessage()).Returns(rawMessage1).Returns(rawMessage2);
        _mockDataSerializer.Setup(x => x.DeserializeMessage(rawMessage1)).Returns(new Message() { MessageType = MessageType.TelemetryEventMessage, Payload = null });
        _mockDataSerializer.Setup(x => x.DeserializeMessage(rawMessage2)).Returns(new Message() { MessageType = MessageType.BeforeTestRunStartResult, Payload = null });
        _requestSender.SendBeforeTestRunStartAndGetResult(string.Empty, testSources, true, handlerMock.Object);

        handlerMock.Verify(x => x.HandleRawMessage(rawMessage1));
        _mockCommunicationManager.Verify(x => x.SendMessage(MessageType.BeforeTestRunStart, It.Is<BeforeTestRunStartPayload>(p => p.SettingsXml == string.Empty && p.IsTelemetryOptedIn)));
    }

    [TestMethod]
    public void SendBeforeTestRunStartAndGetResultShouldNotSendRawMessageIfTelemetryAndNoHandler()
    {
        var rawMessage1 = "rawMessage1";
        var rawMessage2 = "rawMessage2";
        var testSources = new List<string>() { "test1.dll" };
        _mockCommunicationManager.SetupSequence(x => x.ReceiveRawMessage()).Returns(rawMessage1).Returns(rawMessage2);
        _mockDataSerializer.Setup(x => x.DeserializeMessage(rawMessage1)).Returns(new Message() { MessageType = MessageType.TelemetryEventMessage, Payload = null });
        _mockDataSerializer.Setup(x => x.DeserializeMessage(rawMessage2)).Returns(new Message() { MessageType = MessageType.BeforeTestRunStartResult, Payload = null });
        _requestSender.SendBeforeTestRunStartAndGetResult(string.Empty, testSources, true, null);

        _mockCommunicationManager.Verify(x => x.SendMessage(MessageType.BeforeTestRunStart, It.Is<BeforeTestRunStartPayload>(p => p.SettingsXml == string.Empty && p.IsTelemetryOptedIn)));
    }
}
