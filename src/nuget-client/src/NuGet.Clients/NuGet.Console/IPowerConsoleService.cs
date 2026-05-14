// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGetConsole
{
    public interface IPowerConsoleService
    {
        /// <summary>
        /// Executes the "command" on console with the inputs
        /// </summary>
        /// <param name="console"></param>
        /// <param name="command"></param>
        /// <param name="inputs"></param>
        /// <returns>
        /// true if the command is executed. In the case of async host, this indicates
        /// that the command is being executed and ExecuteEnd event would signal the end of
        /// execution.
        /// </returns>
        bool Execute(string command, object[] inputs);

        /// <summary>
        /// Occurs when an async command execution is completed, disregarding if it succeeded, failed or
        /// aborted. The console depends on this event to prompt for next user input.
        /// </summary>
        event EventHandler ExecuteEnd;
    }
}
