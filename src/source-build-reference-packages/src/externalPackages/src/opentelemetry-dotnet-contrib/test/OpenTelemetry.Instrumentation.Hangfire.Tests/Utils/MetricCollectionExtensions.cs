// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Metrics;

namespace OpenTelemetry.Instrumentation.Hangfire.Tests.Utils;

/// <summary>
/// Extension methods for metric collections.
/// </summary>
internal static class MetricCollectionExtensions
{
    /// <summary>
    /// Gets a metric with the specified name from the exported items.
    /// </summary>
    /// <param name="exportedItems">The list of exported metrics.</param>
    /// <param name="metricName">The name of the metric to find.</param>
    /// <returns>The found metric, or null if not found.</returns>
    public static Metric? GetMetric(this List<Metric> exportedItems, string metricName) => exportedItems.FirstOrDefault(m => m.Name == metricName);
}
