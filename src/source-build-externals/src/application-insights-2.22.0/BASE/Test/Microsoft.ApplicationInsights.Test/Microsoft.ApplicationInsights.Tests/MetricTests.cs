#pragma warning disable 612, 618  // obsolete TelemetryConfigration.Active
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Metrics;
using Microsoft.ApplicationInsights.Metrics.Extensibility;
using Microsoft.ApplicationInsights.Metrics.TestUtility;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.ApplicationInsights
{
    /// <summary />
    [TestClass]
    public class MetricTests
    {
        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        [TestMethod]
        public void Create()
        {
            // The Metric ctor is internal, we use reflection to invoke it.

            MemoryMetricTelemetryPipeline telemetryCollector = new MemoryMetricTelemetryPipeline();
            MetricManager metricManager = new MetricManager(telemetryCollector);

            // ** Validate metricManager parameter:

            Assert.ThrowsException<ArgumentNullException>(
                    () => InvokeMetricCtor(
                                    metricManager: null,
                                    metricNamespace: "NS",
                                    metricId: "Foo",
                                    dimension1Name: "D1",
                                    dimension2Name: "D2",
                                    configuration: MetricConfigurations.Common.Measurement())
            );

            {
                Metric metric = InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "NS",
                                    metricId: "  Foo ",
                                    dimension1Name: "D1",
                                    dimension2Name: "D2",
                                    configuration: MetricConfigurations.Common.Measurement());
                Assert.IsNotNull(metric);
            }


            // ** Validate metricId parameter:

            Assert.ThrowsException<ArgumentNullException>(
                    () => InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "NS",
                                    metricId: null,
                                    dimension1Name: "D1",
                                    dimension2Name: "D2",
                                    configuration: MetricConfigurations.Common.Measurement())
            );

            Assert.ThrowsException<ArgumentException>(
                    () => InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "NS",
                                    metricId: "",
                                    dimension1Name: "D1",
                                    dimension2Name: "D2",
                                    configuration: MetricConfigurations.Common.Measurement())
            );

            Assert.ThrowsException<ArgumentException>(
                    () => InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "NS",
                                    metricId: "   ",
                                    dimension1Name: "D1",
                                    dimension2Name: "D2",
                                    configuration: MetricConfigurations.Common.Measurement())
            );

            {
                Metric metric = InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "NS",
                                    metricId: "  Foo ",
                                    dimension1Name: "D1",
                                    dimension2Name: "D2",
                                    configuration: MetricConfigurations.Common.Measurement());
                Assert.IsNotNull(metric);
                Assert.AreEqual("NS", metric.Identifier.MetricNamespace);
                Assert.AreEqual("Foo", metric.Identifier.MetricId);
            }


            // ** Validate dimension1Name parameter:

            Assert.ThrowsException<ArgumentException>(
                    () => InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "NS",
                                    metricId: "Foo",
                                    dimension1Name: null,
                                    dimension2Name: "D2",
                                    configuration: MetricConfigurations.Common.Measurement())
            );

            Assert.ThrowsException<ArgumentException>(
                    () => InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "NS",
                                    metricId: "Foo",
                                    dimension1Name: "",
                                    dimension2Name: "D2",
                                    configuration: MetricConfigurations.Common.Measurement())
            );

            Assert.ThrowsException<ArgumentException>(
                    () => InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "NS",
                                    metricId: "Foo",
                                    dimension1Name: "  \t",
                                    dimension2Name: "D2",
                                    configuration: MetricConfigurations.Common.Measurement())
            );

            {
                Metric metric = InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "NS",
                                    metricId: "Foo",
                                    dimension1Name: " D1  ",
                                    dimension2Name: "D2",
                                    configuration: MetricConfigurations.Common.Measurement());
                Assert.IsNotNull(metric);
                Assert.AreEqual("D1", metric.Identifier.GetDimensionName(1));
            }


            // ** Validate dimension2Name parameter:

            Assert.ThrowsException<ArgumentException>(
                    () => InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "NS",
                                    metricId: "Foo",
                                    dimension1Name: "D1",
                                    dimension2Name: "",
                                    configuration: MetricConfigurations.Common.Measurement())
            );

            Assert.ThrowsException<ArgumentException>(
                    () => InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "NS",
                                    metricId: "Foo",
                                    dimension1Name: "D1",
                                    dimension2Name: " \r\n",
                                    configuration: MetricConfigurations.Common.Measurement())
            );

            {
                Metric metric = InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "NS",
                                    metricId: "Foo",
                                    dimension1Name: "D1",
                                    dimension2Name: "D2\t",
                                    configuration: MetricConfigurations.Common.Measurement());
                Assert.IsNotNull(metric);
                Assert.AreEqual(2, metric.Identifier.DimensionsCount);
                Assert.AreEqual("D1", metric.Identifier.GetDimensionName(1));
                Assert.AreEqual("D2", metric.Identifier.GetDimensionName(2));
            }
            {
                Metric metric = InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "ns",
                                    metricId: "Foo",
                                    dimension1Name: "D1",
                                    dimension2Name: null,
                                    configuration: MetricConfigurations.Common.Measurement());
                Assert.IsNotNull(metric);
                Assert.AreEqual(1, metric.Identifier.DimensionsCount);
                Assert.AreEqual("D1", metric.Identifier.GetDimensionName(1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.Identifier.GetDimensionName(2));
            }
            {
                Metric metric = InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "ns",
                                    metricId: "Foo",
                                    dimension1Name: null,
                                    dimension2Name: null,
                                    configuration: MetricConfigurations.Common.Measurement());
                Assert.IsNotNull(metric);
                Assert.AreEqual(0, metric.Identifier.DimensionsCount);
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.Identifier.GetDimensionName(1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.Identifier.GetDimensionName(2));
            }


            // ** Validate configuration parameter:

            Assert.ThrowsException<ArgumentNullException>(
                    () => InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "ns",
                                    metricId: "Foo",
                                    dimension1Name: "D1",
                                    dimension2Name: "D2",
                                    configuration: null)
            );

            {
                Metric metric = InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "ns",
                                    metricId: "Foo",
                                    dimension1Name: null,
                                    dimension2Name: null,
                                    configuration: MetricConfigurations.Common.Measurement());
                Assert.IsNotNull(metric);

                Assert.AreEqual(MetricConfigurations.Common.Measurement(), metric.GetConfiguration());
                Assert.AreSame(MetricConfigurations.Common.Measurement(), metric.GetConfiguration());
            }
            {
                MetricConfiguration customConfig = new MetricConfiguration(
                                                                    seriesCountLimit: 10,
                                                                    valuesPerDimensionLimit: 10,
                                                                    seriesConfig: new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: true));

                Metric metric = InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "ns",
                                    metricId: "Foo",
                                    dimension1Name: null,
                                    dimension2Name: null,
                                    configuration: customConfig);
                Assert.IsNotNull(metric);

                Assert.AreEqual(customConfig, metric.GetConfiguration());
                Assert.AreSame(customConfig, metric.GetConfiguration());
            }

            TestUtil.CompleteDefaultAggregationCycle(metricManager);
        }

        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        [TestMethod]
        public void MetricId()
        {
            MemoryMetricTelemetryPipeline telemetryCollector = new MemoryMetricTelemetryPipeline();
            MetricManager metricManager = new MetricManager(telemetryCollector);

            {
                Metric metric = InvokeMetricCtor(
                                        metricManager,
                                        metricNamespace: "ns",
                                        metricId: "Foo",
                                        dimension1Name: null,
                                        dimension2Name: null,
                                        configuration: MetricConfigurations.Common.Measurement());

                Assert.AreEqual("Foo", metric.Identifier.MetricId);
            }
            {
                Metric metric = InvokeMetricCtor(
                                        metricManager,
                                        metricNamespace: "ns",
                                        metricId: "\t\tFoo Bar \r\nx ",
                                        dimension1Name: null,
                                        dimension2Name: null,
                                        configuration: MetricConfigurations.Common.Measurement());

                Assert.AreEqual("Foo Bar \r\nx", metric.Identifier.MetricId);
            }

            TestUtil.CompleteDefaultAggregationCycle(metricManager);
        }

        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        [TestMethod]
        public void MetricNamespace()
        {
            MemoryMetricTelemetryPipeline telemetryCollector = new MemoryMetricTelemetryPipeline();
            MetricManager metricManager = new MetricManager(telemetryCollector);

            {
                Metric metric = InvokeMetricCtor(
                                        metricManager,
                                        metricNamespace: "Foo",
                                        metricId: "mid",
                                        dimension1Name: null,
                                        dimension2Name: null,
                                        configuration: MetricConfigurations.Common.Measurement());

                Assert.AreEqual("Foo", metric.Identifier.MetricNamespace);
            }
            {
                Metric metric = InvokeMetricCtor(
                                        metricManager,
                                        metricNamespace: "\t\tFoo Bar \r\nx ",
                                        metricId: "mid",
                                        dimension1Name: null,
                                        dimension2Name: null,
                                        configuration: MetricConfigurations.Common.Measurement());

                Assert.AreEqual("Foo Bar \r\nx", metric.Identifier.MetricNamespace);
            }

            TestUtil.CompleteDefaultAggregationCycle(metricManager);
        }

        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        [TestMethod]
        public void DimensionsCount()
        {
            // The Metric ctor is internal, we use reflection to invoke it.

            MemoryMetricTelemetryPipeline telemetryCollector = new MemoryMetricTelemetryPipeline();
            MetricManager metricManager = new MetricManager(telemetryCollector);

            {
                Metric metric = InvokeMetricCtor(
                                        metricManager,
                                        metricNamespace: "ns",
                                        metricId: "Foo",
                                        dimension1Name: null,
                                        dimension2Name: null,
                                        configuration: MetricConfigurations.Common.Measurement());

                Assert.AreEqual(0, metric.Identifier.DimensionsCount);
            }
            {
                Metric metric = InvokeMetricCtor(
                                        metricManager,
                                        metricNamespace: "ns",
                                        metricId: "Foo",
                                        dimension1Name: "XXX",
                                        dimension2Name: null,
                                        configuration: MetricConfigurations.Common.Measurement());

                Assert.AreEqual(1, metric.Identifier.DimensionsCount);
            }
            {
                Metric metric = InvokeMetricCtor(
                                        metricManager,
                                        metricNamespace: "ns",
                                        metricId: "Foo",
                                        dimension1Name: "XXX",
                                        dimension2Name: "XXX",
                                        configuration: MetricConfigurations.Common.Measurement());

                Assert.AreEqual(2, metric.Identifier.DimensionsCount);
            }

            TestUtil.CompleteDefaultAggregationCycle(metricManager);
        }

        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        [TestMethod]
        public void SeriesCount()
        {
            MemoryMetricTelemetryPipeline telemetryCollector = new MemoryMetricTelemetryPipeline();
            MetricManager metricManager = new MetricManager(telemetryCollector);

            {
                Metric metric = InvokeMetricCtor(
                                        metricManager,
                                        metricNamespace: "ns",
                                        metricId: "Foo",
                                        dimension1Name: "D1",
                                        dimension2Name: "D2",
                                        configuration: MetricConfigurations.Common.Measurement());

                Assert.AreEqual(1, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(1, "DV11", "DV21"));
                Assert.AreEqual(2, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(2, "DV11", "DV21"));
                Assert.AreEqual(2, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(3, "DV12", "DV21"));
                Assert.AreEqual(3, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(4, "DV13", "DV21"));
                Assert.AreEqual(4, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(5, "DV14", "DV21"));
                Assert.AreEqual(5, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(6, "DV12", "DV22"));
                Assert.AreEqual(6, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(7, "DV12", "DV23"));
                Assert.AreEqual(7, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(8, "DV12", "DV23"));
                Assert.AreEqual(7, metric.SeriesCount);
            }
            {
                MetricConfiguration config = new MetricConfiguration(
                                                            5,
                                                            1000,
                                                            new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

                Metric metric = InvokeMetricCtor(
                                        metricManager,
                                        metricNamespace: "ns",
                                        metricId: "Foo",
                                        dimension1Name: "D1",
                                        dimension2Name: "D2",
                                        configuration: config);

                Assert.AreEqual(1, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(1, "DV11", "DV21"));
                Assert.AreEqual(2, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(2, "DV11", "DV21"));
                Assert.AreEqual(2, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(3, "DV12", "DV21"));
                Assert.AreEqual(3, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(4, "DV13", "DV21"));
                Assert.AreEqual(4, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(5, "DV14", "DV21"));
                Assert.AreEqual(5, metric.SeriesCount);

                Assert.IsFalse(metric.TrackValue(6, "DV12", "DV22"));
                Assert.AreEqual(5, metric.SeriesCount);

                Assert.IsFalse(metric.TrackValue(7, "DV12", "DV23"));
                Assert.AreEqual(5, metric.SeriesCount);

                Assert.IsFalse(metric.TrackValue(8, "DV12", "DV23"));
                Assert.AreEqual(5, metric.SeriesCount);
            }

            TestUtil.CompleteDefaultAggregationCycle(metricManager);
        }

        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        [TestMethod]
        public void GetDimensionName()
        {
            MemoryMetricTelemetryPipeline telemetryCollector = new MemoryMetricTelemetryPipeline();
            MetricManager metricManager = new MetricManager(telemetryCollector);

            {
                Metric metric = InvokeMetricCtor(
                                        metricManager,
                                        metricNamespace: "ns",
                                        metricId: "Foo",
                                        dimension1Name: null,
                                        dimension2Name: null,
                                        configuration: MetricConfigurations.Common.Measurement());

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.Identifier.GetDimensionName(-1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.Identifier.GetDimensionName(0));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.Identifier.GetDimensionName(1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.Identifier.GetDimensionName(2));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.Identifier.GetDimensionName(3));
            }
            {
                Metric metric = InvokeMetricCtor(
                                        metricManager,
                                        metricNamespace: "ns",
                                        metricId: "Foo",
                                        dimension1Name: "D1",
                                        dimension2Name: null,
                                        configuration: MetricConfigurations.Common.Measurement());

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.Identifier.GetDimensionName(-1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.Identifier.GetDimensionName(0));
                Assert.AreEqual("D1", metric.Identifier.GetDimensionName(1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.Identifier.GetDimensionName(2));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.Identifier.GetDimensionName(3));
            }
            {
                Metric metric = InvokeMetricCtor(
                                        metricManager,
                                        metricNamespace: "ns",
                                        metricId: "Foo",
                                        dimension1Name: "D1",
                                        dimension2Name: "D2",
                                        configuration: MetricConfigurations.Common.Measurement());

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.Identifier.GetDimensionName(-1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.Identifier.GetDimensionName(0));
                Assert.AreEqual("D1", metric.Identifier.GetDimensionName(1));
                Assert.AreEqual("D2", metric.Identifier.GetDimensionName(2));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.Identifier.GetDimensionName(3));
            }

            TestUtil.CompleteDefaultAggregationCycle(metricManager);
        }

        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        [TestMethod]
        public void GetDimensionValues()
        {
            MemoryMetricTelemetryPipeline telemetryCollector = new MemoryMetricTelemetryPipeline();
            MetricManager metricManager = new MetricManager(telemetryCollector);

            {
                Metric metric = InvokeMetricCtor(
                                        metricManager,
                                        metricNamespace: "ns",
                                        metricId: "Foo",
                                        dimension1Name: "D1",
                                        dimension2Name: "D2",
                                        configuration: MetricConfigurations.Common.Measurement());

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.GetDimensionValues(-1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.GetDimensionValues(0));

                IReadOnlyCollection<string> dimVals = metric.GetDimensionValues(1);
                Assert.AreEqual(0, dimVals.Count);

                dimVals = metric.GetDimensionValues(2);
                Assert.AreEqual(0, dimVals.Count);

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.GetDimensionValues(3));


                metric.TrackValue(1);
                metric.TrackValue(2);
                metric.TrackValue(3, "A", "B");
                metric.TrackValue(4, "a", "B");
                metric.TrackValue(5, "X", "B");
                metric.TrackValue(6, "Y", "B");
                metric.TrackValue(7, "Y", "C");
                metric.TrackValue(8, "A", "B");

                dimVals = metric.GetDimensionValues(1);
                Assert.AreEqual(4, dimVals.Count);
                Assert.IsTrue(dimVals.Contains("A"));
                Assert.IsTrue(dimVals.Contains("a"));
                Assert.IsTrue(dimVals.Contains("X"));
                Assert.IsTrue(dimVals.Contains("Y"));
                Assert.IsFalse(dimVals.Contains("y"));

                dimVals = metric.GetDimensionValues(2);
                Assert.AreEqual(2, dimVals.Count);
                Assert.IsTrue(dimVals.Contains("B"));
                Assert.IsTrue(dimVals.Contains("C"));
                Assert.IsFalse(dimVals.Contains("b"));
                Assert.IsFalse(dimVals.Contains("c"));

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.GetDimensionValues(-1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.GetDimensionValues(0));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.GetDimensionValues(3));
            }
            {
                Metric metric = InvokeMetricCtor(
                                        metricManager,
                                        metricNamespace: "ns",
                                        metricId: "Foo",
                                        dimension1Name: "D1",
                                        dimension2Name: null,
                                        configuration: MetricConfigurations.Common.Measurement());

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.GetDimensionValues(-1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.GetDimensionValues(0));

                IReadOnlyCollection<string> dimVals = metric.GetDimensionValues(1);
                Assert.AreEqual(0, dimVals.Count);

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.GetDimensionValues(2));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.GetDimensionValues(3));


                metric.TrackValue(1);
                metric.TrackValue(2);
                metric.TrackValue(3, "A");
                metric.TrackValue(4, "a");
                metric.TrackValue(5, "X");
                metric.TrackValue(6, "Y");
                metric.TrackValue(7, "Y");
                metric.TrackValue(8, "A");

                dimVals = metric.GetDimensionValues(1);
                Assert.AreEqual(4, dimVals.Count);
                Assert.IsTrue(dimVals.Contains("A"));
                Assert.IsTrue(dimVals.Contains("a"));
                Assert.IsTrue(dimVals.Contains("X"));
                Assert.IsTrue(dimVals.Contains("Y"));
                Assert.IsFalse(dimVals.Contains("y"));

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.GetDimensionValues(-1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.GetDimensionValues(0));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.GetDimensionValues(2));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.GetDimensionValues(3));
            }
            {
                MetricConfiguration config = new MetricConfiguration(
                                                            4,
                                                            1000,
                                                            new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));
                Metric metric = InvokeMetricCtor(
                                        metricManager,
                                        metricNamespace: "ns",
                                        metricId: "Foo",
                                        dimension1Name: "D1",
                                        dimension2Name: "D2",
                                        configuration: config);

                metric.TrackValue(1);
                metric.TrackValue(1);
                Assert.IsTrue(metric.TrackValue(2, "A", "B"));
                Assert.IsTrue(metric.TrackValue(3, "a", "B"));
                Assert.IsTrue(metric.TrackValue(4, "X", "B"));
                Assert.IsFalse(metric.TrackValue(5, "Y", "B"));
                Assert.IsFalse(metric.TrackValue(5, "Y", "C"));
                Assert.IsTrue(metric.TrackValue(5, "A", "B"));

                IReadOnlyCollection<string> dimVals = metric.GetDimensionValues(1);
                Assert.AreEqual(3, dimVals.Count);
                Assert.IsTrue(dimVals.Contains("A"));
                Assert.IsTrue(dimVals.Contains("a"));
                Assert.IsTrue(dimVals.Contains("X"));
                Assert.IsFalse(dimVals.Contains("Y"));

                dimVals = metric.GetDimensionValues(2);
                Assert.AreEqual(1, dimVals.Count);
                Assert.IsTrue(dimVals.Contains("B"));

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.GetDimensionValues(-1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.GetDimensionValues(0));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.GetDimensionValues(3));
            }
            {
                MetricConfiguration config = new MetricConfiguration(
                                                            4,
                                                            1000,
                                                            new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));
                Metric metric = InvokeMetricCtor(
                                        metricManager,
                                        metricNamespace: "ns",
                                        metricId: "Foo",
                                        dimension1Name: "D1",
                                        dimension2Name: null,
                                        configuration: config);

                metric.TrackValue(1);
                metric.TrackValue(1);
                Assert.IsTrue(metric.TrackValue(2, "A"));
                Assert.IsTrue(metric.TrackValue(3, "a"));
                Assert.IsTrue(metric.TrackValue(4, "X"));
                Assert.IsFalse(metric.TrackValue(5, "Y"));
                Assert.IsFalse(metric.TrackValue(5, "Y"));
                Assert.IsTrue(metric.TrackValue(5, "A"));

                IReadOnlyCollection<string> dimVals = metric.GetDimensionValues(1);
                Assert.AreEqual(3, dimVals.Count);
                Assert.IsTrue(dimVals.Contains("A"));
                Assert.IsTrue(dimVals.Contains("a"));
                Assert.IsTrue(dimVals.Contains("X"));
                Assert.IsFalse(dimVals.Contains("Y"));

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.GetDimensionValues(-1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.GetDimensionValues(0));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.GetDimensionValues(2));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.GetDimensionValues(3));
            }
            {
                MetricConfiguration config = new MetricConfiguration(
                                                            1000,
                                                            2,
                                                            new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));
                Metric metric = InvokeMetricCtor(
                                        metricManager,
                                        metricNamespace: "ns",
                                        metricId: "Foo",
                                        dimension1Name: "D1",
                                        dimension2Name: "D2",
                                        configuration: config);

                metric.TrackValue(0);
                metric.TrackValue(0);
                Assert.IsTrue(metric.TrackValue(0, "A", "B"));
                Assert.IsTrue(metric.TrackValue(0, "a", "B"));
                Assert.IsFalse(metric.TrackValue(0, "X", "B"));
                Assert.IsFalse(metric.TrackValue(0, "Y", "B"));
                Assert.IsFalse(metric.TrackValue(0, "Y", "C"));
                Assert.IsTrue(metric.TrackValue(0, "A", "B"));
                Assert.IsTrue(metric.TrackValue(0, "A", "C"));
                Assert.IsFalse(metric.TrackValue(0, "A", "D"));

                IReadOnlyCollection<string> dimVals = metric.GetDimensionValues(1);
                Assert.AreEqual(2, dimVals.Count);
                Assert.IsTrue(dimVals.Contains("A"));
                Assert.IsTrue(dimVals.Contains("a"));

                dimVals = metric.GetDimensionValues(2);
                Assert.AreEqual(2, dimVals.Count);
                Assert.IsTrue(dimVals.Contains("B"));
                Assert.IsTrue(dimVals.Contains("C"));

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.GetDimensionValues(-1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.GetDimensionValues(0));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.GetDimensionValues(3));
            }
            {
                MetricConfiguration config = new MetricConfiguration(
                                                            1000,
                                                            2,
                                                            new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));
                Metric metric = InvokeMetricCtor(
                                        metricManager,
                                        metricNamespace: "ns",
                                        metricId: "Foo",
                                        dimension1Name: "D1",
                                        dimension2Name: null,
                                        configuration: config);

                metric.TrackValue(0);
                metric.TrackValue(0);
                Assert.IsTrue(metric.TrackValue(0, "A"));
                Assert.IsTrue(metric.TrackValue(0, "a"));
                Assert.IsFalse(metric.TrackValue(0, "X"));
                Assert.IsFalse(metric.TrackValue(0, "Y"));
                Assert.IsTrue(metric.TrackValue(0, "A"));

                IReadOnlyCollection<string> dimVals = metric.GetDimensionValues(1);
                Assert.AreEqual(2, dimVals.Count);
                Assert.IsTrue(dimVals.Contains("A"));
                Assert.IsTrue(dimVals.Contains("a"));

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.GetDimensionValues(-1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.GetDimensionValues(0));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => metric.GetDimensionValues(3));
            }

            TestUtil.CompleteDefaultAggregationCycle(metricManager);
        }

        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        [TestMethod]
        public void GetAllSeries()
        {
            MemoryMetricTelemetryPipeline telemetryCollector = new MemoryMetricTelemetryPipeline();
            MetricManager metricManager = new MetricManager(telemetryCollector);

            MetricConfiguration config = new MetricConfiguration(
                                                        5,
                                                        1000,
                                                        new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

            Metric metric = InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "ns",
                                    metricId: "Foo",
                                    dimension1Name: "D1",
                                    dimension2Name: "D2",
                                    configuration: config);

            IReadOnlyList<KeyValuePair<string[], MetricSeries>> series = metric.GetAllSeries();
            Assert.AreEqual(1, series.Count);
            AssertSeries(
                        series[0],
                        expectedKeys: new string[0],
                        expectedMetricId: "Foo",
                        expectedDimensionNamesAndValues: new Dictionary<string, string> { },
                        expectedCount: null,
                        expectedSum: null);


            metric.TrackValue(1);
            metric.TrackValue(2);
            Assert.IsTrue(metric.TrackValue(3, "A", "B"));
            Assert.IsTrue(metric.TrackValue(4, "a", "B"));
            Assert.IsTrue(metric.TrackValue(7, "Y", "C"));
            Assert.IsTrue(metric.TrackValue(5, "X", "B"));
            Assert.IsFalse(metric.TrackValue(6, "Y", "B"));
            Assert.IsTrue(metric.TrackValue(8, "A", "B"));

            series = metric.GetAllSeries();
            Assert.AreEqual(5, series.Count);

            KeyValuePair<string[], MetricSeries>[] sortedSeries = series.OrderBy((s) => String.Concat(s.Key), StringComparer.InvariantCulture).ToArray();

            AssertSeries(
                        sortedSeries[0],
                        expectedKeys: new string[0],
                        expectedMetricId: "Foo",
                        expectedDimensionNamesAndValues: new Dictionary<string, string> { },
                        expectedCount: 2,
                        expectedSum: 3);

            AssertSeries(
                       sortedSeries[1],
                       expectedKeys: new string[] { "a", "B" },
                       expectedMetricId: "Foo",
                       expectedDimensionNamesAndValues: new Dictionary<string, string> { ["D1"] = "a", ["D2"] = "B" },
                       expectedCount: 1,
                       expectedSum: 4);

            AssertSeries(
                        sortedSeries[2],
                        expectedKeys: new string[] { "A", "B" },
                        expectedMetricId: "Foo",
                        expectedDimensionNamesAndValues: new Dictionary<string, string> { ["D1"] = "A", ["D2"] = "B" },
                        expectedCount: 2,
                        expectedSum: 11);

            AssertSeries(
                        sortedSeries[3],
                        expectedKeys: new string[] { "X", "B" },
                        expectedMetricId: "Foo",
                        expectedDimensionNamesAndValues: new Dictionary<string, string> { ["D1"] = "X", ["D2"] = "B" },
                        expectedCount: 1,
                        expectedSum: 5);

            AssertSeries(
                        sortedSeries[4],
                        expectedKeys: new string[] { "Y", "C" },
                        expectedMetricId: "Foo",
                        expectedDimensionNamesAndValues: new Dictionary<string, string> { ["D1"] = "Y", ["D2"] = "C" },
                        expectedCount: 1,
                        expectedSum: 7);

            TestUtil.CompleteDefaultAggregationCycle(metricManager);
        }

        private static void AssertSeries(
                                    KeyValuePair<string[], MetricSeries> series,
                                    string[] expectedKeys,
                                    string expectedMetricId,
                                    IDictionary<string, string> expectedDimensionNamesAndValues,
                                    int? expectedCount,
                                    double? expectedSum)
        {
            Assert.IsNotNull(series.Key);

            Assert.AreEqual(expectedKeys.Length, series.Key.Length);
            for (int i = 0; i < expectedKeys.Length; i++)
            {
                Assert.AreEqual(expectedKeys[i], series.Key[i]);
            }

            Assert.AreEqual(expectedMetricId, series.Value.MetricIdentifier.MetricId);

            Assert.AreEqual(expectedDimensionNamesAndValues.Count, series.Value.DimensionNamesAndValues.Count);
            foreach (KeyValuePair<string, string> dimNameValue in expectedDimensionNamesAndValues)
            {
                Assert.AreEqual(dimNameValue.Value, series.Value.DimensionNamesAndValues[dimNameValue.Key]);
            }

            MetricAggregate currentAggregate = series.Value.GetCurrentAggregateUnsafe();

            Assert.AreEqual(expectedCount, series.Value.GetCurrentAggregateUnsafe()?.Data?["Count"]);
            Assert.AreEqual(expectedSum, series.Value.GetCurrentAggregateUnsafe()?.Data?["Sum"]);
        }

        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        [TestMethod]
        public void TryGetDataSeries_CreatingSeriesCorrectly()
        {
            MemoryMetricTelemetryPipeline telemetryCollector = new MemoryMetricTelemetryPipeline();
            MetricManager metricManager = new MetricManager(telemetryCollector);

            {
                Metric metric = InvokeMetricCtor(
                                        metricManager,
                                        metricNamespace: "ns",
                                        metricId: "Foo",
                                        dimension1Name: null,
                                        dimension2Name: null,
                                        configuration: MetricConfigurations.Common.Measurement());

                MetricSeries series;
                bool success;

                success = metric.TryGetDataSeries(out series);

                Assert.IsTrue(success);
                Assert.IsNotNull(series);
                Assert.AreEqual("Foo", series.MetricIdentifier.MetricId);

                Assert.AreEqual(0, telemetryCollector.Count);

                series.TrackValue(1);
                metricManager.Flush();

                Assert.AreEqual(1, telemetryCollector.Count);
                Assert.IsNotNull(telemetryCollector[0]);
                Assert.AreEqual(1, telemetryCollector[0].Data["Count"]);
                Assert.AreEqual(1.0, telemetryCollector[0].Data["Sum"]);
                Assert.AreEqual("Foo", telemetryCollector[0].MetricId);
                Assert.AreEqual(0, telemetryCollector[0].Dimensions.Count);

                Assert.ThrowsException<ArgumentException>(() => metric.TryGetDataSeries(out series, "Dim1Value"));
                Assert.ThrowsException<ArgumentException>(() => metric.TryGetDataSeries(out series, "Dim1Value"));
                Assert.ThrowsException<ArgumentException>(() => metric.TryGetDataSeries(out series, false, "Dim1Value"));

                Assert.ThrowsException<ArgumentException>(() => metric.TryGetDataSeries(out series, "Dim1Value", "Dim2Value"));
                Assert.ThrowsException<ArgumentException>(() => metric.TryGetDataSeries(out series, "Dim1Value", "Dim2Value"));
                Assert.ThrowsException<ArgumentException>(() => metric.TryGetDataSeries(out series, false, "Dim1Value", "Dim2Value"));
            }
            {
                telemetryCollector.Clear();
                Metric metric = InvokeMetricCtor(
                                        metricManager,
                                        metricNamespace: "ns",
                                        metricId: "Foo",
                                        dimension1Name: "Bar",
                                        dimension2Name: null,
                                        configuration: MetricConfigurations.Common.Measurement());

                MetricSeries series;
                bool success;

                success = metric.TryGetDataSeries(out series);

                Assert.IsTrue(success);
                Assert.IsNotNull(series);
                Assert.AreEqual("Foo", series.MetricIdentifier.MetricId);

                Assert.AreEqual(0, telemetryCollector.Count);

                series.TrackValue(2);
                metricManager.Flush();

                Assert.AreEqual(1, telemetryCollector.Count);
                Assert.IsNotNull(telemetryCollector[0]);
                Assert.AreEqual(1, telemetryCollector[0].Data["Count"]);
                Assert.AreEqual(2.0, telemetryCollector[0].Data["Sum"]);
                Assert.AreEqual("Foo", telemetryCollector[0].MetricId);
                Assert.AreEqual(0, telemetryCollector[0].Dimensions.Count);

                telemetryCollector.Clear();
                MetricSeries series1, series2, series3, series4;

                success = metric.TryGetDataSeries(out series1, false, "Dim1Value");
                Assert.IsFalse(success);
                Assert.IsNull(series1);

                success = metric.TryGetDataSeries(out series1, "Dim1Value");
                Assert.IsTrue(success);
                Assert.IsNotNull(series1);

                success = metric.TryGetDataSeries(out series2, "Dim1Value");
                Assert.IsTrue(success);
                Assert.IsNotNull(series2);

                success = metric.TryGetDataSeries(out series3, "Dim1Value");
                Assert.IsTrue(success);
                Assert.IsNotNull(series3);

                success = metric.TryGetDataSeries(out series4, "Dim1ValueX");
                Assert.IsTrue(success);
                Assert.IsNotNull(series4);

                Assert.AreEqual(series1, series2);
                Assert.AreEqual(series1, series3);
                Assert.AreEqual(series2, series3);
                Assert.AreNotEqual(series1, series4);

                Assert.AreSame(series1, series2);
                Assert.AreSame(series1, series3);
                Assert.AreSame(series2, series3);
                Assert.AreNotSame(series1, series4);

                Assert.AreEqual(0, telemetryCollector.Count);

                series4.TrackValue(40);
                series3.TrackValue(30);
                series2.TrackValue(20);
                series1.TrackValue(10);
                metricManager.Flush();

                Assert.AreEqual(2, telemetryCollector.Count);
                Assert.IsNotNull(telemetryCollector[0]);
                Assert.IsNotNull(telemetryCollector[1]);

                Assert.AreEqual("Foo", telemetryCollector[0].MetricId);
                Assert.AreEqual(3, telemetryCollector[0].Data["Count"]);
                Assert.AreEqual(60.0, telemetryCollector[0].Data["Sum"]);

                Assert.AreEqual(1, telemetryCollector[0].Dimensions.Count);
                Assert.IsTrue(telemetryCollector[0].Dimensions.ContainsKey("Bar"));
                Assert.AreEqual("Dim1Value", telemetryCollector[0].Dimensions["Bar"]);

                Assert.AreEqual("Foo", telemetryCollector[1].MetricId);
                Assert.AreEqual(1, telemetryCollector[1].Data["Count"]);
                Assert.AreEqual(40.0, telemetryCollector[1].Data["Sum"]);

                Assert.AreEqual(1, telemetryCollector[1].Dimensions.Count);
                Assert.IsTrue(telemetryCollector[1].Dimensions.ContainsKey("Bar"));
                Assert.AreEqual("Dim1ValueX", telemetryCollector[1].Dimensions["Bar"]);

                Assert.ThrowsException<ArgumentException>(() => metric.TryGetDataSeries(out series, "Dim1", "Dim2"));
                Assert.ThrowsException<ArgumentException>(() => metric.TryGetDataSeries(out series, "Dim1", "Dim2"));
                Assert.ThrowsException<ArgumentException>(() => metric.TryGetDataSeries(out series, false, "Dim1", "Dim2"));
            }
            {
                telemetryCollector.Clear();
                Metric metric = InvokeMetricCtor(
                                        metricManager,
                                        metricNamespace: "ns",
                                        metricId: "Foo",
                                        dimension1Name: "Bar",
                                        dimension2Name: "Poo",
                                        configuration: MetricConfigurations.Common.Measurement());

                MetricSeries series;
                bool success;

                success = metric.TryGetDataSeries(out series);

                Assert.IsTrue(success);
                Assert.IsNotNull(series);
                Assert.AreEqual("Foo", series.MetricIdentifier.MetricId);

                Assert.AreEqual(0, telemetryCollector.Count);

                series.TrackValue(2);
                metricManager.Flush();

                Assert.AreEqual(1, telemetryCollector.Count);
                Assert.IsNotNull(telemetryCollector[0]);
                Assert.AreEqual(1, telemetryCollector[0].Data["Count"]);
                Assert.AreEqual(2.0, telemetryCollector[0].Data["Sum"]);
                Assert.AreEqual("Foo", telemetryCollector[0].MetricId);
                Assert.AreEqual(0, telemetryCollector[0].Dimensions.Count);

                Assert.ThrowsException<ArgumentException>(() => metric.TryGetDataSeries(out series, "Dim1Value"));
                Assert.ThrowsException<ArgumentException>(() => metric.TryGetDataSeries(out series, "Dim1Value"));
                Assert.ThrowsException<ArgumentException>(() => metric.TryGetDataSeries(out series, false, "Dim1Value"));

                telemetryCollector.Clear();
                MetricSeries series1, series2, series3, series4, series5, series6;

                success = metric.TryGetDataSeries(out series1, false, "Dim1Value", "Dim2Value");
                Assert.IsFalse(success);
                Assert.IsNull(series1);

                success = metric.TryGetDataSeries(out series1, "Dim1Value", "Dim2Value");
                Assert.IsTrue(success);
                Assert.IsNotNull(series1);

                success = metric.TryGetDataSeries(out series2, "Dim1Value", "Dim2Value");
                Assert.IsTrue(success);
                Assert.IsNotNull(series2);

                success = metric.TryGetDataSeries(out series3, "Dim1Value", "Dim2Value");
                Assert.IsTrue(success);
                Assert.IsNotNull(series3);

                success = metric.TryGetDataSeries(out series4, "Dim1ValueX", "Dim2ValueX");
                Assert.IsTrue(success);
                Assert.IsNotNull(series4);

                success = metric.TryGetDataSeries(out series5, "Dim1ValueX", "Dim2Value");
                Assert.IsTrue(success);
                Assert.IsNotNull(series5);

                success = metric.TryGetDataSeries(out series6, "Dim1Value", "Dim2ValueX");
                Assert.IsTrue(success);
                Assert.IsNotNull(series6);

                Assert.AreEqual(series1, series2);
                Assert.AreEqual(series1, series3);
                Assert.AreEqual(series2, series3);
                Assert.AreNotEqual(series1, series4);
                Assert.AreNotEqual(series1, series5);
                Assert.AreNotEqual(series1, series6);
                Assert.AreNotEqual(series4, series5);
                Assert.AreNotEqual(series4, series6);
                Assert.AreNotEqual(series5, series6);

                Assert.AreSame(series1, series2);
                Assert.AreSame(series1, series3);
                Assert.AreSame(series2, series3);
                Assert.AreNotSame(series1, series4);
                Assert.AreNotSame(series1, series5);
                Assert.AreNotSame(series1, series6);
                Assert.AreNotSame(series4, series5);
                Assert.AreNotSame(series4, series6);
                Assert.AreNotSame(series5, series6);

                Assert.AreEqual(0, telemetryCollector.Count);

                series6.TrackValue(60);
                series5.TrackValue(50);
                series4.TrackValue(40);
                series3.TrackValue(30);
                series2.TrackValue(20);
                series1.TrackValue(10);
                metricManager.Flush();

                Assert.AreEqual(4, telemetryCollector.Count);
                Assert.IsNotNull(telemetryCollector[0]);
                Assert.IsNotNull(telemetryCollector[1]);
                Assert.IsNotNull(telemetryCollector[2]);
                Assert.IsNotNull(telemetryCollector[3]);

                Assert.AreEqual("Foo", telemetryCollector[0].MetricId);
                Assert.AreEqual(3, telemetryCollector[0].Data["Count"]);
                Assert.AreEqual(60.0, telemetryCollector[0].Data["Sum"]);

                Assert.AreEqual(2, telemetryCollector[0].Dimensions.Count);
                Assert.IsTrue(telemetryCollector[0].Dimensions.ContainsKey("Bar"));
                Assert.AreEqual("Dim1Value", telemetryCollector[0].Dimensions["Bar"]);
                Assert.AreEqual("Dim2Value", telemetryCollector[0].Dimensions["Poo"]);

                Assert.AreEqual("Foo", telemetryCollector[1].MetricId);
                Assert.AreEqual(1, telemetryCollector[1].Data["Count"]);
                Assert.AreEqual(40.0, telemetryCollector[1].Data["Sum"]);

                Assert.AreEqual(2, telemetryCollector[1].Dimensions.Count);
                Assert.IsTrue(telemetryCollector[1].Dimensions.ContainsKey("Bar"));
                Assert.AreEqual("Dim1ValueX", telemetryCollector[1].Dimensions["Bar"]);
                Assert.AreEqual("Dim2ValueX", telemetryCollector[1].Dimensions["Poo"]);

                Assert.AreEqual("Foo", telemetryCollector[2].MetricId);
                Assert.AreEqual(1, telemetryCollector[2].Data["Count"]);
                Assert.AreEqual(50.0, telemetryCollector[2].Data["Sum"]);

                Assert.AreEqual(2, telemetryCollector[2].Dimensions.Count);
                Assert.IsTrue(telemetryCollector[2].Dimensions.ContainsKey("Bar"));
                Assert.AreEqual("Dim1ValueX", telemetryCollector[2].Dimensions["Bar"]);
                Assert.AreEqual("Dim2Value", telemetryCollector[2].Dimensions["Poo"]);

                Assert.AreEqual("Foo", telemetryCollector[3].MetricId);
                Assert.AreEqual(1, telemetryCollector[3].Data["Count"]);
                Assert.AreEqual(60.0, telemetryCollector[3].Data["Sum"]);

                Assert.AreEqual(2, telemetryCollector[3].Dimensions.Count);
                Assert.IsTrue(telemetryCollector[3].Dimensions.ContainsKey("Bar"));
                Assert.AreEqual("Dim1Value", telemetryCollector[3].Dimensions["Bar"]);
                Assert.AreEqual("Dim2ValueX", telemetryCollector[3].Dimensions["Poo"]);
            }

            TestUtil.CompleteDefaultAggregationCycle(metricManager);
        }

        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        [TestMethod]
        public void TryGetDataSeries_NotCreatingWhenLimitsAreReached()
        {
            // Removed due to duplication. We asserted this in several other tests.
        }

        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        [TestMethod]
        public void TrackValue()
        {
            MemoryMetricTelemetryPipeline telemetryCollector = new MemoryMetricTelemetryPipeline();
            MetricManager metricManager = new MetricManager(telemetryCollector);

            {
                Metric metric = InvokeMetricCtor(
                                        metricManager,
                                        metricNamespace: "ns",
                                        metricId: "Foo",
                                        dimension1Name: "D1",
                                        dimension2Name: "D2",
                                        configuration: MetricConfigurations.Common.Measurement());

                metric.TrackValue(42);
                metric.TrackValue(-100);
                metric.TrackValue(Double.NaN);
                metric.TrackValue(0.7);
                metricManager.Flush();

                Assert.AreEqual(1, telemetryCollector.Count);
                Assert.IsNotNull(telemetryCollector[0]);

                Assert.AreEqual("Foo", telemetryCollector[0].MetricId);
                Assert.AreEqual(3, telemetryCollector[0].Data["Count"]);
                Assert.AreEqual(-57.3, telemetryCollector[0].Data["Sum"]);

                Assert.AreEqual(0, telemetryCollector[0].Dimensions.Count);
            }
            telemetryCollector.Clear();
            {
                Metric metric = InvokeMetricCtor(
                                        metricManager,
                                        metricNamespace: "ns",
                                        metricId: "Foo",
                                        dimension1Name: "D1",
                                        dimension2Name: "D2",
                                        configuration: MetricConfigurations.Common.Measurement());

                metric.TrackValue("42");
                metric.TrackValue("-100");
                metric.TrackValue(null);
                Assert.ThrowsException<ArgumentException>(() => metric.TrackValue(""));
                Assert.ThrowsException<ArgumentException>(() => metric.TrackValue("karramba!"));
                metric.TrackValue("0.7");
                metricManager.Flush();

                Assert.AreEqual(1, telemetryCollector.Count);
                Assert.IsNotNull(telemetryCollector[0]);

                Assert.AreEqual("Foo", telemetryCollector[0].MetricId);
                Assert.AreEqual(3, telemetryCollector[0].Data["Count"]);
                Assert.AreEqual(-57.3, telemetryCollector[0].Data["Sum"]);

                Assert.AreEqual(0, telemetryCollector[0].Dimensions.Count);
            }

            TestUtil.CompleteDefaultAggregationCycle(metricManager);
        }

        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        [TestMethod]
        public void TrackValue_MultipleSeries()
        {
            MemoryMetricTelemetryPipeline telemetryCollector = new MemoryMetricTelemetryPipeline();
            MetricManager metricManager = new MetricManager(telemetryCollector);

            {
                MetricConfiguration config = new MetricConfiguration(
                                                    seriesCountLimit: 10,
                                                    valuesPerDimensionLimit: 2,
                                                    seriesConfig: new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));
                Metric metric = InvokeMetricCtor(
                                        metricManager,
                                        metricNamespace: "ns",
                                        metricId: "Foo",
                                        dimension1Name: "D1",
                                        dimension2Name: "D2",
                                        configuration: config);

                Assert.AreEqual(1, metric.SeriesCount);

                metric.TrackValue(42);
                Assert.AreEqual(1, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(42, "A", "X"));
                Assert.AreEqual(2, metric.SeriesCount);

                Assert.ThrowsException<ArgumentException>(() => metric.TrackValue(42, "A", ""));
                Assert.AreEqual(2, metric.SeriesCount);

                Assert.ThrowsException<ArgumentNullException>(() => metric.TrackValue(42, "A", null));
                Assert.AreEqual(2, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(42, "B", "X"));
                Assert.AreEqual(3, metric.SeriesCount);

                Assert.IsFalse(metric.TrackValue(42, "C", "X"), "Values per Dim1 limit reached.");
                Assert.AreEqual(3, metric.SeriesCount);

                Assert.IsFalse(metric.TrackValue(42, "C", "Y"), "Values per Dim1 limit reached.");
                Assert.AreEqual(3, metric.SeriesCount);

                Assert.ThrowsException<ArgumentException>(() => metric.TrackValue(42, "C", ""));
                Assert.AreEqual(3, metric.SeriesCount);

                Assert.ThrowsException<ArgumentNullException>(() => metric.TrackValue(42, "C", null));
                Assert.AreEqual(3, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(42, "A", "Y"));
                Assert.AreEqual(4, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(42, "B", "Y"));
                Assert.AreEqual(5, metric.SeriesCount);

                Assert.IsFalse(metric.TrackValue(42, "B", "Z"));
                Assert.AreEqual(5, metric.SeriesCount);

                Assert.IsFalse(metric.TrackValue(42, "A", "Z"));
                Assert.AreEqual(5, metric.SeriesCount);

                metricManager.Flush();

                Assert.AreEqual(5, telemetryCollector.Count);

                HashSet<string> results = new HashSet<string>();

                for (int i = 0; i < telemetryCollector.Count; i++)
                {
                    Assert.IsNotNull(telemetryCollector[i]);
                    MetricAggregate aggregate = telemetryCollector[i];

                    Assert.AreEqual("Foo", aggregate.MetricId);
                    Assert.AreEqual(1, aggregate.Data["Count"]);
                    Assert.AreEqual(42.0, aggregate.Data["Sum"]);

                    if (0 == aggregate.Dimensions.Count)
                    {
                        results.Add("-");
                    }
                    else if (2 == aggregate.Dimensions.Count)
                    {
                        results.Add($"{aggregate.Dimensions["D1"]}-{aggregate.Dimensions["D2"]}");
                    }
                    else
                    {
                        Assert.Fail($"Unexpected number of dimensions: {aggregate.Dimensions.Count}.");
                    }
                }

                Assert.AreEqual(5, results.Count);
                Assert.IsTrue(results.Contains("-"));
                Assert.IsTrue(results.Contains("A-X"));
                Assert.IsTrue(results.Contains("B-X"));
                Assert.IsTrue(results.Contains("A-Y"));
                Assert.IsTrue(results.Contains("B-Y"));
            }
            telemetryCollector.Clear();
            {
                MetricConfiguration config = new MetricConfiguration(
                                                    seriesCountLimit: 4,
                                                    valuesPerDimensionLimit: 25,
                                                    seriesConfig: new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));
                Metric metric = InvokeMetricCtor(
                                        metricManager,
                                        metricNamespace: "ns",
                                        metricId: "Foo",
                                        dimension1Name: "D1",
                                        dimension2Name: null,
                                        configuration: config);

                Assert.AreEqual(1, metric.SeriesCount);

                metric.TrackValue(42);
                Assert.AreEqual(1, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(42, "A"));
                Assert.AreEqual(2, metric.SeriesCount);

                Assert.ThrowsException<ArgumentException>(() => metric.TrackValue(42, ""));
                Assert.AreEqual(2, metric.SeriesCount);

                Assert.ThrowsException<ArgumentNullException>(() => metric.TrackValue(42, null));
                Assert.AreEqual(2, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(42, "B"));
                Assert.AreEqual(3, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(42, "C"));
                Assert.AreEqual(4, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(42, "C"));
                Assert.AreEqual(4, metric.SeriesCount);

                Assert.IsFalse(metric.TrackValue(42, "D"));
                Assert.AreEqual(4, metric.SeriesCount);

                Assert.IsFalse(metric.TrackValue(42, "E"));
                Assert.AreEqual(4, metric.SeriesCount);

                metricManager.Flush();

                Assert.AreEqual(4, telemetryCollector.Count);

                HashSet<string> results = new HashSet<string>();

                for (int i = 0; i < telemetryCollector.Count; i++)
                {
                    Assert.IsNotNull(telemetryCollector[i]);
                    MetricAggregate aggregate = telemetryCollector[i];

                    Assert.AreEqual("Foo", aggregate.MetricId);

                    bool isC = false;
                    if (0 == aggregate.Dimensions.Count)
                    {
                        results.Add("-");
                    }
                    else if (1 == aggregate.Dimensions.Count)
                    {
                        string dimVal = aggregate.Dimensions["D1"];
                        isC = "C".Equals(dimVal);
                        results.Add(dimVal);
                    }
                    else
                    {
                        Assert.Fail($"Unexpected number of dimensions: {aggregate.Dimensions.Count}.");
                    }

                    if (!isC)
                    {
                        Assert.AreEqual(1, aggregate.Data["Count"]);
                        Assert.AreEqual(42.0, aggregate.Data["Sum"]);
                    }
                    else
                    {
                        Assert.AreEqual(2, aggregate.Data["Count"]);
                        Assert.AreEqual(84.0, aggregate.Data["Sum"]);
                    }
                }

                Assert.AreEqual(4, results.Count);
                Assert.IsTrue(results.Contains("-"));
                Assert.IsTrue(results.Contains("A"));
                Assert.IsTrue(results.Contains("B"));
                Assert.IsTrue(results.Contains("C"));
            }
            telemetryCollector.Clear();
            {
                Metric metric = InvokeMetricCtor(
                                        metricManager,
                                        metricNamespace: "ns",
                                        metricId: "Foo",
                                        dimension1Name: null,
                                        dimension2Name: null,
                                        configuration: MetricConfigurations.Common.Measurement());

                Assert.AreEqual(1, metric.SeriesCount);

                metric.TrackValue(42);
                Assert.AreEqual(1, metric.SeriesCount);

                Assert.ThrowsException<ArgumentException>(() => metric.TrackValue(42, "A"));
                Assert.AreEqual(1, metric.SeriesCount);

                Assert.ThrowsException<ArgumentException>(() => metric.TrackValue(42, "A", "X"));
                Assert.AreEqual(1, metric.SeriesCount);

                metricManager.Flush();
            }
            telemetryCollector.Clear();
            {
                MetricConfiguration config = new MetricConfiguration(
                                                    seriesCountLimit: 10,
                                                    valuesPerDimensionLimits: new[] { 3, 1, 2 },
                                                    seriesConfig: new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));
                Metric metric = InvokeMetricCtor(
                                        metricManager,
                                        metricNamespace: "nsx",
                                        metricId: "Foo18",
                                        dimension1Name: "D1",
                                        dimension2Name: "D2",
                                        dimension3Name: "D3",
                                        dimension4Name: "D4",
                                        configuration: config);

                Assert.AreEqual(1, metric.SeriesCount);

                metric.TrackValue(42);
                Assert.AreEqual(1, metric.SeriesCount);

                Assert.ThrowsException<ArgumentException>(() => metric.TrackValue(42, "D1.A", "D2.A", "D3.A", "D4.A", "D5.A"));
                Assert.ThrowsException<ArgumentException>(() => metric.TrackValue(42, "D1.A", "D2.A", "D3.A"));

                Assert.IsTrue(metric.TrackValue(1111, "D1.A", "D2.A", "D3.A", "D4.A"));
                Assert.AreEqual(2, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(1111, "D1.A", "D2.A", "D3.A", "D4.A"));
                Assert.AreEqual(2, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(1112, "D1.A", "D2.A", "D3.A", "D4.B"));
                Assert.AreEqual(3, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(1112, "D1.A", "D2.A", "D3.A", "D4.B"));
                Assert.AreEqual(3, metric.SeriesCount);

                Assert.IsFalse(metric.TrackValue(1113, "D1.A", "D2.A", "D3.A", "D4.C"));
                Assert.AreEqual(3, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(1121, "D1.A", "D2.A", "D3.B", "D4.A"));
                Assert.AreEqual(4, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(1121, "D1.A", "D2.A", "D3.B", "D4.A"));
                Assert.AreEqual(4, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(1122, "D1.A", "D2.A", "D3.B", "D4.B"));
                Assert.AreEqual(5, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(1122, "D1.A", "D2.A", "D3.B", "D4.B"));
                Assert.AreEqual(5, metric.SeriesCount);

                Assert.IsFalse(metric.TrackValue(1123, "D1.A", "D2.A", "D3.B", "D4.C"));
                Assert.AreEqual(5, metric.SeriesCount);

                Assert.IsFalse(metric.TrackValue(1132, "D1.A", "D2.A", "D3.C", "D4.B"));
                Assert.AreEqual(5, metric.SeriesCount);

                Assert.IsFalse(metric.TrackValue(1133, "D1.A", "D2.A", "D3.C", "D4.C"));
                Assert.AreEqual(5, metric.SeriesCount);

                Assert.IsFalse(metric.TrackValue(1211, "D1.A", "D2.B", "D3.A", "D4.A"));
                Assert.AreEqual(5, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(2111, "D1.B", "D2.A", "D3.A", "D4.A"));
                Assert.AreEqual(6, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(2111, "D1.B", "D2.A", "D3.A", "D4.A"));
                Assert.AreEqual(6, metric.SeriesCount);

                Assert.IsFalse(metric.TrackValue(2111, "D1.B", "D2.B", "D3.A", "D4.A"));
                Assert.AreEqual(6, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(2112, "D1.B", "D2.A", "D3.A", "D4.B"));
                Assert.AreEqual(7, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(2112, "D1.B", "D2.A", "D3.A", "D4.B"));
                Assert.AreEqual(7, metric.SeriesCount);

                Assert.IsFalse(metric.TrackValue(2113, "D1.B", "D2.A", "D3.A", "D4.C"));
                Assert.AreEqual(7, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(2121, "D1.B", "D2.A", "D3.B", "D4.A"));
                Assert.AreEqual(8, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(2121, "D1.B", "D2.A", "D3.B", "D4.A"));
                Assert.AreEqual(8, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(2122, "D1.B", "D2.A", "D3.B", "D4.B"));
                Assert.AreEqual(9, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(2122, "D1.B", "D2.A", "D3.B", "D4.B"));
                Assert.AreEqual(9, metric.SeriesCount);

                Assert.IsFalse(metric.TrackValue(2123, "D1.B", "D2.A", "D3.B", "D4.C"));
                Assert.AreEqual(9, metric.SeriesCount);

                Assert.IsFalse(metric.TrackValue(2132, "D1.B", "D2.A", "D3.C", "D4.B"));
                Assert.AreEqual(9, metric.SeriesCount);

                Assert.IsFalse(metric.TrackValue(2133, "D1.B", "D2.A", "D3.C", "D4.C"));
                Assert.AreEqual(9, metric.SeriesCount);

                Assert.IsFalse(metric.TrackValue(2222, "D1.B", "D2.B", "D3.B", "D4.B"));
                Assert.AreEqual(9, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(3111, "D1.C", "D2.A", "D3.A", "D4.A"));
                Assert.AreEqual(10, metric.SeriesCount);

                Assert.IsTrue(metric.TrackValue(3111, "D1.C", "D2.A", "D3.A", "D4.A"));
                Assert.AreEqual(10, metric.SeriesCount);

                Assert.IsFalse(metric.TrackValue(4111, "D1.D", "D2.A", "D3.A", "D4.A"));
                Assert.AreEqual(10, metric.SeriesCount);

                metricManager.Flush();

                Assert.AreEqual(10, telemetryCollector.Count);
                Assert.AreEqual(1, telemetryCollector.Where((a) => a.Dimensions.Count == 0).Count());
                Assert.AreEqual(9, telemetryCollector.Where((a) => a.Dimensions.Count == 4).Count());

                for (int i = 0; i < telemetryCollector.Count; i++)
                {
                    Assert.IsNotNull(telemetryCollector[i]);
                    MetricAggregate aggregate = telemetryCollector[i];

                    Assert.AreEqual("Foo18", aggregate.MetricId);
                    Assert.AreEqual("nsx", aggregate.MetricNamespace);

                    if (aggregate.Dimensions.Count == 0)
                    {
                        Assert.AreEqual(1, aggregate.Data["Count"]);
                        Assert.AreEqual(42.0, aggregate.Data["Sum"]);
                    }
                    else if (aggregate.Dimensions.Count == 4)
                    {
                        Assert.AreEqual(2, aggregate.Data["Count"]);
                        Assert.AreEqual(4, aggregate.Dimensions.Count);

                        int expVal = 1000 * (1 + (int)(aggregate.Dimensions["D1"][3] - 'A'));
                        expVal += 100 * (1 + (int)(aggregate.Dimensions["D2"][3] - 'A'));
                        expVal += 10 * (1 + (int)(aggregate.Dimensions["D3"][3] - 'A'));
                        expVal += 1 * (1 + (int)(aggregate.Dimensions["D4"][3] - 'A'));
                        expVal *= 2;

                        Assert.AreEqual((double)expVal, aggregate.Data["Sum"]);
                    }
                    else
                    {
                        Assert.Fail($"Unexpected number of dimensions: {aggregate.Dimensions.Count}.");
                    }
                }
            }

            TestUtil.CompleteDefaultAggregationCycle(metricManager);
        }

        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        [TestMethod]
        public void TestEquals()
        {
            // We need to refactor this test into testing MetricIdentifier, as it now encapsulates the equality.

            MetricManager metricManager = new MetricManager(new MemoryMetricTelemetryPipeline());
            Metric metric1 = InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "ns",
                                    metricId: "Foo",
                                    dimension1Name: "D1",
                                    dimension2Name: "D2",
                                    configuration: MetricConfigurations.Common.Measurement());

            Assert.IsFalse(metric1.Identifier.Equals(null));
            Assert.IsFalse(metric1.Identifier.Equals("some object"));
            Assert.IsTrue(metric1.Identifier.Equals(metric1.Identifier));

            Metric metric2 = InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "ns",
                                    metricId: "Foo",
                                    dimension1Name: "D1",
                                    dimension2Name: "D2",
                                    configuration: MetricConfigurations.Common.Measurement());

            Assert.IsTrue(metric1.Identifier.Equals(metric2.Identifier));
            Assert.IsTrue(metric2.Identifier.Equals(metric1.Identifier));

            metric2 = InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "ns",
                                    metricId: "Foo",
                                    dimension1Name: "D1",
                                    dimension2Name: "D2x",
                                    configuration: MetricConfigurations.Common.Measurement());

            Assert.IsFalse(metric1.Identifier.Equals(metric2.Identifier));
            Assert.IsFalse(metric2.Identifier.Equals(metric1.Identifier));

            metric2 = InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "ns",
                                    metricId: "Foo",
                                    dimension1Name: "D1",
                                    dimension2Name: null,
                                    configuration: MetricConfigurations.Common.Measurement());

            Assert.IsFalse(metric1.Identifier.Equals(metric2.Identifier));
            Assert.IsFalse(metric2.Identifier.Equals(metric1.Identifier));

            metric2 = InvokeMetricCtor(
                                   metricManager,
                                   metricNamespace: "ns",
                                   metricId: "Foo",
                                   dimension1Name: null,
                                   dimension2Name: null,
                                   configuration: MetricConfigurations.Common.Measurement());

            Assert.IsFalse(metric1.Identifier.Equals(metric2.Identifier));
            Assert.IsFalse(metric2.Identifier.Equals(metric1.Identifier));

            metric2 = InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "ns",
                                    metricId: "Foo",
                                    dimension1Name: "D1x",
                                    dimension2Name: "D2",
                                    configuration: MetricConfigurations.Common.Measurement());

            Assert.IsFalse(metric1.Identifier.Equals(metric2.Identifier));
            Assert.IsFalse(metric2.Identifier.Equals(metric1.Identifier));

            metric2 = InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "ns",
                                    metricId: "Foox",
                                    dimension1Name: "D1",
                                    dimension2Name: "D2",
                                    configuration: MetricConfigurations.Common.Measurement());

            Assert.IsFalse(metric1.Identifier.Equals(metric2.Identifier));
            Assert.IsFalse(metric2.Identifier.Equals(metric1.Identifier));

            metric2 = InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "ns",
                                    metricId: "Foo",
                                    dimension1Name: "D1",
                                    dimension2Name: "D2",
                                    configuration: new MetricConfiguration(100, 10, new MetricSeriesConfigurationForMeasurement(true)));

            Assert.IsTrue(metric1.Identifier.Equals(metric2.Identifier));
            Assert.IsTrue(metric2.Identifier.Equals(metric1.Identifier));

            MetricManager anotherMetricManager = TelemetryConfiguration.Active.GetMetricManager();
            metric2 = InvokeMetricCtor(
                                    anotherMetricManager,
                                    metricNamespace: "ns",
                                    metricId: "Foo",
                                    dimension1Name: "D1",
                                    dimension2Name: "D2",
                                    configuration: MetricConfigurations.Common.Measurement());

            Assert.IsTrue(metric1.Identifier.Equals(metric2.Identifier));
            Assert.IsTrue(metric2.Identifier.Equals(metric1.Identifier));

            TestUtil.CompleteDefaultAggregationCycle(anotherMetricManager);

            TestUtil.CompleteDefaultAggregationCycle(metricManager);
        }

        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        [TestMethod]
        public void TestGetHashCode()
        {
            // We need to refactor this test into testing MetricIdentifier, as it now encapsulates the equality.

            MetricManager metricManager = new MetricManager(new MemoryMetricTelemetryPipeline());
            Metric metric1 = InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "ns",
                                    metricId: "Foo",
                                    dimension1Name: "D1",
                                    dimension2Name: "D2",
                                    configuration: MetricConfigurations.Common.Measurement());

            Assert.AreNotEqual(0, metric1.Identifier.GetHashCode());
            Assert.AreEqual(metric1.Identifier.GetHashCode(), metric1.Identifier.GetHashCode());

            Metric metric2 = InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "ns",
                                    metricId: "Foo",
                                    dimension1Name: "D1",
                                    dimension2Name: "D2",
                                    configuration: MetricConfigurations.Common.Measurement());

            Assert.AreEqual(metric1.Identifier.GetHashCode(), metric2.Identifier.GetHashCode());
            Assert.AreNotEqual(0, metric1.Identifier.GetHashCode());
            Assert.AreNotEqual(0, metric2.Identifier.GetHashCode());

            metric2 = InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "ns",
                                    metricId: "Foo",
                                    dimension1Name: "D1",
                                    dimension2Name: "D2x",
                                    configuration: MetricConfigurations.Common.Measurement());

            Assert.AreNotEqual(metric1.Identifier.GetHashCode(), metric2.Identifier.GetHashCode());
            Assert.AreNotEqual(0, metric1.Identifier.GetHashCode());
            Assert.AreNotEqual(0, metric2.Identifier.GetHashCode());

            metric2 = InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "ns",
                                    metricId: "Foo",
                                    dimension1Name: "D1",
                                    dimension2Name: null,
                                    configuration: MetricConfigurations.Common.Measurement());

            Assert.AreNotEqual(metric1.Identifier.GetHashCode(), metric2.Identifier.GetHashCode());
            Assert.AreNotEqual(0, metric1.Identifier.GetHashCode());
            Assert.AreNotEqual(0, metric2.Identifier.GetHashCode());

            metric2 = InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "ns",
                                    metricId: "Foo",
                                    dimension1Name: null,
                                    dimension2Name: null,
                                    configuration: MetricConfigurations.Common.Measurement());

            Assert.AreNotEqual(metric1.Identifier.GetHashCode(), metric2.Identifier.GetHashCode());
            Assert.AreNotEqual(0, metric1.Identifier.GetHashCode());
            Assert.AreNotEqual(0, metric2.Identifier.GetHashCode());

            metric2 = InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "ns",
                                    metricId: "Foo",
                                    dimension1Name: "D1x",
                                    dimension2Name: "D2",
                                    configuration: MetricConfigurations.Common.Measurement());

            Assert.AreNotEqual(metric1.Identifier.GetHashCode(), metric2.Identifier.GetHashCode());
            Assert.AreNotEqual(0, metric1.Identifier.GetHashCode());
            Assert.AreNotEqual(0, metric2.Identifier.GetHashCode());

            metric2 = InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "ns",
                                    metricId: "Foox",
                                    dimension1Name: "D1",
                                    dimension2Name: "D2",
                                    configuration: MetricConfigurations.Common.Measurement());

            Assert.AreNotEqual(metric1.Identifier.GetHashCode(), metric2.Identifier.GetHashCode());
            Assert.AreNotEqual(0, metric1.Identifier.GetHashCode());
            Assert.AreNotEqual(0, metric2.Identifier.GetHashCode());

            metric2 = InvokeMetricCtor(
                                    metricManager,
                                    metricNamespace: "ns",
                                    metricId: "Foo",
                                    dimension1Name: "D1",
                                    dimension2Name: "D2",
                                    configuration: new MetricConfiguration(100, 10, new MetricSeriesConfigurationForMeasurement(true)));

            Assert.AreEqual(metric1.Identifier.GetHashCode(), metric2.Identifier.GetHashCode());
            Assert.AreNotEqual(0, metric1.Identifier.GetHashCode());
            Assert.AreNotEqual(0, metric2.Identifier.GetHashCode());

            MetricManager anotherMetricManager = TelemetryConfiguration.Active.GetMetricManager();
            metric2 = InvokeMetricCtor(
                                    anotherMetricManager,
                                    metricNamespace: "ns",
                                    metricId: "Foo",
                                    dimension1Name: "D1",
                                    dimension2Name: "D2",
                                    configuration: MetricConfigurations.Common.Measurement());

            Assert.AreEqual(metric1.Identifier.GetHashCode(), metric2.Identifier.GetHashCode());
            Assert.AreNotEqual(0, metric1.Identifier.GetHashCode());
            Assert.AreNotEqual(0, metric2.Identifier.GetHashCode());

            TestUtil.CompleteDefaultAggregationCycle(anotherMetricManager);

            TestUtil.CompleteDefaultAggregationCycle(metricManager);
        }


        private static Metric InvokeMetricCtor(
                                        MetricManager metricManager,
                                        string metricNamespace,
                                        string metricId,
                                        string dimension1Name,
                                        string dimension2Name,
                                        MetricConfiguration configuration)
        {
            // Metric ctor is private..

            Metric metric = new Metric(
                                    metricManager,
                                    new MetricIdentifier(metricNamespace, metricId, dimension1Name, dimension2Name),
                                    configuration);
            return metric;
        }

        private static Metric InvokeMetricCtor(
                                       MetricManager metricManager,
                                       string metricNamespace,
                                       string metricId,
                                       string dimension1Name,
                                       string dimension2Name,
                                       string dimension3Name,
                                       string dimension4Name,
                                       MetricConfiguration configuration)
        {
            // Metric ctor is private..

            Metric metric = new Metric(
                                    metricManager,
                                    new MetricIdentifier(metricNamespace, metricId, dimension1Name, dimension2Name, dimension3Name, dimension4Name),
                                    configuration);
            return metric;
        }
    }
}
#pragma warning restore 612, 618  // obsolete TelemetryConfigration.Active