// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.TestRunAttachmentsProcessing;

/// <summary>
/// Orchestrates test run localAttachments processing operations.
/// </summary>
internal class TestRunAttachmentsProcessingManager : ITestRunAttachmentsProcessingManager
{
    private static readonly string AttachmentsProcessingCompleted = "Completed";
    private static readonly string AttachmentsProcessingCanceled = "Canceled";
    private static readonly string AttachmentsProcessingFailed = "Failed";

    private readonly ITestPlatformEventSource _testPlatformEventSource;
    private readonly IDataCollectorAttachmentsProcessorsFactory _dataCollectorAttachmentsProcessorsFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestRunAttachmentsProcessingManager"/> class.
    /// </summary>
    public TestRunAttachmentsProcessingManager(ITestPlatformEventSource testPlatformEventSource, IDataCollectorAttachmentsProcessorsFactory dataCollectorAttachmentsProcessorsFactory)
    {
        _testPlatformEventSource = testPlatformEventSource ?? throw new ArgumentNullException(nameof(testPlatformEventSource));
        _dataCollectorAttachmentsProcessorsFactory = dataCollectorAttachmentsProcessorsFactory ?? throw new ArgumentNullException(nameof(dataCollectorAttachmentsProcessorsFactory));
    }

    /// <inheritdoc/>
    public async Task ProcessTestRunAttachmentsAsync(string? runSettingsXml, IRequestData requestData, IEnumerable<AttachmentSet> attachments, IEnumerable<InvokedDataCollector>? invokedDataCollector, ITestRunAttachmentsProcessingEventsHandler eventHandler, CancellationToken cancellationToken)
    {
        await InternalProcessTestRunAttachmentsAsync(runSettingsXml, requestData, attachments, invokedDataCollector, eventHandler, cancellationToken).ConfigureAwait(false);
    }
    /// <inheritdoc/>
    public Task<Collection<AttachmentSet>> ProcessTestRunAttachmentsAsync(string? runSettingsXml, IRequestData requestData, IEnumerable<AttachmentSet> attachments, IEnumerable<InvokedDataCollector>? invokedDataCollector, CancellationToken cancellationToken)
    {
        return InternalProcessTestRunAttachmentsAsync(runSettingsXml, requestData, attachments, invokedDataCollector, null, cancellationToken);
    }

    private async Task<Collection<AttachmentSet>> InternalProcessTestRunAttachmentsAsync(string? runSettingsXml, IRequestData requestData, IEnumerable<AttachmentSet> attachments, IEnumerable<InvokedDataCollector>? invokedDataCollector, ITestRunAttachmentsProcessingEventsHandler? eventHandler, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        Collection<AttachmentSet> localAttachments = new(attachments.ToList());

        try
        {
            _testPlatformEventSource.TestRunAttachmentsProcessingStart(localAttachments.Count);
            requestData.MetricsCollection.Add(TelemetryDataConstants.NumberOfAttachmentsSentForProcessing, localAttachments.Count);

            cancellationToken.ThrowIfCancellationRequested();

            var cancelAttachmentProcessingCompletionSource = new TaskCompletionSource<Collection<AttachmentSet>>();
            using (cancellationToken.Register(() => cancelAttachmentProcessingCompletionSource.TrySetCanceled()))
            {
                Task<Collection<AttachmentSet>> task = Task.Run(async () => await ProcessAttachmentsAsync(runSettingsXml, localAttachments, invokedDataCollector, eventHandler, cancellationToken));

                var completedTask = await Task.WhenAny(task, cancelAttachmentProcessingCompletionSource.Task).ConfigureAwait(false);

                if (completedTask == task)
                {
                    return FinalizeOperation(requestData, new TestRunAttachmentsProcessingCompleteEventArgs(false, null), await task, stopwatch, eventHandler);
                }
                else
                {
                    eventHandler?.HandleLogMessage(TestMessageLevel.Informational, "Attachments processing was cancelled.");
                    return FinalizeOperation(requestData, new TestRunAttachmentsProcessingCompleteEventArgs(true, null), localAttachments, stopwatch, eventHandler);
                }
            }
        }
        catch (OperationCanceledException ex)
        {
            // If it's OperationCanceledException of our cancellationToken we log like in case of cancelAttachmentProcessingCompletionSource
            // there's a possible exception race task vs cancelAttachmentProcessingCompletionSource.Task
            if (ex.CancellationToken == cancellationToken)
            {
                eventHandler?.HandleLogMessage(TestMessageLevel.Informational, "Attachments processing was cancelled.");
            }

            EqtTrace.Warning("TestRunAttachmentsProcessingManager: Operation was cancelled.");
            return FinalizeOperation(requestData, new TestRunAttachmentsProcessingCompleteEventArgs(true, null), localAttachments, stopwatch, eventHandler);
        }
        catch (Exception e)
        {
            EqtTrace.Error("TestRunAttachmentsProcessingManager: Exception in ProcessTestRunAttachmentsAsync: " + e);

            eventHandler?.HandleLogMessage(TestMessageLevel.Error, e.ToString());
            return FinalizeOperation(requestData, new TestRunAttachmentsProcessingCompleteEventArgs(false, e), localAttachments, stopwatch, eventHandler);
        }
    }

    private async Task<Collection<AttachmentSet>> ProcessAttachmentsAsync(string? runSettingsXml, Collection<AttachmentSet> attachments, IEnumerable<InvokedDataCollector>? invokedDataCollector, ITestRunAttachmentsProcessingEventsHandler? eventsHandler, CancellationToken cancellationToken)
    {
        if (attachments.Count == 0)
        {
            return attachments;
        }

        // Create a local copy of the collection to avoid modifying original one.
        attachments = new(attachments.ToList());
        var dataCollectionRunSettings = XmlRunSettingsUtilities.GetDataCollectionRunSettings(runSettingsXml);

        var logger = CreateMessageLogger(eventsHandler);
        var dataCollectorAttachmentsProcessors = _dataCollectorAttachmentsProcessorsFactory.Create(invokedDataCollector?.ToArray(), logger);
        for (int i = 0; i < dataCollectorAttachmentsProcessors.Length; i++)
        {
            // We need to dispose the DataCollectorAttachmentProcessor to unload the AppDomain for net462
            using DataCollectorAttachmentProcessor dataCollectorAttachmentsProcessor = dataCollectorAttachmentsProcessors[i];

            int attachmentsHandlerIndex = i + 1;

            if (!dataCollectorAttachmentsProcessor.DataCollectorAttachmentProcessorInstance.SupportsIncrementalProcessing)
            {
                EqtTrace.Error($"TestRunAttachmentsProcessingManager: Non incremental attachment processors are not supported, '{dataCollectorAttachmentsProcessor.DataCollectorAttachmentProcessorInstance.GetType()}'");
                logger.SendMessage(TestMessageLevel.Error, $"Non incremental attachment processors are not supported '{dataCollectorAttachmentsProcessor.DataCollectorAttachmentProcessorInstance.GetType()}'");
                continue;
            }

            // We run processor code inside a try/catch because we want to continue with the others in case of failure.
            Collection<AttachmentSet> attachmentsBackup = null!;
            try
            {
                // We temporarily save the localAttachments to process because, in case of processor exception,
                // we'll restore the attachmentSets to make those available to other processors.
                // NB. localAttachments.ToList() is done on purpose we need a new ref list.
                attachmentsBackup = new Collection<AttachmentSet>(attachments.ToList());

                ICollection<Uri>? attachmentProcessorUris = dataCollectorAttachmentsProcessor.DataCollectorAttachmentProcessorInstance.GetExtensionUris()?.ToList();
                if (attachmentProcessorUris == null || attachmentProcessorUris.Count == 0)
                {
                    continue;
                }

                var attachmentsToBeProcessed = attachments.Where(dataCollectionAttachment => attachmentProcessorUris.Any(uri => uri.Equals(dataCollectionAttachment.Uri))).ToArray();
                if (attachmentsToBeProcessed.Length == 0)
                {
                    continue;
                }

                foreach (var attachment in attachmentsToBeProcessed)
                {
                    attachments.Remove(attachment);
                }

                IProgress<int> progressReporter = new Progress<int>((int progress) =>
                    eventsHandler?.HandleTestRunAttachmentsProcessingProgress(
                        new TestRunAttachmentsProcessingProgressEventArgs(attachmentsHandlerIndex, attachmentProcessorUris, progress, dataCollectorAttachmentsProcessors.Length)));

                XmlElement? configuration = null;
                var collectorConfiguration = dataCollectionRunSettings?.DataCollectorSettingsList.SingleOrDefault(c => c.FriendlyName == dataCollectorAttachmentsProcessor.FriendlyName);
                if (collectorConfiguration != null && collectorConfiguration.IsEnabled)
                {
                    configuration = collectorConfiguration.Configuration;
                }

                EqtTrace.Info($"TestRunAttachmentsProcessingManager: Invocation of data collector attachment processor AssemblyQualifiedName: '{dataCollectorAttachmentsProcessor.DataCollectorAttachmentProcessorInstance.GetType().AssemblyQualifiedName}' FriendlyName: '{dataCollectorAttachmentsProcessor.FriendlyName}' with configuration '{(configuration == null ? "null" : configuration.OuterXml)}'");
                ICollection<AttachmentSet> processedAttachments = await dataCollectorAttachmentsProcessor.DataCollectorAttachmentProcessorInstance.ProcessAttachmentSetsAsync(
                    configuration!,
                    new Collection<AttachmentSet>(attachmentsToBeProcessed),
                    progressReporter,
                    logger,
                    cancellationToken).ConfigureAwait(false);

                if (processedAttachments != null && processedAttachments.Count > 0)
                {
                    foreach (var attachment in processedAttachments)
                    {
                        attachments.Add(attachment);
                    }
                }
            }
            catch (Exception e)
            {
                EqtTrace.Error("TestRunAttachmentsProcessingManager: Exception in ProcessAttachmentsAsync: " + e);

                // If it's OperationCanceledException of our cancellationToken we let the exception bubble up.
                if (e is OperationCanceledException operationCanceled && operationCanceled.CancellationToken == cancellationToken)
                {
                    throw;
                }

                logger.SendMessage(TestMessageLevel.Error, e.ToString());

                // Restore the attachment sets for the others attachment processors.
                attachments = attachmentsBackup;
            }
        }

        return attachments;
    }

    private Collection<AttachmentSet> FinalizeOperation(IRequestData requestData, TestRunAttachmentsProcessingCompleteEventArgs completeArgs, Collection<AttachmentSet> attachments, Stopwatch stopwatch, ITestRunAttachmentsProcessingEventsHandler? eventHandler)
    {
        _testPlatformEventSource.TestRunAttachmentsProcessingStop(attachments.Count);
        requestData.MetricsCollection.Add(TelemetryDataConstants.NumberOfAttachmentsAfterProcessing, attachments.Count);
        requestData.MetricsCollection.Add(TelemetryDataConstants.AttachmentsProcessingState, completeArgs.Error != null ? AttachmentsProcessingFailed : completeArgs.IsCanceled ? AttachmentsProcessingCanceled : AttachmentsProcessingCompleted);

        stopwatch.Stop();
        requestData.MetricsCollection.Add(TelemetryDataConstants.TimeTakenInSecForAttachmentsProcessing, stopwatch.Elapsed.TotalSeconds);

        completeArgs.Metrics = requestData.MetricsCollection.Metrics;
        eventHandler?.HandleTestRunAttachmentsProcessingComplete(completeArgs, attachments);

        return attachments;
    }

    private static IMessageLogger CreateMessageLogger(ITestRunAttachmentsProcessingEventsHandler? eventsHandler)
        => eventsHandler != null
            ? new AttachmentsProcessingMessageLogger(eventsHandler)
            : new NullMessageLogger();

    private class AttachmentsProcessingMessageLogger : IMessageLogger
    {
        private readonly ITestRunAttachmentsProcessingEventsHandler _eventsHandler;

        public AttachmentsProcessingMessageLogger(ITestRunAttachmentsProcessingEventsHandler eventsHandler)
        {
            _eventsHandler = eventsHandler ?? throw new ArgumentNullException(nameof(eventsHandler));
        }

        public void SendMessage(TestMessageLevel testMessageLevel, string message)
        {
            _eventsHandler.HandleLogMessage(testMessageLevel, message);
        }
    }

    private class NullMessageLogger : IMessageLogger
    {
        public void SendMessage(TestMessageLevel testMessageLevel, string message)
        {
        }
    }
}
