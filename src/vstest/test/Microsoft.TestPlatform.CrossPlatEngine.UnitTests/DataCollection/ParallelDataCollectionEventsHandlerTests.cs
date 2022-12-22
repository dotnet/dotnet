// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.Parallel;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Constants = Microsoft.VisualStudio.TestPlatform.ObjectModel.Constants;

namespace Microsoft.TestPlatform.CrossPlatEngine.UnitTests.DataCollection;

[TestClass]
public class ParallelDataCollectionEventsHandlerTests
{
    private const string Uri1 = "datacollector://microsoft/some1/1.0";
    private const string Uri2 = "datacollector://microsoft/some2/2.0";
    private const string Uri3 = "datacollector://microsoft/some3/2.0";

    private readonly Mock<IRequestData> _mockRequestData;
    private readonly Mock<IProxyExecutionManager> _mockProxyExecutionManager;
    private readonly Mock<IInternalTestRunEventsHandler> _mockTestRunEventsHandler;
    private readonly Mock<IParallelProxyExecutionManager> _mockParallelProxyExecutionManager;
    private readonly Mock<ITestRunAttachmentsProcessingManager> _mockTestRunAttachmentsProcessingManager;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly ParallelDataCollectionEventsHandler _parallelDataCollectionEventsHandler;

    public ParallelDataCollectionEventsHandlerTests()
    {
        _mockRequestData = new Mock<IRequestData>();
        _mockRequestData.Setup(r => r.ProtocolConfig).Returns(new ProtocolConfig());
        _mockProxyExecutionManager = new Mock<IProxyExecutionManager>();
        _mockTestRunEventsHandler = new Mock<IInternalTestRunEventsHandler>();
        _mockParallelProxyExecutionManager = new Mock<IParallelProxyExecutionManager>();
        _mockTestRunAttachmentsProcessingManager = new Mock<ITestRunAttachmentsProcessingManager>();
        _cancellationTokenSource = new CancellationTokenSource();
        _parallelDataCollectionEventsHandler = new ParallelDataCollectionEventsHandler(_mockRequestData.Object, _mockProxyExecutionManager.Object, _mockTestRunEventsHandler.Object,
            _mockParallelProxyExecutionManager.Object, new ParallelRunDataAggregator(Constants.EmptyRunSettings), _mockTestRunAttachmentsProcessingManager.Object, _cancellationTokenSource.Token);

        _mockParallelProxyExecutionManager.Setup(m => m.HandlePartialRunComplete(It.IsAny<IProxyExecutionManager>(), It.IsAny<TestRunCompleteEventArgs>(), It.IsAny<TestRunChangedEventArgs>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<ICollection<string>>())).Returns(true);
    }

    [TestMethod]
    public void HandleTestRunComplete_ShouldCallProcessTestRunAttachmentsAsyncWithAttachmentsAndUseResults()
    {
        // arrange
        List<AttachmentSet> inputAttachments = new()
        {
            new AttachmentSet(new Uri(Uri1), "uri1_input1"),
            new AttachmentSet(new Uri(Uri2), "uri2_input1"),
            new AttachmentSet(new Uri(Uri3), "uri3_input1")
        };

        Collection<AttachmentSet> outputAttachments = new()
        {
            new AttachmentSet(new Uri(Uri1), "uri1_input1")
        };

        _mockTestRunAttachmentsProcessingManager.Setup(f => f.ProcessTestRunAttachmentsAsync(Constants.EmptyRunSettings, _mockRequestData.Object, It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<ICollection<InvokedDataCollector>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(outputAttachments));

        // act
        _parallelDataCollectionEventsHandler.HandleTestRunComplete(new TestRunCompleteEventArgs(null, false, false, null, null, null, TimeSpan.FromSeconds(1)), null, inputAttachments, null);

        // assert
        _mockTestRunEventsHandler.Verify(h => h.HandleTestRunComplete(It.IsAny<TestRunCompleteEventArgs>(), It.IsAny<TestRunChangedEventArgs>(), It.Is<ICollection<AttachmentSet>>(c => c.Count == 1 && c.Contains(outputAttachments[0])), It.IsAny<ICollection<string>>()));
        _mockTestRunAttachmentsProcessingManager.Verify(f => f.ProcessTestRunAttachmentsAsync(Constants.EmptyRunSettings, _mockRequestData.Object, It.Is<ICollection<AttachmentSet>>(a => a.Count == 3), It.IsAny<ICollection<InvokedDataCollector>>(), _cancellationTokenSource.Token));
    }

    [TestMethod]
    public void HandleTestRunComplete_ShouldCallProcessTestRunAttachmentsAsyncWithAttachmentsAndNotUserResults_IfManagerReturnsNull()
    {
        // arrange
        List<AttachmentSet> inputAttachments = new()
        {
            new AttachmentSet(new Uri(Uri1), "uri1_input1"),
            new AttachmentSet(new Uri(Uri2), "uri2_input1"),
            new AttachmentSet(new Uri(Uri3), "uri3_input1")
        };

        _mockTestRunAttachmentsProcessingManager.Setup(f => f.ProcessTestRunAttachmentsAsync(Constants.EmptyRunSettings, _mockRequestData.Object, It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<ICollection<InvokedDataCollector>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult((Collection<AttachmentSet>)null!));

        // act
        _parallelDataCollectionEventsHandler.HandleTestRunComplete(new TestRunCompleteEventArgs(null, false, false, null, null, null, TimeSpan.FromSeconds(1)), null, inputAttachments, null);

        // assert
        _mockTestRunEventsHandler.Verify(h => h.HandleTestRunComplete(It.IsAny<TestRunCompleteEventArgs>(), It.IsAny<TestRunChangedEventArgs>(), It.Is<ICollection<AttachmentSet>>(c => c.Count == 3), It.IsAny<ICollection<string>>()));
        _mockTestRunAttachmentsProcessingManager.Verify(f => f.ProcessTestRunAttachmentsAsync(Constants.EmptyRunSettings, _mockRequestData.Object, It.Is<ICollection<AttachmentSet>>(a => a.Count == 3), It.IsAny<ICollection<InvokedDataCollector>>(), _cancellationTokenSource.Token));
    }
}
