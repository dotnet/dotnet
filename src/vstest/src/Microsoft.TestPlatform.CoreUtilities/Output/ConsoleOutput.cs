// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

namespace Microsoft.VisualStudio.TestPlatform.Utilities;

/// <summary>
/// Sends output to the console.
/// </summary>
public class ConsoleOutput : IOutput
{
    private static readonly object LockObject = new();
    private static ConsoleOutput? s_consoleOutput;

    private readonly TextWriter _standardOutput;
    private readonly TextWriter _standardError;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleOutput"/> class.
    /// </summary>
    internal ConsoleOutput()
    {
        _standardOutput = Console.Out;
        _standardError = Console.Error;
    }

    /// <summary>
    /// Gets the instance of <see cref="ConsoleOutput"/>.
    /// </summary>
    public static ConsoleOutput Instance
    {
        get
        {
            if (s_consoleOutput != null)
            {
                return s_consoleOutput;
            }

            lock (LockObject)
            {
                s_consoleOutput ??= new ConsoleOutput();
            }

            return s_consoleOutput;
        }
    }

    /// <summary>
    /// Writes the message with a new line.
    /// </summary>
    /// <param name="message">Message to be output.</param>
    /// <param name="level">Level of the message.</param>
    public void WriteLine(string? message, OutputLevel level)
    {
        Write(message, level);
        Write(Environment.NewLine, level);
    }

    /// <summary>
    /// Writes the message with no new line.
    /// </summary>
    /// <param name="message">Message to be output.</param>
    /// <param name="level">Level of the message.</param>
    public void Write(string? message, OutputLevel level)
    {
        switch (level)
        {
            case OutputLevel.Information:
            case OutputLevel.Warning:
                _standardOutput.Write(message);
                break;

            case OutputLevel.Error:
                _standardError.Write(message);
                break;

            default:
                _standardOutput.Write("ConsoleOutput.WriteLine: The output level is unrecognized: {0}", level);
                break;
        }
    }
}
