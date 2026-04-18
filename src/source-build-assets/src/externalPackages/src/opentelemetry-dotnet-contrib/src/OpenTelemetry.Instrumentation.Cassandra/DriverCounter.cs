// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics.Metrics;
using Cassandra.Metrics.Abstractions;

namespace OpenTelemetry.Instrumentation.Cassandra;

internal sealed class DriverCounter : IDriverCounter
{
    private readonly Counter<long> counter;

    public DriverCounter(string name)
    {
        this.counter = CassandraMeter.Instance.CreateCounter<long>(name);
    }

    public void Increment()
    {
        this.counter.Add(1);
    }

    public void Increment(long value)
    {
        this.counter.Add(value);
    }
}
