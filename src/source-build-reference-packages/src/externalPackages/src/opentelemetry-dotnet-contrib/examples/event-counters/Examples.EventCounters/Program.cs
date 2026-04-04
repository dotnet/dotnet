// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics.Tracing;
using OpenTelemetry;
using OpenTelemetry.Metrics;

// Create EventSources and EventCounters
ThreadLocal<Random> random = new(() => new Random());
using EventSource eventSource = new("MyEventSource");
using EventCounter eventCounter = new("MyEventCounter", eventSource);
using PollingCounter pollingCounter = new("MyPollingCounter", eventSource, () => random.Value!.NextDouble());

// Create and Configure Meter Provider
using var meterProvider = Sdk.CreateMeterProviderBuilder()
    .AddEventCountersInstrumentation(options =>
    {
        options.AddEventSources(eventSource.Name);
        options.RefreshIntervalSecs = 1;
    })
    .AddConsoleExporter()
    .Build();

// Write to EventCounters
eventCounter.WriteMetric(0);
eventCounter.WriteMetric(1000);

// Wait for EventCounter data to be polled (RefreshIntervalSecs is 1 second by default)
Thread.Sleep(1200);
