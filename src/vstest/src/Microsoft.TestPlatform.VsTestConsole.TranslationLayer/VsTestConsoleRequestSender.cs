// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Payloads;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

using TranslationLayerResources = Microsoft.VisualStudio.TestPlatform.VsTestConsole.TranslationLayer.Resources.Resources;

namespace Microsoft.TestPlatform.VsTestConsole.TranslationLayer;

/// <summary>
/// Vstest console request sender for sending requests to vstest.console.exe
/// </summary>
internal class VsTestConsoleRequestSender : ITranslationLayerRequestSender
{
    /// <summary>
    /// The minimum protocol version that has test session support.
    /// </summary>
    private const int MinimumProtocolVersionWithTestSessionSupport = 5;

    private readonly ICommunicationManager _communicationManager;
    private readonly IDataSerializer _dataSerializer;
    private readonly ITestPlatformEventSource _testPlatformEventSource;
    private readonly ManualResetEvent _handShakeComplete = new(false);

    private bool _handShakeSuccessful;
    private int _protocolVersion = ProtocolVersioning.HighestSupportedVersion;

    /// <summary>
    /// Used to cancel blocking tasks associated with the vstest.console process.
    /// </summary>
    private CancellationTokenSource? _processExitCancellationTokenSource;

    /// <summary>
    /// Initializes a new instance of the <see cref="VsTestConsoleRequestSender"/> class.
    /// </summary>
    public VsTestConsoleRequestSender()
        : this(
            new SocketCommunicationManager(),
            JsonDataSerializer.Instance,
            TestPlatformEventSource.Instance)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VsTestConsoleRequestSender"/> class.
    /// </summary>
    ///
    /// <param name="communicationManager">The communication manager.</param>
    /// <param name="dataSerializer">The data serializer.</param>
    /// <param name="testPlatformEventSource">The test platform event source.</param>
    internal VsTestConsoleRequestSender(
        ICommunicationManager communicationManager,
        IDataSerializer dataSerializer,
        ITestPlatformEventSource testPlatformEventSource)
    {
        _communicationManager = communicationManager;
        _dataSerializer = dataSerializer;
        _testPlatformEventSource = testPlatformEventSource;
    }


    #region ITranslationLayerRequestSender

    /// <inheritdoc/>
    public int InitializeCommunication()
    {
        EqtTrace.Info("VsTestConsoleRequestSender.InitializeCommunication: Started.");

        _processExitCancellationTokenSource = new CancellationTokenSource();
        _handShakeSuccessful = false;
        _handShakeComplete.Reset();
        int port = -1;
        try
        {
            port = _communicationManager.HostServer(new IPEndPoint(IPAddress.Loopback, 0)).Port;
            _communicationManager.AcceptClientAsync();

            Task.Run(() =>
            {
                _communicationManager.WaitForClientConnection(Timeout.Infinite);
                _handShakeSuccessful = HandShakeWithVsTestConsole();
                _handShakeComplete.Set();
            });
        }
        catch (Exception ex)
        {
            EqtTrace.Error(
                "VsTestConsoleRequestSender.InitializeCommunication: Error initializing communication with VstestConsole: {0}",
                ex);
            _handShakeComplete.Set();
        }

        EqtTrace.Info("VsTestConsoleRequestSender.InitializeCommunication: Ended.");

        return port;
    }

    /// <inheritdoc/>
    public bool WaitForRequestHandlerConnection(int clientConnectionTimeout)
    {
        var waitSuccess = _handShakeComplete.WaitOne(clientConnectionTimeout);
        return waitSuccess && _handShakeSuccessful;
    }

    /// <inheritdoc/>
    public async Task<int> InitializeCommunicationAsync(int clientConnectionTimeout)
    {
        EqtTrace.Info($"VsTestConsoleRequestSender.InitializeCommunicationAsync: Started with client connection timeout {clientConnectionTimeout} milliseconds.");

        _processExitCancellationTokenSource = new CancellationTokenSource();
        _handShakeSuccessful = false;
        _handShakeComplete.Reset();
        int port = -1;
        try
        {
            port = _communicationManager.HostServer(new IPEndPoint(IPAddress.Loopback, 0)).Port;
            var timeoutSource = new CancellationTokenSource(clientConnectionTimeout);
            await Task.Run(() =>
                _communicationManager.AcceptClientAsync(), timeoutSource.Token).ConfigureAwait(false);

            _handShakeSuccessful = await HandShakeWithVsTestConsoleAsync().ConfigureAwait(false);
            _handShakeComplete.Set();
        }
        catch (Exception ex)
        {
            EqtTrace.Error(
                "VsTestConsoleRequestSender.InitializeCommunicationAsync: Error initializing communication with VstestConsole: {0}",
                ex);
            _handShakeComplete.Set();
        }

        EqtTrace.Info("VsTestConsoleRequestSender.InitializeCommunicationAsync: Ended.");

        return _handShakeSuccessful ? port : -1;
    }

    /// <inheritdoc/>
    public void InitializeExtensions(IEnumerable<string> pathToAdditionalExtensions)
    {
        EqtTrace.Info($"VsTestConsoleRequestSender.InitializeExtensions: Initializing extensions with additional extensions path {string.Join(",", pathToAdditionalExtensions.ToList())}.");

        _communicationManager.SendMessage(
            MessageType.ExtensionsInitialize,
            pathToAdditionalExtensions,
            _protocolVersion);
    }

    /// <inheritdoc/>
    public void DiscoverTests(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestDiscoveryEventsHandler2 eventHandler)
    {
        EqtTrace.Info("VsTestConsoleRequestSender.DiscoverTests: Starting test discovery.");

        SendMessageAndListenAndReportTestCases(
            sources,
            runSettings,
            options,
            testSessionInfo,
            eventHandler);
    }

    /// <inheritdoc/>
    public async Task DiscoverTestsAsync(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestDiscoveryEventsHandler2 eventHandler)
    {
        EqtTrace.Info("VsTestConsoleRequestSender.DiscoverTestsAsync: Starting test discovery.");

        await SendMessageAndListenAndReportTestCasesAsync(
            sources,
            runSettings,
            options,
            testSessionInfo,
            eventHandler).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public void StartTestRun(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler runEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler)
    {
        EqtTrace.Info("VsTestConsoleRequestSender.StartTestRun: Starting test run.");

        SendMessageAndListenAndReportTestResults(
            MessageType.TestRunAllSourcesWithDefaultHost,
            new TestRunRequestPayload()
            {
                Sources = sources.ToList(),
                RunSettings = runSettings,
                TestPlatformOptions = options,
                TestSessionInfo = testSessionInfo
            },
            runEventsHandler,
            telemetryEventsHandler,
            null);
    }

    /// <inheritdoc/>
    public async Task StartTestRunAsync(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler runEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler)
    {
        EqtTrace.Info("VsTestConsoleRequestSender.StartTestRunAsync: Starting test run.");

        await SendMessageAndListenAndReportTestResultsAsync(
            MessageType.TestRunAllSourcesWithDefaultHost,
            new TestRunRequestPayload()
            {
                Sources = sources.ToList(),
                RunSettings = runSettings,
                TestPlatformOptions = options,
                TestSessionInfo = testSessionInfo
            },
            runEventsHandler,
            telemetryEventsHandler,
            null).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public void StartTestRun(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler runEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler)
    {
        EqtTrace.Info("VsTestConsoleRequestSender.StartTestRun: Starting test run.");

        SendMessageAndListenAndReportTestResults(
            MessageType.TestRunAllSourcesWithDefaultHost,
            new TestRunRequestPayload()
            {
                TestCases = testCases.ToList(),
                RunSettings = runSettings,
                TestPlatformOptions = options,
                TestSessionInfo = testSessionInfo
            },
            runEventsHandler,
            telemetryEventsHandler,
            null);
    }

    /// <inheritdoc/>
    public async Task StartTestRunAsync(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler runEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler)
    {
        EqtTrace.Info("VsTestConsoleRequestSender.StartTestRunAsync: Starting test run.");

        await SendMessageAndListenAndReportTestResultsAsync(
            MessageType.TestRunAllSourcesWithDefaultHost,
            new TestRunRequestPayload()
            {
                TestCases = testCases.ToList(),
                RunSettings = runSettings,
                TestPlatformOptions = options,
                TestSessionInfo = testSessionInfo
            },
            runEventsHandler,
            telemetryEventsHandler,
            null).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public void StartTestRunWithCustomHost(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler runEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler,
        ITestHostLauncher customHostLauncher)
    {
        EqtTrace.Info("VsTestConsoleRequestSender.StartTestRunWithCustomHost: Starting test run.");

        SendMessageAndListenAndReportTestResults(
            MessageType.GetTestRunnerProcessStartInfoForRunAll,
            new TestRunRequestPayload()
            {
                Sources = sources.ToList(),
                RunSettings = runSettings,
                DebuggingEnabled = customHostLauncher.IsDebug,
                TestPlatformOptions = options,
                TestSessionInfo = testSessionInfo
            },
            runEventsHandler,
            telemetryEventsHandler,
            customHostLauncher);
    }

    /// <inheritdoc/>
    public async Task StartTestRunWithCustomHostAsync(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler runEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler,
        ITestHostLauncher customHostLauncher)
    {
        EqtTrace.Info("VsTestConsoleRequestSender.StartTestRunWithCustomHostAsync: Starting test run.");

        await SendMessageAndListenAndReportTestResultsAsync(
            MessageType.GetTestRunnerProcessStartInfoForRunAll,
            new TestRunRequestPayload()
            {
                Sources = sources.ToList(),
                RunSettings = runSettings,
                DebuggingEnabled = customHostLauncher.IsDebug,
                TestPlatformOptions = options,
                TestSessionInfo = testSessionInfo
            },
            runEventsHandler,
            telemetryEventsHandler,
            customHostLauncher).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public void StartTestRunWithCustomHost(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler runEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler,
        ITestHostLauncher customHostLauncher)
    {
        EqtTrace.Info("VsTestConsoleRequestSender.StartTestRunWithCustomHost: Starting test run.");

        SendMessageAndListenAndReportTestResults(
            MessageType.GetTestRunnerProcessStartInfoForRunSelected,
            new TestRunRequestPayload
            {
                TestCases = testCases.ToList(),
                RunSettings = runSettings,
                DebuggingEnabled = customHostLauncher.IsDebug,
                TestPlatformOptions = options,
                TestSessionInfo = testSessionInfo
            },
            runEventsHandler,
            telemetryEventsHandler,
            customHostLauncher);
    }

    /// <inheritdoc/>
    public async Task StartTestRunWithCustomHostAsync(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler runEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler,
        ITestHostLauncher customHostLauncher)
    {
        EqtTrace.Info("VsTestConsoleRequestSender.StartTestRunWithCustomHostAsync: Starting test run.");

        await SendMessageAndListenAndReportTestResultsAsync(
            MessageType.GetTestRunnerProcessStartInfoForRunSelected,
            new TestRunRequestPayload()
            {
                TestCases = testCases.ToList(),
                RunSettings = runSettings,
                DebuggingEnabled = customHostLauncher.IsDebug,
                TestPlatformOptions = options,
                TestSessionInfo = testSessionInfo
            },
            runEventsHandler,
            telemetryEventsHandler,
            customHostLauncher).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public TestSessionInfo? StartTestSession(
        IList<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        ITestSessionEventsHandler eventsHandler,
        ITestHostLauncher? testHostLauncher)
    {
        // Make sure vstest.console knows how to handle start/stop test session messages.
        // Bail out if it doesn't, otherwise we'll hang waiting for a reply from the console
        // that will never come.
        if (_protocolVersion < MinimumProtocolVersionWithTestSessionSupport)
        {
            eventsHandler?.HandleStartTestSessionComplete(new());
            return null;
        }

        EqtTrace.Info("VsTestConsoleRequestSender.StartTestSession: Starting test session.");

        try
        {
            var payload = new StartTestSessionPayload
            {
                // TODO (copoiena): When sharing the test host between test discovery and test
                // execution, should we use the test host launcher to launch it ? What side
                // effects does this have ?
                //
                // This is useful for profiling and maybe for launching hosts other than the
                // ones managed by us (i.e., the default host and the dotnet host), examples
                // including UWP and other hosts that don't implement the ITestRuntimeProvider2
                // interface and/or are not aware of the possibility of attaching to an already
                // running process.
                Sources = sources,
                RunSettings = runSettings,
                HasCustomHostLauncher = testHostLauncher != null,
                IsDebuggingEnabled = (testHostLauncher != null)
                                     && testHostLauncher.IsDebug,
                TestPlatformOptions = options
            };

            _communicationManager.SendMessage(
                MessageType.StartTestSession,
                payload,
                _protocolVersion);

            while (true)
            {
                var message = TryReceiveMessage();

                switch (message.MessageType)
                {
                    case MessageType.StartTestSessionCallback:
                        var ackPayload = _dataSerializer.DeserializePayload<StartTestSessionAckPayload>(message);
                        TPDebug.Assert(ackPayload is not null, "ackPayload is null");
                        eventsHandler?.HandleStartTestSessionComplete(ackPayload.EventArgs);
                        return ackPayload.EventArgs!.TestSessionInfo;

                    case MessageType.CustomTestHostLaunch:
                        HandleCustomHostLaunch(testHostLauncher, message);
                        break;

                    case MessageType.EditorAttachDebugger:
                    case MessageType.EditorAttachDebugger2:
                        AttachDebuggerToProcess(testHostLauncher, message);
                        break;

                    case MessageType.TestMessage:
                        var testMessagePayload = _dataSerializer.DeserializePayload<TestMessagePayload>(message);
                        TPDebug.Assert(testMessagePayload is not null, "testMessagePayload is null");
                        eventsHandler?.HandleLogMessage(
                            testMessagePayload.MessageLevel,
                            testMessagePayload.Message);
                        break;

                    default:
                        EqtTrace.Warning(
                            "VsTestConsoleRequestSender.StartTestSession: Unexpected message received: {0}",
                            message.MessageType);
                        break;
                }
            }
        }
        catch (Exception exception)
        {
            EqtTrace.Error(
                "Aborting StartTestSession operation due to error: {0}",
                exception);
            eventsHandler?.HandleLogMessage(
                TestMessageLevel.Error,
                TranslationLayerResources.AbortedStartTestSession);

            eventsHandler?.HandleStartTestSessionComplete(new());
        }
        finally
        {
            _testPlatformEventSource.TranslationLayerStartTestSessionStop();
        }

        return null;
    }

    /// <inheritdoc/>
    public async Task<TestSessionInfo?> StartTestSessionAsync(
        IList<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        ITestSessionEventsHandler eventsHandler,
        ITestHostLauncher? testHostLauncher)
    {
        // Make sure vstest.console knows how to handle start/stop test session messages.
        // Bail out if it doesn't, otherwise we'll hang waiting for a reply from the console
        // that will never come.
        if (_protocolVersion < MinimumProtocolVersionWithTestSessionSupport)
        {
            eventsHandler?.HandleStartTestSessionComplete(new());
            return await Task.FromResult((TestSessionInfo?)null);
        }

        EqtTrace.Info("VsTestConsoleRequestSender.StartTestSession: Starting test session.");

        try
        {
            var payload = new StartTestSessionPayload
            {
                // TODO (copoiena): When sharing the test host between test discovery and test
                // execution, should we use the test host launcher to launch it ? What side
                // effects does this have ?
                //
                // This is useful for profiling and maybe for launching hosts other than the
                // ones managed by us (i.e., the default host and the dotnet host), examples
                // including UWP and other hosts that don't implement the ITestRuntimeProvider2
                // interface and/or are not aware of the possibility of attaching to an already
                // running process.
                Sources = sources,
                RunSettings = runSettings,
                HasCustomHostLauncher = testHostLauncher != null,
                IsDebuggingEnabled = testHostLauncher != null && testHostLauncher.IsDebug,
                TestPlatformOptions = options
            };

            _communicationManager.SendMessage(
                MessageType.StartTestSession,
                payload,
                _protocolVersion);

            while (true)
            {
                var message = await TryReceiveMessageAsync().ConfigureAwait(false);

                switch (message.MessageType)
                {
                    case MessageType.StartTestSessionCallback:
                        var ackPayload = _dataSerializer.DeserializePayload<StartTestSessionAckPayload>(message);
                        TPDebug.Assert(ackPayload is not null, "ackPayload is null");
                        eventsHandler?.HandleStartTestSessionComplete(ackPayload.EventArgs);
                        return ackPayload.EventArgs!.TestSessionInfo;

                    case MessageType.CustomTestHostLaunch:
                        HandleCustomHostLaunch(testHostLauncher, message);
                        break;

                    case MessageType.EditorAttachDebugger:
                    case MessageType.EditorAttachDebugger2:
                        AttachDebuggerToProcess(testHostLauncher, message);
                        break;

                    case MessageType.TestMessage:
                        var testMessagePayload = _dataSerializer.DeserializePayload<TestMessagePayload>(message);
                        TPDebug.Assert(testMessagePayload is not null, "testMessagePayload is null");
                        eventsHandler?.HandleLogMessage(
                            testMessagePayload.MessageLevel,
                            testMessagePayload.Message);
                        break;

                    default:
                        EqtTrace.Warning(
                            "VsTestConsoleRequestSender.StartTestSession: Unexpected message received: {0}",
                            message.MessageType);
                        break;
                }
            }
        }
        catch (Exception exception)
        {
            EqtTrace.Error("Aborting StartTestSession operation due to error: {0}", exception);
            eventsHandler?.HandleLogMessage(
                TestMessageLevel.Error,
                TranslationLayerResources.AbortedStartTestSession);

            eventsHandler?.HandleStartTestSessionComplete(new());
        }
        finally
        {
            _testPlatformEventSource.TranslationLayerStartTestSessionStop();
        }

        return null;
    }

    /// <inheritdoc/>
    public bool StopTestSession(
        TestSessionInfo? testSessionInfo,
        TestPlatformOptions? options,
        ITestSessionEventsHandler eventsHandler)
    {
        // Make sure vstest.console knows how to handle start/stop test session messages.
        // Bail out if it doesn't, otherwise we'll hang waiting for a reply from the console
        // that will never come.
        if (_protocolVersion < MinimumProtocolVersionWithTestSessionSupport)
        {
            eventsHandler?.HandleStopTestSessionComplete(new(testSessionInfo));
            return false;
        }

        EqtTrace.Info("VsTestConsoleRequestSender.StopTestSession: Stop test session.");

        // Due to various considertaions it is possible to end up with a null test session
        // after doing the start test session call. However, we should filter out requests
        // to stop such a session as soon as possible, at the request sender level.
        //
        // We do this here instead of on the wrapper level in order to benefit from the
        // testplatform events being fired still.
        if (testSessionInfo == null)
        {
            _testPlatformEventSource.TranslationLayerStopTestSessionStop();
            return true;
        }

        try
        {
            var stopTestSessionPayload = new StopTestSessionPayload
            {
                TestSessionInfo = testSessionInfo,
                CollectMetrics = options?.CollectMetrics ?? false,
            };

            _communicationManager.SendMessage(
                MessageType.StopTestSession,
                stopTestSessionPayload,
                _protocolVersion);

            while (true)
            {
                var message = TryReceiveMessage();

                switch (message.MessageType)
                {
                    case MessageType.StopTestSessionCallback:
                        var payload = _dataSerializer.DeserializePayload<StopTestSessionAckPayload>(message);
                        TPDebug.Assert(payload is not null, "payload is null");
                        eventsHandler?.HandleStopTestSessionComplete(payload.EventArgs);
                        return payload.EventArgs!.IsStopped;

                    case MessageType.TestMessage:
                        var testMessagePayload = _dataSerializer.DeserializePayload<TestMessagePayload>(message);
                        TPDebug.Assert(testMessagePayload is not null, "testMessagePayload is null");
                        eventsHandler?.HandleLogMessage(
                            testMessagePayload.MessageLevel,
                            testMessagePayload.Message);
                        break;

                    default:
                        EqtTrace.Warning(
                            "VsTestConsoleRequestSender.StopTestSession: Unexpected message received: {0}",
                            message.MessageType);
                        break;
                }
            }
        }
        catch (Exception exception)
        {
            EqtTrace.Error(
                "Aborting StopTestSession operation for id {0} due to error: {1}",
                testSessionInfo?.Id,
                exception);
            eventsHandler?.HandleLogMessage(
                TestMessageLevel.Error,
                TranslationLayerResources.AbortedStopTestSession);

            eventsHandler?.HandleStopTestSessionComplete(new(testSessionInfo));
        }
        finally
        {
            _testPlatformEventSource.TranslationLayerStopTestSessionStop();
        }

        return false;
    }

    /// <inheritdoc/>
    public async Task<bool> StopTestSessionAsync(
        TestSessionInfo? testSessionInfo,
        TestPlatformOptions? options,
        ITestSessionEventsHandler eventsHandler)
    {
        // Make sure vstest.console knows how to handle start/stop test session messages.
        // Bail out if it doesn't, otherwise we'll hang waiting for a reply from the console
        // that will never come.
        if (_protocolVersion < MinimumProtocolVersionWithTestSessionSupport)
        {
            eventsHandler?.HandleStopTestSessionComplete(new(testSessionInfo));
            return await Task.FromResult(false);
        }

        EqtTrace.Info("VsTestConsoleRequestSender.StopTestSession: Stop test session.");

        // Due to various considertaions it is possible to end up with a null test session
        // after doing the start test session call. However, we should filter out requests
        // to stop such a session as soon as possible, at the request sender level.
        //
        // We do this here instead of on the wrapper level in order to benefit from the
        // testplatform events being fired still.
        if (testSessionInfo == null)
        {
            _testPlatformEventSource.TranslationLayerStopTestSessionStop();
            return true;
        }

        try
        {
            var stopTestSessionPayload = new StopTestSessionPayload
            {
                TestSessionInfo = testSessionInfo,
                CollectMetrics = options?.CollectMetrics ?? false,
            };

            _communicationManager.SendMessage(
                MessageType.StopTestSession,
                stopTestSessionPayload,
                _protocolVersion);

            while (true)
            {
                var message = await TryReceiveMessageAsync().ConfigureAwait(false);

                switch (message.MessageType)
                {
                    case MessageType.StopTestSessionCallback:
                        var payload = _dataSerializer.DeserializePayload<StopTestSessionAckPayload>(message);
                        TPDebug.Assert(payload is not null, "payload is null");
                        eventsHandler?.HandleStopTestSessionComplete(payload.EventArgs);
                        return payload.EventArgs!.IsStopped;

                    case MessageType.TestMessage:
                        var testMessagePayload = _dataSerializer.DeserializePayload<TestMessagePayload>(message);
                        TPDebug.Assert(testMessagePayload is not null, "testMessagePayload is null");
                        eventsHandler?.HandleLogMessage(
                            testMessagePayload.MessageLevel,
                            testMessagePayload.Message);
                        break;

                    default:
                        EqtTrace.Warning(
                            "VsTestConsoleRequestSender.StopTestSession: Unexpected message received: {0}",
                            message.MessageType);
                        break;
                }
            }
        }
        catch (Exception exception)
        {
            EqtTrace.Error(
                "Aborting StopTestSession operation for id {0} due to error: {1}",
                testSessionInfo?.Id,
                exception);
            eventsHandler?.HandleLogMessage(
                TestMessageLevel.Error,
                TranslationLayerResources.AbortedStopTestSession);

            eventsHandler?.HandleStopTestSessionComplete(new(testSessionInfo));
        }
        finally
        {
            _testPlatformEventSource.TranslationLayerStopTestSessionStop();
        }

        return false;
    }

    /// <inheritdoc/>
    public void CancelTestRun()
    {
        EqtTrace.Info("VsTestConsoleRequestSender.CancelTestRun: Canceling test run.");

        _communicationManager.SendMessage(MessageType.CancelTestRun);
    }

    /// <inheritdoc/>
    public void AbortTestRun()
    {
        EqtTrace.Info("VsTestConsoleRequestSender.AbortTestRun: Aborting test run.");

        _communicationManager.SendMessage(MessageType.AbortTestRun);
    }

    /// <inheritdoc/>
    public void CancelDiscovery()
    {
        EqtTrace.Info("VsTestConsoleRequestSender.CancelDiscovery: Canceling test discovery.");

        _communicationManager.SendMessage(MessageType.CancelDiscovery);
    }

    /// <inheritdoc/>
    public void OnProcessExited()
    {
        _processExitCancellationTokenSource?.Cancel();
    }

    /// <inheritdoc/>
    public void Close()
    {
        Dispose();
    }

    /// <inheritdoc/>
    public void EndSession()
    {
        _communicationManager.SendMessage(MessageType.SessionEnd);
    }

    /// <inheritdoc/>
    public Task ProcessTestRunAttachmentsAsync(
        IEnumerable<AttachmentSet> attachments,
        IEnumerable<InvokedDataCollector>? invokedDataCollectors,
        string? runSettings,
        bool collectMetrics,
        ITestRunAttachmentsProcessingEventsHandler testSessionEventsHandler,
        CancellationToken cancellationToken)
    {
        return SendMessageAndListenAndReportAttachmentsProcessingResultAsync(
            attachments,
            invokedDataCollectors,
            runSettings,
            collectMetrics,
            testSessionEventsHandler,
            cancellationToken);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _communicationManager?.StopServer();
    }

    #endregion

    private bool HandShakeWithVsTestConsole()
    {
        var message = _communicationManager.ReceiveMessage();

        if (message?.MessageType != MessageType.SessionConnected)
        {
            EqtTrace.Error(
                "VsTestConsoleRequestSender.HandShakeWithVsTestConsole: SessionConnected Message Expected but different message received: Received MessageType: {0}",
                message?.MessageType);
            return false;
        }

        _communicationManager.SendMessage(
            MessageType.VersionCheck,
            _protocolVersion);

        message = _communicationManager.ReceiveMessage();
        var success = false;

        if (message?.MessageType == MessageType.VersionCheck)
        {
            _protocolVersion = _dataSerializer
                .DeserializePayload<int>(message);
            success = true;
        }
        else if (message?.MessageType == MessageType.ProtocolError)
        {
            // TODO : Payload for ProtocolError needs to finalized.
            EqtTrace.Error(
                "VsTestConsoleRequestSender.HandShakeWithVsTestConsole: Version Check failed. ProtolError was received from the runner");
        }
        else
        {
            EqtTrace.Error(
                "VsTestConsoleRequestSender.HandShakeWithVsTestConsole: VersionCheck Message Expected but different message received: Received MessageType: {0}",
                message?.MessageType);
        }

        return success;
    }

    private async Task<bool> HandShakeWithVsTestConsoleAsync()
    {
        TPDebug.Assert(_processExitCancellationTokenSource is not null, "_processExitCancellationTokenSource is null");
        var message = await _communicationManager.ReceiveMessageAsync(
            _processExitCancellationTokenSource.Token).ConfigureAwait(false);

        if (message?.MessageType != MessageType.SessionConnected)
        {
            EqtTrace.Error(
                "VsTestConsoleRequestSender.HandShakeWithVsTestConsoleAsync: SessionConnected Message Expected but different message received: Received MessageType: {0}",
                message?.MessageType);
            return false;
        }

        _communicationManager.SendMessage(
            MessageType.VersionCheck,
            _protocolVersion);

        message = await _communicationManager.ReceiveMessageAsync(
            _processExitCancellationTokenSource.Token).ConfigureAwait(false);

        var success = false;
        if (message?.MessageType == MessageType.VersionCheck)
        {
            _protocolVersion = _dataSerializer.DeserializePayload<int>(message);
            success = true;
        }
        else if (message?.MessageType == MessageType.ProtocolError)
        {
            // TODO : Payload for ProtocolError needs to finalized.
            EqtTrace.Error(
                "VsTestConsoleRequestSender.HandShakeWithVsTestConsoleAsync: Version Check failed. ProtolError was received from the runner");
        }
        else
        {
            EqtTrace.Error(
                "VsTestConsoleRequestSender.HandShakeWithVsTestConsoleAsync: VersionCheck Message Expected but different message received: Received MessageType: {0}",
                message?.MessageType);
        }

        return success;
    }

    private void SendMessageAndListenAndReportTestCases(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestDiscoveryEventsHandler2 eventHandler)
    {
        try
        {
            _communicationManager.SendMessage(
                MessageType.StartDiscovery,
                new DiscoveryRequestPayload()
                {
                    Sources = sources,
                    RunSettings = runSettings,
                    TestPlatformOptions = options,
                    TestSessionInfo = testSessionInfo
                },
                _protocolVersion);
            var isDiscoveryComplete = false;

            // Cycle through the messages that vstest.console sends.
            // Currently each operation is not a separate task since it should not take that
            // much time to complete.
            //
            // This is just a notification.
            while (!isDiscoveryComplete)
            {
                var message = TryReceiveMessage();

                if (string.Equals(MessageType.TestCasesFound, message.MessageType))
                {
                    var testCases = _dataSerializer
                        .DeserializePayload<IEnumerable<TestCase>>(message);

                    eventHandler.HandleDiscoveredTests(testCases);
                }
                else if (string.Equals(MessageType.DiscoveryComplete, message.MessageType))
                {
                    EqtTrace.Info(
                        "VsTestConsoleRequestSender.SendMessageAndListenAndReportTestCases: Discovery complete.");

                    var discoveryCompletePayload = _dataSerializer.DeserializePayload<DiscoveryCompletePayload>(message);
                    TPDebug.Assert(discoveryCompletePayload is not null, "discoveryCompletePayload is null");

                    var discoveryCompleteEventArgs = new DiscoveryCompleteEventArgs
                    {
                        TotalCount = discoveryCompletePayload.TotalTests,
                        IsAborted = discoveryCompletePayload.IsAborted,
                        FullyDiscoveredSources = discoveryCompletePayload.FullyDiscoveredSources,
                        PartiallyDiscoveredSources = discoveryCompletePayload.PartiallyDiscoveredSources,
                        NotDiscoveredSources = discoveryCompletePayload.NotDiscoveredSources,
                        SkippedDiscoveredSources = discoveryCompletePayload.SkippedDiscoverySources,
                        DiscoveredExtensions = discoveryCompletePayload.DiscoveredExtensions,
                        Metrics = discoveryCompletePayload.Metrics,
                    };

                    eventHandler.HandleDiscoveryComplete(
                        discoveryCompleteEventArgs,
                        discoveryCompletePayload.LastDiscoveredTests);
                    isDiscoveryComplete = true;
                }
                else if (string.Equals(MessageType.TestMessage, message.MessageType))
                {
                    var testMessagePayload = _dataSerializer.DeserializePayload<TestMessagePayload>(message);
                    TPDebug.Assert(testMessagePayload is not null, "testMessagePayload is null");
                    eventHandler.HandleLogMessage(
                        testMessagePayload.MessageLevel,
                        testMessagePayload.Message);
                }
            }
        }
        catch (Exception exception)
        {
            EqtTrace.Error("Aborting Test Discovery Operation: {0}", exception);
            eventHandler.HandleLogMessage(
                TestMessageLevel.Error,
                TranslationLayerResources.AbortedTestsDiscovery);
            var discoveryCompleteEventArgs = new DiscoveryCompleteEventArgs(-1, true);
            eventHandler.HandleDiscoveryComplete(discoveryCompleteEventArgs, null);

            // Earlier we were closing the connection with vstest.console in case of exceptions.
            // Removing that code because vstest.console might be in a healthy state and letting
            // the client know of the error, so that the TL can wait for the next instruction
            // from the client itself.
            // Also, connection termination might not kill the process which could result in
            // files being locked by testhost.
        }

        _testPlatformEventSource.TranslationLayerDiscoveryStop();
    }

    private async Task SendMessageAndListenAndReportTestCasesAsync(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestDiscoveryEventsHandler2 eventHandler)
    {
        try
        {
            _communicationManager.SendMessage(
                MessageType.StartDiscovery,
                new DiscoveryRequestPayload()
                {
                    Sources = sources,
                    RunSettings = runSettings,
                    TestPlatformOptions = options,
                    TestSessionInfo = testSessionInfo
                },
                _protocolVersion);
            var isDiscoveryComplete = false;

            // Cycle through the messages that vstest.console sends.
            // Currently each operation is not a separate task since it should not take that
            // much time to complete.
            //
            // This is just a notification.
            while (!isDiscoveryComplete)
            {
                var message = await TryReceiveMessageAsync().ConfigureAwait(false);

                if (string.Equals(MessageType.TestCasesFound, message.MessageType))
                {
                    var testCases = _dataSerializer
                        .DeserializePayload<IEnumerable<TestCase>>(message);

                    eventHandler.HandleDiscoveredTests(testCases);
                }
                else if (string.Equals(MessageType.DiscoveryComplete, message.MessageType))
                {
                    EqtTrace.Info(
                        "VsTestConsoleRequestSender.SendMessageAndListenAndReportTestCasesAsync: Discovery complete.");

                    var discoveryCompletePayload =
                        _dataSerializer.DeserializePayload<DiscoveryCompletePayload>(message);
                    TPDebug.Assert(discoveryCompletePayload is not null, "discoveryCompletePayload is null");
                    var discoveryCompleteEventArgs = new DiscoveryCompleteEventArgs
                    {
                        TotalCount = discoveryCompletePayload.TotalTests,
                        IsAborted = discoveryCompletePayload.IsAborted,
                        FullyDiscoveredSources = discoveryCompletePayload.FullyDiscoveredSources,
                        PartiallyDiscoveredSources = discoveryCompletePayload.PartiallyDiscoveredSources,
                        NotDiscoveredSources = discoveryCompletePayload.NotDiscoveredSources,
                        SkippedDiscoveredSources = discoveryCompletePayload.SkippedDiscoverySources,
                        DiscoveredExtensions = discoveryCompletePayload.DiscoveredExtensions,
                    };

                    // Adding Metrics from VsTestConsole
                    discoveryCompleteEventArgs.Metrics = discoveryCompletePayload.Metrics;

                    eventHandler.HandleDiscoveryComplete(
                        discoveryCompleteEventArgs,
                        discoveryCompletePayload.LastDiscoveredTests);
                    isDiscoveryComplete = true;
                }
                else if (string.Equals(MessageType.TestMessage, message.MessageType))
                {
                    var testMessagePayload = _dataSerializer
                        .DeserializePayload<TestMessagePayload>(message);
                    TPDebug.Assert(testMessagePayload is not null, "testMessagePayload is null");
                    eventHandler.HandleLogMessage(
                        testMessagePayload.MessageLevel,
                        testMessagePayload.Message);
                }
            }
        }
        catch (Exception exception)
        {
            EqtTrace.Error("Aborting Test Discovery Operation: {0}", exception);

            eventHandler.HandleLogMessage(
                TestMessageLevel.Error,
                TranslationLayerResources.AbortedTestsDiscovery);

            eventHandler.HandleDiscoveryComplete(new(-1, true), null);

            // Earlier we were closing the connection with vstest.console in case of exceptions.
            // Removing that code because vstest.console might be in a healthy state and letting
            // the client know of the error, so that the TL can wait for the next instruction
            // from the client itself.
            // Also, connection termination might not kill the process which could result in
            // files being locked by testhost.
        }

        _testPlatformEventSource.TranslationLayerDiscoveryStop();
    }

    private void SendMessageAndListenAndReportTestResults(
        string messageType,
        object payload,
        ITestRunEventsHandler eventHandler,
        ITelemetryEventsHandler telemetryEventsHandler,
        ITestHostLauncher? customHostLauncher)
    {
        try
        {
            _communicationManager.SendMessage(messageType, payload, _protocolVersion);
            var isTestRunComplete = false;

            // Cycle through the messages that vstest.console sends.
            // Currently each operation is not a separate task since it should not take that
            // much time to complete.
            //
            // This is just a notification.
            while (!isTestRunComplete)
            {
                var message = TryReceiveMessage();

                if (string.Equals(MessageType.TestRunStatsChange, message.MessageType))
                {
                    var testRunChangedArgs = _dataSerializer
                        .DeserializePayload<TestRunChangedEventArgs>(
                            message);
                    eventHandler.HandleTestRunStatsChange(testRunChangedArgs);
                }
                else if (string.Equals(MessageType.ExecutionComplete, message.MessageType))
                {
                    EqtTrace.Info(
                        "VsTestConsoleRequestSender.SendMessageAndListenAndReportTestResults: Execution complete.");

                    var testRunCompletePayload = _dataSerializer
                        .DeserializePayload<TestRunCompletePayload>(message);
                    TPDebug.Assert(testRunCompletePayload is not null, "testRunCompletePayload is null");
                    eventHandler.HandleTestRunComplete(
                        testRunCompletePayload.TestRunCompleteArgs!,
                        testRunCompletePayload.LastRunTests,
                        testRunCompletePayload.RunAttachments,
                        testRunCompletePayload.ExecutorUris);
                    isTestRunComplete = true;
                }
                else if (string.Equals(MessageType.TestMessage, message.MessageType))
                {
                    var testMessagePayload = _dataSerializer
                        .DeserializePayload<TestMessagePayload>(message);
                    TPDebug.Assert(testMessagePayload is not null, "testMessagePayload is null");
                    eventHandler.HandleLogMessage(
                        testMessagePayload.MessageLevel,
                        testMessagePayload.Message);
                }
                else if (string.Equals(MessageType.CustomTestHostLaunch, message.MessageType))
                {
                    HandleCustomHostLaunch(customHostLauncher, message);
                }
                else if (string.Equals(MessageType.EditorAttachDebugger, message.MessageType) || string.Equals(MessageType.EditorAttachDebugger2, message.MessageType))
                {
                    AttachDebuggerToProcess(customHostLauncher, message);
                }
                else if (string.Equals(MessageType.TelemetryEventMessage, message.MessageType))
                {
                    HandleTelemetryEvent(telemetryEventsHandler, message);
                }
            }
        }
        catch (Exception exception)
        {
            EqtTrace.Error("Aborting Test Run Operation: {0}", exception);
            eventHandler.HandleLogMessage(
                TestMessageLevel.Error,
                TranslationLayerResources.AbortedTestsRun + " " + exception.ToString());
            var completeArgs = new TestRunCompleteEventArgs(
                null, false, true, exception, null, null, TimeSpan.Zero);
            eventHandler.HandleTestRunComplete(completeArgs, null, null, null);

            // Earlier we were closing the connection with vstest.console in case of exceptions.
            // Removing that code because vstest.console might be in a healthy state and letting
            // the client know of the error, so that the TL can wait for the next instruction
            // from the client itself.
            // Also, connection termination might not kill the process which could result in
            // files being locked by testhost.
        }

        _testPlatformEventSource.TranslationLayerExecutionStop();
    }

    private async Task SendMessageAndListenAndReportTestResultsAsync(
        string messageType,
        object payload,
        ITestRunEventsHandler eventHandler,
        ITelemetryEventsHandler telemetryEventsHandler,
        ITestHostLauncher? customHostLauncher)
    {
        try
        {
            _communicationManager.SendMessage(messageType, payload, _protocolVersion);
            var isTestRunComplete = false;

            // Cycle through the messages that vstest.console sends.
            // Currently each operation is not a separate task since it should not take that
            // much time to complete.
            //
            // This is just a notification.
            while (!isTestRunComplete)
            {
                var message = await TryReceiveMessageAsync().ConfigureAwait(false);

                if (string.Equals(MessageType.TestRunStatsChange, message.MessageType))
                {
                    var testRunChangedArgs = _dataSerializer
                        .DeserializePayload<TestRunChangedEventArgs>(message);
                    eventHandler.HandleTestRunStatsChange(testRunChangedArgs);
                }
                else if (string.Equals(MessageType.ExecutionComplete, message.MessageType))
                {
                    EqtTrace.Info(
                        "VsTestConsoleRequestSender.SendMessageAndListenAndReportTestResultsAsync: Execution complete.");

                    var testRunCompletePayload = _dataSerializer
                        .DeserializePayload<TestRunCompletePayload>(message);
                    TPDebug.Assert(testRunCompletePayload is not null, "testRunCompletePayload is null");
                    eventHandler.HandleTestRunComplete(
                        testRunCompletePayload.TestRunCompleteArgs!,
                        testRunCompletePayload.LastRunTests,
                        testRunCompletePayload.RunAttachments,
                        testRunCompletePayload.ExecutorUris);
                    isTestRunComplete = true;
                }
                else if (string.Equals(MessageType.TestMessage, message.MessageType))
                {
                    var testMessagePayload = _dataSerializer.DeserializePayload<TestMessagePayload>(message);
                    TPDebug.Assert(testMessagePayload is not null, "testMessagePayload is null");
                    eventHandler.HandleLogMessage(
                        testMessagePayload.MessageLevel,
                        testMessagePayload.Message);
                }
                else if (string.Equals(MessageType.CustomTestHostLaunch, message.MessageType))
                {
                    HandleCustomHostLaunch(customHostLauncher, message);
                }
                else if (string.Equals(MessageType.EditorAttachDebugger, message.MessageType))
                {
                    AttachDebuggerToProcess(customHostLauncher, message);
                }
                else if (string.Equals(MessageType.TelemetryEventMessage, message.MessageType))
                {
                    HandleTelemetryEvent(telemetryEventsHandler, message);
                }
            }
        }
        catch (Exception exception)
        {
            EqtTrace.Error("Aborting Test Run Operation: {0}", exception);
            eventHandler.HandleLogMessage(
                TestMessageLevel.Error,
                TranslationLayerResources.AbortedTestsRun + " " + exception.ToString());
            var completeArgs = new TestRunCompleteEventArgs(
                null, false, true, exception, null, null, TimeSpan.Zero);
            eventHandler.HandleTestRunComplete(completeArgs, null, null, null);

            // Earlier we were closing the connection with vstest.console in case of exceptions.
            // Removing that code because vstest.console might be in a healthy state and letting
            // the client know of the error, so that the TL can wait for the next instruction
            // from the client itself.
            // Also, connection termination might not kill the process which could result in
            // files being locked by testhost.
        }

        _testPlatformEventSource.TranslationLayerExecutionStop();
    }

    private async Task SendMessageAndListenAndReportAttachmentsProcessingResultAsync(
        IEnumerable<AttachmentSet> attachments,
        IEnumerable<InvokedDataCollector>? invokedDataCollectors,
        string? runSettings,
        bool collectMetrics,
        ITestRunAttachmentsProcessingEventsHandler eventHandler,
        CancellationToken cancellationToken)
    {
        try
        {
            var payload = new TestRunAttachmentsProcessingPayload
            {
                Attachments = attachments,
                InvokedDataCollectors = invokedDataCollectors,
                RunSettings = runSettings,
                CollectMetrics = collectMetrics
            };

            _communicationManager.SendMessage(
                MessageType.TestRunAttachmentsProcessingStart,
                payload);
            var isTestRunAttachmentsProcessingComplete = false;

            using (cancellationToken.Register(() =>
                       _communicationManager.SendMessage(MessageType.TestRunAttachmentsProcessingCancel)))
            {
                // Cycle through the messages that vstest.console sends.
                // Currently each operation is not a separate task since it should not take that
                // much time to complete.
                //
                // This is just a notification.
                while (!isTestRunAttachmentsProcessingComplete)
                {
                    var message = await TryReceiveMessageAsync().ConfigureAwait(false);

                    if (string.Equals(
                            MessageType.TestRunAttachmentsProcessingComplete,
                            message.MessageType))
                    {
                        EqtTrace.Info(
                            "VsTestConsoleRequestSender.SendMessageAndListenAndReportAttachments: Process complete.");

                        var testRunAttachmentsProcessingCompletePayload = _dataSerializer
                            .DeserializePayload<TestRunAttachmentsProcessingCompletePayload>(message);
                        TPDebug.Assert(testRunAttachmentsProcessingCompletePayload is not null, "testRunAttachmentsProcessingCompletePayload is null");

                        eventHandler.HandleTestRunAttachmentsProcessingComplete(
                            testRunAttachmentsProcessingCompletePayload.AttachmentsProcessingCompleteEventArgs!,
                            testRunAttachmentsProcessingCompletePayload.Attachments);
                        isTestRunAttachmentsProcessingComplete = true;
                    }
                    else if (string.Equals(
                                 MessageType.TestRunAttachmentsProcessingProgress,
                                 message.MessageType))
                    {
                        var testRunAttachmentsProcessingProgressPayload = _dataSerializer
                            .DeserializePayload<TestRunAttachmentsProcessingProgressPayload>(message);
                        TPDebug.Assert(testRunAttachmentsProcessingProgressPayload is not null, "testRunAttachmentsProcessingProgressPayload is null");

                        eventHandler.HandleTestRunAttachmentsProcessingProgress(
                            testRunAttachmentsProcessingProgressPayload.AttachmentsProcessingProgressEventArgs!);
                    }
                    else if (string.Equals(MessageType.TestMessage, message.MessageType))
                    {
                        var testMessagePayload = _dataSerializer.DeserializePayload<TestMessagePayload>(message);
                        TPDebug.Assert(testMessagePayload is not null, "testMessagePayload is null");

                        eventHandler.HandleLogMessage(
                            testMessagePayload.MessageLevel,
                            testMessagePayload.Message);
                    }
                    else
                    {
                        EqtTrace.Warning(
                            $"VsTestConsoleRequestSender.SendMessageAndListenAndReportAttachments: Unexpected message received {message.MessageType}.");
                    }
                }
            }
        }
        catch (Exception exception)
        {
            EqtTrace.Error("Aborting Test Session End Operation: {0}", exception);
            eventHandler.HandleLogMessage(
                TestMessageLevel.Error,
                TranslationLayerResources.AbortedTestRunAttachmentsProcessing);
            eventHandler.HandleTestRunAttachmentsProcessingComplete(
                new TestRunAttachmentsProcessingCompleteEventArgs(false, exception),
                null);

            // Earlier we were closing the connection with vstest.console in case of exceptions.
            // Removing that code because vstest.console might be in a healthy state and letting
            // the client know of the error, so that the TL can wait for the next instruction
            // from the client itself.
            // Also, connection termination might not kill the process which could result in
            // files being locked by testhost.
        }
        finally
        {
            _testPlatformEventSource.TranslationLayerTestRunAttachmentsProcessingStop();
        }
    }

    private Message TryReceiveMessage()
    {
        return TryReceiveMessageAsync().GetAwaiter().GetResult();
    }

    private async Task<Message> TryReceiveMessageAsync()
    {
        // TODO: Rework logic of this class to avoid relying on throwing/catching exceptions:
        // - NRE on null _processExitCancellationTokenSource
        // - TransationLayerException on null message
        Message? message = await _communicationManager.ReceiveMessageAsync(_processExitCancellationTokenSource!.Token)
            .ConfigureAwait(false);

        return message ?? throw new TransationLayerException(TranslationLayerResources.FailedToReceiveMessage);
    }

    private void HandleCustomHostLaunch(ITestHostLauncher? customHostLauncher, Message message)
    {
        var ackPayload = new CustomHostLaunchAckPayload()
        {
            HostProcessId = -1,
            ErrorMessage = null
        };

        try
        {
            var testProcessStartInfo = _dataSerializer.DeserializePayload<TestProcessStartInfo>(message);

            ackPayload.HostProcessId = customHostLauncher?.LaunchTestHost(testProcessStartInfo!) ?? -1;
        }
        catch (Exception ex)
        {
            EqtTrace.Error("Error while launching custom host: {0}", ex);

            // Vstest.console will send the abort message properly while cleaning up all the
            // flow, so do not abort here.
            // Let the ack go through and let vstest.console handle the error.
            ackPayload.ErrorMessage = ex.Message;
        }
        finally
        {
            // Always unblock the vstest.console thread which is indefinitely waiting on this
            // ACK.
            _communicationManager.SendMessage(
                MessageType.CustomTestHostLaunchCallback,
                ackPayload,
                _protocolVersion);
        }
    }

    private void AttachDebuggerToProcess(ITestHostLauncher? customHostLauncher, Message message)
    {
        var ackPayload = new EditorAttachDebuggerAckPayload()
        {
            Attached = false,
            ErrorMessage = null
        };

        try
        {
            // Handle EditorAttachDebugger2.
            if (message.MessageType == MessageType.EditorAttachDebugger2)
            {
                var attachDebuggerPayload = _dataSerializer.DeserializePayload<EditorAttachDebuggerPayload>(message);
                TPDebug.Assert(attachDebuggerPayload is not null, "attachDebuggerPayload is null");
                switch (customHostLauncher)
                {
                    case ITestHostLauncher3 launcher3:
                        var attachDebuggerInfo = new AttachDebuggerInfo
                        {
                            ProcessId = attachDebuggerPayload.ProcessID,
                            TargetFramework = attachDebuggerPayload.TargetFramework,
                            Sources = attachDebuggerPayload.Sources,
                        };
                        ackPayload.Attached = launcher3.AttachDebuggerToProcess(attachDebuggerInfo, CancellationToken.None);
                        break;
                    case ITestHostLauncher2 launcher2:
                        ackPayload.Attached = launcher2.AttachDebuggerToProcess(attachDebuggerPayload.ProcessID);
                        break;
                    default:
                        // TODO: Maybe we should do something, but the rest of the story is broken, so it's better to not block users.
                        break;
                }
            }

            // Handle EditorAttachDebugger.
            if (message.MessageType == MessageType.EditorAttachDebugger)
            {
                var pid = _dataSerializer.DeserializePayload<int>(message);

                switch (customHostLauncher)
                {
                    case ITestHostLauncher3 launcher3:
                        ackPayload.Attached = launcher3.AttachDebuggerToProcess(new AttachDebuggerInfo { ProcessId = pid }, CancellationToken.None);
                        break;
                    case ITestHostLauncher2 launcher2:
                        ackPayload.Attached = launcher2.AttachDebuggerToProcess(pid);
                        break;
                    default:
                        // TODO: Maybe we should do something, but the rest of the story is broken, so it's better to not block users.
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            EqtTrace.Error("VsTestConsoleRequestSender.AttachDebuggerToProcess: Error while attaching debugger to process: {0}", ex);

            // vstest.console will send the abort message properly while cleaning up all the
            // flow, so do not abort here.
            // Let the ack go through and let vstest.console handle the error.
            ackPayload.ErrorMessage = ex.Message;
        }
        finally
        {
            // Always unblock the vstest.console thread which is indefintitely waiting on this
            // ACK.
            _communicationManager.SendMessage(
                MessageType.EditorAttachDebuggerCallback,
                ackPayload,
                _protocolVersion);
        }
    }

    private void HandleTelemetryEvent(ITelemetryEventsHandler telemetryEventsHandler, Message message)
    {
        try
        {
            TelemetryEvent? telemetryEvent = _dataSerializer.DeserializePayload<TelemetryEvent>(message);
            if (telemetryEvent is not null)
            {
                telemetryEventsHandler.HandleTelemetryEvent(telemetryEvent);
            }
        }
        catch (Exception ex)
        {
            EqtTrace.Error("VsTestConsoleRequestSender.HandleTelemetryEvent: Error while handling telemetry event: {0}", ex);
        }
    }
}
