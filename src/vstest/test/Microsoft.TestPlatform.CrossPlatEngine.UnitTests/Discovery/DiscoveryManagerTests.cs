// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.Common.Logging;
using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Discovery;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using FluentAssertions;

using CrossPlatEngineResources = Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Resources.Resources;

namespace TestPlatform.CrossPlatEngine.UnitTests.Discovery;

[TestClass]
public class DiscoveryManagerTests
{
    private readonly DiscoveryManager _discoveryManager;
    private readonly Mock<IRequestData> _mockRequestData;
    private readonly Mock<IMetricsCollection> _mockMetricsCollection;
    private readonly TestSessionMessageLogger _sessionLogger;

    public DiscoveryManagerTests()
    {
        _mockRequestData = new Mock<IRequestData>();
        _mockMetricsCollection = new Mock<IMetricsCollection>();
        _sessionLogger = TestSessionMessageLogger.Instance;
        _mockRequestData.Setup(rd => rd.MetricsCollection).Returns(_mockMetricsCollection.Object);
        _discoveryManager = new DiscoveryManager(_mockRequestData.Object);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        TestDiscoveryExtensionManager.Destroy();
        TestPluginCache.Instance = null;
    }

    #region Initialize tests

    [TestMethod]
    public void InitializeShouldUpdateAdditionalExtenions()
    {
        var mockFileHelper = new Mock<IFileHelper>();
        var mockLogger = new Mock<ITestDiscoveryEventsHandler2>();
        mockFileHelper.Setup(fh => fh.DirectoryExists(It.IsAny<string>())).Returns(false);
        TestPluginCache.Instance = new TestableTestPluginCache();

        _discoveryManager.Initialize(
            new string[] { typeof(DiscoveryManagerTests).Assembly.Location }, mockLogger.Object);

        var allDiscoverers = TestDiscoveryExtensionManager.Create().Discoverers;

        Assert.IsNotNull(allDiscoverers);
        Assert.IsTrue(allDiscoverers.Any());
    }

    [TestMethod]
    public void InitializeShouldClearMetricsCollection()
    {
        var metricsCollection = new MetricsCollection();

        metricsCollection.Add("metric", "value");
        _mockRequestData.Setup(rd => rd.MetricsCollection).Returns(metricsCollection);
        _mockRequestData.Setup(rd => rd.IsTelemetryOptedIn).Returns(true);

        var discoveryManager = new DiscoveryManager(_mockRequestData.Object);

        metricsCollection.Metrics.Should().ContainKey("metric");
        discoveryManager.Initialize(null, new Mock<ITestDiscoveryEventsHandler2>().Object);
        metricsCollection.Metrics.Should().BeEmpty();
    }

    [TestMethod]
    public void InitializeShouldNotFailIfMetricsFieldIsNull()
    {
        _mockRequestData.Object.MetricsCollection.Metrics.Should().BeNull();

        var action = () => (new DiscoveryManager(_mockRequestData.Object))
            .Initialize(null, new Mock<ITestDiscoveryEventsHandler2>().Object);

        action.Should().NotThrow();
    }
    #endregion

    #region DiscoverTests tests

    [TestMethod]
    public void DiscoverTestsShouldLogIfTheSourceDoesNotExist()
    {
        var criteria = new DiscoveryCriteria(new List<string> { "imaginary.dll" }, 100, null);
        var mockLogger = new Mock<ITestDiscoveryEventsHandler2>();

        _discoveryManager.DiscoverTests(criteria, mockLogger.Object);

        var errorMessage = string.Format(CultureInfo.CurrentCulture, "Could not find file {0}.", Path.Combine(Directory.GetCurrentDirectory(), "imaginary.dll"));
        mockLogger.Verify(
            l =>
                l.HandleLogMessage(
                    TestMessageLevel.Warning,
                    errorMessage),
            Times.Once);
    }

    [TestMethod]
    public void DiscoverTestsShouldLogIfTheSourceDoesNotExistIfItHasAPackage()
    {
        var criteria = new DiscoveryCriteria(new List<string> { "imaginary.exe" }, 100, null);

        var packageName = "recipe.AppxRecipe";

        var fakeDirectory = Directory.GetDirectoryRoot(typeof(DiscoveryManagerTests).Assembly.Location);

        criteria.Package = Path.Combine(fakeDirectory, Path.Combine(packageName));
        var mockLogger = new Mock<ITestDiscoveryEventsHandler2>();

        _discoveryManager.DiscoverTests(criteria, mockLogger.Object);

        var errorMessage = string.Format(CultureInfo.CurrentCulture, "Could not find file {0}.", Path.Combine(fakeDirectory, "imaginary.exe"));
        mockLogger.Verify(
            l =>
                l.HandleLogMessage(
                    TestMessageLevel.Warning,
                    errorMessage),
            Times.Once);
    }

    [TestMethod]
    public void DiscoverTestsShouldLogIfThereAreNoValidSources()
    {
        var sources = new List<string> { "imaginary.dll" };
        var criteria = new DiscoveryCriteria(sources, 100, null);
        var mockLogger = new Mock<ITestDiscoveryEventsHandler2>();

        _discoveryManager.DiscoverTests(criteria, mockLogger.Object);

        var sourcesString = string.Join(",", sources.ToArray());
        var errorMessage = string.Format(CultureInfo.CurrentCulture, CrossPlatEngineResources.NoValidSourceFoundForDiscovery, sourcesString);
        mockLogger.Verify(
            l =>
                l.HandleLogMessage(
                    TestMessageLevel.Warning,
                    errorMessage),
            Times.Once);
    }

    [TestMethod]
    public void DiscoverTestsShouldLogIfTheSameSourceIsSpecifiedTwice()
    {
        TestPluginCacheHelper.SetupMockExtensions(
            [typeof(DiscovererEnumeratorTests).Assembly.Location],
            () => { });

        var sources = new List<string>
        {
            typeof(DiscoveryManagerTests).Assembly.Location,
            typeof(DiscoveryManagerTests).Assembly.Location
        };

        var criteria = new DiscoveryCriteria(sources, 100, null);
        var mockLogger = new Mock<ITestDiscoveryEventsHandler2>();

        _discoveryManager.DiscoverTests(criteria, mockLogger.Object);

        var errorMessage = string.Format(CultureInfo.CurrentCulture, CrossPlatEngineResources.DuplicateSource, sources[0]);
        mockLogger.Verify(
            l =>
                l.HandleLogMessage(
                    TestMessageLevel.Warning,
                    errorMessage),
            Times.Once);
    }

    [TestMethod]
    public void DiscoverTestsShouldDiscoverTestsInTheSpecifiedSource()
    {
        TestPluginCacheHelper.SetupMockExtensions(
            [typeof(DiscovererEnumeratorTests).Assembly.Location],
            () => { });

        var sources = new List<string>
        {
            typeof(DiscoveryManagerTests).Assembly.Location
        };

        var criteria = new DiscoveryCriteria(sources, 1, null);
        var mockLogger = new Mock<ITestDiscoveryEventsHandler2>();

        _discoveryManager.DiscoverTests(criteria, mockLogger.Object);

        // Assert that the tests are passed on via the handletestruncomplete event.
        mockLogger.Verify(l => l.HandleDiscoveryComplete(It.IsAny<DiscoveryCompleteEventArgs>(), It.IsAny<IEnumerable<TestCase>>()), Times.Once);
    }

    [TestMethod]
    public void DiscoverTestsShouldSendMetricsOnDiscoveryComplete()
    {
        var metricsCollector = new MetricsCollection();
        metricsCollector.Add("DummyMessage", "DummyValue");

        _mockRequestData.Setup(rd => rd.MetricsCollection).Returns(metricsCollector);

        DiscoveryCompleteEventArgs? receivedDiscoveryCompleteEventArgs = null;

        TestPluginCacheHelper.SetupMockExtensions(
            [typeof(DiscovererEnumeratorTests).Assembly.Location],
            () => { });

        var sources = new List<string>
        {
            typeof(DiscoveryManagerTests).Assembly.Location
        };

        var mockLogger = new Mock<ITestDiscoveryEventsHandler2>();
        var criteria = new DiscoveryCriteria(sources, 1, null);

        mockLogger.Setup(ml => ml.HandleDiscoveryComplete(It.IsAny<DiscoveryCompleteEventArgs>(), It.IsAny<IEnumerable<TestCase>>()))
            .Callback(
                (DiscoveryCompleteEventArgs complete,
                    IEnumerable<TestCase> tests) => receivedDiscoveryCompleteEventArgs = complete);

        // Act.
        _discoveryManager.DiscoverTests(criteria, mockLogger.Object);

        // Assert
        Assert.IsNotNull(receivedDiscoveryCompleteEventArgs!.Metrics);
        Assert.IsTrue(receivedDiscoveryCompleteEventArgs.Metrics.Any());
        Assert.IsTrue(receivedDiscoveryCompleteEventArgs.Metrics.ContainsKey("DummyMessage"));
    }

    [TestMethod]
    public void DiscoverTestsShouldCollectMetrics()
    {
        var mockMetricsCollector = new Mock<IMetricsCollection>();
        var dict = new Dictionary<string, object>
        {
            { "DummyMessage", "DummyValue" }
        };

        mockMetricsCollector.Setup(mc => mc.Metrics).Returns(dict);
        _mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollector.Object);

        TestPluginCacheHelper.SetupMockExtensions(
            [typeof(DiscovererEnumeratorTests).Assembly.Location],
            () => { });

        var sources = new List<string>
        {
            typeof(DiscoveryManagerTests).Assembly.Location
        };

        var mockLogger = new Mock<ITestDiscoveryEventsHandler2>();
        var criteria = new DiscoveryCriteria(sources, 1, null);

        // Act.
        _discoveryManager.DiscoverTests(criteria, mockLogger.Object);

        // Verify.
        mockMetricsCollector.Verify(rd => rd.Add(TelemetryDataConstants.DiscoveryState, It.IsAny<string>()), Times.Once);
        mockMetricsCollector.Verify(rd => rd.Add(TelemetryDataConstants.TotalTestsDiscovered, It.IsAny<object>()), Times.Once);
    }

    [TestMethod]
    public void DiscoveryInitializeShouldVerifyWarningMessageIfAdapterFailedToLoad()
    {
        var assemblyLocation = typeof(DiscoveryManagerTests).Assembly.Location;
        var mockLogger = new Mock<ITestDiscoveryEventsHandler2>();
        TestPluginCacheHelper.SetupMockExtensions(
            [assemblyLocation],
            () => { });

        //Act
        _discoveryManager.Initialize(new List<string> { assemblyLocation }, mockLogger.Object);

        //when handler instance returns warning
        _sessionLogger.SendMessage(TestMessageLevel.Warning, "verify that the HandleLogMessage method getting invoked at least once");

        // Verify.
        mockLogger.Verify(rd => rd.HandleLogMessage(TestMessageLevel.Warning, "verify that the HandleLogMessage method getting invoked at least once"), Times.Once);
    }

    [TestMethod]
    public void DiscoveryTestsShouldSendAbortValuesCorrectlyIfAbortionHappened()
    {
        // Arrange
        var sources = new List<string> { typeof(DiscoveryManagerTests).Assembly.Location };

        var criteria = new DiscoveryCriteria(sources, 100, null);
        var mockHandler = new Mock<ITestDiscoveryEventsHandler2>();

        DiscoveryCompleteEventArgs? receivedDiscoveryCompleteEventArgs = null;

        mockHandler.Setup(ml => ml.HandleDiscoveryComplete(It.IsAny<DiscoveryCompleteEventArgs>(), It.IsAny<IEnumerable<TestCase>>()))
            .Callback((DiscoveryCompleteEventArgs complete, IEnumerable<TestCase> tests) => receivedDiscoveryCompleteEventArgs = complete);

        // Act
        _discoveryManager.DiscoverTests(criteria, mockHandler.Object);
        _discoveryManager.Abort(mockHandler.Object);

        // Assert
        Assert.AreEqual(true, receivedDiscoveryCompleteEventArgs!.IsAborted);
        Assert.AreEqual(-1, receivedDiscoveryCompleteEventArgs.TotalCount);
    }
    #endregion
}
