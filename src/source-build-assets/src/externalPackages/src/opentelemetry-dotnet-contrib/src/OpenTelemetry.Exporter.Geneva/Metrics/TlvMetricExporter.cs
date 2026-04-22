// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using OpenTelemetry.Metrics;

namespace OpenTelemetry.Exporter.Geneva.Metrics;

internal sealed class TlvMetricExporter : IDisposable
{
    private readonly ushort prepopulatedDimensionsCount;
    private readonly int fixedPayloadStartIndex;
    private readonly IMetricDataTransport metricDataTransport;
    private readonly List<byte[]>? serializedPrepopulatedDimensionsKeys;
    private readonly List<byte[]>? serializedPrepopulatedDimensionsValues;
    private readonly byte[] buffer = new byte[GenevaMetricExporter.BufferSize];
    private readonly string defaultMonitoringAccount;
    private readonly string defaultMetricNamespace;

    private bool isDisposed;

    internal TlvMetricExporter(
        ConnectionStringBuilder connectionStringBuilder,
        IReadOnlyDictionary<string, object>? prepopulatedMetricDimensions)
    {
        this.defaultMonitoringAccount = connectionStringBuilder.Account;
        this.defaultMetricNamespace = connectionStringBuilder.Namespace;

        if (prepopulatedMetricDimensions != null)
        {
            this.prepopulatedDimensionsCount = (ushort)prepopulatedMetricDimensions.Count;
            this.serializedPrepopulatedDimensionsKeys = this.SerializePrepopulatedDimensionsKeys(prepopulatedMetricDimensions.Keys);
            this.serializedPrepopulatedDimensionsValues = this.SerializePrepopulatedDimensionsValues(prepopulatedMetricDimensions.Values);
        }

        switch (connectionStringBuilder.Protocol)
        {
            case TransportProtocol.Unix:
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    throw new ArgumentException("Unix domain socket should not be used on Windows.");
                }

                var unixDomainSocketPath = connectionStringBuilder.ParseUnixDomainSocketPath();
                this.metricDataTransport = new MetricUnixDomainSocketDataTransport(unixDomainSocketPath);
                break;
            case TransportProtocol.Unspecified:
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    this.metricDataTransport = MetricWindowsEventTracingDataTransport.Instance;
                    break;
                }
                else
                {
                    throw new ArgumentException("Endpoint not specified");
                }

            case TransportProtocol.Etw:
            case TransportProtocol.Tcp:
            case TransportProtocol.Udp:
            case TransportProtocol.EtwTld:
            default:
                throw new NotSupportedException($"Protocol '{connectionStringBuilder.Protocol}' is not supported");
        }

        unsafe
        {
            this.fixedPayloadStartIndex = sizeof(BinaryHeader);
        }
    }

    public void Dispose()
    {
        if (this.isDisposed)
        {
            return;
        }

        try
        {
            // The ETW data transport singleton on Windows should NOT be disposed.
            // On Linux, Unix Domain Socket is used and should be disposed.
            if (this.metricDataTransport != MetricWindowsEventTracingDataTransport.Instance)
            {
                this.metricDataTransport.Dispose();
            }

            this.isDisposed = true;
        }
        catch (Exception ex)
        {
            ExporterEventSource.Log.ExporterException("TlvMetricExporter Dispose failed.", ex);
        }
    }

    internal ExportResult Export(in Batch<Metric> batch)
    {
        var monitoringAccount = this.defaultMonitoringAccount;
        var metricNamespace = this.defaultMetricNamespace;

        var result = ExportResult.Success;
        foreach (var metric in batch)
        {
            foreach (ref readonly var metricPoint in metric.GetMetricPoints())
            {
                try
                {
                    metricPoint.TryGetExemplars(out var exemplars);

                    var metricType = metric.MetricType;
                    switch (metricType)
                    {
                        case MetricType.LongSum:
                            {
                                var ulongSum = Convert.ToUInt64(metricPoint.GetSumLong());
                                var metricData = new MetricData { UInt64Value = ulongSum };
                                var bodyLength = this.SerializeMetricWithTLV(
                                    MetricEventType.ULongMetric,
                                    metric.Name,
                                    metricPoint.EndTime.ToFileTime(), // Using the endTime here as the timestamp as Geneva Metrics only allows for one field for timestamp
                                    metricPoint.Tags,
                                    metricData,
                                    metricType,
                                    exemplars,
                                    out monitoringAccount,
                                    out metricNamespace);
                                this.metricDataTransport.Send(MetricEventType.TLV, this.buffer, bodyLength);
                                break;
                            }

                        // The value here could be negative hence we have to use `MetricEventType.DoubleMetric`
                        case MetricType.LongGauge:
                            {
                                // potential for minor precision loss implicitly going from long->double
                                // see: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/numeric-conversions#implicit-numeric-conversions
                                var doubleSum = Convert.ToDouble(metricPoint.GetGaugeLastValueLong());
                                var metricData = new MetricData { DoubleValue = doubleSum };
                                var bodyLength = this.SerializeMetricWithTLV(
                                    MetricEventType.DoubleMetric,
                                    metric.Name,
                                    metricPoint.EndTime.ToFileTime(),
                                    metricPoint.Tags,
                                    metricData,
                                    metricType,
                                    exemplars,
                                    out monitoringAccount,
                                    out metricNamespace);
                                this.metricDataTransport.Send(MetricEventType.TLV, this.buffer, bodyLength);
                                break;
                            }

                        // The value here could be negative hence we have to use `MetricEventType.DoubleMetric`
                        case MetricType.LongSumNonMonotonic:
                            {
                                // potential for minor precision loss implicitly going from long->double
                                // see: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/numeric-conversions#implicit-numeric-conversions
                                var doubleSum = Convert.ToDouble(metricPoint.GetSumLong());
                                var metricData = new MetricData { DoubleValue = doubleSum };
                                var bodyLength = this.SerializeMetricWithTLV(
                                    MetricEventType.DoubleMetric,
                                    metric.Name,
                                    metricPoint.EndTime.ToFileTime(),
                                    metricPoint.Tags,
                                    metricData,
                                    metricType,
                                    exemplars,
                                    out monitoringAccount,
                                    out metricNamespace);
                                this.metricDataTransport.Send(MetricEventType.TLV, this.buffer, bodyLength);
                                break;
                            }

                        case MetricType.DoubleSum:
                        case MetricType.DoubleSumNonMonotonic:
                            {
                                var doubleSum = metricPoint.GetSumDouble();
                                var metricData = new MetricData { DoubleValue = doubleSum };
                                var bodyLength = this.SerializeMetricWithTLV(
                                    MetricEventType.DoubleMetric,
                                    metric.Name,
                                    metricPoint.EndTime.ToFileTime(),
                                    metricPoint.Tags,
                                    metricData,
                                    metricType,
                                    exemplars,
                                    out monitoringAccount,
                                    out metricNamespace);
                                this.metricDataTransport.Send(MetricEventType.TLV, this.buffer, bodyLength);
                                break;
                            }

                        case MetricType.DoubleGauge:
                            {
                                var doubleSum = metricPoint.GetGaugeLastValueDouble();
                                var metricData = new MetricData { DoubleValue = doubleSum };
                                var bodyLength = this.SerializeMetricWithTLV(
                                    MetricEventType.DoubleMetric,
                                    metric.Name,
                                    metricPoint.EndTime.ToFileTime(),
                                    metricPoint.Tags,
                                    metricData,
                                    metricType,
                                    exemplars,
                                    out monitoringAccount,
                                    out metricNamespace);
                                this.metricDataTransport.Send(MetricEventType.TLV, this.buffer, bodyLength);
                                break;
                            }

                        case MetricType.Histogram:
                            {
                                var sum = Convert.ToUInt64(metricPoint.GetHistogramSum());
                                var count = Convert.ToUInt32(metricPoint.GetHistogramCount());
                                if (!metricPoint.TryGetHistogramMinMaxValues(out var min, out var max))
                                {
                                    min = 0;
                                    max = 0;
                                }

                                var bodyLength = this.SerializeHistogramMetricWithTLV(
                                    metric.Name,
                                    metricPoint.EndTime.ToFileTime(),
                                    metricPoint.Tags,
                                    metricPoint.GetHistogramBuckets(),
                                    sum,
                                    count,
                                    min,
                                    max,
                                    metricType,
                                    exemplars,
                                    out monitoringAccount,
                                    out metricNamespace);
                                this.metricDataTransport.Send(MetricEventType.TLV, this.buffer, bodyLength);
                                break;
                            }

                        case MetricType.ExponentialHistogram:
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    ExporterEventSource.Log.FailedToSendMetricData(monitoringAccount, metricNamespace, metric.Name, ex); // TODO: preallocate exception or no exception
                    result = ExportResult.Failure;
                }
            }
        }

        return result;
    }

    internal unsafe ushort SerializeMetricWithTLV(
        MetricEventType eventType,
        string metricName,
        long timestamp,
        in ReadOnlyTagCollection tags,
        MetricData value,
        MetricType metricType,
        ReadOnlyExemplarCollection exemplars,
        out string monitoringAccount,
        out string metricNamespace)
    {
        ushort bodyLength;
        try
        {
            // The buffer format is as follows:
            // -- BinaryHeader
            // -- Sequence of payload types

            // Leave enough space for the header
            var bufferIndex = sizeof(BinaryHeader);

            SerializeMetricName(metricName, this.buffer, ref bufferIndex);

            SerializeNonHistogramMetricData(eventType, value, timestamp, this.buffer, ref bufferIndex);

            // Serializes metric dimensions and also gets the custom account name and metric namespace
            // if specified by adding custom tags: _microsoft_metrics_namespace and _microsoft_metrics_namespace
            this.SerializeDimensionsAndGetCustomAccountNamespace(
                tags,
                this.buffer,
                ref bufferIndex,
                out monitoringAccount,
                out metricNamespace);

            SerializeExemplars(exemplars, metricType, this.buffer, ref bufferIndex);

            SerializeMonitoringAccount(monitoringAccount, this.buffer, ref bufferIndex);

            SerializeMetricNamespace(metricNamespace, this.buffer, ref bufferIndex);

            // Write the final size of the payload
            bodyLength = (ushort)(bufferIndex - this.fixedPayloadStartIndex);

            // Copy in the final structures to the front
            fixed (byte* bufferBytes = this.buffer)
            {
                var ptr = (BinaryHeader*)bufferBytes;
                ptr->EventId = (ushort)MetricEventType.TLV;
                ptr->BodyLength = bodyLength;
            }
        }
        finally
        {
        }

        return bodyLength;
    }

    internal unsafe ushort SerializeHistogramMetricWithTLV(
        string metricName,
        long timestamp,
        in ReadOnlyTagCollection tags,
        HistogramBuckets buckets,
        double sum,
        uint count,
        double min,
        double max,
        MetricType metricType,
        ReadOnlyExemplarCollection exemplars,
        out string monitoringAccount,
        out string metricNamespace)
    {
        ushort bodyLength;
        try
        {
            // The buffer format is as follows:
            // -- BinaryHeader
            // -- Sequence of payload types

            // Leave enough space for the header
            var bufferIndex = sizeof(BinaryHeader);

            SerializeMetricName(metricName, this.buffer, ref bufferIndex);

            SerializeHistogramMetricData(buckets, sum, count, min, max, timestamp, this.buffer, ref bufferIndex);

            // Serializes metric dimensions and also gets the custom account name and metric namespace
            // if specified by adding custom tags: _microsoft_metrics_namespace and _microsoft_metrics_namespace
            this.SerializeDimensionsAndGetCustomAccountNamespace(
                tags,
                this.buffer,
                ref bufferIndex,
                out monitoringAccount,
                out metricNamespace);

            SerializeExemplars(exemplars, metricType, this.buffer, ref bufferIndex);

            SerializeMonitoringAccount(monitoringAccount, this.buffer, ref bufferIndex);

            SerializeMetricNamespace(metricNamespace, this.buffer, ref bufferIndex);

            // Write the final size of the payload
            bodyLength = (ushort)(bufferIndex - this.fixedPayloadStartIndex);

            // Copy in the final structures to the front
            fixed (byte* bufferBytes = this.buffer)
            {
                var ptr = (BinaryHeader*)bufferBytes;
                ptr->EventId = (ushort)MetricEventType.TLV;
                ptr->BodyLength = bodyLength;
            }
        }
        finally
        {
        }

        return bodyLength;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SerializeMetricName(string metricName, byte[] buffer, ref int bufferIndex)
    {
        MetricSerializer.SerializeByte(buffer, ref bufferIndex, (byte)PayloadType.MetricName);
        MetricSerializer.SerializeString(buffer, ref bufferIndex, metricName);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SerializeMetricNamespace(string metricNamespace, byte[] buffer, ref int bufferIndex)
    {
        MetricSerializer.SerializeByte(buffer, ref bufferIndex, (byte)PayloadType.Namespace);
        MetricSerializer.SerializeString(buffer, ref bufferIndex, metricNamespace);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SerializeMonitoringAccount(string monitoringAccount, byte[] buffer, ref int bufferIndex)
    {
        MetricSerializer.SerializeByte(buffer, ref bufferIndex, (byte)PayloadType.AccountName);
        MetricSerializer.SerializeString(buffer, ref bufferIndex, monitoringAccount);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SerializeExemplars(in ReadOnlyExemplarCollection exemplars, MetricType metricType, byte[] buffer, ref int bufferIndex)
    {
        var exemplarsCount = 0;
        foreach (ref readonly var exemplar in exemplars)
        {
            exemplarsCount++;
        }

        if (exemplarsCount > 0)
        {
            MetricSerializer.SerializeByte(buffer, ref bufferIndex, (byte)PayloadType.Exemplars);

            // Get a placeholder to add the payloadType length
            var payloadTypeStartIndex = bufferIndex;
            bufferIndex += 2;

            MetricSerializer.SerializeByte(buffer, ref bufferIndex, 0); // version

            MetricSerializer.SerializeInt32AsBase128(buffer, ref bufferIndex, exemplarsCount);

            foreach (ref readonly var exemplar in exemplars)
            {
                SerializeSingleExemplar(exemplar, metricType, buffer, ref bufferIndex);
            }

            var payloadTypeLength = (ushort)(bufferIndex - payloadTypeStartIndex - 2);
            MetricSerializer.SerializeUInt16(buffer, ref payloadTypeStartIndex, payloadTypeLength);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SerializeSingleExemplar(Exemplar exemplar, MetricType metricType, byte[] buffer, ref int bufferIndex)
    {
        MetricSerializer.SerializeByte(buffer, ref bufferIndex, 0); // version

        var bufferIndexForLength = bufferIndex;
        bufferIndex++;

        var bufferIndexForFlags = bufferIndex;
        bufferIndex++;

        var flags = ExemplarFlags.IsTimestampAvailable; // we only serialize exemplars with Timestamp != default

        if (metricType.IsLong())
        {
            flags |= ExemplarFlags.IsMetricValueDoubleStoredAsLong;
            MetricSerializer.SerializeInt64AsBase128(buffer, ref bufferIndex, exemplar.LongValue); // serialize long value
        }
        else
        {
            MetricSerializer.SerializeFloat64(buffer, ref bufferIndex, exemplar.DoubleValue); // serialize double value
        }

        var bufferIndexForNumberOfLabels = bufferIndex;
        MetricSerializer.SerializeByte(buffer, ref bufferIndex, 0); // serialize zero as the count of labels; this would be updated later if the exemplar has labels

        // Convert exemplar timestamp to unix nanoseconds
        var unixNanoSeconds = DateTime.FromFileTimeUtc(exemplar.Timestamp.ToFileTime())
                        .ToUniversalTime()
                        .Subtract(new DateTime(1970, 1, 1))
                        .TotalMilliseconds * 1000000;

        MetricSerializer.SerializeInt64(buffer, ref bufferIndex, (long)unixNanoSeconds); // serialize timestamp

        if (exemplar.TraceId != default)
        {
            Span<byte> traceIdBytes = stackalloc byte[16];
            exemplar.TraceId.CopyTo(traceIdBytes);
            MetricSerializer.SerializeSpanOfBytes(buffer, ref bufferIndex, traceIdBytes, traceIdBytes.Length); // serialize traceId

            flags |= ExemplarFlags.TraceIdExists;
        }

        if (exemplar.SpanId != default)
        {
            Span<byte> spanIdBytes = stackalloc byte[8];
            exemplar.SpanId.CopyTo(spanIdBytes);
            MetricSerializer.SerializeSpanOfBytes(buffer, ref bufferIndex, spanIdBytes, spanIdBytes.Length); // serialize spanId

            flags |= ExemplarFlags.SpanIdExists;
        }

        byte numberOfLabels = 0;

        foreach (var tag in exemplar.FilteredTags)
        {
            MetricSerializer.SerializeBase128String(buffer, ref bufferIndex, tag.Key);
            MetricSerializer.SerializeBase128String(buffer, ref bufferIndex, Convert.ToString(tag.Value, CultureInfo.InvariantCulture));
            numberOfLabels++;
        }

        if (numberOfLabels > 0)
        {
            MetricSerializer.SerializeByte(buffer, ref bufferIndexForNumberOfLabels, numberOfLabels);
        }

        MetricSerializer.SerializeByte(buffer, ref bufferIndexForFlags, (byte)flags);

        var exemplarLength = bufferIndex - bufferIndexForLength + 1;
        MetricSerializer.SerializeByte(buffer, ref bufferIndexForLength, (byte)exemplarLength);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SerializeNonHistogramMetricData(MetricEventType eventType, MetricData value, long timestamp, byte[] buffer, ref int bufferIndex)
    {
        var payloadType = eventType == MetricEventType.ULongMetric ? PayloadType.ULongMetric : PayloadType.DoubleMetric;
        MetricSerializer.SerializeByte(buffer, ref bufferIndex, (byte)payloadType);

        // Get a placeholder to add the payloadType length
        var payloadTypeStartIndex = bufferIndex;
        bufferIndex += 2;

        MetricSerializer.SerializeUInt64(buffer, ref bufferIndex, (ulong)timestamp); // timestamp

        if (payloadType == PayloadType.ULongMetric)
        {
            MetricSerializer.SerializeUInt64(buffer, ref bufferIndex, value.UInt64Value);
        }
        else
        {
            MetricSerializer.SerializeFloat64(buffer, ref bufferIndex, value.DoubleValue);
        }

        var payloadTypeLength = (ushort)(bufferIndex - payloadTypeStartIndex - 2);
        MetricSerializer.SerializeUInt16(buffer, ref payloadTypeStartIndex, payloadTypeLength);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SerializeHistogramMetricData(HistogramBuckets buckets, double sum, uint count, double min, double max, long timestamp, byte[] buffer, ref int bufferIndex)
    {
        MetricSerializer.SerializeByte(buffer, ref bufferIndex, (byte)PayloadType.ExternallyAggregatedULongDistributionMetric);

        // Get a placeholder to add the payloadType length
        var payloadTypeStartIndex = bufferIndex;
        bufferIndex += 2;

        // Serialize sum, count, min, and max
        MetricSerializer.SerializeUInt32(buffer, ref bufferIndex, count); // histogram count
        MetricSerializer.SerializeUInt32(buffer, ref bufferIndex, 0); // padding
        MetricSerializer.SerializeUInt64(buffer, ref bufferIndex, (ulong)timestamp); // timestamp
        MetricSerializer.SerializeUInt64(buffer, ref bufferIndex, Convert.ToUInt64(sum)); // histogram sum
        MetricSerializer.SerializeUInt64(buffer, ref bufferIndex, Convert.ToUInt64(min)); // histogram min
        MetricSerializer.SerializeUInt64(buffer, ref bufferIndex, Convert.ToUInt64(max)); // histogram max

        var payloadTypeLength = (ushort)(bufferIndex - payloadTypeStartIndex - 2);
        MetricSerializer.SerializeUInt16(buffer, ref payloadTypeStartIndex, payloadTypeLength);

        // Serialize histogram buckets as value-count pairs
        MetricSerializer.SerializeByte(buffer, ref bufferIndex, (byte)PayloadType.HistogramULongValueCountPairs);

        // Get a placeholder to add the payloadType length
        payloadTypeStartIndex = bufferIndex;
        bufferIndex += 2;

        // Get a placeholder to add the number of value-count pairs added
        // with value being the bucket boundary and count being the respective count

        var itemsWrittenIndex = bufferIndex;
        MetricSerializer.SerializeUInt16(buffer, ref bufferIndex, 0);

        // Bucket values
        ushort bucketCount = 0;
        double lastExplicitBound = default;
        foreach (var bucket in buckets)
        {
            if (bucket.BucketCount > 0)
            {
                SerializeHistogramBucketWithTLV(bucket, buffer, ref bufferIndex, lastExplicitBound);
                bucketCount++;
            }

            lastExplicitBound = bucket.ExplicitBound;
        }

        // Write the number of items in distribution emitted and reset back to end.
        MetricSerializer.SerializeUInt16(buffer, ref itemsWrittenIndex, bucketCount);

        payloadTypeLength = (ushort)(bufferIndex - payloadTypeStartIndex - 2);
        MetricSerializer.SerializeUInt16(buffer, ref payloadTypeStartIndex, payloadTypeLength);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SerializeHistogramBucketWithTLV(in HistogramBucket bucket, byte[] buffer, ref int bufferIndex, double lastExplicitBound)
    {
        if (bucket.ExplicitBound != double.PositiveInfinity)
        {
            MetricSerializer.SerializeUInt64(buffer, ref bufferIndex, Convert.ToUInt64(bucket.ExplicitBound));
        }
        else
        {
            // The bucket to catch the overflows is one greater than the last bound provided
            MetricSerializer.SerializeUInt64(buffer, ref bufferIndex, Convert.ToUInt64(lastExplicitBound + 1));
        }

        MetricSerializer.SerializeUInt32(buffer, ref bufferIndex, Convert.ToUInt32(bucket.BucketCount));
    }

    private static string? ConvertTagValueToString(object? value)
    {
        string? repr;
        try
        {
            repr = Convert.ToString(value, CultureInfo.InvariantCulture);
        }
        catch
        {
            repr = $"ERROR: type {value?.GetType().FullName} is not supported";
        }

        return repr;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void SerializeDimensionsAndGetCustomAccountNamespace(in ReadOnlyTagCollection tags, byte[] buffer, ref int bufferIndex, out string monitoringAccount, out string metricNamespace)
    {
        monitoringAccount = this.defaultMonitoringAccount;
        metricNamespace = this.defaultMetricNamespace;

        MetricSerializer.SerializeByte(buffer, ref bufferIndex, (byte)PayloadType.Dimensions);

        // Get a placeholder to add the payloadType length
        var payloadTypeStartIndex = bufferIndex;
        bufferIndex += 2;

        // Get a placeholder to add dimensions count later
        var bufferIndexForDimensionsCount = bufferIndex;
        bufferIndex += 2;

        ushort dimensionsWritten = 0;

        // Serialize PrepopulatedDimensions keys
        for (ushort i = 0; i < this.prepopulatedDimensionsCount; i++)
        {
            MetricSerializer.SerializeEncodedString(buffer, ref bufferIndex, this.serializedPrepopulatedDimensionsKeys![i]);
        }

        if (this.prepopulatedDimensionsCount > 0)
        {
            dimensionsWritten += this.prepopulatedDimensionsCount;
        }

        var reservedTags = 0;

        // Serialize MetricPoint Dimension keys
        foreach (var tag in tags)
        {
            if (tag.Key.Length > GenevaMetricExporter.MaxDimensionNameSize)
            {
                // TODO: Data Validation
            }

            if (tag.Key.Equals(GenevaMetricExporter.DimensionKeyForCustomMonitoringAccount, StringComparison.OrdinalIgnoreCase) ||
                tag.Key.Equals(GenevaMetricExporter.DimensionKeyForCustomMetricsNamespace, StringComparison.OrdinalIgnoreCase))
            {
                reservedTags++;
                continue;
            }

            MetricSerializer.SerializeString(buffer, ref bufferIndex, tag.Key);
        }

        dimensionsWritten += (ushort)(tags.Count - reservedTags);

        // Serialize PrepopulatedDimensions values
        for (ushort i = 0; i < this.prepopulatedDimensionsCount; i++)
        {
            MetricSerializer.SerializeEncodedString(buffer, ref bufferIndex, this.serializedPrepopulatedDimensionsValues![i]);
        }

        // Serialize MetricPoint Dimension values
        foreach (var tag in tags)
        {
            if (tag.Key.Equals(GenevaMetricExporter.DimensionKeyForCustomMonitoringAccount, StringComparison.OrdinalIgnoreCase) && tag.Value is string metricsAccount)
            {
                if (!string.IsNullOrWhiteSpace(metricsAccount))
                {
                    monitoringAccount = metricsAccount;
                }

                continue;
            }

            if (tag.Key.Equals(GenevaMetricExporter.DimensionKeyForCustomMetricsNamespace, StringComparison.OrdinalIgnoreCase) && tag.Value is string metricsNamespace)
            {
                if (!string.IsNullOrWhiteSpace(metricsNamespace))
                {
                    metricNamespace = metricsNamespace;
                }

                continue;
            }

            var dimensionValue = ConvertTagValueToString(tag.Value);
            if (dimensionValue?.Length > GenevaMetricExporter.MaxDimensionValueSize)
            {
                // TODO: Data Validation
            }

            MetricSerializer.SerializeString(buffer, ref bufferIndex, dimensionValue);
        }

        // Backfill the number of dimensions written
        MetricSerializer.SerializeUInt16(buffer, ref bufferIndexForDimensionsCount, dimensionsWritten);

        var payloadTypeLength = (ushort)(bufferIndex - payloadTypeStartIndex - 2);
        MetricSerializer.SerializeUInt16(buffer, ref payloadTypeStartIndex, payloadTypeLength);
    }

    private List<byte[]> SerializePrepopulatedDimensionsKeys(IEnumerable<string> keys)
    {
        var serializedKeys = new List<byte[]>(this.prepopulatedDimensionsCount);
        foreach (var key in keys)
        {
            serializedKeys.Add(Encoding.UTF8.GetBytes(key));
        }

        return serializedKeys;
    }

    private List<byte[]> SerializePrepopulatedDimensionsValues(IEnumerable<object> values)
    {
        var serializedValues = new List<byte[]>(this.prepopulatedDimensionsCount);
        foreach (var value in values)
        {
            var valueAsString = ConvertTagValueToString(value);
            if (valueAsString != null)
            {
                serializedValues.Add(Encoding.UTF8.GetBytes(valueAsString));
            }
        }

        return serializedValues;
    }
}
