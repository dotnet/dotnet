namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Linq;

    using Microsoft.ApplicationInsights.Extensibility.Filtering;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CollectionConfigurationAccumulatorTests
    {
        [TestMethod]
        public void CollectionConfigurationAccumulatorPreparesMetricAccumulatorsTest()
        {
            // ARRANGE
            CollectionConfigurationError[] error;
            var metricInfo = new CalculatedMetricInfo()
                                 {
                                     Id = "Metric1",
                                     TelemetryType = TelemetryType.Request,
                                     Projection = "Name",
                                     Aggregation = AggregationType.Min,
                                     FilterGroups = new FilterConjunctionGroupInfo[0]
                                 };

            var collectionConfigurationInfo = new CollectionConfigurationInfo() { Metrics = new[] { metricInfo } };
            var collectionConfiguration = new CollectionConfiguration(collectionConfigurationInfo, out error, new ClockMock());

            // ACT
            var accumulator = new CollectionConfigurationAccumulator(collectionConfiguration);

            // ASSERT
            Assert.AreSame(collectionConfiguration, accumulator.CollectionConfiguration);
            Assert.AreEqual("Metric1", accumulator.MetricAccumulators.Single().Key);
        }
    }
}