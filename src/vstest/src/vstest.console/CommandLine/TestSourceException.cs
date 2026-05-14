// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine;

/// <summary>
/// Exception thrown by argument processors when they encounter an error with test source
/// arguments.
/// </summary>
public class TestSourceException : Exception
{
    /// <summary>
    /// Creates a new TestSourceException
    /// </summary>
    public TestSourceException()
    {
    }

    /// <summary>
    /// Initializes with the message.
    /// </summary>
    /// <param name="message">Message for the exception.</param>
    public TestSourceException(string? message)
        : this(message, innerException: null)
    {
    }

    /// <summary>
    /// Initializes with the message and innerException.
    /// </summary>
    /// <param name="message">Message for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference.</param>
    public TestSourceException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

}
