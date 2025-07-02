namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;

    [TestClass]
    public sealed class DiagnoisticsEventThrottlingManagerTest
    {
        private const uint SampleIntervalInMinutes = 10;

        private const int SampleEventId = 1;
        private const int SampleKeywords = 0;

        private const int ThrottlingStartedEventId = 4;
        private const int ThrottlingResetEventId = 5;

        private DiagnoisticsEventThrottlingMock throttleAllContainer;

        private DiagnoisticsEventThrottlingMock notThrottleContainer;

        private DiagnoisticsEventThrottlingSchedulerMock scheduler;

        private DiagnoisticsEventThrottlingManager<DiagnoisticsEventThrottlingMock> throttleFirstCallManager;
        private DiagnoisticsEventThrottlingManager<DiagnoisticsEventThrottlingMock> notThrottleManager;

        private DiagnosticsEventCollectingMock sender;

        private DiagnosticsListener listener;

        [TestInitialize]
        public void TestInitialize()
        {
            this.throttleAllContainer = new DiagnoisticsEventThrottlingMock(
                throttleAll: true,
                signalJustExceeded: true,
                sampleCounters: new Dictionary<int, DiagnoisticsEventCounters>());

            this.notThrottleContainer = new DiagnoisticsEventThrottlingMock(
                throttleAll: false,
                signalJustExceeded: false,
                sampleCounters: new Dictionary<int, DiagnoisticsEventCounters>());

            this.scheduler = new DiagnoisticsEventThrottlingSchedulerMock();

            this.sender = new DiagnosticsEventCollectingMock();

            this.throttleFirstCallManager = new DiagnoisticsEventThrottlingManager<DiagnoisticsEventThrottlingMock>(
                this.throttleAllContainer,
                this.scheduler,
                SampleIntervalInMinutes);

            this.notThrottleManager = new DiagnoisticsEventThrottlingManager<DiagnoisticsEventThrottlingMock>(
                this.notThrottleContainer,
                this.scheduler,
                SampleIntervalInMinutes);

            this.listener = new DiagnosticsListener(new List<IDiagnosticsSender> { this.sender });

            this.listener.LogLevel = EventLevel.Verbose;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.listener.Dispose();

            // Give some time as listener is not instantly disposed.
            Thread.Sleep(1000);
        }

        [TestMethod]
        public void TestEventThrottlingOnFirstCall()
        {
            Assert.IsTrue(this.throttleFirstCallManager.ThrottleEvent(SampleEventId, SampleKeywords));

            var snapshot = this.throttleAllContainer.CollectSnapshot();
            Assert.AreEqual(1, snapshot.Count, "Unexpected number of snapshot records");
            Assert.AreEqual(SampleEventId, snapshot.First().Key, "Incorrect event id in snapshot item");

            Assert.AreEqual(1, this.sender.EventList.Count, "Unexpected count of trace records");

            var evt = this.sender.EventList.First();
            Assert.AreEqual(ThrottlingStartedEventId, evt.MetaData.EventId, "Unexpected trace event id");

            Assert.AreEqual(2, evt.Payload.Length, "Unexpected payload items count");

            Assert.AreEqual(SampleEventId.ToString(), evt.Payload[0], "Unexpected event Id in payload item");

            Assert.IsNotNull(evt.Payload[1], "Payload item[1] is null");
            Assert.IsInstanceOfType(evt.Payload[1], typeof(string), "Payload item[1] has wrong type");

            Assert.AreEqual(
                "Diagnostics event throttling has been started for the event {0}",
                evt.MetaData.MessageFormat,
                "Unexpected event message format");
        }

        [TestMethod]
        public void TestEventThrottlingIsNotHappeningOnFirstCall()
        {
            Assert.IsFalse(this.notThrottleManager.ThrottleEvent(SampleEventId, SampleKeywords));

            Assert.AreEqual(0, this.sender.EventList.Count, "Unexpected count of trace records");
        }

        [TestMethod]
        public void TestResetThrottling()
        {
            this.notThrottleManager.ThrottleEvent(SampleEventId, SampleKeywords);

            // executing throttling reset routine
            foreach (var item in this.scheduler.Items)
            {
                item.Action();
            }

            Assert.AreEqual(1, this.sender.EventList.Count, "Unexpected count of trace records");

            var evt = this.sender.EventList.First();
            Assert.AreEqual(ThrottlingResetEventId, evt.MetaData.EventId, "Unexpected trace event id");

            Assert.AreEqual(3, evt.Payload.Length, "Unexpected payload items count");

            Assert.IsNotNull(evt.Payload[0], "Payload item[0] is null");
            Assert.IsInstanceOfType(evt.Payload[0], typeof(int), "Payload item[0] has wrong type");
            var eventId = Convert.ToInt32(evt.Payload[0], CultureInfo.InvariantCulture);
            Assert.AreEqual(SampleEventId, eventId, "Unexpected event Id in payload item");

            Assert.IsNotNull(evt.Payload[1], "Payload item[1] is null");
            Assert.IsInstanceOfType(evt.Payload[1], typeof(int), "Payload item[1] has wrong type");
            var executionCount = Convert.ToInt32(evt.Payload[1], CultureInfo.InvariantCulture);
            Assert.AreEqual(1, executionCount, "Unexpected execution count for the event");

            Assert.IsNotNull(evt.Payload[2], "Payload item[2] is null");
            Assert.IsInstanceOfType(evt.Payload[2], typeof(string), "Payload item[2] has wrong type");

            Assert.AreEqual(
                "Diagnostics event throttling has been reset for the event {0}, event was fired {1} times during last interval",
                evt.MetaData.MessageFormat,
                "Unexpected event message format");
        }
    }
}