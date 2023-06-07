// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Versioning;
using System.Threading.Tasks;

using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.VisualStudio.TestPlatform.Client;
using Microsoft.VisualStudio.TestPlatform.Client.RequestHelper;
using Microsoft.VisualStudio.TestPlatform.CommandLine.Internal;
using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestPlatform.CommandLine.Publisher;
using Microsoft.VisualStudio.TestPlatform.CommandLine.TestPlatformHelpers;
using Microsoft.VisualStudio.TestPlatform.CommandLineUtilities;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using vstest.console.Internal;
using vstest.console.UnitTests.Processors;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.Processors;

/// <summary>
/// Tests for RunTestsArgumentProcessor
/// </summary>
[TestClass]
public class RunTestsArgumentProcessorTests
{
    private readonly Mock<IFileHelper> _mockFileHelper;
    private readonly Mock<IOutput> _mockOutput;
    private readonly Mock<IAssemblyMetadataProvider> _mockAssemblyMetadataProvider;
    private readonly InferHelper _inferHelper;
    private readonly string _dummyTestFilePath = "DummyTest.dll";
    private readonly Mock<ITestPlatformEventSource> _mockTestPlatformEventSource;
    private readonly Task<IMetricsPublisher> _mockMetricsPublisherTask;
    private readonly Mock<IMetricsPublisher> _mockMetricsPublisher;
    private readonly Mock<IProcessHelper> _mockProcessHelper;
    private readonly Mock<ITestRunAttachmentsProcessingManager> _mockAttachmentsProcessingManager;
    private readonly Mock<IArtifactProcessingManager> _artifactProcessingManager;
    private readonly Mock<IEnvironment> _environment;
    private readonly Mock<IEnvironmentVariableHelper> _environmentVariableHelper;

    public RunTestsArgumentProcessorTests()
    {
        _mockFileHelper = new Mock<IFileHelper>();
        _mockOutput = new Mock<IOutput>();
        _mockFileHelper.Setup(fh => fh.Exists(_dummyTestFilePath)).Returns(true);
        _mockFileHelper.Setup(x => x.GetCurrentDirectory()).Returns("");
        _mockMetricsPublisher = new Mock<IMetricsPublisher>();
        _mockMetricsPublisherTask = Task.FromResult(_mockMetricsPublisher.Object);
        _mockTestPlatformEventSource = new Mock<ITestPlatformEventSource>();
        _mockAssemblyMetadataProvider = new Mock<IAssemblyMetadataProvider>();
        _artifactProcessingManager = new Mock<IArtifactProcessingManager>();
        _inferHelper = new InferHelper(_mockAssemblyMetadataProvider.Object);
        SetupMockExtensions();
        _mockAssemblyMetadataProvider.Setup(a => a.GetArchitecture(It.IsAny<string>()))
            .Returns(Architecture.X86);
        _mockAssemblyMetadataProvider.Setup(x => x.GetFrameworkName(It.IsAny<string>())).Returns(new FrameworkName(Constants.DotNetFramework40));
        _mockProcessHelper = new Mock<IProcessHelper>();
        _mockAttachmentsProcessingManager = new Mock<ITestRunAttachmentsProcessingManager>();
        _environment = new Mock<IEnvironment>();
        _environmentVariableHelper = new Mock<IEnvironmentVariableHelper>();
    }

    [TestMethod]
    public void GetMetadataShouldReturnRunTestsArgumentProcessorCapabilities()
    {
        RunTestsArgumentProcessor processor = new();
        Assert.IsTrue(processor.Metadata.Value is RunTestsArgumentProcessorCapabilities);
    }

    [TestMethod]
    public void GetExecuterShouldReturnRunTestsArgumentProcessorCapabilities()
    {
        RunTestsArgumentProcessor processor = new();
        Assert.IsTrue(processor.Executor!.Value is RunTestsArgumentExecutor);
    }

    #region RunTestsArgumentProcessorCapabilitiesTests

    [TestMethod]
    public void CapabilitiesShouldReturnAppropriateProperties()
    {
        RunTestsArgumentProcessorCapabilities capabilities = new();
        Assert.AreEqual("/RunTests", capabilities.CommandName);
        var expected = "[TestFileNames]\r\n      Run tests from the specified files or wild card pattern. Separate multiple test file names or pattern\r\n      by spaces. Set console logger verbosity to detailed to view matched test files.\r\n      Examples: mytestproject.dll\r\n                mytestproject.dll myothertestproject.exe\r\n                testproject*.dll my*project.dll";
        Assert.AreEqual(expected.NormalizeLineEndings().ShowWhiteSpace(), capabilities.HelpContentResourceName.NormalizeLineEndings().ShowWhiteSpace());

        Assert.AreEqual(HelpContentPriority.RunTestsArgumentProcessorHelpPriority, capabilities.HelpPriority);
        Assert.IsTrue(capabilities.IsAction);
        Assert.AreEqual(ArgumentProcessorPriority.Normal, capabilities.Priority);

        Assert.IsFalse(capabilities.AllowMultiple);
        Assert.IsFalse(capabilities.AlwaysExecute);
        Assert.IsTrue(capabilities.IsSpecialCommand);
    }
    #endregion

    #region RunTestsArgumentExecutorTests

    [TestMethod]
    public void ExecutorExecuteShouldReturnSuccessWithoutExecutionInDesignMode()
    {
        var runSettingsProvider = new TestableRunSettingsProvider();
        runSettingsProvider.UpdateRunSettings("<RunSettings/>");

        CommandLineOptions.Reset();
        CommandLineOptions.Instance.IsDesignMode = true;
        var testRequestManager = new TestRequestManager(CommandLineOptions.Instance, TestPlatformFactory.GetTestPlatform(), TestRunResultAggregator.Instance, _mockTestPlatformEventSource.Object, _inferHelper, _mockMetricsPublisherTask, _mockProcessHelper.Object, _mockAttachmentsProcessingManager.Object, _environment.Object, _environmentVariableHelper.Object);
        var executor = new RunTestsArgumentExecutor(CommandLineOptions.Instance, runSettingsProvider, testRequestManager, _artifactProcessingManager.Object, _mockOutput.Object);

        Assert.AreEqual(ArgumentProcessorResult.Success, executor.Execute());
    }

    [TestMethod]
    public void ExecutorExecuteForNoSourcesShouldThrowCommandLineException()
    {
        CommandLineOptions.Reset();
        var testRequestManager = new TestRequestManager(CommandLineOptions.Instance, TestPlatformFactory.GetTestPlatform(), TestRunResultAggregator.Instance, _mockTestPlatformEventSource.Object, _inferHelper, _mockMetricsPublisherTask, _mockProcessHelper.Object, _mockAttachmentsProcessingManager.Object, _environment.Object, _environmentVariableHelper.Object);
        var executor = GetExecutor(testRequestManager);

        Assert.ThrowsException<CommandLineException>(() => executor.Execute());
    }

    private RunTestsArgumentExecutor GetExecutor(ITestRequestManager testRequestManager)
    {
        var runSettingsProvider = new TestableRunSettingsProvider();
        runSettingsProvider.AddDefaultRunSettings();
        var executor = new RunTestsArgumentExecutor(
            CommandLineOptions.Instance,
            runSettingsProvider,
            testRequestManager,
            _artifactProcessingManager.Object,
            _mockOutput.Object
        );
        return executor;
    }

    [TestMethod]
    public void ExecutorExecuteShouldThrowTestPlatformException()
    {
        var mockTestPlatform = new Mock<ITestPlatform>();
        var mockTestRunRequest = new Mock<ITestRunRequest>();

        mockTestRunRequest.Setup(tr => tr.ExecuteAsync()).Throws(new TestPlatformException("DummyTestPlatformException"));
        mockTestPlatform.Setup(tp => tp.CreateTestRunRequest(It.IsAny<IRequestData>(), It.IsAny<TestRunCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Returns(mockTestRunRequest.Object);

        ResetAndAddSourceToCommandLineOptions();
        var testRequestManager = new TestRequestManager(CommandLineOptions.Instance, mockTestPlatform.Object, TestRunResultAggregator.Instance, _mockTestPlatformEventSource.Object, _inferHelper, _mockMetricsPublisherTask, _mockProcessHelper.Object, _mockAttachmentsProcessingManager.Object, _environment.Object, _environmentVariableHelper.Object);
        var executor = GetExecutor(testRequestManager);

        Assert.ThrowsException<TestPlatformException>(() => executor.Execute());
    }

    [TestMethod]
    public void ExecutorExecuteShouldThrowSettingsException()
    {
        var mockTestPlatform = new Mock<ITestPlatform>();
        var mockTestRunRequest = new Mock<ITestRunRequest>();

        mockTestRunRequest.Setup(tr => tr.ExecuteAsync()).Throws(new SettingsException("DummySettingsException"));
        mockTestPlatform.Setup(tp => tp.CreateTestRunRequest(It.IsAny<IRequestData>(), It.IsAny<TestRunCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Returns(mockTestRunRequest.Object);

        ResetAndAddSourceToCommandLineOptions();
        var testRequestManager = new TestRequestManager(CommandLineOptions.Instance, mockTestPlatform.Object, TestRunResultAggregator.Instance, _mockTestPlatformEventSource.Object, _inferHelper, _mockMetricsPublisherTask, _mockProcessHelper.Object, _mockAttachmentsProcessingManager.Object, _environment.Object, _environmentVariableHelper.Object);
        var executor = GetExecutor(testRequestManager);

        Assert.ThrowsException<SettingsException>(() => executor.Execute());
    }

    [TestMethod]
    public void ExecutorExecuteShouldThrowInvalidOperationException()
    {
        var mockTestPlatform = new Mock<ITestPlatform>();
        var mockTestRunRequest = new Mock<ITestRunRequest>();

        mockTestRunRequest.Setup(tr => tr.ExecuteAsync()).Throws(new InvalidOperationException("DummyInvalidOperationException"));
        mockTestPlatform.Setup(tp => tp.CreateTestRunRequest(It.IsAny<IRequestData>(), It.IsAny<TestRunCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Returns(mockTestRunRequest.Object);

        ResetAndAddSourceToCommandLineOptions();
        var testRequestManager = new TestRequestManager(CommandLineOptions.Instance, mockTestPlatform.Object, TestRunResultAggregator.Instance, _mockTestPlatformEventSource.Object, _inferHelper, _mockMetricsPublisherTask, _mockProcessHelper.Object, _mockAttachmentsProcessingManager.Object, _environment.Object, _environmentVariableHelper.Object);
        var executor = GetExecutor(testRequestManager);

        Assert.ThrowsException<InvalidOperationException>(() => executor.Execute());
    }

    [TestMethod]
    public void ExecutorExecuteShouldThrowOtherExceptions()
    {
        var mockTestPlatform = new Mock<ITestPlatform>();
        var mockTestRunRequest = new Mock<ITestRunRequest>();

        mockTestRunRequest.Setup(tr => tr.ExecuteAsync()).Throws(new Exception("DummyException"));
        mockTestPlatform.Setup(tp => tp.CreateTestRunRequest(It.IsAny<IRequestData>(), It.IsAny<TestRunCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Returns(mockTestRunRequest.Object);

        ResetAndAddSourceToCommandLineOptions();
        var testRequestManager = new TestRequestManager(CommandLineOptions.Instance, mockTestPlatform.Object, TestRunResultAggregator.Instance, _mockTestPlatformEventSource.Object, _inferHelper, _mockMetricsPublisherTask, _mockProcessHelper.Object, _mockAttachmentsProcessingManager.Object, _environment.Object, _environmentVariableHelper.Object);
        var executor = GetExecutor(testRequestManager);

        Assert.ThrowsException<Exception>(() => executor.Execute());
    }

    [TestMethod]
    public void ExecutorExecuteShouldForListOfTestsReturnSuccess()
    {
        var mockTestRunRequest = new Mock<ITestRunRequest>();

        var result = RunRunArgumentProcessorExecuteWithMockSetup(mockTestRunRequest.Object);

        mockTestRunRequest.Verify(tr => tr.ExecuteAsync(), Times.Once);
        Assert.AreEqual(ArgumentProcessorResult.Success, result);
    }

    [TestMethod]
    public void TestRunRequestManagerShouldInstrumentExecutionRequestStart()
    {
        var mockTestRunRequest = new Mock<ITestRunRequest>();

        var result = RunRunArgumentProcessorExecuteWithMockSetup(mockTestRunRequest.Object);

        _mockTestPlatformEventSource.Verify(x => x.ExecutionRequestStart(), Times.Once);
    }

    [TestMethod]
    public void TestRunRequestManagerShouldInstrumentExecutionRequestStop()
    {
        var mockTestRunRequest = new Mock<ITestRunRequest>();

        var result = RunRunArgumentProcessorExecuteWithMockSetup(mockTestRunRequest.Object);

        _mockTestPlatformEventSource.Verify(x => x.ExecutionRequestStop(), Times.Once);
    }

    #endregion

    private ArgumentProcessorResult RunRunArgumentProcessorExecuteWithMockSetup(ITestRunRequest testRunRequest)
    {
        var mockTestPlatform = new Mock<ITestPlatform>();
        var mockConsoleOutput = new Mock<IOutput>();

        List<TestCase> list = new()
        {
            new TestCase("Test1", new Uri("http://FooTestUri1"), "Source1"),
            new TestCase("Test2", new Uri("http://FooTestUri2"), "Source2")
        };
        var mockTestRunStats = new Mock<ITestRunStatistics>();

        var args = new TestRunCompleteEventArgs(mockTestRunStats.Object, false, false, null, null, null, new TimeSpan());

        mockTestPlatform.Setup(tp => tp.CreateTestRunRequest(It.IsAny<IRequestData>(), It.IsAny<TestRunCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Returns(testRunRequest);

        ResetAndAddSourceToCommandLineOptions();

        var testRequestManager = new TestRequestManager(CommandLineOptions.Instance, mockTestPlatform.Object, TestRunResultAggregator.Instance, _mockTestPlatformEventSource.Object, _inferHelper, _mockMetricsPublisherTask, _mockProcessHelper.Object, _mockAttachmentsProcessingManager.Object, _environment.Object, _environmentVariableHelper.Object);
        var executor = GetExecutor(testRequestManager);

        return executor.Execute();
    }

    private void ResetAndAddSourceToCommandLineOptions()
    {
        CommandLineOptions.Reset();

        CommandLineOptions.Instance.FileHelper = _mockFileHelper.Object;
        CommandLineOptions.Instance.FilePatternParser = new FilePatternParser(new Mock<Matcher>().Object, _mockFileHelper.Object);
        CommandLineOptions.Instance.AddSource(_dummyTestFilePath);
    }

    public static void SetupMockExtensions()
    {
        // Setup mocks.
        var mockFileHelper = new Mock<IFileHelper>();
        mockFileHelper.Setup(fh => fh.DirectoryExists(It.IsAny<string>())).Returns(true);
        mockFileHelper.Setup(fh => fh.EnumerateFiles(It.IsAny<string>(), SearchOption.TopDirectoryOnly, new[] { ".dll" }))
            .Callback(() => { })
            .Returns(new string[] { typeof(RunTestsArgumentProcessorTests).Assembly.Location, typeof(ConsoleLogger).Assembly.Location });

        var testableTestPluginCache = new TestableTestPluginCache();

        // Setup the testable instance.
        TestPluginCache.Instance = testableTestPluginCache;
    }

    [ExtensionUri("testlogger://logger")]
    [FriendlyName("TestLoggerExtension")]
    private class ValidLogger3 : ITestLogger
    {
        public void Initialize(TestLoggerEvents events, string testRunDirectory)
        {
            events.TestRunMessage += TestMessageHandler;
            events.TestRunComplete += Events_TestRunComplete;
            events.TestResult += Events_TestResult;
        }

        private void Events_TestResult(object? sender, TestResultEventArgs e)
        {
        }

        private void Events_TestRunComplete(object? sender, TestRunCompleteEventArgs e)
        {

        }

        private void TestMessageHandler(object? sender, TestRunMessageEventArgs e)
        {
        }
    }
}

#region Testable implementation

public class TestableTestPluginCache : TestPluginCache
{
}

#endregion
