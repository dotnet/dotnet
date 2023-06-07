// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;

using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.DataCollection.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace TestPlatform.CrossPlatEngine.UnitTests.DataCollection;

[TestClass]
public class ProxyDataCollectionManagerTests
{
    private readonly Mock<IDataCollectionRequestSender> _mockDataCollectionRequestSender;
    private ProxyDataCollectionManager _proxyDataCollectionManager;
    private readonly Mock<IDataCollectionLauncher> _mockDataCollectionLauncher;
    private readonly Mock<IProcessHelper> _mockProcessHelper;
    private readonly Mock<IRequestData> _mockRequestData;
    private readonly Mock<IMetricsCollection> _mockMetricsCollection;
    private static readonly string TimoutErrorMessage =
        "vstest.console process failed to connect to datacollector process after 90 seconds. This may occur due to machine slowness, please set environment variable VSTEST_CONNECTION_TIMEOUT to increase timeout.";

    public ProxyDataCollectionManagerTests()
    {
        _mockDataCollectionRequestSender = new Mock<IDataCollectionRequestSender>();
        _mockDataCollectionLauncher = new Mock<IDataCollectionLauncher>();
        _mockProcessHelper = new Mock<IProcessHelper>();
        _mockRequestData = new Mock<IRequestData>();
        _mockMetricsCollection = new Mock<IMetricsCollection>();
        _mockRequestData.Setup(rd => rd.MetricsCollection).Returns(_mockMetricsCollection.Object);
        _proxyDataCollectionManager = new ProxyDataCollectionManager(_mockRequestData.Object, string.Empty, new List<string>() { "testsource1.dll" }, _mockDataCollectionRequestSender.Object, _mockProcessHelper.Object, _mockDataCollectionLauncher.Object);
    }

    [TestCleanup]
    public void Cleanup()
    {
        Environment.SetEnvironmentVariable(EnvironmentHelper.VstestConnectionTimeout, string.Empty);
        Environment.SetEnvironmentVariable(ProxyDataCollectionManager.DebugEnvironmentVaribleName, string.Empty);
    }

    [TestMethod]
    public void InitializeShouldInitializeCommunication()
    {
        _mockDataCollectionRequestSender.Setup(x => x.WaitForRequestHandlerConnection(EnvironmentHelper.DefaultConnectionTimeout * 1000)).Returns(true);
        _proxyDataCollectionManager.Initialize();

        _mockDataCollectionLauncher.Verify(x => x.LaunchDataCollector(It.IsAny<IDictionary<string, string?>>(), It.IsAny<IList<string>>()), Times.Once);
        _mockDataCollectionRequestSender.Verify(x => x.WaitForRequestHandlerConnection(EnvironmentHelper.DefaultConnectionTimeout * 1000), Times.Once);
    }

    [TestMethod]
    public void InitializeShouldThrowExceptionIfConnectionTimeouts()
    {
        _mockDataCollectionRequestSender.Setup(x => x.WaitForRequestHandlerConnection(It.IsAny<int>())).Returns(false);

        var message = Assert.ThrowsException<TestPlatformException>(() => _proxyDataCollectionManager.Initialize()).Message;
        Assert.AreEqual(message, TimoutErrorMessage);
    }

    [TestMethod]
    public void InitializeShouldSetTimeoutBasedOnTimeoutEnvironmentVarible()
    {
        var timeout = 10;
        Environment.SetEnvironmentVariable(EnvironmentHelper.VstestConnectionTimeout, timeout.ToString(CultureInfo.CurrentCulture));
        _mockDataCollectionRequestSender.Setup(x => x.WaitForRequestHandlerConnection(timeout * 1000)).Returns(true);

        _proxyDataCollectionManager.Initialize();

        _mockDataCollectionRequestSender.Verify(x => x.WaitForRequestHandlerConnection(timeout * 1000), Times.Once);
    }

    [TestMethod]
    public void InitializeShouldSetTimeoutBasedOnDebugEnvironmentVaribleName()
    {
        Environment.SetEnvironmentVariable(ProxyDataCollectionManager.DebugEnvironmentVaribleName, "1");
        var expectedTimeout = EnvironmentHelper.DefaultConnectionTimeout * 1000 * 5;
        _mockDataCollectionRequestSender.Setup(x => x.WaitForRequestHandlerConnection(expectedTimeout)).Returns(true);

        _proxyDataCollectionManager.Initialize();

        _mockDataCollectionRequestSender.Verify(x => x.WaitForRequestHandlerConnection(expectedTimeout), Times.Once);
    }

    [TestMethod]
    public void InitializeShouldPassDiagArgumentsIfDiagIsEnabled()
    {
        // Saving the EqtTrace state
#if NETFRAMEWORK
        var traceLevel = EqtTrace.TraceLevel;
        EqtTrace.TraceLevel = TraceLevel.Off;
#else
        var traceLevel = (TraceLevel)EqtTrace.TraceLevel;
        EqtTrace.TraceLevel = (PlatformTraceLevel)TraceLevel.Off;
#endif
        var traceFileName = EqtTrace.LogFile;

        try
        {
            EqtTrace.InitializeTrace("mylog.txt", PlatformTraceLevel.Info);
            _mockDataCollectionRequestSender.Setup(x => x.WaitForRequestHandlerConnection(It.IsAny<int>())).Returns(true);

            _proxyDataCollectionManager.Initialize();

            var expectedTraceLevel = (int)PlatformTraceLevel.Info;
            _mockDataCollectionLauncher.Verify(
                x =>
                    x.LaunchDataCollector(
                        It.IsAny<IDictionary<string, string?>>(),
                        It.Is<IList<string>>(list => list.Contains("--diag") && list.Contains("--tracelevel") && list.Contains(expectedTraceLevel.ToString(CultureInfo.CurrentCulture)))),
                Times.Once);
        }
        finally
        {
            // Restoring to initial state for EqtTrace
            EqtTrace.InitializeTrace(traceFileName, PlatformTraceLevel.Verbose);
#if NETFRAMEWORK
            EqtTrace.TraceLevel = traceLevel;
#else
            EqtTrace.TraceLevel = (PlatformTraceLevel)traceLevel;
#endif
        }
    }

    [TestMethod]
    public void SendTestHostInitiazliedShouldPassProcessIdToRequestSender()
    {
        _proxyDataCollectionManager.TestHostLaunched(1234);

        _mockDataCollectionRequestSender.Verify(x => x.SendTestHostLaunched(It.Is<TestHostLaunchedPayload>(y => y.ProcessId == 1234)));
    }

    [TestMethod]
    public void BeforeTestRunStartShouldPassRunSettingsWithExtensionsFolderUpdatedAsTestAdapterPath()
    {
        string runsettings = $"<?xml version=\"1.0\" encoding=\"utf-8\"?><RunSettings><RunConfiguration></RunConfiguration></RunSettings>";
        var sourceList = new List<string>() { "testsource1.dll" };
        _proxyDataCollectionManager = new ProxyDataCollectionManager(_mockRequestData.Object, runsettings, sourceList, _mockDataCollectionRequestSender.Object, _mockProcessHelper.Object, _mockDataCollectionLauncher.Object);
        _mockRequestData.Setup(r => r.IsTelemetryOptedIn).Returns(true);

        BeforeTestRunStartResult res = new(new Dictionary<string, string?>(), 123);
        _mockDataCollectionRequestSender.Setup(x => x.SendBeforeTestRunStartAndGetResult(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>(), It.IsAny<ITestMessageEventHandler>())).Returns(res);

        var result = _proxyDataCollectionManager.BeforeTestRunStart(true, true, null);

        var extensionsFolderPath = Path.Combine(Path.GetDirectoryName(typeof(ITestPlatform).Assembly.Location)!, "Extensions");
        var expectedSettingsXml = $"<?xml version=\"1.0\" encoding=\"utf-8\"?><RunSettings><RunConfiguration><TestAdaptersPaths>{extensionsFolderPath}</TestAdaptersPaths></RunConfiguration></RunSettings>";
        _mockDataCollectionRequestSender.Verify(
            x => x.SendBeforeTestRunStartAndGetResult(expectedSettingsXml, sourceList, true, It.IsAny<ITestMessageEventHandler>()), Times.Once);
    }

    [TestMethod]
    public void BeforeTestRunStartShouldReturnDataCollectorParameters()
    {
        BeforeTestRunStartResult res = new(new Dictionary<string, string?>(), 123);
        var sourceList = new List<string>() { "testsource1.dll" };
        _mockDataCollectionRequestSender.Setup(x => x.SendBeforeTestRunStartAndGetResult(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>(), It.IsAny<ITestMessageEventHandler>())).Returns(res);

        var result = _proxyDataCollectionManager.BeforeTestRunStart(true, true, null);

        _mockDataCollectionRequestSender.Verify(
            x => x.SendBeforeTestRunStartAndGetResult(It.IsAny<string>(), sourceList, false, It.IsAny<ITestMessageEventHandler>()), Times.Once);
        Assert.IsNotNull(result);
        Assert.AreEqual(res.DataCollectionEventsPort, result.DataCollectionEventsPort);
        Assert.AreEqual(res.EnvironmentVariables.Count, result.EnvironmentVariables!.Count);
    }

    [TestMethod]
    public void BeforeTestRunStartsShouldInvokeRunEventsHandlerIfExceptionIsThrown()
    {
        var mockRunEventsHandler = new Mock<ITestMessageEventHandler>();
        _mockDataCollectionRequestSender.Setup(
                x => x.SendBeforeTestRunStartAndGetResult(It.IsAny<string>(), new List<string>() { "testsource1.dll" }, false, It.IsAny<ITestMessageEventHandler>()))
            .Throws<Exception>();

        var result = _proxyDataCollectionManager.BeforeTestRunStart(true, true, mockRunEventsHandler.Object);

        mockRunEventsHandler.Verify(eh => eh.HandleLogMessage(TestMessageLevel.Error, It.IsRegex("Exception of type 'System.Exception' was thrown..*")), Times.Once);
        Assert.AreEqual(0, result.EnvironmentVariables!.Count);
        Assert.IsFalse(result.AreTestCaseLevelEventsRequired);
        Assert.AreEqual(0, result.DataCollectionEventsPort);
    }

    [TestMethod]
    public void SendBeforeTestRunStartAndGetResultShouldBeInvokedWithCorrectTestSources()
    {
        var testSources = new List<string>() { "abc.dll", "efg.dll" };
        _proxyDataCollectionManager = new ProxyDataCollectionManager(_mockRequestData.Object, string.Empty, testSources, _mockDataCollectionRequestSender.Object, _mockProcessHelper.Object, _mockDataCollectionLauncher.Object);

        BeforeTestRunStartResult res = new(new Dictionary<string, string?>(), 123);
        _mockDataCollectionRequestSender.Setup(x => x.SendBeforeTestRunStartAndGetResult(string.Empty, testSources, It.IsAny<bool>(), It.IsAny<ITestMessageEventHandler>())).Returns(res);

        var result = _proxyDataCollectionManager.BeforeTestRunStart(true, true, null);

        _mockDataCollectionRequestSender.Verify(
            x => x.SendBeforeTestRunStartAndGetResult(string.Empty, testSources, false, It.IsAny<ITestMessageEventHandler>()), Times.Once);
        Assert.IsNotNull(result);
        Assert.AreEqual(res.DataCollectionEventsPort, result.DataCollectionEventsPort);
        Assert.AreEqual(res.EnvironmentVariables.Count, result.EnvironmentVariables!.Count);
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public void AfterTestRunEndShouldReturnAttachments(bool telemetryOptedIn)
    {
        var attachments = new Collection<AttachmentSet>();
        var invokedDataCollectors = new Collection<InvokedDataCollector>();
        var dispName = "MockAttachments";
        var uri = new Uri("Mock://Attachments");
        var attachmentSet = new AttachmentSet(uri, dispName);
        attachments.Add(attachmentSet);
        _mockRequestData.Setup(m => m.IsTelemetryOptedIn).Returns(telemetryOptedIn);

        var metrics = new Dictionary<string, object>()
        {
            {"key", "value"}
        };

        _mockDataCollectionRequestSender.Setup(x => x.SendAfterTestRunEndAndGetResult(It.IsAny<ITestRunEventsHandler>(), It.IsAny<bool>())).Returns(new AfterTestRunEndResult(attachments, invokedDataCollectors, metrics));

        var result = _proxyDataCollectionManager.AfterTestRunEnd(false, null);

        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Attachments!.Count);
        Assert.IsNotNull(result.Attachments[0]);
        Assert.AreEqual(dispName, result.Attachments[0].DisplayName);
        Assert.AreEqual(uri, result.Attachments[0].Uri);

        if (telemetryOptedIn)
        {
            _mockMetricsCollection.Verify(m => m.Add("key", "value"), Times.Once);
        }
        else
        {
            _mockMetricsCollection.Verify(m => m.Add(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }

    [TestMethod]
    public void AfterTestRunEndShouldInvokeRunEventsHandlerIfExceptionIsThrown()
    {
        var mockRunEventsHandler = new Mock<ITestMessageEventHandler>();
        _mockDataCollectionRequestSender.Setup(
                x => x.SendAfterTestRunEndAndGetResult(It.IsAny<ITestMessageEventHandler>(), It.IsAny<bool>()))
            .Throws<Exception>();

        var result = _proxyDataCollectionManager.AfterTestRunEnd(false, mockRunEventsHandler.Object);

        mockRunEventsHandler.Verify(eh => eh.HandleLogMessage(TestMessageLevel.Error, It.IsRegex("Exception of type 'System.Exception' was thrown..*")), Times.Once);
    }


    [TestMethod]
    public void ProxyDataCollectionShouldLogEnabledDataCollectors()
    {
        string settings = @"<RunSettings>
              <DataCollectionRunSettings>
                <DataCollectors>
                    <DataCollector friendlyName=""Code Coverage"" uri=""datacollector://Microsoft/CodeCoverage/2.0"" assemblyQualifiedName=""Microsoft.VisualStudio.Coverage.DynamicCoverageDataCollector, Microsoft.VisualStudio.TraceCollector, Version=11.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"">
                </DataCollector>
              </DataCollectors>
             </DataCollectionRunSettings>
           </RunSettings>";

        var testSources = new List<string>() { "abc.dll", "efg.dll" };
        _mockRequestData.Setup(rd => rd.IsTelemetryOptedIn).Returns(true);

        var proxyExecutionManager = new ProxyDataCollectionManager(_mockRequestData.Object, settings, testSources, _mockDataCollectionRequestSender.Object, _mockProcessHelper.Object, _mockDataCollectionLauncher.Object);

        var resultString = "{ FriendlyName = Code Coverage, Uri = datacollector://microsoft/CodeCoverage/2.0 }";
        _mockMetricsCollection.Verify(rd => rd.Add(TelemetryDataConstants.DataCollectorsEnabled, resultString), Times.Once);
    }
}
