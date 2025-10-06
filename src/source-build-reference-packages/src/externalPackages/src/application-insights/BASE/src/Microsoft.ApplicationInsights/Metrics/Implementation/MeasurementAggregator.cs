namespace Microsoft.ApplicationInsights.Metrics
{
    using System;
    using System.Runtime.CompilerServices;
    using Microsoft.ApplicationInsights.Metrics.Extensibility;

    internal sealed class MeasurementAggregator : MetricSeriesAggregatorBase<double>, IMetricSeriesAggregator
    {
        private static readonly Func<MetricValuesBufferBase<double>> MetricValuesBufferFactory = () => new MetricValuesBuffer_Double(capacity: 500);

        private readonly object updateLock = new object();

        private readonly bool restrictToUInt32Values;

        private readonly Data data = new Data();

        public MeasurementAggregator(MetricSeriesConfigurationForMeasurement configuration, MetricSeries dataSeries, MetricAggregationCycleKind aggregationCycleKind)
            : base(MetricValuesBufferFactory, configuration, dataSeries, aggregationCycleKind)
        {
            Util.ValidateNotNull(configuration, nameof(configuration));

            this.restrictToUInt32Values = configuration.RestrictToUInt32Values;
            this.ResetAggregate();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override double ConvertMetricValue(double metricValue)
        {
            if (this.restrictToUInt32Values)
            {
                return Util.RoundAndValidateValue(metricValue);
            }
            else
            {
                return metricValue;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override double ConvertMetricValue(object metricValue)
        {
            if (metricValue == null)
            {
                return Double.NaN;
            }

            double value = Util.ConvertToDoubleValue(metricValue);
            return this.ConvertMetricValue(value);
        }

        protected override MetricAggregate CreateAggregate(DateTimeOffset periodEnd)
        {
            int count;
            double sum, min, max, stdDev;

            lock (this.updateLock)
            {
                count = this.data.Count;
                sum = this.data.Sum;
                min = 0.0;
                max = 0.0;
                stdDev = 0.0;

                if (count > 0)
                {
                    min = this.data.Min;
                    max = this.data.Max;

                    if (Double.IsInfinity(this.data.SumOfSquares) || Double.IsInfinity(sum))
                    {
                        stdDev = Double.NaN;
                    }
                    else
                    {
                        double mean = sum / count;
                        double variance = (this.data.SumOfSquares / count) - (mean * mean);
                        stdDev = Math.Sqrt(variance);
                    }
                }
            }

            sum = Util.EnsureConcreteValue(sum);

            if (count > 0)
            {
                min = Util.EnsureConcreteValue(min);
                max = Util.EnsureConcreteValue(max);
                stdDev = Util.EnsureConcreteValue(stdDev);
            }

            MetricAggregate aggregate = new MetricAggregate(
                                                this.DataSeries?.MetricIdentifier.MetricNamespace ?? String.Empty,
                                                this.DataSeries?.MetricIdentifier.MetricId ?? Util.NullString,
                                                MetricSeriesConfigurationForMeasurement.Constants.AggregateKindMoniker);

            aggregate.Data[MetricSeriesConfigurationForMeasurement.Constants.AggregateKindDataKeys.Count] = count;
            aggregate.Data[MetricSeriesConfigurationForMeasurement.Constants.AggregateKindDataKeys.Sum] = sum;
            aggregate.Data[MetricSeriesConfigurationForMeasurement.Constants.AggregateKindDataKeys.Min] = min;
            aggregate.Data[MetricSeriesConfigurationForMeasurement.Constants.AggregateKindDataKeys.Max] = max;
            aggregate.Data[MetricSeriesConfigurationForMeasurement.Constants.AggregateKindDataKeys.StdDev] = stdDev;

            this.AddInfo_Timing_Dimensions_Context(aggregate, periodEnd);

            return aggregate;
        }

        protected override void ResetAggregate()
        {
            lock (this.updateLock)
            {
                this.data.Count = 0;
                this.data.Min = Double.MaxValue;
                this.data.Max = Double.MinValue;
                this.data.Sum = 0.0;
                this.data.SumOfSquares = 0.0;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062", Justification = "buffer is validated by base")]
        protected override object UpdateAggregate_Stage1(MetricValuesBufferBase<double> buffer, int minFlushIndex, int maxFlushIndex)
        {
            Data bufferData = new Data();
            bufferData.Count = 0;
            bufferData.Min = Double.MaxValue;
            bufferData.Max = Double.MinValue;
            bufferData.Sum = 0.0;
            bufferData.SumOfSquares = 0.0;

            for (int index = minFlushIndex; index <= maxFlushIndex; index++)
            {
                double metricValue = buffer.GetAndResetValue(index);

                if (Double.IsNaN(metricValue))
                {
                    continue;
                }

                bufferData.Count++;
                bufferData.Max = (metricValue > bufferData.Max) ? metricValue : bufferData.Max;
                bufferData.Min = (metricValue < bufferData.Min) ? metricValue : bufferData.Min;
                bufferData.Sum += metricValue;
                bufferData.SumOfSquares += metricValue * metricValue;
            }

            if (this.restrictToUInt32Values)
            {
                bufferData.Max = Math.Round(bufferData.Max);
                bufferData.Min = Math.Round(bufferData.Min);
            }

            return bufferData;
        }

        protected override void UpdateAggregate_Stage2(object stage1Result)
        {
            Data bufferData = (Data)stage1Result;

            if (bufferData.Count == 0)
            {
                return;
            }

            // Take a lock and update the aggregate:
            lock (this.updateLock)
            {
                this.data.Count += bufferData.Count;
                this.data.Max = (bufferData.Max > this.data.Max) ? bufferData.Max : this.data.Max;
                this.data.Min = (bufferData.Min < this.data.Min) ? bufferData.Min : this.data.Min;
                this.data.Sum += bufferData.Sum;
                this.data.SumOfSquares += bufferData.SumOfSquares;

                if (this.restrictToUInt32Values)
                {
                    this.data.Sum = Math.Round(this.data.Sum);
                    this.data.SumOfSquares = Math.Round(this.data.SumOfSquares);
                }
            }
        }

#pragma warning disable SA1401 // Field must be private
        private class Data
        {
            public int Count;
            public double Min;
            public double Max;
            public double Sum;
            public double SumOfSquares;
        }
#pragma warning restore SA1401 // Field must be private
    }
}
