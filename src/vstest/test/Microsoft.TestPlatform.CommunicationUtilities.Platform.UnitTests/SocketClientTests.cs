// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.TestPlatform.CommunicationUtilities.PlatformTests;

[TestClass]
public class SocketClientTests : SocketTestsBase, IDisposable
{
    private readonly TcpListener _tcpListener;

    private readonly ICommunicationEndPoint _socketClient;

    private TcpClient? _tcpClient;

    public SocketClientTests()
    {
        _socketClient = new SocketClient();

        var endpoint = new IPEndPoint(IPAddress.Loopback, 0);
        _tcpListener = new TcpListener(endpoint);
    }

    protected override TcpClient? Client => _tcpClient;

    public void Dispose()
    {
        _socketClient.Stop();
        _tcpListener.Stop();
        // tcpClient.Close() calls tcpClient.Dispose().
        _tcpClient?.Close();
        GC.SuppressFinalize(this);
    }

    [TestMethod]
    public void SocketClientStartShouldConnectToLoopbackOnGivenPort()
    {
        var connectionInfo = StartLocalServer();

        _socketClient.Start(connectionInfo);

#pragma warning disable MSTEST0049 // AcceptTcpClientAsync(CancellationToken) unavailable on .NET Framework
        var acceptClientTask = _tcpListener.AcceptTcpClientAsync();
        Assert.IsTrue(acceptClientTask.Wait(Timeout));
#pragma warning restore MSTEST0049
        using var client = acceptClientTask.Result;
        Assert.IsTrue(client.Connected);
    }

    [TestMethod]
    [Ignore]
    public void SocketClientStartShouldThrowIfServerIsNotListening()
    {
        var dummyConnectionInfo = "5345";

        _socketClient.Start(dummyConnectionInfo);

        var exceptionThrown = false;
        try
        {
            _socketClient.Start(dummyConnectionInfo);
        }
        catch (PlatformNotSupportedException)
        {
            // Thrown on unix
            exceptionThrown = true;
        }
        catch (SocketException)
        {
            exceptionThrown = true;
        }

        Assert.IsTrue(exceptionThrown);
    }

    [TestMethod]
    public void SocketClientStopShouldRaiseClientDisconnectedEventOnClientDisconnection()
    {
        using var waitEvent = SetupClientDisconnect(out ICommunicationChannel? _);

        // Close the communication from client side
        _socketClient.Stop();

        Assert.IsTrue(waitEvent.Wait(Timeout, TestContext.CancellationToken));
    }

    [TestMethod]
    public void SocketClientShouldRaiseClientDisconnectedEventIfConnectionIsBroken()
    {
        using var waitEvent = SetupClientDisconnect(out ICommunicationChannel? _);

        // Close the communication from server side
        _tcpClient?.GetStream().Dispose();
        // tcpClient.Close() calls tcpClient.Dispose().
        _tcpClient?.Close();
        Assert.IsTrue(waitEvent.Wait(Timeout, TestContext.CancellationToken));
    }

    [TestMethod]
    public void SocketClientStopShouldStopCommunication()
    {
        using var waitEvent = SetupClientDisconnect(out ICommunicationChannel? _);

        // Close the communication from socket client side
        _socketClient.Stop();

        // Validate that the server side observes the closed connection.
        Assert.IsTrue(waitEvent.Wait(Timeout, TestContext.CancellationToken));
        Assert.IsTrue(Client!.Client.Poll(Timeout * 1000, SelectMode.SelectRead));
        Assert.AreEqual(0, Client.Client.Available);
    }

    [TestMethod]
    public void SocketClientStopShouldCloseChannel()
    {
        var channel = new Mock<ICommunicationChannel>();
        var socketClient = new TestSocketClient(_ => channel.Object);
        using ManualResetEventSlim waitEvent = new(false);
        socketClient.Connected += (sender, eventArgs) => waitEvent.Set();
        var connectionInfo = StartLocalServer();
        socketClient.Start(connectionInfo);

#pragma warning disable MSTEST0049 // Use 'TestContext.CancellationToken' - AcceptTcpClientAsync/Wait overloads unavailable on .NET Framework
        var acceptClientTask = _tcpListener.AcceptTcpClientAsync();
        Assert.IsTrue(acceptClientTask.Wait(Timeout, TestContext.CancellationToken));
#pragma warning restore MSTEST0049
        _tcpClient = acceptClientTask.Result;
        Assert.IsTrue(waitEvent.Wait(Timeout, TestContext.CancellationToken));
        waitEvent.Reset();
        socketClient.Disconnected += (sender, eventArgs) => waitEvent.Set();

        socketClient.Stop();

        Assert.IsTrue(waitEvent.Wait(Timeout, TestContext.CancellationToken));
        channel.Verify(communicationChannel => communicationChannel.Dispose(), Times.Once);
    }

    protected override ICommunicationChannel? SetupChannel(out ConnectedEventArgs? connectedEvent)
    {
        ICommunicationChannel? channel = null;
        ConnectedEventArgs? serverConnectedEvent = null;
        using ManualResetEventSlim waitEvent = new(false);
        _socketClient.Connected += (sender, eventArgs) =>
        {
            serverConnectedEvent = eventArgs;
            channel = eventArgs.Channel;
            waitEvent.Set();
        };

        var connectionInfo = StartLocalServer();
        _socketClient.Start(connectionInfo);

#pragma warning disable MSTEST0049 // Use 'TestContext.CancellationToken' - AcceptTcpClientAsync/Wait overloads unavailable on .NET Framework
        var acceptClientTask = _tcpListener.AcceptTcpClientAsync();
        Assert.IsTrue(acceptClientTask.Wait(Timeout, TestContext.CancellationToken));
#pragma warning restore MSTEST0049
        _tcpClient = acceptClientTask.Result;
        Assert.IsTrue(waitEvent.Wait(Timeout, TestContext.CancellationToken));

        connectedEvent = serverConnectedEvent;
        return channel;
    }

    private ManualResetEventSlim SetupClientDisconnect(out ICommunicationChannel? channel)
    {
        var waitEvent = new ManualResetEventSlim(false);
        _socketClient.Disconnected += (s, e) => waitEvent.Set();
        channel = SetupChannel(out ConnectedEventArgs? _);
        channel!.MessageReceived.Subscribe((sender, args) =>
        {
        });
        return waitEvent;
    }

    private string StartLocalServer()
    {
        _tcpListener.Start();

        return ((IPEndPoint)_tcpListener.LocalEndpoint).ToString();
    }

    private sealed class TestSocketClient(Func<Stream, ICommunicationChannel> channelFactory) : SocketClient(channelFactory);
}
