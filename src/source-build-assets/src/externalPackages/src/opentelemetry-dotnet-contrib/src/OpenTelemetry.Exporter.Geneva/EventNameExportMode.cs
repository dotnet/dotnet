// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Exporter.Geneva;

/// <summary>
/// Contains the event name export mode defintions.
/// </summary>
[Flags]
public enum EventNameExportMode
{
    /// <summary>
    /// GenevaExporter will not export <a href="https://learn.microsoft.com/dotnet/api/microsoft.extensions.logging.eventid.name">eventId.Name</a>
    /// if the enum instance has no other flag selected.
    /// </summary>
    None = 0,

    /// <summary>
    /// GenevaExporter will export <a href="https://learn.microsoft.com/dotnet/api/microsoft.extensions.logging.eventid.name">eventId.Name</a>
    /// as Part A name field when this flag is selected.
    /// </summary>
    ExportAsPartAName = 1,

    /* Note: This might be added in future.
    /// <summary>
    /// GenevaExporter will export <a href="https://learn.microsoft.com/dotnet/api/microsoft.extensions.logging.eventid.name">eventId.Name</a>
    /// as the table name when this flag is selected.
    /// </summary>
    ExportAsTableName = 2,
    */
}
