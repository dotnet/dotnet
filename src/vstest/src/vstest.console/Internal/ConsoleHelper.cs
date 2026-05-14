// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Internal;

internal class ConsoleHelper : IConsoleHelper
{
    /// <inheritdoc />
    public int CursorLeft => Console.CursorLeft;

    /// <inheritdoc />
    public int CursorTop => Console.CursorTop;

    /// <inheritdoc />
    public int WindowWidth => Console.WindowWidth;

    /// <inheritdoc />
    public void SetCursorPosition(int left, int top)
    {
        Console.SetCursorPosition(left, top);
    }
}
