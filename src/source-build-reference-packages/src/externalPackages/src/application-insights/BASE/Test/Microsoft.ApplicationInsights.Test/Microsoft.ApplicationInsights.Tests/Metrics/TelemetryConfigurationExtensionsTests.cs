using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Metrics;
using Microsoft.ApplicationInsights.Metrics.TestUtility;
using Microsoft.ApplicationInsights.DataContracts;

namespace SomeCustomerNamespace
{
    /// <summary />
    [TestClass]
    public class TelemetryConfigurationExtensionsTests
    {
        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        [TestMethod]
        public void Metrics_DefaultPipeline()
        {
            TelemetryConfiguration defaultTelemetryPipeline = TelemetryConfiguration.CreateDefault();
            using (defaultTelemetryPipeline)
            {
                Metrics_SpecifiedPipeline(defaultTelemetryPipeline);
                TestUtil.CompleteDefaultAggregationCycle(defaultTelemetryPipeline.GetMetricManager());
            }
        }

        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        [TestMethod]
        public void Metrics_CustomPipeline()
        {
            TelemetryConfiguration defaultTelemetryPipeline = TelemetryConfiguration.CreateDefault();
            using (defaultTelemetryPipeline)
            using (TelemetryConfiguration customTelemetryPipeline1 = TestUtil.CreateAITelemetryConfig())
            using (TelemetryConfiguration customTelemetryPipeline2 = TestUtil.CreateAITelemetryConfig())
            {
                Assert.IsFalse(Object.ReferenceEquals(defaultTelemetryPipeline, customTelemetryPipeline1));
                Assert.IsFalse(Object.ReferenceEquals(defaultTelemetryPipeline, customTelemetryPipeline2));
                Assert.IsFalse(Object.ReferenceEquals(customTelemetryPipeline1, customTelemetryPipeline2));

                MetricManager managerDef = defaultTelemetryPipeline.GetMetricManager();
                MetricManager managerCust1 = customTelemetryPipeline1.GetMetricManager();
                MetricManager managerCust2 = customTelemetryPipeline2.GetMetricManager();

                Assert.IsNotNull(managerDef);
                Assert.IsNotNull(managerCust1);
                Assert.IsNotNull(managerCust2);

                Assert.AreNotEqual(managerDef, managerCust1);
                Assert.AreNotEqual(managerDef, managerCust2);
                Assert.AreNotEqual(managerCust1, managerCust2);

                Assert.AreNotSame(managerDef, managerCust1);
                Assert.AreNotSame(managerDef, managerCust2);
                Assert.AreNotSame(managerCust1, managerCust2);

                Assert.IsFalse(Object.ReferenceEquals(managerDef, managerCust1));
                Assert.IsFalse(Object.ReferenceEquals(managerDef, managerCust2));
                Assert.IsFalse(Object.ReferenceEquals(managerCust1, managerCust2));

                Metrics_SpecifiedPipeline(customTelemetryPipeline1);
                Metrics_SpecifiedPipeline(customTelemetryPipeline2);

                TestUtil.CompleteDefaultAggregationCycle(managerDef);
                TestUtil.CompleteDefaultAggregationCycle(managerCust1);
                TestUtil.CompleteDefaultAggregationCycle(managerCust2);
            }
        }

        private static void Metrics_SpecifiedPipeline(TelemetryConfiguration telemetryPipeline)
        { 
            telemetryPipeline.InstrumentationKey = Guid.NewGuid().ToString("D");

            MetricManager manager1 = telemetryPipeline.GetMetricManager();
            Assert.IsNotNull(manager1);

            MetricManager manager2 = telemetryPipeline.GetMetricManager();
            Assert.IsNotNull(manager2);

            Assert.AreEqual(manager1, manager2);
            Assert.AreSame(manager1, manager2);
            Assert.IsTrue(Object.ReferenceEquals(manager1, manager2));

            StubApplicationInsightsTelemetryChannel telemetryCollector = new StubApplicationInsightsTelemetryChannel();
            telemetryPipeline.TelemetryChannel = telemetryCollector;
            Assert.AreSame(telemetryCollector, telemetryPipeline.TelemetryChannel);

            //CollectingTelemetryInitializer telemetryCollector = new CollectingTelemetryInitializer();
            //defaultTelemetryPipeline.TelemetryInitializers.Add(coll);

            IMetricSeriesConfiguration seriesConfig = new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false);

            manager1.CreateNewSeries("ns", "Metric A", seriesConfig).TrackValue(42);
            manager1.CreateNewSeries("ns", "Metric A", seriesConfig).TrackValue("18");
            manager2.CreateNewSeries("ns", "Metric A", seriesConfig).TrackValue(10000);
            manager2.CreateNewSeries("ns", "Metric B", seriesConfig).TrackValue(-0.001);
            manager1.Flush();

            Assert.AreEqual(4, telemetryCollector.TelemetryItems.Count);

            Assert.IsInstanceOfType(telemetryCollector.TelemetryItems[0], typeof(MetricTelemetry));
            Assert.AreEqual("Metric B", ((MetricTelemetry) telemetryCollector.TelemetryItems[0]).Name);
            Assert.AreEqual(1, ((MetricTelemetry) telemetryCollector.TelemetryItems[0]).Count);
            Assert.AreEqual(-0.001, ((MetricTelemetry) telemetryCollector.TelemetryItems[0]).Sum);

            Assert.IsInstanceOfType(telemetryCollector.TelemetryItems[1], typeof(MetricTelemetry));
            Assert.AreEqual("Metric A", ((MetricTelemetry) telemetryCollector.TelemetryItems[1]).Name);
            Assert.AreEqual(1, ((MetricTelemetry) telemetryCollector.TelemetryItems[1]).Count);
            Assert.AreEqual(10000, ((MetricTelemetry) telemetryCollector.TelemetryItems[1]).Sum);

            Assert.IsInstanceOfType(telemetryCollector.TelemetryItems[2], typeof(MetricTelemetry));
            Assert.AreEqual("Metric A", ((MetricTelemetry) telemetryCollector.TelemetryItems[2]).Name);
            Assert.AreEqual(1, ((MetricTelemetry) telemetryCollector.TelemetryItems[2]).Count);
            Assert.AreEqual(18, ((MetricTelemetry) telemetryCollector.TelemetryItems[2]).Sum);

            Assert.IsInstanceOfType(telemetryCollector.TelemetryItems[3], typeof(MetricTelemetry));
            Assert.AreEqual("Metric A", ((MetricTelemetry) telemetryCollector.TelemetryItems[3]).Name);
            Assert.AreEqual(1, ((MetricTelemetry) telemetryCollector.TelemetryItems[3]).Count);
            Assert.AreEqual(42, ((MetricTelemetry) telemetryCollector.TelemetryItems[3]).Sum);
        }

        //private class CollectingTelemetryInitializer : ITelemetryInitializer
        //{
        //    private List<ITelemetry> _telemetryItems = new List<ITelemetry>();

        //    public IList<ITelemetry> TelemetryItems { get { return _telemetryItems; } }

        //    public void Initialize(ITelemetry telemetry)
        //    {
        //        _telemetryItems.Add(telemetry);
        //    }
        //}
    }
}
