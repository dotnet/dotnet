// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

/// <summary>
/// Facilitates communication using sockets
/// </summary>
[SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "Would cause a breaking change if users are inheriting this class and implement IDisposable")]
public class SocketCommunicationManager : ICommunicationManager, IConnectionValidationConfigurable, IConnectionAuthenticationResponder
{
    /// <summary>
    /// The server stream read timeout constant (in microseconds).
    /// </summary>
    private const int STREAMREADTIMEOUT = 1000 * 1000;

    /// <summary>
    /// TCP Listener to host TCP channel and listen
    /// </summary>
    private TcpListener? _tcpListener;

    /// <summary>
    /// TCP Client that can connect to a TCP listener
    /// </summary>
    private TcpClient? _tcpClient;

    private TcpClient? _acceptedTcpClient;

    /// <summary>
    /// Binary Writer to write to channel stream
    /// </summary>
    private BinaryWriter? _binaryWriter;

    /// <summary>
    /// Binary reader to read from channel stream
    /// </summary>
    private BinaryReader? _binaryReader;

    /// <summary>
    /// Serializer for the data objects
    /// </summary>
    private readonly IDataSerializer _dataSerializer;

    /// <summary>
    /// Event used to maintain client connection state
    /// </summary>
    private readonly ManualResetEvent _clientConnectedEvent = new(false);

    /// <summary>
    /// Event used to maintain client connection state
    /// </summary>
    private readonly ManualResetEvent _clientConnectionAcceptedEvent = new(false);

    private readonly ManualResetEvent _connectionValidationFailedEvent = new(false);

    /// <summary>
    /// Sync object for sending messages
    /// SendMessage over socket channel is NOT thread-safe
    /// </summary>
    private readonly object _sendSyncObject = new();

    /// <summary>
    /// Sync object for receiving messages
    /// </summary>
    private readonly object _receiveSyncObject = new();

    private Socket? _socket;

    private readonly ConnectionValidator _connectionValidator;
    private CancellationTokenSource _connectionValidationCancellation = new();
    private string? _connectionSecretsResponder;

    /// <summary>
    /// Initializes a new instance of the <see cref="SocketCommunicationManager"/> class.
    /// </summary>
    public SocketCommunicationManager()
        : this(JsonDataSerializer.Instance)
    {
    }

    internal SocketCommunicationManager(IDataSerializer dataSerializer)
    {
        _dataSerializer = dataSerializer;
        _connectionValidator = new ConnectionValidator();
    }

    string? IConnectionValidationConfigurable.ConnectionValidationError => _connectionValidator.Error;

    void IConnectionValidationConfigurable.RequireConnectionValidation()
        => _connectionValidator.RequireValidation();

    void IConnectionValidationConfigurable.UseConnectionSecrets(string connectionSecrets)
        => _connectionValidator.UseConnectionSecrets(connectionSecrets);

    void IConnectionValidationConfigurable.CannotValidateConnection(string error)
        => _connectionValidator.CannotValidate(error);
    void IConnectionAuthenticationResponder.UseConnectionSecretsAsChild(string connectionSecrets)
    {
        if (string.IsNullOrWhiteSpace(connectionSecrets))
        {
            throw new ArgumentException("The connection secrets cannot be empty.", nameof(connectionSecrets));
        }

        _connectionSecretsResponder = connectionSecrets;
    }

    #region ServerMethods

    /// <summary>
    /// Host TCP Socket Server and start listening
    /// </summary>
    /// <param name="endpoint">End point where server is hosted</param>
    /// <returns>Port of the listener</returns>
    public IPEndPoint HostServer(IPEndPoint endpoint)
    {
        if (_connectionValidationCancellation.IsCancellationRequested)
        {
            _connectionValidationCancellation.Dispose();
            _connectionValidationCancellation = new CancellationTokenSource();
        }

        _connectionValidationFailedEvent.Reset();
        _tcpListener = new TcpListener(endpoint);
        _tcpListener.Start();
        EqtTrace.Info("Listening on Endpoint : {0}", (IPEndPoint)_tcpListener.LocalEndpoint);

        return (IPEndPoint)_tcpListener.LocalEndpoint;
    }

    /// <summary>
    /// Accepts client async
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task AcceptClientAsync()
    {
        var listener = _tcpListener;
        if (listener is null || _connectionValidationCancellation.IsCancellationRequested)
        {
            return;
        }

        _clientConnectedEvent.Reset();

        TcpClient client;
        try
        {
            client = await listener.AcceptTcpClientAsync();
        }
        catch (ObjectDisposedException) when (_connectionValidationCancellation.IsCancellationRequested)
        {
            return;
        }
        catch (SocketException) when (_connectionValidationCancellation.IsCancellationRequested)
        {
            return;
        }

        client.Client.NoDelay = true;

        // Accept one client, as before authentication was added. A rejected peer ends the
        // connection attempt; the same secrets must not be offered to another candidate.
        var validationResult = ValidateConnection(client);
        if (!validationResult.Succeeded)
        {
            client.Close();
            EqtTrace.Warning(
                "SocketCommunicationManager.AcceptClientAsync: Rejected a TCP connection: {0}",
                validationResult.Error);
            StopServer();
            _connectionValidationFailedEvent.Set();
            return;
        }

        if (_connectionValidationCancellation.IsCancellationRequested)
        {
            client.Close();
            return;
        }

        _acceptedTcpClient = client;
        _socket = client.Client;

        // Using Buffered stream only in case of write, and Network stream in case of read.
        var bufferedStream = new PlatformStream().CreateBufferedStream(client.GetStream(), SocketConstants.BufferSize);
        var networkStream = client.GetStream();
        _binaryReader = new BinaryReader(networkStream);
        _binaryWriter = new BinaryWriter(bufferedStream);

        _clientConnectedEvent.Set();
        EqtTrace.Info("Using the buffer size of {0} bytes", SocketConstants.BufferSize);
        EqtTrace.Info("Accepted and validated client connection");
    }

    /// <summary>
    /// Waits for Client Connection
    /// </summary>
    /// <param name="clientConnectionTimeout">Time to Wait for the connection</param>
    /// <returns>True if Client is connected, false otherwise</returns>
    public bool WaitForClientConnection(int clientConnectionTimeout)
    {
        var stopWatch = Stopwatch.StartNew();
        var waitResult = WaitHandle.WaitAny(
            [_clientConnectedEvent, _connectionValidationFailedEvent],
            clientConnectionTimeout);
        var result = waitResult == 0;
        EqtTrace.Verbose("SocketCommunicationManager.WaitForClientConnection took: {0} ms, with {1} ms timeout, and finished with {2}.", stopWatch.ElapsedMilliseconds, clientConnectionTimeout, result);

        return result;
    }

    /// <summary>
    /// Stop Listener
    /// </summary>
    public void StopServer()
    {
        _connectionValidationCancellation.Cancel();
        _tcpListener?.Stop();
        _tcpListener = null;
        _acceptedTcpClient?.Close();
        _acceptedTcpClient = null;
        _binaryReader?.Dispose();
        _binaryWriter?.Dispose();
    }

    #endregion

    #region ClientMethods

    /// <summary>
    /// Connects to server async
    /// </summary>
    /// <param name="endpoint">EndPointAddress for client to connect</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task SetupClientAsync(IPEndPoint endpoint)
    {
        // TODO: pass cancellation token, if user cancels the operation, so we don't wait 50 secs to connect
        // for now added a check for validation of this.tcpclient
        if (_connectionValidationCancellation.IsCancellationRequested)
        {
            _connectionValidationCancellation.Dispose();
            _connectionValidationCancellation = new CancellationTokenSource();
        }

        _clientConnectionAcceptedEvent.Reset();
        _connectionValidationFailedEvent.Reset();
        EqtTrace.Info("Trying to connect to server on socket : {0} ", endpoint);
        _tcpClient = new TcpClient { NoDelay = true };
        _socket = _tcpClient.Client;

        Stopwatch watch = new();
        watch.Start();
        var connectionTimeout = EnvironmentHelper.GetConnectionTimeout() * 1000;
        do
        {
            try
            {
                EqtTrace.Verbose("SocketCommunicationManager : SetupClientAsync : Attempting to connect to the server.");
                await _tcpClient.ConnectAsync(endpoint.Address, endpoint.Port);

                if (_tcpClient.Connected)
                {
                    var validationResult = ValidateConnection(_tcpClient);
                    if (!validationResult.Succeeded)
                    {
                        EqtTrace.Error(
                            "SocketCommunicationManager.SetupClientAsync: TCP peer validation failed: {0}",
                            validationResult.Error);
                        _tcpClient.Close();
                        _connectionValidationFailedEvent.Set();
                        return;
                    }

                    // Using Buffered stream only in case of write, and Network stream in case of read.
                    var bufferedStream = new PlatformStream().CreateBufferedStream(_tcpClient.GetStream(), SocketConstants.BufferSize);
                    var networkStream = _tcpClient.GetStream();
                    _binaryReader = new BinaryReader(networkStream);
                    _binaryWriter = new BinaryWriter(bufferedStream);

                    EqtTrace.Info("Connected to the server successfully ");
                    EqtTrace.Info("Using the buffer size of {0} bytes", SocketConstants.BufferSize);

                    _clientConnectionAcceptedEvent.Set();
                }
            }
            catch (Exception ex)
            {
                EqtTrace.Error("Connection Failed with error {0}, retrying", ex.ToString());
            }
        }
        while ((_tcpClient != null) && !_tcpClient.Connected && watch.ElapsedMilliseconds < connectionTimeout);
    }

    /// <summary>
    /// Waits for server to be connected
    /// Whoever creating the client and trying to connect to a server
    /// should use this method to wait for connection to be established with server
    /// </summary>
    /// <param name="connectionTimeout">Time to wait for the connection</param>
    /// <returns>True, if Server got a connection from client</returns>
    public bool WaitForServerConnection(int connectionTimeout)
    {
        var stopWatch = Stopwatch.StartNew();
        var waitResult = WaitHandle.WaitAny(
            [_clientConnectionAcceptedEvent, _connectionValidationFailedEvent],
            connectionTimeout);
        var result = waitResult == 0;
        EqtTrace.Verbose("SocketCommunicationManager.WaitForServerConnection took: {0} ms, with {1} ms timeout, and finished with {2}.", stopWatch.ElapsedMilliseconds, connectionTimeout, result);

        return result;
    }

    /// <summary>
    /// Stop Listener
    /// </summary>
    public void StopClient()
    {
        _connectionValidationCancellation.Cancel();
        // tcpClient.Close() calls tcpClient.Dispose().
        _tcpClient?.Close();
        _tcpClient = null;
        _binaryReader?.Dispose();
        _binaryWriter?.Dispose();
    }

    #endregion

    /// <summary>
    /// Writes message to the binary writer.
    /// </summary>
    /// <param name="messageType">Type of Message to be sent, for instance TestSessionStart</param>
    public void SendMessage(string messageType)
    {
        var serializedObject = _dataSerializer.SerializeMessage(messageType);
        WriteAndFlushToChannel(serializedObject);
    }

    /// <summary>
    ///  Writes message to the binary writer with payload
    /// </summary>
    /// <param name="messageType">Type of Message to be sent, for instance TestSessionStart</param>
    /// <param name="payload">payload to be sent</param>
    public void SendMessage(string messageType, object payload)
    {
        var rawMessage = _dataSerializer.SerializePayload(messageType, payload);
        WriteAndFlushToChannel(rawMessage);
    }

    /// <summary>
    ///  Writes message to the binary writer with payload
    /// </summary>
    /// <param name="messageType">Type of Message to be sent, for instance TestSessionStart</param>
    /// <param name="payload">payload to be sent</param>
    /// <param name="version">version to be sent</param>
    public void SendMessage(string messageType, object payload, int version)
    {
        var rawMessage = _dataSerializer.SerializePayload(messageType, payload, version);
        WriteAndFlushToChannel(rawMessage);
    }

    /// <summary>
    /// Send serialized raw message
    /// </summary>
    /// <param name="rawMessage">serialized message</param>
    public void SendRawMessage(string rawMessage)
    {
        WriteAndFlushToChannel(rawMessage);
    }

    /// <summary>
    /// Reads message from the binary reader
    /// </summary>
    /// <returns>Returns message read from the binary reader</returns>
    public Message? ReceiveMessage()
    {
        var rawMessage = ReceiveRawMessage();
        return !rawMessage.IsNullOrEmpty()
            ? _dataSerializer.DeserializeMessage(rawMessage)
            : null;
    }

    /// <summary>
    /// Reads message from the binary reader using read timeout
    /// </summary>
    /// <param name="cancellationToken">
    /// The cancellation Token.
    /// </param>
    /// <returns>
    /// Returns message read from the binary reader
    /// </returns>
    public async Task<Message?> ReceiveMessageAsync(CancellationToken cancellationToken)
    {
        var rawMessage = await ReceiveRawMessageAsync(cancellationToken);
        return !rawMessage.IsNullOrEmpty()
            ? _dataSerializer.DeserializeMessage(rawMessage)
            : null;
    }

    /// <summary>
    /// Reads message from the binary reader
    /// </summary>
    /// <returns> Raw message string </returns>
    public string? ReceiveRawMessage()
    {
        lock (_receiveSyncObject)
        {
            // Reading message on binaryreader is not thread-safe
            return _binaryReader?.ReadString();
        }
    }

    /// <summary>
    /// Reads message from the binary reader using read timeout
    /// </summary>
    /// <param name="cancellationToken">
    /// The cancellation Token.
    /// </param>
    /// <returns>
    /// Raw message string
    /// </returns>
    public async Task<string?> ReceiveRawMessageAsync(CancellationToken cancellationToken)
    {
        var str = await Task.Run(() => TryReceiveRawMessage(cancellationToken));
        return str;
    }

    /// <summary>
    /// Deserializes the Message into actual TestPlatform objects
    /// </summary>
    /// <typeparam name="T"> The type of object to deserialize to. </typeparam>
    /// <param name="message"> Message object </param>
    /// <returns> TestPlatform object </returns>
    public T? DeserializePayload<T>(Message message)
    {
        return _dataSerializer.DeserializePayload<T>(message);
    }

    private PeerProcessValidationResult ValidateConnection(TcpClient client)
    {
        if (_connectionSecretsResponder is not null)
        {
            return ConnectionHandshake.AuthenticateAsChild(
                client,
                _connectionSecretsResponder,
                EnvironmentHelper.GetConnectionTimeout() * 1000,
                _connectionValidationCancellation.Token);
        }

        return _connectionValidator.ValidateAsParent(
            client,
            _connectionValidationCancellation.Token);
    }

    private string? TryReceiveRawMessage(CancellationToken cancellationToken)
    {
        string? str = null;
        bool success = false;

        // Set read timeout to avoid blocking receive raw message
        while (!cancellationToken.IsCancellationRequested && !success)
        {
            try
            {
                if (_socket is null)
                {
                    EqtTrace.Error("SocketCommunicationManager.TryReceiveRawMessage: Socket is null");
                    break;
                }

                if (_socket.Poll(STREAMREADTIMEOUT, SelectMode.SelectRead) == true)
                {
                    str = ReceiveRawMessage();
                    success = true;
                }
            }
            catch (IOException ioException)
            {
                if (ioException.InnerException is SocketException socketException
                    && socketException.SocketErrorCode == SocketError.TimedOut)
                {
                    EqtTrace.Info(
                        "SocketCommunicationManager.ReceiveMessage: failed to receive message because read timeout {0}",
                        ioException);
                }
                else
                {
                    EqtTrace.Error(
                        "SocketCommunicationManager.ReceiveMessage: failed to receive message {0}",
                        ioException);
                    break;
                }
            }
            catch (Exception exception)
            {
                EqtTrace.Error(
                    "SocketCommunicationManager.ReceiveMessage: failed to receive message {0}",
                    exception);
                break;
            }
        }

        return str;
    }

    /// <summary>
    /// Writes the data on socket and flushes the buffer
    /// </summary>
    /// <param name="rawMessage">message to write</param>
    private void WriteAndFlushToChannel(string rawMessage)
    {
        // Writing Message on binarywriter is not Thread-Safe
        // Need to sync one by one to avoid buffer corruption
        lock (_sendSyncObject)
        {
            _binaryWriter?.Write(rawMessage);
            _binaryWriter?.Flush();
        }
    }
}
