// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.TestPlatform.Extensions.EventLogCollector;

/// <summary>
/// Stores the start and end index for EventLogEntries corresponding to a data collection session.
/// </summary>
internal class EventLogSessionContext
{
    private readonly IDictionary<string, IEventLogContainer> _eventLogContainerMap;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventLogSessionContext"/> class.
    /// </summary>
    /// <param name="eventLogContainerMap">
    /// Event Log container map.
    /// </param>
    public EventLogSessionContext(IDictionary<string, IEventLogContainer> eventLogContainerMap)
    {
        _eventLogContainerMap = eventLogContainerMap;
        CreateEventLogContainerStartIndexMap();
    }

    /// <summary>
    /// Gets the start index for EventLogs Entries.
    /// </summary>
    internal Dictionary<string, int> EventLogContainerStartIndexMap { get; private set; }

    /// <summary>
    /// Gets the end index for EventLogs Entries
    /// </summary>
    internal Dictionary<string, int>? EventLogContainerEndIndexMap { get; private set; }

    /// <summary>
    /// Creates the end index map for EventLogs Entries
    /// </summary>
    [MemberNotNull(nameof(EventLogContainerEndIndexMap))]
    public void CreateEventLogContainerEndIndexMap()
    {
        EventLogContainerEndIndexMap = new Dictionary<string, int>(_eventLogContainerMap.Count);

        foreach (KeyValuePair<string, IEventLogContainer> kvp in _eventLogContainerMap)
        {
            kvp.Value.OnEventLogEntryWritten(kvp.Value.EventLog, null);

            EventLogContainerEndIndexMap.Add(kvp.Key, kvp.Value.EventLogEntries.Count - 1);
        }
    }

    /// <summary>
    /// Creates the start index map for EventLogs Entries
    /// </summary>
    [MemberNotNull(nameof(EventLogContainerStartIndexMap))]
    public void CreateEventLogContainerStartIndexMap()
    {
        EventLogContainerStartIndexMap = new Dictionary<string, int>(_eventLogContainerMap.Count);

        foreach (KeyValuePair<string, IEventLogContainer> kvp in _eventLogContainerMap)
        {
            EventLogContainerStartIndexMap.Add(kvp.Key, kvp.Value.EventLogEntries.Count);
        }
    }
}
