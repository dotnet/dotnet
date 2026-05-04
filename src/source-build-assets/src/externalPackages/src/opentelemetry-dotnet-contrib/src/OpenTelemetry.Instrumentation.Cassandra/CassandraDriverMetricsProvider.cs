// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Cassandra.Metrics;
using Cassandra.Metrics.Abstractions;

namespace OpenTelemetry.Instrumentation.Cassandra;

internal sealed class CassandraDriverMetricsProvider : IDriverMetricsProvider
{
    private const string Prefix = "cassandra";

    public IDriverTimer Timer(string bucket, IMetric metric)
    {
        return new DriverTimer($"{Prefix}.{metric.Name}");
    }

    public IDriverMeter Meter(string bucket, IMetric metric)
    {
        return new DriverMeter($"{Prefix}.{metric.Name}");
    }

    public IDriverCounter Counter(string bucket, IMetric metric)
    {
        return new DriverCounter($"{Prefix}.{metric.Name}");
    }

    public IDriverGauge Gauge(string bucket, IMetric metric, Func<double?> valueProvider)
    {
        return new DriverGauge($"{Prefix}.{metric.Name}", Value(valueProvider));
    }

    public void ShutdownMetricsBucket(string bucket)
    {
    }

    private static Func<double> Value(Func<double?> valueProvider)
    {
        return () => valueProvider.Invoke() ?? 0;
    }
}
