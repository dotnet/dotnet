// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;

using Microsoft.TestPlatform.CommunicationUtilities.UnitTests.TestDoubles;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollector.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.DataCollection;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Newtonsoft.Json.Linq;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests;

[TestClass]
public class DataCollectionRequestHandlerTests
{
    private readonly Mock<ICommunicationManager> _mockCommunicationManager;
    private readonly Mock<IMessageSink> _mockMessageSink;
    private readonly Mock<IDataCollectionManager> _mockDataCollectionManager;
    private readonly Mock<IDataCollectionTestCaseEventHandler> _mockDataCollectionTestCaseEventHandler;
    private readonly TestableDataCollectionRequestHandler _requestHandler;
    private readonly Mock<IDataSerializer> _mockDataSerializer;
    private readonly Mock<IFileHelper> _mockFileHelper;
    private readonly Mock<IRequestData> _mockRequestData;
    private readonly Mock<IMetricsCollection> _mockMetricsCollection;
    private readonly Message _afterTestRunEnd = new() { MessageType = MessageType.AfterTestRunEnd, Payload = "false" };
    private readonly Message _beforeTestRunStart = new()
    {
        MessageType = MessageType.BeforeTestRunStart,
        Payload = JToken.FromObject(new BeforeTestRunStartPayload { SettingsXml = "settingsxml", Sources = new List<string> { "test1.dll" } })
    };

    public DataCollectionRequestHandlerTests()
    {
        _mockCommunicationManager = new Mock<ICommunicationManager>();
        _mockMessageSink = new Mock<IMessageSink>();
        _mockDataCollectionManager = new Mock<IDataCollectionManager>();
        _mockDataSerializer = new Mock<IDataSerializer>();
        _mockDataCollectionTestCaseEventHandler = new Mock<IDataCollectionTestCaseEventHandler>();
        _mockDataCollectionTestCaseEventHandler.Setup(x => x.WaitForRequestHandlerConnection(It.IsAny<int>())).Returns(true);
        _mockFileHelper = new Mock<IFileHelper>();
        _mockRequestData = new Mock<IRequestData>();
        _mockMetricsCollection = new Mock<IMetricsCollection>();
        _mockRequestData.Setup(r => r.MetricsCollection).Returns(_mockMetricsCollection.Object);
        _requestHandler = new TestableDataCollectionRequestHandler(_mockCommunicationManager.Object, _mockMessageSink.Object, _mockDataCollectionManager.Object, _mockDataCollectionTestCaseEventHandler.Object, _mockDataSerializer.Object, _mockFileHelper.Object, _mockRequestData.Object);

        _mockCommunicationManager.SetupSequence(x => x.ReceiveMessage()).Returns(_beforeTestRunStart).Returns(_afterTestRunEnd);

        _mockDataCollectionManager.Setup(x => x.SessionStarted(It.IsAny<SessionStartEventArgs>())).Returns(true);
    }

    [TestCleanup]
    public void Cleanup()
    {
        Environment.SetEnvironmentVariable(EnvironmentHelper.VstestConnectionTimeout, string.Empty);
    }

    [TestMethod]
    public void CreateInstanceShouldThrowExceptionIfInstanceCommunicationManagerIsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() => DataCollectionRequestHandler.Create(null!, _mockMessageSink.Object));
    }

    [TestMethod]
    public void CreateInstanceShouldThrowExceptinIfInstanceMessageSinkIsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() => DataCollectionRequestHandler.Create(_mockCommunicationManager.Object, null!));
    }

    [TestMethod]
    public void CreateInstanceShouldCreateInstance()
    {
        var result = DataCollectionRequestHandler.Create(_mockCommunicationManager.Object, _mockMessageSink.Object);

        Assert.AreEqual(result, DataCollectionRequestHandler.Instance);
    }

    [TestMethod]
    public void InitializeCommunicationShouldInitializeCommunication()
    {
        _requestHandler.InitializeCommunication(123);

        _mockCommunicationManager.Verify(x => x.SetupClientAsync(new IPEndPoint(IPAddress.Loopback, 123)), Times.Once);
    }

    [TestMethod]
    public void InitializeCommunicationShouldThrowExceptionIfThrownByCommunicationManager()
    {
        _mockCommunicationManager.Setup(x => x.SetupClientAsync(It.IsAny<IPEndPoint>())).Throws<Exception>();

        Assert.ThrowsException<Exception>(() => _requestHandler.InitializeCommunication(123));
    }

    [TestMethod]
    public void WaitForRequestSenderConnectionShouldInvokeCommunicationManager()
    {
        _requestHandler.WaitForRequestSenderConnection(0);

        _mockCommunicationManager.Verify(x => x.WaitForServerConnection(It.IsAny<int>()), Times.Once);
    }

    [TestMethod]
    public void WaitForRequestSenderConnectionShouldThrowExceptionIfThrownByCommunicationManager()
    {
        _mockCommunicationManager.Setup(x => x.WaitForServerConnection(It.IsAny<int>())).Throws<Exception>();

        Assert.ThrowsException<Exception>(() => _requestHandler.WaitForRequestSenderConnection(0));
    }

    [TestMethod]
    public void SendDataCollectionMessageShouldSendMessageToCommunicationManager()
    {
        var message = new DataCollectionMessageEventArgs(TestMessageLevel.Error, "message");

        _requestHandler.SendDataCollectionMessage(message);

        _mockCommunicationManager.Verify(x => x.SendMessage(MessageType.DataCollectionMessage, message), Times.Once);
    }

    [TestMethod]
    public void SendDataCollectionMessageShouldThrowExceptionIfThrownByCommunicationManager()
    {
        _mockCommunicationManager.Setup(x => x.SendMessage(MessageType.DataCollectionMessage, It.IsAny<DataCollectionMessageEventArgs>())).Throws<Exception>();
        var message = new DataCollectionMessageEventArgs(TestMessageLevel.Error, "message");

        Assert.ThrowsException<Exception>(() => _requestHandler.SendDataCollectionMessage(message));
    }

    [TestMethod]
    public void CloseShouldCloseCommunicationChannel()
    {
        _requestHandler.Close();

        _mockCommunicationManager.Verify(x => x.StopClient(), Times.Once);
    }

    [TestMethod]
    public void CloseShouldThrowExceptionIfThrownByCommunicationManager()
    {
        _mockCommunicationManager.Setup(x => x.StopClient()).Throws<Exception>();

        Assert.ThrowsException<Exception>(() => _requestHandler.Close());
    }

    [TestMethod]
    public void DisposeShouldCloseCommunicationChannel()
    {
        _requestHandler.Dispose();

        _mockCommunicationManager.Verify(x => x.StopClient(), Times.Once);
    }

    [TestMethod]
    public void ProcessRequestsShouldProcessRequests()
    {
        var testHostLaunchedPayload = new TestHostLaunchedPayload();
        testHostLaunchedPayload.ProcessId = 1234;

        _mockCommunicationManager.SetupSequence(x => x.ReceiveMessage()).Returns(_beforeTestRunStart)
            .Returns(new Message() { MessageType = MessageType.TestHostLaunched, Payload = JToken.FromObject(testHostLaunchedPayload) })
            .Returns(_afterTestRunEnd);

        _mockDataCollectionManager.Setup(x => x.SessionStarted(It.IsAny<SessionStartEventArgs>())).Returns(true);
        _mockDataCollectionManager.Setup(x => x.TestHostLaunched(It.IsAny<int>()));
        _mockDataSerializer.Setup(x => x.DeserializePayload<TestHostLaunchedPayload>(It.Is<Message>(y => y.MessageType == MessageType.TestHostLaunched)))
            .Returns(testHostLaunchedPayload);
        var beforeTestRunSTartPayload = new BeforeTestRunStartPayload { SettingsXml = "settingsxml", Sources = new List<string> { "test1.dll" } };
        _mockDataSerializer.Setup(x => x.DeserializePayload<BeforeTestRunStartPayload>(It.Is<Message>(y => y.MessageType == MessageType.BeforeTestRunStart)))
            .Returns(beforeTestRunSTartPayload);

        _requestHandler.ProcessRequests();

        _mockDataCollectionTestCaseEventHandler.Verify(x => x.InitializeCommunication(), Times.Once);
        _mockDataCollectionTestCaseEventHandler.Verify(x => x.WaitForRequestHandlerConnection(It.IsAny<int>()), Times.Once);
        _mockDataCollectionTestCaseEventHandler.Verify(x => x.ProcessRequests(), Times.Once);

        // Verify SessionStarted events
        _mockDataCollectionManager.Verify(x => x.SessionStarted(It.IsAny<SessionStartEventArgs>()), Times.Once);
        _mockCommunicationManager.Verify(x => x.SendMessage(MessageType.BeforeTestRunStartResult, It.IsAny<BeforeTestRunStartResult>()), Times.Once);

        // Verify TestHostLaunched events
        _mockDataCollectionManager.Verify(x => x.TestHostLaunched(1234), Times.Once);

        // Verify AfterTestRun events.
        _mockDataCollectionManager.Verify(x => x.SessionEnded(It.IsAny<bool>()), Times.Once);
        _mockCommunicationManager.Verify(x => x.SendMessage(MessageType.AfterTestRunEndResult, It.IsAny<AfterTestRunEndResult>()), Times.Once);
    }

    [TestMethod]
    public void ProcessRequestsShouldDisposeDataCollectorsOnAfterTestRunEnd()
    {
        _mockCommunicationManager.SetupSequence(x => x.ReceiveMessage()).Returns(new Message() { MessageType = MessageType.AfterTestRunEnd, Payload = "false" });

        _requestHandler.ProcessRequests();

        _mockDataCollectionManager.Verify(x => x.Dispose());
    }

    [TestMethod]
    public void ProcessRequestsShouldAddSourceDirectoryToTestPluginCache()
    {
        var testHostLaunchedPayload = new TestHostLaunchedPayload();
        testHostLaunchedPayload.ProcessId = 1234;

        var temp = Path.GetTempPath();
        string runSettings = "<RunSettings><RunConfiguration><TestAdaptersPaths></TestAdaptersPaths></RunConfiguration></RunSettings>";

        _mockCommunicationManager.SetupSequence(x => x.ReceiveMessage()).Returns(_beforeTestRunStart)
            .Returns(new Message() { MessageType = MessageType.TestHostLaunched, Payload = JToken.FromObject(testHostLaunchedPayload) })
            .Returns(_afterTestRunEnd);

        _mockDataCollectionManager.Setup(x => x.SessionStarted(It.IsAny<SessionStartEventArgs>())).Returns(true);
        _mockDataCollectionManager.Setup(x => x.TestHostLaunched(It.IsAny<int>()));
        _mockDataSerializer.Setup(x => x.DeserializePayload<TestHostLaunchedPayload>(It.Is<Message>(y => y.MessageType == MessageType.TestHostLaunched)))
            .Returns(testHostLaunchedPayload);
        var beforeTestRunSTartPayload = new BeforeTestRunStartPayload
        {
            SettingsXml = runSettings,
            Sources = new List<string>
            {
                Path.Combine(temp, "dir1", "test1.dll"),
                Path.Combine(temp, "dir2", "test2.dll"),
                Path.Combine(temp, "dir3", "test3.dll")
            }
        };
        _mockDataSerializer.Setup(x => x.DeserializePayload<BeforeTestRunStartPayload>(It.Is<Message>(y => y.MessageType == MessageType.BeforeTestRunStart)))
            .Returns(beforeTestRunSTartPayload);
        _mockFileHelper.Setup(x => x.DirectoryExists($@"{temp}dir1")).Returns(true);
        _mockFileHelper.Setup(x => x.EnumerateFiles($@"{temp}dir1", SearchOption.AllDirectories, @"Collector.dll")).Returns(new List<string> { Path.Combine(temp, "dir1", "abc.DataCollector.dll") });

        _requestHandler.ProcessRequests();

        _mockFileHelper.Verify(x => x.EnumerateFiles($@"{temp}dir1", SearchOption.AllDirectories, @"Collector.dll"), Times.Once);
        Assert.IsTrue(TestPluginCache.Instance.GetExtensionPaths(@"Collector.dll").Contains(Path.Combine(temp, "dir1", "abc.DataCollector.dll")));
    }

    [TestMethod]
    public void ProcessRequestsShouldThrowExceptionIfThrownByCommunicationManager()
    {
        _mockCommunicationManager.Setup(x => x.ReceiveMessage()).Throws<Exception>();

        Assert.ThrowsException<Exception>(() => _requestHandler.ProcessRequests());
    }

    [TestMethod]
    public void ProcessRequestsShouldInitializeTestCaseEventHandlerIfTestCaseLevelEventsAreEnabled()
    {
        var beforeTestRunSTartPayload = new BeforeTestRunStartPayload { SettingsXml = "settingsxml", Sources = new List<string> { "test1.dll" } };
        _mockDataSerializer.Setup(x => x.DeserializePayload<BeforeTestRunStartPayload>(It.Is<Message>(y => y.MessageType == MessageType.BeforeTestRunStart)))
            .Returns(beforeTestRunSTartPayload);

        _requestHandler.ProcessRequests();

        _mockDataCollectionTestCaseEventHandler.Verify(x => x.InitializeCommunication(), Times.Once);
        _mockDataCollectionTestCaseEventHandler.Verify(x => x.ProcessRequests(), Times.Once);
        _mockDataCollectionTestCaseEventHandler.Verify(x => x.WaitForRequestHandlerConnection(It.IsAny<int>()), Times.Once);
    }

    [TestMethod]
    public void ProcessRequestsShouldSetDefaultTimeoutIfNoEnvVarialbeSet()
    {
        var beforeTestRunSTartPayload = new BeforeTestRunStartPayload { SettingsXml = "settingsxml", Sources = new List<string> { "test1.dll" } };
        _mockDataSerializer.Setup(x => x.DeserializePayload<BeforeTestRunStartPayload>(It.Is<Message>(y => y.MessageType == MessageType.BeforeTestRunStart)))
            .Returns(beforeTestRunSTartPayload);

        _requestHandler.ProcessRequests();

        _mockDataCollectionTestCaseEventHandler.Verify(h => h.WaitForRequestHandlerConnection(EnvironmentHelper.DefaultConnectionTimeout * 1000));
    }

    [TestMethod]
    public void ProcessRequestsShouldSetTimeoutBasedOnEnvVariable()
    {
        var timeout = 10;
        Environment.SetEnvironmentVariable(EnvironmentHelper.VstestConnectionTimeout, timeout.ToString(CultureInfo.InvariantCulture));
        var beforeTestRunSTartPayload = new BeforeTestRunStartPayload { SettingsXml = "settingsxml", Sources = new List<string> { "test1.dll" } };
        _mockDataSerializer.Setup(x => x.DeserializePayload<BeforeTestRunStartPayload>(It.Is<Message>(y => y.MessageType == MessageType.BeforeTestRunStart)))
            .Returns(beforeTestRunSTartPayload);

        _requestHandler.ProcessRequests();

        _mockDataCollectionTestCaseEventHandler.Verify(h => h.WaitForRequestHandlerConnection(timeout * 1000));
    }

    [TestMethod]
    public void ProcessRequestsShouldNotInitializeTestCaseEventHandlerIfTestCaseLevelEventsAreNotEnabled()
    {
        _mockDataCollectionManager.Setup(x => x.SessionStarted(It.IsAny<SessionStartEventArgs>())).Returns(false);
        var beforeTestRunSTartPayload = new BeforeTestRunStartPayload { SettingsXml = "settingsxml", Sources = new List<string> { "test1.dll" } };
        _mockDataSerializer.Setup(x => x.DeserializePayload<BeforeTestRunStartPayload>(It.Is<Message>(y => y.MessageType == MessageType.BeforeTestRunStart)))
            .Returns(beforeTestRunSTartPayload);

        _requestHandler.ProcessRequests();

        _mockDataCollectionTestCaseEventHandler.Verify(x => x.InitializeCommunication(), Times.Never);
        _mockDataCollectionTestCaseEventHandler.Verify(x => x.ProcessRequests(), Times.Never);
        _mockDataCollectionTestCaseEventHandler.Verify(x => x.WaitForRequestHandlerConnection(It.IsAny<int>()), Times.Never);
    }

    [TestMethod]
    public void ProcessRequestsShouldReceiveCorrectPayloadInBeforeTestRunStart()
    {
        var beforeTestRunStartPayload = new BeforeTestRunStartPayload { SettingsXml = "settingsxml", Sources = new List<string> { "test1.dll" } };
        _mockDataSerializer.Setup(x => x.DeserializePayload<BeforeTestRunStartPayload>(It.Is<Message>(y => y.MessageType == MessageType.BeforeTestRunStart)))
            .Returns(beforeTestRunStartPayload);
        var message = new Message() { MessageType = MessageType.BeforeTestRunStart, Payload = JToken.FromObject(beforeTestRunStartPayload) };
        _mockCommunicationManager.SetupSequence(x => x.ReceiveMessage()).Returns(message).Returns(_afterTestRunEnd);
        _requestHandler.ProcessRequests();

        _mockDataSerializer.Verify(x => x.DeserializePayload<BeforeTestRunStartPayload>(message), Times.Once);
    }

    [TestMethod]
    public void ProcessRequestShouldInitializeDataCollectorsWithCorrectSettings()
    {
        var beforeTestRunStartPayload = new BeforeTestRunStartPayload { SettingsXml = "settingsxml", Sources = new List<string> { "test1.dll" } };
        _mockDataSerializer.Setup(x => x.DeserializePayload<BeforeTestRunStartPayload>(It.Is<Message>(y => y.MessageType == MessageType.BeforeTestRunStart)))
            .Returns(beforeTestRunStartPayload);
        var message = new Message() { MessageType = MessageType.BeforeTestRunStart, Payload = JToken.FromObject(beforeTestRunStartPayload) };
        _mockCommunicationManager.SetupSequence(x => x.ReceiveMessage()).Returns(message).Returns(_afterTestRunEnd);
        _requestHandler.ProcessRequests();

        _mockDataCollectionManager.Verify(x => x.InitializeDataCollectors("settingsxml"), Times.Once);
    }

    [TestMethod]
    public void ProcessRequestShouldCallSessionStartWithCorrectTestSources()
    {
        var beforeTestRunStartPayload = new BeforeTestRunStartPayload { SettingsXml = "settingsxml", Sources = new List<string> { "test1.dll", "test2.dll" } };
        _mockDataSerializer.Setup(x => x.DeserializePayload<BeforeTestRunStartPayload>(It.Is<Message>(y => y.MessageType == MessageType.BeforeTestRunStart)))
            .Returns(beforeTestRunStartPayload);
        var message = new Message() { MessageType = MessageType.BeforeTestRunStart, Payload = JToken.FromObject(beforeTestRunStartPayload) };
        _mockCommunicationManager.SetupSequence(x => x.ReceiveMessage()).Returns(message).Returns(_afterTestRunEnd);
        _requestHandler.ProcessRequests();

        _mockDataCollectionManager.Verify(x => x.SessionStarted(It.Is<SessionStartEventArgs>(
            y => y.GetPropertyValue<IEnumerable<string>>("TestSources")!.Contains("test1.dll") &&
                 y.GetPropertyValue<IEnumerable<string>>("TestSources")!.Contains("test2.dll"))));
    }

    [TestMethod]
    public void ProcessRequestShouldEnableTelemetry()
    {
        var beforeTestRunStartPayload = new BeforeTestRunStartPayload { SettingsXml = "settingsxml", Sources = new List<string> { "test1.dll", "test2.dll" }, IsTelemetryOptedIn = true };
        _mockRequestData.Setup(r => r.IsTelemetryOptedIn).Returns(false);
        _mockDataSerializer.Setup(x => x.DeserializePayload<BeforeTestRunStartPayload>(It.Is<Message>(y => y.MessageType == MessageType.BeforeTestRunStart)))
            .Returns(beforeTestRunStartPayload);
        var message = new Message() { MessageType = MessageType.BeforeTestRunStart, Payload = JToken.FromObject(beforeTestRunStartPayload) };
        _mockCommunicationManager.SetupSequence(x => x.ReceiveMessage()).Returns(message).Returns(_afterTestRunEnd);
        _requestHandler.ProcessRequests();

        _mockRequestData.VerifySet(r => r.IsTelemetryOptedIn = true);
        _mockRequestData.VerifySet(r => r.MetricsCollection = It.IsAny<MetricsCollection>());
    }

    [TestMethod]
    public void ProcessRequestShouldNotEnableTelemetryIfTelemetryEnabled()
    {
        var beforeTestRunStartPayload = new BeforeTestRunStartPayload { SettingsXml = "settingsxml", Sources = new List<string> { "test1.dll", "test2.dll" }, IsTelemetryOptedIn = true };
        _mockRequestData.Setup(r => r.IsTelemetryOptedIn).Returns(true);
        _mockDataSerializer.Setup(x => x.DeserializePayload<BeforeTestRunStartPayload>(It.Is<Message>(y => y.MessageType == MessageType.BeforeTestRunStart)))
            .Returns(beforeTestRunStartPayload);
        var message = new Message() { MessageType = MessageType.BeforeTestRunStart, Payload = JToken.FromObject(beforeTestRunStartPayload) };
        _mockCommunicationManager.SetupSequence(x => x.ReceiveMessage()).Returns(message).Returns(_afterTestRunEnd);
        _requestHandler.ProcessRequests();

        _mockRequestData.VerifySet(r => r.IsTelemetryOptedIn = It.IsAny<bool>(), Times.Never);
        _mockRequestData.VerifySet(r => r.MetricsCollection = It.IsAny<IMetricsCollection>(), Times.Never);
    }

    [TestMethod]
    public void ProcessRequestShouldNotEnableTelemetryIfTelemetryEnablingNotRequested()
    {
        var beforeTestRunStartPayload = new BeforeTestRunStartPayload { SettingsXml = "settingsxml", Sources = new List<string> { "test1.dll", "test2.dll" }, IsTelemetryOptedIn = false };
        _mockRequestData.Setup(r => r.IsTelemetryOptedIn).Returns(false);
        _mockDataSerializer.Setup(x => x.DeserializePayload<BeforeTestRunStartPayload>(It.Is<Message>(y => y.MessageType == MessageType.BeforeTestRunStart)))
            .Returns(beforeTestRunStartPayload);
        var message = new Message() { MessageType = MessageType.BeforeTestRunStart, Payload = JToken.FromObject(beforeTestRunStartPayload) };
        _mockCommunicationManager.SetupSequence(x => x.ReceiveMessage()).Returns(message).Returns(_afterTestRunEnd);
        _requestHandler.ProcessRequests();

        _mockRequestData.VerifySet(r => r.IsTelemetryOptedIn = It.IsAny<bool>(), Times.Never);
        _mockRequestData.VerifySet(r => r.MetricsCollection = It.IsAny<IMetricsCollection>(), Times.Never);
    }
}
