// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Common;
using NuGet.Shared;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// Represents a unidirectional communications channel from a target.
    /// </summary>
    /// <remarks>
    /// Any public static members of this type are thread safe.
    /// Any instance members are not guaranteed to be thread safe.
    /// </remarks>
    public sealed class StandardOutputReceiver : Receiver
    {
        private bool _hasConnected;
        private readonly IPluginProcess _process;
        private readonly IEnvironmentVariableReader? _environmentVariableReader;

        /// <summary>
        /// Instantiates a new <see cref="StandardOutputReceiver" /> class.
        /// </summary>
        /// <param name="process">A plugin process.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="process" /> is <see langword="null" />.</exception>
        public StandardOutputReceiver(IPluginProcess process)
            : this(process, environmentVariableReader: null)
        {
        }

        internal StandardOutputReceiver(IPluginProcess process, IEnvironmentVariableReader? environmentVariableReader)
        {
            if (process == null)
            {
                throw new ArgumentNullException(nameof(process));
            }

            _process = process;
            _environmentVariableReader = environmentVariableReader;
        }

        /// <summary>
        /// Disposes of this instance.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (IsDisposed)
            {
                return;
            }

            // The process instance is shared with other classes and will be disposed elsewhere.
            if (disposing)
            {
                Close();
            }

            IsDisposed = true;
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        /// <remarks>This does not call <see cref="IDisposable.Dispose" />.</remarks>
        public override void Close()
        {
            if (!IsClosed)
            {
                base.Close();

                _process.LineRead -= OnLineRead;

                _process.CancelRead();
            }
        }

        /// <summary>
        /// Connects.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown if this object is disposed.</exception>
        /// <exception cref="InvalidOperationException">Thrown if this object is closed.</exception>
        /// <exception cref="InvalidOperationException">Thrown if this method has already been called.</exception>
        public override void Connect()
        {
            ThrowIfDisposed();
            ThrowIfClosed();

            if (_hasConnected)
            {
                throw new InvalidOperationException(Strings.Plugin_ConnectionAlreadyStarted);
            }

            _process.LineRead += OnLineRead;
            _process.BeginReadLine();

            _hasConnected = true;
        }

        private void OnLineRead(object? sender, LineReadEventArgs e)
        {
            Message? message = null;

            // Top-level exception handler for a worker pool thread.
            try
            {
                string? line = e.Line;
                if (!IsClosed && line != null && line.Length > 0)
                {
                    if (NuGetFeatureFlags.UseSystemTextJsonDeserializationFeatureSwitch)
                    {
                        message = System.Text.Json.JsonSerializer.Deserialize(line, PluginJsonContext.Default.Message);
                    }
                    else if (NuGetFeatureFlags.IsSystemTextJsonDeserializationEnabledByEnvironment(_environmentVariableReader))
                    {
                        message = System.Text.Json.JsonSerializer.Deserialize(line, PluginJsonContext.Default.Message);
                    }
                    else
                    {
#pragma warning disable IL2026, IL3050 // Legacy Newtonsoft.Json code path is unreachable when feature switch is true; ILC trims this branch in AOT
                        message = JsonSerializationUtilities.Deserialize<Message>(line);
#pragma warning restore IL2026, IL3050
                    }

                    if (message != null)
                    {
                        FireMessageReceivedEvent(message);
                    }
                }
            }
            catch (Exception ex)
            {
                FireFaultEvent(ex, message);
            }
        }
    }
}
