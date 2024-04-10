namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Sampling
{
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class SamplingRateStoreTest
    {
        [TestMethod]
        public void StoreIsInitializedWith100Rate()
        {
            var store = new SamplingRateStore();
            Assert.AreEqual(100, store.GetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.Request));
            Assert.AreEqual(100, store.GetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.RemoteDependency));
            Assert.AreEqual(100, store.GetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.Event));
            Assert.AreEqual(100, store.GetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.Exception));
            Assert.AreEqual(100, store.GetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.Message));
            Assert.AreEqual(100, store.GetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.PageView));
        }

        [TestMethod]
        public void StoreAllowsToSetTheRatePerType()
        {
            var store = new SamplingRateStore();
            store.SetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.Request, 10);
            store.SetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.RemoteDependency, 11);
            store.SetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.Event, 12);
            store.SetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.Exception, 13);
            store.SetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.Message, 14);
            store.SetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.PageView, 15);

            Assert.AreEqual(10, store.GetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.Request));
            Assert.AreEqual(11, store.GetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.RemoteDependency));
            Assert.AreEqual(12, store.GetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.Event));
            Assert.AreEqual(13, store.GetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.Exception));
            Assert.AreEqual(14, store.GetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.Message));
            Assert.AreEqual(15, store.GetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.PageView));
        }

        [TestMethod]
        public void StoreAllowsToOverrideTheRatePerType()
        {
            var store = new SamplingRateStore();
            store.SetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.Request, 10);
            store.SetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.RemoteDependency, 11);
            store.SetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.Event, 12);
            store.SetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.Exception, 13);
            store.SetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.Message, 14);
            store.SetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.PageView, 15);

            store.SetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.Request, 1);
            store.SetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.RemoteDependency, 2);
            store.SetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.Event, 3);
            store.SetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.Exception, 4);
            store.SetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.Message, 5);
            store.SetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.PageView, 6);

            Assert.AreEqual(1, store.GetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.Request));
            Assert.AreEqual(2, store.GetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.RemoteDependency));
            Assert.AreEqual(3, store.GetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.Event));
            Assert.AreEqual(4, store.GetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.Exception));
            Assert.AreEqual(5, store.GetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.Message));
            Assert.AreEqual(6, store.GetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.PageView));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StoreThrowsOnSavingUnsupportedType()
        {
            var store = new SamplingRateStore();
            store.SetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.Metric, 10);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StoreThrowsOnSavingUnknownType()
        {
            var store = new SamplingRateStore();
            store.SetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.None, 10);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StoreThrowsOnSavingMultiTypedItem()
        {
            var store = new SamplingRateStore();
            store.SetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.Request | SamplingTelemetryItemTypes.RemoteDependency, 100);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StoreThrowsForUnsupportedTypes()
        {
            var store = new SamplingRateStore();
            store.GetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.Metric);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StoreThrowsForUnknownType()
        {
            var store = new SamplingRateStore();
            store.GetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.None);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StoreThrowsForMultiTypeGet()
        {
            var store = new SamplingRateStore();
            store.GetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.Request | SamplingTelemetryItemTypes.RemoteDependency);
        }
    }
}
