// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

using Resource = Microsoft.TestPlatform.Extensions.EventLogCollector.Resources.Resources;

namespace Microsoft.TestPlatform.Extensions.EventLogCollector;

/// <summary>
/// A data collector that collects event log data
/// </summary>
[DataCollectorTypeUri(DefaultUri)]
[DataCollectorFriendlyName("Event Log")]
public class EventLogDataCollector : DataCollector
{
    /// <summary>
    /// The event log file name.
    /// </summary>
    private const string EventLogFileName = "Event Log";

    /// <summary>
    /// DataCollector URI.
    /// </summary>
    private const string DefaultUri = @"datacollector://Microsoft/EventLog/2.0";

    /// <summary>
    /// Event handler delegate for the SessionStart event
    /// </summary>
    private readonly EventHandler<SessionStartEventArgs> _sessionStartEventHandler;

    /// <summary>
    /// Event handler delegate for the SessionEnd event
    /// </summary>
    private readonly EventHandler<SessionEndEventArgs> _sessionEndEventHandler;

    /// <summary>
    /// Event handler delegate for the TestCaseStart event
    /// </summary>
    private readonly EventHandler<TestCaseStartEventArgs> _testCaseStartEventHandler;

    /// <summary>
    /// Event handler delegate for the TestCaseEnd event
    /// </summary>
    private readonly EventHandler<TestCaseEndEventArgs> _testCaseEndEventHandler;

    /// <summary>
    /// The event log directories.
    /// </summary>
    private readonly List<string> _eventLogDirectories;

    /// <summary>
    /// Object containing the execution events the data collector registers for
    /// </summary>
    private DataCollectionEvents? _events;

    /// <summary>
    /// The sink used by the data collector to send its data
    /// </summary>
    private DataCollectionSink? _dataSink;

    /// <summary>
    /// The data collector context.
    /// </summary>
    private DataCollectionContext? _dataCollectorContext;

    /// <summary>
    /// Used by the data collector to send warnings, errors, or other messages
    /// </summary>
    private DataCollectionLogger? _logger;

    /// <summary>
    /// The file helper.
    /// </summary>
    private readonly IFileHelper _fileHelper;

    /// <summary>
    /// The event log map.
    /// </summary>
    private readonly IDictionary<string, IEventLogContainer> _eventLogContainerMap = new Dictionary<string, IEventLogContainer>();

    private bool _isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventLogDataCollector"/> class.
    /// </summary>
    public EventLogDataCollector()
        : this(new FileHelper())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EventLogDataCollector"/> class.
    /// </summary>
    /// <param name="fileHelper">
    /// File Helper.
    /// </param>
    internal EventLogDataCollector(IFileHelper fileHelper)
    {
        _sessionStartEventHandler = OnSessionStart;
        _sessionEndEventHandler = OnSessionEnd;
        _testCaseStartEventHandler = OnTestCaseStart;
        _testCaseEndEventHandler = OnTestCaseEnd;

        _eventLogDirectories = new List<string>();
        ContextMap = new Dictionary<DataCollectionContext, EventLogSessionContext>();
        _fileHelper = fileHelper;
    }

    internal int MaxEntries { get; private set; }

    internal ISet<string>? EventSources { get; private set; }

    internal ISet<EventLogEntryType>? EntryTypes { get; private set; }

    internal ISet<string>? EventLogNames { get; private set; }

    /// <summary>
    /// Gets the context data.
    /// </summary>
    internal Dictionary<DataCollectionContext, EventLogSessionContext> ContextMap { get; }

    #region DataCollector Members

    /// <summary>
    /// Initializes the data collector
    /// </summary>
    /// <param name="configurationElement">
    /// The XML element containing configuration information for the data collector. Currently,
    /// this data collector does not have any configuration, so we ignore this parameter.
    /// </param>
    /// <param name="events">
    /// Object containing the execution events the data collector registers for
    /// </param>
    /// <param name="dataSink">The sink used by the data collector to send its data</param>
    /// <param name="logger">
    /// Used by the data collector to send warnings, errors, or other messages
    /// </param>
    /// <param name="dataCollectionEnvironmentContext">Provides contextual information about the agent environment</param>
    [MemberNotNull(nameof(_events), nameof(_dataSink), nameof(_logger), nameof(_dataCollectorContext))]
    public override void Initialize(
        XmlElement? configurationElement,
        DataCollectionEvents events,
        DataCollectionSink dataSink,
        DataCollectionLogger logger,
        DataCollectionEnvironmentContext? dataCollectionEnvironmentContext)
    {
        ValidateArg.NotNull(events, nameof(events));
        ValidateArg.NotNull(dataSink, nameof(dataSink));
        ValidateArg.NotNull(logger, nameof(logger));
        ValidateArg.NotNull(dataCollectionEnvironmentContext, nameof(dataCollectionEnvironmentContext));

        _events = events;
        _dataSink = dataSink;
        _logger = logger;
        _dataCollectorContext = dataCollectionEnvironmentContext!.SessionDataCollectionContext;

        // Load the configuration
        CollectorNameValueConfigurationManager nameValueSettings =
            new(configurationElement);

        // Apply the configuration
        ConfigureEventSources(nameValueSettings);
        ConfigureEntryTypes(nameValueSettings);
        ConfigureMaxEntries(nameValueSettings);
        ConfigureEventLogNames(nameValueSettings, _dataCollectorContext);

        // Register for events
        events.SessionStart += _sessionStartEventHandler;
        events.SessionEnd += _sessionEndEventHandler;
        events.TestCaseStart += _testCaseStartEventHandler;
        events.TestCaseEnd += _testCaseEndEventHandler;
    }

    #endregion

    /// <summary>
    /// The write event logs.
    /// </summary>
    /// <param name="eventLogEntries">
    /// The event log entries.
    /// </param>
    /// <param name="maxLogEntries">
    /// Max Log Entries.
    /// </param>
    /// <param name="dataCollectionContext">
    /// The data collection context.
    /// </param>
    /// <param name="requestedDuration">
    /// The requested duration.
    /// </param>
    /// <param name="timeRequestReceived">
    /// The time request received.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    internal string WriteEventLogs(List<EventLogEntry> eventLogEntries, int maxLogEntries, DataCollectionContext dataCollectionContext, TimeSpan requestedDuration, DateTime timeRequestReceived)
    {
        // Generate a unique but friendly Directory name in the temp directory
        string eventLogDirName = string.Format(
            CultureInfo.InvariantCulture,
            "{0}-{1}-{2:yyyy}{2:MM}{2:dd}-{2:HH}{2:mm}{2:ss}.{2:fff}",
            "Event Log",
            Environment.MachineName,
            DateTime.UtcNow);

        string eventLogDirPath = Path.Combine(Path.GetTempPath(), eventLogDirName);

        // Create the directory
        _fileHelper.CreateDirectory(eventLogDirPath);

        string eventLogBasePath = Path.Combine(eventLogDirPath, EventLogFileName);
        bool unusedFilenameFound = false;

        string eventLogPath = eventLogBasePath + ".xml";

        if (_fileHelper.Exists(eventLogPath))
        {
            for (int i = 1; !unusedFilenameFound; i++)
            {
                eventLogPath = $"{eventLogBasePath}-{i.ToString(CultureInfo.InvariantCulture)}.xml";

                if (!_fileHelper.Exists(eventLogPath))
                {
                    unusedFilenameFound = true;
                }
            }
        }

        DateTime minDate = DateTime.MinValue;

        // Limit entries to a certain time range if requested
        if (requestedDuration < TimeSpan.MaxValue)
        {
            try
            {
                minDate = timeRequestReceived - requestedDuration;
            }
            catch (ArgumentOutOfRangeException)
            {
                minDate = DateTime.MinValue;
            }
        }

        Stopwatch stopwatch = new();
        stopwatch.Start();
        EventLogXmlWriter.WriteEventLogEntriesToXmlFile(
            eventLogPath,
            eventLogEntries.Where(
                entry => entry.TimeGenerated > minDate && entry.TimeGenerated < DateTime.MaxValue).OrderBy(x => x.TimeGenerated).Take(maxLogEntries).ToList(),
            _fileHelper);

        stopwatch.Stop();

        EqtTrace.Verbose(
            "EventLogDataContainer: Wrote {0} event log entries to file '{1}' in {2} seconds",
            eventLogEntries.Count,
            eventLogPath,
            stopwatch.Elapsed.TotalSeconds.ToString(CultureInfo.InvariantCulture));

        // Write the event log file
        FileTransferInformation fileTransferInformation =
            new(dataCollectionContext, eventLogPath, true, _fileHelper);
        TPDebug.Assert(_dataSink != null, "Initialize should have been called.");
        _dataSink.SendFileAsync(fileTransferInformation);

        EqtTrace.Verbose(
            "EventLogDataContainer: Event log successfully sent for data collection context '{0}'.",
            dataCollectionContext.ToString());

        return eventLogPath;
    }

    #region IDisposable Members

    /// <summary>
    /// Cleans up resources allocated by the data collector
    /// </summary>
    /// <param name="disposing">Not used since this class does not have a finalizer.</param>
    protected override void Dispose(bool disposing)
    {
        if (_isDisposed)
            return;

        base.Dispose(disposing);

        if (disposing)
        {
            // Unregister events
            if (_events != null)
            {
                _events.SessionStart -= _sessionStartEventHandler;
                _events.SessionEnd -= _sessionEndEventHandler;
                _events.TestCaseStart -= _testCaseStartEventHandler;
                _events.TestCaseEnd -= _testCaseEndEventHandler;
            }

            // Unregister EventLogEntry Written.
            foreach (var eventLogContainer in _eventLogContainerMap.Values)
            {
                eventLogContainer.Dispose();
            }

            // Delete all the temp event log directories
            RemoveTempEventLogDirs(_eventLogDirectories);
        }

        _isDisposed = true;
    }

    #endregion

    private static ISet<string> ParseCommaSeparatedList(string commaSeparatedList)
    {
        ISet<string> strings = new HashSet<string>();
        string[] items = commaSeparatedList.Split(new char[] { ',' });
        foreach (string item in items)
        {
            strings.Add(item.Trim());
        }

        return strings;
    }

    private void OnSessionStart(object? sender, SessionStartEventArgs e)
    {
        ValidateArg.NotNull(e, nameof(e));
        ValidateArg.NotNull(e.Context, "SessionStartEventArgs.Context");

        EqtTrace.Verbose("EventLogDataCollector: SessionStart received");

        StartCollectionForContext(e.Context);
    }

    private void OnSessionEnd(object? sender, SessionEndEventArgs e)
    {
        ValidateArg.NotNull(e, nameof(e));
        ValidateArg.NotNull(e.Context, "SessionEndEventArgs.Context");

        EqtTrace.Verbose("EventLogDataCollector: SessionEnd received");

        WriteCollectedEventLogEntries(e.Context, true, TimeSpan.MaxValue, DateTime.UtcNow);
    }

    private void OnTestCaseStart(object? sender, TestCaseStartEventArgs e)
    {
        ValidateArg.NotNull(e, nameof(e));
        ValidateArg.NotNull(e.Context, "TestCaseStartEventArgs.Context");

        if (!e.Context.HasTestCase)
        {
            Debug.Fail("Context is not for a test case");
            ValidateArg.NotNull(e.Context.TestExecId, "TestCaseStartEventArgs.Context.HasTestCase");
        }

        EqtTrace.Verbose("EventLogDataCollector: TestCaseStart received for test '{0}'.", e.TestCaseName);

        StartCollectionForContext(e.Context);
    }

    private void OnTestCaseEnd(object? sender, TestCaseEndEventArgs e)
    {
        ValidateArg.NotNull(e, nameof(e));
        TPDebug.Assert(e.Context != null, "Context is null");
        TPDebug.Assert(e.Context.HasTestCase, "Context is not for a test case");

        EqtTrace.Verbose(
            "EventLogDataCollector: TestCaseEnd received for test '{0}' with Test Outcome: {1}.",
            e.TestCaseName,
            e.TestOutcome);

        WriteCollectedEventLogEntries(e.Context, false, TimeSpan.MaxValue, DateTime.UtcNow);
    }

    private void RemoveTempEventLogDirs(List<string> tempDirs)
    {
        if (tempDirs != null)
        {
            foreach (string dir in tempDirs)
            {
                // Delete only if the directory is empty
                _fileHelper.DeleteEmptyDirectroy(dir);
            }
        }
    }

    private void StartCollectionForContext(DataCollectionContext dataCollectionContext)
    {
        lock (ContextMap)
        {
            var eventLogSessionContext = new EventLogSessionContext(_eventLogContainerMap);
            ContextMap.Add(dataCollectionContext, eventLogSessionContext);
        }
    }

    private void WriteCollectedEventLogEntries(
        DataCollectionContext dataCollectionContext,
        bool isSessionEnd,
        TimeSpan requestedDuration,
        DateTime timeRequestReceived)
    {
        var context = GetEventLogSessionContext(dataCollectionContext);
        context.CreateEventLogContainerEndIndexMap();

        List<EventLogEntry> eventLogEntries = new();
        foreach (KeyValuePair<string, IEventLogContainer> kvp in _eventLogContainerMap)
        {
            try
            {
                if (isSessionEnd)
                {
                    kvp.Value.EventLog.EnableRaisingEvents = false;
                }

                for (int i = context.EventLogContainerStartIndexMap[kvp.Key]; i <= context.EventLogContainerEndIndexMap[kvp.Key]; i++)
                {
                    eventLogEntries.Add(kvp.Value.EventLogEntries[i]);
                }
            }
            catch (Exception e)
            {
                TPDebug.Assert(_logger != null, "Initialize should have been called.");
                _logger.LogWarning(
                    dataCollectionContext,
                    string.Format(
                        CultureInfo.InvariantCulture,
                        Resource.CleanupException,
                        kvp.Value.EventLog,
                        e.ToString()));
            }
        }

        var fileName = WriteEventLogs(eventLogEntries, isSessionEnd ? int.MaxValue : MaxEntries, dataCollectionContext, requestedDuration, timeRequestReceived);

        // Add the directory to the list
        _eventLogDirectories.Add(Path.GetDirectoryName(fileName)!);

        lock (ContextMap)
        {
            ContextMap.Remove(dataCollectionContext);
        }
    }

    [MemberNotNull(nameof(EventLogNames))]
    private void ConfigureEventLogNames(CollectorNameValueConfigurationManager collectorNameValueConfigurationManager, DataCollectionContext dataCollectorContext)
    {
        EventLogNames = new HashSet<string>();
        string? eventLogs = collectorNameValueConfigurationManager[EventLogConstants.SettingEventLogs];
        if (eventLogs != null)
        {
            EventLogNames = ParseCommaSeparatedList(eventLogs);
            EqtTrace.Verbose(
                $"EventLogDataCollector configuration: {EventLogConstants.SettingEventLogs}={eventLogs}");
        }
        else
        {
            // Default to collecting these standard logs
            EventLogNames.Add("System");
            EventLogNames.Add("Application");
        }

        TPDebug.Assert(_logger != null && EntryTypes != null, "Initialize should have been called.");

        foreach (string eventLogName in EventLogNames)
        {
            try
            {
                // Create an EventLog object and add it to the eventLogContext if one does not already exist
                if (!_eventLogContainerMap.ContainsKey(eventLogName))
                {
                    IEventLogContainer eventLogContainer = new EventLogContainer(
                        eventLogName,
                        EventSources,
                        EntryTypes,
                        int.MaxValue,
                        _logger,
                        dataCollectorContext);
                    _eventLogContainerMap.Add(eventLogName, eventLogContainer);
                }

                EqtTrace.Verbose("EventLogDataCollector: Created EventSource '{0}'", eventLogName);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    dataCollectorContext,
                    new EventLogCollectorException(string.Format(CultureInfo.CurrentCulture, Resource.ReadError, eventLogName, Environment.MachineName), ex));
            }
        }
    }

    private void ConfigureEventSources(CollectorNameValueConfigurationManager collectorNameValueConfigurationManager)
    {
        string? eventSourcesStr = collectorNameValueConfigurationManager[EventLogConstants.SettingEventSources];
        if (!eventSourcesStr.IsNullOrEmpty())
        {
            EventSources = ParseCommaSeparatedList(eventSourcesStr);
            EqtTrace.Verbose(
                $"EventLogDataCollector configuration: {EventLogConstants.SettingEventSources}={EventSources}");
        }
    }

    [MemberNotNull(nameof(EntryTypes))]
    private void ConfigureEntryTypes(CollectorNameValueConfigurationManager collectorNameValueConfigurationManager)
    {
        EntryTypes = new HashSet<EventLogEntryType>();
        string? entryTypesStr = collectorNameValueConfigurationManager[EventLogConstants.SettingEntryTypes];
        if (entryTypesStr != null)
        {
            foreach (string entryTypestring in ParseCommaSeparatedList(entryTypesStr))
            {
                EntryTypes.Add(
                    (EventLogEntryType)Enum.Parse(typeof(EventLogEntryType), entryTypestring, true));
            }

            EqtTrace.Verbose(
                $"EventLogDataCollector configuration: {EventLogConstants.SettingEntryTypes}={EntryTypes}");
        }
        else
        {
            EntryTypes.Add(EventLogEntryType.Error);
            EntryTypes.Add(EventLogEntryType.Warning);
            EntryTypes.Add(EventLogEntryType.FailureAudit);
        }
    }

    private void ConfigureMaxEntries(CollectorNameValueConfigurationManager collectorNameValueConfigurationManager)
    {
        string? maxEntriesstring = collectorNameValueConfigurationManager[EventLogConstants.SettingMaxEntries];
        if (maxEntriesstring != null)
        {
            try
            {
                MaxEntries = int.Parse(maxEntriesstring, CultureInfo.InvariantCulture);

                // A negative or 0 value means no maximum
                if (MaxEntries <= 0)
                {
                    MaxEntries = int.MaxValue;
                }
            }
            catch (FormatException)
            {
                MaxEntries = EventLogConstants.DefaultMaxEntries;
            }

            EqtTrace.Verbose(
                $"EventLogDataCollector configuration: {EventLogConstants.SettingMaxEntries}={MaxEntries}");
        }
        else
        {
            MaxEntries = EventLogConstants.DefaultMaxEntries;
        }
    }

    private EventLogSessionContext GetEventLogSessionContext(DataCollectionContext dataCollectionContext)
    {
        lock (ContextMap)
        {
            if (ContextMap.TryGetValue(dataCollectionContext, out var eventLogSessionContext))
            {
                return eventLogSessionContext;
            }
        }

        string msg = string.Format(
                CultureInfo.CurrentCulture,
                Resource.ContextNotFoundException,
                dataCollectionContext.ToString());
        throw new EventLogCollectorException(msg, null);
    }

}
