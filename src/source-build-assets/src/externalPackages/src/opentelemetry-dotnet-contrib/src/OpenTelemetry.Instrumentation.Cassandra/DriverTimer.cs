// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics.Metrics;
using Cassandra.Metrics.Abstractions;

namespace OpenTelemetry.Instrumentation.Cassandra;

internal sealed class DriverTimer : IDriverTimer
{
    private readonly Histogram<double> timer;

    public DriverTimer(string name)
    {
        this.timer = CassandraMeter.Instance.CreateHistogram<double>(name, "ms");
    }

    public void Record(long elapsedNanoseconds)
    {
        var elapsedMilliseconds = elapsedNanoseconds * 0.000001;

        this.timer.Record(elapsedMilliseconds);
    }
}
