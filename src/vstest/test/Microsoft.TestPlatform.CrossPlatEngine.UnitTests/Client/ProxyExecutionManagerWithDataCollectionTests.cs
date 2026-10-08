// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.DataCollection.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace TestPlatform.CrossPlatEngine.UnitTests.Client;

[TestClass]
public class ProxyExecutionManagerWithDataCollectionTests
{
    private readonly ProxyExecutionManager _testExecutionManager;
    private readonly Mock<ITestRuntimeProvider> _mockTestHostManager;
    private readonly Mock<ITestRequestSender> _mockRequestSender;
    private readonly Mock<IProxyDataCollectionManager> _mockDataCollectionManager;
    private readonly Mock<IProcessHelper> _mockProcessHelper;
    private readonly ProxyExecutionManagerWithDataCollection _proxyExecutionManager;
    private readonly Mock<IDataSerializer> _mockDataSerializer;
    private readonly Mock<IRequestData> _mockRequestData;
    private readonly Mock<IMetricsCollection> _mockMetricsCollection;
    private readonly Mock<IFileHelper> _mockFileHelper;

    public ProxyExecutionManagerWithDataCollectionTests()
    {
        _mockTestHostManager = new Mock<ITestRuntimeProvider>();
        _mockRequestSender = new Mock<ITestRequestSender>();
        _mockRequestSender.As<IConnectionValidationConfigurable>();
        _mockDataSerializer = new Mock<IDataSerializer>();
        _mockRequestData = new Mock<IRequestData>();
        _mockMetricsCollection = new Mock<IMetricsCollection>();
        _mockFileHelper = new Mock<IFileHelper>();
        _mockRequestData.Setup(rd => rd.MetricsCollection).Returns(_mockMetricsCollection.Object);
        _testExecutionManager = new ProxyExecutionManager(_mockRequestData.Object, _mockRequestSender.Object, _mockTestHostManager.Object, Framework.DefaultFramework, _mockDataSerializer.Object, _mockFileHelper.Object);
        _mockDataCollectionManager = new Mock<IProxyDataCollectionManager>();
        _mockProcessHelper = new Mock<IProcessHelper>();
        _proxyExecutionManager = new ProxyExecutionManagerWithDataCollection(_mockRequestData.Object, _mockRequestSender.Object, _mockTestHostManager.Object, Framework.DefaultFramework, _mockDataCollectionManager.Object);
    }

    [TestMethod]
    public void InitializeShouldInitializeDataCollectionProcessIfDataCollectionIsEnabled()
    {
        _proxyExecutionManager.Initialize(false);

        _mockDataCollectionManager.Verify(dc => dc.BeforeTestRunStart(It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<ITestMessageEventHandler>()), Times.Once);
    }

    [TestMethod]
    public void InitializeShouldThrowExceptionIfThrownByDataCollectionManager()
    {
        _mockDataCollectionManager.Setup(x => x.Initialize()).Throws<Exception>();

        Assert.ThrowsException<Exception>(() => _proxyExecutionManager.Initialize(false));
    }

    [TestMethod]
    public void InitializeShouldCallAfterTestRunIfExceptionIsThrownWhileCreatingDataCollectionProcess()
    {
        _mockDataCollectionManager.Setup(dc => dc.BeforeTestRunStart(It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<ITestMessageEventHandler>())).Throws(new Exception("MyException"));

        Assert.ThrowsException<Exception>(() => _proxyExecutionManager.Initialize(false));

        _mockDataCollectionManager.Verify(dc => dc.BeforeTestRunStart(It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<ITestMessageEventHandler>()), Times.Once);
        _mockDataCollectionManager.Verify(dc => dc.AfterTestRunEnd(It.IsAny<bool>(), It.IsAny<ITestMessageEventHandler>()), Times.Once);
    }

    [TestMethod]
    public void InitializeShouldSaveExceptionMessagesIfThrownByDataCollectionProcess()
    {
        var mockRequestSender = new Mock<IDataCollectionRequestSender>();
        var testSources = new List<string>() { "abc.dll", "efg.dll" };
        mockRequestSender.Setup(x => x.SendBeforeTestRunStartAndGetResult(string.Empty, testSources, It.IsAny<bool>(), It.IsAny<ITestMessageEventHandler>())).Throws(new Exception("MyException"));
        mockRequestSender.Setup(x => x.WaitForRequestHandlerConnection(It.IsAny<int>())).Returns(true);

        var mockDataCollectionLauncher = new Mock<IDataCollectionLauncher>();
        var proxyDataCollectonManager = new ProxyDataCollectionManager(_mockRequestData.Object, string.Empty, testSources, mockRequestSender.Object, _mockProcessHelper.Object, mockDataCollectionLauncher.Object);

        var proxyExecutionManager = new ProxyExecutionManagerWithDataCollection(_mockRequestData.Object, _mockRequestSender.Object, _mockTestHostManager.Object, Framework.DefaultFramework, proxyDataCollectonManager);
        proxyExecutionManager.Initialize(false);
        Assert.IsNotNull(proxyExecutionManager.DataCollectionRunEventsHandler.Messages);
        Assert.AreEqual(TestMessageLevel.Error, proxyExecutionManager.DataCollectionRunEventsHandler.Messages[0].Item1);
        StringAssert.Contains(proxyExecutionManager.DataCollectionRunEventsHandler.Messages[0].Item2, "MyException");
    }

    [TestMethod]
    public void UpdateTestProcessStartInfoShouldUpdateDataCollectionPortArg()
    {
        _mockDataCollectionManager.Setup(x => x.BeforeTestRunStart(It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<ITestMessageEventHandler>())).Returns(DataCollectionParameters.CreateDefaultParameterInstance());

        var testProcessStartInfo = new TestProcessStartInfo();
        testProcessStartInfo.Arguments = string.Empty;

        var proxyExecutionManager = new TestableProxyExecutionManagerWithDataCollection(_mockRequestSender.Object, _mockTestHostManager.Object, _mockDataCollectionManager.Object);
        proxyExecutionManager.UpdateTestProcessStartInfoWrapper(testProcessStartInfo);

        Assert.IsTrue(testProcessStartInfo.Arguments.Contains("--datacollectionport 0"));
    }

    [TestMethod]
    public void UpdateTestProcessStartInfoShouldUpdateTelemetryOptedInArgTrueIfTelemetryOptedIn()
    {
        var mockRequestData = new Mock<IRequestData>();
        _mockRequestData.Setup(rd => rd.IsTelemetryOptedIn).Returns(true);

        _mockDataCollectionManager.Setup(x => x.BeforeTestRunStart(It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<ITestMessageEventHandler>())).Returns(DataCollectionParameters.CreateDefaultParameterInstance());

        var testProcessStartInfo = new TestProcessStartInfo();
        testProcessStartInfo.Arguments = string.Empty;

        var proxyExecutionManager = new TestableProxyExecutionManagerWithDataCollection(_mockRequestData.Object, _mockRequestSender.Object, _mockTestHostManager.Object, _mockDataCollectionManager.Object);

        // Act.
        proxyExecutionManager.UpdateTestProcessStartInfoWrapper(testProcessStartInfo);

        // Verify.
        Assert.IsTrue(testProcessStartInfo.Arguments.Contains("--telemetryoptedin true"));
    }

    [TestMethod]
    public void UpdateTestProcessStartInfoShouldUpdateTelemetryOptedInArgFalseIfTelemetryOptedOut()
    {
        var mockRequestData = new Mock<IRequestData>();
        _mockRequestData.Setup(rd => rd.IsTelemetryOptedIn).Returns(false);

        _mockDataCollectionManager.Setup(x => x.BeforeTestRunStart(It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<ITestMessageEventHandler>())).Returns(DataCollectionParameters.CreateDefaultParameterInstance());

        var testProcessStartInfo = new TestProcessStartInfo();
        testProcessStartInfo.Arguments = string.Empty;

        var proxyExecutionManager = new TestableProxyExecutionManagerWithDataCollection(_mockRequestData.Object, _mockRequestSender.Object, _mockTestHostManager.Object, _mockDataCollectionManager.Object);

        // Act.
        proxyExecutionManager.UpdateTestProcessStartInfoWrapper(testProcessStartInfo);

        // Verify.
        Assert.IsTrue(testProcessStartInfo.Arguments.Contains("--telemetryoptedin false"));
    }

    // These two cover the connection secrets that let the testhost open the test-case-events channel
    // to the datacollector. They deliberately use the concrete ProxyDataCollectionManager rather than
    // the mocked interface: the secret is only reachable through the concrete type, so a test built
    // on the mock would keep passing while nothing read it, which is exactly how this went unnoticed.
    [TestMethod]
    public void UpdateTestProcessStartInfoShouldPassDataCollectionConnectionSecretsToTestHost()
    {
        var proxyDataCollectionManager = CreateInitializedDataCollectionManager();
        var testProcessStartInfo = new TestProcessStartInfo { Arguments = string.Empty };

        var proxyExecutionManager = new TestableProxyExecutionManagerWithDataCollection(_mockRequestSender.Object, _mockTestHostManager.Object, proxyDataCollectionManager);

        // Act.
        proxyExecutionManager.UpdateTestProcessStartInfoWrapper(testProcessStartInfo);

        // Verify.
        Assert.IsNotNull(proxyDataCollectionManager.TestHostConnectionSecrets);
        Assert.IsNotNull(testProcessStartInfo.EnvironmentVariables);
        Assert.AreEqual(
            proxyDataCollectionManager.TestHostConnectionSecrets,
            testProcessStartInfo.EnvironmentVariables[EnvironmentHelper.VstestDataCollectionConnectionSecrets]);
    }

    [TestMethod]
    public void UpdateTestProcessStartInfoShouldNotPassDataCollectionConnectionSecretsWhenThereAreNone()
    {
        _mockDataCollectionManager.Setup(x => x.BeforeTestRunStart(It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<ITestMessageEventHandler>())).Returns(DataCollectionParameters.CreateDefaultParameterInstance());

        var testProcessStartInfo = new TestProcessStartInfo { Arguments = string.Empty };

        var proxyExecutionManager = new TestableProxyExecutionManagerWithDataCollection(_mockRequestSender.Object, _mockTestHostManager.Object, _mockDataCollectionManager.Object);

        // Act.
        proxyExecutionManager.UpdateTestProcessStartInfoWrapper(testProcessStartInfo);

        // Verify.
        var carriesSecrets = testProcessStartInfo.EnvironmentVariables is not null
            && testProcessStartInfo.EnvironmentVariables.ContainsKey(EnvironmentHelper.VstestDataCollectionConnectionSecrets);
        Assert.IsFalse(carriesSecrets, "An empty secret must not be written to the testhost environment.");
    }

    private ProxyDataCollectionManager CreateInitializedDataCollectionManager()
    {
        var mockRequestSender = new Mock<IDataCollectionRequestSender>();
        mockRequestSender.Setup(x => x.WaitForRequestHandlerConnection(It.IsAny<int>())).Returns(true);

        var proxyDataCollectionManager = new ProxyDataCollectionManager(
            _mockRequestData.Object,
            string.Empty,
            new List<string> { "abc.dll" },
            mockRequestSender.Object,
            _mockProcessHelper.Object,
            new Mock<IDataCollectionLauncher>().Object);

        // Initialize is what generates the pair, and it runs before UpdateTestProcessStartInfo in
        // the real flow: ProxyExecutionManagerWithDataCollection.Initialize calls it before
        // base.Initialize, and UpdateTestProcessStartInfo runs later, from SetupChannel.
        proxyDataCollectionManager.Initialize();

        return proxyDataCollectionManager;
    }

    [TestMethod]
    public void LaunchProcessWithDebuggerAttachedShouldUpdateEnvironmentVariables()
    {
        // Setup
        var mockRunEventsHandler = new Mock<IInternalTestRunEventsHandler>();
        TestProcessStartInfo? launchedStartInfo = null;
        mockRunEventsHandler.Setup(runHandler => runHandler.LaunchProcessWithDebuggerAttached(It.IsAny<TestProcessStartInfo>())).Callback
            ((TestProcessStartInfo startInfo) => launchedStartInfo = startInfo);
        var proxyExecutionManager = new ProxyExecutionManagerWithDataCollection(_mockRequestData.Object, _mockRequestSender.Object, _mockTestHostManager.Object, Framework.DefaultFramework, _mockDataCollectionManager.Object);
        var mockTestRunCriteria = new Mock<TestRunCriteria>(new List<string> { "source.dll" }, 10);
        var testProcessStartInfo = new TestProcessStartInfo
        {
            Arguments = string.Empty,
            EnvironmentVariables = new Dictionary<string, string?>
            {
                {"variable1", "value1" },
                {"variable2", "value2" }
            }
        };

        string raw1 = JsonDataSerializer.Instance.SerializePayload(MessageType.TelemetryEventMessage, new TelemetryEvent("aaa", new Dictionary<string, object>()));
        string raw2 = JsonDataSerializer.Instance.SerializePayload(MessageType.TelemetryEventMessage, new TelemetryEvent("aaa", new Dictionary<string, object>()));

        proxyExecutionManager.DataCollectionRunEventsHandler.HandleRawMessage(raw1);
        proxyExecutionManager.DataCollectionRunEventsHandler.HandleRawMessage(raw2);

        // Act.
        proxyExecutionManager.StartTestRun(mockTestRunCriteria.Object, mockRunEventsHandler.Object);
        proxyExecutionManager.LaunchProcessWithDebuggerAttached(testProcessStartInfo);

        // Verify.
        Assert.IsTrue(launchedStartInfo != null, "Failed to get the start info");
        foreach (var envVaribale in testProcessStartInfo.EnvironmentVariables)
        {
            Assert.AreEqual(envVaribale.Value, launchedStartInfo.EnvironmentVariables![envVaribale.Key], $"Expected environment variable {envVaribale.Key} : {envVaribale.Value} not found");
        }

        mockRunEventsHandler.Verify(r => r.HandleRawMessage(raw1));
        mockRunEventsHandler.Verify(r => r.HandleRawMessage(raw2));
        Assert.AreEqual(0, proxyExecutionManager.DataCollectionRunEventsHandler.RawMessages.Count);
    }

    [TestMethod]
    public void TestHostManagerHostLaunchedTriggerShouldSendTestHostLaunchedEvent()
    {
        var proxyExecutionManager = new ProxyExecutionManagerWithDataCollection(_mockRequestData.Object, _mockRequestSender.Object, _mockTestHostManager.Object, Framework.DefaultFramework, _mockDataCollectionManager.Object);

        _mockTestHostManager.Raise(x => x.HostLaunched += null, new HostProviderEventArgs("launched", 0, 1234));

        _mockDataCollectionManager.Verify(x => x.TestHostLaunched(It.IsAny<int>()));
    }
}

internal class TestableProxyExecutionManagerWithDataCollection : ProxyExecutionManagerWithDataCollection
{
    public TestableProxyExecutionManagerWithDataCollection(ITestRequestSender testRequestSender, ITestRuntimeProvider testHostManager, IProxyDataCollectionManager proxyDataCollectionManager) : base(new RequestData { MetricsCollection = new NoOpMetricsCollection() }, testRequestSender, testHostManager, Framework.DefaultFramework, proxyDataCollectionManager)
    {
    }

    public TestableProxyExecutionManagerWithDataCollection(IRequestData requestData, ITestRequestSender testRequestSender, ITestRuntimeProvider testHostManager, IProxyDataCollectionManager proxyDataCollectionManager) : base(requestData, testRequestSender, testHostManager, Framework.DefaultFramework, proxyDataCollectionManager)
    {
    }

    public TestProcessStartInfo UpdateTestProcessStartInfoWrapper(TestProcessStartInfo testProcessStartInfo)
    {
        return UpdateTestProcessStartInfo(testProcessStartInfo);
    }

    public override TestProcessStartInfo UpdateTestProcessStartInfo(TestProcessStartInfo testProcessStartInfo)
    {
        return base.UpdateTestProcessStartInfo(testProcessStartInfo);
    }
}
