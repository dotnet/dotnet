namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class TelemetryExtensionsTests
    {
        class NonSerializableTelemetry : ITelemetry
        {
            public DateTimeOffset Timestamp { get; set; }

            public TelemetryContext Context { get; set; }

            public IExtension Extension { get; set; }
            public string Sequence { get; set; }

            public ITelemetry DeepClone()
            {
                return new NonSerializableTelemetry();
            }

            public void Sanitize()
            {}

            public void SerializeData(ISerializationWriter serializationWriter)
            {}
        }

        [TestMethod]
        public void CanSetEnvelopeNameForSupportedTypes()
        {
            string testEnvelopeName = "Non_Standard*Envelope.Name";

            var at = new AvailabilityTelemetry();
            var dt = new DependencyTelemetry();
            var et = new EventTelemetry();
            var ext = new ExceptionTelemetry();
            var mt = new MetricTelemetry();
            var pvpt = new PageViewPerformanceTelemetry();
            var pvt = new PageViewTelemetry();
            var rt = new RequestTelemetry();
#pragma warning disable CS0618 // Type or member is obsolete
            var pct = new PerformanceCounterTelemetry();
            var sst = new SessionStateTelemetry();
#pragma warning restore CS0618 // Type or member is obsolete

            Assert.IsTrue(at.TrySetEnvelopeName(testEnvelopeName));
            Assert.IsTrue(dt.TrySetEnvelopeName(testEnvelopeName));
            Assert.IsTrue(et.TrySetEnvelopeName(testEnvelopeName));
            Assert.IsTrue(ext.TrySetEnvelopeName(testEnvelopeName));
            Assert.IsTrue(mt.TrySetEnvelopeName(testEnvelopeName));
            Assert.IsTrue(pvpt.TrySetEnvelopeName(testEnvelopeName));
            Assert.IsTrue(pvt.TrySetEnvelopeName(testEnvelopeName));
            Assert.IsTrue(rt.TrySetEnvelopeName(testEnvelopeName));
            Assert.IsTrue(pct.TrySetEnvelopeName(testEnvelopeName));
            Assert.IsTrue(sst.TrySetEnvelopeName(testEnvelopeName));

            Assert.AreEqual(testEnvelopeName, at.GetEnvelopeName());
            Assert.AreEqual(testEnvelopeName, dt.GetEnvelopeName());
            Assert.AreEqual(testEnvelopeName, et.GetEnvelopeName());
            Assert.AreEqual(testEnvelopeName, ext.GetEnvelopeName());
            Assert.AreEqual(testEnvelopeName, mt.GetEnvelopeName());
            Assert.AreEqual(testEnvelopeName, pvpt.GetEnvelopeName());
            Assert.AreEqual(testEnvelopeName, pvt.GetEnvelopeName());
            Assert.AreEqual(testEnvelopeName, rt.GetEnvelopeName());
            Assert.AreEqual(testEnvelopeName, pct.Data.GetEnvelopeName());
            Assert.AreEqual(testEnvelopeName, sst.Data.GetEnvelopeName());
        }

        [TestMethod]
        public void TrySetEnvelopeNameReturnsFalseForUnsupportedTypes()
        {
            var nst = new NonSerializableTelemetry();
            Assert.IsFalse(nst.TrySetEnvelopeName("Any")); // Returns false, NonSerializableTelemetry does not implement IAiSerializableTelemetry
        }

        [TestMethod]        
        public void GetEnvelopeNameReturnsDefaultForUnsupportedTypes()
        {
            var nst = new NonSerializableTelemetry();
            Assert.AreEqual("Event", nst.GetEnvelopeName());
        }
    }
}
