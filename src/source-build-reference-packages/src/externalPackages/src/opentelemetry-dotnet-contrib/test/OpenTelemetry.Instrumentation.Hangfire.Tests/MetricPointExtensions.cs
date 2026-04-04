// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Metrics;

namespace OpenTelemetry.Instrumentation.Hangfire.Tests;

/// <summary>
/// Extension methods for working with MetricPoint collections in tests.
/// </summary>
internal static class MetricPointExtensions
{
    /// <summary>
    /// Filters metric points to those having the specified tag (with optional value matching).
    /// </summary>
    public static IEnumerable<MetricPoint> HavingTag(this IEnumerable<MetricPoint> metricPoints, string tagName, string? tagValue = null)
    {
        foreach (var point in metricPoints)
        {
            foreach (var tag in point.Tags)
            {
                if (tag.Key == tagName && (tagValue == null || tag.Value?.ToString() == tagValue))
                {
                    yield return point;
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Finds the first metric point with the specified tag value.
    /// </summary>
    public static MetricPoint? FindFirstWithTag(this IEnumerable<MetricPoint> metricPoints, string tagName, string tagValue)
    {
        return metricPoints.HavingTag(tagName, tagValue).FirstOrDefault();
    }

    /// <summary>
    /// Checks if a metric point has a specific tag with a specific value.
    /// </summary>
    public static bool HasTag(this MetricPoint metricPoint, string tagName, string tagValue)
    {
        foreach (var tag in metricPoint.Tags)
        {
            if (tag.Key == tagName)
            {
                return tag.Value?.ToString() == tagValue;
            }
        }

        return false;
    }

    /// <summary>
    /// Gets the value of a tag from a metric point, or null if not present.
    /// </summary>
    public static string? GetTagValue(this MetricPoint metricPoint, string tagName)
    {
        foreach (var tag in metricPoint.Tags)
        {
            if (tag.Key == tagName)
            {
                return tag.Value?.ToString();
            }
        }

        return null;
    }

    /// <summary>
    /// Converts metric points from a metric to a list.
    /// </summary>
    public static List<MetricPoint> ToMetricPointList(this Metric metric)
    {
        var metricPoints = new List<MetricPoint>();
        foreach (var mp in metric.GetMetricPoints())
        {
            metricPoints.Add(mp);
        }

        return metricPoints;
    }
}
