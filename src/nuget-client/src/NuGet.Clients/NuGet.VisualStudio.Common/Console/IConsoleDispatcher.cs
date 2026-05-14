// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// IConsoleDispatcher dispatches and executes console command line inputs on the host.
    /// </summary>
    public interface IConsoleDispatcher
    {
        /// <summary>
        /// Start dispatching console command line inputs. This method can execute asynchronously.
        /// </summary>
        void Start();

        Task StartAsync();

        /// <summary>
        /// Raised when the Start method completes asynchronously.
        /// </summary>
        event EventHandler StartCompleted;

        /// <summary>
        /// Raised every time the WaitKey() method is called.
        /// </summary>
        event EventHandler StartWaitingKey;

        /// <summary>
        /// Indicates whether the StartCompleted event has raised.
        /// </summary>
        /// <returns></returns>
        bool IsStartCompleted { get; }

        /// <summary>
        /// Indicates whether the console is busy executing an earlier command;
        /// </summary>
        bool IsExecutingCommand { get; }

        bool IsExecutingReadKey { get; }

        bool IsKeyAvailable { get; }

        void AcceptKeyInput();

        VsKeyInfo WaitKey();

        /// <summary>
        /// Clear existing console content. This must be used if you want to clear the console
        /// content externally (not inside a host command execution). The console dispatcher manages
        /// console state, displays prompt, etc. Call this method to avoid interfere with user
        /// input dispatching.
        /// On the other hand, if you need to clear the console content inside host command execution,
        /// use IConsole.Clear() instead.
        /// </summary>
        void ClearConsole();
    }
}
