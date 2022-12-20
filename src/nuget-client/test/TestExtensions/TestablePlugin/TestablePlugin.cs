// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Plugins;
using NuGet.Versioning;

namespace NuGet.Test.TestExtensions.TestablePlugin
{
    internal sealed class TestablePlugin : IRequestHandler, IDisposable
    {
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isDisposed;
        private IPlugin _plugin;
        private readonly BlockingCollection<Response> _responses;

        public CancellationToken CancellationToken { get; private set; }

        internal TestablePlugin(BlockingCollection<Response> responses)
        {
            _responses = responses;
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                if (_plugin != null)
                {
                    _plugin.Dispose();
                }

                if (_cancellationTokenSource != null)
                {
                    using (_cancellationTokenSource)
                    {
                        _cancellationTokenSource.Cancel();
                    }
                }

                GC.SuppressFinalize(this);

                _isDisposed = true;
            }
        }

        internal async Task StartAsync(bool causeProtocolException, CancellationToken cancellationToken)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            CancellationToken = _cancellationTokenSource.Token;

            IRequestHandlers requestHandlers = CreateRequestHandlers();
            ConnectionOptions options = ConnectionOptions.CreateDefault();

            if (causeProtocolException)
            {
                options = new ConnectionOptions(
                    new SemanticVersion(100, 0, 0),
                    new SemanticVersion(100, 0, 0),
                    options.HandshakeTimeout,
                    options.RequestTimeout);
            }

            _plugin = await PluginFactory.CreateFromCurrentProcessAsync(requestHandlers, options, CancellationToken);

            if (_plugin.Connection.ProtocolVersion != ProtocolConstants.CurrentVersion)
            {
                throw new NotSupportedException();
            }

            await Task.Delay(Timeout.InfiniteTimeSpan, CancellationToken);
        }

        public async Task HandleResponseAsync(
            IConnection connection,
            Message message,
            IResponseHandler responseHandler,
            CancellationToken cancellationToken)
        {
            var response = _responses.Take(cancellationToken);

            if (message.Type == MessageType.Request)
            {
                switch (message.Method)
                {
                    case MessageMethod.Initialize:
                        {
                            var initializeRequest = JsonSerializationUtilities.ToObject<InitializeRequest>(message.Payload);

                            _plugin.Connection.Options.SetRequestTimeout(initializeRequest.RequestTimeout);
                        }
                        break;

                    case MessageMethod.Close:
                        _cancellationTokenSource.Cancel();
                        break;

                    default:
                        break;
                }
            }

            await responseHandler.SendResponseAsync(message, response.Payload, cancellationToken);
        }

        private IRequestHandlers CreateRequestHandlers()
        {
            var handlers = new RequestHandlers();

            handlers.TryAdd(MessageMethod.Initialize, this);
            handlers.TryAdd(MessageMethod.GetOperationClaims, this);

            return handlers;
        }

        private void OnShuttingDown(object sender, EventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
