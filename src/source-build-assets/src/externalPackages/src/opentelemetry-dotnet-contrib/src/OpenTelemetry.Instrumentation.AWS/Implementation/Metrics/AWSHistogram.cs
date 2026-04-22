// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Concurrent;
using Amazon.Runtime.Telemetry;
using Amazon.Runtime.Telemetry.Metrics;

namespace OpenTelemetry.Instrumentation.AWS.Implementation.Metrics;

internal sealed class AWSHistogram<T> : Histogram<T>
    where T : struct
{
    private static readonly ConcurrentDictionary<string, System.Diagnostics.Metrics.Histogram<T>> HistogramsDictionary = new();

    private readonly System.Diagnostics.Metrics.Histogram<T> histogram;

    public AWSHistogram(
        System.Diagnostics.Metrics.Meter meter,
        string name,
        string? units = null,
        string? description = null)
    {
        if (HistogramsDictionary.TryGetValue(name, out var histogram))
        {
            this.histogram = histogram;
        }

        this.histogram = HistogramsDictionary.GetOrAdd(
            name,
            meter.CreateHistogram<T>(name, units, description));
    }

    public override void Record(T value, Attributes? attributes = null)
    {
        if (attributes != null)
        {
            // TODO: remove ToArray call and use when AttributesAsSpan expected to be added at AWS SDK v4.
            this.histogram.Record(value, attributes.AllAttributes.ToArray());
        }
        else
        {
            this.histogram.Record(value);
        }
    }
}
