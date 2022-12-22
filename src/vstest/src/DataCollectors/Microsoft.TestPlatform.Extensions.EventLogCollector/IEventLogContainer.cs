// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.TestPlatform.Extensions.EventLogCollector;

/// <summary>
/// Event log container interface
/// </summary>
internal interface IEventLogContainer : IDisposable
{
    /// <summary>
    /// Gets the event log.
    /// </summary>
    EventLog EventLog { get; }

    /// <summary>
    /// Gets the event log entries.
    /// </summary>
    List<EventLogEntry> EventLogEntries { get; }

    /// <summary>
    /// Event Handler for handling log entries.
    /// </summary>
    /// <param name="source">
    /// The source object that raised EventLog entry event.
    /// </param>
    /// <param name="e">
    /// Contains data related to EventLog entry.
    /// </param>
    void OnEventLogEntryWritten(object? source, EntryWrittenEventArgs? e);
}
