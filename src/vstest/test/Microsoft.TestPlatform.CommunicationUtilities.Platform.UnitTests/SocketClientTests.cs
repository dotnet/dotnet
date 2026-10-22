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

namespace Microsoft.TestPlatform.CommunicationUtilities.PlatformTests;

[TestClass]
[Ignore("Flaky tests")]
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
        // tcpClient.Close() calls tcpClient.Dispose().
        _tcpClient?.Close();
        GC.SuppressFinalize(this);
    }

    [TestMethod]
    public void SocketClientStartShouldConnectToLoopbackOnGivenPort()
    {
        var connectionInfo = StartLocalServer();

        _socketClient.Start(connectionInfo);

        var acceptClientTask = _tcpListener.AcceptTcpClientAsync();
        Assert.IsTrue(acceptClientTask.Wait(Timeout));
        Assert.IsTrue(acceptClientTask.Result.Connected);
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
        var waitEvent = SetupClientDisconnect(out ICommunicationChannel? _);

        // Close the communication from client side
        _socketClient.Stop();

        Assert.IsTrue(waitEvent.WaitOne(Timeout));
    }

    [TestMethod]
    public void SocketClientShouldRaiseClientDisconnectedEventIfConnectionIsBroken()
    {
        var waitEvent = SetupClientDisconnect(out ICommunicationChannel? _);

        // Close the communication from server side
        _tcpClient?.GetStream().Dispose();
        // tcpClient.Close() calls tcpClient.Dispose().
        _tcpClient?.Close();
        Assert.IsTrue(waitEvent.WaitOne(Timeout));
    }

    [TestMethod]
    public void SocketClientStopShouldStopCommunication()
    {
        var waitEvent = SetupClientDisconnect(out ICommunicationChannel? _);

        // Close the communication from socket client side
        _socketClient.Stop();

        // Validate that write on server side fails
        waitEvent.WaitOne(Timeout);
        Assert.ThrowsException<IOException>(() => WriteData(Client!));
    }

    [TestMethod]
    public void SocketClientStopShouldCloseChannel()
    {
        var waitEvent = SetupClientDisconnect(out ICommunicationChannel? channel);

        _socketClient.Stop();

        waitEvent.WaitOne(Timeout);
        Assert.ThrowsException<CommunicationException>(() => channel!.Send(Dummydata));
    }

    protected override ICommunicationChannel? SetupChannel(out ConnectedEventArgs? connectedEvent)
    {
        ICommunicationChannel? channel = null;
        ConnectedEventArgs? serverConnectedEvent = null;
        ManualResetEvent waitEvent = new(false);
        _socketClient.Connected += (sender, eventArgs) =>
        {
            serverConnectedEvent = eventArgs;
            channel = eventArgs.Channel;
            waitEvent.Set();
        };

        var connectionInfo = StartLocalServer();
        _socketClient.Start(connectionInfo);

        var acceptClientTask = _tcpListener.AcceptTcpClientAsync();
        if (acceptClientTask.Wait(TimeSpan.FromMilliseconds(1000)))
        {
            _tcpClient = acceptClientTask.Result;
            waitEvent.WaitOne(1000);
        }

        connectedEvent = serverConnectedEvent;
        return channel;
    }

    private ManualResetEvent SetupClientDisconnect(out ICommunicationChannel? channel)
    {
        var waitEvent = new ManualResetEvent(false);
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
}
