// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Globalization;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace OpenTelemetry.Exporter.InfluxDB;

internal sealed class TelegrafPrometheusWriterV1 : IMetricsWriter
{
    public void Write(Metric metric, Resource? resource, WriteApi writeApi)
    {
        var measurement = metric.Name;

        switch (metric.MetricType)
        {
            case MetricType.LongGauge:
                {
                    foreach (var dataPoint in metric.GetMetricPoints())
                    {
                        var pointData = PointData
                            .Measurement(measurement)
                            .Field("gauge", dataPoint.GetGaugeLastValueLong())
                            .Tags(dataPoint.Tags)
                            .Tags(resource?.Attributes)
                            .Timestamp(dataPoint.EndTime.UtcDateTime, WritePrecision.Ns);
                        writeApi.WritePoint(pointData);
                    }

                    break;
                }

            case MetricType.DoubleGauge:
                {
                    foreach (var dataPoint in metric.GetMetricPoints())
                    {
                        var pointData = PointData
                            .Measurement(measurement)
                            .Field("gauge", dataPoint.GetGaugeLastValueDouble())
                            .Tags(dataPoint.Tags)
                            .Tags(resource?.Attributes)
                            .Timestamp(dataPoint.EndTime.UtcDateTime, WritePrecision.Ns);
                        writeApi.WritePoint(pointData);
                    }

                    break;
                }

            case MetricType.LongSum:
                {
                    foreach (var dataPoint in metric.GetMetricPoints())
                    {
                        var pointData = PointData
                            .Measurement(measurement)
                            .Field("counter", dataPoint.GetSumLong())
                            .Tags(dataPoint.Tags)
                            .Tags(resource?.Attributes)
                            .Timestamp(dataPoint.EndTime.UtcDateTime, WritePrecision.Ns);
                        writeApi.WritePoint(pointData);
                    }

                    break;
                }

            case MetricType.DoubleSum:
                {
                    foreach (var dataPoint in metric.GetMetricPoints())
                    {
                        var pointData = PointData
                            .Measurement(measurement)
                            .Field("counter", dataPoint.GetSumDouble())
                            .Tags(dataPoint.Tags)
                            .Tags(resource?.Attributes)
                            .Timestamp(dataPoint.EndTime.UtcDateTime, WritePrecision.Ns);
                        writeApi.WritePoint(pointData);
                    }

                    break;
                }

            case MetricType.LongSumNonMonotonic:
                {
                    foreach (var dataPoint in metric.GetMetricPoints())
                    {
                        var pointData = PointData
                            .Measurement(measurement)
                            .Field("gauge", dataPoint.GetSumLong())
                            .Tags(dataPoint.Tags)
                            .Tags(resource?.Attributes)
                            .Timestamp(dataPoint.EndTime.UtcDateTime, WritePrecision.Ns);
                        writeApi.WritePoint(pointData);
                    }

                    break;
                }

            case MetricType.DoubleSumNonMonotonic:
                {
                    foreach (var dataPoint in metric.GetMetricPoints())
                    {
                        var pointData = PointData
                            .Measurement(measurement)
                            .Field("gauge", dataPoint.GetSumDouble())
                            .Tags(dataPoint.Tags)
                            .Tags(resource?.Attributes)
                            .Timestamp(dataPoint.EndTime.UtcDateTime, WritePrecision.Ns);
                        writeApi.WritePoint(pointData);
                    }

                    break;
                }

            case MetricType.Histogram:
                foreach (var dataPoint in metric.GetMetricPoints())
                {
                    var pointData = PointData
                        .Measurement(measurement)
                        .Field("count", dataPoint.GetHistogramCount())
                        .Field("sum", dataPoint.GetHistogramSum())
                        .Tags(dataPoint.Tags)
                        .Tags(resource?.Attributes)
                        .Timestamp(dataPoint.EndTime.UtcDateTime, WritePrecision.Ns);

                    if (dataPoint.TryGetHistogramMinMaxValues(out var min, out var max))
                    {
                        pointData = pointData.Field("min", min)
                            .Field("max", max);
                    }

                    foreach (var histogramBucket in dataPoint.GetHistogramBuckets())
                    {
                        var boundFieldKey = double.IsPositiveInfinity(histogramBucket.ExplicitBound)
                            ? "+Inf"
                            : histogramBucket.ExplicitBound.ToString("F", CultureInfo.InvariantCulture);
                        pointData = pointData.Field(boundFieldKey, histogramBucket.BucketCount);
                    }

                    writeApi.WritePoint(pointData);
                }

                break;
            case MetricType.ExponentialHistogram:
                break;
            default:
                break;
        }
    }
}
