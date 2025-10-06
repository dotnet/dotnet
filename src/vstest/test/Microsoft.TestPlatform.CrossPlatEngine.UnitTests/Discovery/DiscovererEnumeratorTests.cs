// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Discovery;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace TestPlatform.CrossPlatEngine.UnitTests.Discovery;

[TestClass]
public class DiscovererEnumeratorTests
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly DiscovererEnumerator _discovererEnumerator;
    private readonly Mock<ITestPlatformEventSource> _mockTestPlatformEventSource;
    private readonly DiscoveryResultCache _discoveryResultCache;
    private readonly Mock<IRequestData> _mockRequestData;
    private readonly Mock<IMetricsCollection> _mockMetricsCollection;
    private readonly Mock<IAssemblyProperties> _mockAssemblyProperties;
    private readonly Mock<IRunSettings> _runSettingsMock;
    private readonly Mock<IMessageLogger> _messageLoggerMock;

    public DiscovererEnumeratorTests()
    {
        _mockTestPlatformEventSource = new Mock<ITestPlatformEventSource>();
        _discoveryResultCache = new DiscoveryResultCache(1000, TimeSpan.FromHours(1), (tests) => { });
        _mockRequestData = new Mock<IRequestData>();
        _mockMetricsCollection = new Mock<IMetricsCollection>();
        _mockAssemblyProperties = new Mock<IAssemblyProperties>();
        _mockRequestData.Setup(rd => rd.MetricsCollection).Returns(_mockMetricsCollection.Object);
        _discovererEnumerator = new DiscovererEnumerator(_mockRequestData.Object, _discoveryResultCache, _mockTestPlatformEventSource.Object, _mockAssemblyProperties.Object, _cancellationTokenSource.Token);
        _runSettingsMock = new Mock<IRunSettings>();
        _messageLoggerMock = new Mock<IMessageLogger>();
        TestPluginCacheHelper.SetupMockExtensions(
            [typeof(DiscovererEnumeratorTests).Assembly.Location],
            () => { });
        TestDiscoveryExtensionManager.Destroy();
    }

    [TestCleanup]
    public void Cleanup()
    {
        ManagedDllTestDiscoverer.Reset();
        NativeDllTestDiscoverer.Reset();
        JsonTestDiscoverer.Reset();
        NotImplementedTestDiscoverer.Reset();
        EverythingTestDiscoverer.Reset();
        DirectoryTestDiscoverer.Reset();
        DirectoryAndFileTestDiscoverer.Reset();
    }

    [TestMethod]
    public void LoadTestsShouldReportWarningOnNoDiscoverers()
    {
        TestPluginCacheHelper.SetupMockExtensions(
            [typeof(TestPluginCache).Assembly.Location],
            () => { });
        var sources = new List<string> { typeof(DiscoveryResultCacheTests).Assembly.Location };

        var extensionSourceMap = new Dictionary<string, IEnumerable<string>>
        {
            { "_none_", sources }
        };

        _discovererEnumerator.LoadTests(extensionSourceMap, _runSettingsMock.Object, null, _messageLoggerMock.Object);

        var message = $"No test is available in {string.Join(" ", sources)}. Make sure that test discoverer & executors are registered and platform & framework version settings are appropriate and try again.";

        _messageLoggerMock.Verify(l => l.SendMessage(TestMessageLevel.Warning, message), Times.Once);
    }

    [TestMethod]
    public void LoadTestsShouldNotCallIntoDiscoverersIfNoneMatchesSources()
    {
        TestPluginCacheHelper.SetupMockExtensions(
            [typeof(DiscovererEnumeratorTests).Assembly.Location],
            () => { });
        var sources = new List<string> { "temp.jpeg" };

        var extensionSourceMap = new Dictionary<string, IEnumerable<string>>
        {
            { "_none_", sources }
        };

        _discovererEnumerator.LoadTests(extensionSourceMap, _runSettingsMock.Object, null, _messageLoggerMock.Object);

        Assert.IsTrue(EverythingTestDiscoverer.IsDiscoverTestCalled);
        CollectionAssert.AreEqual(sources, EverythingTestDiscoverer.Sources!.ToList());

        Assert.IsFalse(ManagedDllTestDiscoverer.IsManagedDiscoverTestCalled);
        Assert.IsFalse(NativeDllTestDiscoverer.IsNativeDiscoverTestCalled);
        Assert.IsFalse(JsonTestDiscoverer.IsDiscoverTestCalled);
        Assert.IsFalse(DirectoryTestDiscoverer.IsDiscoverTestCalled);
        Assert.IsFalse(DirectoryAndFileTestDiscoverer.IsDiscoverTestCalled);
    }

    [TestMethod]
    public void LoadTestsShouldCallOnlyNativeDiscovererIfNativeAssembliesPassed()
    {
        TestPluginCacheHelper.SetupMockExtensions(
            [typeof(DiscovererEnumeratorTests).Assembly.Location],
            () => { });

        _mockAssemblyProperties.Setup(pe => pe.GetAssemblyType("native.dll")).Returns(AssemblyType.Native);

        var sources = new List<string>
        {
            "native.dll"
        };

        var extensionSourceMap = new Dictionary<string, IEnumerable<string>>
        {
            { "_none_", sources }
        };

        string testCaseFilter = "TestFilter";

        _discovererEnumerator.LoadTests(extensionSourceMap, _runSettingsMock.Object, testCaseFilter, _messageLoggerMock.Object);

        Assert.IsTrue(NativeDllTestDiscoverer.IsNativeDiscoverTestCalled);
        CollectionAssert.AreEqual(sources, NativeDllTestDiscoverer.Sources!.ToList());

        Assert.IsTrue(EverythingTestDiscoverer.IsDiscoverTestCalled);
        CollectionAssert.AreEqual(sources, EverythingTestDiscoverer.Sources!.ToList());

        Assert.IsFalse(ManagedDllTestDiscoverer.IsManagedDiscoverTestCalled);
        Assert.IsFalse(JsonTestDiscoverer.IsDiscoverTestCalled);
        Assert.IsFalse(DirectoryTestDiscoverer.IsDiscoverTestCalled);
        Assert.IsFalse(DirectoryAndFileTestDiscoverer.IsDiscoverTestCalled);
    }

    [TestMethod]
    public void LoadTestsShouldCallOnlyManagedDiscovererIfManagedAssembliesPassed()
    {
        TestPluginCacheHelper.SetupMockExtensions(
            [typeof(DiscovererEnumeratorTests).Assembly.Location],
            () => { });

        _mockAssemblyProperties.Setup(pe => pe.GetAssemblyType("managed.dll")).Returns(AssemblyType.Managed);

        var sources = new List<string>
        {
            "managed.dll"
        };

        var extensionSourceMap = new Dictionary<string, IEnumerable<string>>
        {
            { "_none_", sources }
        };

        string testCaseFilter = "TestFilter";

        _discovererEnumerator.LoadTests(extensionSourceMap, _runSettingsMock.Object, testCaseFilter, _messageLoggerMock.Object);

        Assert.IsTrue(ManagedDllTestDiscoverer.IsManagedDiscoverTestCalled);
        CollectionAssert.AreEqual(sources, ManagedDllTestDiscoverer.Sources!.ToList());

        Assert.IsTrue(EverythingTestDiscoverer.IsDiscoverTestCalled);
        CollectionAssert.AreEqual(sources, EverythingTestDiscoverer.Sources!.ToList());

        Assert.IsFalse(NativeDllTestDiscoverer.IsNativeDiscoverTestCalled);
        Assert.IsFalse(JsonTestDiscoverer.IsDiscoverTestCalled);
        Assert.IsFalse(DirectoryTestDiscoverer.IsDiscoverTestCalled);
        Assert.IsFalse(DirectoryAndFileTestDiscoverer.IsDiscoverTestCalled);
    }

    [TestMethod]
    public void LoadTestsShouldCallBothNativeAndManagedDiscoverersWithCorrectSources()
    {
        TestPluginCacheHelper.SetupMockExtensions(
            [typeof(DiscovererEnumeratorTests).Assembly.Location],
            () => { });

        _mockAssemblyProperties.Setup(pe => pe.GetAssemblyType("native.dll")).Returns(AssemblyType.Native);
        _mockAssemblyProperties.Setup(pe => pe.GetAssemblyType("managed.dll")).Returns(AssemblyType.Managed);

        var nativeSources = new List<string>
        {
            "native.dll"
        };
        var managedSources = new List<string>
        {
            "managed.dll"
        };

        var extensionSourceMap = new Dictionary<string, IEnumerable<string>>
        {
            { "_none_", nativeSources.Concat(managedSources) }
        };

        string testCaseFilter = "TestFilter";

        _discovererEnumerator.LoadTests(extensionSourceMap, _runSettingsMock.Object, testCaseFilter, _messageLoggerMock.Object);

        Assert.IsTrue(ManagedDllTestDiscoverer.IsManagedDiscoverTestCalled);
        CollectionAssert.AreEqual(managedSources, ManagedDllTestDiscoverer.Sources!.ToList());

        Assert.IsTrue(NativeDllTestDiscoverer.IsNativeDiscoverTestCalled);
        CollectionAssert.AreEqual(nativeSources, NativeDllTestDiscoverer.Sources!.ToList());

        var allSources = nativeSources.Concat(managedSources).OrderBy(source => source).ToList();
        Assert.IsTrue(EverythingTestDiscoverer.IsDiscoverTestCalled);
        CollectionAssert.AreEqual(allSources, EverythingTestDiscoverer.Sources!.OrderBy(source => source).ToList());

        Assert.IsFalse(JsonTestDiscoverer.IsDiscoverTestCalled);
        Assert.IsFalse(DirectoryTestDiscoverer.IsDiscoverTestCalled);
        Assert.IsFalse(DirectoryAndFileTestDiscoverer.IsDiscoverTestCalled);
    }

    [TestMethod]
    public void LoadTestsShouldCallIntoADiscovererThatMatchesTheSources()
    {
        TestPluginCacheHelper.SetupMockExtensions(
            [typeof(DiscovererEnumeratorTests).Assembly.Location],
            () => { });

        var sources = new List<string>
        {
            typeof(DiscoveryResultCacheTests).Assembly.Location,
            typeof(DiscoveryResultCacheTests).Assembly.Location
        };

        var extensionSourceMap = new Dictionary<string, IEnumerable<string>>
        {
            { "_none_", sources }
        };
        string testCaseFilter = "TestFilter";

        _discovererEnumerator.LoadTests(extensionSourceMap, _runSettingsMock.Object, testCaseFilter, _messageLoggerMock.Object);

        Assert.IsTrue(ManagedDllTestDiscoverer.IsManagedDiscoverTestCalled);
        Assert.IsFalse(JsonTestDiscoverer.IsDiscoverTestCalled);
        Assert.IsTrue(EverythingTestDiscoverer.IsDiscoverTestCalled);
        Assert.IsFalse(DirectoryTestDiscoverer.IsDiscoverTestCalled);
        Assert.IsFalse(DirectoryAndFileTestDiscoverer.IsDiscoverTestCalled);

        // Also validate that the right set of arguments were passed on to the discoverer.
        CollectionAssert.AreEqual(sources.Distinct().ToList(), ManagedDllTestDiscoverer.Sources!.ToList());
        Assert.AreEqual(_runSettingsMock.Object, DllTestDiscoverer.DiscoveryContext!.RunSettings);
        Assert.AreEqual(testCaseFilter, ((DiscoveryContext)DllTestDiscoverer.DiscoveryContext).FilterExpressionWrapper!.FilterString);
        Assert.AreEqual(_messageLoggerMock.Object, DllTestDiscoverer.MessageLogger);
        Assert.IsNotNull(DllTestDiscoverer.DiscoverySink);

        CollectionAssert.AreEqual(sources.Distinct().ToList(), EverythingTestDiscoverer.Sources!.ToList());
        Assert.AreEqual(_runSettingsMock.Object, EverythingTestDiscoverer.DiscoveryContext!.RunSettings);
        Assert.AreEqual(testCaseFilter, ((DiscoveryContext)EverythingTestDiscoverer.DiscoveryContext).FilterExpressionWrapper!.FilterString);
        Assert.AreEqual(_messageLoggerMock.Object, EverythingTestDiscoverer.MessageLogger);
        Assert.IsNotNull(EverythingTestDiscoverer.DiscoverySink);
    }

    [TestMethod]
    public void LoadTestsShouldCallIntoMultipleDiscoverersThatMatchesTheSources()
    {
        TestPluginCacheHelper.SetupMockExtensions(
            [typeof(DiscovererEnumeratorTests).Assembly.Location],
            () => { });

        var dllsources = new List<string>
        {
            typeof(DiscoveryResultCacheTests).Assembly.Location,
            typeof(DiscoveryResultCacheTests).Assembly.Location
        };

        var jsonsources = new List<string>
        {
            "test1.json",
            "test2.json"
        };

        var currentDirectory = Directory.GetCurrentDirectory();
        var directorySources = new List<string>
        {
            currentDirectory,
            Path.GetDirectoryName(currentDirectory)!
        };

        var sources = new List<string>(dllsources);
        sources.AddRange(jsonsources);
        sources.AddRange(directorySources);

        var extensionSourceMap = new Dictionary<string, IEnumerable<string>>
        {
            { "_none_", sources }
        };

        var runSettings = _runSettingsMock.Object;

        string testCaseFilter = "TestFilter";

        _discovererEnumerator.LoadTests(extensionSourceMap, runSettings, testCaseFilter, _messageLoggerMock.Object);

        Assert.IsTrue(ManagedDllTestDiscoverer.IsManagedDiscoverTestCalled);
        Assert.IsTrue(JsonTestDiscoverer.IsDiscoverTestCalled);
        Assert.IsTrue(EverythingTestDiscoverer.IsDiscoverTestCalled);
        Assert.IsTrue(DirectoryTestDiscoverer.IsDiscoverTestCalled);
        Assert.IsTrue(DirectoryAndFileTestDiscoverer.IsDiscoverTestCalled);

        // Also validate that the right set of arguments were passed on to the discoverer.
        CollectionAssert.AreEqual(dllsources.Distinct().ToList(), ManagedDllTestDiscoverer.Sources!.ToList());
        Assert.AreEqual(runSettings, DllTestDiscoverer.DiscoveryContext!.RunSettings);
        Assert.AreEqual(testCaseFilter, ((DiscoveryContext)DllTestDiscoverer.DiscoveryContext).FilterExpressionWrapper!.FilterString);
        Assert.AreEqual(_messageLoggerMock.Object, DllTestDiscoverer.MessageLogger);
        Assert.IsNotNull(DllTestDiscoverer.DiscoverySink);

        CollectionAssert.AreEqual(jsonsources, JsonTestDiscoverer.Sources!.ToList());
        Assert.AreEqual(runSettings, JsonTestDiscoverer.DiscoveryContext!.RunSettings);
        Assert.AreEqual(testCaseFilter, ((DiscoveryContext)JsonTestDiscoverer.DiscoveryContext).FilterExpressionWrapper!.FilterString);
        Assert.AreEqual(_messageLoggerMock.Object, JsonTestDiscoverer.MessageLogger);
        Assert.IsNotNull(JsonTestDiscoverer.DiscoverySink);

        var allSources = sources.Distinct().OrderBy(source => source).ToList();
        CollectionAssert.AreEqual(allSources, EverythingTestDiscoverer.Sources!.OrderBy(source => source).ToList());
        Assert.AreEqual(runSettings, EverythingTestDiscoverer.DiscoveryContext!.RunSettings);
        Assert.AreEqual(testCaseFilter, ((DiscoveryContext)EverythingTestDiscoverer.DiscoveryContext).FilterExpressionWrapper!.FilterString);
        Assert.AreEqual(_messageLoggerMock.Object, EverythingTestDiscoverer.MessageLogger);
        Assert.IsNotNull(EverythingTestDiscoverer.DiscoverySink);

        CollectionAssert.AreEqual(directorySources, DirectoryTestDiscoverer.Sources!.ToList());
        Assert.AreEqual(runSettings, DirectoryTestDiscoverer.DiscoveryContext!.RunSettings);
        Assert.AreEqual(testCaseFilter, ((DiscoveryContext)DirectoryTestDiscoverer.DiscoveryContext).FilterExpressionWrapper!.FilterString);
        Assert.AreEqual(_messageLoggerMock.Object, DirectoryTestDiscoverer.MessageLogger);
        Assert.IsNotNull(DirectoryTestDiscoverer.DiscoverySink);

        var jsonAndDirectorySources = jsonsources.Concat(directorySources).OrderBy(source => source).ToList();
        CollectionAssert.AreEqual(jsonAndDirectorySources, DirectoryAndFileTestDiscoverer.Sources!.OrderBy(source => source).ToList());
        Assert.AreEqual(runSettings, DirectoryAndFileTestDiscoverer.DiscoveryContext!.RunSettings);
        Assert.AreEqual(testCaseFilter, ((DiscoveryContext)DirectoryAndFileTestDiscoverer.DiscoveryContext).FilterExpressionWrapper!.FilterString);
        Assert.AreEqual(_messageLoggerMock.Object, DirectoryAndFileTestDiscoverer.MessageLogger);
        Assert.IsNotNull(DirectoryAndFileTestDiscoverer.DiscoverySink);
    }

    [TestMethod]
    public void LoadTestsShouldCallIntoOtherDiscoverersWhenCreatingOneFails()
    {
        TestPluginCacheHelper.SetupMockExtensions(
            [typeof(DiscovererEnumeratorTests).Assembly.Location],
            () => { });

        var sources = new List<string>
        {
            "test1.csv",
            typeof(DiscoveryResultCacheTests).Assembly.Location
        };

        var extensionSourceMap = new Dictionary<string, IEnumerable<string>>
        {
            { "_none_", sources }
        };

        var runSettings = _runSettingsMock.Object;

        string testCaseFilter = "TestFilter";

        _discovererEnumerator.LoadTests(extensionSourceMap, runSettings, testCaseFilter, _messageLoggerMock.Object);

        Assert.IsTrue(ManagedDllTestDiscoverer.IsManagedDiscoverTestCalled);
        Assert.IsFalse(SingletonTestDiscoverer.IsDiscoverTestCalled);
        Assert.IsTrue(EverythingTestDiscoverer.IsDiscoverTestCalled);
        Assert.IsFalse(DirectoryTestDiscoverer.IsDiscoverTestCalled);
        Assert.IsFalse(DirectoryAndFileTestDiscoverer.IsDiscoverTestCalled);

        // Also validate that the right set of arguments were passed on to the discoverer.
        CollectionAssert.AreEqual(new List<string> { sources[1] }, ManagedDllTestDiscoverer.Sources!.ToList());
        Assert.AreEqual(runSettings, DllTestDiscoverer.DiscoveryContext!.RunSettings);
        Assert.AreEqual(testCaseFilter, ((DiscoveryContext)DllTestDiscoverer.DiscoveryContext).FilterExpressionWrapper!.FilterString);
        Assert.AreEqual(_messageLoggerMock.Object, DllTestDiscoverer.MessageLogger);
        Assert.IsNotNull(DllTestDiscoverer.DiscoverySink);

        CollectionAssert.AreEqual(sources.ToList(), EverythingTestDiscoverer.Sources!.ToList());
        Assert.AreEqual(runSettings, EverythingTestDiscoverer.DiscoveryContext!.RunSettings);
        Assert.AreEqual(testCaseFilter, ((DiscoveryContext)EverythingTestDiscoverer.DiscoveryContext).FilterExpressionWrapper!.FilterString);
        Assert.AreEqual(_messageLoggerMock.Object, EverythingTestDiscoverer.MessageLogger);
        Assert.IsNotNull(EverythingTestDiscoverer.DiscoverySink);
    }

    [TestMethod]
    public void LoadTestsShouldCallIntoOtherDiscoverersEvenIfDiscoveryInOneFails()
    {
        TestPluginCacheHelper.SetupMockExtensions(
            [typeof(DiscovererEnumeratorTests).Assembly.Location],
            () => { });

        var sources = new List<string>
        {
            "test1.cs",
            typeof(DiscoveryResultCacheTests).Assembly.Location
        };

        var extensionSourceMap = new Dictionary<string, IEnumerable<string>>
        {
            { "_none_", sources }
        };

        var runSettings = _runSettingsMock.Object;

        string testCaseFilter = "TestFilter";

        _discovererEnumerator.LoadTests(extensionSourceMap, runSettings, testCaseFilter, _messageLoggerMock.Object);

        Assert.IsTrue(ManagedDllTestDiscoverer.IsManagedDiscoverTestCalled);
        Assert.IsTrue(NotImplementedTestDiscoverer.IsDiscoverTestCalled);
        Assert.IsTrue(EverythingTestDiscoverer.IsDiscoverTestCalled);
        Assert.IsFalse(DirectoryTestDiscoverer.IsDiscoverTestCalled);
        Assert.IsFalse(DirectoryAndFileTestDiscoverer.IsDiscoverTestCalled);

        // Also validate that the right set of arguments were passed on to the discoverer.
        CollectionAssert.AreEqual(new List<string> { sources[1] }, ManagedDllTestDiscoverer.Sources!.ToList());
        Assert.AreEqual(runSettings, DllTestDiscoverer.DiscoveryContext!.RunSettings);
        Assert.AreEqual(testCaseFilter, ((DiscoveryContext)DllTestDiscoverer.DiscoveryContext).FilterExpressionWrapper!.FilterString);
        Assert.AreEqual(_messageLoggerMock.Object, DllTestDiscoverer.MessageLogger);
        Assert.IsNotNull(DllTestDiscoverer.DiscoverySink);

        CollectionAssert.AreEqual(sources.ToList(), EverythingTestDiscoverer.Sources!.ToList());
        Assert.AreEqual(runSettings, EverythingTestDiscoverer.DiscoveryContext!.RunSettings);
        Assert.AreEqual(testCaseFilter, ((DiscoveryContext)EverythingTestDiscoverer.DiscoveryContext).FilterExpressionWrapper!.FilterString);
        Assert.AreEqual(_messageLoggerMock.Object, EverythingTestDiscoverer.MessageLogger);
        Assert.IsNotNull(EverythingTestDiscoverer.DiscoverySink);

        // Check if we log the failure.
        var message = $"An exception occurred while test discoverer '{typeof(NotImplementedTestDiscoverer).Name}' was loading tests. Exception: The method or operation is not implemented.";

        _messageLoggerMock.Verify(l => l.SendMessage(TestMessageLevel.Error, message), Times.Once);
    }

    [TestMethod]
    public void LoadTestsShouldCollectMetrics()
    {
        var mockMetricsCollector = new Mock<IMetricsCollection>();
        var dict = new Dictionary<string, object>
        {
            { "DummyMessage", "DummyValue" }
        };

        TestPluginCacheHelper.SetupMockExtensions(
            [typeof(DiscovererEnumeratorTests).Assembly.Location],
            () => { });

        var sources = new List<string>
        {
            typeof(DiscoveryResultCacheTests).Assembly.Location,
            typeof(DiscoveryResultCacheTests).Assembly.Location
        };

        var extensionSourceMap = new Dictionary<string, IEnumerable<string>>
        {
            { "_none_", sources }
        };

        mockMetricsCollector.Setup(mc => mc.Metrics).Returns(dict);
        _mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollector.Object);

        string testCaseFilter = "TestFilter";
        _discovererEnumerator.LoadTests(extensionSourceMap, _runSettingsMock.Object, testCaseFilter, _messageLoggerMock.Object);

        // Verify.
        mockMetricsCollector.Verify(rd => rd.Add(TelemetryDataConstants.TimeTakenInSecByAllAdapters, It.IsAny<object>()), Times.Once);
        mockMetricsCollector.Verify(rd => rd.Add(TelemetryDataConstants.NumberOfAdapterUsedToDiscoverTests, It.IsAny<object>()), Times.Once);
        mockMetricsCollector.Verify(rd => rd.Add(TelemetryDataConstants.NumberOfAdapterDiscoveredDuringDiscovery, It.IsAny<object>()), Times.Once);
        mockMetricsCollector.Verify(rd => rd.Add(TelemetryDataConstants.TotalTestsByAdapter + ".discoverer://manageddlldiscoverer/", It.IsAny<object>()), Times.Once);
        mockMetricsCollector.Verify(rd => rd.Add(TelemetryDataConstants.TimeTakenToDiscoverTestsByAnAdapter + ".discoverer://manageddlldiscoverer/", It.IsAny<object>()), Times.Once);
        mockMetricsCollector.Verify(rd => rd.Add(TelemetryDataConstants.TotalTestsByAdapter + ".discoverer://nativedlldiscoverer/", It.IsAny<object>()), Times.Once);
        mockMetricsCollector.Verify(rd => rd.Add(TelemetryDataConstants.TimeTakenToDiscoverTestsByAnAdapter + ".discoverer://nativedlldiscoverer/", It.IsAny<object>()), Times.Once);
        mockMetricsCollector.Verify(rd => rd.Add(TelemetryDataConstants.TimeTakenToLoadAdaptersInSec, It.IsAny<object>()), Times.Once);
    }

    [TestMethod]
    public void LoadTestsShouldNotCallIntoDiscoverersWhenCancelled()
    {
        // Setup
        string[] extensions = [typeof(DiscovererEnumeratorTests).Assembly.Location];
        TestPluginCacheHelper.SetupMockExtensions(extensions, () => { });

        var dllsources = new List<string>
        {
            typeof(DiscoveryResultCacheTests).Assembly.Location,
            typeof(DiscoveryResultCacheTests).Assembly.Location
        };
        var jsonsources = new List<string>
        {
            "test1.json",
            "test2.json"
        };
        var sources = new List<string>(dllsources);
        sources.AddRange(jsonsources);

        var extensionSourceMap = new Dictionary<string, IEnumerable<string>>
        {
            { "_none_", sources }
        };

        // Act
        _cancellationTokenSource.Cancel();
        var runSettings = _runSettingsMock.Object;
        string testCaseFilter = "TestFilter";
        _discovererEnumerator.LoadTests(extensionSourceMap, runSettings, testCaseFilter, _messageLoggerMock.Object);

        // Validate
        Assert.IsFalse(ManagedDllTestDiscoverer.IsManagedDiscoverTestCalled);
        Assert.IsFalse(JsonTestDiscoverer.IsDiscoverTestCalled);
        Assert.IsFalse(EverythingTestDiscoverer.IsDiscoverTestCalled);
        Assert.IsFalse(DirectoryTestDiscoverer.IsDiscoverTestCalled);
        Assert.IsFalse(DirectoryAndFileTestDiscoverer.IsDiscoverTestCalled);

        _messageLoggerMock.Verify(logger => logger.SendMessage(TestMessageLevel.Warning, "Discovery of tests cancelled."), Times.Once);
    }

    [TestMethod]
    public void LoadTestsShouldCallIntoTheAdapterWithTheRightTestCaseSink()
    {
        InvokeLoadTestWithMockSetup();

        Assert.IsTrue(ManagedDllTestDiscoverer.IsManagedDiscoverTestCalled);
        Assert.AreEqual(2, _discoveryResultCache.Tests.Count);
    }

    [TestMethod]
    public void LoadTestsShouldNotShowAnyWarningOnTestsDiscovered()
    {
        InvokeLoadTestWithMockSetup();

        Assert.AreEqual(2, _discoveryResultCache.Tests.Count);

        _messageLoggerMock.Verify(m => m.SendMessage(TestMessageLevel.Warning, It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public void LoadTestShouldInstrumentDiscoveryStart()
    {
        InvokeLoadTestWithMockSetup();
        _mockTestPlatformEventSource.Verify(x => x.DiscoveryStart(), Times.Once);
    }

    [TestMethod]
    public void LoadTestShouldInstrumentDiscoveryStop()
    {
        InvokeLoadTestWithMockSetup();
        _mockTestPlatformEventSource.Verify(x => x.DiscoveryStop(It.IsAny<long>()), Times.Once);
    }

    [TestMethod]
    public void LoadTestShouldInstrumentAdapterDiscoveryStart()
    {
        InvokeLoadTestWithMockSetup();
        _mockTestPlatformEventSource.Verify(x => x.AdapterDiscoveryStart(It.IsAny<string>()), Times.AtLeastOnce);
    }

    [TestMethod]
    public void LoadTestShouldInstrumentAdapterDiscoveryStop()
    {
        InvokeLoadTestWithMockSetup();
        _mockTestPlatformEventSource.Verify(x => x.AdapterDiscoveryStop(It.IsAny<long>()), Times.AtLeastOnce);
    }

    [TestMethod]
    public void LoadTestsShouldIterateOverAllExtensionsInTheMapAndDiscoverTests()
    {
        TestPluginCacheHelper.SetupMockExtensions(
            [typeof(DiscovererEnumeratorTests).Assembly.Location],
            () => { });

        var dllsources = new List<string>
        {
            typeof(DiscoveryResultCacheTests).Assembly.Location,
            typeof(DiscoveryResultCacheTests).Assembly.Location
        };
        var jsonsources = new List<string>
        {
            "test1.json",
            "test2.json"
        };

        var extensionSourceMap = new Dictionary<string, IEnumerable<string>>
        {
            { typeof(DiscovererEnumeratorTests).Assembly.Location, jsonsources },
            { "_none_", dllsources }
        };

        var runSettings = _runSettingsMock.Object;

        string testCaseFilter = "TestFilter";

        _discovererEnumerator.LoadTests(extensionSourceMap, runSettings, testCaseFilter, _messageLoggerMock.Object);

        Assert.IsTrue(ManagedDllTestDiscoverer.IsManagedDiscoverTestCalled);
        Assert.IsTrue(JsonTestDiscoverer.IsDiscoverTestCalled);
        Assert.IsTrue(EverythingTestDiscoverer.IsDiscoverTestCalled);
        Assert.IsTrue(DirectoryAndFileTestDiscoverer.IsDiscoverTestCalled);
        Assert.IsFalse(DirectoryTestDiscoverer.IsDiscoverTestCalled);

        // Also validate that the right set of arguments were passed on to the discoverer.
        CollectionAssert.AreEqual(dllsources.Distinct().ToList(), ManagedDllTestDiscoverer.Sources!.ToList());
        Assert.AreEqual(runSettings, DllTestDiscoverer.DiscoveryContext!.RunSettings);
        Assert.AreEqual(testCaseFilter, ((DiscoveryContext)DllTestDiscoverer.DiscoveryContext).FilterExpressionWrapper!.FilterString);
        Assert.AreEqual(_messageLoggerMock.Object, DllTestDiscoverer.MessageLogger);
        Assert.IsNotNull(DllTestDiscoverer.DiscoverySink);

        CollectionAssert.AreEqual(jsonsources, JsonTestDiscoverer.Sources!.ToList());
        Assert.AreEqual(runSettings, JsonTestDiscoverer.DiscoveryContext!.RunSettings);
        Assert.AreEqual(testCaseFilter, ((DiscoveryContext)JsonTestDiscoverer.DiscoveryContext).FilterExpressionWrapper!.FilterString);
        Assert.AreEqual(_messageLoggerMock.Object, JsonTestDiscoverer.MessageLogger);
        Assert.IsNotNull(JsonTestDiscoverer.DiscoverySink);

        var allSources = jsonsources.Concat(dllsources).Distinct().OrderBy(source => source).ToList();
        CollectionAssert.AreEqual(allSources, EverythingTestDiscoverer.Sources!.OrderBy(source => source).ToList());
        Assert.AreEqual(runSettings, EverythingTestDiscoverer.DiscoveryContext!.RunSettings);
        Assert.AreEqual(testCaseFilter, ((DiscoveryContext)EverythingTestDiscoverer.DiscoveryContext).FilterExpressionWrapper!.FilterString);
        Assert.AreEqual(_messageLoggerMock.Object, EverythingTestDiscoverer.MessageLogger);
        Assert.IsNotNull(EverythingTestDiscoverer.DiscoverySink);

        CollectionAssert.AreEqual(jsonsources, DirectoryAndFileTestDiscoverer.Sources!.ToList());
        Assert.AreEqual(runSettings, DirectoryAndFileTestDiscoverer.DiscoveryContext!.RunSettings);
        Assert.AreEqual(testCaseFilter, ((DiscoveryContext)DirectoryAndFileTestDiscoverer.DiscoveryContext).FilterExpressionWrapper!.FilterString);
        Assert.AreEqual(_messageLoggerMock.Object, DirectoryAndFileTestDiscoverer.MessageLogger);
        Assert.IsNotNull(DirectoryAndFileTestDiscoverer.DiscoverySink);
    }

    [TestMethod]
    public void LoadTestsShouldLogWarningMessageOnNoTestsInAssemblies()
    {
        SetupForNoTestsAvailableInGivenAssemblies(out var extensionSourceMap, out var sourcesString);

        _discovererEnumerator.LoadTests(extensionSourceMap, _runSettingsMock.Object, null, _messageLoggerMock.Object);

        var expectedMessage =
            $"No test is available in {sourcesString}. Make sure that test discoverer & executors are registered and platform & framework version settings are appropriate and try again.";

        _messageLoggerMock.Verify(l => l.SendMessage(TestMessageLevel.Warning, expectedMessage));
    }

    [TestMethod]
    public void LoadTestsShouldLogWarningMessageOnNoTestsInAssembliesWithTestCaseFilter()
    {
        SetupForNoTestsAvailableInGivenAssemblies(out var extensionSourceMap, out var sourcesString);

        var testCaseFilter = "Name~TestMethod1";

        _discovererEnumerator.LoadTests(extensionSourceMap, _runSettingsMock.Object, testCaseFilter, _messageLoggerMock.Object);

        var expectedMessage =
            $"No test matches the given testcase filter `{testCaseFilter}` in {sourcesString}";

        _messageLoggerMock.Verify(l => l.SendMessage(TestMessageLevel.Warning, expectedMessage));
    }

    [TestMethod]
    public void LoadTestsShouldShortenTheLongTestCaseFilterWhenNoTestsDiscovered()
    {
        SetupForNoTestsAvailableInGivenAssemblies(out var extensionSourceMap, out var sourcesString);

        var veryLengthyTestCaseFilter = "FullyQualifiedName=TestPlatform.CrossPlatEngine" +
                                        ".UnitTests.Discovery.DiscovererEnumeratorTests." +
                                        "LoadTestsShouldShortenTheLongTestCaseFilterWhenNoTestsDiscovered" +
                                        "TestCaseFilterWithVeryLengthTestCaseNameeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee";

        _discovererEnumerator.LoadTests(extensionSourceMap, _runSettingsMock.Object, veryLengthyTestCaseFilter, _messageLoggerMock.Object);

        var expectedTestCaseFilter = veryLengthyTestCaseFilter.Substring(0, 256) + "...";
        var expectedMessage =
            $"No test matches the given testcase filter `{expectedTestCaseFilter}` in {sourcesString}";

        _messageLoggerMock.Verify(l => l.SendMessage(TestMessageLevel.Warning, expectedMessage));
    }

    private static void SetupForNoTestsAvailableInGivenAssemblies(
        out Dictionary<string, IEnumerable<string>> extensionSourceMap,
        out string sourcesString)
    {
        var crossPlatEngineAssemblyLocation = typeof(DiscovererEnumerator).Assembly.Location;
        var objectModelAseeAssemblyLocation = typeof(TestCase).Assembly.Location;
        var sources = new string[] { crossPlatEngineAssemblyLocation, objectModelAseeAssemblyLocation };

        extensionSourceMap = new Dictionary<string, IEnumerable<string>>
        {
            { "_none_", sources }
        };
        sourcesString = string.Join(" ", crossPlatEngineAssemblyLocation, objectModelAseeAssemblyLocation);
    }

    private void InvokeLoadTestWithMockSetup()
    {
        TestPluginCacheHelper.SetupMockExtensions(
            [typeof(DiscovererEnumeratorTests).Assembly.Location],
            () => { });

        var sources = new List<string>
        {
            typeof(DiscoveryResultCacheTests).Assembly.Location
        };

        var extensionSourceMap = new Dictionary<string, IEnumerable<string>>
        {
            { "_none_", sources }
        };

        _discovererEnumerator.LoadTests(extensionSourceMap, _runSettingsMock.Object, null, _messageLoggerMock.Object);
    }

    #region Implementation

    /// <summary>
    /// Placing this before others so that at runtime this would be the first to be discovered as a discoverer.
    /// </summary>
    [FileExtension(".csv")]
    [DefaultExecutorUri("discoverer://csvdiscoverer")]
    private class SingletonTestDiscoverer : ITestDiscoverer
    {
        private SingletonTestDiscoverer()
        {
        }

        public static bool IsDiscoverTestCalled { get; private set; }

        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
        {
            IsDiscoverTestCalled = true;
        }

        public static void Reset()
        {
            IsDiscoverTestCalled = false;
        }
    }

    [FileExtension(".cs")]
    [DefaultExecutorUri("discoverer://csvdiscoverer")]
    private class NotImplementedTestDiscoverer : ITestDiscoverer
    {
        public static bool IsDiscoverTestCalled { get; private set; }

        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
        {
            IsDiscoverTestCalled = true;
            throw new NotImplementedException();
        }

        public static void Reset()
        {
            IsDiscoverTestCalled = false;
        }
    }

    [FileExtension(".dll")]
    [FileExtension(".exe")]
    [DefaultExecutorUri("discoverer://manageddlldiscoverer")]
    [Category("managed")]
    private class ManagedDllTestDiscoverer : DllTestDiscoverer
    {
        public static bool IsManagedDiscoverTestCalled { get; private set; }

        public static IEnumerable<string>? Sources { get; set; }

        public override void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
        {
            Sources = sources;
            IsManagedDiscoverTestCalled = true;
            base.DiscoverTests(sources, discoveryContext, logger, discoverySink);
        }

        public static void Reset()
        {
            IsManagedDiscoverTestCalled = false;
            Sources = null;
        }
    }

    [FileExtension(".dll")]
    [FileExtension(".exe")]
    [DefaultExecutorUri("discoverer://nativedlldiscoverer")]
    [Category("native")]
    private class NativeDllTestDiscoverer : DllTestDiscoverer
    {
        public static bool IsNativeDiscoverTestCalled { get; private set; }

        public static IEnumerable<string>? Sources { get; set; }

        public override void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger,
            ITestCaseDiscoverySink discoverySink)
        {
            Sources = sources;
            IsNativeDiscoverTestCalled = true;
            base.DiscoverTests(sources, discoveryContext, logger, discoverySink);
        }

        public static void Reset()
        {
            IsNativeDiscoverTestCalled = false;
            Sources = null;
        }
    }

    private class DllTestDiscoverer : ITestDiscoverer
    {
        public static IDiscoveryContext? DiscoveryContext { get; private set; }

        public static IMessageLogger? MessageLogger { get; private set; }

        public static ITestCaseDiscoverySink? DiscoverySink { get; private set; }

        public virtual void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger,
            ITestCaseDiscoverySink discoverySink)
        {
            if (ShouldTestDiscovered(sources) == false)
            {
                return;
            }

            DiscoveryContext = discoveryContext;
            MessageLogger = logger;
            DiscoverySink = discoverySink;

            var testCase = new TestCase("A.C.M", new Uri("executor://dllexecutor"), "A");
            discoverySink.SendTestCase(testCase);
        }

        private static bool ShouldTestDiscovered(IEnumerable<string> sources)
        {
            var shouldTestDiscovered = false;
            foreach (var source in sources)
            {
                if (source.Equals("native.dll") || source.Equals("managed.dll") || source.EndsWith("CrossPlatEngine.UnitTests.dll") || source.EndsWith("CrossPlatEngine.UnitTests.exe"))
                {
                    shouldTestDiscovered = true;
                    break;
                }
            }

            return shouldTestDiscovered;
        }
    }

    [FileExtension(".json")]
    [DefaultExecutorUri("discoverer://jsondiscoverer")]
    private class JsonTestDiscoverer : ITestDiscoverer
    {
        public static bool IsDiscoverTestCalled { get; private set; }

        public static IEnumerable<string>? Sources { get; private set; }

        public static IDiscoveryContext? DiscoveryContext { get; private set; }

        public static IMessageLogger? MessageLogger { get; private set; }

        public static ITestCaseDiscoverySink? DiscoverySink { get; private set; }

        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger,
            ITestCaseDiscoverySink discoverySink)
        {
            IsDiscoverTestCalled = true;
            Sources = sources;
            DiscoveryContext = discoveryContext;
            MessageLogger = logger;
            DiscoverySink = discoverySink;
        }

        public static void Reset()
        {
            IsDiscoverTestCalled = false;
        }
    }

    [DefaultExecutorUri("discoverer://everythingdiscoverer")]
    private class EverythingTestDiscoverer : ITestDiscoverer
    {
        public static bool IsDiscoverTestCalled { get; private set; }

        public static IEnumerable<string>? Sources { get; private set; }

        public static IDiscoveryContext? DiscoveryContext { get; private set; }

        public static IMessageLogger? MessageLogger { get; private set; }

        public static ITestCaseDiscoverySink? DiscoverySink { get; private set; }

        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger,
            ITestCaseDiscoverySink discoverySink)
        {
            IsDiscoverTestCalled = true;
            Sources = Sources is null ? sources : Sources.Concat(sources);
            DiscoveryContext = discoveryContext;
            MessageLogger = logger;
            DiscoverySink = discoverySink;
        }

        public static void Reset()
        {
            IsDiscoverTestCalled = false;
            Sources = null;
        }
    }

    [DirectoryBasedTestDiscoverer]
    [DefaultExecutorUri("discoverer://dirdiscoverer")]
    private class DirectoryTestDiscoverer : ITestDiscoverer
    {
        public static bool IsDiscoverTestCalled { get; private set; }

        public static IEnumerable<string>? Sources { get; private set; }

        public static IDiscoveryContext? DiscoveryContext { get; private set; }

        public static IMessageLogger? MessageLogger { get; private set; }

        public static ITestCaseDiscoverySink? DiscoverySink { get; private set; }

        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger,
            ITestCaseDiscoverySink discoverySink)
        {
            IsDiscoverTestCalled = true;
            Sources = Sources is null ? sources : Sources.Concat(sources);
            DiscoveryContext = discoveryContext;
            MessageLogger = logger;
            DiscoverySink = discoverySink;
        }

        public static void Reset()
        {
            IsDiscoverTestCalled = false;
            Sources = null;
        }
    }

    [DirectoryBasedTestDiscoverer]
    [FileExtension(".json")]
    [DefaultExecutorUri("discoverer://dirandfilediscoverer")]
    private class DirectoryAndFileTestDiscoverer : ITestDiscoverer
    {
        public static bool IsDiscoverTestCalled { get; private set; }

        public static IEnumerable<string>? Sources { get; private set; }

        public static IDiscoveryContext? DiscoveryContext { get; private set; }

        public static IMessageLogger? MessageLogger { get; private set; }

        public static ITestCaseDiscoverySink? DiscoverySink { get; private set; }

        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger,
            ITestCaseDiscoverySink discoverySink)
        {
            IsDiscoverTestCalled = true;
            Sources = Sources is null ? sources : Sources.Concat(sources);
            DiscoveryContext = discoveryContext;
            MessageLogger = logger;
            DiscoverySink = discoverySink;
        }

        public static void Reset()
        {
            IsDiscoverTestCalled = false;
            Sources = null;
        }
    }

    #endregion
}
