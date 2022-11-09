// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.IO;

using Microsoft.VisualStudio.TestPlatform.Common.DataCollector.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollectorUnitTests;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.VisualStudio.TestPlatform.Common.DataCollector.UnitTests;

[TestClass]
public class TestPlatformDataCollectionSinkTests
{
    private readonly Mock<IDataCollectionAttachmentManager> _attachmentManager;
    private readonly DataCollectorConfig _dataCollectorConfig;
    private static readonly string TempDirectoryPath = Path.GetTempPath();

    private TestPlatformDataCollectionSink _dataCollectionSink;
    private bool _isEventHandlerInvoked;

    public TestPlatformDataCollectionSinkTests()
    {
        _attachmentManager = new Mock<IDataCollectionAttachmentManager>();
        _dataCollectorConfig = new DataCollectorConfig(typeof(CustomDataCollector));
        _dataCollectionSink = new TestPlatformDataCollectionSink(_attachmentManager.Object, _dataCollectorConfig);
        _isEventHandlerInvoked = false;
    }

    [TestCleanup]
    public void Cleanup()
    {
        File.Delete(Path.Combine(TempDirectoryPath, "filename.txt"));
    }

    [TestMethod]
    public void SendFileAsyncShouldThrowExceptionIfFileTransferInformationIsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() => _dataCollectionSink.SendFileAsync(default!));
    }

    [TestMethod]
    public void SendFileAsyncShouldInvokeAttachmentManagerWithValidFileTransferInfo()
    {
        var filename = Path.Combine(TempDirectoryPath, "filename.txt");
        File.WriteAllText(filename, string.Empty);

        var guid = Guid.NewGuid();
        var sessionId = new SessionId(guid);
        var context = new DataCollectionContext(sessionId);

        var fileTransferInfo = new FileTransferInformation(context, filename, false);

        _dataCollectionSink.SendFileAsync(fileTransferInfo);

        _attachmentManager.Verify(x => x.AddAttachment(It.IsAny<FileTransferInformation>(), It.IsAny<AsyncCompletedEventHandler>(), It.IsAny<Uri>(), It.IsAny<string>()), Times.Once());
    }

    [TestMethod]
    public void SendFileAsyncShouldInvokeSendFileCompletedIfRegistered()
    {
        var filename = Path.Combine(TempDirectoryPath, "filename.txt");
        File.WriteAllText(filename, string.Empty);

        var guid = Guid.NewGuid();
        var sessionId = new SessionId(guid);
        var context = new DataCollectionContext(sessionId);

        var fileTransferInfo = new FileTransferInformation(context, filename, false);

        var attachmentManager = new DataCollectionAttachmentManager();
        attachmentManager.Initialize(sessionId, TempDirectoryPath, new Mock<IMessageSink>().Object);

        _dataCollectionSink = new TestPlatformDataCollectionSink(attachmentManager, _dataCollectorConfig);
        _dataCollectionSink.SendFileCompleted += SendFileCompleted_Handler;
        _dataCollectionSink.SendFileAsync(fileTransferInfo);

        var result = attachmentManager.GetAttachments(context);

        Assert.IsNotNull(result);
        Assert.IsTrue(_isEventHandlerInvoked);
    }

    [TestMethod]
    public void SendFileAsyncShouldInvokeAttachmentManagerWithValidFileTransferInfoOverLoaded1()
    {
        var filename = Path.Combine(TempDirectoryPath, "filename.txt");
        File.WriteAllText(filename, string.Empty);

        var guid = Guid.NewGuid();
        var sessionId = new SessionId(guid);
        var context = new DataCollectionContext(sessionId);

        _dataCollectionSink.SendFileAsync(context, filename, false);

        _attachmentManager.Verify(x => x.AddAttachment(It.IsAny<FileTransferInformation>(), It.IsAny<AsyncCompletedEventHandler>(), It.IsAny<Uri>(), It.IsAny<string>()), Times.Once());
    }

    [TestMethod]
    public void SendFileAsyncShouldInvokeAttachmentManagerWithValidFileTransferInfoOverLoaded2()
    {
        var filename = Path.Combine(TempDirectoryPath, "filename.txt");
        File.WriteAllText(filename, string.Empty);

        var guid = Guid.NewGuid();
        var sessionId = new SessionId(guid);
        var context = new DataCollectionContext(sessionId);

        _dataCollectionSink.SendFileAsync(context, filename, string.Empty, false);

        _attachmentManager.Verify(x => x.AddAttachment(It.IsAny<FileTransferInformation>(), It.IsAny<AsyncCompletedEventHandler>(), It.IsAny<Uri>(), It.IsAny<string>()), Times.Once());
    }

    void SendFileCompleted_Handler(object? sender, AsyncCompletedEventArgs e)
    {
        _isEventHandlerInvoked = true;
    }
}
