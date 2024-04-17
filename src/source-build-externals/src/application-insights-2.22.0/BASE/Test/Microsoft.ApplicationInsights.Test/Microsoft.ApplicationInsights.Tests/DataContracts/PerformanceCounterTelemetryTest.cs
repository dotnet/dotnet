namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    using CompareLogic = KellermanSoftware.CompareNetObjects.CompareLogic;

    [TestClass]
    public class PerformanceCounterTelemetryTest
    {
#pragma warning disable 618

        [TestMethod]
        public void SerializeWritesNullValuesAsExpectedByEndpoint()
        {
            PerformanceCounterTelemetry original = new PerformanceCounterTelemetry();
            original.CategoryName = null;
            original.CounterName = null;
            original.InstanceName = null;
            ((ITelemetry)original).Sanitize();
            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.MetricData>(original);

            Assert.AreEqual(2, item.data.baseData.ver);
        }

        [TestMethod]
        public void ContextPropertiesUsedAsTelemetryItemProperties()
        {
            PerformanceCounterTelemetry item = new PerformanceCounterTelemetry();

            item.Context.Properties["a"] = "b";

            Assert.AreEqual("b", item.Properties["a"]);
        }

        [TestMethod]
        public void PerformanceCounterTelemetryDeepCloneCopiesAllProperties()
        {
            PerformanceCounterTelemetry item = new PerformanceCounterTelemetry("someCategory", "someCounter", "an instance", 15.7);
            item.Timestamp = DateTimeOffset.Now;
            item.Properties.Add("p1", "p1Val");
            item.Extension = new MyTestExtension();

            PerformanceCounterTelemetry other = (PerformanceCounterTelemetry)item.DeepClone();

            CompareLogic deepComparator = new CompareLogic();

            var result = deepComparator.Compare(item, other);
            Assert.IsTrue(result.AreEqual, result.DifferencesString);
        }

        [TestMethod]
        public void PerformanceCounterTelemetryDeepCloneWithNullExtensionDoesNotThrow()
        {
            var telemetry = new PerformanceCounterTelemetry();
            // Extension is not set, means it'll be null.
            // Validate that cloning with null Extension does not throw.
            var other = telemetry.DeepClone();
        }

#pragma warning restore 618
    }
}
