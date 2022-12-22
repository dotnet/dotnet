// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.TesthostProtocol;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace TestPlatform.CrossPlatEngine.UnitTests.Client;

[TestClass]
public class InProcessProxyDiscoveryManagerTests
{
    private readonly Mock<ITestHostManagerFactory> _mockTestHostManagerFactory;
    private InProcessProxyDiscoveryManager _inProcessProxyDiscoveryManager;
    private readonly Mock<IDiscoveryManager> _mockDiscoveryManager;
    private readonly Mock<ITestRuntimeProvider> _mockTestHostManager;

    public InProcessProxyDiscoveryManagerTests()
    {
        _mockTestHostManagerFactory = new Mock<ITestHostManagerFactory>();
        _mockDiscoveryManager = new Mock<IDiscoveryManager>();
        _mockTestHostManager = new Mock<ITestRuntimeProvider>();
        _mockTestHostManagerFactory.Setup(o => o.GetDiscoveryManager()).Returns(_mockDiscoveryManager.Object);
        _inProcessProxyDiscoveryManager = new InProcessProxyDiscoveryManager(_mockTestHostManager.Object, _mockTestHostManagerFactory.Object);
    }

    [TestMethod]
    public void DiscoverTestsShouldCallInitialize()
    {
        var manualResetEvent = new ManualResetEvent(false);
        _mockDiscoveryManager.Setup(o => o.Initialize(Enumerable.Empty<string>(), null)).Callback(
            () => manualResetEvent.Set());

        var discoveryCriteria = new DiscoveryCriteria(new[] { "test.dll" }, 1, string.Empty);
        _inProcessProxyDiscoveryManager.DiscoverTests(discoveryCriteria, null!);

        Assert.IsTrue(manualResetEvent.WaitOne(5000), "DiscoverTests should call Initialize");
    }

    [TestMethod]
    public void DiscoverTestsShouldUpdateTestPluginCacheWithExtensionsReturnByTestHost()
    {
        var manualResetEvent = new ManualResetEvent(false);
        _mockDiscoveryManager.Setup(o => o.Initialize(Enumerable.Empty<string>(), null)).Callback(
            () => manualResetEvent.Set());

        var path = Path.Combine(Path.GetTempPath(), "DiscoveryDummy.dll");
        _mockTestHostManager.Setup(o => o.GetTestPlatformExtensions(It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>())).Returns(new List<string> { path });
        var expectedResult = TestPluginCache.Instance.GetExtensionPaths(string.Empty);
        expectedResult.Add(path);
        var discoveryCriteria = new DiscoveryCriteria(new[] { "test.dll" }, 1, string.Empty);

        _inProcessProxyDiscoveryManager.DiscoverTests(discoveryCriteria, null!);

        Assert.IsTrue(manualResetEvent.WaitOne(5000), "DiscoverTests should call Initialize");
        CollectionAssert.AreEquivalent(expectedResult, TestPluginCache.Instance.GetExtensionPaths(string.Empty));
    }

    [TestMethod]
    public void DiscoverTestsShouldCallDiscoveryManagerDiscoverTests()
    {
        var discoveryCriteria = new DiscoveryCriteria(new[] { "test.dll" }, 1, string.Empty);
        var mockTestDiscoveryEventsHandler = new Mock<ITestDiscoveryEventsHandler2>();
        var manualResetEvent = new ManualResetEvent(false);

        _mockDiscoveryManager.Setup(o => o.DiscoverTests(discoveryCriteria, mockTestDiscoveryEventsHandler.Object)).Callback(
            () => manualResetEvent.Set());

        _inProcessProxyDiscoveryManager.DiscoverTests(discoveryCriteria, mockTestDiscoveryEventsHandler.Object);

        Assert.IsTrue(manualResetEvent.WaitOne(5000), "IDiscoveryManager.DiscoverTests should get called");
    }

    [TestMethod]
    public void DiscoverTestsShouldCatchExceptionAndCallHandleDiscoveryComplete()
    {
        var discoveryCriteria = new DiscoveryCriteria(new[] { "test.dll" }, 1, string.Empty);
        var mockTestDiscoveryEventsHandler = new Mock<ITestDiscoveryEventsHandler2>();
        var manualResetEvent = new ManualResetEvent(false);

        _mockDiscoveryManager.Setup(o => o.DiscoverTests(discoveryCriteria, mockTestDiscoveryEventsHandler.Object)).Callback(
            () => throw new Exception());

        mockTestDiscoveryEventsHandler.Setup(o => o.HandleDiscoveryComplete(It.IsAny<DiscoveryCompleteEventArgs>(), It.IsAny<IEnumerable<TestCase>>())).Callback(
            () => manualResetEvent.Set());

        _inProcessProxyDiscoveryManager.DiscoverTests(discoveryCriteria, mockTestDiscoveryEventsHandler.Object);

        Assert.IsTrue(manualResetEvent.WaitOne(5000), "ITestDiscoveryEventsHandler.HandleDiscoveryComplete should get called");
    }

    [TestMethod]
    public void AbortShouldCallDiscoveryManagerAbort()
    {
        var manualResetEvent = new ManualResetEvent(false);

        _mockDiscoveryManager.Setup(o => o.Abort()).Callback(
            () => manualResetEvent.Set());

        _inProcessProxyDiscoveryManager.Abort();

        Assert.IsTrue(manualResetEvent.WaitOne(5000), "IDiscoveryManager.Abort should get called");
    }

    [TestMethod]
    public void DiscoverTestRunShouldAllowRuntimeProviderToUpdateAdapterSource()
    {
        var inputSources = new List<string> { "test.dll" };
        var discoveryCriteria = new DiscoveryCriteria(inputSources, 1, string.Empty);
        var mockTestDiscoveryEventsHandler = new Mock<ITestDiscoveryEventsHandler2>();
        var manualResetEvent = new ManualResetEvent(false);

        _mockTestHostManager.Setup(hm => hm.GetTestSources(discoveryCriteria.Sources)).Returns(discoveryCriteria.Sources);
        _mockDiscoveryManager.Setup(o => o.DiscoverTests(discoveryCriteria, mockTestDiscoveryEventsHandler.Object)).Callback(
            () => manualResetEvent.Set());

        _inProcessProxyDiscoveryManager = new InProcessProxyDiscoveryManager(_mockTestHostManager.Object, _mockTestHostManagerFactory.Object);
        _inProcessProxyDiscoveryManager.DiscoverTests(discoveryCriteria, mockTestDiscoveryEventsHandler.Object);

        Assert.IsTrue(manualResetEvent.WaitOne(5000), "IDiscoveryManager.DiscoverTests should get called");
        _mockTestHostManager.Verify(hm => hm.GetTestSources(inputSources), Times.Once);
    }

    [TestMethod]
    public void DiscoverTestRunShouldUpdateTestSourcesIfSourceDiffersFromTestHostManagerSource()
    {
        var actualSources = new List<string> { "actualSource.dll" };
        var inputSource = new List<string> { "inputPackage.appxrecipe" };

        var discoveryCriteria = new DiscoveryCriteria(inputSource, 1, string.Empty);
        var mockTestDiscoveryEventsHandler = new Mock<ITestDiscoveryEventsHandler2>();
        var manualResetEvent = new ManualResetEvent(false);

        _mockTestHostManager.Setup(hm => hm.GetTestSources(discoveryCriteria.Sources)).Returns(actualSources);

        _mockDiscoveryManager.Setup(o => o.DiscoverTests(discoveryCriteria, mockTestDiscoveryEventsHandler.Object)).Callback(
            () => manualResetEvent.Set());

        _inProcessProxyDiscoveryManager.DiscoverTests(discoveryCriteria, mockTestDiscoveryEventsHandler.Object);

        Assert.IsTrue(manualResetEvent.WaitOne(5000), "IDiscoveryManager.DiscoverTests should get called");

        // AdapterSourceMap should contain updated testSources.
        Assert.AreEqual(actualSources.FirstOrDefault(), discoveryCriteria.AdapterSourceMap.FirstOrDefault().Value.FirstOrDefault());
        Assert.AreEqual(inputSource.FirstOrDefault(), discoveryCriteria.Package);
    }
}
