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
    public interface IConnection : IDisposable
    {
        /// <summary>
        /// Occurs when an unrecoverable fault has been caught.
        /// </summary>
        event EventHandler<ProtocolErrorEventArgs> Faulted;

        /// <summary>
        /// Occurs when a message has been received.
        /// </summary>
        event EventHandler<MessageEventArgs> MessageReceived;

        /// <summary>
        /// Gets the message dispatcher.
        /// </summary>
        IMessageDispatcher MessageDispatcher { get; }

        /// <summary>
        /// Gets the connection options
        /// </summary>
        ConnectionOptions Options { get; }

        /// <summary>
        /// Gets the negotiated protocol version, or <see langword="null" /> if not yet connected.
        /// </summary>
        SemanticVersion ProtocolVersion { get; }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        /// <remarks>This does not call <see cref="IDisposable.Dispose" />.</remarks>
        void Close();

        /// <summary>
        /// Asynchronously sends a message to the remote target.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message" /> is <see langword="null" />.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        /// <exception cref="InvalidOperationException">Thrown if not connected.</exception>
        Task SendAsync(Message message, CancellationToken cancellationToken);

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
        Task<TInbound> SendRequestAndReceiveResponseAsync<TOutbound, TInbound>(
            MessageMethod method,
            TOutbound payload,
            CancellationToken cancellationToken)
            where TOutbound : class
            where TInbound : class;
    }
}
