// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A response handler.
    /// </summary>
    public interface IResponseHandler
    {
        /// <summary>
        /// Asynchronously handles responding to a request.
        /// </summary>
        /// <typeparam name="TPayload">The response payload type.</typeparam>
        /// <param name="request">The request message.</param>
        /// <param name="payload">The response payload.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="request" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="payload" /> is <see langword="null" />.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        Task SendResponseAsync<TPayload>(Message request, TPayload payload, CancellationToken cancellationToken)
            where TPayload : class;
    }
}
