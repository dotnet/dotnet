namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DiagnoisticsEventThrottlingTest
    {        
        private const int ThrottleAfterCountAsOne = 1;
        private const int FirstTraceEventId = 1;
        private const int SecondTraceEventId = 2;

        private const int EventThrottlingEnabledKeywords = 0;
        private const int EventThrottlingNotEnabledKeywords = DiagnoisticsEventThrottlingDefaults.KeywordsExcludedFromEventThrottling;

        private readonly DiagnoisticsEventThrottling throttling = new DiagnoisticsEventThrottling(ThrottleAfterCountAsOne);

        [TestMethod]
        public void TestThrottleEvent()
        {   
            bool justExceededThreshold;

            Assert.IsFalse(
                this.throttling.ThrottleEvent(FirstTraceEventId, EventThrottlingEnabledKeywords, out justExceededThreshold), 
                "First call causes event throttling");
            Assert.IsFalse(justExceededThreshold, "First call causes event justExceededThreshold=true");

            Assert.IsTrue(
                this.throttling.ThrottleEvent(FirstTraceEventId, EventThrottlingEnabledKeywords, out justExceededThreshold), 
                "Second call does not cause event throttling");
            Assert.IsTrue(justExceededThreshold, "Second call causes event justExceededThreshold=false");

            Assert.IsTrue(
                this.throttling.ThrottleEvent(FirstTraceEventId, EventThrottlingEnabledKeywords, out justExceededThreshold),
                "Third call does not cause event throttling");
            Assert.IsFalse(justExceededThreshold, "Third call causes event justExceededThreshold=true");

            var snapshot = this.throttling.CollectSnapshot();
            Assert.AreEqual(1, snapshot.Count, "Unexpected snapshot state");

            Assert.IsTrue(snapshot.ContainsKey(FirstTraceEventId), "Expected event record {0} does not exist", FirstTraceEventId);

            var eventRecord = snapshot[FirstTraceEventId];
            Assert.AreEqual(3, eventRecord.ExecCount, "Unexpected eventRecord.executedCount value");
        }

        [TestMethod]
        public void TestThrottleMultiplyEvents()
        {
            bool justExceededThreshold;

            Assert.IsFalse(
                this.throttling.ThrottleEvent(FirstTraceEventId, EventThrottlingEnabledKeywords, out justExceededThreshold), 
                "First call causes event throttling");
            Assert.IsTrue(
                this.throttling.ThrottleEvent(FirstTraceEventId, EventThrottlingEnabledKeywords, out justExceededThreshold), 
                "Second call does not cause event throttling");

            Assert.IsFalse(
                this.throttling.ThrottleEvent(SecondTraceEventId, EventThrottlingEnabledKeywords, out justExceededThreshold), 
                "First call causes event throttling");
            Assert.IsTrue(
                this.throttling.ThrottleEvent(SecondTraceEventId, EventThrottlingEnabledKeywords, out justExceededThreshold), 
                "Second call does not cause event throttling");
            Assert.IsTrue(
                this.throttling.ThrottleEvent(SecondTraceEventId, EventThrottlingEnabledKeywords, out justExceededThreshold), 
                "Third call does not cause event throttling");

            var snapshot = this.throttling.CollectSnapshot();

            Assert.AreEqual(2, snapshot.Count, "Unexpected snapshot state");

            Assert.IsTrue(snapshot.ContainsKey(FirstTraceEventId), "Expected event record {0} does not exist", FirstTraceEventId);
            Assert.AreEqual(2, snapshot[FirstTraceEventId].ExecCount, "Unexpected eventRecord.executedCount value");

            Assert.IsTrue(snapshot.ContainsKey(SecondTraceEventId), "Expected event record {0} does not exist", SecondTraceEventId);
            Assert.AreEqual(3, snapshot[SecondTraceEventId].ExecCount, "Unexpected eventRecord.executedCount value");
        }

        [TestMethod]
        public void TestNotThrottleEventsWithSpecialKeywords()
        {
            bool justExceededThreshold;

            Assert.IsFalse(
                this.throttling.ThrottleEvent(SecondTraceEventId, EventThrottlingNotEnabledKeywords, out justExceededThreshold),
                "First call causes event throttling");
            Assert.IsFalse(
                this.throttling.ThrottleEvent(SecondTraceEventId, EventThrottlingNotEnabledKeywords, out justExceededThreshold),
                "Second call causes event throttling");
            Assert.IsFalse(
                this.throttling.ThrottleEvent(SecondTraceEventId, EventThrottlingNotEnabledKeywords, out justExceededThreshold),
                "Third call causes event throttling");

            Assert.AreEqual(0, this.throttling.CollectSnapshot().Count, "Unexpected snapshot state");
        }

        [TestMethod]
        public void TestStateCleanupAfterCollectingSnapshot()
        {
            bool justExceededThreshold;

            this.throttling.ThrottleEvent(FirstTraceEventId, EventThrottlingEnabledKeywords, out justExceededThreshold);
            this.throttling.ThrottleEvent(FirstTraceEventId, EventThrottlingEnabledKeywords, out justExceededThreshold);
            this.throttling.ThrottleEvent(FirstTraceEventId, EventThrottlingEnabledKeywords, out justExceededThreshold);
            this.throttling.ThrottleEvent(FirstTraceEventId, EventThrottlingEnabledKeywords, out justExceededThreshold);
            this.throttling.ThrottleEvent(FirstTraceEventId, EventThrottlingEnabledKeywords, out justExceededThreshold);

            var snapshot1 = this.throttling.CollectSnapshot();
            Assert.AreEqual(1, snapshot1.Count, "Unexpected first snapshot state");

            this.throttling.ThrottleEvent(SecondTraceEventId, EventThrottlingEnabledKeywords, out justExceededThreshold);
            var snapshot2 = this.throttling.CollectSnapshot();
            Assert.AreEqual(1, snapshot2.Count, "Unexpected second snapshot state");

            Assert.AreNotSame(snapshot1, snapshot2, "Snapshot objects are same");

            Assert.AreEqual(0, this.throttling.CollectSnapshot().Count, "Unexpected state after snapshot cleanup");
        }
    }
}