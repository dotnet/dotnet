// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#nullable disable

using System.Diagnostics;
using System.Diagnostics.Metrics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Xunit;
using OtlpCollector = OpenTelemetry.Proto.Collector.Metrics.V1;
using OtlpCommon = OpenTelemetry.Proto.Common.V1;

namespace OpenTelemetry.Exporter.Geneva.Tests;

public abstract class OtlpProtobufMetricExporterTests
{
    public TagList TagList;

    public TagList TagListWithPrepopulatedDimensions;

    private static readonly Dictionary<string, object> prepopulatedMetricDimensions = new()
    {
        { "Dim1", 1 },
        { "Dim2", 2 },
        { "Dim3", 3 },
    };

    private static readonly string[] TagKeys =
    [
        "boolKey",
        "doubleKey",
        "intKey",
        "longKey",
        "negativeDoubleKey",
        "negativeIntKey",
        "negativeLongKey",
        "negativeSbyteKey",
        "negativeShortKey",
        "sByteKey",
        "shortKey",
        "stringValueAsciiKey",
        "stringValueMixAsciiAndUnicodeKey",
        "stringValueUnicodeKey",
        "uintKey",
        "ulongKey",
        "ushortKey"
    ];

    private readonly TagList exemplarTagList;

    protected OtlpProtobufMetricExporterTests()
    {
        this.TagList = default;

        var boolValue = true;
        var doubleValue = 23.45;
        var intValue = 29;
        long longValue = 345;
        var negativeDoubleValue = -23.45;
        var negativeIntValue = -29;
        long negativeLongValue = -97;
        var negativeSbyteValue = sbyte.MinValue;
        short negativeShortValue = -12;
        var sByteValue = sbyte.MaxValue;
        var shortValue = short.MaxValue;
        var stringValueAscii = "TestString";
        var stringValueMixAsciiAndUnicode = "\u0418TestString";
        var stringValueUnicode = "\u0418";
        var uintValue = uint.MaxValue;
        ulong ulongValue = 1234;
        var ushortValue = ushort.MaxValue;

        // Keep the keys in sorted order, Sdk outputs them in sorted order.
        this.TagList.Add(new("boolKey", boolValue));
        this.TagList.Add(new("doubleKey", doubleValue));
        this.TagList.Add(new("intKey", intValue));
        this.TagList.Add(new("longKey", longValue));
        this.TagList.Add(new("negativeDoubleKey", negativeDoubleValue));
        this.TagList.Add(new("negativeIntKey", negativeIntValue));
        this.TagList.Add(new("negativeLongKey", negativeLongValue));
        this.TagList.Add(new("negativeSbyteKey", negativeSbyteValue));
        this.TagList.Add(new("negativeShortKey", negativeShortValue));
        this.TagList.Add(new("sByteKey", sByteValue));
        this.TagList.Add(new("shortKey", shortValue));
        this.TagList.Add(new("stringValueAsciiKey", stringValueAscii));
        this.TagList.Add(new("stringValueMixAsciiAndUnicodeKey", stringValueMixAsciiAndUnicode));
        this.TagList.Add(new("stringValueUnicodeKey", stringValueUnicode));
        this.TagList.Add(new("uintKey", uintValue));
        this.TagList.Add(new("ulongKey", ulongValue));
        this.TagList.Add(new("ushortKey", ushortValue));

        this.exemplarTagList = this.TagList;
        this.exemplarTagList.Add(new("zfilteredKey1", "zfilteredValue1"));
    }

    protected abstract bool PrefixBufferWithUInt32LittleEndianLength { get; }

    [Theory]
    [InlineData("longcounter", 123L, null, true, true, true, true)]
    [InlineData("longcounter", 123L, null, true, true, true, false)]
    [InlineData("longcounter", 123L, null, true, true, false, true)]
    [InlineData("longcounter", 123L, null, true, true, false, false)]
    [InlineData("longcounter", 123L, null, true, false, true, true)]
    [InlineData("longcounter", 123L, null, true, false, true, false)]
    [InlineData("longcounter", 123L, null, true, false, false, true)]
    [InlineData("longcounter", 123L, null, true, false, false, false)]
    [InlineData("longcounter", 123L, null, false, true, true, true)]
    [InlineData("longcounter", 123L, null, false, true, true, false)]
    [InlineData("longcounter", 123L, null, false, true, false, true)]
    [InlineData("longcounter", 123L, null, false, true, false, false)]
    [InlineData("longcounter", 123L, null, false, false, true, true)]
    [InlineData("longcounter", 123L, null, false, false, true, false)]
    [InlineData("longcounter", 123L, null, false, false, false, true)]
    [InlineData("longcounter", 123L, null, false, false, false, false)]
    [InlineData("doublecounter", null, 123.45, true, true, true, true)]
    [InlineData("doublecounter", null, 123.45, true, true, true, false)]
    [InlineData("doublecounter", null, 123.45, true, true, false, true)]
    [InlineData("doublecounter", null, 123.45, true, true, false, false)]
    [InlineData("doublecounter", null, 123.45, true, false, true, true)]
    [InlineData("doublecounter", null, 123.45, true, false, true, false)]
    [InlineData("doublecounter", null, 123.45, true, false, false, true)]
    [InlineData("doublecounter", null, 123.45, true, false, false, false)]
    [InlineData("doublecounter", null, 123.45, false, true, true, true)]
    [InlineData("doublecounter", null, 123.45, false, true, true, false)]
    [InlineData("doublecounter", null, 123.45, false, true, false, true)]
    [InlineData("doublecounter", null, 123.45, false, true, false, false)]
    [InlineData("doublecounter", null, 123.45, false, false, true, true)]
    [InlineData("doublecounter", null, 123.45, false, false, true, false)]
    [InlineData("doublecounter", null, 123.45, false, false, false, true)]
    [InlineData("doublecounter", null, 123.45, false, false, false, false)]
    [InlineData("longcounter", -123L, null, true, true, true, true)]
    [InlineData("longcounter", -123L, null, true, true, true, false)]
    [InlineData("longcounter", -123L, null, true, true, false, true)]
    [InlineData("longcounter", -123L, null, true, true, false, false)]
    [InlineData("longcounter", -123L, null, true, false, true, true)]
    [InlineData("longcounter", -123L, null, true, false, true, false)]
    [InlineData("longcounter", -123L, null, true, false, false, true)]
    [InlineData("longcounter", -123L, null, true, false, false, false)]
    [InlineData("longcounter", -123L, null, false, true, true, true)]
    [InlineData("longcounter", -123L, null, false, true, true, false)]
    [InlineData("longcounter", -123L, null, false, true, false, true)]
    [InlineData("longcounter", -123L, null, false, true, false, false)]
    [InlineData("longcounter", -123L, null, false, false, true, true)]
    [InlineData("longcounter", -123L, null, false, false, true, false)]
    [InlineData("longcounter", -123L, null, false, false, false, true)]
    [InlineData("longcounter", -123L, null, false, false, false, false)]
    [InlineData("doublecounter", null, -123.45, true, true, true, true)]
    [InlineData("doublecounter", null, -123.45, true, true, true, false)]
    [InlineData("doublecounter", null, -123.45, true, true, false, true)]
    [InlineData("doublecounter", null, -123.45, true, true, false, false)]
    [InlineData("doublecounter", null, -123.45, true, false, true, true)]
    [InlineData("doublecounter", null, -123.45, true, false, true, false)]
    [InlineData("doublecounter", null, -123.45, true, false, false, true)]
    [InlineData("doublecounter", null, -123.45, true, false, false, false)]
    [InlineData("doublecounter", null, -123.45, false, true, true, true)]
    [InlineData("doublecounter", null, -123.45, false, true, true, false)]
    [InlineData("doublecounter", null, -123.45, false, true, false, true)]
    [InlineData("doublecounter", null, -123.45, false, true, false, false)]
    [InlineData("doublecounter", null, -123.45, false, false, true, true)]
    [InlineData("doublecounter", null, -123.45, false, false, true, false)]
    [InlineData("doublecounter", null, -123.45, false, false, false, true)]
    [InlineData("doublecounter", null, -123.45, false, false, false, false)]
    public void CounterSerializationSingleMetricPoint(string instrumentName, long? longValue, double? doubleValue, bool addPrepopulatedDimensions, bool addAccountAndNamespace, bool isExemplarsEnabled, bool isTracingEnabled)
    {
        Activity activity = null;

        if (isTracingEnabled)
        {
            activity = new Activity("Custom Activity");
            activity.Start();
        }

        using var meter = new Meter(nameof(this.CounterSerializationSingleMetricPoint), "0.0.1");

        var exportedItems = new List<Metric>();
        using var inMemoryReader = new BaseExportingMetricReader(new InMemoryExporter<Metric>(exportedItems))
        {
            TemporalityPreference = MetricReaderTemporalityPreference.Delta,
        };

        var resourceAttributes = new Dictionary<string, object>
        {
            { "TestResourceKey", "TestResourceValue" },
            { GenevaMetricExporter.DimensionKeyForCustomMonitoringAccount, "ResourceAccount" },
            { GenevaMetricExporter.DimensionKeyForCustomMetricsNamespace, "ResourceNamespace" },
        };

        var expectedAccount = "TestAccount";
        var expectedNamespace = "TestNameSpace";
        var accountAndNamespace = new Dictionary<string, object>
        {
            { GenevaMetricExporter.DimensionKeyForCustomMonitoringAccount, expectedAccount },
            { GenevaMetricExporter.DimensionKeyForCustomMetricsNamespace, expectedNamespace },
        };

        var resourceBuilder = ResourceBuilder.CreateDefault().Clear()
            .AddAttributes(resourceAttributes);
        var meterProviderBuilder = Sdk.CreateMeterProviderBuilder()
            .SetResourceBuilder(resourceBuilder)
            .AddMeter(nameof(this.CounterSerializationSingleMetricPoint))
            .AddReader(inMemoryReader);
        if (isExemplarsEnabled)
        {
            meterProviderBuilder.SetExemplarFilter(ExemplarFilterType.AlwaysOn);
            meterProviderBuilder.AddView("*", new MetricStreamConfiguration { TagKeys = TagKeys });
        }

        var meterProvider = meterProviderBuilder.Build();

        if (longValue != null)
        {
            var counter = meter.CreateCounter<long>(instrumentName);
            if (isExemplarsEnabled)
            {
                counter.Add(longValue.Value, this.exemplarTagList);
            }
            else
            {
                counter.Add(longValue.Value, this.TagList);
            }
        }
        else
        {
            var counter = meter.CreateCounter<double>(instrumentName);
            if (isExemplarsEnabled)
            {
                counter.Add(doubleValue.Value, this.exemplarTagList);
            }
            else
            {
                counter.Add(doubleValue.Value, this.TagList);
            }
        }

        meterProvider.ForceFlush();

        var buffer = new byte[65360];

        var testTransport = new TestTransport();

        var otlpProtobufSerializer = new OtlpProtobufSerializer(
            testTransport,
            addAccountAndNamespace ? expectedAccount : null,
            addAccountAndNamespace ? expectedNamespace : null,
            addPrepopulatedDimensions ? prepopulatedMetricDimensions : null,
            prefixBufferWithUInt32LittleEndianLength: this.PrefixBufferWithUInt32LittleEndianLength);

        otlpProtobufSerializer.SerializeAndSendMetrics(buffer, meterProvider.GetResource(), new Batch<Metric>([.. exportedItems], exportedItems.Count));

        Assert.Single(testTransport.ExportedItems);

        var request = this.AssertAndConvertExportedBlobToRequest(
            testTransport.ExportedItems[0]);

        Assert.Single(request.ResourceMetrics);

        Assert.NotNull(request.ResourceMetrics[0].Resource);

        AssertOtlpAttributes(addAccountAndNamespace ? resourceAttributes.Concat(accountAndNamespace) : resourceAttributes, request.ResourceMetrics[0].Resource.Attributes);

        Assert.Single(request.ResourceMetrics[0].ScopeMetrics);

        var scope = request.ResourceMetrics[0].ScopeMetrics[0];

        Assert.Equal(meter.Name, scope.Scope.Name);

        Assert.Single(request.ResourceMetrics[0].ScopeMetrics[0].Metrics);

        var metric = request.ResourceMetrics[0].ScopeMetrics[0].Metrics[0];

        Assert.Equal(instrumentName, metric.Name);

        Assert.NotNull(metric.Sum);

        Assert.Equal(1, (int)metric.Sum.AggregationTemporality);

        Assert.True(metric.Sum.IsMonotonic);

        Assert.Single(metric.Sum.DataPoints);

        var dataPoint = metric.Sum.DataPoints[0];

        if (longValue != null)
        {
            Assert.Equal(longValue, dataPoint.AsInt);
        }
        else
        {
            Assert.Equal(doubleValue, dataPoint.AsDouble);
        }

        // Assert time
        var metricPointsEnumerator = exportedItems[0].GetMetricPoints().GetEnumerator();
        metricPointsEnumerator.MoveNext();
        var metricPoint = metricPointsEnumerator.Current;

        Assert.Equal((ulong)metricPoint.StartTime.ToUnixTimeNanoseconds(), dataPoint.StartTimeUnixNano);

        Assert.Equal((ulong)metricPoint.EndTime.ToUnixTimeNanoseconds(), dataPoint.TimeUnixNano);

        if (isExemplarsEnabled)
        {
            Assert.Single(dataPoint.Exemplars);

            var exemplar = dataPoint.Exemplars[0];

            metricPoint.TryGetExemplars(out var exemplars);

            var exemplarsEnumerator = exemplars.GetEnumerator();

            exemplarsEnumerator.MoveNext();

            var actualExemplar = exemplarsEnumerator.Current;

            Assert.Equal((ulong)actualExemplar.Timestamp.ToUnixTimeNanoseconds(), exemplar.TimeUnixNano);

            if (longValue != null)
            {
                Assert.Equal(longValue.Value, exemplar.AsInt);
            }
            else
            {
                Assert.Equal(doubleValue.Value, exemplar.AsDouble);
            }

            if (isTracingEnabled)
            {
                var spanIdBytes = new byte[8];
                activity.SpanId.CopyTo(spanIdBytes);

                Assert.True(spanIdBytes.SequenceEqual(exemplar.SpanId));

                var traceIdBytes = new byte[16];
                activity.TraceId.CopyTo(traceIdBytes);

                Assert.True(traceIdBytes.SequenceEqual(exemplar.TraceId));
            }
            else
            {
                Assert.Equal(ByteString.Empty, exemplar.SpanId);
                Assert.Equal(ByteString.Empty, exemplar.TraceId);
            }

            AssertOtlpAttributes([new KeyValuePair<string, object>("zfilteredKey1", "zfilteredValue1")], exemplar.FilteredAttributes);
        }
        else
        {
            Assert.Empty(dataPoint.Exemplars);
        }

        if (addPrepopulatedDimensions)
        {
            AssertOtlpAttributes(this.TagList.Concat(prepopulatedMetricDimensions), dataPoint.Attributes);
        }
        else
        {
            AssertOtlpAttributes(this.TagList, dataPoint.Attributes);
        }
    }

    [Theory]
    [InlineData("longcounter", new long[] { 10, 20, 30 }, null)]
    [InlineData("longcounter", new long[] { -10, 2, -30 }, null)]
    [InlineData("doublecounter", null, new double[] { 10.20, 2, 30.65 })]
    [InlineData("doublecounter", null, new double[] { -10.20, 2, -30.65 })]
    public void CounterSerializationMultipleMetricPoints(string instrumentName, long[] longValues, double[] doubleValues)
    {
        using var meter = new Meter(nameof(this.CounterSerializationMultipleMetricPoints), "0.0.1");
        var exportedItems = new List<Metric>();
        using var inMemoryReader = new BaseExportingMetricReader(new InMemoryExporter<Metric>(exportedItems))
        {
            TemporalityPreference = MetricReaderTemporalityPreference.Delta,
        };

        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddMeter(nameof(this.CounterSerializationMultipleMetricPoints))
            .AddReader(inMemoryReader)
            .Build();

        var expectedMetricPoints = longValues != null ? longValues.Length : doubleValues.Length;
        var tags = new TagList[expectedMetricPoints];

        for (var i = 0; i < tags.Length; i++)
        {
            for (var j = 1; j <= (i + 1); j++)
            {
                tags[i].Add(new("tag" + j, "value" + j));
            }
        }

        if (longValues != null)
        {
            var counter = meter.CreateCounter<long>(instrumentName);

            for (var i = 0; i < longValues.Length; i++)
            {
                counter.Add(longValues[i], tags[i]);
            }
        }
        else
        {
            var counter = meter.CreateCounter<double>(instrumentName);

            for (var i = 0; i < doubleValues.Length; i++)
            {
                counter.Add(doubleValues[i], tags[i]);
            }
        }

        meterProvider.ForceFlush();

        var buffer = new byte[65360];

        var testTransport = new TestTransport();

        var otlpProtobufSerializer = new OtlpProtobufSerializer(
            testTransport,
            metricsAccount: null,
            metricsNamespace: null,
            prepopulatedMetricDimensions: null,
            prefixBufferWithUInt32LittleEndianLength: this.PrefixBufferWithUInt32LittleEndianLength);

        otlpProtobufSerializer.SerializeAndSendMetrics(buffer, Resource.Empty, new Batch<Metric>([.. exportedItems], exportedItems.Count));

        Assert.Equal(expectedMetricPoints, testTransport.ExportedItems.Count);

        // For asserting time
        var metricPointsEnumerator = exportedItems[0].GetMetricPoints().GetEnumerator();

        for (var i = 0; i < expectedMetricPoints; i++)
        {
            var request = this.AssertAndConvertExportedBlobToRequest(
                testTransport.ExportedItems[i]);

            Assert.Single(request.ResourceMetrics);

            Assert.Single(request.ResourceMetrics[0].ScopeMetrics);

            var scope = request.ResourceMetrics[0].ScopeMetrics[0];

            Assert.Equal(meter.Name, scope.Scope.Name);

            Assert.Single(request.ResourceMetrics[0].ScopeMetrics[0].Metrics);

            var metric = request.ResourceMetrics[0].ScopeMetrics[0].Metrics[0];

            Assert.Equal(instrumentName, metric.Name);

            Assert.NotNull(metric.Sum);

            Assert.Equal(1, (int)metric.Sum.AggregationTemporality);

            Assert.True(metric.Sum.IsMonotonic);

            Assert.Single(metric.Sum.DataPoints);

            var dataPoint = metric.Sum.DataPoints[0];

            if (longValues != null)
            {
                Assert.Equal(longValues[i], dataPoint.AsInt);
            }
            else
            {
                Assert.Equal(doubleValues[i], dataPoint.AsDouble);
            }

            metricPointsEnumerator.MoveNext();
            var metricPoint = metricPointsEnumerator.Current;

            Assert.Equal((ulong)metricPoint.StartTime.ToUnixTimeNanoseconds(), dataPoint.StartTimeUnixNano);

            Assert.Equal((ulong)metricPoint.EndTime.ToUnixTimeNanoseconds(), dataPoint.TimeUnixNano);

            AssertOtlpAttributes(tags[i], dataPoint.Attributes);
        }
    }

    [Theory]
    [InlineData("updownlongcounter", 123L, null, true, true)]
    [InlineData("updownlongcounter", 123L, null, true, false)]
    [InlineData("updownlongcounter", 123L, null, false, true)]
    [InlineData("updownlongcounter", 123L, null, false, false)]
    [InlineData("updowndoublecounter", null, 123.45, true, true)]
    [InlineData("updowndoublecounter", null, 123.45, true, false)]
    [InlineData("updowndoublecounter", null, 123.45, false, true)]
    [InlineData("updowndoublecounter", null, 123.45, false, false)]
    [InlineData("updownlongcounter", -123L, null, true, true)]
    [InlineData("updownlongcounter", -123L, null, true, false)]
    [InlineData("updownlongcounter", -123L, null, false, true)]
    [InlineData("updownlongcounter", -123L, null, false, false)]
    [InlineData("updowndoublecounter", null, -123.45, true, true)]
    [InlineData("updowndoublecounter", null, -123.45, true, false)]
    [InlineData("updowndoublecounter", null, -123.45, false, true)]
    [InlineData("updowndoublecounter", null, -123.45, false, false)]
    public void UpdownCounterSerializationSingleMetricPoint(string instrumentName, long? longValue, double? doubleValue, bool addPrepopulatedDimensions, bool addAccountAndNamespace)
    {
        using var meter = new Meter(nameof(this.UpdownCounterSerializationSingleMetricPoint), "0.0.1");

        var exportedItems = new List<Metric>();
        using var inMemoryReader = new BaseExportingMetricReader(new InMemoryExporter<Metric>(exportedItems))
        {
            TemporalityPreference = MetricReaderTemporalityPreference.Delta,
        };

        var resourceAttributes = new Dictionary<string, object>
        {
            { "TestResourceKey", "TestResourceValue" },
            { GenevaMetricExporter.DimensionKeyForCustomMonitoringAccount, "ResourceAccount" },
            { GenevaMetricExporter.DimensionKeyForCustomMetricsNamespace, "ResourceNamespace" },
        };

        var expectedAccount = "TestAccount";
        var expectedNamespace = "TestNameSpace";
        var accountAndNamespace = new Dictionary<string, object>
        {
            { GenevaMetricExporter.DimensionKeyForCustomMonitoringAccount, expectedAccount },
            { GenevaMetricExporter.DimensionKeyForCustomMetricsNamespace, expectedNamespace },
        };

        var resourceBuilder = ResourceBuilder.CreateDefault().Clear()
            .AddAttributes(resourceAttributes);
        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .SetResourceBuilder(resourceBuilder)
            .AddMeter(nameof(this.UpdownCounterSerializationSingleMetricPoint))
            .AddReader(inMemoryReader)
        .Build();

        if (longValue != null)
        {
            var counter = meter.CreateUpDownCounter<long>(instrumentName);
            counter.Add(longValue.Value, this.TagList);
        }
        else
        {
            var counter = meter.CreateUpDownCounter<double>(instrumentName);
            counter.Add(doubleValue.Value, this.TagList);
        }

        meterProvider.ForceFlush();

        var buffer = new byte[65360];

        var testTransport = new TestTransport();

        var otlpProtobufSerializer = new OtlpProtobufSerializer(
            testTransport,
            addAccountAndNamespace ? expectedAccount : null,
            addAccountAndNamespace ? expectedNamespace : null,
            addPrepopulatedDimensions ? prepopulatedMetricDimensions : null,
            prefixBufferWithUInt32LittleEndianLength: this.PrefixBufferWithUInt32LittleEndianLength);

        otlpProtobufSerializer.SerializeAndSendMetrics(buffer, meterProvider.GetResource(), new Batch<Metric>([.. exportedItems], exportedItems.Count));

        Assert.Single(testTransport.ExportedItems);

        var request = this.AssertAndConvertExportedBlobToRequest(
            testTransport.ExportedItems[0]);

        Assert.Single(request.ResourceMetrics);

        Assert.NotNull(request.ResourceMetrics[0].Resource);

        AssertOtlpAttributes(addAccountAndNamespace ? resourceAttributes.Concat(accountAndNamespace) : resourceAttributes, request.ResourceMetrics[0].Resource.Attributes);

        Assert.Single(request.ResourceMetrics[0].ScopeMetrics);

        var scope = request.ResourceMetrics[0].ScopeMetrics[0];

        Assert.Equal(meter.Name, scope.Scope.Name);

        Assert.Single(request.ResourceMetrics[0].ScopeMetrics[0].Metrics);

        var metric = request.ResourceMetrics[0].ScopeMetrics[0].Metrics[0];

        Assert.Equal(instrumentName, metric.Name);

        Assert.NotNull(metric.Sum);

        Assert.Equal(2, (int)metric.Sum.AggregationTemporality);

        Assert.False(metric.Sum.IsMonotonic);

        Assert.Single(metric.Sum.DataPoints);

        var dataPoint = metric.Sum.DataPoints[0];

        if (longValue != null)
        {
            Assert.Equal(longValue, dataPoint.AsInt);
        }
        else
        {
            Assert.Equal(doubleValue, dataPoint.AsDouble);
        }

        // Assert time
        var metricPointsEnumerator = exportedItems[0].GetMetricPoints().GetEnumerator();
        metricPointsEnumerator.MoveNext();
        var metricPoint = metricPointsEnumerator.Current;

        Assert.Equal((ulong)metricPoint.StartTime.ToUnixTimeNanoseconds(), dataPoint.StartTimeUnixNano);

        Assert.Equal((ulong)metricPoint.EndTime.ToUnixTimeNanoseconds(), dataPoint.TimeUnixNano);

        if (addPrepopulatedDimensions)
        {
            AssertOtlpAttributes(this.TagList.Concat(prepopulatedMetricDimensions), dataPoint.Attributes);
        }
        else
        {
            AssertOtlpAttributes(this.TagList, dataPoint.Attributes);
        }
    }

    [Theory]
    [InlineData("updownlongcounter", new long[] { 10, 20, 30 }, null)]
    [InlineData("updownlongcounter", new long[] { 10, -2, 30 }, null)]
    [InlineData("updowndoublecounter", null, new double[] { 10.20, 2, 30.65 })]
    [InlineData("updowndoublecounter", null, new double[] { -10.20, 2, -30.65 })]
    public void UpdownCounterSerializationMultipleMetricPoints(string instrumentName, long[] longValues, double[] doubleValues)
    {
        using var meter = new Meter(nameof(this.UpdownCounterSerializationMultipleMetricPoints), "0.0.1");
        var exportedItems = new List<Metric>();
        using var inMemoryReader = new BaseExportingMetricReader(new InMemoryExporter<Metric>(exportedItems))
        {
            TemporalityPreference = MetricReaderTemporalityPreference.Delta,
        };

        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddMeter(nameof(this.UpdownCounterSerializationMultipleMetricPoints))
            .AddReader(inMemoryReader)
            .Build();

        var expectedMetricPoints = longValues != null ? longValues.Length : doubleValues.Length;
        var tags = new TagList[expectedMetricPoints];

        for (var i = 0; i < tags.Length; i++)
        {
            for (var j = 1; j <= (i + 1); j++)
            {
                tags[i].Add(new("tag" + j, "value" + j));
            }
        }

        if (longValues != null)
        {
            var counter = meter.CreateUpDownCounter<long>(instrumentName);

            for (var i = 0; i < longValues.Length; i++)
            {
                counter.Add(longValues[i], tags[i]);
            }
        }
        else
        {
            var counter = meter.CreateUpDownCounter<double>(instrumentName);

            for (var i = 0; i < doubleValues.Length; i++)
            {
                counter.Add(doubleValues[i], tags[i]);
            }
        }

        meterProvider.ForceFlush();

        var buffer = new byte[65360];

        var testTransport = new TestTransport();

        var otlpProtobufSerializer = new OtlpProtobufSerializer(
            testTransport,
            metricsAccount: null,
            metricsNamespace: null,
            prepopulatedMetricDimensions: null,
            prefixBufferWithUInt32LittleEndianLength: this.PrefixBufferWithUInt32LittleEndianLength);

        otlpProtobufSerializer.SerializeAndSendMetrics(buffer, Resource.Empty, new Batch<Metric>([.. exportedItems], exportedItems.Count));

        Assert.Equal(expectedMetricPoints, testTransport.ExportedItems.Count);

        // For asserting time
        var metricPointsEnumerator = exportedItems[0].GetMetricPoints().GetEnumerator();

        for (var i = 0; i < expectedMetricPoints; i++)
        {
            var request = this.AssertAndConvertExportedBlobToRequest(
                testTransport.ExportedItems[i]);

            Assert.Single(request.ResourceMetrics);

            Assert.Single(request.ResourceMetrics[0].ScopeMetrics);

            var scope = request.ResourceMetrics[0].ScopeMetrics[0];

            Assert.Equal(meter.Name, scope.Scope.Name);

            Assert.Single(request.ResourceMetrics[0].ScopeMetrics[0].Metrics);

            var metric = request.ResourceMetrics[0].ScopeMetrics[0].Metrics[0];

            Assert.Equal(instrumentName, metric.Name);

            Assert.NotNull(metric.Sum);

            Assert.Equal(2, (int)metric.Sum.AggregationTemporality);

            Assert.False(metric.Sum.IsMonotonic);

            Assert.Single(metric.Sum.DataPoints);

            var dataPoint = metric.Sum.DataPoints[0];

            if (longValues != null)
            {
                Assert.Equal(longValues[i], dataPoint.AsInt);
            }
            else
            {
                Assert.Equal(doubleValues[i], dataPoint.AsDouble);
            }

            metricPointsEnumerator.MoveNext();
            var metricPoint = metricPointsEnumerator.Current;

            Assert.Equal((ulong)metricPoint.StartTime.ToUnixTimeNanoseconds(), dataPoint.StartTimeUnixNano);

            Assert.Equal((ulong)metricPoint.EndTime.ToUnixTimeNanoseconds(), dataPoint.TimeUnixNano);

            AssertOtlpAttributes(tags[i], dataPoint.Attributes);
        }
    }

    [Theory]
    [InlineData(123.45, true, true, true, true)]
    [InlineData(123.45, true, true, true, false)]
    [InlineData(123.45, true, true, false, true)]
    [InlineData(123.45, true, true, false, false)]
    [InlineData(123.45, true, false, true, true)]
    [InlineData(123.45, true, false, true, false)]
    [InlineData(123.45, true, false, false, true)]
    [InlineData(123.45, true, false, false, false)]
    [InlineData(123.45, false, true, true, true)]
    [InlineData(123.45, false, true, true, false)]
    [InlineData(123.45, false, true, false, true)]
    [InlineData(123.45, false, true, false, false)]
    [InlineData(123.45, false, false, true, true)]
    [InlineData(123.45, false, false, true, false)]
    [InlineData(123.45, false, false, false, true)]
    [InlineData(123.45, false, false, false, false)]
    [InlineData(-123.45, true, true, true, true)]
    [InlineData(-123.45, true, true, true, false)]
    [InlineData(-123.45, true, true, false, true)]
    [InlineData(-123.45, true, true, false, false)]
    [InlineData(-123.45, true, false, true, true)]
    [InlineData(-123.45, true, false, true, false)]
    [InlineData(-123.45, true, false, false, true)]
    [InlineData(-123.45, true, false, false, false)]
    [InlineData(-123.45, false, true, true, true)]
    [InlineData(-123.45, false, true, true, false)]
    [InlineData(-123.45, false, true, false, true)]
    [InlineData(-123.45, false, true, false, false)]
    [InlineData(-123.45, false, false, true, true)]
    [InlineData(-123.45, false, false, true, false)]
    [InlineData(-123.45, false, false, false, true)]
    [InlineData(-123.45, false, false, false, false)]
    public void HistogramSerializationSingleMetricPoint(double doubleValue, bool addPrepopulatedDimensions, bool addAccountAndNamespace, bool isExemplarsEnabled, bool isTracingEnabled)
    {
        Activity activity = null;

        if (isTracingEnabled)
        {
            activity = new Activity("Custom Activity");
            activity.Start();
        }

        using var meter = new Meter(nameof(this.HistogramSerializationSingleMetricPoint), "0.0.1");

        var exportedItems = new List<Metric>();
        using var inMemoryReader = new BaseExportingMetricReader(new InMemoryExporter<Metric>(exportedItems))
        {
            TemporalityPreference = MetricReaderTemporalityPreference.Delta,
        };

        var resourceAttributes = new Dictionary<string, object>
        {
            { "TestResourceKey", "TestResourceValue" },
            { GenevaMetricExporter.DimensionKeyForCustomMonitoringAccount, "ResourceAccount" },
            { GenevaMetricExporter.DimensionKeyForCustomMetricsNamespace, "ResourceNamespace" },
        };

        var expectedAccount = "TestAccount";
        var expectedNamespace = "TestNameSpace";
        var accountAndNamespace = new Dictionary<string, object>
        {
            { GenevaMetricExporter.DimensionKeyForCustomMonitoringAccount, expectedAccount },
            { GenevaMetricExporter.DimensionKeyForCustomMetricsNamespace, expectedNamespace },
        };

        var resourceBuilder = ResourceBuilder.CreateDefault().Clear()
            .AddAttributes(resourceAttributes);
        var meterProviderBuilder = Sdk.CreateMeterProviderBuilder()
           .SetResourceBuilder(resourceBuilder)
           .AddMeter(nameof(this.HistogramSerializationSingleMetricPoint))
           .AddReader(inMemoryReader);
        if (isExemplarsEnabled)
        {
            meterProviderBuilder.SetExemplarFilter(ExemplarFilterType.AlwaysOn);
            meterProviderBuilder.AddView("*", new MetricStreamConfiguration { TagKeys = TagKeys });
        }

        var meterProvider = meterProviderBuilder.Build();

        var histogram = meter.CreateHistogram<double>("TestHistogram");

        if (isExemplarsEnabled)
        {
            histogram.Record(doubleValue, this.exemplarTagList);
        }
        else
        {
            histogram.Record(doubleValue, this.TagList);
        }

        meterProvider.ForceFlush();

        var buffer = new byte[65360];

        var testTransport = new TestTransport();

        var otlpProtobufSerializer = new OtlpProtobufSerializer(
            testTransport,
            addAccountAndNamespace ? expectedAccount : null,
            addAccountAndNamespace ? expectedNamespace : null,
            addPrepopulatedDimensions ? prepopulatedMetricDimensions : null,
            prefixBufferWithUInt32LittleEndianLength: this.PrefixBufferWithUInt32LittleEndianLength);

        otlpProtobufSerializer.SerializeAndSendMetrics(buffer, meterProvider.GetResource(), new Batch<Metric>([.. exportedItems], exportedItems.Count));

        Assert.Single(testTransport.ExportedItems);

        var request = this.AssertAndConvertExportedBlobToRequest(
            testTransport.ExportedItems[0]);

        Assert.Single(request.ResourceMetrics);

        Assert.NotNull(request.ResourceMetrics[0].Resource);

        AssertOtlpAttributes(addAccountAndNamespace ? resourceAttributes.Concat(accountAndNamespace) : resourceAttributes, request.ResourceMetrics[0].Resource.Attributes);

        Assert.Single(request.ResourceMetrics[0].ScopeMetrics);

        var scope = request.ResourceMetrics[0].ScopeMetrics[0];

        Assert.Equal(meter.Name, scope.Scope.Name);

        Assert.Single(request.ResourceMetrics[0].ScopeMetrics[0].Metrics);

        var metric = request.ResourceMetrics[0].ScopeMetrics[0].Metrics[0];

        Assert.Equal("TestHistogram", metric.Name);

        Assert.NotNull(metric.Histogram);

        Assert.Equal(1, (int)metric.Histogram.AggregationTemporality);

        Assert.Single(metric.Histogram.DataPoints);

        var dataPoint = metric.Histogram.DataPoints[0];

        Assert.Equal(doubleValue, dataPoint.Sum);

        Assert.Equal(doubleValue, dataPoint.Max);

        Assert.Equal(doubleValue, dataPoint.Min);

        // Assert time
        var metricPointsEnumerator = exportedItems[0].GetMetricPoints().GetEnumerator();
        metricPointsEnumerator.MoveNext();
        var metricPoint = metricPointsEnumerator.Current;

        var bucketCountIndex = 0;
        var explicitBoundCountIndex = 0;

        foreach (var histogramMeasurement in metricPoint.GetHistogramBuckets())
        {
            var bucketCount = (ulong)histogramMeasurement.BucketCount;

            Assert.Equal(bucketCount, dataPoint.BucketCounts[bucketCountIndex]);

            if (histogramMeasurement.ExplicitBound != double.PositiveInfinity)
            {
                Assert.Equal(histogramMeasurement.ExplicitBound, dataPoint.ExplicitBounds[explicitBoundCountIndex]);
                explicitBoundCountIndex++;
            }

            bucketCountIndex++;
        }

        Assert.Equal(bucketCountIndex, dataPoint.BucketCounts.Count);

        Assert.Equal(explicitBoundCountIndex, dataPoint.ExplicitBounds.Count);

        Assert.Equal((ulong)metricPoint.StartTime.ToUnixTimeNanoseconds(), dataPoint.StartTimeUnixNano);

        Assert.Equal((ulong)metricPoint.EndTime.ToUnixTimeNanoseconds(), dataPoint.TimeUnixNano);

        if (addPrepopulatedDimensions)
        {
            AssertOtlpAttributes(this.TagList.Concat(prepopulatedMetricDimensions), dataPoint.Attributes);
        }
        else
        {
            AssertOtlpAttributes(this.TagList, dataPoint.Attributes);
        }

        if (isExemplarsEnabled)
        {
            Assert.Single(dataPoint.Exemplars);

            var exemplar = dataPoint.Exemplars[0];

            metricPoint.TryGetExemplars(out var exemplars);

            var exemplarsEnumerator = exemplars.GetEnumerator();

            exemplarsEnumerator.MoveNext();

            var actualExemplar = exemplarsEnumerator.Current;

            Assert.Equal((ulong)actualExemplar.Timestamp.ToUnixTimeNanoseconds(), exemplar.TimeUnixNano);

            Assert.Equal(doubleValue, exemplar.AsDouble);

            if (isTracingEnabled)
            {
                var spanIdBytes = new byte[8];
                activity.SpanId.CopyTo(spanIdBytes);

                Assert.True(spanIdBytes.SequenceEqual(exemplar.SpanId));

                var traceIdBytes = new byte[16];
                activity.TraceId.CopyTo(traceIdBytes);

                Assert.True(traceIdBytes.SequenceEqual(exemplar.TraceId));
            }
            else
            {
                Assert.Equal(ByteString.Empty, exemplar.SpanId);
                Assert.Equal(ByteString.Empty, exemplar.TraceId);
            }

            AssertOtlpAttributes([new KeyValuePair<string, object>("zfilteredKey1", "zfilteredValue1")], exemplar.FilteredAttributes);
        }
        else
        {
            Assert.Empty(dataPoint.Exemplars);
        }
    }

    [Theory]
    [InlineData(new[] { -123.45, 23, .05, 100 })]
    public void HistogramSerializationMultipleMetricPoints(double[] doubleValues)
    {
        using var meter = new Meter(nameof(this.HistogramSerializationSingleMetricPoint), "0.0.1");

        var exportedItems = new List<Metric>();
        using var inMemoryReader = new BaseExportingMetricReader(new InMemoryExporter<Metric>(exportedItems))
        {
            TemporalityPreference = MetricReaderTemporalityPreference.Delta,
        };

        var resourceBuilder = ResourceBuilder.CreateDefault().Clear()
            .AddAttributes([new KeyValuePair<string, object>("TestResourceKey", "TestResourceValue")]);
        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .SetResourceBuilder(resourceBuilder)
            .AddMeter(nameof(this.HistogramSerializationSingleMetricPoint))
            .AddReader(inMemoryReader)
            .Build();

        var histogram = meter.CreateHistogram<double>("TestHistogram");

        var expectedMetricPointCount = doubleValues.Length;

        var tags = new TagList[expectedMetricPointCount];

        for (var i = 0; i < tags.Length; i++)
        {
            for (var j = 1; j <= (i + 1); j++)
            {
                tags[i].Add(new("tag" + j, "value" + j));
            }
        }

        for (var i = 0; i < expectedMetricPointCount; i++)
        {
            histogram.Record(doubleValues[i], tags[i]);
        }

        meterProvider.ForceFlush();

        var buffer = new byte[65360];

        var testTransport = new TestTransport();

        var otlpProtobufSerializer = new OtlpProtobufSerializer(
            testTransport,
            metricsAccount: null,
            metricsNamespace: null,
            prepopulatedMetricDimensions: null,
            prefixBufferWithUInt32LittleEndianLength: this.PrefixBufferWithUInt32LittleEndianLength);

        otlpProtobufSerializer.SerializeAndSendMetrics(buffer, meterProvider.GetResource(), new Batch<Metric>([.. exportedItems], exportedItems.Count));

        Assert.Equal(expectedMetricPointCount, testTransport.ExportedItems.Count);

        var metricPointsEnumerator = exportedItems[0].GetMetricPoints().GetEnumerator();

        for (var i = 0; i < expectedMetricPointCount; i++)
        {
            var request = this.AssertAndConvertExportedBlobToRequest(
                testTransport.ExportedItems[i]);

            Assert.NotNull(request.ResourceMetrics[0].Resource);

            AssertOtlpAttributes([new KeyValuePair<string, object>("TestResourceKey", "TestResourceValue")], request.ResourceMetrics[0].Resource.Attributes);

            Assert.Single(request.ResourceMetrics[0].ScopeMetrics);

            var scope = request.ResourceMetrics[0].ScopeMetrics[0];

            Assert.Equal(meter.Name, scope.Scope.Name);

            Assert.Single(request.ResourceMetrics[0].ScopeMetrics[0].Metrics);

            var metric = request.ResourceMetrics[0].ScopeMetrics[0].Metrics[0];

            Assert.Equal("TestHistogram", metric.Name);

            Assert.NotNull(metric.Histogram);

            Assert.Equal(1, (int)metric.Histogram.AggregationTemporality);

            Assert.Single(metric.Histogram.DataPoints);

            var dataPoint = metric.Histogram.DataPoints[0];

            Assert.Equal(doubleValues[i], dataPoint.Sum);

            Assert.Equal(doubleValues[i], dataPoint.Max);

            Assert.Equal(doubleValues[i], dataPoint.Min);

            metricPointsEnumerator.MoveNext();
            var metricPoint = metricPointsEnumerator.Current;

            var bucketCountIndex = 0;
            var explicitBoundCountIndex = 0;

            foreach (var histogramMeasurement in metricPoint.GetHistogramBuckets())
            {
                var bucketCount = (ulong)histogramMeasurement.BucketCount;

                Assert.Equal(bucketCount, dataPoint.BucketCounts[bucketCountIndex]);

                if (histogramMeasurement.ExplicitBound != double.PositiveInfinity)
                {
                    Assert.Equal(histogramMeasurement.ExplicitBound, dataPoint.ExplicitBounds[explicitBoundCountIndex]);
                    explicitBoundCountIndex++;
                }

                bucketCountIndex++;
            }

            Assert.Equal(bucketCountIndex, dataPoint.BucketCounts.Count);

            Assert.Equal(explicitBoundCountIndex, dataPoint.ExplicitBounds.Count);

            Assert.Equal((ulong)metricPoint.StartTime.ToUnixTimeNanoseconds(), dataPoint.StartTimeUnixNano);

            Assert.Equal((ulong)metricPoint.EndTime.ToUnixTimeNanoseconds(), dataPoint.TimeUnixNano);

            AssertOtlpAttributes(tags[i], dataPoint.Attributes);
        }
    }

    [Theory]
    [InlineData("longGuage", 123L, null, true, true)]
    [InlineData("longGuage", 123L, null, true, false)]
    [InlineData("longGuage", 123L, null, false, true)]
    [InlineData("longGuage", 123L, null, false, false)]
    [InlineData("doubleGuage", null, 123.45, true, true)]
    [InlineData("doubleGuage", null, 123.45, true, false)]
    [InlineData("doubleGuage", null, 123.45, false, true)]
    [InlineData("doubleGuage", null, 123.45, false, false)]
    [InlineData("longGuage", -123L, null, true, true)]
    [InlineData("longGuage", -123L, null, true, false)]
    [InlineData("longGuage", -123L, null, false, true)]
    [InlineData("longGuage", -123L, null, false, false)]
    [InlineData("doubleGuage", null, -123.45, true, true)]
    [InlineData("doubleGuage", null, -123.45, true, false)]
    [InlineData("doubleGuage", null, -123.45, false, true)]
    [InlineData("doubleGuage", null, -123.45, false, false)]
    public void GaugeSerializationSingleMetricPoint(string instrumentName, long? longValue, double? doubleValue, bool addPrepopulatedDimensions, bool addAccountAndNamespace)
    {
        using var meter = new Meter(nameof(this.GaugeSerializationSingleMetricPoint), "0.0.1");

        var exportedItems = new List<Metric>();
        using var inMemoryReader = new BaseExportingMetricReader(new InMemoryExporter<Metric>(exportedItems))
        {
            TemporalityPreference = MetricReaderTemporalityPreference.Delta,
        };

        var resourceAttributes = new Dictionary<string, object>
        {
            { "TestResourceKey", "TestResourceValue" },
            { GenevaMetricExporter.DimensionKeyForCustomMonitoringAccount, "ResourceAccount" },
            { GenevaMetricExporter.DimensionKeyForCustomMetricsNamespace, "ResourceNamespace" },
        };

        var expectedAccount = "TestAccount";
        var expectedNamespace = "TestNameSpace";
        var accountAndNamespace = new Dictionary<string, object>
        {
            { GenevaMetricExporter.DimensionKeyForCustomMonitoringAccount, expectedAccount },
            { GenevaMetricExporter.DimensionKeyForCustomMetricsNamespace, expectedNamespace },
        };

        var resourceBuilder = ResourceBuilder.CreateDefault().Clear()
            .AddAttributes(resourceAttributes);
        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .SetResourceBuilder(resourceBuilder)
            .AddMeter(nameof(this.GaugeSerializationSingleMetricPoint))
            .AddReader(inMemoryReader)
            .Build();

        if (longValue.HasValue)
        {
            meter.CreateObservableGauge(
                instrumentName,
                () => new List<Measurement<long>>()
                {
                    new(longValue.Value, this.TagList),
                });
        }
        else
        {
            meter.CreateObservableGauge(
                instrumentName,
                () => new List<Measurement<double>>()
                {
                new(doubleValue.Value, this.TagList),
                });
        }

        meterProvider.ForceFlush();

        var buffer = new byte[65360];

        var testTransport = new TestTransport();

        var otlpProtobufSerializer = new OtlpProtobufSerializer(
            testTransport,
            addAccountAndNamespace ? expectedAccount : null,
            addAccountAndNamespace ? expectedNamespace : null,
            addPrepopulatedDimensions ? prepopulatedMetricDimensions : null,
            prefixBufferWithUInt32LittleEndianLength: this.PrefixBufferWithUInt32LittleEndianLength);

        otlpProtobufSerializer.SerializeAndSendMetrics(buffer, meterProvider.GetResource(), new Batch<Metric>([.. exportedItems], exportedItems.Count));

        Assert.Single(testTransport.ExportedItems);

        var request = this.AssertAndConvertExportedBlobToRequest(
            testTransport.ExportedItems[0]);

        Assert.Single(request.ResourceMetrics);

        Assert.NotNull(request.ResourceMetrics[0].Resource);

        AssertOtlpAttributes(addAccountAndNamespace ? resourceAttributes.Concat(accountAndNamespace) : resourceAttributes, request.ResourceMetrics[0].Resource.Attributes);

        Assert.Single(request.ResourceMetrics[0].ScopeMetrics);

        var scope = request.ResourceMetrics[0].ScopeMetrics[0];

        Assert.Equal(meter.Name, scope.Scope.Name);

        Assert.Single(request.ResourceMetrics[0].ScopeMetrics[0].Metrics);

        var metric = request.ResourceMetrics[0].ScopeMetrics[0].Metrics[0];

        Assert.Equal(instrumentName, metric.Name);

        Assert.NotNull(metric.Gauge);

        Assert.Single(metric.Gauge.DataPoints);

        var dataPoint = metric.Gauge.DataPoints[0];

        if (longValue.HasValue)
        {
            Assert.Equal(longValue.Value, dataPoint.AsInt);
        }
        else
        {
            Assert.Equal(doubleValue.Value, dataPoint.AsDouble);
        }

        // Assert time
        var metricPointsEnumerator = exportedItems[0].GetMetricPoints().GetEnumerator();
        metricPointsEnumerator.MoveNext();
        var metricPoint = metricPointsEnumerator.Current;

        Assert.Equal((ulong)metricPoint.StartTime.ToUnixTimeNanoseconds(), dataPoint.StartTimeUnixNano);

        Assert.Equal((ulong)metricPoint.EndTime.ToUnixTimeNanoseconds(), dataPoint.TimeUnixNano);

        if (addPrepopulatedDimensions)
        {
            AssertOtlpAttributes(this.TagList.Concat(prepopulatedMetricDimensions), dataPoint.Attributes);
        }
        else
        {
            AssertOtlpAttributes(this.TagList, dataPoint.Attributes);
        }
    }

    [Theory]
    [InlineData("longGauge", new long[] { 10, 20, 30 }, null)]
    [InlineData("longGauge", new long[] { -10, 2, -30 }, null)]
    [InlineData("doubleGauge", null, new double[] { 10.20, 2, 30.65 })]
    [InlineData("doubleGauge", null, new double[] { -10.20, 2, -30.65 })]
    public void GaugeSerializationMultipleMetricPoints(string instrumentName, long[] longValues, double[] doubleValues)
    {
        using var meter = new Meter(nameof(this.GaugeSerializationMultipleMetricPoints), "0.0.1");
        var exportedItems = new List<Metric>();
        using var inMemoryReader = new BaseExportingMetricReader(new InMemoryExporter<Metric>(exportedItems))
        {
            TemporalityPreference = MetricReaderTemporalityPreference.Delta,
        };

        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddMeter(nameof(this.GaugeSerializationMultipleMetricPoints))
            .AddReader(inMemoryReader)
            .Build();

        var expectedMetricPoints = longValues != null ? longValues.Length : doubleValues.Length;
        var tags = new TagList[expectedMetricPoints];

        for (var i = 0; i < tags.Length; i++)
        {
            for (var j = 1; j <= (i + 1); j++)
            {
                tags[i].Add(new("tag" + j, "value" + j));
            }
        }

        if (longValues != null)
        {
            var counter = meter.CreateCounter<long>(instrumentName);

            meter.CreateObservableGauge(
                instrumentName,
                () =>
                {
                    List<Measurement<long>> list = [];
                    for (var i = 0; i < longValues.Length; i++)
                    {
                        list.Add(new(longValues[i], tags[i]));
                    }

                    return list;
                });
        }
        else
        {
            meter.CreateObservableGauge(
               instrumentName,
               () =>
               {
                   List<Measurement<double>> list = [];
                   for (var i = 0; i < doubleValues.Length; i++)
                   {
                       list.Add(new(doubleValues[i], tags[i]));
                   }

                   return list;
               });
        }

        meterProvider.ForceFlush();

        var buffer = new byte[65360];

        var testTransport = new TestTransport();

        var otlpProtobufSerializer = new OtlpProtobufSerializer(
            testTransport,
            metricsAccount: null,
            metricsNamespace: null,
            prepopulatedMetricDimensions: null,
            prefixBufferWithUInt32LittleEndianLength: this.PrefixBufferWithUInt32LittleEndianLength);

        otlpProtobufSerializer.SerializeAndSendMetrics(buffer, Resource.Empty, new Batch<Metric>([.. exportedItems], exportedItems.Count));

        Assert.Equal(expectedMetricPoints, testTransport.ExportedItems.Count);

        // For asserting time
        var metricPointsEnumerator = exportedItems[0].GetMetricPoints().GetEnumerator();

        for (var i = 0; i < expectedMetricPoints; i++)
        {
            var request = this.AssertAndConvertExportedBlobToRequest(
                testTransport.ExportedItems[i]);

            Assert.Single(request.ResourceMetrics);

            Assert.Single(request.ResourceMetrics[0].ScopeMetrics);

            var scope = request.ResourceMetrics[0].ScopeMetrics[0];

            Assert.Equal(meter.Name, scope.Scope.Name);

            Assert.Single(request.ResourceMetrics[0].ScopeMetrics[0].Metrics);

            var metric = request.ResourceMetrics[0].ScopeMetrics[0].Metrics[0];

            Assert.Equal(instrumentName, metric.Name);

            Assert.NotNull(metric.Gauge);

            Assert.Single(metric.Gauge.DataPoints);

            var dataPoint = metric.Gauge.DataPoints[0];

            if (longValues != null)
            {
                Assert.Equal(longValues[i], dataPoint.AsInt);
            }
            else
            {
                Assert.Equal(doubleValues[i], dataPoint.AsDouble);
            }

            metricPointsEnumerator.MoveNext();
            var metricPoint = metricPointsEnumerator.Current;

            Assert.Equal((ulong)metricPoint.StartTime.ToUnixTimeNanoseconds(), dataPoint.StartTimeUnixNano);

            Assert.Equal((ulong)metricPoint.EndTime.ToUnixTimeNanoseconds(), dataPoint.TimeUnixNano);

            AssertOtlpAttributes(tags[i], dataPoint.Attributes);
        }
    }

    [Theory]
    [InlineData(123.45, true, true, true, true)]
    [InlineData(123.45, true, true, true, false)]
    [InlineData(123.45, true, true, false, true)]
    [InlineData(123.45, true, true, false, false)]
    [InlineData(123.45, true, false, true, true)]
    [InlineData(123.45, true, false, true, false)]
    [InlineData(123.45, true, false, false, true)]
    [InlineData(123.45, true, false, false, false)]
    [InlineData(123.45, false, true, true, true)]
    [InlineData(123.45, false, true, true, false)]
    [InlineData(123.45, false, true, false, true)]
    [InlineData(123.45, false, true, false, false)]
    [InlineData(123.45, false, false, true, true)]
    [InlineData(123.45, false, false, true, false)]
    [InlineData(123.45, false, false, false, true)]
    [InlineData(123.45, false, false, false, false)]
    [InlineData(-123.45, true, true, true, true)]
    [InlineData(-123.45, true, true, true, false)]
    [InlineData(-123.45, true, true, false, true)]
    [InlineData(-123.45, true, true, false, false)]
    [InlineData(-123.45, true, false, true, true)]
    [InlineData(-123.45, true, false, true, false)]
    [InlineData(-123.45, true, false, false, true)]
    [InlineData(-123.45, true, false, false, false)]
    [InlineData(-123.45, false, true, true, true)]
    [InlineData(-123.45, false, true, true, false)]
    [InlineData(-123.45, false, true, false, true)]
    [InlineData(-123.45, false, true, false, false)]
    [InlineData(-123.45, false, false, true, true)]
    [InlineData(-123.45, false, false, true, false)]
    [InlineData(-123.45, false, false, false, true)]
    [InlineData(-123.45, false, false, false, false)]
    public void ExponentialHistogramSerializationSingleMetricPoint(double? doubleValue, bool addPrepopulatedDimensions, bool addAccountAndNamespace, bool isExemplarsEnabled, bool isTracingEnabled)
    {
        Activity activity = null;

        if (isTracingEnabled)
        {
            activity = new Activity("Custom Activity");
            activity.Start();
        }

        using var meter = new Meter(nameof(this.ExponentialHistogramSerializationSingleMetricPoint), "0.0.1");

        var exportedItems = new List<Metric>();
        using var inMemoryReader = new BaseExportingMetricReader(new InMemoryExporter<Metric>(exportedItems))
        {
            TemporalityPreference = MetricReaderTemporalityPreference.Delta,
        };

        var resourceAttributes = new Dictionary<string, object>
        {
            { "TestResourceKey", "TestResourceValue" },
            { GenevaMetricExporter.DimensionKeyForCustomMonitoringAccount, "ResourceAccount" },
            { GenevaMetricExporter.DimensionKeyForCustomMetricsNamespace, "ResourceNamespace" },
        };

        var expectedAccount = "TestAccount";
        var expectedNamespace = "TestNameSpace";
        var accountAndNamespace = new Dictionary<string, object>
        {
            { GenevaMetricExporter.DimensionKeyForCustomMonitoringAccount, expectedAccount },
            { GenevaMetricExporter.DimensionKeyForCustomMetricsNamespace, expectedNamespace },
        };

        var resourceBuilder = ResourceBuilder.CreateDefault().Clear()
            .AddAttributes(resourceAttributes);
        var meterProviderBuilder = Sdk.CreateMeterProviderBuilder()
            .SetResourceBuilder(resourceBuilder)
            .AddMeter(nameof(this.ExponentialHistogramSerializationSingleMetricPoint))
            .AddReader(inMemoryReader);

        if (isExemplarsEnabled)
        {
            meterProviderBuilder.SetExemplarFilter(ExemplarFilterType.AlwaysOn);
        }

        meterProviderBuilder.AddView(instrument =>
        {
            return new Base2ExponentialBucketHistogramConfiguration() { TagKeys = TagKeys };
        });

        var meterProvider = meterProviderBuilder.Build();

        var instrumentName = "doubleExponentialHistogram";
        var histogram = meter.CreateHistogram<double>(instrumentName);

        if (isExemplarsEnabled)
        {
            histogram.Record(doubleValue.Value, this.exemplarTagList);
            histogram.Record(0, this.exemplarTagList);
        }
        else
        {
            histogram.Record(doubleValue.Value, this.TagList);
            histogram.Record(0, this.TagList);
        }

        meterProvider.ForceFlush();

        var buffer = new byte[65360];

        var testTransport = new TestTransport();

        var otlpProtobufSerializer = new OtlpProtobufSerializer(
            testTransport,
            addAccountAndNamespace ? expectedAccount : null,
            addAccountAndNamespace ? expectedNamespace : null,
            addPrepopulatedDimensions ? prepopulatedMetricDimensions : null,
            prefixBufferWithUInt32LittleEndianLength: this.PrefixBufferWithUInt32LittleEndianLength);

        otlpProtobufSerializer.SerializeAndSendMetrics(buffer, meterProvider.GetResource(), new Batch<Metric>([.. exportedItems], exportedItems.Count));

        Assert.Single(testTransport.ExportedItems);

        var request = this.AssertAndConvertExportedBlobToRequest(
            testTransport.ExportedItems[0]);

        Assert.Single(request.ResourceMetrics);

        Assert.NotNull(request.ResourceMetrics[0].Resource);

        AssertOtlpAttributes(addAccountAndNamespace ? resourceAttributes.Concat(accountAndNamespace) : resourceAttributes, request.ResourceMetrics[0].Resource.Attributes);

        Assert.Single(request.ResourceMetrics[0].ScopeMetrics);

        var scope = request.ResourceMetrics[0].ScopeMetrics[0];

        Assert.Equal(meter.Name, scope.Scope.Name);

        Assert.Single(request.ResourceMetrics[0].ScopeMetrics[0].Metrics);

        var metric = request.ResourceMetrics[0].ScopeMetrics[0].Metrics[0];

        Assert.Equal(instrumentName, metric.Name);

        Assert.NotNull(metric.ExponentialHistogram);

        Assert.Single(metric.ExponentialHistogram.DataPoints);

        var dataPoint = metric.ExponentialHistogram.DataPoints[0];

        // Assert time
        var metricPointsEnumerator = exportedItems[0].GetMetricPoints().GetEnumerator();
        metricPointsEnumerator.MoveNext();
        var metricPoint = metricPointsEnumerator.Current;

        Assert.Equal((ulong)metricPoint.StartTime.ToUnixTimeNanoseconds(), dataPoint.StartTimeUnixNano);
        Assert.Equal((ulong)metricPoint.EndTime.ToUnixTimeNanoseconds(), dataPoint.TimeUnixNano);

        Assert.Equal(20, dataPoint.Scale);
        Assert.Equal(1UL, dataPoint.ZeroCount);
        if (doubleValue > 0)
        {
            Assert.Equal(2UL, dataPoint.Count);
        }
        else
        {
            Assert.Equal(1UL, dataPoint.Count);
        }

        if (doubleValue > 0)
        {
            Assert.Equal(doubleValue, dataPoint.Max);
            Assert.Equal(0, dataPoint.Min);
            Assert.Equal(doubleValue, dataPoint.Sum);
            Assert.Null(dataPoint.Negative);
            Assert.True(dataPoint.Positive.Offset > 0);
            Assert.Single(dataPoint.Positive.BucketCounts);
            Assert.Equal(1UL, dataPoint.Positive.BucketCounts[0]);
        }
        else
        {
            Assert.Equal(0, dataPoint.Min);
            Assert.Equal(0, dataPoint.Max);
            Assert.Equal(0, dataPoint.Sum);
            Assert.Null(dataPoint.Negative);
            Assert.Equal(0, dataPoint.Positive.Offset);
            Assert.Empty(dataPoint.Positive.BucketCounts);
        }

        if (addPrepopulatedDimensions)
        {
            AssertOtlpAttributes(this.TagList.Concat(prepopulatedMetricDimensions), dataPoint.Attributes);
        }
        else
        {
            AssertOtlpAttributes(this.TagList, dataPoint.Attributes);
        }

        if (isExemplarsEnabled)
        {
            // Exemplars are only emitted for positive/0 values.
            if (doubleValue.Value > 0)
            {
                Assert.Equal(2, dataPoint.Exemplars.Count);
            }
            else
            {
                Assert.Single(dataPoint.Exemplars);
            }

            metricPoint.TryGetExemplars(out var exemplars);

            var exemplarsEnumerator = exemplars.GetEnumerator();

            double[] exemplarValues = doubleValue.Value > 0 ? [doubleValue.Value, 0] : [0];

            var exemplarValuesIndex = 0;

            foreach (var exemplar in dataPoint.Exemplars)
            {
                exemplarsEnumerator.MoveNext();

                var actualExemplar = exemplarsEnumerator.Current;

                Assert.Equal((ulong)actualExemplar.Timestamp.ToUnixTimeNanoseconds(), exemplar.TimeUnixNano);

                Assert.Equal(exemplarValues[exemplarValuesIndex], exemplar.AsDouble);

                if (isTracingEnabled)
                {
                    var spanIdBytes = new byte[8];
                    activity.SpanId.CopyTo(spanIdBytes);

                    Assert.True(spanIdBytes.SequenceEqual(exemplar.SpanId));

                    var traceIdBytes = new byte[16];
                    activity.TraceId.CopyTo(traceIdBytes);

                    Assert.True(traceIdBytes.SequenceEqual(exemplar.TraceId));
                }
                else
                {
                    Assert.Equal(ByteString.Empty, exemplar.SpanId);
                    Assert.Equal(ByteString.Empty, exemplar.TraceId);
                }

                AssertOtlpAttributes([new KeyValuePair<string, object>("zfilteredKey1", "zfilteredValue1")], exemplar.FilteredAttributes);

                exemplarValuesIndex++;
            }
        }
        else
        {
            Assert.Empty(dataPoint.Exemplars);
        }
    }

    internal static void AssertOtlpAttributes(
        IEnumerable<KeyValuePair<string, object>> expected,
        RepeatedField<OtlpCommon.KeyValue> actual)
    {
        var expectedAttributes = expected.ToList();
        var expectedAttributesCount = expectedAttributes.Count;
        for (var i = 0; i < expectedAttributesCount; i++)
        {
            var current = expectedAttributes[i].Value;

            Assert.Equal(expectedAttributes[i].Key, actual[i].Key);
            AssertOtlpAttributeValue(current, actual[i].Value);
        }

        Assert.Equal(expectedAttributesCount, actual.Count);
    }

    private static void AssertOtlpAttributeValue(object expected, OtlpCommon.AnyValue actual)
    {
        switch (expected)
        {
            case string s:
                Assert.Equal(s, actual.StringValue);
                break;
            case bool b:
                Assert.Equal(b, actual.BoolValue);
                break;
            case long l:
                Assert.Equal(l, actual.IntValue);
                break;
            case double d:
                Assert.Equal(d, actual.DoubleValue);
                break;
            case int i:
                Assert.Equal(i, actual.IntValue);
                break;
            case uint u:
                Assert.Equal(u, actual.IntValue);
                break;
            case ushort us:
                Assert.Equal(us, actual.IntValue);
                break;
            case short s:
                Assert.Equal(s, actual.IntValue);
                break;
            case ulong ul:
                Assert.Equal(ul, (ulong)actual.IntValue);
                break;
            case sbyte sb:
                Assert.Equal(sb, actual.IntValue);
                break;
            default:
                Assert.Equal(expected.ToString(), actual.StringValue);
                break;
        }
    }

    private OtlpCollector.ExportMetricsServiceRequest AssertAndConvertExportedBlobToRequest(
        byte[] blob)
    {
        var request = new OtlpCollector.ExportMetricsServiceRequest();

        if (this.PrefixBufferWithUInt32LittleEndianLength)
        {
            var content = blob.AsSpan().Slice(4);
            Assert.Equal((uint)content.Length, BitConverter.ToUInt32(blob, 0));
            request.MergeFrom(content);
        }
        else
        {
            request.MergeFrom(blob);
        }

        return request;
    }

    private class TestTransport : IMetricDataTransport
    {
        public readonly List<byte[]> ExportedItems = [];

        public void SendOtlpProtobufEvent(byte[] body, int size)
        {
            var arr = new byte[size];
            Buffer.BlockCopy(body, 0, arr, 0, arr.Length);
            this.ExportedItems.Add(arr);
        }

        public void Send(MetricEventType eventType, byte[] body, int size)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }
    }
}
