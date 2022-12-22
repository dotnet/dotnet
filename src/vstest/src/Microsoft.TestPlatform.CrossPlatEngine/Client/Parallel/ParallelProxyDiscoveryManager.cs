// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.Parallel;

/// <summary>
/// ParallelProxyDiscoveryManager that manages parallel discovery
/// </summary>
internal sealed class ParallelProxyDiscoveryManager : IParallelProxyDiscoveryManager, IDisposable
{
    private readonly IDataSerializer _dataSerializer;
    private readonly DiscoveryDataAggregator _dataAggregator;
    private readonly bool _isParallel;
    private readonly ParallelOperationManager<IProxyDiscoveryManager, ITestDiscoveryEventsHandler2, DiscoveryCriteria> _parallelOperationManager;
    private readonly Dictionary<string, TestRuntimeProviderInfo> _sourceToTestHostProviderMap;
    private readonly IRequestData _requestData;

    private int _discoveryCompletedClients;
    private int _availableTestSources;
    private int _availableWorkloads;
    private bool _skipDefaultAdapters;
    private bool _isDisposed;

    public bool IsAbortRequested { get; private set; }

    /// <summary>
    /// LockObject to update discovery status in parallel
    /// </summary>
    private readonly object _discoveryStatusLockObject = new();

    public ParallelProxyDiscoveryManager(
        IRequestData requestData,
        Func<TestRuntimeProviderInfo, IProxyDiscoveryManager> actualProxyManagerCreator,
        DiscoveryDataAggregator dataAggregator,
        int parallelLevel,
        List<TestRuntimeProviderInfo> testHostProviders)
        : this(requestData, actualProxyManagerCreator, dataAggregator, JsonDataSerializer.Instance, parallelLevel, testHostProviders)
    {
    }

    internal ParallelProxyDiscoveryManager(
        IRequestData requestData,
        Func<TestRuntimeProviderInfo, IProxyDiscoveryManager> actualProxyManagerCreator,
        DiscoveryDataAggregator dataAggregator,
        IDataSerializer dataSerializer,
        int parallelLevel,
        List<TestRuntimeProviderInfo> testHostProviders)
    {
        _requestData = requestData;
        _dataSerializer = dataSerializer;
        _dataAggregator = dataAggregator;
        _isParallel = parallelLevel > 1;
        _parallelOperationManager = new(actualProxyManagerCreator, parallelLevel);
        _sourceToTestHostProviderMap = testHostProviders
            .SelectMany(provider => provider.SourceDetails.Select(s => new KeyValuePair<string, TestRuntimeProviderInfo>(s.Source!, provider)))
            .ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    #region IProxyDiscoveryManager

    /// <inheritdoc/>
    public void Initialize(bool skipDefaultAdapters)
    {
        _skipDefaultAdapters = skipDefaultAdapters;
    }

    /// <inheritdoc/>
    public void DiscoverTests(DiscoveryCriteria discoveryCriteria, ITestDiscoveryEventsHandler2 eventHandler)
    {
        ValidateArg.NotNull(discoveryCriteria, nameof(discoveryCriteria));
        ValidateArg.NotNull(eventHandler, nameof(eventHandler));

        var workloads = SplitToWorkloads(discoveryCriteria, _sourceToTestHostProviderMap);
        _availableTestSources = workloads.SelectMany(w => w.Work.Sources).Count();
        var runnableWorkloads = workloads.Where(workload => workload.HasProvider).ToList();
        _availableWorkloads = runnableWorkloads.Count;
        var nonRunnableWorkloads = workloads.Where(workload => !workload.HasProvider).ToList();

        EqtTrace.Verbose("ParallelProxyDiscoveryManager.DiscoverTests: Start discovery. Total sources: " + _availableTestSources);

        // Mark all sources as NotDiscovered here because if we get an early cancellation it's
        // possible that we didn't yet start all the proxy managers and so we didn't mark all sources
        // as NotDiscovered.
        // For example, let's assume we have 10 sources, a batch size of 10 but only 8 cores, we
        // will then spawn 8 instances of this and if we now cancel, we will have 2 sources not
        // marked as NotDiscovered.
        _dataAggregator.MarkSourcesWithStatus(discoveryCriteria.Sources, DiscoveryStatus.NotDiscovered);

        if (nonRunnableWorkloads.Count > 0)
        {
            // We found some sources that don't associate to any runtime provider and so they cannot run.
            // Mark the sources as skipped.

            _dataAggregator.MarkSourcesWithStatus(nonRunnableWorkloads.SelectMany(w => w.Work.Sources), DiscoveryStatus.SkippedDiscovery);
            // TODO: in strict mode keep them as non-discovered, and mark the run as aborted.
            // _dataAggregator.MarkAsAborted();
        }

        _parallelOperationManager.StartWork(runnableWorkloads, eventHandler, GetParallelEventHandler, DiscoverTestsOnConcurrentManager);
    }

    private ITestDiscoveryEventsHandler2 GetParallelEventHandler(ITestDiscoveryEventsHandler2 eventHandler, IProxyDiscoveryManager concurrentManager)
        => new ParallelDiscoveryEventsHandler(
            _requestData,
            concurrentManager,
            eventHandler,
            this,
            _dataAggregator);

    /// <inheritdoc/>
    public void Abort()
    {
        IsAbortRequested = true;
        _parallelOperationManager.DoActionOnAllManagers((proxyManager) => proxyManager.Abort(), doActionsInParallel: true);
    }

    /// <inheritdoc/>
    public void Abort(ITestDiscoveryEventsHandler2 eventHandler)
    {
        IsAbortRequested = true;
        _parallelOperationManager.DoActionOnAllManagers((proxyManager) => proxyManager.Abort(eventHandler), doActionsInParallel: true);
    }

    /// <inheritdoc/>
    public void Close()
    {
        _parallelOperationManager.DoActionOnAllManagers(proxyManager => proxyManager.Close(), doActionsInParallel: true);
    }

    #endregion

    #region IParallelProxyDiscoveryManager methods

    /// <inheritdoc/>
    public bool HandlePartialDiscoveryComplete(IProxyDiscoveryManager proxyDiscoveryManager, long totalTests, IEnumerable<TestCase>? lastChunk, bool isAborted)
    {
#if DEBUG
        // Ensures that the total count of sources remains the same between each discovery
        // completion of the same initial discovery request.
        var notDiscoveredCount = _dataAggregator.GetSourcesWithStatus(DiscoveryStatus.NotDiscovered).Count;
        var partiallyDiscoveredCount = _dataAggregator.GetSourcesWithStatus(DiscoveryStatus.PartiallyDiscovered).Count;
        var fullyDiscoveredCount = _dataAggregator.GetSourcesWithStatus(DiscoveryStatus.FullyDiscovered).Count;
        var skippedCount = _dataAggregator.GetSourcesWithStatus(DiscoveryStatus.SkippedDiscovery).Count;
        var expectedCount = _availableTestSources;
        // When this fails, look at the _dataAggregator and look at the sources that it holds. It is possible that adapter incorrectly reports
        // the source on the testcase object. Each distinct source that will appear on TestCase will be considered a file.
        TPDebug.Assert(notDiscoveredCount + partiallyDiscoveredCount + fullyDiscoveredCount + skippedCount == expectedCount,
            $"Total count of sources ({expectedCount}) should match the count of sources with status not discovered ({notDiscoveredCount}), partially discovered ({partiallyDiscoveredCount}), fully discovered ({fullyDiscoveredCount}) and skipped ({skippedCount}).");
#endif

        var allDiscoverersCompleted = false;
        // TODO: Interlocked.Increment the count, and the condition below probably does not need to be in a lock?
        lock (_discoveryStatusLockObject)
        {
            // Each concurrent Executor calls this method
            // So, we need to keep track of total discovery complete calls
            _discoveryCompletedClients++;

            // If there are no more sources/testcases, a parallel executor is truly done with discovery
            allDiscoverersCompleted = _discoveryCompletedClients == _availableWorkloads;

            EqtTrace.Verbose("ParallelProxyDiscoveryManager.HandlePartialDiscoveryComplete: Total completed clients = {0}, Discovery complete = {1}, Aborted = {2}, Abort requested: {3}.", _discoveryCompletedClients, allDiscoverersCompleted, isAborted, IsAbortRequested);
        }

        // If discovery is complete or discovery aborting was requested by testPlatfrom(user)
        // we need to stop all ongoing discoveries, because we want to separate aborting request
        // when testhost crashed by itself and when user requested it (e.g. through TW).
        // Schedule the clean up for managers and handlers.
        if (allDiscoverersCompleted || IsAbortRequested)
        {
            _parallelOperationManager.StopAllManagers();

            if (allDiscoverersCompleted)
            {
                EqtTrace.Verbose("ParallelProxyDiscoveryManager.HandlePartialDiscoveryComplete: All sources were discovered.");
            }
            else
            {
                EqtTrace.Verbose($"ParallelProxyDiscoveryManager.HandlePartialDiscoveryComplete: Abort was requested.");
            }

            return true;
        }

        _parallelOperationManager.RunNextWork(proxyDiscoveryManager);

        return false;
    }

    #endregion

    private List<ProviderSpecificWorkload<DiscoveryCriteria>> SplitToWorkloads(DiscoveryCriteria discoveryCriteria, Dictionary<string, TestRuntimeProviderInfo> sourceToTestHostProviderMap)
    {
        var sources = discoveryCriteria.Sources;
        // Each source is grouped with its respective provider.
        var providerGroups = sources
            .Select(source => new ProviderSpecificWorkload<string>(source, sourceToTestHostProviderMap[source]))
            .GroupBy(psw => psw.Provider);

        List<ProviderSpecificWorkload<DiscoveryCriteria>> workloads = new();
        foreach (var group in providerGroups)
        {
            var testhostProviderInfo = group.Key;
            // If the run is not parallel and the host is shared, put all testcases on single testhost.
            // For parallel we prefer to run each source on its own host because we don't know how big the source is
            // (how many tests there are to discover), and running 10 sources on 10 parallel testhosts is faster
            // in almost all cases than running 10 sources on 1 testhost.
            List<string[]> sourceBatches;
            if (!_isParallel && testhostProviderInfo.Shared)
            {
                // Create one big source batch that will be single workload for single testhost.
                sourceBatches = new List<string[]> { group.Select(w => w.Work).ToArray() };
            }
            else
            {
                // Create multiple source batches, each having one source, so each testhost will end up running one source.
                sourceBatches = group.Select(w => new[] { w.Work }).ToList();
            }

            foreach (var sourcesToDiscover in sourceBatches)
            {
                var updatedCriteria = NewDiscoveryCriteriaFromSourceAndSettings(sourcesToDiscover, discoveryCriteria, testhostProviderInfo.RunSettings);
                var workload = new ProviderSpecificWorkload<DiscoveryCriteria>(updatedCriteria, testhostProviderInfo);
                workloads.Add(workload);
            }
        }

        return workloads;

        static DiscoveryCriteria NewDiscoveryCriteriaFromSourceAndSettings(IEnumerable<string> sources, DiscoveryCriteria discoveryCriteria, string? runsettingsXml)
        {
            var criteria = new DiscoveryCriteria(
                sources,
                discoveryCriteria.FrequencyOfDiscoveredTestsEvent,
                discoveryCriteria.DiscoveredTestEventTimeout,
                runsettingsXml,
                discoveryCriteria.TestSessionInfo
            );

            criteria.TestCaseFilter = discoveryCriteria.TestCaseFilter;

            return criteria;
        }
    }

    /// <summary>
    /// Triggers the discovery for the next data object on the concurrent discoverer
    /// Each concurrent discoverer calls this method, once its completed working on previous data
    /// </summary>
    /// <param name="ProxyDiscoveryManager">Proxy discovery manager instance.</param>
    private void DiscoverTestsOnConcurrentManager(IProxyDiscoveryManager proxyDiscoveryManager, ITestDiscoveryEventsHandler2 eventHandler, DiscoveryCriteria discoveryCriteria)
    {
        // Kick off another discovery task for the next source
        Task.Run(() =>
            {
                EqtTrace.Verbose("ParallelProxyDiscoveryManager: Discovery started.");

                proxyDiscoveryManager.Initialize(_skipDefaultAdapters);
                proxyDiscoveryManager.DiscoverTests(discoveryCriteria, eventHandler);
            })
            .ContinueWith(t =>
                {
                    // Just in case, the actual discovery couldn't start for an instance. Ensure that
                    // we call discovery complete since we have already fetched a source. Otherwise
                    // discovery will not terminate
                    EqtTrace.Error("ParallelProxyDiscoveryManager: Failed to trigger discovery. Exception: " + t.Exception);

                    var handler = eventHandler;
                    var exceptionToString = t.Exception?.ToString();
                    var testMessagePayload = new TestMessagePayload { MessageLevel = TestMessageLevel.Error, Message = exceptionToString };
                    handler.HandleRawMessage(_dataSerializer.SerializePayload(MessageType.TestMessage, testMessagePayload));
                    handler.HandleLogMessage(TestMessageLevel.Error, exceptionToString);

                    // Send discovery complete. Similar logic is also used in ProxyDiscoveryManager.DiscoverTests.
                    // Differences:
                    // Total tests must be zero here since parallel discovery events handler adds the count
                    // Keep `lastChunk` as null since we don't want a message back to the IDE (discovery didn't even begin)
                    // Set `isAborted` as true since we want this instance of discovery manager to be replaced
                    // TODO: the comment above mentions 0 tests but sends -1. Make sense of this.
                    var discoveryCompleteEventsArgs = new DiscoveryCompleteEventArgs(-1, true);
                    handler.HandleDiscoveryComplete(discoveryCompleteEventsArgs, null);
                },
                TaskContinuationOptions.OnlyOnFaulted);

        EqtTrace.Verbose("ProxyParallelDiscoveryManager.DiscoverTestsOnConcurrentManager: No sources available for discovery.");
    }

    public void Dispose()
    {
        if (!_isDisposed)
        {
            _parallelOperationManager.Dispose();
            _isDisposed = true;
        }
    }
}
