// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Cassandra;
using Cassandra.Metrics;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Instrumentation.Cassandra;

/// <summary>
/// Extension methods to simplify registering cassandra driver metrics.
/// </summary>
public static class CassandraBuilderExtensions
{
    /// <summary>
    /// Configuring open telemetry metrics for cassandra.
    /// </summary>
    /// <param name="builder">Cassandra builder.</param>
    /// <returns>Returning Cassandra builder.</returns>
    public static Builder WithOpenTelemetryMetrics(this Builder builder)
    {
        Guard.ThrowIfNull(builder);

        return builder.WithMetrics(new CassandraDriverMetricsProvider(), GetDefaultOptions());
    }

    /// <summary>
    /// Configuring open telemetry metrics for cassandra.
    /// </summary>
    /// <param name="builder">Cassandra builder.</param>
    /// <param name="options">Cassandra driver metrics options.</param>
    /// <returns>Returning Cassandra builder.</returns>
    public static Builder WithOpenTelemetryMetrics(this Builder builder, DriverMetricsOptions? options)
    {
        Guard.ThrowIfNull(builder);

        return builder.WithMetrics(new CassandraDriverMetricsProvider(), options ?? GetDefaultOptions());
    }

    private static DriverMetricsOptions GetDefaultOptions()
    {
        var options = new DriverMetricsOptions();

        options.SetEnabledNodeMetrics(NodeMetric.AllNodeMetrics);
        options.SetEnabledSessionMetrics(SessionMetric.AllSessionMetrics);

        return options;
    }
}
