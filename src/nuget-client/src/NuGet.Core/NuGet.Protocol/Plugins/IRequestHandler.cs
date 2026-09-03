// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// Represents a request handler.
    /// </summary>
    public interface IRequestHandler
    {
        /// <summary>
        /// Gets the <see cref="CancellationToken" /> for a request.
        /// </summary>
        CancellationToken CancellationToken { get; }

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
        Task HandleResponseAsync(
            IConnection connection,
            Message request,
            IResponseHandler responseHandler,
            CancellationToken cancellationToken);
    }
}
