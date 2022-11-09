// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
#if !NET5_0_OR_GREATER
using System.Diagnostics;
#endif
using System.Threading;
using System.Threading.Tasks;

using Microsoft.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.TestPlatform.VsTestConsole.TranslationLayer.UnitTests;

[TestClass]
public class VsTestConsoleWrapperAsyncTests
{
    private readonly IVsTestConsoleWrapper _consoleWrapper;
    private readonly Mock<IProcessManager> _mockProcessManager;
    private readonly Mock<ITranslationLayerRequestSender> _mockRequestSender;
    private readonly Mock<IProcessHelper> _mockProcessHelper;
    private readonly List<string> _testSources = new() { "Hello", "World" };
    private readonly List<TestCase> _testCases = new()
    {
        new TestCase("a.b.c", new Uri("d://uri"), "a.dll"),
        new TestCase("d.e.f", new Uri("g://uri"), "d.dll")
    };
    private readonly ConsoleParameters _consoleParameters;

    public VsTestConsoleWrapperAsyncTests()
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

        _mockRequestSender.Setup(rs => rs.WaitForRequestHandlerConnection(It.IsAny<int>())).Returns(true);
        _mockRequestSender.Setup(rs => rs.InitializeCommunication()).Returns(100);
        _mockRequestSender.Setup(rs => rs.InitializeCommunicationAsync(It.IsAny<int>())).Returns(Task.FromResult(100));
    }

    [TestMethod]
    public async Task StartSessionAsyncShouldStartVsTestConsoleWithCorrectArguments()
    {
        var inputPort = 123;
#if NET5_0_OR_GREATER
        var expectedParentProcessId = Environment.ProcessId;
#else
        int expectedParentProcessId;
        using (var p = Process.GetCurrentProcess())
            expectedParentProcessId = p.Id;
#endif
        _mockRequestSender.Setup(rs => rs.InitializeCommunicationAsync(It.IsAny<int>())).Returns(Task.FromResult(inputPort));

        await _consoleWrapper.StartSessionAsync();

        Assert.AreEqual(expectedParentProcessId, _consoleParameters.ParentProcessId, "Parent process Id must be set");
        Assert.AreEqual(inputPort, _consoleParameters.PortNumber, "Port number must be set");

        _mockProcessManager.Verify(pm => pm.StartProcess(_consoleParameters), Times.Once);
    }

    [TestMethod]
    public void StartSessionAsyncShouldThrowExceptionOnBadPort()
    {
        _mockRequestSender.Setup(rs => rs.InitializeCommunicationAsync(It.IsAny<int>())).Returns(Task.FromResult(-1));

        Assert.ThrowsExceptionAsync<TransationLayerException>(async () => await _consoleWrapper.StartSessionAsync());
    }

    [TestMethod]
    public async Task StartSessionShouldCallWhenProcessNotInitializedAsync()
    {
        _mockProcessManager.Setup(pm => pm.IsProcessInitialized()).Returns(false);

        // To call private method EnsureInitialize call InitializeExtensions
        await _consoleWrapper.InitializeExtensionsAsync(new[] { "path/to/adapter" });

        _mockProcessManager.Verify(pm => pm.StartProcess(It.IsAny<ConsoleParameters>()));
    }

    [TestMethod]
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public async Task StartTestSessionAsyncShouldCallRequestSenderWithCorrectArguments1()
    {
        var testSessionInfo = new TestSessionInfo();
        var mockEventsHandler = new Mock<ITestSessionEventsHandler>();

        _mockRequestSender.Setup(
                rs => rs.StartTestSessionAsync(
                    _testSources,
                    null,
                    null,
                    mockEventsHandler.Object,
                    null))
            .Returns(Task.FromResult<TestSessionInfo?>(testSessionInfo));

        Assert.AreEqual(
            (await _consoleWrapper.StartTestSessionAsync(
                _testSources,
                null,
                mockEventsHandler.Object).ConfigureAwait(false))?.TestSessionInfo,
            testSessionInfo);

        _mockRequestSender.Verify(
            rs => rs.StartTestSessionAsync(
                _testSources,
                null,
                null,
                mockEventsHandler.Object,
                null),
            Times.Once);
    }

    [TestMethod]
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public async Task StartTestSessionAsyncShouldCallRequestSenderWithCorrectArguments2()
    {
        var testSessionInfo = new TestSessionInfo();
        var testPlatformOptions = new TestPlatformOptions();
        var mockEventsHandler = new Mock<ITestSessionEventsHandler>();

        _mockRequestSender.Setup(
                rs => rs.StartTestSessionAsync(
                    _testSources,
                    null,
                    testPlatformOptions,
                    mockEventsHandler.Object,
                    null))
            .Returns(Task.FromResult<TestSessionInfo?>(testSessionInfo));

        Assert.AreEqual(
            (await _consoleWrapper.StartTestSessionAsync(
                _testSources,
                null,
                testPlatformOptions,
                mockEventsHandler.Object).ConfigureAwait(false))?.TestSessionInfo,
            testSessionInfo);

        _mockRequestSender.Verify(
            rs => rs.StartTestSessionAsync(
                _testSources,
                null,
                testPlatformOptions,
                mockEventsHandler.Object,
                null),
            Times.Once);
    }

    [TestMethod]
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public async Task StartTestSessionAsyncShouldCallRequestSenderWithCorrectArguments3()
    {
        var testSessionInfo = new TestSessionInfo();
        var testPlatformOptions = new TestPlatformOptions();
        var mockEventsHandler = new Mock<ITestSessionEventsHandler>();
        var mockTesthostLauncher = new Mock<ITestHostLauncher>();

        _mockRequestSender.Setup(
                rs => rs.StartTestSessionAsync(
                    _testSources,
                    null,
                    testPlatformOptions,
                    mockEventsHandler.Object,
                    mockTesthostLauncher.Object))
            .Returns(Task.FromResult<TestSessionInfo?>(testSessionInfo));

        Assert.AreEqual(
            (await _consoleWrapper.StartTestSessionAsync(
                _testSources,
                null,
                testPlatformOptions,
                mockEventsHandler.Object,
                mockTesthostLauncher.Object).ConfigureAwait(false))?.TestSessionInfo,
            testSessionInfo);

        _mockRequestSender.Verify(
            rs => rs.StartTestSessionAsync(
                _testSources,
                null,
                testPlatformOptions,
                mockEventsHandler.Object,
                mockTesthostLauncher.Object),
            Times.Once);
    }

    [TestMethod]
    [Obsolete("This API is not final yet and is subject to changes.", false)]
    public async Task StopTestSessionAsyncShouldCallRequestSenderWithCorrectArguments()
    {
        var testPlatformOptions = new TestPlatformOptions();
        var testSessionInfo = new TestSessionInfo();
        var mockEventsHandler = new Mock<ITestSessionEventsHandler>();

        _mockRequestSender.Setup(
                rs => rs.StopTestSessionAsync(
                    It.IsAny<TestSessionInfo>(),
                    It.IsAny<TestPlatformOptions>(),
                    It.IsAny<ITestSessionEventsHandler>()))
            .Returns(Task.FromResult(true));

        Assert.IsTrue(
            await _consoleWrapper.StopTestSessionAsync(
                testSessionInfo,
                testPlatformOptions,
                mockEventsHandler.Object).ConfigureAwait(false));

        _mockRequestSender.Verify(
            rs => rs.StopTestSessionAsync(
                testSessionInfo,
                testPlatformOptions,
                mockEventsHandler.Object),
            Times.Once);
    }

    [TestMethod]
    public async Task InitializeExtensionsAsyncShouldCachePathToExtensions()
    {
        var pathToExtensions = new[] { "path/to/adapter" };
        _mockProcessManager.Setup(pm => pm.IsProcessInitialized()).Returns(true);

        await _consoleWrapper.InitializeExtensionsAsync(pathToExtensions);

        _mockProcessManager.Setup(pm => pm.IsProcessInitialized()).Returns(false);
        _mockRequestSender.Setup(rs => rs.InitializeCommunicationAsync(It.IsAny<int>())).Returns(Task.FromResult(100));

        await _consoleWrapper.InitializeExtensionsAsync(pathToExtensions);

        _mockRequestSender.Verify(rs => rs.InitializeExtensions(pathToExtensions), Times.Exactly(3));
    }

    [TestMethod]
    public void ProcessExitedEventShouldSetOnProcessExit()
    {
        _mockProcessManager.Raise(pm => pm.ProcessExited += null, EventArgs.Empty);

        _mockRequestSender.Verify(rs => rs.OnProcessExited(), Times.Once);
    }

    [TestMethod]
    public async Task InitializeExtensionsAsyncShouldSucceed()
    {
        var pathToAdditionalExtensions = new List<string> { "Hello", "World" };

        await _consoleWrapper.InitializeExtensionsAsync(pathToAdditionalExtensions);

        _mockRequestSender.Verify(rs => rs.InitializeExtensions(pathToAdditionalExtensions), Times.Once);
    }

    [TestMethod]
    public void InitializeExtensionsAsyncShouldThrowExceptionOnBadConnection()
    {
        _mockRequestSender.Setup(rs => rs.InitializeCommunicationAsync(It.IsAny<int>())).Returns(Task.FromResult(-1));

        Assert.ThrowsExceptionAsync<TransationLayerException>(async () => await _consoleWrapper.InitializeExtensionsAsync(new List<string> { "Hello", "World" }));
        _mockRequestSender.Verify(rs => rs.InitializeExtensions(It.IsAny<IEnumerable<string>>()), Times.Never);
    }

    [TestMethod]
    public async Task DiscoverTestsAsyncShouldSucceed()
    {
        await _consoleWrapper.DiscoverTestsAsync(_testSources, null, new TestPlatformOptions(), new Mock<ITestDiscoveryEventsHandler2>().Object);

        _mockRequestSender.Verify(rs => rs.DiscoverTestsAsync(_testSources, null, It.IsAny<TestPlatformOptions>(), null, It.IsAny<ITestDiscoveryEventsHandler2>()), Times.Once);
    }

    [TestMethod]
    public async Task DiscoverTestsAsyncShouldPassTestDiscoveryHandler2IfTestDiscoveryHandler1IsInput()
    {
        await _consoleWrapper.DiscoverTestsAsync(_testSources, null, new TestPlatformOptions(), new Mock<ITestDiscoveryEventsHandler2>().Object);

        _mockRequestSender.Verify(rs => rs.DiscoverTestsAsync(_testSources, null, It.IsAny<TestPlatformOptions>(), null, It.IsAny<ITestDiscoveryEventsHandler2>()), Times.Once);
    }

    [TestMethod]
    public async Task DiscoverTestsAndNullOptionsAsyncShouldSucceedOnNullOptions()
    {
        await _consoleWrapper.DiscoverTestsAsync(_testSources, null, null, new Mock<ITestDiscoveryEventsHandler2>().Object);

        _mockRequestSender.Verify(rs => rs.DiscoverTestsAsync(_testSources, null, null, null, It.IsAny<ITestDiscoveryEventsHandler2>()), Times.Once);
    }

    [TestMethod]
    public async Task DiscoverTestsAndOptionsAsyncShouldSucceedOnOptions()
    {
        var options = new TestPlatformOptions();
        await _consoleWrapper.DiscoverTestsAsync(_testSources, null, options, new Mock<ITestDiscoveryEventsHandler2>().Object);

        _mockRequestSender.Verify(rs => rs.DiscoverTestsAsync(_testSources, null, options, null, It.IsAny<ITestDiscoveryEventsHandler2>()), Times.Once);
    }

    [TestMethod]
    public void DiscoverTestsAsyncShouldThrowExceptionOnBadConnection()
    {
        _mockRequestSender.Setup(rs => rs.InitializeCommunicationAsync(It.IsAny<int>())).Returns(Task.FromResult(-1));

        Assert.ThrowsExceptionAsync<TransationLayerException>(async () => await _consoleWrapper.DiscoverTestsAsync(new List<string> { "Hello", "World" }, null, new TestPlatformOptions(), new Mock<ITestDiscoveryEventsHandler2>().Object));
        _mockRequestSender.Verify(rs => rs.DiscoverTestsAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), It.IsAny<TestPlatformOptions>(), null, It.IsAny<ITestDiscoveryEventsHandler2>()), Times.Never);
    }

    [TestMethod]
    public async Task DiscoverTestsShouldSucceedWhenUsingSessions()
    {
        var testSessionInfo = new TestSessionInfo();

        await _consoleWrapper.DiscoverTestsAsync(
                _testSources,
                null,
                null,
                testSessionInfo,
                new Mock<ITestDiscoveryEventsHandler2>().Object)
            .ConfigureAwait(false);

        _mockRequestSender.Verify(
            rs => rs.DiscoverTestsAsync(
                _testSources,
                null,
                null,
                testSessionInfo,
                It.IsAny<ITestDiscoveryEventsHandler2>()),
            Times.Once);
    }

    [TestMethod]
    public async Task RunTestsAsyncWithSourcesShouldSucceed()
    {
        await _consoleWrapper.RunTestsAsync(_testSources, "RunSettings", new Mock<ITestRunEventsHandler>().Object);

        _mockRequestSender.Verify(rs => rs.StartTestRunAsync(_testSources, "RunSettings", null, null, It.IsAny<ITestRunEventsHandler>()), Times.Once);
    }

    [TestMethod]
    public async Task RunTestsAsyncWithSourcesAndNullOptionsShouldSucceedOnNullOptions()
    {
        await _consoleWrapper.RunTestsAsync(_testSources, "RunSettings", null, new Mock<ITestRunEventsHandler>().Object);

        _mockRequestSender.Verify(rs => rs.StartTestRunAsync(_testSources, "RunSettings", null, null, It.IsAny<ITestRunEventsHandler>()), Times.Once);
    }

    [TestMethod]
    public async Task RunTestsAsyncWithSourcesAndOptionsShouldSucceedOnOptions()
    {
        var options = new TestPlatformOptions();
        await _consoleWrapper.RunTestsAsync(_testSources, "RunSettings", options, new Mock<ITestRunEventsHandler>().Object);

        _mockRequestSender.Verify(rs => rs.StartTestRunAsync(_testSources, "RunSettings", options, null, It.IsAny<ITestRunEventsHandler>()), Times.Once);
    }

    [TestMethod]
    public async Task RunTestsAsyncWithSourcesShouldSucceedWhenUsingSessions()
    {
        var testSessionInfo = new TestSessionInfo();
        var options = new TestPlatformOptions() { TestCaseFilter = "PacMan" };
        await _consoleWrapper.RunTestsAsync(
                _testSources,
                "RunSettings",
                options,
                testSessionInfo,
                new Mock<ITestRunEventsHandler>().Object)
            .ConfigureAwait(false);

        _mockRequestSender.Verify(
            rs => rs.StartTestRunAsync(
                _testSources,
                "RunSettings",
                options,
                testSessionInfo,
                It.IsAny<ITestRunEventsHandler>()),
            Times.Once);
    }

    [TestMethod]
    public async Task RunTestsAsyncWithSourcesAndCustomHostShouldSucceed()
    {
        await _consoleWrapper.RunTestsWithCustomTestHostAsync(
            _testSources,
            "RunSettings",
            new Mock<ITestRunEventsHandler>().Object,
            new Mock<ITestHostLauncher>().Object);

        _mockRequestSender.Verify(rs => rs.StartTestRunWithCustomHostAsync(_testSources, "RunSettings", null, null, It.IsAny<ITestRunEventsHandler>(), It.IsAny<ITestHostLauncher>()), Times.Once);
    }

    [TestMethod]
    public async Task RunTestsAsyncWithSourcesAndOptionsUsingCustomHostShouldSucceedOnNullOptions()
    {
        await _consoleWrapper.RunTestsWithCustomTestHostAsync(
            _testSources,
            "RunSettings",
            null,
            new Mock<ITestRunEventsHandler>().Object,
            new Mock<ITestHostLauncher>().Object);

        _mockRequestSender.Verify(rs => rs.StartTestRunWithCustomHostAsync(_testSources, "RunSettings", null, null, It.IsAny<ITestRunEventsHandler>(), It.IsAny<ITestHostLauncher>()), Times.Once);
    }

    [TestMethod]
    public async Task RunTestsAsyncWithSourcesOnOptionsUsingCustomHostShouldSucceedOnOptions()
    {
        var options = new TestPlatformOptions();

        await _consoleWrapper.RunTestsWithCustomTestHostAsync(
            _testSources,
            "RunSettings",
            options,
            new Mock<ITestRunEventsHandler>().Object,
            new Mock<ITestHostLauncher>().Object);

        _mockRequestSender.Verify(rs => rs.StartTestRunWithCustomHostAsync(_testSources, "RunSettings", options, null, It.IsAny<ITestRunEventsHandler>(), It.IsAny<ITestHostLauncher>()), Times.Once);
    }

    [TestMethod]
    public async Task RunTestsAsyncWithSourcesAndACustomHostShouldSucceedWhenUsingSessions()
    {
        var testSessionInfo = new TestSessionInfo();
        var options = new TestPlatformOptions() { TestCaseFilter = "PacMan" };
        await _consoleWrapper.RunTestsWithCustomTestHostAsync(
                _testSources,
                "RunSettings",
                options,
                testSessionInfo,
                new Mock<ITestRunEventsHandler>().Object,
                new Mock<ITestHostLauncher>().Object)
            .ConfigureAwait(false);

        _mockRequestSender.Verify(
            rs => rs.StartTestRunWithCustomHostAsync(
                _testSources,
                "RunSettings",
                options,
                testSessionInfo,
                It.IsAny<ITestRunEventsHandler>(),
                It.IsAny<ITestHostLauncher>()),
            Times.Once);
    }

    [TestMethod]
    public async Task RunTestsAsyncWithSelectedTestsShouldSucceed()
    {
        await _consoleWrapper.RunTestsAsync(_testCases, "RunSettings", new Mock<ITestRunEventsHandler>().Object);

        _mockRequestSender.Verify(rs => rs.StartTestRunAsync(_testCases, "RunSettings", It.IsAny<TestPlatformOptions>(), null, It.IsAny<ITestRunEventsHandler>()), Times.Once);
    }

    [TestMethod]
    public async Task RunTestsAsyncWithSelectedTestsAndOptionsShouldSucceedOnNullOptions()
    {
        await _consoleWrapper.RunTestsAsync(_testCases, "RunSettings", null, new Mock<ITestRunEventsHandler>().Object);

        _mockRequestSender.Verify(rs => rs.StartTestRunAsync(_testCases, "RunSettings", null, null, It.IsAny<ITestRunEventsHandler>()), Times.Once);
    }

    [TestMethod]
    public async Task RunTestsAsyncWithSelectedTestsAndOptionsShouldSucceedOnOptions()
    {
        var options = new TestPlatformOptions();

        await _consoleWrapper.RunTestsAsync(_testCases, "RunSettings", options, new Mock<ITestRunEventsHandler>().Object);

        _mockRequestSender.Verify(rs => rs.StartTestRunAsync(_testCases, "RunSettings", options, null, It.IsAny<ITestRunEventsHandler>()), Times.Once);
    }

    [TestMethod]
    public async Task RunTestsAsyncWithSelectedTestsShouldSucceedWhenUsingSessions()
    {
        var testSessionInfo = new TestSessionInfo();
        var options = new TestPlatformOptions() { TestCaseFilter = "PacMan" };

        await _consoleWrapper.RunTestsAsync(
                _testCases,
                "RunSettings",
                options,
                testSessionInfo,
                new Mock<ITestRunEventsHandler>().Object)
            .ConfigureAwait(false);

        _mockRequestSender.Verify(
            rs => rs.StartTestRunAsync(
                _testCases,
                "RunSettings",
                options,
                testSessionInfo,
                It.IsAny<ITestRunEventsHandler>()),
            Times.Once);
    }

    [TestMethod]
    public async Task RunTestsAsyncWithSelectedTestsAndCustomLauncherShouldSucceed()
    {
        await _consoleWrapper.RunTestsWithCustomTestHostAsync(
            _testCases,
            "RunSettings",
            new Mock<ITestRunEventsHandler>().Object,
            new Mock<ITestHostLauncher>().Object);

        _mockRequestSender.Verify(rs => rs.StartTestRunWithCustomHostAsync(_testCases, "RunSettings", It.IsAny<TestPlatformOptions>(), null, It.IsAny<ITestRunEventsHandler>(), It.IsAny<ITestHostLauncher>()), Times.Once);
    }

    [TestMethod]
    public async Task RunTestsAsyncWithSelectedTestsAndOptionsUsingCustomLauncherShouldSucceedOnNullOptions()
    {
        await _consoleWrapper.RunTestsWithCustomTestHostAsync(
            _testCases,
            "RunSettings",
            null,
            new Mock<ITestRunEventsHandler>().Object,
            new Mock<ITestHostLauncher>().Object);

        _mockRequestSender.Verify(rs => rs.StartTestRunWithCustomHostAsync(_testCases, "RunSettings", null, null, It.IsAny<ITestRunEventsHandler>(), It.IsAny<ITestHostLauncher>()), Times.Once);
    }

    [TestMethod]
    public async Task RunTestsAsyncWithSelectedTestsAndOptionsUsingCustomLauncherShouldSucceedOnOptions()
    {
        var options = new TestPlatformOptions();
        await _consoleWrapper.RunTestsWithCustomTestHostAsync(
            _testCases,
            "RunSettings",
            options,
            new Mock<ITestRunEventsHandler>().Object,
            new Mock<ITestHostLauncher>().Object);

        _mockRequestSender.Verify(rs => rs.StartTestRunWithCustomHostAsync(_testCases, "RunSettings", options, null, It.IsAny<ITestRunEventsHandler>(), It.IsAny<ITestHostLauncher>()), Times.Once);
    }

    [TestMethod]
    public async Task RunTestsAsyncWithSelectedTestsAndACustomHostShouldSucceedWhenUsingSessions()
    {
        var testSessionInfo = new TestSessionInfo();
        var options = new TestPlatformOptions() { TestCaseFilter = "PacMan" };

        await _consoleWrapper.RunTestsWithCustomTestHostAsync(
                _testCases,
                "RunSettings",
                options,
                testSessionInfo,
                new Mock<ITestRunEventsHandler>().Object,
                new Mock<ITestHostLauncher>().Object)
            .ConfigureAwait(false);

        _mockRequestSender.Verify(
            rs => rs.StartTestRunWithCustomHostAsync(
                _testCases,
                "RunSettings",
                options,
                testSessionInfo,
                It.IsAny<ITestRunEventsHandler>(),
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
    }
}
