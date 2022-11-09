// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

using Resource = Microsoft.TestPlatform.Extensions.EventLogCollector.Resources.Resources;

namespace Microsoft.TestPlatform.Extensions.EventLogCollector;

/// <summary>
/// The event log container.
/// </summary>
internal class EventLogContainer : IEventLogContainer
{
    private readonly ISet<string>? _eventSources;

    private readonly ISet<EventLogEntryType> _entryTypes;
    private readonly int _maxLogEntries;

    private readonly DataCollectionLogger _dataCollectionLogger;
    private readonly DataCollectionContext _dataCollectionContext;

    /// <summary>
    /// Keeps track of if we are disposed.
    /// </summary>
    private bool _isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventLogContainer"/> class.
    /// </summary>
    /// <param name="eventLogName">
    /// Event Log Name for which logs has to be collected.
    /// </param>
    /// <param name="eventSources">
    /// The event Sources.
    /// </param>
    /// <param name="entryTypes">
    /// The entry Types.
    /// </param>
    /// <param name="maxLogEntries">
    /// Max entries to store
    /// </param>
    /// <param name="dataCollectionLogger">
    /// Data Collection Logger
    /// </param>
    /// <param name="dataCollectionContext">
    /// Data Collection Context
    /// </param>
    public EventLogContainer(string eventLogName, ISet<string>? eventSources, ISet<EventLogEntryType> entryTypes, int maxLogEntries, DataCollectionLogger dataCollectionLogger, DataCollectionContext dataCollectionContext)
    {
        EventLog = new EventLog(eventLogName);
        EventLog.EnableRaisingEvents = true;
        EventLog.EntryWritten += OnEventLogEntryWritten;
        int currentCount = EventLog.Entries.Count;
        NextEntryIndexToCollect = currentCount == 0 ? 0 : EventLog.Entries[currentCount - 1].Index + 1;
        _eventSources = eventSources;
        _entryTypes = entryTypes;
        _maxLogEntries = maxLogEntries;
        _dataCollectionLogger = dataCollectionLogger;
        _dataCollectionContext = dataCollectionContext;

        EventLogEntries = new List<EventLogEntry>();
    }

    /// <inheritdoc />
    public List<EventLogEntry> EventLogEntries { get; }

    /// <inheritdoc />
    public EventLog EventLog { get; }

    internal int NextEntryIndexToCollect { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether limit reached.
    /// </summary>
    internal bool LimitReached { get; set; }

    public void Dispose()
    {
        Dispose(true);

        // Use SupressFinalize in case a subclass
        // of this type implements a finalizer.
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// This is the event handler for the EntryWritten event of the System.Diagnostics.EventLog class.
    /// Note that the documentation for the EntryWritten event includes these remarks:
    ///     "The system responds to WriteEntry only if the last write event occurred at least five seconds previously.
    ///      This implies you will only receive one EntryWritten event notification within a five-second interval, even if more
    ///      than one event log change occurs. If you insert a sufficiently long sleep interval (around 10 seconds) between calls
    ///      to WriteEntry, no events will be lost. However, if write events occur more frequently, the most recent write events
    ///      could be lost."
    /// This complicates this data collector because we don't want to sleep to wait for all events or lose the most recent events.
    /// To workaround, the implementation does several things:
    /// 1. We get the EventLog entries to collect from the EventLog.Entries collection and ignore the EntryWrittenEventArgs.
    /// 2. When event log collection ends for a data collection context, this method is called explicitly by the EventLogDataCollector
    ///    passing null for EntryWrittenEventArgs (which is fine since the argument is ignored.
    /// 3. We keep track of which EventLogEntry object in the EventLog.Entries we still need to collect.  We do this by inspecting
    ///    the value of the EventLogEntry.Index property.  The value of this property is an integer that is incremented for each entry
    ///    that is written to the event log, but is reset to 0 if the entire event log is cleared.
    /// Another behavior of event logs that we need to account for is that if the event log reaches a size limit, older events are
    /// automatically deleted.  In this case the collection EventLog.Entries contains only the entries remaining in the log,
    /// and the value of the EventLog.Entries[0].Index will not be 0; it will be the index of the oldest entry still in the log.
    /// For example, if the first 1000 entries written to an event log (since it was last completely cleared) are deleted because
    /// of the size limitation, then EventLog.Entries[0].Index would have a value of 1000 (this value is saved in the local variable
    /// "firstIndexInLog" in the method implementation.  Similarly "mostRecentIndexInLog" is the index of the last entry written
    /// to the log at the time we examine it.
    /// </summary>
    /// <param name="source">Source</param>
    /// <param name="e">The System.Diagnostics.EntryWrittenEventArgs object describing the entry that was written.</param>
    public void OnEventLogEntryWritten(object? source, EntryWrittenEventArgs? e)
    {
        while (!LimitReached)
        {
            try
            {
                lock (EventLogEntries)
                {
                    int currentCount = EventLog.Entries.Count;
                    if (currentCount == 0)
                    {
                        break;
                    }

                    int firstIndexInLog = EventLog.Entries[0].Index;
                    int mostRecentIndexInLog = EventLog.Entries[currentCount - 1].Index;

                    if (mostRecentIndexInLog == NextEntryIndexToCollect - 1)
                    {
                        // We've already collected the most recent entry in the log
                        break;
                    }

                    if (mostRecentIndexInLog < NextEntryIndexToCollect - 1)
                    {
                        EqtTrace.Warning(
                            "EventLogDataContainer: OnEventLogEntryWritten: Handling clearing of log (mostRecentIndexInLog < eventLogContainer.NextEntryIndex): firstIndexInLog: {0}:, mostRecentIndexInLog: {1}, NextEntryIndex: {2}",
                            firstIndexInLog,
                            mostRecentIndexInLog,
                            NextEntryIndexToCollect);

                        // Send warning; event log must have been cleared.
                        _dataCollectionLogger.LogWarning(
                            _dataCollectionContext,
                            string.Format(
                                CultureInfo.CurrentCulture,
                                Resource.EventsLostWarning,
                                EventLog.Log));

                        NextEntryIndexToCollect = 0;
                        firstIndexInLog = 0;
                    }

                    for (;
                         NextEntryIndexToCollect <= mostRecentIndexInLog;
                         NextEntryIndexToCollect++)
                    {
                        int nextEntryIndexInCurrentLog = NextEntryIndexToCollect - firstIndexInLog;
                        EventLogEntry nextEntry = EventLog.Entries[nextEntryIndexInCurrentLog];

                        // If an explicit list of event sources was provided, only report log entries from those sources
                        if (_eventSources != null && _eventSources.Count > 0)
                        {
                            if (!_eventSources.Contains(nextEntry.Source))
                            {
                                continue;
                            }
                        }

                        if (!_entryTypes.Contains(nextEntry.EntryType))
                        {
                            continue;
                        }

                        if (EventLogEntries.Count < _maxLogEntries)
                        {
                            EventLogEntries.Add(nextEntry);

                            EqtTrace.Verbose(
                                "EventLogDataContainer.OnEventLogEntryWritten() add event with Id {0} from position {1} in the current {2} log",
                                nextEntry.Index,
                                nextEntryIndexInCurrentLog,
                                EventLog.Log);
                        }
                        else
                        {
                            LimitReached = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                _dataCollectionLogger.LogError(
                    _dataCollectionContext,
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resource.EventsLostError,
                        EventLog.Log,
                        exception), exception);
            }
        }
    }

    /// <summary>
    /// The dispose.
    /// </summary>
    /// <param name="disposing">
    /// The disposing.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                EventLog.EnableRaisingEvents = false;
                EventLog.EntryWritten -= OnEventLogEntryWritten;
                EventLog.Dispose();
            }

            _isDisposed = true;
        }
    }
}
