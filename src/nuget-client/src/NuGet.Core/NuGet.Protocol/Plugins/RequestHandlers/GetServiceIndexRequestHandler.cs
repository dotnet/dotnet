// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A request handler for get service index requests.
    /// </summary>
    public sealed class GetServiceIndexRequestHandler : IRequestHandler, IDisposable
    {
        private bool _isDisposed;
        private readonly IPlugin _plugin;
        private readonly ConcurrentDictionary<string, SourceRepository> _repositories;

        /// <summary>
        /// Gets the <see cref="CancellationToken" /> for a request.
        /// </summary>
        public CancellationToken CancellationToken => CancellationToken.None;

        /// <summary>
        /// Initializes a new <see cref="GetServiceIndexRequestHandler" /> class.
        /// </summary>
        /// <param name="plugin">A plugin.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="plugin" /> is <see langword="null" />.</exception>
        public GetServiceIndexRequestHandler(IPlugin plugin)
        {
            if (plugin == null)
            {
                throw new ArgumentNullException(nameof(plugin));
            }

            _plugin = plugin;
            _repositories = new ConcurrentDictionary<string, SourceRepository>();
        }

        /// <summary>
        /// Disposes of this instance.
        /// </summary>
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
        /// Adds or updates a source repository in a source repository cache.
        /// </summary>
        /// <param name="sourceRepository">A source repository.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sourceRepository" />
        /// is <see langword="null" />.</exception>
        public void AddOrUpdateSourceRepository(SourceRepository sourceRepository)
        {
            if (sourceRepository == null)
            {
                throw new ArgumentNullException(nameof(sourceRepository));
            }

            if (sourceRepository.PackageSource != null && sourceRepository.PackageSource.IsHttp)
            {
                _repositories.AddOrUpdate(
                    sourceRepository.PackageSource.Source,
                    sourceRepository,
                    (source, repo) => sourceRepository);
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

            var getRequest = MessageUtilities.DeserializePayload<GetServiceIndexRequest>(request);
            SourceRepository sourceRepository;
            ServiceIndexResourceV3 serviceIndex = null;
            GetServiceIndexResponse responsePayload;

            if (_repositories.TryGetValue(getRequest.PackageSourceRepository, out sourceRepository))
            {
                serviceIndex = await sourceRepository.GetResourceAsync<ServiceIndexResourceV3>(cancellationToken);
            }

            if (serviceIndex == null)
            {
                responsePayload = new GetServiceIndexResponse(MessageResponseCode.NotFound, serviceIndex: null);
            }
            else
            {
                var serviceIndexJson = JObject.Parse(serviceIndex.Json);

                responsePayload = new GetServiceIndexResponse(MessageResponseCode.Success, serviceIndexJson);
            }

            await responseHandler.SendResponseAsync(request, responsePayload, cancellationToken);
        }
    }
}
