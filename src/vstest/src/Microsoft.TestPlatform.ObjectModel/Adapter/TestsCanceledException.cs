// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
#if NETFRAMEWORK
using System.Runtime.Serialization;
#endif

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

/// <summary>
/// Exception thrown by the framework when an executor attempts to send
/// test result to the framework when the test is canceled.
/// </summary>
#if NETFRAMEWORK
[Serializable]
#endif
public class TestCanceledException : Exception
{
    /// <summary>
    /// Creates a new TestCanceledException
    /// </summary>
    public TestCanceledException()
    {
    }

    /// <summary>
    /// Initializes with the message.
    /// </summary>
    /// <param name="message">Message for the exception.</param>
    public TestCanceledException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes with message and inner exception.
    /// </summary>
    /// <param name="message">Message for the exception.</param>
    /// <param name="innerException">The inner exception.</param>
    public TestCanceledException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

#if NETFRAMEWORK
    /// <summary>
    /// Serialization constructor.
    /// </summary>
    protected TestCanceledException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

#endif
}
