// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics.Metrics;
using Cassandra.Metrics.Abstractions;

namespace OpenTelemetry.Instrumentation.Cassandra;

internal sealed class DriverMeter : IDriverMeter
{
    private readonly Histogram<long> meter;

    public DriverMeter(string name)
    {
        this.meter = CassandraMeter.Instance.CreateHistogram<long>(name);
    }

    public void Mark(long amount)
    {
        this.meter.Record(amount);
    }
}
