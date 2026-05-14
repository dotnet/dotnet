// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Concurrent;
using Amazon.Runtime.Telemetry;
using Amazon.Runtime.Telemetry.Metrics;

namespace OpenTelemetry.Instrumentation.AWS.Implementation.Metrics;

internal sealed class AWSUpDownCounter<T> : UpDownCounter<T>
    where T : struct
{
    private static readonly ConcurrentDictionary<string, System.Diagnostics.Metrics.UpDownCounter<T>> UpDownCountersDictionary = new();

    private readonly System.Diagnostics.Metrics.UpDownCounter<T> upDownCounter;

    public AWSUpDownCounter(
        System.Diagnostics.Metrics.Meter meter,
        string name,
        string? units = null,
        string? description = null)
    {
        if (UpDownCountersDictionary.TryGetValue(name, out var upDownCounter))
        {
            this.upDownCounter = upDownCounter;
        }

        this.upDownCounter = UpDownCountersDictionary.GetOrAdd(
            name,
            meter.CreateUpDownCounter<T>(name, units, description));
    }

    public override void Add(T value, Attributes? attributes = null)
    {
        if (attributes != null)
        {
            // TODO: remove ToArray call and use when AttributesAsSpan expected to be added at AWS SDK v4.
            this.upDownCounter.Add(value, attributes.AllAttributes.ToArray());
        }
        else
        {
            this.upDownCounter.Add(value);
        }
    }
}
