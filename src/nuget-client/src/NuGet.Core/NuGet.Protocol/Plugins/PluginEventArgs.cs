// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// Plugin event arguments.
    /// </summary>
    public sealed class PluginEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the plugin.
        /// </summary>
        public IPlugin Plugin { get; }

        /// <summary>
        /// Instantiates a new <see cref="PluginEventArgs" /> class.
        /// </summary>
        /// <param name="plugin">A plugin.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="plugin" /> is <see langword="null" />.</exception>
        public PluginEventArgs(IPlugin plugin)
        {
            if (plugin == null)
            {
                throw new ArgumentNullException(nameof(plugin));
            }

            Plugin = plugin;
        }
    }
}
