using EventCounterCollector.Tests;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.EventCounterCollector;
using Microsoft.ApplicationInsights.Extensibility.EventCounterCollector.Implementation;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EventCounterCollector.Tests
{
    [TestClass]
    public class EventCounterCollectionModuleTests
    {
        private string TestEventCounterSourceName = "Microsoft-ApplicationInsights-Extensibility-EventCounterCollector.Tests.TestEventCounter";
        private string TestEventCounterName1 = "mycountername1";

        [TestMethod]
        [TestCategory("EventCounter")]
        public void WarnsIfNoCountersConfigured()
        {
            using (var eventListener = new EventCounterCollectorDiagnosticListener())
            using (var module = new EventCounterCollectionModule())
            {
                ConcurrentQueue<ITelemetry> itemsReceived = new ConcurrentQueue<ITelemetry>();
                module.Initialize(GetTestTelemetryConfiguration(itemsReceived));
                Assert.IsTrue(CheckEventReceived(eventListener.EventsReceived, nameof(EventCounterCollectorEventSource.ModuleIsBeingInitializedEvent))); 
                Assert.IsTrue(CheckEventReceived(eventListener.EventsReceived, nameof(EventCounterCollectorEventSource.EventCounterCollectorNoCounterConfigured)));
            }
        }

        [TestMethod]
        [TestCategory("EventCounter")]
        public void IgnoresUnconfiguredEventCounter()
        {
            // ARRANGE
            const int refreshTimeInSecs = 1;
            ConcurrentQueue<ITelemetry> itemsReceived = new ConcurrentQueue<ITelemetry>();

            using (var eventListener = new EventCounterCollectorDiagnosticListener())
            using (var module = new EventCounterCollectionModule(refreshTimeInSecs))
            {
                module.Counters.Add(new EventCounterCollectionRequest() { EventSourceName = this.TestEventCounterSourceName, EventCounterName = this.TestEventCounterName1 });
                module.Initialize(GetTestTelemetryConfiguration(itemsReceived));

                // ACT                
                // These will fire counters 'mycountername2' which is not in the configured list.
                TestEventCounter.Log.SampleCounter2(1500);
                TestEventCounter.Log.SampleCounter2(400);
                
                // Wait at least for refresh time.
                Task.Delay(((int)refreshTimeInSecs * 1000) + 500).Wait();

                // VALIDATE
                Assert.IsTrue(CheckEventReceived(eventListener.EventsReceived, nameof(EventCounterCollectorEventSource.IgnoreEventWrittenAsCounterNotInConfiguredList)));
            }
        }

        [TestMethod]
        [TestCategory("EventCounter")]
        public void ValidateSingleEventCounterCollection()
        {
            // ARRANGE
            const int refreshTimeInSecs = 1;
            ConcurrentQueue<ITelemetry> itemsReceived = new ConcurrentQueue<ITelemetry>();
            string expectedName = this.TestEventCounterSourceName + "|" + this.TestEventCounterName1;
            string expectedMetricNamespace = String.Empty;
            double expectedMetricValue = (1000 + 1500 + 1500 + 400) / 4;
            int expectedMetricCount = 4;

            using (var module = new EventCounterCollectionModule(refreshTimeInSecs))
            {
                module.Counters.Add(new EventCounterCollectionRequest() {EventSourceName = this.TestEventCounterSourceName, EventCounterName = this.TestEventCounterName1 });
                module.Initialize(GetTestTelemetryConfiguration(itemsReceived));

                // ACT
                // Making 4 calls with 1000, 1500, 1500, 400 value, leading to an average of 1100.
                TestEventCounter.Log.SampleCounter1(1000);
                TestEventCounter.Log.SampleCounter1(1500);
                TestEventCounter.Log.SampleCounter1(1500);
                TestEventCounter.Log.SampleCounter1(400);

                // Wait at least for refresh time.
                Task.Delay(((int) refreshTimeInSecs * 1000) + 500).Wait();

                PrintTelemetryItems(itemsReceived);

                // VALIDATE
                ValidateTelemetry(itemsReceived, expectedName, expectedMetricNamespace, expectedMetricValue, expectedMetricCount);

                // Wait another refresh interval to receive more events, but with zero as counter values.
                // as nobody is publishing events.
                Task.Delay(((int)refreshTimeInSecs * 1000)).Wait();                
                Assert.IsTrue(itemsReceived.Count >= 1);
                PrintTelemetryItems(itemsReceived);                
                ValidateTelemetry(itemsReceived, expectedName, expectedMetricNamespace, 0.0, 0);
            }
        }

        [TestMethod]
        [TestCategory("EventCounter")]
        public void ValidateConfiguredNamingOptions()
        {
            // ARRANGE
            const int refreshTimeInSecs = 1;
            ConcurrentQueue<ITelemetry> itemsReceived = new ConcurrentQueue<ITelemetry>();
            string expectedName = this.TestEventCounterName1;
            string expectedMetricNamespace = this.TestEventCounterSourceName;
            double expectedMetricValue = 1000;
            int expectedMetricCount = 1;

            using (var module = new EventCounterCollectionModule(refreshTimeInSecs))
            {
                module.UseEventSourceNameAsMetricsNamespace = true;
                module.Counters.Add(new EventCounterCollectionRequest() { EventSourceName = this.TestEventCounterSourceName, EventCounterName = this.TestEventCounterName1 });
                module.Initialize(GetTestTelemetryConfiguration(itemsReceived));

                // ACT
                // Making a call with 1000
                TestEventCounter.Log.SampleCounter1(1000);

                // Wait at least for refresh time.
                Task.Delay(((int)refreshTimeInSecs * 1000) + 500).Wait();

                PrintTelemetryItems(itemsReceived);

                // VALIDATE
                ValidateTelemetry(itemsReceived, expectedName, expectedMetricNamespace, expectedMetricValue, expectedMetricCount);
            }
        }

        private void ValidateTelemetry(ConcurrentQueue<ITelemetry> metricTelemetries, string expectedName, string expectedMetricNamespace, double expectedSum, double expectedCount)
        {
            double sum = 0.0;
            int count = 0;

            while (metricTelemetries.TryDequeue(out ITelemetry telemetry))
            {
                var metricTelemetry = telemetry as MetricTelemetry;
                count = count + metricTelemetry.Count.Value;

                if (!double.IsNaN(metricTelemetry.Sum)) // TODO: WHY IS SUM NaN ?
                {
                    sum += metricTelemetry.Sum;
                }

                Assert.IsTrue(metricTelemetry.Context.GetInternalContext().SdkVersion.StartsWith("evtc"));
                Assert.AreEqual(expectedName, metricTelemetry.Name);
                Assert.AreEqual(expectedMetricNamespace, metricTelemetry.MetricNamespace);
                Assert.IsFalse((telemetry as ISupportProperties).Properties.ContainsKey("CustomPerfCounter"));
            }

            Assert.AreEqual(expectedSum, sum);
            Assert.AreEqual(expectedCount, count);
        }

        private void PrintTelemetryItems(ConcurrentQueue<ITelemetry> telemetry)
        {
            Trace.WriteLine("Received count:" + telemetry.Count);
            foreach (var item in telemetry)
            {
                if (item is MetricTelemetry metric)
                {
                    Trace.WriteLine("Metric.Name:" + metric.Name);
                    Trace.WriteLine("Metric.MetricNamespace:" + metric.MetricNamespace);
                    Trace.WriteLine("Metric.Sum:" + metric.Sum);
                    Trace.WriteLine("Metric.Count:" + metric.Count);
                    Trace.WriteLine("Metric.Timestamp:" + metric.Timestamp);
                    Trace.WriteLine("Metric.Sdk:" + metric.Context.GetInternalContext().SdkVersion);
                    foreach (var prop in metric.Properties)
                    {
                        Trace.WriteLine("Metric. Prop:" + "Key:" + prop.Key + "Value:" + prop.Value);
                    }
                }
                Trace.WriteLine("======================================");
            }
        }

        private bool CheckEventReceived(ConcurrentQueue<string> allEvents, string expectedEvent)
        {
            bool found = false;
            foreach(var evt in allEvents)
            {
                if(evt.Equals(expectedEvent))
                {
                    found = true;
                    break;
                }
            }

            return found;
        }

        private TelemetryConfiguration GetTestTelemetryConfiguration(ConcurrentQueue<ITelemetry> itemsReceived)
        {
            var configuration = new TelemetryConfiguration();
            configuration.InstrumentationKey = "testkey";
            configuration.TelemetryChannel = new TestChannel(itemsReceived);
            return configuration;
        }
    }
}
