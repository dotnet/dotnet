// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.DataCollection;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.VisualStudio.TestPlatform.DataCollector.PlatformTests;

[TestClass]
[Ignore]    // Tests are flaky
public class CommunicationLayerIntegrationTests
{
    private readonly string _defaultRunSettings = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<RunSettings>\r\n  <DataCollectionRunSettings>\r\n    <DataCollectors >{0}</DataCollectors>\r\n  </DataCollectionRunSettings>\r\n</RunSettings>";
    private readonly Mock<ITestMessageEventHandler> _mockTestMessageEventHandler;
    private readonly string _dataCollectorSettings, _runSettings;
    private readonly IDataCollectionLauncher _dataCollectionLauncher;
    private readonly IProcessHelper _processHelper;
    private readonly Mock<IRequestData> _mockRequestData;
    private readonly Mock<IMetricsCollection> _mockMetricsCollection;
    private readonly List<string> _testSources;

    public CommunicationLayerIntegrationTests()
    {
        _mockTestMessageEventHandler = new Mock<ITestMessageEventHandler>();
        _mockRequestData = new Mock<IRequestData>();
        _mockMetricsCollection = new Mock<IMetricsCollection>();
        _mockRequestData.Setup(rd => rd.MetricsCollection).Returns(_mockMetricsCollection.Object);
        _dataCollectorSettings = string.Format(CultureInfo.InvariantCulture, "<DataCollector friendlyName=\"CustomDataCollector\" uri=\"my://custom/datacollector\" assemblyQualifiedName=\"{0}\" codebase=\"{1}\" />", typeof(CustomDataCollector).AssemblyQualifiedName, typeof(CustomDataCollector).Assembly.Location);
        _runSettings = string.Format(CultureInfo.InvariantCulture, _defaultRunSettings, _dataCollectorSettings);
        _testSources = new List<string>() { "testsource1.dll" };
        _processHelper = new ProcessHelper();
        _dataCollectionLauncher = DataCollectionLauncherFactory.GetDataCollectorLauncher(_processHelper, _runSettings);
    }

    [TestMethod]
    public void BeforeTestRunStartShouldGetEnvironmentVariables()
    {
        var dataCollectionRequestSender = new DataCollectionRequestSender();

        using var proxyDataCollectionManager = new ProxyDataCollectionManager(_mockRequestData.Object, _runSettings, _testSources, dataCollectionRequestSender, _processHelper, _dataCollectionLauncher);
        proxyDataCollectionManager.Initialize();

        var result = proxyDataCollectionManager.BeforeTestRunStart(true, true, _mockTestMessageEventHandler.Object);

        Assert.AreEqual(1, result.EnvironmentVariables?.Count);
    }

    [TestMethod]
    public void AfterTestRunShouldSendGetAttachments()
    {
        var dataCollectionRequestSender = new DataCollectionRequestSender();

        using var proxyDataCollectionManager = new ProxyDataCollectionManager(_mockRequestData.Object, _runSettings, _testSources, dataCollectionRequestSender, _processHelper, _dataCollectionLauncher);
        proxyDataCollectionManager.Initialize();

        proxyDataCollectionManager.BeforeTestRunStart(true, true, _mockTestMessageEventHandler.Object);

        var dataCollectionResult = proxyDataCollectionManager.AfterTestRunEnd(false, _mockTestMessageEventHandler.Object);

        Assert.AreEqual("CustomDataCollector", dataCollectionResult.Attachments![0].DisplayName);
        Assert.AreEqual("my://custom/datacollector", dataCollectionResult.Attachments[0].Uri.ToString());
        Assert.IsTrue(dataCollectionResult.Attachments[0].Attachments[0].Uri.ToString().Contains("filename.txt"));
    }

    [TestMethod]
    public void AfterTestRunShouldHandleSocketFailureGracefully()
    {
        var socketCommManager = new SocketCommunicationManager();
        var dataCollectionRequestSender = new DataCollectionRequestSender(socketCommManager, JsonDataSerializer.Instance);
        var dataCollectionLauncher = DataCollectionLauncherFactory.GetDataCollectorLauncher(_processHelper, _runSettings);

        using var proxyDataCollectionManager = new ProxyDataCollectionManager(_mockRequestData.Object, _runSettings, _testSources, dataCollectionRequestSender, _processHelper, dataCollectionLauncher);
        proxyDataCollectionManager.Initialize();
        proxyDataCollectionManager.BeforeTestRunStart(true, true, _mockTestMessageEventHandler.Object);

        var result = Process.GetProcessById(dataCollectionLauncher.DataCollectorProcessId);
        Assert.IsNotNull(result);

        socketCommManager.StopClient();

        var attachments = proxyDataCollectionManager.AfterTestRunEnd(false, _mockTestMessageEventHandler.Object);

        Assert.IsNull(attachments);

        // Give time to datacollector process to exit.
        Assert.IsTrue(result.WaitForExit(500));
    }
}
