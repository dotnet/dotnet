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

using Moq;

namespace Microsoft.TestPlatform.CommunicationUtilities.PlatformTests;

[TestClass]
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
    public async Task SocketServerStopShouldStopListening()
    {
        var connected = false;
        _socketServer.Connected += (sender, eventArgs) => connected = true;
        var connectionInfo = _socketServer.Start(_defaultConnection);

        _socketServer.Stop();

        var connectionFailed = false;
        try
        {
            await ConnectToServer(connectionInfo.GetIpEndPoint().Port);
        }
        catch (SocketException)
        {
            connectionFailed = true;
        }

        Assert.IsTrue(connectionFailed);
        Assert.IsFalse(connected);
    }

    [TestMethod]
    public void SocketServerStartShouldThrowAfterStop()
    {
        _socketServer.Stop();

        Assert.ThrowsExactly<ObjectDisposedException>(() => _socketServer.Start(_defaultConnection));
    }

    [TestMethod]
    public async Task SocketServerStopShouldNotWaitForConnectedHandlerWhenAcceptCompletesSynchronously()
    {
        var channel = new Mock<ICommunicationChannel>();
        var connectedClients = await CreateConnectedTcpClients();
        using var tcpClient = connectedClients.Client;
        using var acceptedClient = connectedClients.AcceptedClient;

        var socketServer = new SocketServer(_ => channel.Object, _ => Task.FromResult(acceptedClient));
        using var handlerEntered = new ManualResetEventSlim(false);
        using var handlerExited = new ManualResetEventSlim(false);
        using var releaseHandler = new ManualResetEventSlim(false);
        socketServer.Connected += (sender, eventArgs) =>
        {
            handlerEntered.Set();
            releaseHandler.Wait(Timeout * 2, TestContext.CancellationToken);
            handlerExited.Set();
        };

        try
        {
            Task<string?> startTask;
            try
            {
                startTask = Task.Run(() => socketServer.Start(_defaultConnection), TestContext.CancellationToken);
                Assert.IsTrue(handlerEntered.Wait(Timeout, TestContext.CancellationToken));

                var stopTask = Task.Run(socketServer.Stop, TestContext.CancellationToken);
                Assert.IsTrue(stopTask.Wait(Timeout, TestContext.CancellationToken));
            }
            finally
            {
                releaseHandler.Set();
            }

            Assert.IsTrue(handlerExited.Wait(Timeout, TestContext.CancellationToken));
            Assert.IsTrue(startTask.Wait(Timeout, TestContext.CancellationToken));
        }
        finally
        {
            socketServer.Stop();
            acceptedClient.Close();
        }
    }

    [TestMethod]
    public async Task SocketServerShouldCloseAcceptedClientWhenChannelFactoryThrowsDuringStop()
    {
        var connectedClients = await CreateConnectedTcpClients();
        using var tcpClient = connectedClients.Client;
        using var acceptedClient = connectedClients.AcceptedClient;
        using var channelFactoryCalled = new ManualResetEventSlim(false);
        SocketServer? socketServer = null;
        socketServer = new SocketServer(
            _ =>
            {
                socketServer!.Stop();
                channelFactoryCalled.Set();
                throw new InvalidOperationException();
            },
            _ => Task.FromResult(acceptedClient));
        socketServer.Connected += (sender, eventArgs) => { };

        try
        {
            socketServer.Start(_defaultConnection);

            Assert.IsTrue(channelFactoryCalled.Wait(Timeout, TestContext.CancellationToken));
            Assert.IsTrue(tcpClient.Client.Poll(Timeout * 1000, SelectMode.SelectRead));
            Assert.AreEqual(0, tcpClient.Client.Available);
        }
        finally
        {
            socketServer.Stop();
        }
    }

    [TestMethod]
    public void SocketServerStopShouldCloseClient()
    {
        using ManualResetEventSlim waitEvent = new(false);
        _socketServer.Disconnected += (s, e) => waitEvent.Set();
        SetupChannel(out ConnectedEventArgs? clientConnected);

        _socketServer.Stop();

        Assert.IsTrue(waitEvent.Wait(Timeout, TestContext.CancellationToken));
        Assert.IsTrue(_tcpClient.Client.Poll(Timeout * 1000, SelectMode.SelectRead));
        Assert.AreEqual(0, _tcpClient.Client.Available);
    }

    [TestMethod]
    public void SocketServerStopShouldRaiseClientDisconnectedEventOnClientDisconnection()
    {
        DisconnectedEventArgs? disconnected = null;
        using ManualResetEventSlim waitEvent = new(false);
        _socketServer.Disconnected += (s, e) =>
        {
            disconnected = e;
            waitEvent.Set();
        };
        SetupChannel(out ConnectedEventArgs? clientConnected);

        _socketServer.Stop();

        Assert.IsTrue(waitEvent.Wait(Timeout, TestContext.CancellationToken));
        Assert.IsNotNull(disconnected);
        Assert.IsNull(disconnected.Error);
    }

    [TestMethod]
    public void SocketServerStopShouldCloseChannel()
    {
        var channel = new Mock<ICommunicationChannel>();
        var socketServer = new TestSocketServer(_ => channel.Object);
        using var waitEvent = new ManualResetEventSlim(false);
        socketServer.Connected += (sender, eventArgs) => waitEvent.Set();
        var connectionInfo = socketServer.Start(_defaultConnection);
        ConnectToServer(connectionInfo.GetIpEndPoint().Port).GetAwaiter().GetResult();
        Assert.IsTrue(waitEvent.Wait(Timeout, TestContext.CancellationToken));
        waitEvent.Reset();
        socketServer.Disconnected += (sender, eventArgs) => waitEvent.Set();

        socketServer.Stop();

        Assert.IsTrue(waitEvent.Wait(Timeout, TestContext.CancellationToken));
        channel.Verify(communicationChannel => communicationChannel.Dispose(), Times.Once);
    }

    [TestMethod]
    public void SocketServerShouldRaiseClientDisconnectedEventIfConnectionIsBroken()
    {
        DisconnectedEventArgs? clientDisconnected = null;
        using ManualResetEventSlim waitEvent = new(false);
        _socketServer.Disconnected += (sender, eventArgs) =>
        {
            clientDisconnected = eventArgs;
            waitEvent.Set();
        };
        var channel = SetupChannel(out ConnectedEventArgs? clientConnected);

        channel!.MessageReceived.Subscribe((sender, args) =>
        {
        });

        // Close the client channel. Message loop should stop.
        // tcpClient.Close() calls tcpClient.Dispose().
        _tcpClient?.Close();

        Assert.IsTrue(waitEvent.Wait(Timeout, TestContext.CancellationToken));
        Assert.IsNull(clientDisconnected!.Error);
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
        using ManualResetEventSlim waitEvent = new(false);
        _socketServer.Connected += (sender, eventArgs) =>
        {
            clientConnectedEvent = eventArgs;
            channel = eventArgs.Channel;
            waitEvent.Set();
        };

        var connectionInfo = _socketServer.Start(_defaultConnection);
        var port = connectionInfo.GetIpEndPoint().Port;
        ConnectToServer(port).GetAwaiter().GetResult();
        Assert.IsTrue(waitEvent.Wait(Timeout, TestContext.CancellationToken));

        connectedEvent = clientConnectedEvent;
        return channel;
    }

    private async Task ConnectToServer(int port)
    {
#pragma warning disable MSTEST0049 // Use 'TestContext.CancellationToken' - ConnectAsync CancellationToken overload unavailable on .NET Framework
        await _tcpClient.ConnectAsync(IPAddress.Loopback, port);
#pragma warning restore MSTEST0049
    }

    private static async Task<(TcpClient Client, TcpClient AcceptedClient)> CreateConnectedTcpClients()
    {
        var client = new TcpClient();
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        try
        {
#pragma warning disable MSTEST0049 // Use 'TestContext.CancellationToken' - overloads unavailable on .NET Framework
            var acceptClientTask = listener.AcceptTcpClientAsync();
            await client.ConnectAsync(IPAddress.Loopback, ((IPEndPoint)listener.LocalEndpoint).Port);
            var acceptedClient = await acceptClientTask;
#pragma warning restore MSTEST0049
            return (client, acceptedClient);
        }
        catch
        {
            client.Close();
            throw;
        }
        finally
        {
            listener.Stop();
        }
    }

    private sealed class TestSocketServer(Func<Stream, ICommunicationChannel> channelFactory) : SocketServer(channelFactory);
}
