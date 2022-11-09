// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
#if NETFRAMEWORK
using System.Runtime.Serialization;
#endif

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <summary>
/// Exception thrown by Run Settings when an error with a settings provider
/// is encountered.
/// </summary>
#if NETFRAMEWORK
[Serializable]
#endif
public class SettingsException : Exception
{
    /// <summary>
    /// Default Constructor
    /// </summary>
    public SettingsException() : base()
    {
    }

    /// <summary>
    /// Initializes with the message.
    /// </summary>
    /// <param name="message">Message for the exception.</param>
    public SettingsException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes with message and inner exception.
    /// </summary>
    /// <param name="message">Message for the exception.</param>
    /// <param name="innerException">The inner exception.</param>
    public SettingsException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

#if NETFRAMEWORK
    /// <summary>
    /// Serialization constructor.
    /// </summary>
    protected SettingsException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

#endif
}
