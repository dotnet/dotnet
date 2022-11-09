// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using CrossPlatEngineResources = Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Resources;

namespace TestPlatform.CrossPlatEngine.UnitTests.Client;

[TestClass]
public class ProxyExecutionManagerTests : ProxyBaseManagerTests
{
    private readonly Mock<ITestRequestSender> _mockRequestSender;
    private readonly Mock<TestRunCriteria> _mockTestRunCriteria;
    private readonly Mock<IRequestData> _mockRequestData;
    private readonly Mock<IMetricsCollection> _mockMetricsCollection;
    private readonly Mock<IFileHelper> _mockFileHelper;

    private ProxyExecutionManager _testExecutionManager;

    //private Mock<IDataSerializer> mockDataSerializer;

    public ProxyExecutionManagerTests()
    {
        _mockRequestSender = new Mock<ITestRequestSender>();
        _mockTestRunCriteria = new Mock<TestRunCriteria>(new List<string> { "source.dll" }, 10);
        //this.mockDataSerializer = new Mock<IDataSerializer>();
        _mockRequestData = new Mock<IRequestData>();
        _mockMetricsCollection = new Mock<IMetricsCollection>();
        _mockFileHelper = new Mock<IFileHelper>();
        _mockRequestData.Setup(rd => rd.MetricsCollection).Returns(_mockMetricsCollection.Object);

        _testExecutionManager = new ProxyExecutionManager(_mockRequestData.Object, _mockRequestSender.Object, _mockTestHostManager.Object, Framework.DefaultFramework, _mockDataSerializer.Object, _mockFileHelper.Object);

        //this.mockDataSerializer.Setup(mds => mds.DeserializeMessage(null)).Returns(new Message());
        //this.mockDataSerializer.Setup(mds => mds.DeserializeMessage(string.Empty)).Returns(new Message());
    }

    [TestMethod]
    public void StartTestRunShouldNotInitializeExtensionsOnNoExtensions()
    {
        // Make sure TestPlugincache is refreshed.
        TestPluginCache.Instance = null;

        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(true);

        _testExecutionManager.StartTestRun(_mockTestRunCriteria.Object, null!);

        _mockRequestSender.Verify(s => s.InitializeExecution(It.IsAny<IEnumerable<string>>()), Times.Never);
    }

    [TestMethod]
    public void StartTestRunShouldAllowRuntimeProviderToUpdateAdapterSource()
    {
        // Make sure TestPlugincache is refreshed.
        TestPluginCache.Instance = null;

        _mockTestHostManager.Setup(hm => hm.GetTestSources(_mockTestRunCriteria.Object.Sources!)).Returns(_mockTestRunCriteria.Object.Sources!);
        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(true);

        Mock<IInternalTestRunEventsHandler> mockTestRunEventsHandler = new();

        _testExecutionManager.StartTestRun(_mockTestRunCriteria.Object, mockTestRunEventsHandler.Object);

        _mockTestHostManager.Verify(hm => hm.GetTestSources(_mockTestRunCriteria.Object.Sources!), Times.Once);
    }

    [TestMethod]
    public void StartTestRunShouldUpdateTestCaseSourceIfTestCaseSourceDiffersFromTestHostManagerSource()
    {
        var actualSources = new List<string> { "actualSource.dll" };
        var inputSource = new List<string> { "inputPackage.appxrecipe" };

        var testRunCriteria = new TestRunCriteria(
            new List<TestCase> { new TestCase("A.C.M", new Uri("excutor://dummy"), inputSource.First()) },
            frequencyOfRunStatsChangeEvent: 10);

        _mockTestHostManager.Setup(hm => hm.GetTestSources(inputSource)).Returns(actualSources);
        _mockTestHostManager.Setup(tmh => tmh.LaunchTestHostAsync(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(true));
        _mockTestHostManager.Setup(th => th.GetTestPlatformExtensions(It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>())).Returns(new List<string>());
        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(true);
        Mock<IInternalTestRunEventsHandler> mockTestRunEventsHandler = new();

        _testExecutionManager.StartTestRun(testRunCriteria, mockTestRunEventsHandler.Object);

        _mockTestHostManager.Verify(hm => hm.GetTestSources(inputSource), Times.Once);
        Assert.AreEqual(actualSources.FirstOrDefault(), testRunCriteria.Tests!.FirstOrDefault()?.Source);
    }

    [TestMethod]
    public void StartTestRunShouldNotUpdateTestCaseSourceIfTestCaseSourceDoNotDifferFromTestHostManagerSource()
    {
        var actualSources = new List<string> { "actualSource.dll" };
        var inputSource = new List<string> { "actualSource.dll" };

        var testRunCriteria = new TestRunCriteria(
            new List<TestCase> { new TestCase("A.C.M", new Uri("excutor://dummy"), inputSource.First()) },
            frequencyOfRunStatsChangeEvent: 10);

        _mockTestHostManager.Setup(hm => hm.GetTestSources(inputSource)).Returns(actualSources);
        _mockTestHostManager.Setup(tmh => tmh.LaunchTestHostAsync(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(true));
        _mockTestHostManager.Setup(th => th.GetTestPlatformExtensions(It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>())).Returns(new List<string>());
        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(true);
        Mock<IInternalTestRunEventsHandler> mockTestRunEventsHandler = new();

        _testExecutionManager.StartTestRun(testRunCriteria, mockTestRunEventsHandler.Object);

        _mockTestHostManager.Verify(hm => hm.GetTestSources(inputSource), Times.Once);
        Assert.AreEqual(actualSources.FirstOrDefault(), testRunCriteria.Tests!.FirstOrDefault()?.Source);
    }

    [TestMethod]
    public void StartTestRunShouldNotInitializeExtensionsOnCommunicationFailure()
    {
        // Make sure TestPlugincache is refreshed.
        TestPluginCache.Instance = null;

        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(false);

        Mock<IInternalTestRunEventsHandler> mockTestRunEventsHandler = new();

        _testExecutionManager.StartTestRun(_mockTestRunCriteria.Object, mockTestRunEventsHandler.Object);

        _mockRequestSender.Verify(s => s.InitializeExecution(It.IsAny<IEnumerable<string>>()), Times.Never);
    }

    [TestMethod]
    public void StartTestRunShouldInitializeExtensionsIfPresent()
    {
        // Make sure TestPlugincache is refreshed.
        TestPluginCache.Instance = null;

        try
        {
            var extensions = new List<string>() { "C:\\foo.dll" };
            _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(true);
            _mockTestHostManager.Setup(x => x.GetTestPlatformExtensions(It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>()))
                .Returns(extensions);

            _testExecutionManager.StartTestRun(_mockTestRunCriteria.Object, null!);

            // Also verify that we have waited for client connection.
            _mockRequestSender.Verify(s => s.InitializeExecution(extensions), Times.Once);
        }
        finally
        {
            TestPluginCache.Instance = null;
        }
    }

    [TestMethod]
    public void StartTestRunShouldQueryTestHostManagerForExtensions()
    {
        TestPluginCache.Instance = null;
        try
        {
            _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(true);
            _mockTestHostManager.Setup(th => th.GetTestPlatformExtensions(It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>())).Returns(new[] { "he1.dll", "c:\\e1.dll" });

            _testExecutionManager.StartTestRun(_mockTestRunCriteria.Object, null!);

            _mockRequestSender.Verify(s => s.InitializeExecution(new[] { "he1.dll", "c:\\e1.dll" }), Times.Once);
        }
        finally
        {
            TestPluginCache.Instance = null;
        }
    }

    [TestMethod]
    public void StartTestRunShouldPassAdapterToTestHostManagerFromTestPluginCacheExtensions()
    {
        // We are updating extension with testadapter only to make it easy to test.
        // In product code it filter out testadapter from extension
        TestPluginCache.Instance.UpdateExtensions(new List<string> { "abc.TestAdapter.dll", "xyz.TestAdapter.dll" }, false);
        try
        {
            _mockFileHelper.Setup(fh => fh.Exists(It.IsAny<string>())).Returns(true);
            _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(true);
            var expectedResult = TestPluginCache.Instance.GetExtensionPaths(string.Empty);

            _testExecutionManager.StartTestRun(_mockTestRunCriteria.Object, null!);

            _mockTestHostManager.Verify(th => th.GetTestPlatformExtensions(It.IsAny<IEnumerable<string>>(), expectedResult), Times.Once);
        }
        finally
        {
            TestPluginCache.Instance = null;
        }
    }

    [TestMethod]
    public void StartTestRunShouldNotInitializeDefaultAdaptersIfSkipDefaultAdaptersIsTrue()
    {
        InvokeAndVerifyStartTestRun(true);
    }

    [TestMethod]
    public void StartTestRunShouldInitializeDefaultAdaptersIfSkipDefaultAdaptersIsFalse()
    {
        InvokeAndVerifyStartTestRun(false);
    }

    [TestMethod]
    public void StartTestRunShouldIntializeTestHost()
    {
        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(true);
        _mockFileHelper.Setup(fh => fh.Exists(It.IsAny<string>())).Returns(true);

        _testExecutionManager.StartTestRun(_mockTestRunCriteria.Object, null!);

        _mockRequestSender.Verify(s => s.InitializeCommunication(), Times.Once);
        _mockTestHostManager.Verify(thl => thl.LaunchTestHostAsync(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public void StartTestRunShouldNotSendStartTestRunRequestIfCommunicationFails()
    {
        _mockTestHostManager.Setup(tmh => tmh.LaunchTestHostAsync(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>()))
            .Callback(
                () => _mockTestHostManager.Raise(thm => thm.HostLaunched += null, new HostProviderEventArgs(string.Empty)))
            .Returns(Task.FromResult(false));

        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(true);

        // Make sure TestPlugincache is refreshed.
        TestPluginCache.Instance = null;

        Mock<IInternalTestRunEventsHandler> mockTestRunEventsHandler = new();

        _testExecutionManager.StartTestRun(_mockTestRunCriteria.Object, mockTestRunEventsHandler.Object);

        _mockRequestSender.Verify(s => s.StartTestRun(It.IsAny<TestRunCriteriaWithSources>(), It.IsAny<IInternalTestRunEventsHandler>()), Times.Never);
    }

    [TestMethod]
    public void StartTestRunShouldInitializeExtensionsIfTestHostIsNotShared()
    {
        TestPluginCache.Instance = null;
        _mockTestHostManager.SetupGet(th => th.Shared).Returns(false);
        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(true);
        _mockTestHostManager.Setup(th => th.GetTestPlatformExtensions(It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>())).Returns(new[] { "x.dll" });

        _testExecutionManager.StartTestRun(_mockTestRunCriteria.Object, null!);

        _mockRequestSender.Verify(s => s.InitializeExecution(It.IsAny<IEnumerable<string>>()), Times.Once);
    }

    [TestMethod]
    public void StartTestRunShouldInitializeExtensionsWithExistingExtensionsOnly()
    {
        TestPluginCache.Instance = null;
        TestPluginCache.Instance!.UpdateExtensions(new List<string> { "abc.TestAdapter.dll", "def.TestAdapter.dll", "xyz.TestAdapter.dll" }, false);
        var expectedOutputPaths = new[] { "abc.TestAdapter.dll", "xyz.TestAdapter.dll" };

        _mockTestHostManager.SetupGet(th => th.Shared).Returns(false);
        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(true);
        _mockTestHostManager.Setup(th => th.GetTestPlatformExtensions(It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>())).Returns((IEnumerable<string> sources, IEnumerable<string> extensions) => extensions.Select(extension => Path.GetFileName(extension)));

        _mockFileHelper.Setup(fh => fh.Exists(It.IsAny<string>())).Returns((string extensionPath) => !extensionPath.Contains("def.TestAdapter.dll"));

        _mockFileHelper.Setup(fh => fh.Exists("def.TestAdapter.dll")).Returns(false);
        _mockFileHelper.Setup(fh => fh.Exists("xyz.TestAdapter.dll")).Returns(true);

        var mockTestRunEventsHandler = new Mock<IInternalTestRunEventsHandler>();
        _testExecutionManager.StartTestRun(_mockTestRunCriteria.Object, mockTestRunEventsHandler.Object);

        _mockRequestSender.Verify(s => s.InitializeExecution(expectedOutputPaths), Times.Once);
    }

    [TestMethod]
    public void SetupChannelShouldThrowExceptionIfClientConnectionTimeout()
    {
        string runsettings = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<RunSettings>\r\n  <DataCollectionRunSettings>\r\n    <DataCollectors >{0}</DataCollectors>\r\n  </DataCollectionRunSettings>\r\n</RunSettings>";

        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(false);
        _mockTestHostManager.Setup(tmh => tmh.LaunchTestHostAsync(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(false));

        Assert.ThrowsException<TestPlatformException>(() => _testExecutionManager.SetupChannel(new List<string> { "source.dll" }, runsettings));
    }

    [TestMethod]
    public void SetupChannelShouldThrowExceptionWithOneSourceIfTestHostExitedBeforeConnectionIsEstablished()
    {
        string runsettings = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<RunSettings>\r\n  <DataCollectionRunSettings>\r\n    <DataCollectors >{0}</DataCollectors>\r\n  </DataCollectionRunSettings>\r\n</RunSettings>";

        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(false);
        _mockTestHostManager.Setup(tmh => tmh.LaunchTestHostAsync(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(true)).Callback(() => _mockTestHostManager.Raise(t => t.HostExited += null, new HostProviderEventArgs("I crashed!")));

        Assert.AreEqual(string.Format(CultureInfo.CurrentCulture, CrossPlatEngineResources.Resources.TestHostExitedWithError, "source.dll", "I crashed!"), Assert.ThrowsException<TestPlatformException>(() => _testExecutionManager.SetupChannel(new List<string> { "source.dll" }, runsettings)).Message);
    }

    [TestMethod]
    public void SetupChannelShouldThrowExceptionWithAllSourcesIfTestHostExitedBeforeConnectionIsEstablished()
    {
        string runsettings = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<RunSettings>\r\n  <DataCollectionRunSettings>\r\n    <DataCollectors >{0}</DataCollectors>\r\n  </DataCollectionRunSettings>\r\n</RunSettings>";

        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(false);
        _mockTestHostManager.Setup(tmh => tmh.LaunchTestHostAsync(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(true)).Callback(() => _mockTestHostManager.Raise(t => t.HostExited += null, new HostProviderEventArgs("I crashed!")));

        Assert.AreEqual(string.Format(CultureInfo.CurrentCulture, CrossPlatEngineResources.Resources.TestHostExitedWithError, string.Join("', '", new[] { "source1.dll", "source2.dll" }), "I crashed!"), Assert.ThrowsException<TestPlatformException>(() => _testExecutionManager.SetupChannel(new List<string> { "source1.dll", "source2.dll" }, runsettings)).Message);
    }

    [TestMethod]
    public void StartTestRunShouldCatchExceptionAndCallHandleTestRunComplete()
    {
        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(false);
        _mockTestHostManager.Setup(tmh => tmh.LaunchTestHostAsync(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(false));

        Mock<IInternalTestRunEventsHandler> mockTestRunEventsHandler = new();

        _testExecutionManager.StartTestRun(_mockTestRunCriteria.Object, mockTestRunEventsHandler.Object);

        mockTestRunEventsHandler.Verify(s => s.HandleTestRunComplete(It.Is<TestRunCompleteEventArgs>(t => t.IsAborted == true), null, null, null));
    }

    [TestMethod]
    public void StartTestRunShouldCatchExceptionAndCallHandleRawMessageOfTestRunComplete()
    {
        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(false);
        _mockTestHostManager.Setup(tmh => tmh.LaunchTestHostAsync(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(false));

        _mockDataSerializer.Setup(ds => ds.SerializePayload(MessageType.TestMessage, It.IsAny<TestMessagePayload>())).Returns(MessageType.TestMessage);
        _mockDataSerializer.Setup(ds => ds.SerializePayload(MessageType.ExecutionComplete, It.IsAny<TestRunCompletePayload>())).Returns(MessageType.ExecutionComplete);

        _mockDataSerializer.Setup(mds => mds.DeserializeMessage(It.IsAny<string>())).Returns((string rawMessage) =>
        {
            var messageType = rawMessage.Contains(MessageType.ExecutionComplete) ? MessageType.ExecutionComplete : MessageType.TestMessage;
            var message = new Message
            {
                MessageType = messageType
            };

            return message;
        });

        Mock<IInternalTestRunEventsHandler> mockTestRunEventsHandler = new();

        _testExecutionManager.StartTestRun(_mockTestRunCriteria.Object, mockTestRunEventsHandler.Object);

        mockTestRunEventsHandler.Verify(s => s.HandleRawMessage(It.Is<string>(str => str.Contains(MessageType.ExecutionComplete))), Times.Once);
    }

    [TestMethod]
    public void StartTestRunShouldCatchExceptionAndCallHandleRawMessageOfTestMessage()
    {
        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(false);
        _mockTestHostManager.Setup(tmh => tmh.LaunchTestHostAsync(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(false));

        _mockDataSerializer.Setup(ds => ds.SerializePayload(MessageType.TestMessage, It.IsAny<TestMessagePayload>())).Returns(MessageType.TestMessage);
        _mockDataSerializer.Setup(ds => ds.SerializePayload(MessageType.ExecutionComplete, It.IsAny<TestRunCompletePayload>())).Returns(MessageType.ExecutionComplete);

        _mockDataSerializer.Setup(mds => mds.DeserializeMessage(It.IsAny<string>())).Returns((string rawMessage) =>
        {
            var messageType = rawMessage.Contains(MessageType.ExecutionComplete) ? MessageType.ExecutionComplete : MessageType.TestMessage;
            var message = new Message
            {
                MessageType = messageType
            };

            return message;
        });

        Mock<IInternalTestRunEventsHandler> mockTestRunEventsHandler = new();

        _testExecutionManager.StartTestRun(_mockTestRunCriteria.Object, mockTestRunEventsHandler.Object);

        mockTestRunEventsHandler.Verify(s => s.HandleRawMessage(It.Is<string>(str => str.Contains(MessageType.TestMessage))));
    }

    [TestMethod]
    public void StartTestRunShouldCatchExceptionAndCallHandleLogMessageOfError()
    {
        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(false);
        _mockTestHostManager.Setup(tmh => tmh.LaunchTestHostAsync(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(false));

        Mock<IInternalTestRunEventsHandler> mockTestRunEventsHandler = new();

        _testExecutionManager.StartTestRun(_mockTestRunCriteria.Object, mockTestRunEventsHandler.Object);

        mockTestRunEventsHandler.Verify(s => s.HandleLogMessage(TestMessageLevel.Error, It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public void StartTestRunShouldCatchExceptionAndCallHandleRawMessageAndHandleLogMessage()
    {
        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(false);
        _mockTestHostManager.Setup(tmh => tmh.LaunchTestHostAsync(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(false));

        Mock<IInternalTestRunEventsHandler> mockTestRunEventsHandler = new();

        _testExecutionManager.StartTestRun(_mockTestRunCriteria.Object, mockTestRunEventsHandler.Object);

        mockTestRunEventsHandler.Verify(s => s.HandleRawMessage(It.IsAny<string>()));
        mockTestRunEventsHandler.Verify(s => s.HandleLogMessage(TestMessageLevel.Error, It.IsAny<string>()));
    }

    [TestMethod]
    public void StartTestRunForCancelRequestShouldHandleLogMessageWithProperErrorMessage()
    {
        Mock<IInternalTestRunEventsHandler> mockTestRunEventsHandler = new();
        _testExecutionManager.Cancel(mockTestRunEventsHandler.Object);

        _testExecutionManager.StartTestRun(_mockTestRunCriteria.Object, mockTestRunEventsHandler.Object);

        mockTestRunEventsHandler.Verify(s => s.HandleLogMessage(TestMessageLevel.Error, "Cancelling the operation as requested."));
    }

    [TestMethod]
    public void StartTestRunForAnExceptionDuringLaunchOfTestShouldHandleLogMessageWithProperErrorMessage()
    {
        Mock<IInternalTestRunEventsHandler> mockTestRunEventsHandler = new();
        _mockTestHostManager.Setup(tmh => tmh.LaunchTestHostAsync(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>())).Throws(new Exception("DummyException"));

        _testExecutionManager.StartTestRun(_mockTestRunCriteria.Object, mockTestRunEventsHandler.Object);

        mockTestRunEventsHandler.Verify(s => s.HandleLogMessage(TestMessageLevel.Error, It.Is<string>(str => str.StartsWith("Failed to launch testhost with error: System.Exception: DummyException"))));
    }

    [TestMethod]
    public void StartTestRunShouldInitiateTestRunForSourcesThroughTheServer()
    {
        TestRunCriteriaWithSources? testRunCriteriaPassed = null;
        _mockFileHelper.Setup(fh => fh.Exists(It.IsAny<string>())).Returns(true);
        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(true);
        _mockRequestSender.Setup(s => s.StartTestRun(It.IsAny<TestRunCriteriaWithSources>(), _testExecutionManager))
            .Callback(
                (TestRunCriteriaWithSources criteria, IInternalTestRunEventsHandler sink) => testRunCriteriaPassed = criteria);

        _testExecutionManager.StartTestRun(_mockTestRunCriteria.Object, null!);

        Assert.IsNotNull(testRunCriteriaPassed);
        CollectionAssert.AreEqual(_mockTestRunCriteria.Object.AdapterSourceMap!.Keys, testRunCriteriaPassed.AdapterSourceMap.Keys);
        CollectionAssert.AreEqual(_mockTestRunCriteria.Object.AdapterSourceMap.Values, testRunCriteriaPassed.AdapterSourceMap.Values);
        Assert.AreEqual(_mockTestRunCriteria.Object.FrequencyOfRunStatsChangeEvent, testRunCriteriaPassed.TestExecutionContext!.FrequencyOfRunStatsChangeEvent);
        Assert.AreEqual(_mockTestRunCriteria.Object.RunStatsChangeEventTimeout, testRunCriteriaPassed.TestExecutionContext.RunStatsChangeEventTimeout);
        Assert.AreEqual(_mockTestRunCriteria.Object.TestRunSettings, testRunCriteriaPassed.RunSettings);
    }

    [TestMethod]
    public void StartTestRunShouldInitiateTestRunForTestsThroughTheServer()
    {
        TestRunCriteriaWithTests? testRunCriteriaPassed = null;
        _mockFileHelper.Setup(fh => fh.Exists(It.IsAny<string>())).Returns(true);
        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(true);
        _mockRequestSender.Setup(s => s.StartTestRun(It.IsAny<TestRunCriteriaWithTests>(), _testExecutionManager))
            .Callback(
                (TestRunCriteriaWithTests criteria, IInternalTestRunEventsHandler sink) => testRunCriteriaPassed = criteria);
        var runCriteria = new Mock<TestRunCriteria>(
            new List<TestCase> { new TestCase("A.C.M", new Uri("executor://dummy"), "source.dll") },
            10);

        _testExecutionManager.StartTestRun(runCriteria.Object, null!);

        Assert.IsNotNull(testRunCriteriaPassed);
        CollectionAssert.AreEqual(runCriteria.Object.Tests!.ToList(), testRunCriteriaPassed.Tests.ToList());
        Assert.AreEqual(
            runCriteria.Object.FrequencyOfRunStatsChangeEvent,
            testRunCriteriaPassed.TestExecutionContext!.FrequencyOfRunStatsChangeEvent);
        Assert.AreEqual(
            runCriteria.Object.RunStatsChangeEventTimeout,
            testRunCriteriaPassed.TestExecutionContext.RunStatsChangeEventTimeout);
        Assert.AreEqual(
            runCriteria.Object.TestRunSettings,
            testRunCriteriaPassed.RunSettings);
    }

    [TestMethod]
    public void CloseShouldSignalToServerSessionEndIfTestHostWasLaunched()
    {
        string runsettings = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<RunSettings>\r\n  <DataCollectionRunSettings>\r\n    <DataCollectors >{0}</DataCollectors>\r\n  </DataCollectionRunSettings>\r\n</RunSettings>";

        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(true);

        _testExecutionManager.SetupChannel(new List<string> { "source.dll" }, runsettings);

        _testExecutionManager.Close();

        _mockRequestSender.Verify(s => s.EndSession(), Times.Once);
    }

    [TestMethod]
    public void CloseShouldNotSendSignalToServerSessionEndIfTestHostWasNotLaunched()
    {
        _testExecutionManager.Close();

        _mockRequestSender.Verify(s => s.EndSession(), Times.Never);
    }

    [TestMethod]
    public void CloseShouldSignalServerSessionEndEachTime()
    {
        string runsettings = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<RunSettings>\r\n  <DataCollectionRunSettings>\r\n    <DataCollectors >{0}</DataCollectors>\r\n  </DataCollectionRunSettings>\r\n</RunSettings>";

        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(true);

        _testExecutionManager.SetupChannel(new List<string> { "source.dll" }, runsettings);

        _testExecutionManager.Close();
        _testExecutionManager.Close();

        _mockRequestSender.Verify(s => s.EndSession(), Times.Exactly(2));
    }

    [TestMethod]
    public void CancelShouldNotSendSendTestRunCancelIfCommunicationFails()
    {
        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(false);

        Mock<IInternalTestRunEventsHandler> mockTestRunEventsHandler = new();

        _testExecutionManager.StartTestRun(_mockTestRunCriteria.Object, mockTestRunEventsHandler.Object);

        _testExecutionManager.Cancel(It.IsAny<IInternalTestRunEventsHandler>());

        _mockRequestSender.Verify(s => s.SendTestRunCancel(), Times.Never);
    }

    [TestMethod]
    public void AbortShouldSendTestRunAbortIfCommunicationSuccessful()
    {
        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(true);

        Mock<IInternalTestRunEventsHandler> mockTestRunEventsHandler = new();

        _testExecutionManager.StartTestRun(_mockTestRunCriteria.Object, mockTestRunEventsHandler.Object);

        _testExecutionManager.Abort(It.IsAny<IInternalTestRunEventsHandler>());

        _mockRequestSender.Verify(s => s.SendTestRunAbort(), Times.Once);
    }

    [TestMethod]
    public void AbortShouldNotSendTestRunAbortIfCommunicationFails()
    {
        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(false);

        Mock<IInternalTestRunEventsHandler> mockTestRunEventsHandler = new();

        _testExecutionManager.StartTestRun(_mockTestRunCriteria.Object, mockTestRunEventsHandler.Object);

        _testExecutionManager.Abort(It.IsAny<IInternalTestRunEventsHandler>());

        _mockRequestSender.Verify(s => s.SendTestRunAbort(), Times.Never);
    }

    [TestMethod]
    public void ExecuteTestsCloseTestHostIfRawMessageIfOfTypeExecutionComplete()
    {
        Mock<IInternalTestRunEventsHandler> mockTestRunEventsHandler = new();

        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(false);

        _mockDataSerializer.Setup(ds => ds.SerializePayload(MessageType.TestMessage, It.IsAny<TestMessagePayload>())).Returns(MessageType.TestMessage);
        _mockDataSerializer.Setup(ds => ds.SerializePayload(MessageType.ExecutionComplete, It.IsAny<TestRunCompletePayload>())).Returns(MessageType.ExecutionComplete);

        _mockDataSerializer.Setup(mds => mds.DeserializeMessage(It.IsAny<string>())).Returns((string rawMessage) =>
        {
            var messageType = rawMessage.Contains(MessageType.ExecutionComplete) ? MessageType.ExecutionComplete : MessageType.TestMessage;
            var message = new Message
            {
                MessageType = messageType
            };

            return message;
        });

        // Act.
        _testExecutionManager.StartTestRun(_mockTestRunCriteria.Object, mockTestRunEventsHandler.Object);

        // Verify
        _mockTestHostManager.Verify(mthm => mthm.CleanTestHostAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public void ExecuteTestsShouldNotCloseTestHostIfRawMessageIsNotOfTypeExecutionComplete()
    {
        Mock<IInternalTestRunEventsHandler> mockTestRunEventsHandler = new();
        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(false);

        _mockDataSerializer.Setup(mds => mds.DeserializeMessage(It.IsAny<string>())).Returns(() =>
        {
            var message = new Message
            {
                MessageType = MessageType.ExecutionInitialize
            };

            return message;
        });

        // Act.
        _testExecutionManager.StartTestRun(_mockTestRunCriteria.Object, mockTestRunEventsHandler.Object);

        // Verify
        _mockTestHostManager.Verify(mthm => mthm.CleanTestHostAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [TestMethod]
    public void ExecutionManagerShouldPassOnTestRunStatsChange()
    {
        _mockFileHelper.Setup(fh => fh.Exists(It.IsAny<string>())).Returns(true);
        Mock<IInternalTestRunEventsHandler> mockTestRunEventsHandler = new();
        var runCriteria = new Mock<TestRunCriteria>(
            new List<TestCase> { new TestCase("A.C.M", new Uri("executor://dummy"), "source.dll") },
            10);
        var testRunChangedArgs = new TestRunChangedEventArgs(null, null, null);

        _testExecutionManager = GetProxyExecutionManager();

        var completePayload = new TestRunCompletePayload()
        {
            ExecutorUris = null,
            LastRunTests = null,
            RunAttachments = null,
            TestRunCompleteArgs = null
        };
        var completeMessage = new Message() { MessageType = MessageType.ExecutionComplete, Payload = null };
        SetupChannelMessage(MessageType.StartTestExecutionWithTests, MessageType.TestRunStatsChange, testRunChangedArgs);

        mockTestRunEventsHandler.Setup(mh => mh.HandleTestRunStatsChange(It.IsAny<TestRunChangedEventArgs>())).Callback(
            () =>
            {
                _mockDataSerializer.Setup(ds => ds.DeserializeMessage(It.IsAny<string>())).Returns(completeMessage);
                _mockDataSerializer.Setup(ds => ds.DeserializePayload<TestRunCompletePayload>(completeMessage)).Returns(completePayload);
                _mockDataSerializer.Setup(ds => ds.SerializeMessage(It.IsAny<string>()))
                    .Returns(MessageType.SessionEnd);
                RaiseMessageReceived(MessageType.ExecutionComplete);
            });

        var waitHandle = new AutoResetEvent(false);
        mockTestRunEventsHandler.Setup(mh => mh.HandleTestRunComplete(
            It.IsAny<TestRunCompleteEventArgs>(),
            It.IsAny<TestRunChangedEventArgs>(),
            It.IsAny<ICollection<AttachmentSet>>(),
            It.IsAny<ICollection<string>>())).Callback(() => waitHandle.Set());

        // Act.
        _testExecutionManager.StartTestRun(runCriteria.Object, mockTestRunEventsHandler.Object);
        waitHandle.WaitOne();

        // Verify
        mockTestRunEventsHandler.Verify(mtdeh => mtdeh.HandleTestRunStatsChange(It.IsAny<TestRunChangedEventArgs>()), Times.Once);
    }

    [TestMethod]
    public void ExecutionManagerShouldPassOnHandleLogMessage()
    {
        Mock<IInternalTestRunEventsHandler> mockTestRunEventsHandler = new();
        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(false);

        _mockDataSerializer.Setup(mds => mds.DeserializeMessage(It.IsAny<string>())).Returns(() =>
        {
            var message = new Message
            {
                MessageType = MessageType.TestMessage
            };

            return message;
        });

        // Act.
        _testExecutionManager.StartTestRun(_mockTestRunCriteria.Object, mockTestRunEventsHandler.Object);

        // Verify
        mockTestRunEventsHandler.Verify(mtdeh => mtdeh.HandleLogMessage(It.IsAny<TestMessageLevel>(), It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public void ExecutionManagerShouldPassOnLaunchProcessWithDebuggerAttached()
    {
        _mockFileHelper.Setup(fh => fh.Exists(It.IsAny<string>())).Returns(true);
        Mock<IInternalTestRunEventsHandler> mockTestRunEventsHandler = new();
        var runCriteria = new Mock<TestRunCriteria>(
            new List<TestCase> { new TestCase("A.C.M", new Uri("executor://dummy"), "source.dll") },
            10);
        var payload = new TestProcessStartInfo();

        _testExecutionManager = GetProxyExecutionManager();

        var completePayload = new TestRunCompletePayload()
        {
            ExecutorUris = null,
            LastRunTests = null,
            RunAttachments = null,
            TestRunCompleteArgs = null
        };
        var completeMessage = new Message() { MessageType = MessageType.ExecutionComplete, Payload = null };
        SetupChannelMessage(MessageType.StartTestExecutionWithTests,
            MessageType.LaunchAdapterProcessWithDebuggerAttached, payload);

        mockTestRunEventsHandler.Setup(mh => mh.LaunchProcessWithDebuggerAttached(It.IsAny<TestProcessStartInfo>())).Callback(
            () =>
            {
                _mockDataSerializer.Setup(ds => ds.DeserializeMessage(It.IsAny<string>())).Returns(completeMessage);
                _mockDataSerializer.Setup(ds => ds.DeserializePayload<TestRunCompletePayload>(completeMessage)).Returns(completePayload);
                _mockDataSerializer.Setup(ds => ds.SerializeMessage(It.IsAny<string>()))
                    .Returns(MessageType.SessionEnd);
                RaiseMessageReceived(MessageType.ExecutionComplete);
            });

        var waitHandle = new AutoResetEvent(false);
        mockTestRunEventsHandler.Setup(mh => mh.HandleTestRunComplete(
            It.IsAny<TestRunCompleteEventArgs>(),
            It.IsAny<TestRunChangedEventArgs>(),
            It.IsAny<ICollection<AttachmentSet>>(),
            It.IsAny<ICollection<string>>())).Callback(() => waitHandle.Set());

        _testExecutionManager.StartTestRun(runCriteria.Object, mockTestRunEventsHandler.Object);

        waitHandle.WaitOne();

        // Verify
        mockTestRunEventsHandler.Verify(mtdeh => mtdeh.LaunchProcessWithDebuggerAttached(It.IsAny<TestProcessStartInfo>()), Times.Once);
    }

    [TestMethod]
    public void StartTestRunShouldAttemptToTakeProxyFromPoolIfProxyIsNull()
    {
        var testSessionInfo = new TestSessionInfo();

        Func<string, ProxyExecutionManager, ProxyOperationManager>
            proxyOperationManagerCreator = (
                string source,
                ProxyExecutionManager proxyExecutionManager) =>
            {
                var proxyOperationManager = TestSessionPool.Instance.TryTakeProxy(
                    testSessionInfo,
                    source,
                    string.Empty,
                    _mockRequestData.Object);

                return proxyOperationManager!;
            };

        var testExecutionManager = new ProxyExecutionManager(
            testSessionInfo,
            proxyOperationManagerCreator,
            false);

        var mockTestSessionPool = new Mock<TestSessionPool>();
        TestSessionPool.Instance = mockTestSessionPool.Object;

        try
        {
            var mockProxyOperationManager = new Mock<ProxyOperationManager>(
                _mockRequestData.Object,
                _mockRequestSender.Object,
                _mockTestHostManager.Object,
                null);
            mockTestSessionPool.Setup(
                    tsp => tsp.TryTakeProxy(
                        testSessionInfo,
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        _mockRequestData.Object))
                .Returns(mockProxyOperationManager.Object);

            testExecutionManager.Initialize(true);
            testExecutionManager.StartTestRun(
                _mockTestRunCriteria.Object,
                new Mock<IInternalTestRunEventsHandler>().Object);

            mockTestSessionPool.Verify(
                tsp => tsp.TryTakeProxy(
                    testSessionInfo,
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    _mockRequestData.Object),
                Times.Once);
        }
        finally
        {
            TestSessionPool.Instance = null;
        }
    }

    private static void SignalEvent(ManualResetEvent manualResetEvent)
    {
        // Wait for the 100 ms.
        Task.Delay(200).Wait();

        manualResetEvent.Set();
    }

    private void InvokeAndVerifyStartTestRun(bool skipDefaultAdapters)
    {
        TestPluginCache.Instance = null;
        TestPluginCache.Instance!.DefaultExtensionPaths = new List<string> { "default1.dll", "default2.dll" };
        TestPluginCache.Instance.UpdateExtensions(new List<string> { "filterTestAdapter.dll" }, false);
        TestPluginCache.Instance.UpdateExtensions(new List<string> { "unfilter.dll" }, true);

        try
        {
            _mockFileHelper.Setup(fh => fh.Exists(It.IsAny<string>())).Returns(true);
            _mockTestHostManager.Setup(th => th.GetTestPlatformExtensions(It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>())).Returns((IEnumerable<string> sources, IEnumerable<string> extensions) => extensions);
            _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(true);
            var expectedResult = TestPluginCache.Instance.GetExtensionPaths(TestPlatformConstants.TestAdapterEndsWithPattern, skipDefaultAdapters);

            _testExecutionManager.Initialize(skipDefaultAdapters);
            _testExecutionManager.StartTestRun(_mockTestRunCriteria.Object, null!);

            _mockRequestSender.Verify(s => s.InitializeExecution(expectedResult), Times.Once);
        }
        finally
        {
            TestPluginCache.Instance = null;
        }
    }
}
