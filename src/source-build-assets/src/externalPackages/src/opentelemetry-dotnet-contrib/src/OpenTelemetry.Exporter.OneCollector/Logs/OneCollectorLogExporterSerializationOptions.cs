// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Logs;

namespace OpenTelemetry.Exporter.OneCollector;

/// <summary>
/// Contains serialization options used to build a <see
/// cref="OneCollectorExporter{T}"/> instance for exporting <see
/// cref="LogRecord"/> telemetry data.
/// </summary>
public sealed class OneCollectorLogExporterSerializationOptions
{
    /// <summary>
    /// Gets or sets the exception stack trace handling type. Default
    /// value: <see
    /// cref="OneCollectorExporterSerializationExceptionStackTraceHandlingType.Ignore"/>.
    /// </summary>
    public OneCollectorExporterSerializationExceptionStackTraceHandlingType ExceptionStackTraceHandling { get; set; }
        = OneCollectorExporterSerializationExceptionStackTraceHandlingType.Ignore;

    /// <summary>
    /// Gets or sets OneCollector serialization format. Default value: <see
    /// cref="OneCollectorExporterSerializationFormatType.CommonSchemaV4JsonStream"/>.
    /// </summary>
    internal OneCollectorExporterSerializationFormatType Format { get; set; } = OneCollectorExporterSerializationFormatType.CommonSchemaV4JsonStream;

#pragma warning disable CA1822 // Mark members as static
    internal void Validate()
#pragma warning restore CA1822 // Mark members as static
    {
    }
}
