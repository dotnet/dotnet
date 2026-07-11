// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Concurrent;
using Amazon.Runtime.Telemetry;
using Amazon.Runtime.Telemetry.Metrics;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Instrumentation.AWS.Implementation.Metrics;

internal sealed class AWSHistogram<T> : Histogram<T>
    where T : struct
{
    private static readonly ConcurrentDictionary<string, System.Diagnostics.Metrics.Histogram<T>> HistogramsDictionary = new();

    private readonly Func<bool> isDisposed;
    private readonly System.Diagnostics.Metrics.Histogram<T> histogram;

    public AWSHistogram(
        System.Diagnostics.Metrics.Meter meter,
        string name,
        Func<bool> isDisposed,
        string? units = null,
        string? description = null)
    {
        Guard.ThrowIfNull(isDisposed);
        this.isDisposed = isDisposed;

#if NET
        this.histogram = HistogramsDictionary.GetOrAdd(
            name,
            static (histogramName, state) => state.meter.CreateHistogram<T>(histogramName, state.units, state.description),
            (meter, units, description));
#else
        this.histogram = HistogramsDictionary.GetOrAdd(
            name,
            (histogramName) => meter.CreateHistogram<T>(histogramName, units, description));
#endif
    }

    public override void Record(T value, Attributes? attributes = null)
    {
        if (this.isDisposed())
        {
            return;
        }

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
