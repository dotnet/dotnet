using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Microsoft.ApplicationInsights.Metrics.Extensibility;
using Microsoft.ApplicationInsights.Metrics.TestUtility;
using Microsoft.ApplicationInsights.Extensibility;

namespace Microsoft.ApplicationInsights.Metrics
{
    /// <summary />
    [TestClass]
    public class MetricExtensionsTests
    {
        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        [TestMethod]
        public void GetConfiguration()
        {
            TelemetryConfiguration pipeline = TestUtil.CreateAITelemetryConfig();
            TelemetryClient client = new TelemetryClient(pipeline);

            {
                Metric metric = client.GetMetric("CowsSold");
                Assert.AreEqual(MetricConfigurations.Common.Measurement(), metric.GetConfiguration());
                Assert.AreSame(MetricConfigurations.Common.Measurement(), metric.GetConfiguration());
            }
            {
                Metric metric = client.GetMetric("HorsesSold", MetricConfigurations.Common.Measurement());
                Assert.AreEqual(MetricConfigurations.Common.Measurement(), metric.GetConfiguration());
                Assert.AreSame(MetricConfigurations.Common.Measurement(), metric.GetConfiguration());
            }
            {
                MetricConfiguration config = new MetricConfiguration(10, 5, new MetricSeriesConfigurationForMeasurement(false));
                Metric metric = client.GetMetric("ChickensSold", config);
                Assert.AreEqual(config, metric.GetConfiguration());
                Assert.AreSame(config, metric.GetConfiguration());
            }

            TestUtil.CompleteDefaultAggregationCycle(pipeline.GetMetricManager());
            pipeline.Dispose();
        }

        ///// <summary />
        //[TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        //[TestMethod]
        //public void GetMetricManager()
        //{
        //    {
        //        TelemetryClient client = new TelemetryClient();
        //        Metric metric = client.GetMetric("CowsSold");
        //        Assert.AreSame(TelemetryConfiguration.Active.GetMetricManager(), metric.GetMetricManager());
        //        TestUtil.CompleteDefaultAggregationCycle(TelemetryConfiguration.Active.GetMetricManager());
        //    }
        //    {
        //        TelemetryConfiguration telemetryPipeline = TestUtil.CreateAITelemetryConfig();
        //        TelemetryClient client = new TelemetryClient(telemetryPipeline);
        //        Metric metric = client.GetMetric("CowsSold");
        //        Assert.AreSame(telemetryPipeline.GetMetricManager(), metric.GetMetricManager());
        //        TestUtil.CompleteDefaultAggregationCycle(telemetryPipeline.GetMetricManager());
        //    }
        //}

    }
}
