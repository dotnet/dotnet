// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
#if !NET5_0_OR_GREATER
using System.Diagnostics;
#endif
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestPlatform.VsTestConsole.TranslationLayer;
using Microsoft.VisualStudio.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;

using CommunicationUtilitiesResources = Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources.Resources;
using CoreUtilitiesConstants = Microsoft.VisualStudio.TestPlatform.CoreUtilities.Constants;

namespace Microsoft.TestPlatform.VsTestConsole.TranslationLayer;

/// <summary>
/// An implementation of <see cref="IVsTestConsoleWrapper"/> to invoke test operations
/// via the <c>vstest.console</c> test runner.
/// </summary>
public class VsTestConsoleWrapper : IVsTestConsoleWrapper
{
    private readonly IProcessManager _vstestConsoleProcessManager;

    private readonly ITranslationLayerRequestSender _requestSender;

    private readonly IProcessHelper _processHelper;

    private bool _sessionStarted;

    /// <summary>
    /// Path to additional extensions to reinitialize vstest.console
    /// </summary>
    private IEnumerable<string> _pathToAdditionalExtensions;

    /// <summary>
    /// Additional parameters for vstest.console.exe
    /// </summary>
    private readonly ConsoleParameters _consoleParameters;

    private readonly ITestPlatformEventSource _testPlatformEventSource;

    /// <summary>
    /// Initializes a new instance of the <see cref="VsTestConsoleWrapper"/> class.
    /// </summary>
    ///
    /// <param name="vstestConsolePath">
    /// Path to the test runner <c>vstest.console.exe</c>.
    /// </param>
    public VsTestConsoleWrapper(
        string vstestConsolePath)
        : this(
            vstestConsolePath,
            ConsoleParameters.Default)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VsTestConsoleWrapper"/> class.
    /// </summary>
    ///
    /// <param name="vstestConsolePath">Path to the test runner <c>vstest.console.exe</c>.</param>
    /// <param name="consoleParameters">The parameters to be passed onto the runner process.</param>
    public VsTestConsoleWrapper(
        string vstestConsolePath,
        ConsoleParameters consoleParameters)
        : this(
            new VsTestConsoleRequestSender(),
            new VsTestConsoleProcessManager(vstestConsolePath),
            consoleParameters,
            TestPlatformEventSource.Instance,
            new ProcessHelper())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VsTestConsoleWrapper"/> class.
    /// </summary>
    ///
    /// <remarks>Defined for testing purposes.</remarks>
    ///
    /// <param name="vstestConsolePath">Path to the test runner <c>vstest.console.exe</c>.</param>
    /// <param name="dotnetExePath">Path to dotnet exe, needed for CI builds.</param>
    /// <param name="consoleParameters">The parameters to be passed onto the runner process.</param>
    internal VsTestConsoleWrapper(
        string vstestConsolePath,
        string dotnetExePath,
        ConsoleParameters consoleParameters)
        : this(
            new VsTestConsoleRequestSender(),
            new VsTestConsoleProcessManager(vstestConsolePath, dotnetExePath),
            consoleParameters,
            TestPlatformEventSource.Instance,
            new ProcessHelper())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VsTestConsoleWrapper"/> class.
    /// </summary>
    ///
    /// <param name="requestSender">Sender for test messages.</param>
    /// <param name="processManager">Process manager.</param>
    /// <param name="consoleParameters">The parameters to be passed onto the runner process.</param>
    /// <param name="testPlatformEventSource">Performance event source.</param>
    /// <param name="processHelper">Helper for process related utilities.</param>
    internal VsTestConsoleWrapper(
        ITranslationLayerRequestSender requestSender,
        IProcessManager processManager,
        ConsoleParameters consoleParameters,
        ITestPlatformEventSource testPlatformEventSource,
        IProcessHelper processHelper)
    {
        _requestSender = requestSender;
        _vstestConsoleProcessManager = processManager;
        _consoleParameters = consoleParameters;
        _testPlatformEventSource = testPlatformEventSource;
        _processHelper = processHelper;
        _pathToAdditionalExtensions = new List<string>();

        _vstestConsoleProcessManager.ProcessExited += (sender, args) => _requestSender.OnProcessExited();
        _sessionStarted = false;
    }


    #region IVsTestConsoleWrapper

    /// <inheritdoc/>
    public void StartSession()
    {
        EqtTrace.Info("VsTestConsoleWrapper.StartSession: Starting VsTestConsoleWrapper session.");

        _testPlatformEventSource.TranslationLayerInitializeStart();

        // Start communication
        var port = _requestSender.InitializeCommunication();

        if (port > 0)
        {
            // Fill the parameters
#if NET5_0_OR_GREATER
            _consoleParameters.ParentProcessId = Environment.ProcessId;
#else
            using (var process = Process.GetCurrentProcess())
                _consoleParameters.ParentProcessId = process.Id;
#endif
            _consoleParameters.PortNumber = port;

            // Start vstest.console.exe process
            _vstestConsoleProcessManager.StartProcess(_consoleParameters);
        }
        else
        {
            // Close the sender as it failed to host server
            _requestSender.Close();
            throw new TransationLayerException("Error hosting communication channel");
        }
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public ITestSession? StartTestSession(
        IList<string> sources,
        string? runSettings,
        ITestSessionEventsHandler eventsHandler)
    {
        return StartTestSession(
            sources,
            runSettings,
            options: null,
            eventsHandler);
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

        EnsureInitialized();

        var testSessionInfo = _requestSender.StartTestSession(
            sources,
            runSettings,
            options,
            eventsHandler,
            testHostLauncher);

        return (testSessionInfo != null)
            ? new TestSession(
                testSessionInfo,
                eventsHandler,
                this)
            : null;
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public bool StopTestSession(
        TestSessionInfo? testSessionInfo,
        ITestSessionEventsHandler eventsHandler)
    {
        return StopTestSession(
            testSessionInfo,
            options: null,
            eventsHandler);
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public bool StopTestSession(
        TestSessionInfo? testSessionInfo,
        TestPlatformOptions? options,
        ITestSessionEventsHandler eventsHandler)
    {
        _testPlatformEventSource.TranslationLayerStopTestSessionStart();

        EnsureInitialized();
        return _requestSender.StopTestSession(
            testSessionInfo,
            options,
            eventsHandler);
    }

    /// <inheritdoc/>
    public void InitializeExtensions(IEnumerable<string> pathToAdditionalExtensions)
    {
        EnsureInitialized();

        _pathToAdditionalExtensions = pathToAdditionalExtensions.ToList();
        _requestSender.InitializeExtensions(_pathToAdditionalExtensions);
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
            discoveryEventsHandler: new DiscoveryEventsHandleConverter(discoveryEventsHandler));
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

        EnsureInitialized();
        _requestSender.DiscoverTests(
            sources,
            discoverySettings,
            options,
            testSessionInfo,
            discoveryEventsHandler);
    }

    /// <inheritdoc/>
    public void CancelDiscovery()
    {
        _requestSender.CancelDiscovery();
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
            testRunEventsHandler: testRunEventsHandler);
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
        RunTests(
            sources,
            runSettings,
            options,
            testSessionInfo,
            testRunEventsHandler,
            new NoOpTelemetryEventsHandler());
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
        var sourceList = sources.ToList();
        _testPlatformEventSource.TranslationLayerExecutionStart(
            0,
            sourceList.Count,
            0,
            runSettings ?? string.Empty);

        EnsureInitialized();
        _requestSender.StartTestRun(
            sourceList,
            runSettings,
            options,
            testSessionInfo,
            testRunEventsHandler,
            telemetryEventsHandler);
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
        RunTests(
            testCases,
            runSettings,
            options,
            testSessionInfo,
            testRunEventsHandler,
            new NoOpTelemetryEventsHandler());
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
        var testCaseList = testCases.ToList();
        _testPlatformEventSource.TranslationLayerExecutionStart(
            0,
            0,
            testCaseList.Count,
            runSettings ?? string.Empty);

        EnsureInitialized();
        _requestSender.StartTestRun(
            testCaseList,
            runSettings,
            options,
            testSessionInfo,
            testRunEventsHandler,
            telemetryEventsHandler);
    }

    /// <inheritdoc/>
    public void RunTestsWithCustomTestHost(
        IEnumerable<string> sources,
        string? runSettings,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher customTestHostLauncher)
    {
        RunTestsWithCustomTestHost(
            sources,
            runSettings,
            options: null,
            testRunEventsHandler: testRunEventsHandler,
            customTestHostLauncher: customTestHostLauncher);
    }

    /// <inheritdoc/>
    public void RunTestsWithCustomTestHost(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher customTestHostLauncher)
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
        ITestHostLauncher customTestHostLauncher)
    {
        RunTestsWithCustomTestHost(
            sources,
            runSettings,
            options,
            testSessionInfo,
            testRunEventsHandler,
            new NoOpTelemetryEventsHandler(),
            customTestHostLauncher);
    }

    /// <inheritdoc/>
    public void RunTestsWithCustomTestHost(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler,
        ITestHostLauncher customTestHostLauncher)
    {
        var sourceList = sources.ToList();
        _testPlatformEventSource.TranslationLayerExecutionStart(
            1,
            sourceList.Count,
            0,
            runSettings ?? string.Empty);

        EnsureInitialized();
        _requestSender.StartTestRunWithCustomHost(
            sourceList,
            runSettings,
            options,
            testSessionInfo,
            testRunEventsHandler,
            telemetryEventsHandler,
            customTestHostLauncher);
    }

    /// <inheritdoc/>
    public void RunTestsWithCustomTestHost(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher customTestHostLauncher)
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
        ITestHostLauncher customTestHostLauncher)
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
        ITestHostLauncher customTestHostLauncher)
    {
        RunTestsWithCustomTestHost(
            testCases,
            runSettings,
            options,
            testSessionInfo,
            testRunEventsHandler,
            new NoOpTelemetryEventsHandler(),
            customTestHostLauncher);
    }

    /// <inheritdoc/>
    public void RunTestsWithCustomTestHost(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler,
        ITestHostLauncher customTestHostLauncher)
    {
        var testCaseList = testCases.ToList();
        _testPlatformEventSource.TranslationLayerExecutionStart(
            1,
            0,
            testCaseList.Count,
            runSettings ?? string.Empty);

        EnsureInitialized();
        _requestSender.StartTestRunWithCustomHost(
            testCaseList,
            runSettings,
            options,
            testSessionInfo,
            testRunEventsHandler,
            telemetryEventsHandler,
            customTestHostLauncher);
    }

    /// <inheritdoc/>
    public void CancelTestRun()
    {
        _requestSender.CancelTestRun();
    }

    /// <inheritdoc/>
    public void AbortTestRun()
    {
        _requestSender.AbortTestRun();
    }

    /// <inheritdoc/>
    public void EndSession()
    {
        EqtTrace.Info("VsTestConsoleWrapper.EndSession: Ending VsTestConsoleWrapper session");

        _requestSender.EndSession();
        _requestSender.Close();

        // If vstest.console is still hanging around, it should be explicitly killed.
        _vstestConsoleProcessManager.ShutdownProcess();

        _sessionStarted = false;
    }

    #endregion

    #region IVsTestConsoleWrapperAsync

    /// <inheritdoc/>
    public async Task StartSessionAsync()
    {
        EqtTrace.Info("VsTestConsoleWrapperAsync.StartSessionAsync: Starting VsTestConsoleWrapper session");

        _testPlatformEventSource.TranslationLayerInitializeStart();

        var timeout = EnvironmentHelper.GetConnectionTimeout();
        // Start communication
        var port = await _requestSender.InitializeCommunicationAsync(timeout * 1000).ConfigureAwait(false);

        if (port > 0)
        {
            // Fill the parameters
#if NET5_0_OR_GREATER
            _consoleParameters.ParentProcessId = Environment.ProcessId;
#else
            using (var process = Process.GetCurrentProcess())
                _consoleParameters.ParentProcessId = process.Id;
#endif
            _consoleParameters.PortNumber = port;

            // Start vstest.console.exe process
            _vstestConsoleProcessManager.StartProcess(_consoleParameters);
        }
        else
        {
            // Close the sender as it failed to host server
            _requestSender.Close();
            throw new TransationLayerException("Error hosting communication channel and connecting to console");
        }
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
            eventsHandler).ConfigureAwait(false);
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
            testHostLauncher: null).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public async Task<ITestSession?> StartTestSessionAsync(
        IList<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        ITestSessionEventsHandler eventsHandler,
        ITestHostLauncher? testHostLauncher)
    {
        _testPlatformEventSource.TranslationLayerStartTestSessionStart();

        await EnsureInitializedAsync().ConfigureAwait(false);

        var testSessionInfo = await _requestSender.StartTestSessionAsync(
            sources,
            runSettings,
            options,
            eventsHandler,
            testHostLauncher).ConfigureAwait(false);

        return testSessionInfo != null
            ? new TestSession(
                testSessionInfo,
                eventsHandler,
                this)
            : null;
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
            eventsHandler).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public async Task<bool> StopTestSessionAsync(
        TestSessionInfo? testSessionInfo,
        TestPlatformOptions? options,
        ITestSessionEventsHandler eventsHandler)
    {
        _testPlatformEventSource.TranslationLayerStopTestSessionStart();

        await EnsureInitializedAsync().ConfigureAwait(false);
        return await _requestSender.StopTestSessionAsync(
            testSessionInfo,
            options,
            eventsHandler).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task InitializeExtensionsAsync(IEnumerable<string> pathToAdditionalExtensions)
    {
        await EnsureInitializedAsync().ConfigureAwait(false);
        _pathToAdditionalExtensions = pathToAdditionalExtensions.ToList();
        _requestSender.InitializeExtensions(_pathToAdditionalExtensions);
    }

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
                discoveryEventsHandler: new DiscoveryEventsHandleConverter(discoveryEventsHandler))
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
            discoveryEventsHandler).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task DiscoverTestsAsync(
        IEnumerable<string> sources,
        string? discoverySettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestDiscoveryEventsHandler2 discoveryEventsHandler)
    {
        _testPlatformEventSource.TranslationLayerDiscoveryStart();

        await EnsureInitializedAsync().ConfigureAwait(false);
        await _requestSender.DiscoverTestsAsync(
            sources,
            discoverySettings,
            options,
            testSessionInfo,
            discoveryEventsHandler).ConfigureAwait(false);
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
            testRunEventsHandler).ConfigureAwait(false);
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
            testRunEventsHandler).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task RunTestsAsync(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler)
    {
        await RunTestsAsync(
            sources,
            runSettings,
            options,
            testSessionInfo,
            testRunEventsHandler,
            new NoOpTelemetryEventsHandler()).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task RunTestsAsync(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler)
    {
        var sourceList = sources.ToList();
        _testPlatformEventSource.TranslationLayerExecutionStart(
            0,
            sourceList.Count,
            0,
            runSettings ?? string.Empty);

        await EnsureInitializedAsync().ConfigureAwait(false);
        await _requestSender.StartTestRunAsync(
            sourceList,
            runSettings,
            options,
            testSessionInfo,
            testRunEventsHandler,
            telemetryEventsHandler).ConfigureAwait(false);
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
            testRunEventsHandler).ConfigureAwait(false);
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
            testRunEventsHandler).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task RunTestsAsync(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler)
    {
        await RunTestsAsync(
            testCases,
            runSettings,
            options,
            testSessionInfo,
            testRunEventsHandler,
            new NoOpTelemetryEventsHandler()).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task RunTestsAsync(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler)
    {
        var testCaseList = testCases.ToList();
        _testPlatformEventSource.TranslationLayerExecutionStart(
            0,
            0,
            testCaseList.Count,
            runSettings ?? string.Empty);

        await EnsureInitializedAsync().ConfigureAwait(false);
        await _requestSender.StartTestRunAsync(
            testCaseList,
            runSettings,
            options,
            testSessionInfo,
            testRunEventsHandler,
            telemetryEventsHandler).ConfigureAwait(false);
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
            customTestHostLauncher).ConfigureAwait(false);
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
            customTestHostLauncher).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task RunTestsWithCustomTestHostAsync(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher customTestHostLauncher)
    {
        await RunTestsWithCustomTestHostAsync(
            sources,
            runSettings,
            options,
            testSessionInfo,
            testRunEventsHandler,
            new NoOpTelemetryEventsHandler(),
            customTestHostLauncher).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task RunTestsWithCustomTestHostAsync(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler,
        ITestHostLauncher customTestHostLauncher)
    {
        var sourceList = sources.ToList();
        _testPlatformEventSource.TranslationLayerExecutionStart(
            1,
            sourceList.Count,
            0,
            runSettings ?? string.Empty);

        await EnsureInitializedAsync().ConfigureAwait(false);
        await _requestSender.StartTestRunWithCustomHostAsync(
            sourceList,
            runSettings,
            options,
            testSessionInfo,
            testRunEventsHandler,
            telemetryEventsHandler,
            customTestHostLauncher).ConfigureAwait(false);
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
            customTestHostLauncher).ConfigureAwait(false);
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
            customTestHostLauncher).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task RunTestsWithCustomTestHostAsync(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher customTestHostLauncher)
    {
        await RunTestsWithCustomTestHostAsync(
            testCases,
            runSettings,
            options,
            testSessionInfo,
            testRunEventsHandler,
            new NoOpTelemetryEventsHandler(),
            customTestHostLauncher).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task RunTestsWithCustomTestHostAsync(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler,
        ITestHostLauncher customTestHostLauncher)
    {
        var testCaseList = testCases.ToList();
        _testPlatformEventSource.TranslationLayerExecutionStart(
            1,
            0,
            testCaseList.Count,
            runSettings ?? string.Empty);

        await EnsureInitializedAsync().ConfigureAwait(false);
        await _requestSender.StartTestRunWithCustomHostAsync(
            testCaseList,
            runSettings,
            options,
            testSessionInfo,
            testRunEventsHandler,
            telemetryEventsHandler,
            customTestHostLauncher).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task ProcessTestRunAttachmentsAsync(
        IEnumerable<AttachmentSet> attachments,
        IEnumerable<InvokedDataCollector>? invokedDataCollectors,
        string? processingSettings,
        bool isLastBatch,
        bool collectMetrics,
        ITestRunAttachmentsProcessingEventsHandler testSessionEventsHandler,
        CancellationToken cancellationToken)
    {
        _testPlatformEventSource.TranslationLayerTestRunAttachmentsProcessingStart();

        await EnsureInitializedAsync().ConfigureAwait(false);
        await _requestSender.ProcessTestRunAttachmentsAsync(
            attachments,
            invokedDataCollectors,
            processingSettings,
            collectMetrics,
            testSessionEventsHandler,
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public Task ProcessTestRunAttachmentsAsync(
        IEnumerable<AttachmentSet> attachments,
        string? processingSettings,
        bool isLastBatch,
        bool collectMetrics,
        ITestRunAttachmentsProcessingEventsHandler testSessionEventsHandler,
        CancellationToken cancellationToken)
        => ProcessTestRunAttachmentsAsync(attachments, Enumerable.Empty<InvokedDataCollector>(), processingSettings, isLastBatch, collectMetrics, testSessionEventsHandler, cancellationToken);

    #endregion

    private void EnsureInitialized()
    {
        if (!_vstestConsoleProcessManager.IsProcessInitialized())
        {
            EqtTrace.Info("VsTestConsoleWrapper.EnsureInitialized: Process is not started.");
            StartSession();
            _sessionStarted = WaitForConnection();

            if (_sessionStarted)
            {
                EqtTrace.Info("VsTestConsoleWrapper.EnsureInitialized: Send a request to initialize extensions.");
                _requestSender.InitializeExtensions(_pathToAdditionalExtensions);
            }
        }

        if (!_sessionStarted && _requestSender != null)
        {
            EqtTrace.Info("VsTestConsoleWrapper.EnsureInitialized: Process Started.");
            _sessionStarted = WaitForConnection();
        }
    }

    private async Task EnsureInitializedAsync()
    {
        if (!_vstestConsoleProcessManager.IsProcessInitialized())
        {
            EqtTrace.Info("VsTestConsoleWrapper.EnsureInitializedAsync: Process is not started.");
            await StartSessionAsync().ConfigureAwait(false);

            EqtTrace.Info("VsTestConsoleWrapper.EnsureInitializedAsync: Send a request to initialize extensions.");
            _requestSender.InitializeExtensions(_pathToAdditionalExtensions);
        }
    }

    private bool WaitForConnection()
    {
        EqtTrace.Info("VsTestConsoleWrapper.WaitForConnection: Waiting for connection to command line runner.");

        var timeout = EnvironmentHelper.GetConnectionTimeout();
        if (!_requestSender.WaitForRequestHandlerConnection(timeout * 1000))
        {
            var processName = _processHelper.GetCurrentProcessFileName();
            throw new TransationLayerException(
                string.Format(
                    CultureInfo.CurrentCulture,
                    CommunicationUtilitiesResources.ConnectionTimeoutErrorMessage,
                    processName,
                    CoreUtilitiesConstants.VstestConsoleProcessName,
                    timeout,
                    EnvironmentHelper.VstestConnectionTimeout)
            );
        }

        _testPlatformEventSource.TranslationLayerInitializeStop();
        return true;
    }
}
