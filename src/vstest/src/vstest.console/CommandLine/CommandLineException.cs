// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine;

/// <summary>
/// Exception thrown by argument processors when they encounter an error with
/// the command line arguments.
/// </summary>
public class CommandLineException : Exception
{
    /// <summary>
    /// Creates a new CommandLineException
    /// </summary>
    public CommandLineException()
    {
    }

    /// <summary>
    /// Initializes with the message.
    /// </summary>
    /// <param name="message">Message for the exception.</param>
    public CommandLineException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes with message and inner exception.
    /// </summary>
    /// <param name="message">Message for the exception.</param>
    /// <param name="innerException">The inner exception.</param>
    public CommandLineException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

}
