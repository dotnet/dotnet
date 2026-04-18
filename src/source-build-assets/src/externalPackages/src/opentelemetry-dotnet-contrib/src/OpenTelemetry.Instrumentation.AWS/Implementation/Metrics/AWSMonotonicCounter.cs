// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Concurrent;
using Amazon.Runtime.Telemetry;
using Amazon.Runtime.Telemetry.Metrics;

namespace OpenTelemetry.Instrumentation.AWS.Implementation.Metrics;

internal sealed class AWSMonotonicCounter<T> : MonotonicCounter<T>
    where T : struct
{
    private static readonly ConcurrentDictionary<string, System.Diagnostics.Metrics.Counter<T>> MonotonicCountersDictionary = new();

    private readonly System.Diagnostics.Metrics.Counter<T> monotonicCounter;

    public AWSMonotonicCounter(
        System.Diagnostics.Metrics.Meter meter,
        string name,
        string? units = null,
        string? description = null)
    {
        if (MonotonicCountersDictionary.TryGetValue(name, out var monotonicCounter))
        {
            this.monotonicCounter = monotonicCounter;
        }

        this.monotonicCounter = MonotonicCountersDictionary.GetOrAdd(
            name,
            meter.CreateCounter<T>(name, units, description));
    }

    public override void Add(T value, Attributes? attributes = null)
    {
        if (attributes != null)
        {
            // TODO: remove ToArray call and use when AttributesAsSpan expected to be added at AWS SDK v4.
            this.monotonicCounter.Add(value, attributes.AllAttributes.ToArray());
        }
        else
        {
            this.monotonicCounter.Add(value);
        }
    }
}
