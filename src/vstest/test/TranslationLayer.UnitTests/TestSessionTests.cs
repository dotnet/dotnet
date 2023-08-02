// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;
using Microsoft.VisualStudio.TestPlatform.VsTestConsole.TranslationLayer;
using Microsoft.VisualStudio.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.TestPlatform.VsTestConsole.TranslationLayer.UnitTests;

[TestClass]
[Obsolete("This API is not final yet and is subject to changes.", false)]
public class TestSessionTests
{
    private readonly string _testSettings = "TestSettings";
    private readonly List<string> _testSources = new() { "Hello", "World" };
    private readonly List<TestCase> _testCases = new()
    {
        new TestCase("a.b.c", new Uri("d://uri"), "a.dll"),
        new TestCase("d.e.f", new Uri("g://uri"), "d.dll")
    };

    private readonly TestSessionInfo _testSessionInfo;
    private readonly ITestSession _testSession;
    private readonly Mock<ITestSessionEventsHandler> _mockTestSessionEventsHandler;
    private readonly Mock<IVsTestConsoleWrapper> _mockVsTestConsoleWrapper;

    public TestSessionTests()
    {
        _testSessionInfo = new TestSessionInfo();
        _mockTestSessionEventsHandler = new Mock<ITestSessionEventsHandler>();
        _mockVsTestConsoleWrapper = new Mock<IVsTestConsoleWrapper>();

        _testSession = new TestSession(
            _testSessionInfo,
            _mockTestSessionEventsHandler.Object,
            _mockVsTestConsoleWrapper.Object);
    }

    #region ITestSession
    [TestMethod]
    public void AbortTestRunShouldCallConsoleWrapperAbortTestRun()
    {
        _testSession.AbortTestRun();

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.AbortTestRun(),
            Times.Once);
    }

    [TestMethod]
    public void CancelDiscoveryShouldCallConsoleWrapperCancelDiscovery()
    {
        _testSession.CancelDiscovery();

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.CancelDiscovery(),
            Times.Once);
    }

    [TestMethod]
    public void CancelTestRunShouldCallConsoleWrapperCancelTestRun()
    {
        _testSession.CancelTestRun();

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.CancelTestRun(),
            Times.Once);
    }

    [TestMethod]
    public void DiscoverTestsShouldCallConsoleWrapperDiscoverTestsWithCorrectArguments1()
    {
        _testSession.DiscoverTests(
            _testSources,
            _testSettings,
            new Mock<ITestDiscoveryEventsHandler>().Object);

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.DiscoverTests(
                _testSources,
                _testSettings,
                null,
                _testSessionInfo,
                It.IsAny<ITestDiscoveryEventsHandler2>()),
            Times.Once);
    }

    [TestMethod]
    public void DiscoverTestsShouldCallConsoleWrapperDiscoverTestsWithCorrectArguments2()
    {
        var testPlatformOptions = new TestPlatformOptions();
        var mockTestDiscoveryEventsHandler = new Mock<ITestDiscoveryEventsHandler2>();

        _testSession.DiscoverTests(
            _testSources,
            _testSettings,
            testPlatformOptions,
            mockTestDiscoveryEventsHandler.Object);

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.DiscoverTests(
                _testSources,
                _testSettings,
                testPlatformOptions,
                _testSessionInfo,
                mockTestDiscoveryEventsHandler.Object),
            Times.Once);
    }

    [TestMethod]
    public void RunTestsWithSourcesShouldCallConsoleWrapperRunTestsWithCorrectArguments1()
    {
        var mockTestRunEventsHandler = new Mock<ITestRunEventsHandler>();

        _testSession.RunTests(
            _testSources,
            _testSettings,
            mockTestRunEventsHandler.Object);

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.RunTests(
                _testSources,
                _testSettings,
                null,
                _testSessionInfo,
                mockTestRunEventsHandler.Object,
                It.IsAny<NoOpTelemetryEventsHandler>()),
            Times.Once);
    }

    [TestMethod]
    public void RunTestsWithSourcesShouldCallConsoleWrapperRunTestsWithCorrectArguments2()
    {
        var testPlatformOptions = new TestPlatformOptions();
        var mockTestRunEventsHandler = new Mock<ITestRunEventsHandler>();

        _testSession.RunTests(
            _testSources,
            _testSettings,
            testPlatformOptions,
            mockTestRunEventsHandler.Object);

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.RunTests(
                _testSources,
                _testSettings,
                testPlatformOptions,
                _testSessionInfo,
                mockTestRunEventsHandler.Object,
                It.IsAny<NoOpTelemetryEventsHandler>()),
            Times.Once);
    }

    [TestMethod]
    public void RunTestsWithTestCasesShouldCallConsoleWrapperRunTestsWithCorrectArguments1()
    {
        var mockTestRunEventsHandler = new Mock<ITestRunEventsHandler>();

        _testSession.RunTests(
            _testCases,
            _testSettings,
            mockTestRunEventsHandler.Object);

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.RunTests(
                _testCases,
                _testSettings,
                null,
                _testSessionInfo,
                mockTestRunEventsHandler.Object,
                It.IsAny<NoOpTelemetryEventsHandler>()),
            Times.Once);
    }

    [TestMethod]
    public void RunTestsWithTestCasesShouldCallConsoleWrapperRunTestsWithCorrectArguments2()
    {
        var testPlatformOptions = new TestPlatformOptions();
        var mockTestRunEventsHandler = new Mock<ITestRunEventsHandler>();

        _testSession.RunTests(
            _testCases,
            _testSettings,
            testPlatformOptions,
            mockTestRunEventsHandler.Object);

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.RunTests(
                _testCases,
                _testSettings,
                testPlatformOptions,
                _testSessionInfo,
                mockTestRunEventsHandler.Object,
                It.IsAny<NoOpTelemetryEventsHandler>()),
            Times.Once);
    }

    [TestMethod]
    public void RunTestsWithTestCasesShouldCallConsoleWrapperRunTestsWithCorrectArgumentsAndTelemetryHandler()
    {
        var testPlatformOptions = new TestPlatformOptions();
        var mockTestRunEventsHandler = new Mock<ITestRunEventsHandler>();
        var telemetryEventsHandler = new Mock<ITelemetryEventsHandler>();

        _testSession.RunTests(
            _testCases,
            _testSettings,
            testPlatformOptions,
            mockTestRunEventsHandler.Object,
            telemetryEventsHandler.Object);

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.RunTests(
                _testCases,
                _testSettings,
                testPlatformOptions,
                _testSessionInfo,
                mockTestRunEventsHandler.Object,
                telemetryEventsHandler.Object),
            Times.Once);
    }

    [TestMethod]
    public void RunTestsWithSourcesAndCustomTesthostShouldCallConsoleWrapperRunTestsWithCorrectArguments1()
    {
        var mockTestRunEventsHandler = new Mock<ITestRunEventsHandler>();
        var mockTestHostLauncher = new Mock<ITestHostLauncher>();

        _testSession.RunTestsWithCustomTestHost(
            _testSources,
            _testSettings,
            mockTestRunEventsHandler.Object,
            mockTestHostLauncher.Object);

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.RunTestsWithCustomTestHost(
                _testSources,
                _testSettings,
                null,
                _testSessionInfo,
                mockTestRunEventsHandler.Object,
                It.IsAny<NoOpTelemetryEventsHandler>(),
                mockTestHostLauncher.Object),
            Times.Once);
    }

    [TestMethod]
    public void RunTestsWithSourcesAndCustomTesthostShouldCallConsoleWrapperRunTestsWithCorrectArguments2()
    {
        var testPlatformOptions = new TestPlatformOptions();
        var mockTestRunEventsHandler = new Mock<ITestRunEventsHandler>();
        var mockTestHostLauncher = new Mock<ITestHostLauncher>();

        _testSession.RunTestsWithCustomTestHost(
            _testSources,
            _testSettings,
            testPlatformOptions,
            mockTestRunEventsHandler.Object,
            mockTestHostLauncher.Object);

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.RunTestsWithCustomTestHost(
                _testSources,
                _testSettings,
                testPlatformOptions,
                _testSessionInfo,
                mockTestRunEventsHandler.Object,
                It.IsAny<NoOpTelemetryEventsHandler>(),
                mockTestHostLauncher.Object),
            Times.Once);
    }

    [TestMethod]
    public void RunTestsWithTestCasesAndCustomTesthostShouldCallConsoleWrapperRunTestsWithCorrectArguments1()
    {
        var mockTestRunEventsHandler = new Mock<ITestRunEventsHandler>();
        var mockTestHostLauncher = new Mock<ITestHostLauncher>();

        _testSession.RunTestsWithCustomTestHost(
            _testCases,
            _testSettings,
            mockTestRunEventsHandler.Object,
            mockTestHostLauncher.Object);

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.RunTestsWithCustomTestHost(
                _testCases,
                _testSettings,
                null,
                _testSessionInfo,
                mockTestRunEventsHandler.Object,
                It.IsAny<NoOpTelemetryEventsHandler>(),
                mockTestHostLauncher.Object),
            Times.Once);
    }

    [TestMethod]
    public void RunTestsWithTestCasesAndCustomTesthostShouldCallConsoleWrapperRunTestsWithCorrectArguments2()
    {
        var testPlatformOptions = new TestPlatformOptions();
        var mockTestRunEventsHandler = new Mock<ITestRunEventsHandler>();
        var mockTestHostLauncher = new Mock<ITestHostLauncher>();

        _testSession.RunTestsWithCustomTestHost(
            _testCases,
            _testSettings,
            testPlatformOptions,
            mockTestRunEventsHandler.Object,
            mockTestHostLauncher.Object);

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.RunTestsWithCustomTestHost(
                _testCases,
                _testSettings,
                testPlatformOptions,
                _testSessionInfo,
                mockTestRunEventsHandler.Object,
                It.IsAny<NoOpTelemetryEventsHandler>(),
                mockTestHostLauncher.Object),
            Times.Once);
    }

    [TestMethod]
    public void RunTestsWithTestCasesAndCustomTesthostShouldCallConsoleWrapperRunTestsWithCorrectArgumentsAndTelemetryHandler()
    {
        var testPlatformOptions = new TestPlatformOptions();
        var mockTestRunEventsHandler = new Mock<ITestRunEventsHandler>();
        var mockTestHostLauncher = new Mock<ITestHostLauncher>();
        var telemetryEventsHandler = new Mock<ITelemetryEventsHandler>();

        _testSession.RunTestsWithCustomTestHost(
            _testCases,
            _testSettings,
            testPlatformOptions,
            mockTestRunEventsHandler.Object,
            telemetryEventsHandler.Object,
            mockTestHostLauncher.Object);

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.RunTestsWithCustomTestHost(
                _testCases,
                _testSettings,
                testPlatformOptions,
                _testSessionInfo,
                mockTestRunEventsHandler.Object,
                telemetryEventsHandler.Object,
                mockTestHostLauncher.Object),
            Times.Once);
    }

    [TestMethod]
    public void StopTestSessionWithNoArgsShouldCallConsoleWrapperStopTestSessionWithCorrectArguments()
    {
        _testSession.StopTestSession();

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.StopTestSession(
                _testSessionInfo,
                null,
                _mockTestSessionEventsHandler.Object),
            Times.Once);
    }

    [TestMethod]
    public void StopTestSessionWithOneArgShouldCallConsoleWrapperStopTestSessionWithCorrectArguments()
    {
        var mockTestSessionEventsHandler2 = new Mock<ITestSessionEventsHandler>();

        _testSession.StopTestSession(mockTestSessionEventsHandler2.Object);

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.StopTestSession(
                _testSessionInfo,
                null,
                mockTestSessionEventsHandler2.Object),
            Times.Once);
    }

    [TestMethod]
    public void StopTestSessionWithTwoArgsShouldCallConsoleWrapperStopTestSessionWithCorrectArguments()
    {
        var testPlatformOptions = new TestPlatformOptions();
        var mockTestSessionEventsHandler2 = new Mock<ITestSessionEventsHandler>();

        _testSession.StopTestSession(testPlatformOptions, mockTestSessionEventsHandler2.Object);

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.StopTestSession(
                _testSessionInfo,
                testPlatformOptions,
                mockTestSessionEventsHandler2.Object),
            Times.Once);
    }
    #endregion

    #region ITestSessionAsync
    [TestMethod]
    public async Task DiscoverTestsAsyncShouldCallConsoleWrapperDiscoverTestsWithCorrectArguments1()
    {
        await _testSession.DiscoverTestsAsync(
                _testSources,
                _testSettings,
                new Mock<ITestDiscoveryEventsHandler>().Object)
            .ConfigureAwait(false);

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.DiscoverTestsAsync(
                _testSources,
                _testSettings,
                null,
                _testSessionInfo,
                It.IsAny<ITestDiscoveryEventsHandler2>()),
            Times.Once);
    }

    [TestMethod]
    public async Task DiscoverTestsAsyncShouldCallConsoleWrapperDiscoverTestsWithCorrectArguments2()
    {
        var testPlatformOptions = new TestPlatformOptions();
        var mockTestDiscoveryEventsHandler = new Mock<ITestDiscoveryEventsHandler2>();

        await _testSession.DiscoverTestsAsync(
                _testSources,
                _testSettings,
                testPlatformOptions,
                mockTestDiscoveryEventsHandler.Object)
            .ConfigureAwait(false); ;

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.DiscoverTestsAsync(
                _testSources,
                _testSettings,
                testPlatformOptions,
                _testSessionInfo,
                mockTestDiscoveryEventsHandler.Object),
            Times.Once);
    }

    [TestMethod]
    public async Task RunTestsAsyncWithSourcesShouldCallConsoleWrapperRunTestsWithCorrectArguments1()
    {
        var mockTestRunEventsHandler = new Mock<ITestRunEventsHandler>();

        await _testSession.RunTestsAsync(
                _testSources,
                _testSettings,
                mockTestRunEventsHandler.Object)
            .ConfigureAwait(false);

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.RunTestsAsync(
                _testSources,
                _testSettings,
                null,
                _testSessionInfo,
                mockTestRunEventsHandler.Object,
                It.IsAny<NoOpTelemetryEventsHandler>()),
            Times.Once);
    }

    [TestMethod]
    public async Task RunTestsAsyncWithSourcesShouldCallConsoleWrapperRunTestsWithCorrectArguments2()
    {
        var testPlatformOptions = new TestPlatformOptions();
        var mockTestRunEventsHandler = new Mock<ITestRunEventsHandler>();

        await _testSession.RunTestsAsync(
                _testSources,
                _testSettings,
                testPlatformOptions,
                mockTestRunEventsHandler.Object)
            .ConfigureAwait(false);

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.RunTestsAsync(
                _testSources,
                _testSettings,
                testPlatformOptions,
                _testSessionInfo,
                mockTestRunEventsHandler.Object,
                It.IsAny<NoOpTelemetryEventsHandler>()),
            Times.Once);
    }

    [TestMethod]
    public async Task RunTestsAsyncWithTestCasesShouldCallConsoleWrapperRunTestsWithCorrectArguments1()
    {
        var mockTestRunEventsHandler = new Mock<ITestRunEventsHandler>();

        await _testSession.RunTestsAsync(
                _testCases,
                _testSettings,
                mockTestRunEventsHandler.Object)
            .ConfigureAwait(false);

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.RunTestsAsync(
                _testCases,
                _testSettings,
                null,
                _testSessionInfo,
                mockTestRunEventsHandler.Object,
                It.IsAny<NoOpTelemetryEventsHandler>()),
            Times.Once);
    }

    [TestMethod]
    public async Task RunTestsAsyncWithTestCasesShouldCallConsoleWrapperRunTestsWithCorrectArguments2()
    {
        var testPlatformOptions = new TestPlatformOptions();
        var mockTestRunEventsHandler = new Mock<ITestRunEventsHandler>();

        await _testSession.RunTestsAsync(
                _testCases,
                _testSettings,
                testPlatformOptions,
                mockTestRunEventsHandler.Object)
            .ConfigureAwait(false); ;

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.RunTestsAsync(
                _testCases,
                _testSettings,
                testPlatformOptions,
                _testSessionInfo,
                mockTestRunEventsHandler.Object,
                It.IsAny<NoOpTelemetryEventsHandler>()),
            Times.Once);
    }

    [TestMethod]
    public async Task RunTestsAsyncWithTestCasesShouldCallConsoleWrapperRunTestsWithCorrectArgumentsWithTelemetryHandler()
    {
        var testPlatformOptions = new TestPlatformOptions();
        var mockTestRunEventsHandler = new Mock<ITestRunEventsHandler>();
        var telemetryEventsHandler = new Mock<ITelemetryEventsHandler>();

        await _testSession.RunTestsAsync(
                _testCases,
                _testSettings,
                testPlatformOptions,
                mockTestRunEventsHandler.Object,
                telemetryEventsHandler.Object)
            .ConfigureAwait(false); ;

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.RunTestsAsync(
                _testCases,
                _testSettings,
                testPlatformOptions,
                _testSessionInfo,
                mockTestRunEventsHandler.Object,
                telemetryEventsHandler.Object),
            Times.Once);
    }

    [TestMethod]
    public async Task RunTestsAsyncWithSourcesAndCustomTesthostShouldCallConsoleWrapperRunTestsWithCorrectArguments1()
    {
        var mockTestRunEventsHandler = new Mock<ITestRunEventsHandler>();
        var mockTestHostLauncher = new Mock<ITestHostLauncher>();

        await _testSession.RunTestsWithCustomTestHostAsync(
                _testSources,
                _testSettings,
                mockTestRunEventsHandler.Object,
                mockTestHostLauncher.Object)
            .ConfigureAwait(false);

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.RunTestsWithCustomTestHostAsync(
                _testSources,
                _testSettings,
                null,
                _testSessionInfo,
                mockTestRunEventsHandler.Object,
                It.IsAny<NoOpTelemetryEventsHandler>(),
                mockTestHostLauncher.Object),
            Times.Once);
    }

    [TestMethod]
    public async Task RunTestsAsyncWithSourcesAndCustomTesthostShouldCallConsoleWrapperRunTestsWithCorrectArguments2()
    {
        var testPlatformOptions = new TestPlatformOptions();
        var mockTestRunEventsHandler = new Mock<ITestRunEventsHandler>();
        var mockTestHostLauncher = new Mock<ITestHostLauncher>();

        await _testSession.RunTestsWithCustomTestHostAsync(
                _testSources,
                _testSettings,
                testPlatformOptions,
                mockTestRunEventsHandler.Object,
                mockTestHostLauncher.Object)
            .ConfigureAwait(false);

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.RunTestsWithCustomTestHostAsync(
                _testSources,
                _testSettings,
                testPlatformOptions,
                _testSessionInfo,
                mockTestRunEventsHandler.Object,
                It.IsAny<NoOpTelemetryEventsHandler>(),
                mockTestHostLauncher.Object),
            Times.Once);
    }

    [TestMethod]
    public async Task RunTestsAsyncWithTestCasesAndCustomTesthostShouldCallConsoleWrapperRunTestsWithCorrectArguments1()
    {
        var mockTestRunEventsHandler = new Mock<ITestRunEventsHandler>();
        var mockTestHostLauncher = new Mock<ITestHostLauncher>();

        await _testSession.RunTestsWithCustomTestHostAsync(
                _testCases,
                _testSettings,
                mockTestRunEventsHandler.Object,
                mockTestHostLauncher.Object)
            .ConfigureAwait(false);

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.RunTestsWithCustomTestHostAsync(
                _testCases,
                _testSettings,
                null,
                _testSessionInfo,
                mockTestRunEventsHandler.Object,
                It.IsAny<NoOpTelemetryEventsHandler>(),
                mockTestHostLauncher.Object),
            Times.Once);
    }

    [TestMethod]
    public async Task RunTestsAsyncWithTestCasesAndCustomTesthostShouldCallConsoleWrapperRunTestsWithCorrectArguments2()
    {
        var testPlatformOptions = new TestPlatformOptions();
        var mockTestRunEventsHandler = new Mock<ITestRunEventsHandler>();
        var mockTestHostLauncher = new Mock<ITestHostLauncher>();

        await _testSession.RunTestsWithCustomTestHostAsync(
                _testCases,
                _testSettings,
                testPlatformOptions,
                mockTestRunEventsHandler.Object,
                mockTestHostLauncher.Object)
            .ConfigureAwait(false);

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.RunTestsWithCustomTestHostAsync(
                _testCases,
                _testSettings,
                testPlatformOptions,
                _testSessionInfo,
                mockTestRunEventsHandler.Object,
                It.IsAny<NoOpTelemetryEventsHandler>(),
                mockTestHostLauncher.Object),
            Times.Once);
    }

    [TestMethod]
    public async Task RunTestsAsyncWithTestCasesAndCustomTesthostShouldCallConsoleWrapperRunTestsWithCorrectArgumentsWithTelemetryHandler()
    {
        var testPlatformOptions = new TestPlatformOptions();
        var mockTestRunEventsHandler = new Mock<ITestRunEventsHandler>();
        var mockTestHostLauncher = new Mock<ITestHostLauncher>();
        var telemetryEventsHandler = new Mock<ITelemetryEventsHandler>();

        await _testSession.RunTestsWithCustomTestHostAsync(
                _testCases,
                _testSettings,
                testPlatformOptions,
                mockTestRunEventsHandler.Object,
                telemetryEventsHandler.Object,
                mockTestHostLauncher.Object)
            .ConfigureAwait(false);

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.RunTestsWithCustomTestHostAsync(
                _testCases,
                _testSettings,
                testPlatformOptions,
                _testSessionInfo,
                mockTestRunEventsHandler.Object,
                telemetryEventsHandler.Object,
                mockTestHostLauncher.Object),
            Times.Once);
    }

    [TestMethod]
    public async Task StopTestSessionAsyncWithNoArgsShouldCallConsoleWrapperStopTestSessionWithCorrectArguments()
    {
        await _testSession.StopTestSessionAsync().ConfigureAwait(false);

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.StopTestSessionAsync(
                _testSessionInfo,
                null,
                _mockTestSessionEventsHandler.Object),
            Times.Once);
    }

    [TestMethod]
    public async Task StopTestSessionAsyncWithOneArgShouldCallConsoleWrapperStopTestSessionWithCorrectArguments()
    {
        var mockTestSessionEventsHandler2 = new Mock<ITestSessionEventsHandler>();

        await _testSession.StopTestSessionAsync(
                mockTestSessionEventsHandler2.Object)
            .ConfigureAwait(false);

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.StopTestSessionAsync(
                _testSessionInfo,
                null,
                mockTestSessionEventsHandler2.Object),
            Times.Once);
    }

    [TestMethod]
    public async Task StopTestSessionAsyncWithTwoArgsShouldCallConsoleWrapperStopTestSessionWithCorrectArguments()
    {
        var testPlatformOptions = new TestPlatformOptions();
        var mockTestSessionEventsHandler2 = new Mock<ITestSessionEventsHandler>();

        await _testSession.StopTestSessionAsync(
                testPlatformOptions,
                mockTestSessionEventsHandler2.Object)
            .ConfigureAwait(false);

        _mockVsTestConsoleWrapper.Verify(
            vtcw => vtcw.StopTestSessionAsync(
                _testSessionInfo,
                testPlatformOptions,
                mockTestSessionEventsHandler2.Object),
            Times.Once);
    }
    #endregion
}
