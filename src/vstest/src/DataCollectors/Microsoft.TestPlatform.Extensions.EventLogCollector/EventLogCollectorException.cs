// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.TestPlatform.Extensions.EventLogCollector;

/// <summary>
/// Private Exception class used for event log exceptions
/// </summary>
internal class EventLogCollectorException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventLogCollectorException"/> class.
    /// </summary>
    /// <param name="localizedMessage">the localized exception message</param>
    /// <param name="innerException">the inner exception</param>
    public EventLogCollectorException(string? localizedMessage, Exception? innerException)
        : base(localizedMessage, innerException)
    {
    }
}
