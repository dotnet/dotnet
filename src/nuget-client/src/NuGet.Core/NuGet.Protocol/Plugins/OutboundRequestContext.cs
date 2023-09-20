// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// Context for an outbound request.
    /// </summary>
    public abstract class OutboundRequestContext : IDisposable
    {
        /// <summary>
        /// Gets the <see cref="CancellationToken" />.
        /// </summary>
        public CancellationToken CancellationToken { get; protected set; }

        /// <summary>
        /// Gets the request ID.
        /// </summary>
        public string RequestId { get; protected set; }

        /// <summary>
        /// Disposes of this instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected abstract void Dispose(bool disposing);

        /// <summary>
        /// Handles a cancellation response for the outbound request.
        /// </summary>
        public abstract void HandleCancelResponse();

        /// <summary>
        /// Handles progress notifications for the outbound request.
        /// </summary>
        /// <param name="progress">A progress notification.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="progress" /> is <see langword="null" />.</exception>
        public abstract void HandleProgress(Message progress);

        /// <summary>
        /// Handles a response for the outbound request.
        /// </summary>
        /// <param name="response">A response.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="response" /> is <see langword="null" />.</exception>
        public abstract void HandleResponse(Message response);

        /// <summary>
        /// Handles a fault response for the outbound request.
        /// </summary>
        /// <param name="fault">A fault response.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="fault" /> is <see langword="null" />.</exception>
        public abstract void HandleFault(Message fault);
    }
}
