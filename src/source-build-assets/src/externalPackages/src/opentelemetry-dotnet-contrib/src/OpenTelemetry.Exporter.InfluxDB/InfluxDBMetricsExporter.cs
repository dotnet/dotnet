// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using InfluxDB.Client;
using InfluxDB.Client.Writes;
using OpenTelemetry.Metrics;

namespace OpenTelemetry.Exporter.InfluxDB;

internal sealed class InfluxDBMetricsExporter : BaseExporter<Metric>
{
    private readonly IMetricsWriter writer;
    private readonly InfluxDBClient influxDbClient;
    private readonly WriteApi writeApi;

    public InfluxDBMetricsExporter(IMetricsWriter writer, InfluxDBClient influxDbClient, WriteApi writeApi)
    {
        this.writer = writer;
        this.influxDbClient = influxDbClient;
        this.writeApi = writeApi;

        this.writeApi.EventHandler += (_, args) =>
        {
            switch (args)
            {
                case WriteErrorEvent writeErrorEvent:
                    InfluxDBEventSource.Log.FailedToExport(writeErrorEvent.Exception.Message);
                    break;
                default:
                    break;
            }
        };
    }

    public override ExportResult Export(in Batch<Metric> batch)
    {
        try
        {
            foreach (var metric in batch)
            {
                this.writer.Write(metric, this.ParentProvider?.GetResource(), this.writeApi);
            }

            return ExportResult.Success;
        }
        catch (Exception exception)
        {
            InfluxDBEventSource.Log.FailedToExport(exception.Message);
            return ExportResult.Failure;
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.writeApi.Dispose();
            this.influxDbClient.Dispose();
        }

        base.Dispose(disposing);
    }
}
