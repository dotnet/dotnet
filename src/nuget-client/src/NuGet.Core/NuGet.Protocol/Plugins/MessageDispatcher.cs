// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A message dispatcher that maintains state for outstanding requests
    /// and routes messages to configured request handlers.
    /// </summary>
    public sealed class MessageDispatcher : IMessageDispatcher, IResponseHandler
    {
        private IConnection _connection;
        private readonly IIdGenerator _idGenerator;
        private bool _isClosed;
        private bool _isDisposed;
        private readonly ConcurrentDictionary<string, InboundRequestContext> _inboundRequestContexts;
        private readonly IPluginLogger _logger;
        private readonly ConcurrentDictionary<string, OutboundRequestContext> _outboundRequestContexts;
        private readonly InboundRequestProcessingHandler _inboundRequestProcessingContext;

        /// <summary>
        /// Gets the request handlers for use by the dispatcher.
        /// </summary>
        public IRequestHandlers RequestHandlers { get; }

        /// <summary>
        /// Instantiates a new <see cref="MessageDispatcher" /> class.
        /// </summary>
        /// <param name="requestHandlers">Request handlers.</param>
        /// <param name="idGenerator">A unique identifier generator.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="requestHandlers" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="idGenerator" />
        /// is <see langword="null" />.</exception>
        public MessageDispatcher(IRequestHandlers requestHandlers, IIdGenerator idGenerator)
            : this(requestHandlers, idGenerator, new InboundRequestProcessingHandler(), PluginLogger.DefaultInstance)
        {
        }

        /// <summary>
        /// Instantiates a new <see cref="MessageDispatcher" /> class.
        /// </summary>
        /// <param name="requestHandlers">Request handlers.</param>
        /// <param name="idGenerator">A unique identifier generator.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="requestHandlers" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="idGenerator" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="inboundRequestProcessingHandler" />
        /// is <see langword="null" />.</exception>
        /// /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger" />
        /// is <see langword="null" />.</exception>
        internal MessageDispatcher(IRequestHandlers requestHandlers, IIdGenerator idGenerator, InboundRequestProcessingHandler inboundRequestProcessingHandler, IPluginLogger logger)
        {
            if (requestHandlers == null)
            {
                throw new ArgumentNullException(nameof(requestHandlers));
            }

            if (idGenerator == null)
            {
                throw new ArgumentNullException(nameof(idGenerator));
            }

            if (inboundRequestProcessingHandler == null)
            {
                throw new ArgumentNullException(nameof(inboundRequestProcessingHandler));
            }
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            RequestHandlers = requestHandlers;
            _idGenerator = idGenerator;
            _logger = logger;

            _inboundRequestContexts = new ConcurrentDictionary<string, InboundRequestContext>();
            _outboundRequestContexts = new ConcurrentDictionary<string, OutboundRequestContext>();
            _inboundRequestProcessingContext = inboundRequestProcessingHandler;
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

            Close();
            _inboundRequestProcessingContext.Dispose();
            SetConnection(connection: null);

            // Do not dispose of _logger.  This message dispatcher does not own it.

            GC.SuppressFinalize(this);

            _isDisposed = true;
        }

        /// <summary>
        /// Closes the dispatcher.
        /// </summary>
        /// <remarks>This does not call <see cref="IDisposable.Dispose" />.</remarks>
        public void Close()
        {
            if (!_isClosed)
            {
                SetConnection(connection: null);

                foreach (var entry in _inboundRequestContexts)
                {
                    entry.Value.Dispose();
                }

                foreach (var entry in _outboundRequestContexts)
                {
                    entry.Value.Dispose();
                }

                _isClosed = true;
            }
        }

        /// <summary>
        /// Creates a message.
        /// </summary>
        /// <param name="type">The message type.</param>
        /// <param name="method">The message method.</param>
        /// <returns>A message.</returns>
        public Message CreateMessage(MessageType type, MessageMethod method)
        {
            var requestId = _idGenerator.GenerateUniqueId();

            return MessageUtilities.Create(requestId, type, method);
        }

        /// <summary>
        /// Creates a message.
        /// </summary>
        /// <typeparam name="TPayload">The message payload.</typeparam>
        /// <param name="type">The message type.</param>
        /// <param name="method">The message method.</param>
        /// <param name="payload">The message payload.</param>
        /// <returns>A message.</returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="payload" /> is <see langword="null" />.</exception>
        public Message CreateMessage<TPayload>(MessageType type, MessageMethod method, TPayload payload)
            where TPayload : class
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            var requestId = _idGenerator.GenerateUniqueId();

            return MessageUtilities.Create(requestId, type, method, payload);
        }

        /// <summary>
        /// Asynchronously dispatches a cancellation request for the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="request" /> is <see langword="null" />.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public Task DispatchCancelAsync(Message request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            cancellationToken.ThrowIfCancellationRequested();

            // Capture _connection as SetConnection(...) could null it out later.
            var connection = _connection;

            if (connection == null)
            {
                return Task.CompletedTask;
            }

            return DispatchCancelAsync(connection, request, cancellationToken);
        }

        /// <summary>
        /// Asynchronously dispatches a fault notification for the specified request.
        /// </summary>
        /// <param name="request">The cancel request.</param>
        /// <param name="fault">The fault payload.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="fault" /> is <see langword="null" />.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public Task DispatchFaultAsync(Message request, Fault fault, CancellationToken cancellationToken)
        {
            if (fault == null)
            {
                throw new ArgumentNullException(nameof(fault));
            }

            cancellationToken.ThrowIfCancellationRequested();

            // Capture _connection as SetConnection(...) could null it out later.
            var connection = _connection;

            if (connection == null)
            {
                return Task.CompletedTask;
            }

            return DispatchFaultAsync(connection, request, fault, cancellationToken);
        }

        /// <summary>
        /// Asynchronously dispatches a progress notification for the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="progress">The progress payload.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="request" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="progress" /> is <see langword="null" />.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public Task DispatchProgressAsync(Message request, Progress progress, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (progress == null)
            {
                throw new ArgumentNullException(nameof(progress));
            }

            cancellationToken.ThrowIfCancellationRequested();

            // Capture _connection as SetConnection(...) could null it out later.
            var connection = _connection;

            if (connection == null)
            {
                return Task.CompletedTask;
            }

            return DispatchProgressAsync(connection, request, progress, cancellationToken);
        }

        /// <summary>
        /// Asynchronously dispatches a request.
        /// </summary>
        /// <typeparam name="TOutbound">The request payload type.</typeparam>
        /// <typeparam name="TInbound">The expected response payload type.</typeparam>
        /// <param name="method">The request method.</param>
        /// <param name="payload">The request payload.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <typeparamref name="TInbound" />
        /// from the target.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public Task<TInbound> DispatchRequestAsync<TOutbound, TInbound>(
            MessageMethod method,
            TOutbound payload,
            CancellationToken cancellationToken)
            where TOutbound : class
            where TInbound : class
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Capture _connection as SetConnection(...) could null it out later.
            var connection = _connection;

            if (connection == null)
            {
                return TaskResult.Null<TInbound>();
            }

            return DispatchWithNewContextAsync<TOutbound, TInbound>(
                connection,
                MessageType.Request,
                method,
                payload,
                cancellationToken);
        }

        /// <summary>
        /// Asynchronously dispatches a response.
        /// </summary>
        /// <typeparam name="TOutbound">The request payload type.</typeparam>
        /// <param name="request">The associated request.</param>
        /// <param name="responsePayload">The response payload.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="request" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="responsePayload" /> is <see langword="null" />.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public Task DispatchResponseAsync<TOutbound>(
            Message request,
            TOutbound responsePayload,
            CancellationToken cancellationToken)
            where TOutbound : class
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (responsePayload == null)
            {
                throw new ArgumentNullException(nameof(responsePayload));
            }

            cancellationToken.ThrowIfCancellationRequested();

            // Capture _connection as SetConnection(...) could null it out later.
            var connection = _connection;

            if (connection == null)
            {
                return Task.CompletedTask;
            }

            return DispatchAsync(connection, MessageType.Response, request, responsePayload, cancellationToken);
        }

        /// <summary>
        /// Sets the connection to be used for dispatching messages.
        /// </summary>
        /// <param name="connection">A connection instance.  Can be <see langword="null" />.</param>
        public void SetConnection(IConnection connection)
        {
            if (_connection == connection)
            {
                return;
            }

            if (_connection != null)
            {
                _connection.MessageReceived -= OnMessageReceived;
            }

            _connection = connection;

            if (_connection != null)
            {
                _connection.MessageReceived += OnMessageReceived;
            }
        }

        Task IResponseHandler.SendResponseAsync<TPayload>(
            Message request,
            TPayload payload,
            CancellationToken cancellationToken)
        {
            return DispatchResponseAsync(request, payload, cancellationToken);
        }

        private async Task DispatchAsync<TOutgoing>(
            IConnection connection,
            MessageType type,
            Message request,
            TOutgoing payload,
            CancellationToken cancellationToken)
            where TOutgoing : class
        {
            InboundRequestContext requestContext;

            if (!_inboundRequestContexts.TryGetValue(request.RequestId, out requestContext))
            {
                return;
            }

            var message = MessageUtilities.Create(request.RequestId, type, request.Method, payload);

            try
            {
                await connection.SendAsync(message, cancellationToken);
            }
            finally
            {
                RemoveInboundRequestContext(request.RequestId);
            }
        }

        private async Task DispatchCancelAsync(
            IConnection connection,
            Message request,
            CancellationToken cancellationToken)
        {
            var message = new Message(request.RequestId, MessageType.Cancel, request.Method);

            await DispatchWithExistingContextAsync(connection, message, cancellationToken);
        }

        private async Task DispatchFaultAsync(
            IConnection connection,
            Message request,
            Fault fault,
            CancellationToken cancellationToken)
        {
            Message message;

            var jsonPayload = JsonSerializationUtilities.FromObject(fault);

            if (request == null)
            {
                var requestId = _idGenerator.GenerateUniqueId();

                message = new Message(requestId, MessageType.Fault, MessageMethod.None, jsonPayload);

                await connection.SendAsync(message, cancellationToken);
            }
            else
            {
                message = new Message(request.RequestId, MessageType.Fault, request.Method, jsonPayload);

                await DispatchWithExistingContextAsync(connection, message, cancellationToken);
            }
        }

        private async Task DispatchProgressAsync(
            IConnection connection,
            Message request,
            Progress progress,
            CancellationToken cancellationToken)
        {
            var message = MessageUtilities.Create(request.RequestId, MessageType.Progress, request.Method, progress);

            await DispatchWithExistingContextAsync(connection, message, cancellationToken);
        }

        private async Task DispatchWithExistingContextAsync(
            IConnection connection,
            Message response,
            CancellationToken cancellationToken)
        {
            var requestContext = GetOutboundRequestContext(response.RequestId);

            await connection.SendAsync(response, cancellationToken);
        }

        private async Task<TIncoming> DispatchWithNewContextAsync<TOutgoing, TIncoming>(
            IConnection connection,
            MessageType type,
            MessageMethod method,
            TOutgoing payload,
            CancellationToken cancellationToken)
            where TOutgoing : class
            where TIncoming : class
        {
            var message = CreateMessage(type, method, payload);
            var timeout = GetRequestTimeout(connection, type, method);
            var isKeepAlive = GetIsKeepAlive(type, method);
            var requestContext = CreateOutboundRequestContext<TIncoming>(
                message,
                timeout,
                isKeepAlive,
                cancellationToken);

            _outboundRequestContexts.TryAdd(message.RequestId, requestContext);

            switch (type)
            {
                case MessageType.Request:
                case MessageType.Response:
                case MessageType.Fault:
                    var removeRequestContext = true;

                    try
                    {
                        await connection.SendAsync(message, requestContext.CancellationToken);

                        return await requestContext.CompletionTask;
                    }
                    catch (OperationCanceledException) when (requestContext.CancellationToken.IsCancellationRequested)
                    {
                        if (_logger.IsEnabled)
                        {
                            _logger.Write(new CommunicationLogMessage(_logger.Now, message.RequestId, message.Method, message.Type, MessageState.Cancelled));
                        }

                        // Keep the request context around if cancellation was requested.
                        // A race condition exists where after sending a cancellation request,
                        // we could receive a response (which was in flight) or a cancellation
                        // response.
                        // If a normal response (success/failure) and not a cancellation response
                        // is received after a cancellation request, we need to have an active
                        // request context to avoid a protocol exception.
                        removeRequestContext = false;

                        throw;
                    }
                    finally
                    {
                        if (removeRequestContext)
                        {
                            RemoveOutboundRequestContext(message.RequestId);
                        }
                    }

                default:
                    break;
            }

            return null;
        }

        private void OnMessageReceived(object sender, MessageEventArgs e)
        {
            // Capture _connection as SetConnection(...) could null it out later.
            var connection = _connection;

            if (connection == null)
            {
                return;
            }

            OutboundRequestContext requestContext;

            if (_outboundRequestContexts.TryGetValue(e.Message.RequestId, out requestContext))
            {
                switch (e.Message.Type)
                {
                    case MessageType.Response:
                        requestContext.HandleResponse(e.Message);
                        break;

                    case MessageType.Progress:
                        requestContext.HandleProgress(e.Message);
                        break;

                    case MessageType.Fault:
                        requestContext.HandleFault(e.Message);
                        break;

                    case MessageType.Cancel:
                        requestContext.HandleCancelResponse();
                        break;

                    default:
                        throw new ProtocolException(
                            string.Format(
                                CultureInfo.CurrentCulture,
                                Strings.Plugin_InvalidMessageType,
                                e.Message.Type));
                }

                return;
            }

            switch (e.Message.Type)
            {
                case MessageType.Cancel:
                    HandleInboundCancel(e.Message);
                    break;

                case MessageType.Request:
                    HandleInboundRequest(e.Message);
                    break;

                case MessageType.Fault:
                    HandleInboundFault(e.Message);
                    break;

                default:
                    throw new ProtocolException(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.Plugin_InvalidMessageType,
                            e.Message.Type));
            }
        }

        private void HandleInboundCancel(Message message)
        {
            InboundRequestContext requestContext;

            if (_inboundRequestContexts.TryGetValue(message.RequestId, out requestContext))
            {
                requestContext.Cancel();
            }
        }

        private void HandleInboundFault(Message fault)
        {
            if (fault == null)
            {
                throw new ArgumentNullException(nameof(fault));
            }

            var payload = MessageUtilities.DeserializePayload<Fault>(fault);

            throw new ProtocolException(payload.Message);
        }

        private void HandleInboundRequest(Message message)
        {
            var cancellationToken = CancellationToken.None;
            IRequestHandler requestHandler = null;
            ProtocolException exception = null;

            try
            {
                requestHandler = GetInboundRequestHandler(message.Method);
                cancellationToken = requestHandler.CancellationToken;
            }
            catch (ProtocolException ex)
            {
                exception = ex;
            }

            var requestContext = CreateInboundRequestContext(message, cancellationToken);

            if (exception == null && requestHandler != null)
            {
                _inboundRequestContexts.TryAdd(message.RequestId, requestContext);

                requestContext.BeginResponseAsync(message, requestHandler, this);
            }
            else
            {
                requestContext.BeginFaultAsync(message, exception);
            }
        }

        private IRequestHandler GetInboundRequestHandler(MessageMethod method)
        {
            IRequestHandler handler;

            if (!RequestHandlers.TryGet(method, out handler))
            {
                throw new ProtocolException(
                    string.Format(CultureInfo.CurrentCulture, Strings.Plugin_RequestHandlerDoesNotExist, method));
            }

            return handler;
        }

        private OutboundRequestContext GetOutboundRequestContext(string requestId)
        {
            OutboundRequestContext requestContext;

            if (!_outboundRequestContexts.TryGetValue(requestId, out requestContext))
            {
                throw new ProtocolException(
                    string.Format(CultureInfo.CurrentCulture, Strings.Plugin_RequestContextDoesNotExist, requestId));
            }

            return requestContext;
        }

        private void RemoveInboundRequestContext(string requestId)
        {
            InboundRequestContext requestContext;

            if (_inboundRequestContexts.TryRemove(requestId, out requestContext))
            {
                requestContext.Dispose();
            }
        }

        private void RemoveOutboundRequestContext(string requestId)
        {
            OutboundRequestContext requestContext;

            if (_outboundRequestContexts.TryRemove(requestId, out requestContext))
            {
                requestContext.Dispose();
            }
        }

        private InboundRequestContext CreateInboundRequestContext(
            Message message,
            CancellationToken cancellationToken)
        {
            return new InboundRequestContext(
                _connection,
                message.RequestId,
                cancellationToken,
                _inboundRequestProcessingContext,
                _logger);
        }

        private OutboundRequestContext<TIncoming> CreateOutboundRequestContext<TIncoming>(
            Message message,
            TimeSpan? timeout,
            bool isKeepAlive,
            CancellationToken cancellationToken)
        {
            return new OutboundRequestContext<TIncoming>(
                _connection,
                message,
                timeout,
                isKeepAlive,
                cancellationToken,
                _logger);
        }

        private static bool GetIsKeepAlive(MessageType type, MessageMethod method)
        {
            if (type == MessageType.Request && method == MessageMethod.Handshake)
            {
                return false;
            }

            return true;
        }

        private static TimeSpan GetRequestTimeout(IConnection connection, MessageType type, MessageMethod method)
        {
            if (type == MessageType.Request && method == MessageMethod.Handshake)
            {
                return connection.Options.HandshakeTimeout;
            }

            return connection.Options.RequestTimeout;
        }

        private sealed class NullPayload
        {
        }
    }
}
