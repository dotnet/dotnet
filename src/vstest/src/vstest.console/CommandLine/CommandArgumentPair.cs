// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine;

/// <summary>
/// Breaks a string down into command and argument based on the following format:
///     /command:argument.
/// </summary>
internal class CommandArgumentPair
{
    /// <summary>
    /// The separator.
    /// </summary>
    internal const string Separator = ":";

    /// <summary>
    /// The command portion of the input.
    /// </summary>
    public string Command { get; private set; }

    /// <summary>
    /// The argument portion of the input.
    /// </summary>
    public string Argument { get; private set; }

    /// <summary>
    /// Breaks the provided command line switch into the command and argument pair.
    /// </summary>
    /// <param name="input">Input to break up.</param>
    public CommandArgumentPair(string input)
    {
        ValidateArg.NotNullOrWhiteSpace(input, nameof(input));
        Parse(input);
    }

    /// <summary>
    /// Stores the provided command and argument pair.
    /// </summary>
    /// <param name="command">The command portion of the input.</param>
    /// <param name="argument">The argument portion of the input.</param>
    public CommandArgumentPair(string command, string argument)
    {
        ValidateArg.NotNullOrWhiteSpace(command, nameof(command));
        Command = command;
        Argument = argument;
    }

    /// <summary>
    /// Parses the input into the command and argument parts.
    /// </summary>
    /// <param name="input">Input string to parse.</param>
    [MemberNotNull(nameof(Command), nameof(Argument))]
    private void Parse(string input)
    {
        ValidateArg.NotNull(input, nameof(input));

        // Find the index of the separator (":")
        int index = input.IndexOf(Separator, StringComparison.OrdinalIgnoreCase);

        if (index == -1)
        {
            // No separator was found, so use the input as the command.
            Command = input;
            Argument = string.Empty;
        }
        else
        {
            // Separator was found, so separate the command and the input.
            Command = input.Substring(0, index);
            Argument = input.Substring(index + 1);
        }
    }

}
