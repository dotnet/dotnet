// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

/// <summary>
/// Communication client implementation over sockets.
/// </summary>
[SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "Would cause a breaking change if users are inheriting this class and implement IDisposable")]
public class SocketClient : ICommunicationEndPoint
{
    private readonly CancellationTokenSource _cancellation;
    private readonly TcpClient _tcpClient;
    private readonly Func<Stream, ICommunicationChannel> _channelFactory;

    private ICommunicationChannel? _channel;
    private bool _stopped;
    private string? _endPoint;

    public SocketClient()
        : this(stream => new LengthPrefixCommunicationChannel(stream))
    {
    }

    protected SocketClient(Func<Stream, ICommunicationChannel> channelFactory)
    {
        // Used to cancel the message loop
        _cancellation = new CancellationTokenSource();
        _stopped = false;

        _tcpClient = new TcpClient { NoDelay = true };
        _channelFactory = channelFactory;
    }

    /// <inheritdoc />
    public event EventHandler<ConnectedEventArgs>? Connected;

    /// <inheritdoc />
    public event EventHandler<DisconnectedEventArgs>? Disconnected;

    /// <inheritdoc />
    public string Start(string endPoint)
    {
        _endPoint = endPoint;
        var ipEndPoint = endPoint.GetIpEndPoint();

        EqtTrace.Info("SocketClient.Start: connecting to server endpoint: {0}", endPoint);

        // Don't start if the endPoint port is zero
        _tcpClient.ConnectAsync(ipEndPoint.Address, ipEndPoint.Port).ContinueWith(OnServerConnected);
        return ipEndPoint.ToString();
    }

    /// <inheritdoc />
    public void Stop()
    {
        EqtTrace.Info("SocketClient.Stop: Stop communication from server endpoint: {0}", _endPoint);

        if (!_stopped)
        {
            EqtTrace.Info("SocketClient: Stop: Cancellation requested. Stopping message loop.");
            _cancellation.Cancel();
        }
    }

    private void OnServerConnected(Task connectAsyncTask)
    {
        EqtTrace.Info("SocketClient.OnServerConnected: connected to server endpoint: {0}", _endPoint);

        if (Connected == null)
        {
            return;
        }

        if (connectAsyncTask.IsFaulted)
        {
            Connected.SafeInvoke(this, new ConnectedEventArgs(connectAsyncTask.Exception), "SocketClient: Server Failed to Connect");
            EqtTrace.Verbose("Unable to connect to server, Exception occurred: {0}", connectAsyncTask.Exception);
            return;
        }

        _channel = _channelFactory(_tcpClient.GetStream());
        Connected.SafeInvoke(this, new ConnectedEventArgs(_channel), "SocketClient: ServerConnected");

        EqtTrace.Verbose("Connected to server, and starting MessageLoopAsync");

        // Start the message loop
        Task.Run(() => _tcpClient.MessageLoopAsync(
                _channel,
                StopOnError,
                _cancellation.Token))
            .ConfigureAwait(false);
    }

    private void StopOnError(Exception? error)
    {
        EqtTrace.Info("SocketClient.PrivateStop: Stop communication from server endpoint: {0}, error:{1}", _endPoint, error);
        // This is here to prevent stack overflow.
        if (!_stopped)
        {
            // Do not allow stop to be called multiple times.
            _stopped = true;

            // Close the client and dispose the underlying stream
            // tcpClient.Close() calls tcpClient.Dispose().
            _tcpClient?.Close();
            _channel?.Dispose();
            _cancellation.Dispose();

            Disconnected?.SafeInvoke(this, new DisconnectedEventArgs(), "SocketClient: ServerDisconnected");
        }
    }
}
