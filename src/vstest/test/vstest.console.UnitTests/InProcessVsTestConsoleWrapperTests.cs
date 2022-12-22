// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.TestPlatform.VsTestConsole.TranslationLayer;
using Microsoft.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Client;
using Microsoft.VisualStudio.TestPlatform.Client.RequestHelper;
using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Execution;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Payloads;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests;

[TestClass]
public class InProcessVsTestConsoleWrapperTests
{
    private readonly InProcessVsTestConsoleWrapper _consoleWrapper;
    private readonly Mock<IEnvironmentVariableHelper> _mockEnvironmentVariableHelper;
    private readonly Mock<ITranslationLayerRequestSender> _mockRequestSender;
    private readonly Mock<ITestRequestManager> _mockTestRequestManager;
    private readonly Mock<IOutput> _mockOutput;
    private readonly Executor _executor;
    private readonly Mock<ITestPlatformEventSource> _mockEventSource;

    private readonly IList<string> _testSources = new List<String>() { "test1", "test2" };
    private readonly IList<TestCase> _testCases = new List<TestCase>() { new TestCase(), new TestCase() };
    private readonly IList<AttachmentSet> _attachmentSets = new List<AttachmentSet>()
    {
        new AttachmentSet(new Uri("datacollector://AttachmentSetDataCollector1"), "AttachmentSet1"),
        new AttachmentSet(new Uri("datacollector://AttachmentSetDataCollector2"), "AttachmentSet2"),
    };
    private readonly IList<InvokedDataCollector> _invokedDataCollectors = new List<InvokedDataCollector>()
    {
        new InvokedDataCollector(new Uri("datacollector://InvokedDataCollector1"), "InvokedDataCollector1", "DummyAssemblyName1", "DummyFilePath1", true),
        new InvokedDataCollector(new Uri("datacollector://InvokedDataCollector2"), "InvokedDataCollector2", "DummyAssemblyName2", "DummyFilePath2", false),
    };

    private readonly string _runSettings = "dummy runsettings";

    public InProcessVsTestConsoleWrapperTests()
    {
        _mockEnvironmentVariableHelper = new Mock<IEnvironmentVariableHelper>();
        _mockEnvironmentVariableHelper.Setup(evh => evh.GetEnvironmentVariables()).Returns(new Hashtable());

        _mockRequestSender = new Mock<ITranslationLayerRequestSender>();
        _mockRequestSender.Setup(rs => rs.InitializeCommunication()).Returns(1234);
        _mockRequestSender.Setup(rs => rs.WaitForRequestHandlerConnection(It.IsAny<int>())).Returns(true);

        _mockTestRequestManager = new Mock<ITestRequestManager>();
        _mockTestRequestManager.Setup(trm => trm.ResetOptions()).Callback(() => { });

        _mockOutput = new Mock<IOutput>();
        _executor = new Executor(_mockOutput.Object, new Mock<ITestPlatformEventSource>().Object, new ProcessHelper(), new PlatformEnvironment());
        _mockEventSource = new Mock<ITestPlatformEventSource>();

        _consoleWrapper = new InProcessVsTestConsoleWrapper(
            new ConsoleParameters(),
            _mockEnvironmentVariableHelper.Object,
            _mockRequestSender.Object,
            _mockTestRequestManager.Object,
            _executor,
            _mockEventSource.Object,
            new());
    }

    [TestMethod]
    public void InProcessWrapperConstructorShouldThrowIfPortIsInvalid()
    {
        _mockRequestSender.Setup(rs => rs.InitializeCommunication()).Returns(-1);

        Assert.ThrowsException<TransationLayerException>(() =>
            new InProcessVsTestConsoleWrapper(
                new ConsoleParameters(),
                _mockEnvironmentVariableHelper.Object,
                _mockRequestSender.Object,
                _mockTestRequestManager.Object,
                new Executor(_mockOutput.Object, new Mock<ITestPlatformEventSource>().Object, new ProcessHelper(), new PlatformEnvironment()),
                new Mock<ITestPlatformEventSource>().Object,
                new()));
    }

    [TestMethod]
    public void InProcessWrapperConstructorShouldSetEnvironmentVariablesReceivedAsConsoleParametersForProcessHelperNoInherit()
    {
        const string environmentVariableName = "AAAAA";

        var consoleParams = new ConsoleParameters();
        consoleParams.EnvironmentVariables.Add(environmentVariableName, "1");
        consoleParams.InheritEnvironmentVariables = false;

        var _ = new InProcessVsTestConsoleWrapper(
            consoleParams,
            _mockEnvironmentVariableHelper.Object,
            _mockRequestSender.Object,
            _mockTestRequestManager.Object,
            new Executor(_mockOutput.Object, new Mock<ITestPlatformEventSource>().Object, new ProcessHelper(), new PlatformEnvironment()),
            new Mock<ITestPlatformEventSource>().Object,
            new());

        Assert.IsTrue(ProcessHelper.ExternalEnvironmentVariables?.Count == 1);
        Assert.IsTrue(ProcessHelper.ExternalEnvironmentVariables?.ContainsKey(environmentVariableName));
        Assert.IsTrue(ProcessHelper.ExternalEnvironmentVariables?[environmentVariableName] == "1");
    }

    [TestMethod]
    public void InProcessWrapperConstructorShouldSetEnvironmentVariablesReceivedAsConsoleParametersForProcessHelper()
    {
        const string environmentVariableName1 = "AAAAA";
        const string environmentVariableName2 = "BBBBB";
        const string environmentVariableName3 = "CCCCC";

        var consoleParams = new ConsoleParameters();
        consoleParams.EnvironmentVariables.Add(environmentVariableName1, "1");
        consoleParams.InheritEnvironmentVariables = true;

        IDictionary defaultEnvironmentVariables = new Hashtable();
        defaultEnvironmentVariables.Add(environmentVariableName2, "1");
        defaultEnvironmentVariables.Add(environmentVariableName3, "1");

        _mockEnvironmentVariableHelper.Setup(evh => evh.GetEnvironmentVariables()).Returns(defaultEnvironmentVariables);

        var _ = new InProcessVsTestConsoleWrapper(
            consoleParams,
            _mockEnvironmentVariableHelper.Object,
            _mockRequestSender.Object,
            _mockTestRequestManager.Object,
            new Executor(_mockOutput.Object, new Mock<ITestPlatformEventSource>().Object, new ProcessHelper(), new PlatformEnvironment()),
            new Mock<ITestPlatformEventSource>().Object,
            new());

        Assert.IsTrue(ProcessHelper.ExternalEnvironmentVariables?.Count == 3);
        Assert.IsTrue(ProcessHelper.ExternalEnvironmentVariables?.ContainsKey(environmentVariableName1));
        Assert.IsTrue(ProcessHelper.ExternalEnvironmentVariables?[environmentVariableName1] == "1");
        Assert.IsTrue(ProcessHelper.ExternalEnvironmentVariables?.ContainsKey(environmentVariableName2));
        Assert.IsTrue(ProcessHelper.ExternalEnvironmentVariables?[environmentVariableName2] == "1");
        Assert.IsTrue(ProcessHelper.ExternalEnvironmentVariables?.ContainsKey(environmentVariableName3));
        Assert.IsTrue(ProcessHelper.ExternalEnvironmentVariables?[environmentVariableName3] == "1");
    }

    [TestMethod]
    public void InProcessWrapperConstructorShouldSetTheCultureSpecifiedByTheUser()
    {
        // Arrange
        var culture = new CultureInfo("fr-fr");
        _mockEnvironmentVariableHelper.Setup(x => x.GetEnvironmentVariable("DOTNET_CLI_UI_LANGUAGE")).Returns(culture.Name);

        bool threadCultureWasSet = false;

        // Act - We have an exception because we are not passing the right args but that's ok for our test
        var consoleParams = new ConsoleParameters();
        var _ = new InProcessVsTestConsoleWrapper(
            consoleParams,
            _mockEnvironmentVariableHelper.Object,
            _mockRequestSender.Object,
            _mockTestRequestManager.Object,
            new Executor(_mockOutput.Object, new Mock<ITestPlatformEventSource>().Object, new ProcessHelper(), new PlatformEnvironment()),
            new Mock<ITestPlatformEventSource>().Object,
            new UiLanguageOverride(_mockEnvironmentVariableHelper.Object, lang => threadCultureWasSet = lang.Equals(culture)));

        // Assert
        Assert.IsTrue(threadCultureWasSet, "DefaultThreadCurrentUICulture was not set");
        _mockEnvironmentVariableHelper.Verify(x => x.GetEnvironmentVariable("DOTNET_CLI_UI_LANGUAGE"), Times.Exactly(2));
        _mockEnvironmentVariableHelper.Verify(x => x.GetEnvironmentVariable("VSLANG"), Times.Once);
        _mockEnvironmentVariableHelper.Verify(x => x.SetEnvironmentVariable("VSLANG", culture.LCID.ToString(CultureInfo.InvariantCulture)), Times.Once);
        _mockEnvironmentVariableHelper.Verify(x => x.GetEnvironmentVariable("PreferredUILang"), Times.Once);
        _mockEnvironmentVariableHelper.Verify(x => x.SetEnvironmentVariable("PreferredUILang", culture.Name), Times.Once);
    }

    [TestMethod]
    public void InProcessWrapperDiscoverTestsWithThreeParamsIsSuccessfullyInvoked()
    {
        var discoveryEventHandler = new Mock<ITestDiscoveryEventsHandler>();

        DiscoveryRequestPayload? payload = null;
        _mockTestRequestManager
            .Setup(trm => trm.DiscoverTests(
                It.IsAny<DiscoveryRequestPayload>(),
                It.IsAny<ITestDiscoveryEventsRegistrar>(),
                It.IsAny<ProtocolConfig>()))
            .Callback((
                DiscoveryRequestPayload p,
                ITestDiscoveryEventsRegistrar _,
                ProtocolConfig _) => payload = p);

        _consoleWrapper.DiscoverTests(_testSources, _runSettings, discoveryEventHandler.Object);

        Assert.IsNotNull(payload);
        Assert.IsTrue(_testSources.SequenceEqual(payload.Sources!));
        Assert.AreEqual(_runSettings, payload.RunSettings);
        Assert.IsNull(payload.TestPlatformOptions);
        Assert.IsNull(payload.TestSessionInfo);

        _mockEventSource.Verify(es => es.TranslationLayerDiscoveryStart(), Times.Once);
        _mockEventSource.Verify(es => es.TranslationLayerDiscoveryStop(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.ResetOptions(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.DiscoverTests(
                It.IsAny<DiscoveryRequestPayload>(),
                It.IsAny<ITestDiscoveryEventsRegistrar>(),
                It.IsAny<ProtocolConfig>()), Times.Once);
    }

    [TestMethod]
    public void InProcessWrapperDiscoverTestsWithFourParamsIsSuccessfullyInvoked()
    {
        var options = new TestPlatformOptions();
        var discoveryEventHandler = new Mock<ITestDiscoveryEventsHandler2>();

        DiscoveryRequestPayload? payload = null;
        _mockTestRequestManager
            .Setup(trm => trm.DiscoverTests(
                It.IsAny<DiscoveryRequestPayload>(),
                It.IsAny<ITestDiscoveryEventsRegistrar>(),
                It.IsAny<ProtocolConfig>()))
            .Callback((
                DiscoveryRequestPayload p,
                ITestDiscoveryEventsRegistrar _,
                ProtocolConfig _) => payload = p);

        _consoleWrapper.DiscoverTests(_testSources, _runSettings, options, discoveryEventHandler.Object);

        Assert.IsNotNull(payload);
        Assert.IsTrue(_testSources.SequenceEqual(payload.Sources!));
        Assert.AreEqual(_runSettings, payload.RunSettings);
        Assert.AreEqual(options, payload.TestPlatformOptions);
        Assert.IsNull(payload.TestSessionInfo);

        _mockEventSource.Verify(es => es.TranslationLayerDiscoveryStart(), Times.Once);
        _mockEventSource.Verify(es => es.TranslationLayerDiscoveryStop(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.ResetOptions(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.DiscoverTests(
                It.IsAny<DiscoveryRequestPayload>(),
                It.IsAny<ITestDiscoveryEventsRegistrar>(),
                It.IsAny<ProtocolConfig>()), Times.Once);
    }

    [TestMethod]
    public void InProcessWrapperDiscoverTestsWithFiveParamsIsSuccessfullyInvoked()
    {
        var options = new TestPlatformOptions();
        var testSessionInfo = new TestSessionInfo();

        var discoveryEventHandler = new Mock<ITestDiscoveryEventsHandler2>();

        DiscoveryRequestPayload? payload = null;
        _mockTestRequestManager
            .Setup(trm => trm.DiscoverTests(
                It.IsAny<DiscoveryRequestPayload>(),
                It.IsAny<ITestDiscoveryEventsRegistrar>(),
                It.IsAny<ProtocolConfig>()))
            .Callback((
                DiscoveryRequestPayload p,
                ITestDiscoveryEventsRegistrar _,
                ProtocolConfig _) => payload = p);

        _consoleWrapper.DiscoverTests(_testSources, _runSettings, options, testSessionInfo, discoveryEventHandler.Object);

        Assert.IsNotNull(payload);
        Assert.IsTrue(_testSources.SequenceEqual(payload.Sources!));
        Assert.AreEqual(_runSettings, payload.RunSettings);
        Assert.AreEqual(options, payload.TestPlatformOptions);
        Assert.AreEqual(testSessionInfo.Id, payload.TestSessionInfo?.Id);

        _mockEventSource.Verify(es => es.TranslationLayerDiscoveryStart(), Times.Once);
        _mockEventSource.Verify(es => es.TranslationLayerDiscoveryStop(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.ResetOptions(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.DiscoverTests(
                It.IsAny<DiscoveryRequestPayload>(),
                It.IsAny<ITestDiscoveryEventsRegistrar>(),
                It.IsAny<ProtocolConfig>()), Times.Once);
    }

    [TestMethod]
    public void InProcessWrapperRunTestsWithSourcesWithThreeParamsIsSuccessfullyInvoked()
    {
        var runEventHandler = new Mock<ITestRunEventsHandler>();

        TestRunRequestPayload? payload = null;
        _mockTestRequestManager
            .Setup(trm => trm.RunTests(
                It.IsAny<TestRunRequestPayload>(),
                It.IsAny<ITestHostLauncher3>(),
                It.IsAny<ITestRunEventsRegistrar>(),
                It.IsAny<ProtocolConfig>()))
            .Callback((
                TestRunRequestPayload p,
                ITestHostLauncher3 _,
                ITestRunEventsRegistrar _,
                ProtocolConfig _) => payload = p);

        _consoleWrapper.RunTests(_testSources, _runSettings, runEventHandler.Object);

        Assert.IsNotNull(payload);
        Assert.IsTrue(_testSources.SequenceEqual(payload.Sources!));
        Assert.AreEqual(_runSettings, payload.RunSettings);
        Assert.IsNull(payload.TestPlatformOptions);
        Assert.IsNull(payload.TestSessionInfo?.Id);

        _mockEventSource.Verify(es =>
            es.TranslationLayerExecutionStart(
                0,
                0,
                _testSources.Count,
                _runSettings),
            Times.Once);
        _mockEventSource.Verify(es => es.TranslationLayerExecutionStop(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.ResetOptions(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.RunTests(
                It.IsAny<TestRunRequestPayload>(),
                It.IsAny<ITestHostLauncher3>(),
                It.IsAny<ITestRunEventsRegistrar>(),
                It.IsAny<ProtocolConfig>()), Times.Once);
    }

    [TestMethod]
    public void InProcessWrapperRunTestsWithSourcesWithFourParamsIsSuccessfullyInvoked()
    {
        var options = new TestPlatformOptions();

        var runEventHandler = new Mock<ITestRunEventsHandler>();

        TestRunRequestPayload? payload = null;
        _mockTestRequestManager
            .Setup(trm => trm.RunTests(
                It.IsAny<TestRunRequestPayload>(),
                It.IsAny<ITestHostLauncher3>(),
                It.IsAny<ITestRunEventsRegistrar>(),
                It.IsAny<ProtocolConfig>()))
            .Callback((
                TestRunRequestPayload p,
                ITestHostLauncher3 _,
                ITestRunEventsRegistrar _,
                ProtocolConfig _) => payload = p);

        _consoleWrapper.RunTests(_testSources, _runSettings, options, runEventHandler.Object);

        Assert.IsNotNull(payload);
        Assert.IsTrue(_testSources.SequenceEqual(payload.Sources!));
        Assert.AreEqual(_runSettings, payload.RunSettings);
        Assert.AreEqual(options, payload.TestPlatformOptions);
        Assert.IsNull(payload.TestSessionInfo?.Id);

        _mockEventSource.Verify(es =>
            es.TranslationLayerExecutionStart(
                0,
                0,
                _testSources.Count,
                _runSettings),
            Times.Once);
        _mockEventSource.Verify(es => es.TranslationLayerExecutionStop(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.ResetOptions(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.RunTests(
                It.IsAny<TestRunRequestPayload>(),
                It.IsAny<ITestHostLauncher3>(),
                It.IsAny<ITestRunEventsRegistrar>(),
                It.IsAny<ProtocolConfig>()), Times.Once);
    }

    [TestMethod]
    public void InProcessWrapperRunTestsWithSourcesWithFiveParamsIsSuccessfullyInvoked()
    {
        var options = new TestPlatformOptions();
        var testSessionInfo = new TestSessionInfo();

        var runEventHandler = new Mock<ITestRunEventsHandler>();

        TestRunRequestPayload? payload = null;
        _mockTestRequestManager
            .Setup(trm => trm.RunTests(
                It.IsAny<TestRunRequestPayload>(),
                It.IsAny<ITestHostLauncher3>(),
                It.IsAny<ITestRunEventsRegistrar>(),
                It.IsAny<ProtocolConfig>()))
            .Callback((
                TestRunRequestPayload p,
                ITestHostLauncher3 _,
                ITestRunEventsRegistrar _,
                ProtocolConfig _) => payload = p);

        _consoleWrapper.RunTests(_testSources, _runSettings, options, testSessionInfo, runEventHandler.Object);

        Assert.IsNotNull(payload);
        Assert.IsTrue(_testSources.SequenceEqual(payload.Sources!));
        Assert.AreEqual(_runSettings, payload.RunSettings);
        Assert.AreEqual(options, payload.TestPlatformOptions);
        Assert.AreEqual(testSessionInfo.Id, payload.TestSessionInfo?.Id);

        _mockEventSource.Verify(es =>
            es.TranslationLayerExecutionStart(
                0,
                0,
                _testSources.Count,
                _runSettings),
            Times.Once);
        _mockEventSource.Verify(es => es.TranslationLayerExecutionStop(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.ResetOptions(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.RunTests(
                It.IsAny<TestRunRequestPayload>(),
                It.IsAny<ITestHostLauncher3>(),
                It.IsAny<ITestRunEventsRegistrar>(),
                It.IsAny<ProtocolConfig>()), Times.Once);
    }

    [TestMethod]
    public void InProcessWrapperRunTestsWithTestsWithThreeParamsIsSuccessfullyInvoked()
    {
        var runEventHandler = new Mock<ITestRunEventsHandler>();

        TestRunRequestPayload? payload = null;
        _mockTestRequestManager
            .Setup(trm => trm.RunTests(
                It.IsAny<TestRunRequestPayload>(),
                It.IsAny<ITestHostLauncher3>(),
                It.IsAny<ITestRunEventsRegistrar>(),
                It.IsAny<ProtocolConfig>()))
            .Callback((
                TestRunRequestPayload p,
                ITestHostLauncher3 _,
                ITestRunEventsRegistrar _,
                ProtocolConfig _) => payload = p);

        _consoleWrapper.RunTests(_testCases, _runSettings, runEventHandler.Object);

        Assert.IsNotNull(payload);
        Assert.IsTrue(_testCases.SequenceEqual(payload.TestCases!));
        Assert.AreEqual(_runSettings, payload.RunSettings);
        Assert.IsNull(payload.TestPlatformOptions);
        Assert.IsNull(payload.TestSessionInfo?.Id);

        _mockEventSource.Verify(es =>
            es.TranslationLayerExecutionStart(
                0,
                0,
                _testCases.Count,
                _runSettings),
            Times.Once);
        _mockEventSource.Verify(es => es.TranslationLayerExecutionStop(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.ResetOptions(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.RunTests(
                It.IsAny<TestRunRequestPayload>(),
                It.IsAny<ITestHostLauncher3>(),
                It.IsAny<ITestRunEventsRegistrar>(),
                It.IsAny<ProtocolConfig>()), Times.Once);
    }

    [TestMethod]
    public void InProcessWrapperRunTestsWithTestsWithFourParamsIsSuccessfullyInvoked()
    {
        var options = new TestPlatformOptions();

        var runEventHandler = new Mock<ITestRunEventsHandler>();

        TestRunRequestPayload? payload = null;
        _mockTestRequestManager
            .Setup(trm => trm.RunTests(
                It.IsAny<TestRunRequestPayload>(),
                It.IsAny<ITestHostLauncher3>(),
                It.IsAny<ITestRunEventsRegistrar>(),
                It.IsAny<ProtocolConfig>()))
            .Callback((
                TestRunRequestPayload p,
                ITestHostLauncher3 _,
                ITestRunEventsRegistrar _,
                ProtocolConfig _) => payload = p);

        _consoleWrapper.RunTests(_testCases, _runSettings, options, runEventHandler.Object);

        Assert.IsNotNull(payload);
        Assert.IsTrue(_testCases.SequenceEqual(payload.TestCases!));
        Assert.AreEqual(_runSettings, payload.RunSettings);
        Assert.AreEqual(options, payload.TestPlatformOptions);
        Assert.IsNull(payload.TestSessionInfo?.Id);

        _mockEventSource.Verify(es =>
            es.TranslationLayerExecutionStart(
                0,
                0,
                _testCases.Count,
                _runSettings),
            Times.Once);
        _mockEventSource.Verify(es => es.TranslationLayerExecutionStop(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.ResetOptions(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.RunTests(
                It.IsAny<TestRunRequestPayload>(),
                It.IsAny<ITestHostLauncher3>(),
                It.IsAny<ITestRunEventsRegistrar>(),
                It.IsAny<ProtocolConfig>()), Times.Once);
    }

    [TestMethod]
    public void InProcessWrapperRunTestsWithTestsWithFiveParamsIsSuccessfullyInvoked()
    {
        var options = new TestPlatformOptions();
        var testSessionInfo = new TestSessionInfo();

        var runEventHandler = new Mock<ITestRunEventsHandler>();

        TestRunRequestPayload? payload = null;
        _mockTestRequestManager
            .Setup(trm => trm.RunTests(
                It.IsAny<TestRunRequestPayload>(),
                It.IsAny<ITestHostLauncher3>(),
                It.IsAny<ITestRunEventsRegistrar>(),
                It.IsAny<ProtocolConfig>()))
            .Callback((
                TestRunRequestPayload p,
                ITestHostLauncher3 _,
                ITestRunEventsRegistrar _,
                ProtocolConfig _) => payload = p);

        _consoleWrapper.RunTests(_testCases, _runSettings, options, testSessionInfo, runEventHandler.Object);

        Assert.IsNotNull(payload);
        Assert.IsTrue(_testCases.SequenceEqual(payload.TestCases!));
        Assert.AreEqual(_runSettings, payload.RunSettings);
        Assert.AreEqual(options, payload.TestPlatformOptions);
        Assert.AreEqual(testSessionInfo.Id, payload.TestSessionInfo?.Id);

        _mockEventSource.Verify(es =>
            es.TranslationLayerExecutionStart(
                0,
                0,
                _testCases.Count,
                _runSettings),
            Times.Once);
        _mockEventSource.Verify(es => es.TranslationLayerExecutionStop(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.ResetOptions(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.RunTests(
                It.IsAny<TestRunRequestPayload>(),
                It.IsAny<ITestHostLauncher3>(),
                It.IsAny<ITestRunEventsRegistrar>(),
                It.IsAny<ProtocolConfig>()), Times.Once);
    }

    [TestMethod]
    public void InProcessWrapperRunTestsWithCustomTestHostWithSourcesWithThreeParamsIsSuccessfullyInvoked()
    {
        var testHostLauncher = new Mock<ITestHostLauncher3>();
        var runEventHandler = new Mock<ITestRunEventsHandler>();

        TestRunRequestPayload? payload = null;
        ITestHostLauncher3? launcher = null;
        _mockTestRequestManager
            .Setup(trm => trm.RunTests(
                It.IsAny<TestRunRequestPayload>(),
                It.IsAny<ITestHostLauncher3>(),
                It.IsAny<ITestRunEventsRegistrar>(),
                It.IsAny<ProtocolConfig>()))
            .Callback((
                TestRunRequestPayload p,
                ITestHostLauncher3 l,
                ITestRunEventsRegistrar _,
                ProtocolConfig _) =>
            {
                payload = p;
                launcher = l;
            });

        _consoleWrapper.RunTestsWithCustomTestHost(_testSources, _runSettings, runEventHandler.Object, testHostLauncher.Object);

        Assert.IsNotNull(payload);
        Assert.IsTrue(_testSources.SequenceEqual(payload.Sources!));
        Assert.AreEqual(_runSettings, payload.RunSettings);
        Assert.IsNull(payload.TestPlatformOptions);
        Assert.IsNull(payload.TestSessionInfo?.Id);
        Assert.AreEqual(testHostLauncher.Object.IsDebug, payload.DebuggingEnabled);
        Assert.AreEqual(testHostLauncher.Object, launcher);

        _mockEventSource.Verify(es =>
            es.TranslationLayerExecutionStart(
                1,
                _testSources.Count,
                0,
                _runSettings),
            Times.Once);
        _mockEventSource.Verify(es => es.TranslationLayerExecutionStop(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.ResetOptions(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.RunTests(
                It.IsAny<TestRunRequestPayload>(),
                testHostLauncher.Object,
                It.IsAny<ITestRunEventsRegistrar>(),
                It.IsAny<ProtocolConfig>()), Times.Once);
    }

    [TestMethod]
    public void InProcessWrapperRunTestsWithCustomTestHostWithSourcesWithFourParamsIsSuccessfullyInvoked()
    {
        var options = new TestPlatformOptions();

        var testHostLauncher = new Mock<ITestHostLauncher3>();
        var runEventHandler = new Mock<ITestRunEventsHandler>();

        TestRunRequestPayload? payload = null;
        ITestHostLauncher3? launcher = null;
        _mockTestRequestManager
            .Setup(trm => trm.RunTests(
                It.IsAny<TestRunRequestPayload>(),
                It.IsAny<ITestHostLauncher3>(),
                It.IsAny<ITestRunEventsRegistrar>(),
                It.IsAny<ProtocolConfig>()))
            .Callback((
                TestRunRequestPayload p,
                ITestHostLauncher3 l,
                ITestRunEventsRegistrar _,
                ProtocolConfig _) =>
            {
                payload = p;
                launcher = l;
            });

        _consoleWrapper.RunTestsWithCustomTestHost(_testSources, _runSettings, options, runEventHandler.Object, testHostLauncher.Object);

        Assert.IsNotNull(payload);
        Assert.IsTrue(_testSources.SequenceEqual(payload.Sources!));
        Assert.AreEqual(_runSettings, payload.RunSettings);
        Assert.AreEqual(options, payload.TestPlatformOptions);
        Assert.IsNull(payload.TestSessionInfo?.Id);
        Assert.AreEqual(testHostLauncher.Object.IsDebug, payload.DebuggingEnabled);
        Assert.AreEqual(testHostLauncher.Object, launcher);

        _mockEventSource.Verify(es =>
            es.TranslationLayerExecutionStart(
                1,
                _testSources.Count,
                0,
                _runSettings),
            Times.Once);
        _mockEventSource.Verify(es => es.TranslationLayerExecutionStop(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.ResetOptions(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.RunTests(
                It.IsAny<TestRunRequestPayload>(),
                testHostLauncher.Object,
                It.IsAny<ITestRunEventsRegistrar>(),
                It.IsAny<ProtocolConfig>()), Times.Once);
    }

    [TestMethod]
    public void InProcessWrapperRunTestsWithCustomTestHostWithSourcesWithFiveParamsIsSuccessfullyInvoked()
    {
        var options = new TestPlatformOptions();
        var testSessionInfo = new TestSessionInfo();

        var testHostLauncher = new Mock<ITestHostLauncher3>();
        var runEventHandler = new Mock<ITestRunEventsHandler>();

        TestRunRequestPayload? payload = null;
        ITestHostLauncher3? launcher = null;
        _mockTestRequestManager
            .Setup(trm => trm.RunTests(
                It.IsAny<TestRunRequestPayload>(),
                It.IsAny<ITestHostLauncher3>(),
                It.IsAny<ITestRunEventsRegistrar>(),
                It.IsAny<ProtocolConfig>()))
            .Callback((
                TestRunRequestPayload p,
                ITestHostLauncher3 l,
                ITestRunEventsRegistrar _,
                ProtocolConfig _) =>
            {
                payload = p;
                launcher = l;
            });

        _consoleWrapper.RunTestsWithCustomTestHost(_testSources, _runSettings, options, testSessionInfo, runEventHandler.Object, testHostLauncher.Object);

        Assert.IsNotNull(payload);
        Assert.IsTrue(_testSources.SequenceEqual(payload.Sources!));
        Assert.AreEqual(_runSettings, payload.RunSettings);
        Assert.AreEqual(options, payload.TestPlatformOptions);
        Assert.AreEqual(testSessionInfo.Id, payload.TestSessionInfo?.Id);
        Assert.AreEqual(testHostLauncher.Object.IsDebug, payload.DebuggingEnabled);
        Assert.IsNull(launcher);

        _mockEventSource.Verify(es =>
            es.TranslationLayerExecutionStart(
                1,
                _testSources.Count,
                0,
                _runSettings),
            Times.Once);
        _mockEventSource.Verify(es => es.TranslationLayerExecutionStop(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.ResetOptions(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.RunTests(
                It.IsAny<TestRunRequestPayload>(),
                null,
                It.IsAny<ITestRunEventsRegistrar>(),
                It.IsAny<ProtocolConfig>()), Times.Once);
    }

    [TestMethod]
    public void InProcessWrapperRunTestsWithCustomTestHostWithTestsWithThreeParamsIsSuccessfullyInvoked()
    {
        var testHostLauncher = new Mock<ITestHostLauncher3>();
        var runEventHandler = new Mock<ITestRunEventsHandler>();

        TestRunRequestPayload? payload = null;
        ITestHostLauncher3? launcher = null;
        _mockTestRequestManager
            .Setup(trm => trm.RunTests(
                It.IsAny<TestRunRequestPayload>(),
                It.IsAny<ITestHostLauncher3>(),
                It.IsAny<ITestRunEventsRegistrar>(),
                It.IsAny<ProtocolConfig>()))
            .Callback((
                TestRunRequestPayload p,
                ITestHostLauncher3 l,
                ITestRunEventsRegistrar _,
                ProtocolConfig _) =>
            {
                payload = p;
                launcher = l;
            });

        _consoleWrapper.RunTestsWithCustomTestHost(_testCases, _runSettings, runEventHandler.Object, testHostLauncher.Object);

        Assert.IsNotNull(payload);
        Assert.IsTrue(_testCases.SequenceEqual(payload.TestCases!));
        Assert.AreEqual(_runSettings, payload.RunSettings);
        Assert.IsNull(payload.TestPlatformOptions);
        Assert.IsNull(payload.TestSessionInfo?.Id);
        Assert.AreEqual(testHostLauncher.Object.IsDebug, payload.DebuggingEnabled);
        Assert.AreEqual(testHostLauncher.Object, launcher);

        _mockEventSource.Verify(es =>
            es.TranslationLayerExecutionStart(
                1,
                0,
                _testCases.Count,
                _runSettings),
            Times.Once);
        _mockEventSource.Verify(es => es.TranslationLayerExecutionStop(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.ResetOptions(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.RunTests(
                It.IsAny<TestRunRequestPayload>(),
                testHostLauncher.Object,
                It.IsAny<ITestRunEventsRegistrar>(),
                It.IsAny<ProtocolConfig>()), Times.Once);
    }

    [TestMethod]
    public void InProcessWrapperRunTestsWithCustomTestHostWithTestsWithFourParamsIsSuccessfullyInvoked()
    {
        var options = new TestPlatformOptions();

        var testHostLauncher = new Mock<ITestHostLauncher3>();
        var runEventHandler = new Mock<ITestRunEventsHandler>();

        TestRunRequestPayload? payload = null;
        ITestHostLauncher3? launcher = null;
        _mockTestRequestManager
            .Setup(trm => trm.RunTests(
                It.IsAny<TestRunRequestPayload>(),
                It.IsAny<ITestHostLauncher3>(),
                It.IsAny<ITestRunEventsRegistrar>(),
                It.IsAny<ProtocolConfig>()))
            .Callback((
                TestRunRequestPayload p,
                ITestHostLauncher3 l,
                ITestRunEventsRegistrar _,
                ProtocolConfig _) =>
            {
                payload = p;
                launcher = l;
            });

        _consoleWrapper.RunTestsWithCustomTestHost(_testCases, _runSettings, options, runEventHandler.Object, testHostLauncher.Object);

        Assert.IsNotNull(payload);
        Assert.IsTrue(_testCases.SequenceEqual(payload.TestCases!));
        Assert.AreEqual(_runSettings, payload.RunSettings);
        Assert.AreEqual(options, payload.TestPlatformOptions);
        Assert.IsNull(payload.TestSessionInfo?.Id);
        Assert.AreEqual(testHostLauncher.Object.IsDebug, payload.DebuggingEnabled);
        Assert.AreEqual(testHostLauncher.Object, launcher);

        _mockEventSource.Verify(es =>
            es.TranslationLayerExecutionStart(
                1,
                0,
                _testCases.Count,
                _runSettings),
            Times.Once);
        _mockEventSource.Verify(es => es.TranslationLayerExecutionStop(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.ResetOptions(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.RunTests(
                It.IsAny<TestRunRequestPayload>(),
                testHostLauncher.Object,
                It.IsAny<ITestRunEventsRegistrar>(),
                It.IsAny<ProtocolConfig>()), Times.Once);
    }

    [TestMethod]
    public void InProcessWrapperRunTestsWithCustomTestHostWithTestsWithFiveParamsIsSuccessfullyInvoked()
    {
        var options = new TestPlatformOptions();
        var testSessionInfo = new TestSessionInfo();

        var testHostLauncher = new Mock<ITestHostLauncher3>();
        var runEventHandler = new Mock<ITestRunEventsHandler>();

        TestRunRequestPayload? payload = null;
        ITestHostLauncher3? launcher = null;
        _mockTestRequestManager
            .Setup(trm => trm.RunTests(
                It.IsAny<TestRunRequestPayload>(),
                It.IsAny<ITestHostLauncher3>(),
                It.IsAny<ITestRunEventsRegistrar>(),
                It.IsAny<ProtocolConfig>()))
            .Callback((
                TestRunRequestPayload p,
                ITestHostLauncher3 l,
                ITestRunEventsRegistrar _,
                ProtocolConfig _) =>
            {
                payload = p;
                launcher = l;
            });

        _consoleWrapper.RunTestsWithCustomTestHost(_testCases, _runSettings, options, testSessionInfo, runEventHandler.Object, testHostLauncher.Object);

        Assert.IsNotNull(payload);
        Assert.IsTrue(_testCases.SequenceEqual(payload.TestCases!));
        Assert.AreEqual(_runSettings, payload.RunSettings);
        Assert.AreEqual(options, payload.TestPlatformOptions);
        Assert.AreEqual(testSessionInfo.Id, payload.TestSessionInfo?.Id);
        Assert.AreEqual(testHostLauncher.Object.IsDebug, payload.DebuggingEnabled);
        Assert.IsNull(launcher);

        _mockEventSource.Verify(es =>
            es.TranslationLayerExecutionStart(
                1,
                0,
                _testCases.Count,
                _runSettings),
            Times.Once);
        _mockEventSource.Verify(es => es.TranslationLayerExecutionStop(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.ResetOptions(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.RunTests(
                It.IsAny<TestRunRequestPayload>(),
                null,
                It.IsAny<ITestRunEventsRegistrar>(),
                It.IsAny<ProtocolConfig>()), Times.Once);
    }

    [TestMethod]
    [Obsolete("Test uses obsolete API")]
    public void InProcessWrapperStartTestSessionSucceedsWhenNoExceptionIsThrown()
    {
        var mockTestSessionEventsHandler = new Mock<ITestSessionEventsHandler>();
        mockTestSessionEventsHandler
            .Setup(eh => eh.HandleStartTestSessionComplete(It.IsAny<StartTestSessionCompleteEventArgs>()))
            .Callback(() => { });

        var testSessionInfo = new TestSessionInfo();
        var startTestSessionCompleteArgs = new StartTestSessionCompleteEventArgs()
        {
            TestSessionInfo = testSessionInfo
        };

        var stopTestSessionCompleteArgs = new StopTestSessionCompleteEventArgs()
        {
            IsStopped = true,
            TestSessionInfo = testSessionInfo
        };

        _mockTestRequestManager.Setup(trm =>
            trm.StartTestSession(
                It.IsAny<StartTestSessionPayload>(),
                It.IsAny<ITestHostLauncher3>(),
                It.IsAny<InProcessTestSessionEventsHandler>(),
                It.IsAny<ProtocolConfig>()))
            .Callback<StartTestSessionPayload, ITestHostLauncher3, ITestSessionEventsHandler, ProtocolConfig>((
                StartTestSessionPayload _,
                ITestHostLauncher3 _,
                ITestSessionEventsHandler eventsHandler,
                ProtocolConfig _) =>
                    eventsHandler.HandleStartTestSessionComplete(startTestSessionCompleteArgs));

        _mockTestRequestManager.Setup(trm =>
            trm.StopTestSession(
                It.IsAny<StopTestSessionPayload>(),
                It.IsAny<InProcessTestSessionEventsHandler>(),
                It.IsAny<ProtocolConfig>()))
            .Callback<StopTestSessionPayload, ITestSessionEventsHandler, ProtocolConfig>((
                StopTestSessionPayload _,
                ITestSessionEventsHandler eventsHandler,
                ProtocolConfig _) =>
                    eventsHandler.HandleStopTestSessionComplete(stopTestSessionCompleteArgs));

        var consoleWrapper = new InProcessVsTestConsoleWrapper(
            new ConsoleParameters(),
            _mockEnvironmentVariableHelper.Object,
            _mockRequestSender.Object,
            _mockTestRequestManager.Object,
            new Executor(_mockOutput.Object, new Mock<ITestPlatformEventSource>().Object, new ProcessHelper(), new PlatformEnvironment()),
            new Mock<ITestPlatformEventSource>().Object,
            new());

        using (var testSession = consoleWrapper?.StartTestSession(_testSources, _runSettings, mockTestSessionEventsHandler.Object))
        {
            Assert.AreEqual(
                testSession?.TestSessionInfo,
                testSessionInfo);
        }

        _mockTestRequestManager.Verify(trm => trm.ResetOptions(), Times.Exactly(2));
        _mockTestRequestManager.Verify(trm => trm.StartTestSession(
             It.IsAny<StartTestSessionPayload>(),
             It.IsAny<ITestHostLauncher3>(),
             It.IsAny<InProcessTestSessionEventsHandler>(),
             It.IsAny<ProtocolConfig>()), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.StopTestSession(
             It.IsAny<StopTestSessionPayload>(),
             It.IsAny<InProcessTestSessionEventsHandler>(),
             It.IsAny<ProtocolConfig>()), Times.Once);

        mockTestSessionEventsHandler.Verify(eh => eh.HandleStartTestSessionComplete(startTestSessionCompleteArgs), Times.Once);
        mockTestSessionEventsHandler.Verify(eh => eh.HandleStopTestSessionComplete(stopTestSessionCompleteArgs), Times.Once);
    }

    [TestMethod]
    public async Task InProcessWrapperProcessTestRunAttachmentsAsyncWithSevenParamsIsSuccessfullyInvoked()
    {
        var attachmentsEventHandler = new Mock<ITestRunAttachmentsProcessingEventsHandler>();

        TestRunAttachmentsProcessingPayload? payload = null;
        _mockTestRequestManager
            .Setup(trm => trm.ProcessTestRunAttachments(
                It.IsAny<TestRunAttachmentsProcessingPayload>(),
                It.IsAny<ITestRunAttachmentsProcessingEventsHandler>(),
                It.IsAny<ProtocolConfig>()))
            .Callback((
                TestRunAttachmentsProcessingPayload p,
                ITestRunAttachmentsProcessingEventsHandler _,
                ProtocolConfig _) => payload = p);

        await _consoleWrapper.ProcessTestRunAttachmentsAsync(
            _attachmentSets,
            _invokedDataCollectors,
            _runSettings,
            true,
            false,
            attachmentsEventHandler.Object,
            CancellationToken.None).ConfigureAwait(false);

        Assert.IsNotNull(payload);
        Assert.IsTrue(_attachmentSets.SequenceEqual(payload.Attachments!));
        Assert.IsTrue(_invokedDataCollectors.SequenceEqual(payload.InvokedDataCollectors!));
        Assert.AreEqual(_runSettings, payload.RunSettings);
        Assert.IsFalse(payload.CollectMetrics);

        _mockEventSource.Verify(es => es.TranslationLayerTestRunAttachmentsProcessingStart(), Times.Once);
        _mockEventSource.Verify(es => es.TranslationLayerTestRunAttachmentsProcessingStop(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.ResetOptions(), Times.Never);
        _mockTestRequestManager.Verify(trm => trm.ProcessTestRunAttachments(
                It.IsAny<TestRunAttachmentsProcessingPayload>(),
                It.IsAny<ITestRunAttachmentsProcessingEventsHandler>(),
                It.IsAny<ProtocolConfig>()), Times.Once);
    }

    [TestMethod]
    public async Task InProcessWrapperProcessTestRunAttachmentsAsyncWithSevenParamsSuccessfullyHandlesCancellation()
    {
        var attachmentsEventHandler = new Mock<ITestRunAttachmentsProcessingEventsHandler>();

        TestRunAttachmentsProcessingPayload? payload = null;
        _mockTestRequestManager
            .Setup(trm => trm.ProcessTestRunAttachments(
                It.IsAny<TestRunAttachmentsProcessingPayload>(),
                It.IsAny<ITestRunAttachmentsProcessingEventsHandler>(),
                It.IsAny<ProtocolConfig>()))
            .Callback((
                TestRunAttachmentsProcessingPayload p,
                ITestRunAttachmentsProcessingEventsHandler _,
                ProtocolConfig _) => payload = p);
        _mockTestRequestManager.Setup(trm => trm.CancelTestRunAttachmentsProcessing())
            .Callback(() => { });

        var cancellationTokenSource = new CancellationTokenSource();

        cancellationTokenSource.Cancel();
        await _consoleWrapper.ProcessTestRunAttachmentsAsync(
            _attachmentSets,
            _invokedDataCollectors,
            _runSettings,
            true,
            false,
            attachmentsEventHandler.Object,
            cancellationTokenSource.Token).ConfigureAwait(false);

        Assert.IsNotNull(payload);
        Assert.IsTrue(_attachmentSets.SequenceEqual(payload.Attachments!));
        Assert.IsTrue(_invokedDataCollectors.SequenceEqual(payload.InvokedDataCollectors!));
        Assert.AreEqual(_runSettings, payload.RunSettings);
        Assert.IsFalse(payload.CollectMetrics);

        _mockEventSource.Verify(es => es.TranslationLayerTestRunAttachmentsProcessingStart(), Times.Once);
        _mockEventSource.Verify(es => es.TranslationLayerTestRunAttachmentsProcessingStop(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.ResetOptions(), Times.Never);
        _mockTestRequestManager.Verify(trm => trm.ProcessTestRunAttachments(
                It.IsAny<TestRunAttachmentsProcessingPayload>(),
                It.IsAny<ITestRunAttachmentsProcessingEventsHandler>(),
                It.IsAny<ProtocolConfig>()), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.CancelTestRunAttachmentsProcessing(), Times.Once);
    }

    [TestMethod]
    public async Task InProcessWrapperProcessTestRunAttachmentsAsyncWithSevenParamsSuccessfullyHandlesExceptions()
    {
        var attachmentsEventHandler = new Mock<ITestRunAttachmentsProcessingEventsHandler>();

        _mockTestRequestManager
            .Setup(trm => trm.ProcessTestRunAttachments(
                It.IsAny<TestRunAttachmentsProcessingPayload>(),
                It.IsAny<ITestRunAttachmentsProcessingEventsHandler>(),
                It.IsAny<ProtocolConfig>()))
            .Throws(new Exception("Dummy exception"));

        await _consoleWrapper.ProcessTestRunAttachmentsAsync(
            _attachmentSets,
            _invokedDataCollectors,
            _runSettings,
            true,
            false,
            attachmentsEventHandler.Object,
            CancellationToken.None).ConfigureAwait(false);

        _mockEventSource.Verify(es => es.TranslationLayerTestRunAttachmentsProcessingStart(), Times.Once);
        _mockEventSource.Verify(es => es.TranslationLayerTestRunAttachmentsProcessingStop(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.ResetOptions(), Times.Never);
        _mockTestRequestManager.Verify(trm => trm.ProcessTestRunAttachments(
                It.IsAny<TestRunAttachmentsProcessingPayload>(),
                It.IsAny<ITestRunAttachmentsProcessingEventsHandler>(),
                It.IsAny<ProtocolConfig>()), Times.Once);
        attachmentsEventHandler.Verify(eh => eh.HandleLogMessage(TestMessageLevel.Error, It.IsAny<string>()));
    }

    [TestMethod]
    public async Task InProcessWrapperProcessTestRunAttachmentsAsyncWithSevenParamsSuccessfullyHandlesNullTestRequestManager()
    {
        var attachmentsEventHandler = new Mock<ITestRunAttachmentsProcessingEventsHandler>();

        _consoleWrapper.TestRequestManager = null;

        await _consoleWrapper.ProcessTestRunAttachmentsAsync(
            _attachmentSets,
            _invokedDataCollectors,
            _runSettings,
            true,
            false,
            attachmentsEventHandler.Object,
            CancellationToken.None).ConfigureAwait(false);

        _mockEventSource.Verify(es => es.TranslationLayerTestRunAttachmentsProcessingStart(), Times.Once);
        _mockEventSource.Verify(es => es.TranslationLayerTestRunAttachmentsProcessingStop(), Times.Once);
    }

    [TestMethod]
    public async Task InProcessWrapperProcessTestRunAttachmentsAsyncWithSixParamsIsSuccessfullyInvoked()
    {
        var attachmentsEventHandler = new Mock<ITestRunAttachmentsProcessingEventsHandler>();

        TestRunAttachmentsProcessingPayload? payload = null;
        _mockTestRequestManager
            .Setup(trm => trm.ProcessTestRunAttachments(
                It.IsAny<TestRunAttachmentsProcessingPayload>(),
                It.IsAny<ITestRunAttachmentsProcessingEventsHandler>(),
                It.IsAny<ProtocolConfig>()))
            .Callback((
                TestRunAttachmentsProcessingPayload p,
                ITestRunAttachmentsProcessingEventsHandler _,
                ProtocolConfig _) => payload = p);

        await _consoleWrapper.ProcessTestRunAttachmentsAsync(
            _attachmentSets,
            _runSettings,
            true,
            false,
            attachmentsEventHandler.Object,
            CancellationToken.None);

        Assert.IsNotNull(payload);
        Assert.IsTrue(_attachmentSets.SequenceEqual(payload.Attachments!));
        Assert.IsNull(payload.InvokedDataCollectors);
        Assert.AreEqual(_runSettings, payload.RunSettings);
        Assert.IsFalse(payload.CollectMetrics);

        _mockEventSource.Verify(es => es.TranslationLayerTestRunAttachmentsProcessingStart(), Times.Once);
        _mockEventSource.Verify(es => es.TranslationLayerTestRunAttachmentsProcessingStop(), Times.Once);
        _mockTestRequestManager.Verify(trm => trm.ResetOptions(), Times.Never);
        _mockTestRequestManager.Verify(trm => trm.ProcessTestRunAttachments(
                It.IsAny<TestRunAttachmentsProcessingPayload>(),
                It.IsAny<ITestRunAttachmentsProcessingEventsHandler>(),
                It.IsAny<ProtocolConfig>()), Times.Once);
    }
}
