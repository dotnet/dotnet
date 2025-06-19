using System.Diagnostics;

namespace Microsoft.ApplicationInsights
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    using Extensibility.Implementation;
    using TestFramework;
    

    /// <summary>
    /// Tests corresponding to TelemetryClientExtension methods.
    /// </summary>
    [TestClass]
    public class TelemetryClientExtensionAsyncTests
    {
        private TelemetryClient telemetryClient;
        private List<ITelemetry> sendItems;
        private object sendItemsLock;

        [TestInitialize]
        public void TestInitialize()
        {
            var configuration = new TelemetryConfiguration();
            this.sendItems = new List<ITelemetry>();
            this.sendItemsLock = new object();
            configuration.TelemetryChannel = new StubTelemetryChannel { OnSend = item =>
            {
                lock (this.sendItemsLock)
                {
                    this.sendItems.Add(item);
                    Monitor.Pulse(this.sendItemsLock);
                }
            }};
            configuration.InstrumentationKey = Guid.NewGuid().ToString();
            configuration.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
            this.telemetryClient = new TelemetryClient(configuration);
            CallContextHelpers.SaveOperationContext(null);
        }

        /// <summary>
        /// Ensure that context being propagated via async/await.
        /// </summary>
        [TestMethod]
        public async Task ContextPropagatesThroughAsyncAwait()
        {
            await this.TestAsync();
        }

        /// <summary>
        /// Actual async test method.
        /// </summary>
        /// <returns>Task to await.</returns>
        public async Task TestAsync()
        {
            Activity activity;
            using (var op = this.telemetryClient.StartOperation<RequestTelemetry>("request"))
            {
                activity = Activity.Current;
                
                var id1 = Thread.CurrentThread.ManagedThreadId;
                this.telemetryClient.TrackTrace("trace1");

                //HttpClient client = new HttpClient();
                await Task.Delay(TimeSpan.FromMilliseconds(100));//client.GetStringAsync("http://bing.com");

                var id2 = Thread.CurrentThread.ManagedThreadId;
                this.telemetryClient.TrackTrace("trace2");

                Assert.AreNotEqual(id1, id2);
            }

            Assert.AreEqual(3, this.sendItems.Count);
            var requestTelemetry = ((RequestTelemetry) this.sendItems[this.sendItems.Count - 1]);
            var requestId = requestTelemetry.Id;
            var requestOperationId = requestTelemetry.Context.Operation.Id;
            Assert.IsFalse(string.IsNullOrEmpty(requestTelemetry.Id));
            Assert.IsFalse(string.IsNullOrEmpty(requestOperationId));

            foreach (var item in this.sendItems)
            {
                if (item is TraceTelemetry)
                {
                    Assert.AreEqual(requestId, item.Context.Operation.ParentId);
                    Assert.AreEqual(requestOperationId, item.Context.Operation.Id);
                }
                else
                {
                    if (activity.IdFormat == ActivityIdFormat.W3C)
                    {
                        Assert.AreEqual(activity.TraceId.ToHexString(), requestOperationId);
                        Assert.AreEqual(activity.SpanId.ToHexString(), requestId);
                    }
                    else
                    {
                        Assert.AreEqual(activity.RootId, requestOperationId);
                        Assert.AreEqual(activity.Id, requestId);
                    }

                    Assert.IsNull(item.Context.Operation.ParentId);
                }
            }
        }

        /// <summary>
        /// Ensure that context being propagated via Begin/End.
        /// </summary>
        [TestMethod, Timeout(2000)]
        public void ContextPropagatesThroughBeginEnd()
        {
            var op = this.telemetryClient.StartOperation<RequestTelemetry>("request");
            var activity = Activity.Current;
            var id1 = Thread.CurrentThread.ManagedThreadId;
            int id2 = 0;
            this.telemetryClient.TrackTrace("trace1");

            var result = Task.Delay(TimeSpan.FromMilliseconds(50)).ContinueWith((t) =>
            {
                id2 = Thread.CurrentThread.ManagedThreadId;
                this.telemetryClient.TrackTrace("trace2");

                this.telemetryClient.StopOperation(op);
            });

            do
            {
                lock (this.sendItemsLock)
                {
                    if (this.sendItems.Count < 3)
                    {
                        Monitor.Wait(this.sendItemsLock, 50); // We will rely on the overall test timeout to break the wait in case of failure
                    }
                }
            } while (this.sendItems.Count < 3);

            Assert.AreNotEqual(id1, id2);
            Assert.AreEqual(3, this.sendItems.Count);
            var requestTelemetry = ((RequestTelemetry)this.sendItems[this.sendItems.Count - 1]);
            var requestId = requestTelemetry.Id;
            var requestOperationId = requestTelemetry.Context.Operation.Id;
            Assert.IsFalse(string.IsNullOrEmpty(requestTelemetry.Id));
            Assert.IsFalse(string.IsNullOrEmpty(requestOperationId));

            foreach (var item in this.sendItems)
            {
                if (item is TraceTelemetry)
                {
                    Assert.AreEqual(requestId, item.Context.Operation.ParentId);
                    Assert.AreEqual(requestOperationId, item.Context.Operation.Id);
                }
                else
                {
                    if (activity.IdFormat == ActivityIdFormat.W3C)
                    {
                        Assert.AreEqual(activity.TraceId.ToHexString(), requestOperationId);
                        Assert.AreEqual(activity.SpanId.ToHexString(), requestId);
                    }
                    else
                    {
                        Assert.AreEqual(activity.RootId, requestOperationId);
                        Assert.AreEqual(activity.Id, requestId);
                    }

                    Assert.IsNull(item.Context.Operation.ParentId);
                }
            }
        }
    }
}
