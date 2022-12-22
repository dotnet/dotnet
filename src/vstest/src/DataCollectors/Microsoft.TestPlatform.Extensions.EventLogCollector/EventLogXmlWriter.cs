// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

namespace Microsoft.TestPlatform.Extensions.EventLogCollector;

/// <summary>
/// This class writes event log entries to an XML file in a format that can be retrieved into a DataSet
/// </summary>
internal static class EventLogXmlWriter
{
    /// <summary>
    /// The write event log entries to xml file.
    /// </summary>
    /// <param name="xmlFilePath">
    /// The xml file path.
    /// </param>
    /// <param name="eventLogEntries">
    /// The event log entries.
    /// </param>
    /// <param name="fileHelper">
    /// The file Helper.
    /// </param>
    public static void WriteEventLogEntriesToXmlFile(string xmlFilePath, List<EventLogEntry> eventLogEntries, IFileHelper fileHelper)
    {
        using DataTable dataTable = new();
        dataTable.Locale = CultureInfo.InvariantCulture;

        // The MaxLength of the Type and Source columns must be set to allow indices to be created on them
        DataColumn typeColumn = new("Type", typeof(string));
        typeColumn.MaxLength = EventLogConstants.TypeColumnMaxLength;
        dataTable.Columns.Add(typeColumn);

        dataTable.Columns.Add(new DataColumn("DateTime", typeof(DateTime)));

        DataColumn sourceColumn = new("Source", typeof(string));
        sourceColumn.MaxLength = EventLogConstants.SourceColumnMaxLength;
        dataTable.Columns.Add(sourceColumn);

        dataTable.Columns.Add(new DataColumn("Category", typeof(string)));
        dataTable.Columns.Add(new DataColumn("EventID", typeof(long)));
        dataTable.Columns.Add(new DataColumn("Description", typeof(string)));
        dataTable.Columns.Add(new DataColumn("User", typeof(string)));
        dataTable.Columns.Add(new DataColumn("Computer", typeof(string)));
        dataTable.ExtendedProperties.Add("TimestampColumnName", "DateTime");
        dataTable.ExtendedProperties.Add("IndexColumnNames", "Source,Type");

        foreach (EventLogEntry entry in eventLogEntries)
        {
            DataRow row = dataTable.NewRow();
            row["Type"] = entry.EntryType.ToString();
            row["DateTime"] = entry.TimeGenerated;
            row["Source"] = entry.Source;
            row["Category"] = entry.Category;
            row["EventID"] = entry.InstanceId;
            row["Description"] = entry.Message;
            row["User"] = entry.UserName;
            row["Computer"] = entry.MachineName;
            dataTable.Rows.Add(row);
        }

        DataSet dataSet = new();
        dataSet.Locale = CultureInfo.InvariantCulture;
        dataSet.Tables.Add(dataTable);

        // Use UTF-16 encoding
        StringBuilder stringBuilder = new();
        using StringWriter stringWriter = new(stringBuilder);
        dataSet.WriteXml(stringWriter, XmlWriteMode.WriteSchema);
        fileHelper.WriteAllTextToFile(xmlFilePath, stringBuilder.ToString());
    }
}
