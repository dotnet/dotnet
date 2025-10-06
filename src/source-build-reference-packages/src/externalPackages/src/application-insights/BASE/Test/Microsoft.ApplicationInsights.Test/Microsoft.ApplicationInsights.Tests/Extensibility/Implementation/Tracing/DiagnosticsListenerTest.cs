#pragma warning disable 612, 618  // obsolete TelemetryConfigration.Active
namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Tracing.Mocks;

    [TestClass]
    public class DiagnosticsListenerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorThrowsArgumentException()
        {
            var listener = new DiagnosticsListener(null);
        }

        [TestMethod]
        public void TestEventSending()
        {
            var senderMock = new DiagnosticsSenderMock();
            var senders = new List<IDiagnosticsSender> { senderMock };
            using (var listener = new DiagnosticsListener(senders))
            {
                listener.LogLevel = EventLevel.Verbose;
                CoreEventSource.Log.LogVerbose("failure");
            }

            Assert.AreEqual(1, senderMock.Messages.Count);
            Assert.AreEqual("[msg=Log verbose];[msg=failure]", senderMock.Messages[0]);
        }

        [TestMethod]
        public void TestListenerWithDifferentSeverity()
        {
            // Ensure there are no left-over DiagnosticTelemetryModules
            // from previous tests that will mess up this one.
            TelemetryConfiguration.Active.Dispose();
            var modules = TelemetryModules.Instance.Modules;
            if (modules != null)
            {
                foreach (var module in modules)
                {
                    (module as IDisposable)?.Dispose();
                }

                modules.Clear();
            }

            var senderMock = new DiagnosticsSenderMock();
            var senders = new List<IDiagnosticsSender> { senderMock };
            using (var listener = new DiagnosticsListener(senders))
            {
                const EventKeywords AllKeyword = (EventKeywords)(-1);
                Assert.IsTrue(CoreEventSource.Log.IsEnabled(), "Fail: eventSource should be enabled.");
                Assert.IsTrue(CoreEventSource.Log.IsEnabled(EventLevel.Error, AllKeyword), "Fail: Error is expected to be enabled by default");


                listener.LogLevel = EventLevel.Informational;
                Assert.IsTrue(CoreEventSource.Log.IsEnabled(EventLevel.Informational, AllKeyword), "Fail: Informational is expected to be enabled");

                CoreEventSource.Log.LogVerbose("Some verbose tracing");
                Assert.AreEqual(0, senderMock.Messages.Count);

                CoreEventSource.Log.DiagnosticsEventThrottlingHasBeenResetForTheEvent(10, 1);
                Assert.AreEqual(1, senderMock.Messages.Count);

                senderMock.Messages.Clear();

                listener.LogLevel = EventLevel.Verbose;
                Assert.IsTrue(CoreEventSource.Log.IsEnabled(EventLevel.Verbose, AllKeyword), "Fail: Verbose is expected to be enabled");

                CoreEventSource.Log.LogVerbose("Some verbose tracing");
                Assert.AreEqual(1, senderMock.Messages.Count);

                senderMock.Messages.Clear();

                CoreEventSource.Log.DiagnosticsEventThrottlingHasBeenResetForTheEvent(10, 1);
                Assert.AreEqual(1, senderMock.Messages.Count);

                senderMock.Messages.Clear();

                listener.LogLevel = EventLevel.Error;
                Assert.IsTrue(CoreEventSource.Log.IsEnabled(EventLevel.Error, AllKeyword), "Fail: Error is expected to be enabled");

                CoreEventSource.Log.LogError("Logging an error");

                // If you see the following assert fail, it's because another test has
                // leaked a DiagnosticsTelemetryModule (via TelemetryConfiguration.Active
                // for example). There will be a DiagnosticEventListener which forwards
                // error messages to a PortalDiagnosticsSender. That listener is still
                // getting events from EventSources. We send an Error event, which goes
                // not only to our listener here, but also to the leaked one. The event
                // gets forwarded to the PortalDiagnosticsSender. That turns the event
                // into TraceTelemetry and tries to transmit it. Since we set the event
                // level to verbose earlier, RichPayloadEventSource is is enabled and
                // it writes the TraceTelemetry to all listeners (including us).
                // Unfortuantely, the TraceTelemetry contains a nullable field which
                // triggers a known .NET Core 2.0 bug inside WriteEvent. The internal
                // exception gets reported as an error from WriteEvent resulting in another
                // event with EventLevel.Error being reported here.
                Assert.AreEqual(1, senderMock.Messages.Count);
            }
        }
    }
}

#pragma warning restore 612, 618  // obsolete TelemetryConfigration.Active