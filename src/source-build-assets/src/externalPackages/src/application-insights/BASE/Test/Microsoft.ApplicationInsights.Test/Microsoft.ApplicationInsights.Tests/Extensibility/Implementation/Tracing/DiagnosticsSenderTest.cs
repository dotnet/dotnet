namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System.Diagnostics.Tracing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;

    [TestClass]
    public class F5DiagnosticsSenderTest
    {
        [TestMethod]
        public void TestLogMessage()
        {
            var senderMock = new DiagnosticsSenderMock();
            var evt = new TraceEvent
            {
                MetaData = new EventMetaData
                {
                    EventId = 10,
                    Keywords = 0x20,
                    Level = EventLevel.Warning,
                    MessageFormat = "Error occurred at {0}, {1}"
                },
                Payload = new[] { "My function", "some failure" }
            };

            senderMock.Send(evt);
            Assert.AreEqual(1, senderMock.Messages.Count);
            Assert.AreEqual("Error occurred at My function, some failure", senderMock.Messages[0]);
        }

        [TestMethod]
        public void TestLogMessageWithEmptyPayload()
        {
            var senderMock = new DiagnosticsSenderMock();
            var evt = new TraceEvent
            {
                MetaData = new EventMetaData
                {
                    EventId = 10,
                    Keywords = 0x20,
                    Level = EventLevel.Warning,
                    MessageFormat = "Error occurred"
                },
                Payload = null
            };

            senderMock.Send(evt);
            Assert.AreEqual(1, senderMock.Messages.Count);
            Assert.AreEqual("Error occurred", senderMock.Messages[0]);
        }
    }
}
