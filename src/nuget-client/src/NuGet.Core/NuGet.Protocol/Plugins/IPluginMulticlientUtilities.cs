// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// Plugin multiclient utilities.
    /// </summary>
    public interface IPluginMulticlientUtilities
    {
        /// <summary>
        /// Asynchronously executes a task once per plugin lifetime per key.
        /// </summary>
        /// <param name="key">A key that identifies the task.</param>
        /// <param name="taskFunc">A function that returns a task.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="key" />
        /// is either <see langword="null" /> or an empty string.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="taskFunc" />
        /// is either <see langword="null" />.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        Task DoOncePerPluginLifetimeAsync(string key, Func<Task> taskFunc, CancellationToken cancellationToken);
    }
}
