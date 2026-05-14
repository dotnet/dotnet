// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.PackageManagement;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Represents a command host that executes user input commands (synchronously).
    /// </summary>
    public interface IHost
    {
        /// <summary>
        /// Gets a value indicating whether this host accepts command line input.
        /// </summary>
        bool IsCommandEnabled { get; }

        /// <summary>
        /// Do initialization work before the specified console accepts user inputs.
        /// </summary>
        /// <param name="console">The console requesting the initialization.</param>
        void Initialize(IConsole console);

        /// <summary>
        /// Sets the default runspace from the console
        /// </summary>
        void SetDefaultRunspace();

        /// <summary>
        /// Get the current command prompt used by this host.
        /// </summary>
        string Prompt { get; }

        /// <summary>
        /// Execute a command on this host from the specified console.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="console">The console requesting the execution.</param>
        /// <param name="inputs">Inputs for the command</param>
        /// <returns>
        /// true if the command is executed. In the case of async host, this indicates
        /// that the command is being executed and ExecuteEnd event would signal the end of
        /// execution.
        /// </returns>
        bool Execute(IConsole console, string command, object[] inputs);

        /// <summary>
        /// Abort the current execution if this host is executing a command, or discard currently
        /// constructing multiple-line command if any.
        /// </summary>
        void Abort();

        string ActivePackageSource { get; set; }

        PackageManagementContext PackageManagementContext { get; }

        string[] GetPackageSources();

        string DefaultProject { get; }

        void SetDefaultProjectIndex(int index);

        string[] GetAvailableProjects();

        bool IsInitializedSuccessfully { get; }
    }

    /// <summary>
    /// Represents a command host that executes commands asynchronously. The console depends on
    /// ExecuteEnd event to detect end of command execution.
    /// </summary>
    public interface IAsyncHost : IHost
    {
        /// <summary>
        /// Occurs when an async command execution is completed, disregarding if it succeeded, failed or
        /// aborted. The console depends on this event to prompt for next user input.
        /// </summary>
        event EventHandler ExecuteEnd;
    }
}
