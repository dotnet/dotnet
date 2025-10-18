// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

using FluentAssertions;

using Microsoft.VisualStudio.TestPlatform.Client.RequestHelper;
using Microsoft.VisualStudio.TestPlatform.CommandLine;
using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestPlatform.CommandLine.Publisher;
using Microsoft.VisualStudio.TestPlatform.CommandLine.TestPlatformHelpers;
using Microsoft.VisualStudio.TestPlatform.CommandLineUtilities;
using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.Logging;
using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Payloads;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using vstest.console.UnitTests.TestDoubles;

using Constants = Microsoft.VisualStudio.TestPlatform.ObjectModel.Constants;

namespace vstest.console.UnitTests.TestPlatformHelpers;

[TestClass]
public class TestRequestManagerTests
{
    private DummyLoggerEvents _mockLoggerEvents;
    private readonly CommandLineOptions _commandLineOptions;
    private readonly Mock<ITestPlatform> _mockTestPlatform;
    private readonly Mock<IDiscoveryRequest> _mockDiscoveryRequest;
    private readonly Mock<ITestRunRequest> _mockRunRequest;
    private readonly Mock<IAssemblyMetadataProvider> _mockAssemblyMetadataProvider;
    private readonly InferHelper _inferHelper;
    private ITestRequestManager _testRequestManager;
    private readonly Mock<ITestPlatformEventSource> _mockTestPlatformEventSource;
    private readonly ProtocolConfig _protocolConfig;
    private readonly Task<IMetricsPublisher> _mockMetricsPublisherTask;
    private readonly Mock<IMetricsPublisher> _mockMetricsPublisher;
    private readonly Mock<IProcessHelper> _mockProcessHelper;
    private readonly Mock<ITestRunAttachmentsProcessingManager> _mockAttachmentsProcessingManager;
    private readonly Mock<IEnvironment> _mockEnvironment;
    private readonly Mock<IEnvironmentVariableHelper> _mockEnvironmentVariableHelper;

    private const string DefaultRunsettings = @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                     </RunConfiguration>
                </RunSettings>";

    public TestRequestManagerTests()
    {
        _mockLoggerEvents = new DummyLoggerEvents(TestSessionMessageLogger.Instance);
        _commandLineOptions = new DummyCommandLineOptions();
        _mockTestPlatform = new Mock<ITestPlatform>();
        _mockDiscoveryRequest = new Mock<IDiscoveryRequest>();
        _mockRunRequest = new Mock<ITestRunRequest>();
        _mockTestPlatformEventSource = new Mock<ITestPlatformEventSource>();
        _protocolConfig = new ProtocolConfig();
        _mockAssemblyMetadataProvider = new Mock<IAssemblyMetadataProvider>();
        _inferHelper = new InferHelper(_mockAssemblyMetadataProvider.Object);
        var testRunResultAggregator = new DummyTestRunResultAggregator();
        _mockProcessHelper = new Mock<IProcessHelper>();
        _mockEnvironment = new Mock<IEnvironment>();
        _mockEnvironmentVariableHelper = new Mock<IEnvironmentVariableHelper>();

        _mockMetricsPublisher = new Mock<IMetricsPublisher>();
        _mockMetricsPublisherTask = Task.FromResult(_mockMetricsPublisher.Object);
        _mockAttachmentsProcessingManager = new Mock<ITestRunAttachmentsProcessingManager>();
        _testRequestManager = new TestRequestManager(
            _commandLineOptions,
            _mockTestPlatform.Object,
            testRunResultAggregator,
            _mockTestPlatformEventSource.Object,
            _inferHelper,
            _mockMetricsPublisherTask,
            _mockProcessHelper.Object,
            _mockAttachmentsProcessingManager.Object,
            _mockEnvironment.Object,
            _mockEnvironmentVariableHelper.Object);
        _mockTestPlatform.Setup(tp => tp.CreateDiscoveryRequest(It.IsAny<IRequestData>(), It.IsAny<DiscoveryCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>()))
            .Returns(_mockDiscoveryRequest.Object);
        _mockTestPlatform.Setup(tp => tp.CreateTestRunRequest(It.IsAny<IRequestData>(), It.IsAny<TestRunCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>()))
            .Returns(_mockRunRequest.Object);
        _mockAssemblyMetadataProvider.Setup(a => a.GetArchitecture(It.IsAny<string>()))
            .Returns(Architecture.X86);
        _mockAssemblyMetadataProvider.Setup(a => a.GetFrameworkName(It.IsAny<string>()))
            .Returns(new FrameworkName(Constants.DotNetFramework40));
        _mockProcessHelper.Setup(x => x.GetCurrentProcessId()).Returns(1234);
        _mockProcessHelper.Setup(x => x.GetProcessName(It.IsAny<int>())).Returns("dotnet.exe");
    }

    [TestCleanup]
    public void Cleanup()
    {
        CommandLineOptions.Reset();

        // Opt out the Telemetry
        Environment.SetEnvironmentVariable("VSTEST_TELEMETRY_OPTEDIN", "0");
    }

    [TestMethod]
    public void TestRequestManagerShouldNotInitializeConsoleLoggerIfDesignModeIsSet()
    {
        CommandLineOptions.Instance.IsDesignMode = true;
        _mockLoggerEvents = new DummyLoggerEvents(TestSessionMessageLogger.Instance);
        _ = new TestRequestManager(CommandLineOptions.Instance,
            new Mock<ITestPlatform>().Object,
            TestRunResultAggregator.Instance,
            new Mock<ITestPlatformEventSource>().Object,
            _inferHelper,
            _mockMetricsPublisherTask,
            _mockProcessHelper.Object,
            _mockAttachmentsProcessingManager.Object,
            _mockEnvironment.Object,
            _mockEnvironmentVariableHelper.Object);

        Assert.IsFalse(_mockLoggerEvents.EventsSubscribed());
    }

    [TestMethod]
    public void InitializeExtensionsShouldCallTestPlatformToClearAndUpdateExtensions()
    {
        var paths = new List<string>() { "a", "b" };
        _testRequestManager.InitializeExtensions(paths, false);

        _mockTestPlatform.Verify(mt => mt.ClearExtensions(), Times.Once);
        _mockTestPlatform.Verify(mt => mt.UpdateExtensions(paths, false), Times.Once);
    }

    [TestMethod]
    public void ResetShouldResetCommandLineOptionsInstance()
    {
        var oldInstance = CommandLineOptions.Instance;
        _testRequestManager.ResetOptions();

        var newInstance = CommandLineOptions.Instance;

        Assert.AreNotEqual(oldInstance, newInstance, "CommandLineOptions must be cleaned up");
    }

    [TestMethod]
    public void DiscoverTestsShouldReadTheBatchSizeFromSettingsAndSetItForDiscoveryCriteria()
    {
        var payload = new DiscoveryRequestPayload()
        {
            Sources = new List<string>() { "a" },
            RunSettings =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <BatchSize>15</BatchSize>
                     </RunConfiguration>
                </RunSettings>"
        };

        DiscoveryCriteria? actualDiscoveryCriteria = null;
        var mockDiscoveryRequest = new Mock<IDiscoveryRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateDiscoveryRequest(It.IsAny<IRequestData>(), It.IsAny<DiscoveryCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, DiscoveryCriteria discoveryCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualDiscoveryCriteria = discoveryCriteria).Returns(mockDiscoveryRequest.Object);

        _testRequestManager.DiscoverTests(payload, new Mock<ITestDiscoveryEventsRegistrar>().Object, _protocolConfig);
        Assert.AreEqual(15, actualDiscoveryCriteria!.FrequencyOfDiscoveredTestsEvent);
    }

    [TestMethod]
    public void DiscoverTestsShouldCallTestPlatformAndSucceed()
    {
        var payload = new DiscoveryRequestPayload()
        {
            Sources = new List<string>() { "a", "b" },
            RunSettings = DefaultRunsettings
        };

        var createDiscoveryRequestCalled = 0;
        DiscoveryCriteria? actualDiscoveryCriteria = null;
        var mockDiscoveryRequest = new Mock<IDiscoveryRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateDiscoveryRequest(It.IsAny<IRequestData>(), It.IsAny<DiscoveryCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, DiscoveryCriteria discoveryCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) =>
            {
                createDiscoveryRequestCalled++;
                actualDiscoveryCriteria = discoveryCriteria;
            }).Returns(mockDiscoveryRequest.Object);

        var mockDiscoveryRegistrar = new Mock<ITestDiscoveryEventsRegistrar>();

        string testCaseFilterValue = "TestFilter";
        CommandLineOptions.Instance.TestCaseFilterValue = testCaseFilterValue;
        _testRequestManager = new TestRequestManager(CommandLineOptions.Instance,
            _mockTestPlatform.Object,
            TestRunResultAggregator.Instance,
            _mockTestPlatformEventSource.Object,
            _inferHelper,
            _mockMetricsPublisherTask,
            _mockProcessHelper.Object,
            _mockAttachmentsProcessingManager.Object,
            _mockEnvironment.Object,
            _mockEnvironmentVariableHelper.Object);

        _testRequestManager.DiscoverTests(payload, mockDiscoveryRegistrar.Object, _protocolConfig);

        Assert.AreEqual(testCaseFilterValue, actualDiscoveryCriteria!.TestCaseFilter, "TestCaseFilter must be set");

        Assert.AreEqual(1, createDiscoveryRequestCalled, "CreateDiscoveryRequest must be invoked only once.");
        Assert.AreEqual(2, actualDiscoveryCriteria.Sources.Count(), "All Sources must be used for discovery request");
        Assert.AreEqual("a", actualDiscoveryCriteria.Sources.First(), "First Source in list is incorrect");
        Assert.AreEqual("b", actualDiscoveryCriteria.Sources.ElementAt(1), "Second Source in list is incorrect");

        // Default frequency is set to BatchSize (which is set to 1000).
        Assert.AreEqual(1000, actualDiscoveryCriteria.FrequencyOfDiscoveredTestsEvent);

        mockDiscoveryRegistrar.Verify(md => md.RegisterDiscoveryEvents(It.IsAny<IDiscoveryRequest>()), Times.Once);
        mockDiscoveryRegistrar.Verify(md => md.UnregisterDiscoveryEvents(It.IsAny<IDiscoveryRequest>()), Times.Once);

        mockDiscoveryRequest.Verify(md => md.DiscoverAsync(), Times.Once);

        _mockTestPlatformEventSource.Verify(mt => mt.DiscoveryRequestStart(), Times.Once);
        _mockTestPlatformEventSource.Verify(mt => mt.DiscoveryRequestStop(), Times.Once);
    }

    [TestMethod]
    public void DiscoverTestsShouldPassSameProtocolConfigInRequestData()
    {
        var payload = new DiscoveryRequestPayload()
        {
            Sources = new List<string>() { "a", "b" },
            RunSettings = DefaultRunsettings
        };

        var mockProtocolConfig = new ProtocolConfig { Version = 6 };

        IRequestData? actualRequestData = null;
        var mockDiscoveryRequest = new Mock<IDiscoveryRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateDiscoveryRequest(It.IsAny<IRequestData>(), It.IsAny<DiscoveryCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, DiscoveryCriteria discoveryCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualRequestData = requestData).Returns(mockDiscoveryRequest.Object);

        var mockDiscoveryRegistrar = new Mock<ITestDiscoveryEventsRegistrar>();

        string testCaseFilterValue = "TestFilter";
        CommandLineOptions.Instance.TestCaseFilterValue = testCaseFilterValue;
        _testRequestManager = new TestRequestManager(CommandLineOptions.Instance,
            _mockTestPlatform.Object,
            TestRunResultAggregator.Instance,
            _mockTestPlatformEventSource.Object,
            _inferHelper,
            _mockMetricsPublisherTask,
            _mockProcessHelper.Object,
            _mockAttachmentsProcessingManager.Object,
            _mockEnvironment.Object,
            _mockEnvironmentVariableHelper.Object);

        // Act
        _testRequestManager.DiscoverTests(payload, mockDiscoveryRegistrar.Object, mockProtocolConfig);

        // Verify.
        Assert.AreEqual(6, actualRequestData!.ProtocolConfig!.Version);
    }


    [TestMethod]
    public void DiscoverTestsShouldCollectMetrics()
    {
        // Opt in the Telemetry
        Environment.SetEnvironmentVariable("VSTEST_TELEMETRY_OPTEDIN", "1");

        var payload = new DiscoveryRequestPayload()
        {
            Sources = new List<string>() { "a.dll", "b.dll" },
            RunSettings = @"<RunSettings>
                                <RunConfiguration>
                                    <MaxCpuCount>2</MaxCpuCount>
                                    <TargetPlatform>x86</TargetPlatform>
                                    <TargetFrameworkVersion>Framework35</TargetFrameworkVersion>
                                    <DisableAppDomain>True</DisableAppDomain>
                                </RunConfiguration>
                                <MSPhoneTest>
                                  <TargetDevice>169.254.193.190</TargetDevice>
                                </MSPhoneTest>
                            </RunSettings>"
        };

        var mockProtocolConfig = new ProtocolConfig { Version = 6 };
        var mockDiscoveryRegistrar = new Mock<ITestDiscoveryEventsRegistrar>();

        IRequestData? actualRequestData = null;
        var mockDiscoveryRequest = new Mock<IDiscoveryRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateDiscoveryRequest(It.IsAny<IRequestData>(), It.IsAny<DiscoveryCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, DiscoveryCriteria discoveryCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualRequestData = requestData).Returns(mockDiscoveryRequest.Object);

        _testRequestManager = new TestRequestManager(
            CommandLineOptions.Instance,
            _mockTestPlatform.Object,
            TestRunResultAggregator.Instance,
            _mockTestPlatformEventSource.Object,
            _inferHelper,
            _mockMetricsPublisherTask,
            _mockProcessHelper.Object,
            _mockAttachmentsProcessingManager.Object,
            _mockEnvironment.Object,
            _mockEnvironmentVariableHelper.Object);


        // Act
        _testRequestManager.DiscoverTests(payload, mockDiscoveryRegistrar.Object, mockProtocolConfig);

        // Verify.
        Assert.IsTrue(actualRequestData!.MetricsCollection.Metrics.TryGetValue(TelemetryDataConstants.TargetDevice, out var targetDevice));
        Assert.IsTrue(actualRequestData.MetricsCollection.Metrics.TryGetValue(TelemetryDataConstants.MaxCPUcount, out var maxcount));
        Assert.IsTrue(actualRequestData.MetricsCollection.Metrics.TryGetValue(TelemetryDataConstants.TargetPlatform, out var targetPlatform));
        Assert.IsTrue(actualRequestData.MetricsCollection.Metrics.TryGetValue(TelemetryDataConstants.DisableAppDomain, out var disableAppDomain));
        Assert.AreEqual("Other", targetDevice);
        Assert.AreEqual(2, maxcount);
        Assert.AreEqual("X86", targetPlatform.ToString());
        Assert.AreEqual(true, disableAppDomain);
    }

    [TestMethod]
    public void DiscoverTestsShouldCollectTargetDeviceLocalMachineIfTargetDeviceStringisEmpty()
    {
        // Opt in the Telemetry
        Environment.SetEnvironmentVariable("VSTEST_TELEMETRY_OPTEDIN", "1");

        var payload = new DiscoveryRequestPayload()
        {
            Sources = new List<string>() { "a", "b" },
            RunSettings = @"<RunSettings>
                                <RunConfiguration>
                                    <TargetDevice></TargetDevice>
                                </RunConfiguration>
                            </RunSettings>"
        };

        var mockProtocolConfig = new ProtocolConfig { Version = 6 };
        var mockDiscoveryRegistrar = new Mock<ITestDiscoveryEventsRegistrar>();

        IRequestData? actualRequestData = null;
        var mockDiscoveryRequest = new Mock<IDiscoveryRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateDiscoveryRequest(It.IsAny<IRequestData>(), It.IsAny<DiscoveryCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, DiscoveryCriteria discoveryCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualRequestData = requestData).Returns(mockDiscoveryRequest.Object);

        _testRequestManager = new TestRequestManager(
            CommandLineOptions.Instance,
            _mockTestPlatform.Object,
            TestRunResultAggregator.Instance,
            _mockTestPlatformEventSource.Object,
            _inferHelper,
            _mockMetricsPublisherTask,
            _mockProcessHelper.Object,
            _mockAttachmentsProcessingManager.Object,
            _mockEnvironment.Object,
            _mockEnvironmentVariableHelper.Object);


        // Act
        _testRequestManager.DiscoverTests(payload, mockDiscoveryRegistrar.Object, mockProtocolConfig);

        // Verify.
        Assert.IsTrue(actualRequestData!.MetricsCollection.Metrics.TryGetValue(TelemetryDataConstants.TargetDevice, out var targetDevice));
        Assert.AreEqual("Local Machine", targetDevice);
    }

    [TestMethod]
    public void DiscoverTestsShouldCollectTargetDeviceIfTargetDeviceIsDevice()
    {
        // Opt in the Telemetry
        Environment.SetEnvironmentVariable("VSTEST_TELEMETRY_OPTEDIN", "1");

        var payload = new DiscoveryRequestPayload()
        {
            Sources = new List<string>() { "a", "b" },
            RunSettings = @"<RunSettings>
                                <RunConfiguration>
                                    <TargetDevice>Device</TargetDevice>
                                </RunConfiguration>
                            </RunSettings>"
        };

        var mockProtocolConfig = new ProtocolConfig { Version = 6 };
        var mockDiscoveryRegistrar = new Mock<ITestDiscoveryEventsRegistrar>();

        IRequestData? actualRequestData = null;
        var mockDiscoveryRequest = new Mock<IDiscoveryRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateDiscoveryRequest(It.IsAny<IRequestData>(), It.IsAny<DiscoveryCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, DiscoveryCriteria discoveryCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualRequestData = requestData).Returns(mockDiscoveryRequest.Object);

        _testRequestManager = new TestRequestManager(
            CommandLineOptions.Instance,
            _mockTestPlatform.Object,
            TestRunResultAggregator.Instance,
            _mockTestPlatformEventSource.Object,
            _inferHelper,
            _mockMetricsPublisherTask,
            _mockProcessHelper.Object,
            _mockAttachmentsProcessingManager.Object,
            _mockEnvironment.Object,
            _mockEnvironmentVariableHelper.Object);


        // Act
        _testRequestManager.DiscoverTests(payload, mockDiscoveryRegistrar.Object, mockProtocolConfig);

        // Verify.
        Assert.IsTrue(actualRequestData!.MetricsCollection.Metrics.TryGetValue(TelemetryDataConstants.TargetDevice, out var targetDevice));
        Assert.AreEqual("Device", targetDevice);
    }

    [TestMethod]
    public void DiscoverTestsShouldCollectTargetDeviceIfTargetDeviceIsEmulator()
    {
        // Opt in the Telemetry
        Environment.SetEnvironmentVariable("VSTEST_TELEMETRY_OPTEDIN", "1");

        var payload = new DiscoveryRequestPayload()
        {
            Sources = new List<string>() { "a", "b" },
            RunSettings = @"<RunSettings>
                                <RunConfiguration>
                                    <TargetDevice>Emulator 8.1 U1 WVGA 4 inch 512MB</TargetDevice>
                                </RunConfiguration>
                            </RunSettings>"
        };

        var mockProtocolConfig = new ProtocolConfig { Version = 6 };
        var mockDiscoveryRegistrar = new Mock<ITestDiscoveryEventsRegistrar>();

        IRequestData? actualRequestData = null;
        var mockDiscoveryRequest = new Mock<IDiscoveryRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateDiscoveryRequest(It.IsAny<IRequestData>(), It.IsAny<DiscoveryCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, DiscoveryCriteria discoveryCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualRequestData = requestData).Returns(mockDiscoveryRequest.Object);

        _testRequestManager = new TestRequestManager(
            CommandLineOptions.Instance,
            _mockTestPlatform.Object,
            TestRunResultAggregator.Instance,
            _mockTestPlatformEventSource.Object,
            _inferHelper,
            _mockMetricsPublisherTask,
            _mockProcessHelper.Object,
            _mockAttachmentsProcessingManager.Object,
            _mockEnvironment.Object,
            _mockEnvironmentVariableHelper.Object);


        // Act
        _testRequestManager.DiscoverTests(payload, mockDiscoveryRegistrar.Object, mockProtocolConfig);

        // Verify.
        Assert.IsTrue(actualRequestData!.MetricsCollection.Metrics.TryGetValue(TelemetryDataConstants.TargetDevice, out var targetDevice));
        Assert.AreEqual("Emulator 8.1 U1 WVGA 4 inch 512MB", targetDevice);
    }

    [TestMethod]
    public void DiscoverTestsShouldCollectCommands()
    {
        // Opt in the Telemetry
        Environment.SetEnvironmentVariable("VSTEST_TELEMETRY_OPTEDIN", "1");

        var payload = new DiscoveryRequestPayload()
        {
            Sources = new List<string>() { "a", "b" },
            RunSettings = @"<RunSettings>
                                <RunConfiguration>
                                    <TargetDevice>Device</TargetDevice>
                                </RunConfiguration>
                            </RunSettings>"
        };

        var mockProtocolConfig = new ProtocolConfig { Version = 6 };
        var mockDiscoveryRegistrar = new Mock<ITestDiscoveryEventsRegistrar>();

        IRequestData? actualRequestData = null;
        var mockDiscoveryRequest = new Mock<IDiscoveryRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateDiscoveryRequest(It.IsAny<IRequestData>(), It.IsAny<DiscoveryCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, DiscoveryCriteria discoveryCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualRequestData = requestData).Returns(mockDiscoveryRequest.Object);

        _testRequestManager = new TestRequestManager(
            CommandLineOptions.Instance,
            _mockTestPlatform.Object,
            TestRunResultAggregator.Instance,
            _mockTestPlatformEventSource.Object,
            _inferHelper,
            _mockMetricsPublisherTask,
            _mockProcessHelper.Object,
            _mockAttachmentsProcessingManager.Object,
            _mockEnvironment.Object,
            _mockEnvironmentVariableHelper.Object);

        CommandLineOptions.Instance.Parallel = true;
        CommandLineOptions.Instance.EnableCodeCoverage = true;
        CommandLineOptions.Instance.InIsolation = true;
        CommandLineOptions.Instance.UseVsixExtensions = true;
        CommandLineOptions.Instance.SettingsFile = @"c://temp/.runsettings";

        // Act
        _testRequestManager.DiscoverTests(payload, mockDiscoveryRegistrar.Object, mockProtocolConfig);

        // Verify
        Assert.IsTrue(actualRequestData!.MetricsCollection.Metrics.TryGetValue(TelemetryDataConstants.CommandLineSwitches, out var commandLineSwitches));

        var commandLineArray = commandLineSwitches.ToString();

        Assert.IsTrue(commandLineArray!.Contains("/Parallel"));
        Assert.IsTrue(commandLineArray.Contains("/EnableCodeCoverage"));
        Assert.IsTrue(commandLineArray.Contains("/InIsolation"));
        Assert.IsTrue(commandLineArray.Contains("/UseVsixExtensions"));
        Assert.IsTrue(commandLineArray.Contains("/settings//.RunSettings"));
    }

    [TestMethod]
    public void DiscoverTestsShouldCollectTestSettings()
    {
        // Opt in the Telemetry
        Environment.SetEnvironmentVariable("VSTEST_TELEMETRY_OPTEDIN", "1");

        var payload = new DiscoveryRequestPayload()
        {
            Sources = new List<string>() { "a", "b" },
            RunSettings = @"<RunSettings>
                                <RunConfiguration>
                                    <TargetDevice>Device</TargetDevice>
                                </RunConfiguration>
                            </RunSettings>"
        };

        var mockProtocolConfig = new ProtocolConfig { Version = 6 };
        var mockDiscoveryRegistrar = new Mock<ITestDiscoveryEventsRegistrar>();

        IRequestData? actualRequestData = null;
        var mockDiscoveryRequest = new Mock<IDiscoveryRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateDiscoveryRequest(It.IsAny<IRequestData>(), It.IsAny<DiscoveryCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, DiscoveryCriteria discoveryCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualRequestData = requestData).Returns(mockDiscoveryRequest.Object);

        _testRequestManager = new TestRequestManager(
            CommandLineOptions.Instance,
            _mockTestPlatform.Object,
            TestRunResultAggregator.Instance,
            _mockTestPlatformEventSource.Object,
            _inferHelper,
            _mockMetricsPublisherTask,
            _mockProcessHelper.Object,
            _mockAttachmentsProcessingManager.Object,
            _mockEnvironment.Object,
            _mockEnvironmentVariableHelper.Object);

        CommandLineOptions.Instance.SettingsFile = @"c://temp/.testsettings";

        // Act
        _testRequestManager.DiscoverTests(payload, mockDiscoveryRegistrar.Object, mockProtocolConfig);

        // Verify
        Assert.IsTrue(actualRequestData!.MetricsCollection.Metrics.TryGetValue(TelemetryDataConstants.CommandLineSwitches, out var commandLineSwitches));

        var commandLineArray = commandLineSwitches.ToString();
        Assert.IsTrue(commandLineArray!.Contains("/settings//.TestSettings"));
    }

    [TestMethod]
    public void DiscoverTestsShouldCollectVsmdiFile()
    {
        // Opt in the Telemetry
        Environment.SetEnvironmentVariable("VSTEST_TELEMETRY_OPTEDIN", "1");

        var payload = new DiscoveryRequestPayload()
        {
            Sources = new List<string>() { "a", "b" },
            RunSettings = @"<RunSettings>
                                <RunConfiguration>
                                    <TargetDevice>Device</TargetDevice>
                                </RunConfiguration>
                            </RunSettings>"
        };

        var mockProtocolConfig = new ProtocolConfig { Version = 6 };
        var mockDiscoveryRegistrar = new Mock<ITestDiscoveryEventsRegistrar>();

        IRequestData? actualRequestData = null;
        var mockDiscoveryRequest = new Mock<IDiscoveryRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateDiscoveryRequest(It.IsAny<IRequestData>(), It.IsAny<DiscoveryCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, DiscoveryCriteria discoveryCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualRequestData = requestData).Returns(mockDiscoveryRequest.Object);

        _testRequestManager = new TestRequestManager(
            CommandLineOptions.Instance,
            _mockTestPlatform.Object,
            TestRunResultAggregator.Instance,
            _mockTestPlatformEventSource.Object,
            _inferHelper,
            _mockMetricsPublisherTask,
            _mockProcessHelper.Object,
            _mockAttachmentsProcessingManager.Object,
            _mockEnvironment.Object,
            _mockEnvironmentVariableHelper.Object);

        CommandLineOptions.Instance.SettingsFile = @"c://temp/.vsmdi";

        // Act
        _testRequestManager.DiscoverTests(payload, mockDiscoveryRegistrar.Object, mockProtocolConfig);

        // Verify
        Assert.IsTrue(actualRequestData!.MetricsCollection.Metrics.TryGetValue(TelemetryDataConstants.CommandLineSwitches, out var commandLineSwitches));

        var commandLineArray = commandLineSwitches.ToString();
        Assert.IsTrue(commandLineArray!.Contains("/settings//.vsmdi"));
    }

    [TestMethod]
    public void DiscoverTestsShouldCollectTestRunConfigFile()
    {
        // Opt in the Telemetry
        Environment.SetEnvironmentVariable("VSTEST_TELEMETRY_OPTEDIN", "1");

        var payload = new DiscoveryRequestPayload()
        {
            Sources = new List<string>() { "a", "b" },
            RunSettings = @"<RunSettings>
                                <RunConfiguration>
                                    <TargetDevice>Device</TargetDevice>
                                </RunConfiguration>
                            </RunSettings>"
        };

        var mockProtocolConfig = new ProtocolConfig { Version = 6 };
        var mockDiscoveryRegistrar = new Mock<ITestDiscoveryEventsRegistrar>();

        IRequestData? actualRequestData = null;
        var mockDiscoveryRequest = new Mock<IDiscoveryRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateDiscoveryRequest(It.IsAny<IRequestData>(), It.IsAny<DiscoveryCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, DiscoveryCriteria discoveryCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualRequestData = requestData).Returns(mockDiscoveryRequest.Object);

        _testRequestManager = new TestRequestManager(
            CommandLineOptions.Instance,
            _mockTestPlatform.Object,
            TestRunResultAggregator.Instance,
            _mockTestPlatformEventSource.Object,
            _inferHelper,
            _mockMetricsPublisherTask,
            _mockProcessHelper.Object,
            _mockAttachmentsProcessingManager.Object,
            _mockEnvironment.Object,
            _mockEnvironmentVariableHelper.Object);

        CommandLineOptions.Instance.SettingsFile = @"c://temp/.testrunConfig";

        // Act
        _testRequestManager.DiscoverTests(payload, mockDiscoveryRegistrar.Object, mockProtocolConfig);

        // Verify
        Assert.IsTrue(actualRequestData!.MetricsCollection.Metrics.TryGetValue(TelemetryDataConstants.CommandLineSwitches, out var commandLineSwitches));

        var commandLineArray = commandLineSwitches.ToString();
        Assert.IsTrue(commandLineArray!.Contains("/settings//.testrunConfig"));
    }

    [TestMethod]
    public void DiscoverTestsShouldUpdateFrameworkAndPlatformIfNotSpecifiedInDesignMode()
    {
        var payload = new DiscoveryRequestPayload()
        {
            Sources = new List<string>() { "a.dll" },
            RunSettings =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                     </RunConfiguration>
                </RunSettings>"
        };
        _commandLineOptions.IsDesignMode = true;
        _mockAssemblyMetadataProvider.Setup(a => a.GetArchitecture(It.IsAny<string>()))
            .Returns(Architecture.ARM);
        _mockAssemblyMetadataProvider.Setup(a => a.GetFrameworkName(It.IsAny<string>()))
            .Returns(new FrameworkName(Constants.DotNetFramework46));
        DiscoveryCriteria? actualDiscoveryCriteria = null;
        var mockDiscoveryRequest = new Mock<IDiscoveryRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateDiscoveryRequest(It.IsAny<IRequestData>(), It.IsAny<DiscoveryCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, DiscoveryCriteria discoveryCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualDiscoveryCriteria = discoveryCriteria).Returns(mockDiscoveryRequest.Object);

        _testRequestManager.DiscoverTests(payload, new Mock<ITestDiscoveryEventsRegistrar>().Object, _protocolConfig);

        _mockAssemblyMetadataProvider.Verify(a => a.GetArchitecture(It.IsAny<string>()));
        _mockAssemblyMetadataProvider.Verify(a => a.GetFrameworkName(It.IsAny<string>()));

        Assert.IsTrue(actualDiscoveryCriteria!.RunSettings!.Contains(Constants.DotNetFramework46));
        Assert.IsTrue(actualDiscoveryCriteria.RunSettings.Contains(nameof(Architecture.ARM)));
    }

    [TestMethod]
    public void DiscoverTestsShouldNotUpdateFrameworkAndPlatformIfSpecifiedInDesignMode()
    {
        var payload = new DiscoveryRequestPayload()
        {
            Sources = new List<string>() { "a" },
            RunSettings =
                $@"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                     <TargetFrameworkVersion>{Constants.DotNetFramework46}</TargetFrameworkVersion>
                     <TargetPlatform>{Architecture.ARM}</TargetPlatform>
                     </RunConfiguration>
                </RunSettings>"
        };
        _commandLineOptions.IsDesignMode = true;
        _mockAssemblyMetadataProvider.Setup(a => a.GetArchitecture(It.IsAny<string>()))
            .Returns(Architecture.X86);
        _mockAssemblyMetadataProvider.Setup(a => a.GetFrameworkName(It.IsAny<string>()))
            .Returns(new FrameworkName(Constants.DotNetFramework451));
        DiscoveryCriteria? actualDiscoveryCriteria = null;
        var mockDiscoveryRequest = new Mock<IDiscoveryRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateDiscoveryRequest(It.IsAny<IRequestData>(), It.IsAny<DiscoveryCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, DiscoveryCriteria discoveryCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualDiscoveryCriteria = discoveryCriteria).Returns(mockDiscoveryRequest.Object);

        _testRequestManager.DiscoverTests(payload, new Mock<ITestDiscoveryEventsRegistrar>().Object, _protocolConfig);

        _mockAssemblyMetadataProvider.Verify(a => a.GetArchitecture(It.IsAny<string>()), Times.Never);
        _mockAssemblyMetadataProvider.Verify(a => a.GetFrameworkName(It.IsAny<string>()), Times.Never);

        Assert.IsTrue(actualDiscoveryCriteria!.RunSettings!.Contains(Constants.DotNetFramework46));
        Assert.IsTrue(actualDiscoveryCriteria.RunSettings.Contains(nameof(Architecture.ARM)));
    }

    [TestMethod]
    public void DiscoverTestsShouldUpdateFrameworkAndPlatformInCommandLineScenariosIfNotSpecified()
    {
        var payload = new DiscoveryRequestPayload()
        {
            Sources = new List<string>() { "a.dll" },
            RunSettings =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                     </RunConfiguration>
                </RunSettings>"
        };
        _commandLineOptions.IsDesignMode = false;
        _mockAssemblyMetadataProvider.Setup(a => a.GetArchitecture(It.IsAny<string>()))
            .Returns(Architecture.ARM);
        _mockAssemblyMetadataProvider.Setup(a => a.GetFrameworkName(It.IsAny<string>()))
            .Returns(new FrameworkName(Constants.DotNetFramework46));
        DiscoveryCriteria? actualDiscoveryCriteria = null;
        var mockDiscoveryRequest = new Mock<IDiscoveryRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateDiscoveryRequest(It.IsAny<IRequestData>(), It.IsAny<DiscoveryCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, DiscoveryCriteria discoveryCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualDiscoveryCriteria = discoveryCriteria).Returns(mockDiscoveryRequest.Object);

        _testRequestManager.DiscoverTests(payload, new Mock<ITestDiscoveryEventsRegistrar>().Object, _protocolConfig);
        _mockAssemblyMetadataProvider.Verify(a => a.GetArchitecture(It.IsAny<string>()));
        _mockAssemblyMetadataProvider.Verify(a => a.GetFrameworkName(It.IsAny<string>()));

        Assert.IsTrue(actualDiscoveryCriteria!.RunSettings!.Contains(Constants.DotNetFramework46));
        Assert.IsTrue(actualDiscoveryCriteria.RunSettings.Contains(nameof(Architecture.ARM)));
    }

    [TestMethod]
    public void DiscoverTestsShouldNotInferAndUpdateFrameworkAndPlatformInCommandLineScenariosIfSpecified()
    {
        var payload = new DiscoveryRequestPayload()
        {
            Sources = new List<string>() { "a.dll" },
            RunSettings =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                     </RunConfiguration>
                </RunSettings>"
        };
        _commandLineOptions.IsDesignMode = false;

        // specified architecture
        _commandLineOptions.TargetFrameworkVersion = Framework.DefaultFramework;
        _commandLineOptions.TargetArchitecture = Architecture.X86;
        _mockAssemblyMetadataProvider.Setup(a => a.GetArchitecture(It.IsAny<string>()))
            .Returns(Architecture.ARM);
        _mockAssemblyMetadataProvider.Setup(a => a.GetFrameworkName(It.IsAny<string>()))
            .Returns(new FrameworkName(Constants.DotNetFramework46));
        DiscoveryCriteria? actualDiscoveryCriteria = null;
        var mockDiscoveryRequest = new Mock<IDiscoveryRequest>();
        _mockTestPlatform
            .Setup(mt => mt.CreateDiscoveryRequest(It.IsAny<IRequestData>(), It.IsAny<DiscoveryCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>()))
            .Callback(
                (IRequestData requestData, DiscoveryCriteria discoveryCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualDiscoveryCriteria = discoveryCriteria).Returns(mockDiscoveryRequest.Object);

        _testRequestManager.DiscoverTests(payload,
            new Mock<ITestDiscoveryEventsRegistrar>().Object, _protocolConfig);

        // we infer the architecture and framework, so we can print warning when they don't match settings.
        _mockAssemblyMetadataProvider.Verify(a => a.GetArchitecture(It.IsAny<string>()), Times.Once);
        _mockAssemblyMetadataProvider.Verify(a => a.GetFrameworkName(It.IsAny<string>()), Times.Once);

        // but we don't update the settings, to keep what user specified
        Assert.IsFalse(actualDiscoveryCriteria!.RunSettings!.Contains(Constants.DotNetFramework46));
        Assert.IsFalse(actualDiscoveryCriteria.RunSettings.Contains(nameof(Architecture.ARM)));
    }

    [TestMethod]
    public void DiscoverTestsShouldPublishMetrics()
    {
        var payload = new DiscoveryRequestPayload()
        {
            Sources = new List<string>() { "a", "b" }
        };
        var mockProtocolConfig = new ProtocolConfig { Version = 2 };
        var mockDiscoveryRegistrar = new Mock<ITestDiscoveryEventsRegistrar>();

        // Act
        _testRequestManager.DiscoverTests(payload, mockDiscoveryRegistrar.Object, mockProtocolConfig);

        // Verify.
        _mockMetricsPublisher.Verify(mp => mp.PublishMetrics(TelemetryDataConstants.TestDiscoveryCompleteEvent, It.IsAny<IDictionary<string, object?>>()), Times.Once);
    }

    [TestMethod]
    public void CancelShouldNotThrowExceptionIfTestRunRequestHasBeenDisposed()
    {
        var payload = new TestRunRequestPayload()
        {
            Sources = new List<string>() { "a", "b" },
            RunSettings = DefaultRunsettings
        };

        var mockRunEventsRegistrar = new Mock<ITestRunEventsRegistrar>();
        var mockCustomlauncher = new Mock<ITestHostLauncher3>();

        _testRequestManager.RunTests(payload, mockCustomlauncher.Object, mockRunEventsRegistrar.Object, _protocolConfig);
        _testRequestManager.CancelTestRun();
    }

    [TestMethod]
    public void AbortShouldNotThrowExceptionIfTestRunRequestHasBeenDisposed()
    {
        var payload = new TestRunRequestPayload()
        {
            Sources = new List<string>() { "a", "b" },
            RunSettings = DefaultRunsettings
        };

        var mockRunEventsRegistrar = new Mock<ITestRunEventsRegistrar>();
        var mockCustomlauncher = new Mock<ITestHostLauncher3>();

        _testRequestManager.RunTests(payload, mockCustomlauncher.Object, mockRunEventsRegistrar.Object, _protocolConfig);
        _testRequestManager.AbortTestRun();
    }

    [TestMethod]
    public void RunTestsShouldReadTheBatchSizeFromSettingsAndSetItForTestRunCriteria()
    {
        var payload = new TestRunRequestPayload()
        {
            Sources = new List<string>() { "a" },
            RunSettings =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <BatchSize>15</BatchSize>
                     </RunConfiguration>
                </RunSettings>"
        };

        TestRunCriteria? actualTestRunCriteria = null;
        var mockDiscoveryRequest = new Mock<ITestRunRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateTestRunRequest(It.IsAny<IRequestData>(), It.IsAny<TestRunCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, TestRunCriteria runCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualTestRunCriteria = runCriteria).Returns(mockDiscoveryRequest.Object);

        _testRequestManager.RunTests(payload, new Mock<ITestHostLauncher3>().Object, new Mock<ITestRunEventsRegistrar>().Object, _protocolConfig);
        Assert.AreEqual(15, actualTestRunCriteria!.FrequencyOfRunStatsChangeEvent);
    }

    [TestMethod]
    public void RunTestsShouldNotThrowForFramework35()
    {
        var payload = new TestRunRequestPayload()
        {
            Sources = new List<string>() { "a.dll" },
            RunSettings =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <TargetFrameworkVersion>Framework35</TargetFrameworkVersion>
                     </RunConfiguration>
                </RunSettings>"
        };

        TestRunCriteria? actualTestRunCriteria = null;
        var mockDiscoveryRequest = new Mock<ITestRunRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateTestRunRequest(It.IsAny<IRequestData>(), It.IsAny<TestRunCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, TestRunCriteria runCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualTestRunCriteria = runCriteria).Returns(mockDiscoveryRequest.Object);
        _mockAssemblyMetadataProvider.Setup(a => a.GetFrameworkName(It.IsAny<string>())).Returns(new FrameworkName(Constants.DotNetFramework35));

        var mockRunEventsRegistrar = new Mock<ITestRunEventsRegistrar>();
        var mockCustomlauncher = new Mock<ITestHostLauncher3>();

        _testRequestManager.RunTests(payload, mockCustomlauncher.Object, mockRunEventsRegistrar.Object, _protocolConfig);

        mockRunEventsRegistrar.Verify(lw => lw.LogWarning("Framework35 is not supported. For projects targeting .Net Framework 3.5, test will run in CLR 4.0 \"compatibility mode\"."), Times.Once);
        _mockTestPlatformEventSource.Verify(mt => mt.ExecutionRequestStart(), Times.Once);
        _mockTestPlatformEventSource.Verify(mt => mt.ExecutionRequestStop(), Times.Once);
    }

    [TestMethod]
    public void RunTestsShouldPassSameProtocolConfigInRequestData()
    {
        var payload = new TestRunRequestPayload()
        {
            Sources = new List<string>() { "a" },
            RunSettings = DefaultRunsettings
        };
        var mockProtocolConfig = new ProtocolConfig { Version = 6 };
        IRequestData? actualRequestData = null;
        var mockDiscoveryRequest = new Mock<ITestRunRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateTestRunRequest(It.IsAny<IRequestData>(), It.IsAny<TestRunCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, TestRunCriteria runCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualRequestData = requestData).Returns(mockDiscoveryRequest.Object);

        // Act.
        _testRequestManager.RunTests(payload, new Mock<ITestHostLauncher3>().Object, new Mock<ITestRunEventsRegistrar>().Object, mockProtocolConfig);

        // Verify.
        Assert.AreEqual(6, actualRequestData!.ProtocolConfig!.Version);
    }

    [TestMethod]
    public void RunTestsShouldCollectCommands()
    {
        // Opt in the Telemetry
        Environment.SetEnvironmentVariable("VSTEST_TELEMETRY_OPTEDIN", "1");

        var payload = new TestRunRequestPayload()
        {
            Sources = new List<string>() { "a" },
            RunSettings = DefaultRunsettings
        };
        var mockProtocolConfig = new ProtocolConfig { Version = 6 };
        IRequestData? actualRequestData = null;
        var mockDiscoveryRequest = new Mock<ITestRunRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateTestRunRequest(It.IsAny<IRequestData>(), It.IsAny<TestRunCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, TestRunCriteria runCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualRequestData = requestData).Returns(mockDiscoveryRequest.Object);

        _testRequestManager = new TestRequestManager(
            CommandLineOptions.Instance,
            _mockTestPlatform.Object,
            TestRunResultAggregator.Instance,
            _mockTestPlatformEventSource.Object,
            _inferHelper,
            _mockMetricsPublisherTask,
            _mockProcessHelper.Object,
            _mockAttachmentsProcessingManager.Object,
            _mockEnvironment.Object,
            _mockEnvironmentVariableHelper.Object);

        CommandLineOptions.Instance.Parallel = true;
        CommandLineOptions.Instance.EnableCodeCoverage = true;
        CommandLineOptions.Instance.InIsolation = true;
        CommandLineOptions.Instance.UseVsixExtensions = true;
        CommandLineOptions.Instance.SettingsFile = @"c://temp/.runsettings";

        // Act.
        _testRequestManager.RunTests(payload, new Mock<ITestHostLauncher3>().Object, new Mock<ITestRunEventsRegistrar>().Object, mockProtocolConfig);

        // Verify
        Assert.IsTrue(actualRequestData!.MetricsCollection.Metrics.TryGetValue(TelemetryDataConstants.CommandLineSwitches, out var commandLineSwitches));

        var commandLineArray = commandLineSwitches.ToString();

        Assert.IsTrue(commandLineArray!.Contains("/Parallel"));
        Assert.IsTrue(commandLineArray.Contains("/EnableCodeCoverage"));
        Assert.IsTrue(commandLineArray.Contains("/InIsolation"));
        Assert.IsTrue(commandLineArray.Contains("/UseVsixExtensions"));
        Assert.IsTrue(commandLineArray.Contains("/settings//.RunSettings"));
    }

    [TestMethod]
    public void RunTestsShouldCollectTelemetryForLegacySettings()
    {
        // Opt in the Telemetry
        Environment.SetEnvironmentVariable("VSTEST_TELEMETRY_OPTEDIN", "1");

        var payload = new TestRunRequestPayload()
        {
            Sources = new List<string>() { "a" },
            RunSettings = @"<RunSettings>
                                    <LegacySettings>
                                        <Deployment enabled=""true"" deploySatelliteAssemblies=""true"" >
                                            <DeploymentItem filename="".\test.txt"" />
                                        </Deployment>
                                        <Scripts setupScript="".\setup.bat"" cleanupScript="".\cleanup.bat"" />
                                        <Execution hostProcessPlatform=""MSIL"" parallelTestCount=""4"">
                                            <Timeouts testTimeout=""120"" />
                                            <TestTypeSpecific>
                                                <UnitTestRunConfig>
                                                    <AssemblyResolution />
                                                </UnitTestRunConfig>
                                            </TestTypeSpecific>
                                            <Hosts />
                                        </Execution>
                                    </LegacySettings>
                               </RunSettings>"
        };
        var mockProtocolConfig = new ProtocolConfig { Version = 6 };
        IRequestData? actualRequestData = null;
        var mockDiscoveryRequest = new Mock<ITestRunRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateTestRunRequest(It.IsAny<IRequestData>(), It.IsAny<TestRunCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, TestRunCriteria runCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualRequestData = requestData).Returns(mockDiscoveryRequest.Object);

        _testRequestManager = new TestRequestManager(
            CommandLineOptions.Instance,
            _mockTestPlatform.Object,
            TestRunResultAggregator.Instance,
            _mockTestPlatformEventSource.Object,
            _inferHelper,
            _mockMetricsPublisherTask,
            _mockProcessHelper.Object,
            _mockAttachmentsProcessingManager.Object,
            _mockEnvironment.Object,
            _mockEnvironmentVariableHelper.Object);

        // Act.
        _testRequestManager.RunTests(payload, new Mock<ITestHostLauncher3>().Object, new Mock<ITestRunEventsRegistrar>().Object, mockProtocolConfig);

        // Verify
        Assert.IsTrue(actualRequestData!.MetricsCollection.Metrics.TryGetValue("VS.TestRun.LegacySettings.Elements", out var legacySettingsNodes));
        Equals("Deployment, Scripts, Execution, AssemblyResolution, Timeouts, Hosts", legacySettingsNodes);
        Assert.IsTrue(actualRequestData.MetricsCollection.Metrics.TryGetValue("VS.TestRun.LegacySettings.DeploymentAttributes", out var deploymentAttributes));
        Equals("enabled, deploySatelliteAssemblies", deploymentAttributes);
        Assert.IsTrue(actualRequestData.MetricsCollection.Metrics.TryGetValue("VS.TestRun.LegacySettings.ExecutionAttributes", out var executionAttributes));
        Equals("hostProcessPlatform, parallelTestCount", executionAttributes);

        Assert.IsTrue(actualRequestData.MetricsCollection.Metrics.TryGetValue(TelemetryDataConstants.TestSettingsUsed, out var testSettingsUsed));
        Assert.IsFalse((bool)testSettingsUsed);
    }

    [TestMethod]
    public void RunTestsShouldCollectTelemetryForTestSettingsEmbeddedInsideRunSettings()
    {
        // Opt in the Telemetry
        Environment.SetEnvironmentVariable("VSTEST_TELEMETRY_OPTEDIN", "1");

        var payload = new TestRunRequestPayload()
        {
            Sources = new List<string>() { "a" },
            RunSettings = @"<RunSettings>
                                       <MSTest>
                                            <ForcedLegacyMode>true</ForcedLegacyMode>
                                            <SettingsFile>..\..\Foo.testsettings</SettingsFile>
                                       </MSTest>
                               </RunSettings>"
        };
        var mockProtocolConfig = new ProtocolConfig { Version = 6 };
        IRequestData? actualRequestData = null;
        var mockDiscoveryRequest = new Mock<ITestRunRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateTestRunRequest(It.IsAny<IRequestData>(), It.IsAny<TestRunCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, TestRunCriteria runCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualRequestData = requestData).Returns(mockDiscoveryRequest.Object);

        _testRequestManager = new TestRequestManager(
            CommandLineOptions.Instance,
            _mockTestPlatform.Object,
            TestRunResultAggregator.Instance,
            _mockTestPlatformEventSource.Object,
            _inferHelper,
            _mockMetricsPublisherTask,
            _mockProcessHelper.Object,
            _mockAttachmentsProcessingManager.Object,
            _mockEnvironment.Object,
            _mockEnvironmentVariableHelper.Object);

        // Act.
        _testRequestManager.RunTests(payload, new Mock<ITestHostLauncher3>().Object, new Mock<ITestRunEventsRegistrar>().Object, mockProtocolConfig);

        // Verify
        Assert.IsTrue(actualRequestData!.MetricsCollection.Metrics.TryGetValue(TelemetryDataConstants.TestSettingsUsed, out var testSettingsUsed));
        Assert.IsTrue((bool)testSettingsUsed);
    }

    [TestMethod]
    public void RunTestsShouldCollectMetrics()
    {
        // Opt in the Telemetry
        Environment.SetEnvironmentVariable("VSTEST_TELEMETRY_OPTEDIN", "1");

        var payload = new TestRunRequestPayload()
        {
            Sources = new List<string>() { "a.dll" },
            RunSettings = @"<RunSettings>
                                <RunConfiguration>
                                    <MaxCpuCount>2</MaxCpuCount>
                                    <TargetPlatform>x86</TargetPlatform>
                                    <TargetFrameworkVersion>Framework35</TargetFrameworkVersion>
                                    <DisableAppDomain>True</DisableAppDomain>
                                </RunConfiguration>
                                <MSPhoneTest>
                                  <TargetDevice>169.254.193.190</TargetDevice>
                                </MSPhoneTest>
                            </RunSettings>"
        };
        var mockProtocolConfig = new ProtocolConfig { Version = 6 };
        IRequestData? actualRequestData = null;
        var mockDiscoveryRequest = new Mock<ITestRunRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateTestRunRequest(It.IsAny<IRequestData>(), It.IsAny<TestRunCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, TestRunCriteria runCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualRequestData = requestData).Returns(mockDiscoveryRequest.Object);

        _testRequestManager = new TestRequestManager(
            CommandLineOptions.Instance,
            _mockTestPlatform.Object,
            TestRunResultAggregator.Instance,
            _mockTestPlatformEventSource.Object,
            _inferHelper,
            _mockMetricsPublisherTask,
            _mockProcessHelper.Object,
            _mockAttachmentsProcessingManager.Object,
            _mockEnvironment.Object,
            _mockEnvironmentVariableHelper.Object);

        // Act.
        _testRequestManager.RunTests(payload, new Mock<ITestHostLauncher3>().Object, new Mock<ITestRunEventsRegistrar>().Object, mockProtocolConfig);

        // Verify
        Assert.IsTrue(actualRequestData!.MetricsCollection.Metrics.TryGetValue(TelemetryDataConstants.TargetDevice, out var targetDevice));
        Assert.IsTrue(actualRequestData.MetricsCollection.Metrics.TryGetValue(TelemetryDataConstants.MaxCPUcount, out var maxcount));
        Assert.IsTrue(actualRequestData.MetricsCollection.Metrics.TryGetValue(TelemetryDataConstants.TargetPlatform, out var targetPlatform));
        Assert.IsTrue(actualRequestData.MetricsCollection.Metrics.TryGetValue(TelemetryDataConstants.DisableAppDomain, out var disableAppDomain));
        Assert.AreEqual("Other", targetDevice);
        Assert.AreEqual(2, maxcount);
        Assert.AreEqual("X86", targetPlatform.ToString());
        Assert.AreEqual(true, disableAppDomain);
    }

    [TestMethod]
    public void RunTestsWithSourcesShouldCallTestPlatformAndSucceed()
    {
        var payload = new TestRunRequestPayload()
        {
            Sources = new List<string>() { "a", "b" },
            RunSettings = DefaultRunsettings
        };

        var createRunRequestCalled = 0;
        TestRunCriteria? observedCriteria = null;
        var mockRunRequest = new Mock<ITestRunRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateTestRunRequest(It.IsAny<IRequestData>(), It.IsAny<TestRunCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, TestRunCriteria runCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) =>
            {
                createRunRequestCalled++;
                observedCriteria = runCriteria;
            }).Returns(mockRunRequest.Object);

        var mockRunEventsRegistrar = new Mock<ITestRunEventsRegistrar>();
        var mockCustomlauncher = new Mock<ITestHostLauncher3>();

        string testCaseFilterValue = "TestFilter";
        payload.TestPlatformOptions = new TestPlatformOptions { TestCaseFilter = testCaseFilterValue };
        _testRequestManager = new TestRequestManager(CommandLineOptions.Instance,
            _mockTestPlatform.Object,
            TestRunResultAggregator.Instance,
            _mockTestPlatformEventSource.Object,
            _inferHelper,
            _mockMetricsPublisherTask,
            _mockProcessHelper.Object,
            _mockAttachmentsProcessingManager.Object,
            _mockEnvironment.Object,
            _mockEnvironmentVariableHelper.Object);

        _testRequestManager.RunTests(payload, mockCustomlauncher.Object, mockRunEventsRegistrar.Object, _protocolConfig);

        Assert.AreEqual(testCaseFilterValue, observedCriteria!.TestCaseFilter, "TestCaseFilter must be set");

        Assert.AreEqual(1, createRunRequestCalled, "CreateRunRequest must be invoked only once.");
        Assert.AreEqual(2, observedCriteria.Sources!.Count(), "All Sources must be used for discovery request");
        Assert.AreEqual("a", observedCriteria.Sources!.First(), "First Source in list is incorrect");
        Assert.AreEqual("b", observedCriteria.Sources!.ElementAt(1), "Second Source in list is incorrect");

        // Check for the default value for the frequency
        Assert.AreEqual(10, observedCriteria.FrequencyOfRunStatsChangeEvent);
        mockRunEventsRegistrar.Verify(md => md.RegisterTestRunEvents(It.IsAny<ITestRunRequest>()), Times.Once);
        mockRunEventsRegistrar.Verify(md => md.UnregisterTestRunEvents(It.IsAny<ITestRunRequest>()), Times.Once);

        mockRunRequest.Verify(md => md.ExecuteAsync(), Times.Once);

        _mockTestPlatformEventSource.Verify(mt => mt.ExecutionRequestStart(), Times.Once);
        _mockTestPlatformEventSource.Verify(mt => mt.ExecutionRequestStop(), Times.Once);
    }

    [TestMethod]
    public void RunTestsMultipleCallsShouldNotRunInParallel()
    {
        var payload1 = new TestRunRequestPayload()
        {
            Sources = new List<string>() { "a" },
            RunSettings = DefaultRunsettings
        };

        var payload2 = new TestRunRequestPayload()
        {
            Sources = new List<string>() { "b" },
            RunSettings = DefaultRunsettings
        };

        var mockRunRequest = new Mock<ITestRunRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateTestRunRequest(It.IsAny<IRequestData>(), It.IsAny<TestRunCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>()))
            .Returns(mockRunRequest.Object);

        var mockRunEventsRegistrar1 = new Mock<ITestRunEventsRegistrar>();
        var mockRunEventsRegistrar2 = new Mock<ITestRunEventsRegistrar>();

        // Setup the second one to wait
        var sw = new Stopwatch();
        sw.Start();

        long run1Start = 0;
        long run1Stop = 0;
        long run2Start = 0;
        long run2Stop = 0;
        mockRunEventsRegistrar1.Setup(md => md.RegisterTestRunEvents(It.IsAny<ITestRunRequest>())).Callback(() =>
        {
            Thread.Sleep(10);
            run1Start = sw.ElapsedMilliseconds;
            Thread.Sleep(1);
        });
        mockRunEventsRegistrar1.Setup(md => md.UnregisterTestRunEvents(It.IsAny<ITestRunRequest>())).Callback(() =>
        {
            Thread.Sleep(10);
            run1Stop = sw.ElapsedMilliseconds;
            Thread.Sleep(10);
        });

        mockRunEventsRegistrar2.Setup(md => md.RegisterTestRunEvents(It.IsAny<ITestRunRequest>())).Callback(() =>
        {
            Thread.Sleep(10);
            run2Start = sw.ElapsedMilliseconds;
            Thread.Sleep(10);
        });
        mockRunEventsRegistrar2.Setup(md => md.UnregisterTestRunEvents(It.IsAny<ITestRunRequest>())).Callback(() =>
        {
            Thread.Sleep(10);
            run2Stop = sw.ElapsedMilliseconds;
        });

        var mockCustomlauncher = new Mock<ITestHostLauncher3>();
        var task1 = Task.Run(() => _testRequestManager.RunTests(payload1, mockCustomlauncher.Object, mockRunEventsRegistrar1.Object, _protocolConfig));
        var task2 = Task.Run(() => _testRequestManager.RunTests(payload2, mockCustomlauncher.Object, mockRunEventsRegistrar2.Object, _protocolConfig));

        Task.WaitAll(task1, task2);

        if (run1Start < run2Start)
        {
            Assert.IsTrue((run2Stop > run2Start)
                          && (run2Start > run1Stop)
                          && (run1Stop > run1Start));
        }
        else
        {
            Assert.IsTrue((run1Stop > run1Start)
                          && (run1Start > run2Stop)
                          && (run2Stop > run2Start));
        }
    }

    [TestMethod]
    public void RunTestsShouldPublishMetrics()
    {
        var payload = new TestRunRequestPayload()
        {
            Sources = new List<string>() { "a", "b" },
            RunSettings = DefaultRunsettings
        };

        var mockRunEventsRegistrar = new Mock<ITestRunEventsRegistrar>();
        var mockCustomlauncher = new Mock<ITestHostLauncher3>();

        _testRequestManager.RunTests(payload, mockCustomlauncher.Object, mockRunEventsRegistrar.Object, _protocolConfig);

        _mockMetricsPublisher.Verify(mp => mp.PublishMetrics(TelemetryDataConstants.TestExecutionCompleteEvent, It.IsAny<IDictionary<string, object?>>()), Times.Once);
    }

    // TODO: add tests in design mode and executor that they are handling all the exceptions properly including printing inner exception.

    [TestMethod]
    public void RunTestsIfThrowsTestPlatformExceptionShouldThrowOut()
    {
        Assert.ThrowsException<TestPlatformException>(() => RunTestsIfThrowsExceptionShouldThrowOut(new TestPlatformException("HelloWorld")));
    }

    [TestMethod]
    public void RunTestsIfThrowsSettingsExceptionShouldThrowOut()
    {
        Assert.ThrowsException<SettingsException>(() => RunTestsIfThrowsExceptionShouldThrowOut(new SettingsException("HelloWorld")));
    }

    [TestMethod]
    public void RunTestsIfThrowsInvalidOperationExceptionShouldThrowOut()
    {
        Assert.ThrowsException<InvalidOperationException>(() => RunTestsIfThrowsExceptionShouldThrowOut(new InvalidOperationException("HelloWorld")));
    }

    [TestMethod]
    public void RunTestsIfThrowsExceptionShouldThrowOut()
    {
        Assert.ThrowsException<NotImplementedException>(() => RunTestsIfThrowsExceptionShouldThrowOut(new NotImplementedException("HelloWorld")));
    }

    [TestMethod]
    public void DiscoverTestsIfThrowsTestPlatformExceptionShouldThrowOut()
    {
        Assert.ThrowsException<TestPlatformException>(() => DiscoverTestsIfThrowsExceptionShouldThrowOut(new TestPlatformException("HelloWorld")));
    }

    [TestMethod]
    public void DiscoverTestsIfThrowsSettingsExceptionShouldThrowOut()
    {
        Assert.ThrowsException<SettingsException>(() => DiscoverTestsIfThrowsExceptionShouldThrowOut(new SettingsException("HelloWorld")));
    }

    [TestMethod]
    public void DiscoverTestsIfThrowsInvalidOperationExceptionShouldThrowOut()
    {
        Assert.ThrowsException<InvalidOperationException>(() => DiscoverTestsIfThrowsExceptionShouldThrowOut(new InvalidOperationException("HelloWorld")));
    }

    [TestMethod]
    public void DiscoverTestsIfThrowsExceptionShouldThrowOut()
    {
        Assert.ThrowsException<NotImplementedException>(() => DiscoverTestsIfThrowsExceptionShouldThrowOut(new NotImplementedException("HelloWorld")));
    }

    [DataTestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void DiscoverTestsShouldUpdateDesignModeAndCollectSourceInformation(bool designModeValue)
    {
        var runsettings = "<RunSettings><RunConfiguration><TargetFrameworkVersion>.NETFramework,Version=v4.5</TargetFrameworkVersion></RunConfiguration></RunSettings>";
        var discoveryPayload = CreateDiscoveryPayload(runsettings);
        _commandLineOptions.IsDesignMode = designModeValue;

        _testRequestManager.DiscoverTests(discoveryPayload, new Mock<ITestDiscoveryEventsRegistrar>().Object, _protocolConfig);

        var designmode = $"<DesignMode>{designModeValue}</DesignMode>";
        _mockTestPlatform.Verify(
            tp => tp.CreateDiscoveryRequest(It.IsAny<IRequestData>(), It.Is<DiscoveryCriteria>(dc => dc.RunSettings!.Contains(designmode)), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>()));

        var collectSourceInformation = $"<CollectSourceInformation>{designModeValue}</CollectSourceInformation>";
        _mockTestPlatform.Verify(
            tp => tp.CreateDiscoveryRequest(It.IsAny<IRequestData>(), It.Is<DiscoveryCriteria>(dc => dc.RunSettings!.Contains(collectSourceInformation)), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>()));
    }

    [TestMethod]
    public void DiscoverTestsShouldNotUpdateDesignModeIfUserHasSetDesignModeInRunSettings()
    {
        var runsettings = "<RunSettings><RunConfiguration><DesignMode>False</DesignMode><TargetFrameworkVersion>.NETFramework,Version=v4.5</TargetFrameworkVersion></RunConfiguration></RunSettings>";
        var discoveryPayload = CreateDiscoveryPayload(runsettings);
        _commandLineOptions.IsDesignMode = true;

        _testRequestManager.DiscoverTests(discoveryPayload, new Mock<ITestDiscoveryEventsRegistrar>().Object, _protocolConfig);

        var designmode = "<DesignMode>False</DesignMode>";
        _mockTestPlatform.Verify(
            tp => tp.CreateDiscoveryRequest(It.IsAny<IRequestData>(), It.Is<DiscoveryCriteria>(dc => dc.RunSettings!.Contains(designmode)), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>()));
    }

    [DataTestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void RunTestsShouldUpdateDesignModeIfRunnerIsInDesignMode(bool designModeValue)
    {
        var runsettings =
            "<RunSettings><RunConfiguration><TargetFrameworkVersion>.NETFramework,Version=v4.5</TargetFrameworkVersion></RunConfiguration></RunSettings>";
        var payload = new TestRunRequestPayload
        {
            RunSettings = runsettings,
            Sources = new List<string> { "c:\\testproject.dll" }
        };
        _commandLineOptions.IsDesignMode = designModeValue;

        _testRequestManager.RunTests(payload, new Mock<ITestHostLauncher3>().Object, new Mock<ITestRunEventsRegistrar>().Object, _protocolConfig);

        var designmode = $"<DesignMode>{designModeValue}</DesignMode>";
        _mockTestPlatform.Verify(tp => tp.CreateTestRunRequest(It.IsAny<IRequestData>(), It.Is<TestRunCriteria>(rc => rc.TestRunSettings!.Contains(designmode)), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>()));
    }

    [DataTestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void DiscoverTestsShouldNotUpdateCollectSourceInformationIfUserHasSetItInRunSettings(bool val)
    {
        var runsettings = $"<RunSettings><RunConfiguration><CollectSourceInformation>{val}</CollectSourceInformation></RunConfiguration></RunSettings>";
        var discoveryPayload = CreateDiscoveryPayload(runsettings);

        _testRequestManager.DiscoverTests(discoveryPayload, new Mock<ITestDiscoveryEventsRegistrar>().Object, _protocolConfig);

        var collectSourceInformation = $"<CollectSourceInformation>{val}</CollectSourceInformation>";
        _mockTestPlatform.Verify(
            tp => tp.CreateDiscoveryRequest(It.IsAny<IRequestData>(), It.Is<DiscoveryCriteria>(dc => dc.RunSettings!.Contains(collectSourceInformation)), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>()));
    }

    [TestMethod]
    public void RunTestsShouldShouldUpdateFrameworkAndPlatformIfNotSpecifiedInDesignMode()
    {
        var payload = new TestRunRequestPayload()
        {
            Sources = new List<string>() { "a.dll" },
            RunSettings =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                     </RunConfiguration>
                </RunSettings>"
        };

        _commandLineOptions.IsDesignMode = true;
        _mockAssemblyMetadataProvider.Setup(a => a.GetArchitecture(It.IsAny<string>()))
            .Returns(Architecture.ARM);
        _mockAssemblyMetadataProvider.Setup(a => a.GetFrameworkName(It.IsAny<string>()))
            .Returns(new FrameworkName(Constants.DotNetFramework46));
        TestRunCriteria? actualTestRunCriteria = null;
        var mockTestRunRequest = new Mock<ITestRunRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateTestRunRequest(It.IsAny<IRequestData>(), It.IsAny<TestRunCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, TestRunCriteria runCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualTestRunCriteria = runCriteria).Returns(mockTestRunRequest.Object);

        _testRequestManager.RunTests(payload, new Mock<ITestHostLauncher3>().Object, new Mock<ITestRunEventsRegistrar>().Object, _protocolConfig);

        _mockAssemblyMetadataProvider.Verify(a => a.GetArchitecture(It.IsAny<string>()));
        _mockAssemblyMetadataProvider.Verify(a => a.GetFrameworkName(It.IsAny<string>()));

        Assert.IsTrue(actualTestRunCriteria!.TestRunSettings!.Contains(Constants.DotNetFramework46));
        Assert.IsTrue(actualTestRunCriteria.TestRunSettings.Contains(nameof(Architecture.ARM)));

    }

    [TestMethod]
    public void RunTestsShouldNotUpdateFrameworkAndPlatformIfSpecifiedInDesignMode()
    {
        var payload = new TestRunRequestPayload()
        {
            Sources = new List<string>() { "a.dll" },
            // specify architecture and framework
            RunSettings =
                $@"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                         <TargetFrameworkVersion>{Constants.DotNetFramework46}</TargetFrameworkVersion>
                         <TargetPlatform>{Architecture.ARM}</TargetPlatform>
                     </RunConfiguration>
                </RunSettings>"
        };

        _commandLineOptions.IsDesignMode = true;
        _mockAssemblyMetadataProvider.Setup(a => a.GetArchitecture(It.IsAny<string>()))
            .Returns(Architecture.X86);
        _mockAssemblyMetadataProvider.Setup(a => a.GetFrameworkName(It.IsAny<string>()))
            .Returns(new FrameworkName(Constants.DotNetFramework451));
        TestRunCriteria? actualTestRunCriteria = null;
        var mockTestRunRequest = new Mock<ITestRunRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateTestRunRequest(It.IsAny<IRequestData>(), It.IsAny<TestRunCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, TestRunCriteria runCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualTestRunCriteria = runCriteria).Returns(mockTestRunRequest.Object);

        _testRequestManager.RunTests(payload, new Mock<ITestHostLauncher3>().Object, new Mock<ITestRunEventsRegistrar>().Object, _protocolConfig);

        // infer them so we can print warning when dlls are not compatible with runsettings
        _mockAssemblyMetadataProvider.Verify(a => a.GetArchitecture(It.IsAny<string>()), Times.Once);
        _mockAssemblyMetadataProvider.Verify(a => a.GetFrameworkName(It.IsAny<string>()), Times.Once);

        // but don't update runsettings because we want to keep what user specified
        Assert.IsTrue(actualTestRunCriteria!.TestRunSettings!.Contains(Constants.DotNetFramework46));
        Assert.IsTrue(actualTestRunCriteria!.TestRunSettings.Contains(nameof(Architecture.ARM)));
    }

    [TestMethod]
    [DataRow("x86")]
    [DataRow("X86")]
    [DataRow("ARM")]
    [DataRow("aRm")]
    public void RunTestsShouldNotUpdatePlatformIfSpecifiedInDesignMode(string targetPlatform)
    {
        var payload = new TestRunRequestPayload()
        {
            Sources = new List<string>() { "a.dll" },
            // Specify platform
            RunSettings =
                $@"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                         <TargetPlatform>{targetPlatform}</TargetPlatform>
                     </RunConfiguration>
                </RunSettings>"
        };

        _commandLineOptions.IsDesignMode = true;
        _mockAssemblyMetadataProvider.Setup(a => a.GetArchitecture(It.IsAny<string>()))
            .Returns(Architecture.X86);
        _mockAssemblyMetadataProvider.Setup(a => a.GetFrameworkName(It.IsAny<string>()))
            .Returns(new FrameworkName(Constants.DotNetFramework451));
        TestRunCriteria? actualTestRunCriteria = null;
        var mockTestRunRequest = new Mock<ITestRunRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateTestRunRequest(It.IsAny<IRequestData>(), It.IsAny<TestRunCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, TestRunCriteria runCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualTestRunCriteria = runCriteria).Returns(mockTestRunRequest.Object);

        _testRequestManager.RunTests(payload, new Mock<ITestHostLauncher3>().Object, new Mock<ITestRunEventsRegistrar>().Object, _protocolConfig);

        // infer platform and framework so we can print warnings when dlls are not compatible with runsettings
        _mockAssemblyMetadataProvider.Verify(a => a.GetArchitecture(It.IsAny<string>()), Times.Once);
        _mockAssemblyMetadataProvider.Verify(a => a.GetFrameworkName(It.IsAny<string>()), Times.Once);

        // don't update it in runsettings to keep what user provided
        Assert.IsTrue(actualTestRunCriteria!.TestRunSettings!.Contains(targetPlatform));
    }

    [TestMethod]
    public void RunTestsShouldUpdateFrameworkAndPlatformInCommandLineScenarios()
    {
        var payload = new TestRunRequestPayload()
        {
            Sources = new List<string>() { "a.dll" },
            RunSettings =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                     </RunConfiguration>
                </RunSettings>"
        };

        _commandLineOptions.IsDesignMode = false;
        _mockAssemblyMetadataProvider.Setup(a => a.GetArchitecture(It.IsAny<string>()))
            .Returns(Architecture.ARM);
        _mockAssemblyMetadataProvider.Setup(a => a.GetFrameworkName(It.IsAny<string>()))
            .Returns(new FrameworkName(Constants.DotNetFramework46));
        TestRunCriteria? actualTestRunCriteria = null;
        var mockTestRunRequest = new Mock<ITestRunRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateTestRunRequest(It.IsAny<IRequestData>(), It.IsAny<TestRunCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, TestRunCriteria runCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualTestRunCriteria = runCriteria).Returns(mockTestRunRequest.Object);

        _testRequestManager.RunTests(payload, new Mock<ITestHostLauncher3>().Object, new Mock<ITestRunEventsRegistrar>().Object, _protocolConfig);

        _mockAssemblyMetadataProvider.Verify(a => a.GetArchitecture(It.IsAny<string>()));
        _mockAssemblyMetadataProvider.Verify(a => a.GetFrameworkName(It.IsAny<string>()));

        Assert.IsTrue(actualTestRunCriteria!.TestRunSettings!.Contains(Constants.DotNetFramework46));
        Assert.IsTrue(actualTestRunCriteria.TestRunSettings.Contains(nameof(Architecture.ARM)));
    }

    [TestMethod]
    public void RunTestsShouldNotpdateFrameworkAndPlatformInRunsettingsIfSpecifiedByCommandLine()
    {
        var payload = new TestRunRequestPayload()
        {
            Sources = new List<string>() { "a.dll" },
            RunSettings =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                     </RunConfiguration>
                </RunSettings>"
        };

        _commandLineOptions.IsDesignMode = false;

        // specify architecture and framework
        _commandLineOptions.TargetArchitecture = Architecture.X86;
        _commandLineOptions.TargetFrameworkVersion = Framework.DefaultFramework;

        _mockAssemblyMetadataProvider.Setup(a => a.GetArchitecture(It.IsAny<string>()))
            .Returns(Architecture.ARM);
        _mockAssemblyMetadataProvider.Setup(a => a.GetFrameworkName(It.IsAny<string>()))
            .Returns(new FrameworkName(Constants.DotNetFramework46));
        TestRunCriteria? actualTestRunCriteria = null;
        var mockTestRunRequest = new Mock<ITestRunRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateTestRunRequest(It.IsAny<IRequestData>(), It.IsAny<TestRunCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, TestRunCriteria runCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualTestRunCriteria = runCriteria).Returns(mockTestRunRequest.Object);

        _testRequestManager.RunTests(payload, new Mock<ITestHostLauncher3>().Object, new Mock<ITestRunEventsRegistrar>().Object, _protocolConfig);

        // infer them so we can print warnings when the assemblies are not compatible
        _mockAssemblyMetadataProvider.Verify(a => a.GetArchitecture(It.IsAny<string>()), Times.Once);
        _mockAssemblyMetadataProvider.Verify(a => a.GetFrameworkName(It.IsAny<string>()), Times.Once);

        // but don't update them in runsettings so we keep what user specified
        Assert.IsFalse(actualTestRunCriteria!.TestRunSettings!.Contains(Constants.DotNetFramework46));
        Assert.IsFalse(actualTestRunCriteria.TestRunSettings.Contains(nameof(Architecture.ARM)));
    }

    [TestMethod]
    public void RunTestsWithTestCasesShouldUpdateFrameworkAndPlatformIfNotSpecifiedInDesignMode()
    {
        var actualSources = new List<string>() { "1.dll", "2.dll" };
        var payload = new TestRunRequestPayload()
        {
            TestCases = new List<TestCase>()
            {
                new() { Source = actualSources[0] },
                new() { Source = actualSources[0] },
                new() { Source = actualSources[1] }
            },
            RunSettings =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                     </RunConfiguration>
                </RunSettings>"
        };

        List<string> archSources = new(), fxSources = new();

        _commandLineOptions.IsDesignMode = true;
        _mockAssemblyMetadataProvider.Setup(a => a.GetArchitecture(It.IsAny<string>())).Callback<string>(archSources.Add)
            .Returns(Architecture.ARM);
        _mockAssemblyMetadataProvider.Setup(a => a.GetFrameworkName(It.IsAny<string>())).Callback<string>(fxSources.Add)
            .Returns(new FrameworkName(Constants.DotNetFramework46));
        TestRunCriteria? actualTestRunCriteria = null;
        var mockTestRunRequest = new Mock<ITestRunRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateTestRunRequest(It.IsAny<IRequestData>(), It.IsAny<TestRunCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, TestRunCriteria runCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualTestRunCriteria = runCriteria).Returns(mockTestRunRequest.Object);

        _testRequestManager.RunTests(payload, new Mock<ITestHostLauncher3>().Object, new Mock<ITestRunEventsRegistrar>().Object, _protocolConfig);

        _mockAssemblyMetadataProvider.Verify(a => a.GetArchitecture(It.IsAny<string>()));
        _mockAssemblyMetadataProvider.Verify(a => a.GetFrameworkName(It.IsAny<string>()));

        Assert.IsTrue(actualTestRunCriteria!.TestRunSettings!.Contains(Constants.DotNetFramework46));
        Assert.IsTrue(actualTestRunCriteria.TestRunSettings.Contains(nameof(Architecture.ARM)));
        CollectionAssert.AreEqual(actualSources, archSources);
        CollectionAssert.AreEqual(actualSources, fxSources);
    }

    [TestMethod]
    public void RunTestShouldThrowExceptionIfRunSettingWithDcHasTestSettingsInIt()
    {
        var settingXml = @"<RunSettings>
                                    <MSTest>
                                        <SettingsFile>C:\temp.testsettings</SettingsFile>
                                        <ForcedLegacyMode>true</ForcedLegacyMode>
                                    </MSTest>
                                    <DataCollectionRunSettings>
                                        <DataCollectors>
                                            <DataCollector friendlyName=""DummyDataCollector1"">
                                            </DataCollector>
                                            <DataCollector friendlyName=""DummyDataCollector2"">
                                            </DataCollector>
                                        </DataCollectors>
                                    </DataCollectionRunSettings>
                                </RunSettings>";

        var payload = new TestRunRequestPayload()
        {
            Sources = new List<string>() { "a.dll" },
            RunSettings = settingXml
        };

        _commandLineOptions.EnableCodeCoverage = false;
        bool exceptionThrown = false;

        try
        {
            _testRequestManager.RunTests(payload, new Mock<ITestHostLauncher3>().Object, new Mock<ITestRunEventsRegistrar>().Object, _protocolConfig);
        }
        catch (SettingsException ex)
        {
            exceptionThrown = true;
            Assert.IsTrue(ex.Message.Contains(@"<SettingsFile>C:\temp.testsettings</SettingsFile>"), ex.Message);
        }

        Assert.IsTrue(exceptionThrown, "Initialize should throw exception");
    }

    [TestMethod]
    public void RunTestShouldThrowExceptionIfRunSettingWithDcHasTestSettingsAndEnableCodeCoverageTrue()
    {
        var settingXml = @"<RunSettings>
                                    <MSTest>
                                        <SettingsFile>C:\temp.testsettings</SettingsFile>
                                        <ForcedLegacyMode>true</ForcedLegacyMode>
                                    </MSTest>
                                    <DataCollectionRunSettings>
                                        <DataCollectors>
                                            <DataCollector friendlyName=""DummyDataCollector1"">
                                            </DataCollector>
                                            <DataCollector friendlyName=""DummyDataCollector2"">
                                            </DataCollector>
                                        </DataCollectors>
                                    </DataCollectionRunSettings>
                                </RunSettings>";

        var payload = new TestRunRequestPayload()
        {
            Sources = new List<string>() { "a.dll" },
            RunSettings = settingXml
        };

        _commandLineOptions.EnableCodeCoverage = true;
        bool exceptionThrown = false;

        try
        {
            _testRequestManager.RunTests(payload, new Mock<ITestHostLauncher3>().Object, new Mock<ITestRunEventsRegistrar>().Object, _protocolConfig);
        }
        catch (SettingsException ex)
        {
            exceptionThrown = true;
            Assert.IsTrue(ex.Message.Contains(@"<SettingsFile>C:\temp.testsettings</SettingsFile>"), ex.Message);
        }

        Assert.IsTrue(exceptionThrown, "Initialize should throw exception");
    }

    [TestMethod]
    public void RunTestShouldNotThrowExceptionIfRunSettingHasCodeCoverageDcAndTestSettingsInItWithEnableCoverageTrue()
    {
        var payload = new TestRunRequestPayload()
        {
            Sources = new List<string>() { "a.dll" },
            RunSettings = @"<RunSettings>
                                    <MSTest>
                                        <SettingsFile>C:\temp.testsettings</SettingsFile>
                                        <ForcedLegacyMode>true</ForcedLegacyMode>
                                    </MSTest>
                                    <DataCollectionRunSettings>
                                        <DataCollectors>
                                            <DataCollector friendlyName=""Code Coverage"">
                                            </DataCollector>
                                        </DataCollectors>
                                    </DataCollectionRunSettings>
                                </RunSettings>"
        };

        _commandLineOptions.EnableCodeCoverage = true;
        _testRequestManager.RunTests(payload, new Mock<ITestHostLauncher3>().Object, new Mock<ITestRunEventsRegistrar>().Object, _protocolConfig);
    }

    [TestMethod]
    public void RunTestsShouldAddConsoleLoggerInRunSettingsInNonDesignMode()
    {
        var payload = new TestRunRequestPayload()
        {
            Sources = new List<string>() { "a.dll" },
            RunSettings =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                     </RunConfiguration>
                </RunSettings>"
        };

        _commandLineOptions.IsDesignMode = false;
        TestRunCriteria? actualTestRunCriteria = null;
        var mockTestRunRequest = new Mock<ITestRunRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateTestRunRequest(It.IsAny<IRequestData>(), It.IsAny<TestRunCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, TestRunCriteria runCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualTestRunCriteria = runCriteria).Returns(mockTestRunRequest.Object);

        _testRequestManager.RunTests(payload, new Mock<ITestHostLauncher3>().Object, new Mock<ITestRunEventsRegistrar>().Object, _protocolConfig);

        var loggerSettingsList = XmlRunSettingsUtilities.GetLoggerRunSettings(actualTestRunCriteria!.TestRunSettings)!.LoggerSettingsList;
        Assert.AreEqual(1, loggerSettingsList.Count);
        Assert.AreEqual("Console", loggerSettingsList[0].FriendlyName);
        Assert.IsNotNull(loggerSettingsList[0].AssemblyQualifiedName);
        Assert.IsNotNull(loggerSettingsList[0].CodeBase);
    }

    [TestMethod]
    public void RunTestsShouldAddConsoleLoggerInRunSettingsIfDesignModeSetFalseInRunSettings()
    {
        var payload = new TestRunRequestPayload()
        {
            Sources = new List<string>() { "a.dll" },
            RunSettings =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <DesignMode>False</DesignMode>
                     </RunConfiguration>
                     <LoggerRunSettings>
                       <Loggers>
                         <Logger friendlyName=""blabla"">
                           <Configuration>
                             <Key1>Value1</Key1>
                           </Configuration>
                         </Logger>
                       </Loggers>
                     </LoggerRunSettings>
                </RunSettings>"
        };

        _commandLineOptions.IsDesignMode = true;
        TestRunCriteria? actualTestRunCriteria = null;
        var mockTestRunRequest = new Mock<ITestRunRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateTestRunRequest(It.IsAny<IRequestData>(), It.IsAny<TestRunCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, TestRunCriteria runCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualTestRunCriteria = runCriteria).Returns(mockTestRunRequest.Object);
        _testRequestManager.RunTests(payload, new Mock<ITestHostLauncher3>().Object, new Mock<ITestRunEventsRegistrar>().Object, _protocolConfig);

        var loggerSettingsList = XmlRunSettingsUtilities.GetLoggerRunSettings(actualTestRunCriteria!.TestRunSettings)!.LoggerSettingsList;
        Assert.AreEqual(2, loggerSettingsList.Count);
        Assert.IsNotNull(loggerSettingsList[0].Configuration);
        Assert.AreEqual("blabla", loggerSettingsList[0].FriendlyName);
        Assert.AreEqual("Console", loggerSettingsList[1].FriendlyName);
        Assert.IsNotNull(loggerSettingsList[1].AssemblyQualifiedName);
        Assert.IsNotNull(loggerSettingsList[1].CodeBase);
    }

    [TestMethod]
    public void DiscoverTestsShouldAddConsoleLoggerInRunSettingsIfDesignModeSetFalseInRunSettings()
    {
        var payload = new DiscoveryRequestPayload()
        {
            Sources = new List<string>() { "a.dll" },
            RunSettings =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <DesignMode>False</DesignMode>
                     </RunConfiguration>
                     <LoggerRunSettings>
                       <Loggers>
                         <Logger friendlyName=""blabla"">
                           <Configuration>
                             <Key1>Value1</Key1>
                           </Configuration>
                         </Logger>
                       </Loggers>
                     </LoggerRunSettings>
                </RunSettings>"
        };
        _commandLineOptions.IsDesignMode = true;
        DiscoveryCriteria? actualDiscoveryCriteria = null;
        var mockDiscoveryRequest = new Mock<IDiscoveryRequest>();
        _mockTestPlatform
            .Setup(mt => mt.CreateDiscoveryRequest(It.IsAny<IRequestData>(), It.IsAny<DiscoveryCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>()))
            .Callback(
                (IRequestData requestData, DiscoveryCriteria discoveryCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualDiscoveryCriteria = discoveryCriteria).Returns(mockDiscoveryRequest.Object);

        _testRequestManager.DiscoverTests(payload,
            new Mock<ITestDiscoveryEventsRegistrar>().Object, _protocolConfig);

        var loggerSettingsList = XmlRunSettingsUtilities.GetLoggerRunSettings(actualDiscoveryCriteria!.RunSettings)!.LoggerSettingsList;
        Assert.AreEqual(2, loggerSettingsList.Count);
        Assert.IsNotNull(loggerSettingsList[0].Configuration);
        Assert.AreEqual("blabla", loggerSettingsList[0].FriendlyName);
        Assert.AreEqual("Console", loggerSettingsList[1].FriendlyName);
        Assert.IsNotNull(loggerSettingsList[1].AssemblyQualifiedName);
        Assert.IsNotNull(loggerSettingsList[1].CodeBase);
    }

    [TestMethod]
    public void RunTestsShouldNotAddConsoleLoggerInRunSettingsInDesignMode()
    {
        var payload = new TestRunRequestPayload()
        {
            Sources = new List<string>() { "a.dll" },
            RunSettings =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <DesignMode>True</DesignMode>
                     </RunConfiguration>
                </RunSettings>"
        };

        _commandLineOptions.IsDesignMode = false;
        TestRunCriteria? actualTestRunCriteria = null;
        var mockTestRunRequest = new Mock<ITestRunRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateTestRunRequest(It.IsAny<IRequestData>(), It.IsAny<TestRunCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, TestRunCriteria runCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualTestRunCriteria = runCriteria).Returns(mockTestRunRequest.Object);
        _testRequestManager.RunTests(payload, new Mock<ITestHostLauncher3>().Object, new Mock<ITestRunEventsRegistrar>().Object, _protocolConfig);

        Assert.IsFalse(actualTestRunCriteria!.TestRunSettings!.Contains("LoggerRunSettings"));
    }

    [TestMethod]
    public void DiscoverTestsShouldAddConsoleLoggerInRunSettingsInNonDesignMode()
    {
        var payload = new DiscoveryRequestPayload()
        {
            Sources = new List<string>() { "a.dll" },
            RunSettings =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                     </RunConfiguration>
                </RunSettings>"
        };
        _commandLineOptions.IsDesignMode = false;
        DiscoveryCriteria? actualDiscoveryCriteria = null;
        var mockDiscoveryRequest = new Mock<IDiscoveryRequest>();
        _mockTestPlatform
            .Setup(mt => mt.CreateDiscoveryRequest(It.IsAny<IRequestData>(), It.IsAny<DiscoveryCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>()))
            .Callback(
                (IRequestData requestData, DiscoveryCriteria discoveryCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualDiscoveryCriteria = discoveryCriteria).Returns(mockDiscoveryRequest.Object);

        _testRequestManager.DiscoverTests(payload,
            new Mock<ITestDiscoveryEventsRegistrar>().Object, _protocolConfig);

        var loggerSettingsList = XmlRunSettingsUtilities.GetLoggerRunSettings(actualDiscoveryCriteria!.RunSettings)!.LoggerSettingsList;
        Assert.AreEqual(1, loggerSettingsList.Count);
        Assert.AreEqual("Console", loggerSettingsList[0].FriendlyName);
        Assert.IsNotNull(loggerSettingsList[0].AssemblyQualifiedName);
        Assert.IsNotNull(loggerSettingsList[0].CodeBase);
    }

    [TestMethod]
    public void DiscoverTestsShouldNotAddConsoleLoggerInRunSettingsInDesignMode()
    {
        var payload = new DiscoveryRequestPayload()
        {
            Sources = new List<string>() { "a.dll" },
            RunSettings =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <DesignMode>True</DesignMode>
                     </RunConfiguration>
                </RunSettings>"
        };
        _commandLineOptions.IsDesignMode = false;
        DiscoveryCriteria? actualDiscoveryCriteria = null;
        var mockDiscoveryRequest = new Mock<IDiscoveryRequest>();
        _mockTestPlatform
            .Setup(mt => mt.CreateDiscoveryRequest(It.IsAny<IRequestData>(), It.IsAny<DiscoveryCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>()))
            .Callback(
                (IRequestData requestData, DiscoveryCriteria discoveryCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualDiscoveryCriteria = discoveryCriteria).Returns(mockDiscoveryRequest.Object);

        _testRequestManager.DiscoverTests(payload,
            new Mock<ITestDiscoveryEventsRegistrar>().Object, _protocolConfig);

        Assert.IsFalse(actualDiscoveryCriteria!.RunSettings!.Contains("LoggerRunSettings"));
    }

    [TestMethod]
    public void RunTestsShouldOverrideOnlyAssemblyNameIfConsoleLoggerAlreadyPresentInNonDesignMode()
    {
        var payload = new TestRunRequestPayload()
        {
            Sources = new List<string>() { "a.dll" },
            RunSettings =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <DesignMode>False</DesignMode>
                     </RunConfiguration>
                     <LoggerRunSettings>
                       <Loggers>
                         <Logger friendlyName=""blabla"">
                           <Configuration>
                             <Key1>Value1</Key1>
                           </Configuration>
                         </Logger>
                         <Logger friendlyName=""console"" uri=""logger://tempconsoleUri"" assemblyQualifiedName=""tempAssemblyName"" codeBase=""tempCodeBase"">
                           <Configuration>
                             <Key1>Value1</Key1>
                           </Configuration>
                         </Logger>
                       </Loggers>
                     </LoggerRunSettings>
                </RunSettings>"
        };

        _commandLineOptions.IsDesignMode = false;
        TestRunCriteria? actualTestRunCriteria = null;
        var mockTestRunRequest = new Mock<ITestRunRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateTestRunRequest(It.IsAny<IRequestData>(), It.IsAny<TestRunCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, TestRunCriteria runCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualTestRunCriteria = runCriteria).Returns(mockTestRunRequest.Object);
        _testRequestManager.RunTests(payload, new Mock<ITestHostLauncher3>().Object, new Mock<ITestRunEventsRegistrar>().Object, _protocolConfig);

        var loggerSettingsList = XmlRunSettingsUtilities.GetLoggerRunSettings(actualTestRunCriteria!.TestRunSettings)!.LoggerSettingsList;
        Assert.AreEqual(2, loggerSettingsList.Count);
        Assert.IsNotNull(loggerSettingsList[0].Configuration);
        Assert.AreEqual("blabla", loggerSettingsList[0].FriendlyName);
        Assert.AreEqual("console", loggerSettingsList[1].FriendlyName);
        Assert.AreEqual(new Uri("logger://tempconsoleUri").ToString(), loggerSettingsList[1].Uri!.ToString());
        Assert.AreNotEqual("tempAssemblyName", loggerSettingsList[1].AssemblyQualifiedName);
        Assert.AreNotEqual("tempCodeBase", loggerSettingsList[1].CodeBase);
        Assert.IsTrue(loggerSettingsList[1].Configuration!.InnerXml.Contains("Value1"));
        Assert.IsNotNull(loggerSettingsList[1].AssemblyQualifiedName);
        Assert.IsNotNull(loggerSettingsList[1].CodeBase);
    }

    [TestMethod]
    public void DiscoverTestsShouldOverrideOnlyAssemblyNameIfConsoleLoggerAlreadyPresentInNonDesignMode()
    {
        var payload = new DiscoveryRequestPayload()
        {
            Sources = new List<string>() { "a.dll" },
            RunSettings =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <DesignMode>False</DesignMode>
                     </RunConfiguration>
                     <LoggerRunSettings>
                       <Loggers>
                         <Logger friendlyName=""blabla"">
                           <Configuration>
                             <Key1>Value1</Key1>
                           </Configuration>
                         </Logger>
                         <Logger friendlyName=""consoleTemp"" uri=""logger://Microsoft/TestPlatform/ConsoleLogger/v1"" assemblyQualifiedName=""tempAssemblyName"">
                           <Configuration>
                             <Key1>Value1</Key1>
                           </Configuration>
                         </Logger>
                       </Loggers>
                     </LoggerRunSettings>
                </RunSettings>"
        };
        _commandLineOptions.IsDesignMode = false;
        DiscoveryCriteria? actualDiscoveryCriteria = null;
        var mockDiscoveryRequest = new Mock<IDiscoveryRequest>();
        _mockTestPlatform
            .Setup(mt => mt.CreateDiscoveryRequest(It.IsAny<IRequestData>(), It.IsAny<DiscoveryCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>()))
            .Callback(
                (IRequestData requestData, DiscoveryCriteria discoveryCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualDiscoveryCriteria = discoveryCriteria).Returns(mockDiscoveryRequest.Object);

        _testRequestManager.DiscoverTests(payload,
            new Mock<ITestDiscoveryEventsRegistrar>().Object, _protocolConfig);

        var loggerSettingsList = XmlRunSettingsUtilities.GetLoggerRunSettings(actualDiscoveryCriteria!.RunSettings)!.LoggerSettingsList;
        Assert.AreEqual(2, loggerSettingsList.Count);
        Assert.IsNotNull(loggerSettingsList[0].Configuration);
        Assert.AreEqual("blabla", loggerSettingsList[0].FriendlyName);
        Assert.AreEqual("consoleTemp", loggerSettingsList[1].FriendlyName);
        Assert.AreEqual(new Uri("logger://Microsoft/TestPlatform/ConsoleLogger/v1").ToString(), loggerSettingsList[1].Uri!.ToString());
        Assert.AreNotEqual("tempAssemblyName", loggerSettingsList[1].AssemblyQualifiedName);
        Assert.AreNotEqual("tempAssemblyName", loggerSettingsList[1].CodeBase);
        Assert.IsTrue(loggerSettingsList[1].Configuration!.InnerXml.Contains("Value1"));
        Assert.IsNotNull(loggerSettingsList[1].AssemblyQualifiedName);
        Assert.IsNotNull(loggerSettingsList[1].CodeBase);
    }

    [TestMethod]
    public void RunTestsShouldOverrideOnlyAssemblyNameIfConsoleLoggerAlreadyPresentInDesignMode()
    {
        var payload = new TestRunRequestPayload()
        {
            Sources = new List<string>() { "a.dll" },
            RunSettings =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <DesignMode>True</DesignMode>
                     </RunConfiguration>
                     <LoggerRunSettings>
                       <Loggers>
                         <Logger friendlyName=""blabla"">
                           <Configuration>
                             <Key1>Value1</Key1>
                           </Configuration>
                         </Logger>
                         <Logger friendlyName=""console"" uri=""logger://tempconsoleUri"" assemblyQualifiedName=""tempAssemblyName"" codeBase=""tempCodeBase"">
                           <Configuration>
                             <Key1>Value1</Key1>
                           </Configuration>
                         </Logger>
                       </Loggers>
                     </LoggerRunSettings>
                </RunSettings>"
        };

        _commandLineOptions.IsDesignMode = false;
        TestRunCriteria? actualTestRunCriteria = null;
        var mockTestRunRequest = new Mock<ITestRunRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateTestRunRequest(It.IsAny<IRequestData>(), It.IsAny<TestRunCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, TestRunCriteria runCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualTestRunCriteria = runCriteria).Returns(mockTestRunRequest.Object);
        _testRequestManager.RunTests(payload, new Mock<ITestHostLauncher3>().Object, new Mock<ITestRunEventsRegistrar>().Object, _protocolConfig);

        var loggerSettingsList = XmlRunSettingsUtilities.GetLoggerRunSettings(actualTestRunCriteria!.TestRunSettings)!.LoggerSettingsList;
        Assert.AreEqual(2, loggerSettingsList.Count);
        Assert.IsNotNull(loggerSettingsList[0].Configuration);
        Assert.AreEqual("blabla", loggerSettingsList[0].FriendlyName);
        Assert.AreEqual("console", loggerSettingsList[1].FriendlyName);
        Assert.AreEqual(new Uri("logger://tempconsoleUri").ToString(), loggerSettingsList[1].Uri!.ToString());
        Assert.AreNotEqual("tempAssemblyName", loggerSettingsList[1].AssemblyQualifiedName);
        Assert.AreNotEqual("tempCodeBase", loggerSettingsList[1].CodeBase);
        Assert.IsTrue(loggerSettingsList[1].Configuration!.InnerXml.Contains("Value1"));
        Assert.IsNotNull(loggerSettingsList[1].AssemblyQualifiedName);
        Assert.IsNotNull(loggerSettingsList[1].CodeBase);
    }

    [TestMethod]
    public void DiscoverTestsShouldOverrideOnlyAssemblyNameIfConsoleLoggerAlreadyPresentInDesignMode()
    {
        var payload = new DiscoveryRequestPayload()
        {
            Sources = new List<string>() { "a.dll" },
            RunSettings =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <DesignMode>True</DesignMode>
                     </RunConfiguration>
                     <LoggerRunSettings>
                       <Loggers>
                         <Logger friendlyName=""blabla"">
                           <Configuration>
                             <Key1>Value1</Key1>
                           </Configuration>
                         </Logger>
                         <Logger friendlyName=""consoleTemp"" uri=""logger://Microsoft/TestPlatform/ConsoleLogger/v1"" assemblyQualifiedName=""tempAssemblyName"">
                           <Configuration>
                             <Key1>Value1</Key1>
                           </Configuration>
                         </Logger>
                       </Loggers>
                     </LoggerRunSettings>
                </RunSettings>"
        };
        _commandLineOptions.IsDesignMode = false;
        DiscoveryCriteria? actualDiscoveryCriteria = null;
        var mockDiscoveryRequest = new Mock<IDiscoveryRequest>();
        _mockTestPlatform
            .Setup(mt => mt.CreateDiscoveryRequest(It.IsAny<IRequestData>(), It.IsAny<DiscoveryCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>()))
            .Callback(
                (IRequestData requestData, DiscoveryCriteria discoveryCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => actualDiscoveryCriteria = discoveryCriteria).Returns(mockDiscoveryRequest.Object);

        _testRequestManager.DiscoverTests(payload,
            new Mock<ITestDiscoveryEventsRegistrar>().Object, _protocolConfig);

        var loggerSettingsList = XmlRunSettingsUtilities.GetLoggerRunSettings(actualDiscoveryCriteria!.RunSettings)!.LoggerSettingsList;
        Assert.AreEqual(2, loggerSettingsList.Count);
        Assert.IsNotNull(loggerSettingsList[0].Configuration);
        Assert.AreEqual("blabla", loggerSettingsList[0].FriendlyName);
        Assert.AreEqual("consoleTemp", loggerSettingsList[1].FriendlyName);
        Assert.AreEqual(new Uri("logger://Microsoft/TestPlatform/ConsoleLogger/v1").ToString(), loggerSettingsList[1].Uri!.ToString());
        Assert.AreNotEqual("tempAssemblyName", loggerSettingsList[1].AssemblyQualifiedName);
        Assert.AreNotEqual("tempAssemblyName", loggerSettingsList[1].CodeBase);
        Assert.IsTrue(loggerSettingsList[1].Configuration!.InnerXml.Contains("Value1"));
        Assert.IsNotNull(loggerSettingsList[1].AssemblyQualifiedName);
        Assert.IsNotNull(loggerSettingsList[1].CodeBase);
    }

    [TestMethod]
    public void ProcessTestRunAttachmentsShouldSucceedWithTelemetryEnabled()
    {
        var mockEventsHandler = new Mock<ITestRunAttachmentsProcessingEventsHandler>();
        _mockAttachmentsProcessingManager
            .Setup(m => m.ProcessTestRunAttachmentsAsync(Constants.EmptyRunSettings, It.IsAny<IRequestData>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<ICollection<InvokedDataCollector>>(), It.IsAny<ITestRunAttachmentsProcessingEventsHandler>(), It.IsAny<CancellationToken>()))
            .Returns((string runSettingsXml, IRequestData r, ICollection<AttachmentSet> a, ICollection<InvokedDataCollector> b, ITestRunAttachmentsProcessingEventsHandler h, CancellationToken token) => Task.Run(() =>
            {
                r.MetricsCollection.Add(TelemetryDataConstants.NumberOfAttachmentsSentForProcessing, 5);
                r.MetricsCollection.Add(TelemetryDataConstants.NumberOfAttachmentsAfterProcessing, 1);
            }, token));

        var payload = new TestRunAttachmentsProcessingPayload()
        {
            Attachments = new List<AttachmentSet> { new(new Uri("http://www.bing.com"), "out") },
            InvokedDataCollectors = new List<InvokedDataCollector>(),
            RunSettings = Constants.EmptyRunSettings,
            CollectMetrics = true
        };

        _testRequestManager.ProcessTestRunAttachments(payload, mockEventsHandler.Object, _protocolConfig);

        _mockAttachmentsProcessingManager.Verify(m => m.ProcessTestRunAttachmentsAsync(Constants.EmptyRunSettings, It.Is<IRequestData>(r => r.IsTelemetryOptedIn), payload.Attachments, payload.InvokedDataCollectors, mockEventsHandler.Object, It.IsAny<CancellationToken>()));
        _mockTestPlatformEventSource.Verify(es => es.TestRunAttachmentsProcessingRequestStart());
        _mockTestPlatformEventSource.Verify(es => es.TestRunAttachmentsProcessingRequestStop());

        _mockMetricsPublisher.Verify(p => p.PublishMetrics(TelemetryDataConstants.TestAttachmentsProcessingCompleteEvent,
            It.Is<Dictionary<string, object?>>(m =>
                m.Count == 2
                && m.ContainsKey(TelemetryDataConstants.NumberOfAttachmentsSentForProcessing)
                && (int)m[TelemetryDataConstants.NumberOfAttachmentsSentForProcessing]! == 5
                && m.ContainsKey(TelemetryDataConstants.NumberOfAttachmentsAfterProcessing)
                && (int)m[TelemetryDataConstants.NumberOfAttachmentsAfterProcessing]! == 1)));
    }

    [TestMethod]
    public void ProcessTestRunAttachmentsShouldSucceedWithTelemetryDisabled()
    {
        var mockEventsHandler = new Mock<ITestRunAttachmentsProcessingEventsHandler>();
        _mockAttachmentsProcessingManager
            .Setup(m => m.ProcessTestRunAttachmentsAsync(Constants.EmptyRunSettings, It.IsAny<IRequestData>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<ICollection<InvokedDataCollector>>(), It.IsAny<ITestRunAttachmentsProcessingEventsHandler>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));

        var payload = new TestRunAttachmentsProcessingPayload()
        {
            Attachments = new List<AttachmentSet> { new(new Uri("http://www.bing.com"), "out") },
            InvokedDataCollectors = new List<InvokedDataCollector>(),
            RunSettings = Constants.EmptyRunSettings,
            CollectMetrics = false
        };

        _testRequestManager.ProcessTestRunAttachments(payload, mockEventsHandler.Object, _protocolConfig);

        _mockAttachmentsProcessingManager.Verify(m => m.ProcessTestRunAttachmentsAsync(Constants.EmptyRunSettings, It.Is<IRequestData>(r => !r.IsTelemetryOptedIn), payload.Attachments, payload.InvokedDataCollectors, mockEventsHandler.Object, It.IsAny<CancellationToken>()));
        _mockTestPlatformEventSource.Verify(es => es.TestRunAttachmentsProcessingRequestStart());
        _mockTestPlatformEventSource.Verify(es => es.TestRunAttachmentsProcessingRequestStop());
    }

    [TestMethod]
    public async Task CancelTestRunAttachmentsProcessingShouldSucceedIfRequestInProgress()
    {
        var mockEventsHandler = new Mock<ITestRunAttachmentsProcessingEventsHandler>();
        _mockAttachmentsProcessingManager
            .Setup(m => m.ProcessTestRunAttachmentsAsync(Constants.EmptyRunSettings, It.IsAny<IRequestData>(), It.IsAny<ICollection<AttachmentSet>>(), It.IsAny<ICollection<InvokedDataCollector>>(), It.IsAny<ITestRunAttachmentsProcessingEventsHandler>(), It.IsAny<CancellationToken>()))
            .Returns((string runSettingsXml, IRequestData r, ICollection<AttachmentSet> a, ICollection<InvokedDataCollector> b, ITestRunAttachmentsProcessingEventsHandler h, CancellationToken token) => Task.Run(() =>
            {
                int i = 0;
                while (!token.IsCancellationRequested)
                {
                    i++;
                    Console.WriteLine($"Iteration {i}");
                    Task.Delay(5).Wait();
                }

                r.MetricsCollection.Add(TelemetryDataConstants.AttachmentsProcessingState, "Canceled");
            }, token));

        var payload = new TestRunAttachmentsProcessingPayload()
        {
            Attachments = new List<AttachmentSet> { new(new Uri("http://www.bing.com"), "out") },
            InvokedDataCollectors = new List<InvokedDataCollector>(),
            RunSettings = Constants.EmptyRunSettings,
            CollectMetrics = true
        };

        Task task = Task.Run(() => _testRequestManager.ProcessTestRunAttachments(payload, mockEventsHandler.Object, _protocolConfig));
        await Task.Delay(50);
        _testRequestManager.CancelTestRunAttachmentsProcessing();

        await task;

        _mockAttachmentsProcessingManager.Verify(m => m.ProcessTestRunAttachmentsAsync(Constants.EmptyRunSettings, It.IsAny<IRequestData>(), payload.Attachments, payload.InvokedDataCollectors, mockEventsHandler.Object, It.IsAny<CancellationToken>()));
        _mockTestPlatformEventSource.Verify(es => es.TestRunAttachmentsProcessingRequestStart());
        _mockTestPlatformEventSource.Verify(es => es.TestRunAttachmentsProcessingRequestStop());

        _mockMetricsPublisher.Verify(p => p.PublishMetrics(TelemetryDataConstants.TestAttachmentsProcessingCompleteEvent,
            It.Is<Dictionary<string, object?>>(m =>
                m.Count == 1
                && m.ContainsKey(TelemetryDataConstants.AttachmentsProcessingState)
                && (string?)m[TelemetryDataConstants.AttachmentsProcessingState] == "Canceled")));
    }

    [TestMethod]
    public void CancelTestRunAttachmentsProcessingShouldSucceedIfNoRequest()
    {
        _testRequestManager.CancelTestRunAttachmentsProcessing();
    }

    [TestMethod]
    public void StartTestSessionShouldPassCorrectTelemetryOptedInOptionToTestPlatform()
    {
        _mockTestPlatform.Setup(
                tp => tp.StartTestSession(
                    It.IsAny<IRequestData>(),
                    It.IsAny<StartTestSessionCriteria>(),
                    It.IsAny<ITestSessionEventsHandler>(),
                    It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>()))
            .Returns(true)
            .Callback(
                (IRequestData rd, StartTestSessionCriteria _, ITestSessionEventsHandler _, Dictionary<string, SourceDetail> _, IWarningLogger _) => Assert.IsTrue(rd.IsTelemetryOptedIn));

        Environment.SetEnvironmentVariable("VSTEST_TELEMETRY_OPTEDIN", "1");

        _testRequestManager.StartTestSession(
            new StartTestSessionPayload()
            {
                TestPlatformOptions = new TestPlatformOptions()
                {
                    CollectMetrics = true
                }
            },
            new Mock<ITestHostLauncher3>().Object,
            new Mock<ITestSessionEventsHandler>().Object,
            _protocolConfig);
    }

    [TestMethod]
    public void StartTestSessionShouldUpdateSettings()
    {
        var payload = new StartTestSessionPayload()
        {
            Sources = new List<string>() { "a.dll" },
            RunSettings =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                    <RunSettings>
                        <RunConfiguration>
                        </RunConfiguration>
                    </RunSettings>"
        };
        _commandLineOptions.IsDesignMode = true;

        _mockAssemblyMetadataProvider.Setup(
                a => a.GetArchitecture(It.IsAny<string>()))
            .Returns(Architecture.ARM);
        _mockAssemblyMetadataProvider.Setup(
                a => a.GetFrameworkName(It.IsAny<string>()))
            .Returns(new FrameworkName(Constants.DotNetFramework46));

        _mockTestPlatform.Setup(
                tp => tp.StartTestSession(
                    It.IsAny<IRequestData>(),
                    It.IsAny<StartTestSessionCriteria>(),
                    It.IsAny<ITestSessionEventsHandler>(),
                    It.IsAny<Dictionary<string, SourceDetail>>(),
                    It.IsAny<IWarningLogger>()))
            .Returns(true)
            .Callback(
                (IRequestData _, StartTestSessionCriteria criteria, ITestSessionEventsHandler _, Dictionary<string, SourceDetail> _, IWarningLogger _) =>
                {
                    Assert.IsTrue(criteria.RunSettings!.Contains(Constants.DotNetFramework46));
                    Assert.IsTrue(criteria.RunSettings.Contains(nameof(Architecture.ARM)));
                });

        _testRequestManager.StartTestSession(
            payload,
            new Mock<ITestHostLauncher3>().Object,
            new Mock<ITestSessionEventsHandler>().Object,
            _protocolConfig);

        _mockAssemblyMetadataProvider.Verify(a => a.GetArchitecture(It.IsAny<string>()));
        _mockAssemblyMetadataProvider.Verify(a => a.GetFrameworkName(It.IsAny<string>()));
    }

    [TestMethod]
    public void StartTestSessionShouldSendCompletedEventIfTestPlatformReturnsFalse()
    {
        var payload = new StartTestSessionPayload()
        {
            Sources = new List<string>() { "a.dll" },
            RunSettings =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                    <RunSettings>
                        <RunConfiguration>
                        </RunConfiguration>
                    </RunSettings>"
        };

        var eventsHandler = new Mock<ITestSessionEventsHandler>();
        _commandLineOptions.IsDesignMode = true;

        _mockAssemblyMetadataProvider.Setup(
                a => a.GetArchitecture(It.IsAny<string>()))
            .Returns(Architecture.ARM);
        _mockAssemblyMetadataProvider.Setup(
                a => a.GetFrameworkName(It.IsAny<string>()))
            .Returns(new FrameworkName(Constants.DotNetFramework46));

        _mockTestPlatform.Setup(
                tp => tp.StartTestSession(
                    It.IsAny<IRequestData>(),
                    It.IsAny<StartTestSessionCriteria>(),
                    It.IsAny<ITestSessionEventsHandler>(),
                    It.IsAny<Dictionary<string, SourceDetail>>(),
                    It.IsAny<IWarningLogger>()))
            .Returns(false)
            .Callback(
                (IRequestData _, StartTestSessionCriteria criteria, ITestSessionEventsHandler _, Dictionary<string, SourceDetail> _, IWarningLogger _) =>
                {
                    Assert.IsTrue(criteria.RunSettings!.Contains(Constants.DotNetFramework46));
                    Assert.IsTrue(criteria.RunSettings.Contains(nameof(Architecture.ARM)));
                });

        _testRequestManager.StartTestSession(
            payload,
            new Mock<ITestHostLauncher3>().Object,
            eventsHandler.Object,
            _protocolConfig);

        eventsHandler.Verify(eh => eh.HandleStartTestSessionComplete(It.IsAny<StartTestSessionCompleteEventArgs>()));
        _mockAssemblyMetadataProvider.Verify(a => a.GetArchitecture(It.IsAny<string>()));
        _mockAssemblyMetadataProvider.Verify(a => a.GetFrameworkName(It.IsAny<string>()));
    }

    [TestMethod]
    public void StartTestSessionShouldThrowSettingsExceptionWhenFindingIncompatibleDataCollectorsInTestSettings()
    {
        var settingXml = @"<RunSettings>
                                    <MSTest>
                                        <SettingsFile>C:\temp.testsettings</SettingsFile>
                                        <ForcedLegacyMode>true</ForcedLegacyMode>
                                    </MSTest>
                                    <DataCollectionRunSettings>
                                        <DataCollectors>
                                            <DataCollector friendlyName=""DummyDataCollector1"">
                                            </DataCollector>
                                            <DataCollector friendlyName=""DummyDataCollector2"">
                                            </DataCollector>
                                        </DataCollectors>
                                    </DataCollectionRunSettings>
                                </RunSettings>";

        var payload = new StartTestSessionPayload()
        {
            Sources = new List<string>() { "a.dll" },
            RunSettings = settingXml
        };

        _commandLineOptions.EnableCodeCoverage = false;
        bool exceptionThrown = false;

        try
        {
            _testRequestManager.StartTestSession(
                payload,
                new Mock<ITestHostLauncher3>().Object,
                new Mock<ITestSessionEventsHandler>().Object,
                _protocolConfig);
        }
        catch (SettingsException ex)
        {
            exceptionThrown = true;
            Assert.IsTrue(ex.Message.Contains(@"<SettingsFile>C:\temp.testsettings</SettingsFile>"), ex.Message);
        }

        Assert.IsTrue(exceptionThrown, "Initialize should throw exception");
    }

    [TestMethod]
    public void StartTestSessionShouldBeSuccessful()
    {
        _mockTestPlatform.Setup(
                tp => tp.StartTestSession(
                    It.IsAny<IRequestData>(),
                    It.IsAny<StartTestSessionCriteria>(),
                    It.IsAny<ITestSessionEventsHandler>(),
                    It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>()))
            .Returns(true);

        _testRequestManager.StartTestSession(
            new StartTestSessionPayload()
            {
                TestPlatformOptions = new TestPlatformOptions()
                {
                    CollectMetrics = true
                }
            },
            new Mock<ITestHostLauncher3>().Object,
            new Mock<ITestSessionEventsHandler>().Object,
            _protocolConfig);

        _mockTestPlatformEventSource.Verify(
            tpes => tpes.StartTestSessionStart(),
            Times.Once());
        _mockTestPlatformEventSource.Verify(
            tpes => tpes.StartTestSessionStop(),
            Times.Once());
    }

    [TestMethod]
    public void StopTestSessionShouldBeSuccessful()
    {
        var result = true;
        var testSessionInfo = new TestSessionInfo();
        var mockEventsHandler = new Mock<ITestSessionEventsHandler>();

        var mockTestPool = new Mock<TestSessionPool>();
        TestSessionPool.Instance = mockTestPool.Object;

        mockTestPool.Setup(tp => tp.KillSession(testSessionInfo, It.IsAny<IRequestData>()))
            .Returns((TestSessionInfo _, IRequestData rd) =>
            {
                rd.MetricsCollection.Add(TelemetryDataConstants.TestSessionId, testSessionInfo.Id.ToString());
                return result;
            });
        mockEventsHandler.Setup(eh => eh.HandleStopTestSessionComplete(
                It.IsAny<StopTestSessionCompleteEventArgs>()))
            .Callback((StopTestSessionCompleteEventArgs eventArgs) =>
            {
                Assert.IsNotNull(eventArgs.TestSessionInfo);
                Assert.IsNotNull(eventArgs.Metrics);
                Assert.AreEqual(eventArgs.TestSessionInfo, testSessionInfo);
                Assert.AreEqual(
                    eventArgs.Metrics[TelemetryDataConstants.TestSessionId],
                    testSessionInfo.Id.ToString());
                Assert.AreEqual(eventArgs.IsStopped, result);
            });

        _testRequestManager.StopTestSession(
            new()
            {
                TestSessionInfo = testSessionInfo,
                CollectMetrics = true
            },
            mockEventsHandler.Object,
            _protocolConfig);

        mockTestPool.Verify(tp => tp.KillSession(
                testSessionInfo,
                It.IsAny<IRequestData>()),
            Times.Once);
        mockEventsHandler.Verify(eh => eh.HandleStopTestSessionComplete(
                It.IsAny<StopTestSessionCompleteEventArgs>()),
            Times.Once);

        _mockTestPlatformEventSource.Verify(
            tpes => tpes.StopTestSessionStart(),
            Times.Once);
        _mockTestPlatformEventSource.Verify(
            tpes => tpes.StopTestSessionStop(),
            Times.Once);
    }

    [TestMethod]
    public void StopTestSessionShouldFail()
    {
        var result = false;
        var testSessionInfo = new TestSessionInfo();
        var mockEventsHandler = new Mock<ITestSessionEventsHandler>();

        var mockTestPool = new Mock<TestSessionPool>();
        TestSessionPool.Instance = mockTestPool.Object;

        mockTestPool.Setup(tp => tp.KillSession(testSessionInfo, It.IsAny<IRequestData>()))
            .Returns(result);
        mockEventsHandler.Setup(eh => eh.HandleStopTestSessionComplete(
                It.IsAny<StopTestSessionCompleteEventArgs>()))
            .Callback((StopTestSessionCompleteEventArgs eventArgs) =>
            {
                Assert.IsNotNull(eventArgs.TestSessionInfo);
                Assert.IsNull(eventArgs.Metrics);
                Assert.AreEqual(eventArgs.TestSessionInfo, testSessionInfo);
                Assert.AreEqual(eventArgs.IsStopped, result);
            });

        _testRequestManager.StopTestSession(
            new()
            {
                TestSessionInfo = testSessionInfo,
                CollectMetrics = true
            },
            mockEventsHandler.Object,
            _protocolConfig);

        mockTestPool.Verify(tp => tp.KillSession(
                testSessionInfo,
                It.IsAny<IRequestData>()),
            Times.Once);
        mockEventsHandler.Verify(eh => eh.HandleStopTestSessionComplete(
                It.IsAny<StopTestSessionCompleteEventArgs>()),
            Times.Once);

        _mockTestPlatformEventSource.Verify(
            tpes => tpes.StopTestSessionStart(),
            Times.Once);
        _mockTestPlatformEventSource.Verify(
            tpes => tpes.StopTestSessionStop(),
            Times.Once);
    }

    [TestMethod]
    public void StopTestSessionShouldPropagateExceptionWhenKillSessionThrows()
    {
        var testSessionInfo = new TestSessionInfo();
        var mockEventsHandler = new Mock<ITestSessionEventsHandler>();

        var mockTestPool = new Mock<TestSessionPool>();
        TestSessionPool.Instance = mockTestPool.Object;

        mockTestPool.Setup(tp => tp.KillSession(testSessionInfo, It.IsAny<IRequestData>()))
            .Throws(new Exception("DummyException"));
        mockEventsHandler.Setup(eh => eh.HandleStopTestSessionComplete(
                It.IsAny<StopTestSessionCompleteEventArgs>()))
            .Callback((StopTestSessionCompleteEventArgs eventArgs) =>
            {
                Assert.IsNotNull(eventArgs.TestSessionInfo);
                Assert.IsNotNull(eventArgs.Metrics);
                Assert.AreEqual(eventArgs.TestSessionInfo, testSessionInfo);
                Assert.AreEqual(eventArgs.IsStopped, false);
            });

        Assert.ThrowsException<Exception>(() =>
            _testRequestManager.StopTestSession(
                new()
                {
                    TestSessionInfo = testSessionInfo,
                    CollectMetrics = true
                },
                mockEventsHandler.Object,
                _protocolConfig));

        mockTestPool.Verify(tp => tp.KillSession(
                testSessionInfo,
                It.IsAny<IRequestData>()),
            Times.Once);
        mockEventsHandler.Verify(eh => eh.HandleStopTestSessionComplete(
                It.IsAny<StopTestSessionCompleteEventArgs>()),
            Times.Never);

        _mockTestPlatformEventSource.Verify(
            tpes => tpes.StopTestSessionStart(),
            Times.Once);
        _mockTestPlatformEventSource.Verify(
            tpes => tpes.StopTestSessionStop(),
            Times.Once);
    }

    [TestMethod]
    public void AddOrUpdateBatchSizeWhenNotDiscoveryReturnsFalseAndDoesNotUpdateXmlDocument()
    {
        // Arrange
        var xmlDocument = new XmlDocument();
        var configuration = new RunConfiguration();

        // Act
        var result = TestRequestManager.AddOrUpdateBatchSize(xmlDocument, configuration, false);

        // Assert
        Assert.IsFalse(result);
        Assert.AreEqual("", xmlDocument.OuterXml);
    }

    [TestMethod]
    public void AddOrUpdateBatchSizeWhenBatchSizeSetReturnsFalse()
    {
        // Arrange
        var xmlDocument = new XmlDocument();
        var configuration = new RunConfiguration { BatchSize = 10 };

        // Sanity check
        Assert.IsTrue(configuration.BatchSizeSet);

        // Act
        var result = TestRequestManager.AddOrUpdateBatchSize(xmlDocument, configuration, true);

        // Assert
        Assert.IsFalse(result);
        Assert.AreEqual("", xmlDocument.OuterXml);
    }

    [TestMethod]
    public void AddOrUpdateBatchSizeSetsBatchSize()
    {
        // Arrange
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml("""
            <RunSettings>
                <RunConfiguration>
                </RunConfiguration>
            </RunSettings>
            """);
        var configuration = new RunConfiguration();

        // Act
        var result = TestRequestManager.AddOrUpdateBatchSize(xmlDocument, configuration, true);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual("<RunSettings><RunConfiguration><BatchSize>1000</BatchSize></RunConfiguration></RunSettings>", xmlDocument.OuterXml);
    }

    [TestMethod]
    public void AddOrUpdateBatchSizeSetsRunConfigurationAndBatchSize()
    {
        // Arrange
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml("""
            <RunSettings>
            </RunSettings>
            """);
        var configuration = new RunConfiguration();

        // Act
        var result = TestRequestManager.AddOrUpdateBatchSize(xmlDocument, configuration, true);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual("<RunSettings><RunConfiguration><BatchSize>1000</BatchSize></RunConfiguration></RunSettings>", xmlDocument.OuterXml);
    }

    [TestMethod]
    public void UpdateCodeCoverageSettings_SetEnableDynamicNativeInstrumentationToFalse_WhenNotPresent()
    {
        // Arrange
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml("""
            <?xml version="1.0" encoding="utf-8"?>
            <RunSettings>
              <DataCollectionRunSettings>
                <DataCollectors>
                  <DataCollector friendlyName="Code Coverage" uri="datacollector://Microsoft/CodeCoverage/2.0" assemblyQualifiedName="Microsoft.VisualStudio.Coverage.DynamicCoverageDataCollector, Microsoft.VisualStudio.TraceCollector, Version=11.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a">
                    <Configuration>
                      <CoverageLogLevel>All</CoverageLogLevel>
                      <InstrumentationLogLevel>All</InstrumentationLogLevel>
                      <ManagedVanguardLogLevel>Verbose</ManagedVanguardLogLevel>
                      <CoverageFileLogPath>%LOGS_DIR%</CoverageFileLogPath>
                      <CodeCoverage>
                        <FileLogPath>%LOGS_DIR%</FileLogPath>
                        <LogLevel>All</LogLevel>
                        <UseVerifiableInstrumentation>False</UseVerifiableInstrumentation>
                        <EnableStaticNativeInstrumentation>False</EnableStaticNativeInstrumentation>
                      </CodeCoverage>
                    </Configuration>
                  </DataCollector>
                </DataCollectors>
              </DataCollectionRunSettings>
            </RunSettings>
            """);
        var configuration = new RunConfiguration();

        // Act
        var result = TestRequestManager.UpdateCollectCoverageSettings(xmlDocument, configuration);

        // Assert
        Assert.IsTrue(result);
        StringAssert.Contains(xmlDocument.OuterXml, "<EnableStaticNativeInstrumentation>False</EnableStaticNativeInstrumentation><EnableDynamicNativeInstrumentation>False</EnableDynamicNativeInstrumentation></CodeCoverage>");
    }

    [TestMethod]
    public void UpdateCodeCoverageSettings_SetEnableDynamicNativeInstrumentationToFalse_WhenNotPresentAndParentDetailsOfConfigurationAreAlsoNotPresent()
    {
        // Arrange
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml("""
            <?xml version="1.0" encoding="utf-8"?>
            <RunSettings>
              <DataCollectionRunSettings>
                <DataCollectors>
                  <DataCollector friendlyName="Code Coverage" uri="datacollector://Microsoft/CodeCoverage/2.0" assemblyQualifiedName="Microsoft.VisualStudio.Coverage.DynamicCoverageDataCollector, Microsoft.VisualStudio.TraceCollector, Version=11.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a">
                  </DataCollector>
                </DataCollectors>
              </DataCollectionRunSettings>
            </RunSettings>
            """);
        var configuration = new RunConfiguration();

        // Act
        var result = TestRequestManager.UpdateCollectCoverageSettings(xmlDocument, configuration);

        // Assert
        Assert.IsTrue(result);
        StringAssert.Contains(xmlDocument.OuterXml, $"<Configuration><CodeCoverage><EnableDynamicNativeInstrumentation>False</EnableDynamicNativeInstrumentation></CodeCoverage></Configuration></DataCollector>");
    }

    [TestMethod]
    [DataRow("True")]
    [DataRow("False")]
    public void UpdateCodeCoverageSettings_DontSetEnableDynamicNativeInstrumentationToFalse_WhenAlreadyPresent(string setting)
    {
        // Arrange
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml($"""
            <?xml version="1.0" encoding="utf-8"?>
            <RunSettings>
              <DataCollectionRunSettings>
                <DataCollectors>
                  <DataCollector friendlyName="Code Coverage" uri="datacollector://Microsoft/CodeCoverage/2.0" assemblyQualifiedName="Microsoft.VisualStudio.Coverage.DynamicCoverageDataCollector, Microsoft.VisualStudio.TraceCollector, Version=11.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a">
                    <Configuration>
                      <CoverageLogLevel>All</CoverageLogLevel>
                      <InstrumentationLogLevel>All</InstrumentationLogLevel>
                      <ManagedVanguardLogLevel>Verbose</ManagedVanguardLogLevel>
                      <CoverageFileLogPath>%LOGS_DIR%</CoverageFileLogPath>
                      <CodeCoverage>
                        <EnableDynamicNativeInstrumentation>{setting}</EnableDynamicNativeInstrumentation>
                      </CodeCoverage>
                    </Configuration>
                  </DataCollector>
                </DataCollectors>
              </DataCollectionRunSettings>
            </RunSettings>
            """);
        var configuration = new RunConfiguration();

        // Act
        var result = TestRequestManager.UpdateCollectCoverageSettings(xmlDocument, configuration);

        // Assert
        // No matter what user has set, we don't override it.
        Assert.IsFalse(result);
        StringAssert.Contains(xmlDocument.OuterXml, $"<CodeCoverage><EnableDynamicNativeInstrumentation>{setting}</EnableDynamicNativeInstrumentation></CodeCoverage>");
    }

    [TestMethod]
    [DataRow("friendlyName=\"Code Coverage\"")]
    [DataRow("friendlyName=\"code coverage\"")]
    [DataRow("uri=\"datacollector://Microsoft/CodeCoverage/2.0\"")]
    [DataRow("uri=\"datacollector://microsoft/codecoverage/2.0\"")]
    [DataRow("assemblyQualifiedName=\"Microsoft.VisualStudio.Coverage.DynamicCoverageDataCollector, Microsoft.VisualStudio.TraceCollector, Version=11.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a\"")]
    public void UpdateCodeCoverageSettings_SetEnableDynamicNativeInstrumentationToFalse_WhenUserUsesImperfectNamesForCollector(string collector)
    {
        // Arrange
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml($"""
            <?xml version="1.0" encoding="utf-8"?>
            <RunSettings>
              <DataCollectionRunSettings>
                <DataCollectors>
                  <DataCollector {collector}>
                    <Configuration>
                      <CoverageLogLevel>All</CoverageLogLevel>
                      <InstrumentationLogLevel>All</InstrumentationLogLevel>
                      <ManagedVanguardLogLevel>Verbose</ManagedVanguardLogLevel>
                      <CoverageFileLogPath>%LOGS_DIR%</CoverageFileLogPath>
                      <CodeCoverage>
                      </CodeCoverage>
                    </Configuration>
                  </DataCollector>
                </DataCollectors>
              </DataCollectionRunSettings>
            </RunSettings>
            """);
        var configuration = new RunConfiguration();

        // Act
        var result = TestRequestManager.UpdateCollectCoverageSettings(xmlDocument, configuration);

        // Assert
        Assert.IsTrue(result);
        StringAssert.Contains(xmlDocument.OuterXml, $"<CodeCoverage><EnableDynamicNativeInstrumentation>False</EnableDynamicNativeInstrumentation></CodeCoverage>");
    }

    private static DiscoveryRequestPayload CreateDiscoveryPayload(string runsettings)
    {
        var discoveryPayload = new DiscoveryRequestPayload
        {
            RunSettings = runsettings,
            Sources = new[] { "c:\\testproject.dll" }
        };
        return discoveryPayload;
    }

    private void RunTestsIfThrowsExceptionShouldThrowOut(Exception exception)
    {
        var payload = new TestRunRequestPayload()
        {
            Sources = new List<string>() { "a", "b" },
            RunSettings = DefaultRunsettings
        };

        var createRunRequestCalled = 0;
        TestRunCriteria? observedCriteria = null;
        var mockRunRequest = new Mock<ITestRunRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateTestRunRequest(It.IsAny<IRequestData>(), It.IsAny<TestRunCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, TestRunCriteria runCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) =>
            {
                createRunRequestCalled++;
                observedCriteria = runCriteria;
            }).Returns(mockRunRequest.Object);

        mockRunRequest.Setup(mr => mr.ExecuteAsync()).Throws(exception);

        var mockRunEventsRegistrar = new Mock<ITestRunEventsRegistrar>();
        var mockCustomlauncher = new Mock<ITestHostLauncher3>();

        _testRequestManager.RunTests(payload, mockCustomlauncher.Object, mockRunEventsRegistrar.Object, _protocolConfig);
    }

    private void DiscoverTestsIfThrowsExceptionShouldThrowOut(Exception exception)
    {
        var payload = new DiscoveryRequestPayload()
        {
            Sources = new List<string>() { "a.dll", "b.dll" },
            RunSettings = DefaultRunsettings
        };

        DiscoveryCriteria? observedCriteria = null;
        var mockDiscoveryRequest = new Mock<IDiscoveryRequest>();
        _mockTestPlatform.Setup(mt => mt.CreateDiscoveryRequest(It.IsAny<IRequestData>(), It.IsAny<DiscoveryCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>())).Callback(
            (IRequestData requestData, DiscoveryCriteria discoveryCriteria, TestPlatformOptions options, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) => observedCriteria = discoveryCriteria).Returns(mockDiscoveryRequest.Object);

        mockDiscoveryRequest.Setup(mr => mr.DiscoverAsync()).Throws(exception);

        var mockDiscoveryEventsRegistrar = new Mock<ITestDiscoveryEventsRegistrar>();
        var mockCustomlauncher = new Mock<ITestHostLauncher3>();

        _testRequestManager.DiscoverTests(payload, mockDiscoveryEventsRegistrar.Object, _protocolConfig);
    }

    [TestMethod]
    [DataRow("x86")]
    [DataRow("x64")]
    [DataRow("arm64")]
    // Don't parallelize because we can run into conflict with GetDefaultArchitecture -> RunSettingsHelper.Instance.IsDefaultTargetArchitecture
    // which is set by some other test.
    [DoNotParallelize]
    public void SettingDefaultPlatformUsesItForAnyCPUSourceButNotForNonAnyCPUSource(string defaultPlatform)
    {
        // -- Arrange

        RunSettingsHelper.Instance.IsDefaultTargetArchitecture = true;
        var payload = new DiscoveryRequestPayload()
        {
            Sources = new List<string>() { "AnyCPU.dll", "x64.dll" },
            RunSettings =
                $@"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <DefaultPlatform>{defaultPlatform}</DefaultPlatform>
                     </RunConfiguration>
                </RunSettings>"
        };

        Architecture expectedPlatform = (Architecture)Enum.Parse(typeof(Architecture), defaultPlatform, ignoreCase: true);

        Dictionary<string, SourceDetail>? actualSourceToSourceDetailMap = null;
        var mockDiscoveryRequest = new Mock<IDiscoveryRequest>();

        _mockAssemblyMetadataProvider.Setup(m => m.GetArchitecture("AnyCPU.dll")).Returns(Architecture.AnyCPU);
        _mockAssemblyMetadataProvider.Setup(m => m.GetArchitecture("x64.dll")).Returns(Architecture.X64);

        _mockTestPlatform.Setup(mt => mt.CreateDiscoveryRequest(It.IsAny<IRequestData>(), It.IsAny<DiscoveryCriteria>(), It.IsAny<TestPlatformOptions>(), It.IsAny<Dictionary<string, SourceDetail>>(), It.IsAny<IWarningLogger>()))
            .Callback((IRequestData _, DiscoveryCriteria _, TestPlatformOptions _, Dictionary<string, SourceDetail> sourceToSourceDetailMap, IWarningLogger _) =>
                // output the incoming sourceToSourceDetailMap to the variable above so we can inspect it.
                actualSourceToSourceDetailMap = sourceToSourceDetailMap
            ).Returns(mockDiscoveryRequest.Object);

        // -- Act
        // The substitution of architecture happens in runsettings patching which is shared for discovery and run
        // so we can safely just test discovery.
        _testRequestManager.DiscoverTests(payload, new Mock<ITestDiscoveryEventsRegistrar>().Object, _protocolConfig);

        actualSourceToSourceDetailMap.Should().NotBeNull();
        // The AnyCPU dll is the architecture we provide in the default setting, rather than being determined from the
        // current process architecture.
        actualSourceToSourceDetailMap!["AnyCPU.dll"].Architecture.Should().Be(expectedPlatform);
        // The dll that has a specific architecture always remains that specific architecture.
        actualSourceToSourceDetailMap!["x64.dll"].Architecture.Should().Be(Architecture.X64);
    }

    [TestMethod]
    public void UsingInvalidValueForDefaultPlatformSettingThrowsSettingsException()
    {
        var settingXml = @"
            <RunSettings>
                <RunConfiguration>
                    <DefaultPlatform>WRONGPlatform</DefaultPlatform>
                </RunConfiguration>
            </RunSettings>";

        var payload = new TestRunRequestPayload()
        {
            Sources = new List<string>() { "a.dll" },
            RunSettings = settingXml
        };

        _testRequestManager
            .Invoking(m => m.RunTests(payload, new Mock<ITestHostLauncher3>().Object, new Mock<ITestRunEventsRegistrar>().Object, _protocolConfig))
            .Should().Throw<SettingsException>()
            .And.Message.Should().Contain("Invalid value 'WRONGPlatform' specified for 'DefaultPlatform'.");
    }
}
