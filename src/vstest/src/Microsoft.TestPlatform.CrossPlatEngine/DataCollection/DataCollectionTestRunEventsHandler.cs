// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection;

/// <summary>
/// Handles DataCollection attachments by calling DataCollection Process on Test Run Complete.
/// Existing functionality of ITestRunEventsHandler is decorated with additional call to Data Collection Process.
/// </summary>
internal class DataCollectionTestRunEventsHandler : IInternalTestRunEventsHandler
{
    private readonly IProxyDataCollectionManager _proxyDataCollectionManager;
    private readonly IInternalTestRunEventsHandler _testRunEventsHandler;
    private readonly CancellationToken _cancellationToken;
    private readonly IDataSerializer _dataSerializer;

    private Collection<AttachmentSet>? _dataCollectionAttachmentSets;
    private Collection<InvokedDataCollector>? _invokedDataCollectors;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataCollectionTestRunEventsHandler"/> class.
    /// </summary>
    /// <param name="baseTestRunEventsHandler">
    /// The base test run events handler.
    /// </param>
    /// <param name="proxyDataCollectionManager">
    /// The proxy Data Collection Manager.
    /// </param>
    public DataCollectionTestRunEventsHandler(IInternalTestRunEventsHandler baseTestRunEventsHandler, IProxyDataCollectionManager proxyDataCollectionManager, CancellationToken cancellationToken)
        : this(baseTestRunEventsHandler, proxyDataCollectionManager, JsonDataSerializer.Instance, cancellationToken)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataCollectionTestRunEventsHandler"/> class.
    /// </summary>
    /// <param name="baseTestRunEventsHandler">
    /// The base test run events handler.
    /// </param>
    /// <param name="proxyDataCollectionManager">
    /// The proxy Data Collection Manager.
    /// </param>
    /// <param name="dataSerializer">
    /// The data Serializer.
    /// </param>
    public DataCollectionTestRunEventsHandler(IInternalTestRunEventsHandler baseTestRunEventsHandler, IProxyDataCollectionManager proxyDataCollectionManager, IDataSerializer dataSerializer, CancellationToken cancellationToken)
    {
        _proxyDataCollectionManager = proxyDataCollectionManager;
        _testRunEventsHandler = baseTestRunEventsHandler;
        _cancellationToken = cancellationToken;
        _dataSerializer = dataSerializer;
    }

    /// <summary>
    /// The handle log message.
    /// </summary>
    /// <param name="level">
    /// The level.
    /// </param>
    /// <param name="message">
    /// The message.
    /// </param>
    public void HandleLogMessage(TestMessageLevel level, string? message)
    {
        _testRunEventsHandler.HandleLogMessage(level, message);
    }

    /// <summary>
    /// The handle raw message.
    /// </summary>
    /// <param name="rawMessage">
    /// The raw message.
    /// </param>
    public void HandleRawMessage(string rawMessage)
    {
        // In case of data collection, data collection attachments should be attached to raw message for ExecutionComplete
        var message = _dataSerializer.DeserializeMessage(rawMessage);

        if (string.Equals(MessageType.ExecutionComplete, message.MessageType))
        {
            var dataCollectionResult = _proxyDataCollectionManager?.AfterTestRunEnd(_cancellationToken.IsCancellationRequested, this);
            _dataCollectionAttachmentSets = dataCollectionResult?.Attachments;

            var testRunCompletePayload =
                _dataSerializer.DeserializePayload<TestRunCompletePayload>(message);

            if (_dataCollectionAttachmentSets != null && _dataCollectionAttachmentSets.Count != 0)
            {
                GetCombinedAttachmentSets(
                    testRunCompletePayload?.TestRunCompleteArgs?.AttachmentSets,
                    _dataCollectionAttachmentSets);
            }

            _invokedDataCollectors = dataCollectionResult?.InvokedDataCollectors;
            if (_invokedDataCollectors?.Count > 0)
            {
                foreach (var dataCollector in _invokedDataCollectors)
                {
                    testRunCompletePayload?.TestRunCompleteArgs?.InvokedDataCollectors.Add(dataCollector);
                }
            }

            rawMessage = _dataSerializer.SerializePayload(
                MessageType.ExecutionComplete,
                testRunCompletePayload);
        }

        _testRunEventsHandler.HandleRawMessage(rawMessage);
    }

    /// <summary>
    /// The handle test run complete.
    /// </summary>
    /// <param name="testRunCompleteArgs">
    /// The test run complete args.
    /// </param>
    /// <param name="lastChunkArgs">
    /// The last chunk args.
    /// </param>
    /// <param name="runContextAttachments">
    /// The run context attachments.
    /// </param>
    /// <param name="executorUris">
    /// The executor uris.
    /// </param>
    public void HandleTestRunComplete(TestRunCompleteEventArgs testRunCompleteArgs, TestRunChangedEventArgs? lastChunkArgs, ICollection<AttachmentSet>? runContextAttachments, ICollection<string>? executorUris)
    {
        if (_dataCollectionAttachmentSets != null && _dataCollectionAttachmentSets.Count != 0)
        {
            runContextAttachments = GetCombinedAttachmentSets(_dataCollectionAttachmentSets, runContextAttachments);
        }

        // At the moment, we don't support running data collectors inside testhost process, so it will always be empty inside "TestRunCompleteEventArgs testRunCompleteArgs".
        // We load invoked data collectors from data collector process inside "DataCollectionTestRunEventsHandler.HandleRawMessage" method.
        if (_invokedDataCollectors != null && _invokedDataCollectors.Count != 0)
        {
            foreach (var dataCollector in _invokedDataCollectors)
            {
                testRunCompleteArgs.InvokedDataCollectors.Add(dataCollector);
            }
        }

        _testRunEventsHandler.HandleTestRunComplete(testRunCompleteArgs, lastChunkArgs, runContextAttachments, executorUris);
    }

    /// <summary>
    /// The handle test run stats change.
    /// </summary>
    /// <param name="testRunChangedArgs">
    /// The test run changed args.
    /// </param>
    public void HandleTestRunStatsChange(TestRunChangedEventArgs? testRunChangedArgs)
    {
        _testRunEventsHandler.HandleTestRunStatsChange(testRunChangedArgs);
    }

    /// <summary>
    /// Launches a process with a given process info under debugger
    /// Adapter get to call into this to launch any additional processes under debugger
    /// </summary>
    /// <param name="testProcessStartInfo">Process start info</param>
    /// <returns>ProcessId of the launched process</returns>
    public int LaunchProcessWithDebuggerAttached(TestProcessStartInfo testProcessStartInfo)
    {
        return _testRunEventsHandler.LaunchProcessWithDebuggerAttached(testProcessStartInfo);
    }

    /// <inheritdoc />
    public bool AttachDebuggerToProcess(AttachDebuggerInfo attachDebuggerInfo)
    {
        return _testRunEventsHandler.AttachDebuggerToProcess(attachDebuggerInfo);
    }

    /// <summary>
    /// The get combined attachment sets.
    /// </summary>
    /// <param name="originalAttachmentSets">
    /// The run attachments.
    /// </param>
    /// <param name="newAttachments">
    /// The run context attachments.
    /// </param>
    /// <returns>
    /// The <see cref="Collection"/>.
    /// </returns>
    [return: NotNullIfNotNull("originalAttachmentSets")]
    [return: NotNullIfNotNull("newAttachments")]
    internal static ICollection<AttachmentSet>? GetCombinedAttachmentSets(Collection<AttachmentSet>? originalAttachmentSets, ICollection<AttachmentSet>? newAttachments)
    {
        if (newAttachments == null || newAttachments.Count == 0)
        {
            return originalAttachmentSets;
        }

        if (originalAttachmentSets == null)
        {
            return new Collection<AttachmentSet>(newAttachments.ToList());
        }

        foreach (var attachmentSet in newAttachments)
        {
            var attSet = originalAttachmentSets.FirstOrDefault(item => Equals(item.Uri, attachmentSet.Uri));
            if (attSet == null)
            {
                originalAttachmentSets.Add(attachmentSet);
            }
            else
            {
                foreach (var attachment in attachmentSet.Attachments)
                {
                    attSet.Attachments.Add(attachment);
                }
            }
        }

        return originalAttachmentSets;
    }
}
