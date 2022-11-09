// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.Client.RequestHelper;
using Microsoft.VisualStudio.TestPlatform.Client.TestRunAttachmentsProcessing;
using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.Common.Logging;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Payloads;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

using CommunicationUtilitiesResources = Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.Client.DesignMode;

/// <summary>
/// The design mode client, which listens inside of vstest.console when we are running in server mode (e.g. under an IDE like VS), and proceses
/// the incoming requests. See <see cref="ProcessRequests"/> for how the requests are received, and for example <see cref="StartTestRun"/>
/// for how a single request is handled, and a result is sent back to the IDE or other consumer.
/// </summary>
public class DesignModeClient : IDesignModeClient
{
    private readonly ICommunicationManager _communicationManager;
    private readonly IDataSerializer _dataSerializer;

    private readonly ProtocolConfig _protocolConfig = Constants.DefaultProtocolConfig;
    private readonly IEnvironment _platformEnvironment;
    private readonly TestSessionMessageLogger _testSessionMessageLogger;
    private readonly object _lockObject = new();
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Part of the public API.")]
    [SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "Part of the public API")]
    protected Action<Message>? onCustomTestHostLaunchAckReceived;
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Part of the public API.")]
    [SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "Part of the public API")]
    protected Action<Message>? onAttachDebuggerAckRecieved;

    /// <summary>
    /// Initializes a new instance of the <see cref="DesignModeClient"/> class.
    /// </summary>
    public DesignModeClient()
        : this(new SocketCommunicationManager(), JsonDataSerializer.Instance, new PlatformEnvironment())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DesignModeClient"/> class.
    /// </summary>
    /// <param name="communicationManager">
    /// The communication manager.
    /// </param>
    /// <param name="dataSerializer">
    /// The data Serializer.
    /// </param>
    /// <param name="platformEnvironment">
    /// The platform Environment
    /// </param>
    internal DesignModeClient(ICommunicationManager communicationManager, IDataSerializer dataSerializer, IEnvironment platformEnvironment)
    {
        _communicationManager = communicationManager;
        _dataSerializer = dataSerializer;
        _platformEnvironment = platformEnvironment;
        _testSessionMessageLogger = TestSessionMessageLogger.Instance;
        _testSessionMessageLogger.TestRunMessage += TestRunMessageHandler;
    }

    /// <summary>
    /// Property exposing the Instance
    /// </summary>
    public static DesignModeClient? Instance { get; private set; }

    /// <summary>
    /// Gets the test request manager.
    /// </summary>
    public ITestRequestManager? TestRequestManager { get; internal set; }

    /// <summary>
    /// Initializes DesignMode
    /// </summary>
    [MemberNotNull(nameof(Instance))]
    public static void Initialize()
    {
        Instance = new DesignModeClient();
    }

    /// <summary>
    /// Creates a client and waits for server to accept connection asynchronously
    /// </summary>
    /// <param name="port">
    /// Port number to connect
    /// </param>
    /// <param name="testRequestManager">
    /// The test Request Manager.
    /// </param>
    public void ConnectToClientAndProcessRequests(int port, ITestRequestManager testRequestManager)
    {
        // Used by the in-process vstest.console wrapper.
        TestRequestManager = testRequestManager;

        EqtTrace.Info("Trying to connect to server on port : {0}", port);
        _communicationManager.SetupClientAsync(new IPEndPoint(IPAddress.Loopback, port));

        var connectionTimeoutInSecs = EnvironmentHelper.GetConnectionTimeout();

        // Wait for the connection to the server and listen for requests.
        if (_communicationManager.WaitForServerConnection(connectionTimeoutInSecs * 1000))
        {
            _communicationManager.SendMessage(MessageType.SessionConnected);
            ProcessRequests(testRequestManager);
        }
        else
        {
            EqtTrace.Error("DesignModeClient.ConnectToClientAndProcessRequests: Client timed out while connecting to the server.");
            Dispose();
            throw new TimeoutException(
                string.Format(
                    CultureInfo.CurrentCulture,
                    CommunicationUtilitiesResources.ConnectionTimeoutErrorMessage,
                    CoreUtilities.Constants.VstestConsoleProcessName,
                    "translation layer",
                    connectionTimeoutInSecs,
                    EnvironmentHelper.VstestConnectionTimeout)
            );
        }
    }

    public void HandleParentProcessExit()
    {
        // Dispose off the communications to end the session
        // this should end the "ProcessRequests" loop with an exception
        Dispose();

        EqtTrace.Info("DesignModeClient.HandleParentProcessExit: Parent process exited, Exiting myself...");

        _platformEnvironment.Exit(1);
    }

    /// <summary>
    /// Process Requests from the IDE
    /// </summary>
    /// <param name="testRequestManager">
    /// The test Request Manager.
    /// </param>
    private void ProcessRequests(ITestRequestManager testRequestManager)
    {
        var isSessionEnd = false;

        do
        {
            try
            {
                var message = _communicationManager.ReceiveMessage();

                EqtTrace.Info("DesignModeClient.ProcessRequests: Processing Message: {0}", message);

                switch (message?.MessageType)
                {
                    case MessageType.VersionCheck:
                        {
                            var version = _dataSerializer.DeserializePayload<int>(message);
                            _protocolConfig.Version = Math.Min(version, _protocolConfig.Version);
                            _communicationManager.SendMessage(MessageType.VersionCheck, _protocolConfig.Version);
                            break;
                        }

                    case MessageType.ExtensionsInitialize:
                        {
                            // Do not filter the Editor/IDE provided extensions by name
                            var extensionPaths = _communicationManager.DeserializePayload<IEnumerable<string>>(message);
                            testRequestManager.InitializeExtensions(extensionPaths, skipExtensionFilters: true);
                            break;
                        }

                    case MessageType.StartTestSession:
                        {
                            var testSessionPayload = _communicationManager.DeserializePayload<StartTestSessionPayload>(message);
                            StartTestSession(testSessionPayload, testRequestManager);
                            break;
                        }

                    case MessageType.StopTestSession:
                        {
                            var testSessionPayload = _communicationManager.DeserializePayload<StopTestSessionPayload>(message);
                            StopTestSession(testSessionPayload, testRequestManager);
                            break;
                        }

                    case MessageType.StartDiscovery:
                        {
                            var discoveryPayload = _dataSerializer.DeserializePayload<DiscoveryRequestPayload>(message);
                            StartDiscovery(discoveryPayload, testRequestManager);
                            break;
                        }

                    case MessageType.GetTestRunnerProcessStartInfoForRunAll:
                    case MessageType.GetTestRunnerProcessStartInfoForRunSelected:
                        {
                            var testRunPayload = _communicationManager.DeserializePayload<TestRunRequestPayload>(message);
                            StartTestRun(testRunPayload, testRequestManager, shouldLaunchTesthost: true);
                            break;
                        }

                    case MessageType.TestRunAllSourcesWithDefaultHost:
                    case MessageType.TestRunSelectedTestCasesDefaultHost:
                        {
                            var testRunPayload = _communicationManager.DeserializePayload<TestRunRequestPayload>(message);
                            StartTestRun(testRunPayload, testRequestManager, shouldLaunchTesthost: false);
                            break;
                        }

                    case MessageType.TestRunAttachmentsProcessingStart:
                        {
                            var testRunAttachmentsProcessingPayload = _communicationManager.DeserializePayload<TestRunAttachmentsProcessingPayload>(message);
                            StartTestRunAttachmentsProcessing(testRunAttachmentsProcessingPayload, testRequestManager);
                            break;
                        }

                    case MessageType.CancelDiscovery:
                        {
                            testRequestManager.CancelDiscovery();
                            break;
                        }

                    case MessageType.CancelTestRun:
                        {
                            testRequestManager.CancelTestRun();
                            break;
                        }

                    case MessageType.AbortTestRun:
                        {
                            testRequestManager.AbortTestRun();
                            break;
                        }

                    case MessageType.TestRunAttachmentsProcessingCancel:
                        {
                            testRequestManager.CancelTestRunAttachmentsProcessing();
                            break;
                        }

                    case MessageType.CustomTestHostLaunchCallback:
                        {
                            onCustomTestHostLaunchAckReceived?.Invoke(message);
                            break;
                        }

                    case MessageType.EditorAttachDebuggerCallback:
                        {
                            onAttachDebuggerAckRecieved?.Invoke(message);
                            break;
                        }

                    case MessageType.SessionEnd:
                        {
                            EqtTrace.Info("DesignModeClient: Session End message received from server. Closing the connection.");
                            isSessionEnd = true;
                            Dispose();
                            break;
                        }

                    default:
                        {
                            EqtTrace.Info("DesignModeClient: Invalid Message received: {0}", message);
                            if (message is null)
                            {
                                Stop();
                            }
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                EqtTrace.Error("DesignModeClient: Error processing request: {0}", ex);
                Stop();
            }
        }
        while (!isSessionEnd);

        void Stop()
        {
            isSessionEnd = true;
            Dispose();
        }
    }

    /// <summary>
    /// Send a custom host launch message to IDE
    /// </summary>
    /// <param name="testProcessStartInfo">
    /// The test Process Start Info.
    /// </param>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    /// <returns>
    /// The <see cref="int"/>.
    /// </returns>
    public int LaunchCustomHost(TestProcessStartInfo testProcessStartInfo, CancellationToken cancellationToken)
    {
        lock (_lockObject)
        {
            var waitHandle = new AutoResetEvent(false);
            Message? ackMessage = null;
            onCustomTestHostLaunchAckReceived = (ackRawMessage) =>
            {
                ackMessage = ackRawMessage;
                waitHandle.Set();
            };

            _communicationManager.SendMessage(MessageType.CustomTestHostLaunch, testProcessStartInfo);

            // LifeCycle of the TP through DesignModeClient is maintained by the IDEs or user-facing-clients like LUTs, who call TestPlatform
            // TP is handing over the control of launch to these IDEs and so, TP has to wait indefinite
            // Even if TP has a timeout here, there is no way TP can abort or stop the thread/task that is hung in IDE or LUT
            // Even if TP can abort the API somehow, TP is essentially putting IDEs or Clients in inconsistent state without having info on
            // Since the IDEs own user-UI-experience here, TP will let the custom host launch as much time as IDEs define it for their users
            WaitHandle.WaitAny(new WaitHandle[] { waitHandle, cancellationToken.WaitHandle });

            cancellationToken.ThrowTestPlatformExceptionIfCancellationRequested();

            onCustomTestHostLaunchAckReceived = null;

            TPDebug.Assert(ackMessage is not null, "ackMessage is null");
            var ackPayload = _dataSerializer.DeserializePayload<CustomHostLaunchAckPayload>(ackMessage);
            TPDebug.Assert(ackPayload is not null, "ackPayload is null");

            return ackPayload.HostProcessId > 0 ? ackPayload.HostProcessId : throw new TestPlatformException(ackPayload.ErrorMessage);
        }
    }

    /// <inheritdoc/>
    public bool AttachDebuggerToProcess(AttachDebuggerInfo attachDebuggerInfo, CancellationToken cancellationToken)
    {
        // If an attach request is issued but there is no support for attaching on the other
        // side of the communication channel, we simply return and let the caller know the
        // request failed.
        if (_protocolConfig.Version < ObjectModel.Constants.MinimumProtocolVersionWithDebugSupport)
        {
            return false;
        }

        lock (_lockObject)
        {
            var waitHandle = new AutoResetEvent(false);
            Message? ackMessage = null;
            onAttachDebuggerAckRecieved = ackRawMessage =>
            {
                ackMessage = ackRawMessage;
                waitHandle.Set();
            };

            // TODO: formalize this so we can the deprecation version from the message data, and automatically switch
            // to the new message, including type safety, where we determine T from the payload. And maybe give SendMessage
            // a type of T as well to prevent some more mistakes.
            if (_protocolConfig.Version < 7)
            {
                _communicationManager.SendMessage(MessageType.EditorAttachDebugger, attachDebuggerInfo.ProcessId, _protocolConfig.Version);
            }
            else
            {
                var payload = new EditorAttachDebuggerPayload
                {
                    Sources = attachDebuggerInfo.Sources,
                    TargetFramework = attachDebuggerInfo.TargetFramework?.ToString(),
                    ProcessID = attachDebuggerInfo.ProcessId,
                };
                _communicationManager.SendMessage(MessageType.EditorAttachDebugger2, payload);
            }

            WaitHandle.WaitAny(new WaitHandle[] { waitHandle, cancellationToken.WaitHandle });

            cancellationToken.ThrowTestPlatformExceptionIfCancellationRequested();
            onAttachDebuggerAckRecieved = null;

            TPDebug.Assert(ackMessage is not null, "ackMessage is null");
            var ackPayload = _dataSerializer.DeserializePayload<EditorAttachDebuggerAckPayload>(ackMessage);
            TPDebug.Assert(ackPayload is not null, "ackPayload is null");
            if (!ackPayload.Attached)
            {
                EqtTrace.Warning($"DesignModeClient.AttachDebuggerToProcess: Attaching to process failed: {ackPayload.ErrorMessage}");
            }

            return ackPayload.Attached;
        }
    }

    /// <summary>
    /// Send the raw messages to IDE
    /// </summary>
    /// <param name="rawMessage"></param>
    public void SendRawMessage(string rawMessage)
    {
        _communicationManager.SendRawMessage(rawMessage);
    }

    /// <inheritdoc />
    public void SendTestMessage(TestMessageLevel level, string? message)
    {
        var payload = new TestMessagePayload { MessageLevel = level, Message = message };
        _communicationManager.SendMessage(MessageType.TestMessage, payload);
    }

    /// <summary>
    /// Sends the test session logger warning and error messages to IDE;
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void TestRunMessageHandler(object? sender, TestRunMessageEventArgs e)
    {
        // save into trace log and send the message to the IDE
        //
        // there is a mismatch between log levels that VS uses and that TP
        // uses. In VS you can choose Trace level which will enable Test platform
        // logs on Verbose level. Below we report Errors and warnings always to the
        // IDE no matter what the level of VS logging is, but Info only when the Eqt trace
        // info level is enabled (so only when VS enables Trace logging)
        switch (e.Level)
        {
            case TestMessageLevel.Error:
                EqtTrace.Error(e.Message);
                SendTestMessage(e.Level, e.Message);
                break;
            case TestMessageLevel.Warning:
                EqtTrace.Warning(e.Message);
                SendTestMessage(e.Level, e.Message);
                break;

            case TestMessageLevel.Informational:
                EqtTrace.Info(e.Message);

                if (EqtTrace.IsInfoEnabled)
                    SendTestMessage(e.Level, e.Message);
                break;


            default:
                throw new NotSupportedException($"Test message level '{e.Level}' is not supported.");
        }
    }

    private void StartTestRun(TestRunRequestPayload? testRunPayload, ITestRequestManager testRequestManager, bool shouldLaunchTesthost)
    {
        Task.Run(() =>
        {
            try
            {
                testRequestManager.ResetOptions();

                if (testRunPayload is null)
                {
                    OnError(null);
                    return;
                }

                // We must avoid re-launching the test host if the test run payload already
                // contains test session info. Test session info being present is an indicative
                // of an already running test host spawned by a start test session call.
                var customLauncher =
                    shouldLaunchTesthost && testRunPayload!.TestSessionInfo == null // TODO: Avoid throwing/catching NRE
                        ? DesignModeTestHostLauncherFactory.GetCustomHostLauncherForTestRun(
                            this,
                            testRunPayload.DebuggingEnabled)
                        : null;

                testRequestManager.RunTests(testRunPayload!, customLauncher, new DesignModeTestEventsRegistrar(this), _protocolConfig);
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        });

        void OnError(Exception? ex)
        {
            EqtTrace.Error("DesignModeClient.StartTestRun: " + ex ?? "payload was null");

            var testMessagePayload = new TestMessagePayload { MessageLevel = TestMessageLevel.Error, Message = ex?.ToString() };
            _communicationManager.SendMessage(MessageType.TestMessage, testMessagePayload);
            var runCompletePayload = new TestRunCompletePayload()
            {
                TestRunCompleteArgs = new TestRunCompleteEventArgs(null, false, true, ex, null, null, TimeSpan.MinValue),
                LastRunTests = null
            };

            // Send run complete to translation layer
            _communicationManager.SendMessage(MessageType.ExecutionComplete, runCompletePayload);
        }
    }

    private void StartDiscovery(DiscoveryRequestPayload? discoveryRequestPayload, ITestRequestManager testRequestManager)
    {
        Task.Run(() =>
        {
            try
            {
                testRequestManager.ResetOptions();
                if (discoveryRequestPayload is null)
                {
                    OnError(null);
                    return;
                }

                testRequestManager.DiscoverTests(discoveryRequestPayload, new DesignModeTestEventsRegistrar(this), _protocolConfig);
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        });

        void OnError(Exception? ex)
        {
            EqtTrace.Error("DesignModeClient.StartDiscovery: " + ex ?? "payload is null");

            var testMessagePayload = new TestMessagePayload { MessageLevel = TestMessageLevel.Error, Message = ex?.ToString() };
            _communicationManager.SendMessage(MessageType.TestMessage, testMessagePayload);

            var payload = new DiscoveryCompletePayload()
            {
                IsAborted = true,
                LastDiscoveredTests = null,
                TotalTests = -1
            };

            // Send run complete to translation layer
            _communicationManager.SendMessage(MessageType.DiscoveryComplete, payload);
        }
    }

    private void StartTestRunAttachmentsProcessing(TestRunAttachmentsProcessingPayload? attachmentsProcessingPayload, ITestRequestManager testRequestManager)
    {
        Task.Run(() =>
        {
            try
            {
                if (attachmentsProcessingPayload is null)
                {
                    OnError(null);
                    return;
                }

                testRequestManager.ProcessTestRunAttachments(attachmentsProcessingPayload, new TestRunAttachmentsProcessingEventsHandler(_communicationManager), _protocolConfig);
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        });

        void OnError(Exception? ex)
        {
            EqtTrace.Error("DesignModeClient.StartTestRunAttachmentsProcessing: " + ex ?? "payload is null");

            var testMessagePayload = new TestMessagePayload { MessageLevel = TestMessageLevel.Error, Message = ex?.ToString() };
            _communicationManager.SendMessage(MessageType.TestMessage, testMessagePayload);

            var payload = new TestRunAttachmentsProcessingCompletePayload()
            {
                Attachments = null
            };

            // Send run complete to translation layer
            _communicationManager.SendMessage(MessageType.TestRunAttachmentsProcessingComplete, payload);
        }
    }

    private void StartTestSession(StartTestSessionPayload? payload, ITestRequestManager requestManager)
    {
        Task.Run(() =>
        {
            var eventsHandler = new TestSessionEventsHandler(_communicationManager);

            try
            {
                if (payload is null)
                {
                    OnError(eventsHandler, null);
                    return;
                }

                var customLauncher = payload.HasCustomHostLauncher
                    ? DesignModeTestHostLauncherFactory.GetCustomHostLauncherForTestRun(this, payload.IsDebuggingEnabled)
                    : null;

                requestManager.ResetOptions();
                requestManager.StartTestSession(payload, customLauncher, eventsHandler, _protocolConfig);
            }
            catch (Exception ex)
            {
                OnError(eventsHandler, ex);
            }
        });

        static void OnError(TestSessionEventsHandler eventsHandler, Exception? ex)
        {
            EqtTrace.Error("DesignModeClient.StartTestSession: " + ex ?? "payload is null");

            eventsHandler.HandleLogMessage(TestMessageLevel.Error, ex?.ToString());
            eventsHandler.HandleStartTestSessionComplete(new());
        }
    }

    private void StopTestSession(StopTestSessionPayload? payload, ITestRequestManager requestManager)
    {
        Task.Run(() =>
        {
            var eventsHandler = new TestSessionEventsHandler(_communicationManager);

            try
            {
                requestManager.ResetOptions();
                if (payload is null)
                {
                    OnError(eventsHandler, null);
                    return;
                }

                requestManager.StopTestSession(payload, eventsHandler, _protocolConfig);
            }
            catch (Exception ex)
            {
                OnError(eventsHandler, ex);
            }
        });

        void OnError(TestSessionEventsHandler eventsHandler, Exception? ex)
        {
            EqtTrace.Error("DesignModeClient.StopTestSession: " + ex ?? "payload is null");

            eventsHandler.HandleLogMessage(TestMessageLevel.Error, ex?.ToString());
            eventsHandler.HandleStopTestSessionComplete(new(payload?.TestSessionInfo));
        }
    }

    #region IDisposable Support

    private bool _isDisposed; // To detect redundant calls

    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                _communicationManager.StopClient();
            }

            _isDisposed = true;
        }
    }

    // This code added to correctly implement the disposable pattern.
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    #endregion
}
