// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.TestPlatform.Extensions.EventLogCollector.UnitTests;

[TestClass]
public class EventLogDataCollectorTests
{
    private readonly Mock<DataCollectionEvents> _mockDataCollectionEvents;
    private readonly TestableDataCollectionSink _mockDataCollectionSink;
    private readonly Mock<DataCollectionLogger> _mockDataCollectionLogger;
    private readonly DataCollectionEnvironmentContext _dataCollectionEnvironmentContext;
    private readonly EventLogDataCollector _eventLogDataCollector;
    private readonly Mock<IFileHelper> _mockFileHelper;

    public EventLogDataCollectorTests()
    {
        _mockDataCollectionEvents = new Mock<DataCollectionEvents>();
        _mockDataCollectionSink = new TestableDataCollectionSink();
        _mockFileHelper = new Mock<IFileHelper>();
        DataCollectionContext dataCollectionContext =
            new(new SessionId(Guid.NewGuid()));
        _dataCollectionEnvironmentContext = new DataCollectionEnvironmentContext(dataCollectionContext);
        _mockDataCollectionLogger = new Mock<DataCollectionLogger>();
        _eventLogDataCollector = new EventLogDataCollector(_mockFileHelper.Object);
    }

    [TestMethod]
    public void EventLoggerLogsErrorForInvalidEventSources()
    {
        string configurationString =
            @"<Configuration><Setting name=""EventLogs"" value=""MyEventName"" /></Configuration>";
        XmlDocument expectedXmlDoc = new();
        expectedXmlDoc.LoadXml(configurationString);
        var mockCollector = new Mock<DataCollectionLogger>();
        mockCollector.Setup(m => m.LogError(It.IsAny<DataCollectionContext>(), It.Is<string>(s => s.Contains(@"The event log 'MyEventName' on computer '.' does not exist.")), It.IsAny<Exception>()));

        var eventLogDataCollector = new EventLogDataCollector();
        eventLogDataCollector.Initialize(expectedXmlDoc.DocumentElement, _mockDataCollectionEvents.Object, _mockDataCollectionSink, mockCollector.Object, _dataCollectionEnvironmentContext);

        mockCollector.Verify(m => m.LogError(It.IsAny<DataCollectionContext>(), It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
    }

    [TestMethod]
    public void InitializeShouldThrowExceptionIfEventsIsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(
            () => _eventLogDataCollector.Initialize(
                null,
                null!,
                _mockDataCollectionSink,
                _mockDataCollectionLogger.Object,
                _dataCollectionEnvironmentContext));
    }

    [TestMethod]
    public void InitializeShouldThrowExceptionIfCollectionSinkIsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(
            () => _eventLogDataCollector.Initialize(
                null,
                _mockDataCollectionEvents.Object,
                null!,
                _mockDataCollectionLogger.Object,
                _dataCollectionEnvironmentContext));
    }

    [TestMethod]
    public void InitializeShouldThrowExceptionIfLoggerIsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(
            () => _eventLogDataCollector.Initialize(
                null,
                _mockDataCollectionEvents.Object,
                _mockDataCollectionSink,
                null!,
                _dataCollectionEnvironmentContext));
    }

    [TestMethod]
    public void InitializeShouldInitializeDefaultEventLogNames()
    {
        List<string> eventLogNames = new()
        {
            "System",
            "Application"
        };

        _eventLogDataCollector.Initialize(null, _mockDataCollectionEvents.Object, _mockDataCollectionSink, _mockDataCollectionLogger.Object, _dataCollectionEnvironmentContext);

        CollectionAssert.AreEqual(eventLogNames, _eventLogDataCollector.EventLogNames.ToList());
    }

    [TestMethod]
    public void InitializeShouldInitializeCustomEventLogNamesIfSpecifiedInConfiguration()
    {
        string configurationString =
            @"<Configuration><Setting name=""EventLogs"" value=""MyEventName,MyEventName2"" /></Configuration>";

        List<string> eventLogNames = new()
        {
            "MyEventName",
            "MyEventName2"
        };

        XmlDocument expectedXmlDoc = new();
        expectedXmlDoc.LoadXml(configurationString);

        _eventLogDataCollector.Initialize(expectedXmlDoc.DocumentElement, _mockDataCollectionEvents.Object, _mockDataCollectionSink, _mockDataCollectionLogger.Object, _dataCollectionEnvironmentContext);

        CollectionAssert.AreEqual(eventLogNames, _eventLogDataCollector.EventLogNames.ToList());
    }

    [TestMethod]
    public void InitializeShouldInitializeDefaultLogEntryTypes()
    {
        List<EventLogEntryType> entryTypes = new()
        {
            EventLogEntryType.Error,
            EventLogEntryType.Warning,
            EventLogEntryType.FailureAudit
        };

        _eventLogDataCollector.Initialize(null, _mockDataCollectionEvents.Object, _mockDataCollectionSink, _mockDataCollectionLogger.Object, _dataCollectionEnvironmentContext);

        CollectionAssert.AreEqual(entryTypes, _eventLogDataCollector.EntryTypes.ToList());
    }

    [TestMethod]
    public void InitializeShouldInitializeEntryTypesIfSpecifiedInConfiguration()
    {
        string configurationString =
            @"<Configuration><Setting name=""EntryTypes"" value=""Error"" /></Configuration>";

        List<EventLogEntryType> entryTypes = new()
        {
            EventLogEntryType.Error
        };

        XmlDocument expectedXmlDoc = new();
        expectedXmlDoc.LoadXml(configurationString);
        _eventLogDataCollector.Initialize(expectedXmlDoc.DocumentElement, _mockDataCollectionEvents.Object, _mockDataCollectionSink, _mockDataCollectionLogger.Object, _dataCollectionEnvironmentContext);

        CollectionAssert.AreEqual(entryTypes, _eventLogDataCollector.EntryTypes.ToList());
    }

    [TestMethod]
    public void InitializeShouldInitializeEventSourcesIfSpecifiedInConfiguration()
    {
        string configurationString =
            @"<Configuration><Setting name=""EventSources"" value=""MyEventSource"" /></Configuration>";

        List<string> eventSources = new()
        {
            "MyEventSource"
        };

        XmlDocument expectedXmlDoc = new();
        expectedXmlDoc.LoadXml(configurationString);
        _eventLogDataCollector.Initialize(expectedXmlDoc.DocumentElement, _mockDataCollectionEvents.Object, _mockDataCollectionSink, _mockDataCollectionLogger.Object, _dataCollectionEnvironmentContext);

        CollectionAssert.AreEqual(eventSources, _eventLogDataCollector.EventSources.ToList());
    }

    [TestMethod]
    public void InitializeShouldNotInitializeEventSourcesByDefault()
    {
        _eventLogDataCollector.Initialize(null, _mockDataCollectionEvents.Object, _mockDataCollectionSink, _mockDataCollectionLogger.Object, _dataCollectionEnvironmentContext);

        Assert.IsNull(_eventLogDataCollector.EventSources);
    }

    [TestMethod]
    public void InitializeShouldInitializeMaxEntriesIfSpecifiedInConfiguration()
    {
        string configurationString =
            @"<Configuration><Setting name=""MaxEventLogEntriesToCollect"" value=""20"" /></Configuration>";

        XmlDocument expectedXmlDoc = new();
        expectedXmlDoc.LoadXml(configurationString);
        _eventLogDataCollector.Initialize(expectedXmlDoc.DocumentElement, _mockDataCollectionEvents.Object, _mockDataCollectionSink, _mockDataCollectionLogger.Object, _dataCollectionEnvironmentContext);

        Assert.AreEqual(20, _eventLogDataCollector.MaxEntries);
    }

    [TestMethod]
    public void InitializeShouldSetDefaultMaxEntries()
    {
        _eventLogDataCollector.Initialize(null, _mockDataCollectionEvents.Object, _mockDataCollectionSink, _mockDataCollectionLogger.Object, _dataCollectionEnvironmentContext);

        Assert.AreEqual(50000, _eventLogDataCollector.MaxEntries);
    }

    [TestMethod]
    public void InitializeShouldSubscribeToDataCollectionEvents()
    {
        var testableDataCollectionEvents = new TestableDataCollectionEvents();
        _eventLogDataCollector.Initialize(null, testableDataCollectionEvents, _mockDataCollectionSink, _mockDataCollectionLogger.Object, _dataCollectionEnvironmentContext);
        Assert.AreEqual(1, testableDataCollectionEvents.GetTestCaseStartInvocationList()?.Length, "GetTestCaseStartInvocationList");
        Assert.AreEqual(1, testableDataCollectionEvents.GetTestCaseEndInvocationList()?.Length, "GetTestCaseEndInvocationList");
        Assert.AreEqual(1, testableDataCollectionEvents.GetTestSessionEndInvocationList()?.Length, "GetTestSessionEndInvocationList");
        Assert.AreEqual(1, testableDataCollectionEvents.GetTestSessionStartInvocationList()?.Length, "GetTestSessionStartInvocationList");
    }

    [TestMethod]
    public void TestSessionStartEventShouldCreateEventLogContainer()
    {
        var eventLogDataCollector = new EventLogDataCollector();
        Assert.AreEqual(0, eventLogDataCollector.ContextMap.Count);
        eventLogDataCollector.Initialize(null, _mockDataCollectionEvents.Object, _mockDataCollectionSink, _mockDataCollectionLogger.Object, _dataCollectionEnvironmentContext);
        _mockDataCollectionEvents.Raise(x => x.SessionStart += null, new SessionStartEventArgs());
        Assert.AreEqual(1, eventLogDataCollector.ContextMap.Count);
    }

    [TestMethod]
    public void TestCaseStartEventShouldCreateEventLogContainer()
    {
        var eventLogDataCollector = new EventLogDataCollector();
        Assert.AreEqual(0, eventLogDataCollector.ContextMap.Count);

        eventLogDataCollector.Initialize(null, _mockDataCollectionEvents.Object, _mockDataCollectionSink, _mockDataCollectionLogger.Object, _dataCollectionEnvironmentContext);
        _mockDataCollectionEvents.Raise(x => x.TestCaseStart += null, new TestCaseStartEventArgs(new DataCollectionContext(new SessionId(Guid.NewGuid()), new TestExecId(Guid.NewGuid())), new TestCase()));
        Assert.AreEqual(1, eventLogDataCollector.ContextMap.Count);
    }

    [TestMethod]

    public void TestCaseEndEventShouldWriteEventLogEntriesAndSendFile()
    {
        var eventLogDataCollector = new EventLogDataCollector();
        eventLogDataCollector.Initialize(null, _mockDataCollectionEvents.Object, _mockDataCollectionSink, _mockDataCollectionLogger.Object, _dataCollectionEnvironmentContext);
        var tc = new TestCase();
        var context = new DataCollectionContext(new SessionId(Guid.NewGuid()), new TestExecId(Guid.NewGuid()));
        _mockDataCollectionEvents.Raise(x => x.TestCaseStart += null, new TestCaseStartEventArgs(context, tc));
        _mockDataCollectionEvents.Raise(x => x.TestCaseEnd += null, new TestCaseEndEventArgs(context, tc, TestOutcome.Passed));
        Assert.IsTrue(_mockDataCollectionSink.IsSendFileAsyncInvoked);
    }

    public void TestCaseEndEventShouldInvokeSendFileAsync()
    {
        var eventLogDataCollector = new EventLogDataCollector();
        eventLogDataCollector.Initialize(null, _mockDataCollectionEvents.Object, _mockDataCollectionSink, _mockDataCollectionLogger.Object, _dataCollectionEnvironmentContext);
        var tc = new TestCase();
        var context = new DataCollectionContext(new SessionId(Guid.NewGuid()), new TestExecId(Guid.NewGuid()));
        _mockDataCollectionEvents.Raise(x => x.TestCaseStart += null, new TestCaseStartEventArgs(context, tc));
        _mockDataCollectionEvents.Raise(x => x.TestCaseEnd += null, new TestCaseEndEventArgs(context, tc, TestOutcome.Passed));
        Assert.IsTrue(_mockDataCollectionSink.IsSendFileAsyncInvoked);
    }

    [TestMethod]
    public void TestCaseEndEventShouldThrowIfTestCaseStartIsNotInvoked()
    {
        var eventLogDataCollector = new EventLogDataCollector();
        eventLogDataCollector.Initialize(null, _mockDataCollectionEvents.Object, _mockDataCollectionSink, _mockDataCollectionLogger.Object, _dataCollectionEnvironmentContext);
        var tc = new TestCase();
        var context = new DataCollectionContext(new SessionId(Guid.NewGuid()), new TestExecId(Guid.NewGuid()));

        Assert.ThrowsException<EventLogCollectorException>(() => _mockDataCollectionEvents.Raise(x => x.TestCaseEnd += null, new TestCaseEndEventArgs(context, tc, TestOutcome.Passed)));
    }

    public void SessionEndEventShouldThrowIfSessionStartEventtIsNotInvoked()
    {
        var eventLogDataCollector = new EventLogDataCollector();
        eventLogDataCollector.Initialize(null, _mockDataCollectionEvents.Object, _mockDataCollectionSink, _mockDataCollectionLogger.Object, _dataCollectionEnvironmentContext);
        var tc = new TestCase();

        Assert.ThrowsException<EventLogCollectorException>(() => _mockDataCollectionEvents.Raise(x => x.SessionEnd += null, new SessionEndEventArgs(_dataCollectionEnvironmentContext.SessionDataCollectionContext)));
    }

    [TestMethod]
    public void TestSessionEndEventShouldWriteEventLogEntriesAndSendFile()
    {
        var eventLogDataCollector = new EventLogDataCollector();
        eventLogDataCollector.Initialize(null, _mockDataCollectionEvents.Object, _mockDataCollectionSink, _mockDataCollectionLogger.Object, _dataCollectionEnvironmentContext);
        var testcase = new TestCase() { Id = Guid.NewGuid() };
        _mockDataCollectionEvents.Raise(x => x.SessionStart += null, new SessionStartEventArgs(_dataCollectionEnvironmentContext.SessionDataCollectionContext, new Dictionary<string, object?>()));
        _mockDataCollectionEvents.Raise(x => x.SessionEnd += null, new SessionEndEventArgs(_dataCollectionEnvironmentContext.SessionDataCollectionContext));
        Assert.IsTrue(_mockDataCollectionSink.IsSendFileAsyncInvoked);
    }

    [TestMethod]
    public void WriteEventLogsShouldCreateXmlFile()
    {
        string configurationString =
            @"<Configuration><Setting name=""MaxEventLogEntriesToCollect"" value=""20"" /><Setting name=""EventLog"" value=""Application"" /><Setting name=""EntryTypes"" value=""Warning"" /></Configuration>";

        XmlDocument expectedXmlDoc = new();
        expectedXmlDoc.LoadXml(configurationString);

        _mockFileHelper.SetupSequence(x => x.Exists(It.IsAny<string>())).Returns(false).Returns(true);
        _eventLogDataCollector.Initialize(expectedXmlDoc.DocumentElement, _mockDataCollectionEvents.Object, _mockDataCollectionSink, _mockDataCollectionLogger.Object, _dataCollectionEnvironmentContext);
        _eventLogDataCollector.WriteEventLogs(
            new List<EventLogEntry>(),
            20,
            _dataCollectionEnvironmentContext.SessionDataCollectionContext,
            TimeSpan.MaxValue,
            DateTime.Now);

        _mockFileHelper.Verify(x => x.WriteAllTextToFile(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public void WriteEventLogsShouldThrowExceptionIfThrownByFileHelper()
    {
        string configurationString =
            @"<Configuration><Setting name=""MaxEventLogEntriesToCollect"" value=""20"" /><Setting name=""EventLog"" value=""Application"" /><Setting name=""EntryTypes"" value=""Warning"" /></Configuration>";

        XmlDocument expectedXmlDoc = new();
        expectedXmlDoc.LoadXml(configurationString);
        _mockFileHelper.Setup(x => x.Exists(It.IsAny<string>())).Throws<Exception>();
        Assert.ThrowsException<Exception>(
            () => _eventLogDataCollector.WriteEventLogs(
                new List<EventLogEntry>(),
                20,
                _dataCollectionEnvironmentContext.SessionDataCollectionContext,
                TimeSpan.MaxValue,
                DateTime.Now));
    }

    [TestMethod]
    public void WriteEventLogsShouldFilterTestsBasedOnTimeAndMaxValue()
    {
        string configurationString =
            @"<Configuration><Setting name=""MaxEventLogEntriesToCollect"" value=""20"" /><Setting name=""EventLog"" value=""Application"" /><Setting name=""EntryTypes"" value=""Warning"" /></Configuration>";

        XmlDocument expectedXmlDoc = new();
        expectedXmlDoc.LoadXml(configurationString);

        _mockFileHelper.SetupSequence(x => x.Exists(It.IsAny<string>())).Returns(false).Returns(true);
        _eventLogDataCollector.Initialize(expectedXmlDoc.DocumentElement, _mockDataCollectionEvents.Object, _mockDataCollectionSink, _mockDataCollectionLogger.Object, _dataCollectionEnvironmentContext);

        var entries = new List<EventLogEntry>();

        var eventLog = new EventLog("Application");
        int endIndex = eventLog.Entries[eventLog.Entries.Count - 1].Index;
        int firstIndexInLog = eventLog.Entries[0].Index;
        for (int i = endIndex; i > endIndex - 10; i--)
        {
            entries.Add(eventLog.Entries[i - firstIndexInLog]);
        }

        var filteredEntries = entries.Where(entry => entry.TimeGenerated > DateTime.MinValue && entry.TimeGenerated < DateTime.MaxValue)
            .OrderBy(x => x.TimeGenerated).Take(5).ToList();

        _eventLogDataCollector.WriteEventLogs(
            entries,
            5,
            _dataCollectionEnvironmentContext.SessionDataCollectionContext,
            TimeSpan.MaxValue,
            DateTime.Now);

        _mockFileHelper.Verify(
            x => x.WriteAllTextToFile(
                It.IsAny<string>(),
                It.Is<string>(
                    str => str.Contains(filteredEntries[0].InstanceId.ToString(CultureInfo.CurrentCulture))
                           && str.Contains(filteredEntries[1].InstanceId.ToString(CultureInfo.CurrentCulture))
                           && str.Contains(filteredEntries[2].InstanceId.ToString(CultureInfo.CurrentCulture))
                           && str.Contains(filteredEntries[3].InstanceId.ToString(CultureInfo.CurrentCulture))
                           && str.Contains(filteredEntries[4].InstanceId.ToString(CultureInfo.CurrentCulture)))));
    }

    [TestMethod]
    public void WriteEventLogsShouldFilterTestIfMaxValueExceedsEntries()
    {
        string configurationString =
            @"<Configuration><Setting name=""MaxEventLogEntriesToCollect"" value=""20"" /><Setting name=""EventLog"" value=""Application"" /><Setting name=""EntryTypes"" value=""Warning"" /></Configuration>";

        XmlDocument expectedXmlDoc = new();
        expectedXmlDoc.LoadXml(configurationString);

        _mockFileHelper.SetupSequence(x => x.Exists(It.IsAny<string>())).Returns(false).Returns(true);
        _eventLogDataCollector.Initialize(expectedXmlDoc.DocumentElement, _mockDataCollectionEvents.Object, _mockDataCollectionSink, _mockDataCollectionLogger.Object, _dataCollectionEnvironmentContext);

        var entries = new List<EventLogEntry>();

        var eventLog = new EventLog("Application");
        int endIndex = eventLog.Entries[eventLog.Entries.Count - 1].Index;
        int firstIndexInLog = eventLog.Entries[0].Index;
        for (int i = endIndex; i > endIndex - 5; i--)
        {
            entries.Add(eventLog.Entries[i - firstIndexInLog]);
        }

        var filteredEntries = entries.Where(entry => entry.TimeGenerated > DateTime.MinValue && entry.TimeGenerated < DateTime.MaxValue)
            .OrderBy(x => x.TimeGenerated).Take(10).ToList();

        _eventLogDataCollector.WriteEventLogs(
            entries,
            5,
            _dataCollectionEnvironmentContext.SessionDataCollectionContext,
            TimeSpan.MaxValue,
            DateTime.Now);

        _mockFileHelper.Verify(
            x => x.WriteAllTextToFile(
                It.IsAny<string>(),
                It.Is<string>(
                    str => str.Contains(filteredEntries[0].InstanceId.ToString(CultureInfo.CurrentCulture))
                           && str.Contains(filteredEntries[1].InstanceId.ToString(CultureInfo.CurrentCulture))
                           && str.Contains(filteredEntries[2].InstanceId.ToString(CultureInfo.CurrentCulture))
                           && str.Contains(filteredEntries[3].InstanceId.ToString(CultureInfo.CurrentCulture))
                           && str.Contains(filteredEntries[4].InstanceId.ToString(CultureInfo.CurrentCulture)))));
    }
}

/// <summary>
/// The testable data collection events.
/// </summary>
public class TestableDataCollectionEvents : DataCollectionEvents
{
    public override event EventHandler<TestHostLaunchedEventArgs>? TestHostLaunched;
    public override event EventHandler<SessionStartEventArgs>? SessionStart;
    public override event EventHandler<SessionEndEventArgs>? SessionEnd;
    public override event EventHandler<TestCaseStartEventArgs>? TestCaseStart;
    public override event EventHandler<TestCaseEndEventArgs>? TestCaseEnd;

    public Delegate[] GetTestHostLaunchedInvocationList()
    {
        return TestHostLaunched!.GetInvocationList();
    }

    public Delegate[] GetTestCaseStartInvocationList()
    {
        return TestCaseStart!.GetInvocationList();
    }

    public Delegate[] GetTestCaseEndInvocationList()
    {
        return TestCaseEnd!.GetInvocationList();
    }

    public Delegate[] GetTestSessionStartInvocationList()
    {
        return SessionStart!.GetInvocationList();
    }

    public Delegate[] GetTestSessionEndInvocationList()
    {
        return SessionEnd!.GetInvocationList();
    }
}

/// <summary>
/// The testable data collection sink.
/// </summary>
public class TestableDataCollectionSink : DataCollectionSink
{
    /// <summary>
    /// The send file completed.
    /// </summary>
    public override event AsyncCompletedEventHandler? SendFileCompleted;

    /// <summary>
    /// Gets or sets a value indicating whether is send file async invoked.
    /// </summary>
    public bool IsSendFileAsyncInvoked { get; set; }

    public override void SendFileAsync(FileTransferInformation fileTransferInformation)
    {
        IsSendFileAsyncInvoked = true;
        if (SendFileCompleted == null)
        {
            return;
        }
    }
}
