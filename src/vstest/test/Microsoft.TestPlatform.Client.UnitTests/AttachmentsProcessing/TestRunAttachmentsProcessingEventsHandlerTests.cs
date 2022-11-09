// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.Client.TestRunAttachmentsProcessing;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.TestPlatform.Client.UnitTests.TestRunAttachmentsProcessing;

[TestClass]
public class TestRunAttachmentsProcessingEventsHandlerTests
{
    private readonly Mock<ICommunicationManager> _mockCommunicationManager;
    private readonly ITestRunAttachmentsProcessingEventsHandler _handler;

    public TestRunAttachmentsProcessingEventsHandlerTests()
    {
        _mockCommunicationManager = new Mock<ICommunicationManager>();
        _handler = new TestRunAttachmentsProcessingEventsHandler(_mockCommunicationManager.Object);
    }

    [TestMethod]
    public void EventsHandlerHandleLogMessageShouldSendTestMessage()
    {
        string message = "error message";

        _handler.HandleLogMessage(TestMessageLevel.Error, message);

        _mockCommunicationManager.Verify(cm => cm.SendMessage(MessageType.TestMessage, It.Is<TestMessagePayload>(p => p.MessageLevel == TestMessageLevel.Error && p.Message == message)));
    }

    [TestMethod]
    public void EventsHandlerHandleTestRunAttachmentsProcessingCompleteShouldSendAttachmentsProcessingCompleteMessage()
    {
        var attachments = new[] { new AttachmentSet(new System.Uri("http://www.bing.com/"), "code coverage") };
        var args = new TestRunAttachmentsProcessingCompleteEventArgs(false, null);

        _handler.HandleTestRunAttachmentsProcessingComplete(args, attachments);

        _mockCommunicationManager.Verify(cm => cm.SendMessage(MessageType.TestRunAttachmentsProcessingComplete, It.Is<TestRunAttachmentsProcessingCompletePayload>(p => p.Attachments == attachments && p.AttachmentsProcessingCompleteEventArgs == args)));
    }

    [TestMethod]
    public void EventsHandlerHandleTestRunAttachmentsProcessingProgressShouldSendAttachmentsProcessingProgressMessage()
    {
        var args = new TestRunAttachmentsProcessingProgressEventArgs(1, new[] { new System.Uri("http://www.bing.com/") }, 90, 2);

        _handler.HandleTestRunAttachmentsProcessingProgress(args);

        _mockCommunicationManager.Verify(cm => cm.SendMessage(MessageType.TestRunAttachmentsProcessingProgress, It.Is<TestRunAttachmentsProcessingProgressPayload>(p => p.AttachmentsProcessingProgressEventArgs == args)));
    }

    [TestMethod]
    public void EventsHandlerHandleRawMessageShouldDoNothing()
    {
        _handler.HandleRawMessage("any");

        _mockCommunicationManager.Verify(cm => cm.SendMessage(It.IsAny<string>()), Times.Never);
        _mockCommunicationManager.Verify(cm => cm.SendMessage(It.IsAny<string>(), It.IsAny<object>()), Times.Never);
    }
}
