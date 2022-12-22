// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Internal;

/// <summary>
/// Interface for wrapping the Console class
/// </summary>
internal interface IConsoleHelper
{
    /// <summary>
    /// Returns the left position of the cursor
    /// </summary>
    int CursorLeft { get; }

    /// <summary>
    /// Returns the right position of the cursor
    /// </summary>
    int CursorTop { get; }

    /// <summary>
    /// Returns the width of the console window
    /// </summary>
    int WindowWidth { get; }

    /// <summary>
    /// Sets the cursor position based on the left and top values
    /// </summary>
    /// <param name="left"></param>
    /// <param name="top"></param>
    void SetCursorPosition(int left, int top);
}
