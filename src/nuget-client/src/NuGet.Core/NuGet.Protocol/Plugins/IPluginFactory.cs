// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A plugin factory.
    /// </summary>
    public interface IPluginFactory : IDisposable
    {
        /// <summary>
        /// Asynchronously gets an existing plugin instance or creates a new instance and connects to it.
        /// </summary>
        /// <param name="filePath">The file path of the plugin.</param>
        /// <param name="arguments">Command-line arguments to be supplied to the plugin.</param>
        /// <param name="requestHandlers">Request handlers.</param>
        /// <param name="options">Connection options.</param>
        /// <param name="sessionCancellationToken">A cancellation token for the plugin's lifetime.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="Plugin" />
        /// instance.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="filePath" />
        /// is either <see langword="null" /> or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="arguments" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="requestHandlers" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="sessionCancellationToken" />
        /// is cancelled.</exception>
        /// <exception cref="ObjectDisposedException">Thrown if this object is disposed.</exception>
        /// <remarks>This is intended to be called by NuGet client tools.</remarks>
        Task<IPlugin> GetOrCreateAsync(
            string filePath,
            IEnumerable<string> arguments,
            IRequestHandlers requestHandlers,
            ConnectionOptions options,
            CancellationToken sessionCancellationToken);
    }
}
