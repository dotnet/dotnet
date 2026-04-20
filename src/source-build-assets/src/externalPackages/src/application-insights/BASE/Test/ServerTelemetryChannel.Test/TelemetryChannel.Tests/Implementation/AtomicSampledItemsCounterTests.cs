namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Sampling
{
    using System;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AtomicSampledItemsCounterTest
    {
        [TestMethod]
        public void CountersAreInitializedWith0Values()
        {
            var counters = new AtomicSampledItemsCounter();
            Assert.AreEqual(0, counters.GetItems(SamplingTelemetryItemTypes.Request));
            Assert.AreEqual(0, counters.GetItems(SamplingTelemetryItemTypes.RemoteDependency));
            Assert.AreEqual(0, counters.GetItems(SamplingTelemetryItemTypes.Event));
            Assert.AreEqual(0, counters.GetItems(SamplingTelemetryItemTypes.Message));
            Assert.AreEqual(0, counters.GetItems(SamplingTelemetryItemTypes.PageView));
            Assert.AreEqual(0, counters.GetItems(SamplingTelemetryItemTypes.Exception));

            // Counter only stores several supported types, others are combined together
            Assert.AreEqual(0, counters.GetItems(SamplingTelemetryItemTypes.Availability));
            Assert.AreEqual(0, counters.GetItems(SamplingTelemetryItemTypes.SessionState));
        }

        [TestMethod]
        public void CounterAllowsoAddItemsPerType()
        {
            var counters = new AtomicSampledItemsCounter();
            counters.AddItems(SamplingTelemetryItemTypes.Request, 10);
            counters.AddItems(SamplingTelemetryItemTypes.RemoteDependency, 11);
            counters.AddItems(SamplingTelemetryItemTypes.Event, 12);
            counters.AddItems(SamplingTelemetryItemTypes.Exception, 13);
            counters.AddItems(SamplingTelemetryItemTypes.Message, 14);
            counters.AddItems(SamplingTelemetryItemTypes.PageView, 15);

            // Counter only stores several supported types, others are combined together
            counters.AddItems(SamplingTelemetryItemTypes.Availability, 16);
            counters.AddItems(SamplingTelemetryItemTypes.SessionState, 17);

            Assert.AreEqual(10, counters.GetItems(SamplingTelemetryItemTypes.Request));
            Assert.AreEqual(11, counters.GetItems(SamplingTelemetryItemTypes.RemoteDependency));
            Assert.AreEqual(12, counters.GetItems(SamplingTelemetryItemTypes.Event));
            Assert.AreEqual(13, counters.GetItems(SamplingTelemetryItemTypes.Exception));
            Assert.AreEqual(14, counters.GetItems(SamplingTelemetryItemTypes.Message));
            Assert.AreEqual(15, counters.GetItems(SamplingTelemetryItemTypes.PageView));

            // Counter only stores several supported types, others are combined together
            Assert.AreEqual(33, counters.GetItems(SamplingTelemetryItemTypes.Availability));
            Assert.AreEqual(33, counters.GetItems(SamplingTelemetryItemTypes.SessionState));

            counters.AddItems(SamplingTelemetryItemTypes.Request, 20);
            counters.AddItems(SamplingTelemetryItemTypes.RemoteDependency, 19);
            counters.AddItems(SamplingTelemetryItemTypes.Event, 18);
            counters.AddItems(SamplingTelemetryItemTypes.Exception, 17);
            counters.AddItems(SamplingTelemetryItemTypes.Message, 16);
            counters.AddItems(SamplingTelemetryItemTypes.PageView, 15);

            // Counter only stores several supported types, others are combined together
            counters.AddItems(SamplingTelemetryItemTypes.Availability, 17);
            counters.AddItems(SamplingTelemetryItemTypes.SessionState, 16);

            Assert.AreEqual(30, counters.GetItems(SamplingTelemetryItemTypes.Request));
            Assert.AreEqual(30, counters.GetItems(SamplingTelemetryItemTypes.RemoteDependency));
            Assert.AreEqual(30, counters.GetItems(SamplingTelemetryItemTypes.Event));
            Assert.AreEqual(30, counters.GetItems(SamplingTelemetryItemTypes.Exception));
            Assert.AreEqual(30, counters.GetItems(SamplingTelemetryItemTypes.Message));
            Assert.AreEqual(30, counters.GetItems(SamplingTelemetryItemTypes.PageView));

            // Counter only stores several supported types, others are combined together
            Assert.AreEqual(66, counters.GetItems(SamplingTelemetryItemTypes.Availability));
            Assert.AreEqual(66, counters.GetItems(SamplingTelemetryItemTypes.SessionState));
        }

        [TestMethod]
        public void CounterAllowsToClearItemsPerType()
        {
            var counters = new AtomicSampledItemsCounter();
            counters.AddItems(SamplingTelemetryItemTypes.Request, 10);
            counters.AddItems(SamplingTelemetryItemTypes.RemoteDependency, 11);
            counters.AddItems(SamplingTelemetryItemTypes.Event, 12);
            counters.AddItems(SamplingTelemetryItemTypes.Exception, 13);
            counters.AddItems(SamplingTelemetryItemTypes.Message, 14);
            counters.AddItems(SamplingTelemetryItemTypes.PageView, 15);

            // Counter only stores several supported types, others are combined together
            counters.AddItems(SamplingTelemetryItemTypes.Availability, 16);
            counters.AddItems(SamplingTelemetryItemTypes.SessionState, 17);

            counters.ClearItems(SamplingTelemetryItemTypes.Request);
            counters.ClearItems(SamplingTelemetryItemTypes.RemoteDependency);
            counters.ClearItems(SamplingTelemetryItemTypes.Event);
            counters.ClearItems(SamplingTelemetryItemTypes.Exception);
            counters.ClearItems(SamplingTelemetryItemTypes.Message);
            counters.ClearItems(SamplingTelemetryItemTypes.PageView);

            // Counter only stores several supported types, others are combined together, cleaning one will clean the others
            counters.ClearItems(SamplingTelemetryItemTypes.Availability);

            Assert.AreEqual(0, counters.GetItems(SamplingTelemetryItemTypes.Request));
            Assert.AreEqual(0, counters.GetItems(SamplingTelemetryItemTypes.RemoteDependency));
            Assert.AreEqual(0, counters.GetItems(SamplingTelemetryItemTypes.Event));
            Assert.AreEqual(0, counters.GetItems(SamplingTelemetryItemTypes.Exception));
            Assert.AreEqual(0, counters.GetItems(SamplingTelemetryItemTypes.Message));
            Assert.AreEqual(0, counters.GetItems(SamplingTelemetryItemTypes.PageView));
            Assert.AreEqual(0, counters.GetItems(SamplingTelemetryItemTypes.SessionState));
        }
    }
}
