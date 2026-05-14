// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Exporter.InfluxDB;

/// <summary>
/// The OpenTelemetry->InfluxDB conversion schema.
/// </summary>
public enum MetricsSchema
{
    /// <summary>
    /// No schema selected. Default schema will be used.
    /// </summary>
    None,

    /// <summary>
    /// Represents the Telegraf Prometheus V1 schema.
    /// This option converts OpenTelemetry metrics to the first version of the Telegraf Prometheus schema.
    /// - Telegraf/InfluxDB measurement per Prometheus metric
    /// - Fields `count`/`sum`/`gauge`/etc contain metric values.
    /// </summary>
    TelegrafPrometheusV1 = 1,

    /// <summary>
    /// Represents the Telegraf Prometheus V2 schema.
    /// This option converts OpenTelemetry metrics to the second version of the Telegraf Prometheus schema.
    /// - One measurement `prometheus`
    /// - Fields (Prometheus metric name) + `_` + `count`/`sum`/`gauge`/etc contain metric values.
    /// </summary>
    TelegrafPrometheusV2 = 2,
}
