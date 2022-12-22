// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETFRAMEWORK

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.TestRunAttachmentsProcessing;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.TestPlatform.CrossPlatEngine.UnitTests.DataCollectorAttachmentProcessorAppDomainTests;

[TestClass]
public class DataCollectorAttachmentProcessorAppDomainTests
{
    private readonly Mock<IMessageLogger> _loggerMock = new();
    internal static string SomeState = "deafultState";

    [TestMethod]
    public async Task DataCollectorAttachmentProcessorAppDomain_ShouldBeIsolated()
    {
        // arrange
        var invokedDataCollector = new InvokedDataCollector(new Uri("datacollector://AppDomainSample"), "AppDomainSample", typeof(AppDomainSampleDataCollector).AssemblyQualifiedName, typeof(AppDomainSampleDataCollector).Assembly.Location, true);
        var attachmentSet = new AttachmentSet(new Uri("datacollector://AppDomainSample"), string.Empty);
        attachmentSet.Attachments.Add(new UriDataAttachment(new Uri("C:\\temp\\sample"), "sample"));
        Collection<AttachmentSet> attachments = new() { attachmentSet };
        var doc = new XmlDocument();
        doc.LoadXml("<configurationElement/>");

        // act
        using DataCollectorAttachmentProcessorAppDomain dcap = new(invokedDataCollector, _loggerMock.Object);
        Assert.IsTrue(dcap.LoadSucceded);
        await dcap.ProcessAttachmentSetsAsync(doc.DocumentElement, attachments, new Progress<int>((int report) => { }), _loggerMock.Object, CancellationToken.None);

        //Assert
        // If the processor runs in another AppDomain the static state is not shared and should not change.
        Assert.AreEqual("deafultState", SomeState);
    }

    [TestMethod]
    public async Task DataCollectorAttachmentProcessorAppDomain_ShouldCancel()
    {
        // arrange
        var invokedDataCollector = new InvokedDataCollector(new Uri("datacollector://AppDomainSample"), "AppDomainSample", typeof(AppDomainSampleDataCollector).AssemblyQualifiedName, typeof(AppDomainSampleDataCollector).Assembly.Location, true);
        var attachmentSet = new AttachmentSet(new Uri("datacollector://AppDomainSample"), string.Empty);
        attachmentSet.Attachments.Add(new UriDataAttachment(new Uri("C:\\temp\\sample"), "sample"));
        Collection<AttachmentSet> attachments = new() { attachmentSet };
        var doc = new XmlDocument();
        doc.LoadXml("<configurationElement>5000</configurationElement>");
        CancellationTokenSource cts = new();

        // act
        using DataCollectorAttachmentProcessorAppDomain dcap = new(invokedDataCollector, _loggerMock.Object);
        Assert.IsTrue(dcap.LoadSucceded);

        Task runProcessing = dcap.ProcessAttachmentSetsAsync(doc.DocumentElement, attachments, new Progress<int>((int report) => cts.Cancel()), _loggerMock.Object, cts.Token);

        //assert
        await Assert.ThrowsExceptionAsync<OperationCanceledException>(async () => await runProcessing);
    }

    [TestMethod]
    public async Task DataCollectorAttachmentProcessorAppDomain_ShouldReturnCorrectAttachments()
    {
        // arrange
        var invokedDataCollector = new InvokedDataCollector(new Uri("datacollector://AppDomainSample"), "AppDomainSample", typeof(AppDomainSampleDataCollector).AssemblyQualifiedName, typeof(AppDomainSampleDataCollector).Assembly.Location, true);
        var attachmentSet = new AttachmentSet(new Uri("datacollector://AppDomainSample"), "AppDomainSample");
        attachmentSet.Attachments.Add(new UriDataAttachment(new Uri("C:\\temp\\sample"), "sample"));
        Collection<AttachmentSet> attachments = new() { attachmentSet };
        var doc = new XmlDocument();
        doc.LoadXml("<configurationElement/>");

        // act
        using DataCollectorAttachmentProcessorAppDomain dcap = new(invokedDataCollector, _loggerMock.Object);
        Assert.IsTrue(dcap.LoadSucceded);

        var attachmentsResult = await dcap.ProcessAttachmentSetsAsync(doc.DocumentElement, attachments, new Progress<int>(), _loggerMock.Object, CancellationToken.None);

        // assert
        // We return same instance but we're marshaling so we expected different pointers
        Assert.AreNotSame(attachmentSet, attachmentsResult);

        Assert.AreEqual(attachmentSet.DisplayName, attachmentsResult.First().DisplayName);
        Assert.AreEqual(attachmentSet.Uri, attachmentsResult.First().Uri);
        Assert.AreEqual(attachmentSet.Attachments.Count, attachmentsResult.Count);
        Assert.AreEqual(attachmentSet.Attachments[0].Description, attachmentsResult.First().Attachments[0].Description);
        Assert.AreEqual(attachmentSet.Attachments[0].Uri, attachmentsResult.First().Attachments[0].Uri);
        Assert.AreEqual(attachmentSet.Attachments[0].Uri, attachmentsResult.First().Attachments[0].Uri);
    }

    [TestMethod]
    public async Task DataCollectorAttachmentProcessorAppDomain_ShouldReportProgressCorrectly()
    {
        // arrange
        var invokedDataCollector = new InvokedDataCollector(new Uri("datacollector://AppDomainSample"), "AppDomainSample", typeof(AppDomainSampleDataCollector).AssemblyQualifiedName, typeof(AppDomainSampleDataCollector).Assembly.Location, true);
        var attachmentSet = new AttachmentSet(new Uri("datacollector://AppDomainSample"), "AppDomainSample");
        attachmentSet.Attachments.Add(new UriDataAttachment(new Uri("C:\\temp\\sample"), "sample"));
        Collection<AttachmentSet> attachments = new() { attachmentSet };
        var doc = new XmlDocument();
        doc.LoadXml("<configurationElement/>");

        // act
        var progress = new CustomProgress();
        using DataCollectorAttachmentProcessorAppDomain dcap = new(invokedDataCollector, _loggerMock.Object);
        Assert.IsTrue(dcap.LoadSucceded);

        var attachmentsResult = await dcap.ProcessAttachmentSetsAsync(
            doc.DocumentElement,
            attachments,
            progress,
            _loggerMock.Object,
            CancellationToken.None);

        // assert
        progress.CountdownEvent.Wait(new CancellationTokenSource(10000).Token);
        Assert.AreEqual(10, progress.Progress[0]);
        Assert.AreEqual(50, progress.Progress[1]);
        Assert.AreEqual(100, progress.Progress[2]);
    }

    [TestMethod]
    public async Task DataCollectorAttachmentProcessorAppDomain_ShouldLogCorrectly()
    {
        // arrange
        var invokedDataCollector = new InvokedDataCollector(new Uri("datacollector://AppDomainSample"), "AppDomainSample", typeof(AppDomainSampleDataCollector).AssemblyQualifiedName, typeof(AppDomainSampleDataCollector).Assembly.Location, true);
        var attachmentSet = new AttachmentSet(new Uri("datacollector://AppDomainSample"), "AppDomainSample");
        attachmentSet.Attachments.Add(new UriDataAttachment(new Uri("C:\\temp\\sample"), "sample"));
        Collection<AttachmentSet> attachments = new() { attachmentSet };
        var doc = new XmlDocument();
        doc.LoadXml("<configurationElement/>");
        CountdownEvent countdownEvent = new(3);
        List<Tuple<TestMessageLevel, string>> messages = new();
        _loggerMock.Setup(x => x.SendMessage(It.IsAny<TestMessageLevel>(), It.IsAny<string>())).Callback((TestMessageLevel messageLevel, string message)
            =>
        {
            countdownEvent.Signal();
            messages.Add(new Tuple<TestMessageLevel, string>(messageLevel, message));
        });

        // act
        using DataCollectorAttachmentProcessorAppDomain dcap = new(invokedDataCollector, _loggerMock.Object);
        Assert.IsTrue(dcap.LoadSucceded);

        var attachmentsResult = await dcap.ProcessAttachmentSetsAsync(doc.DocumentElement, attachments, new Progress<int>(), _loggerMock.Object, CancellationToken.None);

        // assert
        countdownEvent.Wait(new CancellationTokenSource(10000).Token);
        Assert.AreEqual(3, messages.Count);
        Assert.AreEqual(TestMessageLevel.Informational, messages[0].Item1);
        Assert.AreEqual("Info", messages[0].Item2);
        Assert.AreEqual(TestMessageLevel.Warning, messages[1].Item1);
        Assert.AreEqual("Warning", messages[1].Item2);
        Assert.AreEqual(TestMessageLevel.Error, messages[2].Item1);
        Assert.AreEqual($"line1{Environment.NewLine}line2{Environment.NewLine}line3", messages[2].Item2);
    }

    [TestMethod]
    public void DataCollectorAttachmentProcessorAppDomain_ShouldReportFailureDuringExtensionCreation()
    {
        // arrange
        var invokedDataCollector = new InvokedDataCollector(new Uri("datacollector://AppDomainSampleFailure"), "AppDomainSampleFailure", typeof(AppDomainSampleDataCollectorFailure).AssemblyQualifiedName, typeof(AppDomainSampleDataCollectorFailure).Assembly.Location, true);
        var attachmentSet = new AttachmentSet(new Uri("datacollector://AppDomainSampleFailure"), "AppDomainSampleFailure");
        attachmentSet.Attachments.Add(new UriDataAttachment(new Uri("C:\\temp\\sample"), "sample"));
        Collection<AttachmentSet> attachments = new() { attachmentSet };
        var doc = new XmlDocument();
        doc.LoadXml("<configurationElement/>");
        using ManualResetEventSlim errorReportEvent = new();
        _loggerMock.Setup(x => x.SendMessage(It.IsAny<TestMessageLevel>(), It.IsAny<string>())).Callback((TestMessageLevel messageLevel, string message)
            =>
        {
            if (messageLevel == TestMessageLevel.Error)
            {
                Assert.IsTrue(message.Contains("System.Exception: Failed to create the extension"));
                errorReportEvent.Set();
            }
        });

        // act
        using DataCollectorAttachmentProcessorAppDomain dcap = new(invokedDataCollector, _loggerMock.Object);

        //assert
        errorReportEvent.Wait(new CancellationTokenSource(10000).Token);
        Assert.IsFalse(dcap.LoadSucceded);
    }

    [DataCollectorFriendlyName("AppDomainSample")]
    [DataCollectorTypeUri("datacollector://AppDomainSample")]
    [DataCollectorAttachmentProcessor(typeof(AppDomainDataCollectorAttachmentProcessor))]
    public class AppDomainSampleDataCollector : DataCollector
    {
        public override void Initialize(
            XmlElement? configurationElement,
            DataCollectionEvents events,
            DataCollectionSink dataSink,
            DataCollectionLogger logger,
            DataCollectionEnvironmentContext? environmentContext)
        {

        }
    }

    public class AppDomainDataCollectorAttachmentProcessor : IDataCollectorAttachmentProcessor
    {
        public bool SupportsIncrementalProcessing => false;

        public IEnumerable<Uri> GetExtensionUris() => new[] { new Uri("datacollector://AppDomainSample") };

        public async Task<ICollection<AttachmentSet>> ProcessAttachmentSetsAsync(XmlElement configurationElement, ICollection<AttachmentSet> attachments, IProgress<int> progressReporter, IMessageLogger logger, CancellationToken cancellationToken)
        {
            SomeState = "Updated shared state";

            var timeout = configurationElement.InnerText;
            if (!string.IsNullOrEmpty(timeout))
            {
                progressReporter.Report(100);

                DateTime expire = DateTime.UtcNow + TimeSpan.FromMilliseconds(int.Parse(timeout, CultureInfo.CurrentCulture));
                while (true)
                {
                    if (DateTime.UtcNow > expire)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                    }

#pragma warning disable CA2016 // Forward the 'CancellationToken' parameter to methods
                    await Task.Delay(1000);
#pragma warning restore CA2016 // Forward the 'CancellationToken' parameter to methods
                }
            }

            progressReporter.Report(10);
            progressReporter.Report(50);
            progressReporter.Report(100);

            logger.SendMessage(TestMessageLevel.Informational, "Info");
            logger.SendMessage(TestMessageLevel.Warning, "Warning");
            logger.SendMessage(TestMessageLevel.Error, $"line1{Environment.NewLine}line2\nline3");

            return attachments;
        }
    }

    [DataCollectorFriendlyName("AppDomainSampleFailure")]
    [DataCollectorTypeUri("datacollector://AppDomainSampleFailure")]
    [DataCollectorAttachmentProcessor(typeof(AppDomainDataCollectorAttachmentProcessorFailure))]
    public class AppDomainSampleDataCollectorFailure : DataCollector
    {
        public override void Initialize(
            XmlElement? configurationElement,
            DataCollectionEvents events,
            DataCollectionSink dataSink,
            DataCollectionLogger logger,
            DataCollectionEnvironmentContext? environmentContext)
        {

        }
    }

    public class AppDomainDataCollectorAttachmentProcessorFailure : IDataCollectorAttachmentProcessor
    {
        public AppDomainDataCollectorAttachmentProcessorFailure()
        {
            throw new Exception("Failed to create the extension");
        }

        public bool SupportsIncrementalProcessing => false;

        public IEnumerable<Uri> GetExtensionUris() => throw new NotImplementedException();

        public Task<ICollection<AttachmentSet>> ProcessAttachmentSetsAsync(XmlElement configurationElement, ICollection<AttachmentSet> attachments, IProgress<int> progressReporter, IMessageLogger logger, CancellationToken cancellationToken)
            => throw new NotImplementedException();
    }

    public class CustomProgress : IProgress<int>
    {
        public List<int> Progress { get; set; } = new List<int>();
        public CountdownEvent CountdownEvent { get; set; } = new CountdownEvent(3);

        public void Report(int value)
        {
            Progress.Add(value);
            CountdownEvent.Signal();
        }
    }
}

#endif
