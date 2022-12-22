// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.EventHandlers;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.EventHandlers;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.TesthostProtocol;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

using CrossPlatResources = Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Resources.Resources;
using ObjectModelConstants = Microsoft.VisualStudio.TestPlatform.ObjectModel.Constants;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

/// <summary>
/// Listens inside of testhost for requests, that are sent from vstest.console. Requests are handled in <see cref="OnMessageReceived"/>
/// and responses are sent back via various methods, for example <see cref="SendExecutionComplete"/>.
/// </summary>
public class TestRequestHandler : ITestRequestHandler, IDeploymentAwareTestRequestHandler
{
    private readonly int _highestSupportedVersion = ProtocolVersioning.HighestSupportedVersion;
    private readonly IDataSerializer _dataSerializer;
    private readonly ICommunicationEndpointFactory _communicationEndpointFactory;
    private readonly JobQueue<Action> _jobQueue;
    private readonly IFileHelper _fileHelper;
    private readonly ManualResetEventSlim _requestSenderConnected;
    private readonly ManualResetEventSlim _testHostManagerFactoryReady;
    private readonly ManualResetEventSlim _sessionCompleted;

    private int _protocolVersion = 1;
    private ITestHostManagerFactory? _testHostManagerFactory;
    private ICommunicationEndPoint? _communicationEndPoint;
    private ICommunicationChannel? _channel;
    private Action<Message>? _onLaunchAdapterProcessWithDebuggerAttachedAckReceived;
    private Action<Message>? _onAttachDebuggerAckRecieved;
    private IPathConverter _pathConverter;
    private Exception? _messageProcessingUnrecoverableError;
    private bool _isDisposed;

    public TestHostConnectionInfo ConnectionInfo { get; set; }
    string? IDeploymentAwareTestRequestHandler.LocalPath { get; set; }
    string? IDeploymentAwareTestRequestHandler.RemotePath { get; set; }

    public TestRequestHandler() : this(JsonDataSerializer.Instance, new CommunicationEndpointFactory())
    {
    }

    protected TestRequestHandler(
        TestHostConnectionInfo connectionInfo,
        ICommunicationEndpointFactory communicationEndpointFactory,
        IDataSerializer dataSerializer,
        JobQueue<Action> jobQueue,
        Action<Message> onLaunchAdapterProcessWithDebuggerAttachedAckReceived,
        Action<Message> onAttachDebuggerAckRecieved)
    {
        _communicationEndpointFactory = communicationEndpointFactory;
        ConnectionInfo = connectionInfo;
        _dataSerializer = dataSerializer;
        _requestSenderConnected = new ManualResetEventSlim(false);
        _testHostManagerFactoryReady = new ManualResetEventSlim(false);
        _sessionCompleted = new ManualResetEventSlim(false);
        _onLaunchAdapterProcessWithDebuggerAttachedAckReceived = onLaunchAdapterProcessWithDebuggerAttachedAckReceived;
        _onAttachDebuggerAckRecieved = onAttachDebuggerAckRecieved;
        _jobQueue = jobQueue;

        _fileHelper = new FileHelper();
        _pathConverter = NullPathConverter.Instance;
    }

    protected TestRequestHandler(IDataSerializer dataSerializer, ICommunicationEndpointFactory communicationEndpointFactory)
    {
        _dataSerializer = dataSerializer;
        _communicationEndpointFactory = communicationEndpointFactory;
        _requestSenderConnected = new ManualResetEventSlim(false);
        _sessionCompleted = new ManualResetEventSlim(false);
        _testHostManagerFactoryReady = new ManualResetEventSlim(false);
        _onLaunchAdapterProcessWithDebuggerAttachedAckReceived = (message) => throw new NotImplementedException();
        _onAttachDebuggerAckRecieved = (message) => throw new NotImplementedException();

        _jobQueue = new JobQueue<Action>(
            action => action?.Invoke(),
            "TestHostOperationQueue",
            500,
            25000000,
            true,
            message => EqtTrace.Error(message));

        _fileHelper = new FileHelper();
        _pathConverter = NullPathConverter.Instance;
    }

    /// <inheritdoc />
    public virtual void InitializeCommunication()
    {
        if (this is IDeploymentAwareTestRequestHandler self)
        {
            var currentProcessPath = Process.GetCurrentProcess().MainModule?.FileName;
            var currentProcessDirectory = !currentProcessPath.IsNullOrWhiteSpace()
                ? System.IO.Path.GetDirectoryName(currentProcessPath)
                : null;
            var remotePath = Environment.GetEnvironmentVariable("VSTEST_UWP_DEPLOY_REMOTE_PATH") ?? self.RemotePath ?? currentProcessDirectory;

            var localPath = Environment.GetEnvironmentVariable("VSTEST_UWP_DEPLOY_LOCAL_PATH") ?? self.LocalPath;
            if (!localPath.IsNullOrWhiteSpace()
                && !remotePath.IsNullOrWhiteSpace())
            {
                _pathConverter = new PathConverter(localPath, remotePath, _fileHelper);
            }
        }

        _communicationEndPoint = _communicationEndpointFactory.Create(ConnectionInfo.Role);
        _communicationEndPoint.Connected += (sender, connectedArgs) =>
        {
            if (!connectedArgs.Connected)
            {
                _requestSenderConnected.Set();
                throw connectedArgs.Fault;
            }
            _channel = connectedArgs.Channel;
            _channel.MessageReceived += OnMessageReceived;
            _requestSenderConnected.Set();
        };

        _communicationEndPoint.Start(ConnectionInfo.Endpoint);
    }

    /// <inheritdoc />
    public bool WaitForRequestSenderConnection(int connectionTimeout)
    {
        return _requestSenderConnected.Wait(connectionTimeout);
    }

    /// <inheritdoc />
    public void ProcessRequests(ITestHostManagerFactory testHostManagerFactory)
    {
        _testHostManagerFactory = testHostManagerFactory;
        _testHostManagerFactoryReady.Set();
        _sessionCompleted.Wait();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
            return;

        if (disposing)
        {
            _communicationEndPoint?.Stop();
            _channel?.Dispose();
        }

        _isDisposed = true;
    }

    /// <inheritdoc />
    public void Close()
    {
        Dispose();
        EqtTrace.Info("Closing the connection !");
    }

    /// <inheritdoc />
    public void SendTestCases(IEnumerable<TestCase>? discoveredTestCases)
    {
        var updatedTestCases = _pathConverter.UpdateTestCases(discoveredTestCases, PathConversionDirection.Send);
        var data = _dataSerializer.SerializePayload(MessageType.TestCasesFound, updatedTestCases, _protocolVersion);
        SendData(data);
    }

    /// <inheritdoc />
    public void SendTestRunStatistics(TestRunChangedEventArgs? testRunChangedArgs)
    {
        var updatedTestRunChangedEventArgs = _pathConverter.UpdateTestRunChangedEventArgs(testRunChangedArgs, PathConversionDirection.Send);
        var data = _dataSerializer.SerializePayload(MessageType.TestRunStatsChange, updatedTestRunChangedEventArgs, _protocolVersion);
        SendData(data);
    }

    /// <inheritdoc />
    public void SendLog(TestMessageLevel messageLevel, string? message)
    {
        var data = _dataSerializer.SerializePayload(
            MessageType.TestMessage,
            new TestMessagePayload { MessageLevel = messageLevel, Message = message },
            _protocolVersion);
        SendData(data);
    }

    /// <inheritdoc />
    public void SendExecutionComplete(
        TestRunCompleteEventArgs testRunCompleteArgs,
        TestRunChangedEventArgs? lastChunkArgs,
        ICollection<AttachmentSet>? runContextAttachments,
        ICollection<string>? executorUris)
    {
        // When we abort the run we might have saved the error before we gave the handler the chance to abort
        // if the handler does not return with any new error we report the original one.
        if (testRunCompleteArgs.IsAborted
            && testRunCompleteArgs.Error == null
            && _messageProcessingUnrecoverableError != null)
        {
            var curentArgs = testRunCompleteArgs;
            testRunCompleteArgs = new TestRunCompleteEventArgs(
                curentArgs.TestRunStatistics,
                curentArgs.IsCanceled,
                curentArgs.IsAborted,
                _messageProcessingUnrecoverableError,
                _pathConverter.UpdateAttachmentSets(curentArgs.AttachmentSets, PathConversionDirection.Send), curentArgs.InvokedDataCollectors, curentArgs.ElapsedTimeInRunningTests
            );
        }
        var data = _dataSerializer.SerializePayload(
            MessageType.ExecutionComplete,
            new TestRunCompletePayload
            {
                TestRunCompleteArgs = _pathConverter.UpdateTestRunCompleteEventArgs(testRunCompleteArgs, PathConversionDirection.Send),
                LastRunTests = _pathConverter.UpdateTestRunChangedEventArgs(lastChunkArgs, PathConversionDirection.Send),
                RunAttachments = _pathConverter.UpdateAttachmentSets(runContextAttachments, PathConversionDirection.Send),
                ExecutorUris = executorUris
            },
            _protocolVersion);
        SendData(data);
    }

    /// <inheritdoc />
    public void DiscoveryComplete(DiscoveryCompleteEventArgs discoveryCompleteEventArgs, IEnumerable<TestCase>? lastChunk)
    {
        var data = _dataSerializer.SerializePayload(
            MessageType.DiscoveryComplete,
            new DiscoveryCompletePayload
            {
                TotalTests = discoveryCompleteEventArgs.TotalCount,
                // TODO: avoid failing when lastChunk is null
                LastDiscoveredTests = discoveryCompleteEventArgs.IsAborted ? null : _pathConverter.UpdateTestCases(lastChunk!, PathConversionDirection.Send),
                IsAborted = discoveryCompleteEventArgs.IsAborted,
                Metrics = discoveryCompleteEventArgs.Metrics,
                FullyDiscoveredSources = discoveryCompleteEventArgs.FullyDiscoveredSources,
                PartiallyDiscoveredSources = discoveryCompleteEventArgs.PartiallyDiscoveredSources,
                NotDiscoveredSources = discoveryCompleteEventArgs.NotDiscoveredSources,
                SkippedDiscoverySources = discoveryCompleteEventArgs.SkippedDiscoveredSources,
                DiscoveredExtensions = discoveryCompleteEventArgs.DiscoveredExtensions,
            },
            _protocolVersion);
        SendData(data);
    }

    /// <inheritdoc />
    public int LaunchProcessWithDebuggerAttached(TestProcessStartInfo? testProcessStartInfo)
    {
        var waitHandle = new ManualResetEventSlim(false);
        Message? ackMessage = null;
        _onLaunchAdapterProcessWithDebuggerAttachedAckReceived = (ackRawMessage) =>
        {
            ackMessage = ackRawMessage;
            waitHandle.Set();
        };

        var data = _dataSerializer.SerializePayload(MessageType.LaunchAdapterProcessWithDebuggerAttached,
            testProcessStartInfo, _protocolVersion);

        SendData(data);

        EqtTrace.Verbose("Waiting for LaunchAdapterProcessWithDebuggerAttached ack");
        waitHandle.Wait();
        _onLaunchAdapterProcessWithDebuggerAttachedAckReceived = null;
        return _dataSerializer.DeserializePayload<int>(ackMessage);
    }

    /// <inheritdoc />
    public bool AttachDebuggerToProcess(AttachDebuggerInfo attachDebuggerInfo)
    {
        // If an attach request is issued but there is no support for attaching on the other
        // side of the communication channel, we simply return and let the caller know the
        // request failed.
        if (_protocolVersion < ObjectModelConstants.MinimumProtocolVersionWithDebugSupport)
        {
            return false;
        }

        Message? ackMessage = null;
        var waitHandle = new ManualResetEventSlim(false);

        _onAttachDebuggerAckRecieved = (ackRawMessage) =>
        {
            ackMessage = ackRawMessage;
            waitHandle.Set();
        };

        var data = _dataSerializer.SerializePayload(
            MessageType.AttachDebugger,
            new TestProcessAttachDebuggerPayload(attachDebuggerInfo.ProcessId)
            {
                TargetFramework = attachDebuggerInfo.TargetFramework?.ToString(),
            },
            _protocolVersion);
        SendData(data);

        EqtTrace.Verbose("Waiting for AttachDebuggerToProcess ack ...");
        waitHandle.Wait();

        _onAttachDebuggerAckRecieved = null;
        return _dataSerializer.DeserializePayload<bool>(ackMessage);
    }

    public void OnMessageReceived(object? sender, MessageReceivedEventArgs messageReceivedArgs)
    {
        var message = _dataSerializer.DeserializeMessage(messageReceivedArgs.Data!);
        EqtTrace.Info("TestRequestHandler.OnMessageReceived: received message: {0}", message);

        switch (message?.MessageType)
        {
            case MessageType.VersionCheck:
                try
                {
                    var version = _dataSerializer.DeserializePayload<int>(message);
                    // choose the highest version that we both support
                    var negotiatedVersion = Math.Min(version, _highestSupportedVersion);
                    // BUT don't choose 3, because protocol version 3 has performance problems in 16.7.1-16.8. Those problems are caused
                    // by choosing payloadSerializer instead of payloadSerializer2 for protocol version 3.
                    //
                    // We cannot just update the code to choose the new serializer, because then that change would apply only to testhost.
                    // Testhost is is delivered by Microsoft.NET.Test.SDK nuget package, and can be used with an older vstest.console.
                    // An older vstest.console, that supports protocol version 3, would serialize its messages using payloadSerializer,
                    // but the fixed testhost would serialize it using payloadSerializer2, resulting in incompatible messages.
                    //
                    // Instead we must downgrade to protocol version 2 when 3 would be negotiated. Or higher when higher version
                    // would be negotiated.
                    if (negotiatedVersion != 3)
                    {
                        _protocolVersion = negotiatedVersion;
                    }
                    else
                    {
                        var flag = Environment.GetEnvironmentVariable("VSTEST_DISABLE_PROTOCOL_3_VERSION_DOWNGRADE");
                        var flagIsEnabled = flag is not null and not "0";
                        var dowgradeIsDisabled = flagIsEnabled;
                        _protocolVersion = dowgradeIsDisabled ? negotiatedVersion : 2;
                    }

                    // Send the negotiated protocol to request sender
                    TPDebug.Assert(_channel is not null, "_channel is null");
                    _channel.Send(_dataSerializer.SerializePayload(MessageType.VersionCheck, _protocolVersion));

                    // Can only do this after InitializeCommunication because TestHost cannot "Send Log" unless communications are initialized
                    if (!StringUtils.IsNullOrEmpty(EqtTrace.LogFile))
                    {
                        SendLog(TestMessageLevel.Informational, string.Format(CultureInfo.CurrentCulture, CrossPlatResources.TesthostDiagLogOutputFile, EqtTrace.LogFile));
                    }
                    else if (!StringUtils.IsNullOrEmpty(EqtTrace.ErrorOnInitialization))
                    {
                        SendLog(TestMessageLevel.Warning, EqtTrace.ErrorOnInitialization);
                    }
                }
                catch (Exception ex)
                {
                    _messageProcessingUnrecoverableError = ex;
                    EqtTrace.Error("Failed processing message {0}, aborting test run.", message.MessageType);
                    EqtTrace.Error(ex);
                    goto case MessageType.AbortTestRun;
                }
                break;

            case MessageType.DiscoveryInitialize:
                {
                    try
                    {
                        _testHostManagerFactoryReady.Wait();
                        var discoveryEventsHandler = new TestDiscoveryEventHandler(this);
                        var path = _dataSerializer.DeserializePayload<IEnumerable<string>>(message);
                        TPDebug.Assert(path is not null, "path is null");
                        var pathToAdditionalExtensions = _pathConverter.UpdatePaths(path, PathConversionDirection.Receive);
                        Action job = () =>
                        {
                            EqtTrace.Info("TestRequestHandler.OnMessageReceived: Running job '{0}'.", message.MessageType);
                            TPDebug.Assert(_testHostManagerFactory is not null, "_testHostManagerFactory is null");
                            _testHostManagerFactory.GetDiscoveryManager().Initialize(pathToAdditionalExtensions, discoveryEventsHandler);
                        };
                        _jobQueue.QueueJob(job, 0);
                    }
                    catch (Exception ex)
                    {
                        _messageProcessingUnrecoverableError = ex;
                        EqtTrace.Error("Failed processing message {0}, aborting test run.", message.MessageType);
                        EqtTrace.Error(ex);
                        goto case MessageType.AbortTestRun;
                    }
                    break;
                }

            case MessageType.StartDiscovery:
                {
                    try
                    {
                        _testHostManagerFactoryReady.Wait();
                        var discoveryEventsHandler = new TestDiscoveryEventHandler(this);
                        var discoveryCriteria = _dataSerializer.DeserializePayload<DiscoveryCriteria>(message);
                        TPDebug.Assert(discoveryCriteria is not null, "discoveryCriteria is null");
                        discoveryCriteria = _pathConverter.UpdateDiscoveryCriteria(discoveryCriteria, PathConversionDirection.Receive);

                        Action job = () =>
                        {
                            EqtTrace.Info("TestRequestHandler.OnMessageReceived: Running job '{0}'.", message.MessageType);
                            TPDebug.Assert(_testHostManagerFactory is not null, "_testHostManagerFactory is null");
                            _testHostManagerFactory.GetDiscoveryManager()
                                .DiscoverTests(discoveryCriteria, discoveryEventsHandler);
                        };

                        _jobQueue.QueueJob(job, 0);
                    }
                    catch (Exception ex)
                    {
                        _messageProcessingUnrecoverableError = ex;
                        EqtTrace.Error("Failed processing message {0}, aborting test run.", message.MessageType);
                        EqtTrace.Error(ex);
                        goto case MessageType.AbortTestRun;
                    }
                    break;
                }

            case MessageType.ExecutionInitialize:
                {
                    try
                    {
                        _testHostManagerFactoryReady.Wait();
                        var testInitializeEventsHandler = new TestInitializeEventsHandler(this);
                        var pathToAdditionalExtensions = _dataSerializer.DeserializePayload<IEnumerable<string>>(message);
                        TPDebug.Assert(pathToAdditionalExtensions is not null, "pathToAdditionalExtensions is null");
                        pathToAdditionalExtensions = _pathConverter.UpdatePaths(pathToAdditionalExtensions, PathConversionDirection.Receive);
                        Action job = () =>
                        {
                            EqtTrace.Info("TestRequestHandler.OnMessageReceived: Running job '{0}'.", message.MessageType);
                            TPDebug.Assert(_testHostManagerFactory is not null, "_testHostManagerFactory is null");
                            _testHostManagerFactory.GetExecutionManager().Initialize(pathToAdditionalExtensions, testInitializeEventsHandler);
                        };
                        _jobQueue.QueueJob(job, 0);
                    }
                    catch (Exception ex)
                    {
                        _messageProcessingUnrecoverableError = ex;
                        EqtTrace.Error("Failed processing message {0}, aborting test run.", message.MessageType);
                        EqtTrace.Error(ex);
                        goto case MessageType.AbortTestRun;
                    }
                    break;
                }

            case MessageType.StartTestExecutionWithSources:
                {
                    try
                    {
                        var testRunEventsHandler = new TestRunEventsHandler(this);
                        _testHostManagerFactoryReady.Wait();
                        var testRunCriteriaWithSources = _dataSerializer.DeserializePayload<TestRunCriteriaWithSources>(message);
                        TPDebug.Assert(testRunCriteriaWithSources is not null, "testRunCriteriaWithSources is null");
                        testRunCriteriaWithSources = _pathConverter.UpdateTestRunCriteriaWithSources(testRunCriteriaWithSources, PathConversionDirection.Receive);
                        Action job = () =>
                        {
                            EqtTrace.Info("TestRequestHandler.OnMessageReceived: Running job '{0}'.", message.MessageType);
                            TPDebug.Assert(_testHostManagerFactory is not null, "_testHostManagerFactory is null");
                            _testHostManagerFactory.GetExecutionManager()
                                .StartTestRun(
                                    testRunCriteriaWithSources.AdapterSourceMap,
                                    testRunCriteriaWithSources.Package,
                                    testRunCriteriaWithSources.RunSettings,
                                    testRunCriteriaWithSources.TestExecutionContext,
                                    GetTestCaseEventsHandler(testRunCriteriaWithSources.RunSettings),
                                    testRunEventsHandler);
                        };
                        _jobQueue.QueueJob(job, 0);
                    }
                    catch (Exception ex)
                    {
                        _messageProcessingUnrecoverableError = ex;
                        EqtTrace.Error("Failed processing message {0}, aborting test run.", message.MessageType);
                        EqtTrace.Error(ex);
                        goto case MessageType.AbortTestRun;
                    }
                    break;
                }

            case MessageType.StartTestExecutionWithTests:
                {
                    try
                    {
                        var testRunEventsHandler = new TestRunEventsHandler(this);
                        _testHostManagerFactoryReady.Wait();
                        var testRunCriteriaWithTests = _dataSerializer.DeserializePayload<TestRunCriteriaWithTests>(message);
                        TPDebug.Assert(testRunCriteriaWithTests is not null, "testRunCriteriaWithTests is null");
                        testRunCriteriaWithTests = _pathConverter.UpdateTestRunCriteriaWithTests(testRunCriteriaWithTests, PathConversionDirection.Receive);

                        Action job = () =>
                        {
                            EqtTrace.Info("TestRequestHandler.OnMessageReceived: Running job '{0}'.", message.MessageType);
                            TPDebug.Assert(_testHostManagerFactory is not null, "_testHostManagerFactory is null");
                            _testHostManagerFactory.GetExecutionManager()
                                .StartTestRun(
                                    testRunCriteriaWithTests.Tests,
                                    testRunCriteriaWithTests.Package,
                                    testRunCriteriaWithTests.RunSettings,
                                    testRunCriteriaWithTests.TestExecutionContext,
                                    GetTestCaseEventsHandler(testRunCriteriaWithTests.RunSettings),
                                    testRunEventsHandler);
                        };
                        _jobQueue.QueueJob(job, 0);
                    }
                    catch (Exception ex)
                    {
                        _messageProcessingUnrecoverableError = ex;
                        EqtTrace.Error("Failed processing message {0}, aborting test run.", message.MessageType);
                        EqtTrace.Error(ex);
                        goto case MessageType.AbortTestRun;
                    }
                    break;
                }

            case MessageType.CancelTestRun:
                _jobQueue.Pause();
                _testHostManagerFactoryReady.Wait();
                TPDebug.Assert(_testHostManagerFactory is not null, "_testHostManagerFactory is null");
                _testHostManagerFactory.GetExecutionManager().Cancel(new TestRunEventsHandler(this));
                break;

            case MessageType.LaunchAdapterProcessWithDebuggerAttachedCallback:
                _onLaunchAdapterProcessWithDebuggerAttachedAckReceived?.Invoke(message);
                break;

            case MessageType.AttachDebuggerCallback:
                _onAttachDebuggerAckRecieved?.Invoke(message);
                break;

            case MessageType.CancelDiscovery:
                _jobQueue.Pause();
                _testHostManagerFactoryReady.Wait();
                TPDebug.Assert(_testHostManagerFactory is not null, "_testHostManagerFactory is null");
                _testHostManagerFactory.GetDiscoveryManager().Abort(new TestDiscoveryEventHandler(this));
                break;

            case MessageType.AbortTestRun:
                try
                {
                    _jobQueue.Pause();
                    _testHostManagerFactoryReady.Wait();
                    TPDebug.Assert(_testHostManagerFactory is not null, "_testHostManagerFactory is null");
                    _testHostManagerFactory.GetExecutionManager().Abort(new TestRunEventsHandler(this));
                }
                catch (Exception ex)
                {
                    EqtTrace.Error("Failed processing message {0}. Stopping communication.", message.MessageType);
                    EqtTrace.Error(ex);
                    _sessionCompleted.Set();
                    Close();
                }
                break;

            case MessageType.SessionEnd:
                {
                    EqtTrace.Info("Session End message received from server. Closing the connection.");
                    _sessionCompleted.Set();
                    Close();
                    break;
                }

            case MessageType.SessionAbort:
                {
                    // Don't do anything for now.
                    break;
                }

            default:
                {
                    EqtTrace.Info("Invalid Message types");
                    break;
                }
        }
    }

    private static ITestCaseEventsHandler? GetTestCaseEventsHandler(string? runSettings)
    {
        ITestCaseEventsHandler? testCaseEventsHandler = null;

        // Listen to test case events only if data collection is enabled
        if ((XmlRunSettingsUtilities.IsDataCollectionEnabled(runSettings) && DataCollectionTestCaseEventSender.Instance != null)
            || XmlRunSettingsUtilities.IsInProcDataCollectionEnabled(runSettings))
        {
            testCaseEventsHandler = new TestCaseEventsHandler();
        }

        return testCaseEventsHandler;
    }

    private void SendData(string data)
    {
        EqtTrace.Verbose("TestRequestHandler.SendData: sending data from testhost: {0}", data);
        _channel?.Send(data);
    }
}
