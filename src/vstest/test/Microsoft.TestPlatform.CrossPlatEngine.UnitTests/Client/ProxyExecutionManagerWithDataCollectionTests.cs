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
