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
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.ClientProtocol;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.TesthostProtocol;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace TestPlatform.CrossPlatEngine.UnitTests.Client;

[TestClass]
public class InProcessProxyExecutionManagerTests
{
    private readonly Mock<ITestHostManagerFactory> _mockTestHostManagerFactory;
    private InProcessProxyExecutionManager _inProcessProxyExecutionManager;
    private readonly Mock<IExecutionManager> _mockExecutionManager;
    private readonly Mock<ITestRuntimeProvider> _mockTestHostManager;

    public InProcessProxyExecutionManagerTests()
    {
        _mockTestHostManagerFactory = new Mock<ITestHostManagerFactory>();
        _mockExecutionManager = new Mock<IExecutionManager>();
        _mockTestHostManager = new Mock<ITestRuntimeProvider>();
        _mockTestHostManagerFactory.Setup(o => o.GetExecutionManager()).Returns(_mockExecutionManager.Object);
        _inProcessProxyExecutionManager = new InProcessProxyExecutionManager(_mockTestHostManager.Object, _mockTestHostManagerFactory.Object);
    }

    [TestMethod]
    public void StartTestRunShouldCallInitialize()
    {
        var testRunCriteria = new TestRunCriteria(new List<string> { "source.dll" }, 10);
        var mockTestMessageEventHandler = new Mock<ITestMessageEventHandler>();
        _inProcessProxyExecutionManager.StartTestRun(testRunCriteria, null!);

        _mockExecutionManager.Verify(o => o.Initialize(Enumerable.Empty<string>(), It.IsAny<ITestMessageEventHandler>()), Times.Once, "StartTestRun should call Initialize if not already initialized");
    }

    [TestMethod]
    public void StartTestRunShouldUpdateTestPlauginCacheWithExtensionsReturnByTestHost()
    {
        var path = Path.Combine(Path.GetTempPath(), "dummy.dll");
        _mockTestHostManager.Setup(o => o.GetTestPlatformExtensions(It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>())).Returns(new List<string> { path });
        var expectedResult = TestPluginCache.Instance.GetExtensionPaths(string.Empty);
        expectedResult.Add(path);

        var testRunCriteria = new TestRunCriteria(new List<string> { "source.dll" }, 10);
        _inProcessProxyExecutionManager.StartTestRun(testRunCriteria, null!);

        CollectionAssert.AreEquivalent(expectedResult, TestPluginCache.Instance.GetExtensionPaths(string.Empty));
    }

    [TestMethod]
    public void StartTestRunShouldCallExecutionManagerStartTestRunWithAdapterSourceMap()
    {
        var testRunCriteria = new TestRunCriteria(new List<string> { "source.dll" }, 10);
        var manualResetEvent = new ManualResetEvent(true);

        _mockExecutionManager.Setup(o => o.StartTestRun(testRunCriteria.AdapterSourceMap!, null, testRunCriteria.TestRunSettings, It.IsAny<TestExecutionContext>(), null, null!)).Callback(
            () => manualResetEvent.Set());

        _inProcessProxyExecutionManager.StartTestRun(testRunCriteria, null!);

        Assert.IsTrue(manualResetEvent.WaitOne(5000), "IExecutionManager.StartTestRun should get called");
    }

    [TestMethod]
    public void StartTestRunShouldAllowRuntimeProviderToUpdateAdapterSource()
    {
        var testRunCriteria = new TestRunCriteria(new List<string> { "source.dll" }, 10);

        _mockTestHostManager.Setup(hm => hm.GetTestSources(testRunCriteria.Sources!)).Returns(testRunCriteria.Sources!);

        _inProcessProxyExecutionManager.StartTestRun(testRunCriteria, null!);

        _mockTestHostManager.Verify(hm => hm.GetTestSources(testRunCriteria.Sources!), Times.Once);
    }

    [TestMethod]
    public void StartTestRunShouldCallExecutionManagerStartTestRunWithTestCase()
    {
        var testRunCriteria = new TestRunCriteria(
            new List<TestCase> { new TestCase("A.C.M", new Uri("excutor://dummy"), "s.dll") },
            frequencyOfRunStatsChangeEvent: 10);
        var manualResetEvent = new ManualResetEvent(true);

        _mockExecutionManager.Setup(o => o.StartTestRun(testRunCriteria.Tests!, null, testRunCriteria.TestRunSettings, It.IsAny<TestExecutionContext>(), null, null!)).Callback(
            () => manualResetEvent.Set());

        _inProcessProxyExecutionManager.StartTestRun(testRunCriteria, null!);

        Assert.IsTrue(manualResetEvent.WaitOne(5000), "IExecutionManager.StartTestRun should get called");
    }

    [TestMethod]
    public void StartTestRunShouldUpdateTestCaseSourceIfTestCaseSourceDiffersFromTestHostManagerSource()
    {
        var actualSources = new List<string> { "actualSource.dll" };
        var inputSource = new List<string> { "inputPackage.appxrecipe" };

        var testRunCriteria = new TestRunCriteria(
            new List<TestCase> { new TestCase("A.C.M", new Uri("excutor://dummy"), inputSource.First()) },
            frequencyOfRunStatsChangeEvent: 10);
        var manualResetEvent = new ManualResetEvent(false);

        _mockTestHostManager.Setup(hm => hm.GetTestSources(inputSource)).Returns(actualSources);

        _mockExecutionManager.Setup(o => o.StartTestRun(testRunCriteria.Tests!, inputSource.FirstOrDefault(), testRunCriteria.TestRunSettings, It.IsAny<TestExecutionContext>(), null, null!))
            .Callback(() => manualResetEvent.Set());

        _inProcessProxyExecutionManager = new InProcessProxyExecutionManager(_mockTestHostManager.Object, _mockTestHostManagerFactory.Object);

        _inProcessProxyExecutionManager.StartTestRun(testRunCriteria, null!);

        Assert.IsTrue(manualResetEvent.WaitOne(5000), "IExecutionManager.StartTestRun should get called");
        _mockExecutionManager.Verify(o => o.StartTestRun(testRunCriteria.Tests!, inputSource.FirstOrDefault(), testRunCriteria.TestRunSettings, It.IsAny<TestExecutionContext>(), null, null!));
        _mockTestHostManager.Verify(hm => hm.GetTestSources(inputSource), Times.Once);
        Assert.AreEqual(actualSources.FirstOrDefault(), testRunCriteria.Tests!.FirstOrDefault()?.Source);
    }

    [TestMethod]
    public void StartTestRunShouldNotUpdateTestCaseSourceIfTestCaseSourceDiffersFromTestHostManagerSource()
    {
        var actualSources = new List<string> { "actualSource.dll" };
        var testRunCriteria = new TestRunCriteria(
            new List<TestCase> { new TestCase("A.C.M", new Uri("excutor://dummy"), actualSources.First()) },
            frequencyOfRunStatsChangeEvent: 10);
        var manualResetEvent = new ManualResetEvent(false);

        _mockTestHostManager.Setup(hm => hm.GetTestSources(actualSources)).Returns(actualSources);

        _mockExecutionManager.Setup(o => o.StartTestRun(testRunCriteria.Tests!, null, testRunCriteria.TestRunSettings, It.IsAny<TestExecutionContext>(), null, null!))
            .Callback(() => manualResetEvent.Set());

        _inProcessProxyExecutionManager.StartTestRun(testRunCriteria, null!);

        Assert.IsTrue(manualResetEvent.WaitOne(5000), "IExecutionManager.StartTestRun should get called");
        _mockExecutionManager.Verify(o => o.StartTestRun(testRunCriteria.Tests!, null, testRunCriteria.TestRunSettings, It.IsAny<TestExecutionContext>(), null, null!));
        _mockTestHostManager.Verify(hm => hm.GetTestSources(actualSources));
        Assert.AreEqual(actualSources.FirstOrDefault(), testRunCriteria.Tests!.FirstOrDefault()?.Source);
    }

    [TestMethod]
    public void StartTestRunShouldCatchExceptionAndCallHandleRunComplete()
    {
        var testRunCriteria = new TestRunCriteria(new List<string> { "source.dll" }, 10);
        var mockTestRunEventsHandler = new Mock<IInternalTestRunEventsHandler>();
        var manualResetEvent = new ManualResetEvent(true);

        _mockExecutionManager.Setup(o => o.StartTestRun(testRunCriteria.AdapterSourceMap!, null, testRunCriteria.TestRunSettings, It.IsAny<TestExecutionContext>(), null, mockTestRunEventsHandler.Object)).Callback(
            () => throw new Exception());

        mockTestRunEventsHandler.Setup(o => o.HandleTestRunComplete(It.IsAny<TestRunCompleteEventArgs>(), null, null, null)).Callback(
            () => manualResetEvent.Set());

        _inProcessProxyExecutionManager.StartTestRun(testRunCriteria, mockTestRunEventsHandler.Object);

        Assert.IsTrue(manualResetEvent.WaitOne(5000), "IInternalTestRunEventsHandler.HandleTestRunComplete should get called");
    }

    [TestMethod]
    public void AbortShouldCallExecutionManagerAbort()
    {
        var manualResetEvent = new ManualResetEvent(true);

        _mockExecutionManager.Setup(o => o.Abort(It.IsAny<IInternalTestRunEventsHandler>())).Callback(
            () => manualResetEvent.Set());

        _inProcessProxyExecutionManager.Abort(It.IsAny<IInternalTestRunEventsHandler>());

        Assert.IsTrue(manualResetEvent.WaitOne(5000), "IExecutionManager.Abort should get called");
    }

    [TestMethod]
    public void CancelShouldCallExecutionManagerCancel()
    {
        var manualResetEvent = new ManualResetEvent(true);

        _mockExecutionManager.Setup(o => o.Cancel(It.IsAny<IInternalTestRunEventsHandler>())).Callback(
            () => manualResetEvent.Set());

        _inProcessProxyExecutionManager.Cancel(It.IsAny<IInternalTestRunEventsHandler>());

        Assert.IsTrue(manualResetEvent.WaitOne(5000), "IExecutionManager.Abort should get called");
    }
}
