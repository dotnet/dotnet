namespace Microsoft.ApplicationInsights.Channel
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Extensibility;
    using System.Net.Http;
    using System.Threading;

    public class InMemoryTransmitterTests
    {
        [TestClass]
        public class SendingInterval
        {
            [TestMethod]
            public void DefaultValueIsAppropriateForProductionEnvironmentAndUnitTests()
            {
                var transmitter = new InMemoryTransmitter(new TelemetryBuffer());
                Assert.AreEqual(TimeSpan.FromSeconds(30), transmitter.SendingInterval);
            }

            [TestMethod]
            public void CanBeChangedByChannelToTunePerformance()
            {
                var transmitter = new InMemoryTransmitter(new TelemetryBuffer());

                var expectedValue = TimeSpan.FromSeconds(42);
                transmitter.SendingInterval = expectedValue;

                Assert.AreEqual(expectedValue, transmitter.SendingInterval);
            }

            private class TelemetryBufferWithInternalOperationValidation : TelemetryBuffer
            {
                public bool WasCalled = false;

                public override IEnumerable<ITelemetry> Dequeue()
                {
                    Assert.IsTrue(SdkInternalOperationsMonitor.IsEntered());
                    HttpClient client = new HttpClient();
                    var task = client.GetStringAsync("http://bing.com").ContinueWith((result) => { Assert.IsTrue(SdkInternalOperationsMonitor.IsEntered()); });

                    task.Wait();

                    WasCalled = true;
                    return base.Dequeue();
                }
            }

            [TestMethod]
            public void SendingLogicMarkedAsInternalSdkOperation()
            {
                var buffer = new TelemetryBufferWithInternalOperationValidation();
                using (var transmitter = new InMemoryTransmitter(buffer))
                {
                    buffer.OnFull();

                    for (int i = 0; i < 10; i++)
                    {
                        if (buffer.WasCalled)
                        {
                            break;
                        }

                        Thread.Sleep(TimeSpan.FromSeconds(1));
                    }

                    Assert.IsTrue(buffer.WasCalled);
                }
            }

            [TestMethod]
            public void FlushMarkedAsInternalSdkOperation()
            {
                var buffer = new TelemetryBufferWithInternalOperationValidation();
                var transmitter = new InMemoryTransmitter(buffer);
                transmitter.Flush(TimeSpan.FromSeconds(1));

                for (int i = 0; i < 10; i++)
                {
                    if (buffer.WasCalled)
                    {
                        break;
                    }

                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }

                Assert.IsTrue(buffer.WasCalled);
            }
        }
    }
}
