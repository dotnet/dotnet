namespace Microsoft.ApplicationInsights.Metrics
{
    using System;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Metrics.Extensibility;

    /// <summary>A specific realization of a <see cref="MetricAggregateToApplicationInsightsPipelineConverterBase"/> 
    /// for aggregations of kind "measurement".</summary>
    internal class MeasurementAggregateToApplicationInsightsPipelineConverter : MetricAggregateToApplicationInsightsPipelineConverterBase
    {
        public override string AggregationKindMoniker
        {
            get { return MetricSeriesConfigurationForMeasurement.Constants.AggregateKindMoniker; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062", Justification = "telemetryItem and aggregate are validated by base")]
        protected override void PopulateDataValues(MetricTelemetry telemetryItem, MetricAggregate aggregate)
        {
            telemetryItem.Count = aggregate.GetDataValue<int>(MetricSeriesConfigurationForMeasurement.Constants.AggregateKindDataKeys.Count, 0);
            telemetryItem.Sum = aggregate.GetDataValue<double>(MetricSeriesConfigurationForMeasurement.Constants.AggregateKindDataKeys.Sum, 0.0);
            telemetryItem.Min = aggregate.GetDataValue<double>(MetricSeriesConfigurationForMeasurement.Constants.AggregateKindDataKeys.Min, 0.0);
            telemetryItem.Max = aggregate.GetDataValue<double>(MetricSeriesConfigurationForMeasurement.Constants.AggregateKindDataKeys.Max, 0.0);
            telemetryItem.StandardDeviation = aggregate.GetDataValue<double>(MetricSeriesConfigurationForMeasurement.Constants.AggregateKindDataKeys.StdDev, 0.0);
        }
    }
}
