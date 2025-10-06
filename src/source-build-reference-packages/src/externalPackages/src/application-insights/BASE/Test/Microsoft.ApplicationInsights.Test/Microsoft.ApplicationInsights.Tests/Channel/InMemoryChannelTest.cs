namespace Microsoft.ApplicationInsights.Channel
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using System.Linq;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    
    using DataContracts;
    using System.Threading.Tasks;
    using System.Threading;

    [TestClass]
    public class InMemoryChannelTest
    {
        [TestMethod]
        public void WhenSendIsCalledTheEventIsBeingQueuedInTheBuffer()
        {
            var telemetryBuffer = new TelemetryBuffer();
            var channel = new InMemoryChannel(telemetryBuffer, new InMemoryTransmitter(telemetryBuffer));
            var sentTelemetry = new StubTelemetry();
            sentTelemetry.Context.InstrumentationKey = Guid.NewGuid().ToString();

            channel.Send(sentTelemetry);
            IEnumerable<ITelemetry> telemetries = telemetryBuffer.Dequeue();

            Assert.AreEqual(1, telemetries.Count());
            Assert.AreSame(sentTelemetry, telemetries.First());
        }

        [TestMethod]
        public void TelemetryWithNoInstrumentationKeyIsDropped()
        {
            var telemetryBuffer = new TelemetryBuffer();
            var channel = new InMemoryChannel(telemetryBuffer, new InMemoryTransmitter(telemetryBuffer));
            var sentTelemetry = new StubTelemetry();
            // No instrumentation key

            using (TestEventListener listener = new TestEventListener())
            {
                listener.EnableEvents(CoreEventSource.Log, EventLevel.Verbose);

                channel.Send(sentTelemetry);
                IEnumerable<ITelemetry> telemetries = telemetryBuffer.Dequeue();

                Assert.IsNull(telemetries);

                var expectedMessage = listener.Messages.First();
                Assert.AreEqual(35, expectedMessage.EventId);
            }
        }

#if (!NETCOREAPP) // This constant is defined for all versions of NetCore https://docs.microsoft.com/en-us/dotnet/core/tutorials/libraries#how-to-multitarget

        [Ignore("This test is failing intermittently and needs to be investigated. ~Timothy")]
        [TestMethod]
        public void FlushCanBeAborted()
        {
            var telemetryBuffer = new TelemetryBuffer();
            var channel = new InMemoryChannel(telemetryBuffer, new InMemoryTransmitter(telemetryBuffer))
            {
                SendingInterval = TimeSpan.FromDays(1),
                EndpointAddress = "http://localhost/bad"
            };
            
            var telemetry = new TraceTelemetry("test");
            telemetry.Context.InstrumentationKey = Guid.NewGuid().ToString();
            channel.Send(telemetry); // Send telemetry so that it sets next send interval and does not interfere with Flush
            channel.Flush();

            telemetry = new TraceTelemetry("test");
            telemetry.Context.InstrumentationKey = Guid.NewGuid().ToString();
            channel.Send(telemetry);

            using (TestEventListener listener = new TestEventListener())
            {
                listener.EnableEvents(CoreEventSource.Log, EventLevel.Warning);
                channel.Flush(TimeSpan.FromSeconds(1));

                var expectedMessage = listener.Messages.First();
                Assert.AreEqual(24, expectedMessage.EventId);
            }
        }
#endif

        [TestMethod]
        public async Task FlushAsyncRespectsCancellationToken()
        {
            var telemetryBuffer = new TelemetryBuffer();
            var channel = new InMemoryChannel(telemetryBuffer, new InMemoryTransmitter(telemetryBuffer));
            var sentTelemetry = new StubTelemetry();
            sentTelemetry.Context.InstrumentationKey = Guid.NewGuid().ToString();

            channel.Send(sentTelemetry);
            await Assert.ThrowsExceptionAsync<TaskCanceledException>(() => channel.FlushAsync(new CancellationToken(true)));
        }
    }
}
