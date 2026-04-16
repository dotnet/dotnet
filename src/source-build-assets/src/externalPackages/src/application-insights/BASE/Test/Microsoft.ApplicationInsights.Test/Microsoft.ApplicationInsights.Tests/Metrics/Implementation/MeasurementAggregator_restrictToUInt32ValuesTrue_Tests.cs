using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.ApplicationInsights.Metrics.Extensibility;
using Microsoft.ApplicationInsights.Metrics.TestUtility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using CycleKind = Microsoft.ApplicationInsights.Metrics.Extensibility.MetricAggregationCycleKind;

namespace Microsoft.ApplicationInsights.Metrics
{
    /// <summary />
    [TestClass]
    public class MeasurementAggregator_restrictToUInt32ValuesTrue_Tests
    {
        /// <summary />
        [TestMethod]
        public void Ctor()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new MeasurementAggregator(configuration: null, dataSeries: null, aggregationCycleKind: CycleKind.Custom));

            {
                var aggregator = new MeasurementAggregator(
                                                new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: true),
                                                dataSeries: null,
                                                aggregationCycleKind: CycleKind.Custom);
                Assert.IsNotNull(aggregator);
            }
        }

        /// <summary />
        [TestMethod]
        public void TrackValueDouble()
        {
            var endTS = new DateTimeOffset(2017, 9, 25, 17, 1, 0, TimeSpan.FromHours(-8));
            long periodMillis = (long) (endTS - default(DateTimeOffset)).TotalMilliseconds;

            {
                // Empty aggregator:
                var aggregator = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: true),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);

                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 0, sum: 0.0, max: 0.0, min: 0.0, stdDev: 0.0, timestamp: default(DateTimeOffset), periodMs: periodMillis);
            }
            {
                // Zero value:
                var aggregator = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: true),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);

                aggregator.TrackValue(0);

                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 1, sum: 0.0, max: 0.0, min: 0.0, stdDev: 0.0, timestamp: default(DateTimeOffset), periodMs: periodMillis);
            }
            {
                // Values out of range:
                var aggregator = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: true),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);

                Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue(-1) );
                Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue(Int32.MinValue) );
                Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue(Int64.MinValue) );

                Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue(0.1) );
                Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue(0.9) );
                Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue((float) 50.01) );
                Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue(50.99) );

                Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue(((long) UInt32.MaxValue) + (long) 1) );

                aggregator.TrackValue(Double.NaN);
                Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue(Double.PositiveInfinity) );
                Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue(Double.NegativeInfinity) );
                Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue(Double.MaxValue) );

                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 0, sum: 0, max: 0, min: 0, stdDev: 0.0, timestamp: default(DateTimeOffset), periodMs: periodMillis);
            }
            {
                // A single value:
                var aggregator = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);

                aggregator.TrackValue(42);

                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 1, sum: 42.0, max: 42.0, min: 42.0, stdDev: 0.0, timestamp: default(DateTimeOffset), periodMs: periodMillis);
            }
            {
                // Two values:
                var aggregator = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: true),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);

                aggregator.TrackValue(42);
                aggregator.TrackValue(19);

                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 2, sum: 61.0, max: 42.0, min: 19.0, stdDev: 11.5, timestamp: default(DateTimeOffset), periodMs: periodMillis);
            }
            {
                // 3 values:
                var aggregator = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: true),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);
                aggregator.TrackValue(1800000);
                aggregator.TrackValue(0);
                aggregator.TrackValue(4200000);

                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 3, sum: 6000000, max: 4200000.0, min: 0, stdDev: 1720465.05340853, timestamp: default(DateTimeOffset), periodMs: periodMillis);
            }
            {
                // Rounded values:
                var aggregator = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: true),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);
                aggregator.TrackValue(1);

                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 1, sum: 1, max: 1, min: 1, stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodMillis);

                aggregator.TrackValue(-0.0000001);
                aggregator.TrackValue(0.00000001);

                aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 3, sum: 1, max: 1, min: 0, stdDev: 0.471404520791032, timestamp: default(DateTimeOffset), periodMs: periodMillis);

                aggregator.TrackValue(100.0000001);
                aggregator.TrackValue( 99.9999999);

                aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 5, sum: 201, max: 100, min: 0, stdDev: 48.8278608992858, timestamp: default(DateTimeOffset), periodMs: periodMillis);

                aggregator.TrackValue(((double) Int32.MaxValue) - 0.0000001);
                aggregator.TrackValue(((double) Int32.MaxValue) + 0.0000001);

                aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 7, sum: 4294967495, max: 2147483647, min: 0, stdDev: 970134205.051638, timestamp: default(DateTimeOffset), periodMs: periodMillis);

                aggregator.TrackValue(((double) UInt32.MaxValue) - 0.0000001);
                aggregator.TrackValue(((double) UInt32.MaxValue) + 0.0000001);

                aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 9, sum: 12884902085, max: 4294967295, min: 0, stdDev: 1753413037.5015, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            }
            {
                // Very large numbers:
                var aggregator = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: true),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);

                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 0, sum: 0, max: 0, min: 0, stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodMillis);

                aggregator.TrackValue(UInt32.MaxValue - 10000);
                aggregator.TrackValue(UInt32.MaxValue - 1000);
                aggregator.TrackValue(UInt32.MaxValue - 100);
                aggregator.TrackValue(UInt32.MaxValue);

                aggregate = aggregator.CreateAggregateUnsafe(endTS);

                // ToDo!!
                // We need a more numerically stable value for the calculation of StdDev / variance.
                // For example, in this case, the expected value is 4189.49579305195, but we get 4189.4343293576, which is close, but still quite a bit off.
                // Since StdDev is utilized rarely, we leave this for later and put the actual outcome into the test expectation to catch breaks in the future.
                ValidateNumericAggregateValues(aggregate, count: 4, sum: 17179858080, max: 4294967295, min: 4294957295, stdDev: 4189.4343293576, timestamp: default(DateTimeOffset), periodMs: periodMillis);
            }
            {
                // Large number of small values:
                var aggregator = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: true),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);

                for (int i = 0; i < 100000; i++)
                {
                    for (int v = 0; v <= 100; v++)
                    {
                        aggregator.TrackValue(v);
                    }
                }

                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 10100000, sum: 505000000, max: 100, min: 0, stdDev: 29.1547594742265, timestamp: default(DateTimeOffset), periodMs: periodMillis);
            }
            {
                // Large number of large values:
                var aggregator = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: true),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);

                for (int i = 0; i < 100000; i++)
                {
                    for (int v = 0; v <= 300000; v += 3000)
                    {
                        aggregator.TrackValue(v);
                    }
                }

                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 10100000, sum: 1515000000000, max: 300000, min: 0, stdDev: 87464.2784226795, timestamp: default(DateTimeOffset), periodMs: periodMillis);
            }
        }

        /// <summary />
        [TestMethod]
        public void TrackValueObject()
        {
            var endTS = new DateTimeOffset(2017, 9, 25, 17, 1, 0, TimeSpan.FromHours(-8));
            long periodMillis = (long) (endTS - default(DateTimeOffset)).TotalMilliseconds;

            var aggregator = new MeasurementAggregator(
                                                new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: true),
                                                dataSeries: null,
                                                aggregationCycleKind: CycleKind.Custom);

            aggregator.TrackValue(null);

            Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue((object) (Boolean) true) );

            MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 0, sum: 0.0, max: 0.0, min: 0.0, stdDev: 0.0, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue((object) (SByte) (0-1)) );

            aggregator.TrackValue((object) (Byte) 2);

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 1, sum: 2, max: 2, min: 2, stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue((object) (Int16) (0-3)) );

            aggregator.TrackValue((object) (UInt16) 4);

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 2, sum: 6, max: 4, min: 2, stdDev: 1, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue((object) (Int32) (0-5)) );

            aggregator.TrackValue((object) (UInt32) 6);

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 3, sum: 12, max: 6, min: 2, stdDev: 1.63299316185545, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue((object) (Int64) (0-7)) );

            aggregator.TrackValue((object) (UInt64) 8);

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 4, sum: 20, max: 8, min: 2, stdDev: 2.23606797749979, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue((object) (IntPtr) 0xFF) );
            Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue((object) (UIntPtr) 0xFF) );
            Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue((object) (Char) 'x') );

            Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue((object) (Single) (0f-9.0f)) );

            aggregator.TrackValue((object) (Double) 10.0);

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 5, sum: 30, max: 10, min: 2, stdDev: 2.82842712474619, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue("-11") );

            aggregator.TrackValue("12");

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 6, sum: 42, max: 12, min: 2, stdDev: 3.41565025531987, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue("-1.300E+01") );
            Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue("13.5"));

            aggregator.TrackValue("  +14 ");

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 7, sum: 56, max: 14, min: 2, stdDev: 4, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue("fifteen") );
            Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue("") );
            Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue("foo-bar") );

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 7, sum: 56, max: 14, min: 2, stdDev: 4, timestamp: default(DateTimeOffset), periodMs: periodMillis);
        }

        private static void ValidateNumericAggregateValues(MetricAggregate aggregate, int count, double sum, double max, double min, double stdDev, DateTimeOffset timestamp, long periodMs)
        {
            TestUtil.ValidateNumericAggregateValues(aggregate, "", "null", count, sum, max, min, stdDev, timestamp, periodMs, "Microsoft.Azure.Measurement");
        }

        /// <summary />
        [TestMethod]
        public void TrackValueConcurrently()
        {
            var endTS = new DateTimeOffset(2017, 9, 25, 17, 1, 0, TimeSpan.FromHours(-8));
            long periodMillis = (long) (endTS - default(DateTimeOffset)).TotalMilliseconds;

            var aggregator = new MeasurementAggregator(
                                                new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: true),
                                                dataSeries: null,
                                                aggregationCycleKind: CycleKind.Custom);

            List<Task> parallelTasks = new List<Task>();
            s_trackValueConcurrentWorker_Current = 0;
            s_trackValueConcurrentWorker_Max = 0;

            for (int i = 0; i < 100; i++)
            {
                Task t = Task.Run( () => TrackValueConcurrentWorker(aggregator) );
                parallelTasks.Add(t);
            }

            Task.WaitAll(parallelTasks.ToArray());

            Assert.AreEqual(0, s_trackValueConcurrentWorker_Current);
            Assert.IsTrue(
                        90 <= s_trackValueConcurrentWorker_Max,
                        "The local machine has timing characteristics resuling in not enough concurrency. Try re-running or tweaking delays."
                      + $" (s_trackValueConcurrentWorker_Max = {s_trackValueConcurrentWorker_Max})");

            MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);

#if DEBUG
            Trace.WriteLine($"s_countBufferWaitSpinEvents: {MetricSeriesAggregatorBase<double>.countBufferWaitSpinEvents}");
            Trace.WriteLine($"s_countBufferWaitSpinCycles: {MetricSeriesAggregatorBase<double>.countBufferWaitSpinCycles}");
            Trace.WriteLine($"s_timeBufferWaitSpinMillis: {TimeSpan.FromMilliseconds(MetricSeriesAggregatorBase<double>.timeBufferWaitSpinMillis)}");

            Trace.WriteLine($"s_countBufferFlushes: {MetricSeriesAggregatorBase<double>.countBufferFlushes}");
            Trace.WriteLine($"s_countNewBufferObjectsCreated: {MetricSeriesAggregatorBase<double>.countNewBufferObjectsCreated}");
#endif
            ValidateNumericAggregateValues(aggregate, count: 5050000, sum: 757500000000, max: 300000, min: 0, stdDev: 87464.2784226795, timestamp: default(DateTimeOffset), periodMs: periodMillis);
        }

        private static int s_trackValueConcurrentWorker_Current;
        private static int s_trackValueConcurrentWorker_Max;

        private static async Task TrackValueConcurrentWorker(IMetricSeriesAggregator aggregator)
        {
            await Task.Delay(3);

            int currentWorkersAtStart = Interlocked.Increment(ref s_trackValueConcurrentWorker_Current);
            try
            {
                int maxWorkers = Volatile.Read(ref s_trackValueConcurrentWorker_Max);
                while (currentWorkersAtStart > maxWorkers)
                {
                    int prevMaxWorkers = Interlocked.CompareExchange(ref s_trackValueConcurrentWorker_Max, currentWorkersAtStart, maxWorkers);

                    if (prevMaxWorkers == maxWorkers)
                    {
                        break;
                    }
                    else
                    {
                        maxWorkers = Volatile.Read(ref s_trackValueConcurrentWorker_Max);
                    }
                }

                for (int i = 0; i < 500; i++)
                {
                    for (int v = 0; v <= 300000; v += 3000)
                    {
                        aggregator.TrackValue(v);
                        //Thread.Yield();
                        await Task.Delay(0);
                    }

                    await Task.Delay(1);
                }
            }
            finally
            {
                Interlocked.Decrement(ref s_trackValueConcurrentWorker_Current);
            }
        }

        /// <summary />
        [TestMethod]
        public void CreateAggregateUnsafe()
        {
            var aggregationManager = new MetricAggregationManager();
            var seriesConfig = new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: true);

            IEnumerable<KeyValuePair<string, string>> setDimensionNamesValues = new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("Dim 1", "DV1"),
                                                                                                                     new KeyValuePair<string, string>("Dim 2", "DV2"),
                                                                                                                     new KeyValuePair<string, string>("Dim 3", "DV3"),
                                                                                                                     new KeyValuePair<string, string>("Dim 2", "DV2a") };

            IEnumerable<KeyValuePair<string, string>> expectedDimensionNamesValues = new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("Dim 1", "DV1"),
                                                                                                                          new KeyValuePair<string, string>("Dim 2", "DV2a"),
                                                                                                                          new KeyValuePair<string, string>("Dim 3", "DV3") };

            var metric = new MetricSeries(
                                    aggregationManager,
                                    new MetricIdentifier(String.Empty, "Cows Sold", expectedDimensionNamesValues.Select( nv => nv.Key ).ToArray()),
                                    setDimensionNamesValues,
                                    seriesConfig);

            var aggregator = new MeasurementAggregator(
                                                    (MetricSeriesConfigurationForMeasurement) metric.GetConfiguration(),
                                                    metric,
                                                    CycleKind.Custom);

            CommonSimpleDataSeriesAggregatorTests.CreateAggregateUnsafe(aggregator, metric, expectedDimensionNamesValues);
        }

        /// <summary />
        [TestMethod]
        public void TryRecycle()
        {
            var measurementAggregator = new MeasurementAggregator(
                                                new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: true),
                                                dataSeries: null,
                                                aggregationCycleKind: CycleKind.Custom);

            CommonSimpleDataSeriesAggregatorTests.TryRecycle_NonpersistentAggregator(measurementAggregator);
        }

        /// <summary />
        [TestMethod]
        public void GetDataSeries()
        {
            var aggregationManager = new MetricAggregationManager();
            var seriesConfig = new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: true);
            var metric = new MetricSeries(aggregationManager, new MetricIdentifier("Cows Sold"), null, seriesConfig);

            var aggregatorForConcreteSeries = new MeasurementAggregator(
                                                    (MetricSeriesConfigurationForMeasurement) metric.GetConfiguration(),
                                                    dataSeries: metric,
                                                    aggregationCycleKind: CycleKind.Custom);

            var aggregatorForNullSeries = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: true),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);

            CommonSimpleDataSeriesAggregatorTests.GetDataSeries(aggregatorForConcreteSeries, aggregatorForNullSeries, metric);
        }

        /// <summary />
        [TestMethod]
        public void Reset()
        {
            {
                var aggregator = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: true),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);

                CommonSimpleDataSeriesAggregatorTests.Reset_NonpersistentAggregator(aggregator, aggregateKindMoniker: "Microsoft.Azure.Measurement");
            }
        }

        /// <summary />
        [TestMethod]
        public void CompleteAggregation()
        {
            var aggregationManager = new MetricAggregationManager();

            var mesurementConfig = new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: true);
            var measurementMetric = new MetricSeries(aggregationManager, new MetricIdentifier("Animal Metrics", "Cows Sold"), null, mesurementConfig);

            var measurementAggregator = new MeasurementAggregator(
                                                    (MetricSeriesConfigurationForMeasurement) measurementMetric.GetConfiguration(),
                                                    measurementMetric,
                                                    CycleKind.Custom);

            CommonSimpleDataSeriesAggregatorTests.CompleteAggregation_NonpersistentAggregator(measurementAggregator);
        }
    }
}
