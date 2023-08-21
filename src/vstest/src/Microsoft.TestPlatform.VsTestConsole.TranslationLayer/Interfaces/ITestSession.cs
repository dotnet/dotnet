// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;

/// <summary>
/// Defines a test session that can be used to make calls to the vstest.console
/// process.
/// </summary>
[Obsolete("This API is not final yet and is subject to changes.", false)]
public interface ITestSession : IDisposable, ITestSessionAsync
{
    /// <summary>
    /// Gets the underlying test session info object.
    /// </summary>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    TestSessionInfo? TestSessionInfo { get; }

    /// <summary>
    /// Starts test discovery.
    /// </summary>
    ///
    /// <param name="sources">The list of source assemblies for the discovery.</param>
    /// <param name="discoverySettings">The run settings for the discovery.</param>
    /// <param name="discoveryEventsHandler">The discovery event handler.</param>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    void DiscoverTests(
        IEnumerable<string> sources,
        string discoverySettings,
        ITestDiscoveryEventsHandler discoveryEventsHandler);

    /// <summary>
    /// Starts test discovery.
    /// </summary>
    ///
    /// <param name="sources">The list of source assemblies for the discovery.</param>
    /// <param name="discoverySettings">The run settings for the discovery.</param>
    /// <param name="options">The test platform options.</param>
    /// <param name="discoveryEventsHandler">The discovery event handler.</param>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    void DiscoverTests(
        IEnumerable<string> sources,
        string discoverySettings,
        TestPlatformOptions options,
        ITestDiscoveryEventsHandler2 discoveryEventsHandler);

    /// <summary>
    /// Cancels the last discovery request.
    /// </summary>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    new void CancelDiscovery();

    /// <summary>
    /// Starts a test run.
    /// </summary>
    ///
    /// <param name="sources">The list of source assemblies for the test run.</param>
    /// <param name="runSettings">The run settings for the run.</param>
    /// <param name="testRunEventsHandler">The run event handler.</param>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    void RunTests(
        IEnumerable<string> sources,
        string runSettings,
        ITestRunEventsHandler testRunEventsHandler);

    /// <summary>
    /// Starts a test run.
    /// </summary>
    ///
    /// <param name="sources">The list of source assemblies for the test run.</param>
    /// <param name="runSettings">The run settings for the run.</param>
    /// <param name="options">The test platform options.</param>
    /// <param name="testRunEventsHandler">The run event handler.</param>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    void RunTests(
        IEnumerable<string> sources,
        string runSettings,
        TestPlatformOptions options,
        ITestRunEventsHandler testRunEventsHandler);

    /// <summary>
    /// Starts a test run.
    /// </summary>
    ///
    /// <param name="sources">The list of source assemblies for the test run.</param>
    /// <param name="runSettings">The run settings for the run.</param>
    /// <param name="options">The test platform options.</param>
    /// <param name="testRunEventsHandler">The run event handler.</param>
    /// <param name="telemetryEventsHandler">The telemetry event handler.</param>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    void RunTests(
        IEnumerable<string> sources,
        string runSettings,
        TestPlatformOptions options,
        ITestRunEventsHandler testRunEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler);

    /// <summary>
    /// Starts a test run.
    /// </summary>
    ///
    /// <param name="testCases">The list of test cases for the test run.</param>
    /// <param name="runSettings">The run settings for the run.</param>
    /// <param name="testRunEventsHandler">The run event handler.</param>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    void RunTests(
        IEnumerable<TestCase> testCases,
        string runSettings,
        ITestRunEventsHandler testRunEventsHandler);

    /// <summary>
    /// Starts a test run.
    /// </summary>
    ///
    /// <param name="testCases">The list of test cases for the test run.</param>
    /// <param name="runSettings">The run settings for the run.</param>
    /// <param name="options">The test platform options.</param>
    /// <param name="testRunEventsHandler">The run event handler.</param>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    void RunTests(
        IEnumerable<TestCase> testCases,
        string runSettings,
        TestPlatformOptions options,
        ITestRunEventsHandler testRunEventsHandler);

    /// <summary>
    /// Starts a test run.
    /// </summary>
    ///
    /// <param name="testCases">The list of test cases for the test run.</param>
    /// <param name="runSettings">The run settings for the run.</param>
    /// <param name="options">The test platform options.</param>
    /// <param name="testRunEventsHandler">The run event handler.</param>
    /// <param name="telemetryEventsHandler">The telemetry event handler.</param>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    void RunTests(
        IEnumerable<TestCase> testCases,
        string runSettings,
        TestPlatformOptions options,
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
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    void RunTestsWithCustomTestHost(
        IEnumerable<string> sources,
        string runSettings,
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
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    void RunTestsWithCustomTestHost(
        IEnumerable<string> sources,
        string runSettings,
        TestPlatformOptions options,
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
    /// <param name="telemetryEventsHandler">The telemetry event handler.</param>
    /// <param name="customTestHostLauncher">The custom host launcher.</param>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    void RunTestsWithCustomTestHost(
        IEnumerable<string> sources,
        string runSettings,
        TestPlatformOptions options,
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
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    void RunTestsWithCustomTestHost(
        IEnumerable<TestCase> testCases,
        string runSettings,
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
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    void RunTestsWithCustomTestHost(
        IEnumerable<TestCase> testCases,
        string runSettings,
        TestPlatformOptions options,
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
    /// <param name="telemetryEventsHandler">The telemetry event handler.</param>
    /// <param name="customTestHostLauncher">The custom host launcher.</param>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    void RunTestsWithCustomTestHost(
        IEnumerable<TestCase> testCases,
        string runSettings,
        TestPlatformOptions options,
        ITestRunEventsHandler testRunEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler,
        ITestHostLauncher customTestHostLauncher);

    /// <summary>
    /// Stops the test session.
    /// </summary>
    ///
    /// <returns>True if the session was successfuly stopped, false otherwise.</returns>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    bool StopTestSession();

    /// <summary>
    /// Stops the test session.
    /// </summary>
    ///
    /// <param name="eventsHandler">The session event handler.</param>
    ///
    /// <returns>True if the session was successfuly stopped, false otherwise.</returns>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    bool StopTestSession(ITestSessionEventsHandler eventsHandler);

    /// <summary>
    /// Stops the test session.
    /// </summary>
    ///
    /// <param name="options">Test Platform options.</param>
    /// <param name="eventsHandler">The session event handler.</param>
    ///
    /// <returns>True if the session was successfuly stopped, false otherwise.</returns>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    bool StopTestSession(TestPlatformOptions options, ITestSessionEventsHandler eventsHandler);

    /// <summary>
    /// Cancels the last test run.
    /// </summary>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    new void CancelTestRun();

    /// <summary>
    /// Aborts the last test run.
    /// </summary>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    new void AbortTestRun();
}
