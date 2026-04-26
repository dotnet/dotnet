// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Buffers.Binary;
using System.Diagnostics;
using System.Globalization;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace OpenTelemetry.Exporter.Geneva;

internal sealed class OtlpProtobufSerializer
{
    internal IMetricDataTransport MetricDataTransport;

    private const int TagAndLengthSize = 4;

    private readonly Dictionary<string, List<Metric>> scopeMetrics = [];
    private readonly string? metricNamespace;
    private readonly string? metricAccount;
    private readonly bool prefixBufferWithUInt32LittleEndianLength;
    private readonly byte[]? prepopulatedNumberDataPointAttributes;
    private readonly int prepopulatedNumberDataPointAttributesLength;
    private readonly byte[]? prepopulatedHistogramDataPointAttributes;
    private readonly int prepopulatedHistogramDataPointAttributesLength;
    private readonly byte[]? prepopulatedExponentialHistogramDataPointAttributes;
    private readonly int prepopulatedExponentialHistogramDataPointAttributesLength;
    private int resourceMetricTagAndLengthIndex;
    private int scopeMetricsTagAndLengthIndex;
    private int metricTagAndLengthIndex;
    private int instrumentTagAndLengthIndex;
    private int metricPointTagAndLengthIndex;
    private int resourceMetricValueIndex;
    private int scopeMetricsValueIndex;
    private int metricValueIndex;
    private int instrumentValueIndex;
    private int metricPointValueIndex;
    private ExportResult metricExportResult;

    public OtlpProtobufSerializer(
        IMetricDataTransport metricDataTransport,
        string? metricsAccount,
        string? metricsNamespace,
        IReadOnlyDictionary<string, object>? prepopulatedMetricDimensions,
        bool prefixBufferWithUInt32LittleEndianLength = false)
    {
        Debug.Assert(metricDataTransport != null, "metricDataTransport was null");

        this.MetricDataTransport = metricDataTransport!;
        this.metricAccount = metricsAccount;
        this.metricNamespace = metricsNamespace;
        this.prefixBufferWithUInt32LittleEndianLength = prefixBufferWithUInt32LittleEndianLength;

        // Taking a arbitrary number here for writing attributes.
        var temp = new byte[20000];
        if (prepopulatedMetricDimensions != null)
        {
            // Initialize numberDataPoint attributes.
            var cursor = 0;
            SerializeTags(temp, ref cursor, prepopulatedMetricDimensions!, FieldNumberConstants.NumberDataPoint_attributes);
            this.prepopulatedNumberDataPointAttributes = new byte[cursor];
            Array.Copy(temp, this.prepopulatedNumberDataPointAttributes, cursor);
            this.prepopulatedNumberDataPointAttributesLength = cursor;

            // Initialize histogramDataPoint attributes.
            cursor = 0;
            SerializeTags(temp, ref cursor, prepopulatedMetricDimensions!, FieldNumberConstants.HistogramDataPoint_attributes);
            this.prepopulatedHistogramDataPointAttributes = new byte[cursor];
            Array.Copy(temp, this.prepopulatedHistogramDataPointAttributes, cursor);
            this.prepopulatedHistogramDataPointAttributesLength = cursor;

            cursor = 0;
            SerializeTags(temp, ref cursor, prepopulatedMetricDimensions!, FieldNumberConstants.ExponentialHistogramDataPoint_attributes);
            this.prepopulatedExponentialHistogramDataPointAttributes = new byte[cursor];
            Array.Copy(temp, this.prepopulatedExponentialHistogramDataPointAttributes, cursor);
            this.prepopulatedExponentialHistogramDataPointAttributesLength = cursor;
        }
    }

    internal static void WriteInstrumentDetails(byte[] buffer, ref int cursor, Metric metric)
    {
        // Write metric name
        ProtobufSerializerHelper.WriteStringTag(buffer, ref cursor, FieldNumberConstants.Metric_name, metric.Name);

        // Write metric description
        if (metric.Description != null)
        {
            ProtobufSerializerHelper.WriteStringTag(buffer, ref cursor, FieldNumberConstants.Metric_description, metric.Description);
        }

        // Write metric unit
        if (metric.Unit != null)
        {
            ProtobufSerializerHelper.WriteStringTag(buffer, ref cursor, FieldNumberConstants.Metric_unit, metric.Unit);
        }
    }

    internal static void SerializeInstrumentationScope(byte[] buffer, ref int cursor, string name, IEnumerable<KeyValuePair<string, object?>>? meterTags)
    {
        var tagAndLengthIndex = cursor;
        cursor += TagAndLengthSize;
        var valueIndex = cursor;

        // Write name
        ProtobufSerializerHelper.WriteStringTag(buffer, ref cursor, FieldNumberConstants.InstrumentationScope_name, name);

        SerializeTags(buffer, ref cursor, meterTags, FieldNumberConstants.InstrumentationScope_attributes);

        // Write instrumentation Scope Tag
        ProtobufSerializerHelper.WriteTagAndLengthPrefix(buffer, ref tagAndLengthIndex, cursor - valueIndex, FieldNumberConstants.ScopeMetrics_scope, WireType.LEN);
    }

    internal static void SerializeTags(byte[] buffer, ref int cursor, ReadOnlyTagCollection tags, int fieldNumber)
    {
        foreach (var tag in tags)
        {
            if (tag.Value != null)
            {
                SerializeTag(buffer, ref cursor, tag.Key, tag.Value, fieldNumber);
            }
        }
    }

    internal static void SerializeTag(byte[] buffer, ref int cursor, string key, object value, int fieldNumber)
    {
        try
        {
            // TODO : Check if calculating the length in advance could be more efficient in this case.
            // That way we wouldn't have to leave the fixed length space.
            var keyValueTagAndLengthIndex = cursor;
            cursor += TagAndLengthSize;
            var keyValueIndex = cursor;

            ProtobufSerializerHelper.WriteStringTag(buffer, ref cursor, FieldNumberConstants.KeyValue_key, key);

            var anyValueTagAndLengthIndex = cursor;
            cursor += TagAndLengthSize;
            var anyValueIndex = cursor;

            switch (value)
            {
                case char:
                case string:
                    ProtobufSerializerHelper.WriteStringTag(buffer, ref cursor, FieldNumberConstants.AnyValue_string_value, Convert.ToString(value, CultureInfo.InvariantCulture)!);
                    break;
                case bool b:
                    ProtobufSerializerHelper.WriteBoolWithTag(buffer, ref cursor, FieldNumberConstants.AnyValue_bool_value, (bool)value);
                    break;
                case byte:
                case sbyte:
                case short:
                case ushort:
                case int:
                case uint:
                case long:
                case ulong:
                    ProtobufSerializerHelper.WriteInt64WithTag(buffer, ref cursor, FieldNumberConstants.AnyValue_int_value, (ulong)Convert.ToInt64(value, CultureInfo.InvariantCulture));
                    break;
                case float:
                case double:
                    ProtobufSerializerHelper.WriteDoubleWithTag(buffer, ref cursor, FieldNumberConstants.AnyValue_double_value, Convert.ToDouble(value, CultureInfo.InvariantCulture));
                    break;
                default:
                    string repr;
                    try
                    {
                        repr = Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;
                    }
                    catch
                    {
                        repr = $"ERROR: type {value.GetType().FullName} is not supported";
                    }

                    ProtobufSerializerHelper.WriteStringTag(buffer, ref cursor, FieldNumberConstants.AnyValue_string_value, repr);
                    break;

                    // TODO: Handle array type.
            }

            ProtobufSerializerHelper.WriteTagAndLengthPrefix(buffer, ref anyValueTagAndLengthIndex, cursor - anyValueIndex, FieldNumberConstants.KeyValue_value, WireType.LEN);
            ProtobufSerializerHelper.WriteTagAndLengthPrefix(buffer, ref keyValueTagAndLengthIndex, cursor - keyValueIndex, fieldNumber, WireType.LEN);
        }
        catch
        {
            // TODO: log exception.
        }
    }

    internal ExportResult SerializeAndSendMetrics(byte[] buffer, Resource resource, in Batch<Metric> metricBatch)
    {
        this.metricExportResult = ExportResult.Success;

        foreach (var metric in metricBatch)
        {
            if (this.scopeMetrics.TryGetValue(metric.MeterName, out var metricList))
            {
                metricList.Add(metric);
            }
            else
            {
                var newList = new List<Metric>() { metric };
                this.scopeMetrics[metric.MeterName] = newList;
            }
        }

        // Serialize
        this.SerializeResourceMetrics(buffer, resource);

        this.ClearScopeMetrics();

        return this.metricExportResult;
    }

    internal void ClearScopeMetrics()
    {
        foreach (var entry in this.scopeMetrics)
        {
            entry.Value.Clear();
        }
    }

    internal void SerializeResourceMetrics(byte[] buffer, Resource resource)
    {
        var cursor = this.prefixBufferWithUInt32LittleEndianLength ? 4 : 0;

        this.resourceMetricTagAndLengthIndex = cursor;

        this.resourceMetricValueIndex = cursor + TagAndLengthSize;

        cursor = this.resourceMetricValueIndex;

        // Serialize Resource
        // TODO: Avoid serializing it multiple times.
        this.SerializeResource(buffer, ref cursor, resource);

        // TODO: Serialize schema_url field

        // Serialize ScopeMetrics field
        this.scopeMetricsTagAndLengthIndex = cursor;
        this.scopeMetricsValueIndex = cursor + TagAndLengthSize;
        foreach (var entry in this.scopeMetrics)
        {
            if (entry.Value.Count > 0)
            {
                // Reset cursor to write new scopeMetric
                cursor = this.scopeMetricsValueIndex;

                // Serialize this meter/scope
                this.SerializeScopeMetrics(buffer, ref cursor, entry.Key, entry.Value);
            }
        }
    }

    internal void SerializeScopeMetrics(byte[] buffer, ref int cursor, string scopeName, List<Metric> metrics)
    {
        // Serialize scope information
        // TODO: Avoid serializing for each export.
        SerializeInstrumentationScope(buffer, ref cursor, scopeName, metrics[0].MeterTags);

        this.metricTagAndLengthIndex = cursor;
        this.metricValueIndex = cursor + TagAndLengthSize;
        foreach (var metric in metrics)
        {
            // Reset cursor to write new metric
            cursor = this.metricValueIndex;

            // Serialize metrics for the meter/scope
            this.SerializeMetric(buffer, ref cursor, metric);
        }

        // TODO: Serialize schema_url field.
    }

    private static void SerializeExemplar<T>(byte[] buffer, ref int cursor, in Exemplar exemplar, T value, int fieldNumber)
    {
        var exemplarTagAndLengthIndex = cursor;
        cursor += TagAndLengthSize;
        var valueIndex = cursor;

        SerializeExemplarTags(buffer, ref cursor, exemplar.FilteredTags);

        if (typeof(T) == typeof(long))
        {
            // Casting to ulong is ok here as the bit representation for long versus ulong will be the same
            // The difference would in the way the bit representation is interpreted on decoding side (signed versus unsigned)
            ProtobufSerializerHelper.WriteFixed64WithTag(buffer, ref cursor, FieldNumberConstants.Exemplar_as_int, (ulong)(long)(object)value!);
        }
        else if (typeof(T) == typeof(double))
        {
            ProtobufSerializerHelper.WriteDoubleWithTag(buffer, ref cursor, FieldNumberConstants.Exemplar_as_double, (double)(object)value!);
        }

        var time = (ulong)exemplar.Timestamp.ToUnixTimeNanoseconds();
        ProtobufSerializerHelper.WriteFixed64WithTag(buffer, ref cursor, FieldNumberConstants.Exemplar_time_unix_nano, time);

        if (exemplar.SpanId != default)
        {
            ProtobufSerializerHelper.WriteTagAndLengthPrefix(buffer, ref cursor, 16, FieldNumberConstants.Exemplar_trace_id, WireType.LEN);
            var traceBytes = new Span<byte>(buffer, cursor, 16);
            exemplar.TraceId.CopyTo(traceBytes);
            cursor += 16;
            ProtobufSerializerHelper.WriteTagAndLengthPrefix(buffer, ref cursor, 8, FieldNumberConstants.Exemplar_span_id, WireType.LEN);
            var spanBytes = new Span<byte>(buffer, cursor, 8);
            exemplar.SpanId.CopyTo(spanBytes);
            cursor += 8;
        }

        ProtobufSerializerHelper.WriteTagAndLengthPrefix(buffer, ref exemplarTagAndLengthIndex, cursor - valueIndex, fieldNumber, WireType.LEN);
    }

    private static void SerializeExemplarTags(byte[] buffer, ref int cursor, ReadOnlyFilteredTagCollection tags)
    {
        foreach (var tag in tags)
        {
            if (tag.Value != null)
            {
                SerializeTag(buffer, ref cursor, tag.Key, tag.Value, FieldNumberConstants.Exemplar_attributes);
            }
        }
    }

    private static void SerializeTags(byte[] buffer, ref int cursor, IEnumerable<KeyValuePair<string, object?>>? attributes, int fieldNumber)
    {
        if (attributes != null)
        {
            foreach (var tag in attributes)
            {
                if (tag.Value != null)
                {
                    SerializeTag(buffer, ref cursor, tag.Key, tag.Value, fieldNumber);
                }
            }
        }
    }

    private void SerializeMetric(byte[] buffer, ref int cursor, Metric metric)
    {
        WriteInstrumentDetails(buffer, ref cursor, metric);

        this.instrumentTagAndLengthIndex = cursor;
        this.instrumentValueIndex = cursor + TagAndLengthSize;
        switch (metric.MetricType)
        {
            case MetricType.LongSum:
            case MetricType.LongSumNonMonotonic:
                {
                    cursor = this.instrumentValueIndex;

                    // Write isMonotonic tag
                    ProtobufSerializerHelper.WriteBoolWithTag(buffer, ref cursor, FieldNumberConstants.Sum_is_monotonic, metric.MetricType == MetricType.LongSum);

                    // Write aggregationTemporality tag
                    ProtobufSerializerHelper.WriteEnumWithTag(buffer, ref cursor, FieldNumberConstants.Sum_aggregation_temporality, metric.Temporality == AggregationTemporality.Cumulative ? 2 : 1);

                    this.metricPointTagAndLengthIndex = cursor;
                    this.metricPointValueIndex = cursor + TagAndLengthSize;
                    foreach (ref readonly var metricPoint in metric.GetMetricPoints())
                    {
                        try
                        {
                            // Reset cursor to write new metricPoint
                            cursor = this.metricPointValueIndex;

                            var sum = metricPoint.GetSumLong();

                            this.WriteNumberDataPoint(buffer, ref cursor, FieldNumberConstants.Sum_data_points, in metricPoint, sum);

                            // Finish writing current batch
                            this.WriteIndividualMessageTagsAndLength(buffer, ref cursor, metric.MetricType);

                            // Send metricPoint
                            this.SendMetricPoint(buffer, ref cursor);
                        }
                        catch (Exception ex)
                        {
                            this.metricExportResult = ExportResult.Failure;
                            ExporterEventSource.Log.FailedToSerializeMetric(metric.Name, ex);
                        }
                    }

                    break;
                }

            case MetricType.DoubleSum:
            case MetricType.DoubleSumNonMonotonic:
                {
                    cursor = this.instrumentValueIndex;

                    // Write isMonotonic tag
                    ProtobufSerializerHelper.WriteBoolWithTag(buffer, ref cursor, FieldNumberConstants.Sum_is_monotonic, metric.MetricType == MetricType.DoubleSum);

                    // Write aggregationTemporality tag
                    ProtobufSerializerHelper.WriteEnumWithTag(buffer, ref cursor, FieldNumberConstants.Sum_aggregation_temporality, metric.Temporality == AggregationTemporality.Cumulative ? 2 : 1);

                    this.metricPointTagAndLengthIndex = cursor;
                    this.metricPointValueIndex = cursor + TagAndLengthSize;
                    foreach (ref readonly var metricPoint in metric.GetMetricPoints())
                    {
                        try
                        {
                            // Reset cursor to write new metricPoint
                            cursor = this.metricPointValueIndex;

                            var sum = metricPoint.GetSumDouble();

                            this.WriteNumberDataPoint(buffer, ref cursor, FieldNumberConstants.Sum_data_points, in metricPoint, sum);

                            // Finish writing current batch
                            this.WriteIndividualMessageTagsAndLength(buffer, ref cursor, metric.MetricType);

                            // Send metricPoint
                            this.SendMetricPoint(buffer, ref cursor);
                        }
                        catch (Exception ex)
                        {
                            this.metricExportResult = ExportResult.Failure;
                            ExporterEventSource.Log.FailedToSerializeMetric(metric.Name, ex);
                        }
                    }

                    break;
                }

            case MetricType.LongGauge:
                {
                    cursor = this.instrumentValueIndex;

                    this.metricPointTagAndLengthIndex = cursor;
                    this.metricPointValueIndex = cursor + TagAndLengthSize;
                    foreach (ref readonly var metricPoint in metric.GetMetricPoints())
                    {
                        try
                        {
                            // Reset cursor to write new metricPoint
                            cursor = this.metricPointValueIndex;

                            var lastValue = metricPoint.GetGaugeLastValueLong();

                            this.WriteNumberDataPoint(buffer, ref cursor, FieldNumberConstants.Gauge_data_points, in metricPoint, lastValue);

                            // Finish writing current batch
                            this.WriteIndividualMessageTagsAndLength(buffer, ref cursor, metric.MetricType);

                            // Send metricPoint
                            this.SendMetricPoint(buffer, ref cursor);
                        }
                        catch (Exception ex)
                        {
                            this.metricExportResult = ExportResult.Failure;
                            ExporterEventSource.Log.FailedToSerializeMetric(metric.Name, ex);
                        }
                    }

                    break;
                }

            case MetricType.DoubleGauge:
                {
                    cursor = this.instrumentValueIndex;

                    this.metricPointTagAndLengthIndex = cursor;
                    this.metricPointValueIndex = cursor + TagAndLengthSize;
                    foreach (ref readonly var metricPoint in metric.GetMetricPoints())
                    {
                        try
                        {
                            // Reset cursor to write new metricPoint
                            cursor = this.metricPointValueIndex;

                            var lastValue = metricPoint.GetGaugeLastValueDouble();

                            this.WriteNumberDataPoint(buffer, ref cursor, FieldNumberConstants.Gauge_data_points, in metricPoint, lastValue);

                            // Finish writing current batch
                            this.WriteIndividualMessageTagsAndLength(buffer, ref cursor, metric.MetricType);

                            // Send metricPoint
                            this.SendMetricPoint(buffer, ref cursor);
                        }
                        catch (Exception ex)
                        {
                            this.metricExportResult = ExportResult.Failure;
                            ExporterEventSource.Log.FailedToSerializeMetric(metric.Name, ex);
                        }
                    }

                    break;
                }

            case MetricType.Histogram:
                {
                    cursor = this.instrumentValueIndex;

                    // Write aggregationTemporality tag
                    ProtobufSerializerHelper.WriteEnumWithTag(buffer, ref cursor, FieldNumberConstants.Histogram_aggregation_temporality, metric.Temporality == AggregationTemporality.Cumulative ? 2 : 1);

                    this.metricPointTagAndLengthIndex = cursor;
                    this.metricPointValueIndex = cursor + TagAndLengthSize;
                    foreach (ref readonly var metricPoint in metric.GetMetricPoints())
                    {
                        try
                        {
                            // Reset cursor to write new metricPoint
                            cursor = this.metricPointValueIndex;

                            var startTime = (ulong)metricPoint.StartTime.ToUnixTimeNanoseconds();
                            ProtobufSerializerHelper.WriteFixed64WithTag(buffer, ref cursor, FieldNumberConstants.HistogramDataPoint_start_time_unix_nano, startTime);

                            var endTime = (ulong)metricPoint.EndTime.ToUnixTimeNanoseconds();
                            ProtobufSerializerHelper.WriteFixed64WithTag(buffer, ref cursor, FieldNumberConstants.HistogramDataPoint_time_unix_nano, endTime);

                            SerializeTags(buffer, ref cursor, metricPoint.Tags, FieldNumberConstants.HistogramDataPoint_attributes);

                            if (this.prepopulatedHistogramDataPointAttributes != null)
                            {
                                Array.Copy(this.prepopulatedHistogramDataPointAttributes, 0, buffer, cursor, this.prepopulatedHistogramDataPointAttributesLength);
                                cursor += this.prepopulatedHistogramDataPointAttributesLength;
                            }

                            var count = (ulong)metricPoint.GetHistogramCount();
                            ProtobufSerializerHelper.WriteFixed64WithTag(buffer, ref cursor, FieldNumberConstants.HistogramDataPoint_count, count);

                            var sum = metricPoint.GetHistogramSum();
                            ProtobufSerializerHelper.WriteDoubleWithTag(buffer, ref cursor, FieldNumberConstants.HistogramDataPoint_sum, sum);

                            if (metricPoint.TryGetHistogramMinMaxValues(out var min, out var max))
                            {
                                ProtobufSerializerHelper.WriteDoubleWithTag(buffer, ref cursor, FieldNumberConstants.HistogramDataPoint_min, min);
                                ProtobufSerializerHelper.WriteDoubleWithTag(buffer, ref cursor, FieldNumberConstants.HistogramDataPoint_max, max);
                            }

                            foreach (var histogramMeasurement in metricPoint.GetHistogramBuckets())
                            {
                                var bucketCount = (ulong)histogramMeasurement.BucketCount;

                                ProtobufSerializerHelper.WriteFixed64WithTag(buffer, ref cursor, FieldNumberConstants.HistogramDataPoint_bucket_counts, bucketCount);
                                if (histogramMeasurement.ExplicitBound != double.PositiveInfinity)
                                {
                                    ProtobufSerializerHelper.WriteDoubleWithTag(buffer, ref cursor, FieldNumberConstants.HistogramDataPoint_explicit_bounds, histogramMeasurement.ExplicitBound);
                                }
                            }

                            if (metricPoint.TryGetExemplars(out var exemplars))
                            {
                                foreach (ref readonly var exemplar in exemplars)
                                {
                                    SerializeExemplar(buffer, ref cursor, in exemplar, exemplar.DoubleValue, FieldNumberConstants.HistogramDataPoint_exemplars);
                                }
                            }

                            var metricPointStartPosition = this.metricPointTagAndLengthIndex;

                            // Write histogramdatapoint {Repeated field}
                            ProtobufSerializerHelper.WriteTagAndLengthPrefix(buffer, ref metricPointStartPosition, cursor - this.metricPointValueIndex, FieldNumberConstants.Histogram_data_points, WireType.LEN);

                            // Finish writing current batch
                            this.WriteIndividualMessageTagsAndLength(buffer, ref cursor, metric.MetricType);

                            // Send metricPoint
                            this.SendMetricPoint(buffer, ref cursor);
                        }
                        catch (Exception ex)
                        {
                            this.metricExportResult = ExportResult.Failure;
                            ExporterEventSource.Log.FailedToSerializeMetric(metric.Name, ex);
                        }
                    }

                    break;
                }

            case MetricType.ExponentialHistogram:
                {
                    cursor = this.instrumentValueIndex;

                    // Write aggregationTemporality tag
                    ProtobufSerializerHelper.WriteEnumWithTag(buffer, ref cursor, FieldNumberConstants.ExponentialHistogram_aggregation_temporality, metric.Temporality == AggregationTemporality.Cumulative ? 2 : 1);

                    this.metricPointTagAndLengthIndex = cursor;
                    this.metricPointValueIndex = cursor + TagAndLengthSize;

                    foreach (ref readonly var metricPoint in metric.GetMetricPoints())
                    {
                        try
                        {
                            // Reset cursor to write new metricPoint
                            cursor = this.metricPointValueIndex;

                            var startTime = (ulong)metricPoint.StartTime.ToUnixTimeNanoseconds();
                            ProtobufSerializerHelper.WriteFixed64WithTag(buffer, ref cursor, FieldNumberConstants.ExponentialHistogramDataPoint_start_time_unix_nano, startTime);

                            var endTime = (ulong)metricPoint.EndTime.ToUnixTimeNanoseconds();
                            ProtobufSerializerHelper.WriteFixed64WithTag(buffer, ref cursor, FieldNumberConstants.ExponentialHistogramDataPoint_time_unix_nano, endTime);

                            SerializeTags(buffer, ref cursor, metricPoint.Tags, FieldNumberConstants.ExponentialHistogramDataPoint_attributes);

                            if (this.prepopulatedExponentialHistogramDataPointAttributes != null)
                            {
                                Array.Copy(this.prepopulatedExponentialHistogramDataPointAttributes, 0, buffer, cursor, this.prepopulatedExponentialHistogramDataPointAttributesLength);
                                cursor += this.prepopulatedExponentialHistogramDataPointAttributesLength;
                            }

                            var sum = metricPoint.GetHistogramSum();

                            ProtobufSerializerHelper.WriteDoubleWithTag(buffer, ref cursor, FieldNumberConstants.ExponentialHistogramDataPoint_sum, sum);

                            var count = (ulong)metricPoint.GetHistogramCount();
                            ProtobufSerializerHelper.WriteFixed64WithTag(buffer, ref cursor, FieldNumberConstants.ExponentialHistogramDataPoint_count, count);

                            if (metricPoint.TryGetHistogramMinMaxValues(out var min, out var max))
                            {
                                ProtobufSerializerHelper.WriteDoubleWithTag(buffer, ref cursor, FieldNumberConstants.ExponentialHistogramDataPoint_min, min);
                                ProtobufSerializerHelper.WriteDoubleWithTag(buffer, ref cursor, FieldNumberConstants.ExponentialHistogramDataPoint_max, max);
                            }

                            var exponentialHistogramData = metricPoint.GetExponentialHistogramData();

                            ProtobufSerializerHelper.WriteSInt32WithTag(buffer, ref cursor, FieldNumberConstants.ExponentialHistogramDataPoint_scale, exponentialHistogramData.Scale);

                            ProtobufSerializerHelper.WriteFixed64WithTag(buffer, ref cursor, FieldNumberConstants.ExponentialHistogramDataPoint_zero_count, (ulong)exponentialHistogramData.ZeroCount);

                            var positiveBucketIndex = cursor;
                            cursor += TagAndLengthSize;
                            var positiveBucketValueIndex = cursor;

                            ProtobufSerializerHelper.WriteSInt32WithTag(buffer, ref cursor, FieldNumberConstants.Bucket_offset, exponentialHistogramData.PositiveBuckets.Offset);

                            foreach (var bucketCount in exponentialHistogramData.PositiveBuckets)
                            {
                                ProtobufSerializerHelper.WriteInt64WithTag(buffer, ref cursor, FieldNumberConstants.Bucket_bucket_counts, (ulong)bucketCount);
                            }

                            // write Bucket
                            ProtobufSerializerHelper.WriteTagAndLengthPrefix(buffer, ref positiveBucketIndex, cursor - positiveBucketValueIndex, FieldNumberConstants.ExponentialHistogramDataPoint_positive, WireType.LEN);

                            if (metricPoint.TryGetExemplars(out var exemplars))
                            {
                                foreach (ref readonly var exemplar in exemplars)
                                {
                                    SerializeExemplar(buffer, ref cursor, in exemplar, exemplar.DoubleValue, FieldNumberConstants.ExponentialHistogramDataPoint_exemplars);
                                }
                            }

                            var metricPointStartPosition = this.metricPointTagAndLengthIndex;

                            // Write exponentialhistogramdatapoint {Repeated field}
                            ProtobufSerializerHelper.WriteTagAndLengthPrefix(buffer, ref metricPointStartPosition, cursor - this.metricPointValueIndex, FieldNumberConstants.ExponentialHistogram_data_points, WireType.LEN);

                            // Finish writing current batch
                            this.WriteIndividualMessageTagsAndLength(buffer, ref cursor, metric.MetricType);

                            // Send metricPoint
                            this.SendMetricPoint(buffer, ref cursor);
                        }
                        catch (Exception ex)
                        {
                            this.metricExportResult = ExportResult.Failure;
                            ExporterEventSource.Log.FailedToSerializeMetric(metric.Name, ex);
                        }
                    }

                    break;
                }

            default:
                break;
        }
    }

    private void WriteNumberDataPoint<T>(byte[] buffer, ref int cursor, int fieldNumber, in MetricPoint metricPoint, T value)
    {
        if (typeof(T) == typeof(long))
        {
            // Casting to ulong is ok here as the bit representation for long versus ulong will be the same
            // The difference would in the way the bit representation is interpreted on decoding side (signed versus unsigned)
            ProtobufSerializerHelper.WriteFixed64WithTag(buffer, ref cursor, FieldNumberConstants.NumberDataPoint_as_int, (ulong)(long)(object)value!);
        }
        else if (typeof(T) == typeof(double))
        {
            ProtobufSerializerHelper.WriteDoubleWithTag(buffer, ref cursor, FieldNumberConstants.NumberDataPoint_as_double, (double)(object)value!);
        }

        var startTime = (ulong)metricPoint.StartTime.ToUnixTimeNanoseconds();
        ProtobufSerializerHelper.WriteFixed64WithTag(buffer, ref cursor, FieldNumberConstants.NumberDataPoint_start_time_unix_nano, startTime);

        var endTime = (ulong)metricPoint.EndTime.ToUnixTimeNanoseconds();
        ProtobufSerializerHelper.WriteFixed64WithTag(buffer, ref cursor, FieldNumberConstants.NumberDataPoint_time_unix_nano, endTime);

        SerializeTags(buffer, ref cursor, metricPoint.Tags, FieldNumberConstants.NumberDataPoint_attributes);

        if (this.prepopulatedNumberDataPointAttributes != null)
        {
            Array.Copy(this.prepopulatedNumberDataPointAttributes, 0, buffer, cursor, this.prepopulatedHistogramDataPointAttributesLength);
            cursor += this.prepopulatedNumberDataPointAttributesLength;
        }

        if (metricPoint.TryGetExemplars(out var exemplars))
        {
            foreach (ref readonly var exemplar in exemplars)
            {
                if (typeof(T) == typeof(long))
                {
                    SerializeExemplar(buffer, ref cursor, in exemplar, exemplar.LongValue, FieldNumberConstants.NumberDataPoint_exemplars);
                }
                else if (typeof(T) == typeof(double))
                {
                    SerializeExemplar(buffer, ref cursor, in exemplar, exemplar.DoubleValue, FieldNumberConstants.NumberDataPoint_exemplars);
                }
            }
        }

        var metricPointStartPosition = this.metricPointTagAndLengthIndex;

        // Write numberdatapoint {Repeated field}
        ProtobufSerializerHelper.WriteTagAndLengthPrefix(buffer, ref metricPointStartPosition, cursor - this.metricPointValueIndex, fieldNumber, WireType.LEN);
    }

    private void WriteIndividualMessageTagsAndLength(byte[] buffer, ref int cursor, MetricType metricType)
    {
        var instrumentIndex = this.instrumentTagAndLengthIndex;

        var metricIndex = this.metricTagAndLengthIndex;

        var scopeMetricsIndex = this.scopeMetricsTagAndLengthIndex;

        var resourceMetricIndex = this.resourceMetricTagAndLengthIndex;

        // Write instrument tag and length
        ProtobufSerializerHelper.WriteTagAndLengthPrefix(buffer, ref instrumentIndex, cursor - this.instrumentValueIndex, FieldNumberConstants.GetMetricTypeFieldNumber(metricType), WireType.LEN);

        // Write metric tag and length
        ProtobufSerializerHelper.WriteTagAndLengthPrefix(buffer, ref metricIndex, cursor - this.metricValueIndex, FieldNumberConstants.ScopeMetrics_metrics, WireType.LEN);

        // Write scope tag and length
        ProtobufSerializerHelper.WriteTagAndLengthPrefix(buffer, ref scopeMetricsIndex, cursor - this.scopeMetricsValueIndex, FieldNumberConstants.ResourceMetrics_scope_metrics, WireType.LEN);

        // Write resource metric tag and length
        ProtobufSerializerHelper.WriteTagAndLengthPrefix(buffer, ref resourceMetricIndex, cursor - this.resourceMetricValueIndex, FieldNumberConstants.ResourceMetrics_resource, WireType.LEN);

        if (this.prefixBufferWithUInt32LittleEndianLength)
        {
            BinaryPrimitives.WriteUInt32LittleEndian(buffer, (uint)cursor - 4);
        }
    }

    private void SendMetricPoint(byte[] buffer, ref int cursor)
    {
        this.MetricDataTransport.SendOtlpProtobufEvent(buffer, cursor);
    }

    private void SerializeResource(byte[] buffer, ref int cursor, Resource resource)
    {
        if (resource != Resource.Empty)
        {
            var tagAndLengthIndex = cursor;
            cursor += TagAndLengthSize;
            var valueIndex = cursor;

            SerializeTags(buffer, ref cursor, resource.Attributes!, FieldNumberConstants.Resource_attributes);

            // TODO: check to see if should de-dupe in case the values are also provided via resource attributes.
            if (this.metricAccount != null)
            {
                SerializeTag(buffer, ref cursor, GenevaMetricExporter.DimensionKeyForCustomMonitoringAccount, this.metricAccount, FieldNumberConstants.Resource_attributes);
            }

            if (this.metricNamespace != null)
            {
                SerializeTag(buffer, ref cursor, GenevaMetricExporter.DimensionKeyForCustomMetricsNamespace, this.metricNamespace, FieldNumberConstants.Resource_attributes);
            }

            ProtobufSerializerHelper.WriteTagAndLengthPrefix(buffer, ref tagAndLengthIndex, cursor - valueIndex, FieldNumberConstants.ResourceMetrics_resource, WireType.LEN);
        }
    }
}
