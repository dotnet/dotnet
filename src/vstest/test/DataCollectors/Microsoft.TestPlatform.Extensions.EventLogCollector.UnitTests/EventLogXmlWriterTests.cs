// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;

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
        var eventLog = new EventLog("Application");
        var eventLogEntry = eventLog.Entries[eventLog.Entries.Count - 1];
        var eventLogEntries = new List<EventLogEntry> { eventLogEntry };

        var mockFileHelper = new Mock<IFileHelper>();
        EventLogXmlWriter.WriteEventLogEntriesToXmlFile(FileName, eventLogEntries, mockFileHelper.Object);

        // Serialize the message in case it contains any special character such as <, >, &, which the XML writer would escape
        // because otherwise the raw message and the message used to call WriteAllTextToFile won't match. E.g.
        // api-version=2020-07-01&format=json in raw message, becomes
        // api-version=2020-07-01&amp;format=json in the xml file.
        var serializedMessage = new XElement("t", eventLogEntry.Message).LastNode.ToString();

        mockFileHelper.Verify(x => x.WriteAllTextToFile(FileName, It.Is<string>(str => str.Contains(serializedMessage))));
    }
}
