// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A request handler for monitoring the exit of a NuGet process.
    /// </summary>
    public sealed class MonitorNuGetProcessExitRequestHandler : IRequestHandler, IDisposable
    {
        private bool _isDisposed;
        private readonly IPlugin _plugin;
        private readonly ConcurrentDictionary<int, Process> _processes;

        public CancellationToken CancellationToken => CancellationToken.None;

        /// <summary>
        /// Initializes a new <see cref="MonitorNuGetProcessExitRequestHandler" /> class.
        /// </summary>
        /// <param name="plugin">A plugin.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="plugin" /> is <see langword="null" />.</exception>
        public MonitorNuGetProcessExitRequestHandler(IPlugin plugin)
        {
            if (plugin == null)
            {
                throw new ArgumentNullException(nameof(plugin));
            }

            _plugin = plugin;
            _processes = new ConcurrentDictionary<int, Process>();
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                foreach (var entry in _processes)
                {
                    entry.Value.EnableRaisingEvents = false;
                    entry.Value.Dispose();
                }

                _processes.Clear();

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
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="request" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="responseHandler" />
        /// is <see langword="null" />.</exception>
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

            var monitorRequest = MessageUtilities.DeserializePayload<MonitorNuGetProcessExitRequest>(request);

            Process process = null;

            try
            {
                process = _processes.GetOrAdd(monitorRequest.ProcessId, pid => Process.GetProcessById(pid));
            }
            catch (Exception)
            {
            }

            MessageResponseCode responseCode;

            if (process == null)
            {
                responseCode = MessageResponseCode.NotFound;
            }
            else
            {
                process.Exited += OnProcessExited;

                process.EnableRaisingEvents = true;

                responseCode = MessageResponseCode.Success;
            }

            var response = new MonitorNuGetProcessExitResponse(responseCode);

            await responseHandler.SendResponseAsync(request, response, cancellationToken);
        }

        private void OnProcessExited(object sender, EventArgs e)
        {
            _plugin.Close();
        }
    }
}
