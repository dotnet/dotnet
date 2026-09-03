// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// Faulted plugin event arguments.
    /// </summary>
    public sealed class FaultedPluginEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the exception.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Gets the plugin.
        /// </summary>
        public IPlugin Plugin { get; }

        /// <summary>
        /// Instantiates a new <see cref="FaultedPluginEventArgs" /> class.
        /// </summary>
        /// <param name="plugin">A plugin.</param>
        /// <param name="exception">An exception.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="plugin" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="exception" /> is <see langword="null" />.</exception>
        public FaultedPluginEventArgs(IPlugin plugin, Exception exception)
        {
            if (plugin == null)
            {
                throw new ArgumentNullException(nameof(plugin));
            }

            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            Plugin = plugin;
            Exception = exception;
        }
    }
}
