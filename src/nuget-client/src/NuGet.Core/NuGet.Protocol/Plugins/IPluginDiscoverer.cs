// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A plugin discoverer.
    /// </summary>
    public interface IPluginDiscoverer : IDisposable
    {
        /// <summary>
        /// Asynchronously discovers plugins.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="IEnumerable{PluginDiscoveryResult}" />.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        Task<IEnumerable<PluginDiscoveryResult>> DiscoverAsync(CancellationToken cancellationToken);
    }
}
