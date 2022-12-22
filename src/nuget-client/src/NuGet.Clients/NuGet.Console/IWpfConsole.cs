// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.VisualStudio;

namespace NuGetConsole
{
    /// <summary>
    /// Interface to access more properties of wpf console.
    /// </summary>
    public interface IWpfConsole : IConsole, IDisposable
    {
        /// <summary>
        /// Get the console UIElement to be used as Content for a VS tool window.
        /// </summary>
        object Content { get; }

        /// <summary>
        /// Sets the console width.  Console width will not change again without calling
        /// this method with a new value.
        /// </summary>
        /// <param name="consoleWidth">The new console width in characters.</param>
        void SetConsoleWidth(int consoleWidth);

        /// <summary>
        /// Tells the Wpf console to update its state when command is executing.
        /// </summary>
        void SetExecutionMode(bool isExecuting);

        /// <summary>
        /// Tells the Wpf console to start writing output.
        /// Before this method is called, the console shouldn't write out the output.
        /// If Write() is called before this method, the console should cache the
        /// text. Then when this method is finally called, it will flush all the cached text.
        /// </summary>
        void StartWritingOutput();

        /// <summary>
        /// Get the editor's IVsTextView for further direct interaction.
        /// </summary>
        object VsTextView { get; }
    }
}
