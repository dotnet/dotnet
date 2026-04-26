using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Metrics.Extensibility;

namespace Microsoft.ApplicationInsights.Metrics.TestUtility
{
    public class MetricSeriesConfigurationForTestingAccumulatorBehavior : IMetricSeriesConfiguration
    {
        private readonly int _hashCode;

        static MetricSeriesConfigurationForTestingAccumulatorBehavior()
        {
            MetricAggregateToTelemetryPipelineConverters.Registry.Add(
                        typeof(ApplicationInsightsTelemetryPipeline),
                        MetricSeriesConfigurationForTestingAccumulatorBehavior.Constants.AggregateKindMoniker,
                        new MetricSeriesConfigurationForTestingAccumulatorBehavior.AggregateToApplicationInsightsPipelineConverter());
        }

        public MetricSeriesConfigurationForTestingAccumulatorBehavior()
        {
            _hashCode = Util.CombineHashCodes(11);
        }

        public bool RequiresPersistentAggregation
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return true; }
        }

        public IMetricSeriesAggregator CreateNewAggregator(MetricSeries dataSeries, MetricAggregationCycleKind aggregationCycleKind)
        {

            return new MetricSeriesConfigurationForTestingAccumulatorBehavior.Aggregator(this, dataSeries, aggregationCycleKind);
        }

        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                var otherConfig = obj as MetricSeriesConfigurationForTestingAccumulatorBehavior;
                if (otherConfig != null)
                {
                    return Equals(otherConfig);
                }
            }

            return false;
        }

        public bool Equals(IMetricSeriesConfiguration other)
        {
            return Equals((object) other);
        }

        public bool Equals(MetricSeriesConfigurationForTestingAccumulatorBehavior other)
        {
            if (other == null)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public sealed class AggregateKindConstants
        {
            internal static readonly AggregateKindConstants Instance = new AggregateKindConstants();

            private AggregateKindConstants()
            {
            }

            public string AggregateKindMoniker { get { return Constants.AggregateKindMoniker; } }

            public DataKeysConstants AggregateKindDataKeys { get { return DataKeysConstants.Instance; } }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public sealed class DataKeysConstants
            {
                internal static readonly DataKeysConstants Instance = new DataKeysConstants();

                private DataKeysConstants()
                {
                }

                public string Sum { get { return Constants.AggregateKindDataKeys.Sum; } }

                public string Min { get { return Constants.AggregateKindDataKeys.Min; } }

                public string Max { get { return Constants.AggregateKindDataKeys.Max; } }
            }
        }

        public static class Constants
        {
            public const string AggregateKindMoniker = "Microsoft.Azure.AccumulatorForTesting";

            public static class AggregateKindDataKeys
            {
                public const string Sum = "Sum";
                public const string Min = "Min";
                public const string Max = "Max";
            }
        }

        internal class AggregateToApplicationInsightsPipelineConverter : MetricAggregateToApplicationInsightsPipelineConverterBase
        {
            public override string AggregationKindMoniker { get { return MetricSeriesConfigurationForTestingAccumulatorBehavior.Constants.AggregateKindMoniker; } }

            protected override void PopulateDataValues(MetricTelemetry telemetryItem, MetricAggregate aggregate)
            {
                telemetryItem.Count = 1;
                telemetryItem.Sum = aggregate.GetDataValue<double>(MetricSeriesConfigurationForTestingAccumulatorBehavior.Constants.AggregateKindDataKeys.Sum, 0.0);
                telemetryItem.Min = aggregate.GetDataValue<double>(MetricSeriesConfigurationForTestingAccumulatorBehavior.Constants.AggregateKindDataKeys.Min, 0.0);
                telemetryItem.Max = aggregate.GetDataValue<double>(MetricSeriesConfigurationForTestingAccumulatorBehavior.Constants.AggregateKindDataKeys.Max, 0.0);
                telemetryItem.StandardDeviation = null;
            }
        }

        internal sealed class Aggregator : MetricSeriesAggregatorBase<double>
        {
            private static readonly Func<MetricValuesBufferBase<double>> MetricValuesBufferFactory = () => new MetricValuesBuffer_Double(capacity: 500);

            private readonly object _updateLock = new object();

            private double _min;
            private double _max;
            private double _sum;

            public Aggregator(MetricSeriesConfigurationForTestingAccumulatorBehavior configuration, MetricSeries dataSeries, MetricAggregationCycleKind aggregationCycleKind)
                : base(MetricValuesBufferFactory, configuration, dataSeries, aggregationCycleKind)
            {
                Util.ValidateNotNull(configuration, nameof(configuration));
                ResetAggregate();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            protected override double ConvertMetricValue(double metricValue)
            {
                return metricValue;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            protected override double ConvertMetricValue(object metricValue)
            {
                double value = Util.ConvertToDoubleValue(metricValue);
                return ConvertMetricValue(value);
            }

            protected override MetricAggregate CreateAggregate(DateTimeOffset periodEnd)
            {
                double sum, min, max;

                lock (_updateLock)
                {
                    sum = _sum;
                    min = _min;
                    max = _max;
                }

                if (_min > _max)
                {
                    return null;
                }

                sum = Util.EnsureConcreteValue(sum);
                min = Util.EnsureConcreteValue(min);
                max = Util.EnsureConcreteValue(max);

                MetricAggregate aggregate = new MetricAggregate(
                                                    DataSeries?.MetricIdentifier.MetricNamespace ?? String.Empty,
                                                    DataSeries?.MetricIdentifier.MetricId ?? Util.NullString,
                                                    MetricSeriesConfigurationForTestingAccumulatorBehavior.Constants.AggregateKindMoniker);

                aggregate.Data[MetricSeriesConfigurationForTestingAccumulatorBehavior.Constants.AggregateKindDataKeys.Sum] = sum;
                aggregate.Data[MetricSeriesConfigurationForTestingAccumulatorBehavior.Constants.AggregateKindDataKeys.Min] = min;
                aggregate.Data[MetricSeriesConfigurationForTestingAccumulatorBehavior.Constants.AggregateKindDataKeys.Max] = max;

                AddInfo_Timing_Dimensions_Context(aggregate, periodEnd);

                return aggregate;
            }

            protected override void ResetAggregate()
            {
                lock (_updateLock)
                {
                    _min = Double.MaxValue;
                    _max = Double.MinValue;
                    _sum = 0.0;
                }
            }

            protected override object UpdateAggregate_Stage1(MetricValuesBufferBase<double> buffer, int minFlushIndex, int maxFlushIndex)
            {
                lock (_updateLock)
                {
                    for (int index = minFlushIndex; index <= maxFlushIndex; index++)
                    {
                        double metricValue = buffer.GetAndResetValue(index);

                        if (Double.IsNaN(metricValue))
                        {
                            continue;
                        }

                        _sum += metricValue;
                        _max = (_sum > _max) ? _sum : _max;
                        _min = (_sum < _min) ? _sum : _min;
                    }
                }

                return null;
            }

            protected override void UpdateAggregate_Stage2(object stage1Result)
            {
            }
        }
    }
}
