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

[TestClass]
public class SocketServerShutdownTests
{
    private const int Timeout = 10000;

    [TestMethod]
    public async Task SocketServerStopShouldStopListening()
    {
        var socketServer = new SocketServer();
        var connected = false;
        socketServer.Connected += (_, _) => connected = true;
        try
        {
            var endpoint = socketServer.Start(IPAddress.Loopback + ":0").GetIpEndPoint();

            socketServer.Stop();

            await AssertListenerClosedAsync(endpoint);
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
        socketServer.Stop();

        Assert.ThrowsException<ObjectDisposedException>(() => socketServer.Start(IPAddress.Loopback + ":0"));
    }

    [TestMethod]
    public async Task SocketServerShouldCloseClientAcceptedAfterStopWithoutCreatingChannel()
    {
        var connections = await CreateConnectedTcpClients();
        using var client = connections.Client;
        using var acceptedClient = connections.AcceptedClient;
        var accept = new TaskCompletionSource<TcpClient>(TaskCreationOptions.RunContinuationsAsynchronously);
        var channelFactory = new Mock<Func<Stream, ICommunicationChannel>>(MockBehavior.Strict);
        var socketServer = new SocketServer(channelFactory.Object, _ => accept.Task);
        var connected = false;
        socketServer.Connected += (_, _) => connected = true;
        try
        {
            var endpoint = socketServer.Start(IPAddress.Loopback + ":0").GetIpEndPoint();
            socketServer.Stop();
            await AssertListenerClosedAsync(endpoint);

            accept.SetResult(acceptedClient);

            Assert.IsTrue(client.Client.Poll(Timeout * 1000, SelectMode.SelectRead));
            Assert.AreEqual(0, client.Client.Available);
            Assert.IsFalse(connected);
            channelFactory.Verify(factory => factory(It.IsAny<Stream>()), Times.Never);
        }
        finally
        {
            socketServer.Stop();
            accept.TrySetResult(acceptedClient);
        }
    }

    [TestMethod]
    public async Task SocketServerStopShouldNotWaitForConnectedHandlerWhenAcceptCompletesSynchronously()
    {
        var channel = new Mock<ICommunicationChannel>();
        var connections = await CreateConnectedTcpClients();
        using var client = connections.Client;
        using var acceptedClient = connections.AcceptedClient;
        var socketServer = new SocketServer(_ => channel.Object, _ => Task.FromResult(acceptedClient));
        using var handlerEntered = new ManualResetEventSlim(false);
        using var handlerExited = new ManualResetEventSlim(false);
        using var releaseHandler = new ManualResetEventSlim(false);
        using var disconnected = new ManualResetEventSlim(false);
        socketServer.Connected += (_, _) =>
        {
            handlerEntered.Set();
            releaseHandler.Wait(Timeout * 2);
            handlerExited.Set();
        };
        socketServer.Disconnected += (_, _) => disconnected.Set();

        try
        {
            Task<string?> startTask;
            try
            {
                startTask = Task.Run(() => socketServer.Start(IPAddress.Loopback + ":0"));
                Assert.IsTrue(handlerEntered.Wait(Timeout));

                var stopTask = Task.Run(socketServer.Stop);
                Assert.IsTrue(stopTask.Wait(Timeout));
            }
            finally
            {
                releaseHandler.Set();
            }

            Assert.IsTrue(handlerExited.Wait(Timeout));
            Assert.IsTrue(startTask.Wait(Timeout));
            Assert.IsTrue(disconnected.Wait(Timeout));
            await AssertListenerClosedAsync(startTask.Result.GetIpEndPoint());
            Assert.IsTrue(client.Client.Poll(Timeout * 1000, SelectMode.SelectRead));
            Assert.AreEqual(0, client.Client.Available);
            channel.Verify(communicationChannel => communicationChannel.Dispose(), Times.Once);
        }
        finally
        {
            socketServer.Stop();
        }
    }

    [TestMethod]
    public async Task SocketServerShouldCloseAcceptedClientWhenChannelFactoryThrowsDuringStop()
    {
        var connections = await CreateConnectedTcpClients();
        using var client = connections.Client;
        using var acceptedClient = connections.AcceptedClient;
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
        var connected = false;
        socketServer.Connected += (_, _) => connected = true;

        try
        {
            var endpoint = socketServer.Start(IPAddress.Loopback + ":0").GetIpEndPoint();

            Assert.IsTrue(channelFactoryCalled.Wait(Timeout));
            Assert.IsTrue(client.Client.Poll(Timeout * 1000, SelectMode.SelectRead));
            Assert.AreEqual(0, client.Client.Available);
            Assert.IsFalse(connected);
            await AssertListenerClosedAsync(endpoint);
        }
        finally
        {
            socketServer.Stop();
        }
    }

    [DataTestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public async Task SocketServerShouldNotPublishChannelWhenStoppedDuringChannelCreation(bool stopOnFactoryThread)
    {
        var channel = new Mock<ICommunicationChannel>();
        var connections = await CreateConnectedTcpClients();
        using var client = connections.Client;
        using var acceptedClient = connections.AcceptedClient;
        using var factoryEntered = new ManualResetEventSlim(false);
        using var releaseFactory = new ManualResetEventSlim(false);
        using var disconnected = new ManualResetEventSlim(false);
        var disconnectedCount = 0;
        var stopCompletedDuringDisposal = false;
        SocketServer? socketServer = null;
        socketServer = new SocketServer(
            _ =>
            {
                factoryEntered.Set();
                if (stopOnFactoryThread)
                {
                    socketServer!.Stop();
                }
                else
                {
                    Assert.IsTrue(releaseFactory.Wait(Timeout * 2));
                }

                return channel.Object;
            },
            _ => Task.FromResult(acceptedClient));
        channel.Setup(communicationChannel => communicationChannel.Dispose()).Callback(() =>
        {
            var stopTask = Task.Run(socketServer.Stop);
            stopCompletedDuringDisposal = stopTask.Wait(Timeout);
        });
        var connected = false;
        socketServer.Connected += (_, _) => connected = true;
        socketServer.Disconnected += (_, _) =>
        {
            Interlocked.Increment(ref disconnectedCount);
            disconnected.Set();
        };
        var startTask = Task.Run(() => socketServer.Start(IPAddress.Loopback + ":0"));

        try
        {
            Assert.IsTrue(factoryEntered.Wait(Timeout));
            if (!stopOnFactoryThread)
            {
                var stopTask = Task.Run(socketServer.Stop);
                Assert.IsTrue(stopTask.Wait(Timeout));
            }

            releaseFactory.Set();
            Assert.IsTrue(startTask.Wait(Timeout));
            var endpoint = (await startTask).GetIpEndPoint();

            Assert.IsFalse(connected, "A channel must not be published after Stop completes during channel creation.");
            Assert.AreEqual(0, disconnectedCount, "An unpublished channel must not enter the message loop.");
            channel.Verify(communicationChannel => communicationChannel.Dispose(), Times.Once);
            Assert.IsTrue(stopCompletedDuringDisposal, "Channel disposal must run outside the state lock.");
            channel.Verify(communicationChannel => communicationChannel.NotifyDataAvailable(), Times.Never);
            Assert.IsTrue(client.Client.Poll(Timeout * 1000, SelectMode.SelectRead));
            Assert.AreEqual(0, client.Client.Available);
            await AssertListenerClosedAsync(endpoint);
            socketServer.Stop();
            channel.Verify(communicationChannel => communicationChannel.Dispose(), Times.Once);
        }
        finally
        {
            releaseFactory.Set();
            socketServer.Stop();
            Assert.IsTrue(startTask.Wait(Timeout));
            if (connected)
            {
                Assert.IsTrue(disconnected.Wait(Timeout));
            }
        }
    }

    private static async Task AssertListenerClosedAsync(IPEndPoint endpoint)
    {
        using var secondClient = new TcpClient();
        await Assert.ThrowsExceptionAsync<SocketException>(() => secondClient.ConnectAsync(endpoint.Address, endpoint.Port));
    }

    private static async Task<(TcpClient Client, TcpClient AcceptedClient)> CreateConnectedTcpClients()
    {
        var client = new TcpClient();
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        try
        {
            var acceptClientTask = listener.AcceptTcpClientAsync();
            await client.ConnectAsync(IPAddress.Loopback, ((IPEndPoint)listener.LocalEndpoint).Port);
            var acceptedClient = await acceptClientTask;
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
}
