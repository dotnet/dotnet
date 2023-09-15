// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// Represents a plugin process.
    /// </summary>
    public interface IPluginProcess : IDisposable
    {
        /// <summary>
        /// Occurs when a line of output has been received.
        /// </summary>
        event EventHandler<LineReadEventArgs> LineRead;

        /// <summary>
        /// Occurs when a process exits.
        /// </summary>
        event EventHandler<IPluginProcess> Exited;

        /// <summary>
        /// Gets the exit code if the process has exited; otherwise, <see langword="null" />.
        /// </summary>
        int? ExitCode { get; }

        /// <summary>
        /// Gets the process ID if the process was started; otherwise, <see langword="null" />.
        /// </summary>
        int? Id { get; }

        /// <summary>
        /// Asynchronously starts reading the standard output stream.
        /// </summary>
        void BeginReadLine();

        /// <summary>
        /// Cancels asynchronous reading of the standard output stream.
        /// </summary>
        void CancelRead();

        /// <summary>
        /// Stops the associated process.
        /// </summary>
        void Kill();
    }
}
