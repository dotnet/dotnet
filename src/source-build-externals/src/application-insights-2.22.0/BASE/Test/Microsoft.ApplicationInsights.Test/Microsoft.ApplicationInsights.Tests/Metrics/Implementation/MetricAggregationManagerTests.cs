using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.ApplicationInsights.Metrics.Extensibility;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Metrics.TestUtility;

namespace Microsoft.ApplicationInsights.Metrics
{
    /// <summary />
    [TestClass]
    public class MetricAggregationManagerTests
    {
        /// <summary />
        [TestMethod]
        public void Ctor()
        {
            var aggregationManager = new MetricAggregationManager();
            Assert.IsNotNull(aggregationManager);
        }

        /// <summary />
        [TestMethod]
        public void DefaultState()
        {
            DateTimeOffset dto = new DateTimeOffset(2017, 10, 2, 17, 5, 0, TimeSpan.FromHours(-7));

            var aggregationManager = new MetricAggregationManager();

            var measurementMetric = new MetricSeries(
                                            aggregationManager,
                                            new MetricIdentifier("Measurement Metric"),
                                            null,
                                            new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

            var accumulatorMetric = new MetricSeries(
                                            aggregationManager,
                                            new MetricIdentifier("Accumulator Metric"),
                                            null,
                                            new MetricSeriesConfigurationForTestingAccumulatorBehavior());

            measurementMetric.TrackValue(1);
            accumulatorMetric.TrackValue(2);
            
            AggregationPeriodSummary defaultPeriod = aggregationManager.StartOrCycleAggregators(MetricAggregationCycleKind.Default, dto, futureFilter: null);
            Assert.IsNotNull(defaultPeriod);
            Assert.IsNotNull(defaultPeriod.NonpersistentAggregates);
            Assert.IsNotNull(defaultPeriod.PersistentAggregates);

            Assert.AreEqual(1, defaultPeriod.NonpersistentAggregates.Count);
            Assert.AreEqual("Measurement Metric", (defaultPeriod.NonpersistentAggregates[0]).MetricId);
            Assert.AreEqual(1, defaultPeriod.NonpersistentAggregates[0].Data["Count"]);
            Assert.AreEqual(1.0, defaultPeriod.NonpersistentAggregates[0].Data["Sum"]);

            Assert.AreEqual(1, defaultPeriod.PersistentAggregates.Count);
            Assert.AreEqual("Accumulator Metric", defaultPeriod.PersistentAggregates[0].MetricId);
            Assert.AreEqual(2.0, defaultPeriod.PersistentAggregates[0].Data["Sum"]);

            AggregationPeriodSummary customPeriod = aggregationManager.StartOrCycleAggregators(MetricAggregationCycleKind.Custom, dto, futureFilter: null);
            Assert.IsNotNull(customPeriod);
            Assert.IsNotNull(customPeriod.NonpersistentAggregates);
            Assert.IsNotNull(customPeriod.PersistentAggregates);

            Assert.AreEqual(0, customPeriod.NonpersistentAggregates.Count);

            Assert.AreEqual(1, customPeriod.PersistentAggregates.Count);
            Assert.AreEqual("Accumulator Metric", customPeriod.PersistentAggregates[0].MetricId);
            Assert.AreEqual(2.0, customPeriod.PersistentAggregates[0].Data["Sum"]);

            AggregationPeriodSummary quickpulsePeriod = aggregationManager.StartOrCycleAggregators(MetricAggregationCycleKind.QuickPulse, dto, futureFilter: null);
            Assert.IsNotNull(quickpulsePeriod);
            Assert.IsNotNull(quickpulsePeriod.NonpersistentAggregates);
            Assert.IsNotNull(quickpulsePeriod.PersistentAggregates);

            Assert.AreEqual(0, quickpulsePeriod.NonpersistentAggregates.Count);

            Assert.AreEqual(1, quickpulsePeriod.PersistentAggregates.Count);
            Assert.AreEqual("Accumulator Metric", quickpulsePeriod.PersistentAggregates[0].MetricId);
            Assert.AreEqual(2.0, quickpulsePeriod.PersistentAggregates[0].Data["Sum"]);
        }

        /// <summary />
        [TestMethod]
        public void StartOrCycleAggregators()
        {
            StartOrCycleAggregatorsTest(MetricAggregationCycleKind.Default, supportsSettingFilters: false);
            StartOrCycleAggregatorsTest(MetricAggregationCycleKind.QuickPulse, supportsSettingFilters: true);
            StartOrCycleAggregatorsTest(MetricAggregationCycleKind.Custom, supportsSettingFilters: true);

            {
                DateTimeOffset dto = new DateTimeOffset(2017, 10, 2, 17, 5, 0, TimeSpan.FromHours(-7));

                var aggregationManager = new MetricAggregationManager();
                Assert.ThrowsException<ArgumentException>( () => aggregationManager.StartOrCycleAggregators((MetricAggregationCycleKind) 42, dto, futureFilter: null) );
            }
        }

        private static void StartOrCycleAggregatorsTest(MetricAggregationCycleKind cycleKind, bool supportsSettingFilters)
        {
            DateTimeOffset dto = new DateTimeOffset(2017, 10, 2, 17, 5, 0, TimeSpan.FromHours(-7));

            var aggregationManager = new MetricAggregationManager();

            var measurementMetric = new MetricSeries(
                                            aggregationManager,
                                            new MetricIdentifier("Measurement Metric"),
                                            null,
                                            new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

            var accumulatorMetric = new MetricSeries(
                                            aggregationManager,
                                            new MetricIdentifier("Accumulator Metric"),
                                            null,
                                            new MetricSeriesConfigurationForTestingAccumulatorBehavior());

            // Cycle once, get nothing:
            AggregationPeriodSummary period = aggregationManager.StartOrCycleAggregators(cycleKind, dto, futureFilter: null);
            Assert.IsNotNull(period);
            Assert.IsNotNull(period.NonpersistentAggregates);
            Assert.IsNotNull(period.PersistentAggregates);

            Assert.AreEqual(0, period.NonpersistentAggregates.Count);
            Assert.AreEqual(0, period.PersistentAggregates.Count);

            // Record something, cycle, check for it:

            measurementMetric.TrackValue(1);
            accumulatorMetric.TrackValue(2);

            period = aggregationManager.StartOrCycleAggregators(cycleKind, dto, futureFilter: null);
            Assert.IsNotNull(period);
            Assert.IsNotNull(period.NonpersistentAggregates);
            Assert.IsNotNull(period.PersistentAggregates);

            Assert.AreEqual(1, period.NonpersistentAggregates.Count);
            Assert.AreEqual(1, period.PersistentAggregates.Count);

            Assert.IsNotNull(period.NonpersistentAggregates[0]);
            Assert.IsNotNull(period.PersistentAggregates[0]);

            Assert.AreEqual("Measurement Metric", period.NonpersistentAggregates[0].MetricId);
            Assert.AreEqual(1, period.NonpersistentAggregates[0].Data["Count"]);
            Assert.AreEqual(1.0, period.NonpersistentAggregates[0].Data["Sum"]);

            Assert.AreEqual("Accumulator Metric", period.PersistentAggregates[0].MetricId);
            Assert.AreEqual(2.0, period.PersistentAggregates[0].Data["Sum"]);

            // Now we should be empty again for non-persistent. Persistent stays:

            period = aggregationManager.StartOrCycleAggregators(cycleKind, dto, futureFilter: null);
            Assert.IsNotNull(period);
            Assert.IsNotNull(period.NonpersistentAggregates);
            Assert.IsNotNull(period.PersistentAggregates);

            Assert.AreEqual(0, period.NonpersistentAggregates.Count);
            Assert.AreEqual(1, period.PersistentAggregates.Count);

            Assert.IsNotNull(period.PersistentAggregates[0]);

            Assert.AreEqual("Accumulator Metric", period.PersistentAggregates[0].MetricId);
            Assert.AreEqual(2.0, period.PersistentAggregates[0].Data["Sum"]);

            // Now set a deny filter. Track. Expect to get nothng.
            // Note: for persistent, values tracked under Deny filter should persist for the future, for non-persistent they are just discarded.

            if (false == supportsSettingFilters)
            {
                Assert.ThrowsException<ArgumentException>(() => aggregationManager.StartOrCycleAggregators(
                                                                                       cycleKind,
                                                                                       dto,
                                                                                       futureFilter: new AcceptAllFilter()));
            }
            else
            { 
                period = aggregationManager.StartOrCycleAggregators(cycleKind, dto, futureFilter: new DenyAllFilter());
                Assert.IsNotNull(period);
                Assert.IsNotNull(period.NonpersistentAggregates);
                Assert.IsNotNull(period.PersistentAggregates);

                measurementMetric.TrackValue(3);
                accumulatorMetric.TrackValue(4);

                period = aggregationManager.StartOrCycleAggregators(cycleKind, dto, futureFilter: null);
                Assert.IsNotNull(period);
                Assert.IsNotNull(period.NonpersistentAggregates);
                Assert.IsNotNull(period.PersistentAggregates);

                Assert.AreEqual(0, period.NonpersistentAggregates.Count);
                Assert.AreEqual(0, period.PersistentAggregates.Count);

                period = aggregationManager.StartOrCycleAggregators(cycleKind, dto, futureFilter: null);
                Assert.IsNotNull(period);
                Assert.IsNotNull(period.NonpersistentAggregates);
                Assert.IsNotNull(period.PersistentAggregates);

                Assert.AreEqual(0, period.NonpersistentAggregates.Count);
                Assert.AreEqual(1, period.PersistentAggregates.Count);

                Assert.IsNotNull(period.PersistentAggregates[0]);

                Assert.AreEqual("Accumulator Metric", period.PersistentAggregates[0].MetricId);
                Assert.AreEqual(6.0, period.PersistentAggregates[0].Data["Sum"]);

                // Validate that deny filter was removed:

                measurementMetric.TrackValue(5);
                accumulatorMetric.TrackValue(6);

                period = aggregationManager.StartOrCycleAggregators(cycleKind, dto, futureFilter: null);
                Assert.IsNotNull(period);
                Assert.IsNotNull(period.NonpersistentAggregates);
                Assert.IsNotNull(period.PersistentAggregates);

                Assert.AreEqual(1, period.NonpersistentAggregates.Count);
                Assert.AreEqual(1, period.PersistentAggregates.Count);

                Assert.IsNotNull(period.PersistentAggregates[0]);
                Assert.IsNotNull(period.NonpersistentAggregates[0]);

                Assert.AreEqual("Accumulator Metric", period.PersistentAggregates[0].MetricId);
                Assert.AreEqual(12.0, period.PersistentAggregates[0].Data["Sum"]);

                Assert.AreEqual("Measurement Metric", period.NonpersistentAggregates[0].MetricId);
                Assert.AreEqual(5.0, period.NonpersistentAggregates[0].Data["Sum"]);
            }
        }

        /// <summary />
        [TestMethod]
        public void StopAggregators()
        {
            DateTimeOffset dto = new DateTimeOffset(2017, 10, 2, 17, 5, 0, TimeSpan.FromHours(-7));

            var aggregationManager = new MetricAggregationManager();

            var measurementMetric = new MetricSeries(
                                            aggregationManager,
                                            new MetricIdentifier("Measurement Metric"),
                                            null,
                                            new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

            var accumulatorMetric = new MetricSeries(
                                            aggregationManager,
                                            new MetricIdentifier("Accumulator Metric"),
                                            null,
                                            new MetricSeriesConfigurationForTestingAccumulatorBehavior());

            // Cannot stop default:

            Assert.ThrowsException<ArgumentException>( () => aggregationManager.StopAggregators(MetricAggregationCycleKind.Default, dto) );

            // Stop cycles that never started:

            AggregationPeriodSummary customPeriod = aggregationManager.StopAggregators(MetricAggregationCycleKind.Custom, dto);
            Assert.IsNotNull(customPeriod);
            Assert.IsNotNull(customPeriod.NonpersistentAggregates);
            Assert.IsNotNull(customPeriod.PersistentAggregates);

            Assert.AreEqual(0, customPeriod.NonpersistentAggregates.Count);
            Assert.AreEqual(0, customPeriod.PersistentAggregates.Count);

            AggregationPeriodSummary quickpulsePeriod = aggregationManager.StopAggregators(MetricAggregationCycleKind.QuickPulse, dto);
            Assert.IsNotNull(quickpulsePeriod);
            Assert.IsNotNull(quickpulsePeriod.NonpersistentAggregates);
            Assert.IsNotNull(quickpulsePeriod.PersistentAggregates);

            Assert.AreEqual(0, quickpulsePeriod.NonpersistentAggregates.Count);
            Assert.AreEqual(0, quickpulsePeriod.PersistentAggregates.Count);

            // Track a value. Stop cycles that never started again. Observe that persistent cycle was active by default:

            measurementMetric.TrackValue(1);
            accumulatorMetric.TrackValue(2);

            customPeriod = aggregationManager.StopAggregators(MetricAggregationCycleKind.Custom, dto);
            Assert.IsNotNull(customPeriod);
            Assert.IsNotNull(customPeriod.NonpersistentAggregates);
            Assert.IsNotNull(customPeriod.PersistentAggregates);

            Assert.AreEqual(0, customPeriod.NonpersistentAggregates.Count);
            Assert.AreEqual(1, customPeriod.PersistentAggregates.Count);
            Assert.AreEqual("Accumulator Metric", customPeriod.PersistentAggregates[0].MetricId);
            Assert.AreEqual(2.0, customPeriod.PersistentAggregates[0].Data["Sum"]);

            quickpulsePeriod = aggregationManager.StopAggregators(MetricAggregationCycleKind.QuickPulse, dto);
            Assert.IsNotNull(quickpulsePeriod);
            Assert.IsNotNull(quickpulsePeriod.NonpersistentAggregates);
            Assert.IsNotNull(quickpulsePeriod.PersistentAggregates);

            Assert.AreEqual(0, quickpulsePeriod.NonpersistentAggregates.Count);
            Assert.AreEqual(1, quickpulsePeriod.PersistentAggregates.Count);
            Assert.AreEqual("Accumulator Metric", quickpulsePeriod.PersistentAggregates[0].MetricId);
            Assert.AreEqual(2.0, quickpulsePeriod.PersistentAggregates[0].Data["Sum"]);

            // Now start cycles, track values and stop them again. Observe that values were tracked:

            aggregationManager.StartOrCycleAggregators(MetricAggregationCycleKind.Custom, dto, futureFilter: null);
            aggregationManager.StartOrCycleAggregators(MetricAggregationCycleKind.QuickPulse, dto, futureFilter: null);

            measurementMetric.TrackValue(3);
            accumulatorMetric.TrackValue(4);

            customPeriod = aggregationManager.StopAggregators(MetricAggregationCycleKind.Custom, dto);
            Assert.IsNotNull(customPeriod);
            Assert.IsNotNull(customPeriod.NonpersistentAggregates);
            Assert.IsNotNull(customPeriod.PersistentAggregates);

            Assert.AreEqual(1, customPeriod.NonpersistentAggregates.Count);
            Assert.AreEqual("Measurement Metric", customPeriod.NonpersistentAggregates[0].MetricId);
            Assert.AreEqual(1, customPeriod.NonpersistentAggregates[0].Data["Count"]);
            Assert.AreEqual(3.0, customPeriod.NonpersistentAggregates[0].Data["Sum"]);

            Assert.AreEqual(1, customPeriod.PersistentAggregates.Count);
            Assert.AreEqual("Accumulator Metric", customPeriod.PersistentAggregates[0].MetricId);
            Assert.AreEqual(6.0, customPeriod.PersistentAggregates[0].Data["Sum"]);

            quickpulsePeriod = aggregationManager.StopAggregators(MetricAggregationCycleKind.QuickPulse, dto);
            Assert.IsNotNull(quickpulsePeriod);
            Assert.IsNotNull(quickpulsePeriod.NonpersistentAggregates);
            Assert.IsNotNull(quickpulsePeriod.PersistentAggregates);

            Assert.AreEqual(1, quickpulsePeriod.NonpersistentAggregates.Count);
            Assert.AreEqual("Measurement Metric", quickpulsePeriod.NonpersistentAggregates[0].MetricId);
            Assert.AreEqual(1, quickpulsePeriod.NonpersistentAggregates[0].Data["Count"]);
            Assert.AreEqual(3.0, quickpulsePeriod.NonpersistentAggregates[0].Data["Sum"]);

            Assert.AreEqual(1, quickpulsePeriod.PersistentAggregates.Count);
            Assert.AreEqual("Accumulator Metric", quickpulsePeriod.PersistentAggregates[0].MetricId);
            Assert.AreEqual(6.0, quickpulsePeriod.PersistentAggregates[0].Data["Sum"]);

            measurementMetric.TrackValue(5);
            accumulatorMetric.TrackValue(6);

            quickpulsePeriod = aggregationManager.StopAggregators(MetricAggregationCycleKind.Custom, dto);
            Assert.IsNotNull(quickpulsePeriod);
            Assert.IsNotNull(quickpulsePeriod.NonpersistentAggregates);
            Assert.IsNotNull(quickpulsePeriod.PersistentAggregates);

            Assert.AreEqual(0, quickpulsePeriod.NonpersistentAggregates.Count);
            Assert.AreEqual(1, customPeriod.PersistentAggregates.Count);
            Assert.AreEqual("Accumulator Metric", customPeriod.PersistentAggregates[0].MetricId);
            Assert.AreEqual(6.0, customPeriod.PersistentAggregates[0].Data["Sum"]);

            quickpulsePeriod = aggregationManager.StopAggregators(MetricAggregationCycleKind.QuickPulse, dto);
            Assert.IsNotNull(quickpulsePeriod);
            Assert.IsNotNull(quickpulsePeriod.NonpersistentAggregates);
            Assert.IsNotNull(quickpulsePeriod.PersistentAggregates);

            Assert.AreEqual(0, quickpulsePeriod.NonpersistentAggregates.Count);
            Assert.AreEqual(1, quickpulsePeriod.PersistentAggregates.Count);
            Assert.AreEqual("Accumulator Metric", quickpulsePeriod.PersistentAggregates[0].MetricId);
            Assert.AreEqual(12.0, quickpulsePeriod.PersistentAggregates[0].Data["Sum"]);
        }

        private class AcceptAllFilter : IMetricSeriesFilter
        {
            public bool WillConsume(MetricSeries dataSeries, out IMetricValueFilter valueFilter)
            {
                valueFilter = null;
                return true;
            }
        }

        private class DenyAllFilter : IMetricSeriesFilter
        {
            public bool WillConsume(MetricSeries dataSeries, out IMetricValueFilter valueFilter)
            {
                valueFilter = null;
                return false;
            }
        }

    }
}
