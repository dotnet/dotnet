// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

using CommonResources = Microsoft.VisualStudio.TestPlatform.Common.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.Parallel;

/// <summary>
/// ParallelDiscoveryEventsHandler for handling the discovery events in case of parallel discovery
/// </summary>
internal class ParallelDiscoveryEventsHandler : ITestDiscoveryEventsHandler2
{
    private readonly IProxyDiscoveryManager _proxyDiscoveryManager;
    private readonly ITestDiscoveryEventsHandler2 _actualDiscoveryEventsHandler;
    private readonly IParallelProxyDiscoveryManager _parallelProxyDiscoveryManager;
    private readonly DiscoveryDataAggregator _discoveryDataAggregator;
    private readonly IDataSerializer _dataSerializer;
    private readonly IRequestData _requestData;

    public ParallelDiscoveryEventsHandler(IRequestData requestData,
        IProxyDiscoveryManager proxyDiscoveryManager,
        ITestDiscoveryEventsHandler2 actualDiscoveryEventsHandler,
        IParallelProxyDiscoveryManager parallelProxyDiscoveryManager,
        DiscoveryDataAggregator discoveryDataAggregator) :
        this(requestData, proxyDiscoveryManager, actualDiscoveryEventsHandler, parallelProxyDiscoveryManager, discoveryDataAggregator, JsonDataSerializer.Instance)
    {
    }

    internal ParallelDiscoveryEventsHandler(IRequestData requestData,
        IProxyDiscoveryManager proxyDiscoveryManager,
        ITestDiscoveryEventsHandler2 actualDiscoveryEventsHandler,
        IParallelProxyDiscoveryManager parallelProxyDiscoveryManager,
        DiscoveryDataAggregator discoveryDataAggregator,
        IDataSerializer dataSerializer)
    {
        _proxyDiscoveryManager = proxyDiscoveryManager;
        _actualDiscoveryEventsHandler = actualDiscoveryEventsHandler;
        _parallelProxyDiscoveryManager = parallelProxyDiscoveryManager;
        _discoveryDataAggregator = discoveryDataAggregator;
        _dataSerializer = dataSerializer;
        _requestData = requestData;
    }

    /// <inheritdoc/>
    public void HandleDiscoveryComplete(DiscoveryCompleteEventArgs discoveryCompleteEventArgs, IEnumerable<TestCase>? lastChunk)
    {
        // Aggregate for final discovery complete
        _discoveryDataAggregator.Aggregate(discoveryCompleteEventArgs);

        // We get DiscoveryComplete events from each ProxyDiscoveryManager (each testhost) and
        // they contain the last chunk of tests that were discovered in that particular testhost.
        // We want to send them to the IDE, but we don't want to tell it that discovery finished,
        // because other sources are still being discovered. So we translate the last chunk to a
        // normal TestsDiscovered event and send that to the IDE. Once all sources are completed,
        // we will send a single DiscoveryComplete.
        if (lastChunk != null)
        {
            ConvertToRawMessageAndSend(MessageType.TestCasesFound, lastChunk);
            HandleDiscoveredTests(lastChunk);
        }

        // Do not send TestDiscoveryComplete to actual test discovery handler
        // We need to see if there are still sources left - let the parallel manager decide
        var parallelDiscoveryComplete = _parallelProxyDiscoveryManager.HandlePartialDiscoveryComplete(
            _proxyDiscoveryManager,
            discoveryCompleteEventArgs.TotalCount,
            null, // Set lastChunk to null, because we already sent the data using HandleDiscoveredTests above.
            discoveryCompleteEventArgs.IsAborted);

        if (!parallelDiscoveryComplete
            || !_discoveryDataAggregator.TryAggregateIsMessageSent())
        {
            return;
        }

        // Manager said we are ready to publish the test discovery completed and we haven't yet
        // published it.
        var fullyDiscovered = _discoveryDataAggregator.GetSourcesWithStatus(DiscoveryStatus.FullyDiscovered);
        var partiallyDiscovered = _discoveryDataAggregator.GetSourcesWithStatus(DiscoveryStatus.PartiallyDiscovered);
        var notDiscovered = _discoveryDataAggregator.GetSourcesWithStatus(DiscoveryStatus.NotDiscovered);
        var skippedDiscovery = _discoveryDataAggregator.GetSourcesWithStatus(DiscoveryStatus.SkippedDiscovery);

        // As we immediately return results to IDE in case of aborting we need to set
        // isAborted = true and totalTests = -1
        if (!_discoveryDataAggregator.IsAborted)
        {
            if (_parallelProxyDiscoveryManager.IsAbortRequested)
            {
                EqtTrace.Info("ParallelDiscoveryEventsHandler.HandleDiscoveryComplete: Abort requested but discovery data aggregator not updated, marking discovery as aborted.");
                _discoveryDataAggregator.MarkAsAborted();
            }
            // If any testhost fails we will end up with some sources not fully discovered. In this
            // case, we want to consider the discovery aborted (not user cancelled) as it indicates
            // there was a failure during discovery.
            else if (notDiscovered.Count > 0 || partiallyDiscovered.Count > 0)
            {
                EqtTrace.Info("ParallelDiscoveryEventsHandler.HandleDiscoveryComplete: Discovery completed but some sources are not fully discovered, marking discovery as aborted.");
                _discoveryDataAggregator.MarkAsAborted();
            }
        }
        else
        {
            EqtTrace.Info("ParallelDiscoveryEventsHandler.HandleDiscoveryComplete: Discovery completed.");
        }

        // Collecting Final Discovery State
        _requestData.MetricsCollection.Add(
            TelemetryDataConstants.DiscoveryState,
            discoveryCompleteEventArgs.IsAborted ? "Aborted" : "Completed");

        // Collect Aggregated Metrics Data
        var aggregatedDiscoveryDataMetrics = _discoveryDataAggregator.GetMetrics();

        // In case of sequential discovery - RawMessage would have contained a 'DiscoveryCompletePayload' object
        // To send a raw message - we need to create raw message from an aggregated payload object
        var testDiscoveryCompletePayload = new DiscoveryCompletePayload
        {
            TotalTests = _discoveryDataAggregator.TotalTests,
            IsAborted = _discoveryDataAggregator.IsAborted,
            LastDiscoveredTests = null,
            FullyDiscoveredSources = fullyDiscovered,
            PartiallyDiscoveredSources = partiallyDiscovered,
            NotDiscoveredSources = notDiscovered,
            SkippedDiscoverySources = skippedDiscovery,
            DiscoveredExtensions = _discoveryDataAggregator.DiscoveredExtensions,
            Metrics = aggregatedDiscoveryDataMetrics,
        };

        // Sending discovery complete message to IDE
        EqtTrace.Verbose("ParallelDiscoveryEventsHandler.HandleDiscoveryComplete: Sending discovery complete message to IDE.");
        ConvertToRawMessageAndSend(MessageType.DiscoveryComplete, testDiscoveryCompletePayload);

        var finalDiscoveryCompleteEventArgs = new DiscoveryCompleteEventArgs
        {
            TotalCount = _discoveryDataAggregator.TotalTests,
            IsAborted = _discoveryDataAggregator.IsAborted,
            FullyDiscoveredSources = fullyDiscovered,
            PartiallyDiscoveredSources = partiallyDiscovered,
            NotDiscoveredSources = notDiscovered,
            SkippedDiscoveredSources = skippedDiscovery,
            DiscoveredExtensions = _discoveryDataAggregator.DiscoveredExtensions,
            Metrics = aggregatedDiscoveryDataMetrics,
        };

        // send actual test discovery complete to clients
        EqtTrace.Verbose("ParallelDiscoveryEventsHandler.HandleDiscoveryComplete: Sending discovery complete event to clients.");
        _actualDiscoveryEventsHandler.HandleDiscoveryComplete(finalDiscoveryCompleteEventArgs, null);
    }

    /// <inheritdoc/>
    public void HandleRawMessage(string rawMessage)
    {
        // In case of parallel - we can send everything but handle complete
        // DiscoveryComplete is not true-end of the overall discovery as we only get completion of one host here
        // Always aggregate data, deserialize and raw for complete events
        var message = _dataSerializer.DeserializeMessage(rawMessage);

        // Do not send CancellationRequested message to Output window in IDE, as it is not useful for user
        if (string.Equals(message.MessageType, MessageType.TestMessage)
            && rawMessage.IndexOf(CommonResources.CancellationRequested) >= 0)
        {
            return;
        }

        // Do not deserialize further
        if (!string.Equals(MessageType.DiscoveryComplete, message.MessageType))
        {
            _actualDiscoveryEventsHandler.HandleRawMessage(rawMessage);
        }
    }

    /// <inheritdoc/>
    public void HandleDiscoveredTests(IEnumerable<TestCase>? discoveredTestCases)
    {
        _actualDiscoveryEventsHandler.HandleDiscoveredTests(discoveredTestCases);
    }

    /// <inheritdoc/>
    public void HandleLogMessage(TestMessageLevel level, string? message)
    {
        _actualDiscoveryEventsHandler.HandleLogMessage(level, message);
    }

    /// <summary>
    /// To send message to IDE output window use HandleRawMessage
    /// </summary>
    /// <param name="messageType"></param>
    /// <param name="payload"></param>
    private void ConvertToRawMessageAndSend(string messageType, object payload)
    {
        var rawMessage = _dataSerializer.SerializePayload(messageType, payload, _requestData.ProtocolConfig!.Version);
        _actualDiscoveryEventsHandler.HandleRawMessage(rawMessage);
    }
}
