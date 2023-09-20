// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Versioning;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// Represents a bidirectional channel between a NuGet client and a plugin.
    /// </summary>
    public sealed class Connection : IConnection
    {
        private bool _isDisposed;
        private readonly IReceiver _receiver;
        private readonly ISender _sender;
        private readonly IPluginLogger _logger;

        private int _state = (int)ConnectionState.ReadyToConnect;

        /// <summary>
        /// The connection state.
        /// </summary>
        public ConnectionState State => (ConnectionState)_state;

        /// <summary>
        /// Occurs when an unrecoverable fault has been caught.
        /// </summary>
        public event EventHandler<ProtocolErrorEventArgs> Faulted;

        /// <summary>
        /// Occurs when a message has been received.
        /// </summary>
        public event EventHandler<MessageEventArgs> MessageReceived;

        /// <summary>
        /// Gets the message dispatcher.
        /// </summary>
        public IMessageDispatcher MessageDispatcher { get; }

        /// <summary>
        /// Gets the connection options
        /// </summary>
        public ConnectionOptions Options { get; }

        /// <summary>
        /// Gets the negotiated protocol version, or <c>null</c> if not yet connected.
        /// </summary>
        public SemanticVersion ProtocolVersion { get; private set; }

        /// <summary>
        /// Instantiates a new instance of the <see cref="Connection" /> class.
        /// </summary>
        /// <param name="dispatcher">A message dispatcher.</param>
        /// <param name="sender">A sender.</param>
        /// <param name="receiver">A receiver.</param>
        /// <param name="options">Connection options.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dispatcher" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sender" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="receiver" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options" /> is <c>null</c>.</exception>
        public Connection(IMessageDispatcher dispatcher, ISender sender, IReceiver receiver, ConnectionOptions options)
            : this(dispatcher, sender, receiver, options, PluginLogger.DefaultInstance)
        {
        }

        /// <summary>
        /// Instantiates a new instance of the <see cref="Connection" /> class.
        /// </summary>
        /// <param name="dispatcher">A message dispatcher.</param>
        /// <param name="sender">A sender.</param>
        /// <param name="receiver">A receiver.</param>
        /// <param name="options">Connection options.</param>
        /// <param name="logger">A plugin logger.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dispatcher" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sender" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="receiver" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger" /> is <c>null</c>.</exception>
        internal Connection(IMessageDispatcher dispatcher, ISender sender, IReceiver receiver, ConnectionOptions options, IPluginLogger logger)
        {
            if (dispatcher == null)
            {
                throw new ArgumentNullException(nameof(dispatcher));
            }

            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (receiver == null)
            {
                throw new ArgumentNullException(nameof(receiver));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            MessageDispatcher = dispatcher;
            _sender = sender;
            _receiver = receiver;
            Options = options;
            _logger = logger;

            MessageDispatcher.SetConnection(this);
        }

        /// <summary>
        /// Disposes of this instance.
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                Close();

                _receiver.Dispose();
                _sender.Dispose();
                MessageDispatcher.Dispose();

                // Do not dispose of _logger.  This connection does not own it.

                GC.SuppressFinalize(this);

                _isDisposed = true;
            }
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        /// <remarks>This does not call <see cref="IDisposable.Dispose" />.</remarks>
        public void Close()
        {
            if (_state != (int)ConnectionState.Closed)
            {
                _state = (int)ConnectionState.Closing;

                _receiver.MessageReceived -= OnMessageReceived;
                _receiver.Faulted -= OnFaulted;

                _receiver.Close();
                _sender.Close();
                MessageDispatcher.Close();

                _state = (int)ConnectionState.Closed;
            }
        }

        /// <summary>
        /// Asynchronously connects and handshakes with a remote target.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the method has been called already.</exception>
        public async Task ConnectAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (State != ConnectionState.ReadyToConnect)
            {
                throw new InvalidOperationException(Strings.Plugin_ConnectionAlreadyStarted);
            }

            _state = (int)ConnectionState.Connecting;

            _receiver.MessageReceived += OnMessageReceived;
            _receiver.Faulted += OnFaulted;

            using (var symmetricHandshake = new SymmetricHandshake(
                this,
                Options.HandshakeTimeout,
                Options.ProtocolVersion,
                Options.MinimumProtocolVersion))
            {
                _sender.Connect();
                _receiver.Connect();

                _state = (int)ConnectionState.Handshaking;

                try
                {
                    ProtocolVersion = await symmetricHandshake.HandshakeAsync(cancellationToken);
                }
                catch (Exception)
                {
                    _state = (int)ConnectionState.FailedToHandshake;

                    throw;
                }

                if (ProtocolVersion == null)
                {
                    _state = (int)ConnectionState.FailedToHandshake;

                    throw new ProtocolException(Strings.Plugin_HandshakeFailed);
                }

                _state = (int)ConnectionState.Connected;
            }
        }

        /// <summary>
        /// Asynchronously sends a message to the remote target.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message" /> is <c>null</c>.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        /// <exception cref="InvalidOperationException">Thrown if not connected.</exception>
        public async Task SendAsync(Message message, CancellationToken cancellationToken)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            cancellationToken.ThrowIfCancellationRequested();

            if (State == ConnectionState.Closing ||
                State == ConnectionState.Closed)
            {
                return;
            }

            if (_state < (int)ConnectionState.Connecting)
            {
                throw new InvalidOperationException(Strings.Plugin_NotConnected);
            }

            if (_logger.IsEnabled)
            {
                _logger.Write(new CommunicationLogMessage(_logger.Now, message.RequestId, message.Method, message.Type, MessageState.Sending));
            }

            await _sender.SendAsync(message, cancellationToken);

            if (_logger.IsEnabled)
            {
                _logger.Write(new CommunicationLogMessage(_logger.Now, message.RequestId, message.Method, message.Type, MessageState.Sent));
            }
        }

        /// <summary>
        /// Asynchronously sends a message to the remote target and receives the target's response.
        /// </summary>
        /// <typeparam name="TOutbound">The outbound payload type.</typeparam>
        /// <typeparam name="TInbound">The inbound payload type.</typeparam>
        /// <param name="method">The outbound message method.</param>
        /// <param name="payload">The outbound message payload.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <typeparamref name="TInbound" />
        /// from the target.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        /// <exception cref="InvalidOperationException">Thrown if not connected.</exception>
        public Task<TInbound> SendRequestAndReceiveResponseAsync<TOutbound, TInbound>(
            MessageMethod method,
            TOutbound payload,
            CancellationToken cancellationToken)
            where TOutbound : class
            where TInbound : class
        {
            if (State == ConnectionState.Closing ||
                State == ConnectionState.Closed)
            {
                return TaskResult.Null<TInbound>();
            }

            if (_state < (int)ConnectionState.Connecting)
            {
                throw new InvalidOperationException(Strings.Plugin_NotConnected);
            }

            cancellationToken.ThrowIfCancellationRequested();

            return MessageDispatcher.DispatchRequestAsync<TOutbound, TInbound>(method, payload, cancellationToken);
        }

        private void OnMessageReceived(object sender, MessageEventArgs e)
        {
            if (_logger.IsEnabled)
            {
                _logger.Write(new CommunicationLogMessage(_logger.Now, e.Message.RequestId, e.Message.Method, e.Message.Type, MessageState.Received));
            }

            MessageReceived?.Invoke(this, e);
        }

        private void OnFaulted(object sender, ProtocolErrorEventArgs e)
        {
            Faulted?.Invoke(this, e);
        }
    }
}
