// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.DotNet.VersionTools.Cli;

internal class ConsoleLogger : ILogger
{
    public void WriteMessage(string message, params object[] values)
    {
        Console.WriteLine(String.Format(message, values));
    }

    public void WriteError(string message, params object[] values)
    {
        var fgColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine(String.Format(message, values));
        Console.ForegroundColor = fgColor;
    }
}
