// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.TestPlatform.Extensions.EventLogCollector.UnitTests;

[TestClass]
[Ignore] // "TODO: Gets stuck when running with other tests."
public class EventLogContainerTests
{
    private readonly HashSet<string> _eventSources;
    private readonly HashSet<EventLogEntryType> _entryTypes;
    private readonly Mock<DataCollectionLogger> _logger;
    private readonly DataCollectionContext _dataCollectionContext;
    private readonly EventLog _eventLog;
    private readonly EntryWrittenEventArgs _entryWrittenEventArgs;
    private readonly string _eventLogName = "Application";

    private EventLogContainer _eventLogContainer;

    public EventLogContainerTests()
    {
        _eventSources = new HashSet<string>
        {
            "Application"
        };
        _entryTypes = new HashSet<EventLogEntryType>
        {
            EventLogEntryType.Error
        };

        _logger = new Mock<DataCollectionLogger>();
        _eventLog = new EventLog("Application");
        _entryWrittenEventArgs = new EntryWrittenEventArgs(_eventLog.Entries[_eventLog.Entries.Count - 1]);

        _dataCollectionContext = new DataCollectionContext(new SessionId(Guid.NewGuid()));

        _eventLogContainer = new EventLogContainer(
            _eventLogName,
            _eventSources,
            _entryTypes,
            int.MaxValue,
            _logger.Object,
            _dataCollectionContext);
    }

    [TestMethod]
    [Ignore]
    public void OnEventLogEntryWrittenShouldAddLogs()
    {
        EventLog.WriteEntry("Application", "Application", EventLogEntryType.Error, 234);
        _eventLogContainer.OnEventLogEntryWritten(_eventLog, _entryWrittenEventArgs);
        var newCount = _eventLogContainer.EventLogEntries.Count;

        Assert.IsTrue(newCount > 0);
    }

    [TestMethod]
    public void OnEventLogEntryWrittenShouldNotAddLogsIfNoNewEntryIsPresent()
    {
        _eventLogContainer.OnEventLogEntryWritten(_eventLog, _entryWrittenEventArgs);
        var newCount = _eventLogContainer.EventLogEntries.Count;

        Assert.AreEqual(0, newCount);
    }

    [TestMethod]
    [Ignore] // "TODO: gets stuck"
    public void OnEventLogEntryWrittenShouldFilterLogsBasedOnEventTypeAndEventSource()
    {
        _entryTypes.Add(EventLogEntryType.Warning);
        _eventSources.Add("Application");

        EventLog.WriteEntry("Application", "Application", EventLogEntryType.Warning, 234);
        _eventLogContainer.OnEventLogEntryWritten(_eventLog, _entryWrittenEventArgs);
        var newCount = _eventLogContainer.EventLogEntries.Count;

        Assert.AreEqual(1, newCount);
    }

    [TestMethod]
    public void OnEventLogEntryWrittenShouldNotAddLogsIfEventSourceIsDifferent()
    {
        _eventSources.Clear();
        _eventSources.Add("Application1");
        _eventLogContainer = new EventLogContainer(
            _eventLogName,
            _eventSources,
            _entryTypes,
            int.MaxValue,
            _logger.Object,
            _dataCollectionContext);
        EventLog.WriteEntry("Application", "Application", EventLogEntryType.Warning, 234);
        _eventLogContainer.OnEventLogEntryWritten(_eventLog, _entryWrittenEventArgs);
        var newCount = _eventLogContainer.EventLogEntries.Count;

        Assert.AreEqual(0, newCount);
    }

    [TestMethod]
    public void OnEventLogEntryWrittenShouldNotAddLogsIfEventTypeIsDifferent()
    {
        _entryTypes.Clear();
        _entryTypes.Add(EventLogEntryType.FailureAudit);

        _eventSources.Add("Application1");
        _eventLogContainer = new EventLogContainer(
            _eventLogName,
            _eventSources,
            _entryTypes,
            int.MaxValue,
            _logger.Object,
            _dataCollectionContext);

        EventLog.WriteEntry("Application", "Application", EventLogEntryType.Warning, 234);
        _eventLogContainer.OnEventLogEntryWritten(_eventLog, _entryWrittenEventArgs);
        var newCount = _eventLogContainer.EventLogEntries.Count;

        Assert.AreEqual(0, newCount);
    }
}
