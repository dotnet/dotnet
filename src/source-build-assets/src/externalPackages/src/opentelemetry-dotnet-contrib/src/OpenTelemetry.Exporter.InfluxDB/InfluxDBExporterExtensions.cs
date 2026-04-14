// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using InfluxDB.Client;
using OpenTelemetry.Exporter.InfluxDB;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Metrics;

/// <summary>
/// Extension methods for <see cref="TracerProviderBuilder"/> for using InfluxDB.
/// </summary>
public static class InfluxDBExporterExtensions
{
    /// <summary>
    /// Adds InfluxDB exporter to the TracerProvider.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> builder to use.</param>
    /// <param name="configure">Callback action for configuring <see cref="InfluxDBMetricsExporterOptions"/>.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    public static MeterProviderBuilder AddInfluxDBMetricsExporter(this MeterProviderBuilder builder, Action<InfluxDBMetricsExporterOptions> configure)
    {
        builder.AddReader(_ =>
        {
            var options = new InfluxDBMetricsExporterOptions();
            configure.Invoke(options);

            var influxDbClientOptions = new InfluxDBClientOptions(options.Endpoint?.ToString());
            if (!string.IsNullOrWhiteSpace(options.Bucket))
            {
                influxDbClientOptions.Bucket = options.Bucket;
            }

            if (!string.IsNullOrWhiteSpace(options.Org))
            {
                influxDbClientOptions.Org = options.Org;
            }

            if (!string.IsNullOrWhiteSpace(options.Token))
            {
                influxDbClientOptions.Token = options.Token;
            }

            var influxDbClient = new InfluxDBClient(influxDbClientOptions);
            var writeApi = influxDbClient.GetWriteApi(new WriteOptions
            {
                FlushInterval = options.FlushInterval,
            });
            var metricsWriter = CreateMetricsWriter(options.MetricsSchema);
            var exporter = new InfluxDBMetricsExporter(metricsWriter, influxDbClient, writeApi);
            return new PeriodicExportingMetricReader(exporter, options.MetricExportIntervalMilliseconds)
            {
                TemporalityPreference = MetricReaderTemporalityPreference.Cumulative,
            };
        });
        return builder;
    }

    private static IMetricsWriter CreateMetricsWriter(MetricsSchema metricsSchema)
    {
        return metricsSchema switch
        {
            MetricsSchema.TelegrafPrometheusV2 => new TelegrafPrometheusWriterV2(),
            MetricsSchema.TelegrafPrometheusV1 => new TelegrafPrometheusWriterV1(),
            MetricsSchema.None => new TelegrafPrometheusWriterV1(),
            _ => new TelegrafPrometheusWriterV1(),
        };
    }
}
