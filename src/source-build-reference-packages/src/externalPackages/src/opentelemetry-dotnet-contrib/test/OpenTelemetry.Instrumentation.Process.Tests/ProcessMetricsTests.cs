// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Metrics;
using Xunit;

namespace OpenTelemetry.Instrumentation.Process.Tests;

public class ProcessMetricsTests
{
    private const int MaxTimeToAllowForFlush = 10000;

    [Fact]
    public void ProcessMetricsAreCaptured()
    {
        var exportedItemsA = new List<Metric>();
        var meterProviderA = Sdk.CreateMeterProviderBuilder()
            .AddProcessInstrumentation()
            .AddInMemoryExporter(exportedItemsA)
            .Build();

        meterProviderA.ForceFlush(MaxTimeToAllowForFlush);

        Assert.Equal(5, exportedItemsA.Count);
        var physicalMemoryMetric = exportedItemsA.FirstOrDefault(i => i.Name == "process.memory.usage");
        Assert.NotNull(physicalMemoryMetric);
        var virtualMemoryMetric = exportedItemsA.FirstOrDefault(i => i.Name == "process.memory.virtual");
        Assert.NotNull(virtualMemoryMetric);
        var cpuTimeMetric = exportedItemsA.FirstOrDefault(i => i.Name == "process.cpu.time");
        Assert.NotNull(cpuTimeMetric);
        var processorCountMetric = exportedItemsA.FirstOrDefault(i => i.Name == "process.cpu.count");
        Assert.NotNull(processorCountMetric);
        var threadMetric = exportedItemsA.FirstOrDefault(i => i.Name == "process.cpu.count");
        Assert.NotNull(threadMetric);

        exportedItemsA.Clear();

        var exportedItemsB = new List<Metric>();

        using var meterProviderB = Sdk.CreateMeterProviderBuilder()
            .AddProcessInstrumentation()
            .AddInMemoryExporter(exportedItemsA, metricReaderOptions =>
            {
                metricReaderOptions.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds = 1500;
            })
            .AddInMemoryExporter(exportedItemsB, metricReaderOptions =>
            {
                metricReaderOptions.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds = 5000;
            })
            .Build();

        meterProviderB.ForceFlush(MaxTimeToAllowForFlush);

        Assert.Equal(5, exportedItemsA.Count);
        Assert.Equal(5, exportedItemsB.Count);
    }

    [Fact]
    public void CpuTimeMetricsAreCaptured()
    {
        var exportedItems = new List<Metric>();
        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddProcessInstrumentation()
            .AddInMemoryExporter(exportedItems)
            .Build();

        meterProvider.ForceFlush(MaxTimeToAllowForFlush);

        var cpuTimeMetric = exportedItems.FirstOrDefault(i => i.Name == "process.cpu.time");
        Assert.NotNull(cpuTimeMetric);

        var userTimeCaptured = false;
        var systemTimeCaptured = false;

        var points = cpuTimeMetric.GetMetricPoints().GetEnumerator();
        while (points.MoveNext() && (!userTimeCaptured || !systemTimeCaptured))
        {
            foreach (var tag in points.Current.Tags)
            {
                Assert.NotNull(tag.Value);

                if (tag.Key == "process.cpu.state" && tag.Value!.ToString() == "user")
                {
                    userTimeCaptured = true;
                }
                else if (tag.Key == "process.cpu.state" && tag.Value!.ToString() == "system")
                {
                    systemTimeCaptured = true;
                }
            }
        }

        Assert.True(userTimeCaptured);
        Assert.True(systemTimeCaptured);
    }

    [Fact]
    public async Task ProcessMetricsAreCapturedWhenTasksOverlap()
    {
        var exportedItemsA = new List<Metric>();
        var exportedItemsB = new List<Metric>();

        var tasks = new List<Task>()
        {
            Task.Run(() =>
            {
                var meterProviderA = Sdk.CreateMeterProviderBuilder()
                    .AddProcessInstrumentation()
                    .AddInMemoryExporter(exportedItemsA)
                    .Build();

                Thread.Sleep(3000); // increase the odds of 2 tasks overlaps

                meterProviderA.ForceFlush(MaxTimeToAllowForFlush);
            }),

            Task.Run(() =>
            {
                var meterProviderB = Sdk.CreateMeterProviderBuilder()
                    .AddProcessInstrumentation()
                    .AddInMemoryExporter(exportedItemsB)
                    .Build();

                Thread.Sleep(3000); // increase the odds of 2 tasks overlaps

                meterProviderB.ForceFlush(MaxTimeToAllowForFlush);
            }),
        };

        await Task.WhenAll(tasks);

        Assert.Equal(5, exportedItemsA.Count);
        var physicalMemoryMetricA = exportedItemsA.FirstOrDefault(i => i.Name == "process.memory.usage");
        Assert.NotNull(physicalMemoryMetricA);
        var virtualMemoryMetricA = exportedItemsA.FirstOrDefault(i => i.Name == "process.memory.virtual");
        Assert.NotNull(virtualMemoryMetricA);
        var cpuTimeMetricA = exportedItemsA.FirstOrDefault(i => i.Name == "process.cpu.time");
        Assert.NotNull(cpuTimeMetricA);
        var processorCountMetricA = exportedItemsA.FirstOrDefault(i => i.Name == "process.cpu.count");
        Assert.NotNull(processorCountMetricA);
        var threadMetricA = exportedItemsA.FirstOrDefault(i => i.Name == "process.thread.count");
        Assert.NotNull(threadMetricA);

        Assert.Equal(5, exportedItemsB.Count);
        var physicalMemoryMetricB = exportedItemsB.FirstOrDefault(i => i.Name == "process.memory.usage");
        Assert.NotNull(physicalMemoryMetricB);
        var virtualMemoryMetricB = exportedItemsB.FirstOrDefault(i => i.Name == "process.memory.virtual");
        Assert.NotNull(virtualMemoryMetricB);
        var cpuTimeMetricB = exportedItemsB.FirstOrDefault(i => i.Name == "process.cpu.time");
        Assert.NotNull(cpuTimeMetricB);
        var processorCountMetricB = exportedItemsB.FirstOrDefault(i => i.Name == "process.cpu.count");
        Assert.NotNull(processorCountMetricB);
        var threadMetricB = exportedItemsB.FirstOrDefault(i => i.Name == "process.thread.count");
        Assert.NotNull(threadMetricB);
    }

    [Fact]
    public void CheckValidGaugeValueWhen2MeterProviderInstancesHaveTheSameMeterName()
    {
        var exportedItemsA = new List<Metric>();
        var exportedItemsB = new List<Metric>();

        using var meterProviderA = Sdk.CreateMeterProviderBuilder()
            .AddProcessInstrumentation()
            .AddInMemoryExporter(exportedItemsA)
            .Build();

        using (var meterProviderB = Sdk.CreateMeterProviderBuilder()
            .AddProcessInstrumentation()
            .AddInMemoryExporter(exportedItemsB)
            .Build())
        {
            meterProviderA.ForceFlush(MaxTimeToAllowForFlush);
            meterProviderB.ForceFlush(MaxTimeToAllowForFlush);

            var metricA = exportedItemsA.FirstOrDefault(i => i.Name == "process.memory.usage");
            var metricB = exportedItemsB.FirstOrDefault(i => i.Name == "process.memory.usage");

            Assert.NotNull(metricA);
            Assert.NotNull(metricB);

            Assert.True(GetValue(metricA) > 0);
            Assert.True(GetValue(metricB) > 0);
        }

        exportedItemsA.Clear();
        exportedItemsB.Clear();

        meterProviderA.ForceFlush(MaxTimeToAllowForFlush);

        Assert.NotEmpty(exportedItemsA);
        Assert.Empty(exportedItemsB);

        exportedItemsA.Clear();
        exportedItemsB.Clear();

        meterProviderA.ForceFlush(MaxTimeToAllowForFlush);

        Assert.NotEmpty(exportedItemsA);
        Assert.Empty(exportedItemsB);

        exportedItemsA.Clear();

        meterProviderA.ForceFlush(MaxTimeToAllowForFlush);

        Assert.NotEmpty(exportedItemsA);
        Assert.Empty(exportedItemsB);
    }

    private static double GetValue(Metric metric)
    {
        double sum = 0;

        foreach (ref readonly var metricPoint in metric.GetMetricPoints())
        {
            if (metric.MetricType.IsLong())
            {
                sum += metricPoint.GetSumLong();
            }
        }

        return sum;
    }
}
