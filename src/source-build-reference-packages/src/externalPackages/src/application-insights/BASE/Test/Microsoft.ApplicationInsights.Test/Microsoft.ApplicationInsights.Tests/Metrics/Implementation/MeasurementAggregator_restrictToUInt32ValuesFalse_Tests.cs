using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;

using System.Diagnostics;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.ApplicationInsights.Metrics.Extensibility;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;

using Microsoft.ApplicationInsights.Metrics.TestUtility;
using System.Globalization;
using System.Threading;

using CycleKind = Microsoft.ApplicationInsights.Metrics.Extensibility.MetricAggregationCycleKind;
using System.Linq;

namespace Microsoft.ApplicationInsights.Metrics
{
    /// <summary />
    [TestClass]
    public class MeasurementAggregator_restrictToUInt32ValuesFalse_Tests
    {
        /// <summary />
        [TestMethod]
        public void Ctor()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new MeasurementAggregator(configuration: null, dataSeries: null, aggregationCycleKind: CycleKind.Custom));

            {
                var aggregator = new MeasurementAggregator(
                                                new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false),
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
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);

                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 0, sum: 0.0, max: 0.0, min: 0.0, stdDev: 0.0, timestamp: default(DateTimeOffset), periodMs: periodMillis);
            }
            {
                // Zero value:
                var aggregator = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);

                aggregator.TrackValue(0);

                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 1, sum: 0.0, max: 0.0, min: 0.0, stdDev: 0.0, timestamp: default(DateTimeOffset), periodMs: periodMillis);
            }
            {
                // Non zero value:
                var aggregator = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);

                aggregator.TrackValue(-42);

                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 1, sum: -42.0, max: -42.0, min: -42.0, stdDev: 0.0, timestamp: default(DateTimeOffset), periodMs: periodMillis);
            }
            {
                // Two values:
                var aggregator = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);

                aggregator.TrackValue(-42);
                aggregator.TrackValue(18);

                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 2, sum: -24.0, max: 18.0, min: -42.0, stdDev: 30.0, timestamp: default(DateTimeOffset), periodMs: periodMillis);
            }
            {
                // 3 values:
                var aggregator = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);
                aggregator.TrackValue(1800000);
                aggregator.TrackValue(0);
                aggregator.TrackValue(-4200000);

                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 3, sum: -2400000.0, max: 1800000.0, min: -4200000.0, stdDev: 2513961.018, timestamp: default(DateTimeOffset), periodMs: periodMillis);
            }
            {
                // NaNs:
                var aggregator = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);
                aggregator.TrackValue(Double.NaN);
                aggregator.TrackValue(1);
                aggregator.TrackValue(Double.NaN);

                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 1, sum: 1, max: 1, min: 1, stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodMillis);
            }
            {
                // Infinity:
                var aggregator = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);

                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 0, sum: 0, max: 0, min: 0, stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodMillis);

                aggregator.TrackValue(1);

                aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 1, sum: 1, max: 1, min: 1, stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodMillis);

                aggregator.TrackValue(0.5);

                aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 2, sum: 1.5, max: 1, min: 0.5, stdDev: 0.25, timestamp: default(DateTimeOffset), periodMs: periodMillis);

                aggregator.TrackValue(Double.PositiveInfinity);

                aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 3, sum: Double.MaxValue, max: Double.MaxValue, min: 0.5, stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodMillis);

                aggregator.TrackValue(Int32.MinValue);

                aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 4, sum: Double.MaxValue, max: Double.MaxValue, min: Int32.MinValue, stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodMillis);

                aggregator.TrackValue(Double.PositiveInfinity);

                aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 5, sum: Double.MaxValue, max: Double.MaxValue, min: Int32.MinValue, stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodMillis);

                aggregator.TrackValue(Double.NegativeInfinity);

                aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 6, sum: 0.0, max: Double.MaxValue, min: -Double.MaxValue, stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodMillis);

                aggregator.TrackValue(11);

                aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 7, sum: 0.0, max: Double.MaxValue, min: -Double.MaxValue, stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodMillis);
            }
            {
                // Very large numbers:
                var aggregator = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);

                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 0, sum: 0, max: 0, min: 0, stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodMillis);

                aggregator.TrackValue(Math.Exp(300));

                aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 1, sum: Math.Exp(300), max: Math.Exp(300), min: Math.Exp(300), stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodMillis);

                aggregator.TrackValue(-2 * Math.Exp(300));
                double minus2exp200 = -2 * Math.Exp(300);

                aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 2, sum: -Math.Exp(300), max: Math.Exp(300), min: minus2exp200, stdDev: 2.91363959286188000E+130, timestamp: default(DateTimeOffset), periodMs: periodMillis);

                aggregator.TrackValue(Math.Exp(300));

                aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 3, sum: 0, max: Math.Exp(300), min: minus2exp200, stdDev: 2.74700575206167000E+130, timestamp: default(DateTimeOffset), periodMs: periodMillis);

                aggregator.TrackValue(Math.Exp(700));

                aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 4, sum: Math.Exp(700), max: Math.Exp(700), min: minus2exp200, stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodMillis);

                aggregator.TrackValue(Double.MaxValue);

                aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 5, sum: Double.MaxValue, max: Double.MaxValue, min: minus2exp200, stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodMillis);

                aggregator.TrackValue(Double.MaxValue);

                aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 6, sum: Double.MaxValue, max: Double.MaxValue, min: minus2exp200, stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodMillis);

                aggregator.TrackValue(11);

                aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 7, sum: Double.MaxValue, max: Double.MaxValue, min: minus2exp200, stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodMillis);

                aggregator.TrackValue(-Double.MaxValue);

                aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 8, sum: Double.MaxValue, max: Double.MaxValue, min: -Double.MaxValue, stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodMillis);

                aggregator.TrackValue(-Double.PositiveInfinity);

                aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 9, sum: 0, max: Double.MaxValue, min: -Double.MaxValue, stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodMillis);
            }
            {
                // Large number of small values:
                var aggregator = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);

                for (int i = 0; i < 100000; i++)
                {
                    for (double v = 0; v <= 1.0 || Math.Abs(1.0 - v) < TestUtil.MaxAllowedPrecisionError; v += 0.01)
                    {
                        aggregator.TrackValue(v);
                    }
                }

                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 10100000, sum: 5050000, max: 1, min: 0, stdDev: 0.29154759474226500, timestamp: default(DateTimeOffset), periodMs: periodMillis);
            }
            {
                // Large number of large values:
                var aggregator = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);

                for (int i = 0; i < 100000; i++)
                {
                    for (double v = 0; v <= 300000.0 || Math.Abs(300000.0 - v) < TestUtil.MaxAllowedPrecisionError; v += 3000)
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
                                                new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false),
                                                dataSeries: null,
                                                aggregationCycleKind: CycleKind.Custom);

            aggregator.TrackValue(null);

            Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue((object) (Boolean) true) );

            MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 0, sum: 0.0, max: 0.0, min: 0.0, stdDev: 0.0, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            aggregator.TrackValue((object) (SByte) (0-1));

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 1, sum: -1, max: -1, min: -1, stdDev: 0.0, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            aggregator.TrackValue((object) (Byte) 2);

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 2, sum: 1, max: 2, min: -1, stdDev: 1.5, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            aggregator.TrackValue((object) (Int16) (0-3));

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 3, sum: -2, max: 2, min: -3, stdDev: 2.05480466765633, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            aggregator.TrackValue((object) (UInt16) 4);

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 4, sum: 2, max: 4, min: -3, stdDev: 2.69258240356725, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            aggregator.TrackValue((object) (Int32) (0-5));

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 5, sum: -3, max: 4, min: -5, stdDev: 3.26190128606002, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            aggregator.TrackValue((object) (UInt32) 6);

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 6, sum: 3, max: 6, min: -5, stdDev: 3.86221007541882, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            aggregator.TrackValue((object) (Int64) (0-7));

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 7, sum: -4, max: 6, min: -7, stdDev: 4.43547848464572, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            aggregator.TrackValue((object) (UInt64) 8);

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 8, sum: 4, max: 8, min: -7, stdDev: 5.02493781056044, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue((object) (IntPtr) 0xFF) );
            Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue((object) (UIntPtr) 0xFF) );
            Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue((object) (Char) 'x') );

            aggregator.TrackValue((object) (Single) (0f-9.0f));

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 9, sum: -5, max: 8, min: -9, stdDev: 5.59982363037962000, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            aggregator.TrackValue((object) (Double) 10.0);

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 10, sum: 5, max: 10, min: -9, stdDev: 6.18465843842649000, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            aggregator.TrackValue("-11");

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 11, sum: -6, max: 10, min: -11, stdDev: 6.76036088821026, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            aggregator.TrackValue("12.00");

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 12, sum: 6, max: 12, min: -11, stdDev: 7.34279692397023, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            aggregator.TrackValue("-1.300E+01");

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 13, sum: -7, max: 12, min: -13, stdDev: 7.91896831484996, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            aggregator.TrackValue("  +14. ");

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 14, sum: 7, max: 14, min: -13, stdDev: 8.5, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue("fifteen") );
            Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue("") );
            Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue("foo-bar") );

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 14, sum: 7, max: 14, min: -13, stdDev: 8.5, timestamp: default(DateTimeOffset), periodMs: periodMillis);
           
        }

        /// <summary />
        [TestMethod]
        public void TrackValueConcurrently()
        {
            var endTS = new DateTimeOffset(2017, 9, 25, 17, 1, 0, TimeSpan.FromHours(-8));
            long periodMillis = (long) (endTS - default(DateTimeOffset)).TotalMilliseconds;

            var aggregator = new MeasurementAggregator(
                                                new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false),
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

        private static void ValidateNumericAggregateValues(MetricAggregate aggregate, int count, double sum, double max, double min, double stdDev, DateTimeOffset timestamp, long periodMs)
        {
            TestUtil.ValidateNumericAggregateValues(aggregate, "", "null", count, sum, max, min, stdDev, timestamp, periodMs, "Microsoft.Azure.Measurement");
        }

        /// <summary />
        [TestMethod]
        public void CreateAggregateUnsafe()
        {
            var aggregationManager = new MetricAggregationManager();
            var seriesConfig = new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false);

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
                                                new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false),
                                                dataSeries: null,
                                                aggregationCycleKind: CycleKind.Custom);

            var accumulatorAggregator = new MetricSeriesConfigurationForTestingAccumulatorBehavior.Aggregator(
                                                new MetricSeriesConfigurationForTestingAccumulatorBehavior(),
                                                dataSeries: null,
                                                aggregationCycleKind: CycleKind.Custom);


            CommonSimpleDataSeriesAggregatorTests.TryRecycle_NonpersistentAggregator(measurementAggregator);
            CommonSimpleDataSeriesAggregatorTests.TryRecycle_PersistentAggregator(accumulatorAggregator);
        }

        /// <summary />
        [TestMethod]
        public void GetDataSeries()
        {
            var aggregationManager = new MetricAggregationManager();
            var seriesConfig = new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false);
            var metric = new MetricSeries(aggregationManager, new MetricIdentifier("Cows Sold"), null, seriesConfig);

            var aggregatorForConcreteSeries = new MeasurementAggregator(
                                                    (MetricSeriesConfigurationForMeasurement) metric.GetConfiguration(),
                                                    dataSeries: metric,
                                                    aggregationCycleKind: CycleKind.Custom);

            var aggregatorForNullSeries = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false),
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
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);

                CommonSimpleDataSeriesAggregatorTests.Reset_NonpersistentAggregator(aggregator, aggregateKindMoniker: "Microsoft.Azure.Measurement");
            }
            {
                var aggregator = new MetricSeriesConfigurationForTestingAccumulatorBehavior.Aggregator(
                                                    new MetricSeriesConfigurationForTestingAccumulatorBehavior(),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);

                CommonSimpleDataSeriesAggregatorTests.Reset_PersistentAggregator(aggregator, aggregateKindMoniker: "Microsoft.Azure.AccumulatorForTesting");
            }

        }

        /// <summary />
        [TestMethod]
        public void CompleteAggregation()
        {
            var aggregationManager = new MetricAggregationManager();

            var mesurementConfig = new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false);
            var measurementMetric = new MetricSeries(aggregationManager, new MetricIdentifier("Animal Metrics", "Cows Sold"), null, mesurementConfig);

            var measurementAggregator = new MeasurementAggregator(
                                                    (MetricSeriesConfigurationForMeasurement) measurementMetric.GetConfiguration(),
                                                    measurementMetric,
                                                    CycleKind.Custom);

            var accumulatorConfig = new MetricSeriesConfigurationForTestingAccumulatorBehavior();
            var accumulatorMetric = new MetricSeries(aggregationManager, new MetricIdentifier("Animal Metrics", "Cows Sold"), null, accumulatorConfig);

            var accumulatorAggregator = new MetricSeriesConfigurationForTestingAccumulatorBehavior.Aggregator(
                                                    (MetricSeriesConfigurationForTestingAccumulatorBehavior) accumulatorMetric.GetConfiguration(),
                                                    accumulatorMetric,
                                                    CycleKind.Custom);

            CommonSimpleDataSeriesAggregatorTests.CompleteAggregation_NonpersistentAggregator(measurementAggregator);
            CommonSimpleDataSeriesAggregatorTests.CompleteAggregation_PersistentAggregator(accumulatorAggregator);
        }
    }
}
