// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using System.Threading;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// Represents a plugin.
    /// </summary>
    public sealed class Plugin : IPlugin
    {
        private bool _isClosed;
        private readonly TimeSpan _idleTimeout;
        private readonly Timer _idleTimer;
        private readonly object _idleTimerLock;
        private bool _isDisposed;
        private readonly bool _isOwnProcess;
        private readonly IPluginProcess _process;

        /// <summary>
        /// Occurs before the plugin closes.
        /// </summary>
        public event EventHandler BeforeClose;

        /// <summary>
        /// Occurs when the plugin has closed.
        /// </summary>
        public event EventHandler Closed;

        /// <summary>
        /// Occurs when a plugin process has exited.
        /// </summary>
        public event EventHandler<PluginEventArgs> Exited;

        /// <summary>
        /// Occurs when a plugin or plugin connection has faulted.
        /// </summary>
        public event EventHandler<FaultedPluginEventArgs> Faulted;

        /// <summary>
        /// Occurs when a plugin has been idle for the configured idle timeout period.
        /// </summary>
        public event EventHandler<PluginEventArgs> Idle;

        /// <summary>
        /// Gets the connection for the plugin
        /// </summary>
        public IConnection Connection { get; }

        /// <summary>
        /// Gets the file path for the plugin.
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// Gets the unique identifier for the plugin.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the name of the plugin.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Instantiates a new <see cref="Plugin" /> class.
        /// </summary>
        /// <param name="filePath">The plugin file path.</param>
        /// <param name="connection">The plugin connection.</param>
        /// <param name="process">The plugin process.</param>
        /// <param name="isOwnProcess"><c>true</c> if <paramref name="process" /> is the current process;
        /// otherwise, <c>false</c>.</param>
        /// <param name="idleTimeout">The plugin idle timeout.  Can be <see cref="Timeout.InfiniteTimeSpan" />.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="filePath" /> is either <c>null</c>
        /// or an empty string.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="connection" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="process" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="idleTimeout" /> is smaller than
        /// <see cref="Timeout.InfiniteTimeSpan" />.</exception>
        public Plugin(string filePath, IConnection connection, IPluginProcess process, bool isOwnProcess, TimeSpan idleTimeout)
            : this(filePath, connection, process, isOwnProcess, idleTimeout, id: null)
        {
        }

        internal Plugin(string filePath, IConnection connection, IPluginProcess process, bool isOwnProcess, TimeSpan idleTimeout, string id)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(filePath));
            }

            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            if (process == null)
            {
                throw new ArgumentNullException(nameof(process));
            }

            if (idleTimeout < Timeout.InfiniteTimeSpan)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(idleTimeout),
                    idleTimeout,
                    Strings.Plugin_IdleTimeoutMustBeGreaterThanOrEqualToInfiniteTimeSpan);
            }

            Name = Path.GetFileNameWithoutExtension(filePath);
            FilePath = filePath;
            Id = id ?? CreateNewId();
            Connection = connection;
            _process = process;
            _isOwnProcess = isOwnProcess;
            _idleTimerLock = new object();
            _idleTimeout = idleTimeout;

            if (idleTimeout != Timeout.InfiniteTimeSpan)
            {
                _idleTimer = new Timer(OnIdleTimer, state: null, dueTime: idleTimeout, period: Timeout.InfiniteTimeSpan);
            }

            Connection.Faulted += OnFaulted;
            Connection.MessageReceived += OnMessageReceived;

            if (!isOwnProcess)
            {
                process.Exited += OnExited;
            }
        }

        /// <summary>
        /// Disposes of this instance.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            Close();

            Connection.Dispose();

            lock (_idleTimerLock)
            {
                _idleTimer?.Dispose();
            }

            if (!_isOwnProcess)
            {
                _process.Exited -= OnExited;

                _process.Kill();
            }

            _process.Dispose();

            GC.SuppressFinalize(this);

            _isDisposed = true;
        }

        /// <summary>
        /// Closes the plugin.
        /// </summary>
        /// <remarks>This does not call <see cref="IDisposable.Dispose" />.</remarks>
        public void Close()
        {
            if (!_isClosed)
            {
                Connection.Faulted -= OnFaulted;
                Connection.MessageReceived -= OnMessageReceived;

                FireBeforeClose();

                Connection.Close();

                FireClosed();

                _isClosed = true;
            }
        }

        internal static string CreateNewId()
        {
            return Guid.NewGuid().ToString("N", provider: null);
        }

        private void FireBeforeClose()
        {
            try
            {
                BeforeClose?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception)
            {
            }
        }

        private void FireClosed()
        {
            try
            {
                Closed?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception)
            {
            }
        }

        private void OnExited(object sender, IPluginProcess pluginProcess)
        {
            Exited?.Invoke(this, new PluginEventArgs(this));
        }

        private void OnFaulted(object sender, ProtocolErrorEventArgs e)
        {
            Faulted?.Invoke(this, new FaultedPluginEventArgs(this, e.Exception));
        }

        private void OnIdleTimer(object state)
        {
            Idle?.Invoke(this, new PluginEventArgs(this));
        }

        private void OnMessageReceived(object sender, MessageEventArgs e)
        {
            lock (_idleTimerLock)
            {
                _idleTimer?.Change(_idleTimeout, Timeout.InfiniteTimeSpan);
            }
        }
    }
}
