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
    private readonly Func<Stream, ICommunicationChannel> _channelFactory;

    private ICommunicationChannel? _channel;
    private TcpListener? _tcpListener;
    private TcpClient? _tcpClient;
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
    {
        // Used to cancel the message loop
        _cancellation = new CancellationTokenSource();

        _channelFactory = channelFactory;
    }

    /// <inheritdoc />
    public event EventHandler<ConnectedEventArgs>? Connected;

    /// <inheritdoc />
    public event EventHandler<DisconnectedEventArgs>? Disconnected;

    public string? Start(string endPoint)
    {
        try
        {
            _tcpListener = new TcpListener(endPoint.GetIpEndPoint());

            _tcpListener.Start();

            _endPoint = _tcpListener.LocalEndpoint.ToString();
            EqtTrace.Info("SocketServer.Start: Listening on endpoint : {0}", _endPoint);

            // Serves a single client at the moment. An error in connection, or message loop just
            // terminates the entire server.
            _tcpListener.AcceptTcpClientAsync().ContinueWith(t => OnClientConnected(t.Result));
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
        if (!_stopped)
        {
            EqtTrace.Info("SocketServer.Stop: Cancellation requested. Stopping message loop.");
            _cancellation.Cancel();
        }
    }

    private void OnClientConnected(TcpClient client)
    {
        _tcpClient = client;
        _tcpClient.Client.NoDelay = true;

        if (Connected == null)
        {
            return;
        }

        _channel = _channelFactory(_tcpClient.GetStream());
        Connected.SafeInvoke(this, new ConnectedEventArgs(_channel), "SocketServer: ClientConnected");

        EqtTrace.Verbose("SocketServer.OnClientConnected: Client connected for endPoint: {0}, starting MessageLoopAsync:", _endPoint);

        // Start the message loop
        Task.Run(() => _tcpClient.MessageLoopAsync(_channel, error => StopOnError(error), _cancellation.Token)).ConfigureAwait(false);
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
