// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.Parallel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection;

internal class ParallelDataCollectionEventsHandler : ParallelRunEventsHandler
{
    private readonly ParallelRunDataAggregator _runDataAggregator;
    private readonly ITestRunAttachmentsProcessingManager? _attachmentsProcessingManager;
    private readonly CancellationToken _cancellationToken;

    public ParallelDataCollectionEventsHandler(IRequestData requestData,
        IProxyExecutionManager proxyExecutionManager,
        IInternalTestRunEventsHandler actualRunEventsHandler,
        IParallelProxyExecutionManager parallelProxyExecutionManager,
        ParallelRunDataAggregator runDataAggregator,
        ITestRunAttachmentsProcessingManager attachmentsProcessingManager,
        CancellationToken cancellationToken)
        : this(requestData, proxyExecutionManager, actualRunEventsHandler, parallelProxyExecutionManager, runDataAggregator, JsonDataSerializer.Instance)
    {
        _attachmentsProcessingManager = attachmentsProcessingManager;
        _cancellationToken = cancellationToken;
    }

    internal ParallelDataCollectionEventsHandler(IRequestData requestData,
        IProxyExecutionManager proxyExecutionManager,
        IInternalTestRunEventsHandler actualRunEventsHandler,
        IParallelProxyExecutionManager parallelProxyExecutionManager,
        ParallelRunDataAggregator runDataAggregator,
        IDataSerializer dataSerializer)
        : base(requestData, proxyExecutionManager, actualRunEventsHandler, parallelProxyExecutionManager, runDataAggregator, dataSerializer)
    {
        _runDataAggregator = runDataAggregator;
    }

    /// <summary>
    /// Handles the Run Complete event from a parallel proxy manager
    /// </summary>
    public override void HandleTestRunComplete(
        TestRunCompleteEventArgs testRunCompleteArgs,
        TestRunChangedEventArgs? lastChunkArgs,
        ICollection<AttachmentSet>? runContextAttachments,
        ICollection<string>? executorUris)
    {
        var parallelRunComplete = HandleSingleTestRunComplete(testRunCompleteArgs, lastChunkArgs, runContextAttachments, executorUris);

        if (!parallelRunComplete)
        {
            return;
        }

        TPDebug.Assert(_attachmentsProcessingManager is not null, "_attachmentsProcessingManager is null");
        _runDataAggregator.RunContextAttachments = _attachmentsProcessingManager.ProcessTestRunAttachmentsAsync(_runDataAggregator.RunSettings, _requestData, _runDataAggregator.RunContextAttachments, _runDataAggregator.InvokedDataCollectors, _cancellationToken).Result ?? _runDataAggregator.RunContextAttachments;

        var completedArgs = new TestRunCompleteEventArgs(_runDataAggregator.GetAggregatedRunStats(),
            _runDataAggregator.IsCanceled,
            _runDataAggregator.IsAborted,
            _runDataAggregator.GetAggregatedException(),
            _runDataAggregator.RunContextAttachments,
            _runDataAggregator.InvokedDataCollectors,
            _runDataAggregator.ElapsedTime);

        // Add Metrics from Test Host
        completedArgs.Metrics = _runDataAggregator.GetAggregatedRunDataMetrics();

        HandleParallelTestRunComplete(completedArgs);
    }
}
