namespace Microsoft.ApplicationInsights.Tests
{
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// PerformanceCollectorEventSource tests.
    /// </summary>
    [TestClass]
    public class PerformanceCollectorEventSourceTests
    {
        [TestMethod]
        public void PerformanceCollectorEventSourceSanityTest()
        {
            // check for FormatExceptions and ETW exceptions
            PerformanceCollectorEventSource.Log.ModuleIsBeingInitializedEvent("Test message");
            PerformanceCollectorEventSource.Log.CounterRegisteredEvent("counter");
            PerformanceCollectorEventSource.Log.CountersRefreshedEvent("10", "values");
            PerformanceCollectorEventSource.Log.CounterRegistrationFailedEvent("Test exception", "counter");
            PerformanceCollectorEventSource.Log.CounterParsingFailedEvent("Test exception", "counter");
            PerformanceCollectorEventSource.Log.CounterCheckConfigurationEvent("1", "Test message");
            PerformanceCollectorEventSource.Log.RunningUnderIisExpress();
            PerformanceCollectorEventSource.Log.CounterCollectionAttemptEvent();
            PerformanceCollectorEventSource.Log.CounterCollectionSuccessEvent(0, 0);
            PerformanceCollectorEventSource.Log.CounterReadingFailedEvent("Test exception", "counter");
            PerformanceCollectorEventSource.Log.TelemetrySendFailedEvent("Test exception");
            PerformanceCollectorEventSource.Log.UnknownErrorEvent("Test exception");
        }
    }
}
