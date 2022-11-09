// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.TestRunAttachmentsProcessing;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.Parallel;

/// <summary>
/// ParallelProxyExecutionManager that manages parallel execution
/// </summary>
internal sealed class ParallelProxyExecutionManager : IParallelProxyExecutionManager, IDisposable
{
    private readonly IDataSerializer _dataSerializer;
    private readonly bool _isParallel;
    private readonly ParallelOperationManager<IProxyExecutionManager, IInternalTestRunEventsHandler, TestRunCriteria> _parallelOperationManager;
    private readonly Dictionary<string, TestRuntimeProviderInfo> _sourceToTestHostProviderMap;

    private bool _isDisposed;

    #region TestRunSpecificData

    // This variable id to differentiate between implicit (abort requested by testPlatform) and explicit (test host aborted) abort.
    private bool _abortRequested;

    private int _runCompletedClients;
    private int _runStartedClients;
    private int _availableWorkloads = -1;

    private ParallelRunDataAggregator? _currentRunDataAggregator;

    private readonly IRequestData _requestData;
    private bool _skipDefaultAdapters;

    /// <inheritdoc/>
    public bool IsInitialized { get; private set; }

    #endregion

    #region Concurrency Keeper Objects

    /// <summary>
    /// LockObject to update execution status in parallel
    /// </summary>
    private readonly object _executionStatusLockObject = new();

    #endregion

    public ParallelProxyExecutionManager(
        IRequestData requestData,
        Func<TestRuntimeProviderInfo, IProxyExecutionManager> actualProxyManagerCreator,
        int parallelLevel,
         List<TestRuntimeProviderInfo> testHostProviders)
        : this(requestData, actualProxyManagerCreator, JsonDataSerializer.Instance, parallelLevel, testHostProviders)
    {
    }

    internal ParallelProxyExecutionManager(
        IRequestData requestData,
        Func<TestRuntimeProviderInfo, IProxyExecutionManager> actualProxyManagerCreator,
        IDataSerializer dataSerializer,
        int parallelLevel,
        List<TestRuntimeProviderInfo> testHostProviders)
    {
        _requestData = requestData;
        _dataSerializer = dataSerializer;
        _isParallel = parallelLevel > 1;
        _parallelOperationManager = new(actualProxyManagerCreator, parallelLevel);
        _sourceToTestHostProviderMap = testHostProviders
            .SelectMany(provider => provider.SourceDetails.Select(s => new KeyValuePair<string, TestRuntimeProviderInfo>(s.Source!, provider)))
            .ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    public void Initialize(bool skipDefaultAdapters)
    {
        _skipDefaultAdapters = skipDefaultAdapters;
    }

    public int StartTestRun(TestRunCriteria testRunCriteria, IInternalTestRunEventsHandler eventHandler)
    {
        var workloads = SplitToWorkloads(testRunCriteria, _sourceToTestHostProviderMap);
        var runnableWorkloads = workloads.Where(workload => workload.HasProvider).ToList();
        var nonRunnableWorkloads = workloads.Where(workload => !workload.HasProvider).ToList();

        _availableWorkloads = runnableWorkloads.Count;

        EqtTrace.Verbose("ParallelProxyExecutionManager: Start execution. Total sources: " + _availableWorkloads);

        // Reset the run complete data
        _runCompletedClients = 0;

        // One data aggregator per parallel run
        TPDebug.Assert(testRunCriteria.TestRunSettings is not null, "testRunCriteria.TestRunSettings is null");
        _currentRunDataAggregator = new ParallelRunDataAggregator(testRunCriteria.TestRunSettings);
        if (nonRunnableWorkloads.Count > 0)
        {
            // TODO: in strict mode fail if we find a source that we cannot run.
            // _currentRunDataAggregator.MarkAsAborted();
        }

        _parallelOperationManager.StartWork(runnableWorkloads, eventHandler, GetParallelEventHandler, StartTestRunOnConcurrentManager);

        // Why 1? Because this is supposed to be a processId, and that is just the default that was chosen by someone before me,
        // and maybe is checked somewhere, but I don't see it checked in our codebase.
        return 1;
    }

    public void Abort(IInternalTestRunEventsHandler runEventsHandler)
    {
        // Test platform initiated abort.
        _abortRequested = true;
        _parallelOperationManager.DoActionOnAllManagers((proxyManager) => proxyManager.Abort(runEventsHandler), doActionsInParallel: true);
    }

    public void Cancel(IInternalTestRunEventsHandler runEventsHandler)
    {
        _parallelOperationManager.DoActionOnAllManagers((proxyManager) => proxyManager.Cancel(runEventsHandler), doActionsInParallel: true);
    }

    public void Close()
    {
        _parallelOperationManager.DoActionOnAllManagers(proxyManager => proxyManager.Close(), doActionsInParallel: true);
    }

    /// <inheritdoc/>
    public bool HandlePartialRunComplete(
        IProxyExecutionManager proxyExecutionManager,
        TestRunCompleteEventArgs testRunCompleteArgs,
        TestRunChangedEventArgs? lastChunkArgs,
        ICollection<AttachmentSet>? runContextAttachments,
        ICollection<string>? executorUris)
    {
        var allRunsCompleted = false;
        // TODO: Interlocked.Increment _runCompletedClients, and the condition on the bottom probably does not need to be under lock??
        lock (_executionStatusLockObject)
        {
            // Each concurrent Executor calls this method
            // So, we need to keep track of total run complete calls
            _runCompletedClients++;

            allRunsCompleted = testRunCompleteArgs.IsCanceled || _abortRequested
                ? _runCompletedClients == _runStartedClients
                : _runCompletedClients == _availableWorkloads;

            EqtTrace.Verbose("ParallelProxyExecutionManager: HandlePartialRunComplete: Total completed clients = {0}, Run complete = {1}, Run canceled: {2}.", _runCompletedClients, allRunsCompleted, testRunCompleteArgs.IsCanceled);
        }

        if (allRunsCompleted)
        {
            _parallelOperationManager.StopAllManagers();
            return true;
        }

        // If cancel is triggered for any one run or abort is requested by test platform, there is no reason to fetch next source
        // and queue another test run.
        if (!testRunCompleteArgs.IsCanceled && !_abortRequested)
        {
            // Do NOT return true here, there should be only one place where this method returns true,
            // and cancellation or success or any other other combination or timing should result in only one true.
            // This is largely achieved by returning true above when "allRunsCompleted" is true. That variable is true
            // when we cancel all sources or when we complete all sources.
            //
            // But we can also start a source, and cancel right after, which will remove all managers, and RunNextWork returns
            // false, because we had no more work to do. If we check that result here and return true, then the whole logic is
            // broken and we end up calling RunComplete handlers twice and writing logger output to screen twice. So don't do it.
            // var hadMoreWork = _parallelOperationManager.RunNextWork(proxyExecutionManager);
            // if (!hadMoreWork)
            // {
            //     return true;
            // }
            var _ = _parallelOperationManager.RunNextWork(proxyExecutionManager);
        }

        return false;
    }

    /// <summary>
    ///  Split the incoming work into smaller workloads that we can run on different testhosts.
    ///  Each workload is associated with a type of provider that can run it.
    /// </summary>
    /// <param name="testRunCriteria"></param>
    /// <param name="sourceToTestHostProviderMap"></param>
    /// <returns></returns>
    private List<ProviderSpecificWorkload<TestRunCriteria>> SplitToWorkloads(TestRunCriteria testRunCriteria, Dictionary<string, TestRuntimeProviderInfo> sourceToTestHostProviderMap)
    {
        // We split the work to workloads that will run on each testhost, and add all of them
        // to a bag of work that needs to be processed. (The workloads are just
        // a single source, multiple sources, or all test cases for a given source.)
        //
        // For every workload we associated a given type of testhost that can run the work.
        // This is important when we have shared testhosts.
        //
        // We then start as many instances of testhost as we are allowed by parallel level,
        // and we start sending them work. Once any testhost is done processing a given workload,
        // we will get notified with the completed event for the work we are doing. For example for StartTestRun
        // we will get TestExecutionCompleted, and we will call HandlePartialTestExecutionComplete.
        // (The "partial" here refers to possibly having more work in the work bag. It does not mean that
        // there was an error in the testhost and we only did part of execution.).
        //
        // At that point we know that at least one testhost is not busy doing work anymore. It either
        // processed the workload and waits for another one, or it crashed and we should move to
        // another source.
        //
        // In the "partial" step we check if we have more workloads, and if the currently running testhost
        // is shared we try to find a workload that is appropriate for it. If we don't find any work that the
        // running testhost can do. Or if the testhost already exited (possibly because of crash), we start another one
        // and give it the next workload.


        // List all the sources (dlls) we have and group them by their provider, so we can run either multiple sources
        // on a single instance of the appropriate testhost (for non-paralell, shared),
        // or each source in its own instance of the provider
        // (for non-parallel, non-shared, and both parallel shared, and parallel non-shared).
        //
        // We prefer running more work in parallel when we can, because doing work on 10 testhosts in
        // parallel is faster than doing the same work on 1 testhost. Even when the sources to discover
        // contain just a few tests.
        List<ProviderSpecificWorkload<TestRunCriteria>> workloads = new();
        if (testRunCriteria.HasSpecificTests)
        {
            // We split test cases to their respective sources, and associate them with additional info about on
            // which type of provider they can run so we can later select the correct workload for the provider
            // if we already have a shared provider running, that can take more sources.
            Dictionary<string, TestCase[]> sourceToTestCasesMap = testRunCriteria.Tests.GroupBy(t => t.Source).ToDictionary(k => k.Key, v => v.ToArray());

            var sources = sourceToTestCasesMap.Keys;
            // Each source is grouped with its respective provider.
            var providerGroups = sources.Select(source => new ProviderSpecificWorkload<string>(source, sourceToTestHostProviderMap[source])).GroupBy(psw => psw.Provider);

            foreach (var group in providerGroups)
            {
                var testhostProviderInfo = group.Key;

                List<TestCase[]> testCaseBatches;
                if (!_isParallel && testhostProviderInfo.Shared)
                {
                    // Create one big batch of testcases that will be single workload for single testhost.
                    testCaseBatches = new List<TestCase[]> { group.SelectMany(w => sourceToTestCasesMap[w.Work]).ToArray() };
                }
                else
                {
                    // Create multiple testcase batches, each having set of testcases from single source,
                    // so each testhost will end up running one source.
                    testCaseBatches = group.Select(w => sourceToTestCasesMap[w.Work]).ToList();
                }

                foreach (var testCases in testCaseBatches)
                {
                    var runsettings = testhostProviderInfo.RunSettings;
                    var updatedCriteria = CreateTestRunCriteriaFromTestCasesAndSettings(testCases, testRunCriteria, runsettings);
                    var workload = new ProviderSpecificWorkload<TestRunCriteria>(updatedCriteria, testhostProviderInfo);
                    workloads.Add(workload);
                }
            }
        }
        else
        {
            TPDebug.Assert(testRunCriteria.Sources is not null, "testRunCriteria.Sources is null");
            // Each source is grouped with its respective provider.
            var providerGroups = testRunCriteria.Sources
                .Select(source => new ProviderSpecificWorkload<string>(source, sourceToTestHostProviderMap[source]))
                .GroupBy(psw => psw.Provider);

            foreach (var group in providerGroups)
            {
                var testhostProviderInfo = group.Key;

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

                foreach (var sourcesToRun in sourceBatches)
                {
                    var runsettings = testhostProviderInfo.RunSettings;
                    var updatedCriteria = CreateTestRunCriteriaFromSourceAndSettings(sourcesToRun, testRunCriteria, runsettings);
                    var workload = new ProviderSpecificWorkload<TestRunCriteria>(updatedCriteria, testhostProviderInfo);
                    workloads.Add(workload);
                }
            }
        }

        return workloads;

        TestRunCriteria CreateTestRunCriteriaFromTestCasesAndSettings(IEnumerable<TestCase> testCases, TestRunCriteria criteria, string? runsettingsXml)
        {
            return new TestRunCriteria(
                 testCases,
                 testRunCriteria.FrequencyOfRunStatsChangeEvent,
                 testRunCriteria.KeepAlive,
                 runsettingsXml,
                 testRunCriteria.RunStatsChangeEventTimeout,
                 testRunCriteria.TestHostLauncher,
                 testRunCriteria.TestSessionInfo,
                 testRunCriteria.DebugEnabledForTestSession);
        }

        TestRunCriteria CreateTestRunCriteriaFromSourceAndSettings(IEnumerable<string> sources, TestRunCriteria criteria, string? runsettingsXml)
        {
            return new TestRunCriteria(
                 sources,
                 testRunCriteria.FrequencyOfRunStatsChangeEvent,
                 testRunCriteria.KeepAlive,
                 runsettingsXml,
                 testRunCriteria.RunStatsChangeEventTimeout,
                 testRunCriteria.TestHostLauncher,
                 testRunCriteria.TestCaseFilter,
                 testRunCriteria.FilterOptions,
                 testRunCriteria.TestSessionInfo,
                 testRunCriteria.DebugEnabledForTestSession);
        }
    }

    private ParallelRunEventsHandler GetParallelEventHandler(IInternalTestRunEventsHandler eventHandler, IProxyExecutionManager concurrentManager)
    {
        TPDebug.Assert(_currentRunDataAggregator is not null, "_currentRunDataAggregator is null");

        if (concurrentManager is ProxyExecutionManagerWithDataCollection concurrentManagerWithDataCollection)
        {
            var attachmentsProcessingManager = new TestRunAttachmentsProcessingManager(TestPlatformEventSource.Instance, new DataCollectorAttachmentsProcessorsFactory());

            return new ParallelDataCollectionEventsHandler(
                _requestData,
                concurrentManagerWithDataCollection,
                eventHandler,
                this,
                _currentRunDataAggregator,
                attachmentsProcessingManager,
                concurrentManagerWithDataCollection.CancellationToken);
        }

        return new ParallelRunEventsHandler(
            _requestData,
            concurrentManager,
            eventHandler,
            this,
            _currentRunDataAggregator);
    }

    /// <summary>
    /// Triggers the execution for the next data object on the concurrent executor
    /// Each concurrent executor calls this method, once its completed working on previous data
    /// </summary>
    /// <param name="proxyExecutionManager">Proxy execution manager instance.</param>
    /// <returns>True, if execution triggered</returns>
    private void StartTestRunOnConcurrentManager(IProxyExecutionManager proxyExecutionManager, IInternalTestRunEventsHandler eventHandler, TestRunCriteria testRunCriteria)
    {
        if (testRunCriteria != null)
        {
            if (!proxyExecutionManager.IsInitialized)
            {
                proxyExecutionManager.Initialize(_skipDefaultAdapters);
            }

            Task.Run(() =>
                {
                    Interlocked.Increment(ref _runStartedClients);
                    EqtTrace.Verbose("ParallelProxyExecutionManager: Execution started. Started clients: " + _runStartedClients);

                    proxyExecutionManager.StartTestRun(testRunCriteria, eventHandler);
                })
                .ContinueWith(t =>
                    {
                        // Just in case, the actual execution couldn't start for an instance. Ensure that
                        // we call execution complete since we have already fetched a source. Otherwise
                        // execution will not terminate
                        EqtTrace.Error("ParallelProxyExecutionManager: Failed to trigger execution. Exception: " + t.Exception);

                        var handler = eventHandler;
                        var exceptionToString = t.Exception?.ToString();
                        var testMessagePayload = new TestMessagePayload { MessageLevel = TestMessageLevel.Error, Message = exceptionToString };
                        handler.HandleRawMessage(_dataSerializer.SerializePayload(MessageType.TestMessage, testMessagePayload));
                        handler.HandleLogMessage(TestMessageLevel.Error, exceptionToString);

                        // Send a run complete to caller. Similar logic is also used in ProxyExecutionManager.StartTestRun
                        // Differences:
                        // Aborted is sent to allow the current execution manager replaced with another instance
                        // Ensure that the test run aggregator in parallel run events handler doesn't add these statistics
                        // (since the test run didn't even start)
                        var completeArgs = new TestRunCompleteEventArgs(null, false, true, null, new Collection<AttachmentSet>(), new Collection<InvokedDataCollector>(), TimeSpan.Zero);
                        handler.HandleTestRunComplete(completeArgs, null, null, null);
                    },
                    TaskContinuationOptions.OnlyOnFaulted);
        }

        EqtTrace.Verbose("ProxyParallelExecutionManager: No sources available for execution.");
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

/// <summary>
/// A workload with a specification of a provider that can run that workload. The workload is a list of sources,
/// or a list of testcases. Provider is a testhost manager, that is capable of running this workload, so
/// we end up running .NET sources on .NET testhost, and .NET Framework sources on .NET Framework testhost.
/// </summary>
internal class ProviderSpecificWorkload<T>
{
    public T Work { get; }

    public TestRuntimeProviderInfo Provider { get; protected set; }

    public bool HasProvider => Provider.Type is not null;

    public ProviderSpecificWorkload(T work, TestRuntimeProviderInfo provider)
    {
        Provider = provider;
        Work = work;
    }
}
