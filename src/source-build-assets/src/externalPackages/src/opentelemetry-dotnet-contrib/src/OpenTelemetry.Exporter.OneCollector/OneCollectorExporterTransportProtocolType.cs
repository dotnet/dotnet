// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Exporter.OneCollector;

/// <summary>
/// Describes the OneCollector transport protocol to use when sending telemetry.
/// </summary>
internal enum OneCollectorExporterTransportProtocolType
{
    /// <summary>
    /// HTTP JSON POST protocol.
    /// </summary>
    HttpJsonPost,
}
