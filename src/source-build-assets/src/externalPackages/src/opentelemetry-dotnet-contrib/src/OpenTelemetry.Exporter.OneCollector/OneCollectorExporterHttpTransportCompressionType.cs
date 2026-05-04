// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Exporter.OneCollector;

/// <summary>
/// Describes the OneCollector compression algorithm type to use when sending telemetry over HTTP.
/// </summary>
internal enum OneCollectorExporterHttpTransportCompressionType
{
    /// <summary>
    /// Uncompressed telemetry data.
    /// </summary>
    None,

    /// <summary>
    /// Compressed telemetry data using the Deflate algorithm.
    /// </summary>
    Deflate,
}
