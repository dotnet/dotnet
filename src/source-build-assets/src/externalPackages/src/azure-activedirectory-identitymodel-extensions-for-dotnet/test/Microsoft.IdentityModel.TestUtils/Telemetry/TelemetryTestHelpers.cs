// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using Xunit;

namespace Microsoft.IdentityModel.TestUtils.Telemetry;

/// <summary>
/// Test meter listener to capture telemetry measurements
/// </summary>
public class TestMeterListener : IDisposable
{
    private readonly MeterListener _listener;
    private readonly List<Measurement> _measurements = new List<Measurement>();
    private readonly long _startTimestamp;

    public TestMeterListener()
    {
        _startTimestamp = System.Diagnostics.Stopwatch.GetTimestamp();
        _listener = new MeterListener();
        _listener.InstrumentPublished = (instrument, listener) =>
        {
            if (instrument.Meter.Name == "MicrosoftIdentityModel_Meter")
            {
                listener.EnableMeasurementEvents(instrument);
            }
        };

        _listener.SetMeasurementEventCallback<long>((instrument, measurement, tags, state) =>
        {
            lock (_measurements)
            {
                _measurements.Add(new Measurement
                {
                    InstrumentName = instrument.Name,
                    Value = measurement,
                    Tags = tags.ToArray(),
                    Timestamp = System.Diagnostics.Stopwatch.GetTimestamp()
                });
            }
        });

        _listener.Start();
    }

    public List<Measurement> GetMeasurements(string instrumentName)
    {
        lock (_measurements)
        {
            // Only return measurements that were recorded after this listener started
            return _measurements
                .Where(m => m.InstrumentName == instrumentName && m.Timestamp >= _startTimestamp)
                .ToList();
        }
    }

    public void Dispose()
    {
        _listener?.Dispose();
    }

    public class Measurement
    {
        public string InstrumentName { get; set; }
        public long Value { get; set; }
        public KeyValuePair<string, object>[] Tags { get; set; }
        public long Timestamp { get; set; }
    }
}

/// <summary>
/// Helper methods for telemetry testing
/// </summary>
public static class TelemetryAssertionHelpers
{
    /// <summary>
    /// Asserts that telemetry was recorded with the expected tags.
    /// </summary>
    /// <param name="listener">The test meter listener.</param>
    /// <param name="counterName">The name of the counter to check.</param>
    /// <param name="expectedTags">Dictionary of expected tag key-value pairs.</param>
    public static void AssertTelemetryRecorded(TestMeterListener listener, string counterName, Dictionary<string, object> expectedTags)
    {
        var measurements = listener.GetMeasurements(counterName);
        Assert.NotEmpty(measurements);

        var lastMeasurement = measurements.Last();
        foreach (var expectedTag in expectedTags)
        {
            var actualTag = lastMeasurement.Tags.FirstOrDefault(t => t.Key == expectedTag.Key);
            Assert.NotNull(actualTag.Value);
            Assert.Equal(expectedTag.Value.ToString(), actualTag.Value.ToString());
        }
    }
}
