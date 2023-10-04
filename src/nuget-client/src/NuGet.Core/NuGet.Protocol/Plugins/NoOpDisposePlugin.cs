// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A plugin wrapper that no-ops IDisposable.
    /// </summary>
    public sealed class NoOpDisposePlugin : IPlugin
    {
        private readonly IPlugin _plugin;

        /// <summary>
        /// Occurs before the plugin closes.
        /// </summary>
        public event EventHandler BeforeClose
        {
            add
            {
                _plugin.BeforeClose += value;
            }
            remove
            {
                _plugin.BeforeClose -= value;
            }
        }

        /// <summary>
        /// Occurs when the plugin has closed.
        /// </summary>
        public event EventHandler Closed
        {
            add
            {
                _plugin.Closed += value;
            }
            remove
            {
                _plugin.Closed -= value;
            }
        }

        /// <summary>
        /// Gets the connection for the plugin.
        /// </summary>
        public IConnection Connection => _plugin.Connection;

        /// <summary>
        /// Gets the file path for the plugin.
        /// </summary>
        public string FilePath => _plugin.FilePath;

        /// <summary>
        /// Gets the unique identifier for the plugin.
        /// </summary>
        public string Id => _plugin.Id;

        /// <summary>
        /// Gets the name of the plugin.
        /// </summary>
        public string Name => _plugin.Name;

        /// <summary>
        /// Instantiates a new <see cref="NoOpDisposePlugin" /> class.
        /// </summary>
        /// <param name="plugin">A plugin</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="plugin" /> is <see langword="null" />.</exception>
        public NoOpDisposePlugin(IPlugin plugin)
        {
            if (plugin == null)
            {
                throw new ArgumentNullException(nameof(plugin));
            }

            _plugin = plugin;
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <remarks>Plugin disposal is implemented elsewhere.</remarks>
        public void Dispose()
        {
        }

        /// <summary>
        /// Closes the plugin.
        /// </summary>
        /// <remarks>This does not call <see cref="IDisposable.Dispose" />.</remarks>
        public void Close()
        {
            _plugin.Close();
        }
    }
}
