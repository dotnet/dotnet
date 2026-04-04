// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics.Tracing;
using OpenTelemetry.Metrics;
using Xunit;

namespace OpenTelemetry.Instrumentation.EventCounters.Tests;

public class EventCountersMetricsTests
{
    [Fact]
    public void EventCounter()
    {
        // Arrange
        List<Metric> metricItems = [];
        using EventSource source = new("a");
        using EventCounter counter = new("c", source);

        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddEventCountersInstrumentation(options =>
            {
                options.AddEventSources(source.Name);
            })
            .AddInMemoryExporter(metricItems)
            .Build()!;

        // Act
        counter.WriteMetric(1997.0202);
        var metric = AwaitExport(meterProvider, metricItems, expectedInstrumentName: "ec.a.c");

        // Assert
        Assert.NotNull(metric);
        Assert.Equal(MetricType.DoubleGauge, metric.MetricType);
        Assert.Equal(1997.0202, GetActualValue(metric));
    }

    [Fact]
    public void IncrementingEventCounter()
    {
        // Arrange
        List<Metric> metricItems = [];
        using EventSource source = new("b");
        using IncrementingEventCounter incCounter = new("inc-c", source);

        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddEventCountersInstrumentation(options =>
            {
                options.AddEventSources(source.Name);
            })
            .AddInMemoryExporter(metricItems)
            .Build()!;

        // Act
        incCounter.Increment(1);
        incCounter.Increment(1);
        incCounter.Increment(1);
        var metric = AwaitExport(meterProvider, metricItems, expectedInstrumentName: "ec.b.inc-c");

        // Assert
        Assert.NotNull(metric);
        Assert.Equal(MetricType.DoubleSum, metric.MetricType);
        Assert.Equal(3, GetActualValue(metric));
    }

    [Fact]
    public void PollingCounter()
    {
        // Arrange
        var i = 0;
        List<Metric> metricItems = [];
        using EventSource source = new("c");
        using PollingCounter pollCounter = new("poll-c", source, () => ++i * 10);

        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddEventCountersInstrumentation(options =>
            {
                options.AddEventSources(source.Name);
            })
            .AddInMemoryExporter(metricItems)
            .Build()!;

        // Act
        var metric = AwaitExport(meterProvider, metricItems, expectedInstrumentName: "ec.c.poll-c");

        // Assert
        Assert.NotNull(metric);
        Assert.Equal(MetricType.DoubleGauge, metric.MetricType);
        Assert.Equal(10, GetActualValue(metric));
    }

    [Fact]
    public void IncrementingPollingCounter()
    {
        // Arrange
        var i = 1;
        List<Metric> metricItems = [];
        using EventSource source = new("d");
        using IncrementingPollingCounter incPollCounter = new("inc-poll-c", source, () => i++);

        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddEventCountersInstrumentation(options =>
            {
                options.AddEventSources(source.Name);
            })
            .AddInMemoryExporter(metricItems)
            .Build()!;

        // Act
        var metric = AwaitExport(meterProvider, metricItems, expectedInstrumentName: "ec.d.inc-poll-c");

        // Assert
        Assert.NotNull(metric);
        Assert.Equal(MetricType.DoubleSum, metric.MetricType);
        Assert.Equal(1, GetActualValue(metric));
    }

    [Fact]
    public void ThrowExceptionForUnsupportedEventSources()
    {
        var ex = Assert.Throws<NotSupportedException>(() =>
        {
            Sdk.CreateMeterProviderBuilder()
            .AddEventCountersInstrumentation(options =>
            {
                options.AddEventSources("System.Runtime");
            });
        });

        Assert.Equal("Use the `OpenTelemetry.Instrumentation.Runtime` or `OpenTelemetry.Instrumentation.Process` instrumentations.", ex.Message);
    }

    [Theory]
    [InlineData("Microsoft-AspNetCore-Server-Kestrel-1", "tls-handshakes-per-second", "ec.Microsoft-AspNetCore-Server-Kestre.tls-handshakes-per-second")]
    [InlineData("Microsoft-AspNetCore-Server-Kestrel-1", "tls-handshakes-per-sec", "ec.Microsoft-AspNetCore-Server-Kestrel-1.tls-handshakes-per-sec")]
    [InlineData("Microsoft.AspNetCore.Http.Connections-1", "connections-stopped", "ec.Microsoft.AspNetCore.Http.Connections-1.connections-stopped")]
    [InlineData("Microsoft.AspNetCore.Http.Connections-1", "connections-timed-out-longer", "ec.Microsoft.AspNetCore.Http.Conne.connections-timed-out-longer")]
    [InlineData("Microsoft.AspNetCore.Http.Conn.Something", "connections-timed-out-longer", "ec.Microsoft.AspNetCore.Http.Conn.connections-timed-out-longer")]
    [InlineData("Microsoft.AspNetCore.One.Two", "very-very-very-very-very-very-very-very-very-long-event-name", "ec.very-very-very-very-very-very-very-very-very-long-event-name")]
    [InlineData("Microsoft.AspNetCore.One.Two", "very-very-very-very-very-very-very-very-long-event-name", "ec.Micr.very-very-very-very-very-very-very-very-long-event-name")]
    [InlineData("Microsoft.AspNetCore.One.Two", "very-very-very-very-very-very-very-long-event-name", "ec.Microsoft.very-very-very-very-very-very-very-long-event-name")]
    public void EventSourceNameShortening(string sourceName, string eventName, string expectedInstrumentName)
    {
        // Arrange
        List<Metric> metricItems = [];
        using EventSource source = new(sourceName);
        using IncrementingEventCounter connections = new(eventName, source);

        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddEventCountersInstrumentation(options =>
            {
                options.AddEventSources(source.Name);
            })
            .AddInMemoryExporter(metricItems)
            .Build()!;

        // Act
        connections.Increment(1);
        var metric = AwaitExport(meterProvider, metricItems, expectedInstrumentName);

        // Assert
        Assert.NotNull(metric);
        Assert.Equal(1, GetActualValue(metric));
    }

    [Fact]
    public async Task InstrumentNameTooLong()
    {
        // Arrange
        List<Metric> metricItems = [];
        using EventSource source = new("source");

        // ec.s. + event name is 63;
        var veryLongEventName = new string('e', 100);
        using IncrementingEventCounter connections = new(veryLongEventName, source);

        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddEventCountersInstrumentation(options =>
            {
                options.AddEventSources(source.Name);
            })
            .AddInMemoryExporter(metricItems)
            .Build();

        // Act
        connections.Increment(1);

        await Task.Delay(1800);
        meterProvider.ForceFlush();

        // Assert
        foreach (var item in metricItems)
        {
            Assert.False(item.Name.StartsWith("ec.source.ee", StringComparison.Ordinal));
            Assert.False(item.Name.StartsWith("ec.s.ee", StringComparison.Ordinal));
        }
    }

    private static double GetActualValue(Metric metric)
    {
        double sum = 0;

        foreach (ref readonly var metricPoint in metric.GetMetricPoints())
        {
            sum += metric.MetricType.IsSum()
                ? metricPoint.GetSumDouble()
                : metricPoint.GetGaugeLastValueDouble();
        }

        return sum;
    }

    private static Metric? AwaitExport(MeterProvider meterProvider, List<Metric> exportedItems, string expectedInstrumentName)
    {
        Metric? metric = null;

        SpinWait.SpinUntil(
            () =>
            {
                Thread.Sleep(100);
                meterProvider.ForceFlush();
                metric = exportedItems.FirstOrDefault(x => x.Name == expectedInstrumentName);
                return metric != null && Math.Abs(GetActualValue(metric)) > double.Epsilon;
            },
            10_000);

        return metric;
    }
}
