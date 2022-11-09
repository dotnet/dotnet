// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;

using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.VisualStudio.TestPlatform.Client;
using Microsoft.VisualStudio.TestPlatform.Client.RequestHelper;
using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestPlatform.CommandLine.Publisher;
using Microsoft.VisualStudio.TestPlatform.CommandLine.TestPlatformHelpers;
using Microsoft.VisualStudio.TestPlatform.CommandLineUtilities;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using vstest.console.Internal;
using vstest.console.UnitTests.Processors;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.Processors;

// <summary>
// Tests for ListTestsArgumentProcessor
// </summary>
[TestClass]
public class ListTestsArgumentProcessorTests
{
    private readonly Mock<IFileHelper> _mockFileHelper;
    private readonly Mock<IAssemblyMetadataProvider> _mockAssemblyMetadataProvider;
    private readonly InferHelper _inferHelper;
    private readonly string _dummyTestFilePath = "DummyTest.dll";
    private readonly Mock<ITestPlatformEventSource> _mockTestPlatformEventSource;
    private readonly Task<IMetricsPublisher> _mockMetricsPublisherTask;
    private readonly Mock<IMetricsPublisher> _mockMetricsPublisher;
    private readonly Mock<IProcessHelper> _mockProcessHelper;
    private readonly Mock<ITestRunAttachmentsProcessingManager> _mockAttachmentsProcessingManager;
    private readonly Mock<IEnvironment> _mockEnvironment;
    private readonly Mock<IEnvironmentVariableHelper> _mockEnvironmentVariableHelper;

    private static ListTestsArgumentExecutor GetExecutor(ITestRequestManager testRequestManager, IOutput? output)
    {
        var runSettingsProvider = new TestableRunSettingsProvider();

        runSettingsProvider.AddDefaultRunSettings();
        var listTestsArgumentExecutor =
            new ListTestsArgumentExecutor(
                CommandLineOptions.Instance,
                runSettingsProvider,
                testRequestManager,
                output ?? ConsoleOutput.Instance);
        return listTestsArgumentExecutor;
    }

    [TestCleanup]
    public void Cleanup()
    {
        CommandLineOptions.Reset();
    }

    public ListTestsArgumentProcessorTests()
    {
        _mockFileHelper = new Mock<IFileHelper>();
        _mockFileHelper.Setup(fh => fh.Exists(_dummyTestFilePath)).Returns(true);
        _mockFileHelper.Setup(x => x.GetCurrentDirectory()).Returns("");
        _mockMetricsPublisher = new Mock<IMetricsPublisher>();
        _mockMetricsPublisherTask = Task.FromResult(_mockMetricsPublisher.Object);
        _mockTestPlatformEventSource = new Mock<ITestPlatformEventSource>();
        _mockAssemblyMetadataProvider = new Mock<IAssemblyMetadataProvider>();
        _mockAssemblyMetadataProvider.Setup(x => x.GetArchitecture(It.IsAny<string>())).Returns(Architecture.X64);
        _mockAssemblyMetadataProvider.Setup(x => x.GetFrameworkName(It.IsAny<string>())).Returns(new FrameworkName(Constants.DotNetFramework40));
        _inferHelper = new InferHelper(_mockAssemblyMetadataProvider.Object);
        _mockProcessHelper = new Mock<IProcessHelper>();
        _mockAttachmentsProcessingManager = new Mock<ITestRunAttachmentsProcessingManager>();
        _mockEnvironment = new Mock<IEnvironment>();
        _mockEnvironmentVariableHelper = new Mock<IEnvironmentVariableHelper>();
    }

    /// <summary>
    /// The help argument processor get metadata should return help argument processor capabilities.
    /// </summary>
    [TestMethod]
    public void GetMetadataShouldReturnListTestsArgumentProcessorCapabilities()
    {
        var processor = new ListTestsArgumentProcessor();
        Assert.IsTrue(processor.Metadata.Value is ListTestsArgumentProcessorCapabilities);
    }

    /// <summary>
    /// The help argument processor get executer should return help argument processor capabilities.
    /// </summary>
    [TestMethod]
    public void GetExecuterShouldReturnListTestsArgumentProcessorCapabilities()
    {
        var processor = new ListTestsArgumentProcessor();
        Assert.IsTrue(processor.Executor!.Value is ListTestsArgumentExecutor);
    }

    #region ListTestsArgumentProcessorCapabilitiesTests

    [TestMethod]
    public void CapabilitiesShouldReturnAppropriateProperties()
    {
        var capabilities = new ListTestsArgumentProcessorCapabilities();
        Assert.AreEqual("/ListTests", capabilities.CommandName);
        Assert.AreEqual("/lt", capabilities.ShortCommandName);
        var expected = "-lt|--ListTests|/lt|/ListTests:<File Name>\r\n      Lists all discovered tests from the given test container.";
        Assert.AreEqual(expected.NormalizeLineEndings().ShowWhiteSpace(), capabilities.HelpContentResourceName.NormalizeLineEndings().ShowWhiteSpace());

        Assert.AreEqual(HelpContentPriority.ListTestsArgumentProcessorHelpPriority, capabilities.HelpPriority);
        Assert.IsTrue(capabilities.IsAction);
        Assert.AreEqual(ArgumentProcessorPriority.Normal, capabilities.Priority);

        Assert.IsFalse(capabilities.AllowMultiple);
        Assert.IsFalse(capabilities.AlwaysExecute);
        Assert.IsFalse(capabilities.IsSpecialCommand);
    }
    #endregion

    #region ListTestsArgumentExecutorTests

    [TestMethod]
    public void ExecutorInitializeWithValidSourceShouldAddItToTestSources()
    {
        CommandLineOptions.Instance.FileHelper = _mockFileHelper.Object;
        CommandLineOptions.Instance.FilePatternParser = new FilePatternParser(new Mock<Matcher>().Object, _mockFileHelper.Object);
        var testRequestManager = new TestRequestManager(CommandLineOptions.Instance, TestPlatformFactory.GetTestPlatform(), TestRunResultAggregator.Instance, _mockTestPlatformEventSource.Object, _inferHelper, _mockMetricsPublisherTask, _mockProcessHelper.Object, _mockAttachmentsProcessingManager.Object, _mockEnvironment.Object, _mockEnvironmentVariableHelper.Object);
        var executor = GetExecutor(testRequestManager, null);

        executor.Initialize(_dummyTestFilePath);

        Assert.IsTrue(Enumerable.Contains(CommandLineOptions.Instance.Sources, _dummyTestFilePath));
    }

    [TestMethod]
    public void ExecutorExecuteForNoSourcesShouldReturnFail()
    {
        CommandLineOptions.Reset();

        var testRequestManager = new TestRequestManager(CommandLineOptions.Instance, TestPlatformFactory.GetTestPlatform(), TestRunResultAggregator.Instance, _mockTestPlatformEventSource.Object, _inferHelper, _mockMetricsPublisherTask, _mockProcessHelper.Object, _mockAttachmentsProcessingManager.Object, _mockEnvironment.Object, _mockEnvironmentVariableHelper.Object);
        var executor = GetExecutor(testRequestManager, null);

        Assert.ThrowsException<CommandLineException>(() => executor.Execute());
    }

    [TestMethod]
    public void ExecutorExecuteShouldThrowTestPlatformException()
    {
        var mockTestPlatform = new Mock<ITestPlatform>();
        var mockDiscoveryRequest = new Mock<IDiscoveryRequest>();

        mockDiscoveryRequest.Setup(dr => dr.DiscoverAsync()).Throws(new TestPlatformException("DummyTestPlatformException"));
        mockTestPlatform.Setup(tp => tp.CreateDiscoveryRequest(It.IsAny<IRequestData>(), It.IsAny<DiscoveryCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Returns(mockDiscoveryRequest.Object);

        ResetAndAddSourceToCommandLineOptions();

        var testRequestManager = new TestRequestManager(CommandLineOptions.Instance, mockTestPlatform.Object, TestRunResultAggregator.Instance, _mockTestPlatformEventSource.Object, _inferHelper, _mockMetricsPublisherTask, _mockProcessHelper.Object, _mockAttachmentsProcessingManager.Object, _mockEnvironment.Object, _mockEnvironmentVariableHelper.Object);
        var executor = GetExecutor(testRequestManager, null);

        Assert.ThrowsException<TestPlatformException>(() => executor.Execute());
    }

    [TestMethod]
    public void ExecutorExecuteShouldThrowSettingsException()
    {
        var mockTestPlatform = new Mock<ITestPlatform>();
        var mockDiscoveryRequest = new Mock<IDiscoveryRequest>();

        mockDiscoveryRequest.Setup(dr => dr.DiscoverAsync()).Throws(new SettingsException("DummySettingsException"));
        mockTestPlatform.Setup(tp => tp.CreateDiscoveryRequest(It.IsAny<IRequestData>(), It.IsAny<DiscoveryCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Returns(mockDiscoveryRequest.Object);

        ResetAndAddSourceToCommandLineOptions();

        var testRequestManager = new TestRequestManager(CommandLineOptions.Instance, mockTestPlatform.Object, TestRunResultAggregator.Instance, _mockTestPlatformEventSource.Object, _inferHelper, _mockMetricsPublisherTask, _mockProcessHelper.Object, _mockAttachmentsProcessingManager.Object, _mockEnvironment.Object, _mockEnvironmentVariableHelper.Object);
        var listTestsArgumentExecutor = GetExecutor(testRequestManager, null);

        Assert.ThrowsException<SettingsException>(() => listTestsArgumentExecutor.Execute());
    }

    [TestMethod]
    public void ExecutorExecuteShouldThrowInvalidOperationException()
    {
        var mockTestPlatform = new Mock<ITestPlatform>();
        var mockDiscoveryRequest = new Mock<IDiscoveryRequest>();

        mockDiscoveryRequest.Setup(dr => dr.DiscoverAsync()).Throws(new InvalidOperationException("DummyInvalidOperationException"));
        mockTestPlatform.Setup(tp => tp.CreateDiscoveryRequest(It.IsAny<IRequestData>(), It.IsAny<DiscoveryCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Returns(mockDiscoveryRequest.Object);

        ResetAndAddSourceToCommandLineOptions();

        var testRequestManager = new TestRequestManager(CommandLineOptions.Instance, mockTestPlatform.Object, TestRunResultAggregator.Instance, _mockTestPlatformEventSource.Object, _inferHelper, _mockMetricsPublisherTask, _mockProcessHelper.Object, _mockAttachmentsProcessingManager.Object, _mockEnvironment.Object, _mockEnvironmentVariableHelper.Object);
        var listTestsArgumentExecutor = GetExecutor(testRequestManager, null);

        Assert.ThrowsException<InvalidOperationException>(() => listTestsArgumentExecutor.Execute());
    }

    [TestMethod]
    public void ExecutorExecuteShouldThrowOtherExceptions()
    {
        var mockTestPlatform = new Mock<ITestPlatform>();
        var mockDiscoveryRequest = new Mock<IDiscoveryRequest>();

        mockDiscoveryRequest.Setup(dr => dr.DiscoverAsync()).Throws(new Exception("DummyException"));
        mockTestPlatform.Setup(tp => tp.CreateDiscoveryRequest(It.IsAny<IRequestData>(), It.IsAny<DiscoveryCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Returns(mockDiscoveryRequest.Object);

        ResetAndAddSourceToCommandLineOptions();

        var testRequestManager = new TestRequestManager(CommandLineOptions.Instance, mockTestPlatform.Object, TestRunResultAggregator.Instance, _mockTestPlatformEventSource.Object, _inferHelper, _mockMetricsPublisherTask, _mockProcessHelper.Object, _mockAttachmentsProcessingManager.Object, _mockEnvironment.Object, _mockEnvironmentVariableHelper.Object);
        var executor = GetExecutor(testRequestManager, null);

        Assert.ThrowsException<Exception>(() => executor.Execute());
    }

    [TestMethod]
    public void ExecutorExecuteShouldOutputDiscoveredTestsAndReturnSuccess()
    {
        var mockDiscoveryRequest = new Mock<IDiscoveryRequest>();
        var mockConsoleOutput = new Mock<IOutput>();

        RunListTestArgumentProcessorExecuteWithMockSetup(mockDiscoveryRequest, mockConsoleOutput);

        // Assert
        mockDiscoveryRequest.Verify(dr => dr.DiscoverAsync(), Times.Once);

        mockConsoleOutput.Verify((IOutput co) => co.WriteLine("    Test1", OutputLevel.Information));
        mockConsoleOutput.Verify((IOutput co) => co.WriteLine("    Test2", OutputLevel.Information));
    }

    [TestMethod]
    public void ListTestArgumentProcessorExecuteShouldInstrumentDiscoveryRequestStart()
    {
        var mockDiscoveryRequest = new Mock<IDiscoveryRequest>();
        var mockConsoleOutput = new Mock<IOutput>();

        RunListTestArgumentProcessorExecuteWithMockSetup(mockDiscoveryRequest, mockConsoleOutput);

        _mockTestPlatformEventSource.Verify(x => x.DiscoveryRequestStart(), Times.Once);
    }

    [TestMethod]
    public void ListTestArgumentProcessorExecuteShouldInstrumentDiscoveryRequestStop()
    {
        var mockDiscoveryRequest = new Mock<IDiscoveryRequest>();
        var mockConsoleOutput = new Mock<IOutput>();

        RunListTestArgumentProcessorExecuteWithMockSetup(mockDiscoveryRequest, mockConsoleOutput);

        _mockTestPlatformEventSource.Verify(x => x.DiscoveryRequestStop(), Times.Once);
    }
    #endregion

    private void RunListTestArgumentProcessorExecuteWithMockSetup(Mock<IDiscoveryRequest> mockDiscoveryRequest, Mock<IOutput> mockConsoleOutput)
    {
        var mockTestPlatform = new Mock<ITestPlatform>();
        var list = new List<TestCase>
        {
            new TestCase("Test1", new Uri("http://FooTestUri1"), "Source1"),
            new TestCase("Test2", new Uri("http://FooTestUri2"), "Source2")
        };
        mockDiscoveryRequest.Setup(dr => dr.DiscoverAsync()).Raises(dr => dr.OnDiscoveredTests += null, new DiscoveredTestsEventArgs(list));

        mockTestPlatform.Setup(tp => tp.CreateDiscoveryRequest(It.IsAny<IRequestData>(), It.IsAny<DiscoveryCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Returns(mockDiscoveryRequest.Object);


        ResetAndAddSourceToCommandLineOptions();

        var testRequestManager = new TestRequestManager(CommandLineOptions.Instance, mockTestPlatform.Object, TestRunResultAggregator.Instance, _mockTestPlatformEventSource.Object, _inferHelper, _mockMetricsPublisherTask, _mockProcessHelper.Object, _mockAttachmentsProcessingManager.Object, _mockEnvironment.Object, _mockEnvironmentVariableHelper.Object);
        GetExecutor(testRequestManager, mockConsoleOutput.Object).Execute();
    }

    private void ResetAndAddSourceToCommandLineOptions()
    {
        CommandLineOptions.Reset();

        CommandLineOptions.Instance.FileHelper = _mockFileHelper.Object;
        CommandLineOptions.Instance.FilePatternParser = new FilePatternParser(new Mock<Matcher>().Object, _mockFileHelper.Object);
        CommandLineOptions.Instance.AddSource(_dummyTestFilePath);
    }
}
