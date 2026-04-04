// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Metrics;
using Xunit;

namespace OpenTelemetry.Instrumentation.Hangfire.Tests.Utils;

/// <summary>
/// Utility methods for asserting metric properties in tests.
/// </summary>
internal static class AssertUtils
{
    /// <summary>
    /// Asserts that a metric point has a tag with the specified key (tag exists and is not null).
    /// </summary>
    /// <param name="metricPoint">The metric point to check.</param>
    /// <param name="tagKey">The tag key to look for.</param>
    public static void AssertHasTag(MetricPoint metricPoint, string tagKey)
    {
        var tagValue = metricPoint.GetTagValue(tagKey);
        Assert.NotNull(tagValue);
    }

    /// <summary>
    /// Asserts that a metric point has a tag with the specified key and exact value.
    /// </summary>
    /// <param name="metricPoint">The metric point to check.</param>
    /// <param name="tagKey">The tag key to look for.</param>
    /// <param name="expectedValue">The expected tag value.</param>
    public static void AssertHasTagValue(MetricPoint metricPoint, string tagKey, string expectedValue)
    {
        var tagValue = metricPoint.GetTagValue(tagKey);
        Assert.Equal(expectedValue, tagValue);
    }

    /// <summary>
    /// Asserts that a metric point has a tag with the specified key and that its value contains the specified substring.
    /// </summary>
    /// <param name="metricPoint">The metric point to check.</param>
    /// <param name="tagKey">The tag key to look for.</param>
    /// <param name="substring">The substring that should be present in the tag value.</param>
    public static void AssertTagContains(MetricPoint metricPoint, string tagKey, string substring)
    {
        var tagValue = metricPoint.GetTagValue(tagKey);
        Assert.NotNull(tagValue);
        Assert.Contains(substring, tagValue);
    }

    /// <summary>
    /// Asserts that a metric point does not have a tag with the specified key (tag is null).
    /// </summary>
    /// <param name="metricPoint">The metric point to check.</param>
    /// <param name="tagKey">The tag key to look for.</param>
    public static void AssertHasNoTag(MetricPoint metricPoint, string tagKey)
    {
        var tagValue = metricPoint.GetTagValue(tagKey);
        Assert.Null(tagValue);
    }

    /// <summary>
    /// Asserts that a metric with the specified name exists in the exported items.
    /// </summary>
    /// <param name="exportedItems">The list of exported metrics.</param>
    /// <param name="metricName">The name of the metric to find.</param>
    public static void AssertMetricExists(List<Metric> exportedItems, string metricName)
    {
        var metric = exportedItems.GetMetric(metricName);
        Assert.NotNull(metric);
    }

    /// <summary>
    /// Asserts that a metric either doesn't exist or has a sum of zero.
    /// Useful for verifying that a counter metric was not incremented.
    /// </summary>
    /// <param name="exportedItems">The list of exported metrics.</param>
    /// <param name="metricName">The name of the metric to check.</param>
    public static void AssertMetricNotRecordedOrZero(List<Metric> exportedItems, string metricName)
    {
        var metric = exportedItems.GetMetric(metricName);
        if (metric != null)
        {
            var metricPoints = metric.ToMetricPointList();
            var sum = metricPoints.Sum(mp => mp.GetSumLong());
            Assert.Equal(0, sum);
        }
    }

    public static void AssertHasMetricPoints(Metric? metricPoints)
    {
        Assert.NotNull(metricPoints);
        Assert.NotEmpty(metricPoints.ToMetricPointList());
    }
}
