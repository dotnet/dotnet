// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A message dispatcher that maintains state for outstanding requests
    /// and routes messages to configured request handlers.
    /// </summary>
    public interface IMessageDispatcher : IDisposable
    {
        /// <summary>
        /// Gets the request handlers for use by the dispatcher.
        /// </summary>
        IRequestHandlers RequestHandlers { get; }

        /// <summary>
        /// Closes the dispatcher.
        /// </summary>
        /// <remarks>This does not call <see cref="IDisposable.Dispose" />.</remarks>
        void Close();

        /// <summary>
        /// Creates a message.
        /// </summary>
        /// <param name="type">The message type.</param>
        /// <param name="method">The message method.</param>
        /// <returns>A message.</returns>
        Message CreateMessage(MessageType type, MessageMethod method);

        /// <summary>
        /// Creates a message.
        /// </summary>
        /// <typeparam name="TPayload">The message payload.</typeparam>
        /// <param name="type">The message type.</param>
        /// <param name="method">The message method.</param>
        /// <param name="payload">The message payload.</param>
        /// <returns>A message.</returns>
        Message CreateMessage<TPayload>(MessageType type, MessageMethod method, TPayload payload)
            where TPayload : class;

        /// <summary>
        /// Asynchronously dispatches a cancellation request for the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DispatchCancelAsync(Message request, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously dispatches a fault notification for the specified request.
        /// </summary>
        /// <param name="request">The cancel request.</param>
        /// <param name="fault">The fault payload.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DispatchFaultAsync(Message request, Fault fault, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously dispatches a progress notification for the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="progress">The progress payload.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DispatchProgressAsync(Message request, Progress progress, CancellationToken cancellationToken);

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
        Task<TInbound> DispatchRequestAsync<TOutbound, TInbound>(
            MessageMethod method,
            TOutbound payload,
            CancellationToken cancellationToken)
            where TOutbound : class
            where TInbound : class;

        /// <summary>
        /// Asynchronously dispatches a response.
        /// </summary>
        /// <typeparam name="TOutbound">The request payload type.</typeparam>
        /// <param name="request">The associated request.</param>
        /// <param name="responsePayload">The response payload.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DispatchResponseAsync<TOutbound>(Message request, TOutbound responsePayload, CancellationToken cancellationToken)
            where TOutbound : class;

        /// <summary>
        /// Sets the connection to be used for dispatching messages.
        /// </summary>
        /// <param name="connection">A connection instance.  Can be <see langword="null" />.</param>
        void SetConnection(IConnection connection);
    }
}
