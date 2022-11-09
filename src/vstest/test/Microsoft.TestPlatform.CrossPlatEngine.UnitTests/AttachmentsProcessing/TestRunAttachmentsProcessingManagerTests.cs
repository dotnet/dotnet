// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.TestRunAttachmentsProcessing;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.TestPlatform.CrossPlatEngine.UnitTests.TestRunAttachmentsProcessing;

[TestClass]
public class TestRunAttachmentsProcessingManagerTests
{
    private const string Uri1 = "datacollector://microsoft/some1/1.0";
    private const string Uri2 = "datacollector://microsoft/some2/2.0";
    private const string Uri3 = "datacollector://microsoft/some3/2.0";

    private readonly Mock<IRequestData> _mockRequestData;
    private readonly Mock<IMetricsCollection> _mockMetricsCollection;
    private readonly Mock<ITestPlatformEventSource> _mockEventSource;
    private readonly Mock<IDataCollectorAttachmentProcessor> _mockAttachmentHandler1;
    private readonly Mock<IDataCollectorAttachmentProcessor> _mockAttachmentHandler2;
    private readonly Mock<IDataCollectorAttachmentsProcessorsFactory> _mockDataCollectorAttachmentsProcessorsFactory;
    private readonly Mock<ITestRunAttachmentsProcessingEventsHandler> _mockEventsHandler;
    private readonly TestRunAttachmentsProcessingManager _manager;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public TestRunAttachmentsProcessingManagerTests()
    {
        _mockRequestData = new Mock<IRequestData>();
        _mockMetricsCollection = new Mock<IMetricsCollection>();
        _mockRequestData.Setup(r => r.MetricsCollection).Returns(_mockMetricsCollection.Object);

        _mockEventSource = new Mock<ITestPlatformEventSource>();
        _mockAttachmentHandler1 = new Mock<IDataCollectorAttachmentProcessor>();
        _mockAttachmentHandler1.Setup(x => x.SupportsIncrementalProcessing).Returns(true);
        _mockAttachmentHandler2 = new Mock<IDataCollectorAttachmentProcessor>();
        _mockAttachmentHandler2.Setup(x => x.SupportsIncrementalProcessing).Returns(true);
        _mockEventsHandler = new Mock<ITestRunAttachmentsProcessingEventsHandler>();
        _mockDataCollectorAttachmentsProcessorsFactory = new Mock<IDataCollectorAttachmentsProcessorsFactory>();

        _mockAttachmentHandler1.Setup(h => h.GetExtensionUris()).Returns(new[] { new Uri(Uri1) });
        _mockAttachmentHandler2.Setup(h => h.GetExtensionUris()).Returns(new[] { new Uri(Uri2) });
        _mockDataCollectorAttachmentsProcessorsFactory.Setup(p => p.Create(It.IsAny<InvokedDataCollector[]>(), It.IsAny<IMessageLogger>()))
            .Returns(new DataCollectorAttachmentProcessor[]
            {
                new DataCollectorAttachmentProcessor( "friendlyNameA", _mockAttachmentHandler1.Object ),
                new DataCollectorAttachmentProcessor( "friendlyNameB"  ,_mockAttachmentHandler2.Object )
            });

        _manager = new TestRunAttachmentsProcessingManager(_mockEventSource.Object, _mockDataCollectorAttachmentsProcessorsFactory.Object);

        _cancellationTokenSource = new CancellationTokenSource();
    }

    [TestMethod]
    public async Task ProcessTestRunAttachmentsAsync_ShouldReturnInitialAttachmentsThroughEventsHandler_IfNoAttachmentsOnInput()
    {
        // arrange
        List<AttachmentSet> inputAttachments = new();

        // act
        await _manager.ProcessTestRunAttachmentsAsync(Constants.EmptyRunSettings, _mockRequestData.Object, inputAttachments, Array.Empty<InvokedDataCollector>(), _mockEventsHandler.Object, _cancellationTokenSource.Token);

        // assert
        VerifyCompleteEvent(false, false);
        _mockEventsHandler.Verify(h => h.HandleTestRunAttachmentsProcessingComplete(It.Is<TestRunAttachmentsProcessingCompleteEventArgs>(a => !a.IsCanceled), It.Is<ICollection<AttachmentSet>>(c => c.Count == 0)));
        _mockEventsHandler.Verify(h => h.HandleLogMessage(It.IsAny<TestMessageLevel>(), It.IsAny<string>()), Times.Never);
        _mockEventsHandler.Verify(h => h.HandleTestRunAttachmentsProcessingProgress(It.IsAny<TestRunAttachmentsProcessingProgressEventArgs>()), Times.Never);
        _mockEventSource.Verify(s => s.TestRunAttachmentsProcessingStart(0));
        _mockEventSource.Verify(s => s.TestRunAttachmentsProcessingStop(0));
        _mockAttachmentHandler1.Verify(h => h.GetExtensionUris(), Times.Never);
        _mockAttachmentHandler2.Verify(h => h.GetExtensionUris(), Times.Never);
        _mockAttachmentHandler1.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockAttachmentHandler2.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()), Times.Never);

        VerifyMetrics(inputCount: 0, outputCount: 0);
    }

    [TestMethod]
    public async Task ProcessTestRunAttachmentsAsync_ShouldReturnNoAttachments_IfNoAttachmentsOnInput()
    {
        // arrange
        List<AttachmentSet> inputAttachments = new();

        // act
        var result = await _manager.ProcessTestRunAttachmentsAsync(Constants.EmptyRunSettings, _mockRequestData.Object, inputAttachments, Array.Empty<InvokedDataCollector>(), _cancellationTokenSource.Token);

        // assert
        Assert.AreEqual(0, result.Count);
        _mockAttachmentHandler1.Verify(h => h.GetExtensionUris(), Times.Never);
        _mockAttachmentHandler2.Verify(h => h.GetExtensionUris(), Times.Never);
        _mockAttachmentHandler1.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockAttachmentHandler2.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()), Times.Never);

        VerifyMetrics(inputCount: 0, outputCount: 0);
    }

    [TestMethod]
    public async Task ProcessTestRunAttachmentsAsync_ShouldReturn1NotProcessedAttachmentThroughEventsHandler_If1NotRelatedAttachmentOnInput()
    {
        // arrange
        List<AttachmentSet> inputAttachments = new()
        {
            new AttachmentSet(new Uri(Uri3), "uri3_input")
        };

        // act
        await _manager.ProcessTestRunAttachmentsAsync(Constants.EmptyRunSettings, _mockRequestData.Object, inputAttachments, Array.Empty<InvokedDataCollector>(), _mockEventsHandler.Object, _cancellationTokenSource.Token);

        // assert
        VerifyCompleteEvent(false, false, inputAttachments[0]);
        _mockEventsHandler.Verify(h => h.HandleTestRunAttachmentsProcessingProgress(It.IsAny<TestRunAttachmentsProcessingProgressEventArgs>()), Times.Never);
        _mockEventsHandler.Verify(h => h.HandleLogMessage(It.IsAny<TestMessageLevel>(), It.IsAny<string>()), Times.Never);
        _mockAttachmentHandler1.Verify(h => h.GetExtensionUris());
        _mockAttachmentHandler2.Verify(h => h.GetExtensionUris());
        _mockAttachmentHandler1.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockAttachmentHandler2.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()), Times.Never);

        VerifyMetrics(inputCount: 1, outputCount: 1);
    }

    [TestMethod]
    public async Task ProcessTestRunAttachmentsAsync_ShouldReturn1NotProcessedAttachment_If1NotRelatedAttachmentOnInput()
    {
        // arrange
        List<AttachmentSet> inputAttachments = new()
        {
            new AttachmentSet(new Uri(Uri3), "uri3_input")
        };

        // act
        var result = await _manager.ProcessTestRunAttachmentsAsync(Constants.EmptyRunSettings, _mockRequestData.Object, inputAttachments, Array.Empty<InvokedDataCollector>(), _cancellationTokenSource.Token);

        // assert
        Assert.AreEqual(1, result.Count);
        Assert.IsTrue(result.Contains(inputAttachments[0]));
        _mockAttachmentHandler1.Verify(h => h.GetExtensionUris());
        _mockAttachmentHandler2.Verify(h => h.GetExtensionUris());
        _mockAttachmentHandler1.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockAttachmentHandler2.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()), Times.Never);

        VerifyMetrics(inputCount: 1, outputCount: 1);
    }

    [TestMethod]
    public async Task ProcessTestRunAttachmentsAsync_ShouldReturn1ProcessedAttachmentThroughEventsHandler_IfRelatedAttachmentOnInput()
    {
        // arrange
        List<AttachmentSet> inputAttachments = new()
        {
            new AttachmentSet(new Uri(Uri1), "uri1_input")
        };

        List<AttachmentSet> outputAttachments = new()
        {
            new AttachmentSet(new Uri(Uri1), "uri1_output")
        };

        _mockAttachmentHandler1.Setup(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>())).ReturnsAsync(outputAttachments);

        // act
        await _manager.ProcessTestRunAttachmentsAsync(Constants.EmptyRunSettings, _mockRequestData.Object, inputAttachments, Array.Empty<InvokedDataCollector>(), _mockEventsHandler.Object, _cancellationTokenSource.Token);

        // assert
        VerifyCompleteEvent(false, false, outputAttachments[0]);
        _mockEventsHandler.Verify(h => h.HandleTestRunAttachmentsProcessingProgress(It.IsAny<TestRunAttachmentsProcessingProgressEventArgs>()), Times.Never);
        _mockEventsHandler.Verify(h => h.HandleLogMessage(It.IsAny<TestMessageLevel>(), It.IsAny<string>()), Times.Never);
        _mockAttachmentHandler1.Verify(h => h.GetExtensionUris());
        _mockAttachmentHandler2.Verify(h => h.GetExtensionUris());
        _mockAttachmentHandler1.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.Is<ICollection<AttachmentSet>>(c => c.Count == 1 && c.Contains(inputAttachments[0])), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), _cancellationTokenSource.Token));
        _mockAttachmentHandler2.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()), Times.Never);

        VerifyMetrics(inputCount: 1, outputCount: 1);
    }

    [TestMethod]
    public async Task ProcessTestRunAttachmentsAsync_ShouldReturn1ProcessedAttachment_IfRelatedAttachmentOnInput()
    {
        // arrange
        List<AttachmentSet> inputAttachments = new()
        {
            new AttachmentSet(new Uri(Uri1), "uri1_input")
        };

        List<AttachmentSet> outputAttachments = new()
        {
            new AttachmentSet(new Uri(Uri1), "uri1_output")
        };

        _mockAttachmentHandler1.Setup(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>())).ReturnsAsync(outputAttachments);

        // act
        var result = await _manager.ProcessTestRunAttachmentsAsync(Constants.EmptyRunSettings, _mockRequestData.Object, inputAttachments, Array.Empty<InvokedDataCollector>(), _cancellationTokenSource.Token);

        // assert
        Assert.AreEqual(1, result.Count);
        Assert.IsTrue(result.Contains(outputAttachments[0]));
        _mockEventSource.Verify(s => s.TestRunAttachmentsProcessingStart(1));
        _mockEventSource.Verify(s => s.TestRunAttachmentsProcessingStop(1));
        _mockAttachmentHandler1.Verify(h => h.GetExtensionUris());
        _mockAttachmentHandler2.Verify(h => h.GetExtensionUris());
        _mockAttachmentHandler1.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.Is<ICollection<AttachmentSet>>(c => c.Count == 1 && c.Contains(inputAttachments[0])), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), _cancellationTokenSource.Token));
        _mockAttachmentHandler2.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()), Times.Never);

        VerifyMetrics(inputCount: 1, outputCount: 1);
    }

    [TestMethod]
    public async Task ProcessTestRunAttachmentsAsync_ShouldReturnInitialAttachmentsThroughEventsHandler_IfRelatedAttachmentOnInputButHandlerThrowsException()
    {
        // arrange
        List<AttachmentSet> inputAttachments = new()
        {
            new AttachmentSet(new Uri(Uri1), "uri1_input")
        };

        var exceptionToThrow = new Exception("exception message");
        _mockAttachmentHandler1.Setup(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>())).Throws(exceptionToThrow);

        // act
        await _manager.ProcessTestRunAttachmentsAsync(Constants.EmptyRunSettings, _mockRequestData.Object, inputAttachments, Array.Empty<InvokedDataCollector>(), _mockEventsHandler.Object, _cancellationTokenSource.Token);

        // assert
        VerifyCompleteEvent(false, false, inputAttachments[0]);
        _mockEventsHandler.Verify(h => h.HandleTestRunAttachmentsProcessingProgress(It.IsAny<TestRunAttachmentsProcessingProgressEventArgs>()), Times.Never);
        _mockEventsHandler.Verify(h => h.HandleLogMessage(TestMessageLevel.Error, exceptionToThrow.ToString()), Times.Once);
        _mockAttachmentHandler1.Verify(h => h.GetExtensionUris());
        _mockAttachmentHandler2.Verify(h => h.GetExtensionUris(), Times.Once);
        _mockAttachmentHandler1.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.Is<ICollection<AttachmentSet>>(c => c.Count == 1 && c.Contains(inputAttachments[0])), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), _cancellationTokenSource.Token));
        _mockAttachmentHandler2.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()), Times.Never);

        VerifyMetrics(inputCount: 1, outputCount: 1);
    }

    [TestMethod]
    public async Task ProcessTestRunAttachmentsAsync_ShouldReturnInitialAttachments_IfRelatedAttachmentOnInputButHandlerThrowsException()
    {
        // arrange
        List<AttachmentSet> inputAttachments = new()
        {
            new AttachmentSet(new Uri(Uri1), "uri1_input")
        };

        _mockAttachmentHandler1.Setup(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>())).Throws(new Exception("exception message"));

        // act
        var result = await _manager.ProcessTestRunAttachmentsAsync(Constants.EmptyRunSettings, _mockRequestData.Object, inputAttachments, Array.Empty<InvokedDataCollector>(), _cancellationTokenSource.Token);

        // assert
        Assert.AreEqual(1, result.Count);
        Assert.IsTrue(result.Contains(inputAttachments[0]));
        _mockAttachmentHandler1.Verify(h => h.GetExtensionUris());
        _mockAttachmentHandler2.Verify(h => h.GetExtensionUris(), Times.Once);
        _mockAttachmentHandler1.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.Is<ICollection<AttachmentSet>>(c => c.Count == 1 && c.Contains(inputAttachments[0])), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), _cancellationTokenSource.Token));
        _mockAttachmentHandler2.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()), Times.Never);

        VerifyMetrics(inputCount: 1, outputCount: 1);
    }

    [TestMethod]
    public async Task ProcessTestRunAttachmentsAsync_ShouldReturnInitialAttachmentsThroughEventsHandler_IfOperationIsCancelled()
    {
        // arrange
        _cancellationTokenSource.Cancel();
        List<AttachmentSet> inputAttachments = new()
        {
            new AttachmentSet(new Uri(Uri1), "uri1_input")
        };

        // act
        await _manager.ProcessTestRunAttachmentsAsync(Constants.EmptyRunSettings, _mockRequestData.Object, inputAttachments, Array.Empty<InvokedDataCollector>(), _mockEventsHandler.Object, _cancellationTokenSource.Token);

        // assert
        VerifyCompleteEvent(true, false, inputAttachments[0]);
        _mockEventsHandler.Verify(h => h.HandleTestRunAttachmentsProcessingProgress(It.IsAny<TestRunAttachmentsProcessingProgressEventArgs>()), Times.Never);
        _mockAttachmentHandler1.Verify(h => h.GetExtensionUris(), Times.Never);
        _mockAttachmentHandler2.Verify(h => h.GetExtensionUris(), Times.Never);
        _mockAttachmentHandler1.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockAttachmentHandler2.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()), Times.Never);

        VerifyMetrics(inputCount: 1, outputCount: 1, status: "Canceled");
    }

    [TestMethod]
    public async Task ProcessTestRunAttachmentsAsync_ShouldReturnInitialAttachments_IfOperationIsCancelled()
    {
        // arrange
        _cancellationTokenSource.Cancel();
        List<AttachmentSet> inputAttachments = new()
        {
            new AttachmentSet(new Uri(Uri1), "uri1_input")
        };

        // act
        var result = await _manager.ProcessTestRunAttachmentsAsync(Constants.EmptyRunSettings, _mockRequestData.Object, inputAttachments, Array.Empty<InvokedDataCollector>(), _cancellationTokenSource.Token);

        // assert
        Assert.AreEqual(1, result.Count);
        Assert.IsTrue(result.Contains(inputAttachments[0]));
        _mockAttachmentHandler1.Verify(h => h.GetExtensionUris(), Times.Never);
        _mockAttachmentHandler2.Verify(h => h.GetExtensionUris(), Times.Never);
        _mockAttachmentHandler1.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockAttachmentHandler2.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()), Times.Never);

        VerifyMetrics(inputCount: 1, outputCount: 1, status: "Canceled");
    }

    [TestMethod]
    public async Task ProcessTestRunAttachmentsAsync_ShouldReturnProcessedAttachmentsThroughEventsHandler_IfRelatedAttachmentsOnInput()
    {
        // arrange
        List<AttachmentSet> inputAttachments = new()
        {
            new AttachmentSet(new Uri(Uri1), "uri1_input1"),
            new AttachmentSet(new Uri(Uri1), "uri1_input2"),
            new AttachmentSet(new Uri(Uri2), "uri2_input1"),
            new AttachmentSet(new Uri(Uri2), "uri2_input2"),
            new AttachmentSet(new Uri(Uri3), "uri3_input1"),
        };

        List<AttachmentSet> outputAttachmentsForHandler1 = new()
        {
            new AttachmentSet(new Uri(Uri1), "uri1_output")
        };

        List<AttachmentSet> outputAttachmentsForHandler2 = new()
        {
            new AttachmentSet(new Uri(Uri2), "uri2_output")
        };

        _mockAttachmentHandler1.Setup(h => h.ProcessAttachmentSetsAsync(null!, It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>())).ReturnsAsync(outputAttachmentsForHandler1);
        _mockAttachmentHandler2.Setup(h => h.ProcessAttachmentSetsAsync(null!, It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>())).ReturnsAsync(outputAttachmentsForHandler2);

        // act
        await _manager.ProcessTestRunAttachmentsAsync(Constants.EmptyRunSettings, _mockRequestData.Object, inputAttachments, Array.Empty<InvokedDataCollector>(), _mockEventsHandler.Object, _cancellationTokenSource.Token);

        // assert
        VerifyCompleteEvent(false, false, inputAttachments[4], outputAttachmentsForHandler1.First(), outputAttachmentsForHandler2.First());
        _mockEventsHandler.Verify(h => h.HandleTestRunAttachmentsProcessingProgress(It.IsAny<TestRunAttachmentsProcessingProgressEventArgs>()), Times.Never);
        _mockEventsHandler.Verify(h => h.HandleLogMessage(It.IsAny<TestMessageLevel>(), It.IsAny<string>()), Times.Never);
        _mockAttachmentHandler1.Verify(h => h.GetExtensionUris());
        _mockAttachmentHandler2.Verify(h => h.GetExtensionUris());
        _mockAttachmentHandler1.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.Is<ICollection<AttachmentSet>>(c => c.Count == 2 && c.Contains(inputAttachments[0]) && c.Contains(inputAttachments[1])), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), _cancellationTokenSource.Token));
        _mockAttachmentHandler2.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.Is<ICollection<AttachmentSet>>(c => c.Count == 2 && c.Contains(inputAttachments[2]) && c.Contains(inputAttachments[3])), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), _cancellationTokenSource.Token));

        VerifyMetrics(inputCount: 5, outputCount: 3);
    }

    [TestMethod]
    public async Task ProcessTestRunAttachmentsAsync_ShouldReturnProcessedAttachments_IfRelatedAttachmentsOnInput()
    {
        // arrange
        List<AttachmentSet> inputAttachments = new()
        {
            new AttachmentSet(new Uri(Uri1), "uri1_input1"),
            new AttachmentSet(new Uri(Uri1), "uri1_input2"),
            new AttachmentSet(new Uri(Uri2), "uri2_input1"),
            new AttachmentSet(new Uri(Uri2), "uri2_input2"),
            new AttachmentSet(new Uri(Uri3), "uri3_input1"),
        };

        List<AttachmentSet> outputAttachmentsForHandler1 = new()
        {
            new AttachmentSet(new Uri(Uri1), "uri1_output")
        };

        List<AttachmentSet> outputAttachmentsForHandler2 = new()
        {
            new AttachmentSet(new Uri(Uri2), "uri2_output")
        };

        _mockAttachmentHandler1.Setup(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>())).ReturnsAsync(outputAttachmentsForHandler1);
        _mockAttachmentHandler2.Setup(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>())).ReturnsAsync(outputAttachmentsForHandler2);

        // act
        var result = await _manager.ProcessTestRunAttachmentsAsync(Constants.EmptyRunSettings, _mockRequestData.Object, inputAttachments, Array.Empty<InvokedDataCollector>(), _cancellationTokenSource.Token);

        // assert
        Assert.AreEqual(3, result.Count);
        Assert.IsTrue(result.Contains(inputAttachments[4]));
        Assert.IsTrue(result.Contains(outputAttachmentsForHandler1[0]));
        Assert.IsTrue(result.Contains(outputAttachmentsForHandler2[0]));
        _mockAttachmentHandler1.Verify(h => h.GetExtensionUris());
        _mockAttachmentHandler2.Verify(h => h.GetExtensionUris());
        _mockAttachmentHandler1.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.Is<ICollection<AttachmentSet>>(c => c.Count == 2 && c.Contains(inputAttachments[0]) && c.Contains(inputAttachments[1])), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), _cancellationTokenSource.Token));
        _mockAttachmentHandler2.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.Is<ICollection<AttachmentSet>>(c => c.Count == 2 && c.Contains(inputAttachments[2]) && c.Contains(inputAttachments[3])), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), _cancellationTokenSource.Token));

        VerifyMetrics(inputCount: 5, outputCount: 3);
    }

    [TestMethod]
    public async Task ProcessTestRunAttachmentsAsync_ShouldReturnInitialAttachmentsThroughEventsHandler_IfOperationCancelled()
    {
        // arrange
        List<AttachmentSet> inputAttachments = new()
        {
            new AttachmentSet(new Uri(Uri1), "uri1_input")
        };

        ICollection<AttachmentSet> outputAttachments = new List<AttachmentSet>
        {
            new AttachmentSet(new Uri(Uri1), "uri1_output")
        };

        var innerTaskCompletionSource = new TaskCompletionSource<object?>();

        CountdownEvent expectedProgress = new(4);
        _mockEventsHandler.Setup(h => h.HandleTestRunAttachmentsProcessingProgress(It.IsAny<TestRunAttachmentsProcessingProgressEventArgs>())).Callback((TestRunAttachmentsProcessingProgressEventArgs _) => expectedProgress.Signal());

        _mockAttachmentHandler1.Setup(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()))
            .Returns((string configElement, ICollection<AttachmentSet> i1, IProgress<int> progress, IMessageLogger logger, CancellationToken cancellation) =>
            {
                try
                {
                    for (int i = 0; i < 100; ++i)
                    {
                        Console.WriteLine($"Iteration: {i}");
                        logger.SendMessage(TestMessageLevel.Informational, $"Iteration: {i}");

                        cancellation.ThrowIfCancellationRequested();
                        progress.Report(i + 1);

                        if (i == 3)
                        {
                            _cancellationTokenSource.Cancel();
                        }
                    }
                }
                finally
                {
                    innerTaskCompletionSource.TrySetResult(null);
                }

                return Task.FromResult(outputAttachments);
            });

        ManualResetEventSlim attachmentProcessingComplete = new(false);
        _mockEventsHandler.Setup(h => h.HandleTestRunAttachmentsProcessingComplete(It.IsAny<TestRunAttachmentsProcessingCompleteEventArgs>(), It.IsAny<IEnumerable<AttachmentSet>>()))
            .Callback((TestRunAttachmentsProcessingCompleteEventArgs _, IEnumerable<AttachmentSet> _) => attachmentProcessingComplete.Set());

        ManualResetEventSlim handleLogMessage = new(false);
        _mockEventsHandler.Setup(h => h.HandleLogMessage(TestMessageLevel.Informational, "Attachments processing was cancelled."))
            .Callback((TestMessageLevel _, string _) => handleLogMessage.Set());

        // act
        await _manager.ProcessTestRunAttachmentsAsync(Constants.EmptyRunSettings, _mockRequestData.Object, inputAttachments, Array.Empty<InvokedDataCollector>(), _mockEventsHandler.Object, _cancellationTokenSource.Token);
        Console.WriteLine("Attachments processing done");
        await innerTaskCompletionSource.Task;

        // Wait to drain all progress events
        Assert.IsTrue(expectedProgress.Wait(TimeSpan.FromMinutes(1)), "expectedProgress not signaled");

        // Wait for the HandleTestRunAttachmentsProcessingComplete
        Assert.IsTrue(attachmentProcessingComplete.Wait(TimeSpan.FromMinutes(1)), "attachmentProcessingComplete not signaled");

        // Wait for the HandleLogMessage
        Assert.IsTrue(handleLogMessage.Wait(TimeSpan.FromMinutes(1)), "handleLogMessage not signaled");

        // assert
        VerifyCompleteEvent(true, false, inputAttachments[0]);
        _mockEventsHandler.Verify(h => h.HandleTestRunAttachmentsProcessingProgress(It.IsAny<TestRunAttachmentsProcessingProgressEventArgs>()), Times.Exactly(4));
        _mockEventsHandler.Verify(h => h.HandleTestRunAttachmentsProcessingProgress(It.Is<TestRunAttachmentsProcessingProgressEventArgs>(a => VerifyProgressArgs(a, 1))));
        _mockEventsHandler.Verify(h => h.HandleTestRunAttachmentsProcessingProgress(It.Is<TestRunAttachmentsProcessingProgressEventArgs>(a => VerifyProgressArgs(a, 2))));
        _mockEventsHandler.Verify(h => h.HandleTestRunAttachmentsProcessingProgress(It.Is<TestRunAttachmentsProcessingProgressEventArgs>(a => VerifyProgressArgs(a, 3))));
        _mockEventsHandler.Verify(h => h.HandleTestRunAttachmentsProcessingProgress(It.Is<TestRunAttachmentsProcessingProgressEventArgs>(a => VerifyProgressArgs(a, 4))));
        _mockEventsHandler.Verify(h => h.HandleLogMessage(TestMessageLevel.Informational, "Attachments processing was cancelled."));
        _mockAttachmentHandler1.Verify(h => h.GetExtensionUris());
        _mockAttachmentHandler2.Verify(h => h.GetExtensionUris(), Times.Never);
        _mockAttachmentHandler1.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.Is<ICollection<AttachmentSet>>(c => c.Count == 1 && c.Contains(inputAttachments[0])), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), _cancellationTokenSource.Token));
        _mockAttachmentHandler2.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()), Times.Never);

        VerifyMetrics(inputCount: 1, outputCount: 1, status: "Canceled");
    }

    [TestMethod]
    public async Task ProcessTestRunAttachmentsAsync_ShouldReturnInitialAttachments_IfOperationCancelled()
    {
        // arrange
        List<AttachmentSet> inputAttachments = new()
        {
            new AttachmentSet(new Uri(Uri1), "uri1_input")
        };

        ICollection<AttachmentSet> outputAttachments = new List<AttachmentSet>
        {
            new AttachmentSet(new Uri(Uri1), "uri1_output")
        };

        var innerTaskCompletionSource = new TaskCompletionSource<object?>();

        _mockAttachmentHandler1.Setup(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()))
            .Returns((XmlElement configurationElement, ICollection<AttachmentSet> i1, IProgress<int> p, IMessageLogger logger, CancellationToken cancellation) =>
            {
                try
                {
                    for (int i = 0; i < 1000; ++i)
                    {
                        Task.Delay(200, cancellation).Wait(cancellation);
                        Console.WriteLine($"Iteration: {i}");
                        logger.SendMessage(TestMessageLevel.Informational, $"Iteration: {i}");

                        cancellation.ThrowIfCancellationRequested();

                        if (i == 3)
                        {
                            _cancellationTokenSource.Cancel();
                            Task.Delay(1000, cancellation).Wait(cancellation);
                        }
                    }
                }
                finally
                {
                    innerTaskCompletionSource.TrySetResult(null);
                }

                return Task.FromResult(outputAttachments);
            });

        // act
        var result = await _manager.ProcessTestRunAttachmentsAsync(Constants.EmptyRunSettings, _mockRequestData.Object, inputAttachments, Array.Empty<InvokedDataCollector>(), _cancellationTokenSource.Token);
        Console.WriteLine("Attachments processing done");
        await innerTaskCompletionSource.Task;

        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count);
        Assert.IsTrue(result.Contains(inputAttachments[0]));
        _mockAttachmentHandler1.Verify(h => h.GetExtensionUris());
        _mockAttachmentHandler2.Verify(h => h.GetExtensionUris(), Times.Never);
        _mockAttachmentHandler1.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.Is<ICollection<AttachmentSet>>(c => c.Count == 1 && c.Contains(inputAttachments[0])), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), _cancellationTokenSource.Token));
        _mockAttachmentHandler2.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()), Times.Never);

        VerifyMetrics(inputCount: 1, outputCount: 1, status: "Canceled");
    }

    [TestMethod]
    public async Task ProcessTestRunAttachmentsAsync_ShouldReturnProperlySendProgressEvents_IfHandlersPropagesEvents()
    {
        // arrange
        List<AttachmentSet> inputAttachments = new()
        {
            new AttachmentSet(new Uri(Uri1), "uri1_input"),
            new AttachmentSet(new Uri(Uri2), "uri2_input")
        };

        ICollection<AttachmentSet> outputAttachments1 = new List<AttachmentSet>
        {
            new AttachmentSet(new Uri(Uri1), "uri1_output")
        };

        ICollection<AttachmentSet> outputAttachments2 = new List<AttachmentSet>
        {
            new AttachmentSet(new Uri(Uri2), "uri2_output")
        };

        var innerTaskCompletionSource = new TaskCompletionSource<object?>();

        int counter = 0;
        _mockEventsHandler.Setup(h => h.HandleTestRunAttachmentsProcessingProgress(It.IsAny<TestRunAttachmentsProcessingProgressEventArgs>())).Callback(() =>
        {
            counter++;
            if (counter == 6)
            {
                innerTaskCompletionSource.TrySetResult(null);
            }
        });

        _mockAttachmentHandler1.Setup(h => h.ProcessAttachmentSetsAsync(null!, It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()))
            .Returns((XmlElement configurationElement, ICollection<AttachmentSet> i1, IProgress<int> progress, IMessageLogger logger, CancellationToken cancellation) =>
            {
                progress.Report(25);
                progress.Report(50);
                progress.Report(75);
                logger.SendMessage(TestMessageLevel.Error, "error");
                progress.Report(100);
                return Task.FromResult(outputAttachments1);
            });

        _mockAttachmentHandler2.Setup(h => h.ProcessAttachmentSetsAsync(null!, It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()))
            .Returns((XmlElement configurationElement, ICollection<AttachmentSet> i1, IProgress<int> progress, IMessageLogger logger, CancellationToken cancellation) =>
            {
                progress.Report(50);
                logger.SendMessage(TestMessageLevel.Informational, "info");
                progress.Report(100);
                return Task.FromResult(outputAttachments2);
            });

        // act
        await _manager.ProcessTestRunAttachmentsAsync(Constants.EmptyRunSettings, _mockRequestData.Object, inputAttachments, Array.Empty<InvokedDataCollector>(), _mockEventsHandler.Object, CancellationToken.None);

        // assert
        await innerTaskCompletionSource.Task;
        VerifyCompleteEvent(false, false, outputAttachments1.First(), outputAttachments2.First());
        _mockEventsHandler.Verify(h => h.HandleTestRunAttachmentsProcessingProgress(It.IsAny<TestRunAttachmentsProcessingProgressEventArgs>()), Times.Exactly(6));
        _mockEventsHandler.Verify(h => h.HandleTestRunAttachmentsProcessingProgress(It.Is<TestRunAttachmentsProcessingProgressEventArgs>(a => VerifyProgressArgsForTwoHandlers(a, 1, 25, Uri1))));
        _mockEventsHandler.Verify(h => h.HandleTestRunAttachmentsProcessingProgress(It.Is<TestRunAttachmentsProcessingProgressEventArgs>(a => VerifyProgressArgsForTwoHandlers(a, 1, 50, Uri1))));
        _mockEventsHandler.Verify(h => h.HandleTestRunAttachmentsProcessingProgress(It.Is<TestRunAttachmentsProcessingProgressEventArgs>(a => VerifyProgressArgsForTwoHandlers(a, 1, 75, Uri1))));
        _mockEventsHandler.Verify(h => h.HandleTestRunAttachmentsProcessingProgress(It.Is<TestRunAttachmentsProcessingProgressEventArgs>(a => VerifyProgressArgsForTwoHandlers(a, 1, 100, Uri1))));
        _mockEventsHandler.Verify(h => h.HandleTestRunAttachmentsProcessingProgress(It.Is<TestRunAttachmentsProcessingProgressEventArgs>(a => VerifyProgressArgsForTwoHandlers(a, 2, 50, Uri2))));
        _mockEventsHandler.Verify(h => h.HandleTestRunAttachmentsProcessingProgress(It.Is<TestRunAttachmentsProcessingProgressEventArgs>(a => VerifyProgressArgsForTwoHandlers(a, 2, 100, Uri2))));
        _mockAttachmentHandler1.Verify(h => h.GetExtensionUris());
        _mockAttachmentHandler2.Verify(h => h.GetExtensionUris());
        _mockAttachmentHandler1.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.Is<ICollection<AttachmentSet>>(c => c.Count == 1 && c.Contains(inputAttachments[0])), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), CancellationToken.None));
        _mockAttachmentHandler2.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.Is<ICollection<AttachmentSet>>(c => c.Count == 1 && c.Contains(inputAttachments[1])), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), CancellationToken.None));

        _mockEventsHandler.Verify(h => h.HandleLogMessage(TestMessageLevel.Informational, "info"));
        _mockEventsHandler.Verify(h => h.HandleLogMessage(TestMessageLevel.Error, "error"));

        VerifyMetrics(inputCount: 2, outputCount: 2);
    }

    [TestMethod]
    public async Task ProcessTestRunAttachmentsAsync_ShouldNotFailIfRunsettingsIsNull()
    {
        // arrange
        List<AttachmentSet> inputAttachments = new()
        {
            new AttachmentSet(new Uri(Uri1), "uri1_input"),
            new AttachmentSet(new Uri(Uri2), "uri2_input")
        };
        ICollection<AttachmentSet> outputAttachments = new List<AttachmentSet>
        {
            new AttachmentSet(new Uri(Uri2), "uri2_output")
        };
        _mockAttachmentHandler2.Setup(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()))
            .Returns((XmlElement configurationElement, ICollection<AttachmentSet> i1, IProgress<int> progress, IMessageLogger logger, CancellationToken cancellation) =>
            {
                // assert
                Assert.IsNull(configurationElement);
                return Task.FromResult(outputAttachments);
            });

        // act
        await _manager.ProcessTestRunAttachmentsAsync(null, _mockRequestData.Object, inputAttachments, Array.Empty<InvokedDataCollector>(), _mockEventsHandler.Object, _cancellationTokenSource.Token);

        // assert
        _mockAttachmentHandler2.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()));
    }

    [DataTestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public async Task ProcessTestRunAttachmentsAsync_ShouldFlowCorrectDataCollectorConfiguration(bool withConfig)
    {
        // arrange
        List<AttachmentSet> inputAttachments = new()
        {
            new AttachmentSet(new Uri(Uri1), "uri1_input"),
            new AttachmentSet(new Uri(Uri2), "uri2_input")
        };

        List<InvokedDataCollector> invokedDataCollectors = new()
        {
            new InvokedDataCollector(new Uri(Uri1), withConfig ? "friendlyNameA" : "friendlyNameB", typeof(string).AssemblyQualifiedName!, typeof(string).Assembly.Location, false)
        };

        string runSettingsXml =
            $@"
<RunSettings>
  <DataCollectionRunSettings>
    <DataCollectors>
      <DataCollector friendlyName=""{(withConfig ? "friendlyNameA" : "friendlyNameB")}"">
        <Configuration>
          <ConfigSample>Value</ConfigSample>
        </Configuration>
      </DataCollector>
    </DataCollectors>
  </DataCollectionRunSettings>
</RunSettings>
";

        _mockAttachmentHandler1.Setup(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()))
            .Returns((XmlElement configurationElement, ICollection<AttachmentSet> i1, IProgress<int> progress, IMessageLogger logger, CancellationToken cancellation) =>
            {
                // assert
                if (withConfig)
                {
                    Assert.IsNotNull(configurationElement);
                    Assert.AreEqual("<ConfigSample>Value</ConfigSample>", configurationElement.InnerXml);
                }
                else
                {
                    Assert.IsNull(configurationElement);
                }

                ICollection<AttachmentSet> outputAttachments = new List<AttachmentSet>
                {
                    new AttachmentSet(new Uri(Uri2), "uri2_output")
                };
                return Task.FromResult(outputAttachments);
            });

        // act
        await _manager.ProcessTestRunAttachmentsAsync(runSettingsXml, _mockRequestData.Object, inputAttachments, invokedDataCollectors, _mockEventsHandler.Object, _cancellationTokenSource.Token);

        // assert
        _mockAttachmentHandler1.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()));
    }

    [TestMethod]
    public async Task ProcessTestRunAttachmentsAsync_ShouldNotConsumeAttachmentsIfProcessorFails()
    {
        // arrange
        List<AttachmentSet> inputAttachments = new()
        {
            new AttachmentSet(new Uri(Uri1), "uri1_input_1")
        };
        inputAttachments[0].Attachments.Add(UriDataAttachment.CreateFrom("file1", "Sample1"));
        inputAttachments[0].Attachments.Add(UriDataAttachment.CreateFrom("file2", "Sample2"));
        inputAttachments[0].Attachments.Add(UriDataAttachment.CreateFrom("file3", "Sample3"));


        _mockAttachmentHandler1.Setup(h => h.GetExtensionUris()).Returns(new[] { new Uri(Uri1) });
        _mockAttachmentHandler2.Setup(h => h.GetExtensionUris()).Returns(new[] { new Uri(Uri1) });

        bool firstProcessorFailed = false;

        _mockAttachmentHandler1.Setup(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()))
            .Returns((XmlElement configurationElement, ICollection<AttachmentSet> i1, IProgress<int> progress, IMessageLogger logger, CancellationToken cancellation) =>
            {
                try
                {
                    throw new Exception("Processor exception");
                }
                catch
                {
                    firstProcessorFailed = true;
                    throw;
                }
            });

        ICollection<AttachmentSet> output = new List<AttachmentSet>
        {
            new AttachmentSet(new Uri(Uri1), "uri1_input_1")
        };
        output.Single().Attachments.Add(UriDataAttachment.CreateFrom("file4", "Merged"));

        _mockAttachmentHandler2.Setup(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()))
            .Returns((XmlElement configurationElement, ICollection<AttachmentSet> i1, IProgress<int> progress, IMessageLogger logger, CancellationToken cancellation) =>
            {
                // assert
                Assert.IsTrue(firstProcessorFailed);
                Assert.AreEqual(1, i1.Count);
                Assert.AreEqual(3, i1.Single().Attachments.Count);
                for (int i = 0; i < i1.Single().Attachments.Count; i++)
                {
                    Assert.AreEqual(inputAttachments.Single().Attachments[i], i1.Single().Attachments[i]);
                }

                return Task.FromResult(output);
            });

        // act
        await _manager.ProcessTestRunAttachmentsAsync(null, _mockRequestData.Object, inputAttachments, new List<InvokedDataCollector>(), _mockEventsHandler.Object, _cancellationTokenSource.Token);

        // assert
        _mockAttachmentHandler1.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()), Times.Once());
        _mockAttachmentHandler2.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()), Times.Once());
        VerifyCompleteEvent(false, false, output.ToArray());
    }

    [TestMethod]
    public async Task ProcessTestRunAttachmentsAsync_ShouldNotConsumeAttachmentsIfAllProcessorsFail()
    {
        // arrange
        List<AttachmentSet> inputAttachments = new()
        {
            new AttachmentSet(new Uri(Uri1), "uri1_input_1"),
        };
        inputAttachments[0].Attachments.Add(UriDataAttachment.CreateFrom("file1", "Sample1"));
        inputAttachments[0].Attachments.Add(UriDataAttachment.CreateFrom("file2", "Sample2"));
        inputAttachments[0].Attachments.Add(UriDataAttachment.CreateFrom("file3", "Sample3"));


        _mockAttachmentHandler1.Setup(h => h.GetExtensionUris()).Returns(new[] { new Uri(Uri1) });
        _mockAttachmentHandler2.Setup(h => h.GetExtensionUris()).Returns(new[] { new Uri(Uri1) });

        bool firstProcessorFailed = false;
        _mockAttachmentHandler1.Setup(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()))
            .Returns((XmlElement configurationElement, ICollection<AttachmentSet> i1, IProgress<int> progress, IMessageLogger logger, CancellationToken cancellation) =>
            {
                try
                {
                    throw new Exception("Processor exception");
                }
                catch
                {
                    firstProcessorFailed = true;
                    throw;
                }
            });

        bool secondProcessorFailed = false;
        _mockAttachmentHandler2.Setup(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()))
            .Returns((XmlElement configurationElement, ICollection<AttachmentSet> i1, IProgress<int> progress, IMessageLogger logger, CancellationToken cancellation) =>
            {
                try
                {
                    // assert
                    Assert.IsTrue(firstProcessorFailed);
                    Assert.AreEqual(1, i1.Count);
                    Assert.AreEqual(3, i1.Single().Attachments.Count);
                    for (int i = 0; i < i1.Single().Attachments.Count; i++)
                    {
                        Assert.AreEqual(inputAttachments.Single().Attachments[i], i1.Single().Attachments[i]);
                    }
                    throw new Exception("Processor exception");
                }
                catch
                {
                    secondProcessorFailed = true;
                    throw;
                }
            });

        // act
        await _manager.ProcessTestRunAttachmentsAsync(null, _mockRequestData.Object, inputAttachments, new List<InvokedDataCollector>(), _mockEventsHandler.Object, _cancellationTokenSource.Token);

        // assert
        Assert.IsTrue(firstProcessorFailed && secondProcessorFailed);
        _mockAttachmentHandler1.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()), Times.Once());
        _mockAttachmentHandler2.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()), Times.Once());
        VerifyCompleteEvent(false, false, inputAttachments.ToArray());
    }

    [TestMethod]
    public async Task ProcessTestRunAttachmentsAsync_ShouldSkipAttachmentProcessorIfDoesNotSupportIncrementalProcessing()
    {
        // arrange
        var inputAttachments = new List<AttachmentSet>
        {
            new AttachmentSet(new Uri(Uri1), "uri1_input"),
            new AttachmentSet(new Uri(Uri2), "uri2_input")
        };
        _mockAttachmentHandler1.Setup(x => x.SupportsIncrementalProcessing).Returns(false);

        // act
        await _manager.ProcessTestRunAttachmentsAsync(Constants.EmptyRunSettings, _mockRequestData.Object, inputAttachments, Array.Empty<InvokedDataCollector>(), _mockEventsHandler.Object, _cancellationTokenSource.Token);

        // assert
        // We expect that first attachment is still returned as-is because not processed.
        VerifyCompleteEvent(false, false, inputAttachments.First());
        _mockAttachmentHandler1.Verify(h => h.GetExtensionUris(), Times.Never);
        _mockAttachmentHandler1.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()), Times.Never);

        _mockAttachmentHandler2.Verify(h => h.GetExtensionUris(), Times.Once);
        _mockAttachmentHandler2.Verify(h => h.ProcessAttachmentSetsAsync(It.IsAny<XmlElement>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<IProgress<int>>(), It.IsAny<IMessageLogger>(), It.IsAny<CancellationToken>()), Times.Once);

        _mockEventsHandler.Verify(h => h.HandleLogMessage(TestMessageLevel.Error, It.IsAny<string>()), Times.Once);
    }

    private void VerifyMetrics(int inputCount, int outputCount, string status = "Completed")
    {
        _mockEventSource.Verify(s => s.TestRunAttachmentsProcessingStart(inputCount));
        _mockEventSource.Verify(s => s.TestRunAttachmentsProcessingStop(outputCount));

        _mockMetricsCollection.Verify(m => m.Add(TelemetryDataConstants.NumberOfAttachmentsSentForProcessing, inputCount));
        _mockMetricsCollection.Verify(m => m.Add(TelemetryDataConstants.NumberOfAttachmentsAfterProcessing, outputCount));
        _mockMetricsCollection.Verify(m => m.Add(TelemetryDataConstants.AttachmentsProcessingState, status));
        _mockMetricsCollection.Verify(m => m.Add(TelemetryDataConstants.TimeTakenInSecForAttachmentsProcessing, It.IsAny<double>()));
    }

    private void VerifyCompleteEvent(bool isCanceled, bool containsError, params AttachmentSet[] expectedSets)
    {
        _mockEventsHandler.Verify(h => h.HandleTestRunAttachmentsProcessingComplete(
            It.Is<TestRunAttachmentsProcessingCompleteEventArgs>(a => a.IsCanceled == isCanceled && a.Error != null == containsError),
            It.Is<ICollection<AttachmentSet>>(c => c.Count == expectedSets.Length && expectedSets.All(e => c.Contains(e)))));
    }

    private static bool VerifyProgressArgs(TestRunAttachmentsProcessingProgressEventArgs args, int progress)
    {
        Assert.AreEqual(1, args.CurrentAttachmentProcessorIndex);
        Assert.AreEqual(2, args.AttachmentProcessorsCount);
        Assert.AreEqual(1, args.CurrentAttachmentProcessorUris.Count);
        Assert.AreEqual(Uri1, args.CurrentAttachmentProcessorUris.First().AbsoluteUri);
        return progress == args.CurrentAttachmentProcessorProgress;
    }

    private static bool VerifyProgressArgsForTwoHandlers(TestRunAttachmentsProcessingProgressEventArgs args, long handlerIndex, long progress, string uri)
    {
        return progress == args.CurrentAttachmentProcessorProgress &&
               args.CurrentAttachmentProcessorIndex == handlerIndex &&
               args.CurrentAttachmentProcessorUris.First().AbsoluteUri == uri &&
               args.AttachmentProcessorsCount == 2;
    }
}
