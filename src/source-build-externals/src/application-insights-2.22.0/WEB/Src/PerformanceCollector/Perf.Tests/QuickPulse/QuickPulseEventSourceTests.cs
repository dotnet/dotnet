namespace Microsoft.ApplicationInsights.Tests
{
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// QuickPulseEventSource tests.
    /// </summary>
    [TestClass]
    public class QuickPulseEventSourceTests
    {
        [TestMethod]
        public void QuickPulseEventSourceSanityTest()
        {
            // check for FormatExceptions and ETW exceptions
            QuickPulseEventSource.Log.ModuleIsBeingInitializedEvent("Endpoint", false, false, "authApiKey");
            QuickPulseEventSource.Log.CounterRegisteredEvent("counter");
            QuickPulseEventSource.Log.CounterRegistrationFailedEvent("Test exception", "counter");
            QuickPulseEventSource.Log.CounterParsingFailedEvent("Test exception", "counter");
            QuickPulseEventSource.Log.ProcessorRegistered("count");
            QuickPulseEventSource.Log.CounterReadingFailedEvent("Test exception", "counter");
            QuickPulseEventSource.Log.ProcessesReadingFailedEvent("Test exception");
            QuickPulseEventSource.Log.PingSentEvent("outgoingEtag", "incomingEtag", "true");
            QuickPulseEventSource.Log.SampleSubmittedEvent("outgoingEtag", "incomingEtag", "true");
            QuickPulseEventSource.Log.CollectionConfigurationUpdating("oldEtag", "newEtag", "configuration");
            QuickPulseEventSource.Log.CollectionConfigurationUpdateFailed("oldEtag", "newEtag", "configuration", "exception");
            QuickPulseEventSource.Log.ServiceCommunicationFailedEvent("Test exception");
            QuickPulseEventSource.Log.UnknownErrorEvent("Test exception");
            QuickPulseEventSource.Log.CollectionConfigurationSampleCooldownEvent(true);
        }
    }
}
