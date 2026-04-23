// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Extensions.EventLogCollector.UnitTests;

[TestClass]
public class EventLogSessionContextTests
{
    private readonly Dictionary<string, IEventLogContainer> _eventLogContainersMap;
    private readonly DummyEventLogContainer _mockEventLogContainer;

    private EventLogSessionContext? _eventLogSessionContext;

    public EventLogSessionContextTests()
    {
        _mockEventLogContainer = new DummyEventLogContainer(true);
        _eventLogContainersMap = new Dictionary<string, IEventLogContainer>
        {
            { "LogName", _mockEventLogContainer }
        };
    }

    [TestMethod]
    public void CreateEventLogContainerStartIndexMapShouldCreateStartIndexMap()
    {
        _eventLogSessionContext = new EventLogSessionContext(_eventLogContainersMap);
        Assert.AreEqual(2, _eventLogSessionContext.EventLogContainerStartIndexMap["LogName"]);
    }

    [TestMethod]
    public void CreateEventLogContainerEndIndexMapShouldCreateEndIndexMap()
    {
        _eventLogSessionContext = new EventLogSessionContext(_eventLogContainersMap);
        _eventLogSessionContext.CreateEventLogContainerEndIndexMap();
        Assert.AreEqual(1, _eventLogSessionContext.EventLogContainerEndIndexMap["LogName"]);
    }

    [TestMethod]
    public void CreateEventLogContainerShouldNotAddIndexEntriesIfEventLogContainerMapsIsEmpty()
    {
        _eventLogSessionContext = new EventLogSessionContext(new Dictionary<string, IEventLogContainer>());
        _eventLogSessionContext.CreateEventLogContainerStartIndexMap();
        _eventLogSessionContext.CreateEventLogContainerEndIndexMap();

        Assert.IsEmpty(_eventLogSessionContext.EventLogContainerStartIndexMap);
        Assert.IsEmpty(_eventLogSessionContext.EventLogContainerEndIndexMap);
    }

    [TestMethod]
    public void CreateEventLogContainerShouldCreateNegativeEndIndexIfLogEntriesAreEmpty()
    {
        var dict = new Dictionary<string, IEventLogContainer>();
        var dummyEventLogContainer = new DummyEventLogContainer(false);
        dict.Add("DummyEventLog", dummyEventLogContainer);

        _eventLogSessionContext = new EventLogSessionContext(dict);
        _eventLogSessionContext.CreateEventLogContainerStartIndexMap();
        _eventLogSessionContext.CreateEventLogContainerEndIndexMap();

        Assert.AreEqual(0, _eventLogSessionContext.EventLogContainerStartIndexMap["DummyEventLog"]);
        Assert.AreEqual(-1, _eventLogSessionContext.EventLogContainerEndIndexMap["DummyEventLog"]);
    }
}

public sealed class DummyEventLogContainer : IEventLogContainer
{
    public DummyEventLogContainer(bool initialize)
    {
        EventLogEntries = new List<EventLogEntry>(10);
        EventLog = new("Application");

        if (initialize)
        {
            int currentIndex = EventLog.Entries[EventLog.Entries.Count - 1].Index - EventLog.Entries[0].Index;
            EventLogEntries.Add(EventLog.Entries[currentIndex]);
            EventLogEntries.Add(EventLog.Entries[currentIndex - 1]);
        }
    }

    public void Dispose()
    {
    }

    public EventLog EventLog { get; }

    public List<EventLogEntry> EventLogEntries { get; set; }

    public void OnEventLogEntryWritten(object? source, EntryWrittenEventArgs? e)
    {
    }
}
