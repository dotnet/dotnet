// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Versioning;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A helper class that performs a symmetric handshake.
    /// </summary>
    public sealed class SymmetricHandshake : IRequestHandler, IDisposable
    {
        private readonly IConnection _connection;
        private readonly HandshakeResponse _handshakeFailedResponse;
        private readonly TimeSpan _handshakeTimeout;
        private bool _isDisposed;
        private readonly SemanticVersion _minimumProtocolVersion;
        private HandshakeRequest _outboundHandshakeRequest;
        private readonly SemanticVersion _protocolVersion;
        private TaskCompletionSource<int> _responseSentTaskCompletionSource;
        private readonly CancellationTokenSource _timeoutCancellationTokenSource;

        /// <summary>
        /// Gets the <see cref="CancellationToken" /> for a request.
        /// </summary>
        public CancellationToken CancellationToken { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SymmetricHandshake" /> class.
        /// </summary>
        /// <param name="connection">A connection.</param>
        /// <param name="handshakeTimeout">The handshake timeout.</param>
        /// <param name="protocolVersion">The handshaker's protocol version.</param>
        /// <param name="minimumProtocolVersion">The handshaker's minimum protocol version.</param>
        public SymmetricHandshake(
            IConnection connection,
            TimeSpan handshakeTimeout,
            SemanticVersion protocolVersion,
            SemanticVersion minimumProtocolVersion)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            if (!TimeoutUtilities.IsValid(handshakeTimeout))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(handshakeTimeout),
                    handshakeTimeout,
                    Strings.Plugin_TimeoutOutOfRange);
            }

            if (protocolVersion == null)
            {
                throw new ArgumentNullException(nameof(protocolVersion));
            }

            if (minimumProtocolVersion == null)
            {
                throw new ArgumentNullException(nameof(minimumProtocolVersion));
            }

            _connection = connection;
            _handshakeTimeout = handshakeTimeout;
            _protocolVersion = protocolVersion;
            _minimumProtocolVersion = minimumProtocolVersion;
            _handshakeFailedResponse = new HandshakeResponse(MessageResponseCode.Error, protocolVersion: null);
            _responseSentTaskCompletionSource = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
            _timeoutCancellationTokenSource = new CancellationTokenSource(handshakeTimeout);

            _timeoutCancellationTokenSource.Token.Register(() =>
            {
                _responseSentTaskCompletionSource.TrySetCanceled();
            });

            CancellationToken = _timeoutCancellationTokenSource.Token;

            if (!_connection.MessageDispatcher.RequestHandlers.TryAdd(MessageMethod.Handshake, this))
            {
                throw new ArgumentException(Strings.Plugin_HandshakeRequestHandlerAlreadyExists, nameof(connection));
            }
        }

        /// <summary>
        /// Disposes of this instance.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _connection.MessageDispatcher.RequestHandlers.TryRemove(MessageMethod.Handshake);
            _timeoutCancellationTokenSource.Dispose();
            GC.SuppressFinalize(this);

            _isDisposed = true;
        }

        /// <summary>
        /// Asynchronously handles handshaking.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="SemanticVersion" />
        /// if the handshake was successful; otherwise, <see langword="null" />.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public async Task<SemanticVersion> HandshakeAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _outboundHandshakeRequest = new HandshakeRequest(_protocolVersion, _minimumProtocolVersion);

            var response = await _connection.SendRequestAndReceiveResponseAsync<HandshakeRequest, HandshakeResponse>(
                MessageMethod.Handshake,
                _outboundHandshakeRequest,
                cancellationToken);

            if (response != null && response.ResponseCode == MessageResponseCode.Success)
            {
                if (IsSupportedVersion(response.ProtocolVersion))
                {
                    await _responseSentTaskCompletionSource.Task;

                    return response.ProtocolVersion;
                }
            }

            await _responseSentTaskCompletionSource.Task;

            return null;
        }

        /// <summary>
        /// Asynchronously handles responding to a request.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="request">A request message.</param>
        /// <param name="responseHandler">A response handler.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="connection" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="request" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="responseHandler" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public async Task HandleResponseAsync(
            IConnection connection,
            Message request,
            IResponseHandler responseHandler,
            CancellationToken cancellationToken)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (responseHandler == null)
            {
                throw new ArgumentNullException(nameof(responseHandler));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var response = _handshakeFailedResponse;
            var handshakeRequest = MessageUtilities.DeserializePayload<HandshakeRequest>(request);

            if (handshakeRequest != null)
            {
                if (!(handshakeRequest.MinimumProtocolVersion > handshakeRequest.ProtocolVersion ||
                    handshakeRequest.ProtocolVersion < _minimumProtocolVersion ||
                    handshakeRequest.MinimumProtocolVersion > _protocolVersion))
                {
                    SemanticVersion negotiatedProtocolVersion;

                    if (_protocolVersion <= handshakeRequest.ProtocolVersion)
                    {
                        negotiatedProtocolVersion = _protocolVersion;
                    }
                    else
                    {
                        negotiatedProtocolVersion = handshakeRequest.ProtocolVersion;
                    }

                    response = new HandshakeResponse(MessageResponseCode.Success, negotiatedProtocolVersion);
                }
            }

            await responseHandler.SendResponseAsync(request, response, cancellationToken)
                .ContinueWith(task => _responseSentTaskCompletionSource.TrySetResult(0));
        }

        private bool IsSupportedVersion(SemanticVersion requestedProtocolVersion)
        {
            return _minimumProtocolVersion <= requestedProtocolVersion && requestedProtocolVersion <= _protocolVersion;
        }
    }
}
