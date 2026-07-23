// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;

namespace Microsoft.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;

/// <summary>
/// Controller for various test operations on the test runner.
/// </summary>
public interface IVsTestConsoleWrapper : IVsTestConsoleWrapperAsync
{
    /// <summary>
    /// Starts the test runner process and readies for requests.
    /// </summary>
    void StartSession();

    /// <summary>
    /// Initializes the test platform with paths to extensions like adapters, loggers and any
    /// other extensions.
    /// </summary>
    ///
    /// <param name="pathToAdditionalExtensions">Full paths to extension DLLs.</param>
    void InitializeExtensions(IEnumerable<string> pathToAdditionalExtensions);

    /// <summary>
    /// Starts test discovery.
    /// </summary>
    ///
    /// <param name="sources">The list of source assemblies for the discovery.</param>
    /// <param name="discoverySettings">The run settings for the discovery.</param>
    /// <param name="discoveryEventsHandler">The discovery event handler.</param>
    void DiscoverTests(
        IEnumerable<string> sources,
        string? discoverySettings,
        ITestDiscoveryEventsHandler discoveryEventsHandler);

    /// <summary>
    /// Starts test discovery.
    /// </summary>
    ///
    /// <param name="sources">The list of source assemblies for the discovery.</param>
    /// <param name="discoverySettings">The run settings for the discovery.</param>
    /// <param name="options">The test platform options.</param>
    /// <param name="discoveryEventsHandler">The discovery event handler.</param>
    void DiscoverTests(
        IEnumerable<string> sources,
        string? discoverySettings,
        TestPlatformOptions? options,
        ITestDiscoveryEventsHandler2 discoveryEventsHandler);

    /// <summary>
    /// Starts test discovery.
    /// </summary>
    ///
    /// <param name="sources">The list of source assemblies for the discovery.</param>
    /// <param name="discoverySettings">The run settings for the discovery.</param>
    /// <param name="options">The test platform options.</param>
    /// <param name="testSessionInfo">The test session info object.</param>
    /// <param name="discoveryEventsHandler">The discovery event handler.</param>
    void DiscoverTests(
        IEnumerable<string> sources,
        string? discoverySettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestDiscoveryEventsHandler2 discoveryEventsHandler);

    /// <summary>
    /// Cancels the last discovery request.
    /// </summary>
    new void CancelDiscovery();

    /// <summary>
    /// Starts a test run.
    /// </summary>
    ///
    /// <param name="sources">The list of source assemblies for the test run.</param>
    /// <param name="runSettings">The run settings for the run.</param>
    /// <param name="testRunEventsHandler">The run event handler.</param>
    void RunTests(
        IEnumerable<string> sources,
        string? runSettings,
        ITestRunEventsHandler testRunEventsHandler);

    /// <summary>
    /// Starts a test run.
    /// </summary>
    ///
    /// <param name="sources">The list of source assemblies for the test run.</param>
    /// <param name="runSettings">The run settings for the run.</param>
    /// <param name="options">The test platform options.</param>
    /// <param name="testRunEventsHandler">The run event handler.</param>
    void RunTests(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        ITestRunEventsHandler testRunEventsHandler);

    /// <summary>
    /// Starts a test run.
    /// </summary>
    ///
    /// <param name="sources">The list of source assemblies for the test run.</param>
    /// <param name="runSettings">The run settings for the run.</param>
    /// <param name="options">The test platform options.</param>
    /// <param name="testSessionInfo">The test session info object.</param>
    /// <param name="testRunEventsHandler">The run event handler.</param>
    void RunTests(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler);

    /// <summary>
    /// Starts a test run.
    /// </summary>
    ///
    /// <param name="sources">The list of source assemblies for the test run.</param>
    /// <param name="runSettings">The run settings for the run.</param>
    /// <param name="options">The test platform options.</param>
    /// <param name="testSessionInfo">The test session info object.</param>
    /// <param name="testRunEventsHandler">The run event handler.</param>
    /// <param name="telemetryEventsHandler">The telemetry events handler.</param>
    void RunTests(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler);

    /// <summary>
    /// Starts a test run.
    /// </summary>
    ///
    /// <param name="testCases">The list of test cases for the test run.</param>
    /// <param name="runSettings">The run settings for the run.</param>
    /// <param name="testRunEventsHandler">The run event handler.</param>
    void RunTests(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        ITestRunEventsHandler testRunEventsHandler);

    /// <summary>
    /// Starts a test run.
    /// </summary>
    ///
    /// <param name="testCases">The list of test cases for the test run.</param>
    /// <param name="runSettings">The run settings for the run.</param>
    /// <param name="options">The test platform options.</param>
    /// <param name="testRunEventsHandler">The run event handler.</param>
    void RunTests(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        ITestRunEventsHandler testRunEventsHandler);

    /// <summary>
    /// Starts a test run.
    /// </summary>
    ///
    /// <param name="testCases">The list of test cases for the test run.</param>
    /// <param name="runSettings">The run settings for the run.</param>
    /// <param name="options">The test platform options.</param>
    /// <param name="testSessionInfo">The test session info object.</param>
    /// <param name="testRunEventsHandler">The run event handler.</param>
    void RunTests(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler);

    /// <summary>
    /// Starts a test run.
    /// </summary>
    ///
    /// <param name="testCases">The list of test cases for the test run.</param>
    /// <param name="runSettings">The run settings for the run.</param>
    /// <param name="options">The test platform options.</param>
    /// <param name="testSessionInfo">The test session info object.</param>
    /// <param name="testRunEventsHandler">The run event handler.</param>
    /// <param name="telemetryEventsHandler">The telemetry events handler.</param>
    void RunTests(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler);

    /// <summary>
    /// Starts a test run.
    /// </summary>
    ///
    /// <param name="sources">The list of source assemblies for the test run.</param>
    /// <param name="runSettings">The run settings for the run.</param>
    /// <param name="testRunEventsHandler">The run event handler.</param>
    /// <param name="customTestHostLauncher">The custom host launcher.</param>
    void RunTestsWithCustomTestHost(
        IEnumerable<string> sources,
        string? runSettings,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher customTestHostLauncher);

    /// <summary>
    /// Starts a test run.
    /// </summary>
    ///
    /// <param name="sources">The list of source assemblies for the test run.</param>
    /// <param name="runSettings">The run settings for the run.</param>
    /// <param name="options">The test platform options.</param>
    /// <param name="testRunEventsHandler">The run event handler.</param>
    /// <param name="customTestHostLauncher">The custom host launcher.</param>
    void RunTestsWithCustomTestHost(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher customTestHostLauncher);

    /// <summary>
    /// Starts a test run.
    /// </summary>
    ///
    /// <param name="sources">The list of source assemblies for the test run.</param>
    /// <param name="runSettings">The run settings for the run.</param>
    /// <param name="options">The test platform options.</param>
    /// <param name="testSessionInfo">The test session info object.</param>
    /// <param name="testRunEventsHandler">The run event handler.</param>
    /// <param name="customTestHostLauncher">The custom host launcher.</param>
    void RunTestsWithCustomTestHost(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher customTestHostLauncher);

    /// <summary>
    /// Starts a test run.
    /// </summary>
    ///
    /// <param name="sources">The list of source assemblies for the test run.</param>
    /// <param name="runSettings">The run settings for the run.</param>
    /// <param name="options">The test platform options.</param>
    /// <param name="testSessionInfo">The test session info object.</param>
    /// <param name="testRunEventsHandler">The run event handler.</param>
    /// <param name="telemetryEventsHandler">The telemetry events handler.</param>
    /// <param name="customTestHostLauncher">The custom host launcher.</param>
    void RunTestsWithCustomTestHost(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler,
        ITestHostLauncher customTestHostLauncher);

    /// <summary>
    /// Starts a test run.
    /// </summary>
    ///
    /// <param name="testCases">The list of test cases for the test run.</param>
    /// <param name="runSettings">The run settings for the run.</param>
    /// <param name="testRunEventsHandler">The run event handler.</param>
    /// <param name="customTestHostLauncher">The custom host launcher.</param>
    void RunTestsWithCustomTestHost(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher customTestHostLauncher);

    /// <summary>
    /// Starts a test run.
    /// </summary>
    ///
    /// <param name="testCases">The list of test cases for the test run.</param>
    /// <param name="runSettings">The run settings for the run.</param>
    /// <param name="options">The test platform options.</param>
    /// <param name="testRunEventsHandler">The run event handler.</param>
    /// <param name="customTestHostLauncher">The custom host launcher.</param>
    void RunTestsWithCustomTestHost(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher customTestHostLauncher);

    /// <summary>
    /// Starts a test run.
    /// </summary>
    ///
    /// <param name="testCases">The list of test cases for the test run.</param>
    /// <param name="runSettings">The run settings for the run.</param>
    /// <param name="options">The test platform options.</param>
    /// <param name="testSessionInfo">The test session info object.</param>
    /// <param name="testRunEventsHandler">The run event handler.</param>
    /// <param name="customTestHostLauncher">The custom host launcher.</param>
    void RunTestsWithCustomTestHost(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher customTestHostLauncher);

    /// <summary>
    /// Starts a test run.
    /// </summary>
    ///
    /// <param name="testCases">The list of test cases for the test run.</param>
    /// <param name="runSettings">The run settings for the run.</param>
    /// <param name="options">The test platform options.</param>
    /// <param name="testSessionInfo">The test session info object.</param>
    /// <param name="testRunEventsHandler">The run event handler.</param>
    /// <param name="telemetryEventsHandler">The telemetry events handler.</param>
    /// <param name="customTestHostLauncher">The custom host launcher.</param>
    void RunTestsWithCustomTestHost(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler,
        ITestHostLauncher customTestHostLauncher);

    /// <summary>
    /// Cancels the last test run.
    /// </summary>
    new void CancelTestRun();

    /// <summary>
    /// Aborts the last test run.
    /// </summary>
    new void AbortTestRun();

    /// <summary>
    /// Ends the test session and stops processing requests.
    /// </summary>
    new void EndSession();
}
