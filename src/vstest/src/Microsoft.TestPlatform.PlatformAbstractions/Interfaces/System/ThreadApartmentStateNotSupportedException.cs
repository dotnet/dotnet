// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

/// <summary>
/// Thread apartment state not supported exception.
/// </summary>
public class ThreadApartmentStateNotSupportedException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ThreadApartmentStateNotSupportedException"/> class.
    /// Initializes with the message.
    /// </summary>
    /// <param name="message">Message for the exception.</param>
    public ThreadApartmentStateNotSupportedException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ThreadApartmentStateNotSupportedException"/> class.
    /// Initializes with the message.
    /// </summary>
    /// <param name="message">Message for the exception.</param>
    public ThreadApartmentStateNotSupportedException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ThreadApartmentStateNotSupportedException"/> class.
    /// Initializes with message and inner exception.
    /// </summary>
    /// <param name="message">Message for the exception.</param>
    /// <param name="innerException">The inner exception.</param>
    public ThreadApartmentStateNotSupportedException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

}
