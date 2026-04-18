using System;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Metrics;
using Microsoft.ApplicationInsights.Metrics.Extensibility;
using System.Collections.Generic;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using System.Threading.Tasks;
using System.Diagnostics;

using CycleKind = Microsoft.ApplicationInsights.Metrics.Extensibility.MetricAggregationCycleKind;
using Microsoft.ApplicationInsights.Metrics.TestUtility;

namespace SomeCustomerNamespace
{
    /// <summary />
    [TestClass]
    public class MetricEmissionTestsVirtualTime
    {
        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        [TestMethod]
        public void RecordNormalMetric()
        {
            TelemetryConfiguration telemetryPipeline = TelemetryConfiguration.CreateDefault();
            //using (telemetryPipeline)
            {
                RecordNormalMetric(telemetryPipeline);
                TestUtil.CompleteDefaultAggregationCycle(telemetryPipeline.GetMetricManager());
            }
        }


        private void RecordNormalMetric(TelemetryConfiguration telemetryPipeline)
        {
            MetricSeries durationMeric = telemetryPipeline.GetMetricManager().CreateNewSeries(
                                                                        "Test Metrics",
                                                                        "Item Add duration",
                                                                        new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

            MockContainerDataStructure dataStructure = new MockContainerDataStructure((c) => TimeSpan.FromSeconds(c));

            DateTimeOffset experimentStart = new DateTimeOffset(2017, 9, 14, 0, 0, 0, TimeSpan.Zero);

            // Stop the default minute-ly cycle so that it does not interfere with our virtual time debugging:
            Task fireAndForget = telemetryPipeline.GetMetricManager().StopDefaultAggregationCycleAsync();   

            telemetryPipeline.GetMetricManager().StartOrCycleAggregators(CycleKind.Custom, experimentStart, futureFilter: null);

            const int ExperimentLengthSecs = 60 * 10;
            const int IntervalLengthSecs = 60;

            int totalSecs = 0;
            int intervalSecs = 0;

            int itemsThisTime = 0;
            const int maxItemsAtATime = 4;

            int operationsCount = 0;

            while (totalSecs < ExperimentLengthSecs)
            {
                itemsThisTime = (itemsThisTime + 1) % maxItemsAtATime;

                int addItemCount = 1 + (itemsThisTime + 1) % maxItemsAtATime;
                int removeItemCount = 1 + itemsThisTime % maxItemsAtATime;

                Trace.WriteLine($"{totalSecs})");
                Trace.WriteLine(addItemCount);
                Trace.WriteLine(removeItemCount);
                Trace.WriteLine("");

                TimeSpan duration;

                dataStructure.AddItems(addItemCount, out duration);
                operationsCount++;

                int durationSecs = (int) duration.TotalSeconds;
                durationMeric.TrackValue(durationSecs);

                totalSecs += durationSecs;
                intervalSecs += durationSecs;

                dataStructure.RemoveItems(removeItemCount, out duration);
                operationsCount++;

                durationSecs = (int) duration.TotalSeconds;
                durationMeric.TrackValue(durationSecs);

                totalSecs += durationSecs;
                intervalSecs += durationSecs;

                if (intervalSecs >= IntervalLengthSecs)
                {
                    AggregationPeriodSummary aggregatedMetrics = telemetryPipeline.GetMetricManager().StartOrCycleAggregators(
                                                                                                    CycleKind.Custom,
                                                                                                    experimentStart.AddSeconds(totalSecs),
                                                                                                    futureFilter: null);
                    Assert.IsNotNull(aggregatedMetrics);

                    IReadOnlyList<MetricAggregate> aggregates = aggregatedMetrics.NonpersistentAggregates;
                    Assert.IsNotNull(aggregates);
                    Assert.AreEqual(1, aggregates.Count);

                    MetricAggregate aggregate = aggregates[0];
                    Assert.IsNotNull(aggregates);

                    Assert.AreEqual(1.0, aggregate.Data["Min"]);
                    Assert.AreEqual(4.0, aggregate.Data["Max"]);
                    Assert.AreEqual(operationsCount, aggregate.Data["Count"]);
                    Assert.AreEqual("Item Add duration", aggregate.MetricId);
                    Assert.IsNotNull(aggregate.Dimensions);
                    Assert.AreEqual(0, aggregate.Dimensions.Count);
                    Assert.AreEqual((double) intervalSecs, aggregate.Data["Sum"]);
                    Assert.AreEqual(experimentStart.AddSeconds(totalSecs - intervalSecs), aggregate.AggregationPeriodStart);

                    intervalSecs %= IntervalLengthSecs;
                    operationsCount = 0;

                    Assert.AreEqual(0, intervalSecs, "For the above to work, the number of wirtual secs must exactly fit into IntervalLengthSecs.");
                }
            }
            {
                AggregationPeriodSummary aggregatedMetrics = telemetryPipeline.GetMetricManager().StartOrCycleAggregators(
                                                                                                        CycleKind.Custom,
                                                                                                        experimentStart.AddSeconds(totalSecs),
                                                                                                        futureFilter: null);
                Assert.IsNotNull(aggregatedMetrics);

                IReadOnlyList<MetricAggregate> aggregates = aggregatedMetrics.NonpersistentAggregates;
                Assert.IsNotNull(aggregates);
                Assert.AreEqual(0, aggregates.Count);
            }
            {
                durationMeric.TrackValue("7");
                durationMeric.TrackValue("8");
                durationMeric.TrackValue("9.0");
                totalSecs += 24;
            }
            {
                AggregationPeriodSummary aggregatedMetrics = telemetryPipeline.GetMetricManager().StopAggregators(
                                                                                                    CycleKind.Custom,
                                                                                                    experimentStart.AddSeconds(totalSecs));
                Assert.IsNotNull(aggregatedMetrics);

                IReadOnlyList<MetricAggregate> aggregates = aggregatedMetrics.NonpersistentAggregates;
                Assert.IsNotNull(aggregates);

                MetricAggregate aggregate = aggregates[0];
                Assert.IsNotNull(aggregates);

                Assert.AreEqual(7.0, aggregate.Data["Min"]);
                Assert.AreEqual(9.0, aggregate.Data["Max"]);
                Assert.AreEqual(3, aggregate.Data["Count"]);
                Assert.AreEqual("Item Add duration", aggregate.MetricId);
                Assert.IsNotNull(aggregate.Dimensions);
                Assert.AreEqual(0, aggregate.Dimensions.Count);
                Assert.AreEqual(24.0, aggregate.Data["Sum"]);
                Assert.AreEqual(experimentStart.AddSeconds(totalSecs - 24), aggregate.AggregationPeriodStart);
            }
        }

        #region class MockContainerDataStructure
        private class MockContainerDataStructure
        {
            private readonly Random _rnd;
            private readonly Func<int, TimeSpan> _customOperationDurationCalculator;

            private int _itemCount;

            public MockContainerDataStructure()
                : this(customOperationDurationCalculator : null)
            {
            }

            public MockContainerDataStructure(Func<int, TimeSpan> customOperationDurationCalculator)
            {
                if (customOperationDurationCalculator == null)
                {
                    _rnd = new Random();
                    _customOperationDurationCalculator = null;
                }
                else
                {
                    _rnd = null;
                    _customOperationDurationCalculator = customOperationDurationCalculator;
                }
                
                _itemCount = 0;
            }

            public void AddItems(int count, out TimeSpan duration)
            {
                if (count < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(count));
                }

                duration = GetVirtualOperationDuration(count);
                Interlocked.Add(ref _itemCount, count);
            }

            public void RemoveItems(int count, out TimeSpan duration)
            {
                if (count < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(count));
                }

                duration = GetVirtualOperationDuration(count);
                Interlocked.Add(ref _itemCount, -count);
            }

            private TimeSpan GetVirtualOperationDuration(int count)
            {
                if (_customOperationDurationCalculator != null)
                {
                    return _customOperationDurationCalculator(count);
                }

                int millis = _rnd.Next(99) + 1;
                return TimeSpan.FromMilliseconds(millis);
            }
        }
        #endregion class MockContainerDataStructure
    }
}
