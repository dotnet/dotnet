// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
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
    private const int Timeout = 10000;
    private readonly string _defaultConnection = IPAddress.Loopback + ":0";

    public TestContext TestContext { get; set; } = null!;

    [TestMethod]
    public void SocketServerStartShouldThrowAfterStop()
    {
        var socketServer = new SocketServer();
        try
        {
            socketServer.Stop();

            Assert.ThrowsExactly<ObjectDisposedException>(() => socketServer.Start(_defaultConnection));
        }
        finally
        {
            socketServer.Stop();
        }
    }

    [TestMethod]
    public async Task SocketServerStopShouldNotWaitForConnectedHandlerWhenAcceptCompletesSynchronously()
    {
        var connectedClients = await CreateConnectedTcpClients();
        using var tcpClient = connectedClients.Client;
        using var acceptedClient = connectedClients.AcceptedClient;
        var channel = new Mock<ICommunicationChannel>();
        var socketServer = new SocketServer(_ => channel.Object, _ => Task.FromResult(acceptedClient));
        using var handlerEntered = new ManualResetEventSlim(false);
        using var handlerExited = new ManualResetEventSlim(false);
        using var releaseHandler = new ManualResetEventSlim(false);
        socketServer.Connected += (_, _) =>
        {
            handlerEntered.Set();
            releaseHandler.Wait(Timeout * 2, TestContext.CancellationToken);
            handlerExited.Set();
        };

        var startTask = Task.Run(() => socketServer.Start(_defaultConnection), TestContext.CancellationToken);
        try
        {
            try
            {
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
            await startTask;
        }
        finally
        {
            releaseHandler.Set();
            socketServer.Stop();
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
        socketServer.Connected += (_, _) => { };

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
    [DataRow(true)]
    [DataRow(false)]
    public async Task SocketServerShouldNotPublishChannelWhenFactoryReturnsAfterStop(bool stopInsideFactory)
    {
        var connectedClients = await CreateConnectedTcpClients();
        using var tcpClient = connectedClients.Client;
        using var acceptedClient = connectedClients.AcceptedClient;
        using var factoryEntered = new ManualResetEventSlim(false);
        using var releaseFactory = new ManualResetEventSlim(false);
        var channel = new Mock<ICommunicationChannel>();
        var connectedCount = 0;
        var disconnectedCount = 0;
        var stopCompletedDuringDisposal = false;
        SocketServer? socketServer = null;
        socketServer = new SocketServer(
            _ =>
            {
                factoryEntered.Set();
                if (stopInsideFactory)
                {
                    socketServer!.Stop();
                }
                else
                {
                    releaseFactory.Wait(Timeout * 2, TestContext.CancellationToken);
                }

                return channel.Object;
            },
            _ => Task.FromResult(acceptedClient));
        channel.Setup(communicationChannel => communicationChannel.Dispose()).Callback(() =>
        {
            var stopTask = Task.Run(socketServer.Stop, TestContext.CancellationToken);
            stopCompletedDuringDisposal = stopTask.Wait(Timeout, TestContext.CancellationToken);
        });
        socketServer.Connected += (_, _) => Interlocked.Increment(ref connectedCount);
        socketServer.Disconnected += (_, _) => Interlocked.Increment(ref disconnectedCount);
        var startTask = Task.Run(() => socketServer.Start(_defaultConnection), TestContext.CancellationToken);

        try
        {
            Assert.IsTrue(factoryEntered.Wait(Timeout, TestContext.CancellationToken));
            if (!stopInsideFactory)
            {
                socketServer.Stop();
            }

            releaseFactory.Set();
            Assert.IsTrue(startTask.Wait(Timeout, TestContext.CancellationToken));
            await startTask;

            Assert.AreEqual(0, connectedCount, "A channel returned after Stop must not be published.");
            Assert.AreEqual(0, disconnectedCount, "An unpublished channel must not enter the message loop.");
            channel.Verify(communicationChannel => communicationChannel.Dispose(), Times.Once);
            Assert.IsTrue(stopCompletedDuringDisposal, "Channel disposal must run outside the state lock.");
            channel.Verify(communicationChannel => communicationChannel.NotifyDataAvailable(It.IsAny<CancellationToken>()), Times.Never);
            Assert.IsTrue(tcpClient.Client.Poll(Timeout * 1000, SelectMode.SelectRead));
            Assert.AreEqual(0, tcpClient.Client.Available);
            socketServer.Stop();
            channel.Verify(communicationChannel => communicationChannel.Dispose(), Times.Once);
        }
        finally
        {
            releaseFactory.Set();
            socketServer.Stop();
            Assert.IsTrue(startTask.Wait(Timeout, TestContext.CancellationToken));
        }
    }

    [TestMethod]
    public async Task SocketServerShouldCloseClientAcceptedAfterStopWithoutCreatingChannel()
    {
        var connectedClients = await CreateConnectedTcpClients();
        using var tcpClient = connectedClients.Client;
        using var acceptedClient = connectedClients.AcceptedClient;
        var acceptCompletion = new TaskCompletionSource<TcpClient>();
        var channel = new Mock<ICommunicationChannel>();
        var factoryCount = 0;
        var connectedCount = 0;
        var socketServer = new SocketServer(
            _ =>
            {
                Interlocked.Increment(ref factoryCount);
                return channel.Object;
            },
            _ => acceptCompletion.Task);
        socketServer.Connected += (_, _) => Interlocked.Increment(ref connectedCount);

        try
        {
            socketServer.Start(_defaultConnection);
            socketServer.Stop();
            acceptCompletion.SetResult(acceptedClient);

            Assert.IsTrue(tcpClient.Client.Poll(Timeout * 1000, SelectMode.SelectRead));
            Assert.AreEqual(0, tcpClient.Client.Available);
            Assert.AreEqual(0, factoryCount);
            Assert.AreEqual(0, connectedCount);
            channel.Verify(communicationChannel => communicationChannel.Dispose(), Times.Never);
        }
        finally
        {
            socketServer.Stop();
            acceptCompletion.TrySetResult(acceptedClient);
        }
    }

    private static async Task<(TcpClient Client, TcpClient AcceptedClient)> CreateConnectedTcpClients()
    {
        var client = new TcpClient();
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        try
        {
#pragma warning disable MSTEST0049 // CancellationToken overloads are unavailable on .NET Framework.
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
}
