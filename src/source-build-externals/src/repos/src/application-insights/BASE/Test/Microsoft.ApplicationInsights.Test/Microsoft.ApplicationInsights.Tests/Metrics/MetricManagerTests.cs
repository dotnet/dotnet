using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.ApplicationInsights.Metrics.Extensibility;
using Microsoft.ApplicationInsights.Metrics.TestUtility;


namespace Microsoft.ApplicationInsights.Metrics
{ 
    /// <summary />
    [TestClass]
    public class MetricManagerTests
    {
        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        [TestMethod]
        public void Ctor()
        {
            Assert.ThrowsException<ArgumentNullException>( () => new MetricManager(telemetryPipeline: null));

            var manager = new MetricManager(new MemoryMetricTelemetryPipeline());
            Assert.IsNotNull(manager);

            TestUtil.CompleteDefaultAggregationCycle(manager);
        }

        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        [TestMethod]
        public void CreateNewSeries()
        {
            var manager = new MetricManager(new MemoryMetricTelemetryPipeline());

            IMetricSeriesConfiguration config = new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false);

            Assert.ThrowsException<ArgumentNullException>( () => manager.CreateNewSeries("ns", null, config) );
            Assert.ThrowsException<ArgumentNullException>( () => manager.CreateNewSeries("ns", "Foo Bar", null) );

            MetricSeries series = manager.CreateNewSeries("NS", "Foo Bar", config);
            Assert.IsNotNull(series);

            Assert.AreEqual(config, series.GetConfiguration());
            Assert.AreSame(config, series.GetConfiguration());

            Assert.AreEqual("NS", series.MetricIdentifier.MetricNamespace);
            Assert.AreEqual("Foo Bar", series.MetricIdentifier.MetricId);

            TestUtil.CompleteDefaultAggregationCycle(manager);
        }

        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        [TestMethod]
        public void Flush()
        {
            {
                var metricsCollector = new MemoryMetricTelemetryPipeline();
                var manager = new MetricManager(metricsCollector);
                manager.Flush();

                Assert.AreEqual(0, metricsCollector.Count);
                TestUtil.CompleteDefaultAggregationCycle(manager);
            }
            {
                var metricsCollector = new MemoryMetricTelemetryPipeline();
                var manager = new MetricManager(metricsCollector);

                IMetricSeriesConfiguration measurementConfig = new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false);

                MetricSeries series1 = manager.CreateNewSeries("Test Metrics", "Measurement 1", measurementConfig);
                MetricSeries series2 = manager.CreateNewSeries("Test Metrics", "Measurement 2", measurementConfig);

                series1.TrackValue(1);
                series1.TrackValue(1);
                series1.TrackValue(1);

                series2.TrackValue(-1);
                series2.TrackValue(-1);
                series2.TrackValue(-1);

                manager.Flush();

                Assert.AreEqual(2, metricsCollector.Count);

                Assert.AreEqual(1, metricsCollector.Where( (item) => item.MetricId.Equals("Measurement 1") ).Count());
                Assert.AreEqual("Test Metrics", (metricsCollector.First( (item) => item.MetricId.Equals("Measurement 1") )).MetricNamespace);
                Assert.AreEqual(3, (metricsCollector.First( (item) => item.MetricId.Equals("Measurement 1") )).Data["Count"]);
                Assert.AreEqual(3.0, (metricsCollector.First( (item) => item.MetricId.Equals("Measurement 1") )).Data["Sum"]);

                Assert.AreEqual(1, metricsCollector.Where( (item) => item.MetricId.Equals("Measurement 2") ).Count());
                Assert.AreEqual("Test Metrics", (metricsCollector.First( (item) => item.MetricId.Equals("Measurement 2") )).MetricNamespace);
                Assert.AreEqual(3, (metricsCollector.First( (item) => item.MetricId.Equals("Measurement 2") )).Data["Count"]);
                Assert.AreEqual(-3.0, (metricsCollector.First( (item) => item.MetricId.Equals("Measurement 2") )).Data["Sum"]);

                metricsCollector.Clear();
                Assert.AreEqual(0, metricsCollector.Count);

                manager.Flush();

                Assert.AreEqual(0, metricsCollector.Count);

                TestUtil.CompleteDefaultAggregationCycle(manager);
            }

        }
    }
}
