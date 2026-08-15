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
/// Communication server implementation over sockets.
/// </summary>
[SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "Would cause a breaking change if users are inheriting this class and implement IDisposable")]
public class SocketServer : ICommunicationEndPoint
{
    private readonly CancellationTokenSource _cancellation;
    private readonly Func<TcpListener, Task<TcpClient>> _acceptClientAsync;
    private readonly Func<Stream, ICommunicationChannel> _channelFactory;
    private readonly object _stateSyncObject = new();

    private ICommunicationChannel? _channel;
    private TcpListener? _tcpListener;
    private TcpClient? _tcpClient;
    private int _stopRequested;
    private bool _stopped;
    private string? _endPoint;

    /// <summary>
    /// Initializes a new instance of the <see cref="SocketServer"/> class.
    /// </summary>
    public SocketServer()
        : this(stream => new LengthPrefixCommunicationChannel(stream))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SocketServer"/> class with given channel
    /// factory implementation.
    /// </summary>
    /// <param name="channelFactory">Factory to create communication channel.</param>
    protected SocketServer(Func<Stream, ICommunicationChannel> channelFactory)
        : this(channelFactory, tcpListener => tcpListener.AcceptTcpClientAsync())
    {
    }

    internal SocketServer(
        Func<Stream, ICommunicationChannel> channelFactory,
        Func<TcpListener, Task<TcpClient>> acceptClientAsync)
    {
        // Used to cancel the message loop
        _cancellation = new CancellationTokenSource();

        _channelFactory = channelFactory;
        _acceptClientAsync = acceptClientAsync;
    }

    /// <inheritdoc />
    public event EventHandler<ConnectedEventArgs>? Connected;

    /// <inheritdoc />
    public event EventHandler<DisconnectedEventArgs>? Disconnected;

    public string? Start(string endPoint)
    {
        try
        {
            TcpListener tcpListener;
            lock (_stateSyncObject)
            {
                if (_stopRequested != 0)
                {
                    throw new ObjectDisposedException(nameof(SocketServer));
                }

                _tcpListener = new TcpListener(endPoint.GetIpEndPoint());

                _tcpListener.Start();
                tcpListener = _tcpListener;

                _endPoint = _tcpListener.LocalEndpoint.ToString();
                EqtTrace.Info("SocketServer.Start: Listening on endpoint : {0}", _endPoint);
            }

            // Serves a single client at the moment. An error in connection, or message loop just
            // terminates the entire server.
            _ = AcceptClientAsync(tcpListener);
            return _endPoint;
        }
        catch (SocketException ex)
        {
            EqtTrace.Error("Failed for address {0}, with: {1}", endPoint, ex);
            throw;
        }
    }

    /// <inheritdoc />
    public void Stop()
    {
        EqtTrace.Info("SocketServer.Stop: Stop server endPoint: {0}", _endPoint);
        lock (_stateSyncObject)
        {
            if (_stopRequested != 0)
            {
                return;
            }

            _stopRequested = 1;
            EqtTrace.Info("SocketServer.Stop: Cancellation requested. Stopping message loop.");
            try
            {
                _cancellation.Cancel();
            }
            catch (ObjectDisposedException)
            {
                // StopOnError disposed the cancellation source concurrently.
            }

            _tcpListener?.Stop();
        }
    }

    private async Task AcceptClientAsync(TcpListener tcpListener)
    {
        TcpClient? client = null;
        try
        {
            try
            {
                client = await _acceptClientAsync(tcpListener).ConfigureAwait(false);
            }
            catch (Exception ex) when (Volatile.Read(ref _stopRequested) != 0 && ex is ObjectDisposedException or SocketException or InvalidOperationException)
            {
                EqtTrace.Verbose("SocketServer.AcceptClientAsync: Listener stopped before a client connected: {0}", ex);
                return;
            }

            lock (_stateSyncObject)
            {
                if (_stopRequested != 0)
                {
                    client.Close();
                    return;
                }

                _tcpClient = client;
            }

            OnClientConnected(client);
        }
        catch (Exception ex)
        {
            EqtTrace.Error("SocketServer.AcceptClientAsync: Failed to accept a client: {0}", ex);
            client?.Close();
            Stop();
        }
    }

    private void OnClientConnected(TcpClient client)
    {
        client.Client.NoDelay = true;

        if (Connected is null)
        {
            return;
        }

        _channel = _channelFactory(client.GetStream());
        Connected.SafeInvoke(this, new ConnectedEventArgs(_channel), "SocketServer: ClientConnected");

        EqtTrace.Verbose("SocketServer.OnClientConnected: Client connected for endPoint: {0}, starting MessageLoopAsync:", _endPoint);

        // Start the message loop
        _ = Task.Run(() => client.MessageLoopAsync(_channel, error => StopOnError(error), _cancellation.Token));
    }

    /// <summary>
    /// Stop the connection when error was encountered. Dispose all communication, and notify subscribers of Disconnected event
    /// that we aborted.
    /// </summary>
    /// <param name="error"></param>
    private void StopOnError(Exception? error)
    {
        EqtTrace.Info("SocketServer.PrivateStop: Stopping server endPoint: {0} error: {1}", _endPoint, error);

        if (_stopped)
        {
            return;
        }

        TPDebug.Assert(_tcpListener is not null, $"{nameof(_tcpListener)} is null");
        TPDebug.Assert(_channel is not null, $"{nameof(_channel)} is null");

        // Do not allow stop to be called multiple times.
        _stopped = true;

        // Stop accepting any other connections
        _tcpListener.Stop();

        // Close the client and dispose the underlying stream
        // tcpClient.Close() calls tcpClient.Dispose().
        _tcpClient?.Close();
        _channel.Dispose();
        _cancellation.Dispose();

        EqtTrace.Info("SocketServer.Stop: Raise disconnected event endPoint: {0} error: {1}", _endPoint, error);
        Disconnected?.SafeInvoke(this, new DisconnectedEventArgs { Error = error }, "SocketServer: ClientDisconnected");
    }
}
