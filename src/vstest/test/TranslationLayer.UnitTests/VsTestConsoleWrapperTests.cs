// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestPlatform.VsTestConsole.TranslationLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.TestPlatform.VsTestConsole.TranslationLayer.UnitTests;

[TestClass]
public class VsTestConsoleWrapperTests
{
    private readonly IVsTestConsoleWrapper _consoleWrapper;
    private readonly Mock<IProcessManager> _mockProcessManager;
    private readonly Mock<IProcessHelper> _mockProcessHelper;
    private readonly Mock<ITranslationLayerRequestSender> _mockRequestSender;
    private readonly List<string> _testSources = new() { "Hello", "World" };
    private readonly List<TestCase> _testCases = new()
    {
        new TestCase("a.b.c", new Uri("d://uri"), "a.dll"),
        new TestCase("d.e.f", new Uri("g://uri"), "d.dll")
    };
    private readonly ConsoleParameters _consoleParameters;
    private readonly Mock<ITelemetryEventsHandler> _telemetryEventsHandler;

    public VsTestConsoleWrapperTests()
    {
        _consoleParameters = new ConsoleParameters();

        _mockRequestSender = new Mock<ITranslationLayerRequestSender>();
        _mockProcessManager = new Mock<IProcessManager>();
        _mockProcessHelper = new Mock<IProcessHelper>();
        _consoleWrapper = new VsTestConsoleWrapper(
            _mockRequestSender.Object,
            _mockProcessManager.Object,
            _consoleParameters,
            new Mock<ITestPlatformEventSource>().Object,
            _mockProcessHelper.Object);
        _telemetryEventsHandler = new Mock<ITelemetryEventsHandler>();

        _mockRequestSender.Setup(rs => rs.WaitForRequestHandlerConnection(It.IsAny<int>())).Returns(true);
        _mockRequestSender.Setup(rs => rs.InitializeCommunication()).Returns(100);
        _mockRequestSender.Setup(rs => rs.InitializeCommunicationAsync(It.IsAny<int>())).Returns(Task.FromResult(100));
    }

    [TestMethod]
    public void StartSessionShouldStartVsTestConsoleWithCorrectArguments()
    {
        var inputPort = 123;
#if NET5_0_OR_GREATER
        var expectedParentProcessId = Environment.ProcessId;
#else
        int expectedParentProcessId;
        using (var p = Process.GetCurrentProcess())
            expectedParentProcessId = p.Id;
#endif
        _mockRequestSender.Setup(rs => rs.InitializeCommunication()).Returns(inputPort);

        _consoleWrapper.StartSession();

        Assert.AreEqual(expectedParentProcessId, _consoleParameters.ParentProcessId, "Parent process Id must be set");
        Assert.AreEqual(inputPort, _consoleParameters.PortNumber, "Port number must be set");
        Assert.AreEqual(TraceLevel.Verbose, _consoleParameters.TraceLevel, "Default value of trace level should be verbose.");

        _mockProcessManager.Verify(pm => pm.StartProcess(_consoleParameters), Times.Once);
    }

    [TestMethod]
    public void StartSessionShouldThrowExceptionOnBadPort()
    {
        _mockRequestSender.Setup(rs => rs.InitializeCommunication()).Returns(-1);

        Assert.ThrowsException<TransationLayerException>(() => _consoleWrapper.StartSession());
    }

    [TestMethod]
    public void StartSessionShouldCallWhenProcessNotInitialized()
    {
        _mockProcessManager.Setup(pm => pm.IsProcessInitialized()).Returns(false);

        // To call private method EnsureInitialize call InitializeExtensions
        _consoleWrapper.InitializeExtensions(new[] { "path/to/adapter" });

        _mockProcessManager.Verify(pm => pm.StartProcess(It.IsAny<ConsoleParameters>()));
    }

    [TestMethod]
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public void StartTestSessionShouldCallRequestSenderWithCorrectArguments1()
    {
        var testSessionInfo = new TestSessionInfo();
        var mockEventsHandler = new Mock<ITestSessionEventsHandler>();

        _mockRequestSender.Setup(
                rs => rs.StartTestSession(
                    _testSources,
                    null,
                    null,
                    mockEventsHandler.Object,
                    null))
            .Returns(testSessionInfo);

        Assert.AreEqual(
            _consoleWrapper.StartTestSession(
                _testSources,
                null,
                mockEventsHandler.Object)?.TestSessionInfo,
            testSessionInfo);

        _mockRequestSender.Verify(
            rs => rs.StartTestSession(
                _testSources,
                null,
                null,
                mockEventsHandler.Object,
                null),
            Times.Once);
    }

    [TestMethod]
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public void StartTestSessionShouldCallRequestSenderWithCorrectArguments2()
    {
        var testSessionInfo = new TestSessionInfo();
        var testPlatformOptions = new TestPlatformOptions();
        var mockEventsHandler = new Mock<ITestSessionEventsHandler>();

        _mockRequestSender.Setup(
                rs => rs.StartTestSession(
                    _testSources,
                    null,
                    testPlatformOptions,
                    mockEventsHandler.Object,
                    null))
            .Returns(testSessionInfo);

        Assert.AreEqual(
            _consoleWrapper.StartTestSession(
                _testSources,
                null,
                testPlatformOptions,
                mockEventsHandler.Object)?.TestSessionInfo,
            testSessionInfo);

        _mockRequestSender.Verify(
            rs => rs.StartTestSession(
                _testSources,
                null,
                testPlatformOptions,
                mockEventsHandler.Object,
                null),
            Times.Once);
    }

    [TestMethod]
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public void StartTestSessionShouldCallRequestSenderWithCorrectArguments3()
    {
        var testSessionInfo = new TestSessionInfo();
        var testPlatformOptions = new TestPlatformOptions();
        var mockEventsHandler = new Mock<ITestSessionEventsHandler>();
        var mockTesthostLauncher = new Mock<ITestHostLauncher>();

        _mockRequestSender.Setup(
                rs => rs.StartTestSession(
                    _testSources,
                    null,
                    testPlatformOptions,
                    mockEventsHandler.Object,
                    mockTesthostLauncher.Object))
            .Returns(testSessionInfo);

        Assert.AreEqual(
            _consoleWrapper.StartTestSession(
                _testSources,
                null,
                testPlatformOptions,
                mockEventsHandler.Object,
                mockTesthostLauncher.Object)?.TestSessionInfo,
            testSessionInfo);

        _mockRequestSender.Verify(
            rs => rs.StartTestSession(
                _testSources,
                null,
                testPlatformOptions,
                mockEventsHandler.Object,
                mockTesthostLauncher.Object),
            Times.Once);
    }

    [TestMethod]
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public void StopTestSessionShouldCallRequestSenderWithCorrectArguments()
    {
        var testSessionInfo = new TestSessionInfo();
        var testPlatformOptions = new TestPlatformOptions();
        var mockEventsHandler = new Mock<ITestSessionEventsHandler>();

        _mockRequestSender.Setup(
                rs => rs.StopTestSession(
                    It.IsAny<TestSessionInfo>(),
                    It.IsAny<TestPlatformOptions>(),
                    It.IsAny<ITestSessionEventsHandler>()))
            .Returns(true);

        Assert.IsTrue(
            _consoleWrapper.StopTestSession(
                testSessionInfo,
                testPlatformOptions,
                mockEventsHandler.Object));

        _mockRequestSender.Verify(
            rs => rs.StopTestSession(
                testSessionInfo,
                testPlatformOptions,
                mockEventsHandler.Object),
            Times.Once);
    }

    [TestMethod]
    public void InitializeExtensionsShouldCachePathToExtensions()
    {
        var pathToExtensions = new[] { "path/to/adapter" };
        _mockProcessManager.Setup(pm => pm.IsProcessInitialized()).Returns(true);

        _consoleWrapper.InitializeExtensions(pathToExtensions);

        _mockProcessManager.Setup(pm => pm.IsProcessInitialized()).Returns(false);
        _mockRequestSender.Setup(rs => rs.InitializeCommunication()).Returns(100);

        _consoleWrapper.InitializeExtensions(pathToExtensions);

        _mockRequestSender.Verify(rs => rs.InitializeExtensions(pathToExtensions), Times.Exactly(3));
    }

    [TestMethod]
    public void ProcessExitedEventShouldSetOnProcessExit()
    {
        _mockProcessManager.Raise(pm => pm.ProcessExited += null, EventArgs.Empty);

        _mockRequestSender.Verify(rs => rs.OnProcessExited(), Times.Once);
    }

    [TestMethod]
    public void InitializeExtensionsShouldSucceed()
    {
        var pathToAdditionalExtensions = new List<string> { "Hello", "World" };

        _consoleWrapper.InitializeExtensions(pathToAdditionalExtensions);

        _mockRequestSender.Verify(rs => rs.InitializeExtensions(pathToAdditionalExtensions), Times.Once);
    }

    [TestMethod]
    public void InitializeExtensionsShouldThrowExceptionOnBadConnection()
    {
        _mockProcessHelper.Setup(x => x.GetCurrentProcessFileName()).Returns("DummyProcess");
        _mockRequestSender.Setup(rs => rs.WaitForRequestHandlerConnection(It.IsAny<int>())).Returns(false);

        var exception = Assert.ThrowsException<TransationLayerException>(() => _consoleWrapper.InitializeExtensions(new List<string> { "Hello", "World" }));
        Assert.AreEqual("DummyProcess process failed to connect to vstest.console process after 90 seconds. This may occur due to machine slowness, please set environment variable VSTEST_CONNECTION_TIMEOUT to increase timeout.", exception.Message);
        _mockRequestSender.Verify(rs => rs.InitializeExtensions(It.IsAny<IEnumerable<string>>()), Times.Never);
    }

    [TestMethod]
    public void DiscoverTestsShouldSucceed()
    {
        var options = new TestPlatformOptions() { TestCaseFilter = "PacMan" };
        _consoleWrapper.DiscoverTests(_testSources, null, options, new Mock<ITestDiscoveryEventsHandler2>().Object);

        _mockRequestSender.Verify(rs => rs.DiscoverTests(_testSources, null, options, null, It.IsAny<ITestDiscoveryEventsHandler2>()), Times.Once);
    }

    [TestMethod]
    public void DiscoverTestsShouldPassOnNullOptions()
    {
        _consoleWrapper.DiscoverTests(_testSources, null, null, new Mock<ITestDiscoveryEventsHandler2>().Object);

        _mockRequestSender.Verify(rs => rs.DiscoverTests(_testSources, null, null, null, It.IsAny<ITestDiscoveryEventsHandler2>()), Times.Once);
    }

    [TestMethod]
    public void DiscoverTestsShouldCallTestDiscoveryHandler2IfTestDiscoveryHandler1IsUsed()
    {
        _consoleWrapper.DiscoverTests(_testSources, null, new Mock<ITestDiscoveryEventsHandler>().Object);

        _mockRequestSender.Verify(rs => rs.DiscoverTests(_testSources, null, null, null, It.IsAny<ITestDiscoveryEventsHandler2>()), Times.Once);
    }

    [TestMethod]
    public void DiscoverTestsShouldSucceedWhenUsingSessions()
    {
        var testSessionInfo = new TestSessionInfo();

        _consoleWrapper.DiscoverTests(
            _testSources,
            null,
            null,
            testSessionInfo,
            new Mock<ITestDiscoveryEventsHandler2>().Object);

        _mockRequestSender.Verify(
            rs => rs.DiscoverTests(
                _testSources,
                null,
                null,
                testSessionInfo,
                It.IsAny<ITestDiscoveryEventsHandler2>()),
            Times.Once);
    }

    [TestMethod]
    public void DiscoverTestsShouldThrowExceptionOnBadConnection()
    {
        _mockProcessHelper.Setup(x => x.GetCurrentProcessFileName()).Returns("DummyProcess");
        _mockRequestSender.Setup(rs => rs.WaitForRequestHandlerConnection(It.IsAny<int>())).Returns(false);

        var exception = Assert.ThrowsException<TransationLayerException>(() => _consoleWrapper.DiscoverTests(new List<string> { "Hello", "World" }, null, null, new Mock<ITestDiscoveryEventsHandler2>().Object));
        Assert.AreEqual("DummyProcess process failed to connect to vstest.console process after 90 seconds. This may occur due to machine slowness, please set environment variable VSTEST_CONNECTION_TIMEOUT to increase timeout.", exception.Message);
        _mockRequestSender.Verify(rs => rs.DiscoverTests(It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), null, null, It.IsAny<ITestDiscoveryEventsHandler2>()), Times.Never);
    }

    [TestMethod]
    public void RunTestsWithSourcesShouldSucceed()
    {
        _consoleWrapper.RunTests(_testSources, "RunSettings", new Mock<ITestRunEventsHandler>().Object);

        _mockRequestSender.Verify(rs => rs.StartTestRun(_testSources, "RunSettings", It.IsAny<TestPlatformOptions>(), null, It.IsAny<ITestRunEventsHandler>(), It.IsAny<NoOpTelemetryEventsHandler>()), Times.Once);
    }

    [TestMethod]
    public void RunTestsWithSourcesAndNullOptionsShouldPassOnNullOptions()
    {
        _consoleWrapper.RunTests(
            _testSources,
            "RunSettings",
            null,
            new Mock<ITestRunEventsHandler>().Object);

        _mockRequestSender.Verify(rs => rs.StartTestRun(_testSources, "RunSettings", null, null, It.IsAny<ITestRunEventsHandler>(), It.IsAny<NoOpTelemetryEventsHandler>()), Times.Once);
    }

    [TestMethod]
    public void RunTestsWithSourcesAndOptionsShouldPassOnOptions()
    {
        var options = new TestPlatformOptions() { TestCaseFilter = "PacMan" };
        _consoleWrapper.RunTests(
            _testSources,
            "RunSettings",
            options,
            new Mock<ITestRunEventsHandler>().Object);

        _mockRequestSender.Verify(rs => rs.StartTestRun(_testSources, "RunSettings", options, null, It.IsAny<ITestRunEventsHandler>(), It.IsAny<NoOpTelemetryEventsHandler>()), Times.Once);
    }

    [TestMethod]
    public void RunTestsWithSourcesShouldSucceedWhenUsingSessions()
    {
        var testSessionInfo = new TestSessionInfo();
        var options = new TestPlatformOptions() { TestCaseFilter = "PacMan" };
        _consoleWrapper.RunTests(
            _testSources,
            "RunSettings",
            options,
            testSessionInfo,
            new Mock<ITestRunEventsHandler>().Object);

        _mockRequestSender.Verify(
            rs => rs.StartTestRun(
                _testSources,
                "RunSettings",
                options,
                testSessionInfo,
                It.IsAny<ITestRunEventsHandler>(),
                It.IsAny<NoOpTelemetryEventsHandler>()),
            Times.Once);
    }

    [TestMethod]
    public void RunTestsWithSourcesShouldSucceedWhenUsingSessionsAndTelemetryHandler()
    {
        var testSessionInfo = new TestSessionInfo();
        var options = new TestPlatformOptions() { TestCaseFilter = "PacMan" };

        _consoleWrapper.RunTests(
            _testSources,
            "RunSettings",
            options,
            testSessionInfo,
            new Mock<ITestRunEventsHandler>().Object,
            _telemetryEventsHandler.Object);

        _mockRequestSender.Verify(
            rs => rs.StartTestRun(
                _testSources,
                "RunSettings",
                options,
                testSessionInfo,
                It.IsAny<ITestRunEventsHandler>(),
                _telemetryEventsHandler.Object),
            Times.Once);
    }

    [TestMethod]
    public void RunTestsWithSourcesAndCustomHostShouldSucceed()
    {
        _consoleWrapper.RunTestsWithCustomTestHost(
            _testSources,
            "RunSettings",
            new Mock<ITestRunEventsHandler>().Object,
            new Mock<ITestHostLauncher>().Object);

        _mockRequestSender.Verify(rs => rs.StartTestRunWithCustomHost(_testSources, "RunSettings", It.IsAny<TestPlatformOptions>(), null, It.IsAny<ITestRunEventsHandler>(), It.IsAny<NoOpTelemetryEventsHandler>(), It.IsAny<ITestHostLauncher>()), Times.Once);
    }

    [TestMethod]
    public void RunTestsWithSourcesAndOptionsUsingACustomHostShouldPassOnOptions()
    {
        var options = new TestPlatformOptions() { TestCaseFilter = "PacMan" };
        _consoleWrapper.RunTestsWithCustomTestHost(
            _testSources,
            "RunSettings",
            options,
            new Mock<ITestRunEventsHandler>().Object,
            new Mock<ITestHostLauncher>().Object);

        _mockRequestSender.Verify(
            rs => rs.StartTestRunWithCustomHost(
                _testSources,
                "RunSettings",
                options,
                null,
                It.IsAny<ITestRunEventsHandler>(),
                It.IsAny<NoOpTelemetryEventsHandler>(),
                It.IsAny<ITestHostLauncher>()),
            Times.Once);
    }

    [TestMethod]
    public void RunTestsWithSourcesAndACustomHostShouldSucceedWhenUsingSessions()
    {
        var testSessionInfo = new TestSessionInfo();
        var options = new TestPlatformOptions() { TestCaseFilter = "PacMan" };
        _consoleWrapper.RunTestsWithCustomTestHost(
            _testSources,
            "RunSettings",
            options,
            testSessionInfo,
            new Mock<ITestRunEventsHandler>().Object,
            new Mock<ITestHostLauncher>().Object);

        _mockRequestSender.Verify(
            rs => rs.StartTestRunWithCustomHost(
                _testSources,
                "RunSettings",
                options,
                testSessionInfo,
                It.IsAny<ITestRunEventsHandler>(),
                It.IsAny<NoOpTelemetryEventsHandler>(),
                It.IsAny<ITestHostLauncher>()),
            Times.Once);
    }

    [TestMethod]
    public void RunTestsWithSourcesAndACustomHostShouldSucceedWhenUsingSessionsWithTelemetryHandler()
    {
        var testSessionInfo = new TestSessionInfo();
        var options = new TestPlatformOptions() { TestCaseFilter = "PacMan" };
        _consoleWrapper.RunTestsWithCustomTestHost(
            _testSources,
            "RunSettings",
            options,
            testSessionInfo,
            new Mock<ITestRunEventsHandler>().Object,
            _telemetryEventsHandler.Object,
            new Mock<ITestHostLauncher>().Object);

        _mockRequestSender.Verify(
            rs => rs.StartTestRunWithCustomHost(
                _testSources,
                "RunSettings",
                options,
                testSessionInfo,
                It.IsAny<ITestRunEventsHandler>(),
                _telemetryEventsHandler.Object,
                It.IsAny<ITestHostLauncher>()),
            Times.Once);
    }

    [TestMethod]
    public void RunTestsWithSelectedTestsShouldSucceed()
    {
        _consoleWrapper.RunTests(_testCases, "RunSettings", new Mock<ITestRunEventsHandler>().Object);

        _mockRequestSender.Verify(rs => rs.StartTestRun(_testCases, "RunSettings", It.IsAny<TestPlatformOptions>(), null, It.IsAny<ITestRunEventsHandler>(), It.IsAny<NoOpTelemetryEventsHandler>()), Times.Once);
    }

    [TestMethod]
    public void RunTestsWithSelectedTestsAndNullOptionsShouldPassOnNullOptions()
    {
        _consoleWrapper.RunTests(_testCases, "RunSettings", null, new Mock<ITestRunEventsHandler>().Object);

        _mockRequestSender.Verify(rs => rs.StartTestRun(_testCases, "RunSettings", null, null, It.IsAny<ITestRunEventsHandler>(), It.IsAny<NoOpTelemetryEventsHandler>()), Times.Once);
    }

    [TestMethod]
    public void RunTestsWithSelectedTestsAndOptionsShouldPassOnOptions()
    {
        var options = new TestPlatformOptions() { TestCaseFilter = "PacMan" };

        _consoleWrapper.RunTests(_testCases, "RunSettings", options, new Mock<ITestRunEventsHandler>().Object);

        _mockRequestSender.Verify(rs => rs.StartTestRun(_testCases, "RunSettings", options, null, It.IsAny<ITestRunEventsHandler>(), It.IsAny<NoOpTelemetryEventsHandler>()), Times.Once);
    }

    [TestMethod]
    public void RunTestsWithSelectedTestsShouldSucceedWhenUsingSessions()
    {
        var testSessionInfo = new TestSessionInfo();
        var options = new TestPlatformOptions() { TestCaseFilter = "PacMan" };

        _consoleWrapper.RunTests(
            _testCases,
            "RunSettings",
            options,
            testSessionInfo,
            new Mock<ITestRunEventsHandler>().Object);

        _mockRequestSender.Verify(
            rs => rs.StartTestRun(
                _testCases,
                "RunSettings",
                options,
                testSessionInfo,
                It.IsAny<ITestRunEventsHandler>(),
                It.IsAny<NoOpTelemetryEventsHandler>()),
            Times.Once);
    }

    [TestMethod]
    public void RunTestsWithSelectedTestsShouldSucceedWhenUsingSessionsWithTelemetryHandler()
    {
        var testSessionInfo = new TestSessionInfo();
        var options = new TestPlatformOptions() { TestCaseFilter = "PacMan" };

        _consoleWrapper.RunTests(
            _testCases,
            "RunSettings",
            options,
            testSessionInfo,
            new Mock<ITestRunEventsHandler>().Object,
            _telemetryEventsHandler.Object);

        _mockRequestSender.Verify(
            rs => rs.StartTestRun(
                _testCases,
                "RunSettings",
                options,
                testSessionInfo,
                It.IsAny<ITestRunEventsHandler>(),
                _telemetryEventsHandler.Object),
            Times.Once);
    }

    [TestMethod]
    public void RunTestsWithSelectedTestsAndCustomLauncherShouldSucceed()
    {
        _consoleWrapper.RunTestsWithCustomTestHost(
            _testCases,
            "RunSettings",
            new Mock<ITestRunEventsHandler>().Object,
            new Mock<ITestHostLauncher>().Object);

        _mockRequestSender.Verify(rs => rs.StartTestRunWithCustomHost(_testCases, "RunSettings", It.IsAny<TestPlatformOptions>(), null, It.IsAny<ITestRunEventsHandler>(), It.IsAny<NoOpTelemetryEventsHandler>(), It.IsAny<ITestHostLauncher>()), Times.Once);
    }

    [TestMethod]
    public void RunTestsWithSelectedTestsAndNullOptionsUsingACustomHostShouldPassOnNullOptions()
    {
        _consoleWrapper.RunTestsWithCustomTestHost(
            _testCases,
            "RunSettings",
            null,
            new Mock<ITestRunEventsHandler>().Object,
            new Mock<ITestHostLauncher>().Object);

        _mockRequestSender.Verify(rs => rs.StartTestRunWithCustomHost(_testCases, "RunSettings", null, null, It.IsAny<ITestRunEventsHandler>(), It.IsAny<NoOpTelemetryEventsHandler>(), It.IsAny<ITestHostLauncher>()), Times.Once);
    }

    [TestMethod]
    public void RunTestsWithSelectedTestsAndOptionsUsingACustomHostShouldPassOnOptions()
    {
        var options = new TestPlatformOptions() { TestCaseFilter = "PacMan" };

        _consoleWrapper.RunTestsWithCustomTestHost(
            _testCases,
            "RunSettings",
            options,
            new Mock<ITestRunEventsHandler>().Object,
            new Mock<ITestHostLauncher>().Object);

        _mockRequestSender.Verify(rs => rs.StartTestRunWithCustomHost(_testCases, "RunSettings", options, null, It.IsAny<ITestRunEventsHandler>(), It.IsAny<NoOpTelemetryEventsHandler>(), It.IsAny<ITestHostLauncher>()), Times.Once);
    }

    [TestMethod]
    public void RunTestsWithSelectedTestsAndACustomHostShouldSucceedWhenUsingSessions()
    {
        var testSessionInfo = new TestSessionInfo();
        var options = new TestPlatformOptions() { TestCaseFilter = "PacMan" };

        _consoleWrapper.RunTestsWithCustomTestHost(
            _testCases,
            "RunSettings",
            options,
            testSessionInfo,
            new Mock<ITestRunEventsHandler>().Object,
            new Mock<ITestHostLauncher>().Object);

        _mockRequestSender.Verify(
            rs => rs.StartTestRunWithCustomHost(
                _testCases,
                "RunSettings",
                options,
                testSessionInfo,
                It.IsAny<ITestRunEventsHandler>(),
                It.IsAny<NoOpTelemetryEventsHandler>(),
                It.IsAny<ITestHostLauncher>()),
            Times.Once);
    }

    [TestMethod]
    public void RunTestsWithSelectedTestsAndACustomHostShouldSucceedWhenUsingSessionsWithTelemetryHandler()
    {
        var testSessionInfo = new TestSessionInfo();
        var options = new TestPlatformOptions() { TestCaseFilter = "PacMan" };

        _consoleWrapper.RunTestsWithCustomTestHost(
            _testCases,
            "RunSettings",
            options,
            testSessionInfo,
            new Mock<ITestRunEventsHandler>().Object,
            _telemetryEventsHandler.Object,
            new Mock<ITestHostLauncher>().Object);

        _mockRequestSender.Verify(
            rs => rs.StartTestRunWithCustomHost(
                _testCases,
                "RunSettings",
                options,
                testSessionInfo,
                It.IsAny<ITestRunEventsHandler>(),
                _telemetryEventsHandler.Object,
                It.IsAny<ITestHostLauncher>()),
            Times.Once);
    }

    [TestMethod]
    public async Task ProcessTestRunAttachmentsAsyncShouldSucceed()
    {
        var attachments = new Collection<AttachmentSet>();
        var invokedDataCollectors = new Collection<InvokedDataCollector>();
        var cancellationToken = new CancellationToken();

        await _consoleWrapper.ProcessTestRunAttachmentsAsync(
            attachments,
            invokedDataCollectors,
            Constants.EmptyRunSettings,
            true,
            true,
            new Mock<ITestRunAttachmentsProcessingEventsHandler>().Object,
            cancellationToken);

        _mockRequestSender.Verify(rs => rs.ProcessTestRunAttachmentsAsync(attachments, invokedDataCollectors, Constants.EmptyRunSettings, true, It.IsAny<ITestRunAttachmentsProcessingEventsHandler>(), cancellationToken));
    }

    [TestMethod]
    public void EndSessionShouldSucceed()
    {
        _consoleWrapper.EndSession();

        _mockRequestSender.Verify(rs => rs.EndSession(), Times.Once);
        _mockRequestSender.Verify(rs => rs.Close(), Times.Once);
        _mockProcessManager.Verify(x => x.ShutdownProcess(), Times.Once);
    }
}
