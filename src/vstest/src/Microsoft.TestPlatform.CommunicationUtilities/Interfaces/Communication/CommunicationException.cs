// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;

/// <summary>
/// Exception thrown when communication on a channel fails.
/// </summary>
public class CommunicationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommunicationException" /> class.
    /// </summary>
    public CommunicationException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommunicationException" /> class with provided
    /// message.
    /// </summary>
    /// <param name="message">Message describing the error.</param>
    public CommunicationException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommunicationException" /> class with provided
    /// message and inner exception.
    /// </summary>
    /// <param name="message">Message describing the error.</param>
    /// <param name="inner">Inner exception.</param>
    public CommunicationException(string? message, Exception? inner)
        : base(message, inner)
    {
    }
}
