// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;

using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.ClientProtocol;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client;

/// <summary>
/// Orchestrates test execution operations for the engine communicating with the client.
/// </summary>
internal class ProxyExecutionManager : IProxyExecutionManager, IBaseProxy, IInternalTestRunEventsHandler
{
    private readonly TestSessionInfo? _testSessionInfo;
    private readonly Func<string, ProxyExecutionManager, ProxyOperationManager>? _proxyOperationManagerCreator;
    private readonly IFileHelper _fileHelper;
    private readonly IDataSerializer _dataSerializer;
    private readonly bool _debugEnabledForTestSession;

    private List<string>? _testSources;
    private ITestRuntimeProvider? _testHostManager;
    private bool _isCommunicationEstablished;
    private ProxyOperationManager? _proxyOperationManager;
    private IInternalTestRunEventsHandler? _baseTestRunEventsHandler;
    private bool _skipDefaultAdapters;

    /// <inheritdoc/>
    public bool IsInitialized { get; private set; }

    /// <summary>
    /// Gets or sets the cancellation token source.
    /// </summary>
    public CancellationTokenSource CancellationTokenSource
    {
        get
        {
            TPDebug.Assert(_proxyOperationManager is not null, "_proxyOperationManager is null");
            return _proxyOperationManager.CancellationTokenSource;
        }
        set
        {
            TPDebug.Assert(_proxyOperationManager is not null, "_proxyOperationManager is null");
            _proxyOperationManager.CancellationTokenSource = value;
        }
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="ProxyExecutionManager"/> class.
    /// </summary>
    ///
    /// <param name="testSessionInfo">The test session info.</param>
    /// <param name="proxyOperationManagerCreator">The proxy operation manager creator.</param>
    /// <param name="debugEnabledForTestSession">
    /// A flag indicating if debugging should be enabled or not.
    /// </param>
    public ProxyExecutionManager(
        TestSessionInfo testSessionInfo,
        Func<string, ProxyExecutionManager, ProxyOperationManager> proxyOperationManagerCreator,
        bool debugEnabledForTestSession)
    {
        // Filling in test session info and proxy information.
        _testSessionInfo = testSessionInfo;
        _proxyOperationManagerCreator = proxyOperationManagerCreator;

        // This should be set to enable debugging when we have test session info available.
        _debugEnabledForTestSession = debugEnabledForTestSession;

        _testHostManager = null;
        _dataSerializer = JsonDataSerializer.Instance;
        _fileHelper = new FileHelper();
        _isCommunicationEstablished = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProxyExecutionManager"/> class.
    /// </summary>
    ///
    /// <param name="requestData">
    /// The request data for providing services and data for run.
    /// </param>
    /// <param name="requestSender">Test request sender instance.</param>
    /// <param name="testHostManager">Test host manager for this proxy.</param>
    public ProxyExecutionManager(
        IRequestData requestData,
        ITestRequestSender requestSender,
        ITestRuntimeProvider testHostManager,
        Framework testHostManagerFramework) :
        this(
            requestData,
            requestSender,
            testHostManager,
            testHostManagerFramework,
            JsonDataSerializer.Instance,
            new FileHelper())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProxyExecutionManager"/> class.
    /// </summary>
    ///
    /// <remarks>
    /// Constructor with dependency injection. Used for unit testing.
    /// </remarks>
    ///
    /// <param name="requestData">The request data for common services and data for run.</param>
    /// <param name="requestSender">Request sender instance.</param>
    /// <param name="testHostManager">Test host manager instance.</param>
    /// <param name="dataSerializer">Data serializer instance.</param>
    /// <param name="fileHelper">File helper instance.</param>
    internal ProxyExecutionManager(
        IRequestData requestData,
        ITestRequestSender requestSender,
        ITestRuntimeProvider testHostManager,
        Framework testHostManagerFramework,
        IDataSerializer dataSerializer,
        IFileHelper fileHelper)
    {
        _testHostManager = testHostManager;
        _dataSerializer = dataSerializer;
        _isCommunicationEstablished = false;
        _fileHelper = fileHelper;

        // Create a new proxy operation manager.
        _proxyOperationManager = new ProxyOperationManager(requestData, requestSender, testHostManager, testHostManagerFramework, this);
    }


    #region IProxyExecutionManager implementation.

    /// <inheritdoc/>
    public virtual void Initialize(bool skipDefaultAdapters)
    {
        _skipDefaultAdapters = skipDefaultAdapters;
        IsInitialized = true;
    }

    public virtual void InitializeTestRun(TestRunCriteria testRunCriteria, IInternalTestRunEventsHandler eventHandler)
    {
        if (_proxyOperationManager == null)
        {
            // In case we have an active test session, we always prefer the already
            // created proxies instead of the ones that need to be created on the spot.
            var sources = testRunCriteria.HasSpecificTests
                ? TestSourcesUtility.GetSources(testRunCriteria.Tests)
                : testRunCriteria.Sources;

            TPDebug.Assert(_proxyOperationManagerCreator is not null, "_proxyOperationManagerCreator is null");
            TPDebug.Assert(sources is not null, "sources is null");
            _proxyOperationManager = _proxyOperationManagerCreator(
                sources.First(),
                this);

            _testHostManager = _proxyOperationManager.TestHostManager;
        }

        _baseTestRunEventsHandler = eventHandler;
        try
        {
            EqtTrace.Verbose("ProxyExecutionManager: Test host is always Lazy initialize.");

            _testSources = new List<string>(
                testRunCriteria.HasSpecificSources
                    ? testRunCriteria.Sources
                    // If the test execution is with a test filter, group them by sources.
                    : testRunCriteria.Tests.GroupBy(tc => tc.Source).Select(g => g.Key));

            _isCommunicationEstablished = _proxyOperationManager.SetupChannel(
                _testSources,
                testRunCriteria.TestRunSettings);

            if (_isCommunicationEstablished)
            {
                _proxyOperationManager.CancellationTokenSource.Token.ThrowTestPlatformExceptionIfCancellationRequested();

                InitializeExtensions(_testSources);
            }
        }
        catch (Exception ex)
        {
            HandleError(ex);
        }
    }
    /// <inheritdoc/>
    public virtual int StartTestRun(TestRunCriteria testRunCriteria, IInternalTestRunEventsHandler eventHandler)
    {
        try
        {
            if (!_isCommunicationEstablished)
            {
                InitializeTestRun(testRunCriteria, eventHandler);
            }

            // In certain scenarios (like the one for non-parallel dotnet runs) we may end up
            // using the incorrect events handler which can have nasty side effects, like failing to
            // properly terminate the communication with any data collector. The reason for this is
            // that the initialization and the actual test run have been decoupled and are now two
            // separate operations. In the initialization phase we already provide an events handler
            // to be invoked when data flows back from the testhost, but the "correct" handler is
            // only provided in the run phase which may occur later on. This was not a problem when
            // initialization was part of the normal test run workflow. However, now that the two
            // operations are separate and because initialization could've already taken place, the
            // communication channel could be properly set up, which means that we don't get to
            // overwrite the old events handler anymore.
            // The solution to this is to make sure that we always use the most "up-to-date"
            // handler, and that would be the handler we got as an argument when this method was
            // called. When initialization and test run are part of the same operation the behavior
            // is still correct, since the two events handler will be equal and there'll be no need
            // for an overwrite.
            if (eventHandler != _baseTestRunEventsHandler)
            {
                _baseTestRunEventsHandler = eventHandler;
            }

            TPDebug.Assert(_proxyOperationManager is not null, "ProxyOperationManager is null.");

            if (_isCommunicationEstablished)
            {
                var testSources = new List<string>(
                    testRunCriteria.HasSpecificSources
                        ? testRunCriteria.Sources
                        // If the test execution is with a test filter, group them by sources.
                        : testRunCriteria.Tests.GroupBy(tc => tc.Source).Select(g => g.Key));

                // This code should be in sync with InProcessProxyExecutionManager.StartTestRun
                // execution context.
                var executionContext = new TestExecutionContext(
                    testRunCriteria.FrequencyOfRunStatsChangeEvent,
                    testRunCriteria.RunStatsChangeEventTimeout,
                    inIsolation: false,
                    keepAlive: testRunCriteria.KeepAlive,
                    isDataCollectionEnabled: false,
                    areTestCaseLevelEventsRequired: false,
                    hasTestRun: true,
                    // Debugging should happen if there's a custom test host launcher present
                    // and is in debugging mode, or if the debugging is enabled in case the
                    // test session info is present.
                    isDebug:
                        (testRunCriteria.TestHostLauncher != null && testRunCriteria.TestHostLauncher.IsDebug)
                        || _debugEnabledForTestSession,
                    testCaseFilter: testRunCriteria.TestCaseFilter,
                    filterOptions: testRunCriteria.FilterOptions);

                // This is workaround for the bug https://github.com/Microsoft/vstest/issues/970
                var runsettings = _proxyOperationManager.RemoveNodesFromRunsettingsIfRequired(
                    testRunCriteria.TestRunSettings,
                    LogMessage);

                if (testRunCriteria.HasSpecificSources)
                {
                    var runRequest = testRunCriteria.CreateTestRunCriteriaForSources(
                        _testHostManager,
                        runsettings,
                        executionContext,
                        _testSources);
                    _proxyOperationManager.RequestSender.StartTestRun(runRequest, this);
                }
                else
                {
                    var runRequest = testRunCriteria.CreateTestRunCriteriaForTests(
                        _testHostManager,
                        runsettings,
                        executionContext,
                        _testSources);
                    _proxyOperationManager.RequestSender.StartTestRun(runRequest, this);
                }
            }
        }
        catch (Exception exception)
        {
            HandleError(exception);
        }

        return 0;
    }

    private void HandleError(Exception exception)
    {
        EqtTrace.Error("ProxyExecutionManager.StartTestRun: Failed to start test run: {0}", exception);

        // Log error message to design mode and CLI.
        // TestPlatformException is expected exception, log only the message.
        // For other exceptions, log the stacktrace as well.
        var errorMessage = exception is TestPlatformException ? exception.Message : exception.ToString();
        var testMessagePayload = new TestMessagePayload
        {
            MessageLevel = TestMessageLevel.Error,
            Message = errorMessage
        };
        HandleRawMessage(_dataSerializer.SerializePayload(MessageType.TestMessage, testMessagePayload));
        LogMessage(TestMessageLevel.Error, errorMessage);

        // Send a run complete to caller. Similar logic is also used in
        // ParallelProxyExecutionManager.StartTestRunOnConcurrentManager.
        //
        // Aborted is `true`: in case of parallel run (or non shared host), an aborted
        // message ensures another execution manager created to replace the current one.
        // This will help if the current execution manager is aborted due to irreparable
        // error and the test host is lost as well.
        var completeArgs = new TestRunCompleteEventArgs(null, false, true, null, new Collection<AttachmentSet>(), new Collection<InvokedDataCollector>(), TimeSpan.Zero);
        var testRunCompletePayload = new TestRunCompletePayload { TestRunCompleteArgs = completeArgs };
        HandleRawMessage(_dataSerializer.SerializePayload(MessageType.ExecutionComplete, testRunCompletePayload));
        HandleTestRunComplete(completeArgs, null, null, null);
    }

    /// <inheritdoc/>
    public virtual void Cancel(IInternalTestRunEventsHandler eventHandler)
    {
        // Just in case ExecuteAsync isn't called yet, set the eventhandler.
        _baseTestRunEventsHandler ??= eventHandler;

        // Do nothing if the proxy is not initialized yet.
        if (_proxyOperationManager == null)
        {
            return;
        }

        // Cancel fast, try to stop testhost deployment/launch.
        _proxyOperationManager.CancellationTokenSource.Cancel();
        if (_isCommunicationEstablished)
        {
            _proxyOperationManager.RequestSender.SendTestRunCancel();
        }
    }

    /// <inheritdoc/>
    public void Abort(IInternalTestRunEventsHandler eventHandler)
    {
        // Just in case ExecuteAsync isn't called yet, set the eventhandler.
        _baseTestRunEventsHandler ??= eventHandler;

        // Do nothing if the proxy is not initialized yet.
        if (_proxyOperationManager == null)
        {
            return;
        }

        // Cancel fast, try to stop testhost deployment/launch.
        _proxyOperationManager.CancellationTokenSource.Cancel();

        if (_isCommunicationEstablished)
        {
            _proxyOperationManager.RequestSender.SendTestRunAbort();
        }
    }

    /// <inheritdoc/>
    public void Close()
    {
        // Do nothing if the proxy is not initialized yet.
        if (_proxyOperationManager == null)
        {
            return;
        }

        // When no test session is being used, we don't share the testhost
        // between test discovery and test run. The testhost is closed upon
        // successfully completing the operation it was spawned for.
        //
        // In contrast, the new workflow (using test sessions) means we should keep
        // the testhost alive until explicitly closed by the test session owner, but
        // only if the testhost is part of a test session (i.e. the proxy operation manager
        // id is valid), since there is the distinct possibility of test session criteria
        // changing between spawn and discovery/run, causing a new proxy operation manager
        // to be spawned on demand instead of dequeuing an incompatible proxy from the pool.
        if (_testSessionInfo == null || _proxyOperationManager.Id < 0)
        {
            _proxyOperationManager.Close();
            return;
        }

        TestSessionPool.Instance.ReturnProxy(_testSessionInfo, _proxyOperationManager.Id);
    }

    /// <inheritdoc/>
    public virtual int LaunchProcessWithDebuggerAttached(TestProcessStartInfo testProcessStartInfo)
    {
        return _baseTestRunEventsHandler?.LaunchProcessWithDebuggerAttached(testProcessStartInfo) ?? -1;
    }

    /// <inheritdoc />
    public bool AttachDebuggerToProcess(AttachDebuggerInfo attachDebuggerInfo)
    {
        // TestHost did not provide any additional TargetFramework info for the process it wants to attach to,
        // specify the TargetFramework of the testhost, in case it is just an old testhost that is not aware
        // of this capability.
        attachDebuggerInfo.TargetFramework ??= _proxyOperationManager?.TestHostManagerFramework?.ToString();

        if (attachDebuggerInfo.Sources is null || attachDebuggerInfo.Sources.Count == 0)
        {
            attachDebuggerInfo.Sources = _testSources;
        }

        return _baseTestRunEventsHandler?.AttachDebuggerToProcess(attachDebuggerInfo) ?? false;
    }

    /// <inheritdoc/>
    public void HandleTestRunComplete(TestRunCompleteEventArgs testRunCompleteArgs, TestRunChangedEventArgs? lastChunkArgs, ICollection<AttachmentSet>? runContextAttachments, ICollection<string>? executorUris)
    {
        _baseTestRunEventsHandler?.HandleTestRunComplete(testRunCompleteArgs, lastChunkArgs, runContextAttachments, executorUris);
    }

    /// <inheritdoc/>
    public void HandleTestRunStatsChange(TestRunChangedEventArgs? testRunChangedArgs)
    {
        _baseTestRunEventsHandler?.HandleTestRunStatsChange(testRunChangedArgs);
    }

    /// <inheritdoc/>
    public void HandleRawMessage(string rawMessage)
    {
        // TODO: PERF: - why do we have to deserialize the messages here only to read that this is
        // execution complete? Why can't we act on it somewhere else where the result of deserialization is not
        // thrown away?
        var message = _dataSerializer.DeserializeMessage(rawMessage);

        if (string.Equals(message.MessageType, MessageType.ExecutionComplete))
        {
            Close();
        }

        _baseTestRunEventsHandler?.HandleRawMessage(rawMessage);
    }

    /// <inheritdoc/>
    public void HandleLogMessage(TestMessageLevel level, string? message)
    {
        _baseTestRunEventsHandler?.HandleLogMessage(level, message);
    }

    #endregion

    #region IBaseProxy implementation.
    /// <inheritdoc/>
    public virtual TestProcessStartInfo UpdateTestProcessStartInfo(TestProcessStartInfo testProcessStartInfo)
    {
        // Update Telemetry Opt in status because by default in Test Host Telemetry is opted out
        var telemetryOptedIn = _proxyOperationManager?.RequestData?.IsTelemetryOptedIn == true ? "true" : "false";
        testProcessStartInfo.Arguments += " --telemetryoptedin " + telemetryOptedIn;
        return testProcessStartInfo;
    }
    #endregion

    /// <summary>
    /// Ensures that the engine is ready for test operations. Usually includes starting up the
    /// test host process.
    /// </summary>
    ///
    /// <param name="sources">List of test sources.</param>
    /// <param name="runSettings">Run settings to be used.</param>
    ///
    /// <returns>
    /// Returns true if the communication is established b/w runner and host, false otherwise.
    /// </returns>
    public virtual bool SetupChannel(IEnumerable<string> sources, string runSettings)
    {
        return _proxyOperationManager?.SetupChannel(sources, runSettings) ?? false;
    }

    private void LogMessage(TestMessageLevel testMessageLevel, string message)
    {
        // Log to vs ide test output.
        var testMessagePayload = new TestMessagePayload { MessageLevel = testMessageLevel, Message = message };
        var rawMessage = _dataSerializer.SerializePayload(MessageType.TestMessage, testMessagePayload);
        HandleRawMessage(rawMessage);

        // Log to vstest.console.
        HandleLogMessage(testMessageLevel, message);
    }

    private void InitializeExtensions(IEnumerable<string> sources)
    {
        var extensions = TestPluginCache.Instance.GetExtensionPaths(TestPlatformConstants.TestAdapterEndsWithPattern, _skipDefaultAdapters);

        // Filter out non existing extensions.
        var nonExistingExtensions = extensions.Where(extension => !_fileHelper.Exists(extension));
        if (nonExistingExtensions.Any())
        {
            LogMessage(TestMessageLevel.Warning, string.Format(CultureInfo.CurrentCulture, Resources.Resources.NonExistingExtensions, string.Join(",", nonExistingExtensions)));
        }

        var sourceList = sources.ToList();
        var platformExtensions = _testHostManager?.GetTestPlatformExtensions(sourceList, extensions.Except(nonExistingExtensions));

        // Only send this if needed.
        if (platformExtensions is not null && platformExtensions.Any())
        {
            _proxyOperationManager?.RequestSender.InitializeExecution(platformExtensions);
        }
    }
}
