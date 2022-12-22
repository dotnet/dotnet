// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;
using Microsoft.VisualStudio.TestPlatform.Common.Filtering;
using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Adapter;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Discovery;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.ClientProtocol;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

using CrossPlatEngineResources = Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Execution;

internal class RunTestsWithSources : BaseRunTests
{
    private readonly Dictionary<string, IEnumerable<string>> _adapterSourceMap;

    private Dictionary<Tuple<Uri, string>, IEnumerable<string>>? _executorUriVsSourceList;

    private readonly ITestCaseEventsHandler? _testCaseEventsHandler;

    public RunTestsWithSources(IRequestData requestData, Dictionary<string, IEnumerable<string>> adapterSourceMap, string? package, string? runSettings, TestExecutionContext testExecutionContext, ITestCaseEventsHandler? testCaseEventsHandler, IInternalTestRunEventsHandler testRunEventsHandler)
        : this(requestData, adapterSourceMap, package, runSettings, testExecutionContext, testCaseEventsHandler, testRunEventsHandler, null)
    {
    }

    /// <summary>
    /// Used for unit testing only.
    /// </summary>
    /// <param name="requestData"></param>
    /// <param name="adapterSourceMap"></param>
    /// <param name="package">The user input test source(package) if it differ from actual test source otherwise null.</param>
    /// <param name="runSettings"></param>
    /// <param name="testExecutionContext"></param>
    /// <param name="testCaseEventsHandler"></param>
    /// <param name="testRunEventsHandler"></param>
    /// <param name="executorUriVsSourceList"></param>
    /// <param name="testRunCache"></param>
    internal RunTestsWithSources(IRequestData requestData, Dictionary<string, IEnumerable<string>> adapterSourceMap, string? package, string? runSettings, TestExecutionContext testExecutionContext, ITestCaseEventsHandler? testCaseEventsHandler, IInternalTestRunEventsHandler testRunEventsHandler, Dictionary<Tuple<Uri, string>, IEnumerable<string>>? executorUriVsSourceList)
        : base(requestData, package, runSettings, testExecutionContext, testCaseEventsHandler, testRunEventsHandler, TestPlatformEventSource.Instance)
    {
        _adapterSourceMap = adapterSourceMap;
        _executorUriVsSourceList = executorUriVsSourceList;
        _testCaseEventsHandler = testCaseEventsHandler;
    }

    protected override void BeforeRaisingTestRunComplete(bool exceptionsHitDuringRunTests)
    {
        // If run was with sources and no test was executed and cancellation was not requested,
        // then raise a warning saying that no test was present in the sources.
        // The warning is raised only if total no of tests that have been run is zero.
        if (!exceptionsHitDuringRunTests && _executorUriVsSourceList?.Count > 0 && !IsCancellationRequested
            && TestRunCache?.TotalExecutedTests <= 0)
        {
            LogWarningOnNoTestsExecuted();
        }
    }

    private void LogWarningOnNoTestsExecuted()
    {
        IEnumerable<string> sources = new List<string>();
        var sourcesArray = _adapterSourceMap.Values
            .Aggregate(sources, (current, enumerable) => enumerable is not null ? current.Concat(enumerable) : current).ToArray();
        var sourcesString = string.Join(" ", sourcesArray);

        if (TestExecutionContext.TestCaseFilter != null)
        {
            var testCaseFilterToShow = TestCaseFilterDeterminer.ShortenTestCaseFilterIfRequired(TestExecutionContext.TestCaseFilter);
            TestRunEventsHandler?.HandleLogMessage(
                TestMessageLevel.Warning,
                string.Format(CultureInfo.CurrentCulture, CrossPlatEngineResources.NoTestsAvailableForGivenTestCaseFilter, testCaseFilterToShow, sourcesString));
        }
        else
        {
            TestRunEventsHandler?.HandleLogMessage(
                TestMessageLevel.Warning,
                string.Format(
                    CultureInfo.CurrentCulture,
                    CrossPlatEngineResources.TestRunFailed_NoDiscovererFound_NoTestsAreAvailableInTheSources,
                    sourcesString));
        }
    }

    protected override IEnumerable<Tuple<Uri, string>> GetExecutorUriExtensionMap(IFrameworkHandle testExecutorFrameworkHandle, RunContext runContext)
    {
        _executorUriVsSourceList = GetExecutorVsSourcesList(testExecutorFrameworkHandle);
        var executorUris = _executorUriVsSourceList.Keys;

        runContext.FilterExpressionWrapper = !TestExecutionContext.TestCaseFilter.IsNullOrEmpty()
            ? new FilterExpressionWrapper(TestExecutionContext.TestCaseFilter, TestExecutionContext.FilterOptions)
            : null;

        return executorUris;
    }

    protected override void InvokeExecutor(
        LazyExtension<ITestExecutor, ITestExecutorCapabilities> executor,
        Tuple<Uri, string> executorUriExtensionTuple,
        RunContext? runContext,
        IFrameworkHandle? frameworkHandle)
    {
        executor?.Value.RunTests(_executorUriVsSourceList?[executorUriExtensionTuple], runContext, frameworkHandle);
    }

    /// <inheritdoc />
    protected override bool ShouldAttachDebuggerToTestHost(
        LazyExtension<ITestExecutor, ITestExecutorCapabilities> executor,
        Tuple<Uri, string> executorUriExtensionTuple,
        RunContext runContext)
    {
        // If the adapter doesn't implement the new test executor interface we should attach to
        // the default test host by default to preserve old behavior.
        return executor?.Value is not ITestExecutor2 convertedExecutor
               || convertedExecutor.ShouldAttachToTestHost(
                   _executorUriVsSourceList?[executorUriExtensionTuple],
                   runContext);
    }

    /// <summary>
    /// Returns executor Vs sources list
    /// </summary>
    private Dictionary<Tuple<Uri, string>, IEnumerable<string>> GetExecutorVsSourcesList(IMessageLogger logger)
    {
        var result = new Dictionary<Tuple<Uri, string>, IEnumerable<string>>();

        var verifiedExtensionSourceMap = new Dictionary<string, IEnumerable<string>>();

        // Validate the sources
        foreach (var kvp in _adapterSourceMap)
        {
            var verifiedSources = DiscoveryManager.GetValidSources(kvp.Value, logger, _package);
            if (verifiedSources.Any())
            {
                verifiedExtensionSourceMap.Add(kvp.Key, kvp.Value);
            }
        }

        // No valid source is found => no executor vs source map
        if (!verifiedExtensionSourceMap.Any())
        {
            return result;
        }

        foreach (var kvp in verifiedExtensionSourceMap)
        {
            var discovererToSourcesMap = DiscovererEnumerator.GetDiscovererToSourcesMap(
                kvp.Key,
                kvp.Value,
                logger,
                new AssemblyProperties());

            // Warning is logged by the inner layer
            if (discovererToSourcesMap == null || discovererToSourcesMap.Count == 0)
            {
                continue;
            }

            foreach (var discoverer in discovererToSourcesMap.Keys)
            {
                var executorUri = discoverer.Metadata.DefaultExecutorUri;
                if (executorUri == null)
                {
                    string errorMessage = string.Format(
                        CultureInfo.CurrentCulture,
                        CrossPlatEngineResources.IgnoringExecutorAsNoDefaultExecutorUriAttribute,
                        discoverer.Value);
                    logger.SendMessage(TestMessageLevel.Warning, errorMessage);
                    continue;
                }

                var executorUriExtensionTuple = new Tuple<Uri, string>(executorUri, kvp.Key);

                if (!result.ContainsKey(executorUriExtensionTuple))
                {
                    result.Add(executorUriExtensionTuple, discovererToSourcesMap[discoverer]);
                }
                else
                {
                    string errorMessage = string.Format(
                        CultureInfo.CurrentCulture,
                        CrossPlatEngineResources.DuplicateAdaptersFound,
                        executorUri,
                        discoverer.Value);
                    logger.SendMessage(TestMessageLevel.Warning, errorMessage);
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Sends Session-End event on in-proc datacollectors
    /// </summary>
    protected override void SendSessionEnd()
    {
        _testCaseEventsHandler?.SendSessionEnd();
    }

    /// <summary>
    /// Sends Session-Start event on in-proc datacollectors
    /// </summary>
    protected override void SendSessionStart()
    {
        // Send session start with test sources in property bag for session start event args.
        if (_testCaseEventsHandler == null)
        {
            return;
        }

        var properties = new Dictionary<string, object?>
        {
            { "TestSources", TestSourcesUtility.GetSources(_adapterSourceMap!)! }
        };

        _testCaseEventsHandler.SendSessionStart(properties);
    }
}
