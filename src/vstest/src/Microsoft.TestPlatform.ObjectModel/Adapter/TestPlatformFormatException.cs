// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
#if NETFRAMEWORK
using System.Runtime.Serialization;
#endif

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

/// <summary>
/// Exception thrown on parsing error in user provided filter expression.
/// This can happen when filter has invalid format or has unsupported properties.
/// </summary>
#if NETFRAMEWORK
[Serializable]
#endif
public class TestPlatformFormatException : Exception
{
    /// <summary>
    /// Creates a new TestPlatformFormatException
    /// </summary>
    public TestPlatformFormatException()
        : base()
    {
    }

    /// <summary>
    /// Initializes with the message.
    /// </summary>
    /// <param name="message">Message for the exception.</param>
    public TestPlatformFormatException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes with the message and filter string.
    /// </summary>
    /// <param name="message">Message for the exception.</param>
    /// <param name="filterValue">Filter expression.</param>
    public TestPlatformFormatException(string? message, string? filterValue)
        : base(message)
    {
        FilterValue = filterValue;
    }

    /// <summary>
    /// Initializes with message and inner exception.
    /// </summary>
    /// <param name="message">Message for the exception.</param>
    /// <param name="innerException">The inner exception.</param>
    public TestPlatformFormatException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

#if NETFRAMEWORK
    /// <summary>
    /// Serialization constructor.
    /// </summary>
    protected TestPlatformFormatException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        // Save the basic properties.
        FilterValue = info.GetString("FilterValue");
    }

#endif
    /// <summary>
    /// Filter expression.
    /// </summary>
    public string? FilterValue
    {
        get;
        private set;
    }

#if NETFRAMEWORK
    /// <summary>
    /// Serialization helper.
    /// </summary>
    /// <param name="info">Serialization info to add to</param>
    /// <param name="context">not used</param>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue("FilterValue", FilterValue);
    }
#endif
}
