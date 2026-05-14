// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.TestPlatform.Protocol
{
    /// <summary>
    /// The message type.
    /// </summary>
    public static class MessageType
    {
        /// <summary>
        /// The session start.
        /// </summary>
        public const string SessionStart = "TestSession.Start";

        /// <summary>
        /// The session end.
        /// </summary>
        public const string SessionEnd = "TestSession.Terminate";

        /// <summary>
        /// The is aborted.
        /// </summary>
        public const string SessionAbort = "TestSession.Abort";

        /// <summary>
        /// The session connected.
        /// </summary>
        public const string SessionConnected = "TestSession.Connected";

        /// <summary>
        /// Test Message
        /// </summary>
        public const string TestMessage = "TestSession.Message";

        /// <summary>
        /// Protocol Version
        /// </summary>
        public const string VersionCheck = "ProtocolVersion";

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

     }
}
