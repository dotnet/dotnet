using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Microsoft.ApplicationInsights.Metrics.TestUtility;
using Microsoft.ApplicationInsights.Extensibility;

namespace Microsoft.ApplicationInsights.Metrics.Extensibility
{
    /// <summary />
    [TestClass]
    public class TelemetryClientExtensionsTests
    {
        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        [TestMethod]
        public void GetMetricManager()
        {
            TelemetryConfiguration telemetryPipeline1 = TestUtil.CreateAITelemetryConfig();
            TelemetryConfiguration telemetryPipeline2 = TestUtil.CreateAITelemetryConfig();
            TelemetryClient client1 = new TelemetryClient(telemetryPipeline1);
            TelemetryClient client2 = new TelemetryClient(telemetryPipeline1);

            MetricManager managerP11 = telemetryPipeline1.GetMetricManager();
            MetricManager managerP12 = telemetryPipeline1.GetMetricManager();
            MetricManager managerP21 = telemetryPipeline2.GetMetricManager();
            MetricManager managerP22 = telemetryPipeline2.GetMetricManager();

            MetricManager managerCp11 = client1.GetMetricManager(MetricAggregationScope.TelemetryConfiguration);
            MetricManager managerCp12 = client1.GetMetricManager(MetricAggregationScope.TelemetryConfiguration);
            MetricManager managerCp21 = client2.GetMetricManager(MetricAggregationScope.TelemetryConfiguration);
            MetricManager managerCp22 = client2.GetMetricManager(MetricAggregationScope.TelemetryConfiguration);

            MetricManager managerCc11 = client1.GetMetricManager(MetricAggregationScope.TelemetryClient);
            MetricManager managerCc12 = client1.GetMetricManager(MetricAggregationScope.TelemetryClient);
            MetricManager managerCc21 = client2.GetMetricManager(MetricAggregationScope.TelemetryClient);
            MetricManager managerCc22 = client2.GetMetricManager(MetricAggregationScope.TelemetryClient);

            Assert.IsNotNull(managerP11);
            Assert.IsNotNull(managerP12);
            Assert.IsNotNull(managerP21);
            Assert.IsNotNull(managerP22);
            Assert.IsNotNull(managerCp11);
            Assert.IsNotNull(managerCp12);
            Assert.IsNotNull(managerCp21);
            Assert.IsNotNull(managerCp22);
            Assert.IsNotNull(managerCc11);
            Assert.IsNotNull(managerCc12);
            Assert.IsNotNull(managerCc21);
            Assert.IsNotNull(managerCc22);

            Assert.AreSame(managerP11, managerP12);
            Assert.AreSame(managerP21, managerP22);
            Assert.AreNotSame(managerP11, managerP21);

            Assert.AreSame(managerP11, managerCp11);
            Assert.AreSame(managerP11, managerCp12);
            Assert.AreSame(managerP11, managerCp21);
            Assert.AreSame(managerP11, managerCp22);

            Assert.AreSame(managerCc11, managerCc12);
            Assert.AreSame(managerCc21, managerCc22);

            Assert.AreNotSame(managerCc11, managerCc21);
            Assert.AreNotSame(managerP11, managerCc11);
            Assert.AreNotSame(managerP11, managerCc21);

            Assert.AreNotSame(managerP21, managerCc11);
            Assert.AreNotSame(managerP21, managerCc21);

            TestUtil.CompleteDefaultAggregationCycle(
                        managerP11,
                        managerP21,
                        managerCc11,
                        managerCc21);

            telemetryPipeline1.Dispose();
            telemetryPipeline2.Dispose();
        }
    }
}
