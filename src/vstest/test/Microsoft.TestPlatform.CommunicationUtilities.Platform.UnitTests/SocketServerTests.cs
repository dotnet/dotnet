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
public class SocketServerLifecycleTests
{
    private const int Timeout = 5000;
    private static readonly string DefaultConnection = IPAddress.Loopback + ":0";

    [TestMethod]
    public async Task SocketServerStopBeforeAcceptShouldCloseListener()
    {
        var socketServer = new SocketServer();
        var connected = false;
        socketServer.Connected += (_, _) => connected = true;
        try
        {
            var endpoint = socketServer.Start(DefaultConnection).GetIpEndPoint();

            socketServer.Stop();
            socketServer.Stop();

            await AssertListenerClosed(endpoint);
            Assert.IsFalse(connected);
        }
        finally
        {
            socketServer.Stop();
        }
    }

    [TestMethod]
    public void SocketServerStartShouldThrowAfterStop()
    {
        var socketServer = new SocketServer();
        socketServer.Stop();

        Assert.ThrowsExactly<ObjectDisposedException>(() => socketServer.Start(DefaultConnection));
    }

    [TestMethod]
    public async Task SocketServerShouldCloseClientDeliveredAfterStop()
    {
        var connectedClients = await CreateConnectedTcpClients();
        using var client = connectedClients.Client;
        using var acceptedClient = connectedClients.AcceptedClient;
        var accept = new TaskCompletionSource<TcpClient>(TaskCreationOptions.RunContinuationsAsynchronously);
        var channel = new Mock<ICommunicationChannel>();
        var channelCreated = 0;
        var connected = 0;
        var socketServer = new SocketServer(
            _ =>
            {
                Interlocked.Increment(ref channelCreated);
                return channel.Object;
            },
            _ => accept.Task);
        socketServer.Connected += (_, _) => Interlocked.Increment(ref connected);
        try
        {
            var endpoint = socketServer.Start(DefaultConnection).GetIpEndPoint();

            socketServer.Stop();
            accept.SetResult(acceptedClient);

            Assert.IsTrue(client.Client.Poll(Timeout * 1000, SelectMode.SelectRead));
            Assert.AreEqual(0, client.Client.Available);
            await AssertListenerClosed(endpoint);
            Assert.AreEqual(0, channelCreated);
            Assert.AreEqual(0, connected);
            channel.Verify(c => c.Dispose(), Times.Never);
        }
        finally
        {
            socketServer.Stop();
            accept.TrySetResult(acceptedClient);
        }
    }

    [TestMethod]
    public async Task SocketServerStopShouldNotWaitForConnectedHandler()
    {
        var channel = new Mock<ICommunicationChannel>();
        var connectedClients = await CreateConnectedTcpClients();
        using var client = connectedClients.Client;
        using var acceptedClient = connectedClients.AcceptedClient;
        using var handlerEntered = new ManualResetEventSlim(false);
        using var releaseHandler = new ManualResetEventSlim(false);
        using var disconnected = new ManualResetEventSlim(false);
        var socketServer = new SocketServer(_ => channel.Object, _ => Task.FromResult(acceptedClient));
        socketServer.Connected += (_, _) =>
        {
            handlerEntered.Set();
            Assert.IsTrue(releaseHandler.Wait(Timeout * 2));
        };
        socketServer.Disconnected += (_, _) => disconnected.Set();
        Task<string?>? startTask = null;
        try
        {
            startTask = Task.Run(() => socketServer.Start(DefaultConnection));
            Assert.IsTrue(handlerEntered.Wait(Timeout));

            var stopTask = Task.Run(socketServer.Stop);
            Assert.IsTrue(stopTask.Wait(Timeout));
            releaseHandler.Set();

            Assert.IsTrue(startTask.Wait(Timeout));
            Assert.IsTrue(disconnected.Wait(Timeout));
            Assert.IsTrue(client.Client.Poll(Timeout * 1000, SelectMode.SelectRead));
            Assert.AreEqual(0, client.Client.Available);
            await AssertListenerClosed((await startTask).GetIpEndPoint());
            channel.Verify(c => c.Dispose(), Times.Once);
        }
        finally
        {
            releaseHandler.Set();
            socketServer.Stop();
            if (startTask is not null)
            {
                Assert.IsTrue(startTask.Wait(Timeout));
            }
        }
    }

    [TestMethod]
    public async Task SocketServerShouldCloseAcceptedClientWhenChannelFactoryThrowsDuringStop()
    {
        var connectedClients = await CreateConnectedTcpClients();
        using var client = connectedClients.Client;
        using var acceptedClient = connectedClients.AcceptedClient;
        using var factoryCalled = new ManualResetEventSlim(false);
        var connected = false;
        SocketServer? socketServer = null;
        socketServer = new SocketServer(
            _ =>
            {
                socketServer!.Stop();
                factoryCalled.Set();
                throw new InvalidOperationException();
            },
            _ => Task.FromResult(acceptedClient));
        socketServer.Connected += (_, _) => connected = true;
        try
        {
            var endpoint = socketServer.Start(DefaultConnection).GetIpEndPoint();

            Assert.IsTrue(factoryCalled.Wait(Timeout));
            Assert.IsTrue(client.Client.Poll(Timeout * 1000, SelectMode.SelectRead));
            Assert.AreEqual(0, client.Client.Available);
            await AssertListenerClosed(endpoint);
            Assert.IsFalse(connected);
        }
        finally
        {
            socketServer.Stop();
        }
    }

    [TestMethod]
    public async Task SocketServerShouldNotPublishChannelWhenStoppedDuringChannelCreation()
    {
        var connectedClients = await CreateConnectedTcpClients();
        using var client = connectedClients.Client;
        using var acceptedClient = connectedClients.AcceptedClient;
        using var factoryEntered = new ManualResetEventSlim(false);
        using var releaseFactory = new ManualResetEventSlim(false);
        var channel = new Mock<ICommunicationChannel>();
        var connected = 0;
        var socketServer = new SocketServer(
            _ =>
            {
                factoryEntered.Set();
                Assert.IsTrue(releaseFactory.Wait(Timeout * 2));
                return channel.Object;
            },
            _ => Task.FromResult(acceptedClient));
        socketServer.Connected += (_, _) => Interlocked.Increment(ref connected);
        var startTask = Task.Run(() => socketServer.Start(DefaultConnection));
        try
        {
            Assert.IsTrue(factoryEntered.Wait(Timeout));
            socketServer.Stop();
            releaseFactory.Set();

            var endpoint = (await startTask).GetIpEndPoint();
            Assert.IsTrue(client.Client.Poll(Timeout * 1000, SelectMode.SelectRead));
            Assert.AreEqual(0, client.Client.Available);
            await AssertListenerClosed(endpoint);
            Assert.AreEqual(0, connected);
            channel.Verify(c => c.Dispose(), Times.Once);
        }
        finally
        {
            releaseFactory.Set();
            socketServer.Stop();
            await startTask;
        }
    }

    private static async Task AssertListenerClosed(IPEndPoint endpoint)
    {
        using var secondClient = new TcpClient();
        await Assert.ThrowsExactlyAsync<SocketException>(() => secondClient.ConnectAsync(endpoint.Address, endpoint.Port));
    }

    private static async Task<(TcpClient Client, TcpClient AcceptedClient)> CreateConnectedTcpClients()
    {
        var client = new TcpClient();
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        try
        {
            var accept = listener.AcceptTcpClientAsync();
            await client.ConnectAsync(IPAddress.Loopback, ((IPEndPoint)listener.LocalEndpoint).Port);
            return (client, await accept);
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
}

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

        channel!.MessageReceived.Subscribe((sender, args) =>
        {
        });

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
