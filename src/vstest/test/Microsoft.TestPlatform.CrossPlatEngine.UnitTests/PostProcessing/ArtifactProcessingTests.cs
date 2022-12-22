// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.ArtifactProcessing;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace TestPlatform.CrossPlatEngine.UnitTests;

[TestClass]
public class ArtifactProcessingTests
{
    private readonly Mock<IFileHelper> _fileHelperMock = new();
    private readonly Mock<ITestRunAttachmentsProcessingManager> _testRunAttachmentsProcessingManagerMock = new();
    private readonly Mock<ITestRunAttachmentsProcessingEventsHandler> _testRunAttachmentsProcessingEventsHandlerMock = new();
    private readonly Mock<IFeatureFlag> _featureFlagMock = new();
    private readonly Mock<IDataSerializer> _dataSerializer = new();
    private readonly Mock<ITestRunStatistics> _testRunStatistics = new();
    private ArtifactProcessingManager _artifactProcessingManager;

    public ArtifactProcessingTests()
    {
        _featureFlagMock.Setup(x => x.IsSet(It.IsAny<string>())).Returns(false);
        _fileHelperMock.Setup(x => x.GetTempPath()).Returns("/tmp");

        _artifactProcessingManager =
            new ArtifactProcessingManager(Guid.NewGuid().ToString(),
            _fileHelperMock.Object,
            _testRunAttachmentsProcessingManagerMock.Object,
            _dataSerializer.Object,
            _testRunAttachmentsProcessingEventsHandlerMock.Object,
            _featureFlagMock.Object);
    }

    [TestMethod]
    public void CollectArtifacts_NullSessionIdShouldReturn()
    {
        // arrange
        _artifactProcessingManager =
            new ArtifactProcessingManager(null,
            _fileHelperMock.Object,
            _testRunAttachmentsProcessingManagerMock.Object,
            _dataSerializer.Object,
            _testRunAttachmentsProcessingEventsHandlerMock.Object,
            _featureFlagMock.Object);

        // act
        var testRunCompleteEventArgs = new TestRunCompleteEventArgs(_testRunStatistics.Object,
            false,
            false,
            null,
            new Collection<AttachmentSet>()
            {
               new AttachmentSet(new Uri("//sample"),"")
            },
            TimeSpan.Zero);

        _artifactProcessingManager.CollectArtifacts(testRunCompleteEventArgs, string.Empty);

        //assert
        _fileHelperMock.Verify(x => x.WriteAllTextToFile(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public void CollectArtifacts_ShouldSerializeToDisk()
    {
        // arrange
        var testRunCompleteEventArgs = new TestRunCompleteEventArgs(_testRunStatistics.Object,
            false,
            false,
            null,
            new Collection<AttachmentSet>()
            {
               new AttachmentSet(new Uri("//sample"),"")
            },
            TimeSpan.Zero);

        // act
        _artifactProcessingManager.CollectArtifacts(testRunCompleteEventArgs, string.Empty);

        // assert
        _fileHelperMock.Verify(x => x.CreateDirectory(It.IsAny<string>()), Times.Once);
        _fileHelperMock.Verify(x => x.WriteAllTextToFile(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
        _dataSerializer.Verify(x => x.SerializePayload(It.IsAny<string>(), It.IsAny<TestRunCompleteEventArgs>()), Times.Once);
    }

    [TestMethod]
    public async Task PostProcessArtifactsAsync_NullSessionIdShouldReturn()
    {
        // arrange
        _artifactProcessingManager =
            new ArtifactProcessingManager(null,
            _fileHelperMock.Object,
            _testRunAttachmentsProcessingManagerMock.Object,
            _dataSerializer.Object,
            _testRunAttachmentsProcessingEventsHandlerMock.Object,
            _featureFlagMock.Object);

        // act
        await _artifactProcessingManager.PostProcessArtifactsAsync();

        // assert
        _fileHelperMock.Verify(x => x.DeleteDirectory(It.IsAny<string>(), It.IsAny<bool>()), Times.Never);
    }

    [TestMethod]
    public async Task PostProcessArtifactsAsync_NoArtifactsShouldReturn()
    {
        // arrange
        _fileHelperMock.Setup(x => x.DirectoryExists(It.IsAny<string>())).Returns(false);

        // act
        await _artifactProcessingManager.PostProcessArtifactsAsync();

        // assert
        _fileHelperMock.Verify(x => x.DeleteDirectory(It.IsAny<string>(), It.IsAny<bool>()), Times.Never);
    }

    [TestMethod]
    public async Task PostProcessArtifactsAsync_ShouldRunPostProcessing()
    {
        // arrange
        _fileHelperMock.Setup(x => x.DirectoryExists(It.IsAny<string>())).Returns(true);
        _fileHelperMock.Setup(x => x.GetFiles(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>()))
            .Returns((string path, string pattern, SearchOption so) => new string[2] { "/tmp/sessionId/runsettings.xml", "/tmp/sessionId/executionComplete.json" });
        _fileHelperMock.Setup(x => x.GetStream(It.IsAny<string>(), It.IsAny<FileMode>(), It.IsAny<FileAccess>()))
            .Returns((string path, FileMode mode, FileAccess access) =>
            {
                if (path.EndsWith("runsettings.xml"))
                {
                    return new MemoryStream(Encoding.UTF8.GetBytes(RunSettingsProviderExtensions.EmptyRunSettings));
                }

                if (path.EndsWith("executionComplete.json"))
                {
                    var testRunCompleteEventArgs = new TestRunCompleteEventArgs(null,
                    false,
                    false,
                    null,
                    new Collection<AttachmentSet>() { new AttachmentSet(new Uri("attachment://dummy"), "attachment") },
                    TimeSpan.Zero);

                    string serializedEventArgs = JsonDataSerializer.Instance.SerializePayload(MessageType.ExecutionComplete, testRunCompleteEventArgs);
                    return new MemoryStream(Encoding.UTF8.GetBytes(serializedEventArgs));
                }

                Assert.Fail();
                throw new Exception("Unexpected");
            });
        _dataSerializer.Setup(x => x.SerializePayload(It.IsAny<string>(), It.IsAny<object>())).Returns((string message, object payload)
            => JsonDataSerializer.Instance.SerializePayload(message, payload));
        _dataSerializer.Setup(x => x.DeserializeMessage(It.IsAny<string>())).Returns((string rawMessage)
            => JsonDataSerializer.Instance.DeserializeMessage(rawMessage));
        _dataSerializer.Setup(x => x.DeserializePayload<TestRunCompleteEventArgs>(It.IsAny<Message>())).Returns((Message message)
            => JsonDataSerializer.Instance.DeserializePayload<TestRunCompleteEventArgs>(message));

        // act
        await _artifactProcessingManager.PostProcessArtifactsAsync();

        // assert
        _fileHelperMock.Verify(x => x.DeleteDirectory(It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        _testRunAttachmentsProcessingManagerMock.Verify(x => x.ProcessTestRunAttachmentsAsync(It.IsAny<string>(),
            It.IsAny<IRequestData>(), It.IsAny<IEnumerable<AttachmentSet>>(), It.IsAny<IEnumerable<InvokedDataCollector>>(), It.IsAny<ITestRunAttachmentsProcessingEventsHandler>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task PostProcessArtifactsAsync_NullRunSettings_ShouldRunPostProcessing()
    {
        // arrange
        _fileHelperMock.Setup(x => x.DirectoryExists(It.IsAny<string>())).Returns(true);
        _fileHelperMock.Setup(x => x.GetFiles(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>()))
            .Returns((string path, string pattern, SearchOption so) => new string[1] { "/tmp/sessionId/executionComplete.json" });
        _fileHelperMock.Setup(x => x.GetStream(It.IsAny<string>(), It.IsAny<FileMode>(), It.IsAny<FileAccess>()))
            .Returns((string path, FileMode mode, FileAccess access) =>
            {
                if (path.EndsWith("executionComplete.json"))
                {
                    var testRunCompleteEventArgs = new TestRunCompleteEventArgs(null,
                    false,
                    false,
                    null,
                    new Collection<AttachmentSet>() { new AttachmentSet(new Uri("attachment://dummy"), "attachment") },
                    TimeSpan.Zero);

                    string serializedEventArgs = JsonDataSerializer.Instance.SerializePayload(MessageType.ExecutionComplete, testRunCompleteEventArgs);
                    return new MemoryStream(Encoding.UTF8.GetBytes(serializedEventArgs));
                }

                Assert.Fail();
                throw new Exception("Unexpected");
            });
        _dataSerializer.Setup(x => x.SerializePayload(It.IsAny<string>(), It.IsAny<object>())).Returns((string message, object payload)
            => JsonDataSerializer.Instance.SerializePayload(message, payload));
        _dataSerializer.Setup(x => x.DeserializeMessage(It.IsAny<string>())).Returns((string rawMessage)
            => JsonDataSerializer.Instance.DeserializeMessage(rawMessage));
        _dataSerializer.Setup(x => x.DeserializePayload<TestRunCompleteEventArgs>(It.IsAny<Message>())).Returns((Message message)
            => JsonDataSerializer.Instance.DeserializePayload<TestRunCompleteEventArgs>(message));

        // act
        await _artifactProcessingManager.PostProcessArtifactsAsync();

        // assert
        _fileHelperMock.Verify(x => x.DeleteDirectory(It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        _testRunAttachmentsProcessingManagerMock.Verify(x => x.ProcessTestRunAttachmentsAsync(It.IsAny<string>(),
            It.IsAny<IRequestData>(), It.IsAny<IEnumerable<AttachmentSet>>(), It.IsAny<IEnumerable<InvokedDataCollector>>(),
            It.IsAny<ITestRunAttachmentsProcessingEventsHandler>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task PostProcessArtifactsAsync_EmptyInvokedDataCollectors_ShouldRunPostProcessing()
    {
        // arrange
        _fileHelperMock.Setup(x => x.DirectoryExists(It.IsAny<string>())).Returns(true);
        _fileHelperMock.Setup(x => x.GetFiles(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>()))
            .Returns((string path, string pattern, SearchOption so) => new string[1] { "/tmp/sessionId/runsettings.xml" });
        _fileHelperMock.Setup(x => x.GetStream(It.IsAny<string>(), It.IsAny<FileMode>(), It.IsAny<FileAccess>()))
            .Returns((string path, FileMode mode, FileAccess access) =>
            {
                if (path.EndsWith("runsettings.xml"))
                {
                    return new MemoryStream(Encoding.UTF8.GetBytes(RunSettingsProviderExtensions.EmptyRunSettings));
                }

                Assert.Fail();
                throw new Exception("Unexpected");
            });
        _dataSerializer.Setup(x => x.SerializePayload(It.IsAny<string>(), It.IsAny<object>())).Returns((string message, object payload)
            => JsonDataSerializer.Instance.SerializePayload(message, payload));
        _dataSerializer.Setup(x => x.DeserializeMessage(It.IsAny<string>())).Returns((string rawMessage)
            => JsonDataSerializer.Instance.DeserializeMessage(rawMessage));
        _dataSerializer.Setup(x => x.DeserializePayload<TestRunCompleteEventArgs>(It.IsAny<Message>())).Returns((Message message)
            => JsonDataSerializer.Instance.DeserializePayload<TestRunCompleteEventArgs>(message));

        // act
        await _artifactProcessingManager.PostProcessArtifactsAsync();

        // assert
        _fileHelperMock.Verify(x => x.DeleteDirectory(It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        _testRunAttachmentsProcessingManagerMock.Verify(x => x.ProcessTestRunAttachmentsAsync(It.IsAny<string>(),
            It.IsAny<IRequestData>(), It.IsAny<IEnumerable<AttachmentSet>>(), It.IsAny<IEnumerable<InvokedDataCollector>>(),
            It.IsAny<ITestRunAttachmentsProcessingEventsHandler>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task PostProcessArtifactsAsync_DeserializationException_ShouldStopPostProcessing()
    {
        // arrange
        _fileHelperMock.Setup(x => x.DirectoryExists(It.IsAny<string>())).Returns(true);
        _fileHelperMock.Setup(x => x.GetFiles(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>()))
            .Returns((string path, string pattern, SearchOption so) => new string[1] { "/tmp/sessionId/executionComplete.json" });
        _fileHelperMock.Setup(x => x.GetStream(It.IsAny<string>(), It.IsAny<FileMode>(), It.IsAny<FileAccess>()))
            .Returns((string path, FileMode mode, FileAccess access) =>
            {
                if (path.EndsWith("executionComplete.json"))
                {
                    var testRunCompleteEventArgs = new TestRunCompleteEventArgs(null,
                    false,
                    false,
                    null,
                    new Collection<AttachmentSet>() { new AttachmentSet(new Uri("attachment://dummy"), "attachment") },
                    TimeSpan.Zero);

                    string serializedEventArgs = JsonDataSerializer.Instance.SerializePayload(MessageType.ExecutionComplete, testRunCompleteEventArgs);
                    return new MemoryStream(Encoding.UTF8.GetBytes(serializedEventArgs));
                }

                Assert.Fail();
                throw new Exception("Unexpected");
            });
        _dataSerializer.Setup(x => x.SerializePayload(It.IsAny<string>(), It.IsAny<object>())).Returns((string message, object payload)
            => JsonDataSerializer.Instance.SerializePayload(message, payload));
        _dataSerializer.Setup(x => x.DeserializeMessage(It.IsAny<string>())).Returns((string rawMessage) => throw new Exception("Malformed json"));

        // act
        await Assert.ThrowsExceptionAsync<Exception>(() => _artifactProcessingManager.PostProcessArtifactsAsync());

        // assert
        _fileHelperMock.Verify(x => x.DeleteDirectory(It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        _testRunAttachmentsProcessingManagerMock.Verify(x => x.ProcessTestRunAttachmentsAsync(It.IsAny<string>(),
            It.IsAny<IRequestData>(), It.IsAny<IEnumerable<AttachmentSet>>(), It.IsAny<IEnumerable<InvokedDataCollector>>(),
            It.IsAny<ITestRunAttachmentsProcessingEventsHandler>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
