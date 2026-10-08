// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
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

[TestClass]
public class SocketServerAuthenticationTests
{
    [TestMethod]
    public async Task SocketServerShouldStopAfterFirstClientFailsAuthentication()
    {
        var socketServer = new SocketServer();
        try
        {
            var validator = (IConnectionValidationConfigurable)socketServer;
            validator.RequireConnectionValidation();
            validator.UseConnectionSecrets(ConnectionSecrets.GeneratePair());
            var connected = new TaskCompletionSource<ConnectedEventArgs>(TaskCreationOptions.RunContinuationsAsynchronously);
            socketServer.Connected += (_, args) => connected.TrySetResult(args);
            var endpoint = socketServer.Start(IPAddress.Loopback + ":0").GetIpEndPoint();
            using var client = new TcpClient();
            await client.ConnectAsync(endpoint.Address, endpoint.Port);
            using (var writer = new BinaryWriter(client.GetStream(), Encoding.UTF8, leaveOpen: true))
            {
                writer.Write("Dummy Data");
            }

            Assert.IsTrue(connected.Task.Wait(10000));
            var result = await connected.Task;
            Assert.IsFalse(result.Connected);
            Assert.IsNull(result.Channel);
            Assert.IsInstanceOfType<PeerProcessValidationException>(result.Fault);
            using var secondClient = new TcpClient();
            await Assert.ThrowsExceptionAsync<SocketException>(() => secondClient.ConnectAsync(endpoint.Address, endpoint.Port));
        }
        finally
        {
            socketServer.Stop();
        }
    }

    [TestMethod]
    public async Task SocketServerStopShouldInterruptAuthentication()
    {
        var socketServer = new SocketServer();
        try
        {
            ((IConnectionAuthenticationResponder)socketServer).UseConnectionSecretsAsChild(ConnectionSecrets.GeneratePair());
            socketServer.Connected += (_, _) => { };
            var endpoint = socketServer.Start(IPAddress.Loopback + ":0").GetIpEndPoint();
            using var client = new TcpClient();
            await client.ConnectAsync(endpoint.Address, endpoint.Port);
            var stream = client.GetStream();
            stream.ReadTimeout = 10000;
            using var reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true);
            _ = reader.ReadString();

            socketServer.Stop();

            Assert.IsTrue(client.Client.Poll(1000000, SelectMode.SelectRead));
            Assert.AreEqual(0, client.Client.Available);
            using var secondClient = new TcpClient();
            await Assert.ThrowsExceptionAsync<SocketException>(() => secondClient.ConnectAsync(endpoint.Address, endpoint.Port));
        }
        finally
        {
            socketServer.Stop();
        }
    }
}

[TestClass]
public class SocketServerLifecycleTests
{
    private const int Timeout = 10000;
    private static readonly string DefaultConnection = IPAddress.Loopback + ":0";

    [TestMethod]
    public async Task SocketServerStopShouldStopListeningBeforeAnyClientConnects()
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

        Assert.ThrowsException<ObjectDisposedException>(() => socketServer.Start(DefaultConnection));
    }

    [TestMethod]
    public async Task SocketServerShouldCloseAcceptedClientDeliveredAfterStop()
    {
        using var client = new TcpClient();
        var accepted = new TaskCompletionSource<TcpClient>(TaskCreationOptions.RunContinuationsAsynchronously);
        var releaseAccept = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        var channelFactory = new Mock<Func<Stream, ICommunicationChannel>>();
        var socketServer = new SocketServer(channelFactory.Object, async listener =>
        {
            var acceptedClient = await listener.AcceptTcpClientAsync();
            accepted.SetResult(acceptedClient);
            await releaseAccept.Task;
            return acceptedClient;
        });
        var connected = false;
        socketServer.Connected += (_, _) => connected = true;

        try
        {
            var endpoint = socketServer.Start(DefaultConnection).GetIpEndPoint();
            await client.ConnectAsync(endpoint.Address, endpoint.Port);
            Assert.IsTrue(accepted.Task.Wait(Timeout));
            using var acceptedClient = await accepted.Task;

            socketServer.Stop();
            await AssertListenerClosed(endpoint);
            releaseAccept.SetResult(true);

            Assert.IsTrue(client.Client.Poll(Timeout * 1000, SelectMode.SelectRead));
            Assert.AreEqual(0, client.Client.Available);
            channelFactory.Verify(factory => factory(It.IsAny<Stream>()), Times.Never);
            Assert.IsFalse(connected);
        }
        finally
        {
            socketServer.Stop();
            releaseAccept.TrySetResult(true);
        }
    }

    [TestMethod]
    public async Task SocketServerStopShouldNotWaitForConnectedHandlerWhenAcceptCompletesSynchronously()
    {
        var channel = new Mock<ICommunicationChannel>();
        var connectedClients = await CreateConnectedTcpClients();
        using var client = connectedClients.Client;
        using var acceptedClient = connectedClients.AcceptedClient;
        using var handlerEntered = new ManualResetEventSlim(false);
        using var handlerExited = new ManualResetEventSlim(false);
        using var releaseHandler = new ManualResetEventSlim(false);
        IPEndPoint? endpoint = null;
        var socketServer = new SocketServer(_ => channel.Object, listener =>
        {
            endpoint = (IPEndPoint)listener.LocalEndpoint;
            return Task.FromResult(acceptedClient);
        });
        socketServer.Connected += (_, _) =>
        {
            handlerEntered.Set();
            releaseHandler.Wait(Timeout * 2);
            handlerExited.Set();
        };

        try
        {
            Task<string?> startTask;
            try
            {
                startTask = Task.Run(() => socketServer.Start(DefaultConnection));
                Assert.IsTrue(handlerEntered.Wait(Timeout));
                var stopTask = Task.Run(socketServer.Stop);
                Assert.IsTrue(stopTask.Wait(Timeout));
                await AssertListenerClosed(endpoint!);
            }
            finally
            {
                releaseHandler.Set();
            }

            Assert.IsTrue(handlerExited.Wait(Timeout));
            Assert.IsTrue(startTask.Wait(Timeout));
            Assert.IsTrue(client.Client.Poll(Timeout * 1000, SelectMode.SelectRead));
            Assert.AreEqual(0, client.Client.Available);
        }
        finally
        {
            socketServer.Stop();
        }
    }

    [TestMethod]
    public async Task SocketServerShouldCloseAcceptedClientWhenChannelFactoryThrowsDuringStop()
    {
        var connectedClients = await CreateConnectedTcpClients();
        using var client = connectedClients.Client;
        using var acceptedClient = connectedClients.AcceptedClient;
        using var channelFactoryCalled = new ManualResetEventSlim(false);
        var connected = false;
        SocketServer? socketServer = null;
        socketServer = new SocketServer(_ =>
        {
            socketServer!.Stop();
            channelFactoryCalled.Set();
            throw new InvalidOperationException();
        }, _ => Task.FromResult(acceptedClient));
        socketServer.Connected += (_, _) => connected = true;

        try
        {
            var endpoint = socketServer.Start(DefaultConnection).GetIpEndPoint();

            Assert.IsTrue(channelFactoryCalled.Wait(Timeout));
            Assert.IsTrue(client.Client.Poll(Timeout * 1000, SelectMode.SelectRead));
            Assert.AreEqual(0, client.Client.Available);
            Assert.IsFalse(connected);
            await AssertListenerClosed(endpoint);
        }
        finally
        {
            socketServer.Stop();
        }
    }

    [TestMethod]
    public async Task SocketServerStopInsideConnectedHandlerShouldCloseClientAndChannelOnce()
    {
        var channel = new Mock<ICommunicationChannel>();
        var connectedClients = await CreateConnectedTcpClients();
        using var client = connectedClients.Client;
        using var acceptedClient = connectedClients.AcceptedClient;
        using var disconnected = new ManualResetEventSlim(false);
        var socketServer = new SocketServer(_ => channel.Object, _ => Task.FromResult(acceptedClient));
        socketServer.Connected += (_, _) => socketServer.Stop();
        socketServer.Disconnected += (_, _) => disconnected.Set();

        try
        {
            var endpoint = socketServer.Start(DefaultConnection).GetIpEndPoint();

            Assert.IsTrue(disconnected.Wait(Timeout));
            socketServer.Stop();
            Assert.IsTrue(client.Client.Poll(Timeout * 1000, SelectMode.SelectRead));
            Assert.AreEqual(0, client.Client.Available);
            channel.Verify(value => value.Dispose(), Times.Once);
            await AssertListenerClosed(endpoint);
        }
        finally
        {
            socketServer.Stop();
        }
    }

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public async Task SocketServerShouldNotPublishChannelWhenFactoryReturnsAfterStop(bool stopInsideFactory)
    {
        var channel = new Mock<ICommunicationChannel>();
        var connectedClients = await CreateConnectedTcpClients();
        using var client = connectedClients.Client;
        using var acceptedClient = connectedClients.AcceptedClient;
        using var factoryEntered = new ManualResetEventSlim(false);
        using var releaseFactory = new ManualResetEventSlim(false);
        var connectedCount = 0;
        var disconnectedCount = 0;
        var stopCompletedDuringDisposal = false;
        SocketServer? socketServer = null;
        socketServer = new SocketServer(_ =>
        {
            factoryEntered.Set();
            if (stopInsideFactory)
            {
                socketServer!.Stop();
            }
            else
            {
                releaseFactory.Wait(Timeout * 2);
            }

            return channel.Object;
        }, _ => Task.FromResult(acceptedClient));
        channel.Setup(value => value.Dispose()).Callback(() =>
        {
            var stopTask = Task.Run(socketServer.Stop);
            stopCompletedDuringDisposal = stopTask.Wait(Timeout);
        });
        socketServer.Connected += (_, _) => Interlocked.Increment(ref connectedCount);
        socketServer.Disconnected += (_, _) => Interlocked.Increment(ref disconnectedCount);
        var startTask = Task.Run(() => socketServer.Start(DefaultConnection));

        try
        {
            Assert.IsTrue(factoryEntered.Wait(Timeout));
            if (!stopInsideFactory)
            {
                socketServer.Stop();
            }

            releaseFactory.Set();
            Assert.IsTrue(startTask.Wait(Timeout));
            var endpoint = (await startTask).GetIpEndPoint();

            Assert.AreEqual(0, connectedCount, "A channel returned after Stop must not be published.");
            Assert.AreEqual(0, disconnectedCount, "An unpublished channel must not enter the message loop.");
            channel.Verify(value => value.Dispose(), Times.Once);
            Assert.IsTrue(stopCompletedDuringDisposal, "Channel disposal must run outside the state lock.");
            channel.Verify(value => value.NotifyDataAvailable(It.IsAny<CancellationToken>()), Times.Never);
            Assert.IsTrue(client.Client.Poll(Timeout * 1000, SelectMode.SelectRead));
            Assert.AreEqual(0, client.Client.Available);
            socketServer.Stop();
            channel.Verify(value => value.Dispose(), Times.Once);
            await AssertListenerClosed(endpoint);
        }
        finally
        {
            releaseFactory.Set();
            socketServer.Stop();
            Assert.IsTrue(startTask.Wait(Timeout));
        }
    }

    private static async Task AssertListenerClosed(IPEndPoint endpoint)
    {
        using var client = new TcpClient();
        var connectionFailed = false;
        try
        {
            await client.ConnectAsync(endpoint.Address, endpoint.Port);
        }
        catch (SocketException)
        {
            connectionFailed = true;
        }

        Assert.IsTrue(connectionFailed, "The listener still accepted a connection after Stop.");
    }

    private static async Task<(TcpClient Client, TcpClient AcceptedClient)> CreateConnectedTcpClients()
    {
        var client = new TcpClient();
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        try
        {
            var accepted = listener.AcceptTcpClientAsync();
            await client.ConnectAsync(IPAddress.Loopback, ((IPEndPoint)listener.LocalEndpoint).Port);
            return (client, await accepted);
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
