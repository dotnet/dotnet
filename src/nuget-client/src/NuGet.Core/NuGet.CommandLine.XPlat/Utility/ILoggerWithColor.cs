// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using NuGet.Common;

namespace NuGet.CommandLine.XPlat
{
    internal interface ILoggerWithColor : ILogger
    {
        /// <summary>
        /// Logs a minimal level message to the console with the specified text and foreground color.
        /// </summary>
        /// <param name="data">The message text to be logged.</param>
        /// <param name="color">The ConsoleColor value to set the text color.</param>
        void LogMinimal(string data, ConsoleColor color);

        /// <summary>
        /// Log a message directly to the console with the specified text and foreground color.
        /// It does not add a newline character at the end of the message.
        /// </summary>
        /// <param name="data">The message text to be logged.</param>
        /// <param name="color">The ConsoleColor value to set the text color.</param>
        void LogInline(string data, ConsoleColor color);
    }
}
