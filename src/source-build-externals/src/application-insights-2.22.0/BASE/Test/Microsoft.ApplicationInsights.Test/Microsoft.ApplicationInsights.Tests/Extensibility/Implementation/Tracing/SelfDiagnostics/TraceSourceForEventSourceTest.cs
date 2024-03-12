namespace Microsoft.ApplicationInsights.TestFramework.Extensibility.Implementation.Tracing.SelfDiagnostics
{
    using System.Diagnostics;
    using System.Diagnostics.Tracing;

    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.FileDiagnosticsModule;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TraceSourceForEventSourceTest
    {
        [TestMethod]
        public void TestErrorMessageWillBeTraced()
        {
            InMemoryTraceListener listener = new InMemoryTraceListener();
            using (TraceSourceForEventSource source = new TraceSourceForEventSource(SourceLevels.Error))
            {
                source.Listeners.Add(listener);

                using (TestEventSource eventSource = new TestEventSource())
                {
                    eventSource.TraceError("MyError");

                    Assert.IsTrue(listener.Trace.Contains("MyError"), "Actual: " + listener.Trace);
                }
            }
        }

        [TestMethod]
        public void TestVerboseMessageWillBeTraced()
        {
            InMemoryTraceListener listener = new InMemoryTraceListener();
            using (TraceSourceForEventSource source = new TraceSourceForEventSource(SourceLevels.Error))
            {
                source.LogLevel = EventLevel.Verbose;
                source.Listeners.Add(listener);

                using (TestEventSource eventSource = new TestEventSource())
                {
                    eventSource.TraceVerbose("MyVerbose");

                    Assert.IsTrue(listener.Trace.Contains("MyVerbose"), "Actual: " + listener.Trace);
                }
            }
        }

        [TestMethod]
        public void TestMessageWithKeywordsWillBeTraced()
        {
            InMemoryTraceListener listener = new InMemoryTraceListener();
            using (TraceSourceForEventSource source = new TraceSourceForEventSource(SourceLevels.Error))
            {
                source.LogLevel = EventLevel.Verbose;
                source.Listeners.Add(listener);

                using (TestEventSource eventSource = new TestEventSource())
                {
                    eventSource.TraceKeywords("MyVerbose");

                    Assert.IsTrue(listener.Trace.Contains("MyVerbose"), "Actual: " + listener.Trace);
                }
            }
        }

        [TestMethod]
        public void TestCanChangeLogLevelAfterEventSourceCreated()
        {
            InMemoryTraceListener listener = new InMemoryTraceListener();
            using (TraceSourceForEventSource source = new TraceSourceForEventSource(SourceLevels.Error))
            {
                source.Listeners.Add(listener);

                using (TestEventSource eventSource = new TestEventSource())
                {
                    eventSource.TraceVerbose("MyVerbose");

                    source.LogLevel = EventLevel.Verbose;

                    eventSource.TraceVerbose("MyVerbose");

                    Assert.IsTrue(listener.Trace.Contains("MyVerbose"));
                    Assert.AreEqual(listener.Trace.IndexOf("MyVerbose"), listener.Trace.LastIndexOf("MyVerbose"));
                }
            }
        }
    }
}
