// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace OpenTelemetry.Exporter.Geneva;

internal sealed class OtlpProtobufMetricExporter : IDisposable
{
    private readonly byte[] buffer = new byte[GenevaMetricExporter.BufferSize];

    private readonly OtlpProtobufSerializer otlpProtobufSerializer;

    private readonly Func<Resource> getResource;

    public OtlpProtobufMetricExporter(
        Func<Resource> getResource,
        IMetricDataTransport transport,
        string? metricsAccount,
        string? metricsNamespace,
        IReadOnlyDictionary<string, object>? prepopulatedMetricDimensions)
    {
        Debug.Assert(getResource != null, "getResource was null");
        Debug.Assert(transport != null, "transport was null");

        this.getResource = getResource!;

        this.otlpProtobufSerializer = new OtlpProtobufSerializer(
            transport!,
            metricsAccount,
            metricsNamespace,
            prepopulatedMetricDimensions,
            prefixBufferWithUInt32LittleEndianLength: transport is MetricUnixDomainSocketDataTransport);
    }

    public ExportResult Export(in Batch<Metric> batch)
    {
        try
        {
            return this.otlpProtobufSerializer.SerializeAndSendMetrics(this.buffer, this.getResource(), batch);
        }
        catch (Exception ex)
        {
            ExporterEventSource.Log.ExporterException("Failed to export metrics batch", ex);
            return ExportResult.Failure;
        }
    }

    public void Dispose()
    {
        if (this.otlpProtobufSerializer.MetricDataTransport is MetricUnixDomainSocketDataTransport udsTransport)
        {
            udsTransport.Dispose();
        }
    }
}
