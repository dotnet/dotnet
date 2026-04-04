// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Diagnostics.Metrics;
using BenchmarkDotNet.Attributes;
using OpenTelemetry.Exporter.Geneva.Metrics;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

/*
// * Summary *

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3296/23H2/2023Update/SunValley3)
13th Gen Intel Core i9-13900H, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.202
  [Host]     : .NET 8.0.3 (8.0.324.11423), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.3 (8.0.324.11423), X64 RyuJIT AVX2


| Method                                                   | Mean      | Error    | StdDev   | Gen0   | Allocated |
|--------------------------------------------------------- |----------:|---------:|---------:|-------:|----------:|
| InstrumentWithNoListener3Dimensions                      |  13.22 ns | 0.035 ns | 0.033 ns |      - |         - |
| InstrumentWithNoListener4Dimensions                      |  31.12 ns | 0.386 ns | 0.361 ns |      - |         - |
| InstrumentWithWithListener3Dimensions                    |  13.69 ns | 0.036 ns | 0.034 ns |      - |         - |
| InstrumentWithWithListener4Dimensions                    |  30.90 ns | 0.276 ns | 0.258 ns |      - |         - |
| InstrumentWithWithDummyReader3Dimensions                 |  66.19 ns | 0.473 ns | 0.442 ns |      - |         - |
| InstrumentWithWithDummyReader4Dimensions                 |  94.12 ns | 1.274 ns | 1.191 ns |      - |         - |
| InstrumentWithWithGenevaCounterMetricExporter3Dimensions |  68.57 ns | 0.489 ns | 0.457 ns |      - |         - |
| InstrumentWithWithGenevaCounterMetricExporter4Dimensions |  94.61 ns | 0.672 ns | 0.595 ns |      - |         - |
| SerializeCounterMetricItemWith3Dimensions                |  84.15 ns | 0.660 ns | 0.585 ns |      - |         - |
| SerializeCounterMetricItemWith4Dimensions                |  94.97 ns | 1.129 ns | 1.056 ns |      - |         - |
| ExportCounterMetricItemWith3Dimensions                   | 236.62 ns | 2.071 ns | 1.836 ns |      - |         - |
| ExportCounterMetricItemWith4Dimensions                   | 254.66 ns | 2.884 ns | 2.697 ns |      - |         - |
| SerializeHistogramMetricItemWith3Dimensions              | 142.76 ns | 2.709 ns | 2.534 ns |      - |         - |
| SerializeHistogramMetricItemWith4Dimensions              | 156.16 ns | 2.884 ns | 2.698 ns |      - |         - |
| ExportHistogramMetricItemWith3Dimensions                 | 329.75 ns | 3.602 ns | 3.369 ns |      - |         - |
| ExportHistogramMetricItemWith4Dimensions                 | 325.98 ns | 6.001 ns | 5.614 ns |      - |         - |
| SerializeCounterMetricItemWith3Dimensions_Otlp           | 227.27 ns | 3.010 ns | 2.815 ns | 0.0038 |      48 B |
| SerializeCounterMetricItemWith4Dimensions_Otlp           | 260.18 ns | 5.151 ns | 6.131 ns | 0.0038 |      48 B |
| ExportCounterMetricItemWith3Dimensions_Otlp              | 407.64 ns | 4.364 ns | 3.868 ns | 0.0038 |      48 B |
| ExportCounterMetricItemWith4Dimensions_Otlp              | 435.45 ns | 8.376 ns | 8.226 ns | 0.0038 |      48 B |
| SerializeHistogramMetricItemWith3Dimensions_Otlp         | 261.77 ns | 5.108 ns | 5.883 ns | 0.0038 |      48 B |
| SerializeHistogramMetricItemWith4Dimensions_Otlp         | 285.66 ns | 5.285 ns | 5.191 ns | 0.0038 |      48 B |
| ExportHistogramMetricItemWith3Dimensions_Otlp            | 489.10 ns | 9.641 ns | 9.901 ns | 0.0038 |      48 B |
| ExportHistogramMetricItemWith4Dimensions_Otlp            | 513.72 ns | 7.481 ns | 6.998 ns | 0.0038 |      48 B |
*/

namespace OpenTelemetry.Exporter.Geneva.Benchmarks;

[MemoryDiagnoser]
public class MetricExporterBenchmarks
{
    private readonly Meter meterWithNoListener = new("MeterWithNoListener", "0.0.1");
    private readonly Meter meterWithListener = new("MeterWithListener", "0.0.1");
    private readonly Meter meterWithDummyReader = new("MeterWithDummyReader", "0.0.1");
    private readonly Meter meterWithGenevaMetricExporter = new("MeterWithGenevaMetricExporter", "0.0.1");
    private readonly ThreadLocal<Random> random = new(() => new Random());
    private Metric? counterMetricWith3Dimensions;
    private Metric? counterMetricWith4Dimensions;
    private MetricPoint counterMetricPointWith3Dimensions;
    private MetricPoint counterMetricPointWith4Dimensions;
    private MetricData counterMetricDataWith3Dimensions;
    private MetricData counterMetricDataWith4Dimensions;
    private Batch<Metric> counterMetricBatchWith3Dimensions;
    private Batch<Metric> counterMetricBatchWith4Dimensions;
    private Metric? histogramMetricWith3Dimensions;
    private Metric? histogramMetricWith4Dimensions;
    private MetricPoint histogramMetricPointWith3Dimensions;
    private MetricPoint histogramMetricPointWith4Dimensions;
    private ulong histogramSumWith3Dimensions;
    private ulong histogramSumWith4Dimensions;
    private double histogramMinWith3Dimensions;
    private double histogramMinWith4Dimensions;
    private double histogramMaxWith3Dimensions;
    private double histogramMaxWith4Dimensions;
    private uint histogramCountWith3Dimensions;
    private uint histogramCountWith4Dimensions;
    private Batch<Metric> histogramMetricBatchWith3Dimensions;
    private Batch<Metric> histogramMetricBatchWith4Dimensions;
    private Counter<long>? counterWithNoListener;
    private Counter<long>? counterWithListener;
    private Counter<long>? counterWithDummyReader;
    private Counter<long>? counterWithGenevaMetricExporter;
    private MeterListener? listener;
    private MeterProvider? meterProviderWithDummyReader;
    private MeterProvider? meterProviderWithGenevaMetricExporter;
    private MeterProvider? meterProviderForCounterBatchWith3Dimensions;
    private MeterProvider? meterProviderForCounterBatchWith4Dimensions;
    private MeterProvider? meterProviderForHistogramBatchWith3Dimensions;
    private MeterProvider? meterProviderForHistogramBatchWith4Dimensions;
    private TlvMetricExporter? tlvMetricsExporter;
    private OtlpProtobufMetricExporter? otlpProtobufMetricExporter;
    private OtlpProtobufSerializer? otlpProtobufSerializer;
    private Resource? resource;
    private byte[]? buffer;

    private static readonly Random randomForHistogram = new(); // Use the same seed for all the benchmarks to have the same data exported
    private static readonly string[] dimensionValues = ["DimVal1", "DimVal2", "DimVal3", "DimVal4", "DimVal5", "DimVal6", "DimVal7", "DimVal8", "DimVal9", "DimVal10"
    ];

    [GlobalSetup]
    public void Setup()
    {
        this.counterWithNoListener = this.meterWithNoListener.CreateCounter<long>("counter");
        this.counterWithListener = this.meterWithListener.CreateCounter<long>("counter");
        this.counterWithDummyReader = this.meterWithDummyReader.CreateCounter<long>("counter");
        this.counterWithGenevaMetricExporter = this.meterWithGenevaMetricExporter.CreateCounter<long>("counter");

        var exporterOptions = new GenevaMetricExporterOptions() { ConnectionString = "Account=OTelMonitoringAccount;Namespace=OTelMetricNamespace" };
        var connectionStringBuilder = new ConnectionStringBuilder(exporterOptions.ConnectionString);
        this.tlvMetricsExporter = new TlvMetricExporter(connectionStringBuilder, exporterOptions.PrepopulatedMetricDimensions);

        // Using test transport here with noop to benchmark just the serialization part.
        this.otlpProtobufSerializer = new OtlpProtobufSerializer(new TestTransport(), metricsAccount: null, metricsNamespace: null, prepopulatedMetricDimensions: null);

        var resourceBuilder = ResourceBuilder.CreateDefault().Clear()
           .AddAttributes([new KeyValuePair<string, object>("TestResourceKey", "TestResourceValue")]);
        this.resource = resourceBuilder.Build();
        this.otlpProtobufMetricExporter = new OtlpProtobufMetricExporter(() => { return this.resource; }, new TestTransport(), metricsAccount: null, metricsNamespace: null, prepopulatedMetricDimensions: null);
        this.buffer = new byte[GenevaMetricExporter.BufferSize];

        this.counterMetricPointWith3Dimensions = this.GenerateCounterMetricItemWith3Dimensions(out this.counterMetricDataWith3Dimensions);
        this.counterMetricPointWith4Dimensions = this.GenerateCounterMetricItemWith4Dimensions(out this.counterMetricDataWith4Dimensions);

        this.counterMetricBatchWith3Dimensions = this.GenerateCounterBatchWith3Dimensions();
        this.counterMetricBatchWith4Dimensions = this.GenerateCounterBatchWith4Dimensions();

        using var enumeratorForCounterBatchWith3Dimensions = this.counterMetricBatchWith3Dimensions.GetEnumerator();
        enumeratorForCounterBatchWith3Dimensions.MoveNext();
        this.counterMetricWith3Dimensions = enumeratorForCounterBatchWith3Dimensions.Current;

        using var enumeratorForCounterBatchWith4Dimensions = this.counterMetricBatchWith4Dimensions.GetEnumerator();
        enumeratorForCounterBatchWith4Dimensions.MoveNext();
        this.counterMetricWith4Dimensions = enumeratorForCounterBatchWith4Dimensions.Current;

        this.histogramMetricPointWith3Dimensions = this.GenerateHistogramMetricItemWith3Dimensions(out this.histogramSumWith3Dimensions, out this.histogramCountWith3Dimensions, out this.histogramMinWith3Dimensions, out this.histogramMaxWith3Dimensions);
        this.histogramMetricPointWith4Dimensions = this.GenerateHistogramMetricItemWith4Dimensions(out this.histogramSumWith4Dimensions, out this.histogramCountWith4Dimensions, out this.histogramMinWith4Dimensions, out this.histogramMaxWith4Dimensions);

        this.histogramMetricBatchWith3Dimensions = this.GenerateHistogramBatchWith3Dimensions();
        this.histogramMetricBatchWith4Dimensions = this.GenerateHistogramBatchWith4Dimensions();

        using var enumeratorForHistogramBatchWith3Dimensions = this.histogramMetricBatchWith3Dimensions.GetEnumerator();
        enumeratorForHistogramBatchWith3Dimensions.MoveNext();
        this.histogramMetricWith3Dimensions = enumeratorForHistogramBatchWith3Dimensions.Current;

        using var enumeratorForHistogramBatchWith4Dimensions = this.histogramMetricBatchWith4Dimensions.GetEnumerator();
        enumeratorForHistogramBatchWith4Dimensions.MoveNext();
        this.histogramMetricWith4Dimensions = enumeratorForHistogramBatchWith4Dimensions.Current;

        #region Setup MeterListener
        this.listener = new MeterListener
        {
            InstrumentPublished = (instrument, listener) =>
            {
                if (instrument.Meter.Name == this.meterWithListener.Name)
                {
                    listener.EnableMeasurementEvents(instrument);
                }
            },
        };

        this.listener.Start();
        #endregion

        this.meterProviderWithDummyReader = Sdk.CreateMeterProviderBuilder()
            .AddMeter(this.meterWithDummyReader.Name)
            .AddReader(new DummyReader(new DummyMetricExporter()))
            .Build();

        this.meterProviderWithGenevaMetricExporter = Sdk.CreateMeterProviderBuilder()
            .AddMeter(this.meterWithGenevaMetricExporter.Name)
            .AddGenevaMetricExporter(options =>
            {
                options.ConnectionString = "Account=OTelMonitoringAccount;Namespace=OTelMetricNamespace";
            })
            .Build();
    }

    private MetricPoint GenerateCounterMetricItemWith3Dimensions(out MetricData metricData)
    {
        using var meterWithInMemoryExporter = new Meter("GenerateCounterMetricItemWith3Dimensions", "0.0.1");
        var counter = meterWithInMemoryExporter.CreateCounter<long>("CounterWithThreeDimensions");

        var exportedItems = new List<Metric>();
        using var inMemoryReader = new BaseExportingMetricReader(new InMemoryExporter<Metric>(exportedItems))
        {
            TemporalityPreference = MetricReaderTemporalityPreference.Delta,
        };

        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddMeter("GenerateCounterMetricItemWith3Dimensions")
            .AddReader(inMemoryReader)
            .Build();

        counter.Add(
            100,
            new("DimName1", dimensionValues[this.random.Value!.Next(0, 10)]),
            new("DimName2", dimensionValues[this.random.Value.Next(0, 10)]),
            new("DimName3", dimensionValues[this.random.Value.Next(0, 10)]));

        inMemoryReader.Collect();

        var metric = exportedItems[0];
        var metricPointsEnumerator = metric.GetMetricPoints().GetEnumerator();
        metricPointsEnumerator.MoveNext();
        var metricPoint = metricPointsEnumerator.Current;
        var metricDataValue = Convert.ToUInt64(metricPoint.GetSumLong());
        metricData = new MetricData { UInt64Value = metricDataValue };

        return metricPoint;
    }

    private MetricPoint GenerateCounterMetricItemWith4Dimensions(out MetricData metricData)
    {
        using var meterWithInMemoryExporter = new Meter("GenerateCounterMetricItemWith4Dimensions", "0.0.1");
        var counter = meterWithInMemoryExporter.CreateCounter<long>("CounterWith4Dimensions");

        var exportedItems = new List<Metric>();
        using var inMemoryReader = new BaseExportingMetricReader(new InMemoryExporter<Metric>(exportedItems))
        {
            TemporalityPreference = MetricReaderTemporalityPreference.Delta,
        };

        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddMeter("GenerateCounterMetricItemWith4Dimensions")
            .AddReader(inMemoryReader)
            .Build();

        var tags = new TagList
        {
            { "DimName1", dimensionValues[this.random.Value!.Next(0, 2)] },
            { "DimName2", dimensionValues[this.random.Value.Next(0, 5)] },
            { "DimName3", dimensionValues[this.random.Value.Next(0, 10)] },
            { "DimName4", dimensionValues[this.random.Value.Next(0, 10)] },
        };

        counter.Add(100, tags);

        inMemoryReader.Collect();

        var metric = exportedItems[0];
        var metricPointsEnumerator = metric.GetMetricPoints().GetEnumerator();
        metricPointsEnumerator.MoveNext();
        var metricPoint = metricPointsEnumerator.Current;
        var metricDataValue = Convert.ToUInt64(metricPoint.GetSumLong());
        metricData = new MetricData { UInt64Value = metricDataValue };

        return metricPoint;
    }

    private Batch<Metric> GenerateCounterBatchWith3Dimensions()
    {
        using var meterWithInMemoryExporter = new Meter("GenerateCounterBatchWith3Dimensions", "0.0.1");
        var counter = meterWithInMemoryExporter.CreateCounter<long>("CounterWithThreeDimensions");

        var batchGeneratorExporter = new BatchGenerator();
        var batchGeneratorReader = new BaseExportingMetricReader(batchGeneratorExporter)
        {
            TemporalityPreference = MetricReaderTemporalityPreference.Delta,
        };

        this.meterProviderForCounterBatchWith3Dimensions = Sdk.CreateMeterProviderBuilder()
            .AddMeter("GenerateCounterBatchWith3Dimensions")
            .AddReader(batchGeneratorReader)
            .Build();

        counter.Add(
            100,
            new("DimName1", dimensionValues[this.random.Value!.Next(0, 10)]),
            new("DimName2", dimensionValues[this.random.Value.Next(0, 10)]),
            new("DimName3", dimensionValues[this.random.Value.Next(0, 10)]));

        this.meterProviderForCounterBatchWith3Dimensions.ForceFlush();
        return batchGeneratorExporter.Batch;
    }

    private Batch<Metric> GenerateCounterBatchWith4Dimensions()
    {
        using var meterWithInMemoryExporter = new Meter("GenerateCounterBatchWith4Dimensions", "0.0.1");
        var counter = meterWithInMemoryExporter.CreateCounter<long>("CounterWith4Dimensions");

        var batchGeneratorExporter = new BatchGenerator();
        var batchGeneratorReader = new BaseExportingMetricReader(batchGeneratorExporter)
        {
            TemporalityPreference = MetricReaderTemporalityPreference.Delta,
        };

        this.meterProviderForCounterBatchWith4Dimensions = Sdk.CreateMeterProviderBuilder()
            .AddMeter("GenerateCounterBatchWith4Dimensions")
            .AddReader(batchGeneratorReader)
            .Build();

        var tags = new TagList
        {
            { "DimName1", dimensionValues[this.random.Value!.Next(0, 2)] },
            { "DimName2", dimensionValues[this.random.Value.Next(0, 5)] },
            { "DimName3", dimensionValues[this.random.Value.Next(0, 10)] },
            { "DimName4", dimensionValues[this.random.Value.Next(0, 10)] },
        };

        counter.Add(100, tags);

        this.meterProviderForCounterBatchWith4Dimensions.ForceFlush();
        return batchGeneratorExporter.Batch;
    }

    private MetricPoint GenerateHistogramMetricItemWith3Dimensions(out ulong sum, out uint count, out double min, out double max)
    {
        using var meterWithInMemoryExporter = new Meter("GenerateHistogramMetricItemWith3Dimensions", "0.0.1");
        var histogram = meterWithInMemoryExporter.CreateHistogram<long>("HistogramWith3Dimensions");

        var exportedItems = new List<Metric>();
        using var inMemoryReader = new BaseExportingMetricReader(new InMemoryExporter<Metric>(exportedItems))
        {
            TemporalityPreference = MetricReaderTemporalityPreference.Delta,
        };

        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddMeter("GenerateHistogramMetricItemWith3Dimensions")
            .AddReader(inMemoryReader)
            .Build();

        var tag1 = new KeyValuePair<string, object?>("DimName1", dimensionValues[this.random.Value!.Next(0, 10)]);
        var tag2 = new KeyValuePair<string, object?>("DimName2", dimensionValues[this.random.Value.Next(0, 10)]);
        var tag3 = new KeyValuePair<string, object?>("DimName3", dimensionValues[this.random.Value.Next(0, 10)]);

        for (var i = 0; i < 1000; i++)
        {
            histogram.Record(randomForHistogram.Next(1, 1000), tag1, tag2, tag3);
        }

        inMemoryReader.Collect();

        var metric = exportedItems[0];
        var metricPointsEnumerator = metric.GetMetricPoints().GetEnumerator();
        metricPointsEnumerator.MoveNext();
        var metricPoint = metricPointsEnumerator.Current;
        sum = Convert.ToUInt64(metricPoint.GetHistogramSum());
        count = Convert.ToUInt32(metricPoint.GetHistogramCount());

        if (!metricPoint.TryGetHistogramMinMaxValues(out min, out max))
        {
            min = 0;
            max = 0;
        }

        return metricPoint;
    }

    private MetricPoint GenerateHistogramMetricItemWith4Dimensions(out ulong sum, out uint count, out double min, out double max)
    {
        using var meterWithInMemoryExporter = new Meter("GenerateHistogramMetricItemWith4Dimensions", "0.0.1");
        var histogram = meterWithInMemoryExporter.CreateHistogram<long>("HistogramWith4Dimensions");

        var exportedItems = new List<Metric>();
        using var inMemoryReader = new BaseExportingMetricReader(new InMemoryExporter<Metric>(exportedItems))
        {
            TemporalityPreference = MetricReaderTemporalityPreference.Delta,
        };

        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddMeter("GenerateHistogramMetricItemWith4Dimensions")
            .AddReader(inMemoryReader)
            .Build();

        var tags = new TagList
        {
            { "DimName1", dimensionValues[this.random.Value!.Next(0, 2)] },
            { "DimName2", dimensionValues[this.random.Value.Next(0, 5)] },
            { "DimName3", dimensionValues[this.random.Value.Next(0, 10)] },
            { "DimName4", dimensionValues[this.random.Value.Next(0, 10)] },
        };

        for (var i = 0; i < 1000; i++)
        {
            histogram.Record(randomForHistogram.Next(1, 1000), tags);
        }

        inMemoryReader.Collect();

        var metric = exportedItems[0];
        var metricPointsEnumerator = metric.GetMetricPoints().GetEnumerator();
        metricPointsEnumerator.MoveNext();
        var metricPoint = metricPointsEnumerator.Current;
        sum = Convert.ToUInt64(metricPoint.GetHistogramSum());
        count = Convert.ToUInt32(metricPoint.GetHistogramCount());

        if (!metricPoint.TryGetHistogramMinMaxValues(out min, out max))
        {
            min = 0;
            max = 0;
        }

        return metricPoint;
    }

    private Batch<Metric> GenerateHistogramBatchWith3Dimensions()
    {
        using var meterWithInMemoryExporter = new Meter("GenerateHistogramBatchWith3Dimensions", "0.0.1");
        var histogram = meterWithInMemoryExporter.CreateHistogram<long>("HistogramWith3Dimensions");

        var batchGeneratorExporter = new BatchGenerator();
        var batchGeneratorReader = new BaseExportingMetricReader(batchGeneratorExporter)
        {
            TemporalityPreference = MetricReaderTemporalityPreference.Delta,
        };

        this.meterProviderForHistogramBatchWith3Dimensions = Sdk.CreateMeterProviderBuilder()
            .AddMeter("GenerateHistogramBatchWith3Dimensions")
            .AddReader(batchGeneratorReader)
            .Build();

        var tag1 = new KeyValuePair<string, object?>("DimName1", dimensionValues[this.random.Value!.Next(0, 10)]);
        var tag2 = new KeyValuePair<string, object?>("DimName2", dimensionValues[this.random.Value.Next(0, 10)]);
        var tag3 = new KeyValuePair<string, object?>("DimName3", dimensionValues[this.random.Value.Next(0, 10)]);

        for (var i = 0; i < 1000; i++)
        {
            histogram.Record(randomForHistogram.Next(1, 1000), tag1, tag2, tag3);
        }

        this.meterProviderForHistogramBatchWith3Dimensions.ForceFlush();
        return batchGeneratorExporter.Batch;
    }

    private Batch<Metric> GenerateHistogramBatchWith4Dimensions()
    {
        using var meterWithInMemoryExporter = new Meter("GenerateHistogramBatchWith4Dimensions", "0.0.1");
        var histogram = meterWithInMemoryExporter.CreateHistogram<long>("HistogramWith4Dimensions");

        var batchGeneratorExporter = new BatchGenerator();
        var batchGeneratorReader = new BaseExportingMetricReader(batchGeneratorExporter)
        {
            TemporalityPreference = MetricReaderTemporalityPreference.Delta,
        };

        this.meterProviderForHistogramBatchWith4Dimensions = Sdk.CreateMeterProviderBuilder()
            .AddMeter("GenerateHistogramBatchWith4Dimensions")
            .AddReader(batchGeneratorReader)
            .Build();

        var tags = new TagList
        {
            { "DimName1", dimensionValues[this.random.Value!.Next(0, 2)] },
            { "DimName2", dimensionValues[this.random.Value.Next(0, 5)] },
            { "DimName3", dimensionValues[this.random.Value.Next(0, 10)] },
            { "DimName4", dimensionValues[this.random.Value.Next(0, 10)] },
        };

        for (var i = 0; i < 1000; i++)
        {
            histogram.Record(randomForHistogram.Next(1, 1000), tags);
        }

        this.meterProviderForHistogramBatchWith4Dimensions.ForceFlush();
        return batchGeneratorExporter.Batch;
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        this.meterWithNoListener?.Dispose();
        this.meterWithListener?.Dispose();
        this.meterWithDummyReader?.Dispose();
        this.meterWithGenevaMetricExporter?.Dispose();
        this.listener?.Dispose();
        this.meterProviderWithDummyReader?.Dispose();
        this.meterProviderWithGenevaMetricExporter?.Dispose();
        this.meterProviderForCounterBatchWith3Dimensions?.Dispose();
        this.meterProviderForCounterBatchWith4Dimensions?.Dispose();
        this.meterProviderForHistogramBatchWith3Dimensions?.Dispose();
        this.meterProviderForHistogramBatchWith4Dimensions?.Dispose();
        this.tlvMetricsExporter?.Dispose();
        this.otlpProtobufMetricExporter?.Dispose();
    }

    [Benchmark]
    public void InstrumentWithNoListener3Dimensions()
    {
        var tag1 = new KeyValuePair<string, object?>("DimName1", dimensionValues[this.random.Value!.Next(0, 10)]);
        var tag2 = new KeyValuePair<string, object?>("DimName2", dimensionValues[this.random.Value.Next(0, 10)]);
        var tag3 = new KeyValuePair<string, object?>("DimName3", dimensionValues[this.random.Value.Next(0, 10)]);
        this.counterWithNoListener?.Add(100, tag1, tag2, tag3);
    }

    [Benchmark]
    public void InstrumentWithNoListener4Dimensions()
    {
        var tags = new TagList
        {
            { "DimName1", dimensionValues[this.random.Value!.Next(0, 2)] },
            { "DimName2", dimensionValues[this.random.Value.Next(0, 5)] },
            { "DimName3", dimensionValues[this.random.Value.Next(0, 10)] },
            { "DimName4", dimensionValues[this.random.Value.Next(0, 10)] },
        };

        // 2 * 5 * 10 * 10 = 1000 time series max.
        this.counterWithNoListener?.Add(100, tags);
    }

    [Benchmark]
    public void InstrumentWithWithListener3Dimensions()
    {
        var tag1 = new KeyValuePair<string, object?>("DimName1", dimensionValues[this.random.Value!.Next(0, 10)]);
        var tag2 = new KeyValuePair<string, object?>("DimName2", dimensionValues[this.random.Value.Next(0, 10)]);
        var tag3 = new KeyValuePair<string, object?>("DimName3", dimensionValues[this.random.Value.Next(0, 10)]);
        this.counterWithListener?.Add(100, tag1, tag2, tag3);
    }

    [Benchmark]
    public void InstrumentWithWithListener4Dimensions()
    {
        var tags = new TagList
        {
            { "DimName1", dimensionValues[this.random.Value!.Next(0, 2)] },
            { "DimName2", dimensionValues[this.random.Value.Next(0, 5)] },
            { "DimName3", dimensionValues[this.random.Value.Next(0, 10)] },
            { "DimName4", dimensionValues[this.random.Value.Next(0, 10)] },
        };

        // 2 * 5 * 10 * 10 = 1000 time series max.
        this.counterWithListener?.Add(100, tags);
    }

    [Benchmark]
    public void InstrumentWithWithDummyReader3Dimensions()
    {
        var tag1 = new KeyValuePair<string, object?>("DimName1", dimensionValues[this.random.Value!.Next(0, 10)]);
        var tag2 = new KeyValuePair<string, object?>("DimName2", dimensionValues[this.random.Value.Next(0, 10)]);
        var tag3 = new KeyValuePair<string, object?>("DimName3", dimensionValues[this.random.Value.Next(0, 10)]);
        this.counterWithDummyReader?.Add(100, tag1, tag2, tag3);
    }

    [Benchmark]
    public void InstrumentWithWithDummyReader4Dimensions()
    {
        var tags = new TagList
        {
            { "DimName1", dimensionValues[this.random.Value!.Next(0, 2)] },
            { "DimName2", dimensionValues[this.random.Value.Next(0, 5)] },
            { "DimName3", dimensionValues[this.random.Value.Next(0, 10)] },
            { "DimName4", dimensionValues[this.random.Value.Next(0, 10)] },
        };

        // 2 * 5 * 10 * 10 = 1000 time series max.
        this.counterWithDummyReader?.Add(100, tags);
    }

    [Benchmark]
    public void InstrumentWithWithGenevaCounterMetricExporter3Dimensions()
    {
        var tag1 = new KeyValuePair<string, object?>("DimName1", dimensionValues[this.random.Value!.Next(0, 10)]);
        var tag2 = new KeyValuePair<string, object?>("DimName2", dimensionValues[this.random.Value.Next(0, 10)]);
        var tag3 = new KeyValuePair<string, object?>("DimName3", dimensionValues[this.random.Value.Next(0, 10)]);
        this.counterWithGenevaMetricExporter?.Add(100, tag1, tag2, tag3);
    }

    [Benchmark]
    public void InstrumentWithWithGenevaCounterMetricExporter4Dimensions()
    {
        var tags = new TagList
        {
            { "DimName1", dimensionValues[this.random.Value!.Next(0, 2)] },
            { "DimName2", dimensionValues[this.random.Value.Next(0, 5)] },
            { "DimName3", dimensionValues[this.random.Value.Next(0, 10)] },
            { "DimName4", dimensionValues[this.random.Value.Next(0, 10)] },
        };

        // 2 * 5 * 10 * 10 = 1000 time series max.
        this.counterWithGenevaMetricExporter?.Add(100, tags);
    }

    [Benchmark]
    public void SerializeCounterMetricItemWith3Dimensions()
    {
        this.counterMetricPointWith3Dimensions.TryGetExemplars(out var exemplars);

        this.tlvMetricsExporter!.SerializeMetricWithTLV(
            MetricEventType.ULongMetric,
            this.counterMetricWith3Dimensions!.Name,
            this.counterMetricPointWith3Dimensions.EndTime.ToFileTime(),
            this.counterMetricPointWith3Dimensions.Tags,
            this.counterMetricDataWith3Dimensions,
            MetricType.LongSum,
            exemplars,
            out _,
            out _);
    }

    [Benchmark]
    public void SerializeCounterMetricItemWith4Dimensions()
    {
        this.counterMetricPointWith4Dimensions.TryGetExemplars(out var exemplars);
        this.tlvMetricsExporter!.SerializeMetricWithTLV(
            MetricEventType.ULongMetric,
            this.counterMetricWith4Dimensions!.Name,
            this.counterMetricPointWith4Dimensions.EndTime.ToFileTime(),
            this.counterMetricPointWith4Dimensions.Tags,
            this.counterMetricDataWith4Dimensions,
            MetricType.LongSum,
            exemplars,
            out _,
            out _);
    }

    [Benchmark]
    public void ExportCounterMetricItemWith3Dimensions()
    {
        this.tlvMetricsExporter!.Export(this.counterMetricBatchWith3Dimensions);
    }

    [Benchmark]
    public void ExportCounterMetricItemWith4Dimensions()
    {
        this.tlvMetricsExporter!.Export(this.counterMetricBatchWith4Dimensions);
    }

    [Benchmark]
    public void SerializeHistogramMetricItemWith3Dimensions()
    {
        this.histogramMetricPointWith3Dimensions.TryGetExemplars(out var exemplars);
        this.tlvMetricsExporter!.SerializeHistogramMetricWithTLV(
            this.histogramMetricWith3Dimensions!.Name,
            this.histogramMetricPointWith3Dimensions.EndTime.ToFileTime(),
            this.histogramMetricPointWith3Dimensions.Tags,
            this.histogramMetricPointWith3Dimensions.GetHistogramBuckets(),
            this.histogramSumWith3Dimensions,
            this.histogramCountWith3Dimensions,
            this.histogramMinWith3Dimensions,
            this.histogramMaxWith3Dimensions,
            MetricType.Histogram,
            exemplars,
            out _,
            out _);
    }

    [Benchmark]
    public void SerializeHistogramMetricItemWith4Dimensions()
    {
        this.histogramMetricPointWith4Dimensions.TryGetExemplars(out var exemplars);
        this.tlvMetricsExporter!.SerializeHistogramMetricWithTLV(
            this.histogramMetricWith4Dimensions!.Name,
            this.histogramMetricPointWith4Dimensions.EndTime.ToFileTime(),
            this.histogramMetricPointWith4Dimensions.Tags,
            this.histogramMetricPointWith4Dimensions.GetHistogramBuckets(),
            this.histogramSumWith4Dimensions,
            this.histogramCountWith4Dimensions,
            this.histogramMinWith4Dimensions,
            this.histogramMaxWith4Dimensions,
            MetricType.Histogram,
            exemplars,
            out _,
            out _);
    }

    [Benchmark]
    public void ExportHistogramMetricItemWith3Dimensions()
    {
        this.tlvMetricsExporter!.Export(this.histogramMetricBatchWith3Dimensions);
    }

    [Benchmark]
    public void ExportHistogramMetricItemWith4Dimensions()
    {
        this.tlvMetricsExporter!.Export(this.histogramMetricBatchWith4Dimensions);
    }

    [Benchmark]
    public void SerializeCounterMetricItemWith3Dimensions_Otlp()
    {
        this.otlpProtobufSerializer!.SerializeAndSendMetrics(this.buffer!, this.resource!, this.counterMetricBatchWith3Dimensions);
    }

    [Benchmark]
    public void SerializeCounterMetricItemWith4Dimensions_Otlp()
    {
        this.otlpProtobufSerializer!.SerializeAndSendMetrics(this.buffer!, this.resource!, this.counterMetricBatchWith4Dimensions);
    }

    [Benchmark]
    public void ExportCounterMetricItemWith3Dimensions_Otlp()
    {
        this.otlpProtobufMetricExporter!.Export(this.counterMetricBatchWith3Dimensions);
    }

    [Benchmark]
    public void ExportCounterMetricItemWith4Dimensions_Otlp()
    {
        this.otlpProtobufMetricExporter!.Export(this.counterMetricBatchWith4Dimensions);
    }

    [Benchmark]
    public void SerializeHistogramMetricItemWith3Dimensions_Otlp()
    {
        this.otlpProtobufSerializer!.SerializeAndSendMetrics(this.buffer!, this.resource!, this.histogramMetricBatchWith3Dimensions);
    }

    [Benchmark]
    public void SerializeHistogramMetricItemWith4Dimensions_Otlp()
    {
        this.otlpProtobufSerializer!.SerializeAndSendMetrics(this.buffer!, this.resource!, this.histogramMetricBatchWith4Dimensions);
    }

    [Benchmark]
    public void ExportHistogramMetricItemWith3Dimensions_Otlp()
    {
        this.otlpProtobufMetricExporter!.Export(this.histogramMetricBatchWith3Dimensions);
    }

    [Benchmark]
    public void ExportHistogramMetricItemWith4Dimensions_Otlp()
    {
        this.otlpProtobufMetricExporter!.Export(this.histogramMetricBatchWith4Dimensions);
    }

    private class DummyReader : BaseExportingMetricReader
    {
        public DummyReader(BaseExporter<Metric> exporter)
            : base(exporter)
        {
        }
    }

    private class DummyMetricExporter : BaseExporter<Metric>
    {
        public override ExportResult Export(in Batch<Metric> batch)
        {
            return ExportResult.Success;
        }
    }

    private class BatchGenerator : BaseExporter<Metric>
    {
        public Batch<Metric> Batch { get; set; }

        public override ExportResult Export(in Batch<Metric> batch)
        {
            this.Batch = batch;
            return ExportResult.Success;
        }
    }

    private class TestTransport : IMetricDataTransport
    {
        public void SendOtlpProtobufEvent(byte[] body, int size)
        {
            // Drop
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
