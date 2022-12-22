// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.Parallel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace TestPlatform.CrossPlatEngine.UnitTests.Client;

[TestClass]
public class ProxyDiscoveryManagerTests : ProxyBaseManagerTests
{
    private readonly DiscoveryCriteria _discoveryCriteria;
    private readonly DiscoveryDataAggregator _discoveryDataAggregator;
    private readonly Mock<ITestRequestSender> _mockRequestSender;
    private readonly Mock<IRequestData> _mockRequestData;
    private readonly Mock<IMetricsCollection> _mockMetricsCollection;
    private readonly Mock<IFileHelper> _mockFileHelper;
    private readonly ProxyDiscoveryManager _discoveryManager;

    public ProxyDiscoveryManagerTests()
    {
        _mockRequestSender = new Mock<ITestRequestSender>();
        _mockRequestData = new Mock<IRequestData>();
        _mockMetricsCollection = new Mock<IMetricsCollection>();
        _mockFileHelper = new Mock<IFileHelper>();
        _mockRequestData.Setup(rd => rd.MetricsCollection).Returns(_mockMetricsCollection.Object);
        _discoveryDataAggregator = new();
        _discoveryManager = new ProxyDiscoveryManager(
            _mockRequestData.Object,
            _mockRequestSender.Object,
            _mockTestHostManager.Object,
            Framework.DefaultFramework,
            _discoveryDataAggregator,
            _mockDataSerializer.Object,
            _mockFileHelper.Object);
        _discoveryCriteria = new DiscoveryCriteria(new[] { "test.dll" }, 1, string.Empty);
    }

    [TestMethod]
    public void DiscoverTestsShouldNotInitializeExtensionsOnNoExtensions()
    {
        // Make sure TestPlugincache is refreshed.
        TestPluginCache.Instance = null;

        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(true);

        _discoveryManager.DiscoverTests(_discoveryCriteria, null!);

        _mockRequestSender.Verify(s => s.InitializeDiscovery(It.IsAny<IEnumerable<string>>()), Times.Never);
    }

    [TestMethod]
    public void DiscoverTestsShouldNotInitializeExtensionsOnCommunicationFailure()
    {
        // Make sure TestPlugincache is refreshed.
        TestPluginCache.Instance = null;

        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(false);

        Mock<ITestDiscoveryEventsHandler2> mockTestDiscoveryEventHandler = new();

        _discoveryManager.DiscoverTests(_discoveryCriteria, mockTestDiscoveryEventHandler.Object);

        _mockRequestSender.Verify(s => s.InitializeExecution(It.IsAny<IEnumerable<string>>()), Times.Never);
    }

    [TestMethod]
    public void DiscoverTestsShouldAllowRuntimeProviderToUpdateAdapterSource()
    {
        // Make sure TestPlugincache is refreshed.
        TestPluginCache.Instance = null;

        _mockTestHostManager.Setup(hm => hm.GetTestSources(_discoveryCriteria.Sources)).Returns(_discoveryCriteria.Sources);
        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(true);

        Mock<ITestDiscoveryEventsHandler2> mockTestDiscoveryEventHandler = new();

        _discoveryManager.DiscoverTests(_discoveryCriteria, mockTestDiscoveryEventHandler.Object);

        _mockTestHostManager.Verify(hm => hm.GetTestSources(_discoveryCriteria.Sources), Times.Once);
    }

    [TestMethod]
    public void DiscoverTestsShouldUpdateTestSourcesIfSourceDiffersFromTestHostManagerSource()
    {
        var actualSources = new List<string> { "actualSource.dll" };
        var inputSource = new List<string> { "inputPackage.appxrecipe" };

        var localDiscoveryCriteria = new DiscoveryCriteria(inputSource, 1, string.Empty);

        _mockTestHostManager.Setup(hm => hm.GetTestSources(localDiscoveryCriteria.Sources)).Returns(actualSources);
        _mockTestHostManager.Setup(tmh => tmh.LaunchTestHostAsync(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(true));
        _mockTestHostManager.Setup(th => th.GetTestPlatformExtensions(It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>())).Returns(new List<string>());
        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(true);

        Mock<ITestDiscoveryEventsHandler2> mockTestDiscoveryEventHandler = new();

        _discoveryManager.DiscoverTests(localDiscoveryCriteria, mockTestDiscoveryEventHandler.Object);

        Assert.IsNotNull(localDiscoveryCriteria.Package);
        // AdapterSourceMap should contain updated testSources.
        Assert.AreEqual(actualSources.FirstOrDefault(), localDiscoveryCriteria.AdapterSourceMap.FirstOrDefault().Value.FirstOrDefault());
        Assert.AreEqual(inputSource.FirstOrDefault(), localDiscoveryCriteria.Package);
    }

    [TestMethod]
    public void DiscoverTestsShouldNotUpdateTestSourcesIfSourceDoNotDifferFromTestHostManagerSource()
    {
        var actualSources = new List<string> { "actualSource.dll" };
        var inputSource = new List<string> { "actualSource.dll" };

        var localDiscoveryCriteria = new DiscoveryCriteria(inputSource, 1, string.Empty);

        _mockTestHostManager.Setup(hm => hm.GetTestSources(localDiscoveryCriteria.Sources)).Returns(actualSources);
        _mockTestHostManager.Setup(tmh => tmh.LaunchTestHostAsync(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(true));
        _mockTestHostManager.Setup(th => th.GetTestPlatformExtensions(It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>())).Returns(new List<string>());
        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(true);

        Mock<ITestDiscoveryEventsHandler2> mockTestDiscoveryEventHandler = new();

        _discoveryManager.DiscoverTests(localDiscoveryCriteria, mockTestDiscoveryEventHandler.Object);

        Assert.IsNull(localDiscoveryCriteria.Package);
        // AdapterSourceMap should contain updated testSources.
        Assert.AreEqual(actualSources.FirstOrDefault(), localDiscoveryCriteria.AdapterSourceMap.FirstOrDefault().Value.FirstOrDefault());
    }

    [TestMethod]
    public void DiscoverTestsShouldNotSendDiscoveryRequestIfCommunicationFails()
    {
        _mockTestHostManager.Setup(tmh => tmh.LaunchTestHostAsync(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>()))
            .Callback(
                () => _mockTestHostManager.Raise(thm => thm.HostLaunched += null, new HostProviderEventArgs(string.Empty)))
            .Returns(Task.FromResult(false));

        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(true);

        // Make sure TestPlugincache is refreshed.
        TestPluginCache.Instance = null;

        Mock<ITestDiscoveryEventsHandler2> mockTestDiscoveryEventHandler = new();

        _discoveryManager.DiscoverTests(_discoveryCriteria, mockTestDiscoveryEventHandler.Object);

        _mockRequestSender.Verify(s => s.DiscoverTests(It.IsAny<DiscoveryCriteria>(), It.IsAny<ITestDiscoveryEventsHandler2>()), Times.Never);
    }

    [TestMethod]
    public void DiscoverTestsShouldInitializeExtensionsIfPresent()
    {
        // Make sure TestPlugincache is refreshed.
        TestPluginCache.Instance = null;

        try
        {
            var extensions = new[] { "c:\\e1.dll", "c:\\e2.dll" };

            // Setup Mocks.
            TestPluginCacheHelper.SetupMockAdditionalPathExtensions(extensions);
            _mockFileHelper.Setup(fh => fh.Exists(It.IsAny<string>())).Returns(true);
            _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(true);
            _mockTestHostManager.Setup(th => th.GetTestPlatformExtensions(It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>())).Returns(new[] { "c:\\e1.dll", "c:\\e2.dll" });

            _discoveryManager.DiscoverTests(_discoveryCriteria, null!);

            // Also verify that we have waited for client connection.
            _mockRequestSender.Verify(s => s.InitializeDiscovery(extensions), Times.Once);
        }
        finally
        {
            TestPluginCache.Instance = null;
        }
    }

    [TestMethod]
    public void DiscoverTestsShouldInitializeExtensionsWithExistingExtensionsOnly()
    {
        var inputExtensions = new[] { "abc.TestAdapter.dll", "def.TestAdapter.dll", "xyz.TestAdapter.dll" };
        var expectedOutputPaths = new[] { "abc.TestAdapter.dll", "xyz.TestAdapter.dll" };

        TestPluginCacheHelper.SetupMockAdditionalPathExtensions(inputExtensions);
        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(true);
        _mockTestHostManager.Setup(th => th.GetTestPlatformExtensions(It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>())).Returns((IEnumerable<string> sources, IEnumerable<string> extensions) => extensions.Select(extension => Path.GetFileName(extension)));

        _mockFileHelper.Setup(fh => fh.Exists(It.IsAny<string>())).Returns((string extensionPath) => !extensionPath.Contains("def.TestAdapter.dll"));

        _mockFileHelper.Setup(fh => fh.Exists("def.TestAdapter.dll")).Returns(false);
        _mockFileHelper.Setup(fh => fh.Exists("xyz.TestAdapter.dll")).Returns(true);

        var mockTestDiscoveryEventHandler = new Mock<ITestDiscoveryEventsHandler2>();
        _discoveryManager.DiscoverTests(_discoveryCriteria, mockTestDiscoveryEventHandler.Object);

        _mockRequestSender.Verify(s => s.InitializeDiscovery(expectedOutputPaths), Times.Once);
    }

    [TestMethod]
    public void DiscoverTestsShouldQueryTestHostManagerForExtensions()
    {
        TestPluginCache.Instance = null;
        try
        {
            TestPluginCacheHelper.SetupMockAdditionalPathExtensions(new[] { "c:\\e1.dll" });
            _mockFileHelper.Setup(fh => fh.Exists(It.IsAny<string>())).Returns(true);
            _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(true);
            _mockTestHostManager.Setup(th => th.GetTestPlatformExtensions(It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>())).Returns(new[] { "he1.dll", "c:\\e1.dll" });

            _discoveryManager.DiscoverTests(_discoveryCriteria, null!);

            _mockRequestSender.Verify(s => s.InitializeDiscovery(new[] { "he1.dll", "c:\\e1.dll" }), Times.Once);
        }
        finally
        {
            TestPluginCache.Instance = null;
        }
    }

    [TestMethod]
    public void DiscoverTestsShouldPassAdapterToTestHostManagerFromTestPluginCacheExtensions()
    {
        // We are updating extension with test adapter only to make it easy to test.
        // In product code it filter out test adapter from extension
        TestPluginCache.Instance.UpdateExtensions(new List<string> { "abc.TestAdapter.dll", "xyz.TestAdapter.dll" }, false);
        try
        {
            _mockFileHelper.Setup(fh => fh.Exists(It.IsAny<string>())).Returns(true);
            _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(true);
            var expectedResult = TestPluginCache.Instance.GetExtensionPaths(string.Empty);

            _discoveryManager.DiscoverTests(_discoveryCriteria, null!);

            _mockTestHostManager.Verify(th => th.GetTestPlatformExtensions(It.IsAny<IEnumerable<string>>(), expectedResult), Times.Once);
        }
        finally
        {
            TestPluginCache.Instance = null;
        }
    }

    [TestMethod]
    public void DiscoverTestsShouldNotInitializeDefaultAdaptersIfSkipDefaultAdaptersIsTrue()
    {
        InvokeAndVerifyDiscoverTests(true);
    }

    [TestMethod]
    public void DiscoverTestsShouldInitializeDefaultAdaptersIfSkipDefaultAdaptersIsFalse()
    {
        InvokeAndVerifyDiscoverTests(false);
    }

    [TestMethod]
    public void DiscoverTestsShouldNotIntializeTestHost()
    {
        // Setup mocks.
        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(true);

        // Act.
        _discoveryManager.DiscoverTests(_discoveryCriteria, null!);

        _mockRequestSender.Verify(s => s.InitializeCommunication(), Times.Once);
        _mockTestHostManager.Verify(thl => thl.LaunchTestHostAsync(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public void DiscoverTestsShouldCatchExceptionAndCallHandleDiscoveryComplete()
    {
        // Setup mocks.
        Mock<ITestDiscoveryEventsHandler2> mockTestDiscoveryEventsHandler = new();
        _mockTestHostManager.Setup(tmh => tmh.LaunchTestHostAsync(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(false));

        // Act.
        _discoveryManager.DiscoverTests(_discoveryCriteria, mockTestDiscoveryEventsHandler.Object);

        // Verify
        mockTestDiscoveryEventsHandler.Verify(s => s.HandleDiscoveryComplete(It.IsAny<DiscoveryCompleteEventArgs>(), It.IsAny<IEnumerable<TestCase>>()));
        mockTestDiscoveryEventsHandler.Verify(s => s.HandleRawMessage(It.IsAny<string>()));
        mockTestDiscoveryEventsHandler.Verify(s => s.HandleLogMessage(TestMessageLevel.Error, It.IsAny<string>()));
    }

    [TestMethod]
    public void DiscoverTestsShouldCatchExceptionAndCallHandleRawMessageOfDiscoveryComplete()
    {
        // Setup mocks.
        Mock<ITestDiscoveryEventsHandler2> mockTestDiscoveryEventsHandler = new();
        _mockTestHostManager.Setup(tmh => tmh.LaunchTestHostAsync(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(false));

        _mockDataSerializer.Setup(ds => ds.SerializePayload(MessageType.TestMessage, It.IsAny<TestMessagePayload>())).Returns(MessageType.TestMessage);
        _mockDataSerializer.Setup(ds => ds.SerializePayload(MessageType.DiscoveryComplete, It.IsAny<DiscoveryCompletePayload>())).Returns(MessageType.DiscoveryComplete);

        _mockDataSerializer.Setup(mds => mds.DeserializeMessage(It.IsAny<string>())).Returns((string rawMessage) =>
        {
            var messageType = rawMessage.Contains(MessageType.DiscoveryComplete) ? MessageType.DiscoveryComplete : MessageType.TestMessage;
            var message = new Message
            {
                MessageType = messageType
            };

            return message;
        });

        // Act.
        _discoveryManager.DiscoverTests(_discoveryCriteria, mockTestDiscoveryEventsHandler.Object);

        // Verify
        mockTestDiscoveryEventsHandler.Verify(s => s.HandleRawMessage(It.Is<string>(str => str.Contains(MessageType.DiscoveryComplete))), Times.Once);
    }

    [TestMethod]
    public void DiscoverTestsShouldCatchExceptionAndCallHandleRawMessageOfTestMessage()
    {
        // Setup mocks.
        Mock<ITestDiscoveryEventsHandler2> mockTestDiscoveryEventsHandler = new();
        _mockTestHostManager.Setup(tmh => tmh.LaunchTestHostAsync(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(false));

        _mockDataSerializer.Setup(ds => ds.SerializePayload(MessageType.TestMessage, It.IsAny<TestMessagePayload>())).Returns(MessageType.TestMessage);
        _mockDataSerializer.Setup(ds => ds.SerializePayload(MessageType.DiscoveryComplete, It.IsAny<DiscoveryCompletePayload>())).Returns(MessageType.DiscoveryComplete);

        _mockDataSerializer.Setup(mds => mds.DeserializeMessage(It.IsAny<string>())).Returns((string rawMessage) =>
        {
            var messageType = rawMessage.Contains(MessageType.DiscoveryComplete) ? MessageType.DiscoveryComplete : MessageType.TestMessage;
            var message = new Message
            {
                MessageType = messageType
            };

            return message;
        });

        // Act.
        _discoveryManager.DiscoverTests(_discoveryCriteria, mockTestDiscoveryEventsHandler.Object);

        // Verify
        mockTestDiscoveryEventsHandler.Verify(s => s.HandleRawMessage(It.Is<string>(str => str.Contains(MessageType.TestMessage))), Times.Once);
    }

    [TestMethod]
    public void DiscoverTestsShouldCatchExceptionAndCallHandleLogMessageOfError()
    {
        // Setup mocks.
        Mock<ITestDiscoveryEventsHandler2> mockTestDiscoveryEventsHandler = new();
        _mockTestHostManager.Setup(tmh => tmh.LaunchTestHostAsync(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(false));

        // Act.
        _discoveryManager.DiscoverTests(_discoveryCriteria, mockTestDiscoveryEventsHandler.Object);

        // Verify
        mockTestDiscoveryEventsHandler.Verify(s => s.HandleLogMessage(TestMessageLevel.Error, It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public void DiscoverTestsShouldInitiateServerDiscoveryLoop()
    {
        // Setup mocks.
        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(true);
        _mockFileHelper.Setup(fh => fh.Exists(It.IsAny<string>())).Returns(true);

        // Act.
        _discoveryManager.DiscoverTests(_discoveryCriteria, null!);

        // Assert.
        _mockRequestSender.Verify(s => s.DiscoverTests(It.IsAny<DiscoveryCriteria>(), _discoveryManager), Times.Once);
    }

    [TestMethod]
    public void DiscoverTestsCloseTestHostIfRawMessageIsOfTypeDiscoveryComplete()
    {
        Mock<ITestDiscoveryEventsHandler2> mockTestDiscoveryEventsHandler = new();

        _mockDataSerializer.Setup(ds => ds.SerializePayload(MessageType.TestMessage, It.IsAny<TestMessagePayload>())).Returns(MessageType.TestMessage);
        _mockDataSerializer.Setup(ds => ds.SerializePayload(MessageType.DiscoveryComplete, It.IsAny<DiscoveryCompletePayload>())).Returns(MessageType.DiscoveryComplete);

        _mockDataSerializer.Setup(mds => mds.DeserializeMessage(It.IsAny<string>())).Returns((string rawMessage) =>
        {
            var messageType = rawMessage.Contains(MessageType.DiscoveryComplete) ? MessageType.DiscoveryComplete : MessageType.TestMessage;
            var message = new Message
            {
                MessageType = messageType
            };

            return message;
        });

        // Act.
        _discoveryManager.DiscoverTests(_discoveryCriteria, mockTestDiscoveryEventsHandler.Object);

        // Verify
        _mockTestHostManager.Verify(mthm => mthm.CleanTestHostAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public void DiscoverTestsShouldNotCloseTestHostIfRawMessageIsNotOfTypeDiscoveryComplete()
    {
        Mock<ITestDiscoveryEventsHandler2> mockTestDiscoveryEventsHandler = new();

        _mockDataSerializer.Setup(mds => mds.DeserializeMessage(It.IsAny<string>())).Returns(() =>
        {
            var message = new Message
            {
                MessageType = MessageType.DiscoveryInitialize
            };

            return message;
        });

        // Act.
        _discoveryManager.DiscoverTests(_discoveryCriteria, mockTestDiscoveryEventsHandler.Object);

        // Verify
        _mockTestHostManager.Verify(mthm => mthm.CleanTestHostAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [TestMethod]
    public void DiscoveryTestsMarksAllSourcesAsNotDiscovered()
    {
        // Arrange
        Mock<ITestDiscoveryEventsHandler2> mockTestDiscoveryEventsHandler = new();
        var inputSource = new List<string> { "source1.dll", "source2.dll", "source3.dll" };

        var localDiscoveryCriteria = new DiscoveryCriteria(inputSource, 1, string.Empty);

        // Act
        _discoveryManager.DiscoverTests(localDiscoveryCriteria, mockTestDiscoveryEventsHandler.Object);

        // Assert
        CollectionAssert.AreEquivalent(inputSource, _discoveryDataAggregator.GetSourcesWithStatus(DiscoveryStatus.NotDiscovered));
        Assert.AreEqual(0, _discoveryDataAggregator.GetSourcesWithStatus(DiscoveryStatus.PartiallyDiscovered).Count);
        Assert.AreEqual(0, _discoveryDataAggregator.GetSourcesWithStatus(DiscoveryStatus.FullyDiscovered).Count);
    }

    [TestMethod]
    public void DiscoveryManagerShouldPassOnHandleDiscoveredTests()
    {
        Mock<ITestDiscoveryEventsHandler2> mockTestDiscoveryEventsHandler = new();
        var testCases = new List<TestCase>() { new TestCase("eventHandler.y.z", new Uri("eventHandler://y"), "eventHandler.dll") };

        var discoveryManager = GetProxyDiscoveryManager();
        SetupChannelMessage(MessageType.StartDiscovery, MessageType.TestCasesFound, testCases);
        SetupChannelMessage(MessageType.TestMessage, MessageType.TestMessage, string.Empty);

        var completePayload = new DiscoveryCompletePayload()
        {
            IsAborted = false,
            LastDiscoveredTests = null,
            TotalTests = 1
        };
        var completeMessage = new Message() { MessageType = MessageType.DiscoveryComplete, Payload = null };
        mockTestDiscoveryEventsHandler.Setup(mh => mh.HandleDiscoveredTests(It.IsAny<IEnumerable<TestCase>>())).Callback(
            () =>
            {
                _mockDataSerializer.Setup(ds => ds.DeserializeMessage(It.IsAny<string>())).Returns(completeMessage);
                _mockDataSerializer.Setup(ds => ds.DeserializePayload<DiscoveryCompletePayload>(completeMessage)).Returns(completePayload);
            });

        // Act.
        discoveryManager.DiscoverTests(_discoveryCriteria, mockTestDiscoveryEventsHandler.Object);

        // Verify
        mockTestDiscoveryEventsHandler.Verify(mtdeh => mtdeh.HandleDiscoveredTests(It.IsAny<IEnumerable<TestCase>>()), Times.AtLeastOnce);
    }

    [TestMethod]
    public void DiscoveryManagerShouldPassOnHandleLogMessage()
    {
        Mock<ITestDiscoveryEventsHandler2> mockTestDiscoveryEventsHandler = new();

        _mockDataSerializer.Setup(mds => mds.DeserializeMessage(It.IsAny<string>())).Returns(() =>
        {
            var message = new Message
            {
                MessageType = MessageType.TestMessage
            };

            return message;
        });

        // Act.
        _discoveryManager.DiscoverTests(_discoveryCriteria, mockTestDiscoveryEventsHandler.Object);

        // Verify
        mockTestDiscoveryEventsHandler.Verify(mtdeh => mtdeh.HandleLogMessage(It.IsAny<TestMessageLevel>(), It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public void AbortShouldSendTestDiscoveryCancelIfCommunicationSuccessful()
    {
        var mockTestDiscoveryEventsHandler = new Mock<ITestDiscoveryEventsHandler2>();
        _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(true);

        Mock<ITestRunEventsHandler> mockTestRunEventsHandler = new();

        _discoveryManager.DiscoverTests(_discoveryCriteria, mockTestDiscoveryEventsHandler.Object);

        _discoveryManager.Abort();

        _mockRequestSender.Verify(s => s.EndSession(), Times.Once);
    }

    [TestMethod]
    public void StartTestRunShouldAttemptToTakeProxyFromPoolIfProxyIsNull()
    {
        var testSessionInfo = new TestSessionInfo();

        Func<string, ProxyDiscoveryManager, ProxyOperationManager>
            proxyOperationManagerCreator = (
                string source,
                ProxyDiscoveryManager proxyDiscoveryManager) =>
            {
                var proxyOperationManager = TestSessionPool.Instance.TryTakeProxy(
                    testSessionInfo,
                    source,
                    _discoveryCriteria.RunSettings,
                    _mockRequestData.Object);

                return proxyOperationManager!;
            };

        var testDiscoveryManager = new ProxyDiscoveryManager(
            testSessionInfo,
            proxyOperationManagerCreator);

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

            testDiscoveryManager.Initialize(true);
            testDiscoveryManager.DiscoverTests(
                _discoveryCriteria,
                new Mock<ITestDiscoveryEventsHandler2>().Object);

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

    [TestMethod]
    public void HandleDiscoveredTestsMarksDiscoveryStatus()
    {
        // Arrange
        var localDiscoveryCriteria = new DiscoveryCriteria(new[] { "a" }, 1, string.Empty);
        _discoveryManager.DiscoverTests(localDiscoveryCriteria, new Mock<ITestDiscoveryEventsHandler2>().Object);

        // Marks 'a' as partially discovered
        _discoveryManager.HandleDiscoveredTests(new TestCase[]
        {
            new() { Source = "a" },
        });

        // Act
        _discoveryManager.HandleDiscoveredTests(new TestCase[]
        {
            new() { Source = "b" },
            new() { Source = "c" },
            new() { Source = "c" },
            new() { Source = "d" },
        });

        // Assert
        Assert.AreEqual(0, _discoveryDataAggregator.GetSourcesWithStatus(DiscoveryStatus.NotDiscovered).Count);
        CollectionAssert.AreEquivalent(
            new List<string> { "d" },
            _discoveryDataAggregator.GetSourcesWithStatus(DiscoveryStatus.PartiallyDiscovered));
        CollectionAssert.AreEquivalent(
            new List<string> { "a", "b", "c" },
            _discoveryDataAggregator.GetSourcesWithStatus(DiscoveryStatus.FullyDiscovered));
    }

    [DataTestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void HandleDiscoveryCompleteWhenAbortedNoPastDiscoveryAndNoLastCunkNotifiesWithCorrectDiscovery(bool trueIsEmptyFalseIsNull)
    {
        // Arrange
        var localDiscoveryCriteria = new DiscoveryCriteria(new[] { "a", "b" }, 1, string.Empty);
        var eventHandler = new Mock<ITestDiscoveryEventsHandler2>();
        _discoveryManager.DiscoverTests(localDiscoveryCriteria, eventHandler.Object);

        var lastChunk = trueIsEmptyFalseIsNull
            ? Enumerable.Empty<TestCase>()
            : null;

        // Act
        _discoveryManager.HandleDiscoveryComplete(new(-1, true), lastChunk);

        // Assert
        CollectionAssert.AreEquivalent(
            new[] { "a", "b" },
            _discoveryDataAggregator.GetSourcesWithStatus(DiscoveryStatus.NotDiscovered));
        Assert.AreEqual(0, _discoveryDataAggregator.GetSourcesWithStatus(DiscoveryStatus.PartiallyDiscovered).Count);
        Assert.AreEqual(0, _discoveryDataAggregator.GetSourcesWithStatus(DiscoveryStatus.FullyDiscovered).Count);
    }

    [DataTestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void HandleDiscoveryCompleteWhenAbortedPastDiscoveryAndNoLastCunkNotifiesWithCorrectDiscovery(bool trueIsEmptyFalseIsNull)
    {
        // Arrange
        var localDiscoveryCriteria = new DiscoveryCriteria(new[] { "a" }, 1, string.Empty);
        _discoveryManager.DiscoverTests(localDiscoveryCriteria, new Mock<ITestDiscoveryEventsHandler2>().Object);

        _discoveryManager.HandleDiscoveredTests(new TestCase[]
        {
            new() { Source = "a" },
            new() { Source = "b" },
        });

        var lastChunk = trueIsEmptyFalseIsNull
            ? Enumerable.Empty<TestCase>()
            : null;

        // Act
        _discoveryManager.HandleDiscoveryComplete(new(-1, true), lastChunk);

        // Assert
        CollectionAssert.AreEquivalent(
            new[] { "a" },
            _discoveryDataAggregator.GetSourcesWithStatus(DiscoveryStatus.FullyDiscovered));
        CollectionAssert.AreEquivalent(
            new[] { "b" },
            _discoveryDataAggregator.GetSourcesWithStatus(DiscoveryStatus.PartiallyDiscovered));
        Assert.AreEqual(0, _discoveryDataAggregator.GetSourcesWithStatus(DiscoveryStatus.NotDiscovered).Count);
    }

    [TestMethod]
    public void HandleDiscoveryCompleteWhenAbortedNoPastDiscoveryAndLastCunkNotifiesWithCorrectDiscovery()
    {
        // Arrange
        var localDiscoveryCriteria = new DiscoveryCriteria(new[] { "a", "b" }, 1, string.Empty);
        _discoveryManager.DiscoverTests(localDiscoveryCriteria, new Mock<ITestDiscoveryEventsHandler2>().Object);

        var lastChunk = new TestCase[]
        {
            new() { Source = "c" },
            new() { Source = "d" },
        };

        // Act
        _discoveryManager.HandleDiscoveryComplete(new(-1, true), lastChunk);

        // Assert
        CollectionAssert.AreEquivalent(
            new[] { "c" },
            _discoveryDataAggregator.GetSourcesWithStatus(DiscoveryStatus.FullyDiscovered));
        CollectionAssert.AreEquivalent(
            new[] { "d" },
            _discoveryDataAggregator.GetSourcesWithStatus(DiscoveryStatus.PartiallyDiscovered));
        CollectionAssert.AreEquivalent(
            new[] { "a", "b" },
            _discoveryDataAggregator.GetSourcesWithStatus(DiscoveryStatus.NotDiscovered));
    }

    [TestMethod]
    public void HandleDiscoveryCompleteWhenAbortedPastDiscoveryAndLastCunkNotifiesWithCorrectDiscovery()
    {
        // Arrange
        var localDiscoveryCriteria = new DiscoveryCriteria(new[] { "a" }, 1, string.Empty);
        _discoveryManager.DiscoverTests(localDiscoveryCriteria, new Mock<ITestDiscoveryEventsHandler2>().Object);

        _discoveryManager.HandleDiscoveredTests(new TestCase[]
        {
            new() { Source = "a" },
            new() { Source = "b" },
        });

        var lastChunk = new TestCase[]
        {
            new() { Source = "c" },
            new() { Source = "d" },
        };

        // Act
        _discoveryManager.HandleDiscoveryComplete(new(-1, true), lastChunk);

        // Assert
        CollectionAssert.AreEquivalent(
            new[] { "a", "b", "c" },
            _discoveryDataAggregator.GetSourcesWithStatus(DiscoveryStatus.FullyDiscovered));
        CollectionAssert.AreEquivalent(
            new[] { "d" },
            _discoveryDataAggregator.GetSourcesWithStatus(DiscoveryStatus.PartiallyDiscovered));
        Assert.AreEqual(0, _discoveryDataAggregator.GetSourcesWithStatus(DiscoveryStatus.NotDiscovered).Count);
    }

    private void InvokeAndVerifyDiscoverTests(bool skipDefaultAdapters)
    {
        TestPluginCache.Instance = null;
        // It's ok to use Instance. because on nulls, instance is re-instantiated.
        TestPluginCache.Instance!.DefaultExtensionPaths = new List<string> { "default1.dll", "default2.dll" };
        TestPluginCache.Instance.UpdateExtensions(new List<string> { "filterTestAdapter.dll" }, false);
        TestPluginCache.Instance.UpdateExtensions(new List<string> { "unfilter.dll" }, true);

        try
        {
            _mockFileHelper.Setup(fh => fh.Exists(It.IsAny<string>())).Returns(true);
            _mockTestHostManager.Setup(th => th.GetTestPlatformExtensions(It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>())).Returns((IEnumerable<string> sources, IEnumerable<string> extensions) => extensions);
            _mockRequestSender.Setup(s => s.WaitForRequestHandlerConnection(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(true);
            var expectedResult = TestPluginCache.Instance.GetExtensionPaths(TestPlatformConstants.TestAdapterEndsWithPattern, skipDefaultAdapters);

            _discoveryManager.Initialize(skipDefaultAdapters);
            _discoveryManager.DiscoverTests(_discoveryCriteria, null!);

            _mockRequestSender.Verify(s => s.InitializeDiscovery(expectedResult), Times.Once);
        }
        finally
        {
            TestPluginCache.Instance = null;
        }
    }

    //private void SetupAndInitializeTestRequestSender()
    //{
    //    var connectionInfo = new TestHostConnectionInfo
    //    {
    //        Endpoint = IPAddress.Loopback + ":0",
    //        Role = ConnectionRole.Client,
    //        Transport = Transport.Sockets
    //    };

    //    this.mockCommunicationEndpoint = new Mock<ICommunicationEndPoint>();
    //    this.mockDataSerializer = new Mock<IDataSerializer>();
    //    this.testRequestSender = new TestRequestSender(this.mockCommunicationEndpoint.Object, connectionInfo, this.mockDataSerializer.Object, this.protocolConfig, CLIENTPROCESSEXITWAIT);
    //    this.mockCommunicationEndpoint.Setup(mc => mc.Start(connectionInfo.Endpoint)).Returns(connectionInfo.Endpoint).Callback(() =>
    //        {
    //            this.mockCommunicationEndpoint.Raise(
    //                s => s.Connected += null,
    //                this.mockCommunicationEndpoint.Object,
    //                new ConnectedEventArgs(this.mockChannel.Object));
    //        });
    //    this.SetupChannelMessage(MessageType.VersionCheck, MessageType.VersionCheck, this.protocolConfig.Version);
    //    this.testRequestSender.InitializeCommunication();

    //    this.testDiscoveryManager = new ProxyDiscoveryManager(
    //                                    this.mockRequestData.Object,
    //                                    this.testRequestSender,
    //                                    this.mockTestHostManager.Object,
    //                                    this.mockDataSerializer.Object,
    //                                    this.testableClientConnectionTimeout);
    //}

    //private void SetupChannelMessage<TPayload>(string messageType, string returnMessageType, TPayload returnPayload)
    //{
    //    this.mockChannel.Setup(mc => mc.Send(It.Is<string>(s => s.Contains(messageType))))
    //                    .Callback(() => this.mockChannel.Raise(c => c.MessageReceived += null,  this.mockChannel.Object, new MessageReceivedEventArgs { Data = messageType }));

    //    this.mockDataSerializer.Setup(ds => ds.SerializePayload(It.Is<string>(s => s.Equals(messageType)), It.IsAny<object>())).Returns(messageType);
    //    this.mockDataSerializer.Setup(ds => ds.SerializePayload(It.Is<string>(s => s.Equals(messageType)), It.IsAny<object>(), It.IsAny<int>())).Returns(messageType);
    //    this.mockDataSerializer.Setup(ds => ds.DeserializeMessage(It.Is<string>(s => s.Equals(messageType))))
    //        .Returns(new Message { MessageType = returnMessageType });
    //        this.mockDataSerializer.Setup(ds => ds.DeserializePayload<TPayload>(It.Is<Message>(m => m.MessageType.Equals(messageType))))
    //        .Returns(returnPayload);
    //}
}
