// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.ClientProtocol;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.TesthostProtocol;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client;

internal class InProcessProxyExecutionManager : IProxyExecutionManager
{
    private readonly ITestHostManagerFactory _testHostManagerFactory;
    private readonly IExecutionManager _executionManager;
    private readonly ITestRuntimeProvider _testHostManager;

    public bool IsInitialized { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InProcessProxyexecutionManager"/> class.
    /// </summary>
    /// <param name="testHostManager">
    /// The test Host Manager.
    /// </param>
    /// <param name="testHostManagerFactory">
    /// Manager factory
    /// </param>
    public InProcessProxyExecutionManager(ITestRuntimeProvider testHostManager, ITestHostManagerFactory testHostManagerFactory)
    {
        _testHostManager = testHostManager;
        _testHostManagerFactory = testHostManagerFactory;
        _executionManager = _testHostManagerFactory.GetExecutionManager();
    }

    /// <summary>
    /// Initialize adapters.
    /// <param name="skipDefaultAdapters">Skip default adapters flag.</param>
    /// </summary>
    public void Initialize(bool skipDefaultAdapters)
    {
    }

    /// <inheritdoc/>
    public int StartTestRun(TestRunCriteria testRunCriteria, IInternalTestRunEventsHandler eventHandler)
    {
        try
        {
            var runConfiguration = XmlRunSettingsUtilities.GetRunConfigurationNode(testRunCriteria.TestRunSettings);
            var testPackages = new List<string>(testRunCriteria.HasSpecificSources ? testRunCriteria.Sources :
                // If the test execution is with a test filter, group them by sources
                testRunCriteria.Tests!.GroupBy(tc => tc.Source).Select(g => g.Key));

            // This code should be in sync with ProxyExecutionManager.StartTestRun executionContext
            var executionContext = new TestExecutionContext(
                testRunCriteria.FrequencyOfRunStatsChangeEvent,
                testRunCriteria.RunStatsChangeEventTimeout,
                inIsolation: runConfiguration.InIsolation,
                keepAlive: testRunCriteria.KeepAlive,
                isDataCollectionEnabled: false,
                areTestCaseLevelEventsRequired: false,
                hasTestRun: true,
                isDebug: (testRunCriteria.TestHostLauncher != null && testRunCriteria.TestHostLauncher.IsDebug),
                testCaseFilter: testRunCriteria.TestCaseFilter,
                filterOptions: testRunCriteria.FilterOptions);

            // Initialize extension before execution
            InitializeExtensions(testPackages);

            if (testRunCriteria.HasSpecificSources)
            {
                var runRequest = testRunCriteria.CreateTestRunCriteriaForSources(_testHostManager, testRunCriteria.TestRunSettings, executionContext, testPackages);

                Task.Run(() => _executionManager.StartTestRun(runRequest.AdapterSourceMap, runRequest.Package,
                    runRequest.RunSettings, runRequest.TestExecutionContext, null, eventHandler));
            }
            else
            {
                var runRequest = testRunCriteria.CreateTestRunCriteriaForTests(_testHostManager, testRunCriteria.TestRunSettings, executionContext, testPackages);

                Task.Run(() => _executionManager.StartTestRun(runRequest.Tests, runRequest.Package,
                    runRequest.RunSettings, runRequest.TestExecutionContext, null, eventHandler));
            }
        }
        catch (Exception exception)
        {
            EqtTrace.Error("InProcessProxyexecutionManager.StartTestRun: Failed to start test run: {0}", exception);

            // Send exception message.
            eventHandler.HandleLogMessage(TestMessageLevel.Error, exception.ToString());

            // Send a run complete to caller.
            var completeArgs = new TestRunCompleteEventArgs(null, false, true, exception, new Collection<AttachmentSet>(), new Collection<InvokedDataCollector>(), TimeSpan.Zero);
            eventHandler.HandleTestRunComplete(completeArgs, null, null, null);
        }

        return 0;
    }

    /// <summary>
    /// Aborts the test operation.
    /// </summary>
    /// <param name="eventHandler"> EventHandler for handling execution events from Engine. </param>
    public void Abort(IInternalTestRunEventsHandler eventHandler)
    {
        Task.Run(() => _testHostManagerFactory.GetExecutionManager().Abort(eventHandler));
    }

    /// <summary>
    /// Cancels the test run.
    /// </summary>
    /// <param name="eventHandler"> EventHandler for handling execution events from Engine. </param>
    public void Cancel(IInternalTestRunEventsHandler eventHandler)
    {
        Task.Run(() => _testHostManagerFactory.GetExecutionManager().Cancel(eventHandler));
    }

    /// <summary>
    /// Closes the current test operation.
    /// This function is of no use in this context as we are not creating any testhost
    /// </summary>
    public void Close()
    {
    }

    private void InitializeExtensions(IEnumerable<string> sources)
    {
        var extensionsFromSource = _testHostManager.GetTestPlatformExtensions(sources, Enumerable.Empty<string>());
        if (extensionsFromSource.Any())
        {
            TestPluginCache.Instance.UpdateExtensions(extensionsFromSource, false);
        }

        // We don't need to pass list of extension as we are running inside vstest.console and
        // it will use TestPluginCache of vstest.console
        _executionManager.Initialize(Enumerable.Empty<string>(), null);
    }

    public void InitializeTestRun(TestRunCriteria testRunCriteria, IInternalTestRunEventsHandler eventHandler)
    {
        // Leaving this empty as it is not really relevant to the in-process proxy managers since
        // there's no external testhost to be started. The idea of pre-initializing the test run
        // makes sense only for out-of-process proxies like ProxyExecutionManager or
        // ProxyDiscoveryManager.
    }
}
