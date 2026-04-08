// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.TestPlatform.Extensions.EventLogCollector.UnitTests;

[TestClass]
public class EventLogXmlWriterTests
{
    private const string FileName = "Event Log.xml";

    [TestMethod]
    public void WriteEventLogEntriesToXmlFileShouldWriteToXmlFile()
    {
        var mockFileHelper = new Mock<IFileHelper>();
        var eventLogEntries = new List<EventLogEntry>();
        EventLogXmlWriter.WriteEventLogEntriesToXmlFile(
            FileName,
            eventLogEntries,
            mockFileHelper.Object);

        mockFileHelper.Verify(x => x.WriteAllTextToFile(FileName, It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public void WriteEventLogEntriesToXmlFileShouldWriteLogEntryIfPresent()
    {
        // Get any available event log entry. The Application log may be empty on
        // clean/quiet machines, so fall back to the System log which always has
        // entries on a running Windows machine.
        var eventLogEntry = GetLastEventLogEntry("Application") ?? GetLastEventLogEntry("System");
        Assert.IsNotNull(eventLogEntry, "No event log entries found in Application or System logs.");
        var eventLogEntries = new List<EventLogEntry> { eventLogEntry };

        var mockFileHelper = new Mock<IFileHelper>();
        EventLogXmlWriter.WriteEventLogEntriesToXmlFile(FileName, eventLogEntries, mockFileHelper.Object);

        // Escape XML special characters (&, <, >) the same way XmlWriter does for
        // text content so we can match the DataSet-serialized XML.  We intentionally
        // avoid XElement for this because XElement normalises lone \n to \r\n, which
        // does not match the output of DataSet.WriteXml and causes false negatives.
        var escapedMessage = eventLogEntry.Message
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;");

        mockFileHelper.Verify(x => x.WriteAllTextToFile(FileName, It.Is<string>(str => str.Contains(escapedMessage))));
    }

    private static EventLogEntry? GetLastEventLogEntry(string logName)
    {
        try
        {
            var eventLog = new EventLog(logName);
            if (eventLog.Entries.Count > 0)
            {
                return eventLog.Entries[eventLog.Entries.Count - 1];
            }
        }
        catch
        {
            // Log may be inaccessible; return null so callers can try another log.
        }

        return null;
    }
}
