// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.CommunicationUtilities.PlatformTests;

[TestClass]
public class SocketCommunicationManagerTests : IDisposable
{
    private const string TestDiscoveryStartMessageWithNullPayload = "{\"MessageType\":\"TestDiscovery.Start\",\"Payload\":null}";

    private const string TestDiscoveryStartMessageWithDummyPayload = "{\"MessageType\":\"TestDiscovery.Start\",\"Payload\":\"Dummy Payload\"}";

    private const string TestDiscoveryStartMessageWithVersionAndPayload = "{\"Version\":2,\"MessageType\":\"TestDiscovery.Start\",\"Payload\":\"Dummy Payload\"}";

    private const string DummyPayload = "Dummy Payload";

    private readonly SocketCommunicationManager _communicationManager;
    private readonly TcpClient _tcpClient;
    private readonly TcpListener _tcpListener;

    public SocketCommunicationManagerTests()
    {
        _communicationManager = new SocketCommunicationManager();
        _tcpClient = new TcpClient();
        _tcpListener = new TcpListener(IPAddress.Loopback, 0);
    }

    public void Dispose()
    {
        _tcpListener.Stop();
        // tcpClient.Close() calls tcpClient.Dispose().
        _tcpClient?.Close();
        _communicationManager.StopServer();
        _communicationManager.StopClient();
        GC.SuppressFinalize(this);
    }

    #region Server tests

    [TestMethod]
    public async Task HostServerShouldStartServerAndReturnPortNumber()
    {
        var port = _communicationManager.HostServer(new IPEndPoint(IPAddress.Loopback, 0)).Port;

        Assert.IsTrue(port > 0);
        await _tcpClient.ConnectAsync(IPAddress.Loopback, port);
        Assert.IsTrue(_tcpClient.Connected);
    }

    [TestMethod]
    public async Task AcceptClientAsyncShouldWaitForClientConnection()
    {
        var clientConnected = false;
        var waitEvent = new ManualResetEvent(false);
        var port = _communicationManager.HostServer(new IPEndPoint(IPAddress.Loopback, 0)).Port;

        var acceptClientTask = _communicationManager.AcceptClientAsync().ContinueWith(
            (continuationTask, state) =>
            {
                clientConnected = true;
                waitEvent.Set();
            },
            null);

        await _tcpClient.ConnectAsync(IPAddress.Loopback, port);
        Assert.IsTrue(_tcpClient.Connected);
        Assert.IsTrue(waitEvent.WaitOne(1000) && clientConnected);
    }

    [TestMethod]
    public async Task WaitForClientConnectionShouldWaitUntilClientIsConnected()
    {
        var port = _communicationManager.HostServer(new IPEndPoint(IPAddress.Loopback, 0)).Port;
        _ = _communicationManager.AcceptClientAsync();
        await _tcpClient.ConnectAsync(IPAddress.Loopback, port);

        var clientConnected = _communicationManager.WaitForClientConnection(1000);

        Assert.IsTrue(_tcpClient.Connected);
        Assert.IsTrue(clientConnected);
    }

    [TestMethod]
    public void WaitForClientConnectionShouldReturnFalseIfClientIsNotConnected()
    {
        _communicationManager.HostServer(new IPEndPoint(IPAddress.Loopback, 0));
        _ = _communicationManager.AcceptClientAsync();

        // Do not attempt the client to connect to server. Directly wait until timeout.
        var clientConnected = _communicationManager.WaitForClientConnection(100);

        Assert.IsFalse(clientConnected);
    }

    [TestMethod]
    public void StopServerShouldCloseServer()
    {
        var port = _communicationManager.HostServer(new IPEndPoint(IPAddress.Loopback, 0)).Port;
        var acceptClientTask = _communicationManager.AcceptClientAsync();

        _communicationManager.StopServer();

        Assert.ThrowsException<AggregateException>(() => _tcpClient.ConnectAsync(IPAddress.Loopback, port).Wait());
    }

    #endregion

    #region Client tests

    [TestMethod]
    public async Task SetupClientAsyncShouldConnectToServer()
    {
        var port = StartServer();
        _ = _communicationManager.SetupClientAsync(new IPEndPoint(IPAddress.Loopback, port));

        var client = await _tcpListener.AcceptTcpClientAsync();
        Assert.IsTrue(client.Connected);
    }

    [TestMethod]
    public async Task WaitForServerConnectionShouldWaitUntilClientIsConnected()
    {
        var port = StartServer();
        _ = _communicationManager.SetupClientAsync(new IPEndPoint(IPAddress.Loopback, port));
        await _tcpListener.AcceptTcpClientAsync();

        var serverConnected = _communicationManager.WaitForServerConnection(1000);

        Assert.IsTrue(serverConnected);
    }

    [TestMethod]
    public void WaitForServerConnectionShouldReturnFalseIfClientIsNotConnected()
    {
        // There is no server listening on port 20000.
        _ = _communicationManager.SetupClientAsync(new IPEndPoint(IPAddress.Loopback, 20000));

        var serverConnected = _communicationManager.WaitForServerConnection(100);

        Assert.IsFalse(serverConnected);
    }

    [TestMethod]
    public async Task StopClientShouldDisconnectClient()
    {
        // TODO: This won't throw on MacOS? No way to try it.
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return;
        }

        var client = await StartServerAndWaitForConnection();

        _communicationManager.StopClient();

        // Attempt to write on client socket should throw since it should have disconnected.
        Assert.ThrowsException<SocketException>(() => WriteOnSocket(client.Client));
    }

    #endregion

    #region Message sender tests

    [TestMethod]
    public async Task SendMessageShouldSendMessageWithoutAnyPayload()
    {
        var client = await StartServerAndWaitForConnection();

        _communicationManager.SendMessage(MessageType.StartDiscovery);

        Assert.AreEqual(TestDiscoveryStartMessageWithNullPayload, ReadFromStream(client.GetStream()));
    }

    [TestMethod]
    public async Task SendMessageWithPayloadShouldSerializeAndSendThePayload()
    {
        var client = await StartServerAndWaitForConnection();

        _communicationManager.SendMessage(MessageType.StartDiscovery, DummyPayload);

        Assert.AreEqual(TestDiscoveryStartMessageWithDummyPayload, ReadFromStream(client.GetStream()));
    }

    [TestMethod]
    public async Task SendMessageWithPayloadShouldSerializeAndSendThePayloadWithVersionStamped()
    {
        var client = await StartServerAndWaitForConnection();

        _communicationManager.SendMessage(MessageType.StartDiscovery, DummyPayload, 2);

        Assert.AreEqual(TestDiscoveryStartMessageWithVersionAndPayload, ReadFromStream(client.GetStream()));
    }

    [TestMethod]
    public async Task SendMessageWithRawMessageShouldNotSerializeThePayload()
    {
        var client = await StartServerAndWaitForConnection();

        _communicationManager.SendRawMessage(DummyPayload);

        Assert.AreEqual(DummyPayload, ReadFromStream(client.GetStream()));
    }

    #endregion

    #region Message receiver tests

    [TestMethod]
    public async Task ReceiveMessageShouldReadMessageTypeButNotDeserializeThePayload()
    {
        var client = await StartServerAndWaitForConnection();
        WriteToStream(client.GetStream(), TestDiscoveryStartMessageWithDummyPayload);

        var message = _communicationManager.ReceiveMessage();

        Assert.AreEqual(MessageType.StartDiscovery, message?.MessageType);
        // Payload property is present on the Message, but we don't populate it in the newer versions,
        // instead we populate internal field with the rawMessage, and wait until Serializer.DeserializePayload<T>(message)
        // is called by the message consumer. This avoids deserializing the payload when we just want to route the message.
        Assert.IsNull(message!.Payload);
    }

    [TestMethod]
    public async Task ReceiveMessageAsyncShouldReceiveDeserializedMessage()
    {
        var client = await StartServerAndWaitForConnection();
        WriteToStream(client.GetStream(), TestDiscoveryStartMessageWithVersionAndPayload);

        var message = await _communicationManager.ReceiveMessageAsync(CancellationToken.None);
        var versionedMessage = (VersionedMessage)message!;
        Assert.AreEqual(MessageType.StartDiscovery, versionedMessage.MessageType);
        Assert.AreEqual(2, versionedMessage.Version);
        // Payload property is present on the Message, but we don't populate it in the newer versions,
        // instead we populate internal field with the rawMessage, and wait until Serializer.DeserializePayload<T>(message)
        // is called by the message consumer. This avoids deserializing the payload when we just want to route the message.
        Assert.IsNull(versionedMessage.Payload);
    }

    [TestMethod]
    public async Task ReceiveRawMessageShouldNotDeserializeThePayload()
    {
        var client = await StartServerAndWaitForConnection();
        WriteToStream(client.GetStream(), DummyPayload);

        var message = _communicationManager.ReceiveRawMessage();

        Assert.AreEqual(DummyPayload, message);
    }

    [TestMethod]
    public async Task ReceiveRawMessageAsyncShouldNotDeserializeThePayload()
    {
        var client = await StartServerAndWaitForConnection();
        WriteToStream(client.GetStream(), DummyPayload);

        var message = await _communicationManager.ReceiveRawMessageAsync(CancellationToken.None);

        Assert.AreEqual(DummyPayload, message);
    }
    #endregion

    [TestMethod]
    public void SocketPollShouldNotHangServerClientCommunication()
    {
        // Measure the throughput with socket communication v1 (SocketCommunicationManager)
        // implementation.
        var server = new SocketCommunicationManager();
        var client = new SocketCommunicationManager();

        int port = server.HostServer(new IPEndPoint(IPAddress.Loopback, 0)).Port;
        client.SetupClientAsync(new IPEndPoint(IPAddress.Loopback, port)).Wait();
        server.AcceptClientAsync().Wait();

        server.WaitForClientConnection(1000);
        client.WaitForServerConnection(1000);

        var clientThread = new Thread(() => SendData(client));
        clientThread.Start();

        var dataReceived = 0;
        while (dataReceived < 2048 * 5)
        {
            dataReceived += server.ReceiveRawMessageAsync(CancellationToken.None).Result!.Length;
            Task.Delay(1000).Wait();
        }

        clientThread.Join();

        Assert.IsTrue(true);
    }

    [TestMethod]
    public async Task ReceiveRawMessageNotConnectedSocketShouldReturnNull()
    {
        var peer = new SocketCommunicationManager();
        Assert.IsNull(peer.ReceiveRawMessage());
        Assert.IsNull(await peer.ReceiveRawMessageAsync(CancellationToken.None));
    }

    [TestMethod]
    public async Task ReceiveMessageNotConnectedSocketShouldReturnNull()
    {
        var peer = new SocketCommunicationManager();
        Assert.IsNull(peer.ReceiveMessage());
        Assert.IsNull(await peer.ReceiveMessageAsync(CancellationToken.None));
    }

    private static void SendData(ICommunicationManager communicationManager)
    {
        // Having less than the buffer size in SocketConstants.BUFFERSIZE.
        var dataBytes = new byte[2048];
        for (int i = 0; i < dataBytes.Length; i++)
        {
            dataBytes[i] = 0x65;
        }

        var dataBytesStr = Encoding.UTF8.GetString(dataBytes);

        for (int i = 0; i < 5; i++)
        {
            communicationManager.SendRawMessage(dataBytesStr);
        }
    }

    private int StartServer()
    {
        _tcpListener.Start();

        return ((IPEndPoint)_tcpListener.LocalEndpoint).Port;
    }

    private async Task<TcpClient> StartServerAndWaitForConnection()
    {
        var port = StartServer();
        _ = _communicationManager.SetupClientAsync(new IPEndPoint(IPAddress.Loopback, port));
        var client = await _tcpListener.AcceptTcpClientAsync();
        _communicationManager.WaitForServerConnection(1000);

        return client;
    }

    private static void WriteOnSocket(Socket socket)
    {
        for (int i = 0; i < 10; i++)
        {
            socket.Send(new byte[2] { 0x1, 0x0 });
        }
    }

    private static string ReadFromStream(Stream stream)
    {
        using var reader = new BinaryReader(stream, Encoding.UTF8, true);
        return reader.ReadString();
    }

    private static void WriteToStream(Stream stream, string data)
    {
        using var writer = new BinaryWriter(stream, Encoding.UTF8, true);
        writer.Write(data);
        writer.Flush();
    }
}
