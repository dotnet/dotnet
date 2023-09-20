// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A request handler for closing a plugin.
    /// </summary>
    public sealed class CloseRequestHandler : IRequestHandler, IDisposable
    {
        private bool _isDisposed;
        private readonly IPlugin _plugin;

        public CancellationToken CancellationToken => CancellationToken.None;

        /// <summary>
        /// Initializes a new <see cref="CloseRequestHandler" /> class.
        /// </summary>
        /// <param name="plugin">A plugin.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="plugin" /> is <c>null</c>.</exception>
        public CloseRequestHandler(IPlugin plugin)
        {
            if (plugin == null)
            {
                throw new ArgumentNullException(nameof(plugin));
            }

            _plugin = plugin;
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _plugin.Dispose();

                GC.SuppressFinalize(this);

                _isDisposed = true;
            }
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
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="request" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="responseHandler" />
        /// is <c>null</c>.</exception>
        public Task HandleResponseAsync(
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

            _plugin.Close();

            return Task.CompletedTask;
        }
    }
}
