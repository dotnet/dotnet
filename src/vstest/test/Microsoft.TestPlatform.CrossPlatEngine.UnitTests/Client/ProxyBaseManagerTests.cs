// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.Parallel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace TestPlatform.CrossPlatEngine.UnitTests.Client;

[TestClass]
public class ProxyBaseManagerTests
{
    private const int Clientprocessexitwait = 10 * 1000;
    private readonly ProtocolConfig _protocolConfig = new() { Version = 2 };
    private readonly Mock<IRequestData> _mockRequestData;
    private readonly Mock<IFileHelper> _mockFileHelper;
    private readonly DiscoveryDataAggregator _discoveryDataAggregator;

    protected readonly Mock<ITestRuntimeProvider> _mockTestHostManager;

    private Mock<ICommunicationEndPoint>? _mockCommunicationEndpoint;
    private ITestRequestSender? _testRequestSender;

    protected Mock<IDataSerializer> _mockDataSerializer;
    protected Mock<ICommunicationChannel> _mockChannel;

    public ProxyBaseManagerTests()
    {
        _mockTestHostManager = new Mock<ITestRuntimeProvider>();
        _mockDataSerializer = new Mock<IDataSerializer>();
        _mockRequestData = new Mock<IRequestData>();
        _mockChannel = new Mock<ICommunicationChannel>();
        _mockFileHelper = new Mock<IFileHelper>();
        _discoveryDataAggregator = new();

        _mockRequestData.Setup(rd => rd.MetricsCollection).Returns(new Mock<IMetricsCollection>().Object);
        _mockDataSerializer.Setup(mds => mds.DeserializeMessage(null!)).Returns(new Message());
        _mockDataSerializer.Setup(mds => mds.DeserializeMessage(string.Empty)).Returns(new Message());
        _mockTestHostManager.SetupGet(th => th.Shared).Returns(true);
        _mockTestHostManager.Setup(
                m => m.GetTestHostProcessStartInfo(
                    It.IsAny<IEnumerable<string>>(),
                    It.IsAny<IDictionary<string, string?>>(),
                    It.IsAny<TestRunnerConnectionInfo>()))
            .Returns(new TestProcessStartInfo());
        _mockTestHostManager.Setup(tmh => tmh.LaunchTestHostAsync(It.IsAny<TestProcessStartInfo>(), It.IsAny<CancellationToken>()))
            .Callback(
                () => _mockTestHostManager.Raise(thm => thm.HostLaunched += null, new HostProviderEventArgs(string.Empty)))
            .Returns(Task.FromResult(true));
    }

    [MemberNotNull(nameof(_testRequestSender), nameof(_testRequestSender))]
    private void SetupAndInitializeTestRequestSender()
    {
        var connectionInfo = new TestHostConnectionInfo
        {
            Endpoint = IPAddress.Loopback + ":0",
            Role = ConnectionRole.Client,
            Transport = Transport.Sockets
        };
        _mockCommunicationEndpoint = new Mock<ICommunicationEndPoint>();
        _testRequestSender = new TestRequestSender(_mockCommunicationEndpoint.Object, connectionInfo, _mockDataSerializer.Object, _protocolConfig, Clientprocessexitwait);
        _mockCommunicationEndpoint.Setup(mc => mc.Start(connectionInfo.Endpoint)).Returns(connectionInfo.Endpoint).Callback(() => _mockCommunicationEndpoint.Raise(
            s => s.Connected += null,
            _mockCommunicationEndpoint.Object,
            new ConnectedEventArgs(_mockChannel.Object)));
        SetupChannelMessage(MessageType.VersionCheck, MessageType.VersionCheck, _protocolConfig.Version);

        _testRequestSender.InitializeCommunication();
    }

    public void SetupChannelMessage<TPayload>(string messageType, string returnMessageType, TPayload returnPayload)
    {
        _mockChannel.Setup(mc => mc.Send(It.Is<string>(s => s.Contains(messageType))))
            .Callback(() => _mockChannel.Raise(c => c.MessageReceived += null, _mockChannel.Object, new MessageReceivedEventArgs { Data = messageType }));

        _mockDataSerializer.Setup(ds => ds.SerializePayload(It.Is<string>(s => s.Equals(messageType)), It.IsAny<object>())).Returns(messageType);
        _mockDataSerializer.Setup(ds => ds.SerializePayload(It.Is<string>(s => s.Equals(messageType)), It.IsAny<object>(), It.IsAny<int>())).Returns(messageType);
        _mockDataSerializer.Setup(ds => ds.DeserializeMessage(It.Is<string>(s => s.Equals(messageType)))).Returns(new Message { MessageType = returnMessageType });
        _mockDataSerializer.Setup(ds => ds.DeserializePayload<TPayload>(It.Is<Message>(m => string.Equals(m.MessageType, messageType)))).Returns(returnPayload);
    }

    public void RaiseMessageReceived(string data)
    {
        _mockChannel.Raise(c => c.MessageReceived += null, _mockChannel.Object,
            new MessageReceivedEventArgs { Data = data });
    }

    protected ProxyDiscoveryManager GetProxyDiscoveryManager()
    {
        SetupAndInitializeTestRequestSender();
        var testDiscoveryManager = new ProxyDiscoveryManager(
            _mockRequestData.Object,
            _testRequestSender,
            _mockTestHostManager.Object,
            Framework.DefaultFramework,
            _discoveryDataAggregator,
            _mockDataSerializer.Object,
            _mockFileHelper.Object);

        return testDiscoveryManager;
    }

    internal ProxyExecutionManager GetProxyExecutionManager()
    {
        SetupAndInitializeTestRequestSender();
        _mockFileHelper.Setup(fh => fh.Exists(It.IsAny<string>())).Returns(true);
        var testExecutionManager = new ProxyExecutionManager(_mockRequestData.Object, _testRequestSender,
            _mockTestHostManager.Object, Framework.DefaultFramework, _mockDataSerializer.Object, _mockFileHelper.Object);

        return testExecutionManager;
    }
}
