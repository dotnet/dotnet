// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.CommunicationUtilities.PlatformTests;

[TestClass]
[Ignore("Flaky")]
public class SocketServerTests : SocketTestsBase, IDisposable
{
    private readonly TcpClient _tcpClient;
    private readonly string _defaultConnection = IPAddress.Loopback.ToString() + ":0";
    private readonly ICommunicationEndPoint _socketServer;

    public SocketServerTests()
    {
        _socketServer = new SocketServer();

        _tcpClient = new TcpClient();
    }

    protected override TcpClient Client => _tcpClient;

    public void Dispose()
    {
        _socketServer.Stop();
        // tcpClient.Close() calls tcpClient.Dispose().
        _tcpClient?.Close();
        GC.SuppressFinalize(this);
    }

    [TestMethod]
    public async Task SocketServerStartShouldHostServer()
    {
        var connectionInfo = _socketServer.Start(_defaultConnection);

        Assert.IsFalse(string.IsNullOrEmpty(connectionInfo));
        await ConnectToServer(connectionInfo.GetIpEndPoint().Port);
        Assert.IsTrue(_tcpClient.Connected);
    }

    [TestMethod]
    public void SocketServerStopShouldStopListening()
    {
        var connectionInfo = _socketServer.Start(_defaultConnection);

        _socketServer.Stop();

        try
        {
            // This method throws ExtendedSocketException (which is private). It is not possible
            // to use Assert.ThrowsException in this case.
            ConnectToServer(connectionInfo.GetIpEndPoint().Port).GetAwaiter().GetResult();
        }
        catch (SocketException)
        {
        }
    }

    [TestMethod]
    public void SocketServerStopShouldCloseClient()
    {
        ManualResetEvent waitEvent = new(false);
        _socketServer.Disconnected += (s, e) => waitEvent.Set();
        SetupChannel(out ConnectedEventArgs? clientConnected);

        _socketServer.Stop();

        waitEvent.WaitOne();
        Assert.ThrowsException<IOException>(() => WriteData(_tcpClient));
    }

    [TestMethod]
    public void SocketServerStopShouldRaiseClientDisconnectedEventOnClientDisconnection()
    {
        DisconnectedEventArgs? disconnected = null;
        ManualResetEvent waitEvent = new(false);
        _socketServer.Disconnected += (s, e) =>
        {
            disconnected = e;
            waitEvent.Set();
        };
        SetupChannel(out ConnectedEventArgs? clientConnected);

        _socketServer.Stop();

        waitEvent.WaitOne();
        Assert.IsNotNull(disconnected);
        Assert.IsNull(disconnected.Error);
    }

    [TestMethod]
    public void SocketServerStopShouldCloseChannel()
    {
        var waitEvent = new ManualResetEventSlim(false);
        var channel = SetupChannel(out ConnectedEventArgs? clientConnected);
        _socketServer.Disconnected += (s, e) => waitEvent.Set();

        _socketServer.Stop();

        waitEvent.Wait();
        Assert.ThrowsException<CommunicationException>(() => channel!.Send(Dummydata));
    }

    [TestMethod]
    public void SocketServerShouldRaiseClientDisconnectedEventIfConnectionIsBroken()
    {
        DisconnectedEventArgs? clientDisconnected = null;
        ManualResetEvent waitEvent = new(false);
        _socketServer.Disconnected += (sender, eventArgs) =>
        {
            clientDisconnected = eventArgs;
            waitEvent.Set();
        };
        var channel = SetupChannel(out ConnectedEventArgs? clientConnected);

        channel!.MessageReceived += (sender, args) =>
        {
        };

        // Close the client channel. Message loop should stop.
        // tcpClient.Close() calls tcpClient.Dispose().
        _tcpClient?.Close();

        Assert.IsTrue(waitEvent.WaitOne(1000));
        Assert.IsTrue(clientDisconnected!.Error is IOException);
    }

    [TestMethod]
    public async Task SocketEndpointShouldInitializeChannelOnServerConnection()
    {
        var channel = SetupChannel(out ConnectedEventArgs? _);

        await channel!.Send(Dummydata);

        Assert.AreEqual(Dummydata, ReadData(Client));
    }

    protected override ICommunicationChannel? SetupChannel(out ConnectedEventArgs? connectedEvent)
    {
        ICommunicationChannel? channel = null;
        ConnectedEventArgs? clientConnectedEvent = null;
        ManualResetEvent waitEvent = new(false);
        _socketServer.Connected += (sender, eventArgs) =>
        {
            clientConnectedEvent = eventArgs;
            channel = eventArgs.Channel;
            waitEvent.Set();
        };

        var connectionInfo = _socketServer.Start(_defaultConnection);
        var port = connectionInfo.GetIpEndPoint().Port;
        ConnectToServer(port).GetAwaiter().GetResult();
        waitEvent.WaitOne();

        connectedEvent = clientConnectedEvent;
        return channel;
    }

    private async Task ConnectToServer(int port)
    {
        await _tcpClient.ConnectAsync(IPAddress.Loopback, port);
    }
}
