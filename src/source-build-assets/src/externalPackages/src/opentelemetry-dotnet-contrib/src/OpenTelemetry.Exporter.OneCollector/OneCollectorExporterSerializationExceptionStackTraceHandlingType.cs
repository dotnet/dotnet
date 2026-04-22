// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Exporter.OneCollector;

/// <summary>
/// Describes the OneCollector serialization behavior to use when writing the
/// stack trace for telemetry containing an <see cref="Exception"/> instance.
/// </summary>
public enum OneCollectorExporterSerializationExceptionStackTraceHandlingType
{
    /// <summary>
    /// Exception stack traces will be ignored when serializing.
    /// </summary>
    Ignore,

    /// <summary>
    /// Exception stack traces will be included when serializing by calling <see
    /// cref="Exception.ToString"/>.
    /// </summary>
    IncludeAsString,
}
