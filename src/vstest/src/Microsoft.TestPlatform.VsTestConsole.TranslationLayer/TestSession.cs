// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;
using Microsoft.VisualStudio.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;

namespace Microsoft.TestPlatform.VsTestConsole.TranslationLayer;

/// <summary>
/// Defines a test session object that can be used to make calls to the vstest.console
/// process.
/// </summary>
[Obsolete("This API is not final yet and is subject to changes.", false)]
public class TestSession : ITestSession
{
    private bool _disposed;

    private readonly ITestSessionEventsHandler _eventsHandler;
    private readonly IVsTestConsoleWrapper _consoleWrapper;

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public TestSessionInfo? TestSessionInfo { get; private set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="TestSession"/> class.
    /// </summary>
    ///
    /// <param name="testSessionInfo">The test session info object.</param>
    /// <param name="eventsHandler">The session event handler.</param>
    /// <param name="consoleWrapper">The encapsulated console wrapper.</param>
    public TestSession(
        TestSessionInfo? testSessionInfo,
        ITestSessionEventsHandler eventsHandler,
        IVsTestConsoleWrapper consoleWrapper)
    {
        TestSessionInfo = testSessionInfo;
        _eventsHandler = eventsHandler;
        _consoleWrapper = consoleWrapper;
    }

    /// <summary>
    /// Destroys the current instance of the <see cref="TestSession"/> class.
    /// </summary>
    ~TestSession() => Dispose(false);

    /// <summary>
    /// Disposes of the current instance of the <see cref="TestSession"/> class.
    /// </summary>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes of the current instance of the <see cref="TestSession"/> class.
    /// </summary>
    ///
    /// <param name="disposing">Indicates if managed resources should be disposed.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            StopTestSession();
        }

        _disposed = true;
    }

    #region ITestSession
    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public void AbortTestRun()
    {
        _consoleWrapper.AbortTestRun();
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public void CancelDiscovery()
    {
        _consoleWrapper.CancelDiscovery();
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public void CancelTestRun()
    {
        _consoleWrapper.CancelTestRun();
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public void DiscoverTests(
        IEnumerable<string> sources,
        string discoverySettings,
        ITestDiscoveryEventsHandler discoveryEventsHandler)
    {
        DiscoverTests(
            sources,
            discoverySettings,
            options: null,
            discoveryEventsHandler: new DiscoveryEventsHandleConverter(discoveryEventsHandler));
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public void DiscoverTests(
        IEnumerable<string> sources,
        string discoverySettings,
        TestPlatformOptions? options,
        ITestDiscoveryEventsHandler2 discoveryEventsHandler)
    {
        _consoleWrapper.DiscoverTests(
            sources,
            discoverySettings,
            options,
            TestSessionInfo,
            discoveryEventsHandler);
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public void RunTests(
        IEnumerable<string> sources,
        string runSettings,
        ITestRunEventsHandler testRunEventsHandler)
    {
        RunTests(
            sources,
            runSettings,
            options: null,
            testRunEventsHandler);
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public void RunTests(
        IEnumerable<string> sources,
        string runSettings,
        TestPlatformOptions? options,
        ITestRunEventsHandler testRunEventsHandler)
    {
        _consoleWrapper.RunTests(
            sources,
            runSettings,
            options,
            TestSessionInfo,
            testRunEventsHandler);
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public void RunTests(
        IEnumerable<TestCase> testCases,
        string runSettings,
        ITestRunEventsHandler testRunEventsHandler)
    {
        RunTests(
            testCases,
            runSettings,
            options: null,
            testRunEventsHandler);
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public void RunTests(
        IEnumerable<TestCase> testCases,
        string runSettings,
        TestPlatformOptions? options,
        ITestRunEventsHandler testRunEventsHandler)
    {
        _consoleWrapper.RunTests(
            testCases,
            runSettings,
            options,
            TestSessionInfo,
            testRunEventsHandler);
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public void RunTestsWithCustomTestHost(
        IEnumerable<string> sources,
        string runSettings,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher customTestHostLauncher)
    {
        RunTestsWithCustomTestHost(
            sources,
            runSettings,
            options: null,
            testRunEventsHandler,
            customTestHostLauncher);
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public void RunTestsWithCustomTestHost(
        IEnumerable<string> sources,
        string runSettings,
        TestPlatformOptions? options,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher customTestHostLauncher)
    {
        _consoleWrapper.RunTestsWithCustomTestHost(
            sources,
            runSettings,
            options,
            TestSessionInfo,
            testRunEventsHandler,
            customTestHostLauncher);
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public void RunTestsWithCustomTestHost(
        IEnumerable<TestCase> testCases,
        string runSettings,
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
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public void RunTestsWithCustomTestHost(
        IEnumerable<TestCase> testCases,
        string runSettings,
        TestPlatformOptions? options,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher customTestHostLauncher)
    {
        _consoleWrapper.RunTestsWithCustomTestHost(
            testCases,
            runSettings,
            options,
            TestSessionInfo,
            testRunEventsHandler,
            customTestHostLauncher);
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public bool StopTestSession()
    {
        return StopTestSession(_eventsHandler);
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public bool StopTestSession(ITestSessionEventsHandler eventsHandler)
    {
        return StopTestSession(options: null, eventsHandler);
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public bool StopTestSession(
        TestPlatformOptions? options,
        ITestSessionEventsHandler eventsHandler)
    {
        if (TestSessionInfo == null)
        {
            return true;
        }

        try
        {
            return _consoleWrapper.StopTestSession(
                TestSessionInfo,
                options,
                eventsHandler);
        }
        finally
        {
            TestSessionInfo = null;
        }
    }
    #endregion

    #region ITestSessionAsync
    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public async Task DiscoverTestsAsync(
        IEnumerable<string> sources,
        string discoverySettings,
        ITestDiscoveryEventsHandler discoveryEventsHandler)
    {
        await DiscoverTestsAsync(
                sources,
                discoverySettings,
                options: null,
                discoveryEventsHandler:
                new DiscoveryEventsHandleConverter(discoveryEventsHandler))
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public async Task DiscoverTestsAsync(
        IEnumerable<string> sources,
        string discoverySettings,
        TestPlatformOptions? options,
        ITestDiscoveryEventsHandler2 discoveryEventsHandler)
    {
        await _consoleWrapper.DiscoverTestsAsync(
            sources,
            discoverySettings,
            options,
            TestSessionInfo,
            discoveryEventsHandler).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public async Task RunTestsAsync(
        IEnumerable<string> sources,
        string runSettings,
        ITestRunEventsHandler testRunEventsHandler)
    {
        await RunTestsAsync(
            sources,
            runSettings,
            options: null,
            testRunEventsHandler).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public async Task RunTestsAsync(
        IEnumerable<string> sources,
        string runSettings,
        TestPlatformOptions? options,
        ITestRunEventsHandler testRunEventsHandler)
    {
        await _consoleWrapper.RunTestsAsync(
            sources,
            runSettings,
            options,
            TestSessionInfo,
            testRunEventsHandler).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public async Task RunTestsAsync(
        IEnumerable<TestCase> testCases,
        string runSettings,
        ITestRunEventsHandler testRunEventsHandler)
    {
        await RunTestsAsync(
            testCases,
            runSettings,
            options: null,
            testRunEventsHandler).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public async Task RunTestsAsync(
        IEnumerable<TestCase> testCases,
        string runSettings,
        TestPlatformOptions? options,
        ITestRunEventsHandler testRunEventsHandler)
    {
        await _consoleWrapper.RunTestsAsync(
            testCases,
            runSettings,
            options,
            TestSessionInfo,
            testRunEventsHandler).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public async Task RunTestsWithCustomTestHostAsync(
        IEnumerable<string> sources,
        string runSettings,
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
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public async Task RunTestsWithCustomTestHostAsync(
        IEnumerable<string> sources,
        string runSettings,
        TestPlatformOptions? options,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher customTestHostLauncher)
    {
        await _consoleWrapper.RunTestsWithCustomTestHostAsync(
            sources,
            runSettings,
            options,
            TestSessionInfo,
            testRunEventsHandler,
            customTestHostLauncher).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public async Task RunTestsWithCustomTestHostAsync(
        IEnumerable<TestCase> testCases,
        string runSettings,
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
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public async Task RunTestsWithCustomTestHostAsync(
        IEnumerable<TestCase> testCases,
        string runSettings,
        TestPlatformOptions? options,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher customTestHostLauncher)
    {
        await _consoleWrapper.RunTestsWithCustomTestHostAsync(
            testCases,
            runSettings,
            options,
            TestSessionInfo,
            testRunEventsHandler,
            customTestHostLauncher).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public async Task<bool> StopTestSessionAsync()
    {
        return await StopTestSessionAsync(_eventsHandler).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public async Task<bool> StopTestSessionAsync(ITestSessionEventsHandler eventsHandler)
    {
        return await StopTestSessionAsync(options: null, eventsHandler).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public async Task<bool> StopTestSessionAsync(
        TestPlatformOptions? options,
        ITestSessionEventsHandler eventsHandler)
    {
        if (TestSessionInfo == null)
        {
            return true;
        }

        try
        {
            return await _consoleWrapper.StopTestSessionAsync(
                TestSessionInfo,
                options,
                eventsHandler).ConfigureAwait(false);
        }
        finally
        {
            TestSessionInfo = null;
        }
    }
    #endregion
}
