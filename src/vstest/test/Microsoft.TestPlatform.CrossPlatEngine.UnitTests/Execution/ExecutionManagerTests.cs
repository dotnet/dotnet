// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Xml;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;
using Microsoft.VisualStudio.TestPlatform.Common.Logging;
using Microsoft.VisualStudio.TestPlatform.Common.SettingsProvider;
using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Execution;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.ClientProtocol;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using FluentAssertions;

using static TestPlatform.CrossPlatEngine.UnitTests.Execution.RunTestsWithSourcesTests;

namespace TestPlatform.CrossPlatEngine.UnitTests.Execution;

[TestClass]
public class ExecutionManagerTests
{
    private readonly ExecutionManager _executionManager;
    private readonly TestExecutionContext _testExecutionContext;
    private readonly Mock<IRequestData> _mockRequestData;
    private readonly TestSessionMessageLogger _sessionLogger;

    public ExecutionManagerTests()
    {
        _mockRequestData = new Mock<IRequestData>();
        _mockRequestData.Setup(rd => rd.MetricsCollection).Returns(new NoOpMetricsCollection());
        _sessionLogger = TestSessionMessageLogger.Instance;
        _executionManager = new ExecutionManager(new RequestData
        {
            MetricsCollection = new NoOpMetricsCollection()
        });

        TestPluginCache.Instance = null;

        _testExecutionContext = new TestExecutionContext(
            frequencyOfRunStatsChangeEvent: 1,
            runStatsChangeEventTimeout: TimeSpan.MaxValue,
            inIsolation: false,
            keepAlive: false,
            isDataCollectionEnabled: false,
            areTestCaseLevelEventsRequired: false,
            hasTestRun: false,
            isDebug: false,
            testCaseFilter: null,
            filterOptions: null);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        RunTestWithSourcesExecutor.RunTestsWithSourcesCallback = null;
        RunTestWithSourcesExecutor.RunTestsWithTestsCallback = null;

        TestDiscoveryExtensionManager.Destroy();
        TestExecutorExtensionManager.Destroy();
        SettingsProviderExtensionManager.Destroy();
    }

    [TestMethod]
    public void InitializeShouldLoadAndInitializeAllExtensions()
    {
        var commonAssemblyLocation = typeof(ExecutionManagerTests).Assembly.Location;
        var mockTestMessageEventHandler = new Mock<ITestMessageEventHandler>();
        TestPluginCacheHelper.SetupMockExtensions(
            [commonAssemblyLocation],
            () => { });


        _executionManager.Initialize(new List<string> { commonAssemblyLocation }, mockTestMessageEventHandler.Object);

        Assert.IsNotNull(TestPluginCache.Instance.TestExtensions);

        // Executors
        Assert.IsTrue(TestPluginCache.Instance.TestExtensions.TestExecutors!.Count > 0);
        var allExecutors = TestExecutorExtensionManager.Create().TestExtensions;

        foreach (var executor in allExecutors)
        {
            Assert.IsTrue(executor.IsExtensionCreated);
        }

        // Settings Providers
        Assert.IsTrue(TestPluginCache.Instance.TestExtensions.TestSettingsProviders!.Count > 0);
        var settingsProviders = SettingsProviderExtensionManager.Create().SettingsProvidersMap.Values;

        foreach (var provider in settingsProviders)
        {
            Assert.IsTrue(provider.IsExtensionCreated);
        }
    }

    [TestMethod]
    public void InitializeShouldClearMetricsCollection()
    {
        var metricsCollection = new MetricsCollection();

        metricsCollection.Add("metric", "value");
        _mockRequestData.Setup(rd => rd.MetricsCollection).Returns(metricsCollection);
        _mockRequestData.Setup(rd => rd.IsTelemetryOptedIn).Returns(true);

        var discoveryManager = new ExecutionManager(_mockRequestData.Object);

        metricsCollection.Metrics.Should().ContainKey("metric");
        discoveryManager.Initialize(null, new Mock<ITestDiscoveryEventsHandler2>().Object);
        metricsCollection.Metrics.Should().BeEmpty();
    }

    [TestMethod]
    public void InitializeShouldNotFailIfMetricsFieldIsNull()
    {
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();

        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        mockRequestData.Object.MetricsCollection.Metrics.Should().BeNull();

        var action = () => (new ExecutionManager(mockRequestData.Object))
            .Initialize(null, new Mock<ITestDiscoveryEventsHandler2>().Object);

        action.Should().NotThrow();
    }

    [TestMethod]
    public void StartTestRunShouldRunTestsInTheProvidedSources()
    {
        var assemblyLocation = typeof(ExecutionManagerTests).Assembly.Location;
        TestPluginCacheHelper.SetupMockExtensions(
            [assemblyLocation],
            () => { });
        TestPluginCache.Instance.DiscoverTestExtensions<TestExecutorPluginInformation, ITestExecutor>(TestPlatformConstants.TestAdapterEndsWithPattern);
        TestPluginCache.Instance.DiscoverTestExtensions<TestDiscovererPluginInformation, ITestDiscoverer>(TestPlatformConstants.TestAdapterEndsWithPattern);


        var adapterSourceMap = new Dictionary<string, IEnumerable<string>>
        {
            { assemblyLocation, new List<string> { assemblyLocation } }
        };

        var mockTestRunEventsHandler = new Mock<IInternalTestRunEventsHandler>();

        var isExecutorCalled = false;
        RunTestWithSourcesExecutor.RunTestsWithSourcesCallback = (s, rc, fh) =>
        {
            isExecutorCalled = true;
            var tr =
                new Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult(
                    new TestCase(
                        "A.C.M",
                        new Uri("e://d/"),
                        "A.dll"));
            fh!.RecordResult(tr);
        };

        _executionManager.StartTestRun(adapterSourceMap, null, null, _testExecutionContext, null, mockTestRunEventsHandler.Object);

        Assert.IsTrue(isExecutorCalled);
        mockTestRunEventsHandler.Verify(
            treh => treh.HandleTestRunComplete(It.IsAny<TestRunCompleteEventArgs>(),
                It.IsAny<TestRunChangedEventArgs>(),
                It.IsAny<ICollection<AttachmentSet>>(),
                It.IsAny<ICollection<string>>()), Times.Once);

        // Also verify that run stats are passed through.
        mockTestRunEventsHandler.Verify(treh => treh.HandleTestRunStatsChange(It.IsAny<TestRunChangedEventArgs>()), Times.Once);
    }

    [TestMethod]
    public void StartTestRunShouldRunTestsForTheProvidedTests()
    {
        var assemblyLocation = typeof(ExecutionManagerTests).Assembly.Location;

        var tests = new List<TestCase>
        {
            new("A.C.M1", new Uri(RunTestsWithSourcesTestsExecutorUri), assemblyLocation)
        };

        var mockTestRunEventsHandler = new Mock<IInternalTestRunEventsHandler>();

        var isExecutorCalled = false;
        RunTestWithSourcesExecutor.RunTestsWithTestsCallback = (s, rc, fh) =>
        {
            isExecutorCalled = true;
            var tr =
                new Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult(
                    new TestCase(
                        "A.C.M",
                        new Uri(RunTestsWithSourcesTestsExecutorUri),
                        "A.dll"));
            fh!.RecordResult(tr);
        };
        TestPluginCacheHelper.SetupMockExtensions([assemblyLocation], () => { });


        _executionManager.StartTestRun(tests, null, null, _testExecutionContext, null, mockTestRunEventsHandler.Object);

        Assert.IsTrue(isExecutorCalled);
        mockTestRunEventsHandler.Verify(
            treh => treh.HandleTestRunComplete(It.IsAny<TestRunCompleteEventArgs>(),
                It.IsAny<TestRunChangedEventArgs>(),
                It.IsAny<ICollection<AttachmentSet>>(),
                It.IsAny<ICollection<string>>()), Times.Once);

        // Also verify that run stats are passed through.
        mockTestRunEventsHandler.Verify(treh => treh.HandleTestRunStatsChange(It.IsAny<TestRunChangedEventArgs>()), Times.Once);
    }

    [TestMethod]
    public void StartTestRunShouldAbortTheRunIfAnyExceptionComesForTheProvidedTests()
    {
        var mockTestRunEventsHandler = new Mock<IInternalTestRunEventsHandler>();

        // Call StartTestRun with faulty runsettings so that it will throw exception
        _executionManager.StartTestRun(new List<TestCase>(), null, @"<RunSettings><RunConfiguration><TestSessionTimeout>-1</TestSessionTimeout></RunConfiguration></RunSettings>", _testExecutionContext, null, mockTestRunEventsHandler.Object);

        // Verify that TestRunComplete get called and error message are getting logged
        mockTestRunEventsHandler.Verify(treh => treh.HandleTestRunComplete(It.IsAny<TestRunCompleteEventArgs>(), null, null, null), Times.Once);
        mockTestRunEventsHandler.Verify(treh => treh.HandleLogMessage(TestMessageLevel.Error, It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public void StartTestRunShouldAbortTheRunIfAnyExceptionComesForTheProvidedSources()
    {
        var mockTestRunEventsHandler = new Mock<IInternalTestRunEventsHandler>();

        // Call StartTestRun with faulty runsettings so that it will throw exception
        _executionManager.StartTestRun(new Dictionary<string, IEnumerable<string>>(), null, @"<RunSettings><RunConfiguration><TestSessionTimeout>-1</TestSessionTimeout></RunConfiguration></RunSettings>", _testExecutionContext, null, mockTestRunEventsHandler.Object);

        // Verify that TestRunComplete get called and error message are getting logged
        mockTestRunEventsHandler.Verify(treh => treh.HandleTestRunComplete(It.IsAny<TestRunCompleteEventArgs>(), null, null, null), Times.Once);
        mockTestRunEventsHandler.Verify(treh => treh.HandleLogMessage(TestMessageLevel.Error, It.IsAny<string>()), Times.Once);
    }

    //[TestMethod]
    //public void InitializeShouldVerifyWarningMessageIfAdapterFailedToLoad()
    //{
    //    var assemblyLocation = typeof(ExecutionManagerTests).Assembly.Location;
    //    var mockLogger = new Mock<ITestMessageEventHandler>();
    //    TestPluginCacheHelper.SetupMockExtensions(
    //       new string[] { assemblyLocation },
    //       () => { });
    //    //Act
    //    this.executionManager.Initialize(new List<string> { assemblyLocation }, mockLogger.Object);

    //    //when handler instance returns warning
    //    sessionLogger.SendMessage(TestMessageLevel.Warning, "verify that it is downgraded to warning");

    //    // Verify.
    //    mockLogger.Verify(rd => rd.HandleLogMessage(TestMessageLevel.Warning, "verify that it is downgraded to warning"), Times.Once);
    //}

    [TestMethod]
    public void InitializeShouldVerifyTheHandlerInitializationWhenAdapterIsFailedToLoad()
    {
        var mockLogger = new Mock<ITestMessageEventHandler>();

        //when handler instance is null
        _sessionLogger.SendMessage(It.IsAny<TestMessageLevel>(), "verify that the HandleLogMessage method will not be invoked when handler is not initialized");

        // Verify.
        mockLogger.Verify(rd => rd.HandleLogMessage(It.IsAny<TestMessageLevel>(), "verify that the HandleLogMessage method will not be invoked when handler is not initialized"), Times.Never);
    }

    #region Implementations

    #region Discoverers

    private abstract class AbstractTestDiscoverer : ITestDiscoverer
    {
        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
        {
            throw new NotImplementedException();
        }
    }

    private class ValidDiscoverer : ITestDiscoverer
    {
        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
        {
            throw new NotImplementedException();
        }
    }

    private class ValidDiscoverer2 : ITestDiscoverer
    {
        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    #region Executors

    [ExtensionUri("ValidExecutor")]
    private class ValidExecutor : ITestExecutor
    {
        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public void RunTests(IEnumerable<string>? sources, IRunContext? runContext, IFrameworkHandle? frameworkHandle)
        {
            throw new NotImplementedException();
        }

        public void RunTests(IEnumerable<TestCase>? tests, IRunContext? runContext, IFrameworkHandle? frameworkHandle)
        {
            throw new NotImplementedException();
        }
    }

    [ExtensionUri("ValidExecutor2")]
    private class ValidExecutor2 : ITestExecutor
    {
        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public void RunTests(IEnumerable<string>? sources, IRunContext? runContext, IFrameworkHandle? frameworkHandle)
        {
            throw new NotImplementedException();
        }

        public void RunTests(IEnumerable<TestCase>? tests, IRunContext? runContext, IFrameworkHandle? frameworkHandle)
        {
            throw new NotImplementedException();
        }
    }

    [ExtensionUri("ValidExecutor")]
    private class DuplicateExecutor : ITestExecutor
    {
        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public void RunTests(IEnumerable<string>? sources, IRunContext? runContext, IFrameworkHandle? frameworkHandle)
        {
            throw new NotImplementedException();
        }

        public void RunTests(IEnumerable<TestCase>? tests, IRunContext? runContext, IFrameworkHandle? frameworkHandle)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    #region Loggers

    [ExtensionUri("csv")]
    private class ValidLogger : ITestLogger
    {
        public void Initialize(TestLoggerEvents events, string testRunDirectory)
        {
            throw new NotImplementedException();
        }
    }

    [ExtensionUri("docx")]
    private class ValidLogger2 : ITestLogger
    {
        public void Initialize(TestLoggerEvents events, string testRunDirectory)
        {
            throw new NotImplementedException();
        }
    }

    [ExtensionUri("csv")]
    private class DuplicateLogger : ITestLogger
    {
        public void Initialize(TestLoggerEvents events, string testRunDirectory)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    #region Settings Providers

    [SettingsName("ValidSettingsProvider")]
    private class ValidSettingsProvider : ISettingsProvider
    {
        public void Load(XmlReader reader)
        {
            throw new NotImplementedException();
        }
    }

    [SettingsName("ValidSettingsProvider2")]
    private class ValidSettingsProvider2 : ISettingsProvider
    {
        public void Load(XmlReader reader)
        {
            throw new NotImplementedException();
        }
    }

    [SettingsName("ValidSettingsProvider")]
    private class DuplicateSettingsProvider : ISettingsProvider
    {
        public void Load(XmlReader reader)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    #region  DataCollectors

    public class InvalidDataCollector : DataCollector
    {
        public override void Initialize(
            XmlElement? configurationElement,
            DataCollectionEvents events,
            DataCollectionSink dataSink,
            DataCollectionLogger logger,
            DataCollectionEnvironmentContext? environmentContext)
        {

        }
    }

    /// <summary>
    /// The a data collector inheriting from another data collector.
    /// </summary>
    [DataCollectorFriendlyName("Foo1")]
    [DataCollectorTypeUri("datacollector://foo/bar1")]
    public class ADataCollectorInheritingFromAnotherDataCollector : InvalidDataCollector
    {
    }

    [DataCollectorFriendlyName("Foo")]
    [DataCollectorTypeUri("datacollector://foo/bar")]
    public class ValidDataCollector : DataCollector
    {
        public override void Initialize(
            XmlElement? configurationElement,
            DataCollectionEvents events,
            DataCollectionSink dataSink,
            DataCollectionLogger logger,
            DataCollectionEnvironmentContext? environmentContext)
        {

        }
    }
    #endregion

    internal class FaultyTestExecutorPluginInformation : TestExtensionPluginInformation
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="type"> The Type. </param>
        public FaultyTestExecutorPluginInformation(Type type) : base(type)
        {
            throw new Exception();
        }
    }
    #endregion
}
