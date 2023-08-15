// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;

using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.Parallel;

/// <summary>
/// ParallelRunEventsHandler for handling the run events in case of parallel execution
/// </summary>
internal class ParallelRunEventsHandler : IInternalTestRunEventsHandler
{
    private readonly IProxyExecutionManager _proxyExecutionManager;

    private readonly IInternalTestRunEventsHandler _actualRunEventsHandler;

    private readonly IParallelProxyExecutionManager _parallelProxyExecutionManager;

    private readonly ParallelRunDataAggregator _runDataAggregator;

    private readonly IDataSerializer _dataSerializer;

    protected IRequestData _requestData;

    public ParallelRunEventsHandler(IRequestData requestData,
        IProxyExecutionManager proxyExecutionManager,
        IInternalTestRunEventsHandler actualRunEventsHandler,
        IParallelProxyExecutionManager parallelProxyExecutionManager,
        ParallelRunDataAggregator runDataAggregator) :
        this(requestData, proxyExecutionManager, actualRunEventsHandler, parallelProxyExecutionManager, runDataAggregator, JsonDataSerializer.Instance)
    {
    }

    internal ParallelRunEventsHandler(IRequestData requestData,
        IProxyExecutionManager proxyExecutionManager,
        IInternalTestRunEventsHandler actualRunEventsHandler,
        IParallelProxyExecutionManager parallelProxyExecutionManager,
        ParallelRunDataAggregator runDataAggregator,
        IDataSerializer dataSerializer)
    {
        _proxyExecutionManager = proxyExecutionManager;
        _actualRunEventsHandler = actualRunEventsHandler;
        _parallelProxyExecutionManager = parallelProxyExecutionManager;
        _runDataAggregator = runDataAggregator;
        _dataSerializer = dataSerializer;
        _requestData = requestData;
    }

    /// <summary>
    /// Handles the Run Complete event from a parallel proxy manager
    /// </summary>
    public virtual void HandleTestRunComplete(
        TestRunCompleteEventArgs testRunCompleteArgs,
        TestRunChangedEventArgs? lastChunkArgs,
        ICollection<AttachmentSet>? runContextAttachments,
        ICollection<string>? executorUris)
    {
        EqtTrace.Verbose($"ParallelRunEventsHandler.HandleTestRunComplete: Handling a run completion, this can be either one part of parallel run completing, or the whole parallel run completing.");
        var parallelRunComplete = HandleSingleTestRunComplete(testRunCompleteArgs, lastChunkArgs, runContextAttachments, executorUris);

        if (parallelRunComplete)
        {
            EqtTrace.Verbose($"ParallelRunEventsHandler.HandleTestRunComplete: Whole parallel run completed.");
            var completedArgs = new TestRunCompleteEventArgs(_runDataAggregator.GetAggregatedRunStats(),
                _runDataAggregator.IsCanceled,
                _runDataAggregator.IsAborted,
                _runDataAggregator.GetAggregatedException(),
                new Collection<AttachmentSet>(_runDataAggregator.RunCompleteArgsAttachments),
                new Collection<InvokedDataCollector>(_runDataAggregator.InvokedDataCollectors),
                _runDataAggregator.ElapsedTime);

            // Collect Final RunState
            _requestData.MetricsCollection.Add(
                TelemetryDataConstants.RunState,
                _runDataAggregator.IsAborted
                    ? "Aborted"
                    : _runDataAggregator.IsCanceled ? "Canceled" : "Completed");

            // Add the map containing discovered extensions to the event args. This map contains
            // only extensions discovered by the testhost processes. Current process extensions
            // (i.e. vstest.console) are added later on.
            completedArgs.DiscoveredExtensions = _runDataAggregator.DiscoveredExtensions;

            // Collect Aggregated Metrics Data
            var aggregatedRunDataMetrics = _runDataAggregator.GetAggregatedRunDataMetrics();
            completedArgs.Metrics = aggregatedRunDataMetrics;

            HandleParallelTestRunComplete(completedArgs);
        }
        else
        {
            EqtTrace.Verbose($"ParallelRunEventsHandler.HandleTestRunComplete: Single part of parallel run completed, but whole run is not complete yet.");
        }
    }

    protected bool HandleSingleTestRunComplete(TestRunCompleteEventArgs testRunCompleteArgs,
        TestRunChangedEventArgs? lastChunkArgs,
        ICollection<AttachmentSet>? runContextAttachments,
        ICollection<string>? executorUris)
    {
        // we get run complete events from each executor process
        // so we cannot "complete" the actual executor operation until all sources/testcases are consumed
        // We should not block last chunk results while we aggregate overall run data
        if (lastChunkArgs != null)
        {
            ConvertToRawMessageAndSend(MessageType.TestRunStatsChange, lastChunkArgs);
            HandleTestRunStatsChange(lastChunkArgs);
        }

        // Update run stats, executorUris, etc.
        // we need this data when we send the final run complete
        _runDataAggregator.Aggregate(
            testRunCompleteArgs.TestRunStatistics,
            executorUris,
            testRunCompleteArgs.Error,
            testRunCompleteArgs.ElapsedTimeInRunningTests,
            testRunCompleteArgs.IsAborted,
            testRunCompleteArgs.IsCanceled,
            runContextAttachments,
            testRunCompleteArgs.AttachmentSets,
            testRunCompleteArgs.InvokedDataCollectors,
            testRunCompleteArgs.DiscoveredExtensions);

        // Aggregate Run Data Metrics
        _runDataAggregator.AggregateRunDataMetrics(testRunCompleteArgs.Metrics);

        return _parallelProxyExecutionManager.HandlePartialRunComplete(
            _proxyExecutionManager,
            testRunCompleteArgs,
            null, // lastChunk should be null as we already sent this data above
            runContextAttachments,
            executorUris);
    }

    protected void HandleParallelTestRunComplete(TestRunCompleteEventArgs completedArgs)
    {
        // In case of sequential execution - RawMessage would have contained a 'TestRunCompletePayload' object
        // To send a rawmessge - we need to create rawmessage from an aggregated payload object
        var testRunCompletePayload = new TestRunCompletePayload()
        {
            ExecutorUris = _runDataAggregator.ExecutorUris,
            LastRunTests = null,
            RunAttachments = _runDataAggregator.RunContextAttachments,
            TestRunCompleteArgs = completedArgs
        };

        // we have to send rawmessages as we block the run complete actual raw messages
        ConvertToRawMessageAndSend(MessageType.ExecutionComplete, testRunCompletePayload);

        // send actual test run complete to clients
        _actualRunEventsHandler.HandleTestRunComplete(
            completedArgs, null, _runDataAggregator.RunContextAttachments, _runDataAggregator.ExecutorUris);
    }

    public void HandleRawMessage(string rawMessage)
    {
        // In case of parallel - we can send everything but handle complete
        // HandleComplete is not true-end of the overall execution as we only get completion of one executor here
        // Always aggregate data, deserialize and raw for complete events
        var message = _dataSerializer.DeserializeMessage(rawMessage);

        // Do not deserialize further - just send if not execution complete
        if (!string.Equals(MessageType.ExecutionComplete, message.MessageType))
        {
            _actualRunEventsHandler.HandleRawMessage(rawMessage);
        }
    }

    public void HandleTestRunStatsChange(TestRunChangedEventArgs? testRunChangedArgs)
    {
        _actualRunEventsHandler.HandleTestRunStatsChange(testRunChangedArgs);
    }

    public void HandleLogMessage(TestMessageLevel level, string? message)
    {
        _actualRunEventsHandler.HandleLogMessage(level, message);
    }

    public int LaunchProcessWithDebuggerAttached(TestProcessStartInfo testProcessStartInfo)
    {
        return _actualRunEventsHandler.LaunchProcessWithDebuggerAttached(testProcessStartInfo);
    }

    /// <inheritdoc />
    public bool AttachDebuggerToProcess(AttachDebuggerInfo attachDebuggerInfo)
    {
        return _actualRunEventsHandler.AttachDebuggerToProcess(attachDebuggerInfo);
    }

    private void ConvertToRawMessageAndSend(string messageType, object payload)
    {
        var rawMessage = _dataSerializer.SerializePayload(messageType, payload, _requestData.ProtocolConfig!.Version);
        _actualRunEventsHandler.HandleRawMessage(rawMessage);
    }
}
