// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.Common.DataCollector.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.VisualStudio.TestPlatform.Common.DataCollector.UnitTests;

[TestClass]
public class DataCollectionAttachmentManagerTests
{
    private const int Timeout = 10 * 60 * 1000;
    private readonly DataCollectionAttachmentManager _attachmentManager;
    private readonly Mock<IMessageSink> _messageSink;
    private readonly SessionId _sessionId;
    private static readonly string TempDirectoryPath = Path.GetTempPath();

    public TestContext TestContext { get; set; }

    public DataCollectionAttachmentManagerTests()
    {
        _attachmentManager = new DataCollectionAttachmentManager();
        _messageSink = new Mock<IMessageSink>();
        var guid = Guid.NewGuid();
        _sessionId = new SessionId(guid);
    }

    [TestCleanup]
    public void Cleanup()
    {
        File.Delete(Path.Combine(TempDirectoryPath, "filename.txt"));
        File.Delete(Path.Combine(TempDirectoryPath, "filename1.txt"));
    }

    [TestMethod]
    public void ParallelAccessShouldNotBreak()
    {
        string outputDirectory = Path.Combine(TempDirectoryPath, Guid.NewGuid().ToString());
        var dataCollectorSessionId = new SessionId(Guid.NewGuid());

        try
        {
            _attachmentManager.Initialize(dataCollectorSessionId, outputDirectory, _messageSink.Object);

            CancellationTokenSource cts = new(TimeSpan.FromSeconds(3));
            List<Task> parallelTasks = new();
            int totalTasks = 3;

            // 3 tasks are enough to break bugged code
            for (int i = 0; i < totalTasks; i++)
            {
                parallelTasks.Add(Task.Run(() =>
                {
                    while (true)
                    {
                        if (cts.IsCancellationRequested)
                        {
                            break;
                        }
                        _ = TestCaseEvent($"test_{Guid.NewGuid()}");
                    }
                }, TestContext.CancellationToken));
            }

            Task.WaitAll(parallelTasks.ToArray(), TestContext.CancellationToken);
        }
        finally
        {
            if (Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, true);
            }
        }

        List<AttachmentSet> TestCaseEvent(string uri)
        {
            var testCaseCtx = new DataCollectionContext(dataCollectorSessionId, new TestExecId(Guid.NewGuid()));
            string path = Path.Combine(outputDirectory, Guid.NewGuid().ToString());
            File.WriteAllText(path, "test");
            _attachmentManager.AddAttachment(new FileTransferInformation(testCaseCtx, path, true), null, new Uri($"//{uri}"), $"{uri}");
            return _attachmentManager.GetAttachments(testCaseCtx);
        }
    }

    [TestMethod]
    public void InitializeShouldThrowExceptionIfSessionIdIsNull()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => _attachmentManager.Initialize(null!, string.Empty, _messageSink.Object));
    }

    [TestMethod]
    public void InitializeShouldThrowExceptionIfMessageSinkIsNull()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => _attachmentManager.Initialize(_sessionId, string.Empty, null!));
    }

    [TestMethod]
    public void InitializeShouldSetDefaultPathIfOutputDirectoryPathIsNull()
    {
        _attachmentManager.Initialize(_sessionId, string.Empty, _messageSink.Object);

        Assert.AreEqual(_attachmentManager.SessionOutputDirectory, Path.Combine(Path.GetTempPath(), "TestResults", _sessionId.Id.ToString()));
    }

    [TestMethod]
    public void InitializeShouldSetCorrectGuidAndOutputPath()
    {
        _attachmentManager.Initialize(_sessionId, TempDirectoryPath, _messageSink.Object);

        Assert.AreEqual(Path.Combine(TempDirectoryPath, _sessionId.Id.ToString()), _attachmentManager.SessionOutputDirectory);
    }

    [TestMethod]
    public void AddAttachmentShouldNotAddNewFileTransferIfSessionIsNotConfigured()
    {
        var filename = "filename.txt";
        File.WriteAllText(Path.Combine(TempDirectoryPath, filename), string.Empty);

        var datacollectioncontext = new DataCollectionContext(_sessionId);
        var friendlyName = "TestDataCollector";
        var uri = new Uri("datacollector://Company/Product/Version");

        var dataCollectorDataMessage = new FileTransferInformation(datacollectioncontext, Path.Combine(TempDirectoryPath, filename), false);

        _attachmentManager.AddAttachment(dataCollectorDataMessage, null, uri, friendlyName);

        Assert.IsEmpty(_attachmentManager.AttachmentSets);
    }

    [TestMethod]
    public void AddAttachmentShouldAddNewFileTransferAndCopyFileToOutputDirectoryIfDeleteFileIsFalse()
    {
        var filename = "filename.txt";
        File.WriteAllText(Path.Combine(TempDirectoryPath, filename), string.Empty);


        _attachmentManager.Initialize(_sessionId, TempDirectoryPath, _messageSink.Object);

        var datacollectioncontext = new DataCollectionContext(_sessionId);
        var friendlyName = "TestDataCollector";
        var uri = new Uri("datacollector://Company/Product/Version");

        EventWaitHandle waitHandle = new AutoResetEvent(false);
        var handler = new AsyncCompletedEventHandler((a, e) => waitHandle.Set());
        var dataCollectorDataMessage = new FileTransferInformation(datacollectioncontext, Path.Combine(TempDirectoryPath, filename), false);


        _attachmentManager.AddAttachment(dataCollectorDataMessage, handler, uri, friendlyName);

        // Wait for file operations to complete
        waitHandle.WaitOne(Timeout);

        Assert.IsTrue(File.Exists(Path.Combine(TempDirectoryPath, filename)));
        Assert.IsTrue(File.Exists(Path.Combine(TempDirectoryPath, _sessionId.Id.ToString(), filename)));
        Assert.HasCount(1, _attachmentManager.AttachmentSets[datacollectioncontext][uri].Attachments);
    }

    [TestMethod]
    public void AddAttachmentsShouldAddFilesCorrespondingToDifferentDataCollectors()
    {
        var filename = "filename.txt";
        var filename1 = "filename1.txt";
        File.WriteAllText(Path.Combine(TempDirectoryPath, filename), string.Empty);
        File.WriteAllText(Path.Combine(TempDirectoryPath, filename1), string.Empty);

        _attachmentManager.Initialize(_sessionId, TempDirectoryPath, _messageSink.Object);

        var datacollectioncontext = new DataCollectionContext(_sessionId);
        var friendlyName = "TestDataCollector";
        var uri = new Uri("datacollector://Company/Product/Version");
        var uri1 = new Uri("datacollector://Company/Product/Version1");

        EventWaitHandle waitHandle = new AutoResetEvent(false);
        var handler = new AsyncCompletedEventHandler((a, e) => waitHandle.Set());
        var dataCollectorDataMessage = new FileTransferInformation(datacollectioncontext, Path.Combine(TempDirectoryPath, filename), false);

        _attachmentManager.AddAttachment(dataCollectorDataMessage, handler, uri, friendlyName);

        // Wait for file operations to complete
        waitHandle.WaitOne(Timeout);

        waitHandle.Reset();
        dataCollectorDataMessage = new FileTransferInformation(datacollectioncontext, Path.Combine(TempDirectoryPath, filename1), false);
        _attachmentManager.AddAttachment(dataCollectorDataMessage, handler, uri1, friendlyName);

        // Wait for file operations to complete
        waitHandle.WaitOne(Timeout);

        Assert.HasCount(1, _attachmentManager.AttachmentSets[datacollectioncontext][uri].Attachments);
        Assert.HasCount(1, _attachmentManager.AttachmentSets[datacollectioncontext][uri1].Attachments);
    }

    [TestMethod]
    public void AddAttachmentShouldAddNewFileTransferAndMoveFileToOutputDirectoryIfDeleteFileIsTrue()
    {
        var filename = "filename1.txt";
        File.WriteAllText(Path.Combine(TempDirectoryPath, filename), string.Empty);


        _attachmentManager.Initialize(_sessionId, TempDirectoryPath, _messageSink.Object);

        var datacollectioncontext = new DataCollectionContext(_sessionId);
        var friendlyName = "TestDataCollector";
        var uri = new Uri("datacollector://Company/Product/Version");

        var waitHandle = new AutoResetEvent(false);
        var handler = new AsyncCompletedEventHandler((a, e) => waitHandle.Set());
        var dataCollectorDataMessage = new FileTransferInformation(datacollectioncontext, Path.Combine(TempDirectoryPath, filename), true);

        _attachmentManager.AddAttachment(dataCollectorDataMessage, handler, uri, friendlyName);

        // Wait for file operations to complete
        waitHandle.WaitOne(Timeout);

        Assert.HasCount(1, _attachmentManager.AttachmentSets[datacollectioncontext][uri].Attachments);
        Assert.IsTrue(File.Exists(Path.Combine(TempDirectoryPath, _sessionId.Id.ToString(), filename)));
        Assert.IsFalse(File.Exists(Path.Combine(TempDirectoryPath, filename)));
    }

    [TestMethod]
    public void AddAttachmentShouldAddMultipleAttachmentsForSameDc()
    {
        var filename = "filename.txt";
        var filename1 = "filename1.txt";
        File.WriteAllText(Path.Combine(TempDirectoryPath, filename), string.Empty);
        File.WriteAllText(Path.Combine(TempDirectoryPath, filename1), string.Empty);

        _attachmentManager.Initialize(_sessionId, TempDirectoryPath, _messageSink.Object);

        var datacollectioncontext = new DataCollectionContext(_sessionId);
        var friendlyName = "TestDataCollector";
        var uri = new Uri("datacollector://Company/Product/Version");

        EventWaitHandle waitHandle = new AutoResetEvent(false);
        var handler = new AsyncCompletedEventHandler((a, e) => waitHandle.Set());
        var dataCollectorDataMessage = new FileTransferInformation(datacollectioncontext, Path.Combine(TempDirectoryPath, filename), false);

        _attachmentManager.AddAttachment(dataCollectorDataMessage, handler, uri, friendlyName);

        // Wait for file operations to complete
        waitHandle.WaitOne(Timeout);

        waitHandle.Reset();
        dataCollectorDataMessage = new FileTransferInformation(datacollectioncontext, Path.Combine(TempDirectoryPath, filename1), false);
        _attachmentManager.AddAttachment(dataCollectorDataMessage, handler, uri, friendlyName);

        // Wait for file operations to complete
        waitHandle.WaitOne(Timeout);

        Assert.HasCount(2, _attachmentManager.AttachmentSets[datacollectioncontext][uri].Attachments);
    }

    [TestMethod]
    public void AddAttachmentShouldNotAddNewFileTransferIfNullIsPassed()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => _attachmentManager.AddAttachment(null!, null, null!, null!));
    }

    [TestMethod]
    public void GetAttachmentsShouldReturnAllAttachments()
    {
        var filename = "filename1.txt";
        File.WriteAllText(Path.Combine(TempDirectoryPath, filename), string.Empty);

        _attachmentManager.Initialize(_sessionId, TempDirectoryPath, _messageSink.Object);

        var datacollectioncontext = new DataCollectionContext(_sessionId);
        var friendlyName = "TestDataCollector";
        var uri = new Uri("datacollector://Company/Product/Version");

        var dataCollectorDataMessage = new FileTransferInformation(datacollectioncontext, Path.Combine(TempDirectoryPath, filename), true);

        _attachmentManager.AddAttachment(dataCollectorDataMessage, null, uri, friendlyName);

        Assert.HasCount(1, _attachmentManager.AttachmentSets);
        var result = _attachmentManager.GetAttachments(datacollectioncontext);

        Assert.IsEmpty(_attachmentManager.AttachmentSets);
        Assert.HasCount(1, result);
        Assert.AreEqual(friendlyName, result[0].DisplayName);
        Assert.AreEqual(uri, result[0].Uri);
        Assert.HasCount(1, result[0].Attachments);
    }

    [TestMethod]
    public void GetAttachmentsShouldNotReturnAnyDataWhenActiveFileTransferAreNotPresent()
    {
        _attachmentManager.Initialize(_sessionId, TempDirectoryPath, _messageSink.Object);

        var datacollectioncontext = new DataCollectionContext(_sessionId);

        var result = _attachmentManager.GetAttachments(datacollectioncontext);
        Assert.IsEmpty(result);
    }

    [TestMethod]
    public void GetAttachmentsShouldNotReturnAttachmentsAfterCancelled()
    {
        var fileHelper = new Mock<IFileHelper>();
        var testableAttachmentManager = new TestableDataCollectionAttachmentManager(fileHelper.Object);
        var attachmentPath = Path.Combine(TempDirectoryPath, "filename.txt");
        File.WriteAllText(attachmentPath, string.Empty);
        var datacollectioncontext = new DataCollectionContext(_sessionId);
        var friendlyName = "TestDataCollector";
        var uri = new Uri("datacollector://Company/Product/Version");
        var dataCollectorDataMessage = new FileTransferInformation(datacollectioncontext, attachmentPath, true);
        var waitHandle = new AutoResetEvent(false);
        var handler = new AsyncCompletedEventHandler((a, e) => Assert.Fail("Handler shouldn't be called since operation is canceled."));

        // We cancel the operation in the actual operation. This ensures the follow up task to is never called, attachments
        // are not added.
        Action cancelAddAttachment = () => testableAttachmentManager.Cancel();
        fileHelper.Setup(fh => fh.MoveFile(It.IsAny<string>(), It.IsAny<string>())).Callback(cancelAddAttachment);
        testableAttachmentManager.Initialize(_sessionId, TempDirectoryPath, _messageSink.Object);
        testableAttachmentManager.AddAttachment(dataCollectorDataMessage, handler, uri, friendlyName);

        // Wait for the attachment transfer tasks to complete
        var result = testableAttachmentManager.GetAttachments(datacollectioncontext);
        Assert.IsEmpty(result[0].Attachments);
    }

    private class TestableDataCollectionAttachmentManager : DataCollectionAttachmentManager
    {
        public TestableDataCollectionAttachmentManager(IFileHelper fileHelper)
            : base(fileHelper)
        {
        }
    }
}
