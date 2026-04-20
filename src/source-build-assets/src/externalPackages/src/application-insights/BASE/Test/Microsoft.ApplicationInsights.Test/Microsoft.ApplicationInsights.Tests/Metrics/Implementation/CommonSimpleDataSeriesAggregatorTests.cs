using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.ApplicationInsights.Metrics.Extensibility;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;

using Microsoft.ApplicationInsights.Metrics.TestUtility;
using System.Globalization;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using System.Linq;

namespace Microsoft.ApplicationInsights.Metrics
{
    /// <summary />
    internal static class CommonSimpleDataSeriesAggregatorTests
    {
        public static void CreateAggregateUnsafe(
                                        IMetricSeriesAggregator aggregator,
                                        MetricSeries metric,
                                        IEnumerable<KeyValuePair<string, string>> expectedDimensionNamesValues)
        {
            var startTS = new DateTimeOffset(2017, 9, 25, 17, 0, 0, TimeSpan.FromHours(-8));
            var endTS = new DateTimeOffset(2017, 9, 25, 17, 1, 0, TimeSpan.FromHours(-8));

            aggregator.Reset(startTS, valueFilter: null);

            aggregator.TrackValue(42);
            aggregator.TrackValue(43);

            MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
            Assert.IsNotNull(aggregate);

            Assert.AreEqual("Cows Sold", aggregate.MetricId, "aggregate.MetricId mismatch");
            Assert.AreEqual(2, aggregate.Data["Count"], "aggregate.Data[Count] mismatch");
            Assert.AreEqual(85, (double) aggregate.Data["Sum"], TestUtil.MaxAllowedPrecisionError, "aggregate.Data[Sum] mismatch");
            Assert.AreEqual(43, (double) aggregate.Data["Max"], TestUtil.MaxAllowedPrecisionError, "aggregate.Data[Max] mismatch");
            Assert.AreEqual(42, (double) aggregate.Data["Min"], TestUtil.MaxAllowedPrecisionError, "aggregate.Data[Min] mismatch");
            Assert.AreEqual(0.5, (double) aggregate.Data["StdDev"], TestUtil.MaxAllowedPrecisionError, "aggregate.Data[StdDev] mismatch");

            Assert.AreEqual(startTS, aggregate.AggregationPeriodStart, "aggregate.AggregationPeriodStart mismatch");
            Assert.AreEqual(
                        (endTS - startTS).TotalMilliseconds,
                        aggregate.AggregationPeriodDuration.TotalMilliseconds,
                        "aggregate.AggregationPeriodDuration mismatch");

            Assert.AreEqual(expectedDimensionNamesValues.Count(), aggregate.Dimensions.Count);

            foreach(KeyValuePair<string, string> dimNameValue in expectedDimensionNamesValues)
            {
                Assert.IsTrue(aggregate.Dimensions.ContainsKey(dimNameValue.Key), $"missing aggregate.Dimensions[{dimNameValue.Key}]");
                Assert.AreEqual(dimNameValue.Value, aggregate.Dimensions[dimNameValue.Key], $"wrong aggregate.Dimensions[{dimNameValue.Key}]");
            }
        }

        
        public static void TryRecycle_NonpersistentAggregator(IMetricSeriesAggregator measurementAggregator)
        {
            var startTS = new DateTimeOffset(2017, 9, 25, 17, 0, 0, TimeSpan.FromHours(-8));
            var endTS = new DateTimeOffset(2017, 9, 25, 17, 1, 0, TimeSpan.FromHours(-8));

            long periodStringDef = (long) (endTS - default(DateTimeOffset)).TotalMilliseconds;
            long periodStringStart = (long) (endTS - startTS).TotalMilliseconds;

            {
                measurementAggregator.TrackValue(10);

                MetricAggregate aggregate = measurementAggregator.CreateAggregateUnsafe(endTS);
                TestUtil.ValidateNumericAggregateValues(aggregate, ns: "", name: "null", count: 1, sum: 10.0, max: 10.0, min: 10.0, stdDev: 0.0, timestamp: default(DateTimeOffset), periodMs: periodStringDef, aggKindMoniker: "Microsoft.Azure.Measurement");

                measurementAggregator.Reset(startTS, valueFilter: null);

                measurementAggregator.TrackValue(10);
                measurementAggregator.TrackValue(20);

                aggregate = measurementAggregator.CreateAggregateUnsafe(endTS);
                TestUtil.ValidateNumericAggregateValues(aggregate, ns: "", name: "null", count: 2, sum: 30.0, max: 20.0, min: 10.0, stdDev: 5.0, timestamp: startTS, periodMs: periodStringStart, aggKindMoniker: "Microsoft.Azure.Measurement");

                bool canRecycle = measurementAggregator.TryRecycle();

                Assert.IsTrue(canRecycle);

                aggregate = measurementAggregator.CreateAggregateUnsafe(endTS);
                TestUtil.ValidateNumericAggregateValues(aggregate, ns: "", name: "null", count: 0, sum: 0, max: 0, min: 0, stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodStringDef, aggKindMoniker: "Microsoft.Azure.Measurement");

                canRecycle = measurementAggregator.TryRecycle();

                Assert.IsTrue(canRecycle);

                aggregate = measurementAggregator.CreateAggregateUnsafe(endTS);
                TestUtil.ValidateNumericAggregateValues(aggregate, ns: "", name: "null", count: 0, sum: 0, max: 0, min: 0, stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodStringDef, aggKindMoniker: "Microsoft.Azure.Measurement");
            }
        }

        
        public static void TryRecycle_PersistentAggregator(IMetricSeriesAggregator accumulatorAggregator)
        {
            var startTS = new DateTimeOffset(2017, 9, 25, 17, 0, 0, TimeSpan.FromHours(-8));
            var endTS = new DateTimeOffset(2017, 9, 25, 17, 1, 0, TimeSpan.FromHours(-8));

            long periodStringDef = (long) (endTS - default(DateTimeOffset)).TotalMilliseconds;
            long periodStringStart = (long) (endTS - startTS).TotalMilliseconds;

            {
                accumulatorAggregator.TrackValue(10);

                MetricAggregate aggregate = accumulatorAggregator.CreateAggregateUnsafe(endTS);
                TestUtil.ValidateNumericAggregateValues(aggregate, ns: "", name: "null", count: 0, sum: 10.0, max: 10.0, min: 10.0, stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodStringDef, aggKindMoniker: "Microsoft.Azure.AccumulatorForTesting");

                accumulatorAggregator.Reset(startTS, valueFilter: null);

                accumulatorAggregator.TrackValue(0);
                accumulatorAggregator.TrackValue(10);
                accumulatorAggregator.TrackValue(20);

                aggregate = accumulatorAggregator.CreateAggregateUnsafe(endTS);
                TestUtil.ValidateNumericAggregateValues(aggregate, ns: "", name: "null", count: 0, sum: 30.0, max: 30.0, min: 0.0, stdDev: 0, timestamp: startTS, periodMs: periodStringStart, aggKindMoniker: "Microsoft.Azure.AccumulatorForTesting");

                bool canRecycle = accumulatorAggregator.TryRecycle();

                Assert.IsFalse(canRecycle);

                aggregate = accumulatorAggregator.CreateAggregateUnsafe(endTS);
                TestUtil.ValidateNumericAggregateValues(aggregate, ns: "", name: "null", count: 2, sum: 30.0, max: 30.0, min: 0.0, stdDev: 5.0, timestamp: startTS, periodMs: periodStringStart, aggKindMoniker: "Microsoft.Azure.AccumulatorForTesting");
            }
        }


        public static void GetDataSeries(IMetricSeriesAggregator aggregatorForConcreteSeries, IMetricSeriesAggregator aggregatorForNullSeries, MetricSeries series)
        {
            Assert.IsNotNull(aggregatorForConcreteSeries.DataSeries);
            Assert.AreSame(series, aggregatorForConcreteSeries.DataSeries);

            Assert.IsNull(aggregatorForNullSeries.DataSeries);
        }

        public static void Reset_NonpersistentAggregator(IMetricSeriesAggregator aggregator, string aggregateKindMoniker)
        {
            Assert.AreEqual(MetricConfigurations.Common.Measurement().Constants().AggregateKindMoniker, aggregateKindMoniker);

            var startTS = new DateTimeOffset(2017, 9, 25, 17, 0, 0, TimeSpan.FromHours(-8));
            var endTS = new DateTimeOffset(2017, 9, 25, 17, 1, 0, TimeSpan.FromHours(-8));

            long periodStringDef = (long) (endTS - default(DateTimeOffset)).TotalMilliseconds;
            long periodStringStart = (long) (endTS - startTS).TotalMilliseconds;

            int filterInvocationsCount = 0;

            MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
            TestUtil.ValidateNumericAggregateValues(aggregate, ns: "", name: "null", count: 0, sum: 0, max: 0, min: 0, stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodStringDef, aggKindMoniker: aggregateKindMoniker);
            
            aggregator.Reset(startTS, valueFilter: null);

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            TestUtil.ValidateNumericAggregateValues(aggregate, ns: "", name: "null", count: 0, sum: 0, max: 0, min: 0, stdDev: 0, timestamp: startTS, periodMs: periodStringStart, aggKindMoniker: aggregateKindMoniker);

            aggregator.TrackValue(10);
            aggregator.TrackValue(20);

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            TestUtil.ValidateNumericAggregateValues(aggregate, ns: "", name: "null", count: 2, sum: 30.0, max: 20.0, min: 10.0, stdDev: 5.0, timestamp: startTS, periodMs: periodStringStart, aggKindMoniker: aggregateKindMoniker);
            Assert.AreEqual(0, filterInvocationsCount);

            aggregator.Reset(default(DateTimeOffset), valueFilter: new CustomDoubleValueFilter( (s, v) => { filterInvocationsCount++; return true; } ));

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            TestUtil.ValidateNumericAggregateValues(aggregate, ns: "", name: "null", count: 0, sum: 0, max: 0, min: 0, stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodStringDef, aggKindMoniker: aggregateKindMoniker);

            aggregator.TrackValue(100);
            aggregator.TrackValue(200);

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            TestUtil.ValidateNumericAggregateValues(aggregate, ns: "", name: "null", count: 2, sum: 300, max: 200, min: 100, stdDev: 50, timestamp: default(DateTimeOffset), periodMs: periodStringDef, aggKindMoniker: aggregateKindMoniker);
            Assert.AreEqual(2, filterInvocationsCount);

            aggregator.Reset(startTS, valueFilter: new CustomDoubleValueFilter((s, v) => { filterInvocationsCount++; return false; }));

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            TestUtil.ValidateNumericAggregateValues(aggregate, ns: "", name: "null", count: 0, sum: 0, max: 0, min: 0, stdDev: 0, timestamp: startTS, periodMs: periodStringStart, aggKindMoniker: aggregateKindMoniker);

            aggregator.TrackValue(100);
            aggregator.TrackValue(200);

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            TestUtil.ValidateNumericAggregateValues(aggregate, ns: "", name: "null", count: 0, sum: 0, max: 0, min: 0, stdDev: 0, timestamp: startTS, periodMs: periodStringStart, aggKindMoniker: aggregateKindMoniker);
            Assert.AreEqual(4, filterInvocationsCount);

            aggregator.Reset(startTS, valueFilter: new CustomDoubleValueFilter((s, v) => { filterInvocationsCount++; return true; }));

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            TestUtil.ValidateNumericAggregateValues(aggregate, ns: "", name: "null", count: 0, sum: 0, max: 0, min: 0, stdDev: 0, timestamp: startTS, periodMs: periodStringStart, aggKindMoniker: aggregateKindMoniker);

            aggregator.TrackValue(100);
            aggregator.TrackValue(200);

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            TestUtil.ValidateNumericAggregateValues(aggregate, ns: "", name: "null", count: 2, sum: 300, max: 200, min: 100, stdDev: 50, timestamp: startTS, periodMs: periodStringStart, aggKindMoniker: aggregateKindMoniker);
            Assert.AreEqual(6, filterInvocationsCount);
        }

        public static void Reset_PersistentAggregator(IMetricSeriesAggregator aggregator, string aggregateKindMoniker)
        {
            Assert.AreEqual(MetricSeriesConfigurationForTestingAccumulatorBehavior.Constants.AggregateKindMoniker, aggregateKindMoniker);

            var startTS = new DateTimeOffset(2017, 9, 25, 17, 0, 0, TimeSpan.FromHours(-8));
            var endTS = new DateTimeOffset(2017, 9, 25, 17, 1, 0, TimeSpan.FromHours(-8));

            long periodStringDef = (long) (endTS - default(DateTimeOffset)).TotalMilliseconds;
            long periodStringStart = (long) (endTS - startTS).TotalMilliseconds;

            int filterInvocationsCount = 0;

            MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
            Assert.IsNull(aggregate); 

            aggregator.Reset(startTS, valueFilter: null);

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            Assert.IsNull(aggregate);

            aggregator.TrackValue(10);
            aggregator.TrackValue(20);

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            TestUtil.ValidateNumericAggregateValues(aggregate, ns: "", name: "null", count: 0, sum: 30.0, max: 30, min: 10.0, stdDev: 5.0, timestamp: startTS, periodMs: periodStringStart, aggKindMoniker: aggregateKindMoniker);
            Assert.AreEqual(0, filterInvocationsCount);

            aggregator.Reset(default(DateTimeOffset), valueFilter: new CustomDoubleValueFilter((s, v) => { filterInvocationsCount++; return true; }));

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            Assert.IsNull(aggregate);

            aggregator.TrackValue(100);
            aggregator.TrackValue(200);

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            TestUtil.ValidateNumericAggregateValues(aggregate, ns: "", name: "null", count: 0, sum: 300, max: 300, min: 100, stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodStringDef, aggKindMoniker: aggregateKindMoniker);
            Assert.AreEqual(2, filterInvocationsCount);

            aggregator.Reset(startTS, valueFilter: new CustomDoubleValueFilter((s, v) => { filterInvocationsCount++; return false; }));

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            Assert.IsNull(aggregate);

            aggregator.TrackValue(100);
            aggregator.TrackValue(200);

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            Assert.IsNull(aggregate);
            Assert.AreEqual(4, filterInvocationsCount);

            aggregator.Reset(startTS, valueFilter: new CustomDoubleValueFilter((s, v) => { filterInvocationsCount++; return true; }));

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            Assert.IsNull(aggregate);

            aggregator.TrackValue(100);
            aggregator.TrackValue(-200);

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            TestUtil.ValidateNumericAggregateValues(aggregate, ns: "", name: "null", count: 0, sum: -100, max: 100, min: -100, stdDev: 0, timestamp: startTS, periodMs: periodStringStart, aggKindMoniker: aggregateKindMoniker);
            Assert.AreEqual(6, filterInvocationsCount);
        }

        public static void CompleteAggregation_NonpersistentAggregator(IMetricSeriesAggregator measurementAggregator)
        {
            var startTS = new DateTimeOffset(2017, 9, 25, 17, 0, 0, TimeSpan.FromHours(-8));
            var endTS = new DateTimeOffset(2017, 9, 25, 17, 1, 0, TimeSpan.FromHours(-8));
            long periodString = (long) (endTS - startTS).TotalMilliseconds;

            int filterInvocationsCount = 0;

            measurementAggregator.Reset(startTS, valueFilter: new CustomDoubleValueFilter( (s, v) => { filterInvocationsCount++; return true; }) );

            Assert.AreEqual(0, filterInvocationsCount);

            measurementAggregator.TrackValue(1);
            measurementAggregator.TrackValue("2");

            MetricAggregate aggregate = measurementAggregator.CompleteAggregation(endTS);
            TestUtil.ValidateNumericAggregateValues(aggregate, ns: "Animal Metrics", name: "Cows Sold", count: 2, sum: 3, max: 2, min: 1, stdDev: 0.5, timestamp: startTS, periodMs: periodString, aggKindMoniker: "Microsoft.Azure.Measurement");
            Assert.AreEqual(2, filterInvocationsCount);

            measurementAggregator.TrackValue("3");
            measurementAggregator.TrackValue(4);

            aggregate = measurementAggregator.CompleteAggregation(endTS);
            //// In earlier versions we used to reject further values after Complete Aggregation was called. However, that complication is not really necesary.

            //TestUtil.ValidateNumericAggregateValues(aggregate, ns: "Animal Metrics", name: "Cows Sold", count: 2, sum: 3, max: 2, min: 1, stdDev: 0.5, timestamp: startTS, periodMs: periodString, aggKindMoniker: "Microsoft.Azure.Measurement");
            //Assert.AreEqual(2, filterInvocationsCount);

            //aggregate = measurementAggregator.CreateAggregateUnsafe(endTS);
            //TestUtil.ValidateNumericAggregateValues(aggregate, ns: "Animal Metrics", name: "Cows Sold", count: 2, sum: 3, max: 2, min: 1, stdDev: 0.5, timestamp: startTS, periodMs: periodString, aggKindMoniker: "Microsoft.Azure.Measurement");

            TestUtil.ValidateNumericAggregateValues(aggregate, ns: "Animal Metrics", name: "Cows Sold", count: 4, sum: 10, max: 4, min: 1, stdDev: 1.11803398874989, timestamp: startTS, periodMs: periodString, aggKindMoniker: "Microsoft.Azure.Measurement");
            Assert.AreEqual(4, filterInvocationsCount);

            aggregate = measurementAggregator.CreateAggregateUnsafe(endTS);
            TestUtil.ValidateNumericAggregateValues(aggregate, ns: "Animal Metrics", name: "Cows Sold", count: 4, sum: 10, max: 4, min: 1, stdDev: 1.11803398874989, timestamp: startTS, periodMs: periodString, aggKindMoniker: "Microsoft.Azure.Measurement");
        }

        public static void CompleteAggregation_PersistentAggregator(IMetricSeriesAggregator accumulatorAggregator)
        {
            var startTS = new DateTimeOffset(2017, 9, 25, 17, 0, 0, TimeSpan.FromHours(-8));
            var endTS = new DateTimeOffset(2017, 9, 25, 17, 1, 0, TimeSpan.FromHours(-8));
            long periodString = (long) (endTS - startTS).TotalMilliseconds;

            int filterInvocationsCount = 0;

            accumulatorAggregator.Reset(startTS, valueFilter: new CustomDoubleValueFilter((s, v) => { filterInvocationsCount++; return true; }));

            Assert.AreEqual(0, filterInvocationsCount);

            accumulatorAggregator.TrackValue(1);
            accumulatorAggregator.TrackValue("2");

            MetricAggregate aggregate = accumulatorAggregator.CompleteAggregation(endTS);
            TestUtil.ValidateNumericAggregateValues(aggregate, ns: "Animal Metrics", name: "Cows Sold", count: 0, sum: 3, max: 3, min: 1, stdDev: 0, timestamp: startTS, periodMs: periodString, aggKindMoniker: "Microsoft.Azure.AccumulatorForTesting");
            Assert.AreEqual(2, filterInvocationsCount);

            accumulatorAggregator.TrackValue("3");
            accumulatorAggregator.TrackValue(4);

            aggregate = accumulatorAggregator.CompleteAggregation(endTS);
            TestUtil.ValidateNumericAggregateValues(aggregate, ns: "Animal Metrics", name: "Cows Sold", count: 0, sum: 10, max: 10, min: 1, stdDev: 0, timestamp: startTS, periodMs: periodString, aggKindMoniker: "Microsoft.Azure.AccumulatorForTesting");
            Assert.AreEqual(4, filterInvocationsCount);

            aggregate = accumulatorAggregator.CreateAggregateUnsafe(endTS);
            TestUtil.ValidateNumericAggregateValues(aggregate, ns: "Animal Metrics", name: "Cows Sold", count: 0, sum: 10, max: 10, min: 1, stdDev: 0, timestamp: startTS, periodMs: periodString, aggKindMoniker: "Microsoft.Azure.AccumulatorForTesting");

            accumulatorAggregator.TrackValue(-18);
            accumulatorAggregator.TrackValue(2);

            aggregate = accumulatorAggregator.CompleteAggregation(endTS);
            TestUtil.ValidateNumericAggregateValues(aggregate, ns: "Animal Metrics", name: "Cows Sold", count: 0, sum: -6, max: 10, min: -8, stdDev: 0, timestamp: startTS, periodMs: periodString, aggKindMoniker: "Microsoft.Azure.AccumulatorForTesting");
            Assert.AreEqual(6, filterInvocationsCount);
        }

        #region class CustomDoubleValueFilter
        internal class CustomDoubleValueFilter : IMetricValueFilter
        {
            public CustomDoubleValueFilter(Func<MetricSeries, double, bool> filterFunctionDouble)
            {
                FilterFunctionDouble = filterFunctionDouble;
                FilterFunctionObject = InterpretObjectAsDoubleFilter;
            }

            public CustomDoubleValueFilter(
                                Func<MetricSeries, double, bool> filterFunctionDouble,
                                Func<MetricSeries, object, bool> filterFunctionObject)
            {
                FilterFunctionDouble = filterFunctionDouble;
                FilterFunctionObject = filterFunctionObject;
            }

            public Func<MetricSeries, double, bool> FilterFunctionDouble { get; }
            public Func<MetricSeries, object, bool> FilterFunctionObject { get; }

            public bool WillConsume(MetricSeries dataSeries, double metricValue)
            {
                if (FilterFunctionDouble == null)
                {
                    return true;
                }

                return FilterFunctionDouble(dataSeries, metricValue);
            }

            public bool WillConsume(MetricSeries dataSeries, object metricValue)
            {
                if (FilterFunctionObject == null)
                {
                    return true;
                }

                return FilterFunctionObject(dataSeries, metricValue);
            }

            private bool InterpretObjectAsDoubleFilter(MetricSeries dataSeries, object metricValue)
            {
                if (metricValue == null)
                {
                    return WillConsume(dataSeries, Double.NaN);
                }

                double doubleValue;

                string stringValue = metricValue as string;
                if (stringValue != null)
                {
                    if (!Double.TryParse(stringValue, out doubleValue))
                    {
                        doubleValue = Double.NaN;
                    }
                }
                else
                {
                    try
                    {
                        doubleValue = (double) metricValue;
                    }
                    catch (InvalidCastException)
                    {
                        doubleValue = Double.NaN;
                    }
                }

                return WillConsume(dataSeries, doubleValue);
            }
        }
        #endregion class CustomDoubleValueFilter
    }
}
