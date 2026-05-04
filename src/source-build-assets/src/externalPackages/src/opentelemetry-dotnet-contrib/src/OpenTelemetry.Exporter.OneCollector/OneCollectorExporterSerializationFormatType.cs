// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Exporter.OneCollector;

/// <summary>
/// Describes the OneCollector serialization format to use when writing telemetry.
/// </summary>
internal enum OneCollectorExporterSerializationFormatType
{
    /// <summary>
    /// Common Schema v4.0 UTF-8 JSON stream serialization format.
    /// </summary>
    CommonSchemaV4JsonStream,
}
