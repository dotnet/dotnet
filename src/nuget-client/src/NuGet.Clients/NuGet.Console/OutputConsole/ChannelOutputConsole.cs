// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Text;
using System.Threading;
using Microsoft;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.RpcContracts.OutputChannel;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.ServiceBroker;
using Microsoft.VisualStudio.Threading;
using IAsyncServiceProvider = Microsoft.VisualStudio.Shell.IAsyncServiceProvider;
using Task = System.Threading.Tasks.Task;

namespace NuGetConsole
{
    internal class ChannelOutputConsole : BaseOutputConsole, IDisposable
    {
        private static readonly Encoding TextEncoding = Encoding.UTF8;

        private readonly List<string> _deferredOutputMessages = new List<string>();
#pragma warning disable RS0030 // Do not used banned APIs
        private readonly AsyncSemaphore _pipeLock = new AsyncSemaphore(1);
#pragma warning restore RS0030 // Do not used banned APIs

        private Guid _channelGuid;
        private readonly string _channelId;
        private readonly string _channelOutputName;
        private readonly JoinableTaskFactory _joinableTaskFactory;
#pragma warning disable ISB001 // This is disposed in the dispose method
        private AsyncLazy<ServiceBrokerClient> _serviceBrokerClient;
#pragma warning restore ISB001 // This is disposed in the dispose method
        private PipeWriter _channelPipeWriter;
        private bool _disposedValue = false;

        public ChannelOutputConsole(IAsyncServiceProvider asyncServiceProvider, Guid channelId, string outputName, JoinableTaskFactory joinableTaskFactory)
        {
            if (asyncServiceProvider == null)
            {
                throw new ArgumentNullException(nameof(asyncServiceProvider));
            }
            _channelGuid = channelId;
            _channelId = _channelGuid.ToString();
            _channelOutputName = outputName ?? throw new ArgumentNullException(nameof(outputName));
            _joinableTaskFactory = joinableTaskFactory ?? throw new ArgumentNullException(nameof(joinableTaskFactory));

            _serviceBrokerClient = new AsyncLazy<ServiceBrokerClient>(async () =>
            {
                IBrokeredServiceContainer container = await asyncServiceProvider.GetServiceAsync<SVsBrokeredServiceContainer, IBrokeredServiceContainer>();
                Assumes.Present(container);
                IServiceBroker serviceBroker = container.GetFullAccessServiceBroker();
                return new ServiceBrokerClient(serviceBroker, _joinableTaskFactory);
            }, _joinableTaskFactory);
        }

        public override Task ActivateAsync()
        {
            ThrowIfDisposed();
            // Nothing to do here in Visual Studio Online Environments mode. We do not create the pane
            return Task.CompletedTask;
        }

        public override Task ClearAsync()
        {
            ThrowIfDisposed();
            return CloseChannelAsync();
        }

        public override Task WriteAsync(string text)
        {
            ThrowIfDisposed();
            if (text != null)
            {
                return WriteToOutputChannelAsync(_channelId, _channelOutputName, text, CancellationToken.None);
            }
            return Task.CompletedTask;
        }

        private async Task WriteToOutputChannelAsync(string channelId, string displayNameResourceId, string content, CancellationToken cancellationToken)
        {
#pragma warning disable RS0030 // Do not used banned APIs
            using (await _pipeLock.EnterAsync())
            {
                if (_channelPipeWriter == null)
                {
                    var pipe = new Pipe();

                    using (var outputChannelStore = await (await _serviceBrokerClient.GetValueAsync()).GetProxyAsync<IOutputChannelStore>(VisualStudioServices.VS2019_4.OutputChannelStore, cancellationToken))
                    {
                        if (outputChannelStore.Proxy != null)
                        {
                            await outputChannelStore.Proxy.CreateChannelAsync(channelId, displayNameResourceId, pipe.Reader, TextEncoding, cancellationToken);
                            _channelPipeWriter = pipe.Writer;

                            // write any deferred messages
                            foreach (var defferedMessage in _deferredOutputMessages)
                            {
                                // Flush when the original content is logged below
                                await _channelPipeWriter.WriteAsync(GetBytes(defferedMessage), cancellationToken);
                            }
                            _deferredOutputMessages.Clear();
                        }
                        else
                        {
                            // OutputChannel is not available so cache the output messages for later
                            _deferredOutputMessages.Add(content);
                            return;
                        }
                    }
                }
                await _channelPipeWriter.WriteAsync(GetBytes(content), cancellationToken);
                await _channelPipeWriter.FlushAsync(cancellationToken);
            }
#pragma warning restore RS0030 // Do not used banned APIs
        }

        private static byte[] GetBytes(string content)
        {
            return TextEncoding.GetBytes(content);
        }

        private async Task CloseChannelAsync()
        {
#pragma warning disable RS0030 // Do not used banned APIs
            using (await _pipeLock.EnterAsync())
            {
                try
                {
                    _channelPipeWriter?.CancelPendingFlush();
                    _channelPipeWriter?.Complete();
                    _channelPipeWriter = null;
                }
                catch
                {
                    // Ignore exceptions when trying to close a pipe
                }
            }
#pragma warning restore RS0030 // Do not used banned APIs
        }

        public override void StartConsoleDispatcher()
        {
            // Nothing to do here in Visual Studio Online Environments mode. We do not create the pane
        }

        public override Task StartConsoleDispatcherAsync()
        {
            return Task.CompletedTask;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (_serviceBrokerClient.IsValueCreated)
                    {
                        _serviceBrokerClient.GetValue().Dispose();
                    }
                    try
                    {
                        CloseChannelAsync().GetAwaiter().GetResult();
                    }
                    catch
                    {
                        // Ignore exceptions
                    }

#pragma warning disable RS0030 // Do not used banned APIs
                    _pipeLock.Dispose();
#pragma warning restore RS0030 // Do not used banned APIs
                }
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void ThrowIfDisposed()
        {
            if (_disposedValue)
            {
                throw new ObjectDisposedException(nameof(ChannelOutputConsole));
            }
        }
    }
}
