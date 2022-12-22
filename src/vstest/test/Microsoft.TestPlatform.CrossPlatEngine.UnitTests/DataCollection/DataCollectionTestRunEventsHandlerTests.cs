// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.TestPlatform.CrossPlatEngine.UnitTests.DataCollection;

[TestClass]
public class DataCollectionTestRunEventsHandlerTests
{
    private readonly Mock<IInternalTestRunEventsHandler> _baseTestRunEventsHandler;
    private DataCollectionTestRunEventsHandler _testRunEventHandler;
    private readonly Mock<IProxyDataCollectionManager> _proxyDataCollectionManager;
    private readonly Mock<IDataSerializer> _mockDataSerializer;

    public DataCollectionTestRunEventsHandlerTests()
    {
        _baseTestRunEventsHandler = new Mock<IInternalTestRunEventsHandler>();
        _proxyDataCollectionManager = new Mock<IProxyDataCollectionManager>();
        _mockDataSerializer = new Mock<IDataSerializer>();
        _testRunEventHandler = new DataCollectionTestRunEventsHandler(_baseTestRunEventsHandler.Object, _proxyDataCollectionManager.Object, _mockDataSerializer.Object, CancellationToken.None);
    }

    [TestMethod]
    public void HandleLogMessageShouldSendMessageToBaseTestRunEventsHandler()
    {
        _testRunEventHandler.HandleLogMessage(TestMessageLevel.Informational, null);
        _baseTestRunEventsHandler.Verify(th => th.HandleLogMessage(0, null), Times.AtLeast(1));
    }

    [TestMethod]
    public void HandleRawMessageShouldSendMessageToBaseTestRunEventsHandler()
    {
        _mockDataSerializer.Setup(x => x.DeserializeMessage(It.IsAny<string>())).Returns(new Message() { MessageType = MessageType.BeforeTestRunStart });
        _testRunEventHandler.HandleRawMessage(null!);
        _baseTestRunEventsHandler.Verify(th => th.HandleRawMessage(null!), Times.AtLeast(1));
    }

    [TestMethod]
    public void HandleRawMessageShouldGetDataCollectorAttachments()
    {
        var testRunCompleteEventArgs = new TestRunCompleteEventArgs(null, false, false, null, new Collection<AttachmentSet>(), new Collection<InvokedDataCollector>(), new TimeSpan());

        _mockDataSerializer.Setup(x => x.DeserializeMessage(It.IsAny<string>())).Returns(new Message() { MessageType = MessageType.ExecutionComplete });
        _mockDataSerializer.Setup(x => x.DeserializePayload<TestRunCompletePayload>(It.IsAny<Message>()))
            .Returns(new TestRunCompletePayload() { TestRunCompleteArgs = testRunCompleteEventArgs });

        _testRunEventHandler.HandleRawMessage(string.Empty);
        _proxyDataCollectionManager.Verify(
            dcm => dcm.AfterTestRunEnd(false, It.IsAny<IInternalTestRunEventsHandler>()),
            Times.Once);
    }

    [TestMethod]
    public void HandleRawMessageShouldInvokeAfterTestRunEndPassingFalseIfRequestNotCancelled()
    {
        var testRunCompleteEventArgs = new TestRunCompleteEventArgs(null, false, false, null, new Collection<AttachmentSet>(), new Collection<InvokedDataCollector>(), new TimeSpan());

        _mockDataSerializer.Setup(x => x.DeserializeMessage(It.IsAny<string>())).Returns(new Message() { MessageType = MessageType.ExecutionComplete });
        _mockDataSerializer.Setup(x => x.DeserializePayload<TestRunCompletePayload>(It.IsAny<Message>()))
            .Returns(new TestRunCompletePayload() { TestRunCompleteArgs = testRunCompleteEventArgs });

        var cancellationTokenSource = new CancellationTokenSource();
        _testRunEventHandler = new DataCollectionTestRunEventsHandler(_baseTestRunEventsHandler.Object, _proxyDataCollectionManager.Object, _mockDataSerializer.Object, cancellationTokenSource.Token);

        _testRunEventHandler.HandleRawMessage(string.Empty);

        _proxyDataCollectionManager.Verify(
            dcm => dcm.AfterTestRunEnd(false, It.IsAny<IInternalTestRunEventsHandler>()),
            Times.Once);
    }

    [TestMethod]
    public void HandleRawMessageShouldInvokeAfterTestRunEndPassingTrueIfRequestCancelled()
    {
        var testRunCompleteEventArgs = new TestRunCompleteEventArgs(null, false, false, null, new Collection<AttachmentSet>(), new Collection<InvokedDataCollector>(), new TimeSpan());

        _mockDataSerializer.Setup(x => x.DeserializeMessage(It.IsAny<string>())).Returns(new Message() { MessageType = MessageType.ExecutionComplete });
        _mockDataSerializer.Setup(x => x.DeserializePayload<TestRunCompletePayload>(It.IsAny<Message>()))
            .Returns(new TestRunCompletePayload() { TestRunCompleteArgs = testRunCompleteEventArgs });

        var cancellationTokenSource = new CancellationTokenSource();
        _testRunEventHandler = new DataCollectionTestRunEventsHandler(_baseTestRunEventsHandler.Object, _proxyDataCollectionManager.Object, _mockDataSerializer.Object, cancellationTokenSource.Token);
        cancellationTokenSource.Cancel();

        _testRunEventHandler.HandleRawMessage(string.Empty);

        _proxyDataCollectionManager.Verify(
            dcm => dcm.AfterTestRunEnd(true, It.IsAny<IInternalTestRunEventsHandler>()),
            Times.Once);
    }

    [TestMethod]
    public void HandleRawMessageShouldInvokeAfterTestRunEndAndReturnInvokedDataCollectors()
    {
        var invokedDataCollectors = new Collection<InvokedDataCollector>
        {
            new InvokedDataCollector(new Uri("datacollector://sample"), "sample", typeof(string).AssemblyQualifiedName!, typeof(string).Assembly.Location, true)
        };

        var testRunCompleteEventArgs = new TestRunCompleteEventArgs(null, false, false, null, new Collection<AttachmentSet>(), new Collection<InvokedDataCollector>(), new TimeSpan());
        _mockDataSerializer.Setup(x => x.DeserializeMessage(It.IsAny<string>())).Returns(new Message() { MessageType = MessageType.ExecutionComplete });
        _mockDataSerializer.Setup(x => x.DeserializePayload<TestRunCompletePayload>(It.IsAny<Message>()))
            .Returns(new TestRunCompletePayload() { TestRunCompleteArgs = testRunCompleteEventArgs });
        _proxyDataCollectionManager.Setup(p => p.AfterTestRunEnd(It.IsAny<bool>(), It.IsAny<ITestMessageEventHandler>()))
            .Returns(new DataCollectionResult(null, invokedDataCollectors));
        _mockDataSerializer.Setup(r => r.SerializePayload(It.IsAny<string>(), It.IsAny<object>())).Callback((string message, object o) =>
        {
            var testRunCompleteArgs = o as TestRunCompletePayload;
            Assert.IsNotNull(testRunCompleteArgs);
            Assert.AreEqual(1, testRunCompleteArgs.TestRunCompleteArgs!.InvokedDataCollectors.Count);
            Assert.AreEqual(invokedDataCollectors[0], testRunCompleteArgs.TestRunCompleteArgs.InvokedDataCollectors[0]);
        });

        _testRunEventHandler = new DataCollectionTestRunEventsHandler(_baseTestRunEventsHandler.Object, _proxyDataCollectionManager.Object, _mockDataSerializer.Object, CancellationToken.None);
        _testRunEventHandler.HandleRawMessage(string.Empty);

        var testRunCompleteEventArgs2 = new TestRunCompleteEventArgs(null, false, false, null, new Collection<AttachmentSet>(), new Collection<InvokedDataCollector>(), new TimeSpan());
        _testRunEventHandler.HandleTestRunComplete(testRunCompleteEventArgs2, null, null, null);
        Assert.AreEqual(1, testRunCompleteEventArgs2.InvokedDataCollectors.Count);
        Assert.AreEqual(invokedDataCollectors[0], testRunCompleteEventArgs2.InvokedDataCollectors[0]);

        _proxyDataCollectionManager.Verify(
            dcm => dcm.AfterTestRunEnd(false, It.IsAny<IInternalTestRunEventsHandler>()),
            Times.Once);
    }

    #region Get Combined Attachments
    [TestMethod]
    public void GetCombinedAttachmentSetsShouldReturnCombinedAttachments()
    {
        Collection<AttachmentSet> attachments1 = new();
        AttachmentSet attachmentset1 = new(new Uri("DataCollection://Attachment/v1"), "AttachmentV1");
        attachmentset1.Attachments.Add(new UriDataAttachment(new Uri("DataCollection://Attachment/v11"), "AttachmentV1-Attachment1"));
        attachments1.Add(attachmentset1);

        Collection<AttachmentSet> attachments2 = new();
        AttachmentSet attachmentset2 = new(new Uri("DataCollection://Attachment/v1"), "AttachmentV1");
        attachmentset2.Attachments.Add(new UriDataAttachment(new Uri("DataCollection://Attachment/v12"), "AttachmentV1-Attachment2"));

        attachments2.Add(attachmentset2);

        var result = DataCollectionTestRunEventsHandler.GetCombinedAttachmentSets(attachments1, attachments2);

        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(2, result.First().Attachments.Count);
    }

    [TestMethod]
    public void GetCombinedAttachmentSetsShouldReturnFirstArgumentIfSecondArgumentIsNull()
    {
        Collection<AttachmentSet> attachments1 = new();
        AttachmentSet attachmentset1 = new(new Uri("DataCollection://Attachment/v1"), "AttachmentV1");
        attachmentset1.Attachments.Add(new UriDataAttachment(new Uri("DataCollection://Attachment/v11"), "AttachmentV1-Attachment1"));
        attachments1.Add(attachmentset1);

        var result = DataCollectionTestRunEventsHandler.GetCombinedAttachmentSets(attachments1, null);

        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(1, result.First().Attachments.Count);
    }

    [TestMethod]
    public void GetCombinedAttachmentSetsShouldReturnNullIfFirstArgumentIsNull()
    {
        var result = DataCollectionTestRunEventsHandler.GetCombinedAttachmentSets(null, null);
        Assert.IsNull(result);
    }

    #endregion
}
