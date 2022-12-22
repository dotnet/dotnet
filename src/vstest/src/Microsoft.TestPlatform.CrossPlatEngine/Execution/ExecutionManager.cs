// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.Common.Logging;
using Microsoft.VisualStudio.TestPlatform.Common.SettingsProvider;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.ClientProtocol;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.TesthostProtocol;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Execution;

/// <summary>
/// Orchestrates test execution related functionality for the engine communicating with the test host process.
/// </summary>
public class ExecutionManager : IExecutionManager
{
    private readonly ITestPlatformEventSource _testPlatformEventSource;
    private readonly IRequestData _requestData;
    private readonly TestSessionMessageLogger? _sessionMessageLogger;
    private BaseRunTests? _activeTestRun;
    private ITestMessageEventHandler? _testMessageEventsHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExecutionManager"/> class.
    /// </summary>
    public ExecutionManager(IRequestData requestData)
        : this(TestPlatformEventSource.Instance, requestData)
    {
        _sessionMessageLogger = TestSessionMessageLogger.Instance;
        _sessionMessageLogger.TestRunMessage += TestSessionMessageHandler;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExecutionManager"/> class.
    /// </summary>
    /// <param name="testPlatformEventSource">Test platform event source.</param>
    protected ExecutionManager(ITestPlatformEventSource testPlatformEventSource, IRequestData requestData)
    {
        _testPlatformEventSource = testPlatformEventSource;
        _requestData = requestData ?? throw new ArgumentNullException(nameof(requestData));
    }

    #region IExecutionManager Implementation

    /// <summary>
    /// Initializes the execution manager.
    /// </summary>
    /// <param name="pathToAdditionalExtensions"> The path to additional extensions. </param>
    public void Initialize(IEnumerable<string>? pathToAdditionalExtensions, ITestMessageEventHandler? testMessageEventsHandler)
    {
        // Clear the request data metrics left over from a potential previous run.
        _requestData.MetricsCollection?.Metrics?.Clear();

        _testMessageEventsHandler = testMessageEventsHandler;
        _testPlatformEventSource.AdapterSearchStart();

        if (pathToAdditionalExtensions != null && pathToAdditionalExtensions.Any())
        {
            // Start using these additional extensions
            TestPluginCache.Instance.DefaultExtensionPaths = pathToAdditionalExtensions;
        }

        LoadExtensions();

        //unsubscribe session logger
        if (_sessionMessageLogger is not null)
        {
            _sessionMessageLogger.TestRunMessage -= TestSessionMessageHandler;
        }

        _testPlatformEventSource.AdapterSearchStop();
    }

    /// <summary>
    /// Starts the test run
    /// </summary>
    /// <param name="adapterSourceMap"> The adapter Source Map.  </param>
    /// <param name="package">The user input test source(package) if it differ from actual test source otherwise null.</param>
    /// <param name="runSettings"> The run Settings.  </param>
    /// <param name="testExecutionContext"> The test Execution Context. </param>
    /// <param name="testCaseEventsHandler"> EventHandler for handling test cases level events from Engine. </param>
    /// <param name="runEventsHandler"> EventHandler for handling execution events from Engine.  </param>
    public void StartTestRun(
        Dictionary<string, IEnumerable<string>> adapterSourceMap,
        string? package,
        string? runSettings,
        TestExecutionContext testExecutionContext,
        ITestCaseEventsHandler? testCaseEventsHandler,
        IInternalTestRunEventsHandler runEventsHandler)
    {
        try
        {
            InitializeDataCollectors(runSettings, testCaseEventsHandler as ITestEventsPublisher, TestSourcesUtility.GetDefaultCodebasePath(adapterSourceMap!));

            _activeTestRun = new RunTestsWithSources(_requestData, adapterSourceMap, package, runSettings, testExecutionContext, testCaseEventsHandler, runEventsHandler);
            _activeTestRun.RunTests();
        }
        catch (Exception e)
        {
            runEventsHandler.HandleLogMessage(TestMessageLevel.Error, e.ToString());
            Abort(runEventsHandler);
        }
        finally
        {
            _activeTestRun = null;
        }
    }

    /// <summary>
    /// Starts the test run with tests.
    /// </summary>
    /// <param name="tests"> The test list. </param>
    /// <param name="package">The user input test source(package) if it differ from actual test source otherwise null.</param>
    /// <param name="runSettings"> The run Settings.  </param>
    /// <param name="testExecutionContext"> The test Execution Context. </param>
    /// <param name="testCaseEventsHandler"> EventHandler for handling test cases level events from Engine. </param>
    /// <param name="runEventsHandler"> EventHandler for handling execution events from Engine. </param>
    public void StartTestRun(
        IEnumerable<TestCase> tests,
        string? package,
        string? runSettings,
        TestExecutionContext testExecutionContext,
        ITestCaseEventsHandler? testCaseEventsHandler,
        IInternalTestRunEventsHandler runEventsHandler)
    {
        try
        {
            InitializeDataCollectors(runSettings, testCaseEventsHandler as ITestEventsPublisher, TestSourcesUtility.GetDefaultCodebasePath(tests));

            _activeTestRun = new RunTestsWithTests(_requestData, tests, package, runSettings, testExecutionContext, testCaseEventsHandler, runEventsHandler);
            _activeTestRun.RunTests();
        }
        catch (Exception e)
        {
            runEventsHandler.HandleLogMessage(TestMessageLevel.Error, e.ToString());
            Abort(runEventsHandler);
        }
        finally
        {
            _activeTestRun = null;
        }
    }

    /// <summary>
    /// Cancel the test execution.
    /// </summary>
    public void Cancel(IInternalTestRunEventsHandler testRunEventsHandler)
    {
        if (_activeTestRun == null)
        {
            var testRunCompleteEventArgs = new TestRunCompleteEventArgs(null, true, false, null, null, null, TimeSpan.Zero);
            testRunEventsHandler.HandleTestRunComplete(testRunCompleteEventArgs, null, null, null);
        }
        else
        {
            _activeTestRun.Cancel();
        }
    }

    /// <summary>
    /// Aborts the test execution.
    /// </summary>
    public void Abort(IInternalTestRunEventsHandler testRunEventsHandler)
    {
        if (_activeTestRun == null)
        {
            var testRunCompleteEventArgs = new TestRunCompleteEventArgs(null, false, true, null, null, null, TimeSpan.Zero);
            testRunEventsHandler.HandleTestRunComplete(testRunCompleteEventArgs, null, null, null);
        }
        else
        {
            _activeTestRun.Abort();
        }
    }

    #endregion
    private static void LoadExtensions()
    {
        try
        {
            // Load the extensions on creation so that we dont have to spend time during first execution.
            EqtTrace.Verbose("TestExecutorService: Loading the extensions");

            TestExecutorExtensionManager.LoadAndInitializeAllExtensions(false);

            EqtTrace.Verbose("TestExecutorService: Loaded the executors");

            SettingsProviderExtensionManager.LoadAndInitializeAllExtensions(false);

            EqtTrace.Verbose("TestExecutorService: Loaded the settings providers");
            EqtTrace.Info("TestExecutorService: Loaded the extensions");
        }
        catch (Exception ex)
        {
            EqtTrace.Warning("TestExecutorWebService: Exception occurred while calling test connection. {0}", ex);
        }
    }

    /// <summary>
    /// Initializes out-proc and in-proc data collectors.
    /// </summary>
    private static void InitializeDataCollectors(string? runSettings, ITestEventsPublisher? testEventsPublisher, string? defaultCodeBase)
    {
        // Initialize out-proc data collectors if declared in run settings.
        if (DataCollectionTestCaseEventSender.Instance != null && XmlRunSettingsUtilities.IsDataCollectionEnabled(runSettings))
        {
            TPDebug.Assert(testEventsPublisher is not null, "testEventsPublisher is null");
            _ = new ProxyOutOfProcDataCollectionManager(DataCollectionTestCaseEventSender.Instance, testEventsPublisher);
        }

        // Initialize in-proc data collectors if declared in run settings.
        if (XmlRunSettingsUtilities.IsInProcDataCollectionEnabled(runSettings))
        {
            TPDebug.Assert(testEventsPublisher is not null, "testEventsPublisher is null");
            _ = new InProcDataCollectionExtensionManager(runSettings, testEventsPublisher, defaultCodeBase, TestPluginCache.Instance);
        }
    }

    private void TestSessionMessageHandler(object? sender, TestRunMessageEventArgs e)
    {
        if (_testMessageEventsHandler != null)
        {
            _testMessageEventsHandler.HandleLogMessage(e.Level, e.Message);
        }
        else
        {
            EqtTrace.Warning(
                "ExecutionManager: Could not pass the log message  '{0}' as the callback is null.",
                e.Message);
        }
    }

}
