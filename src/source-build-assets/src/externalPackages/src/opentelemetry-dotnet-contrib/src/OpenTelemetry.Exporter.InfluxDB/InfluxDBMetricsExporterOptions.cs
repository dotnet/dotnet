// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Internal;

namespace OpenTelemetry.Exporter.InfluxDB;

/// <summary>
/// InfluxDB exporter options.
/// </summary>
public class InfluxDBMetricsExporterOptions
{
    private int metricExportIntervalMilliseconds = 60000;

    /// <summary>
    /// Gets or sets HTTP/S destination for line protocol.
    /// </summary>
    public Uri? Endpoint { get; set; }

    /// <summary>
    /// Gets or sets name of InfluxDB organization that owns the destination bucket.
    /// </summary>
    public string? Org { get; set; }

    /// <summary>
    /// Gets or sets the name of InfluxDB bucket to which signals will be written.
    /// </summary>
    public string? Bucket { get; set; }

    /// <summary>
    /// Gets or sets the authentication token for InfluxDB.
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// Gets or sets the chosen metrics schema to write.
    /// </summary>
    public MetricsSchema MetricsSchema { get; set; } = MetricsSchema.TelegrafPrometheusV1;

    /// <summary>
    /// Gets or sets the time to wait at most (milliseconds) with the write.
    /// </summary>
    public int FlushInterval { get; set; } = 1000;

    /// <summary>
    /// Gets or sets the metric export interval in milliseconds. The default value is 60000.
    /// </summary>
    public int MetricExportIntervalMilliseconds
    {
        get => this.metricExportIntervalMilliseconds;
        set
        {
            Guard.ThrowIfOutOfRange(value, min: 1000);
            this.metricExportIntervalMilliseconds = value;
        }
    }
}
