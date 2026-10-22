// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

using static Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ProtocolVersioning;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;

/// <summary>
/// The message type.
/// </summary>
public static class MessageType
{
    /// <summary>
    /// The session start.
    /// </summary>
    [ProtocolVersion(Version0, typeof(void), Description = "Not used.")]
    public const string SessionStart = "TestSession.Start";

    /// <summary>
    /// The session end.
    /// </summary>
    [ProtocolVersion(Version0, typeof(void))]
    public const string SessionEnd = "TestSession.Terminate";

    /// <summary>
    /// The is aborted.
    /// </summary>
    [ProtocolVersion(Version0, typeof(void), Description = "Is sent to testhost, and then ignored.")]
    public const string SessionAbort = "TestSession.Abort";

    /// <summary>
    /// The session connected.
    /// </summary>
    [ProtocolVersion(Version0, typeof(void), Description = "Is sent from DesignMode client (vstest.console) back to IDE, to signal that we are ready to recieve requests.")]
    public const string SessionConnected = "TestSession.Connected";

    /// <summary>
    /// Test Message
    /// </summary>
    [ProtocolVersion(Version0, typeof(TestMessagePayload), Description = "A log message or similar message sent back from console or testhost.")]
    public const string TestMessage = "TestSession.Message";

    /// <summary>
    /// Protocol Version
    /// </summary>
    [ProtocolVersion(Version1, typeof(int), Description = @"Used for version handshake from IDE to vstest.console and from vstest.console to testhost,
        the highest common version is used. Version that is negotiated with vstest.console, is used for testhost handshake to ensure that testhost
        does not use a protocol that is newer than what we negotiated with IDE.")]
    public const string VersionCheck = "ProtocolVersion";

    /// <summary>
    /// Protocol Error
    /// </summary>
    public const string ProtocolError = "ProtocolError";

    /// <summary>
    /// The session start.
    /// </summary>
    public const string DiscoveryInitialize = "TestDiscovery.Initialize";

    /// <summary>
    /// The discovery started.
    /// </summary>
    public const string StartDiscovery = "TestDiscovery.Start";

    /// <summary>
    /// The test cases found.
    /// </summary>
    public const string TestCasesFound = "TestDiscovery.TestFound";

    /// <summary>
    /// The discovery complete.
    /// </summary>
    public const string DiscoveryComplete = "TestDiscovery.Completed";

    /// <summary>
    /// Cancel Test Discovery
    /// </summary>
    public const string CancelDiscovery = "TestDiscovery.Cancel";

    /// <summary>
    /// The session start.
    /// </summary>
    public const string ExecutionInitialize = "TestExecution.Initialize";

    /// <summary>
    /// Cancel the current test run
    /// </summary>
    public const string CancelTestRun = "TestExecution.Cancel";

    /// <summary>
    /// Cancel the current test run
    /// </summary>
    public const string AbortTestRun = "TestExecution.Abort";

    /// <summary>
    /// Start test execution.
    /// </summary>
    public const string StartTestExecutionWithSources = "TestExecution.StartWithSources";

    /// <summary>
    /// Start test execution.
    /// </summary>
    public const string StartTestExecutionWithTests = "TestExecution.StartWithTests";

    /// <summary>
    /// The test run stats change.
    /// </summary>
    public const string TestRunStatsChange = "TestExecution.StatsChange";

    /// <summary>
    /// The execution complete.
    /// </summary>
    public const string ExecutionComplete = "TestExecution.Completed";

    /// <summary>
    /// The message to get runner process startInfo for run all tests in given sources
    /// </summary>
    public const string GetTestRunnerProcessStartInfoForRunAll = "TestExecution.GetTestRunnerProcessStartInfoForRunAll";

    /// <summary>
    /// The message to get runner process startInfo for run selected tests
    /// </summary>
    public const string GetTestRunnerProcessStartInfoForRunSelected = "TestExecution.GetTestRunnerProcessStartInfoForRunSelected";

    /// <summary>
    /// CustomTestHostLaunch
    /// </summary>
    public const string CustomTestHostLaunch = "TestExecution.CustomTestHostLaunch";

    /// <summary>
    /// Custom Test Host launch callback
    /// </summary>
    public const string CustomTestHostLaunchCallback = "TestExecution.CustomTestHostLaunchCallback";

    /// <summary>
    /// Test run attachments processing
    /// </summary>
    public const string TestRunAttachmentsProcessingStart = "TestRunAttachmentsProcessing.Start";

    /// <summary>
    /// Test run attachments processing callback
    /// </summary>
    public const string TestRunAttachmentsProcessingComplete = "TestRunAttachmentsProcessing.Complete";

    /// <summary>
    /// Test run attachments processing progress
    /// </summary>
    public const string TestRunAttachmentsProcessingProgress = "TestRunAttachmentsProcessing.Progress";

    /// <summary>
    /// Cancel test run attachments processing
    /// </summary>
    public const string TestRunAttachmentsProcessingCancel = "TestRunAttachmentsProcessing.Cancel";

    /// <summary>
    /// Extensions Initialization
    /// </summary>
    public const string ExtensionsInitialize = "Extensions.Initialize";

    /// <summary>
    /// Start Test Run All Sources
    /// </summary>
    public const string TestRunAllSourcesWithDefaultHost = "TestExecution.RunAllWithDefaultHost";

    /// <summary>
    ///  Start Test Run - Testcases
    /// </summary>
    public const string TestRunSelectedTestCasesDefaultHost = "TestExecution.RunSelectedWithDefaultHost";

    /// <summary>
    /// Launch Adapter Process With DebuggerAttached
    /// </summary>
    public const string LaunchAdapterProcessWithDebuggerAttached = "TestExecution.LaunchAdapterProcessWithDebuggerAttached";

    /// <summary>
    /// Launch Adapter Process With DebuggerAttached
    /// </summary>
    public const string LaunchAdapterProcessWithDebuggerAttachedCallback = "TestExecution.LaunchAdapterProcessWithDebuggerAttachedCallback";

    /// <summary>
    /// Attach debugger to process.
    /// </summary>
    [ProtocolVersion(Version3, typeof(TestProcessAttachDebuggerPayload))]
    public const string AttachDebugger = "TestExecution.AttachDebugger";

    /// <summary>
    /// Attach debugger to process callback.
    /// </summary>
    [ProtocolVersion(Version3, typeof(bool))]
    public const string AttachDebuggerCallback = "TestExecution.AttachDebuggerCallback";

    /// <summary>
    /// Attach debugger to process.
    /// </summary>
    [ProtocolVersion(Version3, typeof(int), Deprecated = Version7, Description = "Carries the process id the IDE should attach to. DEPRECATED, use EditorAttachDebugger2 instead.")]
    public const string EditorAttachDebugger = "TestExecution.EditorAttachDebugger";

    /// <summary>
    /// Attach debugger to process callback.
    /// </summary>
    [ProtocolVersion(Version3, typeof(EditorAttachDebuggerAckPayload))]
    public const string EditorAttachDebuggerCallback = "TestExecution.EditorAttachDebuggerCallback";

    /// <summary>
    /// Data Collection Message
    /// </summary>
    public const string DataCollectionMessage = "DataCollection.SendMessage";

    /// <summary>
    /// StartTestSession message.
    /// </summary>
    public const string StartTestSession = "TestSession.StartTestSession";

    /// <summary>
    /// StartTestSession callback message.
    /// </summary>
    public const string StartTestSessionCallback = "TestSession.StartTestSessionCallback";

    /// <summary>
    /// StopTestSession message.
    /// </summary>
    public const string StopTestSession = "TestSession.StopTestSession";

    /// <summary>
    /// StopTestSession callback message.
    /// </summary>
    public const string StopTestSessionCallback = "TestSession.StopTestSessionCallback";

    /// <summary>
    /// Event message type sent to datacollector process right after test host process has started.
    /// </summary>
    public const string TestHostLaunched = "DataCollection.TestHostLaunched";

    /// <summary>
    /// Event message type send to datacollector process before test run starts.
    /// </summary>
    public const string BeforeTestRunStart = "DataCollection.BeforeTestRunStart";

    /// <summary>
    /// Event message type used by datacollector to send results  after receiving test run start event.
    /// </summary>
    public const string BeforeTestRunStartResult = "DataCollection.BeforeTestRunStartResult";

    /// <summary>
    /// Event message type send to datacollector process after test run ends.
    /// </summary>
    public const string AfterTestRunEnd = "DataCollection.AfterTestRunEnd";

    /// <summary>
    /// Event message type used by datacollector to send result on receiving test run end event.
    /// </summary>
    public const string AfterTestRunEndResult = "DataCollection.AfterTestRunEndResult";

    /// <summary>
    /// Event message type send to datacollector process before test case execution starts.
    /// </summary>
    public const string DataCollectionTestStart = "DataCollection.TestStart";

    /// <summary>
    /// Event message type used to signal datacollector process that test case execution has ended.
    /// </summary>
    public const string DataCollectionTestEnd = "DataCollection.TestEnd";

    /// <summary>
    /// Event message type used by datacollector to send result on receiving TestEnd.
    /// </summary>
    public const string DataCollectionTestEndResult = "DataCollection.TestEndResult";

    /// <summary>
    /// Ack Event message type send to datacollector process before test case execution starts.
    /// </summary>
    public const string DataCollectionTestStartAck = "DataCollection.TestStartAck";

    /// <summary>
    /// Attach debugger to process.
    /// </summary>
    [ProtocolVersion(Version7, typeof(EditorAttachDebuggerPayload))]
    public const string EditorAttachDebugger2 = "TestExecution.EditorAttachDebugger2";

    /// <summary>
    /// Telemetry event.
    /// </summary>
    public const string TelemetryEventMessage = "TestPlatform.TelemetryEvent";
}
