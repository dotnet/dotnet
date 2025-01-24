// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

extern alias Abstraction;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Abstraction::Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

using Microsoft.TestPlatform.VsTestConsole.TranslationLayer;
using Microsoft.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Client;
using Microsoft.VisualStudio.TestPlatform.Client.DesignMode;
using Microsoft.VisualStudio.TestPlatform.Client.RequestHelper;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Execution;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Payloads;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine;

/// <summary>
/// The in-process wrapper.
/// </summary>
internal class InProcessVsTestConsoleWrapper : IVsTestConsoleWrapper
{
    // Must be in sync with the highest supported version in
    // src/Microsoft.TestPlatform.CrossPlatEngine/EventHandlers/TestRequestHandler.cs file.
    private readonly int _highestSupportedVersion = 6;

    private readonly ITranslationLayerRequestSender _requestSender;
    private readonly ITestPlatformEventSource _testPlatformEventSource;
    private readonly IEnvironmentVariableHelper _environmentVariableHelper;

    /// <summary>
    /// Creates a new instance of <see cref="InProcessVsTestConsoleWrapper"/>.
    /// </summary>
    /// 
    /// <param name="consoleParameters">The console parameters.</param>
    public InProcessVsTestConsoleWrapper(ConsoleParameters consoleParameters)
        : this(
              consoleParameters,
              environmentVariableHelper: new EnvironmentVariableHelper(),
              requestSender: new VsTestConsoleRequestSender(),
              testRequestManager: null,
              executor: new Executor(ConsoleOutput.Instance),
              testPlatformEventSource: TestPlatformEventSource.Instance,
              new())
    { }

    internal InProcessVsTestConsoleWrapper(
        ConsoleParameters consoleParameters,
        IEnvironmentVariableHelper environmentVariableHelper,
        ITranslationLayerRequestSender requestSender,
        ITestRequestManager? testRequestManager,
        Executor executor,
        ITestPlatformEventSource testPlatformEventSource,
        UiLanguageOverride languageOverride)
    {
        // Setting the culture specified by user here since there's no more vstest.console process
        // to set it for us. See vstest.console Main method for more info.
        languageOverride.SetCultureSpecifiedByUser();

        EqtTrace.Info("VsTestConsoleWrapper.StartSession: Starting VsTestConsoleWrapper session.");

        _environmentVariableHelper = environmentVariableHelper;
        _testPlatformEventSource = testPlatformEventSource;
        _testPlatformEventSource.TranslationLayerInitializeStart();

        // Start communication.
        _requestSender = requestSender;
        var port = _requestSender.InitializeCommunication();
        if (port <= 0)
        {
            // Close the sender as it failed to host server.
            _requestSender.Close();
            throw new TransationLayerException(Resources.Resources.ErrorHostingCommunicationChannel);
        }

        // Fill the parameters.
        consoleParameters.ParentProcessId =
#if NET6_0_OR_GREATER
            Environment.ProcessId;
#else
            System.Diagnostics.Process.GetCurrentProcess().Id;
#endif
        consoleParameters.PortNumber = port;

        // Start vstest.console.
        // Running vstest.console in process means we inherit all environment variables from the
        // process we load the wrapper into. We do not want to alter this environment since that
        // would mean we may interfer with the way the host process works. However, certain
        // alterations are desired. The solution is to pass the environment variables we get via
        // the console parameters directly to the testhost process and make sure that at least the
        // testhost environment is predictable.
        IDictionary<string, string?> environmentVariableBaseline = new Dictionary<string, string?>();
        if (consoleParameters.InheritEnvironmentVariables)
        {
            // This is needed because GetEnvironmentVariables() returns a non-generic dictionary
            // and we need to convert it to a generic dictionary for our use-case.
            foreach (DictionaryEntry? entry in _environmentVariableHelper.GetEnvironmentVariables())
            {
                environmentVariableBaseline[entry?.Key.ToString()!] = entry?.Value?.ToString();
            }
        }

        foreach (var pair in consoleParameters.EnvironmentVariables)
        {
            environmentVariableBaseline[pair.Key] = pair.Value;
        }

        ProcessHelper.ExternalEnvironmentVariables = environmentVariableBaseline;

        string someExistingFile = typeof(InProcessVsTestConsoleWrapper).Assembly.Location;
        using var manager = new VsTestConsoleProcessManager(someExistingFile);
        var args = manager.BuildArguments(consoleParameters);
        // Skip vstest.console path, we are already running in process, so it would just end up
        // being understood as test dll to run. (it is present even though we don't provide
        // dotnet path, because it is a .dll file).
        args = args.Skip(1).ToArray();

        // We standup the client, and it will allocate port as normal that we will never use.
        // This is just to avoid "duplicating" all the setup logic that is done in argument
        // processors before client processor. It created design mode client, and stores it as
        // single instance.
        // We connect back to the client but never use that connection, it only serves as
        // "await" to make sure the design mode client is already initialized.
        Task.Run<int>(() => executor.Execute(args));
        WaitForConnection();

        // Set the test request manager here.
        TestRequestManager = testRequestManager;
        if (TestRequestManager is null)
        {
            TPDebug.Assert(
                DesignModeClient.Instance != null,
                "DesignModeClient.Instance is null");
            TestRequestManager = DesignModeClient.Instance.TestRequestManager;
        }

        _testPlatformEventSource.TranslationLayerInitializeStop();
    }

    internal ITestRequestManager? TestRequestManager { get; set; }

    /// <inheritdoc/>
    public void AbortTestRun()
    {
        try
        {
            TestRequestManager?.AbortTestRun();
        }
        catch (Exception ex)
        {
            EqtTrace.Error("InProcessVsTestConsoleWrapper.AbortTestRun: Exception occurred: " + ex);
        }
    }

    /// <inheritdoc/>
    public void CancelDiscovery()
    {
        try
        {
            TestRequestManager?.CancelDiscovery();
        }
        catch (Exception ex)
        {
            EqtTrace.Error("InProcessVsTestConsoleWrapper.CancelDiscovery: Exception occurred: " + ex);
        }
    }

    /// <inheritdoc/>
    public void CancelTestRun()
    {
        try
        {
            TestRequestManager?.CancelTestRun();
        }
        catch (Exception ex)
        {
            EqtTrace.Error("InProcessVsTestConsoleWrapper.CancelTestRun: Exception occurred: " + ex);
        }
    }

    /// <inheritdoc/>
    public void EndSession()
    {
        // Session means vstest.console process in the original api
        // we don't have a process to manage, we are in-process.
    }

    /// <inheritdoc/>
    public void StartSession()
    {
        // Session means vstest.console process in the original api
        // we don't have a process to manage, we are in-process.
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public ITestSession? StartTestSession(
        IList<string> sources,
        string? runSettings,
        ITestSessionEventsHandler eventsHandler)
    {
        return StartTestSession(sources, runSettings, options: null, eventsHandler);
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public ITestSession? StartTestSession(
        IList<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        ITestSessionEventsHandler eventsHandler)
    {
        return StartTestSession(
            sources,
            runSettings,
            options,
            eventsHandler,
            testHostLauncher: null);
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public ITestSession? StartTestSession(
        IList<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        ITestSessionEventsHandler eventsHandler,
        ITestHostLauncher? testHostLauncher)
    {
        _testPlatformEventSource.TranslationLayerStartTestSessionStart();

        var resetEvent = new ManualResetEvent(false);
        TestSessionInfo? testSessionInfo = null;

        try
        {
            TestRequestManager?.ResetOptions();
            var startTestSessionPayload = new StartTestSessionPayload()
            {
                Sources = sources,
                RunSettings = runSettings,
                HasCustomHostLauncher = testHostLauncher != null,
                IsDebuggingEnabled = testHostLauncher != null
                                     && testHostLauncher.IsDebug,
                TestPlatformOptions = options
            };

            var inProcessEventsHandler = new InProcessTestSessionEventsHandler(eventsHandler);
            inProcessEventsHandler.StartTestSessionCompleteEventHandler += (_, eventArgs) =>
            {
                testSessionInfo = eventArgs?.TestSessionInfo;
                resetEvent.Set();
            };

            TestRequestManager?.StartTestSession(
                startTestSessionPayload,
                (ITestHostLauncher3?)testHostLauncher,
                inProcessEventsHandler,
                new ProtocolConfig { Version = _highestSupportedVersion });

            var timeout = EnvironmentHelper.GetConnectionTimeout() * 1000;
            if (!resetEvent.WaitOne(timeout))
            {
                throw new TransationLayerException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.Resources.StartTestSessionTimedOut,
                        timeout));
            }

            inProcessEventsHandler.StartTestSessionCompleteEventHandler = null;
        }
        catch (Exception ex)
        {
            EqtTrace.Error("InProcessVsTestConsoleWrapper.StartTestSession: Exception occurred: " + ex);

            eventsHandler.HandleLogMessage(TestMessageLevel.Error, ex.ToString());
            eventsHandler.HandleStartTestSessionComplete(new());
        }

        _testPlatformEventSource.TranslationLayerStartTestSessionStop();

        return new TestSession(testSessionInfo, eventsHandler, this);
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public bool StopTestSession(
        TestSessionInfo? testSessionInfo,
        ITestSessionEventsHandler eventsHandler)
    {
        return StopTestSession(testSessionInfo, options: null, eventsHandler);
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public bool StopTestSession(
        TestSessionInfo? testSessionInfo,
        TestPlatformOptions? options,
        ITestSessionEventsHandler eventsHandler)
    {
        _testPlatformEventSource.TranslationLayerStopTestSessionStart();

        var isStopped = false;
        var resetEvent = new ManualResetEvent(false);

        try
        {
            TestRequestManager?.ResetOptions();
            var stopTestSessionPayload = new StopTestSessionPayload()
            {
                TestSessionInfo = testSessionInfo,
                CollectMetrics = options?.CollectMetrics ?? false
            };

            var inProcessEventsHandler = new InProcessTestSessionEventsHandler(eventsHandler);
            inProcessEventsHandler.StopTestSessionCompleteEventHandler += (_, eventArgs) =>
            {
                isStopped = (eventArgs?.IsStopped == true);
                resetEvent.Set();
            };

            TestRequestManager?.StopTestSession(
                stopTestSessionPayload,
                inProcessEventsHandler,
                new ProtocolConfig { Version = _highestSupportedVersion });

            var timeout = EnvironmentHelper.GetConnectionTimeout() * 1000;
            if (!resetEvent.WaitOne(timeout))
            {
                throw new TransationLayerException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.Resources.StopTestSessionTimedOut,
                        timeout));
            }

            inProcessEventsHandler.StopTestSessionCompleteEventHandler = null;
        }
        catch (Exception ex)
        {
            EqtTrace.Error("InProcessVsTestConsoleWrapper.StopTestSession: Exception occurred: " + ex);

            eventsHandler.HandleLogMessage(TestMessageLevel.Error, ex.ToString());
            eventsHandler.HandleStopTestSessionComplete(new());
        }

        _testPlatformEventSource.TranslationLayerStopTestSessionStop();

        return isStopped;
    }

    /// <inheritdoc/>
    public void InitializeExtensions(IEnumerable<string> pathToAdditionalExtensions)
    {
        try
        {
            TestRequestManager?.InitializeExtensions(
                pathToAdditionalExtensions,
                skipExtensionFilters: true);
        }
        catch (Exception ex)
        {
            EqtTrace.Error("InProcessVsTestConsoleWrapper.InitializeExtensions: Exception occurred: " + ex);
        }
    }

    /// <inheritdoc/>
    public void DiscoverTests(
        IEnumerable<string> sources,
        string? discoverySettings,
        ITestDiscoveryEventsHandler discoveryEventsHandler)
    {
        DiscoverTests(
            sources,
            discoverySettings,
            options: null,
            new DiscoveryEventsHandleConverter(discoveryEventsHandler));
    }

    /// <inheritdoc/>
    public void DiscoverTests(
        IEnumerable<string> sources,
        string? discoverySettings,
        TestPlatformOptions? options,
        ITestDiscoveryEventsHandler2 discoveryEventsHandler)
    {
        DiscoverTests(
            sources,
            discoverySettings,
            options,
            testSessionInfo: null,
            discoveryEventsHandler);
    }

    /// <inheritdoc/>
    public void DiscoverTests(
        IEnumerable<string> sources,
        string? discoverySettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestDiscoveryEventsHandler2 discoveryEventsHandler)
    {
        _testPlatformEventSource.TranslationLayerDiscoveryStart();

        try
        {
            TestRequestManager?.ResetOptions();
            var discoveryRequestPayload = new DiscoveryRequestPayload()
            {
                Sources = sources,
                RunSettings = discoverySettings,
                TestPlatformOptions = options,
                TestSessionInfo = testSessionInfo
            };

            TestRequestManager?.DiscoverTests(
                discoveryRequestPayload,
                new DiscoveryHandlerToEventsRegistrarAdapter(discoveryEventsHandler),
                new ProtocolConfig { Version = _highestSupportedVersion });
        }
        catch (Exception ex)
        {
            EqtTrace.Error("InProcessVsTestConsoleWrapper.DiscoverTests: Exception occurred: " + ex);

            discoveryEventsHandler.HandleLogMessage(TestMessageLevel.Error, ex.ToString());
            var errorDiscoveryComplete = new DiscoveryCompleteEventArgs
            {
                IsAborted = true,
                TotalCount = -1,
            };
            discoveryEventsHandler.HandleDiscoveryComplete(
                errorDiscoveryComplete,
                lastChunk: null);
        }

        _testPlatformEventSource.TranslationLayerDiscoveryStop();
    }

    /// <inheritdoc/>
    public void RunTests(
        IEnumerable<string> sources,
        string? runSettings,
        ITestRunEventsHandler testRunEventsHandler)
    {
        RunTests(
            sources,
            runSettings,
            options: null,
            testRunEventsHandler);
    }

    /// <inheritdoc/>
    public void RunTests(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        ITestRunEventsHandler testRunEventsHandler)
    {
        RunTests(
            sources,
            runSettings,
            options,
            testSessionInfo: null,
            testRunEventsHandler);
    }

    /// <inheritdoc/>
    public void RunTests(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler)
    {
        var sourceList = sources.ToList();
        _testPlatformEventSource.TranslationLayerExecutionStart(
            0,
            0,
            sourceList.Count,
            runSettings ?? string.Empty);

        try
        {
            TestRequestManager?.ResetOptions();

            var testRunPayload = new TestRunRequestPayload
            {
                Sources = sourceList,
                RunSettings = runSettings,
                TestPlatformOptions = options,
                TestSessionInfo = testSessionInfo
            };

            TestRequestManager?.RunTests(
                testRunPayload,
                customTestHostLauncher: null,
                new RunHandlerToEventsRegistrarAdapter(testRunEventsHandler),
                new ProtocolConfig { Version = _highestSupportedVersion });
        }
        catch (Exception ex)
        {
            EqtTrace.Error("InProcessVsTestConsoleWrapper.RunTests: Exception occurred: " + ex);
            var testRunCompleteArgs = new TestRunCompleteEventArgs(null, false, true, ex, null, null, TimeSpan.MinValue);

            testRunEventsHandler.HandleLogMessage(TestMessageLevel.Error, ex.ToString());
            testRunEventsHandler.HandleTestRunComplete(testRunCompleteArgs, null, null, null);
        }

        _testPlatformEventSource.TranslationLayerExecutionStop();
    }

    /// <inheritdoc/>
    public void RunTests(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler)
    {
        RunTests(
            sources,
            runSettings,
            options,
            testSessionInfo,
            testRunEventsHandler);
    }

    /// <inheritdoc/>
    public void RunTests(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        ITestRunEventsHandler testRunEventsHandler)
    {
        RunTests(
            testCases,
            runSettings,
            options: null,
            testRunEventsHandler);
    }

    /// <inheritdoc/>
    public void RunTests(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        ITestRunEventsHandler testRunEventsHandler)
    {
        RunTests(
            testCases,
            runSettings,
            options,
            testSessionInfo: null,
            testRunEventsHandler);
    }

    /// <inheritdoc/>
    public void RunTests(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler)
    {
        var testCaseList = testCases.ToList();
        _testPlatformEventSource.TranslationLayerExecutionStart(
            0,
            0,
            testCaseList.Count,
            runSettings ?? string.Empty);

        try
        {
            TestRequestManager?.ResetOptions();

            var testRunPayload = new TestRunRequestPayload
            {
                TestCases = testCaseList,
                RunSettings = runSettings,
                TestPlatformOptions = options,
                TestSessionInfo = testSessionInfo
            };

            TestRequestManager?.RunTests(
                testRunPayload,
                customTestHostLauncher: null,
                new RunHandlerToEventsRegistrarAdapter(testRunEventsHandler),
                new ProtocolConfig { Version = _highestSupportedVersion });
        }
        catch (Exception ex)
        {
            EqtTrace.Error("InProcessVsTestConsoleWrapper.RunTests: Exception occurred: " + ex);
            var testRunCompleteArgs = new TestRunCompleteEventArgs(null, false, true, ex, null, null, TimeSpan.MinValue);

            testRunEventsHandler.HandleLogMessage(TestMessageLevel.Error, ex.ToString());
            testRunEventsHandler.HandleTestRunComplete(testRunCompleteArgs, null, null, null);
        }

        _testPlatformEventSource.TranslationLayerExecutionStop();
    }

    /// <inheritdoc/>
    public void RunTests(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler)
    {
        RunTests(
            testCases,
            runSettings,
            options,
            testSessionInfo,
            testRunEventsHandler);
    }

    /// <inheritdoc/>
    public void RunTestsWithCustomTestHost(
        IEnumerable<string> sources,
        string? runSettings,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher? customTestHostLauncher)
    {
        RunTestsWithCustomTestHost(
            sources,
            runSettings,
            options: null,
            testRunEventsHandler,
            customTestHostLauncher);
    }

    /// <inheritdoc/>
    public void RunTestsWithCustomTestHost(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher? customTestHostLauncher)
    {
        RunTestsWithCustomTestHost(
            sources,
            runSettings,
            options,
            testSessionInfo: null,
            testRunEventsHandler,
            customTestHostLauncher);
    }

    /// <inheritdoc/>
    public void RunTestsWithCustomTestHost(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher? customTestHostLauncher)
    {
        var sourceList = sources.ToList();
        _testPlatformEventSource.TranslationLayerExecutionStart(
            1,
            sourceList.Count,
            0,
            runSettings ?? string.Empty);

        try
        {
            TestRequestManager?.ResetOptions();

            // We must avoid re-launching the test host if the test run payload already
            // contains test session info. Test session info being present is an indicative
            // of an already running test host spawned by a start test session call.
            var customLauncher =
                testSessionInfo is null
                    ? customTestHostLauncher
                    : null;

            var testRunPayload = new TestRunRequestPayload
            {
                Sources = sourceList,
                RunSettings = runSettings,
                DebuggingEnabled = customLauncher?.IsDebug == true,
                TestPlatformOptions = options,
                TestSessionInfo = testSessionInfo
            };

            // TODO: this will need to be via an adapter instead. Because we will get a "live"
            // implementation rather than our own implementation that we normally have in design
            // mode client. It probably will even throw on this cast.
            var upcastCustomLauncher = (ITestHostLauncher3?)customLauncher;

            TestRequestManager?.RunTests(
                testRunPayload,
                upcastCustomLauncher,
                new RunHandlerToEventsRegistrarAdapter(testRunEventsHandler),
                new ProtocolConfig { Version = _highestSupportedVersion });
        }
        catch (Exception ex)
        {
            EqtTrace.Error("InProcessVsTestConsoleWrapper.RunTestsWithCustomTestHost: Exception occurred: " + ex);
            var testRunCompleteArgs = new TestRunCompleteEventArgs(null, false, true, ex, null, null, TimeSpan.MinValue);

            testRunEventsHandler.HandleLogMessage(TestMessageLevel.Error, ex.ToString());
            testRunEventsHandler.HandleTestRunComplete(testRunCompleteArgs, null, null, null);
        }

        _testPlatformEventSource.TranslationLayerExecutionStop();
    }

    /// <inheritdoc/>
    public void RunTestsWithCustomTestHost(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler,
        ITestHostLauncher? customTestHostLauncher)
    {
        RunTestsWithCustomTestHost(
            sources,
            runSettings,
            options,
            testSessionInfo,
            testRunEventsHandler,
            customTestHostLauncher);
    }

    /// <inheritdoc/>
    public void RunTestsWithCustomTestHost(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher? customTestHostLauncher)
    {
        RunTestsWithCustomTestHost(
            testCases,
            runSettings,
            options: null,
            testRunEventsHandler,
            customTestHostLauncher);
    }

    /// <inheritdoc/>
    public void RunTestsWithCustomTestHost(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher? customTestHostLauncher)
    {
        RunTestsWithCustomTestHost(
            testCases,
            runSettings,
            options,
            testSessionInfo: null,
            testRunEventsHandler,
            customTestHostLauncher);
    }

    /// <inheritdoc/>
    public void RunTestsWithCustomTestHost(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher? customTestHostLauncher)
    {
        var testCaseList = testCases.ToList();
        _testPlatformEventSource.TranslationLayerExecutionStart(
            1,
            0,
            testCaseList.Count,
            runSettings ?? string.Empty);

        try
        {
            TestRequestManager?.ResetOptions();

            // We must avoid re-launching the test host if the test run payload already
            // contains test session info. Test session info being present is an indicative
            // of an already running test host spawned by a start test session call.
            var customLauncher =
                testSessionInfo is null
                    ? customTestHostLauncher
                    : null;

            var testRunPayload = new TestRunRequestPayload
            {
                TestCases = testCaseList,
                RunSettings = runSettings,
                DebuggingEnabled = customLauncher?.IsDebug == true,
                TestPlatformOptions = options,
                TestSessionInfo = testSessionInfo
            };

            // TODO: this will need to be via an adapter instead. Because we will get a "live"
            // implementation rather than our own implementation that we normally have in design
            // mode client. It probably will even throw on this cast.
            var upcastCustomLauncher = (ITestHostLauncher3?)customLauncher;

            TestRequestManager?.RunTests(
                testRunPayload,
                upcastCustomLauncher,
                new RunHandlerToEventsRegistrarAdapter(testRunEventsHandler),
                new ProtocolConfig { Version = _highestSupportedVersion });
        }
        catch (Exception ex)
        {
            EqtTrace.Error("InProcessVsTestConsoleWrapper.RunTestsWithCustomTestHost: Exception occurred: " + ex);
            var testRunCompleteArgs = new TestRunCompleteEventArgs(null, false, true, ex, null, null, TimeSpan.MinValue);

            testRunEventsHandler.HandleLogMessage(TestMessageLevel.Error, ex.ToString());
            testRunEventsHandler.HandleTestRunComplete(testRunCompleteArgs, null, null, null);
        }

        _testPlatformEventSource.TranslationLayerExecutionStop();
    }

    /// <inheritdoc/>
    public void RunTestsWithCustomTestHost(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler,
        ITestHostLauncher? customTestHostLauncher)
    {
        RunTestsWithCustomTestHost(
            testCases,
            runSettings,
            options,
            testSessionInfo,
            testRunEventsHandler,
            customTestHostLauncher);
    }

    #region Async, not implemented
    /// <inheritdoc/>
    public async Task DiscoverTestsAsync(
        IEnumerable<string> sources,
        string? discoverySettings,
        ITestDiscoveryEventsHandler discoveryEventsHandler)
    {
        await DiscoverTestsAsync(
                sources,
                discoverySettings,
                options: null,
                new DiscoveryEventsHandleConverter(discoveryEventsHandler))
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task DiscoverTestsAsync(
        IEnumerable<string> sources,
        string? discoverySettings,
        TestPlatformOptions? options,
        ITestDiscoveryEventsHandler2 discoveryEventsHandler)
    {
        await DiscoverTestsAsync(
                sources,
                discoverySettings,
                options,
                testSessionInfo: null,
                discoveryEventsHandler)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public Task DiscoverTestsAsync(
        IEnumerable<string> sources,
        string? discoverySettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestDiscoveryEventsHandler2 discoveryEventsHandler)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task InitializeExtensionsAsync(IEnumerable<string> pathToAdditionalExtensions)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task ProcessTestRunAttachmentsAsync(
        IEnumerable<AttachmentSet> attachments,
        string? processingSettings,
        bool isLastBatch,
        bool collectMetrics,
        ITestRunAttachmentsProcessingEventsHandler eventsHandler,
        CancellationToken cancellationToken)
    {
        await ProcessTestRunAttachmentsAsync(
                attachments,
                invokedDataCollectors: null,
                processingSettings,
                isLastBatch,
                collectMetrics,
                eventsHandler,
                cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task ProcessTestRunAttachmentsAsync(
        IEnumerable<AttachmentSet> attachments,
        IEnumerable<InvokedDataCollector>? invokedDataCollectors,
        string? processingSettings,
        bool isLastBatch,
        bool collectMetrics,
        ITestRunAttachmentsProcessingEventsHandler eventsHandler,
        CancellationToken cancellationToken)
    {
        _testPlatformEventSource.TranslationLayerTestRunAttachmentsProcessingStart();

        try
        {
            var attachmentProcessingPayload = new TestRunAttachmentsProcessingPayload
            {
                Attachments = attachments,
                InvokedDataCollectors = invokedDataCollectors,
                RunSettings = processingSettings,
                CollectMetrics = collectMetrics
            };

            using (cancellationToken.Register(() =>
                TestRequestManager?.CancelTestRunAttachmentsProcessing()))
            {
                // Awaiting the attachment processing task here. The implementation of the
                // underlying operation guarantees the event handler is called when processing
                // is complete, so when awaiting ends, the results have already been passed to
                // the caller via the event handler. No need for further synchronization.
                //
                // NOTE: We're passing in CancellationToken.None and that is by design, *DO NOT*
                // attempt to optimize this code by passing in the cancellation token registered
                // above. Passing in the said token would result in potentially leaving the caller
                // hanging when the token is signaled before even starting the test run attachment
                // processing. In this scenario, the Task.Run should not even run in the first
                // place, and as such the event handler that signals processing is complete will
                // not be triggered anymore.
                await Task.Run(() =>
                        TestRequestManager?.ProcessTestRunAttachments(
                            attachmentProcessingPayload,
                            new InProcessTestRunAttachmentsProcessingEventsHandler(eventsHandler),
                            new ProtocolConfig { Version = _highestSupportedVersion }),
                        CancellationToken.None)
                    .ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            EqtTrace.Error("InProcessVsTestConsoleWrapper.ProcessTestRunAttachmentsAsync: Exception occurred: " + ex);

            var attachmentsProcessingArgs = new TestRunAttachmentsProcessingCompleteEventArgs(
                isCanceled: cancellationToken.IsCancellationRequested,
                ex);

            eventsHandler.HandleLogMessage(TestMessageLevel.Error, ex.ToString());
            eventsHandler.HandleTestRunAttachmentsProcessingComplete(attachmentsProcessingArgs, lastChunk: null);
        }

        _testPlatformEventSource.TranslationLayerTestRunAttachmentsProcessingStop();
    }

    /// <inheritdoc/>
    public async Task RunTestsAsync(
        IEnumerable<string> sources,
        string? runSettings,
        ITestRunEventsHandler testRunEventsHandler)
    {
        await RunTestsAsync(
                sources,
                runSettings,
                options: null,
                testRunEventsHandler)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task RunTestsAsync(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        ITestRunEventsHandler testRunEventsHandler)
    {
        await RunTestsAsync(
                sources,
                runSettings,
                options,
                testSessionInfo: null,
                testRunEventsHandler)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public Task RunTestsAsync(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task RunTestsAsync(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task RunTestsAsync(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        ITestRunEventsHandler testRunEventsHandler)
    {
        await RunTestsAsync(
                testCases,
                runSettings,
                options: null,
                testRunEventsHandler)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task RunTestsAsync(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        ITestRunEventsHandler testRunEventsHandler)
    {
        await RunTestsAsync(
                testCases,
                runSettings,
                options,
                testSessionInfo: null,
                testRunEventsHandler)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public Task RunTestsAsync(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task RunTestsAsync(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task RunTestsWithCustomTestHostAsync(
        IEnumerable<string> sources,
        string? runSettings,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher customTestHostLauncher)
    {
        await RunTestsWithCustomTestHostAsync(
                sources,
                runSettings,
                options: null,
                testRunEventsHandler,
                customTestHostLauncher)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task RunTestsWithCustomTestHostAsync(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher customTestHostLauncher)
    {
        await RunTestsWithCustomTestHostAsync(
                sources,
                runSettings,
                options,
                testSessionInfo: null,
                testRunEventsHandler,
                customTestHostLauncher)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public Task RunTestsWithCustomTestHostAsync(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher customTestHostLauncher)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task RunTestsWithCustomTestHostAsync(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler,
        ITestHostLauncher customTestHostLauncher)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task RunTestsWithCustomTestHostAsync(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher customTestHostLauncher)
    {
        await RunTestsWithCustomTestHostAsync(
                testCases,
                runSettings,
                options: null,
                testRunEventsHandler,
                customTestHostLauncher)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task RunTestsWithCustomTestHostAsync(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher customTestHostLauncher)
    {
        await RunTestsWithCustomTestHostAsync(
                testCases,
                runSettings,
                options,
                testSessionInfo: null,
                testRunEventsHandler,
                customTestHostLauncher)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public Task RunTestsWithCustomTestHostAsync(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher customTestHostLauncher)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task RunTestsWithCustomTestHostAsync(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler,
        ITestHostLauncher customTestHostLauncher)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task StartSessionAsync()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public async Task<ITestSession?> StartTestSessionAsync(
        IList<string> sources,
        string? runSettings,
        ITestSessionEventsHandler eventsHandler)
    {
        return await StartTestSessionAsync(
                sources,
                runSettings,
                options: null,
                eventsHandler)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public async Task<ITestSession?> StartTestSessionAsync(
        IList<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        ITestSessionEventsHandler eventsHandler)
    {
        return await StartTestSessionAsync(
                sources,
                runSettings,
                options,
                eventsHandler,
                testHostLauncher: null)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public Task<ITestSession?> StartTestSessionAsync(
        IList<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        ITestSessionEventsHandler eventsHandler,
        ITestHostLauncher? testHostLauncher)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public async Task<bool> StopTestSessionAsync(
        TestSessionInfo? testSessionInfo,
        ITestSessionEventsHandler eventsHandler)
    {
        return await StopTestSessionAsync(
                testSessionInfo,
                options: null,
                eventsHandler)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public Task<bool> StopTestSessionAsync(
        TestSessionInfo? testSessionInfo,
        TestPlatformOptions? options,
        ITestSessionEventsHandler eventsHandler)
    {
        throw new NotImplementedException();
    }
    #endregion

    private bool WaitForConnection()
    {
        EqtTrace.Info("InProcessVsTestConsoleWrapper.WaitForConnection: Waiting for connection to command line runner.");

        var timeout = EnvironmentHelper.GetConnectionTimeout() * 1000;
        if (!_requestSender.WaitForRequestHandlerConnection(timeout))
        {
            throw new TransationLayerException(
                string.Format(
                    CultureInfo.CurrentCulture,
                    Resources.Resources.RequestHandlerConnectionTimedOut,
                    timeout));
        }

        _testPlatformEventSource.TranslationLayerInitializeStop();
        return true;
    }
}
