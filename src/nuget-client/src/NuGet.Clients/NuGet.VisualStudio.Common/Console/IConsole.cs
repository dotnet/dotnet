// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Represents a console (editor) which a user interacts with.
    /// </summary>
    public interface IConsole : IOutputConsole
    {
        /// <summary>
        /// The host associated with this console. Each console is associated with 1 host
        /// to perform command interpretation. The console creator is responsible to
        /// setup this association.
        /// </summary>
        IHost Host { get; set; }

        /// <summary>
        /// Indicates to the active host whether this console wants to show a disclaimer header.
        /// </summary>
        bool ShowDisclaimerHeader { get; }

        /// <summary>
        /// Get the console dispatcher which dispatches user interaction.
        /// </summary>
        IConsoleDispatcher Dispatcher { get; }
    }
}
