// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics.Metrics;
using Cassandra.Metrics.Abstractions;

namespace OpenTelemetry.Instrumentation.Cassandra;

internal sealed class DriverGauge : IDriverGauge
{
#pragma warning disable IDE0052 // Remove unread private members
    private readonly ObservableGauge<double> gauge;
#pragma warning restore IDE0052 // Remove unread private members

    public DriverGauge(string name, Func<double> value)
    {
        this.gauge = CassandraMeter.Instance.CreateObservableGauge(name, value);
    }
}
