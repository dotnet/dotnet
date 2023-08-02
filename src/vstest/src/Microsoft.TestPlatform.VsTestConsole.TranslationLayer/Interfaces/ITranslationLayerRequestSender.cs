// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;

namespace Microsoft.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;

/// <summary>
/// Defines contract to send test platform requests to test host
/// </summary>
internal interface ITranslationLayerRequestSender : IDisposable, ITranslationLayerRequestSenderAsync
{
    /// <summary>
    /// Initializes communication with the vstest.console.exe process.
    /// Hosts a communication channel and asynchronously connects to vstest.console.exe.
    /// </summary>
    ///
    /// <returns>Port number of the hosted server on this side.</returns>
    int InitializeCommunication();

    /// <summary>
    /// Waits for the request handler to be connected.
    /// </summary>
    ///
    /// <param name="connectionTimeout">Time to wait for connection.</param>
    ///
    /// <returns>True if the handler has connected, false otherwise.</returns>
    bool WaitForRequestHandlerConnection(int connectionTimeout);

    /// <summary>
    /// Closes the sender.
    /// </summary>
    void Close();

    /// <summary>
    /// Initializes the extensions while probing additional extension paths.
    /// </summary>
    ///
    /// <param name="pathToAdditionalExtensions">Paths to check for additional extensions.</param>
    void InitializeExtensions(IEnumerable<string> pathToAdditionalExtensions);

    /// <summary>
    /// Discovers the tests
    /// </summary>
    ///
    /// <param name="sources">Sources for discovering tests.</param>
    /// <param name="runSettings">Run settings for discovering tests.</param>
    /// <param name="options">Options to be passed into the platform.</param>
    /// <param name="testSessionInfo">Test session info.</param>
    /// <param name="discoveryEventsHandler">Event handler for discovery events.</param>
    void DiscoverTests(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestDiscoveryEventsHandler2 discoveryEventsHandler);

    /// <summary>
    /// Starts the test run with given sources and criteria.
    /// </summary>
    ///
    /// <param name="sources">Sources for test run.</param>
    /// <param name="runSettings">Run settings for test run.</param>
    /// <param name="options">Options to be passed into the platform.</param>
    /// <param name="testSessionInfo">Test session info.</param>
    /// <param name="runEventsHandler">Event handler for test run events.</param>
    /// <param name="telemetryEventsHandler">Event handler for telemetry events.</param>
    void StartTestRun(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler runEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler);

    /// <summary>
    /// Starts the test run with given sources and criteria.
    /// </summary>
    ///
    /// <param name="testCases">Test cases to run.</param>
    /// <param name="runSettings">Run settings for test run.</param>
    /// <param name="options">Options to be passed into the platform.</param>
    /// <param name="testSessionInfo">Test session info.</param>
    /// <param name="runEventsHandler">Event handler for test run events.</param>
    /// <param name="telemetryEventsHandler">Event handler for telemetry events.</param>
    void StartTestRun(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler runEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler);

    /// <summary>
    /// Starts the test run with given sources and criteria and a custom launcher.
    /// </summary>
    ///
    /// <param name="sources">Sources for test run.</param>
    /// <param name="runSettings">Run settings for test run.</param>
    /// <param name="options">Options to be passed into the platform.</param>
    /// <param name="testSessionInfo">Test session info.</param>
    /// <param name="runEventsHandler">Event handler for test run events.</param>
    /// <param name="telemetryEventsHandler">Event handler for telemetry events.</param>
    /// <param name="customTestHostLauncher">Custom test host launcher.</param>
    void StartTestRunWithCustomHost(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler runEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler,
        ITestHostLauncher customTestHostLauncher);

    /// <summary>
    /// Starts the test run with given sources and criteria and a custom launcher.
    /// </summary>
    ///
    /// <param name="testCases">Test cases to run.</param>
    /// <param name="runSettings">Run settings for test run.</param>
    /// <param name="options">Options to be passed into the platform.</param>
    /// <param name="testSessionInfo">Test session info.</param>
    /// <param name="runEventsHandler">Event handler for test run events.</param>
    /// <param name="telemetryEventsHandler">Event handler for telemetry events.</param>
    /// <param name="customTestHostLauncher">Custom test host launcher.</param>
    void StartTestRunWithCustomHost(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler runEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler,
        ITestHostLauncher customTestHostLauncher);

    /// <summary>
    /// Starts a new test session.
    /// </summary>
    ///
    /// <param name="sources">Sources for test run.</param>
    /// <param name="runSettings">Run settings for test run.</param>
    /// <param name="options">Options to be passed into the platform.</param>
    /// <param name="eventsHandler">Event handler for test session events.</param>
    /// <param name="testHostLauncher">Custom test host launcher.</param>
    /// <returns></returns>
    TestSessionInfo? StartTestSession(
        IList<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        ITestSessionEventsHandler eventsHandler,
        ITestHostLauncher? testHostLauncher);

    /// <summary>
    /// Stops the test session.
    /// </summary>
    ///
    /// <param name="testSessionInfo">Test session info.</param>
    /// <param name="options">Test Platform options.</param>
    /// <param name="eventsHandler">Event handler for test session events.</param>
    bool StopTestSession(
        TestSessionInfo? testSessionInfo,
        TestPlatformOptions? options,
        ITestSessionEventsHandler eventsHandler);

    /// <summary>
    /// Ends the session.
    /// </summary>
    void EndSession();

    /// <summary>
    /// Cancels the test run.
    /// </summary>
    void CancelTestRun();

    /// <summary>
    /// Aborts the test run.
    /// </summary>
    void AbortTestRun();

    /// <summary>
    /// On process exit unblocks communication waiting calls.
    /// </summary>
    void OnProcessExited();

    /// <summary>
    /// Cancels the discovery of tests.
    /// </summary>
    void CancelDiscovery();
}
